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
    public partial class PageSettings : ContentPage
    {
        private eSettings item = null;
        public PageSettings()
        {
            InitializeComponent();

            item = cSQLite.getSettings();
            initEntries();

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
            LabelTitle.Text = cTranslate.getValue("Instellingen");
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
        }

        private void initEntries()
        {
            EntryConnectionString.Text = item.ConnString;
            EntryMobileService.Text = item.MobileService;
            EntryIdentification.Text = item.Identification;
            EntryCustomerCode.Text = item.CustomerCode;
            CheckBoxFocusPrice.IsChecked = item.VerifyBarcode;
            CheckBoxRepeatQuantity.IsChecked = item.RepeatQuantity;
            EntryLanguage.Text = item.Language;
            EntryTicket.Text = Convert.ToString(item.Ticket);
        }

        private void BtnSave_Clicked(object sender, EventArgs e)
        {
            item.MobileService = EntryMobileService.Text;
            item.ConnString = EntryConnectionString.Text;
            item.Identification = EntryIdentification.Text;
            item.CustomerCode = EntryCustomerCode.Text;
            item.VerifyBarcode = CheckBoxFocusPrice.IsChecked;
            item.RepeatQuantity = CheckBoxRepeatQuantity.IsChecked;
            item.Ticket = Convert.ToInt64(EntryTicket.Text);
            item.Language = EntryLanguage.Text;

            cSQLite.saveSettings(item);
            cNavigate.openPreviousPage();
        }

        private void BtnReturn_Clicked(object sender, EventArgs e)
        {
            cNavigate.openSpecificPage("MainPage");
        }

        private void BtnInit_Clicked(object sender, EventArgs e)
        {
            
        }


        private void init()
        {
            //cSQLite.initDatabase(false);
            //item = cSQLite.getSettings();
            //initEntries();
        }
    }
}