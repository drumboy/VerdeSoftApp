using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VerdeSoftApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageOptions : ContentPage
    {
        public PageOptions()
        {
            InitializeComponent();
            translatePage();
        }

        protected override bool OnBackButtonPressed()
        {
            //return base.OnBackButtonPressed();
            cNavigate.openPreviousPage();
            return true;
        }

        private void translatePage()
        {
            LabelTitle.Text = cTranslate.getValue("Instellingen"); //change title Betalingen to Instellingen - Laurens

            BtnReturn.Text = cTranslate.getValue("Terug");
            BtnInit.Text = cTranslate.getValue("Initialiseer");
            BtnSettings.Text = cTranslate.getValue("Instellingen");
        }

        private async void BtnInit_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Vraag?", "Alles initialiseren" + Environment.NewLine + "Bent u zeker?", "Ja", "Nee");
            if (answer == true)
            {
                cSQLite.initDatabase(true);
                await DisplayAlert("Bericht", "Initialisatie geslaagd", "OK");
            }
        }


        private void BtnSettings_Clicked(object sender, EventArgs e)
        {
            //cNavigate.openPage(new PageSettings());
            cNavigate.openPage(new PageSettingsTabbed());
        }

        private void BtnReturn_Clicked(object sender, EventArgs e)
        {
            cNavigate.openPreviousPage();
        }

        
    }
}