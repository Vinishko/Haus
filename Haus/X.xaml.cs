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
        class Report
        {
            public int id { get; set; }
            public string name { get; set; }
            public double countOfPaid { get; set; }
            public double countOfUnpaid { get; set; }
            public double totalSum { get; set; }
            public Report(int Id, string Name, double CountOfPaid)
            {
                id = Id;
                name = Name;
                countOfPaid = CountOfPaid;
                countOfUnpaid = 0;
                totalSum = 0;
            }
            public Report(int Id, string Name, double CountOfPaid, double CountOfUnpaid)
            {
                id = Id;
                name = Name;
                countOfPaid = CountOfPaid;
                countOfUnpaid = CountOfUnpaid;
                totalSum = 0;
            }
        }
        class UsedComponents
        {
            public string name { get; set; }
            public double count { get; set; }
            public UsedComponents()
            {
            }
            public UsedComponents( string Name, double Count)
            {
                name = Name;
                count = Count;
            }
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
                             amount = g.Sum(orderhasfoods=>orderhasfoods.Amount)
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
            var FoodPrices = from food in db.Foods
                             select new
                             {
                                 Name = food.Name,
                                 Price = food.Price
                             };
            var TotSum = from order in db.Orders
                         where (order.Time >= startDT && order.Time <= finishDT) && order.Status == Status.Closed
                         select order.OrderSum;
            var ReportList = new List<Report>();
            foreach (var item in result)
            {
                ReportList.Add(new Report(ReportList.Count + 1, item.Name, item.amount));
            }
            foreach (var item in result2)
            {
                var tmp = ReportList.Find(r => r.name == item.Name);
                if (tmp != null)
                {
                    tmp.countOfUnpaid = item.amount;
                }
                else
                {
                    ReportList.Add(new Report(ReportList.Count + 1, item.Name, 0, item.amount));
                }
            }
            //foreach (var item in FoodPrices)
            //{
            //    var tmp = ReportList.Find(r => r.name == item.Name);
            //    tmp.totalSum = item.Price * tmp.countOfPaid;
            //}
            var endtotsum = 0.0;
            foreach (var d in TotSum)
            {
                endtotsum += d;
            }
            var directory = Directory.GetCurrentDirectory();
            var newdir = directory + "\\" + DateTime.Now.ToString().Replace("/","").Replace(" ", "_").Replace(".", "").Replace(":", "") + ".txt";
            StreamWriter sw = new StreamWriter(newdir);
            sw.WriteLine("Звіт за період {0}  -  {1}",startDT.ToString(),finishDT.ToString());
            sw.WriteLine("Сформував звіт:   {0}",user.Name);
            sw.WriteLine("\n");
            //foreach (var r in result)
            //{
            //    totalcups += r.amount;
            //    sw.WriteLine("{0}   {1}    ",r.Name, r.amount);
            //}
            var nulls = 0.0;
            sw.WriteLine("-".PadRight(100, '-'));
            sw.WriteLine("|Назва товару		       | Кількість платних порцій  |  Кількість безкоштовних  | Загальна кількість  |");
            foreach (var item in ReportList)
            {
                totalcups += item.countOfPaid;
                nulls += item.countOfUnpaid;
                sw.WriteLine("-".PadRight(108, '-'));
                sw.WriteLine("|{0}|{1}|{2}|{3}|", item.name.PadRight(30,' '), item.countOfPaid.ToString().PadRight(27,' '), item.countOfUnpaid.ToString().PadRight(26, ' '), (item.countOfPaid+ item.countOfUnpaid).ToString().PadRight(21,' '));
            }
            sw.WriteLine("-".PadRight(100, '-'));
            sw.WriteLine("Загальна к-сть проданих стаканів {0}", totalcups);
            sw.WriteLine("Загальна к-сть безплатних стаканів {0}", nulls);

            //var nulls = 0.0;
            sw.WriteLine("\n");
            //sw.WriteLine("\n");
            //sw.WriteLine("Нулі");
            //foreach (var r in result2)
            //{
            //    nulls += r.amount;
            //    sw.WriteLine("{0}   x  {1}", r.Name, r.amount);
            //}
            //sw.WriteLine("Загальна к-сть нулів {0}", nulls);
            sw.WriteLine("\n");
            sw.WriteLine("Сума каси:  {0}" , endtotsum);
            sw.WriteLine("\n\n\n");
            var components = new List<UsedComponents>();
            foreach (var item in ReportList)
            {
                var componentsForThisFood = db.FoodHasComponents.Where(f => f.Food.Name == item.name).Select(f => new UsedComponents { name = f.Component.Name, count = f.Amount }).ToList();
                foreach (var component in componentsForThisFood)
                {
                    var tmp = components.Find(x => x.name == component.name);
                    if(tmp != null)
                    {
                        tmp.count += component.count;
                    }
                    else
                    {
                        components.Add(new UsedComponents(component.name, component.count));
                    }
                }
            }
            foreach (var item in ReportList)
            {

            }
            sw.WriteLine("-".PadRight(37, '-'));
            sw.WriteLine("|Назва складової        | Кількість |");
            foreach (var item in components)
            {
                sw.WriteLine("-".PadRight(37, '-'));
                sw.WriteLine("|{0}|{1}|",item.name.PadRight(23, ' '), item.count.ToString().PadRight(11,' '));
            }
            sw.WriteLine("-".PadRight(37, '-'));
            sw.Close();
            MessageBox.Show("Звіт збережено");
        }
    }
}
