using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;

namespace VerdeSoftApp
{
    public static class cWCF
    {
        public static string MobileServiceStr = ""; //@"http://192.168.1.116:8000/MobileService";
        public static MobileServiceRef.MobileServiceClient client = null;

        public static string SearchArticles_WCF(string pIdentification, string pField, string pValue, bool pContains)
        {
            getMobileServiceStr();

            string err = "";
            cData.articleList = new List<eArticle>();
            MobileServiceRef.cArticleItem[] l = null;
            try
            {
                if (client == null)
                {
                    client = new MobileServiceRef.MobileServiceClient(MobileServiceRef.MobileServiceClient.EndpointConfiguration.BasicHttpBinding_IMobileService, MobileServiceStr);
                }
                l = client.Article_Search(cData.settings.Identification, pField, pValue, pContains);
            }
            catch (Exception ex)
            {
                err = ex.Message;
                if (ex.InnerException != null)
                {
                    err = ex.InnerException.ToString();
                }
            }
            if (err == "")
            {
                foreach (MobileServiceRef.cArticleItem i in l)
                {
                    eArticle a = new eArticle();
                    a.ArtId = i.Id;
                    a.Code = i.Code;
                    a.Name = i.Name;
                    a.Barcode = i.Barcode;
                    a.Size1 = i.Size1;
                    a.Size2 = i.Size2;
                    a.Size3 = i.Size3;
                    a.Size4 = i.Size4;
                    a.Price1 = i.Price1;
                    a.Price2 = i.Price2;
                    cData.articleList.Add(a);
                }
            }
            return err;
        }

        public static string InsertScannedArticles_WCF(string pIdentification, IList<eScanned> scannedList)
        {
            getMobileServiceStr();

            string err = "";
            if (scannedList != null)
            {
                MobileServiceRef.cScannedArticlesItem[] l = new MobileServiceRef.cScannedArticlesItem[scannedList.Count];
                int t = 0;
                foreach (eScanned e in scannedList)
                {
                    MobileServiceRef.cScannedArticlesItem a = new MobileServiceRef.cScannedArticlesItem();
                    a.ArtId = e.ArtId;
                    a.Code = e.Code;
                    a.Name = e.Name;
                    a.Barcode = e.Barcode;
                    a.Size1 = e.Size1;
                    a.Size2 = e.Size2;
                    a.Size3 = e.Size3;
                    a.Size4 = e.Size4;
                    a.Discount = e.Discount;
                    a.Price = e.Price;
                    a.Quantity = e.Quantity;
                    a.Kind = e.Kind;
                    a.Reference = e.Reference;
                    a.CustId = e.CustId; //v35
                    a.StockLocation = e.StockLocation; //1.0.4
                    l[t] = a;
                    t++;
                }

                try
                {
                    MobileServiceRef.MobileServiceClient client = new MobileServiceRef.MobileServiceClient(MobileServiceRef.MobileServiceClient.EndpointConfiguration.BasicHttpBinding_IMobileService, MobileServiceStr);
                    bool ok = client.ScannedArticles_Add(pIdentification, l);
                }
                catch (Exception ex)
                {
                    err = ex.Message;
                    if (ex.InnerException != null)
                    {
                        err = ex.InnerException.ToString();
                    }
                }
                if (err == "")
                {

                }

            }

            return err;
        }

