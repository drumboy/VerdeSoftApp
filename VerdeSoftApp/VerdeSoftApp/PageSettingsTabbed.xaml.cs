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
    public partial class PageSettingsTabbed : TabbedPage
    {

        private eSettings item = null;

        public PageSettingsTabbed()
        {
            InitializeComponent();

            item = cSQLite.getSettings();
            initEntries();

            translatePage();
        }

        private void translatePage()
        {
            LabelTitleVervolg.Text = cTranslate.getValue("Vervolg");
            contentTabVervolg.Title = cTranslate.getValue("Vervolg");
            contentTabInstellingen.Title = cTranslate.getValue("Instellingen");
            LabelTitleInstellingen.Text = cTranslate.getValue("Instellingen");
            LabelCNSTR.Text = cTranslate.getValue("Connection string");
            LabelWCF.Text = cTranslate.getValue("WCF Service");
            LabelIdentif.Text = cTranslate.getValue("Identificatie");
            LabelTicket.Text = cTranslate.getValue("Ticket");
            LabelLanguage.Text = cTranslate.getValue("Taal");
            LabelFocusPrice.Text = cTranslate.getValue("Lengte barcode"); //Stoppen prijs
            LabelPayments.Text = cTranslate.getValue("Herhaal aantal"); //nieuw
            LabelCustomerCode.Text = cTranslate.getValue("Klantcode");
            BtnSave.Text = cTranslate.getValue("Opslaan");
            BtnReturn.Text = cTranslate.getValue("Terug");
            BtnSaveVervolg.Text = cTranslate.getValue("Opslaan");
            BtnReturnVervolg.Text = cTranslate.getValue("Terug");
        }

        private async void BtnSaveInstellingen_Clicked(object sender, EventArgs e)
        {
            item.MobileService = EntryMobileService.Text;
            item.Identification = EntryIdentification.Text;
            item.CustomerCode = EntryCustomerCode.Text;
            item.FocusPrice = CheckBoxFocusPrice.IsChecked;
            item.Payments = CheckBoxPayments.IsChecked;
            item.Ticket = Convert.ToInt64(EntryTicket.Text);
            item.Language = EntryLanguage.Text;

            cSQLite.saveSettings(item);
            await DisplayAlert(cTranslate.getValue("Bericht"), cTranslate.getValue("Opslaan gelukt"), "OK");
            cNavigate.openSpecificPage("MainPage");
        }

        private async void BtnSaveVervolg_Clicked(object sender, EventArgs e)
        {
            item.ConnString = EntryConnectionString.Text;

            cSQLite.saveSettings(item);
            await DisplayAlert(cTranslate.getValue("Bericht"), cTranslate.getValue("Opslaan gelukt"), "OK");
            cNavigate.openSpecificPage("MainPage");
        }

        private void initEntries()
        {
            EntryConnectionString.Text = item.ConnString;
            EntryMobileService.Text = item.MobileService;
            EntryIdentification.Text = item.Identification;
            EntryCustomerCode.Text = item.CustomerCode;
            CheckBoxFocusPrice.IsChecked = item.FocusPrice;
            CheckBoxPayments.IsChecked = item.Payments;
            EntryLanguage.Text = item.Language;
            EntryTicket.Text = Convert.ToString(item.Ticket);
        }

        private void BtnReturn_Clicked(object sender, EventArgs e)
        {
            //cNavigate.openSpecificPage("MainPage");
            cNavigate.openPreviousPage();
        }
    }
}