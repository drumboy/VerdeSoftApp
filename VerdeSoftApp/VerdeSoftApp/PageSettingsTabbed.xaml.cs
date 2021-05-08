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

        private List<string> SearchFieldList = null;

        public PageSettingsTabbed()
        {
            InitializeComponent();

            SearchFieldList = new List<String>();
            SearchFieldList.Add("Code");
            SearchFieldList.Add("Name");

            DropDownSearchField.ItemsSource = SearchFieldList;
            DropDownSearchField.SelectedIndex = 0; //Code
            DropDownSearchField.SelectedIndexChanged += DropDownSearchField_SelectedIndexChanged;

            item = cSQLite.getSettings();
            initEntries();

            translatePage();
        }

        private void DropDownSearchField_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Xamarin.Forms.Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                item.SearchField = (string)picker.ItemsSource[selectedIndex]; //0: Code, //1: Name
            }
        }

        private void translatePage()
        {
            LabelTitleVervolg.Text = cTranslate.getValue("Vervolg");
            contentTabVervolg.Title = cTranslate.getValue("Vervolg");
            contentTabInstellingen.Title = cTranslate.getValue("Instellingen");
            LabelTitleInstellingen.Text = cTranslate.getValue("Instellingen");
            //LabelCNSTR.Text = cTranslate.getValue("Connection string");
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
            LabelKeybAlpha.Text = cTranslate.getValue("Keyboard Alpha"); //105
            LabelSearchField.Text = cTranslate.getValue("Zoekveld");
            LabelCustomerCard.Text = cTranslate.getValue("CustomerCard");
            LabelPaymentScreen.Text = cTranslate.getValue("PaymentScreen"); //105
            LabelQuestionNotFound.Text = cTranslate.getValue("QuestionNotFound"); //105
            LabelIsCataloogArticle.Text = cTranslate.getValue("CataloogArtikel"); //105
            
            EntryIdentification.Placeholder = cTranslate.getValue("Identificatie");
            EntryTicket.Placeholder = cTranslate.getValue("Ticket");
            EntryCustomerCode.Placeholder = cTranslate.getValue("Klantcode");
            EntryLanguage.Placeholder = cTranslate.getValue("Taal");
            EntryCustomerCard.Placeholder = cTranslate.getValue("CustomerCard");

            LabelStaffels.Text = cTranslate.getValue("GebruikStaffels"); //105
            LabelNoConfirmationSended.Text = cTranslate.getValue("GeenBevestigingVerstuurd"); //105 (geen lijst verstuurd, voor Abeels)
        }

        private async void BtnSaveInstellingen_Clicked(object sender, EventArgs e)
        {
            item.MobileService = EntryMobileService.Text;
            item.Identification = EntryIdentification.Text;
            item.CustomerCode = EntryCustomerCode.Text;
            item.Ticket = Convert.ToInt64(EntryTicket.Text);
            item.Language = EntryLanguage.Text;

            cSQLite.saveSettings(item);
            await DisplayAlert(cTranslate.getValue("Bericht"), cTranslate.getValue("Opslaan gelukt"), cTranslate.getValue("OK"));
            cSQLite.getSettings();
            cNavigate.openSpecificPage("MainPage");
        }

        private async void BtnSaveVervolg_Clicked(object sender, EventArgs e)
        {
            //item.ConnString = EntryConnectionString.Text;
            item.VerifyBarcode = CheckBoxVerifyBarcode.IsChecked;
            item.RepeatQuantity = CheckBoxRepeatQuantity.IsChecked;
            item.KeybAlpha = CheckBoxKeybAlpha.IsChecked;

            //Save SearchField //werkt niet met Android v 4.4 (Kitkat) //105
            //if (RbtSearchFieldName.IsChecked)
            //{
            //    item.SearchField = "Name";
            //}
            //else if (RbtSearchFieldCode.IsChecked)
            //{
            //    item.SearchField = "Code";
            //}
            //else 
            //{
            //    item.SearchField = "Code";
            //}

            item.SearchField = DropDownSearchField.SelectedItem.ToString(); //0: Code, 1: Name

            item.CustomerCard = EntryCustomerCard.Text;

            item.PaymentScreen = CheckBoxPaymentScreen.IsChecked; //105

            item.QuestionNotFound = CheckBoxQuestionNotFound.IsChecked; //105

            item.SyncCatalogue = CheckBoxIsCataloogArticle.IsChecked; //105 //enkel cataloogartikels ophalen

            item.Staffels = CheckBoxStaffels.IsChecked; //105

            item.NoConfirmation = CheckBoxNoConfirmationSended.IsChecked; //105 (geen verzend bevestiging) (voor klant abeels)

            cSQLite.saveSettings(item);
            await DisplayAlert(cTranslate.getValue("Bericht"), cTranslate.getValue("Opslaan gelukt"), cTranslate.getValue("OK"));
            cSQLite.getSettings();
            cNavigate.openSpecificPage("MainPage");
        }

        private void initEntries()
        {
            //EntryConnectionString.Text = item.ConnString;
            EntryMobileService.Text = item.MobileService;
            EntryIdentification.Text = item.Identification;
            EntryCustomerCode.Text = item.CustomerCode;
            CheckBoxVerifyBarcode.IsChecked = item.VerifyBarcode;
            CheckBoxRepeatQuantity.IsChecked = item.RepeatQuantity;
            EntryLanguage.Text = item.Language;
            EntryTicket.Text = Convert.ToString(item.Ticket);
            CheckBoxKeybAlpha.IsChecked = item.KeybAlpha; //105

            //CheckedInSearchFieldRadio(item.SearchField); //werkt niet met Android v 4.4 (KitKat) //105
            DropDownSearchField.SelectedItem = item.SearchField;

            EntryCustomerCard.Text = item.CustomerCard;

            CheckBoxPaymentScreen.IsChecked = item.PaymentScreen; //105
            CheckBoxQuestionNotFound.IsChecked = item.QuestionNotFound; //105
            CheckBoxIsCataloogArticle.IsChecked = item.SyncCatalogue; //105
            CheckBoxStaffels.IsChecked = item.Staffels; //105
            CheckBoxNoConfirmationSended.IsChecked = item.NoConfirmation; //105
        }

        //werkt niet met Android v 4.4 (KitKat) //105
        //private void CheckedInSearchFieldRadio(string searchField)
        //{
        //    switch (searchField)
        //    {
        //        case "Name":
        //            item.SearchField = "Name";
        //            RbtSearchFieldName.IsChecked = true;
        //            break;
        //        case "Code":
        //            item.SearchField = "Code";
        //            RbtSearchFieldCode.IsChecked = true;
        //            break;
        //    }
        //}

        private void BtnReturn_Clicked(object sender, EventArgs e)
        {
            //cNavigate.openSpecificPage("MainPage");
            cNavigate.openPreviousPage();
        }

        //werkt niet met Android v 4.4 (KitKat) //105
        //private void RadioButton_CheckedChanged(object sender, EventArgs e)
        //{
        //    RadioButton rb = sender as RadioButton;
        //    if (rb.IsChecked) item.SearchField = rb.Text;
        //}

    }
}