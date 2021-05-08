using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using VerdeSoftApp;

namespace VerdeSoftApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageCashReg : ContentPage
    {

        private string kind = "";

        private string lastFocused = "";

        private string lastStatusSwitchKeyboard = "";
        private string lastStatusSwitchKeyboardSourceImage = "";

        public PageCashReg()
        {
            InitializeComponent();

        }

        public PageCashReg(string pKind)
        {
            InitializeComponent();


            if (!string.IsNullOrEmpty(lastStatusSwitchKeyboard))
            {
                SwitchKeyboard(lastStatusSwitchKeyboard);
                ImgBtnSwitchKeyboard.Source = ImageSource.FromResource(lastStatusSwitchKeyboardSourceImage);
            }
            else 
            {
                //is keyboard alpha numeric uit settings - Laurens
                if (cData.settings.KeybAlpha == true)
                {
                    EntryScan.Keyboard = Keyboard.Default;
                    lastStatusSwitchKeyboard = "alpha";
                    lastStatusSwitchKeyboardSourceImage = "VerdeSoftApp.keyboard-num.png";
                }
                else
                {
                    EntryScan.Keyboard = Keyboard.Numeric; //cData.kBoard;
                    lastStatusSwitchKeyboard = "num";
                    lastStatusSwitchKeyboardSourceImage = "VerdeSoftApp.keyboard-alpha.png";
                }
                ImgBtnSwitchKeyboard.Source = ImageSource.FromResource(lastStatusSwitchKeyboardSourceImage);
            }

            
            
            

            kind = pKind;

            initEntries();

            //EntryPrice.Completed += EntryPrice_Completed;
            EntryPrice.Focused += EntryPrice_Focused;
            cData.hArticleSelected += ArticleSelected_Handler;
            cData.hCustomerSelected += CData_hCustomerSelected;
            cData.hTotalChanged += CData_hTotalChanged;

            calculateTotal();

            //BindingContext = this;

            //BtnSearch.ImageSource = ImageSource.FromResource("VerdeSoftApp.search.png");
            ImgBtnSearch.Source = ImageSource.FromResource("VerdeSoftApp.search.png");
            

            //Device.StartTimer(TimeSpan.FromMilliseconds(50), () =>
            //{
            //    EntryScan.Focus();
            //    return false;
            //});


            //var LabelTotal_tap = new TapGestureRecognizer();
            //LabelTotal_tap.Tapped += (s, e) =>
            //{
            //    cWCF.InsertScannedArticles_WCF("",scannedArticles);
            //};
            //LabelTotal.GestureRecognizers.Add(LabelTotal_tap);

            cData.setDefaultCustomerCode();

            this.Title = "PageCashReg";

            translatePage();
        }

        private void SwitchKeyboard(string statusKeyboard) 
        {
            if (statusKeyboard.Equals("num")) 
            {
                lastStatusSwitchKeyboard = "alpha";
                ImgBtnSwitchKeyboard.Source = ImageSource.FromResource("VerdeSoftApp.keyboard-num.png");
                lastStatusSwitchKeyboardSourceImage = "VerdeSoftApp.keyboard-num.png";
                EntryScan.Keyboard = Keyboard.Default;
                EntryScan.Focus();
            }
            else
            {
                lastStatusSwitchKeyboard = "num";
                ImgBtnSwitchKeyboard.Source = ImageSource.FromResource("VerdeSoftApp.keyboard-alpha.png");
                lastStatusSwitchKeyboardSourceImage = "VerdeSoftApp.keyboard-alpha.png";
                EntryScan.Keyboard = Keyboard.Numeric;
                EntryScan.Focus();
            }
        }



        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            if (cData.selectedArticle == null) //v1.0.3
            {
                EntryScan.Focus();
            }
        }

        protected override bool OnBackButtonPressed()
        {
            //return base.OnBackButtonPressed();
            cNavigate.openPreviousPage();
            return true;
        }
        
        private void translatePage()
        {
            BtnCustomers.Text = cTranslate.getValue("Klanten");
            BtnReturn.Text = cTranslate.getValue("Terug");
            BtnList.Text = cTranslate.getValue("Lijst");

            if (kind == "K")
            {
                LabelTitle.Text = cTranslate.getValue("Kassa");

                if (cData.settings.PaymentScreen) //105
                {
                    BtnPay.Text = cTranslate.getValue("Betalen");
                }
                else //105
                {
                    BtnPay.Text = cTranslate.getValue("Versturen");
                }
            }
            else if (kind == "L")
            {
                LabelTitle.Text = cTranslate.getValue("Etiketten");
                BtnPay.Text = cTranslate.getValue("Versturen");
            }
            else if (kind == "O")
            {
                LabelTitle.Text = cTranslate.getValue("Bestellingen");
                BtnPay.Text = cTranslate.getValue("Versturen");
            }
            else if (kind == "I")
            {
                LabelTitle.Text = cTranslate.getValue("Inventaris");
                BtnPay.Text = cTranslate.getValue("Versturen");
                BtnStockLocation.Text = cTranslate.getValue("Stockplaats");
            }
            else if (kind == "C")
            {
                LabelTitle.Text = cTranslate.getValue("Controle");
                BtnPay.Text = cTranslate.getValue("Versturen");
            }
            else if (kind == "D")
            {
                LabelTitle.Text = cTranslate.getValue("Breuk");
                BtnPay.Text = cTranslate.getValue("Versturen");
            }

            EntryScan.Placeholder = cTranslate.getValue("Barcode");
            EntryQuantity.Placeholder = cTranslate.getValue("Aantal");
            EntryPrice.Placeholder = cTranslate.getValue("Prijs");
            EntryDiscount.Placeholder = cTranslate.getValue("Korting");
            BtnNext.Text = ">";
            BtnPrev.Text = "<";
        }

        private void CData_hTotalChanged()
        {
            calculateTotal();
        }

        private void ArticleSelected_Handler()
        {
            if (cData.selectedArticle != null)
            {
                Device.StartTimer(TimeSpan.FromMilliseconds(30), () =>
                {
                    if (cData.selectedArticle != null) //
                    {
                        LabelArtName.Text = cData.selectedArticle.Name;
                        EntryPrice.Text = cData.getSelectedPriceStr("1");
                        EntryQuantity.Text = "1";
                        if (cData.settings.RepeatQuantity == true) EntryQuantity.Text = cData.qty; //1.0.4b (repeat quantity)
                        EntryQuantity.Focus();
                        if (cData.DefPricePerc > 0) EntryDiscount.Text = Convert.ToString(cData.DefPricePerc); //1.0.4 na getSelectedPrice
                        LabelBesteleenheidArtikelcode.Text = "Code: " + cData.selectedArticle.Code; //105 add besteleenheid van artikel
                        LabelBesteleenheidArtikelcode.Text += "    " + cTranslate.getValue("BestelEenheid") + ": " + cData.selectedArticle.OrderQuantity;
                    }

                    return false; // return true to repeat counting, false to stop timer
                });
            }
        }

        private void CData_hCustomerSelected()
        {
            if (cData.selectedCustomer != null)
            {
                Device.StartTimer(TimeSpan.FromMilliseconds(30), () =>
                {
                    LabelCustomer.Text = cData.selectedCustomer.Name + " - " + cTranslate.getValue("Prijs") + " " + cData.selectedCustomer.Price.ToString();

                    return false; // return true to repeat counting, false to stop timer
                });
            }
        }

        private void initEntries()
        {
            cData.selectedArticle = null; //!
            EntryScan.Text = "";
            EntryPrice.Text = "0,00";
            EntryQuantity.Text = "1";
            if (cData.settings.RepeatQuantity == true) 
                EntryQuantity.Text = cData.qty; //1.0.4b (repeat quantity)
            EntryDiscount.Text = "0";
            LabelArtName.Text = "";
            LabelBesteleenheidArtikelcode.Text = "";
            if (kind != "I") //inventory
            {
                BtnStockLocation.IsVisible = false;
            }
        }

        private async void SearchValue()
        {
            //cData.name = EntryScan.Text;
            //cData.articleList = new List<eArticle>();
            //eArticle a = new eArticle();
            //a.ArtId = 0;
            //a.Code = "TEST";
            //a.Name = "TEST";
            //a.Price1 = 0;
            //cData.articleList.Add(a);
            //cData.selectedArticle = cData.articleList[0];
            //ArticleSelected_Handler();

            cData.name = EntryScan.Text;

            if ((cData.settings.VerifyBarcode == true) && (cTools.isNumeric(cData.name, System.Globalization.NumberStyles.Number, false) == true)) //Lengte barcode
            {
                if (cData.name.Length == 7) cData.name = "0" + cData.name;
                if (cData.name.Length == 12) cData.name = "0" + cData.name;
            }

            //check als kassa is EN Klantenkaart is ingevuld EN barcode begint met klantenkaart, toon boodschap dat het om een klantenkaart is //105
            if (kind == "K" && (!String.IsNullOrEmpty(cData.settings.CustomerCard)) && (cData.name.StartsWith(cData.settings.CustomerCard)))
            {
                //await DisplayAlert("Klantenkaart!", "Ingevulde barcode is start met klantenkaart code, ", "OK");
                
                cSQLite.selectCustomersField("Barcode", cData.name, enumOperator.Equals);

                if (cData.customerList.Count >= 1)
                {
                    eCustomer customerItem = cData.customerList[0];
                    List<eScanned> scannedArticles = cData.Get_ScannedArticles(kind);
                    if (scannedArticles.Count > 0)
                    {
                        if (cData.selectedCustomer.Price != 1 && cData.selectedCustomer.Price > 1) 
                        {
                            cSound.beepSound();
                            bool answer = await DisplayAlert(cTranslate.getValue("Vraag"), cTranslate.getValue("Prijzen aanpassen") + " ?", cTranslate.getValue("Ja"), cTranslate.getValue("Nee"));
                            if (answer == true)
                            {
                                cData.Set_Customer(kind, customerItem);
                                cData.bTotalChanged = true;
                            }
                        }
                    }
                    cData.selectedCustomer = customerItem; //hierdoor wordt de klant geset en event CData_hCustomerSelected uitgevoerd. (event is in cEntities gedefinieerd)
                    EntryScan.Text = "";
                    EntryScan.Focus();
                }
                else 
                {
                    //klantenkaart met barcode niet gevonden //105
                    cSound.beepSound();
                    await DisplayAlert(cTranslate.getValue("Foute klantenkaart"), cTranslate.getValue("CustomerCard") + ": " + EntryScan.Text +  " " + cTranslate.getValue("Niet gevonden").ToLower()+ ".", "OK");
                    EntryScan.Focus();
                    EntryScan.CursorPosition = 0;
                    EntryScan.SelectionLength = EntryScan.Text.Length;
                }
            }
            else 
            {
                //cWCF.SearchArticles_WCF(cData.settings.Identification, "Barcode", EntryScan.Text, false);
                cSQLite.selectArticlesBarcodeOrCodeName(cData.name); //105 (search also on Code or Name)
                if (cData.articleList.Count > 1)
                {
                    cNavigate.openPage(new PageArticles());
                }
                else if (cData.articleList.Count == 1)
                {
                    cData.selectedArticle = cData.articleList[0];
                    ArticleSelected_Handler();
                }
                else
                {
                    string entryNotFound = EntryScan.Text;
                    EntryScan.Text = "";
                    cSound.beepSound();
                    if (cData.settings.QuestionNotFound == false)
                    {
                        await DisplayAlert("Alert", cData.name + " " + cTranslate.getValue("Niet gevonden"), "OK");
                        EntryScan.Focus();
                    }
                    else 
                    {
                        bool answer = await DisplayAlert(cTranslate.getValue("Vraag"), cData.name + " " + cTranslate.getValue("Niet gevonden") + ". " + cTranslate.getValue("Doorgaan") + " ?", cTranslate.getValue("Ja"), cTranslate.getValue("Nee"));
                        if (answer == true)
                        {
                            EntryScan.Text = entryNotFound;
                            eArticle newArt = cData.createArtNotFound(entryNotFound);
                            cData.selectedArticle = newArt;
                            ArticleSelected_Handler();
                        }
                        else 
                        {
                            EntryPrice.Text = "";
                            EntryDiscount.Text = "";
                            EntryQuantity.Text = "";
                            EntryScan.Text = "";
                            EntryScan.CursorPosition = 0;
                            EntryScan.SelectionLength = EntryScan.Text.Length;
                            EntryScan.Focus();
                        }
                    }
                }
            }
        }

        public async void EntryScan_Completed(object sender, EventArgs e)
        {
            SearchValue();
        }

        private void EntryQuantity_Completed(object sender, EventArgs e)
        {
            if (cData.selectedArticle != null)
            {
                double selectedPrice = 0;
                if (EntryQuantity.Text.Length >= 7) //barcode in quantity (0)barcode = 7 lang
                {
                    selectedPrice = cData.getSelectedPrice("1");
                    if (selectedPrice != 0)
                    {
                        string nextBarcode = EntryQuantity.Text;
                        string qty = "1";
                        if (cData.settings.RepeatQuantity == true) qty = cData.qty; //1.0.4b (repeat quantity) *
                        insertScannedArticle(qty); //huidig toevoegen met aantal 1 (standaard)
                        EntryScan.Text = nextBarcode;
                        SearchValue();
                    }
                    else
                    {
                        cSound.beepSound();
                        EntryQuantity.Text = "1";
                        if (cData.settings.RepeatQuantity == true) EntryQuantity.Text = cData.qty; //1.0.4b (repeat quantity) *
                        EntryPrice.Focus();
                    }
                }
                else
                {
                    cData.qty = EntryQuantity.Text; //1.0.4 b

                    selectedPrice = cData.getSelectedPrice(EntryQuantity.Text);
                    if (selectedPrice != 0)
                    {
                        EntryPrice.Text = selectedPrice.ToString("0.00");
                        if (cData.DefPricePerc > 0) EntryDiscount.Text = Convert.ToString(cData.DefPricePerc); //1.0.4 na getSelectedPrice
                        insertScannedArticle(EntryQuantity.Text);
                        EntryScan.Focus();
                    }
                    else
                    {
                        //cSound.beepSound();
                        EntryPrice.Focus();
                    }
                }
            }
            else
            {
                EntryScan.Focus();
            }
        }

        private void EntryDiscount_Completed(object sender, EventArgs e)
        {
            if (LabelArtName.Text != "")
            {
                //if (cData.selectedArticle.Price1 > 0)
                //{
                //insertScannedArticle(EntryQuantity.Text);
                //EntryScan.Focus();
                //}
                //else
                //{
                EntryPrice.Focus();
                //}
            }
        }

        private void EntryPrice_Completed(object sender, EventArgs e)
        {
            if (EntryPrice.Text.Length < 7) //barcode in quantity (0)barcode = 7 lang
            {
                insertScannedArticle(EntryQuantity.Text);
                EntryScan.Focus();
            }
            else
            {
                cSound.beepSound();
                EntryPrice.Focus();
            }
        }

        private void insertScannedArticle(string pQuantity)
        {
            eScanned a = cData.Prepare_eScanned(cData.selectedArticle, 0, pQuantity, EntryPrice.Text, EntryDiscount.Text, kind);
            cData.Insert_ScannedArticle(a); //add database

            calculateTotal();

            initEntries();
        }

        private void calculateTotal()
        {
            List<eScanned> scannedArticles = cData.Get_ScannedArticles(kind);
            eTotal total = cData.calculateTotal(scannedArticles);
            if (kind == "I")
            {
                LabelTotal.Text = cData.stockLocation + "  -  # " + total.TotalQuantity.ToString(); 
            }
            else
            {
                LabelTotal.Text = "T " + cData.settings.Ticket.ToString() + "  -  # " + total.TotalQuantity.ToString() + "  -  € " + total.TotalAmount.ToString("0.00");
            }
        }

        protected void EntryQuantity_Focused(object sender, FocusEventArgs e)
        {
            EntryQuantity.BackgroundColor = Color.Yellow;
            EntryQuantity.CursorPosition = 0;
            EntryQuantity.SelectionLength = EntryQuantity.Text.Length;
            lastFocused = "EntryQuantity";
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
            lastFocused = "EntryPrice";
        }

        private void EntryPrice_Unfocused(object sender, FocusEventArgs e)
        {
            EntryPrice.BackgroundColor = Color.White;
            checkLostFocus();
        }

        protected void EntryDiscount_Focused(object sender, FocusEventArgs e)
        {
            EntryDiscount.BackgroundColor = Color.Yellow;
            EntryDiscount.CursorPosition = 0;
            EntryDiscount.SelectionLength = EntryDiscount.Text.Length;
            lastFocused = "EntryDiscount";
        }

        private void EntryDiscount_Unfocused(object sender, FocusEventArgs e)
        {
            EntryDiscount.BackgroundColor = Color.White;
            checkLostFocus();
        }

        protected void EntryScan_Focussed(object sender, FocusEventArgs e)
        {
            EntryScan.BackgroundColor = Color.Yellow;
            EntryScan.CursorPosition = 0;
            EntryScan.SelectionLength = EntryScan.Text.Length;
            lastFocused = "EntryScan";
        }

        private void EntryScan_Unfocused(object sender, FocusEventArgs e)
        {
            EntryScan.BackgroundColor = Color.White;
            checkLostFocus();
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            cNavigate.openPage(new PageSearchArt("PageCashReg"));
        }

        private void ImgBtnSwitchKeyboard_Clicked(object sender, EventArgs e)
        {
            //cNavigate.openPage(new PageSearchArt("PageCashReg"));
            SwitchKeyboard(lastStatusSwitchKeyboard);
        }

        async void OnAlertYesNoClicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Question?", "Would you like to play a game", "Yes", "No");
            // Debug.WriteLine("Answer: " + answer);

            //string result = await DisplayPromptAsync("Question 1", "What's your name?");
        }

        private void BtnMinus_Clicked(object sender, EventArgs e)
        {
            if (EntryQuantity.Text.Contains("-") == false)
            {
                EntryQuantity.Text = "-" + EntryQuantity.Text;
                EntryQuantity.Focus();
            }
            else
            {
                EntryQuantity.Text = EntryQuantity.Text.Replace("-", "");
                EntryQuantity.Focus();
            }
        }

        private void BtnList_Clicked(object sender, EventArgs e)
        {
            //for (int t = 0; t<=200; t++)
            //{
            //    cData.selectedArticle = new eArticle();
            //    cData.selectedArticle.ArtId = 3729;
            //    cData.selectedArticle.Code = "BAAALBA";
            //    cData.selectedArticle.ID = 3727;
            //    cData.selectedArticle.Name = "BAPTIS";
            //    cData.selectedArticle.Price1 = 3;
            //    cData.selectedArticle.Price2 = 0;
            //    cData.selectedArticle.Price3 = 0;
            //    cData.selectedArticle.Price4 = 0;
            //    cData.selectedArticle.Price5 = 0;
            //    cData.selectedArticle.Price6 = 0;
            //    cData.selectedArticle.Price7 = 0;
            //    cData.selectedArticle.Price8 = 0;

            //    insertScannedArticle("1"); //test
            //}
            if (cData.selectedArticle != null)
            {
                double price = Convert.ToDouble(EntryPrice.Text);
                if (price > 0 && price < 25000)
                { 
                    insertScannedArticle(EntryQuantity.Text); //laatste gescande article (gescand op aantal)
                    cNavigate.openPage(new PageScanned(kind));
                }
                else
                {
                    cSound.beepSound();
                    EntryPrice.Focus();
                }
            }
            else
            {
                cNavigate.openPage(new PageScanned(kind));
            }
        }

        private async void BtnPay_Clicked(object sender, EventArgs e)
        {
            bool contin = true;
            if (cData.selectedArticle != null)
            {
                double price = Convert.ToDouble(EntryPrice.Text);
                if (price > 0 && price < 25000)
                {
                    insertScannedArticle(EntryQuantity.Text); //laatste gescande article (gescand op aantal)
                }
                else
                {
                    contin = false;
                    cSound.beepSound();
                    EntryPrice.Focus();
                }
            }

            if (contin == true)
            {
                if (kind == "K")
                {
                    if (cData.settings.PaymentScreen)
                    {
                        cNavigate.openPage(new PagePay(kind));
                    }
                    else 
                    {
                        VersturenNaarWCF();
                    }
                }
                else
                {
                    VersturenNaarWCF();
                }
            }
        }

        private async void VersturenNaarWCF() 
        {
            

            List<eScanned> scannedArticles = cData.Get_ScannedArticles(kind);
            string err = "";
            if (kind == "K")
            {
                List<ePayItem> payList = new List<ePayItem>();
                err = cWCF.InsertScannedArticles_CR_WCF(cData.settings.Ticket, cData.settings.Identification, scannedArticles, payList); //add Receipt (Ticketnr) //106 //Laurens
                if (err == "")
                {
                    cData.Delete_ScannedArticles(kind);
                    cData.IncreaseTicket();
                    cData.setDefaultCustomerCode();
                    calculateTotal();
                    EntryScan.Focus();
                }
            }
            else
            {
                err = cWCF.InsertScannedArticles_WCF(cData.settings.Identification, scannedArticles);
                if (err == "")
                {
                    cData.Delete_ScannedArticles(kind);
                    calculateTotal();
                    EntryScan.Focus(); //1.0.4b (focus)
                }
            }
            if (err == "")
            {

                cTools.clearCacheAndroid(); //Clear Cache Android
                scannedArticles = cData.Get_ScannedArticles(kind);
                if (scannedArticles.Count > 0) //voor scanners met weinig geheugen (abeels)
                {
                    cData.Delete_ScannedArticles(kind);
                    scannedArticles = cData.Get_ScannedArticles(kind);
                    if (scannedArticles.Count > 0)
                    {
                        await DisplayAlert(cTranslate.getValue("Bericht"), cTranslate.getValue("ProbleemLeegmakenLijstScannedArticles"), cTranslate.getValue("OK"));
                    }
                }

                if (cData.settings.NoConfirmation == false)
                {
                    await DisplayAlert(cTranslate.getValue("Bericht"), cTranslate.getValue("Lijst verstuurd"), cTranslate.getValue("OK"));
                }
            }
            else
            {
                await DisplayAlert(cTranslate.getValue("ErrorWCF"), err, cTranslate.getValue("OK"));
            }
        }

        private void BtnReturn_Clicked(object sender, EventArgs e)
        {
            cNavigate.openPreviousPage();
        }

        private void BtnCustomers_Clicked(object sender, EventArgs e)
        {
            cNavigate.openPage(new PageCustomers(kind));
        }

        private async void BtnStockLocation_Clicked(object sender, EventArgs e)
        {
            cData.stockLocation = await DisplayPromptAsync(cTranslate.getValue("Vraag"), cTranslate.getValue("Stockplaats"), cTranslate.getValue("OK"), cTranslate.getValue("Annuleer"));
            EntryScan.Focus();
            calculateTotal(); //set label ...
        }

        private void BtnPrev_Clicked(object sender, EventArgs e)
        {
            if (lastFocused == "EntryQuantity")
            {
                EntryScan.Focus();
            }
            else if (lastFocused == "EntryPrice")
            {
                EntryQuantity.Focus();
            }
            else if (lastFocused == "EntryDiscount")
            {
                EntryPrice.Focus();
            }
        }

        private void BtnNext_Clicked(object sender, EventArgs e)
        {
            if (lastFocused == "EntryScan")
            {
                EntryQuantity.Focus();
            }
            else if (lastFocused == "EntryQuantity")
            {
                EntryPrice.Focus();
            }
            else if (lastFocused == "EntryPrice")
            {
                EntryDiscount.Focus();
            }
        }

        private void OnStackLayout_Tapped(object sender, EventArgs e)
        {
            Console.WriteLine("OnStackLayout_Tapped");
            Device.StartTimer(TimeSpan.FromMilliseconds(50), () =>
            {
                if (lastFocused == "EntryScan")
                {
                    EntryScan.Focus();
                }
                else if (lastFocused == "EntryPrice")
                {
                    EntryPrice.Focus();
                }
                else if (lastFocused == "EntryQuantity")
                {
                    EntryQuantity.Focus();
                }
                else if (lastFocused == "EntryDiscount")
                {
                    EntryDiscount.Focus();
                }
                else
                {
                    EntryScan.Focus();
                }
                return false;
            });
            
        }

        private void OnStackLayout_Tapped2(object sender, EventArgs e)
        {
            //Console.WriteLine("OnStackLayout_Tapped2");
            //Device.StartTimer(TimeSpan.FromMilliseconds(50), () =>
            //{
            //    if (lastFocused == "EntryScan")
            //    {
            //        Console.WriteLine("OnStackLayout_Tapped2: focus EntryScan");
            //        EntryScan.Focus();
            //    }
            //    else if (lastFocused == "EntryPrice")
            //    {
            //        Console.WriteLine("OnStackLayout_Tapped2: focus EntryPrice");
            //        EntryPrice.Focus();
            //    }
            //    else if (lastFocused == "EntryQuantity")
            //    {
            //        Console.WriteLine("OnStackLayout_Tapped2: focus EntryQuantity");
            //        EntryQuantity.Focus();
            //    }
            //    else if (lastFocused == "EntryDiscount")
            //    {
            //        Console.WriteLine("OnStackLayout_Tapped2: focus EntryDiscount");
            //        EntryDiscount.Focus();
            //    }
            //    else
            //    {
            //        Console.WriteLine("OnStackLayout_Tapped2: focus EntryScan");
            //        EntryScan.Focus();
            //    }
            //    return false;
            //});

        }

        private void checkLostFocus()
        {
            //Console.WriteLine("setFocusBackIfLost");

            //Device.StartTimer(TimeSpan.FromSeconds(2), () =>
            //{
            //    if (EntryScan.IsFocused == false && EntryQuantity.IsFocused == false && EntryPrice.IsFocused == false && EntryDiscount.IsFocused == false)
            //    {
            //        Console.WriteLine("setFocusBackIfLost -> EntryScan.Focus()");
            //        //Console.WriteLine("lastFocused -> )"+ lastFocused);
            //        EntryScan.Focus();
            //        if (lastFocused == "EntryScan")
            //        {
            //            EntryScan.Focus();
            //        }
            //        else if (lastFocused == "EntryPrice")
            //        {
            //            EntryPrice.Focus();
            //        }
            //        else if (lastFocused == "EntryQuantity")
            //        {
            //            EntryQuantity.Focus();
            //        }
            //        else if (lastFocused == "EntryDiscount")
            //        {
            //            EntryDiscount.Focus();
            //        }
            //        else
            //        {
            //            EntryScan.Focus();
            //        }
            //    }
            //    return false;
            //});
        }

        
    }
}