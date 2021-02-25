using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VerdeSoftApp
{
    public partial class App : Application
    {
        //public static NavigationPage NavPage; //zelf gezet
        
        public App()
        {

            Device.SetFlags(new string[]
            {   "RadioButton_Experimental",
                "AppTheme_Experimental",
                "Markup_Experimental",
                "Expander_Experimental"
            });

            if (cNavigate.listPages == null)
            {
                InitializeComponent();

                //App.Current.MainPage = new NavigationPage(new MainPage());
                
                MainPage = new MainPage();
                cNavigate.openPage(MainPage);
            }
            else
            {
               cNavigate.openCurrentPage();
            }
            
            
        }

        protected override void OnStart()
        {
            //NavPage = new NavigationPage(new MainPage()); //aangepast
            //MainPage = new MainPage(); //aangepast
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
