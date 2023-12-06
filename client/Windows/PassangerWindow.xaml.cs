using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
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
    /// Логика взаимодействия для PassangerWindow.xaml
    /// </summary>
    public partial class PassangerWindow : Window
    {
        private HttpClient client;
        private Registration? registration;
        public PassangerWindow(String token)
        {
            InitializeComponent();
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            Task.Run(() => LoadRegistration());
        }
        public PassangerWindow(String token, Passanger passanger)
        {
            InitializeComponent();
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            Task.Run(() => LoadRegistration());
            Name.Text = passanger.Name;
            FirstName.Text = passanger.FirstName;
            Lastname.Text = passanger.LastName;
            cbRegistration.SelectedItem = passanger.Registration!.Name;
        }
        private async void LoadRegistration()
        {
            List<Registration>? list = await client.GetFromJsonAsync<List<Registration>>("http://localhost:5079/api/groups");
            Dispatcher.Invoke(() =>
            {
                cbRegistration.ItemsSource = list?.Select(p => p.Name);
            });
        }
        public string? NameProperty
        {
            get { return Name.Text; }
        }
        public string? FirstNameProperty
        {
            get { return FirstName.Text; }
        }
        public string? LastNameProperty
        {
            get { return Lastname.Text; }
        }
        public async Task<int> getIdRegistration()
        {
            Registration? group = await client.GetFromJsonAsync<Registration>("http://localhost:5079/api/registration/" + cbRegistration.Text);
            return registration!.Id;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
