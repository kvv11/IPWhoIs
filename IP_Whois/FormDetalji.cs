using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using SkiaSharp;
using DataAccessLayer;
using System.Data.SqlClient;
using System.Diagnostics;






namespace IP_Whois
{
    public partial class FormDetalji : Form
    {
        private IpsRepository ipsRepository;

        public FormDetalji()
        {
            InitializeComponent();
            ipsRepository = new IpsRepository();
        }

        public FormDetalji(DataGridViewRow selectedRow)
        {
            InitializeComponent();
            ipsRepository = new IpsRepository();       

            foreach (Control control in Controls)
            {
                if (control is TextBox textBox)
                {
                    string propertyName = textBox.Name.Replace("textBox", "");

                    DataGridViewCell matchingCell = selectedRow.Cells.Cast<DataGridViewCell>()
                        .FirstOrDefault(c => c.OwningColumn.Name == propertyName);

                    if (matchingCell != null)
                    {
                        object cellValue = matchingCell.Value;

                        if (cellValue != null)
                        {
                            textBox.Text = cellValue.ToString();
                        }
                    }
                }
            }

            foreach (Control control in Controls)
            {
                if (control is Label label)
                {
                    string propertyName = label.Name.ToLower(); 
                    DataGridViewCell cell = selectedRow.Cells.Cast<DataGridViewCell>().FirstOrDefault(c => c.OwningColumn.Name.ToLower() == propertyName);

                    if (cell != null)
                    {
                        label.Text = cell.Value?.ToString() ?? "N/A";
                    }
                }
            }

            string cc = selectedRow.Cells["country_code"].Value.ToString();
            Prikaz(cc, pictureBox1);

            float lat = Convert.ToSingle(selectedRow.Cells["latitude"].Value);
            float lon = Convert.ToSingle(selectedRow.Cells["longitude"].Value);
            Prikazmape(lat, lon);
            spremi.Visible= false;
            button3.Visible = false;



        }

