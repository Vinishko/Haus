using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
    /// Логика взаимодействия для CloseChek.xaml
    /// </summary>
    public partial class CloseChek : Window
    {
        
        public CloseChek(double frmM)
        {
            

            InitializeComponent();
            TotCount.Text = frmM.ToString();
            Close.IsDefault = true;
        }

        private void Cash_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            double cashFromUser;
            if (!String.IsNullOrEmpty(Cash.Text)&&double.TryParse(Cash.Text,out cashFromUser))
            {
                double temp =cashFromUser- double.Parse(TotCount.Text) ;
                Div.Text = (temp).ToString();
                if (temp>=0)
                {
                    Close.IsEnabled = true;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window window = GetWindow((DependencyObject) sender);
            if (window != null)
            {
                this.DialogResult = true;
                window.Close();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Window window = GetWindow((DependencyObject)sender);
            if (window != null)
            {
                this.DialogResult = false;
                window.Close();
            }
        }
    }
}
