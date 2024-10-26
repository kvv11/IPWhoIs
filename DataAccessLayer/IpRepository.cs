using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;

namespace DataAccessLayer
{
    public class IpsRepository
    {
        private readonly string connectionString;

        public IpsRepository(IConfiguration configuration)
        {

            connectionString = configuration.GetConnectionString("MyDatabase");
        }

        public async Task ImportIPP(string ipAddress)
        {
            try
            {
                if (!Provjera1(ipAddress))
                {
                    MessageBox.Show("Greška: Unesena adresa mora biti u formatu IP adrese.");
                    return;
                }

                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = $"http://ipwho.is/{ipAddress}";
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResult = await response.Content.ReadAsStringAsync();
                        Ip ipData = Newtonsoft.Json.JsonConvert.DeserializeObject<Ip>(jsonResult);

                        if (!Provjera(ipData.ip))
                        {
                            MessageBox.Show($"Podaci su uspješno uneseni!");
                            SaveToDatabase(ipData);
                            ImportRec(ipData.ip, ipData.connection.domain);
                            ImportVrijeme(ipData.ip, ipData.city);
                        }
                        else
                        {
                            MessageBox.Show("Greška u upisu: Unesena IP adresa već postoji u bazi.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Došlo je do greške prilikom dobijanja podataka o IP adresi.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"GRESKAAAAAA: {ex.Message}");
            }
        }

        private bool Provjera1(string ipAddress)
        {
            string ipAddressPattern = @"^(\d{1,3}\.){3}\d{1,3}$";
            return System.Text.RegularExpressions.Regex.IsMatch(ipAddress, ipAddressPattern);
        }

        private bool Provjera(string ipAddress)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM Veletic_Ip WHERE ip = @ip";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ip", ipAddress);
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        private void SaveToDatabase(Ip ipData)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Veletic_Ip (ip, type, continent,continent_code,country,country_code,region,region_code,city,latitude,longitude,postal,calling_code,capital,borders,domain,org,isp,id,utc) VALUES (@ip, @type, @continent,@continent_code,@country,@country_code,@region,@region_code,@city,@latitude,@longitude,@postal,@calling_code,@capital,@borders,@domain,@org,@isp,@id,@utc)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlParameter ipParam = command.Parameters.AddWithValue("@ip", ipData.ip ?? $"Nepoznato_{DateTime.Now.Ticks}");
                        ipParam.SqlDbType = SqlDbType.NVarChar;
                        ipParam.Size = 50;

                        SqlParameter typeParam = command.Parameters.AddWithValue("@type", ipData.type ?? $"Nepoznato_{DateTime.Now.Ticks}");
                        typeParam.SqlDbType = SqlDbType.NVarChar;
                        typeParam.Size = 50;

                        SqlParameter continentParam = command.Parameters.AddWithValue("@continent", ipData.continent ?? $"Nepoznato_{DateTime.Now.Ticks}");
                        continentParam.SqlDbType = SqlDbType.NVarChar;
                        continentParam.Size = 50;

                        SqlParameter continent_codeParam = command.Parameters.AddWithValue("@continent_code", ipData.continent_code ?? $"Nepoznato_{DateTime.Now.Ticks}");
                        continent_codeParam.SqlDbType = SqlDbType.NVarChar;
                        continent_codeParam.Size = 50;

                        SqlParameter countryParam = command.Parameters.AddWithValue("@country", ipData.country ?? DBNull.Value.ToString());
                        countryParam.SqlDbType = SqlDbType.NVarChar;
                        countryParam.Size = 50;

                        SqlParameter country_codeParam = command.Parameters.AddWithValue("@country_code", ipData.country_code ?? $"Nepoznato_{DateTime.Now.Ticks}");
                        country_codeParam.SqlDbType = SqlDbType.NVarChar;
                        country_codeParam.Size = 50;

                        SqlParameter regionParam = command.Parameters.AddWithValue("@region", ipData.region ?? DBNull.Value.ToString());
                        regionParam.SqlDbType = SqlDbType.NVarChar;
                        regionParam.Size = 50;

                        SqlParameter region_codeParam = command.Parameters.AddWithValue("@region_code", ipData.region_code ?? $"Nepoznato_{DateTime.Now.Ticks}");
                        region_codeParam.SqlDbType = SqlDbType.NVarChar;
                        region_codeParam.Size = 50;

                        SqlParameter cityParam = command.Parameters.AddWithValue("@city", ipData.city ?? $"Nepoznato_{DateTime.Now.Ticks}");
                        cityParam.SqlDbType = SqlDbType.NVarChar;
                        cityParam.Size = 50;

                        SqlParameter latitudeParam = command.Parameters.AddWithValue("@latitude", ipData.latitude);
                        latitudeParam.SqlDbType = SqlDbType.Float;
                        latitudeParam.Size = 50;

                        SqlParameter longitudeParam = command.Parameters.AddWithValue("@longitude", ipData.longitude);
                        longitudeParam.SqlDbType = SqlDbType.Float;
                        longitudeParam.Size = 50;

                        SqlParameter postalParam = command.Parameters.AddWithValue("@postal", ipData.postal ?? $"Nepoznato_{DateTime.Now.Ticks}");
                        postalParam.SqlDbType = SqlDbType.NVarChar;
                        postalParam.Size = 50;

                        SqlParameter calling_codeParam = command.Parameters.AddWithValue("@calling_code", ipData.calling_code ?? $"Nepoznato_{DateTime.Now.Ticks}");
                        calling_codeParam.SqlDbType = SqlDbType.NVarChar;
                        calling_codeParam.Size = 50;

                        SqlParameter capitalParam = command.Parameters.AddWithValue("@capital", ipData.capital ?? DBNull.Value.ToString());
                        capitalParam.SqlDbType = SqlDbType.NVarChar;
                        capitalParam.Size = 50;

                        SqlParameter bordersParam = command.Parameters.AddWithValue("@borders", ipData.borders ?? DBNull.Value.ToString());
                        bordersParam.SqlDbType = SqlDbType.NVarChar;
                        bordersParam.Size = 50;

                        string domainValue = ipData.connection?.domain ?? DBNull.Value.ToString();
                        string orgValue = ipData.connection?.org ?? DBNull.Value.ToString();
                        string ispValue = ipData.connection?.isp ?? DBNull.Value.ToString();

                        SqlParameter domainParam = command.Parameters.AddWithValue("@domain", domainValue);
                        domainParam.SqlDbType = SqlDbType.NVarChar;
                        domainParam.Size = 50;

                        SqlParameter orgParam = command.Parameters.AddWithValue("@org", orgValue);
                        orgParam.SqlDbType = SqlDbType.NVarChar;
                        orgParam.Size = 50;

                        SqlParameter ispParam = command.Parameters.AddWithValue("@isp", ispValue);
                        ispParam.SqlDbType = SqlDbType.NVarChar;
                        ispParam.Size = 50;

                        string idValue = ipData.timezone?.id ?? DBNull.Value.ToString();
                        SqlParameter idParam = command.Parameters.AddWithValue("@id", idValue);
                        idParam.SqlDbType = SqlDbType.NVarChar;
                        idParam.Size = 50;

                        string utcValue = ipData.timezone?.utc ?? DBNull.Value.ToString();
                        SqlParameter utcParam = command.Parameters.AddWithValue("@utc", utcValue);
                        utcParam.SqlDbType = SqlDbType.NVarChar;
                        utcParam.Size = 50;

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($" NE RADI - {ex.Message}\nStackTrace: {ex.StackTrace}");
            }
        }

