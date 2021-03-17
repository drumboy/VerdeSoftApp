using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using SQLite;
using PCLStorage;
using System.IO;
using System.Dynamic;
using System.Linq; //1.2
using System.Numerics;
using Xamarin.Forms; //keyboard

namespace VerdeSoftApp
{
    public static class cData
    {

        public static string ConnectionString = "";
        //"Userid=root;database=verdesoft;server=192.168.1.111;password=root;CharSet=utf8";
        //"Userid=root;database=verhulst34;server=192.168.1.116;password=";
        public static eSettings settings = null;

        public static string err = "";
        public static IList<eArticle> articleList = new List<eArticle>();
        public static IList<eCustomer> customerList = new List<eCustomer>();
        public static IList<eUser> userList = new List<eUser>();
        public static IList<eDefPrices> defPricesList = new List<eDefPrices>();
        public static IList<eStock> stockList = new List<eStock>();
        public static IList<eBarcodes> barcodesList = new List<eBarcodes>(); //1.0.4
        public static string barcode = "";
        public static string name = "";
        public static string qty = "1"; //1.0.4b

        public static string stockLocation = ""; //1.0.4
        public static double DefPricePerc = 0; //1.0.0.4

        //public static Keyboard kBoard = Keyboard.Numeric;

        public delegate void ArticleSelected_Handler();
        public static event ArticleSelected_Handler hArticleSelected;

        public delegate void CustomerSelected_Handler();
        public static event CustomerSelected_Handler hCustomerSelected;

        //public static eScanned modifyScanned = null;

        public static string deviceId = "";

        public static string userCode = "";

        public static IList<eScanned> scannedSameArtikelsInKassa = new List<eScanned>();

        //public static byte[] takedPhotoByteArray; //105
        public static string sInitLanguage = ""; //105


        private static eArticle _selectedArticle = null;
        public static eArticle selectedArticle
        {
            get
            {
                return _selectedArticle;
            }
            set
            {
                _selectedArticle = value;

                if (_selectedArticle != null)
                {
                    if (hArticleSelected != null) hArticleSelected();
                }
            }
        }

        //enkel zetten vanuit kassa
        private static eCustomer _selectedCustomer = null;
        public static eCustomer selectedCustomer
        {
            get
            {
                return _selectedCustomer;
            }
            set
            {
                _selectedCustomer = value;
                if (hCustomerSelected != null) hCustomerSelected();
            }
        }

        public delegate void TotalChanged_Handler();
        public static event TotalChanged_Handler hTotalChanged;

        private static bool _bTotalChanged;
        public static bool bTotalChanged
        {
            get
            {
                return _bTotalChanged;
            }
            set
            {
                _bTotalChanged = value;
                if (hTotalChanged != null) hTotalChanged();
            }
        }

        public delegate void MutationChanged_Handler();
        public static event MutationChanged_Handler hMutationChanged;

        private static bool _bMutationChanged;
        public static bool bMutationChanged
        {
            get
            {
                return _bMutationChanged;
            }
            set
            {
                _bMutationChanged = value;
                if (hMutationChanged != null) hMutationChanged();
            }
        }

        public static bool TestDatabaseConnection()
        {
            bool geslaagd = false;

            MySqlConnection cConn = new MySqlConnection(ConnectionString);

            try
            {
                cConn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = cConn;
                cmd.CommandText = "Select Code from articles";

                string code = "";

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    code = (string)reader.GetValue(0);
                }

                cmd.Dispose();
                cmd = null;

                cConn.Close();

                geslaagd = true;
            }
            catch (Exception ex)
            {

                err = ex.Message;
            }

            return geslaagd;
        }

