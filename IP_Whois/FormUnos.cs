using DataAccessLayer;
using System;
using System.Data;
using System.Data.Entity.Core.Common.EntitySql;
using System.Data.SqlClient;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IP_Whois
{
    public partial class FormUnos : Form
    {
        private IpsRepository ipsRepository;
        public FormUnos()
        {
            InitializeComponent();
            ipsRepository = new IpsRepository();
          
        }

        private void FormUnos_Load(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await ipsRepository.ImportIPP(textBox1.Text);
            textBox1.Text = string.Empty;
            

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
  
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormPregled pregledForm = new FormPregled();
            pregledForm.Show();
            Hide();
        }

        public void Aimoo()
        {

        }
    }
}
