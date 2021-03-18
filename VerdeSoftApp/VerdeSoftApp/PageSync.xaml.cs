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
            LabelTitle.Text = cTranslate.getValue("Synchroniseer");
            BtnSyncArt.Text = cTranslate.getValue("Artikels");
            BtnSyncCust.Text = cTranslate.getValue("Klanten");
            BtnSyncUser.Text = cTranslate.getValue("Gebruikers");
            //BtnInit.Text = cTranslate.getValue("Initialiseer");
            BtnClose.Text = cTranslate.getValue("Terug");
            BtnSyncDefPrices.Text = cTranslate.getValue("Prijzentabel");
        }
        private async void BtnSyncArt_Clicked(object sender, EventArgs e)
        {
            LabelSync.TextColor = Color.Blue;
            BtnSyncArt.BackgroundColor = Color.Red;
            enableButtonsSync(false);
            string err = "";
            cSQLite.getSettings();
            cSQLite.beforeSyncArticles();
            int totalReceived = 0;
            int per = 1000;
            LabelSync.Text = cTranslate.getValue("ArtikelsOntvangen") + " : ... ";

            //LabelSync.Text = cTranslate.getValue("ArtikelsOntvangen") + ": " + totalReceived.ToString();
            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    LabelSync.Text = cTranslate.getValue("ArtikelsOntvangen") + ": " + totalReceived.ToString();
            //});

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

            if (err != "")
            {
                LabelSync.TextColor = Color.Red;
                LabelSync.Text = showError(err);
            }
            
            enableButtonsSync(true);
            BtnSyncArt.BackgroundColor = Color.Green;
        }

        private async void BtnSyncCust_Clicked(object sender, EventArgs e)
        {
            LabelSync.TextColor = Color.Blue;
            string err = "";
            BtnSyncCust.BackgroundColor = Color.Red;
            enableButtonsSync(false);
            cSQLite.getSettings();
            cSQLite.beforeSyncCustomers();
            int totalReceived = 0;
            int per = 1000;
            LabelSync.Text = cTranslate.getValue("KlantenOntvangen") + " : ... ";

            await Task.Run(() =>
            {
                err = cWCF.GetCustomers_WCF(cData.settings.Identification, per, totalReceived);
                if (err == "")
                {
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
                }
            });

            if (err != "")
            {
                LabelSync.TextColor = Color.Red;
                LabelSync.Text = showError(err);
            }

            BtnSyncCust.BackgroundColor = Color.Green;
            enableButtonsSync(true);
        }

        private async void BtnSyncUser_Clicked(object sender, EventArgs e)
        {
            LabelSync.TextColor = Color.Blue;
            string err = "";
            BtnSyncUser.BackgroundColor = Color.Red;
            enableButtonsSync(false);
            cSQLite.getSettings();
            cSQLite.beforeSyncUsers();
            LabelSync.Text = cTranslate.getValue("GebruikersOntvangen") + " : ... ";
            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    LabelSync.Text = cTranslate.getValue("GebruikersOntvangen") + ": " + cData.userList.Count.ToString();
            //});

            await Task.Run(() =>
            {
                err = cWCF.GetUsers_WCF(cData.settings.Identification);
                if (err == "")
                {
                    cSQLite.syncUsers();
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        LabelSync.Text = cTranslate.getValue("GebruikersOntvangen") + ": " + cData.userList.Count.ToString();
                    });
                }
            });

            if (err != "") 
            {
                LabelSync.TextColor = Color.Red;
                LabelSync.Text = showError(err);
            }

            BtnSyncUser.BackgroundColor = Color.Green;
            enableButtonsSync(true);
        }

        private async void BtnSyncDefPrices_Clicked(object sender, EventArgs e)
        {
            LabelSync.TextColor = Color.Blue;
            string err = "";
            BtnSyncDefPrices.BackgroundColor = Color.Red;
            enableButtonsSync(false);
            cSQLite.getSettings();
            cSQLite.beforeSyncDefPrices();
            LabelSync.Text = cTranslate.getValue("PrijzentabelOntvangen") + " : ... ";

            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    LabelSync.Text = cTranslate.getValue("PrijzentabelOntvangen") + ": " + cData.defPricesList.Count.ToString();
            //});

            await Task.Run(() =>
            {
                err = cWCF.GetDefPrices_WCF(cData.settings.Identification);
                if (err == "")
                {
                    cSQLite.syncDefPrices();
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        LabelSync.Text = cTranslate.getValue("PrijzentabelOntvangen") + ": " + cData.defPricesList.Count.ToString();
                    });
                }
            });
            if (err != "") 
            {
                LabelSync.TextColor = Color.Red;
                LabelSync.Text = showError(err);
            }

            BtnSyncDefPrices.BackgroundColor = Color.Green;
            enableButtonsSync(true);
        }

        private async void BtnSyncStock_Clicked(object sender, EventArgs e)
        {
            LabelSync.TextColor = Color.Blue;
            string err = "";
            BtnSyncStock.BackgroundColor = Color.Red;
            enableButtonsSync(false);

            cSQLite.getSettings();
            cSQLite.beforeSyncStock();
            LabelSync.Text = cTranslate.getValue("StockOntvangen") + " : ... ";
            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    LabelSync.Text = cTranslate.getValue("StockOntvangen") + ": ... ";
            //});

            await Task.Run(() =>
            {
                err = cWCF.GetStock_WCF(cData.settings.Identification);
                if (err == "")
                {
                    cSQLite.syncStock();
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        LabelSync.Text = cTranslate.getValue("StockOntvangen") + ": " + cData.stockList.Count.ToString();
                    });
                }
                
            });

            if (err != "") 
            {
                LabelSync.TextColor = Color.Red;
                LabelSync.Text = showError(err);
            }

            BtnSyncStock.BackgroundColor = Color.Green;
            enableButtonsSync(true);
        }


        private async void BtnSyncBarcodes_Clicked(object sender, EventArgs e) //1.0.4
        {
            LabelSync.TextColor = Color.Blue;
            string err = "";
            BtnSyncBarcodes.BackgroundColor = Color.Red;
            enableButtonsSync(false);
            cSQLite.getSettings();
            cSQLite.beforeSyncBarcodes();
            LabelSync.Text = cTranslate.getValue("BarcodesOntvangen") + " : ... ";

            await Task.Run(() =>
            {

                err = cWCF.GetBarcodes_WCF(cData.settings.Identification);
                if (err == "")
                {
                    cSQLite.syncBarcodes();
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        LabelSync.Text = cTranslate.getValue("BarcodesOntvangen") + ": " + cData.barcodesList.Count.ToString();
                    });
                }
                
            });

            if (err != "") 
            {
               LabelSync.TextColor = Color.Red;
               LabelSync.Text = showError(err);
            }

            BtnSyncBarcodes.BackgroundColor = Color.Green;
            enableButtonsSync(true);
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
            LabelSync.Text = "";
            LabelSync.TextColor = Color.Blue;
            string ms = cData.settings.MobileService;
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

        private void enableButtonsSync(bool bEnable) 
        {
            BtnSyncArt.IsEnabled = bEnable;
            BtnSyncBarcodes.IsEnabled = bEnable;
            BtnSyncCust.IsEnabled = bEnable;
            BtnSyncDefPrices.IsEnabled = bEnable;
            BtnSyncUser.IsEnabled = bEnable;
            BtnSyncStock.IsEnabled = bEnable;
            BtnClose.IsEnabled = bEnable;
        }

        private string showError(string sErrorInput) //105
        {
            string sError = "";

            if ((sErrorInput.Contains("No route to host")) || (sErrorInput.Contains("The operation has timed out")) || (sErrorInput.Contains("ConnectFailure")))
            {
                if (cData.settings.MobileService != "") 
                {
                    sError = String.Format(cTranslate.getValue("ErrorConnectieWCF"), cData.settings.MobileService);
                }
            }
            return sError;
        }
    }
}