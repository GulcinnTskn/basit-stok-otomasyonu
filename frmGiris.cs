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

namespace StokOtomasyonu
{
    public partial class frmGiris : Form
    {
        public frmGiris()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Stok_Otomasyonu;Integrated Security=True");
        SqlCommand komut;
        SqlDataReader dr;

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmKayit kayit = new frmKayit();
            kayit.Show();
            this.Hide();
            
        }


        private void btnGiris_Click(object sender, EventArgs e)
        {

            string email = txtEmail.Text;
            string sifre = txtSifre.Text;
            komut = new SqlCommand();

            baglanti.Open();
            komut.Connection = baglanti;
            komut.CommandText = "select * from users where email= '" + txtEmail.Text + "' and sifre='" + txtSifre.Text + "'";
            dr = komut.ExecuteReader();
            if (dr.Read())
            {
                MessageBox.Show("Giriş Başarılı!");

                frmSatis satis = new frmSatis();
                satis.Show();
                this.Hide();

            }
            else
            {
                MessageBox.Show("Giriş yapılamadı. Lütfen Bilgilerinizi Kontrol Ediniz!");
                txtEmail.Text = "";
                txtSifre.Text = "";
            }
            baglanti.Close();

           

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                //karakteri göster.
                txtSifre.PasswordChar = '\0';
            }
            //değilse karakterlerin yerine * koy.
            else
            {
                txtSifre.PasswordChar = '*';
            }
        }
    }
}
