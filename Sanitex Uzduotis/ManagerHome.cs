using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity.Migrations;

namespace Sanitex_Uzduotis
{
    public partial class ManagerHome : Form
    {
        public ManagerHome()
        {
            InitializeComponent();
            dataGridView2.CellEndEdit += dataGridView2_CellEndEdit;
        }

        public static List<user> branchUsers;
        public static DataGridView grid;
        public static int currBranchId;
        public static List<branch> branches;
        public static int maxTimeId;
        public static int maxUserId;
        public bool viewInPercentage;
        private void Home_Load(object sender, EventArgs e)
        {
            dataGridView2.Columns[0].ReadOnly = true;
            dataGridView2.Columns[7].ReadOnly = true;
            if (viewInPercentage == true)
            {
                DrawInPercentage();
            }
            else
            {
                DrawInHours();
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void DrawInPercentage()
        {
            dataGridView2.Rows.Clear();
            dataGridView2.Refresh();

            using (var db = new TimetableContext())
            {
                maxTimeId = db.timetables.Max(t => t.timetableId);
                maxUserId = db.users.Max(u => u.userId);
                branches = db.branches.ToList();
                for (int i = 0; i < branches.Count; i++)
                {
                    dataGridView2.Columns[i + 1].HeaderText = branches[i].branchname;
                }
                var currentBranch = db.users.Where(u => u.userId == Login.userId).FirstOrDefault().branchId;
                currBranchId = currentBranch;
                var users = db.users.Where(u => u.branchId == currentBranch
                && u.userType != db.users.Where(us => us.userId == Login.userId).FirstOrDefault().userType
                ).ToList();
                branchUsers = users;
                for (int i = 0; i < users.Count; i++)
                {
                    int totalTime = 0;
                    DataGridViewRow row = (DataGridViewRow)dataGridView2.Rows[0].Clone();
                    row.Cells[0].Value = users[i].username;
                    for (int j = 0; j < branches.Count; j++)
                    {
                        var currBranchId = branches[j].branchId;
                        var currUserId = users[i].userId;
                        var time = db.timetables.Where(t => t.userId == currUserId && t.branchId == currBranchId).FirstOrDefault();
                        row.Cells[j + 1].Value = time != null ? Math.Round((double)time.time / 168 * 100) + "%" : 0 + "%";
                        totalTime += time != null ? time.time : 0;
                    }
                    row.Cells[7].Value = Math.Round((double)totalTime / 168 * 100) + "%";
                    dataGridView2.Rows.Add(row);
                }
                //var userTimes = db.
                // row.Cells[ 1].Value = branches[0].branchname;
                //dataGridView2.Rows.Add(row);
                grid = dataGridView2;
            }
        }

        private void DrawInHours()
        {
            dataGridView2.Rows.Clear();
            dataGridView2.Refresh();

            using (var db = new TimetableContext())
            {
                maxTimeId = db.timetables.Max(t => t.timetableId);
                maxUserId = db.users.Max(u => u.userId);
                branches = db.branches.ToList();
                for (int i = 0; i < branches.Count; i++)
                {
                    dataGridView2.Columns[i + 1].HeaderText = branches[i].branchname;
                }
                var currentBranch = db.users.Where(u => u.userId == Login.userId).FirstOrDefault().branchId;
                currBranchId = currentBranch;
                var users = db.users.Where(u => u.branchId == currentBranch
                && u.userType != db.users.Where(us => us.userId == Login.userId).FirstOrDefault().userType
                ).ToList();
                branchUsers = users;
                for (int i = 0; i < users.Count; i++)
                {
                    int totalTime = 0;
                    DataGridViewRow row = (DataGridViewRow)dataGridView2.Rows[0].Clone();
                    row.Cells[0].Value = users[i].username;
                    for (int j = 0; j < branches.Count; j++)
                    {
                        var currBranchId = branches[j].branchId;
                        var currUserId = users[i].userId;
                        var time = db.timetables.Where(t => t.userId == currUserId && t.branchId == currBranchId).FirstOrDefault();
                        row.Cells[j + 1].Value = time != null ? time.time + "h" : 0 + "h";
                        totalTime += time != null ? time.time : 0;
                    }
                    row.Cells[7].Value = totalTime + "h";
                    dataGridView2.Rows.Add(row);
                }
                //var userTimes = db.
                // row.Cells[ 1].Value = branches[0].branchname;
                //dataGridView2.Rows.Add(row);
                grid = dataGridView2;
            }

        }



        private void button1_Click(object sender, EventArgs e)
        {
            if (viewInPercentage == true)
            {
                saveInPercentage();
            }
            else
            {
                saveInHours();
            }
        }

        private void saveInPercentage()
        {
            var obj = new object[dataGridView2.Rows.Count, dataGridView2.Rows[0].Cells.Count];
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView2.Rows[i].Cells.Count; j++)
                {
                    obj[i, j] = dataGridView2.Rows[i].Cells[j].Value;
                }
            }
            using (var db = new TimetableContext())
            {
                for (int i = 0; i < obj.Length / obj.GetLength(0) && obj[i, 0] != null; i++)
                {
                    var username = (string)obj[i, 0];
                    if (i < branchUsers.Count)
                    {
                        InsertOrUpdateUser(new user { username = (string)obj[i, 0], password = (string)obj[i, 0], userType = 0, branchId = currBranchId, userId = branchUsers[i].userId });
                    }
                    else
                    {
                        maxUserId += 1;
                        InsertOrUpdateUser(new user { username = (string)obj[i, 0], password = (string)obj[i, 0], userType = 0, branchId = currBranchId, userId = maxUserId });
                    }

                    for (int j = 0; j < 6; j++)
                    {
                        var branch = branches[j].branchId;
                        var user = db.users.Where(u => u.username == username).FirstOrDefault();
                        var temp = (String)obj[i, j + 1];
                        temp = temp.Substring(0, temp.Length - 1);
                        var time = System.Convert.ToInt32(temp);
                        time = (int)Math.Round((double)time * 1.68 );



                        if (db.timetables.Any(t => t.branchId == branch && t.userId == user.userId))
                        {
                            var any = db.timetables.Where(t => t.branchId == branch && t.userId == user.userId).FirstOrDefault().timetableId;
                            InsertOrUpdateTimetable((string)obj[i, 0], currBranchId, new timetable { branchId = branch, userId = user.userId, time = time, timetableId = any });
                        }
                        else
                        {
                            maxTimeId += 1;
                            InsertOrUpdateTimetable((string)obj[i, 0], currBranchId, new timetable { branchId = branch, userId = user.userId, time = time, timetableId = maxTimeId });
                        }

                    }
                }
            }
            DrawInPercentage();
        }

