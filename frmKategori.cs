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
    public partial class frmKategori : Form
    {
        public frmKategori()
        {
            InitializeComponent();
        }

        SqlConnection baglanti  = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Stok_Otomasyonu;Integrated Security=True");

        bool durum;
        private void btnEkle_Click(object sender, EventArgs e)
        {
            KategoriKontrol();

            if (durum == true)
            {

            baglanti.Open();
            SqlCommand komut = new SqlCommand("insert into kategori_bilgileri(kategori) values('"+txtKategori.Text+"')",baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Kategori Başarıyla Eklendi!");

            }
            else
            {
                MessageBox.Show("Böyle Bir Kategori Mevcut!", "Uyarı!");
            }
            Temizle();
        }


        private void KategoriKontrol()
        {
            durum = true;
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from kategori_bilgileri where kategori =@kategori", baglanti);
            komut.Parameters.AddWithValue("@kategori",txtKategori.Text);
            SqlDataReader reader = komut.ExecuteReader();
            //while (reader.Read())
            //{
            //    if (txtKategori.Text == reader["kategori"].ToString() || txtKategori.Text == "")
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
        private void Temizle()
        {
            txtKategori.Text = "";
        }
    }
}
