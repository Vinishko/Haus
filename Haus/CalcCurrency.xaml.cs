using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Haus
{
    /// <summary>
    /// Currency calculator
    /// </summary>
    public partial class CalcCurrency : Window
    {
        public CalcCurrency()
        {
            InitializeComponent();
        }
        /// <summary>
        /// TODO: 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TB500_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(TB500.Text))
            {
                Label500.Content = int.Parse(TB500.Text) * 500;
                GetTotSum();
            }
            else
            {
                Label500.Content = 0;
                GetTotSum();
            }
           
        }

        private void TB200_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(TB200.Text))
            {
                Label200.Content = int.Parse(TB200.Text) * 200;
                GetTotSum();
            }
            else
            {
                Label200.Content = 0;
                GetTotSum();
            }
        }

        private void TB100_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(TB100.Text))
            {
                Label100.Content = int.Parse(TB100.Text) * 100;
                GetTotSum();
            }
            else
            {
                Label100.Content = 0;
                GetTotSum();
            }
        }

        private void TB50_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(TB50.Text))
            {
                Label50.Content = int.Parse(TB50.Text) * 50;
                GetTotSum();
            }
            else
            {
                Label50.Content = 0;
                GetTotSum();
            }
        }

        private void TB20_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(TB20.Text))
            {
                Label20.Content = int.Parse(TB20.Text) * 20;
                GetTotSum();
            }
            else
            {
                Label20.Content = 0;
                GetTotSum();
            }
        }

        private void TB10_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(TB10.Text))
            {
                Label10.Content = int.Parse(TB10.Text) * 10;
                GetTotSum();
            }
            else
            {
                Label10.Content = 0;
                GetTotSum();
            }
        }

        private void TB5_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(TB5.Text))
            {
                Label5.Content = int.Parse(TB5.Text) * 5;
                GetTotSum();
            }
            else
            {
                Label5.Content = 0;
                GetTotSum();
            }
        }

        private void TB2_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(TB2.Text))
            {
                Label2.Content = int.Parse(TB2.Text) * 2;
                GetTotSum();
            }
            else
            {
                Label2.Content = 0;
                GetTotSum();
            }
        }

        private void TB1_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(TB1.Text))
            {
                Label1.Content = int.Parse(TB1.Text) * 1;

            }
            else
            {
                Label1.Content = 0;
                GetTotSum();
            }
        }

        private void GetTotSum()
        {
            List<Label> v = new List<Label>{Label500,Label200,Label100,Label50,Label20,Label10,Label5,Label2,Label1};
            int total = 0;
            foreach (var label in v)
            {
                if (!String.IsNullOrEmpty(label.Content.ToString()))
                {
                    total += int.Parse(label.Content.ToString());
                }
            }
            Total.Content = total;
        }
    }
}
