using Plugin.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VerdeSoftApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageImage : ContentPage
    {

        eImagesArticle eImage = null;

        public PageImage()
        {
            InitializeComponent();

            this.Title = "PageImage";

            initEntries();

            cData.lstAllArticles = cSQLite.getAllArtikels(); //haal alle artikels op

            cSQLite.getAllNotSendedImagesArticlesToWCF(); //haal alle niet verzonden afbeeldingen op
            enableDisableVerstuurWCF();
            showAssignedArticle(); //toon met welk artikel er gekoppeld is.
        }

        public PageImage(string sReturnPage)
        {
            InitializeComponent();

            this.Title = "PageImage";

            initEntries();

            cData.lstAllArticles = cSQLite.getAllArtikels(); //haal alle artikels op

            cSQLite.getAllNotSendedImagesArticlesToWCF(); //haal alle niet verzonden afbeeldingen op
            enableDisableVerstuurWCF();
            showAssignedArticle(); //toon met welk artikel er gekoppeld is.
        }

        private void showAssignedArticle() 
        {
            if (cData.isClickedKoppelArtikel == true)
            {
                LabelImageArtikel.Text = String.Format(cTranslate.getValue("MessageKoppelArtikelFoto"), cData.eImage.URL, cData.selectedArticle.Name);
            }
            else 
            {
                LabelImageArtikel.Text = "";
            }
            
        }



        private void initEntries()
        {
            LabelTitle.Text = cTranslate.getValue("Images");
            BtnTakePhoto.Text = cTranslate.getValue("NeemFoto");
            BtnKoppelArtikel.Text = cTranslate.getValue("KoppelArtikelMetFoto");
            BtnVerstuurWCF.Text = cTranslate.getValue("VerstuurPhotosWCF");
            BtnReturn.Text = cTranslate.getValue("Terug");
            MyImage.Source = null;

            if (cData.eImage != null && cData.lstAllArticles.Count > 0)
            {
                LabelImageURL.Text = cData.eImage.URL;
                //MyImage.Source = ImageSource.FromUri(new Uri(cData.eImage.URL));
                string sBase64ImageString = cTools.ConvertImageToBase64(cData.eImage.URL);
                byte[] imageByte = Convert.FromBase64String(sBase64ImageString);
                MyImage.Source = ImageSource.FromStream(() =>
                {
                    var stream = new MemoryStream(imageByte);
                    return stream;
                });

                BtnKoppelArtikel.IsEnabled = true;
            }
            else
            {
                LabelImageURL.Text = "";
                MyImage.Source = null;
                BtnKoppelArtikel.IsEnabled = false;
            }
            LabelImageArtikel.Text = "";

            enableDisableVerstuurWCF();
        }

        private void enableDisableVerstuurWCF() //105 nieuwe module Foto Artikels
        {
            cSQLite.getAllNotSendedImagesArticlesToWCF(); //haal alle niet verzonden afbeeldingen op

            if (cData.lstImagesArticlesNotSended.Count > 0)
            {
                BtnVerstuurWCF.IsEnabled = true;
            }
            else
            {
                BtnVerstuurWCF.IsEnabled = false;
            }
        }

        private void BtnReturn_Clicked(object sender, EventArgs a)
        {
            cNavigate.openPage(new MainPage());
        }

        private async void BtnTakePhoto_Clicked(object sender, EventArgs e)
        {

            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                //Directory = "Sample",
                //Name = "test.jpg"
                Directory = "VerdeSoft",
                SaveToAlbum = true
            });

            //Get the public album path:  var aPpath = file.AlbumPath;
            //Get private path:           var path = file.Path;

            if (file == null)
                return;

            //await DisplayAlert("File Location", file.AlbumPath, "OK");
            if (file.AlbumPath != null)
            {
               
                LabelImageURL.Text = file.AlbumPath;

                if (LabelImageURL.Text != "" && cData.lstAllArticles.Count > 0)
                {
                    BtnKoppelArtikel.IsEnabled = true;
                }
                else
                {
                    await DisplayAlert(cTranslate.getValue("KoppelArtikelMetFoto"), cTranslate.getValue("MessageNoArticles"), cTranslate.getValue("OK"));
                    BtnKoppelArtikel.IsEnabled = false;
                }
            }

            MyImage.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });
        }


        private void BtnKoppelArtikel_Clicked(Object sender, EventArgs a)
        {
            //aanmaak van de nieuwe image
            eImage = new eImagesArticle();
            eImage.URL = LabelImageURL.Text;
            eImage.WCF = false;
            eImage.B1 = false;
            eImage.C1 = "";
            cData.eImage = eImage;

            cData.isClickedKoppelArtikel = true;

            cNavigate.openPage(new PageSearchArt("PageImage")); //Ga naar pagina Search Artikelen en return page PageImage
        }

        private async void BtnVerstuurWCF_Clicked(Object sender, EventArgs a)
        {
            //if (cData.eImage != null) 
            //{
            //    string sBase64ImageString = cTools.ConvertImageToBase64(cData.eImage.URL);
            //    string sErr = cWCF.SaveImageArticle_WCF(sBase64ImageString, cData.eImage.ArtId);

            //    if (sErr == "")
            //    {
            //        await DisplayAlert(cTranslate.getValue("Bericht"), cData.eImage.URL + " is succesvol saved in C:\\ImagesVerdeSoft !!", cTranslate.getValue("OK"));
            //    }
            //    else
            //    {
            //        await DisplayAlert(cTranslate.getValue("ErrorWCF"), sErr, cTranslate.getValue("OK"));
            //    }
            //}
            cSQLite.getAllNotSendedImagesArticlesToWCF();

            if (cData.lstImagesArticlesNotSended.Count > 0) 
            {
                foreach (eImagesArticle selImage in cData.lstImagesArticlesNotSended) 
                {
                    string sBase64ImageString = cTools.ConvertImageToBase64(selImage.URL);
                    string sErr = cWCF.SaveImageArticle_WCF(sBase64ImageString, selImage.ArtId);
                    if (sErr == "")
                    {
                        cSQLite.updateImageWCF(selImage); //105 set WCF to true - Nieuwe module Foto Artikels
                    }
                    else
                    {
                        await DisplayAlert(cTranslate.getValue("ErrorWCF"), sErr, cTranslate.getValue("OK"));
                    }
                }
            }
            enableDisableVerstuurWCF();

        }
    }
}