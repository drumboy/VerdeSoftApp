using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Java.Security;

namespace VerdeSoftApp.Droid
{
    [Activity(Label = "VerdeSoftApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            //==================================== //HIDE BOTTOM BAR
            //int uiOptions = (int)Window.DecorView.SystemUiVisibility;

            //uiOptions |= (int)SystemUiFlags.LowProfile;
            //uiOptions |= (int)SystemUiFlags.Fullscreen;
            //uiOptions |= (int)SystemUiFlags.HideNavigation;
            //uiOptions |= (int)SystemUiFlags.ImmersiveSticky;

            //Window.DecorView.SystemUiVisibility = (StatusBarVisibility)uiOptions;
            //====================================//HIDE BOTTOM BAR
            

            //var decorView = Window.DecorView;
            //decorView.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.HideNavigation;

            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override bool DispatchKeyEvent(KeyEvent e)
        {
            // if the Keycode is A .. there's lots of these
            //you'll have to checkout the Keycode class for more options
            //this example will fire the following code when 'A' is pressed
            if (e.KeyCode == Keycode.NumpadDot)
            {
                //invoke your method here
                //return base.DispatchKeyEvent(new KeyEvent(KeyEventActions.Down, Keycode.NumpadComma));
            }

            //return base if needed, otherwise keystrokes won't get automatically passed into textboxes etc and stuff will act wacky
            return base.DispatchKeyEvent(e);
        }
               

        //DISABLE BACKBUTTON
        //public override void OnBackPressed()
        //{
        //    //var disable = (bool)App.Current.Properties["isBackButtonDisabled"];
        //    //if (disable) return;
        //    base.OnBackPressed();
        //}

        //public override void OnWindowFocusChanged(bool hasFocus)
        //{
        //    int uiOptions = (int)Window.DecorView.SystemUiVisibility; uiOptions |= (int)SystemUiFlags.HideNavigation; uiOptions |= (int)SystemUiFlags.ImmersiveSticky; Window.DecorView.SystemUiVisibility = (StatusBarVisibility)uiOptions;
        //    base.OnWindowFocusChanged(hasFocus);
        //}


    }
}