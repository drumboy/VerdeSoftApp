using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Linq;

namespace VerdeSoftApp
{
    public class NumericValidationBehavior : Behavior<Entry>
    {

        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        private static void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            if (!string.IsNullOrWhiteSpace(args.NewTextValue))
            {
                bool isValid = args.NewTextValue.ToCharArray().All(x => char.IsDigit(x)); //Make sure all characters are numbers

                string newTextValue = args.NewTextValue.Replace('.',',');
                isValid = true;
                foreach(char c in newTextValue.ToCharArray() )
                {
                    if (c == ',' || char.IsDigit(c) || c == '-' )
                    {
                    }
                    else
                    {
                        isValid = false;
                    }
                }

                ((Entry)sender).Text = isValid ? newTextValue : args.OldTextValue;
            }
        }
    }
}
