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
    public partial class PageCustomers : ContentPage
    {
        public IList<eCustomer> Customers { get; private set; }
        private eCustomer selectedItem = null;
        private eCustomer tappedItem = null;

        private bool fromSearchForm = false;
        private string kind = "";

        public PageCustomers()
        {
            InitializeComponent();

            Customers = cData.customerList;

            BindingContext = this;

            this.Title = "PageCustomers";
        }

        public PageCustomers(string pKind)
        {
            InitializeComponent();
            kind = pKind;

            Customers = cData.customerList;

            BindingContext = this;

            this.Title = "PageCustomers";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            EntryName.Focus();

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
            LabelTitle.Text = cTranslate.getValue("Resultaat");
            BtnReturn.Text = cTranslate.getValue("Terug");
            BtnChoose.Text = cTranslate.getValue("Kiezen");
        }

        public async void EntryName_Completed(object sender, EventArgs e)
        {
            try
            {
                if (cTools.isNumeric(EntryName.Text, System.Globalization.NumberStyles.Number, false) == true)
                {
                    cSQLite.selectCustomersField("Barcode", EntryName.Text, enumOperator.Equals);
                }
                else
                {
                    cSQLite.selectCustomersField("Name", EntryName.Text, enumOperator.Contains);
                }
                Customers = cData.customerList;
            }
            catch (Exception)
            {
                EntryName.Text = "";
            }
            
            

            lvCustomers.ItemsSource = null;
            lvCustomers.ItemsSource = Customers;
        }

        public async void EntryCode_Completed(object sender, EventArgs e)
        {
            string code = EntryCode.Text;
            cSQLite.selectCustomersField("Code", code, enumOperator.Contains);
            
            Customers = cData.customerList;

            lvCustomers.ItemsSource = null;
            lvCustomers.ItemsSource = Customers;
        }



        void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            selectedItem = e.SelectedItem as eCustomer;
        }

        void OnListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            tappedItem = e.Item as eCustomer;
        }

        private void BtnReturn_Clicked(object sender, EventArgs e)
        {
            cNavigate.openPreviousPage();

        }
        private async void BtnChoose_Clicked(object sender, EventArgs e)
        {
            if (kind == "K") //v1.0.3
            {
                List<eScanned> scannedArticles = cData.Get_ScannedArticles(kind); //v1.0.3
                if (scannedArticles.Count > 0) //v1.0.3
                {
                    cSound.beepSound();
                    bool answer = await DisplayAlert(cTranslate.getValue("Vraag"), cTranslate.getValue("Prijzen aanpassen")+" ?", cTranslate.getValue("Ja"), cTranslate.getValue("Nee")); //v1.3
                    if (answer == true) //v1.0.3
                    {
                        cData.Set_Customer(kind, selectedItem); //v1.0.3
                        cData.bTotalChanged = true; //v1.0.3
                    }
                }
            }

            if (fromSearchForm == false)
            {
                cNavigate.openPreviousPage();
            }
            else
            {
                cNavigate.openSpecificPage("PageCashReg");
            }
            cData.selectedCustomer = selectedItem;
        }
    }
}