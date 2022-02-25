using KafeTekno.DATA;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KafeTekno.UI
{
    public partial class UrunlerForm : Form
    {
        private readonly KafeVeri _db;
        private readonly BindingList<Urun> _blUrunler;
        private Urun _duzenlenen;

        public UrunlerForm(KafeVeri db)
        {
            _db = db;
            _blUrunler = new BindingList<Urun>(_db.Urunler);
            InitializeComponent();
            dgvUrunler.AutoGenerateColumns = false;
            dgvUrunler.DataSource = _blUrunler;
            //dgvUrunler.Columns[0].HeaderText = "Ürün Adı";
            //dgvUrunler.Columns[1].HeaderText = "Birim Fiyat";
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            string ad = txtUrunAd.Text.Trim();
            if (ad == "")
            {
                MessageBox.Show("Bir Ürün Adı Belirtmediniz..!");
                return;
            }

            if(_duzenlenen == null)
            {
            _blUrunler.Add(new Urun() { UrunAd = ad, BirimFiyat = nudBirimFiyat.Value });
            }
            else
            {
                _duzenlenen.UrunAd = ad;
                _duzenlenen.BirimFiyat = nudBirimFiyat.Value;
            }

            FormuSifirla();
        }

        private void FormuSifirla()
        {
            txtUrunAd.Clear();
            nudBirimFiyat.Value = 0;
            btnIptal.Hide();
            dgvUrunler.Enabled = true;
            btnEkle.Text = "EKLE";
            _duzenlenen = null;
        }

        private void dgvUrunler_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;

            DataGridViewRow satir = dgvUrunler.Rows[e.RowIndex];
            _duzenlenen = (Urun)satir.DataBoundItem;
            txtUrunAd.Text = _duzenlenen.UrunAd;
            nudBirimFiyat.Value = _duzenlenen.BirimFiyat;
            dgvUrunler.Enabled = false;
            btnEkle.Text = "KAYDET";
            btnIptal.Show();
        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            FormuSifirla();
        }
    }
}