        private void FormDetalji_Load(object sender, EventArgs e)
        {
            Label hourLabel = Controls.Find("hour", true).FirstOrDefault() as Label;
            Label monthLabel = Controls.Find("month", true).FirstOrDefault() as Label;
            Label dayLabel = Controls.Find("day", true).FirstOrDefault() as Label;
            Label dateLabel = Controls.Find("date", true).FirstOrDefault() as Label;
            Label yearLabel = Controls.Find("year", true).FirstOrDefault() as Label;
            Label minuteLabel = Controls.Find("minute", true).FirstOrDefault() as Label;
            Label secondLabel = Controls.Find("second", true).FirstOrDefault() as Label;
            Label day_of_weekLabel = Controls.Find("day_of_week", true).FirstOrDefault() as Label;
            Label ipLabel = Controls.Find("ip", true).FirstOrDefault() as Label;

            string connectionString = "Data Source=193.198.57.183;Initial Catalog=STUDENTI_TINF;Persist Security Info=True;User ID=tinf;Password=tinf123!";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM Veletic_Datetime WHERE ip = @ip";

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@ip", ipLabel.Text); 

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                           
                            hourLabel.Text = reader["hour"].ToString();  
                            monthLabel.Text = reader["month"].ToString();  
                            dayLabel.Text = reader["day"].ToString();
                            dateLabel.Text = reader["date"].ToString();
                            minuteLabel.Text = reader["minute"].ToString();
                            secondLabel.Text = reader["second"].ToString();
                            day_of_weekLabel.Text = reader["day_of_week"].ToString();
                            yearLabel.Text = reader["year"].ToString();
                        }
                    }
                }
            }

            string ip = ipLabel.Text;
            string query = "SELECT registrar_name, whois_server, website_url, domain_registered, create_date, update_date, expiry_date FROM Veletic_Whois WHERE ip = @ip";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ip", ip);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            foreach (Control control in Controls)
                            {
                                if (control is TextBox textBox)
                                {
                                    string columnName = textBox.Name;

                                    try
                                    {
                                        int columnIndex = reader.GetOrdinal(columnName);
                                        object columnValue = reader.GetValue(columnIndex);
                                        textBox.Text = columnValue != DBNull.Value ? columnValue.ToString() : string.Empty;
                                    }
                                    catch (Exception ex)
                                    {
                                       
                                    }
                                }
                            }
                        }
                    }
                }
            }



        }



        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox17_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox18_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox19_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox20_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Prikazmape(double latitude, double longitude)
        {
            
            gMapControl1.Position = new GMap.NET.PointLatLng(latitude, longitude);

            gMapControl1.MinZoom = 1;
            gMapControl1.MaxZoom = 100;
            gMapControl1.Zoom = 10;
            gMapControl1.MapProvider = GMap.NET.MapProviders.GMapProviders.GoogleMap;

        }

        private void Prikaz(string cc, PictureBox pictureBox)
        {
            string aa = cc.ToLower();
            try
            {
                using (WebClient client = new WebClient())
                {
                  
                    byte[] imageData = client.DownloadData($"https://flagcdn.com/144x108/{aa}.png");
                
                    using (var ms = new System.IO.MemoryStream(imageData))
                    {
                        Image image = Image.FromStream(ms);             
                        pictureBox.Image = image;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška prilikom učitavanja slike: {ex.Message}");
            }
        }

        private void gMapControl1_Load(object sender, EventArgs e)
        {

        }
        private void GMapControl1_MouseMove(object sender, MouseEventArgs e)
        {
        
        }

        private void dugme1_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
            FormPregled formPregled= new FormPregled();
            formPregled.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Button dugme = sender as Button;

            foreach (Control control in Controls)
            {
                if (control is TextBox textbox)
                {
                    if (textbox.ReadOnly == true)
                    {
                        textbox.ReadOnly = false;
                        dugme.Text = "Onemogući uređivanje";
                        spremi.Visible = true;
                        button3.Visible = true;
                    }
                    else if (textbox.ReadOnly == false)
                    {
                        textbox.ReadOnly = true;
                        dugme.Text = "Omogući uređivanje";
                        spremi.Visible = false;
                        button3.Visible = false;
                    }
                }
            }
        }

        public void spremi_Click(object sender, EventArgs e)
        {
            string ipp = ip.Text;
            string domainn = domain.Text;
            string typee = type.Text;
            string continentt = continent.Text;
            string cc = continent_code.Text;
            string cou = country.Text;
            string coucou = country_code.Text;
            string reg = region.Text;
            string rc = region_code.Text;
            string postall = postal.Text;
            string ccod = calling_code.Text;
            string tz = id.Text;
            string cp = capital.Text;
            string bor = borders.Text;
            double latt = double.Parse(latitude.Text); 
            double longg = double.Parse(longitude.Text);
            string utcc = utc.Text;
            string cityy = city.Text;
            string rn = registrar_name.Text;
            string ws = whois_server.Text;
            string wu = website_url.Text;
            string dr = domain_registered.Text;
            string cd = create_date.Text;
            string ud = update_date.Text;
            string ed = expiry_date.Text;

            ipsRepository.Azurirajgumb(ipp, domainn, typee, continentt, cc, cou, coucou, reg, rc, postall, ccod, tz, cp, bor, latt, longg, utcc, cityy);
            ipsRepository.AzurirajGumb2(ipp, rn, ws, wu, dr, cd, ud, ed);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string ipp = ip.Text;
            ipsRepository.Obrisiip(ipp);
            Hide();
            FormPregled formPregled = new FormPregled();
            formPregled.Show();
        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void year_Click(object sender, EventArgs e)
        {

        }

        private void label25_Click(object sender, EventArgs e)
        {

        }

        private void label29_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string ipf = ip.Text;
            string cityf = city.Text;
            ipsRepository.ImportVrijeme2(ipf, cityf);
                    
            var najnovijiPodaci = ipsRepository.NoviPodaci(ipf);
             hour.Text = najnovijiPodaci?.hour.ToString() ?? "N/A";
            month.Text = najnovijiPodaci?.month.ToString() ?? "N/A";
            year.Text = najnovijiPodaci?.year.ToString() ?? "N/A";
            day_of_week.Text = najnovijiPodaci?.day_of_week.ToString() ?? "N/A";
            day.Text = najnovijiPodaci?.day.ToString() ?? "N/A";
            date.Text = najnovijiPodaci?.date.ToString() ?? "N/A";
            minute.Text = najnovijiPodaci?.minute.ToString() ?? "N/A";
            second.Text = najnovijiPodaci?.second.ToString() ?? "N/A";



        }

        private void minute_Click(object sender, EventArgs e)
        {

        }

        private void toolStripContainer1_ContentPanel_Load(object sender, EventArgs e)
        {

        }

        private void gMapControl1_Load_1(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = website_url.Text; 

            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
               
            }


        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = whois_server.Text;

            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            gMapControl1.Zoom += 1;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            gMapControl1.Zoom -= 1;
        }
    }
}



