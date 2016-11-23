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
    /// Логика взаимодействия для AddUser.xaml
    /// </summary>
    public partial class AddUser : Window
    {
        Context context;
        public AddUser(Context db)
        {
            context = db;
            InitializeComponent();
        }

        private void Add_OnClick(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(NameTB.Text)&& !String.IsNullOrEmpty(Login.Text)&& !String.IsNullOrEmpty(Password.Text))
            {
                context.Users.Add(new User(){Login = Login.Text, Name = NameTB.Text,Password = Password.Text,IsAdmin = IsAdmin.IsChecked.Value});
                context.SaveChanges();
            }
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            if (Users.SelectedItem!=null)
            {
                var removed = Users.SelectedItem as User;
                context.Users.Attach(removed);
                context.Users.Remove(removed);
                context.SaveChanges();
            }
        }
    }
}
