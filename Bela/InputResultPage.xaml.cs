using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace bela
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InputResultPage : ContentPage
    {

        private int index, zvali = -1;
        private Result resultToChange;

        public InputResultPage(int id)
        {
            InitializeComponent();
            index = id;

            if (index != -1)
            {
                using (SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(App.DB_PATH))
                {
                    connection.CreateTable<Result>();
                    resultToChange = connection.Table<Result>().ToList()[1+index];
                }

                miRezEntry.Text = resultToChange.MiResult.ToString();
                viRezEntry.Text = resultToChange.ViResult.ToString();
                miZvanje.Text = resultToChange.MiZvanje.ToString();
                viZvanje.Text = resultToChange.ViZvanje.ToString();

                zvali = resultToChange.Zvali;
                CheckPad();
            }
        }

        private void GetResult(ref int a, ref int b, ref int c, ref int d)
        {
            if (String.IsNullOrWhiteSpace(miZvanje.Text) || !miZvanje.Text.All(char.IsDigit)) a = 0;
            else a = int.Parse(miZvanje.Text);
            b = int.Parse(miRezEntry.Text);
            if (String.IsNullOrWhiteSpace(viZvanje.Text) || !viZvanje.Text.All(char.IsDigit)) c = 0;
            else c = int.Parse(viZvanje.Text);
            d = int.Parse(viRezEntry.Text);
        }

        private bool CheckPad()
        {
            if (zvali == -1) return false;
            if (String.IsNullOrWhiteSpace(miRezEntry.Text) ||
                !miRezEntry.Text.All(char.IsDigit) ||
                String.IsNullOrWhiteSpace(viRezEntry.Text) ||
                !viRezEntry.Text.All(char.IsDigit))
            {
                return false;
            }

            int a = 0, b = 0, c = 0, d = 0;
            GetResult(ref a, ref b, ref c, ref d);
            if (zvali == 0)
            {
                if (d > 0 && (a + b <= c + d  || b == 0))
                {
                    MiLabel.BackgroundColor = Color.DarkRed;
                    return true;
                }
                else MiLabel.BackgroundColor = Color.DeepSkyBlue;
            }
            else
            {
                if (b > 0 && (a + b >= c + d || d == 0))
                {
                    ViLabel.BackgroundColor = Color.DarkRed;
                    return true;
                }
                else ViLabel.BackgroundColor = Color.DeepSkyBlue;
            }
            return false;
        }

        private int a, b, c, d;

        public void Zavrsi()
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (zvali == -1)
            {
                DisplayAlert("Greška", "Stisnite na MI / VI kako biste odabrali ekipu koja je zvala.", "Ok");
                return;
            }
            if (String.IsNullOrWhiteSpace(miRezEntry.Text) ||
                !miRezEntry.Text.All(char.IsDigit) ||
                String.IsNullOrWhiteSpace(viRezEntry.Text) ||
                !viRezEntry.Text.All(char.IsDigit))
            {
                DisplayAlert("Greška", "Unesite pravilne podatke.", "Ok");
                return;
            }

            GetResult(ref a, ref b, ref c, ref d);
            bool isStiglja = false;

            if (b == 0)
            {
                c += a + 90;
                a = 0;
                isStiglja = true;
            }
            else if (d == 0)
            {
                a += c + 90;
                c = 0;
                isStiglja = true;
            }

            using (SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                connection.CreateTable<Result>();
                if (resultToChange == null)
                {
                    connection.CreateTable<Result>();
                    connection.Insert(new Result { MiResult = b, ViResult = d, MiZvanje = a, ViZvanje = c, Zvali = zvali });
                }
                else
                {
                    resultToChange.MiResult = b;
                    resultToChange.ViResult = d;
                    resultToChange.MiZvanje = a;
                    resultToChange.ViZvanje = c;
                    resultToChange.Zvali = zvali;
                    connection.Update(resultToChange);
                }
            }

            if (isStiglja)
            {
                Application.Current.MainPage.Navigation.PushAsync(new Stiglja(this));
            }
            else if (CheckPad())
            {
                Application.Current.MainPage.Navigation.PushAsync(new Pad(this));
            }
            else Zavrsi();
            
        }

        private void miRezEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(miRezEntry.Text))
            {
                miRezEntry.Text = "0";
                return;
            }
            if (!miRezEntry.Text.All(char.IsDigit))
            {
                miRezEntry.Text = "0";
                return;
            }

            int MiResult = int.Parse(miRezEntry.Text);

            if (MiResult < 0)
            {
                MiResult = 0;
            }

            if (MiResult > 162)
            {
                MiResult = 162;
            }

            string str = (162 - MiResult).ToString();
            miRezEntry.Text = MiResult.ToString();
            viRezEntry.Text = str;

            CheckPad();
        }

        private void viRezEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(viRezEntry.Text))
            {
                viRezEntry.Text = "0";
                return;
            }
            if (!viRezEntry.Text.All(char.IsDigit))
            {
                viRezEntry.Text = "0";
                return;
            }

            int ViResult = int.Parse(viRezEntry.Text);

            if (ViResult < 0)
            {
                ViResult = 0;
            }

            if (ViResult > 162)
            {
                ViResult = 162;
            }

            string str = (162 - ViResult).ToString();
            viRezEntry.Text = ViResult.ToString();
            miRezEntry.Text = str;

            CheckPad();
        }

        private void miZvanje_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(miZvanje.Text))
            {
                miZvanje.Text = "0";
                return;
            }
            if (!miZvanje.Text.All(char.IsDigit))
            {
                miZvanje.Text = "0";
                return;
            }

            int temp = int.Parse(miZvanje.Text);
            if (temp > 10000) temp = 10000;
            miZvanje.Text = temp.ToString();

            CheckPad();
        }

        private void viZvanje_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(viZvanje.Text))
            {
                viZvanje.Text = "0";
                return;
            }
            if (!viZvanje.Text.All(char.IsDigit))
            {
                viZvanje.Text = "0";
                return;
            }

            int temp = int.Parse(viZvanje.Text);
            if (temp > 10000) temp = 10000;
            viZvanje.Text = temp.ToString();

            CheckPad();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            MiLabel.BackgroundColor = Color.DeepSkyBlue;
            ViLabel.BackgroundColor = Color.Default;
            zvali = 0;
            CheckPad();
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            ViLabel.BackgroundColor = Color.DeepSkyBlue;
            MiLabel.BackgroundColor = Color.Default;
            zvali = 1;
            CheckPad();
        }


    }

}