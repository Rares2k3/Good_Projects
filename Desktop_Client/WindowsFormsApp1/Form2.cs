using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        // 1. VERIFICA AICI DACA PAROLA E BUNA! (Am pus 'rootpassword' ca exemplu)
        private string connectionString = "Server=localhost;Database=e-commerce_store;Uid=root;Pwd=rootpassword;";

        // ACESTA ESTE CONSTRUCTORUL - Ruleaza primul cand se deschide fereastra
        public Form2()
        {
            InitializeComponent();

            // --- DE AICI PORNESTE TOTUL ---
            // Daca nu pui liniile astea aici, tabelul ramane gol!
            LoadCategories();
            LoadProducts();
        }

        private void LoadCategories()
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la incarcarea Categoriilor: " + ex.Message);
            }
        }

        private void LoadProducts()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    // Selectam tot din produse
                    string query = "SELECT * FROM Product";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Aici legam datele de tabelul gri
                    dgvAdmin.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la incarcarea Produselor: " + ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNume.Text) ||
                string.IsNullOrWhiteSpace(txtPret.Text) ||
                string.IsNullOrWhiteSpace(cmbCategorie.Text))
            {
                MessageBox.Show("Completeaza toate campurile!");
                return;
            }

            string nume = txtNume.Text;
            if (!decimal.TryParse(txtPret.Text, out decimal pret))
            {
                MessageBox.Show("Pretul trebuie sa fie un numar valid!");
                return;
            }

            string categorieNume = cmbCategorie.Text;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string getCatIdQuery = "SELECT id FROM Category WHERE name = @catName";
                    MySqlCommand cmdGetId = new MySqlCommand(getCatIdQuery, conn);
                    cmdGetId.Parameters.AddWithValue("@catName", categorieNume);
                    object result = cmdGetId.ExecuteScalar();

                    if (result != null)
                    {
                        int categoryId = Convert.ToInt32(result);
                        string insertQuery = "INSERT INTO Product (name, price, category_id) VALUES (@name, @price, @catId)";
                        MySqlCommand cmdInsert = new MySqlCommand(insertQuery, conn);
                        cmdInsert.Parameters.AddWithValue("@name", nume);
                        cmdInsert.Parameters.AddWithValue("@price", pret);
                        cmdInsert.Parameters.AddWithValue("@catId", categoryId);

                        cmdInsert.ExecuteNonQuery();
                        MessageBox.Show("Produs adaugat cu succes!");

                        txtNume.Clear();
                        txtPret.Clear();
                        LoadProducts(); // Reincarcam tabelul
                    }
                    else
                    {
                        MessageBox.Show("Categoria nu exista!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Eroare SQL: " + ex.Message);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Verificam daca e selectat un rand in tabel
            if (dgvAdmin.SelectedRows.Count > 0)
            {
                try
                {
                    // Luam ID-ul de pe randul selectat (asigura-te ca in baza de date coloana e 'id' cu litere mici)
                    int idProdus = Convert.ToInt32(dgvAdmin.SelectedRows[0].Cells["id"].Value);

                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        string deleteQuery = "DELETE FROM Product WHERE id = @id";
                        MySqlCommand cmd = new MySqlCommand(deleteQuery, conn);
                        cmd.Parameters.AddWithValue("@id", idProdus);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Produs sters!");
                        LoadProducts(); // Reincarcam tabelul
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Eroare la stergere: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Selecteaza un rand complet din tabel pentru a sterge!");
            }
        }

        // Functia pentru click pe tabel (Optionala - pentru auto-completare)
        private void dgvAdmin_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvAdmin.Rows[e.RowIndex];
                txtNume.Text = row.Cells["name"].Value.ToString();
                txtPret.Text = row.Cells["price"].Value.ToString();
            }
        }

        // Daca ai metoda asta ramasa goala, o poti lasa asa
        private void Form2_Load(object sender, EventArgs e)
        {
        }
    }
}