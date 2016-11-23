using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Haus
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    /// 

    
    public partial class App : Application
    {
        Context context = new Context();
        public App(): base()
        {
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            bool authenticated = false;
            Login login;
            User user=null;
            while (!authenticated)
            {
                login = new Login();
                login.ShowDialog();
                
                authenticated = ValidUser(login.LoginTB.Text, login.PassTB.Password, out user);
                if (!authenticated)
                {
                    MessageBox.Show("Логін/пароль невірний");
                }
                
            }

            MainWindow main = new MainWindow(user,context);
            main.Show();
        }

        

        private bool ValidUser(string p1, string p2, out User outUser)
        {
            var user =
               (from u in context.Users where (u.Login == p1.Trim() && u.Password == p2.Trim()) select u).ToList();

            if (user.Count == 1) {outUser = user[0]; return true;}
            outUser = null; return false;
        }
    }
}
