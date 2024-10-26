using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IP_Whois
{
    public partial class Pocetna : Form
    {
        public Pocetna()
        {
            InitializeComponent();
         
            
             
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            FormPregled pregledForm = new FormPregled();
            pregledForm.Show();
            Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormUnos unosFomr = new FormUnos();
            unosFomr.Show();
            Hide();
        }
    }
}
