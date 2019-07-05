using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sanitex_Uzduotis
{
    public partial class Login : Form
    {
        public static int userId;
        public Login()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txt_Username.Text == "" || txt_Password.Text == "")
            {
                MessageBox.Show("Please enter username and password.");
                return;
            }
            using (var db = new TimetableContext())
            {
                var contains = db.users.Where(u => u.username == txt_Username.Text).FirstOrDefault();
                if (contains == null)
                {
                    MessageBox.Show("No user with this name");
                }
                else
                {
                    if (contains.password == txt_Password.Text)
                    {
                        MessageBox.Show("Logged in successfully.");
                        this.Hide();
                        if (contains.userType == 1)
                        {
                            this.Hide();
                            userId = contains.userId;
                            ManagerHome home = new ManagerHome();
                            home.Show();
                        }
                        else
                        {
                            this.Hide();
                            userId = contains.userId;
                            UserHome home = new UserHome();
                            home.Show();
                        }

                    }
                    else
                    {
                        MessageBox.Show("Wrong password.");
                        return;
                    }
                }
            }

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
