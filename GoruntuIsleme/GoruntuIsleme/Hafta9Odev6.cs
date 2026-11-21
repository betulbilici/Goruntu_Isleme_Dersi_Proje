using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GoruntuIsleme
{
    public enum HareketSinifi
    {
        Insan,
        AgacVeyaSalinim
    }

    public class TakipNesneBilgisi
    {
        public BitmapYardimcisi.BolgeBilgisi Bolge { get; set; }
        public HareketSinifi Sinif { get; set; }
        public double MerkezX { get; set; }
        public double MerkezY { get; set; }
        public double EnBoyOrani { get; set; }
        public double Doluluk { get; set; }
        public double YesilYaprakOrani { get; set; }
        public double NetYerDegistirme { get; set; }
        public int GozlemSayisi { get; set; }
        public string Aciklama { get; set; }
    }

    public class AgacInsanAnalizSonucu
    {
        public Bitmap BolgeMaskesi { get; set; }
        public Bitmap TakipResmi { get; set; }
        public List<TakipNesneBilgisi> Nesneler { get; set; } = new List<TakipNesneBilgisi>();
        public int InsanSayisi { get; set; }
        public int AgacSayisi { get; set; }
    }

    public class HareketIzleyici
    {
        private class IzKaydi
        {
            public List<(double x, double y)> Noktalar { get; } = new List<(double x, double y)>();
            public double SonX { get; set; }
            public double SonY { get; set; }
            public int KayipSayaci { get; set; }
            public bool Eslesti { get; set; }
        }

        private readonly List<IzKaydi> izler = new List<IzKaydi>();
        public List<(double x, double y)> InsanYorungesi { get; } = new List<(double x, double y)>();

        public void KareBaslat()
        {
            foreach (var iz in izler)
            {
                iz.Eslesti = false;
                iz.KayipSayaci++;
            }
        }

        public void GuncelleVeSor(double cx, double cy, out int gozlemSayisi, out double netYerDegistirme)
        {
            IzKaydi enYakin = null;
            double minDist = 38.0;

            foreach (var iz in izler)
            {
                if (iz.Eslesti) continue;
                double d = Math.Sqrt(Math.Pow(cx - iz.SonX, 2) + Math.Pow(cy - iz.SonY, 2));
                if (d <= minDist)
                {
                    minDist = d;
                    enYakin = iz;
                }
            }

            if (enYakin == null)
            {
                enYakin = new IzKaydi { SonX = cx, SonY = cy, KayipSayaci = 0, Eslesti = true };
                enYakin.Noktalar.Add((cx, cy));
                izler.Add(enYakin);
                gozlemSayisi = 1;
                netYerDegistirme = 0;
                return;
            }

            enYakin.Eslesti = true;
            enYakin.KayipSayaci = 0;
            enYakin.SonX = cx;
            enYakin.SonY = cy;
            enYakin.Noktalar.Add((cx, cy));
            if (enYakin.Noktalar.Count > 12)
                enYakin.Noktalar.RemoveAt(0);

            gozlemSayisi = enYakin.Noktalar.Count;
            var ilk = enYakin.Noktalar[0];
            netYerDegistirme = Math.Sqrt(Math.Pow(cx - ilk.x, 2) + Math.Pow(cy - ilk.y, 2));
        }

        public void InsanKonumuEkle(double cx, double cy)
        {
            InsanYorungesi.Add((cx, cy));
            if (InsanYorungesi.Count > 18)
                InsanYorungesi.RemoveAt(0);
        }

        public void KareBitir()
        {
            izler.RemoveAll(iz => iz.KayipSayaci > 4);
        }

        public void Sifirla()
        {
            izler.Clear();
            InsanYorungesi.Clear();
        }
    }

    public class Hafta9Odev6 : Form
    {
        private enum AkisModu
        {
            Statik,
            DeneySimulasyonu,
            CanliKamera,
            VideoDosyasi
        }

        private PictureBox picKare1, picKare2, picMaske, picTakip;
        private ModernSurgu trkEsik, trkMinAlan;
        private ModernHapRozet rozetK1, rozetK2, rozetMaske, rozetTakip;
        private ModernHapButon btnSimulasyon, btnKamera;
        private Label lblSonuc, lblAltAnaliz;

        private System.Windows.Forms.Timer animTimer;
        private OpenCvSharp.VideoCapture videoYakala;
        private AkisModu aktifAkis = AkisModu.Statik;
        private bool kareIsleniyor = false;

        private Bitmap kare1, kare2, arkaPlanRef, zeminOnbellek;
        private readonly HareketIzleyici izleyici = new HareketIzleyici();
        private int animAdim = 1;

        public Hafta9Odev6()
        {
            Text = "Hafta 9 - Ödev 6 | Ağaç ve İnsan Ayrımı ile Artikel (+) Takibi";
            FormGezintiYardimcisi.FormuHazirla(this);
            animTimer = new System.Windows.Forms.Timer { Interval = 160 };
            animTimer.Tick += AnimTimer_Tick;

            ArayuzuOlustur();
            DeneyOrtaminiSifirla();

            FormClosing += (s, e) =>
            {
                AkisiDurdur();
                kare1?.Dispose(); kare1 = null;
                kare2?.Dispose(); kare2 = null;
                arkaPlanRef?.Dispose(); arkaPlanRef = null;
                zeminOnbellek?.Dispose(); zeminOnbellek = null;
            };
        }

        private void ArayuzuOlustur()
        {
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                this,
                "Hafta 9 - Ödev 6 | Sallanan Ağaç & Yürüyen İnsan Ayrımı ile Artikel (+) Takibi",
                "< Ödev 5", () => { AkisiDurdur(); FormGezintiYardimcisi.FormGecisYap(this, new Hafta9Odev5()); },
                "Ana Menü >", () => { AkisiDurdur(); FormGezintiYardimcisi.AnaFormaDon(this); },
                sonrakiYesil: false,
                haftaRozetMetni: "HAFTA 9 • ÖDEV 6");

            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 305);
            pnlSag.AutoScroll = true;

            int x = 16;
            int w = 262;
            int y = 12;

            Label lblKaynak = new Label
            {
                Text = "DENEY ORTAMI VE CANLI AKIŞ",
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi
            };
            y += 20;

            btnSimulasyon = FormGezintiYardimcisi.ModernButonOlustur("Canlı Deney Ortamını Başlat", true, w, 34);
            btnSimulasyon.Location = new Point(x, y);
            btnSimulasyon.Click += BtnSimulasyon_Click;
            y += 40;

            btnKamera = FormGezintiYardimcisi.ModernButonOlustur("Canlı Kamera ile Takip", false, w, 32);
            btnKamera.Location = new Point(x, y);
            btnKamera.Click += BtnKamera_Click;
            y += 38;

            ModernHapButon btnVideoSec = FormGezintiYardimcisi.ModernButonOlustur("Video Dosyası Seç", false, w, 32);
            btnVideoSec.Location = new Point(x, y);
            btnVideoSec.Click += BtnVideoSec_Click;
            y += 40;

            Label lblDosyalar = new Label
            {
                Text = "KARE SEÇİMİ VE YÖNETİM",
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi
            };
            y += 18;

            ModernHapButon btnKare1Sec = FormGezintiYardimcisi.ModernButonOlustur("1. Kareyi Dosyadan Seç", false, w, 30);
            btnKare1Sec.Location = new Point(x, y);
            y += 36;

            ModernHapButon btnKare2Sec = FormGezintiYardimcisi.ModernButonOlustur("2. Kareyi Dosyadan Seç", false, w, 30);
            btnKare2Sec.Location = new Point(x, y);
            y += 36;

            ModernHapButon btnSifirla = FormGezintiYardimcisi.ModernButonOlustur("Deney Ortamını Sıfırla", false, w, 30);
            btnSifirla.Location = new Point(x, y);
            y += 36;

            ModernHapButon btnTemizle = FormGezintiYardimcisi.ModernButonOlustur("Temizle", false, w, 28);
            btnTemizle.Location = new Point(x, y);
            y += 36;

            btnKare1Sec.Click += (s, e) =>
            {
                string yol = FormGezintiYardimcisi.ResimDosyasiSec("1. Kareyi Seç (t-1)");
                if (yol == null) return;
                AkisiDurdur();
                arkaPlanRef?.Dispose();
                arkaPlanRef = null;
                izleyici.Sifirla();
                using Bitmap ham = new Bitmap(yol);
                kare1?.Dispose();
                kare1 = BitmapYardimcisi.BoyutSinirla(ham, 340);
                ResmiAta(picKare1, (Bitmap)kare1.Clone());
                rozetK1.MetinGuncelle("t - 1");
                AgacVeInsanAyirtEt(izleyiciKullan: false);
            };
            btnKare2Sec.Click += (s, e) =>
            {
                string yol = FormGezintiYardimcisi.ResimDosyasiSec("2. Kareyi Seç (t)");
                if (yol == null) return;
                AkisiDurdur();
                arkaPlanRef?.Dispose();
                arkaPlanRef = null;
                izleyici.Sifirla();
                using Bitmap ham = new Bitmap(yol);
                kare2?.Dispose();
                kare2 = BitmapYardimcisi.BoyutSinirla(ham, 340);
                ResmiAta(picKare2, (Bitmap)kare2.Clone());
                rozetK2.MetinGuncelle("Anlık (t)");
                AgacVeInsanAyirtEt(izleyiciKullan: false);
            };
            btnSifirla.Click += (s, e) => DeneyOrtaminiSifirla();
            btnTemizle.Click += (s, e) => Temizle();

            ModernKartPanel kartSonuc = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                new Point(x, y), new Size(w, 278), out lblSonuc);
            kartSonuc.Margin = new Padding(0, 0, 0, 14);
            lblSonuc.Font = new Font("Segoe UI", 8.3f);

            pnlSag.Controls.AddRange(new Control[]
            {
                lblKaynak, btnSimulasyon, btnKamera, btnVideoSec,
                lblDosyalar, btnKare1Sec, btnKare2Sec, btnSifirla, btnTemizle, kartSonuc
            });

            rozetK1 = new ModernHapRozet();
            rozetK1.MetinGuncelle("t - 1");

            rozetK2 = new ModernHapRozet();
            rozetK2.MetinGuncelle("Anlık (t)");

            rozetMaske = new ModernHapRozet();
            rozetMaske.MetinGuncelle("Eşik: 28");

            rozetTakip = new ModernHapRozet();
            rozetTakip.MetinGuncelle("Min: 250 px");

            Panel pnlMaskeAlt = FormGezintiYardimcisi.KartAltSurguPaneliOlustur(
                "Hareket Fark Eşiği", "10 – 100", 10, 100, 28, out trkEsik, out _);
            trkEsik.Scroll += (s, e) =>
            {
                rozetMaske.MetinGuncelle($"Eşik: {trkEsik.Value}");
                AgacVeInsanAyirtEt(izleyiciKullan: false);
            };

            Panel pnlTakipAlt = FormGezintiYardimcisi.KartAltSurguPaneliOlustur(
                "Min İnsan Kütle Alanı", "50 – 1500 px", 50, 1500, 250, out trkMinAlan, out _);
            trkMinAlan.Scroll += (s, e) =>
            {
                rozetTakip.MetinGuncelle($"Min: {trkMinAlan.Value} px");
                AgacVeInsanAyirtEt(izleyiciKullan: false);
            };

            TableLayoutPanel tbl = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(14),
                ColumnCount = 2,
                RowCount = 3,
                BackColor = FormGezintiYardimcisi.TemaZemin
            };
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 43f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 45f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 12f));

            picKare1 = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 0, 0, "1. Kare (t-1 Zaman Adımı)", null, null, rozetK1);
            picKare2 = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 1, 0, "2. Kare (Sallanan Ağaç & Yürüyen İnsan)", null, null, rozetK2);
            picMaske = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 0, 1, "Bölge Maskesi (Morfolojik Temizlik)", null, pnlMaskeAlt, rozetMaske);
            picTakip = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 1, 1, "Ağaç/İnsan Ayrımı & Artikel (+) Takibi", null, pnlTakipAlt, rozetTakip);

            ModernKartPanel altSerit = FormGezintiYardimcisi.AltAnalizSeridiOlustur(
                "Analiz hazır.",
                out lblAltAnaliz,
                "Artikel (+)");
            tbl.Controls.Add(altSerit, 0, 2);
            tbl.SetColumnSpan(altSerit, 2);

            Controls.Add(tbl);
            FormGezintiYardimcisi.YerlesimiDuzenle(this, pnlUst, pnlSag, tbl);
        }

        private void ZeminOnbelleginiHazirla()
        {
            if (zeminOnbellek != null) return;
            Bitmap yuklenen = BitmapYardimcisi.OrnekGorselYukle("manzara_arkaplan.jpg");
            if (yuklenen != null)
            {
                zeminOnbellek = new Bitmap(yuklenen, 320, 210);
                yuklenen.Dispose();
                return;
            }

            Bitmap sentetik = new Bitmap(320, 210);
            using (Graphics g = Graphics.FromImage(sentetik))
            {
                g.Clear(Color.FromArgb(145, 195, 235));
                using Brush yer = new SolidBrush(Color.FromArgb(120, 145, 105));
                g.FillRectangle(yer, 0, 132, 320, 78);
            }
            zeminOnbellek = sentetik;
        }

        private void DeneyOrtaminiSifirla()
        {
            AkisiDurdur();
            ZeminOnbelleginiHazirla();
            izleyici.Sifirla();

            arkaPlanRef?.Dispose();
            arkaPlanRef = DeneySahnesiOlustur(zeminOnbellek, 0, sadeceDuraganArkaPlan: true);

            kare1?.Dispose();
            kare1 = DeneySahnesiOlustur(zeminOnbellek, 2, sadeceDuraganArkaPlan: false);
            kare2?.Dispose();
            kare2 = DeneySahnesiOlustur(zeminOnbellek, 3, sadeceDuraganArkaPlan: false);
            animAdim = 3;

            ResmiAta(picKare1, (Bitmap)kare1.Clone());
            ResmiAta(picKare2, (Bitmap)kare2.Clone());
            rozetK1.MetinGuncelle("t - 1");
            rozetK2.MetinGuncelle("Adım #3");

            AgacVeInsanAyirtEt(izleyiciKullan: true);
        }

        private void BtnSimulasyon_Click(object sender, EventArgs e)
        {
            if (aktifAkis == AkisModu.DeneySimulasyonu)
            {
                AkisiDurdur();
                return;
            }

            AkisiDurdur();
            ZeminOnbelleginiHazirla();
            izleyici.Sifirla();
            arkaPlanRef?.Dispose();
            arkaPlanRef = DeneySahnesiOlustur(zeminOnbellek, 0, sadeceDuraganArkaPlan: true);

            kare1?.Dispose();
            kare1 = DeneySahnesiOlustur(zeminOnbellek, 0, sadeceDuraganArkaPlan: false);
            ResmiAta(picKare1, (Bitmap)kare1.Clone());
            animAdim = 1;

            aktifAkis = AkisModu.DeneySimulasyonu;
            btnSimulasyon.Text = "Deney Ortamını Durdur";
            animTimer.Start();
        }

        private void BtnKamera_Click(object sender, EventArgs e)
        {
            if (aktifAkis == AkisModu.CanliKamera)
            {
                AkisiDurdur();
                return;
            }

            AkisiDurdur();
            OpenCvSharp.VideoCapture yeni = new OpenCvSharp.VideoCapture(0);
            if (!yeni.IsOpened())
            {
                yeni.Dispose();
                MessageBox.Show("Sistemde kullanılabilir bir kamera bulunamadı.",
                    "Kamera Açılamadı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            izleyici.Sifirla();
            kare1?.Dispose(); kare1 = null;
            kare2?.Dispose(); kare2 = null;
            arkaPlanRef?.Dispose(); arkaPlanRef = null;
            videoYakala = yeni;
            aktifAkis = AkisModu.CanliKamera;
            btnKamera.Text = "Kamerayı Durdur";
            animTimer.Start();
        }

        private void BtnVideoSec_Click(object sender, EventArgs e)
        {
            using OpenFileDialog dlg = new OpenFileDialog
            {
                Title = "Ağaç / İnsan Takibi İçin Video Seç",
                Filter = "Video Dosyaları|*.mp4;*.avi;*.mov;*.mkv;*.wmv|Tüm Dosyalar|*.*"
            };
            if (dlg.ShowDialog() != DialogResult.OK) return;

            AkisiDurdur();
            OpenCvSharp.VideoCapture yeni = new OpenCvSharp.VideoCapture(dlg.FileName);
            if (!yeni.IsOpened())
            {
                yeni.Dispose();
                MessageBox.Show("Seçilen video dosyası açılamadı.", "Video Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            izleyici.Sifirla();
            kare1?.Dispose(); kare1 = null;
            kare2?.Dispose(); kare2 = null;
            arkaPlanRef?.Dispose(); arkaPlanRef = null;
            videoYakala = yeni;
            aktifAkis = AkisModu.VideoDosyasi;
            animTimer.Start();
        }

        private void AkisiDurdur()
        {
            animTimer?.Stop();
            if (videoYakala != null)
            {
                videoYakala.Release();
                videoYakala.Dispose();
                videoYakala = null;
            }
            aktifAkis = AkisModu.Statik;
            if (btnSimulasyon != null) btnSimulasyon.Text = "Canlı Deney Ortamını Başlat";
            if (btnKamera != null) btnKamera.Text = "Canlı Kamera ile Takip";
        }

        private void AnimTimer_Tick(object sender, EventArgs e)
        {
            if (IsDisposed || kareIsleniyor || aktifAkis == AkisModu.Statik) return;

            kareIsleniyor = true;
            try
            {
                if (aktifAkis == AkisModu.DeneySimulasyonu)
                {
                    Bitmap yeniKare = DeneySahnesiOlustur(zeminOnbellek, animAdim, sadeceDuraganArkaPlan: false);
                    if (kare2 != null)
                    {
                        kare1?.Dispose();
                        kare1 = kare2;
                        ResmiAta(picKare1, (Bitmap)kare1.Clone());
                    }
                    kare2 = yeniKare;
                    ResmiAta(picKare2, (Bitmap)kare2.Clone());
                    rozetK2.MetinGuncelle($"Adım #{animAdim}");
                    animAdim = (animAdim + 1) % 24;

                    AgacVeInsanAyirtEt(izleyiciKullan: true);
                    return;
                }

                if (videoYakala == null || !videoYakala.IsOpened()) return;

                using OpenCvSharp.Mat mat = new OpenCvSharp.Mat();
                bool ok = videoYakala.Read(mat);
                if ((!ok || mat.Empty()) && aktifAkis == AkisModu.VideoDosyasi)
                {
                    videoYakala.Set(OpenCvSharp.VideoCaptureProperties.PosFrames, 0);
                    ok = videoYakala.Read(mat);
                    izleyici.Sifirla();
                }

                if (!ok || mat.Empty())
                {
                    AkisiDurdur();
                    return;
                }

                using Bitmap ham = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(mat);
                Bitmap kare = BitmapYardimcisi.BoyutSinirla(ham, 280);

                if (kare1 == null)
                {
                    kare1 = kare;
                    arkaPlanRef = (Bitmap)kare.Clone();
                    ResmiAta(picKare1, (Bitmap)kare1.Clone());
                    return;
                }

                if (kare2 != null)
                {
                    kare1.Dispose();
                    kare1 = kare2;
                    ResmiAta(picKare1, (Bitmap)kare1.Clone());
                }

                kare2 = kare;
                ResmiAta(picKare2, (Bitmap)kare2.Clone());
                AgacInsanAnalizSonucu sonuc = AgacVeInsanAyirtEt(izleyiciKullan: true);

                // Kamera/video modunda arka planı insan dışı bölgelerde yavaşça güncelle
                if (sonuc != null && arkaPlanRef != null)
                {
                    Bitmap yeniRef = ArkaPlanYavasGuncelle(arkaPlanRef, kare2, sonuc.Nesneler, 0.08);
                    arkaPlanRef.Dispose();
                    arkaPlanRef = yeniRef;
                }
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

        private static Bitmap ArkaPlanYavasGuncelle(Bitmap mevcutRef, Bitmap guncel, List<TakipNesneBilgisi> nesneler, double alpha)
        {
            int w = mevcutRef.Width;
            int h = mevcutRef.Height;
            Bitmap cikis = new Bitmap(w, h);
            double birEksi = 1.0 - alpha;

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    bool insanKutusuIcinde = false;
                    foreach (var n in nesneler)
                    {
                        if (n.Sinif == HareketSinifi.Insan &&
                            x >= n.Bolge.MinX - 4 && x <= n.Bolge.MaxX + 4 &&
                            y >= n.Bolge.MinY - 4 && y <= n.Bolge.MaxY + 4)
                        {
                            insanKutusuIcinde = true;
                            break;
                        }
                    }

                    Color cb = mevcutRef.GetPixel(x, y);
                    if (insanKutusuIcinde)
                    {
                        cikis.SetPixel(x, y, cb);
                        continue;
                    }

                    Color cu = guncel.GetPixel(x, y);
                    int r = Math.Clamp((int)Math.Round(birEksi * cb.R + alpha * cu.R), 0, 255);
                    int g = Math.Clamp((int)Math.Round(birEksi * cb.G + alpha * cu.G), 0, 255);
                    int b = Math.Clamp((int)Math.Round(birEksi * cb.B + alpha * cu.B), 0, 255);
                    cikis.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
            return cikis;
        }

        private void Temizle()
        {
            AkisiDurdur();
            izleyici.Sifirla();
            kare1?.Dispose(); kare1 = null;
            kare2?.Dispose(); kare2 = null;
            arkaPlanRef?.Dispose(); arkaPlanRef = null;

            ResmiAta(picKare1, null);
            ResmiAta(picKare2, null);
            ResmiAta(picMaske, null);
            ResmiAta(picTakip, null);

            lblSonuc.Text = "Görseller temizlendi.\nCanlı deney ortamını başlatın veya kare seçin.";
            lblAltAnaliz.Text = "Takip bekleniyor.";
        }

        private static void ResmiAta(PictureBox pic, Bitmap yeni)
        {
            Image onceki = pic.Image;
            pic.Image = yeni;
            onceki?.Dispose();
        }

        private AgacInsanAnalizSonucu AgacVeInsanAyirtEt(bool izleyiciKullan)
        {
            if (kare1 == null || kare2 == null) return null;

            AgacInsanAnalizSonucu sonuc = AgacVeInsanTespitEt(
                kare1,
                kare2,
                arkaPlanRef,
                trkEsik.Value,
                trkMinAlan.Value,
                izleyiciKullan ? izleyici : null);

            ResmiAta(picMaske, sonuc.BolgeMaskesi);
            ResmiAta(picTakip, sonuc.TakipResmi);

            string detay = "";
            foreach (var n in sonuc.Nesneler)
            {
                if (n.Sinif == HareketSinifi.Insan)
                {
                    detay = $"\n\nAktif İnsan Merkezi: ({n.MerkezX:F0}, {n.MerkezY:F0})\n" +
                            $"• Alan / H/W : {n.Bolge.PikselSayisi} px / {n.EnBoyOrani:F2}\n" +
                            $"• Yeşil Oran : %{n.YesilYaprakOrani * 100:F1}\n" +
                            $"• Yer Değiştirme: {n.NetYerDegistirme:F1} px";
                    break;
                }
            }

            lblSonuc.Text = $"SINIFLANDIRMA VE TAKİP\n\n" +
                            $"• Tespit Edilen İnsan : {sonuc.InsanSayisi} (Artikel +)\n" +
                            $"• Elenen Ağaç/Salınım : {sonuc.AgacSayisi} parça" +
                            detay +
                            "\n\nAyırt Etme Kriterleri:\n" +
                            "1. Yeşil Yaprak Oranı (2G-R-B)\n" +
                            "2. Bütünleşik Alan & H/W>=1.15\n" +
                            "3. Çok Kareli Salınım Filtresi";

            lblAltAnaliz.Text = $"İnsan (Artikel +): {sonuc.InsanSayisi} | Elenen Ağaç/Salınım: {sonuc.AgacSayisi} | Eşik: {trkEsik.Value} | Min Alan: {trkMinAlan.Value} px";
            return sonuc;
        }

        public static AgacInsanAnalizSonucu AgacVeInsanTespitEt(
            Bitmap oncekiKare,
            Bitmap guncelKare,
            Bitmap arkaPlanReferans,
            int hareketEsik,
            int minInsanAlani,
            HareketIzleyici izleyici = null)
        {
            AgacInsanAnalizSonucu sonuc = new AgacInsanAnalizSonucu();
            if (guncelKare == null || (oncekiKare == null && arkaPlanReferans == null)) return sonuc;

            int w = guncelKare.Width;
            int h = guncelKare.Height;

            using Bitmap oncekiOlcekli = oncekiKare == null || (oncekiKare.Width == w && oncekiKare.Height == h)
                ? null
                : new Bitmap(oncekiKare, w, h);
            Bitmap oncekiKaynak = oncekiKare == null ? null : (oncekiOlcekli ?? oncekiKare);

            using Bitmap arkaOlcekli = arkaPlanReferans == null || (arkaPlanReferans.Width == w && arkaPlanReferans.Height == h)
                ? null
                : new Bitmap(arkaPlanReferans, w, h);
            Bitmap arkaKaynak = arkaPlanReferans == null ? null : (arkaOlcekli ?? arkaPlanReferans);

            Color[,] guncelRenk = new Color[w, h];
            bool[,] onPlanMaske = new bool[w, h];
            bool[,] anlikHareket = new bool[w, h];

            Bitmap zeminReferans = arkaKaynak ?? oncekiKaynak;
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    Color cg = guncelKare.GetPixel(x, y);
                    guncelRenk[x, y] = cg;

                    Color cz = zeminReferans.GetPixel(x, y);
                    int farkZ = (Math.Abs(cg.R - cz.R) + Math.Abs(cg.G - cz.G) + Math.Abs(cg.B - cz.B)) / 3;
                    onPlanMaske[x, y] = farkZ >= hareketEsik;

                    if (oncekiKaynak != null)
                    {
                        Color co = oncekiKaynak.GetPixel(x, y);
                        int farkO = (Math.Abs(cg.R - co.R) + Math.Abs(cg.G - co.G) + Math.Abs(cg.B - co.B)) / 3;
                        anlikHareket[x, y] = farkO >= hareketEsik;
                    }
                    else
                    {
                        anlikHareket[x, y] = onPlanMaske[x, y];
                    }
                }
            }

            onPlanMaske = BitmapYardimcisi.MaskeAcmaKapama(onPlanMaske, w, h, acmaTekrar: 1, kapamaTekrar: 2);

            using Bitmap ikiliMaske = new Bitmap(w, h);
            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                    ikiliMaske.SetPixel(x, y, onPlanMaske[x, y] ? Color.White : Color.Black);

            Bitmap bolgeli = BitmapYardimcisi.BolgeBulVeRenklendir(
                ikiliMaske, 25, out List<BitmapYardimcisi.BolgeBilgisi> bolgeler, out int[,] etiketler);

            izleyici?.KareBaslat();

            foreach (var b in bolgeler)
            {
                int anlikHareketSay = 0;
                int yesilPikselSay = 0;
                int pikselSay = 0;

                for (int py = b.MinY; py <= b.MaxY; py++)
                {
                    for (int px = b.MinX; px <= b.MaxX; px++)
                    {
                        if (etiketler[px, py] != b.EtiketNo) continue;
                        pikselSay++;
                        if (anlikHareket[px, py]) anlikHareketSay++;

                        Color c = guncelRenk[px, py];
                        if (c.G > c.R + 10 && c.G > c.B + 10 && (2 * c.G - c.R - c.B) >= 24)
                            yesilPikselSay++;
                    }
                }

                // Sabit arka plan farkında tamamen durağan cisimleri ele
                if (arkaKaynak != null && oncekiKaynak != null && anlikHareketSay < 8)
                    continue;

                double enBoy = (double)b.Yukseklik / Math.Max(1, b.Genislik);
                double doluluk = (double)b.PikselSayisi / Math.Max(1, b.Genislik * b.Yukseklik);
                double yesilOran = (double)yesilPikselSay / Math.Max(1, pikselSay);

                int gozlemSay = 1;
                double yerDegis = 0;
                if (izleyici != null)
                    izleyici.GuncelleVeSor(b.MerkezX, b.MerkezY, out gozlemSay, out yerDegis);

                bool agacRengi = yesilOran >= 0.42;
                bool yerindeSalinim = gozlemSay >= 4 && yerDegis < 9.0;
                bool insanFormu = b.PikselSayisi >= minInsanAlani && enBoy >= 1.15 && enBoy <= 4.5 && doluluk >= 0.35;
                bool insanMi = insanFormu && !agacRengi && !yerindeSalinim;

                string neden = insanMi
                    ? "İnsan"
                    : (agacRengi ? "Yeşil Yaprak" : (yerindeSalinim ? "Salınım" : "Küçük/Biçim Dışı"));

                TakipNesneBilgisi bilgi = new TakipNesneBilgisi
                {
                    Bolge = b,
                    Sinif = insanMi ? HareketSinifi.Insan : HareketSinifi.AgacVeyaSalinim,
                    MerkezX = b.MerkezX,
                    MerkezY = b.MerkezY,
                    EnBoyOrani = enBoy,
                    Doluluk = doluluk,
                    YesilYaprakOrani = yesilOran,
                    NetYerDegistirme = yerDegis,
                    GozlemSayisi = gozlemSay,
                    Aciklama = neden
                };

                sonuc.Nesneler.Add(bilgi);
                if (insanMi)
                {
                    sonuc.InsanSayisi++;
                    izleyici?.InsanKonumuEkle(b.MerkezX, b.MerkezY);
                }
                else
                {
                    sonuc.AgacSayisi++;
                }
            }

            izleyici?.KareBitir();

            Bitmap takipBmp = new Bitmap(guncelKare);
            using (Graphics g = Graphics.FromImage(takipBmp))
            using (Font f = new Font("Segoe UI", 8.0f, FontStyle.Bold))
            using (Pen pAgac = new Pen(Color.Orange, 1.3f) { DashStyle = DashStyle.Dash })
            using (Pen pInsan = new Pen(Color.Red, 2.4f))
            using (Pen pIz = new Pen(Color.FromArgb(200, 255, 60, 60), 1.8f))
            using (Brush bgEtiket = new SolidBrush(Color.FromArgb(190, 15, 23, 42)))
            {
                if (izleyici != null && izleyici.InsanYorungesi.Count >= 2)
                {
                    for (int i = 1; i < izleyici.InsanYorungesi.Count; i++)
                    {
                        var p0 = izleyici.InsanYorungesi[i - 1];
                        var p1 = izleyici.InsanYorungesi[i];
                        g.DrawLine(pIz, (float)p0.x, (float)p0.y, (float)p1.x, (float)p1.y);
                    }
                }

                foreach (var n in sonuc.Nesneler)
                {
                    var b = n.Bolge;
                    if (n.Sinif == HareketSinifi.Insan)
                    {
                        int mx = (int)Math.Round(n.MerkezX);
                        int my = (int)Math.Round(n.MerkezY);

                        g.DrawRectangle(pInsan, b.MinX, b.MinY, b.Genislik, b.Yukseklik);
                        g.DrawEllipse(pInsan, mx - 8, my - 8, 16, 16);
                        g.DrawLine(pInsan, mx - 14, my, mx + 14, my);
                        g.DrawLine(pInsan, mx, my - 14, mx, my + 14);

                        string tag = "İNSAN (+)";
                        SizeF sz = g.MeasureString(tag, f);
                        int ty = b.MinY >= 17 ? b.MinY - 16 : b.MinY + 2;
                        g.FillRectangle(bgEtiket, b.MinX, ty, sz.Width + 4, 15);
                        g.DrawString(tag, f, Brushes.Red, b.MinX + 2, ty + 1);
                    }
                    else
                    {
                        g.DrawRectangle(pAgac, b.MinX, b.MinY, b.Genislik, b.Yukseklik);
                        string tag = "Ağaç/Salınım";
                        SizeF sz = g.MeasureString(tag, f);
                        int ty = Math.Max(0, b.MinY - 14);
                        g.FillRectangle(bgEtiket, b.MinX, ty, sz.Width + 3, 13);
                        g.DrawString(tag, f, Brushes.Orange, b.MinX + 1, ty);
                    }
                }
            }

            sonuc.BolgeMaskesi = bolgeli;
            sonuc.TakipResmi = takipBmp;
            return sonuc;
        }

        public static Bitmap DeneySahnesiOlustur(
            Bitmap zeminResmi,
            int adim,
            bool sadeceDuraganArkaPlan,
            bool insanGizle = false)
        {
            const int w = 320;
            const int h = 210;
            Bitmap bmp = new Bitmap(w, h);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                if (zeminResmi != null)
                {
                    g.DrawImage(zeminResmi, 0, 0, w, h);
                }
                else
                {
                    g.Clear(Color.FromArgb(145, 195, 235));
                    using Brush yer = new SolidBrush(Color.FromArgb(120, 145, 105));
                    g.FillRectangle(yer, 0, 132, w, 78);
                }

                int salinim = sadeceDuraganArkaPlan
                    ? 0
                    : (int)Math.Round(Math.Sin(adim * 0.75) * 6.0);

                // Sol ağaç
                CizAgac(g, 48, 92, salinim);
                // Sağ ağaç
                CizAgac(g, 268, 96, -salinim);

                // Yürüyen insan (baş + gövde + bacaklar bitişik bütünleşik kütle)
                if (!sadeceDuraganArkaPlan && !insanGizle)
                {
                    int insanX = 78 + (adim % 24) * 7;
                    int bacakAdim = (int)Math.Round(Math.Sin(adim * 0.9) * 5.0);

                    using Brush bTen = new SolidBrush(Color.FromArgb(235, 190, 160));
                    using Brush bMont = new SolidBrush(Color.FromArgb(215, 38, 38));
                    using Brush bPant = new SolidBrush(Color.FromArgb(30, 42, 68));

                    // Baş (y=92..108, gövde ile 1px birleşik)
                    g.FillEllipse(bTen, insanX + 5, 92, 16, 17);
                    // Gövde (y=108..141)
                    g.FillRectangle(bMont, insanX, 108, 26, 33);
                    // Kollar
                    g.FillRectangle(bMont, insanX - 3, 111 - bacakAdim / 2, 4, 22);
                    g.FillRectangle(bMont, insanX + 25, 111 + bacakAdim / 2, 4, 22);
                    // Bacaklar (y=139..168, gövdeyle 2px birleşik)
                    g.FillRectangle(bPant, insanX + 3, 139, 8, 28 + Math.Max(0, bacakAdim));
                    g.FillRectangle(bPant, insanX + 15, 139, 8, 28 + Math.Max(0, -bacakAdim));
                }
            }

            return bmp;
        }

        private static void CizAgac(Graphics g, int govdeX, int govdeY, int salinim)
        {
            using Brush bGovde = new SolidBrush(Color.FromArgb(100, 68, 40));
            using Brush bYaprak1 = new SolidBrush(Color.FromArgb(28, 128, 40));
            using Brush bYaprak2 = new SolidBrush(Color.FromArgb(38, 152, 48));
            using Brush bYaprak3 = new SolidBrush(Color.FromArgb(24, 110, 34));

            g.FillRectangle(bGovde, govdeX, govdeY, 14, 58);
            g.FillEllipse(bYaprak1, govdeX - 24 + salinim, govdeY - 46, 44, 38);
            g.FillEllipse(bYaprak2, govdeX - 2 - salinim / 2, govdeY - 52, 42, 40);
            g.FillEllipse(bYaprak3, govdeX - 10 + salinim / 2, govdeY - 26, 46, 34);
        }
    }
}
