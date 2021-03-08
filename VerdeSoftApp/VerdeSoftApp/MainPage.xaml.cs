using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using VerdeSoftApp;
using System.Net.Http;
using System.Reflection;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Plugin.DeviceInfo;

//version 1.0.1 : klikken op achtergrond : focus terugzetten om verder te kunnen scannen !

//version 1.0.2 : compatible met verdesoft v35

//version 1.0.3 : geen structuurwijziging, compatible met verdesoft v35
//version 1.0.3 : algemene korting kunnen geven in betaalscherm
//version 1.0.3 : bij wijzigen klant in kassa, vraag stellen 'prijzen aanpassen?' (reeds gescande regels aanpassen!)
//version 1.0.3b: onbekend artikel : doorgaan
//version 1.0.3c: onbekend artikel : doorgaan
//version 1.0.3d: mobile service : app.config = maxReceivedMessageSize="2147483647"
//version 1.0.4 : veld stocklocation in eScanned
//version 1.0.4 : tabel barcodes
//version 1.0.4b: herhaal aantal + ping

namespace VerdeSoftApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public List<string> UserList;

        public MainPage()
        {
            InitializeComponent();

            backgroundImage.Source = ImageSource.FromResource("VerdeSoftApp.Verdesoft-02.png");

            //Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            //Xamarin.Forms.NavigationPage.SetHasBackButton(this, false);

            //ImgBtn2.ImageSource = ImageSource.FromResource("VerdeSoftApp.search.png");
            //ImgBtn.Source = ImageSource.FromResource("VerdeSoftApp.search.png", typeof(MainPage).GetTypeInfo().Assembly);

            //UserList = new List<string> { "Andy", "Hendrik", "Ann","Test1","Test2","Test3","Test4","Test5","Test6" };
            UserList = cSQLite.getUsers();

            DropDownUsers.ItemsSource = UserList;
            DropDownUsers.SelectedIndex = 1;
            DropDownUsers.SelectedIndexChanged += DropDownUsers_SelectedIndexChanged;

            cSQLite.getDefPrices();

            BindingContext = this;

            //var assembly = typeof(MainPage).GetTypeInfo().Assembly;
            //foreach (var res in assembly.GetManifestResourceNames()) 
            //	System.Diagnostics.Debug.WriteLine("found resource: " + res);

            //if (System.Globalization.CultureInfo.CurrentCulture.Name == "nl-BE")
            //{
            //    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("nl-BE");
            //    System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".";
            //}
            //if (System.Globalization.CultureInfo.CurrentCulture.Name == "fr-BE")
            //{
            //    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("fr-BE");
            //    System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".";
            //}

            //System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            //customCulture.NumberFormat.NumberDecimalSeparator = ".";
            //System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            this.Title = "MainPage";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            cSQLite.getSettings();
            translatePage();

            cData.deviceId = CrossDeviceInfo.Current.Id;
            //string enc = cTools.EncodeToAsciiSum(cData.deviceId);
            //string enc2 = enc;
        }

        private void translatePage()
        {
            BtnCashRegister.Text = cTranslate.getValue("Kassa");
            BtnClose.Text = cTranslate.getValue("Afsluiten");
            BtnInventory.Text = cTranslate.getValue("Inventaris");
            BtnLabel.Text = cTranslate.getValue("Etiketten");
            BtnOrder.Text = cTranslate.getValue("Bestellingen");
            BtnSettings.Text = cTranslate.getValue("Instellingen");
            BtnSync.Text = cTranslate.getValue("Synchroniseer");
            DropDownUsers.Title = cTranslate.getValue("Gebruikers");
            BtnDamage.Text = cTranslate.getValue("Breuk");
            BtnControle.Text = cTranslate.getValue("Controle");
        }
        
        private void DropDownUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Xamarin.Forms.Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                //do somthing here
                cData.userCode = (string)picker.ItemsSource[selectedIndex];
            }
        }

        private void BtnCashRegister_Clicked(object sender, EventArgs e)
        {
            cSQLite.getSettings();
            cNavigate.openPage(new PageCashReg("K"));
            //cNavigate.openPage(new PageImage());


        }

        private void BtnStock_Clicked(object sender, EventArgs e)
        {
            cSQLite.getSettings();
            cNavigate.openPage(new PageStock());
        }

        private void BtnSettings_Clicked(object sender, EventArgs e)
        {
            cSQLite.connectSQLite();
            cSQLite.verifySQLite();
            cNavigate.openPage(new PageOptions());
            //cNavigate.openPage(new PageSettings());
        }

        private void  BtnTest_Clicked(object sender, EventArgs e)
        {
            cNavigate.openPage(new PageMonkeys());
            
        }

        private void BtnSync_Clicked(object sender, EventArgs e)
        {
            cNavigate.openPage(new PageSync());
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {

        }
        private void BtnInitDatabase_Clicked(object sender, EventArgs e)
        {
            
        }

        private void BtnClose_Clicked(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
        }

        private void BtnLabel_Clicked(object sender, EventArgs e)
        {
            cSQLite.getSettings();
            cNavigate.openPage(new PageCashReg("L"));

            
        }

        private void BtnOrder_Clicked(object sender, EventArgs e)
        {
            cSQLite.getSettings();
            cNavigate.openPage(new PageCashReg("O"));
        }

        private void BtnInventory_Clicked(object sender, EventArgs e)
        {
            cSQLite.getSettings();
            cNavigate.openPage(new PageCashReg("I"));
        }

        private void BtnControle_Clicked(object sender, EventArgs e)
        {
            cSQLite.getSettings();
            cNavigate.openPage(new PageCashReg("C"));
        }

        private void BtnDamage_Clicked(object sender, EventArgs e)
        {
            cSQLite.getSettings();
            cNavigate.openPage(new PageCashReg("D"));
        }

    }
}
