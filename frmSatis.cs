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
    public partial class frmSatis : Form
    {
        public frmSatis()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Stok_Otomasyonu;Integrated Security=True");
        DataSet ds = new DataSet();

        bool durum;

        private void SepetListele()
        {
            baglanti.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select * from sepet",baglanti);
            adtr.Fill(ds,"sepet");
            dataGridView1.DataSource = ds.Tables["sepet"];
            dataGridView1.Columns[5].Visible = false;
            baglanti.Close();
        }
        private void btnKategori_Click(object sender, EventArgs e)
        {
            frmKategori kategori = new frmKategori();
            kategori.ShowDialog();

        }


        private void btnUrunEkle_Click_1(object sender, EventArgs e)
        {
            frmUrunEkle ekle = new frmUrunEkle();
            ekle.ShowDialog();
        }

        private void btnMarka_Click_1(object sender, EventArgs e)
        {
            frmMarka marka = new frmMarka();
            marka.ShowDialog();
        }

        private void btnUrunListele_Click(object sender, EventArgs e)
        {
            frmUrunListele listele = new frmUrunListele();
            listele.ShowDialog();
        }

        private void frmSatis_Load(object sender, EventArgs e)
        {
            SepetListele();
        }

        private void txtBarkod_TextChanged(object sender, EventArgs e)
        {
            Temizle();

            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from urun where barkodNo like '" + txtBarkod.Text + "' ", baglanti);
            SqlDataReader reader = komut.ExecuteReader();
            if (reader.Read())
            {
                txtUrunAdi.Text = reader["urunAdi"].ToString();
                txtSatisFiyati.Text = reader["satisFiyati"].ToString();
            }
            baglanti.Close();
        }

        private void Temizle()
        {
            if (txtBarkod.Text == "")
            {
                foreach (Control item in groupBox1.Controls)
                {
                    if (item is TextBox)
                    {
                        if (item != txtUrunAdeti)
                        {
                            item.Text = "";
                        }

                    }
                }
            }
        }

        private void barkodKontrol()
        {
            durum = true;
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from sepet",baglanti);
            SqlDataReader reader = komut.ExecuteReader();
            while (reader.Read())
            {
                if (txtBarkod.Text == reader["barkodNo"].ToString())
                {
                    durum = false;
                }
                else
                {
                    durum = true;
                }
            }
            baglanti.Close();

            //if (reader.Read())
            //{
            //    durum = false;
            //}
            //else
            //{
            //    durum = true;
            //}
            //baglanti.Close();
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            barkodKontrol();
            if (durum == true)
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("insert into sepet(barkodNo,urunAdi,miktar,satisFiyati,toplamFiyat,tarih) values(@barkodNo,@urunAdi,@miktar,@satisFiyati,@toplamFiyat,@tarih)", baglanti);
                komut.Parameters.AddWithValue("@barkodNo",txtBarkod.Text);
                komut.Parameters.AddWithValue("@urunAdi",txtUrunAdi.Text);
                komut.Parameters.AddWithValue("@miktar",int.Parse(txtUrunAdeti.Text));
                komut.Parameters.AddWithValue("@satisFiyati",double.Parse(txtSatisFiyati.Text));
                komut.Parameters.AddWithValue("@toplamFiyat",double.Parse(txtToplam.Text));
                komut.Parameters.AddWithValue("@tarih",DateTime.Now.ToString());
                komut.ExecuteNonQuery();
                baglanti.Close();
            }
            else
            {
                baglanti.Open();
                SqlCommand komut2 = new SqlCommand("update sepet set miktar = miktar + '"+int.Parse(txtUrunAdeti.Text)+ "'  where barkodNo = '" + txtBarkod.Text + "' ", baglanti);
                komut2.ExecuteNonQuery();
                SqlCommand komut3 = new SqlCommand("update sepet set toplamFiyat = miktar * satisFiyati where barkodNo = '" + txtBarkod.Text + "' ", baglanti);
                komut3.ExecuteNonQuery();
                baglanti.Close();
            }
            txtUrunAdeti.Text = "1";
            ds.Tables["sepet"].Clear();
            SepetListele();
            hesapla();

            foreach (Control item in groupBox1.Controls)
            {
                if (item is TextBox)
                {
                    if (item != txtUrunAdeti)
                    {
                        item.Text = "";
                    }

                }
            }

        }

        private void txtUrunAdeti_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtToplam.Text = (double.Parse(txtUrunAdeti.Text) * double.Parse(txtSatisFiyati.Text)).ToString();
            }
            catch (Exception)
            {


            }
        }

        private void txtSatisFiyati_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtToplam.Text = (double.Parse(txtUrunAdeti.Text) * double.Parse(txtSatisFiyati.Text)).ToString();
            }
            catch (Exception)
            {

                
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("delete from sepet where barkodNo = '"+dataGridView1.CurrentRow.Cells["barkodNo"].Value.ToString()+"' ", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Ürün Sepetten Çıkarıldı!");
            ds.Tables["sepet"].Clear();
            SepetListele();
            hesapla();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("delete from sepet ", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Ürünler Sepetten Çıkarıldı!");
            ds.Tables["sepet"].Clear();
            SepetListele();
            hesapla();
        }

        private void btnSatisListele_Click(object sender, EventArgs e)
        {
            frmSatisListele satisListele = new frmSatisListele();
            satisListele.ShowDialog();
        }

        private void hesapla()
        {
            try
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("select sum(toplamFiyat) from sepet ", baglanti);
                lblToplam.Text = komut.ExecuteScalar() + "TL";
             
                baglanti.Close();
            }
            catch (Exception)
            {

                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count-1; i++)
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("insert into satis(barkodNo,urunAdi,miktar,satisFiyati,toplamFiyat,tarih) values(@barkodNo,@urunAdi,@miktar,@satisFiyati,@toplamFiyat,@tarih)", baglanti);
                komut.Parameters.AddWithValue("@barkodNo", dataGridView1.Rows[i].Cells["barkodNo"].Value.ToString());
                komut.Parameters.AddWithValue("@urunAdi", dataGridView1.Rows[i].Cells["urunAdi"].Value.ToString());
                komut.Parameters.AddWithValue("@miktar", int.Parse(dataGridView1.Rows[i].Cells["miktar"].Value.ToString()));
                komut.Parameters.AddWithValue("@satisFiyati", double.Parse(dataGridView1.Rows[i].Cells["satisFiyati"].Value.ToString()));
                komut.Parameters.AddWithValue("@toplamFiyat", double.Parse(dataGridView1.Rows[i].Cells["toplamFiyat"].Value.ToString()));
                komut.Parameters.AddWithValue("@tarih", DateTime.Now.ToString());
                komut.ExecuteNonQuery();
                SqlCommand komut2 = new SqlCommand("update urun set miktar = miktar - '" + int.Parse(dataGridView1.Rows[i].Cells["miktar"].Value.ToString()) + "' where barkodNo='" + dataGridView1.Rows[i].Cells["barkodNo"].Value.ToString() + "' ", baglanti);
                komut2.ExecuteNonQuery();
                baglanti.Close();

            }
            baglanti.Open();
            SqlCommand komut3 = new SqlCommand("delete from sepet ", baglanti);
            komut3.ExecuteNonQuery();
            baglanti.Close();
            ds.Tables["sepet"].Clear();
            SepetListele();
            hesapla();

        }
    }
}
