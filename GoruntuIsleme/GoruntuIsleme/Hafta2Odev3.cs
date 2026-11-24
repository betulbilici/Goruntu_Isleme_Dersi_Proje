using System;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace GoruntuIsleme
{
    public partial class Hafta2Odev3 : Form
    {
        private Mat orijinalGri = null;
        private Mat islenmisGri = null;

        private ModernHapRozet rozetOrijinal;
        private ModernHapRozet rozetYeni;
        private ModernHapRozet rozetHistOrijinal;
        private ModernHapRozet rozetHistYeni;

        private ModernSurgu surguAlt;
        private ModernSurgu surguUst;
        private Label lblAltDeger;
        private Label lblUstDeger;
        private Label lblAnaliz;

        public Hafta2Odev3()
        {
            InitializeComponent();
            ModernArayuzuUygula();
        }

        private void ModernArayuzuUygula()
        {
            // 1. Form Temel Ayarları (1250x750, Standart Kenarlık ve X ile Ana Menüye Dönüş)
            FormGezintiYardimcisi.FormuHazirla(this);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Text = "Hafta 2 - Ödev 3: Histogram Sıkıştırma (Kontrast Daraltma) ve Piksel İnceleme";

            // Designer'daki tüm eski kontrolleri gizle
            foreach (Control ctrl in this.Controls)
            {
                ctrl.Visible = false;
            }

            // 2. Üst Gezinti Barı
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                form: this,
                baslik: "Hafta 2 - Ödev 3 | Histogram Sıkıştırma [Alt - Üst] ve Piksel Koordinat Okuma",
                oncekiYazi: "< Ödev 2",
                oncekiTikla: () =>
                {
                    Form odev2 = Application.OpenForms["Hafta2Odev2"];
                    if (odev2 != null)
                        FormGezintiYardimcisi.FormGecisYap(this, odev2);
                    else
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta2Odev2());
                },
                sonrakiYazi: "Ödev 4 >",
                sonrakiTikla: () => FormGezintiYardimcisi.FormGecisYap(this, new Hafta2Odev4())
            );

            // 3. Sağ Kontrol Paneli
            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 290);

            Label lblKontrolBaslik = new Label
            {
                Text = "Histogram Sıkıştırma",
                Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                Location = new Point(18, 18),
                AutoSize = true
            };
            pnlSag.Controls.Add(lblKontrolBaslik);

            // Resim Seç Butonu
            ModernHapButon btnModernResimSec = FormGezintiYardimcisi.ModernButonOlustur("📁 Resim Seç", true, 252, 40);
            btnModernResimSec.Location = new Point(18, 52);
            btnModernResimSec.Click += btnResimSec_Click;
            pnlSag.Controls.Add(btnModernResimSec);

            // Alt Sınır Sürgüsü (0..255)
            Panel pnlAltSurgu = FormGezintiYardimcisi.KartAltSurguPaneliOlustur(
                solEtiket: "Alt Sınır (Min): 0",
                sagAralik: "0 .. 255",
                min: 0,
                max: 255,
                deger: 0,
                surgu: out surguAlt,
                solLabel: out lblAltDeger
            );
            pnlAltSurgu.Location = new Point(18, 108);
            pnlAltSurgu.Width = 252;
            pnlSag.Controls.Add(pnlAltSurgu);

            // Üst Sınır Sürgüsü (0..255)
            Panel pnlUstSurgu = FormGezintiYardimcisi.KartAltSurguPaneliOlustur(
                solEtiket: "Üst Sınır (Max): 255",
                sagAralik: "0 .. 255",
                min: 0,
                max: 255,
                deger: 255,
                surgu: out surguUst,
                solLabel: out lblUstDeger
            );
            pnlUstSurgu.Location = new Point(18, 170);
            pnlUstSurgu.Width = 252;
            pnlSag.Controls.Add(pnlUstSurgu);

            surguAlt.Scroll += (s, e) => SurguDegisti(true);
            surguUst.Scroll += (s, e) => SurguDegisti(false);

            // Hızlı Ön Ayar Butonları
            ModernHapButon btnDaralt = FormGezintiYardimcisi.ModernButonOlustur("📉 [80 - 170]", false, 121, 36);
            btnDaralt.Location = new Point(18, 236);
            btnDaralt.Click += (s, e) =>
            {
                surguAlt.Value = 80;
                surguUst.Value = 170;
                SurguDegisti(true);
            };
            pnlSag.Controls.Add(btnDaralt);

            ModernHapButon btnSifirla = FormGezintiYardimcisi.ModernButonOlustur("🔄 [0 - 255]", false, 121, 36);
            btnSifirla.Location = new Point(149, 236);
            btnSifirla.Click += (s, e) =>
            {
                surguAlt.Value = 0;
                surguUst.Value = 255;
                SurguDegisti(true);
            };
            pnlSag.Controls.Add(btnSifirla);

            // Seçilen Piksel Bilgisi Kutusu
            Label lblPikselBaslik = new Label
            {
                Text = "Seçilen Piksel Bilgisi (Sağ Üst Resim):",
                Font = new Font("Segoe UI Semibold", 8.8f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                Location = new Point(18, 286),
                AutoSize = true
            };
            pnlSag.Controls.Add(lblPikselBaslik);

            txtPikselBilgi.Parent?.Controls.Remove(txtPikselBilgi);
            txtPikselBilgi.Visible = true;
            txtPikselBilgi.Location = new Point(18, 308);
            txtPikselBilgi.Size = new Size(252, 28);
            txtPikselBilgi.Font = new Font("Segoe UI Semibold", 8.8f, FontStyle.Bold);
            txtPikselBilgi.ReadOnly = true;
            txtPikselBilgi.BackColor = FormGezintiYardimcisi.TemaZemin;
            txtPikselBilgi.Text = "Fareyi sıkıştırılmış resim üzerinde gezdirin";
            pnlSag.Controls.Add(txtPikselBilgi);

            // Açık Mavi Formül ve Açıklama Kartı
            ModernKartPanel bilgiKarti = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                konum: new Point(18, 350),
                boyut: new Size(252, 220),
                icerikEtiketi: out Label lblBilgi
            );
            lblBilgi.Text =
                "Doğrusal Ölçekleme Formülü:\n\n" +
                "g'(x, y) = Alt + (g(x, y) / 255) × (Üst - Alt)\n\n" +
                "• Tüm piksel yoğunlukları seçilen [Alt, Üst] aralığına sıkıştırılır.\n" +
                "• Sağ üstteki sıkıştırılmış görüntü üzerinde fareyi hareket ettirerek (X, Y) ve RGB/Gri değerlerini okuyabilirsiniz.";
            pnlSag.Controls.Add(bilgiKarti);

            // 4. Orta Çalışma Alanı (2x2 Izgara: Üstte Görüntüler, Altta Histogramlar + Alt Analiz Şeridi)
            Panel pnlOrta = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(12),
                BackColor = FormGezintiYardimcisi.TemaZemin
            };

            TableLayoutPanel anaYerlesim = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2
            };
            anaYerlesim.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            anaYerlesim.RowStyles.Add(new RowStyle(SizeType.Absolute, 52f));

            TableLayoutPanel kartIzgara = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 2
            };
            kartIzgara.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            kartIzgara.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            kartIzgara.RowStyles.Add(new RowStyle(SizeType.Percent, 54f));
            kartIzgara.RowStyles.Add(new RowStyle(SizeType.Percent, 46f));

            rozetOrijinal = new ModernHapRozet();
            rozetOrijinal.MetinGuncelle("Gri-Ton");

            rozetYeni = new ModernHapRozet();
            rozetYeni.MetinGuncelle("[0 - 255]");

            rozetHistOrijinal = new ModernHapRozet();
            rozetHistOrijinal.MetinGuncelle("0 .. 255");

            rozetHistYeni = new ModernHapRozet();
            rozetHistYeni.MetinGuncelle("Sıkıştırılmış");

            MevcutPictureBoxKartaTasi(kartIzgara, 0, 0, "Orijinal Gri Görüntü", picOrijinal, FormGezintiYardimcisi.TemaZemin, rozetOrijinal);
            MevcutPictureBoxKartaTasi(kartIzgara, 1, 0, "Sıkıştırılmış Görüntü", picYeni, FormGezintiYardimcisi.TemaZemin, rozetYeni);
            MevcutPictureBoxKartaTasi(kartIzgara, 0, 1, "Orijinal Histogram", picHistOrijinal, Color.FromArgb(15, 23, 42), rozetHistOrijinal);
            MevcutPictureBoxKartaTasi(kartIzgara, 1, 1, "Sıkıştırılmış Histogram", picHistYeni, Color.FromArgb(15, 23, 42), rozetHistYeni);

            picYeni.Cursor = Cursors.Cross;
            picYeni.MouseMove -= PicYeni_MouseOlayi;
            picYeni.MouseMove += PicYeni_MouseOlayi;
            picYeni.MouseDown -= PicYeni_MouseOlayi;
            picYeni.MouseDown += PicYeni_MouseOlayi;

            picHistOrijinal.Resize += (s, e) =>
            {
                if (orijinalGri != null) HistogramCiz(orijinalGri, picHistOrijinal, Color.FromArgb(250, 204, 21));
            };
            picHistYeni.Resize += (s, e) =>
            {
                if (islenmisGri != null) HistogramCiz(islenmisGri, picHistYeni, Color.FromArgb(56, 189, 248));
            };

            // Alt Analiz Şeridi
            ModernKartPanel altSerit = FormGezintiYardimcisi.AltAnalizSeridiOlustur(
                "Histogram sıkıştırma uygulamak için sağ panelden bir resim seçiniz.",
                out lblAnaliz,
                "Hazır"
            );

            anaYerlesim.Controls.Add(kartIzgara, 0, 0);
            anaYerlesim.Controls.Add(altSerit, 0, 1);
            pnlOrta.Controls.Add(anaYerlesim);

            this.Controls.Add(pnlOrta);
            FormGezintiYardimcisi.YerlesimiDuzenle(this, pnlUst, pnlSag, pnlOrta);
        }

        private void MevcutPictureBoxKartaTasi(
            TableLayoutPanel tablo,
            int sutun,
            int satir,
            string baslik,
            PictureBox mevcutPb,
            Color zeminRengi,
            ModernHapRozet sagUstRozet = null)
        {
            ModernKartPanel kart = new ModernKartPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(6),
                Padding = new Padding(12)
            };

            Panel pnlKartUst = new Panel
            {
                Dock = DockStyle.Top,
                Height = 28,
                BackColor = Color.White
            };

            Label lbl = new Label
            {
                Text = baslik,
                UseMnemonic = false,
                AutoSize = true,
                Location = new Point(0, 3),
                Font = new Font("Segoe UI Semibold", 9.6f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                BackColor = Color.White
            };
            pnlKartUst.Controls.Add(lbl);

            if (sagUstRozet != null)
            {
                sagUstRozet.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                sagUstRozet.Location = new Point(pnlKartUst.Width - sagUstRozet.Width - 2, 1);
                pnlKartUst.Resize += (s, e) =>
                {
                    sagUstRozet.Location = new Point(pnlKartUst.Width - sagUstRozet.Width - 2, 1);
                };
                pnlKartUst.Controls.Add(sagUstRozet);
                sagUstRozet.BringToFront();
            }

            mevcutPb.Parent?.Controls.Remove(mevcutPb);
            mevcutPb.Visible = true;
            mevcutPb.Dock = DockStyle.Fill;
            mevcutPb.SizeMode = PictureBoxSizeMode.Zoom;
            mevcutPb.BorderStyle = BorderStyle.None;
            mevcutPb.BackColor = zeminRengi;

            kart.Controls.Add(mevcutPb);
            kart.Controls.Add(pnlKartUst);
            tablo.Controls.Add(kart, sutun, satir);
        }

        private void btnResimSec_Click(object sender, EventArgs e)
        {
            string dosyaYolu = FormGezintiYardimcisi.ResimDosyasiSec("Histogram Sıkıştırma İçin Resim Seçiniz");
            if (string.IsNullOrEmpty(dosyaYolu)) return;

            using (Mat yuklenen = Cv2.ImRead(dosyaYolu, ImreadModes.Color))
            {
                if (yuklenen.Empty())
                {
                    MessageBox.Show("Resim yüklenemedi!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                orijinalGri?.Dispose();
                orijinalGri = new Mat();
                Cv2.CvtColor(yuklenen, orijinalGri, ColorConversionCodes.BGR2GRAY);
            }

            picOrijinal.Image?.Dispose();
            picOrijinal.Image = BitmapConverter.ToBitmap(orijinalGri);
            rozetOrijinal.MetinGuncelle($"{orijinalGri.Width}×{orijinalGri.Height}");

            HistogramCiz(orijinalGri, picHistOrijinal, Color.FromArgb(250, 204, 21));

            surguAlt.Value = 0;
            surguUst.Value = 255;
            SurguDegisti(true);
        }

        private void SurguDegisti(bool altDegisti)
        {
            if (surguAlt.Value > surguUst.Value)
            {
                if (altDegisti) surguUst.Value = surguAlt.Value;
                else surguAlt.Value = surguUst.Value;
            }

            lblAltDeger.Text = $"Alt Sınır (Min): {surguAlt.Value}";
            lblUstDeger.Text = $"Üst Sınır (Max): {surguUst.Value}";
            rozetYeni?.MetinGuncelle($"[{surguAlt.Value} - {surguUst.Value}]");

            ResmiSikistirVeHistogramGuncelle();
        }

        private void ResmiSikistirVeHistogramGuncelle()
        {
            if (orijinalGri == null) return;

            int alt = surguAlt.Value;
            int ust = surguUst.Value;

            islenmisGri?.Dispose();
            islenmisGri = new Mat(orijinalGri.Size(), MatType.CV_8UC1);

            int w = orijinalGri.Width;
            int h = orijinalGri.Height;

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    byte eskiRenk = orijinalGri.At<byte>(y, x);
                    int yeniRenk = alt + (int)(((float)eskiRenk / 255f) * (ust - alt));
                    yeniRenk = Math.Clamp(yeniRenk, 0, 255);
                    islenmisGri.Set<byte>(y, x, (byte)yeniRenk);
                }
            }

            picYeni.Image?.Dispose();
            picYeni.Image = BitmapConverter.ToBitmap(islenmisGri);

            HistogramCiz(islenmisGri, picHistYeni, Color.FromArgb(56, 189, 248));

            if (lblAnaliz != null)
            {
                lblAnaliz.Text = $"Sıkıştırma Aralığı: [{alt} - {ust}] • Dinamik Aralık Genişliği: {ust - alt} ton • Boyut: {w}×{h} px";
            }
        }

        private void HistogramCiz(Mat griMat, PictureBox hedefBox, Color sutunRengi)
        {
            if (griMat == null || hedefBox == null) return;

            int w = hedefBox.ClientSize.Width;
            int h = hedefBox.ClientSize.Height;
            if (w <= 5 || h <= 5) return;

            int[] pikselSayilari = new int[256];
            for (int y = 0; y < griMat.Height; y++)
            {
                for (int x = 0; x < griMat.Width; x++)
                {
                    byte val = griMat.At<byte>(y, x);
                    pikselSayilari[val]++;
                }
            }

            int maksPiksel = 1;
            for (int i = 0; i < 256; i++)
            {
                if (pikselSayilari[i] > maksPiksel)
                    maksPiksel = pikselSayilari[i];
            }

            Bitmap bmp = new Bitmap(w, h);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.FromArgb(15, 23, 42));

                using (Pen refKalem = new Pen(Color.FromArgb(51, 65, 85), 1))
                {
                    for (int i = 50; i <= 250; i += 50)
                    {
                        float xRef = (float)i / 255f * (w - 1);
                        g.DrawLine(refKalem, xRef, 0, xRef, h);
                    }
                }

                using (Pen kalem = new Pen(sutunRengi, 1.4f))
                {
                    for (int i = 0; i < 256; i++)
                    {
                        float x = (float)i / 255f * (w - 1);
                        float barBoyu = ((float)pikselSayilari[i] / maksPiksel) * (h - 18);
                        g.DrawLine(kalem, x, h - 1, x, h - 1 - barBoyu);
                    }
                }
            }

            hedefBox.Image?.Dispose();
            hedefBox.Image = bmp;
        }

        // Zoom modunda kenar boşluklarını hesaba katarak gerçek piksel koordinatını okuma
        private void PicYeni_MouseOlayi(object sender, MouseEventArgs e)
        {
            if (islenmisGri == null || picYeni.Image == null || picYeni.ClientSize.Width <= 0 || picYeni.ClientSize.Height <= 0)
                return;

            float kutuW = picYeni.ClientSize.Width;
            float kutuH = picYeni.ClientSize.Height;
            float resimW = islenmisGri.Width;
            float resimH = islenmisGri.Height;

            float olcek = Math.Min(kutuW / resimW, kutuH / resimH);
            float cizilenW = resimW * olcek;
            float cizilenH = resimH * olcek;
            float boslukX = (kutuW - cizilenW) / 2f;
            float boslukY = (kutuH - cizilenH) / 2f;

            int pikselX = (int)((e.X - boslukX) / olcek);
            int pikselY = (int)((e.Y - boslukY) / olcek);

            if (pikselX >= 0 && pikselX < islenmisGri.Width && pikselY >= 0 && pikselY < islenmisGri.Height)
            {
                byte gri = islenmisGri.At<byte>(pikselY, pikselX);
                txtPikselBilgi.Text = $"X:{pikselX}, Y:{pikselY} | R:{gri} G:{gri} B:{gri} | Gri:{gri}";
                lblAnaliz.Text = $"Seçilen Piksel: (X={pikselX}, Y={pikselY}) • Yoğunluk: {gri} (R={gri}, G={gri}, B={gri}) • Aralık: [{surguAlt.Value} - {surguUst.Value}]";
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            orijinalGri?.Dispose();
            islenmisGri?.Dispose();
            base.OnFormClosed(e);
        }
    }
}