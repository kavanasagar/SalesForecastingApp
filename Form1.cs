using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesForecastingApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Populate the ComboBox with years
            for (int year = 2018; year <= 2021; year++)
            {
                cbYear.Items.Add(year);
            }
            cbYear.SelectedIndex = 0;
        }
       
        private DataTable GetSalesData(int year)
        {
            // Connection string to your database
            string connectionString = "LAPTOP-S3NB9LAS\\SQLEXPRESS02";

            // SQL query to get sales data for the specified year
            string query = @"
        SELECT State, Sales
        FROM 
            SalesDataTable
        WHERE 
            Year = @Year";

            DataTable dt = new DataTable();
            dt.Columns.Add("State", typeof(string));
            dt.Columns.Add("Sales", typeof(decimal));
            dt.Columns.Add("ForecastedSales", typeof(decimal));

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Year", year);

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string state = reader["State"].ToString();
                            decimal sales = Convert.ToDecimal(reader["Sales"]);
                            dt.Rows.Add(state, sales, 0);
                        }
                    }
                }
            }

            return dt;
        }

        private void BtnCalculate_Click(object sender, EventArgs e)
        {
            int selectedYear = (int)cbYear.SelectedItem;
            if (int.TryParse(txtPercentage.Text, out int percentageIncrease))
            {
                // Fetch sales data for the selected year
                DataTable salesData = GetSalesData(selectedYear);

                // Apply the percentage increase
                foreach (DataRow row in salesData.Rows)
                {
                    decimal sales = (decimal)row["Sales"];
                    row["ForecastedSales"] = sales + (sales * percentageIncrease / 100);
                }
            }
        }
        //private void BtnExport_Click(object sender, EventArgs e)
        //{
        //    ExportToCSV();
        //}

        //private void ExportToCSV()
        //{
        //    using (StreamWriter writer = new StreamWriter("forecasted_data.csv"))
        //    {
        //        writer.WriteLine("State,Percentage Increase,Sales Value");
        //        foreach (DataGridViewRow row in dataGridView1.Rows)
        //        {
        //            if (row.Cells["State"].Value != null && row.Cells["ForecastedSales"].Value != null)
        //            {
        //                writer.WriteLine($"{row.Cells["State"].Value},{txtPercentage.Text},{row.Cells["ForecastedSales"].Value}");
        //            }
        //        }
        //    }
        //    MessageBox.Show("Data exported to CSV successfully.");
        //}


        //// Display the data in the DataGridView
        //dgvSales.DataSource = salesData;

        //// Display the data in the Chart
        //  chartSales.Series[0].Points.Clear();
        //    foreach ((DataRow row in salesData.Rows)
        //    {
        //        chartSales.Series[0].Points.AddXY(row["State"], row["ForecastedSales"]);
        //    }
        //   else
        // { 
        //  MessageBox.Show("Please enter a valid percentage increase.");
        //  }
    }
}



        


    
       


