using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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
    /// Логика взаимодействия для ChangeOrderStatus.xaml
    /// </summary>
    public partial class ChangeOrderStatus : Window
    {
        Context db;
        public ChangeOrderStatus(Context context)
        {
            db = context;
            InitializeComponent();

        }

        private void Calcel_OnClick(object sender, RoutedEventArgs e)
        {
            int number;
            if (!String.IsNullOrEmpty(CardIdInput.Text)&&int.TryParse(CardIdInput.Text,out number))
            {
                var selected = db.Orders.FirstOrDefault(x => x.OrderId == number);
                if (selected != null)
                {
                    selected.Status = Status.Canceled;
                    db.Orders.Attach(selected);
                    db.SaveChanges();
                }
                else
                {
                    MessageBox.Show("Номер чеку невірний");
                }
                
            }
            else
            {
                MessageBox.Show("Введене значення недійсне");
            }
        }
    }
}
