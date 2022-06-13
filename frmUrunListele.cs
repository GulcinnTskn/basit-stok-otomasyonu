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
    public partial class frmUrunListele : Form
    {
        public frmUrunListele()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Stok_Otomasyonu;Integrated Security=True");
        DataSet dset= new DataSet();
        private void frmUrunListele_Load(object sender, EventArgs e)
        {
            UrunListele();
            KategoriGetir();
        }

        private void UrunListele()
        {
            baglanti.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select * from urun", baglanti);
            adtr.Fill(dset, "urun");
            dataGridView1.DataSource = dset.Tables["urun"];
            baglanti.Close();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            txtBarkod.Text = dataGridView1.CurrentRow.Cells["barkodNo"].Value.ToString();
            txtUrunAdi.Text = dataGridView1.CurrentRow.Cells["urunAdi"].Value.ToString();
            txtUrunMiktari.Text = dataGridView1.CurrentRow.Cells["miktar"].Value.ToString();
            txtSatis.Text = dataGridView1.CurrentRow.Cells["satisFiyati"].Value.ToString();
            txtAlis.Text = dataGridView1.CurrentRow.Cells["alisFiyati"].Value.ToString();
            txtKategori.Text = dataGridView1.CurrentRow.Cells["kategori"].Value.ToString();
            txtMarka.Text = dataGridView1.CurrentRow.Cells["marka"].Value.ToString();

        }

        private void btnGüncelle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("update urun set urunAdi=@urunAdi,miktar=@miktar,alisFiyati=@alisFiyati,satisFiyati=@satisFiyati where barkodNo=@barkodNo",baglanti);
            komut.Parameters.AddWithValue("@barkodNo", txtBarkod.Text);
            komut.Parameters.AddWithValue("@urunAdi", txtUrunAdi.Text);
            komut.Parameters.AddWithValue("@miktar", int.Parse(txtUrunMiktari.Text));
            komut.Parameters.AddWithValue("@alisFiyati", double.Parse(txtAlis.Text));
            komut.Parameters.AddWithValue("@satisFiyati", double.Parse(txtSatis.Text));
            komut.ExecuteNonQuery();
            baglanti.Close();
            dset.Tables["urun"].Clear();
            UrunListele();
            MessageBox.Show("Ürün Başarıyla Güncellendi!");
            Temizle();
        }

        private void Temizle()
        {
            txtBarkod.Text = "";
            txtUrunAdi.Text = "";
            txtUrunMiktari.Text = "";
            txtAlis.Text = "";
            txtSatis.Text = "";

        }

        private void btnMarkaKategoriGuncelle_Click(object sender, EventArgs e)
        {
            if (txtBarkod.Text != "")
            {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("update urun set kategori=@kategori, marka=@marka where barkodNo=@barkodNo", baglanti);
            komut.Parameters.AddWithValue("@barkodNo", txtBarkod.Text);
            komut.Parameters.AddWithValue("@kategori", cmbKategori.Text);
            komut.Parameters.AddWithValue("@marka", cmbMarka.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
                dset.Tables["urun"].Clear();
                UrunListele();
                MessageBox.Show("Ürün Başarıyla Güncellendi!");

            }
            else
            {
                MessageBox.Show("Barkod Numarası Boş!");
            }

            cmbTemizle();
        }

        private void cmbTemizle()
        {
            cmbMarka.Text = "";
            cmbKategori.Text = "";
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

        private void cmbKategori_SelectedIndexChanged(object sender, EventArgs e)
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

        private void btnSil_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("delete from urun where barkodNo = '"+dataGridView1.CurrentRow.Cells["barkodNo"].Value.ToString()+"' ",baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            dset.Tables["urun"].Clear();
            UrunListele();
            MessageBox.Show("Ürün Silindi!");
        }

        private void txtAra_TextChanged(object sender, EventArgs e)
        {
            DataTable tablo = new DataTable();
            baglanti.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select * from urun where barkodNo like '%" +txtAra.Text+"%'",baglanti);
            adtr.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }
    }
}
