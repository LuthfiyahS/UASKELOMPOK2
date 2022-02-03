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
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
                submenu1();
            submenu2();
        }
        private void submenu1()
        {
            panelMaster.Visible = false;
        }
        private void hide()
        {
            if (panelMaster.Visible == true)
                panelMaster.Visible = false;
        }

        private void show(Panel sub)
        {
            if (sub.Visible == false)
            {
                hide();
                sub.Visible = true;
            }
            else
                sub.Visible = false;
        }

        private Form activeForm = null;
        private void tampilan(Form tampil)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = tampil;
            tampil.TopLevel = false;
            tampil.FormBorderStyle = FormBorderStyle.None;
            tampil.Dock = DockStyle.Fill;
            panelHsl.Controls.Add(tampil);
            panelHsl.Tag = tampil;
            tampil.BringToFront();
            tampil.Show();
        }

        private void btnMaster_Click(object sender, EventArgs e)
        {
            show(panelMaster);
        }
        private void submenu2()
        {
            panelPendukung.Visible = false;
        }
        private void hide2()
        {
            if (panelPendukung.Visible == true)
                panelPendukung.Visible = false;
        }

        private void show2(Panel sub)
        {
            if (sub.Visible == false)
            {
                hide2();
                sub.Visible = true;
            }
            else
                sub.Visible = false;
        }

        private void btnKuliah_Click(object sender, EventArgs e)
        {
            show2(panelPendukung);
        }


        void Display()
        {
            try
            {

                using (SqlConnection SqlConnect = new SqlConnection(Koneksi.Connect))
                {
                    SqlConnect.Open();
                    SqlDataAdapter sqlDisplay = new SqlDataAdapter("EXEC spDataDashboard", SqlConnect);
                    sqlDisplay.SelectCommand.ExecuteNonQuery();

                    DataTable data = new DataTable();
                    sqlDisplay.Fill(data);

                    dgvTransaksiPenjualan.DataSource = data;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            lblUser.Text = Login.user;
            Display();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Login log = new Login();
            log.Show();
            this.Hide();
        }
        void Search()
        {
            try
            {
                using (SqlConnection SqlConnect = new SqlConnection(Koneksi.Connect))
                {
                    SqlConnect.Open();
                    SqlDataAdapter carisemua = new SqlDataAdapter("EXEC spCariData4tabel @CARI", SqlConnect);
                    carisemua.SelectCommand.Parameters.AddWithValue("@CARI", txtCari.Text.Trim());
                    carisemua.SelectCommand.ExecuteNonQuery();

                    DataTable data = new DataTable();
                    carisemua.Fill(data);

                    dgvTransaksiPenjualan.DataSource = data;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtCari_TextChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtCari.Clear();
            Display();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            tampilan(new Dashboard());
        }


        private void btnHari_Click(object sender, EventArgs e)
        {
            tampilan(new DataHari());
        }


        private void btnRuangan_Click(object sender, EventArgs e)
        {
            tampilan(new DataRuangKelas());
        }

        private void btnMK_Click(object sender, EventArgs e)
        {
            tampilan(new DataMataKuliah());
        }

        private void btnDosen_Click(object sender, EventArgs e)
        {
            tampilan(new DataDosen());
        }

        private void btnProdi_Click(object sender, EventArgs e)
        {
            tampilan(new DataProdi());
        }

        private void btnMaster_Click_1(object sender, EventArgs e)
        {
            show(panelMaster);
        }

        private void btnKuliah_Click_1(object sender, EventArgs e)
        {
            show(panelPendukung);
        }

        private void btnUser_Click_1(object sender, EventArgs e)
        {
            tampilan(new DataUser());
        }

        private void btnTA_Click_1(object sender, EventArgs e)
        {
            tampilan(new DataTahunAkademik());
        }

        private void btnLogout_Click_1(object sender, EventArgs e)
        {
            Login log = new Login();
            log.Show();
            this.Hide();
        }

        private void btnJadwal_Click(object sender, EventArgs e)
        {
            tampilan(new DataJadwalKuliah());
        }

        private void btnRuangan_Click_1(object sender, EventArgs e)
        {
            tampilan(new DataRuangKelas());
        }

        private void Dashboard_Load_1(object sender, EventArgs e)
        {
            tampilan(new Dashboard());
        }
    }
}
