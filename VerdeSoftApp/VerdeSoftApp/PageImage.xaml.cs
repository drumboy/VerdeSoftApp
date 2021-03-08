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

            MyImage.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                //cData.takedPhotoByteArray = ConvertStreamToByteArray(stream);
                return stream;
            });

            

            //var webImage = new Image { Aspect = Aspect.AspectFit };
            //webImage.Source = ImageSource.FromUri(new Uri("https://xamarin.com/content/images/pages/forms/example-app.png"));
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