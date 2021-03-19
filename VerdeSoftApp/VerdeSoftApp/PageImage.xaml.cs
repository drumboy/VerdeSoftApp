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

        public PageImage()
        {
            InitializeComponent();

            this.Title = "PageImage";

            initEntries();

            cData.lstAllArticles = cSQLite.getAllArtikels(); //haal alle artikels op

            cSQLite.getAllNotSendedImagesArticlesToWCF(); //haal alle niet verzonden afbeeldingen op
            enableDisableVerstuurWCF();

        }

        private void initEntries()
        {
            LabelTitle.Text = cTranslate.getValue("Images");
            BtnTakePhoto.Text = cTranslate.getValue("NeemFoto");
            BtnKoppelArtikel.Text = cTranslate.getValue("KoppelArtikelMetFoto");
            BtnVerstuurWCF.Text = cTranslate.getValue("VerstuurPhotosWCF");
            BtnReturn.Text = cTranslate.getValue("Terug");

            if (cData.eImage != null && cData.lstAllArticles.Count > 0)
            {
                LabelImageURL.Text = cData.eImage.URL;
                MyImage.Source = ImageSource.FromUri(new Uri(cData.eImage.URL)); 
                BtnKoppelArtikel.IsEnabled = true;
            }
            else
            {
                LabelImageURL.Text = "";
                MyImage.Source = null;
                BtnKoppelArtikel.IsEnabled = false;
            }

            enableDisableVerstuurWCF();
        }

        private void enableDisableVerstuurWCF() //105 nieuwe module Foto Artikels
        {
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

            await DisplayAlert("File Location", file.AlbumPath, "OK");
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

                if (BtnKoppelArtikel.IsEnabled == true) 
                {
                    cData.eImage = new eImages(); //nieuw object van eImages aanmaken 
                    cData.eImage.URL = LabelImageURL.Text;
                    cData.eImage.WCF = false;
                    cData.eImage.B1 = false;
                    cData.eImage.C1 = "";
                }

                enableDisableVerstuurWCF();
            }

            MyImage.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });



            //var webImage = new Image { Aspect = Aspect.AspectFit };
            //webImage.Source = ImageSource.FromUri(new Uri("https://xamarin.com/content/images/pages/forms/example-app.png"));
        }


        private void BtnKoppelArtikel_Clicked(Object sender, EventArgs a)
        {
            cData.isClickedKoppelArtikel = true;

            cNavigate.openPage(new PageSearchArt("PageImage")); //Ga naar pagina Search Artikelen en return page PageImage
        }

        private void BtnVerstuurWCF_Clicked(Object sender, EventArgs a)
        {
            //if (sTestBase64 != "") 
            //{
            //    byte[] imageByte = Convert.FromBase64String(sTestBase64);
            //    imgTest.Source = ImageSource.FromStream(() =>
            //    {
            //        var stream = new MemoryStream(imageByte);
            //        return stream;
            //    });
            //}
        }
    }
}