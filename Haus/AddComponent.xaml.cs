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
    /// Interaction logic for AddComponent.xaml
    /// </summary>
    public partial class AddComponent : Window
    {
        Context db = new Context();

        public AddComponent()
        {
            InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var component = new Component { Name = ComponentName.Text, UnitOfMeasurment = ComponentUnitOfMeasurment.Text };
            db.Components.Add(component);
            db.SaveChanges();
            MessageBox.Show("Товар додано");
            ComponentName.Text = "";
            ComponentUnitOfMeasurment.Text = "";
        }
    }
}
