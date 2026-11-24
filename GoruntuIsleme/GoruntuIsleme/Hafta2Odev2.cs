using System;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace GoruntuIsleme
{
    public partial class Hafta2Odev2 : Form
    {
        private Mat orijinalMat = null;
        private int[] histogramVerisi = new int[256];

        private ModernHapRozet rozetGiris;
        private ModernHapRozet rozetCikis;
        private ModernHapRozet rozetAralik;

        private ModernSurgu surguMin;
        private ModernSurgu surguMax;
        private Label lblMinDeger;
        private Label lblMaxDeger;
        private Label lblAnaliz;

        public Hafta2Odev2()
        {
            InitializeComponent();
            ModernArayuzuUygula();
        }

        private void ModernArayuzuUygula()
        {
            // 1. Form Temel Ayarları (1250x750, Standart Kenarlık ve X ile Ana Menüye Dönüş)
            FormGezintiYardimcisi.FormuHazirla(this);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Text = "Hafta 2 - Ödev 2: Aralık Eşikleme (Thresholding) ve Histogram Analizi";

            // Designer'daki tüm eski kontrolleri gizle
            foreach (Control ctrl in this.Controls)
            {
                ctrl.Visible = false;
            }

            // 2. Üst Gezinti Barı
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                form: this,
                baslik: "Hafta 2 - Ödev 2 | Aralık Eşikleme (Min - Max Thresholding) ve Histogram",
                oncekiYazi: "< Ödev 1",
                oncekiTikla: () =>
                {
                    Form odev1 = Application.OpenForms["Hafta2Odev1"];
                    if (odev1 != null)
                        FormGezintiYardimcisi.FormGecisYap(this, odev1);
                    else
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta2Odev1());
                },
                sonrakiYazi: "Ödev 3 >",
                sonrakiTikla: () => FormGezintiYardimcisi.FormGecisYap(this, new Hafta2Odev3())
            );

            // 3. Sağ Kontrol Paneli
            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 290);

            Label lblKontrolBaslik = new Label
            {
                Text = "Eşikleme Parametreleri",
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

            // Eşikleme Modu ComboBox
            Label lblMod = new Label
            {
                Text = "Eşikleme Modu:",
                Font = new Font("Segoe UI Semibold", 9.2f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                Location = new Point(18, 106),
                AutoSize = true
            };
            pnlSag.Controls.Add(lblMod);

            cmbMod.Parent?.Controls.Remove(cmbMod);
            cmbMod.Visible = true;
            cmbMod.Location = new Point(18, 128);
            cmbMod.Size = new Size(252, 32);
            cmbMod.Font = new Font("Segoe UI", 9.8f);
            cmbMod.DropDownStyle = ComboBoxStyle.DropDownList;
            if (cmbMod.Items.Count == 0)
            {
                cmbMod.Items.Add("Gri-Ton Eşikleme");
                cmbMod.Items.Add("Renkli Eşikleme (RGB)");
            }
            cmbMod.SelectedIndex = 0;
            cmbMod.SelectedIndexChanged -= CmbMod_SelectedIndexChanged;
            cmbMod.SelectedIndexChanged += CmbMod_SelectedIndexChanged;
            pnlSag.Controls.Add(cmbMod);

            // Alt Eşik Sürgüsü (Min: 0..255)
            Panel pnlMinSurgu = FormGezintiYardimcisi.KartAltSurguPaneliOlustur(
                solEtiket: "Alt Eşik (Min): 50",
                sagAralik: "0 .. 255",
                min: 0,
                max: 255,
                deger: 50,
                surgu: out surguMin,
                solLabel: out lblMinDeger
            );
            pnlMinSurgu.Location = new Point(18, 174);
            pnlMinSurgu.Width = 252;
            pnlSag.Controls.Add(pnlMinSurgu);

            // Üst Eşik Sürgüsü (Max: 0..255)
            Panel pnlMaxSurgu = FormGezintiYardimcisi.KartAltSurguPaneliOlustur(
                solEtiket: "Üst Eşik (Max): 200",
                sagAralik: "0 .. 255",
                min: 0,
                max: 255,
                deger: 200,
                surgu: out surguMax,
                solLabel: out lblMaxDeger
            );
            pnlMaxSurgu.Location = new Point(18, 236);
            pnlMaxSurgu.Width = 252;
            pnlSag.Controls.Add(pnlMaxSurgu);

            surguMin.Scroll += (s, e) => EsikSurguDegisti(true);
            surguMax.Scroll += (s, e) => EsikSurguDegisti(false);

            // Sıfırla Butonu
            ModernHapButon btnSifirla = FormGezintiYardimcisi.ModernButonOlustur("🔄 Varsayılan [50 - 200]", false, 252, 36);
            btnSifirla.Location = new Point(18, 302);
            btnSifirla.Click += (s, e) =>
            {
                surguMin.Value = 50;
                surguMax.Value = 200;
                EsikSurguDegisti(true);
            };
            pnlSag.Controls.Add(btnSifirla);

            // Açık Mavi Bilgi Kartı
            ModernKartPanel bilgiKarti = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                konum: new Point(18, 352),
                boyut: new Size(252, 220),
                icerikEtiketi: out Label lblBilgi
            );
            lblBilgi.Text =
                "Aralık Eşikleme Kuralı:\n\n" +
                "Min ≤ g(x, y) ≤ Max ise:\n" +
                "• Piksel değeri korunur.\n" +
                "Aralık dışındaysa:\n" +
                "• Piksel 0 (Siyah) yapılır.\n\n" +
                "Histogram üzerindeki kırmızı çizgiler seçili alt ve üst eşik sınırlarını gösterir.";
            pnlSag.Controls.Add(bilgiKarti);

            // 4. Orta Çalışma Alanı (3 Kart Yan Yana + Alt Analiz Şeridi)
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
            anaYerlesim.RowStyles.Add(new RowStyle(SizeType.Absolute, 54f));

            TableLayoutPanel kartIzgara = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1
            };
            kartIzgara.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32f));
            kartIzgara.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32f));
            kartIzgara.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 36f));
            kartIzgara.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            rozetGiris = new ModernHapRozet();
            rozetGiris.MetinGuncelle("Bekleniyor");

            rozetCikis = new ModernHapRozet();
            rozetCikis.MetinGuncelle("Gri-Ton");

            rozetAralik = new ModernHapRozet();
            rozetAralik.MetinGuncelle("[50 - 200]");

            MevcutPictureBoxKartaTasi(kartIzgara, 0, 0, "Orijinal Görüntü", picGiris, FormGezintiYardimcisi.TemaZemin, rozetGiris);
            MevcutPictureBoxKartaTasi(kartIzgara, 1, 0, "Eşiklenmiş Sonuç", picCikis, FormGezintiYardimcisi.TemaZemin, rozetCikis);
            MevcutPictureBoxKartaTasi(kartIzgara, 2, 0, "Histogram Grafiği", picHistogram, Color.FromArgb(15, 23, 42), rozetAralik);

            picHistogram.Resize += (s, e) => HistogramCiz();

            // Alt Analiz Şeridi
            ModernKartPanel altSerit = FormGezintiYardimcisi.AltAnalizSeridiOlustur(
                "Aralık eşikleme ve histogram grafiğini incelemek için sağ panelden bir resim seçiniz.",
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
                Height = 30,
                BackColor = Color.White
            };

            Label lbl = new Label
            {
                Text = baslik,
                UseMnemonic = false,
                AutoSize = true,
                Location = new Point(0, 4),
                Font = new Font("Segoe UI Semibold", 9.8f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                BackColor = Color.White
            };
            pnlKartUst.Controls.Add(lbl);

            if (sagUstRozet != null)
            {
                sagUstRozet.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                sagUstRozet.Location = new Point(pnlKartUst.Width - sagUstRozet.Width - 2, 2);
                pnlKartUst.Resize += (s, e) =>
                {
                    sagUstRozet.Location = new Point(pnlKartUst.Width - sagUstRozet.Width - 2, 2);
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

        private void CmbMod_SelectedIndexChanged(object sender, EventArgs e)
        {
            rozetCikis?.MetinGuncelle(cmbMod.SelectedIndex == 0 ? "Gri-Ton" : "RGB");
            EsiklemeUygula();
        }

        private void EsikSurguDegisti(bool minDegisti)
        {
            if (surguMin.Value > surguMax.Value)
            {
                if (minDegisti) surguMax.Value = surguMin.Value;
                else surguMin.Value = surguMax.Value;
            }

            lblMinDeger.Text = $"Alt Eşik (Min): {surguMin.Value}";
            lblMaxDeger.Text = $"Üst Eşik (Max): {surguMax.Value}";
            rozetAralik?.MetinGuncelle($"[{surguMin.Value} - {surguMax.Value}]");

            HistogramCiz();
            EsiklemeUygula();
        }

        private void btnResimSec_Click(object sender, EventArgs e)
        {
            string dosyaYolu = FormGezintiYardimcisi.ResimDosyasiSec("Eşikleme ve Histogram İçin Resim Seçiniz");
            if (string.IsNullOrEmpty(dosyaYolu)) return;

            orijinalMat?.Dispose();
            orijinalMat = Cv2.ImRead(dosyaYolu, ImreadModes.Color);

            if (orijinalMat.Empty())
            {
                MessageBox.Show("Resim yüklenemedi!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            picGiris.Image?.Dispose();
            picGiris.Image = BitmapConverter.ToBitmap(orijinalMat);

            rozetGiris.MetinGuncelle($"{orijinalMat.Width}×{orijinalMat.Height}");

            HistogramHesapla();
            HistogramCiz();
            EsiklemeUygula();
        }

        private void HistogramHesapla()
        {
            if (orijinalMat == null) return;
            Array.Clear(histogramVerisi, 0, histogramVerisi.Length);

            using (Mat gri = new Mat())
            {
                Cv2.CvtColor(orijinalMat, gri, ColorConversionCodes.BGR2GRAY);
                for (int y = 0; y < gri.Height; y++)
                {
                    for (int x = 0; x < gri.Width; x++)
                    {
                        byte val = gri.At<byte>(y, x);
                        histogramVerisi[val]++;
                    }
                }
            }
        }

        private void HistogramCiz()
        {
            if (picHistogram == null || orijinalMat == null) return;

            int w = picHistogram.ClientSize.Width;
            int h = picHistogram.ClientSize.Height;
            if (w <= 5 || h <= 5) return;

            Bitmap bmp = new Bitmap(w, h);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.FromArgb(15, 23, 42));

                int maxSayi = 1;
                for (int i = 0; i < 256; i++)
                {
                    if (histogramVerisi[i] > maxSayi)
                        maxSayi = histogramVerisi[i];
                }

                // Geçirilen aralık içindeki ve dışındaki çubukları belirginleştir
                int minT = surguMin.Value;
                int maxT = surguMax.Value;

                using (Pen penAktif = new Pen(Color.FromArgb(250, 204, 21), 1.5f))
                using (Pen penPasif = new Pen(Color.FromArgb(100, 116, 139), 1f))
                {
                    for (int i = 0; i < 256; i++)
                    {
                        float x = (float)i / 255f * (w - 1);
                        float barBoyu = ((float)histogramVerisi[i] / maxSayi) * (h - 24);
                        Pen seciliKalem = (i >= minT && i <= maxT) ? penAktif : penPasif;
                        g.DrawLine(seciliKalem, x, h - 1, x, h - 1 - barBoyu);
                    }
                }

                // Min ve Max eşik sınırlarını gösteren kırmızı dikey çizgiler
                using (Pen penKirmizi = new Pen(Color.FromArgb(239, 68, 68), 2f))
                {
                    float xMin = (float)minT / 255f * (w - 1);
                    float xMax = (float)maxT / 255f * (w - 1);

                    g.DrawLine(penKirmizi, xMin, 0, xMin, h);
                    g.DrawLine(penKirmizi, xMax, 0, xMax, h);
                }
            }

            picHistogram.Image?.Dispose();
            picHistogram.Image = bmp;
        }

        private void EsiklemeUygula()
        {
            if (orijinalMat == null) return;

            int minT = surguMin.Value;
            int maxT = surguMax.Value;
            bool griMod = (cmbMod != null && cmbMod.SelectedIndex == 0);

            if (griMod)
            {
                using (Mat gri = new Mat())
                {
                    Cv2.CvtColor(orijinalMat, gri, ColorConversionCodes.BGR2GRAY);
                    using (Mat sonucGri = new Mat(gri.Size(), MatType.CV_8UC1))
                    {
                        for (int y = 0; y < gri.Height; y++)
                        {
                            for (int x = 0; x < gri.Width; x++)
                            {
                                byte val = gri.At<byte>(y, x);
                                sonucGri.Set<byte>(y, x, (val >= minT && val <= maxT) ? val : (byte)0);
                            }
                        }

                        picCikis.Image?.Dispose();
                        picCikis.Image = BitmapConverter.ToBitmap(sonucGri);
                    }
                }
            }
            else
            {
                using (Mat sonucRenkli = new Mat(orijinalMat.Size(), MatType.CV_8UC3))
                {
                    for (int y = 0; y < orijinalMat.Height; y++)
                    {
                        for (int x = 0; x < orijinalMat.Width; x++)
                        {
                            Vec3b bgr = orijinalMat.At<Vec3b>(y, x);

                            byte B = (bgr.Item0 >= minT && bgr.Item0 <= maxT) ? bgr.Item0 : (byte)0;
                            byte G = (bgr.Item1 >= minT && bgr.Item1 <= maxT) ? bgr.Item1 : (byte)0;
                            byte R = (bgr.Item2 >= minT && bgr.Item2 <= maxT) ? bgr.Item2 : (byte)0;

                            sonucRenkli.Set(y, x, new Vec3b(B, G, R));
                        }
                    }

                    picCikis.Image?.Dispose();
                    picCikis.Image = BitmapConverter.ToBitmap(sonucRenkli);
                }
            }

            if (lblAnaliz != null)
            {
                string modAdi = griMod ? "Gri-Ton Eşikleme" : "Renkli (RGB) Eşikleme";
                lblAnaliz.Text = $"Mod: {modAdi} • Geçirilen Yoğunluk Aralığı: [{minT} - {maxT}] • Boyut: {orijinalMat.Width}×{orijinalMat.Height} px";
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            orijinalMat?.Dispose();
            base.OnFormClosed(e);
        }
    }
}