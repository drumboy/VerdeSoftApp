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
    public partial class PageScanned : ContentPage
    {
        public List<eScanned> scannedArticles { get; private set; }
        private eScanned selectedItem = null;
        private eScanned tappedItem = null;
        private bool refreshTotal = false;
        private string kind = "";

        public PageScanned()
        {
            InitializeComponent();
        }

        public PageScanned(string pKind)
        {
            InitializeComponent();

            kind = pKind;

            cData.hTotalChanged += CData_hTotalChanged;

            refreshData();

            this.Title = "PageScanned";

            translatePage();
        }

        protected override bool OnBackButtonPressed()
        {
            //return base.OnBackButtonPressed();
            cData.hTotalChanged -= CData_hTotalChanged;
            if (refreshTotal == true)
            {
                cData.bTotalChanged = true;
            }
            cNavigate.openPreviousPage();
            return true;
        }

        private void CData_hTotalChanged()
        {
            refreshData();
        }

        private void translatePage()
        {
            LabelTitle.Text = cTranslate.getValue("Lijst");

            BtnDelete.Text = cTranslate.getValue("Verwijderen");
            BtnReturn.Text = cTranslate.getValue("Terug");
            BtnModify.Text = cTranslate.getValue("Wijzigen");
        }

        private void refreshData()
        {
            scannedArticles = cData.Get_ScannedArticles(kind);
            ListViewArticles.ItemsSource = null;
            ListViewArticles.ItemsSource = scannedArticles;
            calculateTotal();
        }

        private void calculateTotal()
        {
            eTotal total = cData.calculateTotal(scannedArticles);
            LabelTotal.Text = "T " + cData.settings.Ticket.ToString() + "  -  # " + total.TotalQuantity.ToString() + "  -  € " + total.TotalAmount.ToString("0.00");
        }

        async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            selectedItem = e.SelectedItem as eScanned;
            if (selectedItem != null)
            {
                //string action = await DisplayActionSheet("Delete" + Environment.NewLine + selectedItem.Name, "Cancel", "", "Delete", "Modify");
                //if (action == "Delete")
                //{
                //    cData.Delete_ScannedArticle(selectedItem);
                //    refreshData();
                //    refreshTotal = true;
                //}
                //else if (action == "Modify")
                //{
                //    await DisplayAlert("Alert", "Modifiy", "OK");
                //}
            }
        }

        void OnListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            tappedItem = e.Item as eScanned;

        }

        private async void BtnDelete_Clicked(object sender, EventArgs e)
        {
            if (selectedItem != null)
            {
                bool answer = await DisplayAlert(cTranslate.getValue("Vraag")+"?", cTranslate.getValue("Verwijderen") +" "+ selectedItem.Name, cTranslate.getValue("Ja"), cTranslate.getValue("Nee"));
                if (answer == true)
                {
                    cData.Delete_ScannedArticle(selectedItem);
                    
                    if (cData.settings.Staffels == true) //Laurens //106 //if staffels than update also the list
                    {
                        double dTotalScannedSameArticleInKassa = cData.getSumQuantitySelectedArtikelById(Convert.ToInt32(selectedItem.ArtId));
                        cSQLite.selectArticlesArtId(Convert.ToInt32(selectedItem.ArtId)); //need to define the selected Price
                        string sStaffelPrice = selectedItem.Price.ToString();
                        bool staffel = cData.switch_Staffel(dTotalScannedSameArticleInKassa, ref sStaffelPrice,cData.selectedArticle); //calculate the Staffel price
                        if (staffel == true) 
                        {
                            cData.updatePriceScannedArticlesInKassa(Convert.ToDouble(sStaffelPrice)); //update all scanned articles in Kassa from selected article!
                        }
                    }
                    cData.selectedArticle = null; //set selected article on null
                    
                    refreshData();
                    refreshTotal = true;
                }
            }
            else
            {
                bool answer = await DisplayAlert(cTranslate.getValue("Vraag") + "?", cTranslate.getValue("Alles verwijderen") , cTranslate.getValue("Ja"), cTranslate.getValue("Nee"));
                if (answer == true)
                {
                    cData.Delete_ScannedArticles(kind);
                    refreshData();
                    refreshTotal = true;
                }
            }
        }

        private async void BtnModify_Clicked(object sender, EventArgs e)
        {
            if (selectedItem != null)
            {
                //bool answer = await DisplayAlert("Vraag?", cTranslate.getValue("Wijzigen") + selectedItem.Name, "Ja", "Nee");
                //if (answer == true)
                //{
                    cNavigate.openPage(new PageScannedModify(selectedItem));
                    //cData.Delete_ScannedArticle(selectedItem);
                    //refreshData();
                    refreshTotal = true;
                //}
            }

            
        }

        private void BtnReturn_Clicked(object sender, EventArgs e)
        {
            cData.hTotalChanged -= CData_hTotalChanged;
            if (refreshTotal == true)
            {
                cData.bTotalChanged = true;
            }
            cNavigate.openPreviousPage();
        }
    }
}