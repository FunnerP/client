using client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using client.Models;

namespace client.Windows
{
    /// <summary>
    /// Логика взаимодействия для Autorize.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HttpClient client;
        public MainWindow()
        {
            InitializeComponent();
            client = new HttpClient();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Baggage baggage = new Baggage { Weight = Login.Text, Password = Password.Text };
            JsonContent content = JsonContent.Create(baggage);
            using var response = await client.PostAsync("http://localhost:5079/login", content);
            string responseText = await response.Content.ReadAsStringAsync();
            Response? resp = JsonSerializer.Deserialize<Response>(responseText);
            if (resp != null)
            {
                Main main = new Main(resp, this);
                main.Show();
                this.Hide();
            }
        }
    }
}
