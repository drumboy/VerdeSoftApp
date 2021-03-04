using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using PCLStorage;

namespace VerdeSoftApp
{
    public static class cSQLite
    {
        public static SQLiteConnection SQLiteConn = null;
        public static string sqliteFilename = "VerdeSoft.db3";
        public static string path = "";

        public static void connectSQLite()
        {
            IFolder folder = FileSystem.Current.LocalStorage;
            path = PortablePath.Combine(folder.Path.ToString(), sqliteFilename);
            SQLiteConn = new SQLiteConnection(path);
        }

        public static void verifySQLite()
        {
            if (System.IO.File.Exists(path) == false)
            {
                initDatabase(false);
            }
        }

        public static void insertSQLLite_eArticle(eArticle a)
        {
            SQLiteConn.Insert(a);
        }

        public static void getSQLLite()
        {
            eArticle art = SQLiteConn.Get<eArticle>(1); // primary key 
            //var artList = SQLiteConn.Table<Article>();
            SQLite.TableQuery<eArticle> artLIst2 = SQLiteConn.Table<eArticle>();
            int c = artLIst2.Count();
            Console.WriteLine("aantal records: " + c.ToString());

            cData.articleList = new List<eArticle>();
            foreach (eArticle a in artLIst2)
            {
                eArticle artnew = new eArticle();
                artnew.Code = a.Code;
                cData.articleList.Add(artnew);
                artnew = null;
                Console.WriteLine("Record: " + a.Code);
            }

            Console.WriteLine("Einde");

        }

        public static void deleteSQLLiteId()
        {
            long id = 0;
            var rowcount = SQLiteConn.Delete<eArticle>(id);
            SQLiteConn.DeleteAll<eArticle>();
        }

        public static void deleteSQLLiteAll()
        {
            SQLiteConn.DeleteAll<eArticle>();
        }

        public static void autoCreateTableSettings()
        {
            //bool exist = TableExists("eSettings", SQLiteConn);
            //if (exist == true)
            //{
            //    SQLiteConn.DropTable<eSettings>();
            //}
            SQLiteConn.CreateTable<eSettings>();
            SQLite.TableQuery<eSettings> eList = SQLiteConn.Table<eSettings>();
            if (eList.Count() == 0)
            {
                eSettings e = new eSettings();
                e.ConnString = "Userid=root;database=verdesoft;server=192.168.1.111;password=root;CharSet=utf8";
                e.MobileService = @"http://192.168.1.116:8000/MobileService";
                SQLiteConn.Insert(e);
            }

        }

        //public static eSettings getSettings()
        //{
        //    eSettings e = new eSettings();
        //    connectSQLite();

        //    SQLite.TableQuery<eSettings> eList = SQLiteConn.Table<eSettings>();
        //    if (eList.Count() > 0)
        //    {
        //        e = SQLiteConn.Get<eSettings>(1);
        //        cData.ConnectionString = e.ConnString;
        //        cWCF.MobileServiceStr = e.MobileService;
        //        cData.settings = e;
        //    }
        //    return e;
        //}

