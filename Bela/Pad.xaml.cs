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
    public partial class Pad : ContentPage
    {
        public Pad(InputResultPage page)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            Show(page);
        }

        async void Show(InputResultPage page)
        {
            MyLabel.Opacity = 0;
            await MyLabel.FadeTo(1, 1000, Easing.Linear);
            page.Zavrsi();
            await Application.Current.MainPage.Navigation.PopAsync();
        }

    }
}