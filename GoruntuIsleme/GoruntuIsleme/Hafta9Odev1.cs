using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GoruntuIsleme
{
    public class GeceGorusSonucu
    {
        public Bitmap SabitParlaklikResmi { get; set; }
        public Bitmap NormalizeResmi { get; set; }
        public int MinR { get; set; }
        public int MaxR { get; set; }
        public int MinG { get; set; }
        public int MaxG { get; set; }
        public int MinB { get; set; }
        public int MaxB { get; set; }
        public int NormMinR { get; set; }
        public int NormMaxR { get; set; }
        public int NormMinG { get; set; }
        public int NormMaxG { get; set; }
        public int NormMinB { get; set; }
        public int NormMaxB { get; set; }
    }

    public class Hafta9Odev1 : Form
    {
        private enum KaynakModu
        {
            StatikResim,
            CanliKamera,
            VideoDosyasi,
            OrnekVideoSimulasyonu
        }

        private PictureBox picGiris, picSabitParlaklik, picNormalize;
        private ModernSurgu trkSabit, trkAltSinir;
        private ModernHapRozet rozetGiris, rozetSabit, rozetNorm;
        private ModernHapButon btnKamera, btnSimulasyon, btnDuraklat;
        private Label lblGirisAralik, lblR, lblG, lblB, lblCozunurluk;

        private Bitmap orijinalResim;
        private Bitmap simTemelSahne;
        private OpenCvSharp.VideoCapture videoYakala;
        private System.Windows.Forms.Timer akisZamanlayici;
        private KaynakModu aktifMod = KaynakModu.StatikResim;
        private bool kareIsleniyor = false;
        private bool duraklatildi = false;
        private int simKareNo = 0;
        private readonly Stopwatch fpsOlcer = new Stopwatch();
        private double anlikFps = 0;

        public Hafta9Odev1()
        {
            Text = "Hafta 9 - Ödev 1 | Gece Görüş Kamerası İyileştirme";
            FormGezintiYardimcisi.FormuHazirla(this);
            ZamanlayiciHazirla();
            ArayuzuOlustur();
            OrnekKaranlikResimYukle();

            this.FormClosing += (s, e) =>
            {
                AkisiDurdur();
                orijinalResim?.Dispose();
                orijinalResim = null;
                simTemelSahne?.Dispose();
                simTemelSahne = null;
            };
        }

        private void ZamanlayiciHazirla()
        {
            akisZamanlayici = new System.Windows.Forms.Timer { Interval = 65 };
            akisZamanlayici.Tick += AkisZamanlayici_Tick;
        }

        private void ArayuzuOlustur()
        {
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                this,
                "Hafta 9 - Ödev 1 | Gece Görüş Kamerası İyileştirme (Video / Kamera / Fotoğraf)",
                "< Hafta 8", () => { AkisiDurdur(); FormGezintiYardimcisi.FormGecisYap(this, new Hafta8Odev4()); },
                "Ödev 2 >", () => { AkisiDurdur(); FormGezintiYardimcisi.FormGecisYap(this, new Hafta9Odev2()); },
                false,
                "HAFTA 9 • ÖDEV 1");

            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 295);
            pnlSag.AutoScroll = true;

            int x = 16;
            int w = 252;
            int y = 14;

            Label lblKaynakBaslik = new Label
            {
                Text = "VİDEO, KAMERA VE GÖRÜNTÜ",
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 8.8f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi
            };
            y += 24;

            btnKamera = FormGezintiYardimcisi.ModernButonOlustur("Canlı Kamerayı Başlat", true, w, 36);
            btnKamera.Location = new Point(x, y);
            btnKamera.Click += BtnKamera_Click;
            y += 42;

            ModernHapButon btnVideoSec = FormGezintiYardimcisi.ModernButonOlustur("Video Dosyası Seç", false, w, 34);
            btnVideoSec.Location = new Point(x, y);
            btnVideoSec.Click += BtnVideoSec_Click;
            y += 40;

            btnSimulasyon = FormGezintiYardimcisi.ModernButonOlustur("Örnek Gece Videosu Oynat", false, w, 34);
            btnSimulasyon.Location = new Point(x, y);
            btnSimulasyon.Click += BtnSimulasyon_Click;
            y += 40;

            btnDuraklat = FormGezintiYardimcisi.ModernButonOlustur("Duraklat / Devam", false, w, 32);
            btnDuraklat.Location = new Point(x, y);
            btnDuraklat.Enabled = false;
            btnDuraklat.Click += BtnDuraklat_Click;
            y += 38;

            ModernHapButon btnResimSec = FormGezintiYardimcisi.ModernButonOlustur("Fotoğraf Seç", false, w, 34);
            btnResimSec.Location = new Point(x, y);
            btnResimSec.Click += BtnResimSec_Click;
            y += 40;

            ModernHapButon btnOrnek = FormGezintiYardimcisi.ModernButonOlustur("Örnek Gece Fotoğrafı", false, w, 32);
            btnOrnek.Location = new Point(x, y);
            btnOrnek.Click += (s, e) => OrnekKaranlikResimYukle();
            y += 38;

            ModernHapButon btnTemizle = FormGezintiYardimcisi.ModernButonOlustur("Temizle", false, w, 32);
            btnTemizle.Location = new Point(x, y);
            btnTemizle.Click += (s, e) => Temizle();
            y += 40;

            ModernKartPanel kartIstatistik = new ModernKartPanel
            {
                Location = new Point(x, y),
                Size = new Size(w, 152),
                KartArkaPlan = FormGezintiYardimcisi.AcikMaviZemin,
                KenarlikRengi = FormGezintiYardimcisi.AcikMaviKenar,
                Padding = new Padding(12),
                Margin = new Padding(0, 0, 0, 14)
            };

            Label lblKatBaslik = new Label
            {
                Text = "Kanal Aralıkları (Ham Giriş)",
                Location = new Point(12, 12),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 9.2f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi,
                BackColor = FormGezintiYardimcisi.AcikMaviZemin
            };

            Label lRK = new Label { Text = "Kırmızı (R)", Location = new Point(12, 42), AutoSize = true, ForeColor = FormGezintiYardimcisi.AcikYazi, BackColor = FormGezintiYardimcisi.AcikMaviZemin, Font = new Font("Segoe UI", 8.8f) };
            lblR = new Label { Text = "0 – 84", Location = new Point(140, 42), Size = new Size(96, 20), TextAlign = ContentAlignment.MiddleRight, ForeColor = FormGezintiYardimcisi.KoyuYazi, BackColor = FormGezintiYardimcisi.AcikMaviZemin, Font = new Font("Consolas", 9.2f, FontStyle.Bold) };

            Label lGK = new Label { Text = "Yeşil (G)", Location = new Point(12, 68), AutoSize = true, ForeColor = FormGezintiYardimcisi.AcikYazi, BackColor = FormGezintiYardimcisi.AcikMaviZemin, Font = new Font("Segoe UI", 8.8f) };
            lblG = new Label { Text = "0 – 84", Location = new Point(140, 68), Size = new Size(96, 20), TextAlign = ContentAlignment.MiddleRight, ForeColor = FormGezintiYardimcisi.KoyuYazi, BackColor = FormGezintiYardimcisi.AcikMaviZemin, Font = new Font("Consolas", 9.2f, FontStyle.Bold) };

            Label lBK = new Label { Text = "Mavi (B)", Location = new Point(12, 94), AutoSize = true, ForeColor = FormGezintiYardimcisi.AcikYazi, BackColor = FormGezintiYardimcisi.AcikMaviZemin, Font = new Font("Segoe UI", 8.8f) };
            lblB = new Label { Text = "0 – 84", Location = new Point(140, 94), Size = new Size(96, 20), TextAlign = ContentAlignment.MiddleRight, ForeColor = FormGezintiYardimcisi.KoyuYazi, BackColor = FormGezintiYardimcisi.AcikMaviZemin, Font = new Font("Consolas", 9.2f, FontStyle.Bold) };

            lblCozunurluk = new Label
            {
                Text = "Boyut: 360 × 280 px",
                Location = new Point(12, 122),
                Size = new Size(224, 20),
                Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi,
                BackColor = FormGezintiYardimcisi.AcikMaviZemin
            };

            kartIstatistik.Controls.AddRange(new Control[] { lblKatBaslik, lRK, lblR, lGK, lblG, lBK, lblB, lblCozunurluk });

            pnlSag.Controls.AddRange(new Control[]
            {
                lblKaynakBaslik, btnKamera, btnVideoSec, btnSimulasyon, btnDuraklat,
                btnResimSec, btnOrnek, btnTemizle, kartIstatistik
            });

            Panel pnlGirisAlt = new Panel { Height = 52, Padding = new Padding(2, 14, 2, 0) };
            lblGirisAralik = new Label
            {
                Text = "Parlaklık aralığı: 0 – 84",
                Dock = DockStyle.Top,
                Height = 24,
                Font = new Font("Segoe UI", 9.0f),
                ForeColor = FormGezintiYardimcisi.AcikYazi
            };
            pnlGirisAlt.Controls.Add(lblGirisAralik);

            Panel pnlSabitKontrol = FormGezintiYardimcisi.KartAltSurguPaneliOlustur(
                "1a: Eklenecek Sabit Değer", "0 – 255", 0, 255, 90, out trkSabit, out _);
            trkSabit.Scroll += (s, e) =>
            {
                rozetSabit.MetinGuncelle($"+{trkSabit.Value}");
                Uygula();
            };

            Panel pnlNormKontrol = FormGezintiYardimcisi.KartAltSurguPaneliOlustur(
                "1b: Alt Sınır Hedefi [Alt..255]", "0 – 250", 0, 250, 100, out trkAltSinir, out _);
            trkAltSinir.Scroll += (s, e) =>
            {
                rozetNorm.MetinGuncelle($"Min: {trkAltSinir.Value}");
                Uygula();
            };

            rozetGiris = new ModernHapRozet();
            rozetGiris.MetinGuncelle("Fotoğraf");

            rozetSabit = new ModernHapRozet();
            rozetSabit.MetinGuncelle("+90");

            rozetNorm = new ModernHapRozet();
            rozetNorm.MetinGuncelle("Min: 100");

            TableLayoutPanel tbl = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(14),
                ColumnCount = 3,
                RowCount = 2,
                BackColor = FormGezintiYardimcisi.TemaZemin
            };
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 87f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 13f));

            Color gorselZemin = Color.FromArgb(11, 19, 30);
            picGiris = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 0, 0, "Orijinal Gece Görüntüsü", gorselZemin, pnlGirisAlt, rozetGiris);

            picSabitParlaklik = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 1, 0, "1a: Sabit Parlaklık Ekleme", Color.FromArgb(42, 52, 65), pnlSabitKontrol, rozetSabit);

            picNormalize = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 2, 0, "1b: Dinamik Normalize [Alt..255]", Color.FromArgb(86, 99, 112), pnlNormKontrol, rozetNorm);

            ModernKartPanel kartAciklama = FormGezintiYardimcisi.AltAnalizSeridiOlustur(
                "1a: Piksele sabit değer ekleyerek gündüz gibi aydınlatır. 1b: En küçük değeri [Alt Sınır]'a, en büyüğü 255'e normalize eder. (Not: Görünür ışık yükseltmedir, kızılötesi termal kamera değildir.)",
                out _,
                "Gece Görüş");

            tbl.Controls.Add(kartAciklama, 0, 1);
            tbl.SetColumnSpan(kartAciklama, 3);

            Controls.Add(tbl);
            FormGezintiYardimcisi.YerlesimiDuzenle(this, pnlUst, pnlSag, tbl);
        }

        private void BtnKamera_Click(object sender, EventArgs e)
        {
            if (aktifMod == KaynakModu.CanliKamera)
            {
                AkisiDurdur();
                return;
            }

            AkisiDurdur();
            OpenCvSharp.VideoCapture yeniKamera = new OpenCvSharp.VideoCapture(0);
            if (!yeniKamera.IsOpened())
            {
                yeniKamera.Dispose();
                MessageBox.Show(
                    "Sistemde kullanılabilir bir kamera bulunamadı veya kamera başka bir uygulama tarafından kullanılıyor.",
                    "Kamera Açılamadı",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            videoYakala = yeniKamera;
            aktifMod = KaynakModu.CanliKamera;
            duraklatildi = false;
            anlikFps = 0;
            btnKamera.Text = "Kamerayı Durdur";
            btnDuraklat.Enabled = true;
            btnDuraklat.Text = "Duraklat";
            fpsOlcer.Restart();
            akisZamanlayici.Start();
        }

        private void BtnVideoSec_Click(object sender, EventArgs e)
        {
            using OpenFileDialog dlg = new OpenFileDialog
            {
                Title = "Gece / Düşük Işık Video Dosyası Seç",
                Filter = "Video Dosyaları|*.mp4;*.avi;*.mov;*.mkv;*.wmv|Tüm Dosyalar|*.*"
            };
            if (dlg.ShowDialog() != DialogResult.OK) return;

            AkisiDurdur();
            OpenCvSharp.VideoCapture yeniVideo = new OpenCvSharp.VideoCapture(dlg.FileName);
            if (!yeniVideo.IsOpened())
            {
                yeniVideo.Dispose();
                MessageBox.Show("Seçilen video dosyası açılamadı.", "Video Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            videoYakala = yeniVideo;
            aktifMod = KaynakModu.VideoDosyasi;
            duraklatildi = false;
            anlikFps = 0;
            btnDuraklat.Enabled = true;
            btnDuraklat.Text = "Duraklat";
            fpsOlcer.Restart();
            akisZamanlayici.Start();
        }

        private void BtnSimulasyon_Click(object sender, EventArgs e)
        {
            if (aktifMod == KaynakModu.OrnekVideoSimulasyonu)
            {
                AkisiDurdur();
                return;
            }

            AkisiDurdur();
            if (simTemelSahne == null)
                simTemelSahne = BitmapYardimcisi.OrnekGorselYukle("gece_gorus.jpg");

            aktifMod = KaynakModu.OrnekVideoSimulasyonu;
            simKareNo = 0;
            duraklatildi = false;
            anlikFps = 0;
            btnSimulasyon.Text = "Örnek Videoyu Durdur";
            btnDuraklat.Enabled = true;
            btnDuraklat.Text = "Duraklat";
            fpsOlcer.Restart();
            akisZamanlayici.Start();
        }

        private void BtnDuraklat_Click(object sender, EventArgs e)
        {
            if (aktifMod == KaynakModu.StatikResim) return;
            duraklatildi = !duraklatildi;
            btnDuraklat.Text = duraklatildi ? "Devam Et" : "Duraklat";
            if (duraklatildi)
                rozetGiris.MetinGuncelle("Duraklatıldı");
            else
                fpsOlcer.Restart();
        }

        private void BtnResimSec_Click(object sender, EventArgs e)
        {
            string yol = FormGezintiYardimcisi.ResimDosyasiSec("Gece Görüş / Karanlık Fotoğraf Seç");
            if (yol == null) return;

            AkisiDurdur();
            using Bitmap ham = new Bitmap(yol);
            orijinalResim?.Dispose();
            orijinalResim = BitmapYardimcisi.BoyutSinirla(ham, 380);
            ResmiAta(picGiris, (Bitmap)orijinalResim.Clone());
            rozetGiris.MetinGuncelle("Fotoğraf");
            Uygula();
        }

        private void OrnekKaranlikResimYukle()
        {
            AkisiDurdur();
            Bitmap yuklenen = BitmapYardimcisi.OrnekGorselYukle("gece_gorus.jpg");
            if (yuklenen == null)
                yuklenen = OrnekGeceVideoKaresiOlustur(null, 0);

            orijinalResim?.Dispose();
            orijinalResim = yuklenen;
            ResmiAta(picGiris, (Bitmap)orijinalResim.Clone());
            rozetGiris.MetinGuncelle("Örnek Kare");
            Uygula();
        }

        private void Temizle()
        {
            AkisiDurdur();
            orijinalResim?.Dispose();
            orijinalResim = null;
            ResmiAta(picGiris, null);
            ResmiAta(picSabitParlaklik, null);
            ResmiAta(picNormalize, null);

            rozetGiris.MetinGuncelle("Boş");
            lblGirisAralik.Text = "Parlaklık aralığı: -";
            lblR.Text = "-";
            lblG.Text = "-";
            lblB.Text = "-";
            lblCozunurluk.Text = "Boyut: -";
        }

        private void AkisiDurdur()
        {
            akisZamanlayici?.Stop();
            if (videoYakala != null)
            {
                videoYakala.Release();
                videoYakala.Dispose();
                videoYakala = null;
            }
            aktifMod = KaynakModu.StatikResim;
            duraklatildi = false;
            if (btnKamera != null) btnKamera.Text = "Canlı Kamerayı Başlat";
            if (btnSimulasyon != null) btnSimulasyon.Text = "Örnek Gece Videosu Oynat";
            if (btnDuraklat != null)
            {
                btnDuraklat.Enabled = false;
                btnDuraklat.Text = "Duraklat / Devam";
            }
        }

        private void AkisZamanlayici_Tick(object sender, EventArgs e)
        {
            if (IsDisposed || kareIsleniyor || duraklatildi) return;

            kareIsleniyor = true;
            try
            {
                Bitmap yeniKare = null;

                if (aktifMod == KaynakModu.OrnekVideoSimulasyonu)
                {
                    yeniKare = OrnekGeceVideoKaresiOlustur(simTemelSahne, simKareNo++);
                }
                else if (videoYakala != null && videoYakala.IsOpened())
                {
                    using OpenCvSharp.Mat mat = new OpenCvSharp.Mat();
                    bool ok = videoYakala.Read(mat);

                    if ((!ok || mat.Empty()) && aktifMod == KaynakModu.VideoDosyasi)
                    {
                        videoYakala.Set(OpenCvSharp.VideoCaptureProperties.PosFrames, 0);
                        ok = videoYakala.Read(mat);
                    }

                    if (!ok || mat.Empty())
                    {
                        AkisiDurdur();
                        return;
                    }

                    using Bitmap ham = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(mat);
                    yeniKare = BitmapYardimcisi.BoyutSinirla(ham, 260);
                }

                if (yeniKare == null) return;

                double gecenSn = Math.Max(0.001, fpsOlcer.Elapsed.TotalSeconds);
                fpsOlcer.Restart();
                double olculenFps = 1.0 / gecenSn;
                anlikFps = anlikFps <= 0 ? olculenFps : (anlikFps * 0.7 + olculenFps * 0.3);

                orijinalResim?.Dispose();
                orijinalResim = yeniKare;
                ResmiAta(picGiris, (Bitmap)orijinalResim.Clone());

                string modEtiket = aktifMod switch
                {
                    KaynakModu.CanliKamera => "Kamera",
                    KaynakModu.VideoDosyasi => "Video",
                    _ => "Örnek Video"
                };
                rozetGiris.MetinGuncelle($"{modEtiket} ({anlikFps:F1} FPS)");

                Uygula();
            }
            catch
            {
                AkisiDurdur();
            }
            finally
            {
                kareIsleniyor = false;
            }
        }

        private void Uygula()
        {
            if (orijinalResim == null) return;

            GeceGorusSonucu sonuc = GeceGorusUygula(orijinalResim, trkSabit.Value, trkAltSinir.Value);
            ResmiAta(picSabitParlaklik, sonuc.SabitParlaklikResmi);
            ResmiAta(picNormalize, sonuc.NormalizeResmi);

            int genelMin = Math.Min(sonuc.MinR, Math.Min(sonuc.MinG, sonuc.MinB));
            int genelMax = Math.Max(sonuc.MaxR, Math.Max(sonuc.MaxG, sonuc.MaxB));
            lblGirisAralik.Text = $"Parlaklık aralığı: {genelMin} – {genelMax}";
            lblR.Text = $"{sonuc.MinR} – {sonuc.MaxR}";
            lblG.Text = $"{sonuc.MinG} – {sonuc.MaxG}";
            lblB.Text = $"{sonuc.MinB} – {sonuc.MaxB}";
            lblCozunurluk.Text = aktifMod == KaynakModu.StatikResim
                ? $"Boyut: {orijinalResim.Width} × {orijinalResim.Height} px"
                : $"Boyut: {orijinalResim.Width} × {orijinalResim.Height} • {anlikFps:F1} FPS";
        }

        private static void ResmiAta(PictureBox pic, Bitmap yeni)
        {
            Image onceki = pic.Image;
            pic.Image = yeni;
            onceki?.Dispose();
        }

        public static GeceGorusSonucu GeceGorusUygula(Bitmap giris, int sabitDeger, int altSinir)
        {
            GeceGorusSonucu sonuc = new GeceGorusSonucu();
            if (giris == null) return sonuc;

            int sabit = Math.Clamp(sabitDeger, 0, 255);
            int hedefAlt = Math.Clamp(altSinir, 0, 250);
            int w = giris.Width;
            int h = giris.Height;

            byte[,] rDizi = new byte[w, h];
            byte[,] gDizi = new byte[w, h];
            byte[,] bDizi = new byte[w, h];

            Bitmap sabitBmp = new Bitmap(w, h);
            Bitmap normBmp = new Bitmap(w, h);

            int minR = 255, maxR = 0;
            int minG = 255, maxG = 0;
            int minB = 255, maxB = 0;

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    Color c = giris.GetPixel(x, y);
                    byte r = c.R, g = c.G, b = c.B;
                    rDizi[x, y] = r;
                    gDizi[x, y] = g;
                    bDizi[x, y] = b;

                    if (r < minR) minR = r; if (r > maxR) maxR = r;
                    if (g < minG) minG = g; if (g > maxG) maxG = g;
                    if (b < minB) minB = b; if (b > maxB) maxB = b;

                    int sr = Math.Min(255, r + sabit);
                    int sg = Math.Min(255, g + sabit);
                    int sb = Math.Min(255, b + sabit);
                    sabitBmp.SetPixel(x, y, Color.FromArgb(sr, sg, sb));
                }
            }

            int farkR = maxR - minR;
            int farkG = maxG - minG;
            int farkB = maxB - minB;
            double aralikHedef = 255.0 - hedefAlt;

            int nMinR = 255, nMaxR = 0;
            int nMinG = 255, nMaxG = 0;
            int nMinB = 255, nMaxB = 0;

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int nr = farkR == 0
                        ? hedefAlt
                        : (int)Math.Round(hedefAlt + (rDizi[x, y] - minR) * aralikHedef / farkR);
                    int ng = farkG == 0
                        ? hedefAlt
                        : (int)Math.Round(hedefAlt + (gDizi[x, y] - minG) * aralikHedef / farkG);
                    int nb = farkB == 0
                        ? hedefAlt
                        : (int)Math.Round(hedefAlt + (bDizi[x, y] - minB) * aralikHedef / farkB);

                    nr = Math.Clamp(nr, 0, 255);
                    ng = Math.Clamp(ng, 0, 255);
                    nb = Math.Clamp(nb, 0, 255);

                    if (nr < nMinR) nMinR = nr; if (nr > nMaxR) nMaxR = nr;
                    if (ng < nMinG) nMinG = ng; if (ng > nMaxG) nMaxG = ng;
                    if (nb < nMinB) nMinB = nb; if (nb > nMaxB) nMaxB = nb;

                    normBmp.SetPixel(x, y, Color.FromArgb(nr, ng, nb));
                }
            }

            sonuc.SabitParlaklikResmi = sabitBmp;
            sonuc.NormalizeResmi = normBmp;
            sonuc.MinR = minR; sonuc.MaxR = maxR;
            sonuc.MinG = minG; sonuc.MaxG = maxG;
            sonuc.MinB = minB; sonuc.MaxB = maxB;
            sonuc.NormMinR = nMinR; sonuc.NormMaxR = nMaxR;
            sonuc.NormMinG = nMinG; sonuc.NormMaxG = nMaxG;
            sonuc.NormMinB = nMinB; sonuc.NormMaxB = nMaxB;
            return sonuc;
        }

        public static Bitmap OrnekGeceVideoKaresiOlustur(Bitmap temelSahne, int kareNo)
        {
            const int w = 240;
            const int h = 180;
            Bitmap kare = new Bitmap(w, h);

            using (Graphics g = Graphics.FromImage(kare))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                if (temelSahne != null)
                {
                    g.InterpolationMode = InterpolationMode.Bilinear;
                    g.DrawImage(temelSahne, 0, 0, w, h);
                }
                else
                {
                    g.Clear(Color.FromArgb(4, 6, 8));
                    using Brush binaFirca = new SolidBrush(Color.FromArgb(14, 17, 22));
                    using Brush pencereFirca = new SolidBrush(Color.FromArgb(26, 30, 22));
                    g.FillRectangle(binaFirca, 20, 45, 75, 110);
                    g.FillRectangle(binaFirca, 140, 35, 80, 120);
                    g.FillRectangle(pencereFirca, 35, 65, 18, 22);
                    g.FillRectangle(pencereFirca, 165, 55, 22, 26);
                }

                // Karanlık ortamda soldan sağa yürüyen düşük ışıklı figür + hafif fener/ışık hüzmesi
                int cx = 20 + ((kareNo * 5) % (w - 40));
                int cy = 104 + (int)(Math.Sin(kareNo * 0.4) * 3.0);

                using (Brush isikHuzmesi = new SolidBrush(Color.FromArgb(55, 42, 46, 34)))
                {
                    PointF[] koni =
                    {
                        new PointF(cx + 8, cy + 12),
                        new PointF(cx + 68, cy - 8),
                        new PointF(cx + 68, cy + 44)
                    };
                    g.FillPolygon(isikHuzmesi, koni);
                }

                using Brush figür = new SolidBrush(Color.FromArgb(28, 34, 30));
                g.FillEllipse(figür, cx - 7, cy - 18, 14, 14);
                g.FillRectangle(figür, cx - 8, cy - 3, 16, 28);
                g.FillRectangle(figür, cx - 7, cy + 25, 5, 18);
                g.FillRectangle(figür, cx + 2, cy + 25, 5, 18);

                using Font f = new Font("Consolas", 8f, FontStyle.Bold);
                using Brush zamanYazi = new SolidBrush(Color.FromArgb(36, 40, 38));
                g.DrawString($"GECE KAMERA #{kareNo + 1:000}", f, zamanYazi, 8, h - 18);
            }

            return kare;
        }
    }
}
