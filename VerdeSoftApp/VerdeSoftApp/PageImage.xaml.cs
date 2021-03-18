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

            //if (cSQLite.getAllImagesArtikels().Count > 0)
            //{
            //    foreach (eImages imgArtikel in cSQLite.getAllImagesArtikels())
            //    {
            //        if (imgArtikel.ArtId != 0)
            //        {
            //            Console.WriteLine(imgArtikel.ID + ", " + imgArtikel.ArtId + ", " + imgArtikel.URL);
            //        }
            //    }
            //}

            //if (cSQLite.getAllArtikels().Count > 0)
            //{
            //    foreach (eArticle eArt in cSQLite.getAllArtikels())
            //    {
            //        if (eArt.ImageFound == true)
            //        {
            //            Console.WriteLine(eArt.ID + ", " + eArt.ArtId + ", " + eArt.ImageFound.ToString());
            //        }

            //    }
            //}

        }

        private void initEntries()
        {
            LabelTitle.Text = cTranslate.getValue("Images");
            BtnTakePhoto.Text = cTranslate.getValue("NeemFoto");
            BtnKoppelArtikel.Text = cTranslate.getValue("KoppelArtikelMetFoto");
            BtnVerstuurWCF.Text = cTranslate.getValue("VerstuurPhotosWCF");
            BtnReturn.Text = cTranslate.getValue("Terug");

            if (cData.eImage != null)
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
        }

        private void BtnReturn_Clicked(object sender, EventArgs a)
        {
            cNavigate.openPage(new MainPage());
        }

        private async void BtnTakePhoto_Clicked(object sender, EventArgs e)
        {

            //if (cData.takedPhotoByteArray != null)
            //{

            //    Stream stream = new MemoryStream(cData.takedPhotoByteArray);
            //    var imageSource = ImageSource.FromStream(() => stream);
            //    TestTakedPhoto.Source = imageSource;
            //}

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
                if (LabelImageURL.Text != "")
                {
                    BtnKoppelArtikel.IsEnabled = true;
                }
                else
                {
                    BtnKoppelArtikel.IsEnabled = false;
                }

                cData.eImage = new eImages(); //nieuw object van eImages aanmaken 
                cData.eImage.URL = LabelImageURL.Text;
                cData.eImage.WCF = false;
                cData.eImage.B1 = false;
                cData.eImage.C1 = "";
            }

            MyImage.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                //cData.takedPhotoByteArray = ConvertStreamToByteArray(stream);
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

        }



        //private byte[] ConvertStreamToByteArray(System.IO.Stream stream)
        //{
        //    long originalPosition = 0;

        //    if (stream.CanSeek)
        //    {
        //        originalPosition = stream.Position;
        //        stream.Position = 0;
        //    }

        //    try
        //    {
        //        byte[] readBuffer = new byte[stream.Length];

        //        int totalBytesRead = 0;
        //        int bytesRead;

        //        while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
        //        {
        //            totalBytesRead += bytesRead;

        //            if (totalBytesRead == readBuffer.Length)
        //            {
        //                int nextByte = stream.ReadByte();
        //                if (nextByte != -1)
        //                {
        //                    byte[] temp = new byte[readBuffer.Length * 2];
        //                    Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
        //                    Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
        //                    readBuffer = temp;
        //                    totalBytesRead++;
        //                }
        //            }
        //        }

        //        byte[] buffer = readBuffer;
        //        if (readBuffer.Length != totalBytesRead)
        //        {
        //            buffer = new byte[totalBytesRead];
        //            Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
        //        }
        //        return buffer;
        //    }
        //    finally
        //    {
        //        if (stream.CanSeek)
        //        {
        //            stream.Position = originalPosition;
        //        }
        //    }
        //}
    }
}