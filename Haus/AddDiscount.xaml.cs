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
    /// Логика взаимодействия для AddDiscount.xaml
    /// </summary>
    public partial class AddDiscount : Window
    {
        Context db = new Context();
        public AddDiscount()
        {
            InitializeComponent();

            DeleteDiscountCB.ItemsSource = db.Discounts.ToList();
            DiscountCardGrid.DataContext = db.DiscountCards;

            DiscountCardGrid.ItemsSource = db.DiscountCards.ToList();
        }
        
        private void AddDicountButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(NameOfDiscountTB.Text) && !String.IsNullOrEmpty(ValueTB.Text) && !String.IsNullOrEmpty(PercentageTB.Text) && !String.IsNullOrEmpty(SumFromTB.Text) && !String.IsNullOrEmpty(SummToTB.Text))
            {
                int discount;
                double val;
                int sumfrom;
                int sumto;
                if (int.TryParse(PercentageTB.Text,out discount)&& int.TryParse(SumFromTB.Text,out sumfrom)&& int.TryParse(SummToTB.Text,out sumto)&& double.TryParse(ValueTB.Text,out val))
                {
                    var discout = new Discount() { Name = NameOfDiscountTB.Text , Percent = discount, SumFrom = sumfrom, SumTo = sumto, Value = val};
                    db.Discounts.Add(discout);
                    db.SaveChanges();
                }
            }
        }

        private void DeleteDiscountButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (DeleteDiscountCB.SelectedValue!=null)
            {
                var removed = DeleteDiscountCB.SelectedItem as Discount;
                db.Discounts.Attach(removed);
                db.Discounts.Remove(removed);
                db.SaveChanges();
            }
        }

        private void DiscountCardGrid_OnRowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.Row.IsNewItem)
            {
                db.DiscountCards.Add(e.Row.Item as DiscountCard);
                db.SaveChanges();
            }
            else
            {
                db.DiscountCards.AddOrUpdate(e.Row.Item as DiscountCard);
                db.SaveChanges();
            }
            
        }

        private void DiscountCardGrid_OnCurrentCellChanged(object sender, EventArgs e)
        {
            var item = DiscountCardGrid.SelectedItem as DiscountCard;
            if (item!=null)
            {
                db.DiscountCards.AddOrUpdate(item);
            db.SaveChanges();
            }
            
        }
    }
}
