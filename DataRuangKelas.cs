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
    public partial class DataRuangKelas : Form
    {
        private SqlCommand cmd;
        public DataRuangKelas()
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
                    SqlDataAdapter sqlDisplay = new SqlDataAdapter("EXEC spDataRuangan", SqlConnect);
                    sqlDisplay.SelectCommand.ExecuteNonQuery();

                    DataTable data = new DataTable();
                    sqlDisplay.Fill(data);

                    dgvRuangan.DataSource = data;
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
                    SqlDataAdapter GetCari = new SqlDataAdapter("EXEC spCariDataRuangan @CARI", SqlConnect);
                    GetCari.SelectCommand.Parameters.AddWithValue("@CARI", txtCari.Text.Trim());
                    GetCari.SelectCommand.ExecuteNonQuery();

                    DataTable data = new DataTable();
                    GetCari.Fill(data);

                    dgvRuangan.DataSource = data;
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
                cmd = new SqlCommand("EXECUTE spIdRuangan", IdSqlConnect);
                dr = cmd.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    itung = Convert.ToInt64(dr[0].ToString().Substring(dr["kd_ruangan"].ToString().Length - 3, 3)) + 1;
                    string idurut = "000" + itung;
                    urut = "R" + idurut.Substring(idurut.Length - 3, 3);
                }
                else
                {
                    urut = "R001";
                }
                dr.Close();
                txtID.Text = urut;
            }

        }

        void ClearData()
        {
            txtID.Clear();
            txtNama.Clear();
            txtKapasitas.Clear();
            txtKet.Clear();
        }

        private void DataRuangKelas_Load(object sender, EventArgs e)
        {
            Display();
            ClearData();
            IdOtomatis();
        }

        private void txtCari_TextChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtID.Text.Trim() == "" || txtNama.Text.Trim() == "")
                {
                    MessageBox.Show("Data tidak boleh dikosongkan");
                }
                else
                {
                    try
                    {
                        using (SqlConnection SqlConnectSimpan = new SqlConnection(Koneksi.Connect))
                        {
                            SqlConnectSimpan.Open();
                            SqlDataAdapter Insert = new SqlDataAdapter("EXEC spInputRuangan @ID, @NAMA, @KAP, @KET", SqlConnectSimpan);
                            Insert.SelectCommand.Parameters.AddWithValue("@ID", txtID.Text.Trim());
                            Insert.SelectCommand.Parameters.AddWithValue("@NAMA", txtNama.Text.Trim());
                            Insert.SelectCommand.Parameters.AddWithValue("@KAP", txtKapasitas.Text.Trim());
                            Insert.SelectCommand.Parameters.AddWithValue("@KET", txtKet.Text.Trim());
                            Insert.SelectCommand.ExecuteNonQuery();

                            MessageBox.Show("Data Tersimpan");
                            ClearData();
                            Display();
                            IdOtomatis();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Data Harus Kapasitas dengan Angka!");
                    }
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvRuangan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in dgvRuangan.SelectedRows)
                {
                    txtID.Text = row.Cells[0].Value.ToString();
                    txtNama.Text = row.Cells[1].Value.ToString();
                    txtKapasitas.Text = row.Cells[2].Value.ToString();
                    txtKet.Text = row.Cells[3].Value.ToString();
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.ToString());
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection IdSqlConnectEdit = new SqlConnection(Koneksi.Connect))
                {
                    try
                    {
                        IdSqlConnectEdit.Open();
                        DialogResult dr = MessageBox.Show("Anda yakin ingin mengubah data " + txtID.Text + " ?",
                            "Informasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dr == DialogResult.Yes)
                        {
                            SqlCommand update = new SqlCommand("EXEC spUpdateRuangan @ID, @NAMA , @KAP, @KET", IdSqlConnectEdit);
                            update.Parameters.AddWithValue("@ID", txtID.Text.Trim());
                            update.Parameters.AddWithValue("@NAMA", txtNama.Text.Trim());
                            update.Parameters.AddWithValue("@KAP", txtKapasitas.Text.Trim());
                            update.Parameters.AddWithValue("@KET", txtKet.Text.Trim());
                            update.ExecuteNonQuery();

                            MessageBox.Show("Data " + txtNama.Text + "  Terupdate");
                            ClearData();
                            Display();
                            IdOtomatis();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Data Harus Kapasitas dengan Angka!");
                    }
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.ToString());
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection IdSqlConnectHps = new SqlConnection(Koneksi.Connect))
                {
                    IdSqlConnectHps.Open();
                    DialogResult dr = MessageBox.Show("Anda yakin ingin menghapus data " + txtNama.Text + " ?",
                        "Informasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        SqlCommand hapus = new SqlCommand("EXEC spHapusRuangan @ID ", IdSqlConnectHps);
                        hapus.Parameters.AddWithValue("@ID", txtID.Text);
                        hapus.ExecuteNonQuery();

                        MessageBox.Show("Data " + txtNama.Text + "  Terhapus");
                        ClearData();
                        Display();
                        IdOtomatis();
                    }
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.ToString());
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Display();
            ClearData();
            IdOtomatis();
        }
    }
}
