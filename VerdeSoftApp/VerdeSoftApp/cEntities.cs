using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VerdeSoftApp
{
    public class eArticle
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public double Quantity { get; set; }
        public long ArtId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public string Size1 { get; set; }
        public string Size2 { get; set; }
        public string Size3 { get; set; }
        public string Size4 { get; set; }
        public double Price1 { get; set; }
        public double Price2 { get; set; }
        public double Price3 { get; set; }
        public double Price4 { get; set; }
        public double Price5 { get; set; }
        public double Price6 { get; set; }
        public double Price7 { get; set; }
        public double Price8 { get; set; }
        public string DefPriceCode1 { get; set; }
        public string DefPriceCode2 { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }

    public class eCustomer
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public long CustId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double Discount { get; set; }
        public int Price { get; set; }
        public string Barcode { get; set; }
    }

    public class eUser
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class eSettings
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string ConnString { get; set; }
        public string MobileService { get; set; }
        public string Identification { get; set; }
        public string CustomerCode { get; set; }
        public bool FocusPrice { get; set; }
        public bool Payments { get; set; }
        public long Ticket { get; set; }
        public string Language { get; set; }
        public bool KeybAlpha { get; set; }
        public string SearchField { get; set; }


    }

    public class eScanned
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Kind { get; set; }
        public long Reference { get; set; }
        public double Quantity { get; set; }
        public long ArtId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public string Size1 { get; set; }
        public string Size2 { get; set; }
        public string Size3 { get; set; }
        public string Size4 { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public double Total { get; set; }
        public long CustId{ get; set; }
        public string StockLocation { get; set; } //1.0.4
        public override string ToString()
        {
            return Name;
        }
    }

    public class ePayItem
    {
        public string Code { get; set; }
        public double Val { get; set; }
    }

    public class eTotal
    {
        public double TotalAmount { get; set; }
        public double TotalQuantity { get; set; }
    }

    public class eTotalPayed
    {
        public double Total { get; set; }
        public double Cash { get; set; }
        public double Bancontact { get; set; }
        public double Voucher { get; set; }
        public double Cheque { get; set; }

        public eTotalPayed()
        {
            Total = 0;
            Cash = 0;
            Bancontact = 0;
            Voucher = 0;
            Cheque = 0;
        }
    }

    public class eTranslate
    {
        public string Code { get; set; }
        public string NL { get; set; }
        public string FR { get; set; }
        public string EN { get; set; }
        public string DE { get; set; }
    }

    public class eDefPrices
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Code { get; set; }
        public double QFrom { get; set; }
        public double QTill { get; set; }
        public Int32 DefKind { get; set; }
        public double DefValue { get; set; }
    }

    public class eStock
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        public long StockId { get; set; }
        public long ArtId { get; set; }
        public string Place { get; set; }
        public double Total { get; set; }
        public double Quantity { get; set; }
        public SByte Unavailable { get; set; }
        public string Shop { get; set; }
        public DateTime CreationDate { get; set; }
        public string Remark { get; set; }
        public Int32 Priority { get; set; }
        public string PPOrigin { get; set; }
        public SByte Purchased { get; set; }
        public double CostPrice { get; set; }
        public string SupplierName { get; set; }
    }


    public class eStockMutation
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        public string Kind { get; set; }
        public string MutKind { get; set; }
        public long StockId { get; set; }
        public long ArtId { get; set; }
        public string Place { get; set; }
        public double Quantity { get; set; }
        public string Remark { get; set; }
        public string ArtName { get; set; } //ter info
    }

    public class eBarcodes //1.0.4
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        public long ArtId { get; set; }
        public string Barcode { get; set; }
    }

    public enum enumDefPrices
    {
        Geen = 0,
        Prijs = 1,
        Percentage = 2,
        Bedrag = 3
    }

    public enum enumOperator
    {
        Equals = 0,
        Like = 1,
        Contains = 2
    }

}