        public static string InsertScannedArticles_CR_WCF(string pIdentification, IList<eScanned> scannedList, List<ePayItem> payList)
        {
            getMobileServiceStr();

            string err = "";
            if (scannedList != null)
            {
                MobileServiceRef.cScannedArticlesItem[] l = new MobileServiceRef.cScannedArticlesItem[scannedList.Count];
                int t = 0;

                string content = "";

                foreach (eScanned e in scannedList)
                {
                    MobileServiceRef.cScannedArticlesItem a = new MobileServiceRef.cScannedArticlesItem();
                    a.ArtId = e.ArtId;
                    a.Code = e.Code;
                    a.Name = e.Name;
                    a.Barcode = e.Barcode;
                    a.Size1 = e.Size1;
                    a.Size2 = e.Size2;
                    a.Size3 = e.Size3;
                    a.Size4 = e.Size4;
                    a.Discount = e.Discount;
                    a.Price = e.Price;
                    a.Quantity = e.Quantity;
                    a.Kind = e.Kind;
                    a.Reference = e.Reference;
                    a.CustId = e.CustId; //v35

                    //content = content + e.ArtId.ToString();
                    //content = content + ";"+e.Code;
                    //content = content + ";" + e.Name;
                    //content = content + ";" + e.Barcode;
                    //content = content + ";" + e.Size1;
                    //content = content + ";" + e.Size2;
                    //content = content + ";" + e.Size3;
                    //content = content + ";" + e.Size4;
                    //content = content + ";" + e.Discount.ToString();
                    //content = content + ";" + e.Price.ToString();
                    //content = content + ";" + e.Quantity.ToString();
                    //content = content + ";" + e.Kind.ToString();
                    //content = content + ";" + e.Reference.ToString();
                    //content = content + ";" + e.CustId.ToString();
                    //content = content + Environment.NewLine;

                    l[t] = a;
                    t++;
                }

                string xml = cTools.SerializeObject(l);

                t = 0;
                MobileServiceRef.cPayItem[] l2 = new MobileServiceRef.cPayItem[payList.Count];
                foreach (ePayItem e in payList)
                {
                    MobileServiceRef.cPayItem p = new MobileServiceRef.cPayItem();
                    p.Code = e.Code;
                    p.Val = e.Val;
                    l2[t] = p;
                    t++;
                }

                try
                {
                    MobileServiceRef.MobileServiceClient client = new MobileServiceRef.MobileServiceClient(MobileServiceRef.MobileServiceClient.EndpointConfiguration.BasicHttpBinding_IMobileService, MobileServiceStr);
                    //((BasicHttpBinding)client.Endpoint.Binding).MaxReceivedMessageSize = int.MaxValue;
                    //((BasicHttpBinding)client.Endpoint.Binding).ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas();
                    //((BasicHttpBinding)client.Endpoint.Binding).ReaderQuotas.MaxArrayLength = int.MaxValue;
                    //((BasicHttpBinding)client.Endpoint.Binding).MaxBufferSize = int.MaxValue;
                    bool ok = client.ScannedArticles_CR_Add(pIdentification, l, l2,cData.userCode);
                    //bool ok = client.ScannedArticles_CR_String_Add(pIdentification, xml, l2, cData.userCode);
                }
                catch (Exception ex)
                {
                    err = ex.Message;
                    if (ex.InnerException != null)
                    {
                        err = ex.InnerException.ToString();
                    }
                }
                if (err == "")
                {

                }

            }

            return err;
        }


        public static string StockMutations_WCF(string pIdentification)
        {
            getMobileServiceStr();

            string err = "";

            List<eStockMutation> mutList = cSQLite.selectStockMutations();

            if (mutList.Count > 0)
            {
                MobileServiceRef.cStockMutationItem[] l = new MobileServiceRef.cStockMutationItem[mutList.Count];

                int t = 0;
                foreach (eStockMutation e in mutList)
                {
                    MobileServiceRef.cStockMutationItem m = new MobileServiceRef.cStockMutationItem();
                    m.Kind = e.Kind;
                    m.StockId = e.StockId;
                    m.ArtId = e.ArtId;
                    m.Place = e.Place;
                    m.Quantity = e.Quantity;
                    m.Remark = e.Remark;
                    m.MutKind = e.MutKind; //

                    l[t] = m;
                    t++;
                }

                try
                {
                    MobileServiceRef.MobileServiceClient client = new MobileServiceRef.MobileServiceClient(MobileServiceRef.MobileServiceClient.EndpointConfiguration.BasicHttpBinding_IMobileService, MobileServiceStr);
                    bool ok = client.StockMutations_Process(pIdentification, l);
                }
                catch (Exception ex)
                {
                    err = ex.Message;
                    if (ex.InnerException != null)
                    {
                        err = ex.InnerException.ToString();
                    }
                }
                if (err == "")
                {

                }

            }

            return err;
        }


