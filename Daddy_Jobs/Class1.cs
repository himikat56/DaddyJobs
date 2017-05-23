using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daddy_Jobs
{
    class Class1
    {
        Main_Form form;
        string login;
        
     public Class1()
        {
            this.form = new Main_Form();
        }
        public String setLogin(string login)
        {
            this.form.setLogin(login);
            this.login = login;
            return this.login;
        }

        public void show()
        {
            this.form.Show();
        }
    }
}
