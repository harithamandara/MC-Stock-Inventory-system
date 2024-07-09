using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kjfd
{
    public partial class Transaction : Form
    {

        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\harit\OneDrive\Documents\Stock2Db.mdf;Integrated Security=True;Connect Timeout=30");
        public Transaction()
        {
            InitializeComponent();
            dataGridView2_CellContentClick();
        }

        private void dataGridView2_CellContentClick()
        {
            try
            {
                con.Open();

                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM Product", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dataGridView3.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Stock st = new Stock();
            st.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
