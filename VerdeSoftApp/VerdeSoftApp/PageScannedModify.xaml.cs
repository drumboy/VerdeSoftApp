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
    public partial class PageScannedModify : ContentPage
    {
        private eScanned SelectedItem = null;

        public PageScannedModify()
        {
            InitializeComponent();
        }

        public PageScannedModify(eScanned pSelectedItem)
        {
            InitializeComponent();
            SelectedItem = pSelectedItem;

            EntryQuantity.Text = SelectedItem.Quantity.ToString("");
            EntryPrice.Text = SelectedItem.Price.ToString("0.00");
            EntryDiscount.Text = SelectedItem.Discount.ToString("0.00");
            LabelArtName.Text = SelectedItem.Name + " " + SelectedItem.Size1;

            translatePage();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            EntryQuantity.Focus();
        }

        protected override bool OnBackButtonPressed()
        {
            //return base.OnBackButtonPressed();
            cNavigate.openPreviousPage();
            return true;
        }

        private void translatePage()
        {
            LabelTitle.Text = cTranslate.getValue("Wijzigen");
            BtnReturn.Text = cTranslate.getValue("Terug");
            BtnSave.Text = cTranslate.getValue("Opslaan");
            LabelQuantity.Text = cTranslate.getValue("Aantal");
            LabelPrice.Text = cTranslate.getValue("Prijs");
            LabelDiscount.Text = cTranslate.getValue("Korting");
        }

        private void BtnReturn_Clicked(object sender, EventArgs e)
        {
            cNavigate.openPreviousPage();
        }

        private void saveScannedArticle()
        {
            updateScannedArticle();
            cNavigate.openPreviousPage();
        }

        private void BtnSave_Clicked(object sender, EventArgs e)
        {
            saveScannedArticle();
        }

        private void EntryQuantity_Completed(object sender, EventArgs e)
        {
            EntryPrice.Focus();
        }

        private void EntryDiscount_Completed(object sender, EventArgs e)
        {
            saveScannedArticle();
        }

        private void EntryPrice_Completed(object sender, EventArgs e)
        {
            EntryDiscount.Focus();
        }

        protected void EntryQuantity_Focused(object sender, FocusEventArgs e)
        {
            EntryQuantity.BackgroundColor = Color.Yellow;
            EntryQuantity.CursorPosition = 0;
            EntryQuantity.SelectionLength = EntryQuantity.Text.Length;
        }

        private void EntryQuantity_Unfocused(object sender, FocusEventArgs e)
        {
            EntryQuantity.BackgroundColor = Color.White;
        }

        private void EntryPrice_Focused(object sender, FocusEventArgs e)
        {
            EntryPrice.BackgroundColor = Color.Yellow;
            EntryPrice.CursorPosition = 0;
            EntryPrice.SelectionLength = EntryPrice.Text.Length;
        }

        private void EntryPrice_Unfocused(object sender, FocusEventArgs e)
        {
            EntryPrice.BackgroundColor = Color.White;
        }

        protected void EntryDiscount_Focused(object sender, FocusEventArgs e)
        {
            EntryDiscount.BackgroundColor = Color.Yellow;
            EntryDiscount.CursorPosition = 0;
            EntryDiscount.SelectionLength = EntryDiscount.Text.Length;
        }

        private void EntryDiscount_Unfocused(object sender, FocusEventArgs e)
        {
            EntryDiscount.BackgroundColor = Color.White;
        }

        private void updateScannedArticle()
        {
            cSQLite.selectArticlesArtId(Convert.ToInt32(SelectedItem.ArtId)); //set cData.SelectedArticle
            if (cData.selectedArticle != null)
            {
                eScanned a = cData.Prepare_eScanned(SelectedItem.ID, EntryQuantity.Text, EntryPrice.Text, EntryDiscount.Text, SelectedItem.Kind);
                cData.Update_ScannedArticle(a); //modify database
                cData.selectedArticle = null; //!
                cData.bTotalChanged = true;
            }

            calculateTotal();
            //initEntries();
        }

        private void calculateTotal()
        {
            List<eScanned> scannedArticles = cData.Get_ScannedArticles(SelectedItem.Kind);
            eTotal total = cData.calculateTotal(scannedArticles);
        }

    }
}