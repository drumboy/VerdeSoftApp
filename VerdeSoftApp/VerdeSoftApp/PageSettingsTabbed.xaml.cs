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

            item.PaymentScreen = CheckBoxPaymentScreen.IsChecked;

            cSQLite.saveSettings(item);
            await DisplayAlert(cTranslate.getValue("Bericht"), cTranslate.getValue("Opslaan gelukt"), "OK");
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
            CheckBoxKeybAlpha.IsChecked = item.KeybAlpha;

            //CheckedInSearchFieldRadio(item.SearchField); //werkt niet met Android v 4.4 (KitKat) //105
            DropDownSearchField.SelectedItem = item.SearchField;

            EntryCustomerCard.Text = item.CustomerCard;

            CheckBoxPaymentScreen.IsChecked = item.PaymentScreen;
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