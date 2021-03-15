using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VerdeSoftApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageSync : ContentPage
    {
        public PageSync()
        {
            InitializeComponent();
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
            LabelTitle.Text = cTranslate.getValue("Instellingen");
            BtnSyncArt.Text = cTranslate.getValue("Artikels");
            BtnSyncCust.Text = cTranslate.getValue("Klanten");
            BtnSyncUser.Text = cTranslate.getValue("Gebruikers");
            //BtnInit.Text = cTranslate.getValue("Initialiseer");
            BtnClose.Text = cTranslate.getValue("Terug");
            BtnSyncDefPrices.Text = cTranslate.getValue("Prijzentabel");
        }
        private async void BtnSyncArt_Clicked(object sender, EventArgs e)
        {
            string err = "";
            cSQLite.getSettings();
            cSQLite.beforeSyncArticles();
            int totalReceived = 0;
            int per = 1000;

            Device.BeginInvokeOnMainThread(() =>
            {
                
                LabelSync.Text = cTranslate.getValue("ArtikelsOntvangen") + ": " + totalReceived.ToString();
            });

            await Task.Run(() =>
            {
                err = cWCF.GetArticles_WCF(cData.settings.Identification, per, totalReceived);
                if (err == "")
                {

                    totalReceived = totalReceived + cData.articleList.Count;
                    cSQLite.syncArticles();
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        LabelSync.Text = cTranslate.getValue("ArtikelsOntvangen") + ": " + totalReceived.ToString();
                    });


                    while (cData.articleList.Count == per)
                    {
                        cWCF.GetArticles_WCF(cData.settings.Identification, per, totalReceived);
                        totalReceived = totalReceived + cData.articleList.Count;
                        cSQLite.syncArticles();
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            LabelSync.Text = cTranslate.getValue("ArtikelsOntvangen") + ": " + totalReceived.ToString();
                        });
                    }
                }
            });

            LabelSync.Text = cTranslate.getValue("ArtikelsOntvangen") + ": " + totalReceived.ToString();

            if (err != "")
            {
                LabelSync.Text = err;
            }
        }

        private async void BtnSyncCust_Clicked(object sender, EventArgs e)
        {
            cSQLite.getSettings();
            cSQLite.beforeSyncCustomers();
            int totalReceived = 0;
            int per = 1000;

            Device.BeginInvokeOnMainThread(() =>
            {
                LabelSync.Text = cTranslate.getValue("KlantenOntvangen") + ": " + totalReceived.ToString();
            });

            await Task.Run(() =>
            {
                cWCF.GetCustomers_WCF(cData.settings.Identification, per, totalReceived);
                totalReceived = totalReceived + cData.customerList.Count;
                cSQLite.syncCustomers();
                Device.BeginInvokeOnMainThread(() =>
                {
                    LabelSync.Text = cTranslate.getValue("KlantenOntvangen") + ": " + totalReceived.ToString();
                });


                while (cData.customerList.Count == per)
                {
                    cWCF.GetCustomers_WCF(cData.settings.Identification, per, totalReceived);
                    totalReceived = totalReceived + cData.customerList.Count;
                    cSQLite.syncCustomers();
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        LabelSync.Text = cTranslate.getValue("KlantenOntvangen") + ": " + totalReceived.ToString();
                    });
                }
            });

            LabelSync.Text = cTranslate.getValue("KlantenOntvangen") + ": " + totalReceived.ToString();
        }

        private async void BtnSyncUser_Clicked(object sender, EventArgs e)
        {
            cSQLite.getSettings();
            cSQLite.beforeSyncUsers();
            cWCF.GetUsers_WCF(cData.settings.Identification);
            cSQLite.syncUsers();

            LabelSync.Text = cTranslate.getValue("GebruikersOntvangen") + ": " + cData.userList.Count.ToString();
        }

        private async void BtnSyncDefPrices_Clicked(object sender, EventArgs e)
        {
            cSQLite.getSettings();
            cSQLite.beforeSyncDefPrices();
            cWCF.GetDefPrices_WCF(cData.settings.Identification);
            cSQLite.syncDefPrices();

            LabelSync.Text = cTranslate.getValue("PrijzentabelOntvangen") + ": " + cData.defPricesList.Count.ToString();
        }

        private async void BtnSyncStock_Clicked(object sender, EventArgs e)
        {

            Device.BeginInvokeOnMainThread(() =>
            {
                LabelSync.Text = cTranslate.getValue("StockOntvangen") + ": ... ";
            });

            await Task.Run(() =>
            {
                cSQLite.getSettings();
                cSQLite.beforeSyncStock();
                cWCF.GetStock_WCF(cData.settings.Identification);
                cSQLite.syncStock();
            });

            LabelSync.Text = cTranslate.getValue("StockOntvangen") + ": " + cData.stockList.Count.ToString();
        }


        private async void BtnSyncBarcodes_Clicked(object sender, EventArgs e) //1.0.4
        {

            Device.BeginInvokeOnMainThread(() =>
            {
                
                LabelSync.Text = cTranslate.getValue("BarcodesOntvangen") + " : ... ";
            });

            await Task.Run(() =>
            {
                cSQLite.getSettings();
                cSQLite.beforeSyncBarcodes();
                cWCF.GetBarcodes_WCF(cData.settings.Identification);
                cSQLite.syncBarcodes();
            });

            LabelSync.Text = cTranslate.getValue("BarcodesOntvangen") + ": " + cData.barcodesList.Count.ToString();
        }

        //private async void BtnInit_Clicked(object sender, EventArgs e)
        //{
        //    bool answer = await DisplayAlert("Vraag?", "Alles initialiseren"+Environment.NewLine+"Bent u zeker?", "Ja", "Nee");
        //    if (answer == true)
        //    {
        //        cSQLite.initDatabase(true);
        //    }
        //    await DisplayAlert("Bericht", "Initialisatie geslaagd", "OK");
        //}

        private async void BtnPing_Clicked(object sender, EventArgs e)
        {
            string ms = cWCF.MobileServiceStr;
            ms = ms.Replace(@"http://","");
            int idx = ms.IndexOf(":");
            string ip = ms.Substring(0, idx);

            bool ok = cTools.PingHost(ip);
            if (ok == true)
            {
                await DisplayAlert("Ping", ip+" = " + cTranslate.getValue("OK"), cTranslate.getValue("OK"));
            }
            else
            {
                await DisplayAlert("Ping", ip +" = " + cTranslate.getValue("Error"), cTranslate.getValue("Error"));
            }
        }

        private void BtnClose_Clicked(object sender, EventArgs e)
        {
            cNavigate.openPreviousPage();
        }
    }
}