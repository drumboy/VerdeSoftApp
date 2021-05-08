using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace VerdeSoftApp
{
    public class NumericEntry : Entry
    {
        public NumericEntry()
        {
            this.Keyboard = Keyboard.Numeric;
        }
    }
}
