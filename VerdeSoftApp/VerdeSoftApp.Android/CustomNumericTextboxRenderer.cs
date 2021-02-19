using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

//https://stackoverflow.com/questions/46782149/how-do-i-set-the-keyboard-as-a-numeric-keypad-and-can-i-enter-a-negative-number/46793994

[assembly: ExportRenderer(typeof(VerdeSoftApp.NumericEntry), typeof(VerdeSoftApp.Droid.CustomNumericTextboxRenderer))]
namespace VerdeSoftApp.Droid
{
    // nodig voor . te gebruiken ipv komma
    public class CustomNumericTextboxRenderer : EntryRenderer
    {
        public CustomNumericTextboxRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            var native = Control as EditText;

            native.InputType = Android.Text.InputTypes.ClassNumber | Android.Text.InputTypes.NumberFlagSigned | Android.Text.InputTypes.NumberFlagDecimal;
        }
    }
}