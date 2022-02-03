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
    public partial class DataMataKuliah : Form
    {
        private SqlCommand cmd;
        public DataMataKuliah()
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
                    SqlDataAdapter sqlDisplay = new SqlDataAdapter("EXEC spDataMK", SqlConnect);
                    sqlDisplay.SelectCommand.ExecuteNonQuery();

                    DataTable data = new DataTable();
                    sqlDisplay.Fill(data);

                    dgvMK.DataSource = data;
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
                    SqlDataAdapter GetUser = new SqlDataAdapter("EXEC spCariDataMK @CARI", SqlConnect);
                    GetUser.SelectCommand.Parameters.AddWithValue("@CARI", txtCari.Text.Trim());
                    GetUser.SelectCommand.ExecuteNonQuery();

                    DataTable data = new DataTable();
                    GetUser.Fill(data);

                    dgvMK.DataSource = data;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void ClearData()
        {
            txtID.Clear();
            txtNama.Clear();
            txtSks.Clear();
            txtSmt.Clear();
        }
        private void DataMataKuliah_Load(object sender, EventArgs e)
        {
            Display();
            ClearData();
        }

        private void txtCari_TextChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtID.Text.Trim() == "" || txtNama.Text.Trim() == "" || txtSks.Text.Trim() == "" ||
                txtSmt.Text.Trim() == "" )
                {
                    MessageBox.Show("Data tidak boleh dikosongkan");
                }
                else
                {
                    using (SqlConnection SqlConnectSimpan = new SqlConnection(Koneksi.Connect))
                    {
                        try
                        {
                            SqlConnectSimpan.Open();
                            SqlDataAdapter Insert = new SqlDataAdapter("EXEC spInputMK @ID, @NAMA, @SKS, @SMT", SqlConnectSimpan);
                            Insert.SelectCommand.Parameters.AddWithValue("@ID", txtID.Text.Trim());
                            Insert.SelectCommand.Parameters.AddWithValue("@NAMA", txtNama.Text.Trim());
                            Insert.SelectCommand.Parameters.AddWithValue("@SKS", txtSks.Text.Trim());
                            Insert.SelectCommand.Parameters.AddWithValue("@SMT", txtSmt.Text.Trim());
                            Insert.SelectCommand.ExecuteNonQuery();

                            MessageBox.Show("Data Tersimpan");
                            ClearData();
                            Display();

                        }
                        catch (Exception x)
                        {
                            MessageBox.Show("Jumlah SKS harus menggunakan angka! ");
                            ClearData();
                            Display();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvMK_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in dgvMK.SelectedRows)
                {
                    txtID.Text = row.Cells[0].Value.ToString();
                    txtNama.Text = row.Cells[1].Value.ToString();
                    txtSks.Text = row.Cells[2].Value.ToString();
                    txtSmt.Text = row.Cells[3].Value.ToString();
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
                            SqlCommand update = new SqlCommand("EXEC spUpdateDosen @ID, @NAMA,  @SKS, @SMT", IdSqlConnectEdit);
                            update.Parameters.AddWithValue("@ID", txtID.Text.Trim());
                            update.Parameters.AddWithValue("@NAMA", txtNama.Text.Trim());
                            update.Parameters.AddWithValue("@SKS", txtSks.Text.Trim());
                            update.Parameters.AddWithValue("@SMT", txtSmt.Text.Trim());
                            update.ExecuteNonQuery();

                            MessageBox.Show("Data " + txtNama.Text + "  Terupdate");
                            ClearData();
                            Display();
                        }
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show("Jumlah SKS harus menggunakan angka! ");
                        ClearData();
                        Display();
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
                        SqlCommand hapus = new SqlCommand("EXEC spHapusMK @ID ", IdSqlConnectHps);
                        hapus.Parameters.AddWithValue("@ID", txtID.Text);
                        hapus.ExecuteNonQuery();

                        MessageBox.Show("Data " + txtNama.Text + "  Terhapus");
                        ClearData();
                        Display();
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
            ClearData();
            Display();
        }
    }
}
