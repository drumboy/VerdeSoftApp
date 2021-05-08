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
    public partial class PagePay : ContentPage
    {
        List<eScanned> scannedArticles = null;
        List<ePayItem> payList = new List<ePayItem>();
        private string kind = "";

        private eTotal total = null;
        private eTotalPayed totalPayed = null;
        private double rest = 0;

        public PagePay()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            //return base.OnBackButtonPressed();
            cNavigate.openPreviousPage();
            return true;
        }

        public PagePay(string pKind)
        {
            InitializeComponent();
            kind = pKind;
            calculateTotal();
            //EntryPay.Behaviors.Add(new NumericValidationBehavior());


            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.NumberOfTapsRequired = 2;
            tapGestureRecognizer.Tapped += doubletapped;
            EntryPay.GestureRecognizers.Add(tapGestureRecognizer);

            //EntryPay.GestureRecognizers.Add()
            //var tapGestureRecognizer = new TapGestureRecognizer();
            //tapGestureRecognizer.NumberOfTapsRequired = 2; // double-tap
            //tapGestureRecognizer.Tapped += (s, e) => {
            //    // handle the tap
            //    EntryPay.Focus();
            //};
            //EntryPay.GestureRecognizers.Add(tapGestureRecognizer);
            
            translatePage();
        }

        private void translatePage()
        {
            LabelTitle.Text = cTranslate.getValue("Betalen");

            BtnCash.Text = cTranslate.getValue("Cash");
            BtnBancontact.Text = cTranslate.getValue("Bancontact");
            BtnVoucher.Text = cTranslate.getValue("Waardebon");
            BtnCheque.Text = cTranslate.getValue("Cheque");

            BtnReturn.Text = cTranslate.getValue("Terug");

            LTotal1.Text = cTranslate.getValue("Totaal");
            LPayed1.Text = cTranslate.getValue("Betaald");
            LPay.Text = cTranslate.getValue("Betaal");

            LTPayedCash.Text = cTranslate.getValue("Cash");
            LTPayedBC.Text = cTranslate.getValue("Bancontact");
            LTPayedVoucher.Text = cTranslate.getValue("Waardebon");
            LTPayedCheque.Text = cTranslate.getValue("Cheque");
        }

        private void doubletapped(object sender, EventArgs e)
        {
            EntryPay.Focus();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            EntryPay.Focus();
        }

        private void calculateTotal()
        {
            scannedArticles = cData.Get_ScannedArticles(kind);
            total = cData.calculateTotal(scannedArticles);
            totalPayed = cData.calculateTotalPayed(payList);

            LPayedCash.Text = totalPayed.Cash.ToString("0.00");
            LPayedBC.Text = totalPayed.Bancontact.ToString("0.00");
            LPayedVoucher.Text = totalPayed.Voucher.ToString("0.00");
            LPayedCheque.Text = totalPayed.Cheque.ToString("0.00");

            rest = total.TotalAmount - totalPayed.Total;

            LTotal2.Text = total.TotalAmount.ToString("0.00");
            LPayed2.Text = totalPayed.Total.ToString("0.00");
            EntryPay.Text = rest.ToString("0.00");
        }

        private void BtnCash_Clicked(object sender, EventArgs e)
        {
            addPayment("C");
        }

        private void BtnBancontact_Clicked(object sender, EventArgs e)
        {
            addPayment("BC");
        }

        private void BtnVoucher_Clicked(object sender, EventArgs e)
        {
            addPayment("V");
        }

        private void BtnCheque_Clicked(object sender, EventArgs e)
        {
            addPayment("CH");
        }

        private void addPayment(string pCode)
        {
            if (EntryPay.Text != "")
            {
                ePayItem p = new ePayItem();
                p.Code = pCode;
                p.Val = Convert.ToDouble(EntryPay.Text);
                payList.Add(p);

                calculateTotal();

                if (Math.Round(totalPayed.Total, 2) >= Math.Round(total.TotalAmount, 2))
                {
                    sendWCF();
                }
            }
            else
            {
                EntryPay.Text = rest.ToString("0.00");
                EntryPay.Focus();
            }
        }

        private async void sendWCF()
        {
            string err = cWCF.InsertScannedArticles_CR_WCF(cData.settings.Ticket, cData.settings.Identification, scannedArticles,payList);
            if (err == "")
            {
                cData.Delete_ScannedArticles(kind);
                cData.IncreaseTicket();
                cData.setDefaultCustomerCode();
                cNavigate.openPreviousPage();
            }
            else
            {
                await DisplayAlert("Error WCF", err, "OK");
            }
        }

        private void BtnReturn_Clicked(object sender, EventArgs e)
        {
            cNavigate.openPreviousPage();
        }

        public async void EntryPay_Completed(object sender, EventArgs e)
        {
            
        }

        protected void EntryPay_Focussed(object sender, FocusEventArgs e)
        {
            EntryPay.BackgroundColor = Color.Yellow;
            EntryPay.CursorPosition = 0;
            EntryPay.SelectionLength = EntryPay.Text.Length;
        }

        private void EntryPay_Unfocused(object sender, FocusEventArgs e)
        {
            EntryPay.BackgroundColor = Color.White;
        }

        public async void EntryDiscount_Completed(object sender, EventArgs e)
        {

        }

        protected void EntryDiscount_Focussed(object sender, FocusEventArgs e)
        {
            EntryDiscount.BackgroundColor = Color.Yellow;
            EntryDiscount.CursorPosition = 0;
            EntryDiscount.SelectionLength = EntryPay.Text.Length;
        }

        private void EntryDiscount_Unfocused(object sender, FocusEventArgs e)
        {
            EntryDiscount.BackgroundColor = Color.White;
        }

        private void BtnDiscount_Clicked(object sender, EventArgs e)
        {
            if (EntryDiscount.Text != "")
            {
                double discount = Convert.ToDouble(EntryDiscount.Text);
                cData.Set_Discount(kind, discount);
                calculateTotal();
                EntryPay.Focus();
            }
        }
    }
   
}