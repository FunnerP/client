using client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using client.Windows;
using System.Net.Http.Json;

namespace client
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        private HttpClient httpClient;
        private MainWindow mainWindow;
        private string? token;
        public Main(Response response, MainWindow window)
        {
            InitializeComponent();
            this.mainWindow = window;
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + response.access_token);
            token = response.access_token;
            Task.Run(() => Load());
        }
        private async Task Load()
        {
            List<Passanger>? list = await httpClient.GetFromJsonAsync<List<Passanger>>("http://localhost:5079/api/passangers");
            foreach (Passanger i in list!)
            {
                i.Registration = await httpClient.GetFromJsonAsync<Models.Registration>("http://localhost:5079/api/registration/" + i.RegId);
            }
            Dispatcher.Invoke(() =>
            {
                ListPassangers.ItemsSource = null;
                ListPassangers.Items.Clear();
                ListPassangers.ItemsSource = list;
            });
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.mainWindow.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow(token!);
            registrationWindow.ShowDialog();
        }
        //добавление
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            PassangerWindow passangerWindow = new PassangerWindow(token!);
            if (passangerWindow.ShowDialog() == true)
            {
                Passanger passanger = new Passanger
                {
                    Name = passangerWindow.NameProperty,
                    FirstName = passangerWindow.FirstNameProperty,
                    LastName = passangerWindow.LastNameProperty,
                    RegID = await passangerWindow.getIdReg()
                };
                JsonContent content = JsonContent.Create(passanger);
                using var response = await httpClient.PostAsync("http://localhost:5079/api/passanger", content);
                string responseText = await response.Content.ReadAsStringAsync();
                await Load();
            }
        }
        //изменение
        private async void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Passanger? st = ListPassangers.SelectedItem as Passanger;
            PassangerWindow passangerWindow = new PassangerWindow(token!, st!);
            if (passangerWindow.ShowDialog() == true)
            {
                st!.Name = passangerWindow.NameProperty;
                st!.FirstName = passangerWindow.FirstNameProperty;
                st!.LastName = passangerWindow.LastNameProperty;
                st!.RegID = await passangerWindow.getIdReg();
                JsonContent content = JsonContent.Create(st);
                using var response = await httpClient.PutAsync("http://localhost:5079/api/passanger", content);
                string responseText = await response.Content.ReadAsStringAsync();
                await Load();
            }
        }
        //удаление
        private async void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Passanger? st = ListPassangers.SelectedItem as Passanger;
            JsonContent content = JsonContent.Create(st);
            using var response = await httpClient.DeleteAsync("http://localhost:5079/api/student/" + st!.Id);
            string responseText = await response.Content.ReadAsStringAsync();
            await Load();
        }
    }
}
