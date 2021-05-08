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
            enableDisableControls(); //105
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
            LabelTitle.Text = cTranslate.getValue("Instellingen"); //change title Betalingen to Instellingen - Laurens //105

            BtnReturn.Text = cTranslate.getValue("Terug");
            BtnInit.Text = cTranslate.getValue("Initialiseer");
            BtnSettings.Text = cTranslate.getValue("Instellingen");
        }

        private async void BtnInit_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert(cTranslate.getValue("Vraag"),  cTranslate.getValue("AllesInitialiseren") + Environment.NewLine + cTranslate.getValue("BentUZeker"), cTranslate.getValue("Ja"), cTranslate.getValue("Nee"));
            if (answer == true)
            {
                cSQLite.initDatabase(true);
                await DisplayAlert(cTranslate.getValue("Vraag"), cTranslate.getValue("InitialisatieGeslaagd"), cTranslate.getValue("OK"));
                enableDisableControls();
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

        private void enableDisableControls() //105
        {
            if (cData.settings == null) cSQLite.getSettings();
            bool bEnable = cData.settings != null;
            BtnSettings.IsEnabled = bEnable;
        }

        
    }
}