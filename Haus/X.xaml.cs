using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для X.xaml
    /// </summary>
    public partial class X : Window
    {
        private Context db;
        private User user;
        public X(Context context,User inUser)
        {
            db = context;
            user = inUser;
            InitializeComponent();
        }

        private void GetX_OnClick(object sender, RoutedEventArgs e)
        {
            DateTime start = (DateTime) From.SelectedDate;

            var st = DateTime.Parse(TimeFrom.Text);
            var st2 = start.AddHours(st.Hour);
            var fst = st2.AddMinutes(st.Minute);
            var finishD = (DateTime)To.SelectedDate;
            var f = DateTime.Parse(TimeTo.Text);
            var f1 = finishD.AddHours(f.Hour);
            var f2 = f1.AddMinutes(f.Minute);
            XReport(fst,f2);
            this.Close();
        }
        private void XReport(DateTime startDT, DateTime finishDT)
        {
            var totalcups = 0.0;
            var result = from food in db.Foods
                         join orderhasfoods in db.OrderHasFoods on food.FoodId equals orderhasfoods.FoodId
                         join orders in db.Orders on orderhasfoods.OrderId equals orders.OrderId
                         join discounts in db.Discounts on orders.Discount equals discounts
                         where (orders.Time >= startDT && orders.Time <= finishDT) && orders.Status == Status.Closed
                         group orderhasfoods by food.Name into g
                         select new
                         {
                             Name = g.Key,
                             amount = g.Sum(orderhasfoods=>orderhasfoods.Amount),
                         };

            var result2 = from food in db.Foods
                         join orderhasfoods in db.OrderHasFoods on food.FoodId equals orderhasfoods.FoodId
                         join orders in db.Orders on orderhasfoods.OrderId equals orders.OrderId
                         join discounts in db.Discounts on orders.Discount equals discounts
                         where (orders.Time >= startDT && orders.Time <= finishDT) && orders.Status == Status.Closed && orders.OrderSum==0
                         group orderhasfoods by food.Name into g
                         select new
                         {
                             Name = g.Key,
                             amount = g.Sum(orderhasfoods => orderhasfoods.Amount),
                         };
            var TotSum = from order in db.Orders
                         where (order.Time >= startDT && order.Time <= finishDT) && order.Status == Status.Closed
                         select order.OrderSum;
            var endtotsum = 0.0;
            foreach (var d in TotSum)
            {
                endtotsum += d;
            }
            var directory = Directory.GetCurrentDirectory();
            var newdir = directory + "\\" + DateTime.Now.ToString().Replace("/","").Replace(" ", "_").Replace(".", "").Replace(":", "") + ".txt";
            StreamWriter sw = new StreamWriter(newdir);
            sw.WriteLine("Звіт за період {0}  -  {1}",startDT.ToString(),finishDT.ToString());
            sw.WriteLine("Бариста:   {0}",user.Name);
            sw.WriteLine("\n");
            foreach (var r in result)
            {
                totalcups += r.amount;
                sw.WriteLine("{0}   {1}     {2}",r.Name, r.amount);
            }
            sw.WriteLine("Загальна к-сть проданих стаканів {0}", totalcups);
            var nulls = 0.0;
            sw.WriteLine("\n");
            sw.WriteLine("\n");
            sw.WriteLine("Нулі");
            foreach (var r in result2)
            {
                nulls += r.amount;
                sw.WriteLine("{0}   x  {1}", r.Name, r.amount);
            }
            sw.WriteLine("Загальна к-сть нулів {0}", nulls);
            sw.WriteLine("\n");
            sw.WriteLine("Сума каси:  {0}" , endtotsum);
            sw.Close();
            MessageBox.Show("Звіт збережено");
        }
    }
}
