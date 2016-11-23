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
    /// Логика взаимодействия для AddFoodType.xaml
    /// </summary>
    public partial class AddFoodType : Window
    {
        Context db = new Context();
        public AddFoodType()
        {
            InitializeComponent();
            var result = (from foodtype in db.FoodTypes orderby foodtype.Name select foodtype);
            ComboBox.ItemsSource = result.ToList();
        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBox.SelectedItem!=null)
            {
                db.FoodTypes.Remove(ComboBox.SelectedItem as FoodType);
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            db.FoodTypes.Add(new FoodType {Name = InpFoodType.Text});
            db.SaveChanges();
            InpFoodType.Text = String.Empty;
        }
    }
}