        private void saveInHours()
        {
            var obj = new object[dataGridView2.Rows.Count, dataGridView2.Rows[0].Cells.Count];
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView2.Rows[i].Cells.Count; j++)
                {
                    obj[i, j] = dataGridView2.Rows[i].Cells[j].Value;
                }
            }
            using (var db = new TimetableContext())
            {
                for (int i = 0; i < obj.Length / obj.GetLength(0) && obj[i, 0] != null; i++)
                {
                    var username = (string)obj[i, 0];
                    if (i < branchUsers.Count)
                    {
                        InsertOrUpdateUser(new user { username = (string)obj[i, 0], password = (string)obj[i, 0], userType = 0, branchId = currBranchId, userId = branchUsers[i].userId });
                    }
                    else
                    {
                        maxUserId += 1;
                        InsertOrUpdateUser(new user { username = (string)obj[i, 0], password = (string)obj[i, 0], userType = 0, branchId = currBranchId, userId = maxUserId });
                    }

                    for (int j = 0; j < 6; j++)
                    {
                        var branch = branches[j].branchId;
                        var user = db.users.Where(u => u.username == username).FirstOrDefault();
                        var time = (String)obj[i, j + 1];
                        time = time.Substring(0, time.Length - 1);

                        if (db.timetables.Any(t => t.branchId == branch && t.userId == user.userId))
                        {
                            var any = db.timetables.Where(t => t.branchId == branch && t.userId == user.userId).FirstOrDefault().timetableId;
                            InsertOrUpdateTimetable((string)obj[i, 0], currBranchId, new timetable { branchId = branch, userId = user.userId, time = System.Convert.ToInt32(time), timetableId = any });
                        }
                        else
                        {
                            maxTimeId += 1;
                            InsertOrUpdateTimetable((string)obj[i, 0], currBranchId, new timetable { branchId = branch, userId = user.userId, time = System.Convert.ToInt32(time), timetableId = maxTimeId });
                        }

                    }
                }
            }
            DrawInHours();
        }

        private void InsertOrUpdateUser(user user)
        {
            var id = user.userId;
            using (var db = new TimetableContext())
            {

                if (db.users.Any(u => u.userId == id))
                {
                    db.users.Attach(user);
                    db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                }
                
                else
                {
                    if (db.users.Where(u => u.username == user.username).FirstOrDefault() == null)
                    {
                        db.users.Add(user);
                    }
                    else
                    {
                        MessageBox.Show("Vartotojas vardu \"" + user.username +"\" jau yra");
                    }
                }
                db.SaveChanges();
            }
        }

        private void InsertOrUpdateTimetable(string username, int branchId, timetable time)
        {
            using (var db = new TimetableContext())
            {
                var id = time.timetableId;
                if (db.timetables.Any(t => t.timetableId == id))
                {
                    db.timetables.Attach(time);
                    db.Entry(time).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    db.timetables.Add(time);
                }
                db.SaveChanges();
            }
        }

        private void dataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int total = 0;
            var checkSymbol = dataGridView2[e.ColumnIndex, e.RowIndex].Value.ToString();
            if (e.ColumnIndex != 0)
            {
                if (viewInPercentage == true)
                {
                    checkSymbol = checkSymbol.Substring(checkSymbol.Length - 1);
                    if (checkSymbol != "%")
                    {
                        int temp;
                        if (int.TryParse(dataGridView2[e.ColumnIndex, e.RowIndex].Value.ToString(), out temp))
                        {
                            dataGridView2[e.ColumnIndex, e.RowIndex].Value += "%";
                            for (int i = 0; i < dataGridView2.Columns.Count - 2; i++)
                            {
                                var percent = dataGridView2[i + 1, e.RowIndex].Value != null ? dataGridView2[i + 1, e.RowIndex].Value.ToString() : null;
                                percent = percent != null ? percent.Substring(0, percent.Length - 1) : "0";
                                total += System.Convert.ToInt32(percent);
                            }
                            dataGridView2[7, e.RowIndex].Value = viewInPercentage ? total + "%" : total + "h";
                        }
                        else
                        {
                            MessageBox.Show("Neleistini simboliai, veskite skaicius: " + dataGridView2[e.ColumnIndex, e.RowIndex].Value.ToString() + "%'");
                            dataGridView2[e.ColumnIndex, e.RowIndex].Value = null;
                        }
                    }
                    else
                    {
                        var temp = dataGridView2[e.ColumnIndex, e.RowIndex].Value.ToString();
                        temp = temp.Substring(0, temp.Length - 1);
                        if (int.TryParse(temp, out int tmp))
                        {
                            for (int i = 0; i < dataGridView2.Columns.Count - 2; i++)
                            {
                                var percent = dataGridView2[i + 1, e.RowIndex].Value != null ? dataGridView2[i + 1, e.RowIndex].Value.ToString() : null;
                                percent = percent != null ? percent.Substring(0, percent.Length - 1) : "0";
                                total += System.Convert.ToInt32(percent);
                            }
                            dataGridView2[7, e.RowIndex].Value = viewInPercentage ? total + "%" : total + "h";
                        }
                        else
                        {
                            MessageBox.Show("Neleistini simboliai, veskite skaicius: " + dataGridView2[e.ColumnIndex, e.RowIndex].Value.ToString());
                            dataGridView2[e.ColumnIndex, e.RowIndex].Value = null;
                        }
                    }
                }
                else
                {
                    checkSymbol = checkSymbol.Substring(checkSymbol.Length - 1);
                    if (checkSymbol != "h")
                    {
                        int temp;
                        if (int.TryParse(dataGridView2[e.ColumnIndex, e.RowIndex].Value.ToString(), out temp))
                        {
                            dataGridView2[e.ColumnIndex, e.RowIndex].Value += "h";
                            for (int i = 0; i < dataGridView2.Columns.Count - 2; i++)
                            {
                                var percent = dataGridView2[i + 1, e.RowIndex].Value != null ? dataGridView2[i + 1, e.RowIndex].Value.ToString() : null;
                                percent = percent != null ? percent.Substring(0, percent.Length - 1) : "0";
                                total += System.Convert.ToInt32(percent);
                            }
                            dataGridView2[7, e.RowIndex].Value = viewInPercentage ? total + "%" : total + "h";
                        }
                        else
                        {
                            MessageBox.Show("Neleistini simboliai, veskite skaicius: " + dataGridView2[e.ColumnIndex, e.RowIndex].Value.ToString());
                            dataGridView2[e.ColumnIndex, e.RowIndex].Value = null;
                        }
                    }
                    else
                    {
                        var temp = dataGridView2[e.ColumnIndex, e.RowIndex].Value.ToString();
                        temp = temp.Substring(0, temp.Length - 1);
                        if (int.TryParse(temp, out int tmp))
                        {
                            for (int i = 0; i < dataGridView2.Columns.Count - 2; i++)
                            {
                                var percent = dataGridView2[i + 1, e.RowIndex].Value != null ? dataGridView2[i + 1, e.RowIndex].Value.ToString() : null;
                                percent = percent != null ? percent.Substring(0, percent.Length - 1) : "0";
                                total += System.Convert.ToInt32(percent);
                            }
                            dataGridView2[7, e.RowIndex].Value = viewInPercentage ? total + "%" : total + "h";
                        }
                        else
                        {
                            MessageBox.Show("Neleistini simboliai, veskite skaicius: " + dataGridView2[e.ColumnIndex, e.RowIndex].Value.ToString());
                            dataGridView2[e.ColumnIndex, e.RowIndex].Value = null;
                        }
                    }
                }
            }
            dataGridView2.Columns[0].ReadOnly = true;
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView2.Columns[0].ReadOnly = false;
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

        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView2.Columns[0].ReadOnly = true;
            dataGridView2.Columns[7].ReadOnly = true;
            if (viewInPercentage == true)
            {
                DrawInPercentage();
            }
            else
            {
                DrawInHours();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
            Login form = new Login();
            form.Show();
        }
    }
}
