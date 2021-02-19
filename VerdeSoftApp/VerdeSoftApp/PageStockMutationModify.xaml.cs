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
    public partial class PageStockMutationModify : ContentPage
    {
        private eStockMutation SelectedItem = null;
        private double MaxMutationQuantity = 0;
        private double Total = 0; //MUT2

        public PageStockMutationModify()
        {
            InitializeComponent();
        }

        public PageStockMutationModify(eStockMutation pSelectedItem)
        {
            InitializeComponent();
            SelectedItem = pSelectedItem;

            EntryQuantity.Text = SelectedItem.Quantity.ToString("");
            EntryPlace.Text = SelectedItem.Place;
            EntryRemark.Text = SelectedItem.Remark;
            LabelArtName.Text = SelectedItem.ArtName;

            this.Title = "PageStockMutationModify";

            translatePage();

            if (pSelectedItem.Kind == "ADD")
            {
                LabelTitle.Text = cTranslate.getValue("Toevoegen");
            }

            if (pSelectedItem.Kind == "MUT")
            {
                LabelTitle.Text = cTranslate.getValue("Opsplitsen");
                MaxMutationQuantity = SelectedItem.Quantity;
                LabelQuantityMut.Text = SelectedItem.Quantity.ToString("");
                LabelPlaceMut.Text = "";
                LabelRemarkMut.Text = "";
            }
            else
            {
                Grid1.RowDefinitions[1].Height = 0;
            }

            if (pSelectedItem.Kind == "MUT2") //toevoegen of verwijderen aantal uit bestaande regel
            {
                LabelTitle.Text = cTranslate.getValue("Verwijderen");
                EntryQuantity.Text = "0";
            }

            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            EntryQuantity.Focus();
        }

        protected override bool OnBackButtonPressed()
        {
            //return base.OnBackButtonPressed();
            cNavigate.openPreviousPage();
            return true;
        }

        private void translatePage()
        {
            LabelTitle.Text = cTranslate.getValue("Wijzigen");
            BtnReturn.Text = cTranslate.getValue("Terug");
            BtnSave.Text = cTranslate.getValue("Opslaan");
            LabelQuantity.Text = cTranslate.getValue("Aantal");
            LabelPlace.Text = cTranslate.getValue("Plaats");
            LabelRemark.Text = cTranslate.getValue("Opmerking");
        }
        private void EntryQuantity_Completed(object sender, EventArgs e)
        {
            EntryPlace.Focus();
        }

        private void EntryPlace_Completed(object sender, EventArgs e)
        {
            EntryRemark.Focus();
        }

        private void EntryRemark_Completed(object sender, EventArgs e)
        {
            
        }

        protected void EntryQuantity_Focused(object sender, FocusEventArgs e)
        {
            EntryQuantity.BackgroundColor = Color.Yellow;
            EntryQuantity.CursorPosition = 0;
            EntryQuantity.SelectionLength = EntryQuantity.Text.Length;
        }

        private void EntryQuantity_Unfocused(object sender, FocusEventArgs e)
        {
            EntryQuantity.BackgroundColor = Color.White;
        }

        protected void EntryPlace_Focused(object sender, FocusEventArgs e)
        {
            EntryPlace.BackgroundColor = Color.Yellow;
            EntryPlace.CursorPosition = 0;
            EntryPlace.SelectionLength = EntryPlace.Text.Length;
        }

        private void EntryPlace_Unfocused(object sender, FocusEventArgs e)
        {
            EntryPlace.BackgroundColor = Color.White;
        }

        protected void EntryRemark_Focused(object sender, FocusEventArgs e)
        {
            EntryRemark.BackgroundColor = Color.Yellow;
            EntryRemark.CursorPosition = 0;
            EntryRemark.SelectionLength = EntryRemark.Text.Length;
        }

        private void EntryRemark_Unfocused(object sender, FocusEventArgs e)
        {
            EntryRemark.BackgroundColor = Color.White;
        }

        private void BtnReturn_Clicked(object sender, EventArgs e)
        {
            cNavigate.openPreviousPage();
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

        private void saveEntity()
        {
            if (SelectedItem.Kind == "MUT")
            {
                double mutQuantity = Convert.ToDouble(EntryQuantity.Text);
                if (mutQuantity <= MaxMutationQuantity) //controle
                {
                    updateEntityMutation(mutQuantity);
                    cNavigate.openPreviousPage();
                }
                else
                {

                }
            }
            else
            {
                updateEntity();
                cNavigate.openPreviousPage();
            }
        }

        private void BtnSave_Clicked(object sender, EventArgs e)
        {
            saveEntity();
        }

        private void updateEntity()
        {
            if (SelectedItem != null)
            {
                SelectedItem = cData.Modify_eStockMutation(SelectedItem, EntryQuantity.Text, EntryPlace.Text, EntryRemark.Text);

                if (SelectedItem.Kind == "ADD")
                {
                    cData.Insert_StockMutation(SelectedItem); //STOCK TOEVOEGEN = Add record with "ADD"
                }
                if (SelectedItem.Kind == "MOD")
                {
                    cData.Insert_StockMutation(SelectedItem); //STOCK WIJZIGEN = Add record with "MOD"
                }
                if (SelectedItem.Kind == "MUT2")
                {
                    SelectedItem.Quantity = SelectedItem.Quantity * -1;
                    SelectedItem.Kind = "MUT";
                    SelectedItem.MutKind = "ADD"; //bijtellen
                    cData.Insert_StockMutation(SelectedItem); //STOCK WIJZIGEN = Add record with "ADD" +/-
                }
                cData.bMutationChanged = true;
            }
        }

        private void updateEntityMutation(double pMutQuantity)
        {
            if (SelectedItem != null)
            {
                eStockMutation MutationItem = new eStockMutation();
                MutationItem.Kind = "MUT";
                MutationItem.MutKind = "ADD";
                MutationItem.ArtId = SelectedItem.ArtId;
                MutationItem.ArtName = SelectedItem.ArtName;
                MutationItem = cData.Modify_eStockMutation(MutationItem, EntryQuantity.Text, EntryPlace.Text, EntryRemark.Text);
                cData.Insert_StockMutation(MutationItem);

                SelectedItem.Kind = "MUT";
                SelectedItem.MutKind = "MOD";
                SelectedItem.Quantity = SelectedItem.Quantity - MutationItem.Quantity;
                cData.Insert_StockMutation(SelectedItem);

                cData.bMutationChanged = true;
            }
        }


    }
}