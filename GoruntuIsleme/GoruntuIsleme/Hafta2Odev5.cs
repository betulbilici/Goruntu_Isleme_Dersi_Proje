using System;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace GoruntuIsleme
{
    public partial class Hafta2Odev5 : Form
    {
        private Mat orijinalMat = null;
        private int[] histR = new int[256];
        private int[] histG = new int[256];
        private int[] histB = new int[256];

        private ModernHapRozet rozetOrijinal;
        private ModernHapRozet rozetSonuc1;
        private ModernHapRozet rozetSonuc2;
        private ModernHapRozet rozetHistR;
        private ModernHapRozet rozetHistG;
        private ModernHapRozet rozetHistB;
        private Label lblAnaliz;

        public Hafta2Odev5()
        {
            InitializeComponent();
            ModernArayuzuUygula();
        }

        private void ModernArayuzuUygula()
        {
            // 1. Form Temel Ayarları (1250x750, Standart Kenarlık ve X ile Ana Menüye Dönüş)
            FormGezintiYardimcisi.FormuHazirla(this);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Text = "Hafta 2 - Ödev 5: RGB Kanal Bazlı Kontrast Germe vs. Ders Notu Formülü";

            // Designer'daki tüm eski kontrolleri gizle
            foreach (Control ctrl in this.Controls)
            {
                ctrl.Visible = false;
            }

            // 2. Üst Gezinti Barı
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                form: this,
                baslik: "Hafta 2 - Ödev 5 | RGB Kanal Bazlı Kontrast Germe ve C Katsayısı Formülü",
                oncekiYazi: "< Ödev 4",
                oncekiTikla: () =>
                {
                    Form odev4 = Application.OpenForms["Hafta2Odev4"];
                    if (odev4 != null)
                        FormGezintiYardimcisi.FormGecisYap(this, odev4);
                    else
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta2Odev4());
                },
                sonrakiYazi: "Ödev 6 >",
                sonrakiTikla: () => FormGezintiYardimcisi.FormGecisYap(this, new Hafta2Odev6())
            );

            // 3. Sağ Kontrol Paneli
            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 290);

            Label lblKontrolBaslik = new Label
            {
                Text = "Kontrast Parametreleri",
                Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                Location = new Point(18, 16),
                AutoSize = true
            };
            pnlSag.Controls.Add(lblKontrolBaslik);

            // Resim Seç Butonu
            ModernHapButon btnModernResimSec = FormGezintiYardimcisi.ModernButonOlustur("📁 Resim Seç", true, 252, 38);
            btnModernResimSec.Location = new Point(18, 46);
            btnModernResimSec.Click += btnResimSec_Click;
            pnlSag.Controls.Add(btnModernResimSec);

            // C Katsayısı (-255 .. +255)
            Label lblCBaslik = new Label
            {
                Text = "C Katsayısı (-255 .. +255):",
                Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                Location = new Point(18, 96),
                AutoSize = true
            };
            pnlSag.Controls.Add(lblCBaslik);

            txtC.Parent?.Controls.Remove(txtC);
            txtC.Visible = true;
            txtC.Location = new Point(18, 118);
            txtC.Size = new Size(252, 28);
            txtC.Font = new Font("Segoe UI Semibold", 9.8f, FontStyle.Bold);
            txtC.TextAlign = HorizontalAlignment.Center;
            if (string.IsNullOrWhiteSpace(txtC.Text)) txtC.Text = "50";
            pnlSag.Controls.Add(txtC);

            // R, G, B Kanal Sınırları (Min - Max)
            KanalSinirSatiriEkle(pnlSag, "R Sınırları:", Color.FromArgb(220, 38, 38), 156, txtRMin, txtRMax, 0);
            KanalSinirSatiriEkle(pnlSag, "G Sınırları:", Color.FromArgb(22, 163, 74), 194, txtGMin, txtGMax, 1);
            KanalSinirSatiriEkle(pnlSag, "B Sınırları:", Color.FromArgb(37, 99, 235), 232, txtBMin, txtBMax, 2);

            // Kontrast Uygula Butonu
            ModernHapButon btnModernUygula = FormGezintiYardimcisi.ModernButonOlustur("⚡ Kontrast Uygula", false, 252, 38);
            btnModernUygula.Location = new Point(18, 274);
            btnModernUygula.Click += btnKontrastUygula_Click;
            pnlSag.Controls.Add(btnModernUygula);

            // Kullanım İpucu Kartı
            ModernKartPanel bilgiKarti = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                konum: new Point(18, 324),
                boyut: new Size(252, 240),
                icerikEtiketi: out Label lblBilgi
            );
            lblBilgi.Text =
                "Kullanım & Formüller:\n\n" +
                "• Histogram üzerinde Sol Tık: Min sınırını, Sağ Tık: Max sınırını belirler.\n" +
                "• Yöntem (a): R, G, B kanalları ayrı gerilir.\n" +
                "• Yöntem (b): F = 259(C+255) / 255(259-C) katsayısı ile 128 merkezli kontrast uygulanır.";
            pnlSag.Controls.Add(bilgiKarti);

            // 4. Orta Çalışma Alanı (Üst: 3 Görüntü Kartı | Orta: 3 RGB Histogramı | Alt: Akademik Yorum & Analiz)
            Panel pnlOrta = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                BackColor = FormGezintiYardimcisi.TemaZemin
            };

            TableLayoutPanel anaYerlesim = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4
            };
            anaYerlesim.RowStyles.Add(new RowStyle(SizeType.Percent, 46f));
            anaYerlesim.RowStyles.Add(new RowStyle(SizeType.Percent, 28f));
            anaYerlesim.RowStyles.Add(new RowStyle(SizeType.Percent, 26f));
            anaYerlesim.RowStyles.Add(new RowStyle(SizeType.Absolute, 48f));

            // Satır 0: 3 Görüntü Kartı
            TableLayoutPanel goruntuIzgara = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1
            };
            goruntuIzgara.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            goruntuIzgara.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            goruntuIzgara.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34f));
            goruntuIzgara.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            rozetOrijinal = new ModernHapRozet(); rozetOrijinal.MetinGuncelle("RGB");
            rozetSonuc1 = new ModernHapRozet(); rozetSonuc1.MetinGuncelle("Yöntem A");
            rozetSonuc2 = new ModernHapRozet(); rozetSonuc2.MetinGuncelle("Yöntem B");

            MevcutPictureBoxKartaTasi(goruntuIzgara, 0, 0, "Orijinal Renkli Resim", picOrijinal, FormGezintiYardimcisi.TemaZemin, rozetOrijinal);
            MevcutPictureBoxKartaTasi(goruntuIzgara, 1, 0, "Ayrı Kanal Germe (a)", picSonuc1, FormGezintiYardimcisi.TemaZemin, rozetSonuc1);
            MevcutPictureBoxKartaTasi(goruntuIzgara, 2, 0, "Ders Notu Formülü (b)", picSonuc2, FormGezintiYardimcisi.TemaZemin, rozetSonuc2);

            // Satır 1: 3 RGB Histogram Kartı
            TableLayoutPanel histIzgara = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1
            };
            histIzgara.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            histIzgara.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            histIzgara.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34f));
            histIzgara.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            rozetHistR = new ModernHapRozet(); rozetHistR.MetinGuncelle("0 - 255");
            rozetHistG = new ModernHapRozet(); rozetHistG.MetinGuncelle("0 - 255");
            rozetHistB = new ModernHapRozet(); rozetHistB.MetinGuncelle("0 - 255");

            MevcutPictureBoxKartaTasi(histIzgara, 0, 0, "Kırmızı (R) Histogram", picHistR, Color.FromArgb(15, 23, 42), rozetHistR);
            MevcutPictureBoxKartaTasi(histIzgara, 1, 0, "Yeşil (G) Histogram", picHistG, Color.FromArgb(15, 23, 42), rozetHistG);
            MevcutPictureBoxKartaTasi(histIzgara, 2, 0, "Mavi (B) Histogram", picHistB, Color.FromArgb(15, 23, 42), rozetHistB);

            PictureBox[] histKutulari = { picHistR, picHistG, picHistB };
            for (int i = 0; i < 3; i++)
            {
                int kanalIdx = i;
                histKutulari[i].Cursor = Cursors.Hand;
                histKutulari[i].SizeMode = PictureBoxSizeMode.Normal;
                histKutulari[i].MouseDown += (s, e) =>
                {
                    TextBox tMin = kanalIdx == 0 ? txtRMin : (kanalIdx == 1 ? txtGMin : txtBMin);
                    TextBox tMax = kanalIdx == 0 ? txtRMax : (kanalIdx == 1 ? txtGMax : txtBMax);
                    HistMouseDown(e, tMin, tMax, kanalIdx);
                };
                histKutulari[i].Resize += (s, e) => KanalHistograminiYenile(kanalIdx);
            }

            // Satır 2: Akademik Yorum Kartı
            ModernKartPanel yorumKart = new ModernKartPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(5),
                Padding = new Padding(10)
            };

            Label lblYorumBaslik = new Label
            {
                Text = "Yöntem Karşılaştırması ve Akademik Yorum",
                Dock = DockStyle.Top,
                Height = 22,
                Font = new Font("Segoe UI Semibold", 9.3f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                BackColor = Color.White
            };

            txtYorum.Parent?.Controls.Remove(txtYorum);
            txtYorum.Visible = true;
            txtYorum.Dock = DockStyle.Fill;
            txtYorum.BorderStyle = BorderStyle.None;
            txtYorum.BackColor = Color.White;
            txtYorum.Font = new Font("Segoe UI", 9.1f);
            txtYorum.ReadOnly = true;
            txtYorum.Multiline = true;
            txtYorum.ScrollBars = ScrollBars.Vertical;

            yorumKart.Controls.Add(txtYorum);
            yorumKart.Controls.Add(lblYorumBaslik);

            // Satır 3: Alt Analiz Şeridi
            ModernKartPanel altSerit = FormGezintiYardimcisi.AltAnalizSeridiOlustur(
                "RGB kanal sınırlarını histogramlara tıklayarak seçin ve 'Kontrast Uygula' butonuna basınız.",
                out lblAnaliz,
                "Hazır"
            );

            anaYerlesim.Controls.Add(goruntuIzgara, 0, 0);
            anaYerlesim.Controls.Add(histIzgara, 0, 1);
            anaYerlesim.Controls.Add(yorumKart, 0, 2);
            anaYerlesim.Controls.Add(altSerit, 0, 3);
            pnlOrta.Controls.Add(anaYerlesim);

            this.Controls.Add(pnlOrta);
            FormGezintiYardimcisi.YerlesimiDuzenle(this, pnlUst, pnlSag, pnlOrta);
        }

        private void KanalSinirSatiriEkle(Panel pnl, string etiket, Color renk, int y, TextBox tMin, TextBox tMax, int kanalIdx)
        {
            Label lbl = new Label
            {
                Text = etiket,
                Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                ForeColor = renk,
                Location = new Point(18, y + 4),
                AutoSize = true
            };
            pnl.Controls.Add(lbl);

            tMin.Parent?.Controls.Remove(tMin);
            tMin.Visible = true;
            tMin.Location = new Point(108, y);
            tMin.Size = new Size(76, 28);
            tMin.Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold);
            tMin.TextAlign = HorizontalAlignment.Center;
            tMin.Text = "0";
            tMin.TextChanged += (s, e) => KanalHistograminiYenile(kanalIdx);
            pnl.Controls.Add(tMin);

            tMax.Parent?.Controls.Remove(tMax);
            tMax.Visible = true;
            tMax.Location = new Point(194, y);
            tMax.Size = new Size(76, 28);
            tMax.Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold);
            tMax.TextAlign = HorizontalAlignment.Center;
            tMax.Text = "255";
            tMax.TextChanged += (s, e) => KanalHistograminiYenile(kanalIdx);
            pnl.Controls.Add(tMax);
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
                Margin = new Padding(5),
                Padding = new Padding(10)
            };

            Panel pnlKartUst = new Panel
            {
                Dock = DockStyle.Top,
                Height = 26,
                BackColor = Color.White
            };

            Label lbl = new Label
            {
                Text = baslik,
                UseMnemonic = false,
                AutoSize = true,
                Location = new Point(0, 3),
                Font = new Font("Segoe UI Semibold", 9.3f, FontStyle.Bold),
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
            string dosyaYolu = FormGezintiYardimcisi.ResimDosyasiSec("RGB Kontrast Karşılaştırması İçin Resim Seçiniz");
            if (string.IsNullOrEmpty(dosyaYolu)) return;

            orijinalMat?.Dispose();
            orijinalMat = Cv2.ImRead(dosyaYolu, ImreadModes.Color);

            if (orijinalMat.Empty())
            {
                MessageBox.Show("Resim yüklenemedi!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            picOrijinal.Image?.Dispose();
            picOrijinal.Image = BitmapConverter.ToBitmap(orijinalMat);
            rozetOrijinal.MetinGuncelle($"{orijinalMat.Width}×{orijinalMat.Height}");

            HistogramlariHesaplaVeCiz();
            lblAnaliz.Text = $"Resim yüklendi ({orijinalMat.Width}×{orijinalMat.Height} px) • Histogramlar üzerinden Sol/Sağ tıkla sınırları belirleyin.";
        }

        private void HistMouseDown(MouseEventArgs e, TextBox txtMin, TextBox txtMax, int kanalIdx)
        {
            PictureBox pBox = (kanalIdx == 0) ? picHistR : (kanalIdx == 1 ? picHistG : picHistB);
            if (orijinalMat == null || pBox.ClientSize.Width <= 1) return;

            int val = Math.Clamp((int)((float)e.X / (pBox.ClientSize.Width - 1) * 255f), 0, 255);

            if (e.Button == MouseButtons.Left)
                txtMin.Text = val.ToString();
            else if (e.Button == MouseButtons.Right)
                txtMax.Text = val.ToString();

            KanalHistograminiYenile(kanalIdx);
        }

        private void KanalHistograminiYenile(int kanalIdx)
        {
            if (kanalIdx == 0)
            {
                TekHistogramCiz(picHistR, histR, Color.FromArgb(248, 113, 113), txtRMin, txtRMax);
                rozetHistR?.MetinGuncelle($"{txtRMin.Text} - {txtRMax.Text}");
            }
            else if (kanalIdx == 1)
            {
                TekHistogramCiz(picHistG, histG, Color.FromArgb(74, 222, 128), txtGMin, txtGMax);
                rozetHistG?.MetinGuncelle($"{txtGMin.Text} - {txtGMax.Text}");
            }
            else
            {
                TekHistogramCiz(picHistB, histB, Color.FromArgb(56, 189, 248), txtBMin, txtBMax);
                rozetHistB?.MetinGuncelle($"{txtBMin.Text} - {txtBMax.Text}");
            }
        }

        private void HistogramlariHesaplaVeCiz()
        {
            if (orijinalMat == null) return;
            Array.Clear(histR, 0, 256);
            Array.Clear(histG, 0, 256);
            Array.Clear(histB, 0, 256);

            for (int y = 0; y < orijinalMat.Height; y++)
            {
                for (int x = 0; x < orijinalMat.Width; x++)
                {
                    Vec3b c = orijinalMat.At<Vec3b>(y, x);
                    histB[c.Item0]++;
                    histG[c.Item1]++;
                    histR[c.Item2]++;
                }
            }

            KanalHistograminiYenile(0);
            KanalHistograminiYenile(1);
            KanalHistograminiYenile(2);
        }

        private void TekHistogramCiz(PictureBox pb, int[] hist, Color renk, TextBox tMin, TextBox tMax)
        {
            if (pb == null || pb.ClientSize.Width <= 5 || pb.ClientSize.Height <= 5) return;

            int w = pb.ClientSize.Width;
            int h = pb.ClientSize.Height;
            Bitmap bmp = new Bitmap(w, h);

            int max = 1;
            for (int i = 0; i < 256; i++)
            {
                if (hist[i] > max) max = hist[i];
            }

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.FromArgb(15, 23, 42));

                using (Pen pRef = new Pen(Color.FromArgb(51, 65, 85), 1))
                {
                    for (int i = 50; i <= 250; i += 50)
                    {
                        float xRef = (float)i / 255f * (w - 1);
                        g.DrawLine(pRef, xRef, 0, xRef, h);
                    }
                }

                if (orijinalMat != null)
                {
                    using (Pen p = new Pen(renk, 1.3f))
                    {
                        for (int i = 0; i < 256; i++)
                        {
                            float x = (float)i / 255f * (w - 1);
                            float barH = ((float)hist[i] / max) * (h - 10);
                            g.DrawLine(p, x, h - 1, x, h - 1 - barH);
                        }
                    }
                }

                int.TryParse(tMin?.Text, out int minVal);
                int.TryParse(tMax?.Text, out int maxVal);
                using (Pen pLimit = new Pen(Color.White, 1.5f))
                {
                    float xMin = (float)Math.Clamp(minVal, 0, 255) / 255f * (w - 1);
                    float xMax = (float)Math.Clamp(maxVal, 0, 255) / 255f * (w - 1);
                    g.DrawLine(pLimit, xMin, 0, xMin, h);
                    g.DrawLine(pLimit, xMax, 0, xMax, h);
                }
            }

            pb.Image?.Dispose();
            pb.Image = bmp;
        }

        private void btnKontrastUygula_Click(object sender, EventArgs e)
        {
            if (orijinalMat == null)
            {
                MessageBox.Show("Lütfen önce bir resim seçiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int.TryParse(txtRMin?.Text, out int rMin); int.TryParse(txtRMax?.Text, out int rMax);
            int.TryParse(txtGMin?.Text, out int gMin); int.TryParse(txtGMax?.Text, out int gMax);
            int.TryParse(txtBMin?.Text, out int bMin); int.TryParse(txtBMax?.Text, out int bMax);

            if (rMin >= rMax || gMin >= gMax || bMin >= bMax)
            {
                MessageBox.Show("Seçilen minimum değerler maksimum değerlerden küçük olmalıdır!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // YÖNTEM A: 6 Sınır Değeriyle Bağımsız Kanal Germe
            using (Mat sonuc1 = new Mat(orijinalMat.Size(), MatType.CV_8UC3))
            {
                for (int y = 0; y < orijinalMat.Height; y++)
                {
                    for (int x = 0; x < orijinalMat.Width; x++)
                    {
                        Vec3b bgr = orijinalMat.At<Vec3b>(y, x);

                        byte b = GermeUygula(bgr.Item0, bMin, bMax);
                        byte g = GermeUygula(bgr.Item1, gMin, gMax);
                        byte r = GermeUygula(bgr.Item2, rMin, rMax);

                        sonuc1.Set(y, x, new Vec3b(b, g, r));
                    }
                }
                picSonuc1.Image?.Dispose();
                picSonuc1.Image = BitmapConverter.ToBitmap(sonuc1);
            }

            // YÖNTEM B: Ders Notu Formülü F = (259 * (C + 255)) / (255 * (259 - C))
            double C_KontrastSeviyesi = 50;
            if (txtC != null) double.TryParse(txtC.Text, out C_KontrastSeviyesi);
            C_KontrastSeviyesi = Math.Clamp(C_KontrastSeviyesi, -255, 255);

            double F_KontrastFaktoru = (259.0 * (C_KontrastSeviyesi + 255.0)) / (255.0 * (259.0 - C_KontrastSeviyesi));

            using (Mat sonuc2 = new Mat(orijinalMat.Size(), MatType.CV_8UC3))
            {
                for (int y = 0; y < orijinalMat.Height; y++)
                {
                    for (int x = 0; x < orijinalMat.Width; x++)
                    {
                        Vec3b okunanRenk = orijinalMat.At<Vec3b>(y, x);

                        int B = Math.Clamp((int)((F_KontrastFaktoru * (okunanRenk.Item0 - 128)) + 128), 0, 255);
                        int G = Math.Clamp((int)((F_KontrastFaktoru * (okunanRenk.Item1 - 128)) + 128), 0, 255);
                        int R = Math.Clamp((int)((F_KontrastFaktoru * (okunanRenk.Item2 - 128)) + 128), 0, 255);

                        sonuc2.Set(y, x, new Vec3b((byte)B, (byte)G, (byte)R));
                    }
                }

                picSonuc2.Image?.Dispose();
                picSonuc2.Image = BitmapConverter.ToBitmap(sonuc2);
            }

            rozetSonuc1.MetinGuncelle("R/G/B Gerildi");
            rozetSonuc2.MetinGuncelle($"C={C_KontrastSeviyesi:0}, F={F_KontrastFaktoru:0.00}");
            lblAnaliz.Text = $"Uygulandı • R:[{rMin}-{rMax}], G:[{gMin}-{gMax}], B:[{bMin}-{bMax}] • C = {C_KontrastSeviyesi:0} (F = {F_KontrastFaktoru:0.00})";

            if (txtYorum != null)
            {
                txtYorum.Text = "HOCANIN DERS NOTLARINA GÖRE KARŞILAŞTIRMA VE YORUM:\r\n\r\n" +
                    "1. Histogram Sınırları ile Ayrı Kanal Germe (a Şıkkı):\r\n" +
                    "Histogram üzerinden R, G ve B renklerinin yığıldığı gerçek sınırlar seçilerek uygulandığında resim detayları daha iyi korunur. Fakat üç kanal birbirinden bağımsız oranlarda gerdirildiğinde pikseller arasındaki orijinal renk dengesi bozulabilir ve hafif renk sapmaları (renk tonu kayması) gözlemlenebilir.\r\n\r\n" +
                    "2. Hazır Formül Kullanımı (b Şıkkı - Slayt Formülü):\r\n" +
                    "Bu yöntemde temel mantık 128 değerinin üzerindeki renkleri yukarı 255'e, altındaki renkleri 0'a doğru ötelemektir. F katsayısı öteleme miktarını belirler. Pratik bir yöntemdir fakat renk değerleri 0'ın altına veya 255'in üzerine taştığında sınırlarda kırpma (clipping) yapıldığı için aşırı kontrast durumunda siyah ve beyaz alanlar belirgin şekilde artar ve resim doğallığını kaybedebilir.\r\n\r\n" +
                    "Sonuç olarak: Histogram yöntemi hangi sınırların nereye çekileceğini görerek karar vermeyi sağladığı için kontrollü kontrastta daha başarılı sonuç verir; formül yöntemi ise hızlı ve pratik uygulamalar için uygundur.";
            }
        }

        private byte GermeUygula(byte val, int min, int max)
        {
            if (val <= min) return 0;
            if (val >= max) return 255;
            return (byte)(((float)(val - min) / (max - min)) * 255f);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            orijinalMat?.Dispose();
            base.OnFormClosed(e);
        }
    }
}