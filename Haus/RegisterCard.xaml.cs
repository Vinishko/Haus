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
    /// Логика взаимодействия для RegisterCard.xaml
    /// </summary>
    public partial class RegisterCard : Window
    {
        public Context context;
        public RegisterCard(Context dbContext)
        {
            context = dbContext;
            InitializeComponent();
        }

        private void Register_OnClick(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(NumberTB.Text)&& !String.IsNullOrEmpty(ClientNameTB.Text))
            {
                int number;
                if (int.TryParse(NumberTB.Text,out number))
                {
                    int sum;
                    if (!String.IsNullOrEmpty(StartupSumTB.Text)&&int.TryParse(StartupSumTB.Text,out sum))
                    {
                        context.DiscountCards.Add(new DiscountCard()
                        {
                            DiscountCardId = number,
                            HolderName = ClientNameTB.Text,
                            TotSum = sum
                        });
                        context.SaveChanges();
                    }
                    else
                    {
                        
                        context.DiscountCards.Add(new DiscountCard()
                        {
                            DiscountCardId = number,
                            HolderName = ClientNameTB.Text,
                            TotSum = 0
                        });
                        context.SaveChanges();
                        MessageBox.Show("Помилка введення суми, картка створена з 0 балансом");
                    }
                    
                }
                else
                {
                    MessageBox.Show("Перевірте номер картки");
                }
            }
        }
    }
}
