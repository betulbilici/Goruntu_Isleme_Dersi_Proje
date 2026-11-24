using System;
using System.Drawing;
using System.Windows.Forms;

namespace GoruntuIsleme
{
    public partial class Hafta2Odev10 : Form
    {
        private Bitmap bmpAnaRenk;
        private Bitmap bmpTonSkala;
        private Color secilenTemelRenk = Color.Red;
        private Color secilenSonRenk = Color.Red;

        // Sol taraftaki 48 adet özel renk kutucuğu
        private PictureBox[] ozelRenkKutulari = new PictureBox[48];
        private int secilenKutuIndex = -1;

        public Hafta2Odev10()
        {
            InitializeComponent();
            ModernTemaUygula();

            if (btnOzelEkle != null) btnOzelEkle.Click += btnOzelEkle_Click;
            if (btnPaletiTemizle != null) btnPaletiTemizle.Click += btnPaletiTemizle_Click;

            if (picAnaRenk != null)
            {
                picAnaRenk.MouseDown += PicAnaRenk_MouseDown;
                picAnaRenk.MouseMove += PicAnaRenk_MouseMove;
            }

            if (picTonSkala != null)
            {
                picTonSkala.MouseDown += PicTonSkala_MouseDown;
                picTonSkala.MouseMove += PicTonSkala_MouseMove;
            }

            this.Load += Hafta2Odev10_Load;
        }

