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
    public partial class PageStock : ContentPage
    {
        public IList<eStock> StockItems { get; private set; }
        private eStock selectedItem = null;
        private eStock tappedItem = null;

        public PageStock()
        {
            InitializeComponent();

            EntryScan.Keyboard = cData.kBoard;

            EntryScan.Text = "";
            cData.hArticleSelected += ArticleSelected_Handler;

            cData.stockList = new List<eStock>();

            //cSQLite.selectStockArtId((long)5367);
            StockItems = cData.stockList;
            //BindingContext = this;

            ImgBtnSearch.Source = ImageSource.FromResource("VerdeSoftApp.search.png");
            ImgBtnAdd.Source = ImageSource.FromResource("VerdeSoftApp.add146.png");
            ImgBtnDel.Source = ImageSource.FromResource("VerdeSoftApp.delete146.png");
            ImgBtnMod.Source = ImageSource.FromResource("VerdeSoftApp.change146.png");
            ImgBtnMut.Source = ImageSource.FromResource("VerdeSoftApp.copy146.png");
            ImgBtnLst.Source = ImageSource.FromResource("VerdeSoftApp.catalog146.png");

            cData.hMutationChanged += CData_hMutationChanged;

            this.Title = "PageStock";

            translatePage();
        }

        private void CData_hMutationChanged()
        {
            //
        }

        private void translatePage()
        {
            LabelTitle.Text = cTranslate.getValue("Stock");
            BtnList.Text = cTranslate.getValue("Lijst");
            BtnReturn.Text = cTranslate.getValue("Terug");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            EntryScan.Focus();
            //BindingContext = this;
        }

        public async void EntryScan_Completed(object sender, EventArgs e)
        {
            SearchValue();
        }

        protected void EntryScan_Focussed(object sender, FocusEventArgs e)
        {
            EntryScan.BackgroundColor = Color.Yellow;
            EntryScan.CursorPosition = 0;
            EntryScan.SelectionLength = EntryScan.Text.Length;
            //lastFocused = "EntryScan";
        }

        private void EntryScan_Unfocused(object sender, FocusEventArgs e)
        {
            EntryScan.BackgroundColor = Color.White;
            //checkLostFocus();
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            cNavigate.openPage(new PageSearchArt("PageStock"));
        }

        private void ImgBtnAdd_Clicked(object sender, EventArgs e)
        {
            if (cData.selectedArticle != null) //selectedArticle != null
            {
                eStockMutation m = new eStockMutation();
                m = cData.Prepare_eStockMutation(selectedItem, "ADD", cData.selectedArticle); //nieuwe regel
                cNavigate.openPage(new PageStockMutationModify(m));
            }
        }

        private async void ImgBtnDel_Clicked(object sender, EventArgs e)
        {
            if (cData.selectedArticle != null && selectedItem != null) //eStock != null
            {
                eStockMutation m = cData.Prepare_eStockMutation(selectedItem, "DEL", cData.selectedArticle);
                //cData.Insert_StockMutation_Delete(selectedItem, cData.selectedArticle);
                cData.Insert_StockMutation(m);
                await DisplayAlert("Alert", "Wissen" + " " + selectedItem.Place, "OK");
            }
        }

        private void ImgBtnMod_Clicked(object sender, EventArgs e)
        {
            if (cData.selectedArticle != null && selectedItem != null) //eStock != null
            {
                eStockMutation m = new eStockMutation();
                m = cData.Prepare_eStockMutation(selectedItem, "MOD", cData.selectedArticle); //wijzigen bestaande regel
                cNavigate.openPage(new PageStockMutationModify(m));
            }
        }

        private void ImgBtnMut_Clicked(object sender, EventArgs e)
        {
            if (cData.selectedArticle != null && selectedItem != null) //eStock != null
            {
                eStockMutation m = new eStockMutation();
                m = cData.Prepare_eStockMutation(selectedItem, "MUT", cData.selectedArticle); //splitsen bestaande regel
                cNavigate.openPage(new PageStockMutationModify(m));
            }
        }

        private void BtnMinus_Clicked(object sender, EventArgs e)
        {
            if (cData.selectedArticle != null && selectedItem != null) //eStock != null
            {
                eStockMutation m = new eStockMutation();
                m = cData.Prepare_eStockMutation(selectedItem, "MUT2", cData.selectedArticle); // toevoegen of schrappen bestaande regel
                cNavigate.openPage(new PageStockMutationModify(m));
            }
        }

        private void ImgBtnLst_Clicked(object sender, EventArgs e)
        {
            cNavigate.openPage(new PageStockMutation());
        }

        private async void SearchValue()
        {
            cData.name = EntryScan.Text;
            if (cData.settings.FocusPrice == true) //Lengte barcode
            {
                if (cData.name.Length == 7) cData.name = "0" + cData.name;
                if (cData.name.Length == 12) cData.name = "0" + cData.name;
            }
            cSQLite.selectArticlesBarcode(cData.name);
            if (cData.articleList.Count > 1)
            {
                cNavigate.openPage(new PageArticles());
            }
            else if (cData.articleList.Count == 1)
            {
                cData.selectedArticle = cData.articleList[0];
                ArticleSelected_Handler();
            }
            else
            {
                EntryScan.Text = "";
                EntryScan.Focus();
                cSound.beepSound();
                await DisplayAlert("Alert", cData.name + " " + "niet gevonden ", "OK");
            }
        }

        private void ArticleSelected_Handler()
        {
            if (cData.selectedArticle != null)
            {
                Device.StartTimer(TimeSpan.FromMilliseconds(30), () =>
                {
                    if (cData.selectedArticle != null) //
                    {
                        LabelArtName.Text = cData.selectedArticle.Name;
                        //
                        //cData.stockList = new List<eStock>();
                        //StockItems = cData.stockList;
                        //BindingContext = this;

                        cSQLite.selectStockArtId(cData.selectedArticle.ArtId);
                        StockItems = cData.stockList;
                        StockItemList.ItemsSource = StockItems;

                        EntryScan.Focus();
                    }

                    return false; // return true to repeat counting, false to stop timer
                });
            }
        }

        private void BtnAdd_Clicked(object sender, EventArgs e)
        {
        }

        private async void BtnDel_Clicked(object sender, EventArgs e)
        {
            //if (selectedItem != null)
            //{
            //    cData.Insert_StockMutation_Delete(selectedItem, cData.selectedArticle);
            //    await DisplayAlert("Alert", "Wissen"+" "+ selectedItem.Place, "OK");
            //}
        }

        private void BtnMod_Clicked(object sender, EventArgs e)
        {
        }

        private void BtnList_Clicked(object sender, EventArgs e)
        {
            cNavigate.openPage(new PageStockMutation());
        }

        

        //private async void BtnSend_Clicked(object sender, EventArgs e)
        //{
        //    string err = cWCF.StockMutations_WCF(cData.settings.Identification);
        //    if (err == "")
        //    {
        //        await DisplayAlert("WCF: Stock is verwerkt", err, "OK");
        //    }
        //    else
        //    {
        //        await DisplayAlert("Error WCF", err, "OK");
        //    }
        //}

        private void BtnReturn_Clicked(object sender, EventArgs e)
        {
            cData.hMutationChanged -= CData_hMutationChanged;
            cNavigate.openPreviousPage();
        }
        void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            selectedItem = e.SelectedItem as eStock;
        }

        void OnListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            tappedItem = e.Item as eStock;
        }

        private void OnStackLayout_Tapped(object sender, EventArgs e)
        {
            Console.WriteLine("OnStackLayout_Tapped");
            Device.StartTimer(TimeSpan.FromMilliseconds(50), () =>
            {
                EntryScan.Focus();
                return false;
            });

        }

    }
}