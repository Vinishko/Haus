using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.TextFormatting;
using System.Xml.Serialization;
using Haus.Annotations;
using FontFamily = System.Windows.Media.FontFamily;

namespace Haus
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public User thisUser;
        public Context db;
        List<OrderForUser> orderForUsers = new List<OrderForUser>();
        public Discount DiscountForm;
        public MainWindow(User user,Context dContext)
        {
            
            thisUser = user;
            db = dContext;
            InitializeComponent();
            GetCategories();
            Order.ItemsSource = orderForUsers;
            if (user.IsAdmin==true)
            {
                AdminToolbar.IsEnabled = true;
            }
            UserName.Content = "Користувач " + user.Name;
            Discount.ItemsSource = db.Discounts.ToList();
            //XmlSerializer xml = new XmlSerializer(typeof(Discount));
            //FileStream fs = new FileStream("1.xml", FileMode.Open, FileAccess.Read);

            //xml.Serialize(fs, thisUser);
            //fs.Flush();
            //fs.Close();
        }


        #region rightbar

        /// <summary>
        /// Method to fill right bar
        /// </summary>
        public void GetCategories()
        {
            var allcat = db.FoodTypes.ToList();
            foreach (var foodType in allcat)
            {

                var tabItem = new TabItem() { Header = foodType.Name, FontSize = 18, FontFamily = new FontFamily("Buxton Sketch") };
                var dataGrid = new DataGrid() { CanUserResizeRows = false, CanUserDeleteRows = false, CanUserResizeColumns = false,CanUserAddRows = false,FontSize = 18, Name = foodType.Name.Replace(" ",String.Empty) + "Right", Height = 622, VerticalAlignment = VerticalAlignment.Top, Width = 358, AutoGenerateColumns = false, FontFamily = new FontFamily("Buxton Sketch") };
                dataGrid.Columns.Add(new DataGridTextColumn(){Header="Назва", Width=331, Binding= new Binding("Name") , IsReadOnly=true});
                Style rowStyle = new Style(typeof(DataGridRow));
                rowStyle.Setters.Add(new EventSetter(DataGridRow.MouseDoubleClickEvent,
                                         new MouseButtonEventHandler(Row_DoubleClick)));
                dataGrid.RowStyle = rowStyle;
                dataGrid.ItemsSource = (from food in db.Foods 
                                        where food.FoodTypeId==foodType.FoodTypeId
                                        orderby food.Name select food).ToList();
                tabItem.Content = dataGrid;
                RightBar.Items.Add(tabItem);
            }
        }
        
        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
           DataGridRow row = sender as DataGridRow;
            var food = row.Item as Food;
            orderForUsers.Add(new OrderForUser(food));
            Order.Items.Refresh();
            DetTotPrice();
        }
