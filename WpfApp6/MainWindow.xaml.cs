using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string result;

        public int X { get; set; }
        public string Y { get; set; }
        public string Result { 
            get => result;
            set
            {
                result = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Result"));
            }
        }

        HttpClient httpClient = new HttpClient();

        public MainWindow()
        {
            InitializeComponent();
            httpClient.BaseAddress = new Uri("http://localhost:5094/api/");
            DataContext = this;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private async void Test_Get(object sender, RoutedEventArgs e)
        {
            //http://localhost:5094/api/Math/КрутоеИмя?x=11&y=11
            
            /*var responce = await httpClient.GetAsync
                ($"Math/КрутоеИмя?x={X}&y={Y}");
            Result = await responce.Content.ReadAsStringAsync();*/

            var responce = await httpClient.GetAsync
                ($"Math/НереальноКрутоеИмя?x={X}&y={Y}");
            if (responce.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Result = await responce.Content.ReadAsStringAsync();
                return;
            }
            else
            {
                var answer = await responce.Content.ReadFromJsonAsync<MultiplyResult>();
                Result = answer.ToString();
            }
            /*
            var responce = await httpClient.
                GetFromJsonAsync<MultiplyResult>
                    ($"Math/НереальноКрутоеИмя?x={X}&y={Y}");

            Result = responce.ToString();*/


        }

        private async void Test_Post(object sender, RoutedEventArgs e)
        {
            MultiplyResult multiplyResult = new MultiplyResult
            {
                X = this.X, Y = this.Y
            };
            string arg = JsonSerializer.Serialize(multiplyResult);
            var responce = await httpClient.PostAsync($"Math/НереальноКрутоеИмяСоСложнымАргументом",
                new StringContent(arg, Encoding.UTF8, "application/json"));

            if(responce.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Result = await responce.Content.ReadAsStringAsync();
                return;
            }
            else
            {
                var answer = await responce.Content.ReadFromJsonAsync<MultiplyResult>();
                Result = answer.ToString();
            }

        }
    }

    public class MultiplyResult
    {
        public int X { get; set; }
        public string? Y { get; set; }
        public string? Result { get; set; }

        public override string ToString()
        {
            return $"{X} * {Y} = {Result}";
        }
    }
}
