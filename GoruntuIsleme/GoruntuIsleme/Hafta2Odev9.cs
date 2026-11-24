using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace GoruntuIsleme
{
    public partial class Hafta2Odev9 : Form
    {
        private Mat orijinalMat = null;
        private int[] griHistogram = new int[256];
        private Point? bolgeselIsikMerkezi = null;

        private ModernHapRozet rozetGiris;
        private ModernHapRozet rozetCikis;
        private ModernHapRozet rozetHist;
        private Label lblAnaliz;

        private ModernSurgu surguEsik;
        private ModernSurgu surguMin;
        private ModernSurgu surguYaricap;

        private Label lblValEsik;
        private Label lblValMin;
        private Label lblValYaricap;

        public Hafta2Odev9()
        {
            InitializeComponent();
            ModernArayuzuUygula();
        }

        private void ModernArayuzuUygula()
        {
            // 1. Form Temel Ayarları (1250x750, Standart Kenarlık ve X ile Ana Menüye Dönüş)
            FormGezintiYardimcisi.FormuHazirla(this);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Text = "Hafta 2 - Ödev 9: Dinamik Kontrast Germe, Gece Görüşü ve Bölgesel Spot Işık";

            // Designer'daki tüm eski kontrolleri gizle
            foreach (Control ctrl in this.Controls)
            {
                ctrl.Visible = false;
            }

            // 2. Üst Gezinti Barı
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                form: this,
                baslik: "Hafta 2 - Ödev 9 | Parçalı Kontrast Germe, Gece Görüş Modu ve Bölgesel Işık",
                oncekiYazi: "< Ödev 8",
                oncekiTikla: () =>
                {
                    Form odev8 = Application.OpenForms["Hafta2Odev8"];
                    if (odev8 != null)
                        FormGezintiYardimcisi.FormGecisYap(this, odev8);
                    else
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta2Odev8());
                },
                sonrakiYazi: "Ödev 10 >",
                sonrakiTikla: () =>
                {
                    Form odev10 = Application.OpenForms["Hafta2Odev10"];
                    if (odev10 != null)
                        FormGezintiYardimcisi.FormGecisYap(this, odev10);
                    else
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta2Odev10());
                }
            );

            // 3. Sağ Kontrol Paneli
            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 290);

            Label lblKontrolBaslik = new Label
            {
                Text = "Gece Görüş & Kontrast",
                Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                Location = new Point(18, 16),
                AutoSize = true
            };
            pnlSag.Controls.Add(lblKontrolBaslik);

            // Gece Resmi Seç Butonu
            ModernHapButon btnModernResimSec = FormGezintiYardimcisi.ModernButonOlustur("📁 Gece Resmi Seç", true, 252, 38);
            btnModernResimSec.Location = new Point(18, 46);
            btnModernResimSec.Click += btnResimSec_Click;
            pnlSag.Controls.Add(btnModernResimSec);

            // Bölgesel Işığı Sıfırla Butonu
            ModernHapButon btnModernSifirla = FormGezintiYardimcisi.ModernButonOlustur("🔄 Bölgesel Işığı Sıfırla", false, 252, 36);
            btnModernSifirla.Location = new Point(18, 92);
            btnModernSifirla.Click += btnSifirlaBolge_Click;
            pnlSag.Controls.Add(btnModernSifirla);

            // 3 Adet Modern Sürgü
            surguEsik = SurguEkle(pnlSag, "Kırılma Noktası (Mid)", 10, 240, 128, "", 142, out lblValEsik);
            surguMin = SurguEkle(pnlSag, "Alt Sınır (Min)", 0, 100, 15, "", 198, out lblValMin);
            surguYaricap = SurguEkle(pnlSag, "Spot Işık Yarıçapı", 20, 250, 80, " px", 254, out lblValYaricap);

            // Yeşil Fosfor Gece Görüş Efekti CheckBox
            chkYesilMod.Parent?.Controls.Remove(chkYesilMod);
            chkYesilMod.Visible = true;
            chkYesilMod.Location = new Point(18, 314);
            chkYesilMod.Font = new Font("Segoe UI Semibold", 9.4f, FontStyle.Bold);
            chkYesilMod.ForeColor = Color.FromArgb(22, 163, 74);
            chkYesilMod.Text = "Yeşil Fosfor Gece Görüş Efekti";
            chkYesilMod.CheckedChanged -= ChkYesilMod_CheckedChanged;
            chkYesilMod.CheckedChanged += ChkYesilMod_CheckedChanged;
            pnlSag.Controls.Add(chkYesilMod);

            // Açık Mavi Bilgi Kartı
            ModernKartPanel bilgiKarti = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                konum: new Point(18, 352),
                boyut: new Size(252, 220),
                icerikEtiketi: out Label lblBilgi
            );
            lblBilgi.Text =
                "Çalışma Mantığı:\n\n" +
                "• [Min – Mid] aralığındaki karanlık pikseller gamma eğrisi ile [0 – 180] aralığına açılarak detaylar görünür kılınır.\n" +
                "• [Mid – 255] aralığı sıkıştırılarak parlak patlamalar önlenir.\n" +
                "• Sol görüntüye tıklayarak o bölgeye radyal spot ışık uygulayabilirsiniz.";
            pnlSag.Controls.Add(bilgiKarti);

            // 4. Orta Çalışma Alanı (Üst: Giriş & Çıkış Kartları | Alt: Histogram Kartı | En Alt: Analiz Şeridi)
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
            anaYerlesim.RowStyles.Add(new RowStyle(SizeType.Percent, 66f));
            anaYerlesim.RowStyles.Add(new RowStyle(SizeType.Percent, 34f));
            anaYerlesim.RowStyles.Add(new RowStyle(SizeType.Absolute, 50f));

            // Üst Satır: Giriş ve Çıkış Görüntü Kartları
            TableLayoutPanel goruntuIzgara = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1
            };
            goruntuIzgara.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            goruntuIzgara.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            goruntuIzgara.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            rozetGiris = new ModernHapRozet(); rozetGiris.MetinGuncelle("Orijinal");
            rozetCikis = new ModernHapRozet(); rozetCikis.MetinGuncelle("Bekleniyor");

            MevcutPictureBoxKartaTasi(goruntuIzgara, 0, 0, "Orijinal Görüntü (Spot Işık İçin Tıklayın)", picGiris, FormGezintiYardimcisi.TemaZemin, rozetGiris);
            MevcutPictureBoxKartaTasi(goruntuIzgara, 1, 0, "İşlenmiş Gece Görüş / Kontrast Çıktısı", picCikis, FormGezintiYardimcisi.TemaZemin, rozetCikis);

            picGiris.Cursor = Cursors.Cross;
            picGiris.MouseDown -= PicGiris_MouseDown;
            picGiris.MouseDown += PicGiris_MouseDown;

            // Alt Satır: Histogram Kartı
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
                Text = "Gri-Ton Histogramı ve Kırılma Eşiği (Mid Sınırı)",
                AutoSize = true,
                Location = new Point(0, 3),
                Font = new Font("Segoe UI Semibold", 9.6f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi
            };
            pnlHistUst.Controls.Add(lblHistBaslik);

            rozetHist = new ModernHapRozet();
            rozetHist.MetinGuncelle("Mid: 128");
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
                if (orijinalMat != null) HistogramCiz(surguEsik.Value);
            };

            histKart.Controls.Add(picHistogram);
            histKart.Controls.Add(pnlHistUst);

            // En Alt Analiz Şeridi
            ModernKartPanel altSerit = FormGezintiYardimcisi.AltAnalizSeridiOlustur(
                "Düşük ışıklı bir görüntü seçin; sürgülerle kırılma eşiğini ayarlayın ve sol görüntüye tıklayarak bölgesel ışık ekleyin.",
                out lblAnaliz,
                "Parçalı LUT"
            );

            anaYerlesim.Controls.Add(goruntuIzgara, 0, 0);
            anaYerlesim.Controls.Add(histKart, 0, 1);
            anaYerlesim.Controls.Add(altSerit, 0, 2);
            pnlOrta.Controls.Add(anaYerlesim);

            this.Controls.Add(pnlOrta);
            FormGezintiYardimcisi.YerlesimiDuzenle(this, pnlUst, pnlSag, pnlOrta);
        }

        private ModernSurgu SurguEkle(Panel pnl, string baslik, int min, int max, int deger, string birim, int y, out Label lblDeger)
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
                Text = $"{deger}{birim}",
                Location = new Point(200, y),
                Size = new Size(70, 18),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI Semibold", 8.8f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi
            };
            pnl.Controls.Add(lblDeger);

            Label degerRef = lblDeger;
            ModernSurgu surgu = new ModernSurgu
            {
                Minimum = min,
                Maximum = max,
                Value = deger,
                Location = new Point(18, y + 20),
                Size = new Size(252, 24)
            };
            surgu.Scroll += (s, e) =>
            {
                degerRef.Text = $"{surgu.Value}{birim}";
                AlgoritmayiUygula();
            };
            pnl.Controls.Add(surgu);
            return surgu;
        }

        private void ChkYesilMod_CheckedChanged(object sender, EventArgs e)
        {
            AlgoritmayiUygula();
        }

        private void MevcutPictureBoxKartaTasi(
            TableLayoutPanel tablo,
            int sutun,
            int satir,
            string baslik,
            PictureBox mevcutPb,
            Color zeminRengi,
            ModernHapRozet sagUstRozet)
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
                Location = new Point(0, 4),
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
            string dosyaYolu = FormGezintiYardimcisi.ResimDosyasiSec("Gece Görüş / Dinamik Kontrast İçin Resim Seçiniz");
            if (string.IsNullOrEmpty(dosyaYolu)) return;

            orijinalMat?.Dispose();
            orijinalMat = Cv2.ImRead(dosyaYolu, ImreadModes.Color);

            if (orijinalMat.Empty())
            {
                MessageBox.Show("Resim yüklenemedi!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bolgeselIsikMerkezi = null;
            picGiris.Image?.Dispose();
            picGiris.Image = BitmapConverter.ToBitmap(orijinalMat);
            rozetGiris.MetinGuncelle($"{orijinalMat.Width}×{orijinalMat.Height}");

            HistogramHesapla();
            AlgoritmayiUygula();
        }

        private bool EkranKoordinatiniResmeCevir(int mouseX, int mouseY, out int gercekX, out int gercekY)
        {
            gercekX = 0;
            gercekY = 0;
            if (orijinalMat == null || picGiris.ClientSize.Width <= 0 || picGiris.ClientSize.Height <= 0)
                return false;

            float kutuW = picGiris.ClientSize.Width;
            float kutuH = picGiris.ClientSize.Height;
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

        private void PicGiris_MouseDown(object sender, MouseEventArgs e)
        {
            if (orijinalMat == null || e.Button != MouseButtons.Left) return;

            if (EkranKoordinatiniResmeCevir(e.X, e.Y, out int rx, out int ry))
            {
                bolgeselIsikMerkezi = new Point(rx, ry);
                AlgoritmayiUygula();
            }
        }

        private void btnSifirlaBolge_Click(object sender, EventArgs e)
        {
            bolgeselIsikMerkezi = null;
            AlgoritmayiUygula();
        }

        private void HistogramHesapla()
        {
            if (orijinalMat == null) return;
            Array.Clear(griHistogram, 0, 256);

            for (int y = 0; y < orijinalMat.Height; y++)
            {
                for (int x = 0; x < orijinalMat.Width; x++)
                {
                    Vec3b c = orijinalMat.At<Vec3b>(y, x);
                    int gri = (int)(c.Item2 * 0.299 + c.Item1 * 0.587 + c.Item0 * 0.114);
                    griHistogram[Math.Clamp(gri, 0, 255)]++;
                }
            }
        }

        private void HistogramCiz(int kirilmaNoktasi)
        {
            if (picHistogram == null || picHistogram.ClientSize.Width <= 5 || picHistogram.ClientSize.Height <= 5) return;

            int w = picHistogram.ClientSize.Width;
            int h = picHistogram.ClientSize.Height;
            Bitmap bmp = new Bitmap(w, h);

            int max = 1;
            for (int i = 0; i < 256; i++)
            {
                if (griHistogram[i] > max) max = griHistogram[i];
            }

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.FromArgb(15, 23, 42));

                using (Pen pBar = new Pen(Color.FromArgb(250, 204, 21), 1.3f))
                {
                    for (int i = 0; i < 256; i++)
                    {
                        float x = (float)i / 255f * (w - 1);
                        float barH = ((float)griHistogram[i] / max) * (h - 14);
                        g.DrawLine(pBar, x, h - 1, x, h - 1 - barH);
                    }
                }

                using (Pen pRef = new Pen(Color.FromArgb(71, 85, 105), 1))
                {
                    for (int i = 32; i < 256; i += 32)
                    {
                        float xRef = (float)i / 255f * (w - 1);
                        g.DrawLine(pRef, xRef, 0, xRef, h);
                    }
                }

                float xMavi = (float)kirilmaNoktasi / 255f * (w - 1);
                using (Pen pBlue = new Pen(Color.FromArgb(56, 189, 248), 2.5f))
                {
                    g.DrawLine(pBlue, xMavi, 4, xMavi, h - 4);
                }
            }

            picHistogram.Image?.Dispose();
            picHistogram.Image = bmp;
            rozetHist?.MetinGuncelle($"Mid Eşik: {kirilmaNoktasi}");
        }

        private void AlgoritmayiUygula()
        {
            if (orijinalMat == null) return;

            int mid = surguEsik.Value;
            int minVal = surguMin.Value;
            int yaricap = surguYaricap.Value;
            bool yesilMod = chkYesilMod != null && chkYesilMod.Checked;

            HistogramCiz(mid);

            using (Mat islenmis = new Mat(orijinalMat.Height, orijinalMat.Width, MatType.CV_8UC3))
            {
                byte[] lut = new byte[256];
                for (int i = 0; i < 256; i++)
                {
                    if (i <= minVal)
                    {
                        lut[i] = 0;
                    }
                    else if (i <= mid)
                    {
                        double oran = (double)(i - minVal) / Math.Max(1, mid - minVal);
                        lut[i] = (byte)Math.Min(255, Math.Pow(oran, 0.6) * 180.0);
                    }
                    else
                    {
                        double oran = (double)(i - mid) / Math.Max(1, 255 - mid);
                        lut[i] = (byte)Math.Min(255, 180.0 + (oran * 75.0));
                    }
                }

                double yaricapKare = yaricap * yaricap;

                for (int y = 0; y < orijinalMat.Height; y++)
                {
                    for (int x = 0; x < orijinalMat.Width; x++)
                    {
                        Vec3b bgr = orijinalMat.At<Vec3b>(y, x);

                        double b = lut[bgr.Item0];
                        double g = lut[bgr.Item1];
                        double r = lut[bgr.Item2];

                        if (bolgeselIsikMerkezi.HasValue)
                        {
                            double dx = x - bolgeselIsikMerkezi.Value.X;
                            double dy = y - bolgeselIsikMerkezi.Value.Y;
                            double dKare = (dx * dx) + (dy * dy);

                            if (dKare < yaricapKare)
                            {
                                double isikFaktoru = 1.0 + (1.2 * (1.0 - Math.Sqrt(dKare) / yaricap));
                                b *= isikFaktoru;
                                g *= isikFaktoru;
                                r *= isikFaktoru;
                            }
                        }

                        if (yesilMod)
                        {
                            double griDeger = r * 0.299 + g * 0.587 + b * 0.114;
                            b = griDeger * 0.15;
                            g = Math.Min(255, griDeger * 1.35);
                            r = griDeger * 0.20;
                        }

                        b = Math.Clamp(b, 0, 255);
                        g = Math.Clamp(g, 0, 255);
                        r = Math.Clamp(r, 0, 255);

                        islenmis.Set(y, x, new Vec3b((byte)b, (byte)g, (byte)r));
                    }
                }

                Bitmap bmpCikis = BitmapConverter.ToBitmap(islenmis);

                if (bolgeselIsikMerkezi.HasValue)
                {
                    using (Graphics gr = Graphics.FromImage(bmpCikis))
                    using (Pen cemberKalem = new Pen(Color.Cyan, 2))
                    {
                        cemberKalem.DashStyle = DashStyle.Dot;
                        gr.DrawEllipse(cemberKalem,
                            bolgeselIsikMerkezi.Value.X - yaricap,
                            bolgeselIsikMerkezi.Value.Y - yaricap,
                            yaricap * 2, yaricap * 2);
                    }
                }

                picCikis.Image?.Dispose();
                picCikis.Image = bmpCikis;
            }

            rozetCikis.MetinGuncelle(yesilMod ? "Gece Görüş" : "LUT Aktif");
            string spotBilgi = bolgeselIsikMerkezi.HasValue
                ? $"Spot Işık: ({bolgeselIsikMerkezi.Value.X}, {bolgeselIsikMerkezi.Value.Y}), r={yaricap}px"
                : "Spot Işık: Yok";
            lblAnaliz.Text = $"Min: {minVal} | Mid: {mid} • {spotBilgi} • Mod: {(yesilMod ? "Yeşil Fosfor" : "Doğal Renk")}";
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            orijinalMat?.Dispose();
            base.OnFormClosed(e);
        }
    }
}