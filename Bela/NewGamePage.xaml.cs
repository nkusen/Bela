using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace bela
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewGamePage : ContentPage
    {
        public NewGamePage()
        {
            InitializeComponent();
        }

        private List<Par> parovi(List<Result> rawResults)
        {
            List<Par> ret = new List<Par>();
            foreach (var res in rawResults)
            {
                if (res.Zvali == 0 && res.ViResult > 0 && (res.MiResult + res.MiZvanje <= res.ViResult + res.ViZvanje || res.MiResult == 0)) ret.Add(new Par { first = 0, second = res.MiZvanje + res.ViZvanje + 162 });
                else if (res.Zvali == 1 && res.MiResult > 0 && (res.MiResult + res.MiZvanje >= res.ViResult + res.ViZvanje || res.ViResult == 0)) ret.Add(new Par { first = res.MiZvanje + res.ViZvanje + 162, second = 0 });
                else ret.Add(new Par { first = res.MiResult + res.MiZvanje, second = res.ViResult + res.ViZvanje });
            }
            return ret;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            using (SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                connection.CreateTable<Result>();
                var results = connection.Table<Result>().ToList();

                App.MI = results[0].MiResult;
                App.VI = results[0].ViResult;
                results.RemoveAt(0);

                List<Par> pairs = parovi(results);

                int mi = 0, vi = 0;
                foreach (var pair in pairs)
                {
                    mi += pair.first;
                    vi += pair.second;
                }

                if (mi > vi)
                {
                    if (mi >= App.LIMIT[App.LIMIT_ID])
                    {
                        App.MI++;
                        connection.CreateTable<Result>();
                        connection.DeleteAll<Result>();
                        Result temp = new Result
                        {
                            MiResult = App.MI,
                            ViResult = App.VI
                        };
                        connection.Insert(temp);

                    }
                }
                else if (vi > mi)
                {
                    if (vi >= App.LIMIT[App.LIMIT_ID])
                    {
                        App.VI++;
                        connection.CreateTable<Result>();
                        connection.DeleteAll<Result>();
                        Result temp = new Result
                        {
                            MiResult = App.MI,
                            ViResult = App.VI
                        };
                        connection.Insert(temp);
                    }
                }


                results = connection.Table<Result>().ToList();

                App.MI = results[0].MiResult;
                App.VI = results[0].ViResult;
                results.RemoveAt(0);

                pairs = parovi(results);

                rezultati.ItemsSource = pairs;

                mi = 0; vi = 0;
                foreach (var pair in pairs)
                {
                    mi += pair.first;
                    vi += pair.second;
                }
                miUkupno.Text = mi.ToString();
                viUkupno.Text = vi.ToString();

                miPartije.Text = "MI " + App.MI.ToString();
                viPartije.Text = "VI " + App.VI.ToString();
            }
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            Navigation.PushAsync(new InputResultPage(-1));
        }

        private void rezultati_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var i = (rezultati.ItemsSource as List<Par>).IndexOf(e.SelectedItem as Par);
            Navigation.PushAsync(new InputResultPage(i));
        }

    }
}