        // Azuriranje #1
        public void Azurirajgumb(string ip, string domain, string type, string continent, string continent_code, string country, string country_code,
            string region, string region_code, string postal, string calling_code, string id, string capital, string borders, double latitude, double longitude, string utc, string city)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string updateQuery = $"UPDATE Veletic_Ip SET " +
                                     "domain = @domain, " +
                                     "type = @type, " +
                                     "continent = @continent, " +
                                     "continent_code = @continent_code, " +
                                     "country = @country, " +
                                     "country_code = @country_code, " +
                                     "region = @region, " +
                                     "region_code = @region_code, " +
                                     "postal = @postal, " +
                                     "calling_code = @calling_code, " +
                                     "id = @id, " +
                                     "capital = @capital, " +
                                     "borders = @borders, " +
                                     "latitude = @latitude, " +
                                     "longitude = @longitude, " +
                                     "utc = @utc, " +
                                     "city = @city " +
                                     "WHERE ip = @ip";

                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@domain", domain);
                    command.Parameters.AddWithValue("@type", type);
                    command.Parameters.AddWithValue("@continent", continent);
                    command.Parameters.AddWithValue("@continent_code", continent_code);
                    command.Parameters.AddWithValue("@country", country);
                    command.Parameters.AddWithValue("@country_code", country_code);
                    command.Parameters.AddWithValue("@region", region);
                    command.Parameters.AddWithValue("@region_code", region_code);
                    command.Parameters.AddWithValue("@postal", postal);
                    command.Parameters.AddWithValue("@calling_code", calling_code);
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@capital", capital);
                    command.Parameters.AddWithValue("@borders", borders);
                    command.Parameters.AddWithValue("@latitude", latitude);
                    command.Parameters.AddWithValue("@longitude", longitude);
                    command.Parameters.AddWithValue("@utc", utc);
                    command.Parameters.AddWithValue("@city", city);
                    command.Parameters.AddWithValue("@ip", ip);