        public static string GetArticles_WCF(string pIdentification, int per, int totalReceived)
        {
            getMobileServiceStr();

            string err = "";
            cData.articleList = new List<eArticle>();
            MobileServiceRef.cArticleItem[] l = null;
            try
            {
                if (client == null)
                {
                    client = new MobileServiceRef.MobileServiceClient(MobileServiceRef.MobileServiceClient.EndpointConfiguration.BasicHttpBinding_IMobileService, MobileServiceStr);
                }
                l = client.Articles_Get(pIdentification, per, totalReceived);  
            }
            catch (Exception ex)
            {
                err = ex.Message;
                if (ex.InnerException != null)
                {
                    err = ex.InnerException.ToString();
                }
            }
            if (err == "")
            {
                foreach (MobileServiceRef.cArticleItem i in l)
                {
                    eArticle a = new eArticle();
                    a.ArtId = i.Id;
                    a.Code = i.Code;
                    a.Name = i.Name;
                    a.Barcode = i.Barcode;
                    a.Size1 = i.Size1;
                    a.Size2 = i.Size2;
                    a.Size3 = i.Size3;
                    a.Size4 = i.Size4;
                    a.Price1 = i.Price1;
                    a.Price2 = i.Price2;
                    a.Price3 = i.Price3;
                    a.Price4 = i.Price4;
                    a.Price5 = i.Price5;
                    a.Price6 = i.Price6;
                    a.Price7 = i.Price7;
                    a.Price8 = i.Price8;
                    a.DefPriceCode1 = i.DefPriceCode1;
                    a.DefPriceCode2 = i.DefPriceCode2;
                    if (i.Size1 != "") a.Name = a.Name + " " + i.Size1;
                    if (i.Size2 != "") a.Name = a.Name + " " + i.Size2;
                    cData.articleList.Add(a);
                }
            }
            return err;
        }

        public static string GetCustomers_WCF(string pIdentification, int per, int totalReceived)
        {
            getMobileServiceStr();

            string err = "";
            cData.customerList = new List<eCustomer>();
            MobileServiceRef.cCustomerItem[] l = null;
            try
            {
                if (client == null)
                {
                    client = new MobileServiceRef.MobileServiceClient(MobileServiceRef.MobileServiceClient.EndpointConfiguration.BasicHttpBinding_IMobileService, MobileServiceStr);
                }
                l = client.Customers_Get(pIdentification, per, totalReceived);
            }
            catch (Exception ex)
            {
                err = ex.Message;
                if (ex.InnerException != null)
                {
                    err = ex.InnerException.ToString();
                }
            }
            if (err == "")
            {
                foreach (MobileServiceRef.cCustomerItem i in l)
                {
                    eCustomer a = new eCustomer();
                    a.ID = Convert.ToInt32(i.Id);
                    a.CustId = i.Id; //v35
                    a.Code = i.Code;
                    a.Name = i.Name;
                    if (i.Place != "") a.Code = a.Code + " (" + i.Place + ")";
                    a.Discount = i.Discount;
                    a.Price = i.CustomerPrice;
                    a.Barcode = i.Barcode;

                    cData.customerList.Add(a);
                }
            }
            return err;
        }

        public static string GetUsers_WCF(string pIdentification)
        {
            getMobileServiceStr();

            string err = "";
            cData.userList = new List<eUser>();
            MobileServiceRef.cUserItem[] l = null;
            try
            {
                if (client == null)
                {
                    client = new MobileServiceRef.MobileServiceClient(MobileServiceRef.MobileServiceClient.EndpointConfiguration.BasicHttpBinding_IMobileService, MobileServiceStr);
                }
                l = client.Get_Users(pIdentification, "");
            }
            catch (Exception ex)
            {
                err = ex.Message;
                if (ex.InnerException != null)
                {
                    err = ex.InnerException.ToString();
                }
            }
            if (err == "")
            {
                foreach (MobileServiceRef.cUserItem i in l)
                {
                    eUser a = new eUser();
                    a.ID = Convert.ToInt32(i.Id);
                    a.Code = i.Code;
                    a.Name = i.Name;
                    cData.userList.Add(a);
                }
            }
            return err;
        }

