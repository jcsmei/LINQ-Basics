using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Data.OleDb;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LINQBasics
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'employeesDataSet.tblEmploy' table. You can move, or remove it, as needed.
            this.tblEmployTableAdapter.Fill(this.employeesDataSet.tblEmploy);

        }

        private void btnSalary_Click(object sender, EventArgs e)
        {
            var record = from em in employeesDataSet.tblEmploy.AsEnumerable()
                         where em.Rate < 18
                         orderby em.Last_Name
                         select em;
            tblEmployBindingSource.DataSource = record.AsDataView();
            this.Text = "All Enmployees whose salary is less than 18 dollars an hour";
        }

        private void btnCompound_Click(object sender, EventArgs e)
        {
            var record = from em in employeesDataSet.tblEmploy.AsEnumerable()
                         where em.Rate < 15 && em.Field<string>("Status") == "P"
                         orderby em.Last_Name
                         select em;
            tblEmployBindingSource.DataSource = record.AsDataView();
            this.Text = "All full time employees whose salary is less than 15 dollars an hour";

        }

        private void btnAggregate_Click(object sender, EventArgs e)
        {
            decimal topPay = (from em in employeesDataSet.tblEmploy.AsEnumerable()
                              select em.Rate).Max();

            decimal averageRate = (from em in employeesDataSet.tblEmploy.AsEnumerable()
                              select em.Rate).Average();

            decimal averageRateP = (from em in employeesDataSet.tblEmploy.AsEnumerable()
                                    where em.Status == "P"                                  
                              select em.Rate).Average();

            decimal averageRateF = (from em in employeesDataSet.tblEmploy.AsEnumerable()
                                    where em.Status == "F"
                              select em.Rate).Average();

            MessageBox.Show("Highest pay: " + topPay.ToString("c2") + Environment.NewLine +
                "Average Pay: " + averageRate.ToString("c2") + Environment.NewLine +
                "Part-time average pay: " + averageRateP.ToString("c2") + Environment.NewLine +
                "Full-time average pay: " + averageRateF.ToString("C2"));

            //            //assign datatable as variable
            //            var employee = employeesDataSet.tblEmploy.AsEnumerable();

            //            //average
            //            decimal averageRate = employee.Average(x => x.Field<decimal>("Rate"));

            //            //Max
            //            decimal topPay = employee.Max(x => x.Field<decimal>("Rate"));

            //            //Minimum
            //            decimal bottomPay =  employee.Min(x => x.Field<decimal>("Rate"));
            //            MessageBox.Show("Bottom pay: " + bottomPay.ToString("c2") +
            //                @"
            //Top pay: " + topPay.ToString("c2") +
            //@"
            //Average Rate: " + averageRate.ToString("c2"));



        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            tblEmployBindingSource.DataSource = employeesDataSet.tblEmploy;
            //show all items in real time
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            var record = from employee in employeesDataSet.tblEmploy.AsEnumerable()
                         select employee;
            //select employee.Rate;
            tblEmployBindingSource.DataSource = record.AsDataView();
            this.Text = "All Employees";
        }

        private void btnOrderedBy_Click(object sender, EventArgs e)
        {
            var record = from employee in employeesDataSet.tblEmploy.AsEnumerable()
                         orderby employee.Last_Name
                         select employee;
                         
            tblEmployBindingSource.DataSource = record.AsDataView();
            this.Text = "All employees ordered by last name";
        }

        private void btnLastName_Click(object sender, EventArgs e)
        {
            //for string fields, three methods may be used: StartsWith, EndsWith, Contains
            //Last_Name StartsWith("S")
            var record = (from em in employeesDataSet.tblEmploy.AsEnumerable()
                         //where em.Field<string>("Last_Name").StartsWith("S")
                         where em.Last_Name.StartsWith("S") //sometimes works, compiler can't tell datatype
                         orderby em.Last_Name
                         select em).Count();
            tblEmployBindingSource.DataSource = record.ToString();
            this.Text = "All employees whose last name begins with S";
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            tblEmployBindingSource.AddNew();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                this.Validate();
                this.tblEmployBindingSource.EndEdit();
                this.tableAdapterManager.UpdateAll(this.employeesDataSet);
                MessageBox.Show("Record updated",
                    "Employee Records",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                    "EMployee Records",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                tblEmployBindingSource.RemoveCurrent();
                this.Validate();
                this.tblEmployBindingSource.EndEdit();
                this.tableAdapterManager.UpdateAll(this.employeesDataSet);
                //this.tblEmployTableAdapter.Update(this.employeesDataSet);
                MessageBox.Show("Record Deleted",
                    "Employee Records",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                    "EMployee Records",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            var record = from em in employeesDataSet.tblEmploy.AsEnumerable()
                         where em.Last_Name.Contains(txtSearch.Text)
                         select em;
            tblEmployBindingSource.DataSource = record.AsDataView();


        }
    }
}
