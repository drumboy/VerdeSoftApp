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
    public partial class PageArticles : ContentPage
    {
        public IList<eArticle> Articles { get; private set; }
        private eArticle selectedItem = null;
        private eArticle tappedItem = null;
        //private bool fromSearchForm = false;
        private string returnPageName = "";

        public PageArticles()
        {
            InitializeComponent();


            Articles = cData.articleList;

            BindingContext = this;

            this.Title = "PageArticles";
        }

        public PageArticles(string pReturnPageName)
        {
            InitializeComponent();

            Articles = cData.articleList;

            BindingContext = this;

            returnPageName = pReturnPageName;

            this.Title = "PageArticles";

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

        private void BtnReturn_Clicked(object sender, EventArgs e)
        {
            cNavigate.openPreviousPage();
        }

        private async void BtnChoose_Clicked(object sender, EventArgs e)
        {
            cData.selectedArticle = selectedItem; //koppel het geselecteerde artikel aan selectedArticle in cData

            if (returnPageName == "")
            {
                cNavigate.openPreviousPage();
            }
            else if (returnPageName == "PageCashReg")
            {
                cNavigate.openSpecificPage("PageCashReg");
            }
            else if (returnPageName == "PageStock")
            {
                cNavigate.openSpecificPage("PageStock");
            }
            else if (returnPageName == "PageImage") //105 nieuwe module Foto Artikel
            {
                //check if knop Koppel Article is clicked, then insert image
                if (cData.isClickedKoppelArtikel) 
                {
                    cSQLite.insertImage(cData.eImage);

                    //await DisplayAlert(cTranslate.getValue("KoppelArtikelMetFoto"), String.Format(cTranslate.getValue("MessageKoppelArtikelFoto"), cData.eImage.URL, cData.selectedArticle.Name), cTranslate.getValue("OK"));

                    //cNavigate.openSpecificPage("PageImage");
                    cNavigate.openPage(new PageImage("PageImage"));
                }
            }
        }

        void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            selectedItem = e.SelectedItem as eArticle;
            cData.selectedArticle = selectedItem; //koppel het geselecteerde artikel aan selectedArticle in cData
        }

        void OnListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            tappedItem = e.Item as eArticle;
            cData.selectedArticle = tappedItem; //koppel het geselecteerde artikel aan selectedArticle in cData
        }

        private async void btnPhotoArticle_Clicked(object sender, EventArgs e)
        {
            //if (cData.selectedArticle == null)
            //{
            //    await DisplayAlert("Foto Artikel", "Gelieve eerst het artikel te selecteren", "OK");
            //}
            //else 
            //{
            //    await DisplayAlert("Foto Artikel", cData.selectedArticle.Name + ", ID: " + cData.selectedArticle.ID, "OK");
            //}
        }
    }
}