        public static eSettings getSettings()
        {
            eSettings e = new eSettings();
            e.MobileService = "";

            connectSQLite();

            bool exist = false;
            exist = TableExists("eSettings", SQLiteConn);
            if (exist == true)
            {
                List<eSettings> eList = SQLiteConn.Query<eSettings>("select * from eSettings");
                if (eList.Count > 0)
                {
                    try
                    {
                        e = eList[0];
                        cData.settings = e;
                        cData.ConnectionString = e.ConnString;
                        cWCF.MobileServiceStr = e.MobileService;
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            return e;
        }

        public static List<string> getUsers()
        {
            cData.userList = new List<eUser>();
            connectSQLite();

            List<string> l = new List<string>();

            bool exist = false;
            exist = TableExists("eUser", SQLiteConn);
            if (exist == true)
            {

                cData.userList = SQLiteConn.Query<eUser>("select * from eUser");
                if (cData.userList.Count > 0)
                {
                    foreach (eUser u in cData.userList)
                    {
                        l.Add(u.Code);
                    }
                }
            }
            return l;
        }

        public static void getDefPrices()
        {
            cData.defPricesList = new List<eDefPrices>();
            connectSQLite();

            List<string> l = new List<string>();

            bool exist = false;
            exist = TableExists("eDefPrices", SQLiteConn);
            if (exist == true)
            {
                cData.defPricesList = SQLiteConn.Query<eDefPrices>("select * from eDefPrices");
            }
        }

        public static void saveSettings(eSettings e)
        {
            SQLiteConn.Update(e);
        }

        public static void dropTableSettings()
        {
            SQLiteConn.DropTable<eSettings>();
        }

        public static Boolean TableExists(String tableName, SQLiteConnection connection)
        {
            SQLite.TableMapping map = new TableMapping(typeof(System.Data.SqlDbType)); // Instead of mapping to a specific table just map the whole database type
            object[] ps = new object[0]; // An empty parameters object since I never worked out how to use it properly! (At least I'm honest)

            Int32 tableCount = connection.Query(map, "SELECT * FROM sqlite_master WHERE type = 'table' AND name = '" + tableName + "'", ps).Count; // Executes the query from which we can count the results
            if (tableCount == 0)
            {
                return false;
            }
            else if (tableCount == 1)
            {
                return true;
            }
            else
            {
                throw new Exception("More than one table by the name of " + tableName + " exists in the database.", null);
            }

        }

        public static void initDatabase(bool connectDB)
        {
            if (connectDB == true) connectSQLite();

            bool exist = false;

            exist = TableExists("eArticle", SQLiteConn);
            if (exist == true) SQLiteConn.DropTable<eArticle>();
            SQLiteConn.CreateTable<eArticle>();

            exist = TableExists("eSettings", SQLiteConn);
            if (exist == true) SQLiteConn.DropTable<eSettings>();
            SQLiteConn.CreateTable<eSettings>();

            exist = TableExists("eScanned", SQLiteConn);
            if (exist == true) SQLiteConn.DropTable<eScanned>();
            SQLiteConn.CreateTable<eScanned>();

            exist = TableExists("eCustomer", SQLiteConn);
            if (exist == true) SQLiteConn.DropTable<eCustomer>();
            SQLiteConn.CreateTable<eCustomer>();

            exist = TableExists("eBarcodes", SQLiteConn); //1.0.4
            if (exist == true) SQLiteConn.DropTable<eBarcodes>(); //1.0.4
            SQLiteConn.CreateTable<eBarcodes>(); //1.0.4

            exist = TableExists("eStock", SQLiteConn); //1.0.4 a
            if (exist == true) SQLiteConn.DropTable<eStock>(); //1.0.4 a
            SQLiteConn.CreateTable<eStock>(); //1.0.4 a

            eSettings e = new eSettings();
            e.ConnString = "Userid=root;database=verdesoft;server=192.168.1.111;password=root;CharSet=utf8";
            e.MobileService = @"http://192.168.1.116:8000/MobileService";
            e.RepeatQuantity = false;
            e.VerifyBarcode = true; //lengte barcode
            e.Ticket = 1;
            e.CustomerCode = "";
            e.Language = "N";
            e.SearchField = "";
            e.KeybAlpha = false;
            e.PaymentScreen = false;
            e.CustomerCard = "";
            
            SQLiteConn.Insert(e);
        }

        public static void beforeSyncArticles()
        {
            bool exist = false;

            exist = TableExists("eArticle", SQLiteConn);
            if (exist == true) SQLiteConn.DropTable<eArticle>();
            SQLiteConn.CreateTable<eArticle>();
        }

        public static void syncArticles()
        {
            SQLiteConn.BeginTransaction();
            foreach (eArticle a in cData.articleList)
            {
                SQLiteConn.Insert(a);
            }
            SQLiteConn.Commit();
        }

        public static void syncCustomers()
        {
            SQLiteConn.BeginTransaction();
            foreach (eCustomer c in cData.customerList)
            {
                SQLiteConn.Insert(c);
            }
            SQLiteConn.Commit();
        }

        public static void beforeSyncUsers()
        {
            bool exist = false;

            exist = TableExists("eUser", SQLiteConn);
            if (exist == true) SQLiteConn.DropTable<eUser>();
            SQLiteConn.CreateTable<eUser>();
        }

        public static void beforeSyncDefPrices()
        {
            bool exist = false;

            exist = TableExists("eDefPrices", SQLiteConn);
            if (exist == true) SQLiteConn.DropTable<eDefPrices>();
            SQLiteConn.CreateTable<eDefPrices>();
        }

        public static void beforeSyncStock()
        {
            bool exist = false;

            exist = TableExists("eStock", SQLiteConn);
            if (exist == true) 
                SQLiteConn.DropTable<eStock>();
            SQLiteConn.CreateTable<eStock>();

            exist = TableExists("eStockMutation", SQLiteConn);
            if (exist == true) 
                SQLiteConn.DropTable<eStockMutation>();
            SQLiteConn.CreateTable<eStockMutation>();
        }

        public static void syncUsers()
        {
            SQLiteConn.BeginTransaction();
            foreach (eUser u in cData.userList)
            {
                SQLiteConn.Insert(u);
            }
            SQLiteConn.Commit();
        }

        public static void syncDefPrices()
        {
            SQLiteConn.BeginTransaction();
            foreach (eDefPrices dp in cData.defPricesList)
            {
                SQLiteConn.Insert(dp);
            }
            SQLiteConn.Commit();
        }

        public static void syncStock()
        {
            SQLiteConn.BeginTransaction();
            foreach (eStock s in cData.stockList)
            {
                SQLiteConn.Insert(s);
            }
            SQLiteConn.Commit();
        }

        public static void syncBarcodes() //1.0.4
        {
            SQLiteConn.BeginTransaction();
            foreach (eBarcodes s in cData.barcodesList)
            {
                SQLiteConn.Insert(s);
            }
            SQLiteConn.Commit();
        }

        public static void beforeSyncCustomers()
        {
            bool exist = false;

            exist = TableExists("eCustomer", SQLiteConn);
            if (exist == true) SQLiteConn.DropTable<eCustomer>();
            SQLiteConn.CreateTable<eCustomer>();
        }

        public static void beforeSyncBarcodes() //1.0.4
        {
            bool exist = false;

            exist = TableExists("eBarcodes", SQLiteConn);
            if (exist == true) SQLiteConn.DropTable<eBarcodes>();
            SQLiteConn.CreateTable<eBarcodes>();
        }

        public static void selectArticlesBarcodeOrCodeName(string pValue)
        {
            if (pValue != "")
            {
                //if (cTools.isNumeric(pValue, System.Globalization.NumberStyles.Number, false) == true && pValue.Length >= 1 && pValue.Length <= 2)
                //{
                //    cData.articleList = SQLiteConn.Query<eArticle>("select * from eArticle where Code = ? ", pValue);
                //}
                //Zoeken op Code of Naam als lengte < 7
                if (pValue.Length >= 1 && pValue.Length < 7) //105
                {
                    if ((!String.IsNullOrEmpty(cData.settings.SearchField)) && (cData.settings.SearchField.Equals("Name")))
                    {
                        cData.articleList = SQLiteConn.Query<eArticle>("select * from eArticle where Name like ? ", "%" + pValue + "%");
                    }
                    else if ((!String.IsNullOrEmpty(cData.settings.SearchField)) && (cData.settings.SearchField.Equals("Code"))) 
                    {
                        if (cTools.isNumeric(pValue, System.Globalization.NumberStyles.Number, false) == true)
                        {
                            cData.articleList = SQLiteConn.Query<eArticle>("select * from eArticle where Code = ? ", pValue);
                        }
                        else 
                        {
                            cData.articleList = SQLiteConn.Query<eArticle>("select * from eArticle where Code like ? ", "%" + pValue + "%");
                        }
                    }
                }

                //Zoeken op barcode als lengte value > 7 => zoeken op barcode !! //105
                else
                {
                    cData.articleList = SQLiteConn.Query<eArticle>("select * from eArticle where Barcode = ? ", pValue);
                    if (cData.articleList.Count == 0)
                    {
                        cData.barcodesList = SQLiteConn.Query<eBarcodes>("Select * from eBarcodes where Barcode = ? ", pValue);
                        if (cData.barcodesList.Count > 0)
                        {
                            eBarcodes b = cData.barcodesList[0];
                            cData.articleList = SQLiteConn.Query<eArticle>("select * from eArticle where ArtId = ? ", b.ArtId);
                        }
                    }
                }
            }
            else
            {
                cData.articleList = new List<eArticle>();
            }

        }

        public static void selectArticlesField(string pField, string pValue, enumOperator pOperator)
        {
            if (pValue != null)
            {
                string searchNoSpaces = pValue.Replace(" ", "%");
                if (pOperator == enumOperator.Equals)
                {
                    cData.articleList = SQLiteConn.Query<eArticle>("select * from eArticle where " + pField + " = ? ", searchNoSpaces);
                }
                else if (pOperator == enumOperator.Like)
                {
                    cData.articleList = SQLiteConn.Query<eArticle>("select * from eArticle where " + pField + " like ? ", searchNoSpaces + "%");
                }
                else if (pOperator == enumOperator.Contains)
                {
                    cData.articleList = SQLiteConn.Query<eArticle>("select * from eArticle where " + pField + " like ? ", "%" + searchNoSpaces + "%");
                }
            }
            else
            {
                cData.articleList = new List<eArticle>();
            }
        }

        public static void selectArticlesArtId(int pId)
        {
            cData.articleList = SQLiteConn.Query<eArticle>("select * from eArticle where ArtId = ? ", pId);
            if (cData.articleList.Count == 1)
            {
                cData.selectedArticle = cData.articleList[0];
            }
        }

        public static void selectCustomersField(string pField, string pValue, enumOperator pOperator)
        {
            if (pValue != null)
            {
                string searchNoSpaces = pValue; //pValue.Replace(" ", "");
                if (pOperator == enumOperator.Equals)
                {
                    cData.customerList = SQLiteConn.Query<eCustomer>("select * from eCustomer where " + pField + " = ? ", searchNoSpaces);
                }
                else if (pOperator == enumOperator.Like)
                {
                    cData.customerList = SQLiteConn.Query<eCustomer>("select * from eCustomer where " + pField + " like ? ", searchNoSpaces + "%");
                }
                else if (pOperator == enumOperator.Contains)
                {
                    cData.customerList = SQLiteConn.Query<eCustomer>("select * from eCustomer where " + pField + " like ? ", "%" + searchNoSpaces + "%");
                }
            }
            else
            {
                cData.customerList = new List<eCustomer>();
            }
        }

        public static void selectStockArtId(long pArtId)
        {
            if (pArtId > 0)
            {
                cData.stockList = SQLiteConn.Query<eStock>("select * from eStock where ArtId = ? ", pArtId);
            }
            else
            {
                cData.stockList = new List<eStock>();
            }
        }

        public static List<eStockMutation> selectStockMutations()
        {
            List<eStockMutation> mutList = SQLiteConn.Query<eStockMutation>("select * from eStockMutation");
            return mutList;
        }

        public static void deleteAllStockMutations()
        {
            SQLiteConn.DeleteAll<eStockMutation>();
        }
    }
}