        public static void SearchArticles()
        {
            articleList = new List<eArticle>();

            MySqlConnection cConn = new MySqlConnection(ConnectionString);

            try
            {
                cConn.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = cConn;
                cmd.CommandText = "Select Code,Name,Price1 from articles where Name like '" + name + "%'";

                MySqlDataReader reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    eArticle a = new eArticle();
                    a.Code = (string)reader.GetValue(0);
                    a.Name = (string)reader.GetValue(1);
                    a.Price1 = (double)reader.GetValue(2);
                    articleList.Add(a);
                }

                cmd.Dispose();
                cmd = null;

                reader = null;

                cConn.Close();
            }
            catch (Exception ex)
            {

                err = ex.Message;
            }
        }

        public static void SearchArticles3()
        {
            err = "";
            articleList = new List<eArticle>();

            try
            {
                using (var connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();

                    using (var command = new MySqlCommand("Select Code,Name,Price1 from articles ", connection))
                    using (var reader = command.ExecuteReader())
                        while (reader.Read())
                        {
                            eArticle a = new eArticle();
                            a.Code = (string)reader.GetValue(0);
                            a.Name = (string)reader.GetValue(1);
                            a.Price1 = (double)reader.GetValue(2);
                            articleList.Add(a);
                            //Console.WriteLine(reader.GetString(0));
                        }
                }
            }
            catch (Exception e)
            {
                err = e.Message;
            }

        }

        public static eTotal calculateTotal(List<eScanned> eScannedList)
        {
            eTotal total = new eTotal();
            total.TotalAmount = 0;
            total.TotalQuantity = 0;

            foreach (eScanned a in eScannedList)
            {
                total.TotalQuantity = total.TotalQuantity + a.Quantity;
                total.TotalAmount = total.TotalAmount + Math.Round(a.Total,2);
            }

            return total;
        }

        public static eTotalPayed calculateTotalPayed(List<ePayItem> payList)
        {
            eTotalPayed totalPayed = new eTotalPayed();

            foreach (ePayItem p in payList)
            {
                totalPayed.Total = totalPayed.Total + Math.Round(p.Val,2);
                if (p.Code == "C") totalPayed.Cash = totalPayed.Cash + p.Val;
                if (p.Code == "BC") totalPayed.Bancontact = totalPayed.Bancontact + p.Val;
                if (p.Code == "V") totalPayed.Voucher = totalPayed.Voucher + p.Val;
                if (p.Code == "CH") totalPayed.Cheque = totalPayed.Cheque + p.Val;
            }

            return totalPayed;
        }

        #region "PageCashReg"
        public static eScanned Prepare_eScanned(int id , string Quantity, string Price, string Discount, string Kind)
        {
            if (Quantity == "") Quantity = "0";
            if (Price == "") Price = "0";
            if (Discount == "") Discount = "0";

            eScanned a = new eScanned();
            if (id > 0) a.ID = id;
            a.ArtId = cData.selectedArticle.ArtId;
            a.Code = cData.selectedArticle.Code;
            a.Name = cData.selectedArticle.Name;
            a.Barcode = cData.selectedArticle.Barcode;
            a.Price = Convert.ToDouble(Price);
            a.Quantity = Convert.ToDouble(Quantity);
            a.Discount = Convert.ToDouble(Discount);
            a.Total = a.Quantity * a.Price;
            if (a.Discount > 0)
            {
                a.Total = Math.Round(a.Total * (1 - (a.Discount / 100)), 2, MidpointRounding.AwayFromZero);
            }
            a.Kind = Kind;

            a.StockLocation = cData.stockLocation; //1.0.4
            
            a.Reference = cData.settings.Ticket;
            
            a.CustId = cData.selectedCustomer.CustId; //v35
            return a;
        }

        public static void Insert_ScannedArticle(eScanned a)
        {
            cSQLite.SQLiteConn.Insert(a); //insert local database
        }

        public static void Update_ScannedArticle(eScanned a)
        {
            cSQLite.SQLiteConn.Update(a); //update local database
        }

        public static List<eScanned> Get_ScannedArticles(string pKind)
        {
            //SQLite.TableQuery<eScanned> eScannedDbList = cSQLite.SQLiteConn.Table<eScanned>();
            //int c = eScannedDbList.Count();
            //List<eScanned> eScannedList = new List<eScanned>();
            //foreach (eScanned e in eScannedDbList)
            //{
            //    eScanned n = new eScanned();
            //    n.ID = e.ID;
            //    n.ArtId = e.ArtId;
            //    n.Quantity = e.Quantity;
            //    n.Barcode = e.Barcode;
            //    n.Code = e.Code;
            //    n.Name = e.Name;
            //    n.Price = e.Price;
            //    n.Discount = e.Discount;
            //    n.Kind = e.Kind;
            //    n.Size1 = e.Size1;
            //    n.Size2 = e.Size2;
            //    n.Size3 = e.Size3;
            //    n.Size4 = e.Size4;
            //    eScannedList.Add(n);
            //    n = null;
            //}

            List<eScanned> eScannedList = cSQLite.SQLiteConn.Query<eScanned>("select * from eScanned where Kind=? ", pKind);
            return eScannedList;
        }

        public static void Set_Discount(string pKind,double pDiscount) //v1.0.3
        {
            List<eScanned> eScannedList = cSQLite.SQLiteConn.Query<eScanned>("select * from eScanned where Kind=? ", pKind);
            foreach(eScanned a in eScannedList)
            {
                a.Total = a.Quantity * a.Price;
                a.Discount = pDiscount;
                a.Total = Math.Round(a.Total * (1 - (a.Discount / 100)), 2, MidpointRounding.AwayFromZero);
                cSQLite.SQLiteConn.Update(a);
            }
        }

        public static void Set_Customer(string pKind, eCustomer pCustomer) //v1.0.3
        {
            List<eScanned> eScannedList = cSQLite.SQLiteConn.Query<eScanned>("select * from eScanned where Kind=? ", pKind);
            foreach (eScanned a in eScannedList)
            {
                cSQLite.selectArticlesArtId(Convert.ToInt32(a.ArtId)); //search article on id
                if (selectedArticle.ArtId == a.ArtId) //equals id
                {
                    Tuple<double, bool, bool> tupCustPrice = switch_CustomerPrice(pCustomer.Price, selectedArticle);
                    Double custPrice = tupCustPrice.Item1;
                    if (custPrice > 0)
                    {
                        a.CustId = pCustomer.CustId;
                        a.Price = custPrice;
                        a.Total = a.Quantity * a.Price;
                        if (a.Discount > 0)
                        {
                            a.Total = Math.Round(a.Total * (1 - (a.Discount / 100)), 2, MidpointRounding.AwayFromZero);
                        }
                        cSQLite.SQLiteConn.Update(a);
                    }
                }
            }
            cData.selectedArticle = null;
        }

        public static void Delete_ScannedArticle(eScanned a)
        {
            cSQLite.SQLiteConn.Delete<eScanned>(a.ID);
        }

        public static void Delete_ScannedArticles(string pKind)
        {
            cSQLite.SQLiteConn.ExecuteScalar<eScanned>("Delete from eScanned where Kind = ?", pKind);
        }
        #endregion

        public static void IncreaseTicket()
        {
            settings.Ticket = settings.Ticket + 1;
            cSQLite.saveSettings(settings);
            cSQLite.getSettings();
        }

        public static void setDefaultCustomerCode()
        {
            selectedCustomer = new eCustomer();
            selectedCustomer.Code = "";
            selectedCustomer.Name = "";
            selectedCustomer.Price = 1;
            selectedCustomer.Discount = 0;

            if (cData.settings.CustomerCode != "")
            {
                cData.customerList = cSQLite.SQLiteConn.Query<eCustomer>("select * from eCustomer where Code = ?", cData.settings.CustomerCode);
                if (cData.customerList.Count == 1)
                {
                    selectedCustomer = cData.customerList[0];
                }
            }
        }

        public static double getSelectedPrice(string strQuantity)
        {
            string strSelectedPrice = getSelectedPriceStr(strQuantity);
            double selectedPrice = Convert.ToDouble(strSelectedPrice);
            return selectedPrice;
        }

        public static int getSumQuantitySelectedArtikelById(int selectedArtikelID) 
        {
            int sumQuantity = 0;
            scannedSameArtikelsInKassa = cSQLite.SQLiteConn.Query<eScanned>("select * from eScanned where ArtId=? ", selectedArtikelID);

            foreach (eScanned eScannedItemKassa in scannedSameArtikelsInKassa)
            {
                sumQuantity += Convert.ToInt32(eScannedItemKassa.Quantity);
            }
            return sumQuantity;
        }


        public static string getSelectedPriceStr(string strQuantity)
        {
            string price = "0.00";
            double quantityInDocument = Convert.ToDouble(strQuantity);
            double iSumSameQuantityScannedArtikel = 0;
            if (selectedArticle != null)
            {
                price = selectedArticle.Price1.ToString("0.00");
                iSumSameQuantityScannedArtikel = getSumQuantitySelectedArtikelById(Convert.ToInt32(selectedArticle.ArtId));
                quantityInDocument += iSumSameQuantityScannedArtikel;
            }
            bool groothandel = false;
            if (_selectedCustomer != null)
            {
                Tuple<double, bool, bool> tupCustPrice = switch_CustomerPrice(_selectedCustomer.Price, selectedArticle);
                if (tupCustPrice.Item3 == true) //isApplied
                {
                    price = tupCustPrice.Item1.ToString("0.00"); //price
                    groothandel = true;
                }

            }
            if (selectedArticle != null)
            {
                DefPricePerc = 0; //1.0.0.4
                if (groothandel == false && selectedArticle.DefPriceCode1 != "")
                {
                    bool excl = false;
                    double dPrice = Convert.ToDouble(price);
                    switch_DefinePrice_Loop(selectedArticle.DefPriceCode1, quantityInDocument, ref dPrice, ref excl, selectedArticle, ref DefPricePerc); //1.0.0.4
                    price = dPrice.ToString("0.00");
                }
                if (groothandel == true && selectedArticle.DefPriceCode2 != "")
                {
                    bool excl = true;
                    double dPrice = Convert.ToDouble(price);
                    switch_DefinePrice_Loop(selectedArticle.DefPriceCode2, quantityInDocument, ref dPrice, ref excl, selectedArticle, ref DefPricePerc); //1.0.0.4
                    price = dPrice.ToString("0.00");
                }
            }


            switch_Staffel(quantityInDocument, ref price);
            updatePriceScannedArticlesInKassa(cData.scannedSameArtikelsInKassa, Convert.ToDouble(price));

            return price;
        }

        private static void updatePriceScannedArticlesInKassa(IList<eScanned> scannedArtikelsKassa, double price)
        {
            if (scannedArtikelsKassa.Count > 0)
            {
                foreach (eScanned a in scannedArtikelsKassa)
                {
                    a.Price = price;
                    a.Total = a.Quantity * a.Price;
                    if (a.Discount > 0)
                    {
                        a.Total = Math.Round(a.Total * (1 - (a.Discount / 100)), 2, MidpointRounding.AwayFromZero);
                    }
                    cSQLite.SQLiteConn.Update(a);
                }
            }
        }

        private static void switch_Staffel(double quantityInDocument, ref string price) //105 (voor Juiste Staffel prijs te nemen als er staffel prijzen zijn)
        {
            if (selectedArticle != null)
            {
                if (selectedArticle.QuantityPrice8 > 0 && quantityInDocument >= selectedArticle.QuantityPrice8 && selectedArticle.Price8 > 0)
                {
                    price = selectedArticle.Price8.ToString("0.00");
                }
                else if (selectedArticle.QuantityPrice7 > 0 && quantityInDocument >= selectedArticle.QuantityPrice7 && selectedArticle.Price7 > 0) 
                {
                    price = selectedArticle.Price7.ToString("0.00");
                }
                else if (selectedArticle.QuantityPrice6 > 0 && quantityInDocument >= selectedArticle.QuantityPrice6 && selectedArticle.Price6 > 0)
                {
                    price = selectedArticle.Price6.ToString("0.00");
                }
                else if (selectedArticle.QuantityPrice5 > 0 && quantityInDocument >= selectedArticle.QuantityPrice5 && selectedArticle.Price5 > 0)
                {
                    price = selectedArticle.Price5.ToString("0.00");
                }
                else if (selectedArticle.QuantityPrice4 > 0 && quantityInDocument >= selectedArticle.QuantityPrice4 && selectedArticle.Price4 > 0)
                {
                    price = selectedArticle.Price4.ToString("0.00");
                }
                else if (selectedArticle.QuantityPrice3 > 0 && quantityInDocument >= selectedArticle.QuantityPrice3 && selectedArticle.Price3 > 0)
                {
                    price = selectedArticle.Price3.ToString("0.00");
                }
                else if (selectedArticle.QuantityPrice2 > 0 && quantityInDocument >= selectedArticle.QuantityPrice2 && selectedArticle.Price2 > 0)
                {
                    price = selectedArticle.Price2.ToString("0.00");
                }
                else if (selectedArticle.QuantityPrice1 > 0 && quantityInDocument >= selectedArticle.QuantityPrice1 && selectedArticle.Price1 > 0)
                {
                    price = selectedArticle.Price1.ToString("0.00");
                }
            }
        }

        private static bool switch_DefinePrice_Loop(string pCode, double quantityInDocument, ref double currentPrice, ref bool excl, eArticle itemArt, ref double DefPricePerc) //v35 //1.0.0.4
        {
            bool isApplied = false;

            if (cData.defPricesList.Count > 0)
            {
                List<eDefPrices> lst = cData.defPricesList.Where(x => x.Code.Equals(pCode)).ToList();

                foreach (eDefPrices e in lst)
                {
                    if (quantityInDocument >= e.QFrom && quantityInDocument <= e.QTill)
                    {
                        if (e.DefKind == (int)enumDefPrices.Geen) // geen
                        {
                            isApplied = true;
                        }
                        if (e.DefKind == (int)enumDefPrices.Bedrag) //bedrag
                        {
                            currentPrice = currentPrice - e.DefValue;
                            isApplied = true;
                        }
                        if (e.DefKind == (int)enumDefPrices.Percentage) //percentage
                        {
                            DefPricePerc = e.DefValue; //1.0.0.4
                            //currentPrice = currentPrice - ((currentPrice * e.DefValue) / 100);
                            //currentPrice = Math.Round(currentPrice, 2);
                            isApplied = true;
                        }
                        if (e.DefKind == (int)enumDefPrices.Prijs) //prijs
                        {
                            int customerPrice = Convert.ToInt32(e.DefValue);
                            Tuple<double, bool, bool> tupCustPrice = switch_CustomerPrice(customerPrice, itemArt);
                            currentPrice = tupCustPrice.Item1;
                            excl = tupCustPrice.Item2;
                            isApplied = true;
                        }
                        break;
                    }
                }
            }
            return isApplied;
        }

        private static Tuple<double, bool, bool> switch_CustomerPrice(int pCustomerPrice, eArticle itemArt)
        {
            bool isApplied = false;
            double price = (double)itemArt.Price1;
            //bool excl = (cGeneral.AlgInst.Price1_Incl == 0 ? true : false);
            bool excl = false; //first price is inclusive

            switch (pCustomerPrice)
            {
                case 1:
                    price = (double)itemArt.Price1;
                    break;
                case 2:
                    price = (double)itemArt.Price2;
                    isApplied = true;
                    //excl = (cGeneral.AlgInst.Price2_Incl == 0 ? true : false);
                    break;
                case 3:
                    price = (double)itemArt.Price3;
                    isApplied = true;
                    //excl = (cGeneral.AlgInst.Price3_Incl == 0 ? true : false);
                    break;
                case 4:
                    price = (double)itemArt.Price4;
                    isApplied = true;
                    //excl = (cGeneral.AlgInst.Price4_Incl == 0 ? true : false);
                    break;
                case 5:
                    price = (double)itemArt.Price5;
                    isApplied = true;
                    //excl = (cGeneral.AlgInst.Price5_Incl == 0 ? true : false);
                    break;
                case 6:
                    price = (double)itemArt.Price6;
                    isApplied = true;
                    //excl = (cGeneral.AlgInst.Price6_Incl == 0 ? true : false);
                    break;
                case 7:
                    price = (double)itemArt.Price7;
                    //excl = (cGeneral.AlgInst.Price7_Incl == 0 ? true : false);
                    break;
                case 8:
                    price = (double)itemArt.Price8;
                    isApplied = true;
                    //excl = (cGeneral.AlgInst.Price8_Incl == 0 ? true : false);
                    break;
                default:
                    break;
            }

            Tuple<double, bool, bool> tup = new Tuple<double, bool, bool>(price, excl, isApplied);
            return tup;
        }

        //public static void Insert_StockMutation_Delete(eStock s, eArticle a)
        //{
        //    eStockMutation m = new eStockMutation();
        //    m.Kind = "DEL";
        //    m.StockId = s.StockId;
        //    m.ArtId = s.ArtId;
        //    m.Place = s.Place;
        //    m.Remark = s.Remark;
        //    m.ArtName = a.Name+" "+a.Size1;
        //    cSQLite.SQLiteConn.Insert(m); //insert local database
        //}

        public static void Insert_StockMutation(eStockMutation m)
        {
            cSQLite.SQLiteConn.Insert(m); //insert local database
        }

        public static void Delete_StockMutation(eStockMutation m)
        {
            cSQLite.SQLiteConn.Delete<eStockMutation>(m.Id);
        }

        public static void Delete_StockMutations()
        {
            cSQLite.SQLiteConn.ExecuteScalar<eStockMutation>("Delete from eStockMutation");
        }

        public static eStockMutation Modify_eStockMutation(eStockMutation m , string Quantity, string Place, string Remark)
        {
            //eStockMutation m = new eStockMutation();
            //if (id > 0)
            //{
            //    List<eStockMutation> lst = cSQLite.SQLiteConn.Query<eStockMutation>("select * from eStockMutation where Id = ? ", id);
            //    if (lst.Count == 1)
            //    {
            //        m = lst[0];
            //    }
            //}
            m.Quantity = Convert.ToDouble(Quantity);
            m.Place = Place;
            m.Remark = Remark;
            return m;
        }

        public static eStockMutation Prepare_eStockMutation(eStock s, string pKind, eArticle a)
        {
            eStockMutation m = new eStockMutation();
            m.Kind = pKind;
            if (pKind == "ADD")
            {
                m.StockId = 0;
                m.ArtId = a.ArtId;
                m.Quantity = 0;
                m.Place = "";
                m.Remark = "";
            }
            else
            {
                m.StockId = s.StockId;
                m.ArtId = s.ArtId;
                m.Quantity = s.Total;
                m.Place = s.Place;
                m.Remark = s.Remark;
            }
            
            m.ArtName = a.Name;
            if (a.Size1 != "") m.ArtName = m.ArtName + " " + a.Size1;
            if (a.Size2 != "") m.ArtName = m.ArtName + " " + a.Size2;
            if (a.Size3 != "") m.ArtName = m.ArtName + " " + a.Size3;
            if (a.Size4 != "") m.ArtName = m.ArtName + " " + a.Size4;
            
            return m;
        }

        public static void Update_StockMutation(eStockMutation m)
        {
            cSQLite.SQLiteConn.Update(m); //update local database
        }

        public static eArticle createArtNotFound(string pEntry)
        {
            eArticle newArt = new eArticle();
            newArt.ArtId = 0;
            newArt.Code = "";
            newArt.Barcode = pEntry;
            newArt.Name = pEntry;
            newArt.Price1 = 0;
            newArt.Price2 = 0;
            newArt.Price3 = 0;
            newArt.Price4 = 0;
            newArt.Price5 = 0;
            newArt.Price6 = 0;
            newArt.Price7 = 0;
            newArt.Price8 = 0;
            return newArt;
        }

    }
}

