using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private string connectionString = "Server=localhost;Database=e-commerce_store;Uid=root;Pwd=rootpassword;";
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadCategories();
            LoadProductsFromMySql();
        }

        private void LoadCategories()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT name FROM Category";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                comboBox1.Items.Clear();
                comboBox1.Items.Add("All Categories");

                while (reader.Read())
                {
                    comboBox1.Items.Add(reader.GetString("name"));
                }
                comboBox1.SelectedIndex = 0;
            }
        }

        private void LoadProductsFromMySql()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                SELECT p.id, p.name, p.price, c.name AS category,
                    IFNULL(AVG(r.rating),0) AS avgRating
                From Product p
                LEFT JOIN Category c ON p.category_id = c.id
                LEFT JOIN Review r ON p.id = r.product_id
                GROUP BY p.id, name, p.price, c.name";

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dataGridView1.DataSource = dt;
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataTable dt = dataGridView1.DataSource as DataTable;
            if (dt != null && dt.Columns.Contains("avgRating"))
            {
                DataView dv = dt.DefaultView;
                dv.Sort = "avgRating DESC";
                dataGridView1.DataSource = dv.ToTable();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dt = dataGridView1.DataSource as DataTable;
            if (dt != null)
            {
                DataView dv = dt.DefaultView;
                dv.Sort = "price ASC";
                dataGridView1.DataSource = dv.ToTable();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCategory = comboBox1.SelectedItem.ToString();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string query;
                if (selectedCategory == "All Categories")
                {
                    query = @"
                    SELECT p.id, p.name, p.price, c.name AS category,
                       IFNULL(AVG(r.rating), 0) AS avgRating
                    FROM Product p
                    LEFT JOIN Category c ON p.category_id = c.id
                    LEFT JOIN Review r ON p.id = r.product_id
                    GROUP BY p.id, p.name, p.price, c.name";
                }
                else
                {
                    query = @"
                    SELECT p.id, p.name, p.price, c.name AS category,
                       IFNULL(AVG(r.rating), 0) AS avgRating
                    FROM Product p
                    LEFT JOIN Category c ON p.category_id = c.id
                    LEFT JOIN Review r ON p.id = r.product_id
                    WHERE c.name = @category
                    GROUP BY p.id, p.name, p.price, c.name";
                }

                MySqlCommand cmd = new MySqlCommand(query, conn);
                if(selectedCategory != "All Categories")
                {
                    cmd.Parameters.AddWithValue("@category", selectedCategory);
                }
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dataGridView1.DataSource = dt;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            LoadProductsFromMySql();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            string password = Microsoft.VisualBasic.Interaction.InputBox("Enter admin password:", "Verification", "");

            // 2. Verificam parola
            if (password == "admin123") // Poti schimba parola aici
            {
                // 3. Deschidem fereastra de Admin (Form2)
                Form2 adminPage = new Form2();
                adminPage.Show();
            }
            else
            {
                MessageBox.Show("Parola gresita! Acces interzis.");
            }
        }
    }
}