#endregion
        private void DetTotPrice() 
        {

            double totcount = 0;
            foreach (var orderForUser in orderForUsers)
            {
                totcount += orderForUser.TotalPrice1;
            }
            if (DiscountForm!=null)
            {
                totcount *=DiscountForm.Value;
            }
            TotalPrice.Content = totcount;
        }

        
       


        private void CloseOrder_Click(object sender, RoutedEventArgs e)
        {
            
            if (orderForUsers.Count != 0)
            {

                CheckForZerosInOrder();
                CheckForMultInOrder();
                DetTotPrice();
                
                
                Order.Items.Refresh();
                var cc = new CloseChek(double.Parse(TotalPrice.Content.ToString()));
                cc.ShowDialog();
                if (cc.DialogResult == true)
                {
                    Order order;
                    if (DiscountForm!=null)
                    {
                         order = new Order
                        {
                            Status = Status.Closed,
                            Time = DateTime.Now,

                            Discount = DiscountForm,
                            OrderSum = double.Parse(TotalPrice.Content.ToString()),
                            User = thisUser
                        };
                    }
                    else
                    {
                        order = new Order
                        {
                            Status = Status.Closed,
                            Time = DateTime.Now,
                            OrderSum = double.Parse(TotalPrice.Content.ToString()),
                            Discount = db.Discounts.FirstOrDefault(d=> d.Percent==0),

                            User = thisUser
                        };
                    }
                    db.Orders.Add(order);
                    db.SaveChanges();

                    foreach (var item in Order.Items)
                    {
                        db.OrderHasFoods.Add(new OrderHasFood
                        {
                            Amount = (item as OrderForUser).count,
                            Order = order,
                            Food = (item as OrderForUser).food
                        });
                        db.SaveChanges();
                    }
                    //Print();
                    orderForUsers.Clear();
                    Order.Items.Refresh();
                    TotalSumClient.Content = 0;
                    DiscountForm = null;
                    DiscountClient.Content = String.Empty;
                    FIOClient.Content = String.Empty;
                    CardNumber.Text=String.Empty;
                    TotalSumClient.Content = String.Empty;
                    
                }

            }
            else
            {
                MessageBox.Show("Неможливо закрити чек без товару");
            }
        }


        private void CheckForZerosInOrder()
        {
            for (int i = 0; i < orderForUsers.Count; i++)
            {
                if (orderForUsers[i].count==0)
                {
                    orderForUsers.RemoveAt(i);
                    CheckForZerosInOrder();
                }
            }
        }

        private void CheckForMultInOrder()
        {
            if (orderForUsers.Count>2)
            {
                for (var i = 0; i < orderForUsers.Count - 1; i++)
                {
                    for (var j = i + 1; j < orderForUsers.Count; j++)
                    {
                        if (orderForUsers[i].food == orderForUsers[j].food)
                        {
                            orderForUsers[i].count += orderForUsers[j].count;
                            orderForUsers.RemoveAt(j);
                            CheckForMultInOrder();
                        }
                    }
                }
            }
            else
            {
                if (orderForUsers.Count!=1)
                {
                    if (orderForUsers[0].food != orderForUsers[1].food) return;
                    orderForUsers[0].count += orderForUsers[1].count;
                    orderForUsers.RemoveAt(1);
                }
                
            }
            
        }

        public class OrderForUser : INotifyPropertyChanged
        {
           
            public int count {get; set; }
            
            public double TotalPrice;
            public double price { get; set; }
            public string Name { get; set; }
            public double TotalPrice1
            {
                get { return food.Price*count; }
                set { TotalPrice = value; }
            }
            public Food food;

            public OrderForUser(Food foo)
            {
                count = 1;
                Name = foo.Name;
                price = foo.Price;
                food = foo;
            }

            public event PropertyChangedEventHandler PropertyChanged;

            [NotifyPropertyChangedInvocator]
            protected virtual void OnPropertyChanged1([CallerMemberName] string propertyName = "price")
            {
                var handler = PropertyChanged;
                if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void Order_OnCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            DetTotPrice(); 
        }
        #region Main DataGrid Service
        private void Order_OnCurrentCellChanged(object sender, EventArgs e)
        {
            Order.Items.Refresh();
        }
        private void Order_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void CancelItem_Click(object sender, RoutedEventArgs e)
        {
            if (Order.SelectedItem != null)
            {
                orderForUsers.Remove(Order.SelectedItem as OrderForUser);
                Order.Items.Refresh();
                DetTotPrice();
            }
        }

#endregion
        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            App.Current.Shutdown();
        }
        #region MenuButtons
        private void DiscountButton_OnClick(object sender, RoutedEventArgs e)
        {
            AddDiscount discount = new AddDiscount();
            discount.ShowDialog();
        }

        private void ReportStatusDown_OnClick(object sender, RoutedEventArgs e)
        {
            Haus.ChangeOrderStatus orderStatus = new ChangeOrderStatus(db);
            orderStatus.ShowDialog();
        }

        private void UsersButton_OnClick(object sender, RoutedEventArgs e)
        {
            var addUser = new AddUser(db);
            addUser.ShowDialog();
        }

        private void FoodButton_OnClick(object sender, RoutedEventArgs e)
        {
            AddFood af = new AddFood(db);
            af.ShowDialog();
        }

        private void FoodTypeButton_OnClick(object sender, RoutedEventArgs e)
        {
            AddFoodType aft = new AddFoodType();
            aft.ShowDialog();
        }
