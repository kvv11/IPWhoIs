using DataAccessLayer;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace IP_Whois
{
    public partial class FormPregled : Form
    {
        private BindingSource bindingSource;
        private SqlDataAdapter dataAdapter;
        private IpsRepository ipsRepository;

        public FormPregled()
        {
            InitializeComponent();

            ConfigureDataGridView();
            BindDataToDataGridView();
            ipsRepository = new IpsRepository();

        }

        private void ConfigureDataGridView()
        {
            dataGridView1.AutoGenerateColumns = true;
            bindingSource = new BindingSource();
            dataGridView1.DataSource = bindingSource;
            dataGridView1.AllowUserToAddRows = false;
            ipsRepository = new IpsRepository();

        }

        private void BindDataToDataGridView()
        {
            try
            {
                string connectionString = "Data Source=193.198.57.183;Initial Catalog=STUDENTI_TINF;Persist Security Info=True;User ID=tinf;Password=tinf123!";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT ipTable.ip, type, continent, continent_code, country, country_code, region, region_code, " +
               "city, latitude, longitude, postal, calling_code, capital, borders, domain, org, isp, id, utc, " +
               "registrar_name, whois_server, website_url, domain_registered, create_date, update_date, expiry_date " +
               "FROM Veletic_Ip ipTable " +
               "JOIN Veletic_Whois whoisTable ON ipTable.ip = whoisTable.ip";


                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        AddEditButtonColumn();

                        dataGridView1.DataSource = dataTable;

                        dataGridView1.Columns["ip"].HeaderText = "IP";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška prilikom dohvaćanja podataka iz baze: {ex.Message}");
            }

        }

        private void AddEditButtonColumn()
        {
            DataGridViewButtonColumn editButtonColumn = new DataGridViewButtonColumn();
            editButtonColumn.Name = "EditButton";
            editButtonColumn.HeaderText = "Detalji";
            editButtonColumn.Text = "Detalji";
            editButtonColumn.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Insert(0, editButtonColumn);

            dataGridView1.CellContentClick += DataGridView1_CellContentClick;
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string ip = "0";
            string city = "0";
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView1.Columns["EditButton"].Index)
            {
                 ip = dataGridView1.Rows[e.RowIndex].Cells["IP"].Value.ToString();
                 city = dataGridView1.Rows[e.RowIndex].Cells["city"].Value.ToString(); 
                FormDetalji detaljiForm = new FormDetalji(dataGridView1.Rows[e.RowIndex]);
                detaljiForm.Show();
                Hide();
            }

            ipsRepository.ImportVrijeme2(ip, city);
        }






        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void FormPregled_Load(object sender, EventArgs e)
        {

        }

        private void FormPregled_Load_1(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            FormUnos formUnos = new FormUnos();
            formUnos.Show();
        }

        private void FormPregled_Load_2(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.Title = "Save As Text File";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;

                try
                {
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(filePath))
                    {

                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                if (cell.OwningColumn.HeaderText != "Detalji")
                                {
                                    sw.WriteLine($"{cell.OwningColumn.HeaderText} - {cell.Value}");
                                }
                            }
                            sw.WriteLine(new string("---------------------------------------"));
                        }

                        MessageBox.Show("Podaci uspješno spremljeni u .txt datoteku.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"nije spremio {ex.Message}");
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string search = textBox1.Text.Trim();

            if (!string.IsNullOrEmpty(search))
            {
                List<string> filterExpressions = new List<string>();

                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    if (column.DataPropertyName != null && column.DataPropertyName != "")
                    {
                        if (dataGridView1.Columns[column.DataPropertyName].ValueType == typeof(string))
                        {
                            filterExpressions.Add($"[{column.DataPropertyName}] LIKE '%{search}%'");
                        }
                    }
                }

                string combinedFilter = string.Join(" OR ", filterExpressions);

                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = combinedFilter;
            }
            else
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Empty;
            }
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