        public static string GetDefPrices_WCF(string pIdentification)
        {
            getMobileServiceStr();

            string err = "";
            cData.defPricesList = new List<eDefPrices>();
            MobileServiceRef.cDefPriceItem[] l = null;
            try
            {
                if (client == null)
                {
                    client = new MobileServiceRef.MobileServiceClient(MobileServiceRef.MobileServiceClient.EndpointConfiguration.BasicHttpBinding_IMobileService, MobileServiceStr);
                }
                l = client.DefPrices_Get(pIdentification);
            }
            catch (Exception ex)
            {
                err = ex.Message;
                if (ex.InnerException != null)
                {
                    err = ex.InnerException.ToString();
                }
            }
            if (err == "")
            {
                foreach (MobileServiceRef.cDefPriceItem i in l)
                {
                    eDefPrices a = new eDefPrices();
                    a.ID = Convert.ToInt32(i.Id);
                    a.Code = i.Code;
                    a.DefKind = i.DefKind;
                    a.DefValue = i.DefValue;
                    a.QFrom = i.QFrom;
                    a.QTill = i.QTill;
                    cData.defPricesList.Add(a);
                }
            }
            return err;
        }

        public static string GetStock_WCF(string pIdentification)
        {
            getMobileServiceStr();

            string err = "";
            cData.stockList = new List<eStock>();
            MobileServiceRef.cStockItem[] l = null;
            try
            {
                if (client == null)
                {
                    client = new MobileServiceRef.MobileServiceClient(MobileServiceRef.MobileServiceClient.EndpointConfiguration.BasicHttpBinding_IMobileService, MobileServiceStr);
                }
                l = client.Stock_Get(pIdentification);
            }
            catch (Exception ex)
            {
                err = ex.Message;
                if (ex.InnerException != null)
                {
                    err = ex.InnerException.ToString();
                }
            }
            if (err == "")
            {
                foreach (MobileServiceRef.cStockItem i in l)
                {
                    eStock s = new eStock();
                    s.Id = i.Id;
                    s.StockId = i.Id;
                    s.ArtId = i.ArtId;
                    s.Place = i.Place;
                    s.Total = i.Total;
                    s.Remark = i.Remark;
                    cData.stockList.Add(s);
                }
            }
            return err;
        }

        public static string GetBarcodes_WCF(string pIdentification) //1.0.4
        {
            getMobileServiceStr();

            string err = "";
            cData.barcodesList = new List<eBarcodes>();
            MobileServiceRef.cBarcodesItem[] l = null;
            try
            {
                if (client == null)
                {
                    client = new MobileServiceRef.MobileServiceClient(MobileServiceRef.MobileServiceClient.EndpointConfiguration.BasicHttpBinding_IMobileService, MobileServiceStr);
                }
                l = client.Barcodes_Get(pIdentification);
            }
            catch (Exception ex)
            {
                err = ex.Message;
                if (ex.InnerException != null)
                {
                    err = ex.InnerException.ToString();
                }
            }
            if (err == "")
            {
                foreach (MobileServiceRef.cBarcodesItem i in l)
                {
                    eBarcodes s = new eBarcodes();
                    s.Id = i.Id;
                    s.ArtId = i.ArtId;
                    s.Barcode = i.Barcode;
                    cData.barcodesList.Add(s);
                }
            }
            return err;
        }

        private static void getMobileServiceStr()
        {
            if (MobileServiceStr == "")
            {
                eSettings e = cSQLite.getSettings();
                MobileServiceStr = e.MobileService;
            }
        }
    }
}
