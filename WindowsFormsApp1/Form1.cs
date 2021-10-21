using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        Employee employee = new Employee();
        int id = 0;
        public Form1()
        {
            InitializeComponent();
        }


        private void btn_next_Click(object sender, EventArgs e)
        {
            NextEmployee();

        }
        private void NextEmployee()
        {
            bool done = false;
            //employee.EmployeeID = id;
            while (!done)
            {

                using (NORTHWNDEntities db = new NORTHWNDEntities())
                {
                    if (db.Employees.Where(x => x.EmployeeID > id - 1).FirstOrDefault() != null)
                    { 
                        id++;
                        employee = db.Employees.Where(x => x.EmployeeID == id).FirstOrDefault();
                        if (employee != null)
                        {
                            textBox_id.Text = employee.EmployeeID.ToString();
                            textBox_firstName.Text = employee.FirstName;
                            textBox_lastName.Text = employee.LastName;
                           // textBox_birthDate.Text = employee.BirthDate.ToString();
                            dateTimePicker1.Value = employee.BirthDate.Value;
                            done = true;
                        }
                       

                    }
                    else
                    {
                        MessageBox.Show("This is the last employee!", "Error");
                        done = true;
                        id--;
                    }
                }

            }
        }

    

        private void btn_add_Click(object sender, EventArgs e)
        {
            employee.FirstName = textBox_firstName.Text.Trim();
            employee.LastName = textBox_lastName.Text.Trim();
            employee.BirthDate = dateTimePicker1.Value;
            //employee.BirthDate = DateTime.Parse(textBox_birthDate.Text);
            using(NORTHWNDEntities db = new NORTHWNDEntities())
            {
                db.Employees.Add(employee);
                db.SaveChanges();
            }
            id--;
            NextEmployee();
            MessageBox.Show("Employee added");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            id--;
            NextEmployee();      

        }

        private void btn_previous_Click(object sender, EventArgs e)
        {

            if (id > 1)
            {
               bool done = false;
               while(!done)
               {
                    
                        id--;
                        using (NORTHWNDEntities db = new NORTHWNDEntities())
                        {
                            employee = db.Employees.Where(x => x.EmployeeID == id).FirstOrDefault();
                            if (employee != null)
                            {
                                textBox_id.Text = employee.EmployeeID.ToString();
                                textBox_firstName.Text = employee.FirstName;
                                textBox_lastName.Text = employee.LastName;
                                dateTimePicker1.Value = employee.BirthDate.Value;
                                //textBox_birthDate.Text = employee.BirthDate.ToString();
                                done = true;
                            }
                            

                        }
               }
            }
            else MessageBox.Show("This is the first employee!", "Error");
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this employee?","Delete Empoyee", MessageBoxButtons.YesNo) == DialogResult.Yes) 
            {
                using(NORTHWNDEntities db = new NORTHWNDEntities())
                {
                    var entry = db.Entry(employee);
                    if (entry.State == EntityState.Detached)
                        db.Employees.Attach(employee);
                    db.Employees.Remove(employee);
                    db.SaveChanges();
                    NextEmployee();

                }
            }
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            employee.FirstName = textBox_firstName.Text.Trim();
            employee.LastName = textBox_lastName.Text.Trim();
            employee.BirthDate = dateTimePicker1.Value;
            using (NORTHWNDEntities db = new NORTHWNDEntities())
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
            }
            MessageBox.Show("Employee updated");
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
