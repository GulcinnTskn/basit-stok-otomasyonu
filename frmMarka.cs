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
    public partial class frmMarka : Form
    {
        public frmMarka()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Stok_Otomasyonu;Integrated Security=True");
        bool durum;
        private void btnEkle_Click(object sender, EventArgs e)
        {
            MarkaKontrol();

            if (durum == true)
            {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("insert into marka_bilgileri(kategori, marka) values('"+cmbKategori.Text+"','" + txtMarka.Text + "')", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Marka Başarıyla Eklendi!");
            }
            else
            {
                MessageBox.Show("Böyle Bir Marka ve Kategori Mevcut!", "Uyarı!");
            }
            Temizle();

            
        }

        private void Temizle()
        {
            txtMarka.Text = "";
            cmbKategori.Text = "";
        }

        private void frmMarka_Load(object sender, EventArgs e)
        {
            KategoriGetir();
        }

        private void KategoriGetir()
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from kategori_bilgileri", baglanti);
            SqlDataReader reader = komut.ExecuteReader();
            while (reader.Read())
            {
                cmbKategori.Items.Add(reader["kategori"].ToString());
            }
            baglanti.Close();

        }

        private void MarkaKontrol()
        {
            durum = true;
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from marka_bilgileri where marka=@marka", baglanti);
            komut.Parameters.AddWithValue("@marka", txtMarka.Text);
            SqlDataReader reader = komut.ExecuteReader();
            //while (reader.Read())
            //{
            //    if (cmbKategori.Text==reader["kategori"].ToString() && txtMarka.Text == reader["marka"].ToString() || cmbKategori.Text == "" || txtMarka.Text == "")
            //    {
            //        durum = false;
            //    }
            //}
            //baglanti.Close();
            if (reader.Read())
            {
                durum = false;
            }
            else
            {
                durum = true;
            }
            baglanti.Close();
        }
    }
}
