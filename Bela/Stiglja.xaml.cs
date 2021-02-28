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
    public partial class Stiglja : ContentPage
    {
        public Stiglja(InputResultPage page)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            Show(page);
        }

        async void Show(InputResultPage page)
        {
            MyLabel.FontSize *= 0.93;
            MyLayout.Opacity = 0;
            await MyLayout.FadeTo(1, 1000, Easing.Linear);
            page.Zavrsi();
            await Application.Current.MainPage.Navigation.PopAsync();
        }

    }
}