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
            LabelVerifyBarcode.Text = cTranslate.getValue("VerifyBarcode");
            LabelRepeatQuantity.Text = cTranslate.getValue("Herhaal aantal"); //nieuw
            LabelCustomerCode.Text = cTranslate.getValue("Klantcode");
            BtnSave.Text = cTranslate.getValue("Opslaan");
            BtnReturn.Text = cTranslate.getValue("Terug");
            BtnSaveVervolg.Text = cTranslate.getValue("Opslaan");
            BtnReturnVervolg.Text = cTranslate.getValue("Terug");
            LabelKeybAlpha.Text = cTranslate.getValue("Keyboard Alpha");
            LabelSearchField.Text = cTranslate.getValue("Zoekveld");
            LabelCustomerCard.Text = cTranslate.getValue("CustomerCard");
            LabelPaymentScreen.Text = cTranslate.getValue("PaymentScreen");
        }

        private async void BtnSaveInstellingen_Clicked(object sender, EventArgs e)
        {
            item.MobileService = EntryMobileService.Text;
            item.Identification = EntryIdentification.Text;
            item.CustomerCode = EntryCustomerCode.Text;
            item.Ticket = Convert.ToInt64(EntryTicket.Text);
            item.Language = EntryLanguage.Text;

            cSQLite.saveSettings(item);
            await DisplayAlert(cTranslate.getValue("Bericht"), cTranslate.getValue("Opslaan gelukt"), "OK");
            cNavigate.openSpecificPage("MainPage");
        }

        private async void BtnSaveVervolg_Clicked(object sender, EventArgs e)
        {
            item.ConnString = EntryConnectionString.Text;
            item.VerifyBarcode = CheckBoxVerifyBarcode.IsChecked;
            item.RepeatQuantity = CheckBoxRepeatQuantity.IsChecked;
            item.KeybAlpha = CheckBoxKeybAlpha.IsChecked;

            //Save SearchField
            if (RbtSearchFieldName.IsChecked)
            {
                item.SearchField = "Name";
            }
            else if (RbtSearchFieldCode.IsChecked)
            {
                item.SearchField = "Code";
            }
            else 
            {
                item.SearchField = "Code";
            }

            item.CustomerCard = EntryCustomerCard.Text;

            item.PaymentScreen = CheckBoxPaymentScreen.IsChecked;

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
            CheckBoxVerifyBarcode.IsChecked = item.VerifyBarcode;
            CheckBoxRepeatQuantity.IsChecked = item.RepeatQuantity;
            EntryLanguage.Text = item.Language;
            EntryTicket.Text = Convert.ToString(item.Ticket);
            CheckBoxKeybAlpha.IsChecked = item.KeybAlpha;

            if(String.IsNullOrEmpty(item.SearchField)) //Set RbtSearchField Code op checked en save SearchField 'Code'
            {
                item.SearchField = "Code";
            }

            CheckedInSearchFieldRadio(item.SearchField);

            EntryCustomerCard.Text = item.CustomerCard;

            CheckBoxPaymentScreen.IsChecked = item.PaymentScreen;
        }

        private void CheckedInSearchFieldRadio(string searchField)
        {
            switch (searchField)
            {
                case "Name":
                    item.SearchField = "Name";
                    RbtSearchFieldName.IsChecked = true;
                    break;
                case "Code":
                    item.SearchField = "Code";
                    RbtSearchFieldCode.IsChecked = true;
                    break;
            }
        }

        private void BtnReturn_Clicked(object sender, EventArgs e)
        {
            //cNavigate.openSpecificPage("MainPage");
            cNavigate.openPreviousPage();
        }


        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.IsChecked) item.SearchField = rb.Text;
        }

    }
}