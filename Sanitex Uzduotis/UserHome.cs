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
    public partial class UserHome : Form
    {
        public UserHome()
        {
            InitializeComponent();
        }

        public bool viewInPercentage;

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
         
        }



        private void UserHome_Load(object sender, EventArgs e)
        {
            dataGridView2.ReadOnly = true;
            using (var db = new TimetableContext())
            {
                var branches = db.branches.ToList();
                for (int i = 0; i < branches.Count; i++)
                {
                    dataGridView2.Columns[i + 1].HeaderText = branches[i].branchname;
                }
                var user = db.users.Where(u => u.userId == Login.userId).FirstOrDefault();
                int totalTime = 0;
                DataGridViewRow row = (DataGridViewRow)dataGridView2.Rows[0].Clone();
                row.Cells[0].Value = user.username;
                for (int j = 0; j < branches.Count; j++)
                {
                    var currBranchId = branches[j].branchId;
                    var currUserId = user.userId;
                    var time = db.timetables.Where(t => t.userId == currUserId && t.branchId == currBranchId).FirstOrDefault();
                    row.Cells[j + 1].Value = time != null ? time.time + "h": 0 + "h";
                    totalTime += time != null ? time.time : 0;
                }
                    row.Cells[7].Value = totalTime + "h";
                    dataGridView2.Rows.Add(row);
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (viewInPercentage == true)
            {
                viewInPercentage = false;
                RedrawInHours();
            }

            else
            {
                viewInPercentage = true;
                RedrawInPercentage();
            }
        }

        private void RedrawInPercentage()
        {
            if (viewInPercentage == true)
            {
                for (int i = 0; i < dataGridView2.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < dataGridView2.Columns.Count - 1; j++)
                    {
                        var hours = dataGridView2[j + 1, i].Value != null ? dataGridView2[j + 1, i].Value.ToString() : null;

                        hours = hours != null && hours.Length != 1 ? hours.Substring(0, hours.Length - 1) : "0";

                        dataGridView2[j + 1, i].Value = (int)Math.Round((double)System.Convert.ToInt32(hours) / 168 * 100) + "%";
                    }
                }
            }
        }

        private void RedrawInHours()
        {
            if (viewInPercentage != true)
            {
                for (int i = 0; i < dataGridView2.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < dataGridView2.Columns.Count - 1; j++)
                    {
                        var hours = dataGridView2[j + 1, i].Value != null ? dataGridView2[j + 1, i].Value.ToString() : null;

                        hours = hours != null && hours.Length != 1 ? hours.Substring(0, hours.Length - 1) : "0";

                        dataGridView2[j + 1, i].Value = (int)Math.Round((double)System.Convert.ToInt32(hours) * 1.68) + "h";
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            Login form = new Login();
            form.Show();
        }
    }
}
