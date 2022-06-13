using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StokOtomasyonu
{
    public partial class frmUrunEkle : Form
    {
        public frmUrunEkle()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Stok_Otomasyonu;Integrated Security=True");
        bool durum;

        private void BarkodKontrol()
        {
            durum = true;
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from urun where barkodNo=@barkodNo", baglanti);
            komut.Parameters.AddWithValue("@barkodNo", txtYeniBarkod.Text);
            SqlDataReader reader = komut.ExecuteReader();
            //while (reader.Read())
            //{
            //    if (txtYeniBarkod.Text == reader["barkodNo"].ToString() || txtYeniBarkod.Text=="")
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
        private void btnYeniEkle_Click(object sender, EventArgs e)
        {
            BarkodKontrol();

            if (durum == true)
            {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("insert into urun(barkodNo,marka,kategori,urunAdi,miktar,alisFiyati,satisFiyati,tarih) values(@barkodNo,@marka,@kategori,@urunAdi,@miktar,@alisFiyati,@satisFiyati,@tarih) ", baglanti);
            komut.Parameters.AddWithValue("@barkodNo",txtYeniBarkod.Text);
            komut.Parameters.AddWithValue("@kategori", cmbKategori.Text);
            komut.Parameters.AddWithValue("@marka", cmbMarka.Text);
            komut.Parameters.AddWithValue("@urunAdi", txtYeniUrunAdi.Text);
            komut.Parameters.AddWithValue("@miktar", int.Parse(txtYeniUrunMiktar.Text));
            komut.Parameters.AddWithValue("@alisFiyati", double.Parse(txtYeniAlis.Text));
            komut.Parameters.AddWithValue("@satisFiyati", double.Parse(txtYeniSatis.Text));
            komut.Parameters.AddWithValue("@tarih", DateTime.Now.ToString());

            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Ürün Başarıyla Eklendi!");
            cmbMarka.Items.Clear();

            }
            else
            {
                MessageBox.Show("Bu Barkod Numarasıyla Zaten Ürün Kaydedilmiş!", "Uyarı!");
            }
            Temizle();
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

        private void frmUrunEkle_Load(object sender, EventArgs e)
        {
            KategoriGetir();
        }

        private void Temizle()
        {
            txtYeniBarkod.Text = "";
            txtYeniAlis.Text = "";
            txtYeniSatis.Text = "";
            txtYeniUrunAdi.Text = "";
            txtYeniUrunMiktar.Text = "";
            cmbKategori.Text = "";
            cmbMarka.Text = "";
        }

        private void cmbKategori_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            cmbMarka.Items.Clear();
            cmbMarka.Text = "";
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from marka_bilgileri where kategori ='" + cmbKategori.SelectedItem + "' ", baglanti);
            SqlDataReader reader = komut.ExecuteReader();
            while (reader.Read())
            {
                cmbMarka.Items.Add(reader["marka"].ToString());
            }
            baglanti.Close();
            
        }

        private void txtEskiBarkod_TextChanged(object sender, EventArgs e)
        {
            if (txtEskiBarkod.Text == "")
            {
                txtYeniMiktar.Text = "";
                foreach (Control item in groupBox2.Controls)
                {
                    if (item is TextBox)
                    {
                        item.Text = "";
                    }
                }
            }

            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from urun where barkodNo like '"+txtEskiBarkod.Text+"'",baglanti);
            SqlDataReader reader = komut.ExecuteReader();
            while (reader.Read())
            {
                txtEskiKategori.Text = reader["kategori"].ToString();
                txtEskiMarka.Text = reader["marka"].ToString();
                txtEskiUrunAdi.Text = reader["urunAdi"].ToString();
                txtEskiAlis.Text = reader["alisFiyati"].ToString();
                txtEskiSatis.Text = reader["satisFiyati"].ToString();
                txtYeniMiktar.Text = reader["miktar"].ToString();
            }
            baglanti.Close();
        }

        private void btnEskiEkle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("update urun set miktar = miktar + '"+int.Parse(txtEskiUrunMiktari.Text)+"' where barkodNo='"+txtEskiBarkod.Text+"' ",baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();

         
            MessageBox.Show("Ürün Miktarı Stoklara Eklendi!");
            TemizleEski();

        }

        private void TemizleEski()
        {
            txtEskiBarkod.Text = "";
            txtEskiAlis.Text = "";
            txtEskiKategori.Text = "";
            txtEskiMarka.Text = "";
            txtEskiSatis.Text = "";
            txtEskiUrunAdi.Text = "";
            txtEskiUrunMiktari.Text = "";
            txtYeniMiktar.Text = "";
        }
    }
}
