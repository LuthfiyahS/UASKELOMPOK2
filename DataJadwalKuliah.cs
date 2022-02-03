using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace SIPMK
{
    
    public partial class DataJadwalKuliah : Form
    {
        private SqlCommand cmd;
        private SqlDataReader dr;
        public DataJadwalKuliah()
        {
            InitializeComponent();
        }

        void Display()
        {
            try
            {
                using (SqlConnection SqlConnect = new SqlConnection(Koneksi.Connect))
                {
                    SqlConnect.Open();
                    SqlDataAdapter sqlDisplay = new SqlDataAdapter("EXEC spDataProdi", SqlConnect);
                    sqlDisplay.SelectCommand.ExecuteNonQuery();

                    DataTable data = new DataTable();
                    sqlDisplay.Fill(data);

                    dgvJadwal.DataSource = data;
                    comboHari();
                    comboMK();
                    comboDosen();
                    comboRuangan();
                    comboProdi();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Search()
        {
            try
            {
                using (SqlConnection SqlConnect = new SqlConnection(Koneksi.Connect))
                {
                    SqlConnect.Open();
                    SqlDataAdapter GetCari = new SqlDataAdapter("EXEC spCariDataProdi @CARI", SqlConnect);
                    GetCari.SelectCommand.Parameters.AddWithValue("@CARI", txtCari.Text.Trim());
                    GetCari.SelectCommand.ExecuteNonQuery();

                    DataTable data = new DataTable();
                    GetCari.Fill(data);

                    dgvJadwal.DataSource = data;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void IdOtomatis()
        {
            long itung;
            string urut;
            SqlDataReader dr;
            using (SqlConnection IdSqlConnect = new SqlConnection(Koneksi.Connect))
            {
                IdSqlConnect.Open();
                cmd = new SqlCommand("EXECUTE spIdProdi", IdSqlConnect);
                dr = cmd.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    itung = Convert.ToInt64(dr[0].ToString().Substring(dr["kd_prodi"].ToString().Length - 3, 3)) + 1;
                    string idurut = "000" + itung;
                    urut = "PRD" + idurut.Substring(idurut.Length - 3, 3);
                }
                else
                {
                    urut = "PRD001";
                }
                dr.Close();
                txtID.Text = urut;
            }

        }

        void comboHari()
        {
            using (SqlConnection IdSqlConnect = new SqlConnection(Koneksi.Connect))
            {
                IdSqlConnect.Open();
                cmd = new SqlCommand("EXEC spAmbilIDHari", IdSqlConnect);
                dr = cmd.ExecuteReader();
                dr.Read();
                while (dr.Read())
                {
                    cbHari.Items.Add(dr[0].ToString());
                }
            }
        }

        void comboMK()
        {
            using (SqlConnection IdSqlConnect = new SqlConnection(Koneksi.Connect))
            {
                IdSqlConnect.Open();
                cmd = new SqlCommand("EXEC spAmbilIDMK", IdSqlConnect);
                dr = cmd.ExecuteReader();
                dr.Read();
                while (dr.Read())
                {
                    cbJam.Items.Add(dr[0].ToString());
                }
            }
        }

        void comboDosen()
        {
            using (SqlConnection IdSqlConnect = new SqlConnection(Koneksi.Connect))
            {
                IdSqlConnect.Open();
                cmd = new SqlCommand("EXEC spAmbilIDDosen", IdSqlConnect);
                dr = cmd.ExecuteReader();
                dr.Read();
                while (dr.Read())
                {
                    cbJam.Items.Add(dr[0].ToString());
                }
            }
        }

        void comboRuangan()
        {
            using (SqlConnection IdSqlConnect = new SqlConnection(Koneksi.Connect))
            {
                IdSqlConnect.Open();
                cmd = new SqlCommand("EXEC spAmbilIDRuangan", IdSqlConnect);
                dr = cmd.ExecuteReader();
                dr.Read();
                while (dr.Read())
                {
                    cbJam.Items.Add(dr[0].ToString());
                }
            }
        }

        void comboProdi()
        {
            using (SqlConnection IdSqlConnect = new SqlConnection(Koneksi.Connect))
            {
                IdSqlConnect.Open();
                cmd = new SqlCommand("EXEC spAmbilIDProdi", IdSqlConnect);
                dr = cmd.ExecuteReader();
                dr.Read();
                while (dr.Read())
                {
                    cbJam.Items.Add(dr[0].ToString());
                }
            }
        }
        void ClearData()
        {
            txtID.Clear();
        }
        private void DataJadwalKuliah_Load(object sender, EventArgs e)
        {
            ClearData();
            IdOtomatis();
            Display();
        }
    }
}
