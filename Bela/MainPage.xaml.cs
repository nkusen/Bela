using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace bela
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private void IgraDo()
        {
            if (rezLimit.SelectedIndex == -1)
            {
                rezLimit.SelectedIndex = 2;
            }
            App.LIMIT_ID = rezLimit.SelectedIndex;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            using (SQLite.SQLiteConnection connection = new SQLite.SQLiteConnection(App.DB_PATH))
            {
                connection.CreateTable<Result>();
                connection.DeleteAll<Result>();
                connection.Insert(new Result());
            }

            IgraDo();

            Navigation.PushAsync(new NewGamePage());
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            IgraDo();

            Navigation.PushAsync(new NewGamePage());
        }

    }
}
