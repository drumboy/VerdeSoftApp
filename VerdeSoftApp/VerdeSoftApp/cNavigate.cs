using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Schema;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VerdeSoftApp
{
    public static class cNavigate
    {
        public static List<Page> listPages;

        public static void openPage(Page p)
        {
            if (listPages == null) cNavigate.listPages = new List<Page>();
            App.Current.MainPage = p;
            listPages.Add(p);
        }

        public static void openPreviousPage()
        {
            if (listPages != null)
            {
                int c = listPages.Count;
                if (c > 1)
                {
                    listPages.RemoveAt(c - 1);
                    App.Current.MainPage = listPages[c - 2];
                }
            }
        }

        public static void openCurrentPage()
        {
            if (listPages != null)
            {
                int c = listPages.Count;
                if (c > 0)
                {
                    App.Current.MainPage = listPages[c - 1];
                }
            }
        }

        public static void openSpecificPage(string PageName)
        {
            if (listPages != null)
            {
                int c = listPages.Count;
                if (c > 1)
                {
                    for (int i = c - 1; i >= 0; i--)
                    {
                        if ( (listPages[i]).Title == PageName )
                        {
                            App.Current.MainPage = listPages[i];
                            break;
                        }
                        else
                        {
                            listPages.RemoveAt(i);
                        }
                    }
                }
            }
        }

    }

}
