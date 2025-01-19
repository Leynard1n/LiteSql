using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using LiteSql;
using Microsoft.Data.Sqlite;
using Microsoft.Maui.Controls;

namespace LiteSql
{
    public partial class MainPage : ContentPage
    {
        private Database _database;
        private List<Person> _persons;

        public MainPage()
        {
            InitializeComponent();
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "people.db");
            _database = new Database(dbPath);
            _persons = new List<Person>();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadPersonsAsync();
        }

        private async Task LoadPersonsAsync()
        {
            _persons = await _database.GetPersonsAsync();
            PersonsListView.ItemsSource = _persons;
        }

        private async void OnAddPersonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameEntry.Text) || string.IsNullOrWhiteSpace(AgeEntry.Text))
            {
                await DisplayAlert("Ошибка", "Пожалуйста, введите имя и возраст.", "OK");
                return;
            }

            var newPerson = new Person
            {
                Name = NameEntry.Text,
                Age = int.Parse(AgeEntry.Text)
            };

            await _database.AddPersonAsync(newPerson);
            NameEntry.Text = string.Empty;
            AgeEntry.Text = string.Empty;

            await LoadPersonsAsync(); // Обновляем список
        }
    }
}