#endregion
        private void CheckCardButton_OnClick(object sender, RoutedEventArgs e)
        {
            int cardNumber;
            if (int.TryParse(CardNumber.Text.Trim(),out cardNumber))
            {
                var card = db.DiscountCards.FirstOrDefault(x => x.DiscountCardId == cardNumber);
                if (card!=null)
                {
                    //MessageBox.Show("AllGood");
                    FIOClient.Content = card.HolderName;
                    TotalSumClient.Content = card.TotSum;
                    
                    DiscountForm = db.Discounts.FirstOrDefault(x => x.SumFrom <= card.TotSum && x.SumTo > card.TotSum);
                    if (DiscountForm != null) DiscountClient.Content = DiscountForm.Percent;
                }
                else
                {
                    MessageBox.Show("Картка не дійсна");
                }
            }
            else
            {
                MessageBox.Show("Невірно введений номер картки");
                CardNumber.Text = String.Empty;
            }
        }

        private void Currencycalc_OnClick(object sender, RoutedEventArgs e)
        {
            CalcCurrency cc = new CalcCurrency();
            cc.ShowDialog();
        }

        private void CardToUser_OnClick(object sender, RoutedEventArgs e)
        {
            var rc = new RegisterCard(db);
            rc.ShowDialog();
        }

        private void PrintX_OnClick(object sender, RoutedEventArgs e)
        {
            var x = new X(db, thisUser);
            x.ShowDialog();
        }
        public void Print()
        {
            var doc = new PrintDocument();
            doc.PrintPage += new PrintPageEventHandler(ProvideContent);
            doc.Print();
        }

        public void ProvideContent(object sender, PrintPageEventArgs e)
        {

            Graphics graphics = e.Graphics;
            Font font = new Font("Courier New", 10);

            float fontHeight = font.GetHeight();

            int startX = 0;
            int startY = 0;
            int Offset = 20;

            //e.PageSettings.PaperSize.Kind = PaperKind.Custom;
            //e.PageSettings.PaperSize.Width = 56;

            graphics.DrawString("Кав'ярня 'BezБренду'", new Font("Courier New", 8),
                                new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
            Offset = Offset + 20;
            
            foreach (OrderForUser item in Order.Items)
            {
                graphics.DrawString(item.count + "x",
                     new Font("Courier New", 8),
                     new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);

                Offset = Offset + 10;
                graphics.DrawString(item.Name + "    "+ item.TotalPrice1,
                     new Font("Courier New", 8),
                     new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);

                Offset = Offset + 10;
            }
            var id = db.Orders.ToList();
            graphics.DrawString("Чек номер: " + id[id.Count - 1].OrderId,
                     new Font("Courier New", 8),
                     new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
            Offset = Offset + 10;
            String underLine = "------------------------------------------";

            graphics.DrawString(underLine, new Font("Courier New", 8),
                     new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);

            Offset = Offset + 10;
            graphics.DrawString("Сума: " + TotalSumClient.Content,
                      new Font("Courier New", 10),
                      new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
            Offset = Offset + 10;
            graphics.DrawString(underLine, new Font("Courier New", 8),
                     new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);

            Offset = Offset + 10;
            graphics.DrawString("Дата :" + DateTime.Now,
                     new Font("Courier New", 8),
                     new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);

            Offset = Offset + 20;
            graphics.DrawString("",
                     new Font("Courier New", 8),
                     new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
        }

        private void DiscountForce_OnClick(object sender, RoutedEventArgs e)
        {
            if (Discount.SelectedItem!=null)
            {
                DiscountForm = Discount.SelectedItem as Discount;
            }
            DetTotPrice();
        }

        private void ComponentButton_Click(object sender, RoutedEventArgs e)
        {
             var cw = new AddComponent();
            cw.ShowDialog();
        }
    }
}
