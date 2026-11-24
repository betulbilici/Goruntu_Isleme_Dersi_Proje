using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace GoruntuIsleme
{
    public partial class Hafta2Odev6 : Form
    {
        private Mat orijinalMat = null;
        private List<Point> poligonNoktalari = new List<Point>();
        private bool poligonKapandi = false;
        private int[] roiHistogram = new int[256];

        private ModernHapRozet rozetResim;
        private ModernHapRozet rozetHist;
        private Label lblAnaliz;

        private ModernSurgu surguParlaklik;
        private ModernSurgu surguKontrast;
        private ModernSurgu surguR;
        private ModernSurgu surguG;
        private ModernSurgu surguB;

        private Label lblValParlaklik;
        private Label lblValKontrast;
        private Label lblValR;
        private Label lblValG;
        private Label lblValB;

        public Hafta2Odev6()
        {
            InitializeComponent();
            ModernArayuzuUygula();
        }

        private void ModernArayuzuUygula()
        {
            // 1. Form Temel Ayarları (1250x750, Standart Kenarlık ve X ile Ana Menüye Dönüş)
            FormGezintiYardimcisi.FormuHazirla(this);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Text = "Hafta 2 - Ödev 6: Poligon Seçimi (ROI) ve Bölgesel Görüntü İşleme";

            // Designer'daki tüm eski kontrolleri gizle
            foreach (Control ctrl in this.Controls)
            {
                ctrl.Visible = false;
            }

            // 2. Üst Gezinti Barı
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                form: this,
                baslik: "Hafta 2 - Ödev 6 | Poligon Bölgesi (ROI) Seçimi ve Bölgesel Parlaklık / Kontrast / RGB",
                oncekiYazi: "< Ödev 5",
                oncekiTikla: () =>
                {
                    Form odev5 = Application.OpenForms["Hafta2Odev5"];
                    if (odev5 != null)
                        FormGezintiYardimcisi.FormGecisYap(this, odev5);
                    else
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta2Odev5());
                },
                sonrakiYazi: "Ödev 7 >",
                sonrakiTikla: () => FormGezintiYardimcisi.FormGecisYap(this, new Hafta2Odev7())
            );

            // 3. Sağ Kontrol Paneli
            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 290);

            Label lblKontrolBaslik = new Label
            {
                Text = "ROI & Bölgesel Ayarlar",
                Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                Location = new Point(18, 14),
                AutoSize = true
            };
            pnlSag.Controls.Add(lblKontrolBaslik);

            // Resim Seç Butonu
            ModernHapButon btnModernResimSec = FormGezintiYardimcisi.ModernButonOlustur("📁 Resim Seç", true, 252, 36);
            btnModernResimSec.Location = new Point(18, 42);
            btnModernResimSec.Click += btnResimSec_Click;
            pnlSag.Controls.Add(btnModernResimSec);

            // Poligon Oluştur & Çizimi Temizle Butonları (Yan Yana)
            ModernHapButon btnModernKapat = FormGezintiYardimcisi.ModernButonOlustur("⬠ Poligonu Kapat", false, 123, 34);
            btnModernKapat.Location = new Point(18, 84);
            btnModernKapat.Click += btnKapat_Click;
            pnlSag.Controls.Add(btnModernKapat);

            ModernHapButon btnModernTemizle = FormGezintiYardimcisi.ModernButonOlustur("🔄 Temizle", false, 121, 34);
            btnModernTemizle.Location = new Point(149, 84);
            btnModernTemizle.Click += btnTemizle_Click;
            pnlSag.Controls.Add(btnModernTemizle);

            // 5 Adet Kompakt Modern Sürgü (Parlaklık, Kontrast, R, G, B)
            surguParlaklik = KompaktSurguEkle(pnlSag, "Parlaklık", 128, out lblValParlaklik);
            surguKontrast = KompaktSurguEkle(pnlSag, "Kontrast (F)", 178, out lblValKontrast);
            surguR = KompaktSurguEkle(pnlSag, "Kırmızı (R)", 228, out lblValR);
            surguG = KompaktSurguEkle(pnlSag, "Yeşil (G)", 278, out lblValG);
            surguB = KompaktSurguEkle(pnlSag, "Mavi (B)", 328, out lblValB);

            // b Şıkkı Ray-Casting Açıklama Butonu
            ModernHapButon btnModernAciklama = FormGezintiYardimcisi.ModernButonOlustur("📐 Yöntem Açıklaması (b Şıkkı)", false, 252, 36);
            btnModernAciklama.Location = new Point(18, 384);
            btnModernAciklama.Click += btnAciklama_Click;
            pnlSag.Controls.Add(btnModernAciklama);

            // Kısa Kullanım İpucu Kartı
            ModernKartPanel ipucuKarti = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                konum: new Point(18, 428),
                boyut: new Size(252, 145),
                icerikEtiketi: out Label lblIpucu
            );
            lblIpucu.Text =
                "Poligon Seçimi:\n" +
                "• Sol Tık: Köşe noktası ekler.\n" +
                "• Sağ Tık veya 'Poligonu Kapat': Seçili alanı kapatır.\n" +
                "• Sürgüler yalnızca seçili alan içindeki piksellere etki eder.";
            pnlSag.Controls.Add(ipucuKarti);

            // 4. Orta Çalışma Alanı (Üst: Ana Resim Kartı | Alt: Histogram + Açıklama Kartı | En Alt: Analiz Şeridi)
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
                RowCount = 3
            };
            anaYerlesim.RowStyles.Add(new RowStyle(SizeType.Percent, 58f));
            anaYerlesim.RowStyles.Add(new RowStyle(SizeType.Percent, 42f));
            anaYerlesim.RowStyles.Add(new RowStyle(SizeType.Absolute, 50f));

            // Üst Kart: Poligon Seçim Alanı
            ModernKartPanel ustResimKart = new ModernKartPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(6),
                Padding = new Padding(12)
            };

            Panel pnlResimUst = new Panel
            {
                Dock = DockStyle.Top,
                Height = 28,
                BackColor = Color.White
            };

            Label lblResimBaslik = new Label
            {
                Text = "Görüntü Üzerinde Poligon (ROI) Seçimi — Sol Tık: Nokta Ekle | Sağ Tık: Poligonu Kapat",
                UseMnemonic = false,
                AutoSize = true,
                Location = new Point(0, 4),
                Font = new Font("Segoe UI Semibold", 9.6f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi
            };
            pnlResimUst.Controls.Add(lblResimBaslik);

            rozetResim = new ModernHapRozet();
            rozetResim.MetinGuncelle("0 Nokta");
            rozetResim.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            rozetResim.Location = new Point(pnlResimUst.Width - rozetResim.Width - 2, 1);
            pnlResimUst.Resize += (s, e) =>
            {
                rozetResim.Location = new Point(pnlResimUst.Width - rozetResim.Width - 2, 1);
            };
            pnlResimUst.Controls.Add(rozetResim);

            picResim.Parent?.Controls.Remove(picResim);
            picResim.Visible = true;
            picResim.Dock = DockStyle.Fill;
            picResim.SizeMode = PictureBoxSizeMode.Zoom;
            picResim.BorderStyle = BorderStyle.None;
            picResim.BackColor = FormGezintiYardimcisi.TemaZemin;
            picResim.Cursor = Cursors.Cross;
            picResim.MouseDown -= PicResim_MouseDown;
            picResim.MouseDown += PicResim_MouseDown;

            ustResimKart.Controls.Add(picResim);
            ustResimKart.Controls.Add(pnlResimUst);

            // Alt Satır: Sol (Ray-Casting Açıklaması) + Sağ (Seçili Bölge Histogramı)
            TableLayoutPanel altIzgara = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1
            };
            altIzgara.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            altIzgara.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            altIzgara.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            // Sol Alt Kart: Ray-Casting Açıklaması
            ModernKartPanel aciklamaKart = new ModernKartPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(6),
                Padding = new Padding(12)
            };

            Label lblAciklamaBaslik = new Label
            {
                Text = "Analitik Geometri / Ray-Casting Açıklaması (b Şıkkı)",
                Dock = DockStyle.Top,
                Height = 26,
                Font = new Font("Segoe UI Semibold", 9.6f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                BackColor = Color.White
            };

            txtAciklama.Parent?.Controls.Remove(txtAciklama);
            txtAciklama.Visible = true;
            txtAciklama.Dock = DockStyle.Fill;
            txtAciklama.BorderStyle = BorderStyle.None;
            txtAciklama.BackColor = Color.White;
            txtAciklama.Font = new Font("Segoe UI", 9.2f);
            txtAciklama.ReadOnly = true;
            txtAciklama.Multiline = true;
            txtAciklama.ScrollBars = ScrollBars.Vertical;

            aciklamaKart.Controls.Add(txtAciklama);
            aciklamaKart.Controls.Add(lblAciklamaBaslik);

            // Sağ Alt Kart: Seçili Bölge Histogramı
            ModernKartPanel histKart = new ModernKartPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(6),
                Padding = new Padding(12)
            };

            Panel pnlHistUst = new Panel
            {
                Dock = DockStyle.Top,
                Height = 26,
                BackColor = Color.White
            };

            Label lblHistBaslik = new Label
            {
                Text = "Seçili Bölgenin (ROI) Histogramı",
                AutoSize = true,
                Location = new Point(0, 3),
                Font = new Font("Segoe UI Semibold", 9.6f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi
            };
            pnlHistUst.Controls.Add(lblHistBaslik);

            rozetHist = new ModernHapRozet();
            rozetHist.MetinGuncelle("ROI Bekleniyor");
            rozetHist.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            rozetHist.Location = new Point(pnlHistUst.Width - rozetHist.Width - 2, 1);
            pnlHistUst.Resize += (s, e) =>
            {
                rozetHist.Location = new Point(pnlHistUst.Width - rozetHist.Width - 2, 1);
            };
            pnlHistUst.Controls.Add(rozetHist);

            picHistogram.Parent?.Controls.Remove(picHistogram);
            picHistogram.Visible = true;
            picHistogram.Dock = DockStyle.Fill;
            picHistogram.BorderStyle = BorderStyle.None;
            picHistogram.BackColor = Color.FromArgb(15, 23, 42);
            picHistogram.Resize += (s, e) =>
            {
                if (poligonKapandi) RoiHistogramCiz();
            };

            histKart.Controls.Add(picHistogram);
            histKart.Controls.Add(pnlHistUst);

            altIzgara.Controls.Add(aciklamaKart, 0, 0);
            altIzgara.Controls.Add(histKart, 1, 0);

            // En Alt Analiz Şeridi
            ModernKartPanel altSerit = FormGezintiYardimcisi.AltAnalizSeridiOlustur(
                "Resim seçip poligon köşelerini işaretleyin; poligon kapandıktan sonra sürgülerle bölgeyi düzenleyin.",
                out lblAnaliz,
                "Ray-Casting"
            );

            anaYerlesim.Controls.Add(ustResimKart, 0, 0);
            anaYerlesim.Controls.Add(altIzgara, 0, 1);
            anaYerlesim.Controls.Add(altSerit, 0, 2);
            pnlOrta.Controls.Add(anaYerlesim);

            this.Controls.Add(pnlOrta);
            FormGezintiYardimcisi.YerlesimiDuzenle(this, pnlUst, pnlSag, pnlOrta);

            // Başlangıçta açıklamayı da doldur
            btnAciklama_Click(null, null);
        }

        private ModernSurgu KompaktSurguEkle(Panel pnl, string baslik, int y, out Label lblDeger)
        {
            Label lblAd = new Label
            {
                Text = baslik,
                Location = new Point(18, y),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 8.8f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi
            };
            pnl.Controls.Add(lblAd);

            lblDeger = new Label
            {
                Text = "0",
                Location = new Point(215, y),
                Size = new Size(55, 18),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI Semibold", 8.8f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi
            };
            pnl.Controls.Add(lblDeger);

            Label degerRef = lblDeger;
            ModernSurgu surgu = new ModernSurgu
            {
                Minimum = -100,
                Maximum = 100,
                Value = 0,
                Location = new Point(18, y + 19),
                Size = new Size(252, 22)
            };
            surgu.Scroll += (s, e) =>
            {
                int v = surgu.Value;
                degerRef.Text = v > 0 ? $"+{v}" : v.ToString();
                IslemleriUygula();
            };
            pnl.Controls.Add(surgu);
            return surgu;
        }

        private void btnResimSec_Click(object sender, EventArgs e)
        {
            string dosyaYolu = FormGezintiYardimcisi.ResimDosyasiSec("Bölgesel İşlem (ROI) İçin Resim Seçiniz");
            if (string.IsNullOrEmpty(dosyaYolu)) return;

            orijinalMat?.Dispose();
            orijinalMat = Cv2.ImRead(dosyaYolu, ImreadModes.Color);

            if (orijinalMat.Empty())
            {
                MessageBox.Show("Resim yüklenemedi!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnTemizle_Click(null, null);
            lblAnaliz.Text = $"Resim yüklendi ({orijinalMat.Width}×{orijinalMat.Height} px) • Sol tıkla poligon noktalarını işaretleyin.";
        }

        // Zoom modundaki kenar boşluklarını hesaba katarak gerçek resim koordinatını hesaplar
        private bool EkranKoordinatiniResmeCevir(int mouseX, int mouseY, out int gercekX, out int gercekY)
        {
            gercekX = 0;
            gercekY = 0;
            if (orijinalMat == null || picResim.ClientSize.Width <= 0 || picResim.ClientSize.Height <= 0)
                return false;

            float kutuW = picResim.ClientSize.Width;
            float kutuH = picResim.ClientSize.Height;
            float resimW = orijinalMat.Width;
            float resimH = orijinalMat.Height;

            float olcek = Math.Min(kutuW / resimW, kutuH / resimH);
            float cizilenW = resimW * olcek;
            float cizilenH = resimH * olcek;
            float boslukX = (kutuW - cizilenW) / 2f;
            float boslukY = (kutuH - cizilenH) / 2f;

            gercekX = (int)((mouseX - boslukX) / olcek);
            gercekY = (int)((mouseY - boslukY) / olcek);

            return gercekX >= 0 && gercekX < orijinalMat.Width && gercekY >= 0 && gercekY < orijinalMat.Height;
        }

        private void PicResim_MouseDown(object sender, MouseEventArgs e)
        {
            if (orijinalMat == null || poligonKapandi) return;

            if (e.Button == MouseButtons.Left)
            {
                if (EkranKoordinatiniResmeCevir(e.X, e.Y, out int gx, out int gy))
                {
                    poligonNoktalari.Add(new Point(gx, gy));
                    rozetResim.MetinGuncelle($"{poligonNoktalari.Count} Nokta");
                    lblAnaliz.Text = $"Köşe {poligonNoktalari.Count} eklendi: ({gx}, {gy}) • Kapatmak için Sağ Tık yapınız.";
                    CizimiGuncelle();
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (poligonNoktalari.Count >= 3)
                {
                    poligonKapandi = true;
                    rozetResim.MetinGuncelle($"{poligonNoktalari.Count} Köşe (Kapalı)");
                    CizimiGuncelle();
                }
                else
                {
                    MessageBox.Show("Poligonu kapatmak için en az 3 nokta işaretlemelisiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnKapat_Click(object sender, EventArgs e)
        {
            if (poligonNoktalari.Count >= 3)
            {
                poligonKapandi = true;
                rozetResim.MetinGuncelle($"{poligonNoktalari.Count} Köşe (Kapalı)");
                CizimiGuncelle();
            }
            else
            {
                MessageBox.Show("Poligon oluşturmak için resim üzerinde en az 3 noktaya tıklamalısınız.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnTemizle_Click(object sender, EventArgs e)
        {
            poligonNoktalari.Clear();
            poligonKapandi = false;

            if (surguParlaklik != null) { surguParlaklik.Value = 0; lblValParlaklik.Text = "0"; }
            if (surguKontrast != null) { surguKontrast.Value = 0; lblValKontrast.Text = "0"; }
            if (surguR != null) { surguR.Value = 0; lblValR.Text = "0"; }
            if (surguG != null) { surguG.Value = 0; lblValG.Text = "0"; }
            if (surguB != null) { surguB.Value = 0; lblValB.Text = "0"; }

            if (orijinalMat != null)
            {
                picResim.Image?.Dispose();
                picResim.Image = BitmapConverter.ToBitmap(orijinalMat);
            }

            picHistogram.Image?.Dispose();
            picHistogram.Image = null;
            rozetResim?.MetinGuncelle("0 Nokta");
            rozetHist?.MetinGuncelle("ROI Bekleniyor");
        }

        private bool NoktaPoligonunIcindeMi(int x, int y, List<Point> poligon)
        {
            bool iceride = false;
            int n = poligon.Count;

            for (int i = 0, j = n - 1; i < n; j = i++)
            {
                if (((poligon[i].Y > y) != (poligon[j].Y > y)) &&
                    (x < (poligon[j].X - poligon[i].X) * (y - poligon[i].Y) / (poligon[j].Y - poligon[i].Y) + poligon[i].X))
                {
                    iceride = !iceride;
                }
            }
            return iceride;
        }

        private void IslemleriUygula()
        {
            if (orijinalMat == null || !poligonKapandi || poligonNoktalari.Count < 3) return;

            using (Mat islenmisMat = orijinalMat.Clone())
            {
                Array.Clear(roiHistogram, 0, 256);

                int pVal = surguParlaklik.Value;
                double cVal = surguKontrast.Value;
                int rVal = surguR.Value;
                int gVal = surguG.Value;
                int bVal = surguB.Value;

                double fKontrast = 1.0;
                if (cVal != 0)
                {
                    fKontrast = (259.0 * (cVal + 255.0)) / (255.0 * (259.0 - cVal));
                }

                int minX = int.MaxValue, maxX = int.MinValue, minY = int.MaxValue, maxY = int.MinValue;
                foreach (var pt in poligonNoktalari)
                {
                    if (pt.X < minX) minX = pt.X;
                    if (pt.X > maxX) maxX = pt.X;
                    if (pt.Y < minY) minY = pt.Y;
                    if (pt.Y > maxY) maxY = pt.Y;
                }

                minX = Math.Max(0, minX);
                maxX = Math.Min(orijinalMat.Width - 1, maxX);
                minY = Math.Max(0, minY);
                maxY = Math.Min(orijinalMat.Height - 1, maxY);

                int seciliPikselSayisi = 0;

                for (int y = minY; y <= maxY; y++)
                {
                    for (int x = minX; x <= maxX; x++)
                    {
                        if (NoktaPoligonunIcindeMi(x, y, poligonNoktalari))
                        {
                            seciliPikselSayisi++;
                            Vec3b bgr = orijinalMat.At<Vec3b>(y, x);

                            double b = bgr.Item0 + pVal + bVal;
                            double g = bgr.Item1 + pVal + gVal;
                            double r = bgr.Item2 + pVal + rVal;

                            if (cVal != 0)
                            {
                                b = (fKontrast * (b - 128.0)) + 128.0;
                                g = (fKontrast * (g - 128.0)) + 128.0;
                                r = (fKontrast * (r - 128.0)) + 128.0;
                            }

                            b = Math.Clamp(b, 0, 255);
                            g = Math.Clamp(g, 0, 255);
                            r = Math.Clamp(r, 0, 255);

                            islenmisMat.Set(y, x, new Vec3b((byte)b, (byte)g, (byte)r));

                            int griDeger = Math.Clamp((int)(r * 0.299 + g * 0.587 + b * 0.114), 0, 255);
                            roiHistogram[griDeger]++;
                        }
                    }
                }

                Bitmap bmp = BitmapConverter.ToBitmap(islenmisMat);
                using (Graphics gr = Graphics.FromImage(bmp))
                using (Pen kesikliKalem = new Pen(Color.Yellow, 2))
                {
                    kesikliKalem.DashStyle = DashStyle.Dash;
                    gr.DrawPolygon(kesikliKalem, poligonNoktalari.ToArray());
                }

                picResim.Image?.Dispose();
                picResim.Image = bmp;

                rozetHist.MetinGuncelle($"{seciliPikselSayisi:N0} px");
                lblAnaliz.Text = $"ROI Aktif ({poligonNoktalari.Count} köşe, {seciliPikselSayisi:N0} piksel) • P:{pVal}, C:{cVal:0}, R:{rVal}, G:{gVal}, B:{bVal}";
            }

            RoiHistogramCiz();
        }

        private void CizimiGuncelle()
        {
            if (orijinalMat == null) return;

            Bitmap bmp = BitmapConverter.ToBitmap(orijinalMat);
            using (Graphics g = Graphics.FromImage(bmp))
            using (Pen kesikliKalem = new Pen(Color.Yellow, 2))
            using (Brush noktaFircasi = new SolidBrush(Color.Red))
            {
                kesikliKalem.DashStyle = DashStyle.Dash;

                for (int i = 0; i < poligonNoktalari.Count; i++)
                {
                    g.FillEllipse(noktaFircasi, poligonNoktalari[i].X - 4, poligonNoktalari[i].Y - 4, 8, 8);

                    if (i > 0)
                    {
                        g.DrawLine(kesikliKalem, poligonNoktalari[i - 1], poligonNoktalari[i]);
                    }
                }

                if (poligonKapandi && poligonNoktalari.Count >= 3)
                {
                    g.DrawLine(kesikliKalem, poligonNoktalari[poligonNoktalari.Count - 1], poligonNoktalari[0]);
                }
            }

            picResim.Image?.Dispose();
            picResim.Image = bmp;

            if (poligonKapandi)
            {
                IslemleriUygula();
            }
        }

        private void RoiHistogramCiz()
        {
            if (picHistogram == null || picHistogram.ClientSize.Width <= 5 || picHistogram.ClientSize.Height <= 5) return;

            int w = picHistogram.ClientSize.Width;
            int h = picHistogram.ClientSize.Height;
            Bitmap bmp = new Bitmap(w, h);

            int max = 1;
            for (int i = 0; i < 256; i++)
            {
                if (roiHistogram[i] > max) max = roiHistogram[i];
            }

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.FromArgb(15, 23, 42));

                using (Pen p = new Pen(Color.FromArgb(56, 189, 248), 1.4f))
                {
                    for (int i = 0; i < 256; i++)
                    {
                        float x = (float)i / 255f * (w - 1);
                        float barH = ((float)roiHistogram[i] / max) * (h - 14);
                        g.DrawLine(p, x, h - 1, x, h - 1 - barH);
                    }
                }
            }

            picHistogram.Image?.Dispose();
            picHistogram.Image = bmp;
        }

        private void btnAciklama_Click(object sender, EventArgs e)
        {
            if (txtAciklama != null)
            {
                txtAciklama.Text = "B ŞIKKI ANALİTİK GEOMETRİ ARAŞTIRMASI (RAY-CASTING):\r\n\r\n" +
                    "Kapalı bir poligonun içinde bir noktanın yer alıp almadığını tespit etmek için Jordan Eğri Teoremi'ne dayanan 'Işın Gönderme' (Ray-Casting) algoritması kullanılır.\r\n\r\n" +
                    "Çalışma Mantığı:\r\n" +
                    "1. İncelenen (x, y) pikselinden yatay doğrultuda (+X ekseninde) sonsuza bir ışın fırlatılır.\r\n" +
                    "2. Bu ışının poligonu oluşturan kenar doğrularını kestiği noktaların sayısı hesaplanır.\r\n" +
                    "3. Kesişim sayısı TEK ise nokta poligonun İÇİNDEDİR; ÇİFT veya SIFIR ise DIŞINDADIR.\r\n\r\n" +
                    "Kod içerisinde bu mantık kullanılarak sadece kapalı bölgenin pikselleri filtrelenip renklendirilmektedir.";
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            orijinalMat?.Dispose();
            base.OnFormClosed(e);
        }
    }
}