                    try
                    {
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Uspješno ste uredili IP informacije");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"greska - {ex.Message}", "greska - ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                connection.Close();
            }
        }

        // Azuriranje #2
        public void AzurirajGumb2(string ip, string registrar_name, string whois_server, string website_url, string domain_registered, string create_date, string update_date, string expiry_date)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string updateQuery = $"UPDATE Veletic_Whois SET " +
                                     "registrar_name = @registrar_name, " +
                                     "whois_server = @whois_server, " +
                                     "website_url = @website_url, " +
                                     "domain_registered = @domain_registered, " +
                                     "create_date = @create_date, " +
                                     "update_date = @update_date, " +
                                     "expiry_date = @expiry_date " +
                                     "WHERE ip = @ip";

                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@registrar_name", registrar_name);
                    command.Parameters.AddWithValue("@whois_server", whois_server);
                    command.Parameters.AddWithValue("@website_url", website_url);
                    command.Parameters.AddWithValue("@domain_registered", domain_registered);
                    command.Parameters.AddWithValue("@create_date", create_date);
                    command.Parameters.AddWithValue("@update_date", update_date);
                    command.Parameters.AddWithValue("@expiry_date", expiry_date);
                    command.Parameters.AddWithValue("@ip", ip);
                }

                connection.Close();
            }
        }

        // Brisanje 
        public void Obrisiip(string ip)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    string deleteQuery = "DELETE FROM Veletic_Ip WHERE ip = @ip";
                    string deleteQuery1 = "DELETE FROM Veletic_Datetime WHERE ip = @ip";
                    string deleteQuery2 = "DELETE FROM Veletic_Whois WHERE ip = @ip";

                    using (SqlCommand command = new SqlCommand(deleteQuery, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@ip", ip);

                        int affectedRows = command.ExecuteNonQuery();

                        if (affectedRows > 0)
                        {
                            using (SqlCommand command1 = new SqlCommand(deleteQuery1, connection, transaction))
                            {
                                command1.Parameters.AddWithValue("@ip", ip);
                                command1.ExecuteNonQuery();
                            }

                            using (SqlCommand command2 = new SqlCommand(deleteQuery2, connection, transaction))
                            {
                                command2.Parameters.AddWithValue("@ip", ip);
                                command2.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            MessageBox.Show("Uspješno ste obrisali IP adresu");
                        }
                        else
                        {
                            transaction.Rollback();
                            MessageBox.Show("neeeeradiiiiiiiiii");
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show($"greska - {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        // Import vrijeme - Form Unos - prvi put
        public async Task ImportVrijeme(string ipAddress, string city)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiKey = "iTAcVcwfhDJhhc0LXQGVjg==qVm9oPJcP9r2b8K1";
                    string apiUrl = $"https://api.api-ninjas.com/v1/worldtime?city={city}";

                    client.DefaultRequestHeaders.Add("X-Api-Key", apiKey);

                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResult = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(jsonResult);

                        dynamic jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonResult);

                        Spremi(jsonData, ipAddress);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        // Spremanje vremena - Form Unos - Prvi put
        public async Task Spremi(dynamic jsonData, string ipp)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Veletic_Datetime (ip, date, year, month, day, hour, minute, second, day_of_week)" +
                                   " VALUES (@ip, @date, @year, @month, @day, @hour, @minute, @second, @day_of_week)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlParameter ipParam = command.Parameters.AddWithValue("@ip", ipp ?? $"Nepoznato_{DateTime.Now.Ticks}");
                        ipParam.SqlDbType = SqlDbType.NVarChar;
                        ipParam.Size = 50;

                        SqlParameter dateParam = command.Parameters.AddWithValue("@date", jsonData.date ?? DBNull.Value);
                        dateParam.SqlDbType = SqlDbType.NVarChar;
                        dateParam.Size = 50;

                        SqlParameter yearParam = command.Parameters.AddWithValue("@year", jsonData.year ?? DBNull.Value);
                        yearParam.SqlDbType = SqlDbType.NVarChar;
                        yearParam.Size = 50;

                        SqlParameter monthParam = command.Parameters.AddWithValue("@month", jsonData.month ?? DBNull.Value);
                        monthParam.SqlDbType = SqlDbType.NVarChar;
                        monthParam.Size = 50;

                        SqlParameter dayParam = command.Parameters.AddWithValue("@day", jsonData.day ?? DBNull.Value);
                        dayParam.SqlDbType = SqlDbType.NVarChar;
                        dayParam.Size = 50;

                        SqlParameter hourParam = command.Parameters.AddWithValue("@hour", jsonData.hour ?? DBNull.Value);
                        hourParam.SqlDbType = SqlDbType.NVarChar;
                        hourParam.Size = 50;

                        SqlParameter minuteParam = command.Parameters.AddWithValue("@minute", jsonData.minute ?? DBNull.Value);
                        minuteParam.SqlDbType = SqlDbType.NVarChar;
                        minuteParam.Size = 50;

                        SqlParameter secondParam = command.Parameters.AddWithValue("@second", jsonData.second ?? DBNull.Value);
                        secondParam.SqlDbType = SqlDbType.NVarChar;
                        secondParam.Size = 50;

                        SqlParameter day_of_weekParam = command.Parameters.AddWithValue("@day_of_week", jsonData.day_of_week ?? DBNull.Value);
                        day_of_weekParam.SqlDbType = SqlDbType.NVarChar;
                        day_of_weekParam.Size = 50;

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        // Import Vrijeme - Form Detalj - Drugi put
        public async Task ImportVrijeme2(string ipAddress, string city)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiKey = "iTAcVcwfhDJhhc0LXQGVjg==qVm9oPJcP9r2b8K1";
                    string apiUrl = $"https://api.api-ninjas.com/v1/worldtime?city={city}";

                    client.DefaultRequestHeaders.Add("X-Api-Key", apiKey);

                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResult = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(jsonResult);

                        dynamic jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonResult);

                        AzurVrijeme(jsonData, ipAddress);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        // Azuriranje vremena - Form Detalj - drugi put
        public async Task AzurVrijeme(dynamic jsonData, string ipp)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Veletic_Datetime SET date = @date, year = @year, month = @month, " +
                                   "day = @day, hour = @hour, minute = @minute, second = @second, day_of_week = @day_of_week " +
                                   "WHERE ip = @ip";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ip", ipp ?? $"Nepoznato_{DateTime.Now.Ticks}");
                        command.Parameters.AddWithValue("@date", ConvertJValue(jsonData.date));
                        command.Parameters.AddWithValue("@year", ConvertJValue(jsonData.year));
                        command.Parameters.AddWithValue("@month", ConvertJValue(jsonData.month));
                        command.Parameters.AddWithValue("@day", ConvertJValue(jsonData.day));
                        command.Parameters.AddWithValue("@hour", ConvertJValue(jsonData.hour));
                        command.Parameters.AddWithValue("@minute", ConvertJValue(jsonData.minute));
                        command.Parameters.AddWithValue("@second", ConvertJValue(jsonData.second));
                        command.Parameters.AddWithValue("@day_of_week", ConvertJValue(jsonData.day_of_week));

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        // klinac za konvertat 
        private object ConvertJValue(JValue jValue)
        {
            return jValue?.ToObject<object>() ?? DBNull.Value;
        }

        // Prikaz za REFRESH - Form Detalji
        public Datetime NoviPodaci(string ip)
        {
            string selectQuery = "SELECT * FROM Veletic_Datetime WHERE ip = @ip";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@ip", ip);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Datetime
                            {
                                hour = Convert.ToString(reader["hour"]),
                                year = Convert.ToString(reader["year"]),
                                month = Convert.ToString(reader["month"]),
                                day = Convert.ToString(reader["day"]),
                                day_of_week = Convert.ToString(reader["day_of_week"]),
                                minute = Convert.ToString(reader["minute"]),
                                second = Convert.ToString(reader["second"]),
                                date = Convert.ToString(reader["date"]),
                            };
                        }
                    }
                }
            }
            return null;
        }

        // Importanje domain informacija
        public async Task ImportRec(string ipAddress, string domain)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = $"https://api.whoisfreaks.com/v1.0/whois?apiKey=c28aae8bb65d4e9c80a00369db16f3fb&whois=live&domainName={domain}";

                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResult = await response.Content.ReadAsStringAsync();
                        Domain jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject<Domain>(jsonResult);

                        await SpremiRec(jsonData, ipAddress);
                    }
                    else
                    {
                        MessageBox.Show("Informacije o domeni ne postoje!");
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        // Spremanje domain informacija
        public async Task SpremiRec(Domain jsonData, string ipAddress)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO Veletic_Whois (ip, registrar_name, website_url, domain_registered, whois_server, create_date, update_date, expiry_date)" +
                                   " VALUES (@ip, @registrar_name, @website_url, @domain_registered, @whois_server, @create_date, @update_date, @expiry_date)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlParameter ipParam = command.Parameters.AddWithValue("@ip", ipAddress ?? $"Nepoznato_{DateTime.Now.Ticks}");
                        ipParam.SqlDbType = SqlDbType.NVarChar;
                        ipParam.Size = 50;

                        SqlParameter registrar_nameParam = command.Parameters.AddWithValue("@registrar_name", jsonData.domain_registrar?.registrar_name ?? (object)DBNull.Value);
                        registrar_nameParam.SqlDbType = SqlDbType.NVarChar;
                        registrar_nameParam.Size = 50;

                        SqlParameter website_urlParam = command.Parameters.AddWithValue("@website_url", jsonData.domain_registrar?.website_url ?? (object)DBNull.Value);
                        website_urlParam.SqlDbType = SqlDbType.NVarChar;
                        website_urlParam.Size = 255;

                        SqlParameter domain_registeredParam = command.Parameters.AddWithValue("@domain_registered", jsonData.domain_registered ?? (object)DBNull.Value);
                        domain_registeredParam.SqlDbType = SqlDbType.NVarChar;
                        domain_registeredParam.Size = 255;

                        SqlParameter whois_serverParam = command.Parameters.AddWithValue("@whois_server", jsonData.whois_server ?? (object)DBNull.Value);
                        whois_serverParam.SqlDbType = SqlDbType.NVarChar;
                        whois_serverParam.Size = 255;

                        SqlParameter create_dateParam = command.Parameters.AddWithValue("@create_date", jsonData.create_date != DateTime.MinValue ? jsonData.create_date.ToString("yyyy-MM-dd HH:mm:ss") : (object)DBNull.Value);
                        create_dateParam.SqlDbType = SqlDbType.NVarChar;
                        create_dateParam.Size = 50;

                        SqlParameter update_dateParam = command.Parameters.AddWithValue("@update_date", jsonData.update_date != DateTime.MinValue ? jsonData.update_date.ToString("yyyy-MM-dd HH:mm:ss") : (object)DBNull.Value);
                        update_dateParam.SqlDbType = SqlDbType.NVarChar;
                        update_dateParam.Size = 50;

                        SqlParameter expiry_dateParam = command.Parameters.AddWithValue("@expiry_date", jsonData.expiry_date != DateTime.MinValue ? jsonData.expiry_date.ToString("yyyy-MM-dd HH:mm:ss") : (object)DBNull.Value);
                        expiry_dateParam.SqlDbType = SqlDbType.NVarChar;
                        expiry_dateParam.Size = 50;

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Došlo je do pogreške nekih vrijednosti priikom importa Whois informacija");
            }
        }
    }
}
