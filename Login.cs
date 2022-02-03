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
    public partial class Login : Form
    {
        int counter;
        public static string user;
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            user = txtUsername.Text;
            try
            {
                using (SqlConnection SqlConnect = new
               SqlConnection(Koneksi.Connect))
                {
                    SqlConnect.Open();
                    if (txtUsername.Text == "" || txtPassword.Text == "")
                    {
                        MessageBox.Show("Login Gagal! Data yang  diinputkan tidak lengkap.", "Alert", MessageBoxButtons.OK, 
                       MessageBoxIcon.Warning);
                    }
                    else
                    {
                        SqlDataAdapter GetUser = new SqlDataAdapter("EXEC spLogin @USERNAME, @PASSWD", SqlConnect);

                        GetUser.SelectCommand.Parameters.AddWithValue("@USERNAME", txtUsername.Text.Trim());
                        GetUser.SelectCommand.Parameters.AddWithValue("@PASSWD", txtPassword.Text.Trim());
                        GetUser.SelectCommand.ExecuteNonQuery();
                        DataTable dt = new DataTable();
                        GetUser.Fill(dt);
                        counter += 1;
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (dr["role"].ToString() == "Administrator")
                                {
                                    MessageBox.Show("Login  Sukses! Selamat Datang Administrator " + dr["name"].ToString());
                                   
                                   Dashboard admin = new
                                   Dashboard();
                                    admin.Show();
                                    this.Hide();
                                    SqlConnect.Close();
                                }
                                else if
                               (dr["role"].ToString() == "Kasir")
                                {
                                    MessageBox.Show("Login  Sukses! Selamat Datang Kasir " + dr["name"].ToString());
                                   
                                    Dashboard user = new
                                    Dashboard();
                                    user.Show();
                                    this.Hide();
                                    SqlConnect.Close();
                                }
                                else
                                {
                                    MessageBox.Show("Login  Gagal! Username atau Password Salah!");
 }
                            }
                        }
                        else
                        {
                            if (counter < 3)
                            {
                                MessageBox.Show("Username Atau Password Tidak Sesual " +
                                "(Kesempatan : " + (3 - counter) + " login)", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                MessageBox.Show("Login Gagal! Aplikasi Akan Tertutup");
                                Close();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