        private void ModernTemaUygula()
        {
            // 1. Form Temel Ayarları (1250x750, Standart Kenarlık ve X ile Ana Menüye Dönüş)
            FormGezintiYardimcisi.FormuHazirla(this);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Text = "Hafta 2 - Ödev 10 | 16 Milyon Renk Spektrumu ve Özel Palet";

            // Eski alttaki linkleri gizle (üst bara alıyoruz)
            if (lnkGeri != null) lnkGeri.Visible = false;
            if (lnkAnaMenu != null) lnkAnaMenu.Visible = false;

            // 2. Üst Gezinti Barı
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                form: this,
                baslik: "Hafta 2 - Ödev 10 | 16 Milyon Renk Spektrumu ve Özel Palet",
                oncekiYazi: "< Ödev 9",
                oncekiTikla: () =>
                {
                    Form odev9 = Application.OpenForms["Hafta2Odev9"];
                    if (odev9 != null)
                        FormGezintiYardimcisi.FormGecisYap(this, odev9);
                    else
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta2Odev9());
                },
                sonrakiYazi: "Hafta 3 >",
                sonrakiTikla: () =>
                {
                    Form hafta3 = Application.OpenForms["Hafta3Odev1"];
                    if (hafta3 != null)
                        FormGezintiYardimcisi.FormGecisYap(this, hafta3);
                    else
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta3Odev1());
                }
            );

            // 3. Mevcut kontrollerin konumlarını üst barın altına hizala ve modernleştir
            int ustKaydirma = 55;
            int yatayKaydirma = 120;

            foreach (Control c in this.Controls)
            {
                if (c == pnlUst) continue;
                c.Location = new Point(c.Location.X + yatayKaydirma, c.Location.Y + ustKaydirma);
            }

            // Etiket yazı tipleri ve renkleri
            Label[] etiketler = { label1, label2, label3, label4, label5, label6, label7 };
            foreach (Label lbl in etiketler)
            {
                if (lbl == null) continue;
                lbl.Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold);
                lbl.ForeColor = FormGezintiYardimcisi.KoyuYazi;
            }

            // Özel renkler paneli görünümü
            if (pnlOzelRenkler != null)
            {
                pnlOzelRenkler.BackColor = Color.White;
                pnlOzelRenkler.BorderStyle = BorderStyle.FixedSingle;
            }

            // RGB TextBox stilleri
            TextBox[] kutular = { txtR, txtG, txtB };
            foreach (TextBox tb in kutular)
            {
                if (tb == null) continue;
                tb.Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold);
                tb.TextAlign = HorizontalAlignment.Center;
                tb.BackColor = Color.White;
            }

            // Butonları temaya uyarla
            if (btnOzelEkle != null)
            {
                btnOzelEkle.FlatStyle = FlatStyle.Flat;
                btnOzelEkle.FlatAppearance.BorderSize = 0;
                btnOzelEkle.BackColor = Color.FromArgb(30, 58, 95);
                btnOzelEkle.ForeColor = Color.White;
                btnOzelEkle.Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold);
                btnOzelEkle.Cursor = Cursors.Hand;
            }

            if (btnPaletiTemizle != null)
            {
                btnPaletiTemizle.FlatStyle = FlatStyle.Flat;
                btnPaletiTemizle.FlatAppearance.BorderColor = FormGezintiYardimcisi.AcikMaviKenar;
                btnPaletiTemizle.FlatAppearance.BorderSize = 1;
                btnPaletiTemizle.BackColor = FormGezintiYardimcisi.AcikMaviZemin;
                btnPaletiTemizle.ForeColor = FormGezintiYardimcisi.KoyuMaviYazi;
                btnPaletiTemizle.Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold);
                btnPaletiTemizle.Cursor = Cursors.Hand;
            }

            // Sol alt kısma kısa kullanım bilgi kartı ekle
            ModernKartPanel bilgiKarti = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                konum: new Point(pnlOzelRenkler.Left, pnlOzelRenkler.Bottom + 20),
                boyut: new Size(pnlOzelRenkler.Width, 165),
                icerikEtiketi: out Label lblBilgi
            );
            lblBilgi.Text =
                "Kullanım İpuçları:\n\n" +
                "• Ortadaki renk düzleminden ana rengi, sağdaki şeritten koyuluk/açıklık tonunu seçiniz.\n" +
                "• Sol Tık: Kutudaki rengi seçer.\n" +
                "• Sağ Tık: İlgili renk kutusunu siler.";
            this.Controls.Add(bilgiKarti);
        }

        private void Hafta2Odev10_Load(object sender, EventArgs e)
        {
            OzelRenkKutulariniOlustur();
            AnaRenkSpektrumunuCiz();
            TonSkalasiniCiz(secilenTemelRenk);
            RengiGuncelle(secilenTemelRenk);
        }

        // Sol taraftaki 6x8'lik 48 küçük renk kutucuğunu dinamik oluşturma ve ortalama
        private void OzelRenkKutulariniOlustur()
        {
            if (pnlOzelRenkler == null) return;
            pnlOzelRenkler.Controls.Clear();

            int sutunSayisi = 8;
            int satirSayisi = 6;
            int kutuW = 32;
            int kutuH = 32;
            int bosluk = 6;

            int toplamKutuGenisligi = (sutunSayisi * kutuW) + ((sutunSayisi - 1) * bosluk);
            int toplamKutuYuksekligi = (satirSayisi * kutuH) + ((satirSayisi - 1) * bosluk);
            int baslangicX = Math.Max(10, (pnlOzelRenkler.ClientSize.Width - toplamKutuGenisligi) / 2);
            int baslangicY = Math.Max(10, (pnlOzelRenkler.ClientSize.Height - toplamKutuYuksekligi) / 2);

            for (int i = 0; i < 48; i++)
            {
                int satir = i / sutunSayisi;
                int sutun = i % sutunSayisi;

                PictureBox pb = new PictureBox
                {
                    Size = new Size(kutuW, kutuH),
                    Location = new Point(baslangicX + sutun * (kutuW + bosluk), baslangicY + satir * (kutuH + bosluk)),
                    BorderStyle = BorderStyle.Fixed3D,
                    BackColor = Color.White,
                    Cursor = Cursors.Hand
                };

                int index = i;

                pb.MouseDown += (s, e) =>
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        secilenKutuIndex = index;
                        if (pb.BackColor != Color.White)
                        {
                            RengiGuncelle(pb.BackColor);
                        }
                    }
                    else if (e.Button == MouseButtons.Right)
                    {
                        pb.BackColor = Color.White;
                        secilenKutuIndex = index;
                    }
                };

                ozelRenkKutulari[i] = pb;
                pnlOzelRenkler.Controls.Add(pb);
            }
        }

        // 16 Milyon Renk Tabanı: Hue (0-360) ve Saturation (0-1) Gradyanını Çizdirme
        private void AnaRenkSpektrumunuCiz()
        {
            if (picAnaRenk == null || picAnaRenk.Width <= 0 || picAnaRenk.Height <= 0) return;

            int w = picAnaRenk.Width;
            int h = picAnaRenk.Height;
            bmpAnaRenk?.Dispose();
            bmpAnaRenk = new Bitmap(w, h);

            for (int x = 0; x < w; x++)
            {
                double hue = (double)x / w * 360.0;

                for (int y = 0; y < h; y++)
                {
                    double sat = 1.0 - ((double)y / h);
                    Color c = HsvToRgb(hue, sat, 1.0);
                    bmpAnaRenk.SetPixel(x, y, c);
                }
            }

            picAnaRenk.Image = bmpAnaRenk;
        }

        // Seçilen rengin açıktan koyuya ton skalasını çizdirme
        private void TonSkalasiniCiz(Color temelRenk)
        {
            if (picTonSkala == null || picTonSkala.Width <= 0 || picTonSkala.Height <= 0) return;

            int w = picTonSkala.Width;
            int h = picTonSkala.Height;
            bmpTonSkala?.Dispose();
            bmpTonSkala = new Bitmap(w, h);

            for (int y = 0; y < h; y++)
            {
                double factor = 1.0 - ((double)y / (h - 1));

                int r = (int)(temelRenk.R * factor);
                int g = (int)(temelRenk.G * factor);
                int b = (int)(temelRenk.B * factor);

                Color tonRengi = Color.FromArgb(r, g, b);

                for (int x = 0; x < w; x++)
                {
                    bmpTonSkala.SetPixel(x, y, tonRengi);
                }
            }

            picTonSkala.Image = bmpTonSkala;
        }

        private void PicAnaRenk_MouseDown(object sender, MouseEventArgs e) => AnaRenkSec(e);

        private void PicAnaRenk_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) AnaRenkSec(e);
        }

        private void AnaRenkSec(MouseEventArgs e)
        {
            if (bmpAnaRenk == null || e.X < 0 || e.X >= bmpAnaRenk.Width || e.Y < 0 || e.Y >= bmpAnaRenk.Height) return;

            secilenTemelRenk = bmpAnaRenk.GetPixel(e.X, e.Y);
            TonSkalasiniCiz(secilenTemelRenk);
            RengiGuncelle(secilenTemelRenk);
        }

        private void PicTonSkala_MouseDown(object sender, MouseEventArgs e) => TonSec(e);

        private void PicTonSkala_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) TonSec(e);
        }

        private void TonSec(MouseEventArgs e)
        {
            if (bmpTonSkala == null || e.X < 0 || e.X >= bmpTonSkala.Width || e.Y < 0 || e.Y >= bmpTonSkala.Height) return;

            Color sonRenk = bmpTonSkala.GetPixel(e.X, e.Y);
            RengiGuncelle(sonRenk);
        }

        private void RengiGuncelle(Color c)
        {
            secilenSonRenk = c;

            if (picOnizleme != null) picOnizleme.BackColor = c;
            if (txtR != null) txtR.Text = c.R.ToString();
            if (txtG != null) txtG.Text = c.G.ToString();
            if (txtB != null) txtB.Text = c.B.ToString();
        }

        private void btnOzelEkle_Click(object sender, EventArgs e)
        {
            int hedefIndex = -1;

            if (secilenKutuIndex >= 0 && secilenKutuIndex < 48)
            {
                hedefIndex = secilenKutuIndex;
            }
            else
            {
                for (int i = 0; i < 48; i++)
                {
                    if (ozelRenkKutulari[i] != null && ozelRenkKutulari[i].BackColor == Color.White)
                    {
                        hedefIndex = i;
                        break;
                    }
                }
            }

            if (hedefIndex == -1)
            {
                MessageBox.Show("Kişiye özel renk paletiniz tamamen dolu. Yeni renk eklemek için bir kutuyu sağ tık ile silebilir veya değiştirmek istediğiniz kutuya sol tıklayabilirsiniz.",
                                "Palet Dolu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (ozelRenkKutulari[hedefIndex] != null)
            {
                ozelRenkKutulari[hedefIndex].BackColor = secilenSonRenk;
                secilenKutuIndex = -1;
            }
        }

        private void btnPaletiTemizle_Click(object sender, EventArgs e)
        {
            DialogResult secim = MessageBox.Show("Özel renk paletini tamamen temizlemek istiyor musunuz?",
                                                 "Paleti Temizle", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.Yes)
            {
                for (int i = 0; i < 48; i++)
                {
                    if (ozelRenkKutulari[i] != null)
                    {
                        ozelRenkKutulari[i].BackColor = Color.White;
                    }
                }
                secilenKutuIndex = -1;
            }
        }

        private Color HsvToRgb(double h, double s, double v)
        {
            int hi = (int)Math.Floor(h / 60.0) % 6;
            double f = (h / 60.0) - Math.Floor(h / 60.0);

            v = v * 255.0;
            byte vVal = (byte)v;
            byte p = (byte)(v * (1.0 - s));
            byte q = (byte)(v * (1.0 - (f * s)));
            byte t = (byte)(v * (1.0 - ((1.0 - f) * s)));

            switch (hi)
            {
                case 0: return Color.FromArgb(vVal, t, p);
                case 1: return Color.FromArgb(q, vVal, p);
                case 2: return Color.FromArgb(p, vVal, t);
                case 3: return Color.FromArgb(p, q, vVal);
                case 4: return Color.FromArgb(t, p, vVal);
                default: return Color.FromArgb(vVal, p, q);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            bmpAnaRenk?.Dispose();
            bmpTonSkala?.Dispose();
            base.OnFormClosed(e);
        }
    }
}