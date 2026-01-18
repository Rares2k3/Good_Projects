using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form // Aici am schimbat in Form2
    {
        // Conectare la baza de date corecta
        private string connectionString = "Server=localhost;Database=e-commerce_store;Uid=root;Pwd=rootpassword;";

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            LoadCategories();
            LoadProducts();
        }

        // 1. Incarcam categoriile in ComboBox
        private void LoadCategories()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT name FROM Category";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                cmbCategorie.Items.Clear();
                while (reader.Read())
                {
                    cmbCategorie.Items.Add(reader.GetString("name"));
                }
                if (cmbCategorie.Items.Count > 0) cmbCategorie.SelectedIndex = 0;
            }
        }

        // 2. Incarcam produsele in tabel
        private void LoadProducts()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Product";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvAdmin.DataSource = dt;
            }
        }

        // 3. BUTONUL ADAUGA
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Verificam sa nu fie campurile goale
            if (string.IsNullOrWhiteSpace(txtNume.Text) || string.IsNullOrWhiteSpace(txtPret.Text) || cmbCategorie.SelectedItem == null)
            {
                MessageBox.Show("Completeaza toate campurile!");
                return;
            }

            string nume = txtNume.Text;
            string pret = txtPret.Text;
            string categorieNume = cmbCategorie.SelectedItem.ToString();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                // Aflam ID-ul categoriei selectate
                string getCatIdQuery = "SELECT id FROM Category WHERE name = @catName";
                MySqlCommand cmdGetId = new MySqlCommand(getCatIdQuery, conn);
                cmdGetId.Parameters.AddWithValue("@catName", categorieNume);
                object result = cmdGetId.ExecuteScalar();

                if (result != null)
                {
                    int categoryId = Convert.ToInt32(result);

                    // Inseram produsul
                    string insertQuery = "INSERT INTO Product (name, price, category_id) VALUES (@name, @price, @catId)";
                    MySqlCommand cmdInsert = new MySqlCommand(insertQuery, conn);
                    cmdInsert.Parameters.AddWithValue("@name", nume);
                    cmdInsert.Parameters.AddWithValue("@price", decimal.Parse(pret));
                    cmdInsert.Parameters.AddWithValue("@catId", categoryId);

                    cmdInsert.ExecuteNonQuery();
                    MessageBox.Show("Produs adaugat cu succes!");

                    // Curatam campurile si reincarcam tabelul
                    txtNume.Clear();
                    txtPret.Clear();
                    LoadProducts();
                }
            }
        }

        // 4. BUTONUL STERGE
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvAdmin.SelectedRows.Count > 0)
            {
                int idProdus = Convert.ToInt32(dgvAdmin.SelectedRows[0].Cells["id"].Value);

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string deleteQuery = "DELETE FROM Product WHERE id = @id";
                    MySqlCommand cmd = new MySqlCommand(deleteQuery, conn);
                    cmd.Parameters.AddWithValue("@id", idProdus);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Produs sters!");
                    LoadProducts();
                }
            }
            else
            {
                MessageBox.Show("Selecteaza un rand complet pentru a sterge!");
            }
        }
    }
}