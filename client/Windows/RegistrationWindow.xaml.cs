using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using client.Models;

namespace client.Windows
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        private HttpClient client;
        private Registration? registration;
        public RegistrationWindow(string token)
        {
            InitializeComponent();
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            Task.Run(() => Load());
        }
        private async Task Load()
        {
            List<Registration>? list = await client.GetFromJsonAsync<List<Registration>>("http://localhost:5079/api/registrations");
            Dispatcher.Invoke(() =>
            {
                ListRegistrations.ItemsSource = null;
                ListRegistrations.Items.Clear();
                ListRegistrations.ItemsSource = list;
            });
        }
        private async Task Save()
        {
            Registration registration = new Registration
            {
                Name = NameRegistration.Text,
                Weight = Weight.Text
            };
            JsonContent content = JsonContent.Create(registration);
            using var response = await client.PostAsync("http://localhost:5079/api/registration", content);
            string responseText = await response.Content.ReadAsStringAsync();
            await Load();
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await Save();
        }

        private void ListRegistrations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            registration = ListRegistrations.SelectedItem as Group;
            NameRegistration.Text = registration?.Name;
            Weight.Text = weight?.Speciality;
        }

        private async Task Edit()
        {
            registration!.Name = NameRegistration.Text;
            registration!.Weight = Weight.Text;
            JsonContent content = JsonContent.Create(registration);
            using var response = await client.PutAsync("http://localhost:5079/api/registration", content);
            string responseText = await response.Content.ReadAsStringAsync();
            await Load();
        }
        private async Task Delete()
        {
            using var response = await client.DeleteAsync("http://localhost:5079/api/registration/" + registration?.Id);
            string responseText = await response.Content.ReadAsStringAsync();
            await Load();
        }
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            await Edit();
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            await Delete();
        }
    }
}
