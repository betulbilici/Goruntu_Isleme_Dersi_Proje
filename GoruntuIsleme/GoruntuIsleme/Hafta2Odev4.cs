using System;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace GoruntuIsleme
{
    public partial class Hafta2Odev4 : Form
    {
        private Mat griMat = null;
        private int[] histogramVerisi = new int[256];
        private int x1 = 0;
        private int x2 = 255;

        private ModernHapRozet rozetGiris;
        private ModernHapRozet rozetCikis;
        private ModernHapRozet rozetAralik;
        private Label lblAnaliz;

        public Hafta2Odev4()
        {
            InitializeComponent();
            ModernArayuzuUygula();
        }

        private void ModernArayuzuUygula()
        {
            // 1. Form Temel Ayarları (1250x750, Standart Kenarlık ve X ile Ana Menüye Dönüş)
            FormGezintiYardimcisi.FormuHazirla(this);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Text = "Hafta 2 - Ödev 4: Doğrusal Kontrast Germe (Contrast Stretching)";

            // Designer'daki tüm eski kontrolleri gizle
            foreach (Control ctrl in this.Controls)
            {
                ctrl.Visible = false;
            }

            // 2. Üst Gezinti Barı
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                form: this,
                baslik: "Hafta 2 - Ödev 4 | Doğrusal Kontrast Germe (X1 - X2 → [0 - 255])",
                oncekiYazi: "< Ödev 3",
                oncekiTikla: () =>
                {
                    Form odev3 = Application.OpenForms["Hafta2Odev3"];
                    if (odev3 != null)
                        FormGezintiYardimcisi.FormGecisYap(this, odev3);
                    else
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta2Odev3());
                },
                sonrakiYazi: "Ödev 5 >",
                sonrakiTikla: () => FormGezintiYardimcisi.FormGecisYap(this, new Hafta2Odev5())
            );

            // 3. Sağ Kontrol Paneli
            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 290);

            Label lblKontrolBaslik = new Label
            {
                Text = "Kontrast Germe Noktaları",
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

            // X1 - Y1 ve X2 - Y2 Giriş Kutuları
            Label lblX1Baslik = new Label
            {
                Text = "X1 (Sol Tık):",
                Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                Location = new Point(18, 108),
                AutoSize = true
            };
            pnlSag.Controls.Add(lblX1Baslik);

            txtX1.Parent?.Controls.Remove(txtX1);
            txtX1.Visible = true;
            txtX1.Location = new Point(18, 130);
            txtX1.Size = new Size(120, 30);
            txtX1.Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold);
            txtX1.TextAlign = HorizontalAlignment.Center;
            txtX1.Text = "0";
            txtX1.TextChanged += (s, e) =>
            {
                if (int.TryParse(txtX1.Text, out int val))
                {
                    x1 = Math.Clamp(val, 0, 255);
                    rozetAralik?.MetinGuncelle($"[{x1} - {x2}]");
                    HistogramCiz();
                }
            };
            pnlSag.Controls.Add(txtX1);

            Label lblY1Baslik = new Label
            {
                Text = "Y1 (Hedef Alt):",
                Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.AcikYazi,
                Location = new Point(150, 108),
                AutoSize = true
            };
            pnlSag.Controls.Add(lblY1Baslik);

            txtY1.Parent?.Controls.Remove(txtY1);
            txtY1.Visible = true;
            txtY1.Location = new Point(150, 130);
            txtY1.Size = new Size(120, 30);
            txtY1.Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold);
            txtY1.TextAlign = HorizontalAlignment.Center;
            txtY1.ReadOnly = true;
            txtY1.BackColor = FormGezintiYardimcisi.TemaZemin;
            txtY1.Text = "0";
            pnlSag.Controls.Add(txtY1);

            Label lblX2Baslik = new Label
            {
                Text = "X2 (Sağ Tık):",
                Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                Location = new Point(18, 170),
                AutoSize = true
            };
            pnlSag.Controls.Add(lblX2Baslik);

            txtX2.Parent?.Controls.Remove(txtX2);
            txtX2.Visible = true;
            txtX2.Location = new Point(18, 192);
            txtX2.Size = new Size(120, 30);
            txtX2.Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold);
            txtX2.TextAlign = HorizontalAlignment.Center;
            txtX2.Text = "255";
            txtX2.TextChanged += (s, e) =>
            {
                if (int.TryParse(txtX2.Text, out int val))
                {
                    x2 = Math.Clamp(val, 0, 255);
                    rozetAralik?.MetinGuncelle($"[{x1} - {x2}]");
                    HistogramCiz();
                }
            };
            pnlSag.Controls.Add(txtX2);

            Label lblY2Baslik = new Label
            {
                Text = "Y2 (Hedef Üst):",
                Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.AcikYazi,
                Location = new Point(150, 170),
                AutoSize = true
            };
            pnlSag.Controls.Add(lblY2Baslik);

            txtY2.Parent?.Controls.Remove(txtY2);
            txtY2.Visible = true;
            txtY2.Location = new Point(150, 192);
            txtY2.Size = new Size(120, 30);
            txtY2.Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold);
            txtY2.TextAlign = HorizontalAlignment.Center;
            txtY2.ReadOnly = true;
            txtY2.BackColor = FormGezintiYardimcisi.TemaZemin;
            txtY2.Text = "255";
            pnlSag.Controls.Add(txtY2);

            // Kontrast Uygula Butonu
            ModernHapButon btnModernKontrast = FormGezintiYardimcisi.ModernButonOlustur("⚡ Kontrast Uygula", false, 252, 40);
            btnModernKontrast.Location = new Point(18, 238);
            btnModernKontrast.Click += btnKontrast_Click;
            pnlSag.Controls.Add(btnModernKontrast);

            // Otomatik Min-Max Algıla Butonu
            ModernHapButon btnOtoMinMax = FormGezintiYardimcisi.ModernButonOlustur("🎯 Otomatik Min-Max", false, 252, 36);
            btnOtoMinMax.Location = new Point(18, 286);
            btnOtoMinMax.Click += (s, e) =>
            {
                if (griMat == null) return;
                Cv2.MinMaxLoc(griMat, out double minVal, out double maxVal);
                x1 = (int)minVal;
                x2 = (int)maxVal;
                txtX1.Text = x1.ToString();
                txtX2.Text = x2.ToString();
                HistogramCiz();
                btnKontrast_Click(s, e);
            };
            pnlSag.Controls.Add(btnOtoMinMax);

            // Açık Mavi Formül ve Açıklama Kartı
            ModernKartPanel bilgiKarti = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                konum: new Point(18, 336),
                boyut: new Size(252, 235),
                icerikEtiketi: out Label lblBilgiCard
            );
            lblBilgiCard.Text =
                "Kontrast Germe Formülü:\n\n" +
                "g'(x, y) = ((g(x, y) - X1) / (X2 - X1)) × 255\n\n" +
                "• Histogram üzerinde Sol Tık: X1 alt sınırını belirler.\n" +
                "• Histogram üzerinde Sağ Tık: X2 üst sınırını belirler.\n" +
                "• Kırmızı eğik çizgi doğrusal gerdirme dönüşüm eğrisini gösterir.";
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
            rozetGiris.MetinGuncelle("Gri-Ton");

            rozetCikis = new ModernHapRozet();
            rozetCikis.MetinGuncelle("Bekleniyor");

            rozetAralik = new ModernHapRozet();
            rozetAralik.MetinGuncelle("[0 - 255]");

            MevcutPictureBoxKartaTasi(kartIzgara, 0, 0, "Orijinal Gri Görüntü", picGiris, FormGezintiYardimcisi.TemaZemin, rozetGiris);
            MevcutPictureBoxKartaTasi(kartIzgara, 1, 0, "Kontrast Gerilmiş Sonuç", picCikis, FormGezintiYardimcisi.TemaZemin, rozetCikis);
            MevcutPictureBoxKartaTasi(kartIzgara, 2, 0, "Histogram & Eğri", picHistogram, Color.FromArgb(15, 23, 42), rozetAralik);

            picHistogram.Cursor = Cursors.Hand;
            picHistogram.SizeMode = PictureBoxSizeMode.Normal;
            picHistogram.MouseDown -= PicHistogram_MouseDown;
            picHistogram.MouseDown += PicHistogram_MouseDown;
            picHistogram.Resize += (s, e) => HistogramCiz();

            // Alt Analiz Şeridi
            ModernKartPanel altSerit = FormGezintiYardimcisi.AltAnalizSeridiOlustur(
                "Histogram üzerinde Sol Tık ile X1, Sağ Tık ile X2 seçip 'Kontrast Uygula' butonuna basınız.",
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

        private void btnResimSec_Click(object sender, EventArgs e)
        {
            string dosyaYolu = FormGezintiYardimcisi.ResimDosyasiSec("Kontrast Germe İçin Resim Seçiniz");
            if (string.IsNullOrEmpty(dosyaYolu)) return;

            using (Mat temp = Cv2.ImRead(dosyaYolu, ImreadModes.Color))
            {
                if (temp.Empty())
                {
                    MessageBox.Show("Resim yüklenemedi!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                griMat?.Dispose();
                griMat = new Mat();
                Cv2.CvtColor(temp, griMat, ColorConversionCodes.BGR2GRAY);
            }

            picGiris.Image?.Dispose();
            picGiris.Image = BitmapConverter.ToBitmap(griMat);
            rozetGiris.MetinGuncelle($"{griMat.Width}×{griMat.Height}");

            HistogramHesapla();
            HistogramCiz();
            lblAnaliz.Text = $"Resim yüklendi ({griMat.Width}×{griMat.Height} px) • Histogram üzerinde Sol Tık (X1) ve Sağ Tık (X2) yaparak sınırları seçiniz.";
        }

        private void PicHistogram_MouseDown(object sender, MouseEventArgs e)
        {
            if (griMat == null || picHistogram.ClientSize.Width <= 1) return;

            int tiklananRenk = Math.Clamp((int)((float)e.X / (picHistogram.ClientSize.Width - 1) * 255f), 0, 255);

            if (e.Button == MouseButtons.Left)
            {
                x1 = tiklananRenk;
                txtX1.Text = x1.ToString();
            }
            else if (e.Button == MouseButtons.Right)
            {
                x2 = tiklananRenk;
                txtX2.Text = x2.ToString();
            }

            rozetAralik?.MetinGuncelle($"[{x1} - {x2}]");
            HistogramCiz();
        }

        private void HistogramHesapla()
        {
            if (griMat == null) return;
            Array.Clear(histogramVerisi, 0, histogramVerisi.Length);

            for (int y = 0; y < griMat.Height; y++)
            {
                for (int x = 0; x < griMat.Width; x++)
                {
                    byte val = griMat.At<byte>(y, x);
                    histogramVerisi[val]++;
                }
            }
        }

        private void HistogramCiz()
        {
            if (picHistogram == null || griMat == null) return;

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

                using (Pen penSari = new Pen(Color.FromArgb(250, 204, 21), 1.4f))
                {
                    for (int i = 0; i < 256; i++)
                    {
                        float x = (float)i / 255f * (w - 1);
                        float barBoyu = ((float)histogramVerisi[i] / maxSayi) * (h - 24);
                        g.DrawLine(penSari, x, h - 1, x, h - 1 - barBoyu);
                    }
                }

                using (Pen penKirmizi = new Pen(Color.FromArgb(239, 68, 68), 2f))
                using (Brush brushNokta = new SolidBrush(Color.FromArgb(239, 68, 68)))
                {
                    float pos1 = (float)x1 / 255f * (w - 1);
                    float pos2 = (float)x2 / 255f * (w - 1);

                    g.DrawLine(penKirmizi, pos1, 0, pos1, h);
                    g.FillEllipse(brushNokta, pos1 - 4, h - 15, 8, 8);

                    g.DrawLine(penKirmizi, pos2, 0, pos2, h);
                    g.FillEllipse(brushNokta, pos2 - 4, h - 15, 8, 8);

                    g.DrawLine(penKirmizi, pos1, h - 15, pos2, 12);
                }
            }

            picHistogram.Image?.Dispose();
            picHistogram.Image = bmp;
        }

        private void btnKontrast_Click(object sender, EventArgs e)
        {
            if (griMat == null)
            {
                MessageBox.Show("Lütfen önce bir resim seçiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int.TryParse(txtX1.Text, out x1);
            int.TryParse(txtX2.Text, out x2);

            if (x1 >= x2)
            {
                MessageBox.Show("X1 değeri X2 değerinden küçük olmalıdır!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (Mat sonucMat = new Mat(griMat.Size(), MatType.CV_8UC1))
            {
                int genislik = griMat.Width;
                int yukseklik = griMat.Height;

                for (int y = 0; y < yukseklik; y++)
                {
                    for (int x = 0; x < genislik; x++)
                    {
                        byte piksel = griMat.At<byte>(y, x);

                        int yeniRenk;
                        if (piksel <= x1)
                            yeniRenk = 0;
                        else if (piksel >= x2)
                            yeniRenk = 255;
                        else
                            yeniRenk = (int)(((float)(piksel - x1) / (x2 - x1)) * 255);

                        yeniRenk = Math.Clamp(yeniRenk, 0, 255);
                        sonucMat.Set<byte>(y, x, (byte)yeniRenk);
                    }
                }

                picCikis.Image?.Dispose();
                picCikis.Image = BitmapConverter.ToBitmap(sonucMat);
            }

            rozetCikis.MetinGuncelle("Gerildi");
            rozetAralik.MetinGuncelle($"[{x1} - {x2}]");
            lblAnaliz.Text = $"Kontrast germe tamamlandı • Giriş Aralığı: [{x1} - {x2}] → Çıkış Aralığı: [0 - 255]";
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            griMat?.Dispose();
            base.OnFormClosed(e);
        }
    }
}