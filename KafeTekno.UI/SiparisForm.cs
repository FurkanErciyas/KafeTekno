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
    public partial class SiparisForm : Form
    {
        private readonly KafeVeri _db;
        private readonly Siparis _siparis;
        private readonly BindingList<SiparisDetay> _blSiparisDetaylar;


        public SiparisForm(KafeVeri db, Siparis siparis)
        {
            _db = db;
            _siparis = siparis;
            _blSiparisDetaylar = new BindingList<SiparisDetay>(_siparis.SiparisDetaylar);
            InitializeComponent();
            cboUrun.DataSource = _db.Urunler;
            dgvUrunler.DataSource = _blSiparisDetaylar;
            _blSiparisDetaylar.ListChanged += _blSiparisDetaylar_ListChanged;
            MasaNoGuncelle();
            OdemeTutariGuncelle();
        }

        private void _blSiparisDetaylar_ListChanged(object sender, ListChangedEventArgs e)
        {
            OdemeTutariGuncelle();
        }

        private void OdemeTutariGuncelle()
        {
            lblOdemeTutari.Text = _siparis.ToplamTutarTL;
        }

        private void MasaNoGuncelle()
        {
            Text = $"Masa {_siparis.MasaNo:00} (Açılış Zamanı: {_siparis.AcilisZamani})";
            lblMasaNo.Text = _siparis.MasaNo.ToString();
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            if (cboUrun.SelectedIndex == -1) return;
            Urun urun = (Urun)cboUrun.SelectedItem;
            SiparisDetay sd = new SiparisDetay() { UrunAd = urun.UrunAd, BirimFiyat = urun.BirimFiyat, Adet = (int)nudAdet.Value};
            _blSiparisDetaylar.Add(sd);
            EkleFormunuSifirla();
        }

        private void EkleFormunuSifirla()
        {
            cboUrun.SelectedIndex = 0;
            nudAdet.Value = 1;
        }

        private void dgvUrunler_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            DialogResult dr = MessageBox.Show(
                text: "Ürünü silmek istiyor musunuz?",
                caption: "Detay Silme Onay", 
                buttons: MessageBoxButtons.YesNo, 
                icon: MessageBoxIcon.Question, 
                defaultButton: MessageBoxDefaultButton.Button2);

            e.Cancel = dr == DialogResult.No;
        }

        private void btnAnaSayfa_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOdemeAl_Click(object sender, EventArgs e)
        {
            MasayiKapat(SiparisDurum.Odendi, _siparis.ToplamTutar());
        }

        private void btnSiparisIptal_Click(object sender, EventArgs e)
        {
            MasayiKapat(SiparisDurum.Iptal);
        }

        private void MasayiKapat(SiparisDurum durum, decimal odenenTutar = 0)
        {
            _siparis.Durum = durum;
            _siparis.KapanisZamani = DateTime.Now;
            _siparis.OdenenTutar = odenenTutar;
            _db.AktifSiparisler.Remove(_siparis);
            _db.GecmisSiparisler.Add(_siparis);
            Close();
        }
    }
}
