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
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        Context context  = new Context();
        public Login()
        {
            InitializeComponent();
            //context.Users.Add(new User() {IsAdmin = true, Login = "admin", Name = "admin", Password = "1111"});
            //context.SaveChanges();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            
            this.Close();
        }

       
    }
}
