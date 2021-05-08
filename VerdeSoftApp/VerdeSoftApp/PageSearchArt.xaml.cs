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
    public partial class PageSearchArt : ContentPage
    {
        private string returnPageName = "";

        public PageSearchArt()
        {
            InitializeComponent();
        }
        public PageSearchArt(string pReturnPageName)
        {
            InitializeComponent();
            returnPageName = pReturnPageName;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            translatePage();
            EntryName.Focus();
        }

        protected override bool OnBackButtonPressed()
        {
            //return base.OnBackButtonPressed();
            cNavigate.openPreviousPage();
            return true;
        }

        private void translatePage()
        {
            LabelTitle.Text = cTranslate.getValue("Zoeken");
            BtnSearch.Text = cTranslate.getValue("Zoeken");
            BtnReturn.Text = cTranslate.getValue("Terug");
            LabelDescription.Text = cTranslate.getValue("Naam");
            LabelCode.Text = cTranslate.getValue("Code");
        }

        private void BtnReturn_Clicked(object sender, EventArgs e)
        {
            cNavigate.openPreviousPage();
        }

        public void EntryName_Completed(object sender, EventArgs e)
        {
            if (cTools.IsEmpty(EntryName.Text) == false)
            {
                searchField("Name", EntryName.Text);
            }
        }

        public void EntryCode_Completed(object sender, EventArgs e)
        {
            if (cTools.IsEmpty(EntryCode.Text) == false)
            {
                searchField("Code", EntryCode.Text);
            }
        }

        private async void searchField(string pFieldName, string pFieldValue)
        {   
            cSQLite.selectArticlesField(pFieldName, pFieldValue, enumOperator.Contains);
            if (cData.articleList.Count > 1)
            {
                cNavigate.openPage(new PageArticles(returnPageName));
            }
            else if (cData.articleList.Count == 1)
            {
                cData.selectedArticle = cData.articleList[0];
                cNavigate.openPreviousPage();
            }
            else
            {
                await DisplayAlert("Alert", cTranslate.getValue("Geen artikels gevonden"), "OK");
            }
        }

        private async void BtnSearch_Clicked(object sender, EventArgs e)
        {
            if (cTools.IsEmpty(EntryCode.Text) == false || cTools.IsEmpty(EntryName.Text) == false)
            {
                if (cTools.IsEmpty(EntryCode.Text) == false)
                {
                    cSQLite.selectArticlesField("Code", EntryCode.Text, enumOperator.Contains);
                    //cWCF.SearchArticles_WCF(cData.settings.Identification, "Code", EntryCode.Text, true);
                }
                else if (cTools.IsEmpty(EntryName.Text) == false)
                {
                    //cWCF.SearchArticles_WCF(cData.settings.Identification, "Name", EntryName.Text, true);
                    cSQLite.selectArticlesField("Name", EntryName.Text, enumOperator.Contains);
                }
                if (cData.articleList.Count > 1)
                {
                    cNavigate.openPage(new PageArticles(returnPageName));
                }
                else if (cData.articleList.Count == 1)
                {
                    cData.selectedArticle = cData.articleList[0];
                    //ArticleSelected_Handler();
                    cNavigate.openPreviousPage();
                }
                else
                {
                    await DisplayAlert("Alert", cTranslate.getValue("Geen artikels gevonden"), "OK");
                }
            }
            
        }
    }
}