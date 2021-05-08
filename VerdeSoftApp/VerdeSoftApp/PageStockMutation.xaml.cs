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
    public partial class PageStockMutation : ContentPage
    {
        private eStockMutation selectedItem = null;
        private eStockMutation tappedItem = null;

        public List<eStockMutation> stockMutationList { get; private set; }

        public PageStockMutation()
        {
            InitializeComponent();
            this.Title = "PageStockMutation";
            cData.hMutationChanged += CData_hMutationChanged;
            refreshData();
            translatePage();
        }

        private void translatePage()
        {
            LabelTitle.Text = cTranslate.getValue("Stock");
            LabelMut.Text = cTranslate.getValue("Mutaties");
            BtnDelete.Text = cTranslate.getValue("Verwijderen");
            BtnSend.Text = cTranslate.getValue("Versturen");
            BtnReturn.Text = cTranslate.getValue("Terug");
            LabelQuant.Text = cTranslate.getValue("Aant");
        }

        private void CData_hMutationChanged()
        {
            refreshData();
        }

        protected override bool OnBackButtonPressed()
        {
            //return base.OnBackButtonPressed();
            cData.hMutationChanged -= CData_hMutationChanged;
            cNavigate.openPreviousPage();
            return true;
        }

        async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            selectedItem = e.SelectedItem as eStockMutation;
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
            tappedItem = e.Item as eStockMutation;

        }

        private void refreshData()
        {
            stockMutationList = cSQLite.selectStockMutations();
            ListViewStockMutations.ItemsSource = null;
            ListViewStockMutations.ItemsSource = stockMutationList;
        }

        private async void BtnDelete_Clicked(object sender, EventArgs e)
        {
            if (selectedItem != null)
            {
                bool answer = await DisplayAlert(cTranslate.getValue("Vraag") + "?", cTranslate.getValue("Verwijderen") + " " + selectedItem.Place, cTranslate.getValue("Ja"), cTranslate.getValue("Nee"));
                if (answer == true)
                {
                    cData.Delete_StockMutation(selectedItem);
                    refreshData();
                }
            }
            else
            {
                bool answer = await DisplayAlert(cTranslate.getValue("Vraag") + "?", cTranslate.getValue("Alles verwijderen"), cTranslate.getValue("Ja"), cTranslate.getValue("Nee"));
                if (answer == true)
                {
                    cData.Delete_StockMutations();
                    refreshData();
                }
            }
        }

        //private async void BtnModify_Clicked(object sender, EventArgs e)
        //{
        //    if (selectedItem != null)
        //    {
        //        cNavigate.openPage(new PageStockMutationModify(selectedItem));
                
        //    }
        //}

        private async void BtnSend_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert(cTranslate.getValue("Vraag") + "?", cTranslate.getValue("Versturen"), cTranslate.getValue("Ja"), cTranslate.getValue("Nee"));
            if (answer == true)
            {
                string err = cWCF.StockMutations_WCF(cData.settings.Identification);
                if (err == "")
                {
                    await DisplayAlert("WCF: " + cTranslate.getValue("Verwerkt"), err, "OK");

                    bool answer2 = await DisplayAlert(cTranslate.getValue("Vraag") + "?", cTranslate.getValue("Alles verwijderen"), cTranslate.getValue("Ja"), cTranslate.getValue("Nee"));
                    if (answer2 == true)
                    {
                        cData.Delete_StockMutations();
                        refreshData();
                    }
                }
                else
                {
                    await DisplayAlert("Error WCF", err, "OK");
                }
            }
        }

        private void BtnReturn_Clicked(object sender, EventArgs e)
        {
            //cData.hTotalChanged -= CData_hTotalChanged;
            //if (refreshTotal == true)
            //{
            //    cData.bTotalChanged = true;
            //}
            cData.hMutationChanged -= CData_hMutationChanged;
            cNavigate.openPreviousPage();
        }
    }
}