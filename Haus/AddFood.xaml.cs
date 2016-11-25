using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.CompilerServices;
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
using Haus.Annotations;

namespace Haus
{
    /// <summary>
    /// Логика взаимодействия для AddFood.xaml
    /// </summary>
    public partial class AddFood : Window
    {
        private Context db;

        List<ComponentsForFood> ComponentsForFoods = new List<ComponentsForFood>(); 
        public AddFood(Context context)
        {
            db = context;
            InitializeComponent();
            GetFoodType();
            GetComponents();
            Components.ItemsSource = ComponentsForFoods;
        }

        private void GetComponents()
        {
            var result = (from component in db.Components orderby component.Name select component);
            InputComponents.ItemsSource = result.ToList();
        }

        public class ComponentsForFood : INotifyPropertyChanged
        {

            public int count { get; set; }
            public string Name { get; set; }

            public Component Component;

            public ComponentsForFood(Component component)
            {
                count = 1;
                Name = component.Name;
                Component = component;
            }

            public event PropertyChangedEventHandler PropertyChanged;

            [NotifyPropertyChangedInvocator]
            protected virtual void OnPropertyChanged1([CallerMemberName] string propertyName = "count")
            {
                var handler = PropertyChanged;
                if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public void GetFoodType()
        {
            
                var result = (from foodtype in db.FoodTypes orderby foodtype.Name select  foodtype);
                
                //(from food in db.Foods orderby food.Name select food).ToList();
                InputFoodType.ItemsSource = result.ToList();

                DeleteFoodCB.ItemsSource = db.Foods.ToList();
            UpdateFoodTypeCB.ItemsSource = result.ToList();

            UpdateFoodCB.ItemsSource = db.Foods.ToList();
            //InputFoodType.DataContext = db;


        }

        private void AddFoodButt_Click(object sender, RoutedEventArgs e)
        {
            double price;
            if (double.TryParse(InputPrice.Text,out price))
            {
                
                AddFoodWork(InputName.Text.Trim(),price,InputFoodType.SelectedItem as FoodType);
                InputName.Text=String.Empty;
                InputPrice.Text=String.Empty;
            }
            else
            {
                MessageBox.Show("Ціна вказана невірно");
            }
            
        }

        public void AddFoodWork(string name, double price, FoodType foodType)
        {
            var food = new Food {Name = name, Price = price, FoodType = foodType};
            db.Foods.Add(food);
            db.SaveChanges();
            foreach (var item in Components.Items)
            {
                db.FoodHasComponents.Add(new FoodHasComponent
                {
                    Component = (item as ComponentsForFood).Component,
                    Amount = (item as ComponentsForFood).count,
                    Food = food
                });
                db.SaveChanges();
            }
            MessageBox.Show("Товар додано");
            ComponentsForFoods.Clear();
            Components.Items.Refresh();

        }

        private void DeleteFood_Click(object sender, RoutedEventArgs e)
        {
            
                var removed = DeleteFoodCB.SelectedItem as Food;
                db.Foods.Attach(removed);
                db.Foods.Remove(removed);
                db.SaveChanges();
                MessageBox.Show("Товар видалений");
            
        }

        private void UpdateFood_OnClick(object sender, RoutedEventArgs e)
        {
            var obj = (UpdateFoodCB.SelectedItem as Food);
            var result = db.Foods.SingleOrDefault(x=> x.FoodId==obj.FoodId);
            if (result != null)
            {
                result.Name = nameTB.Text;
                result.Price = Double.Parse(priceTB.Text);
                result.FoodType = UpdateFoodTypeCB.SelectedItem as FoodType;
                db.SaveChanges();
            }
            MessageBox.Show("Дані оновлено");
        }

        private void UpdateFoodCB_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UpdateFoodCB.SelectedItem!=null)
            {
                nameTB.Text = (UpdateFoodCB.SelectedItem as Food).Name;
                priceTB.Text = (UpdateFoodCB.SelectedItem as Food).Price.ToString();
                UpdateFoodTypeCB.SelectedItem = (UpdateFoodCB.SelectedItem as Food).FoodType;
            }
            
        }

        private void InputComponents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (InputComponents.SelectedIndex > -1)
            {
                var item = sender as ComboBox;
                var component = item.Items[InputComponents.SelectedIndex] as Component;
                ComponentsForFoods.Add(new ComponentsForFood(component));
                Components.Items.Refresh();
            }
        }
    }
}
