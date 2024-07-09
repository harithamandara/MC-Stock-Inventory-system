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
    public partial class Stock : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\harit\OneDrive\Documents\Stock2Db.mdf;Integrated Security=True;Connect Timeout=30");

        public Stock()
        {
            InitializeComponent();
            LoadDataIntoDataGridView();
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            // Ensure inputs are not empty before proceeding
            if (string.IsNullOrWhiteSpace(txtItemNo.Text) ||
                string.IsNullOrWhiteSpace(txtItemName.Text) ||
                string.IsNullOrWhiteSpace(txtQuantity.Text) ||
                dtpDateTimeAdded.Value == null)
            {
                MessageBox.Show("All fields must be filled in.");
                return;
            }

            try
            {
                int itemNo = int.Parse(txtItemNo.Text);
                string itemName = txtItemName.Text;
                int quantity = int.Parse(txtQuantity.Text);
                DateTime dateTimeAdded = dtpDateTimeAdded.Value;

                // Open the database connection
                con.Open();

                
                string query = "INSERT INTO Product (ItemNo, ItemName, Quantity, DateTimeAdded) VALUES (@ItemNo ,@ItemName, @Quantity, @DateTimeAdded)";

                SqlCommand disableIdentityInsert = new SqlCommand("SET IDENTITY_INSERT Product ON", con);
                disableIdentityInsert.ExecuteNonQuery();

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ItemNo", itemNo);
                cmd.Parameters.AddWithValue("@ItemName", itemName);
                cmd.Parameters.AddWithValue("@Quantity", quantity);
                cmd.Parameters.AddWithValue("@DateTimeAdded", dateTimeAdded);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Stock Added Successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();

                LoadDataIntoDataGridView();  // Reload the data into the DataGridView after adding
            }
        }

        private void btnEditQuantity_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtItemNo.Text) || string.IsNullOrWhiteSpace(txtQuantity.Text))
            {
                MessageBox.Show("Item No and new Quantity must be filled in.");
                return;
            }

            try
            {
                int itemNo = int.Parse(txtItemNo.Text);
                int newQuantity = int.Parse(txtQuantity.Text);
                DateTime dateTimeAdded = dtpDateTimeAdded.Value;

                // Open the database connection
                con.Open();

                // Update the quantity and set the Updated_DateTime column to the current timestamp
                string query = "UPDATE Product SET Quantity = @NewQuantity WHERE ItemNo = @ItemNo";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@NewQuantity", newQuantity);
                // cmd.Parameters.AddWithValue("@DateTimeAdded", DateTime.Now);
                cmd.Parameters.AddWithValue("@ItemNo", itemNo);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Stock Quantity Updated Successfully!");
                con.Close();

                LoadDataIntoDataGridView();

              
                // LoadDataToDataGridView(); 
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


        private void LoadDataIntoDataGridView()
        {
            try
            {
                con.Open();

                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM Product", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dgvStockItems.DataSource = dt;
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

        private void btnDeleteItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtItemNo.Text))
            {
                MessageBox.Show("Item No must be filled in to delete an item.");
                return;
            }

            try
            {
                int itemNo = int.Parse(txtItemNo.Text);

                con.Open();

                string checkQuery = "SELECT COUNT(1) FROM Product WHERE ItemNo = @ItemNo";
                SqlCommand cmdCheck = new SqlCommand(checkQuery, con);
                cmdCheck.Parameters.AddWithValue("@ItemNo", itemNo);

                int count = Convert.ToInt32(cmdCheck.ExecuteScalar());

                if (count == 0)
                {
                    MessageBox.Show("No item found with the specified Item No.");
                    return;
                }

                string deleteQuery = "DELETE FROM Product WHERE ItemNo = @ItemNo";
                SqlCommand cmdDelete = new SqlCommand(deleteQuery, con);
                cmdDelete.Parameters.AddWithValue("@ItemNo", itemNo);

                cmdDelete.ExecuteNonQuery();

                MessageBox.Show("Item Deleted Successfully!");

                con.Close();

                LoadDataIntoDataGridView();
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

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnTransaction_Click(object sender, EventArgs e)
        {
            Transaction ts = new Transaction();
            ts.Show();
            this.Hide();
        }

        private void txtItemNo_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
