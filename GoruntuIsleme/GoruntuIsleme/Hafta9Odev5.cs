using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GoruntuIsleme
{
    public class HareketHedefSonucu
    {
        public Bitmap FarkMaskesi { get; set; }
        public Bitmap NisanResmi { get; set; }
        public bool HedefBulundu { get; set; }
        public int KameraMerkezX { get; set; }
        public int KameraMerkezY { get; set; }
        public int HedefMerkezX { get; set; }
        public int HedefMerkezY { get; set; }
        public int FarkX { get; set; }
        public int FarkY { get; set; }
        public double YatayDonusAci { get; set; }
        public double DikeyDonusAci { get; set; }
        public int HareketPikselSayisi { get; set; }
        public string YatayYon { get; set; }
        public string DikeyYon { get; set; }
    }

    public class Hafta9Odev5 : Form
    {
        private enum AkisModu
        {
            Statik,
            OtoSimulasyon,
            CanliKamera,
            VideoDosyasi
        }

        private PictureBox picKare1, picKare2, picFark, picNisan;
        private ModernSurgu trkEsik, trkHedefX, trkHedefY;
        private ModernHapRozet rozetK1, rozetK2, rozetFark, rozetNisan;
        private ModernHapButon btnOtoSimulasyon, btnKamera;
        private CheckBox chkGurultuTemizle;
        private Label lblHesap, lblAltAnaliz;
        private NumericUpDown numFovYatay, numFovDikey;

        private Bitmap kare1, kare2;
        private Bitmap arkaPlanOnbellek;
        private OpenCvSharp.VideoCapture videoYakala;
        private System.Windows.Forms.Timer akisZamanlayici;
        private AkisModu aktifAkis = AkisModu.Statik;
        private bool kareIsleniyor = false;
        private bool programatikDegisim = false;
        private int simAdim = 0;

        public Hafta9Odev5()
        {
            Text = "Hafta 9 - Ödev 5 | Hareket Sensörü ve Kamera Ekseni Dönüş Hesabı";
            FormGezintiYardimcisi.FormuHazirla(this);
            akisZamanlayici = new System.Windows.Forms.Timer { Interval = 75 };
            akisZamanlayici.Tick += AkisZamanlayici_Tick;

            ArayuzuOlustur();
            OrnekKareleriYukle();

            this.FormClosing += (s, e) =>
            {
                AkisiDurdur();
                kare1?.Dispose();
                kare1 = null;
                kare2?.Dispose();
                kare2 = null;
                arkaPlanOnbellek?.Dispose();
                arkaPlanOnbellek = null;
            };
        }

        private void ArayuzuOlustur()
        {
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                this,
                "Hafta 9 - Ödev 5 | Hareket Sensörü ile Hedef Merkezi ve Kamera Ekseni Dönüş Hesabı",
                "< Ödev 4", () => { AkisiDurdur(); FormGezintiYardimcisi.FormGecisYap(this, new Hafta9Odev4()); },
                "Ödev 6 >", () => { AkisiDurdur(); FormGezintiYardimcisi.FormGecisYap(this, new Hafta9Odev6()); },
                false,
                "HAFTA 9 • ÖDEV 5");

            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 305);
            pnlSag.AutoScroll = true;

            int x = 16;
            int w = 262;
            int y = 10;

            Label lblSensor = new Label
            {
                Text = "HAREKET SENSÖRÜ AKIŞI",
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi
            };
            y += 18;

            btnOtoSimulasyon = FormGezintiYardimcisi.ModernButonOlustur("Otomatik Hedef Hareketini Başlat", true, w, 32);
            btnOtoSimulasyon.Location = new Point(x, y);
            btnOtoSimulasyon.Click += BtnOtoSimulasyon_Click;
            y += 38;

            btnKamera = FormGezintiYardimcisi.ModernButonOlustur("Canlı Kamera Hareket Sensörü", false, w, 32);
            btnKamera.Location = new Point(x, y);
            btnKamera.Click += BtnKamera_Click;
            y += 38;

            ModernHapButon btnVideoSec = FormGezintiYardimcisi.ModernButonOlustur("Video Dosyası Seç", false, w, 32);
            btnVideoSec.Location = new Point(x, y);
            btnVideoSec.Click += BtnVideoSec_Click;
            y += 40;

            Label lblKaynak = new Label
            {
                Text = "MANUEL KARE & AYARLAR",
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi
            };
            y += 18;

            ModernHapButon btnKare1Sec = FormGezintiYardimcisi.ModernButonOlustur("1. Kareyi Seç (t-1)", false, w, 30);
            btnKare1Sec.Location = new Point(x, y);
            y += 35;

            ModernHapButon btnKare2Sec = FormGezintiYardimcisi.ModernButonOlustur("2. Kareyi Seç (t)", false, w, 30);
            btnKare2Sec.Location = new Point(x, y);
            y += 35;

            ModernHapButon btnOrnek = FormGezintiYardimcisi.ModernButonOlustur("Statik Örneği Sıfırla", false, w, 30);
            btnOrnek.Location = new Point(x, y);
            y += 35;

            ModernHapButon btnTemizle = FormGezintiYardimcisi.ModernButonOlustur("Temizle", false, w, 28);
            btnTemizle.Location = new Point(x, y);
            y += 36;

            btnKare1Sec.Click += (s, e) =>
            {
                string yol = FormGezintiYardimcisi.ResimDosyasiSec("1. Kareyi (t-1 Referans) Seç");
                if (yol == null) return;
                AkisiDurdur();
                using Bitmap ham = new Bitmap(yol);
                kare1?.Dispose();
                kare1 = BitmapYardimcisi.BoyutSinirla(ham, 380);
                ResmiAta(picKare1, (Bitmap)kare1.Clone());
                rozetK1.MetinGuncelle("t-1 Kare");
                HedefiBulVeHesapla();
            };
            btnKare2Sec.Click += (s, e) =>
            {
                string yol = FormGezintiYardimcisi.ResimDosyasiSec("2. Kareyi (t Hareketli) Seç");
                if (yol == null) return;
                AkisiDurdur();
                using Bitmap ham = new Bitmap(yol);
                kare2?.Dispose();
                kare2 = BitmapYardimcisi.BoyutSinirla(ham, 380);
                ResmiAta(picKare2, (Bitmap)kare2.Clone());
                rozetK2.MetinGuncelle("t Kare");
                HedefiBulVeHesapla();
            };
            btnOrnek.Click += (s, e) => OrnekKareleriYukle();
            btnTemizle.Click += (s, e) => Temizle();

            Label lblHedefY = new Label
            {
                Text = "Simülasyon Hedef Dikey Konum (Y):",
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI", 8.3f),
                ForeColor = FormGezintiYardimcisi.AcikYazi
            };
            y += 18;

            trkHedefY = new ModernSurgu
            {
                Location = new Point(x, y),
                Width = w,
                Minimum = 25,
                Maximum = 190,
                Value = 65
            };
            trkHedefY.Scroll += (s, e) =>
            {
                if (!programatikDegisim)
                    SimulasyonKaresiCizVeHesapla();
            };
            y += 28;

            chkGurultuTemizle = new CheckBox
            {
                Text = "Gürültü Temizle & En Büyük Hedef",
                Location = new Point(x, y),
                Size = new Size(w, 22),
                Font = new Font("Segoe UI", 8.4f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi,
                Checked = true
            };
            chkGurultuTemizle.CheckedChanged += (s, e) => HedefiBulVeHesapla();
            y += 26;

            Label lblFov = new Label
            {
                Text = "Kamera Görüş Açısı (Yatay / Dikey FOV °):",
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI", 8.3f),
                ForeColor = FormGezintiYardimcisi.AcikYazi
            };
            y += 18;

            numFovYatay = new NumericUpDown { Location = new Point(x, y), Width = 125, Minimum = 10, Maximum = 150, Value = 60 };
            numFovDikey = new NumericUpDown { Location = new Point(x + 137, y), Width = 125, Minimum = 10, Maximum = 120, Value = 45 };
            numFovYatay.ValueChanged += (s, e) => HedefiBulVeHesapla();
            numFovDikey.ValueChanged += (s, e) => HedefiBulVeHesapla();
            y += 30;

            ModernKartPanel kartHesap = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                new Point(x, y), new Size(w, 226), out lblHesap);
            kartHesap.Margin = new Padding(0, 0, 0, 14);
            lblHesap.Font = new Font("Segoe UI", 8.4f);

            pnlSag.Controls.AddRange(new Control[]
            {
                lblSensor, btnOtoSimulasyon, btnKamera, btnVideoSec,
                lblKaynak, btnKare1Sec, btnKare2Sec, btnOrnek, btnTemizle,
                lblHedefY, trkHedefY, chkGurultuTemizle, lblFov, numFovYatay, numFovDikey, kartHesap
            });

            rozetK1 = new ModernHapRozet();
            rozetK1.MetinGuncelle("Referans");

            rozetK2 = new ModernHapRozet();
            rozetK2.MetinGuncelle("X: 235");

            rozetFark = new ModernHapRozet();
            rozetFark.MetinGuncelle("Eşik: 30");

            rozetNisan = new ModernHapRozet();
            rozetNisan.MetinGuncelle("TAKİPTE");

            Panel pnlK2Alt = FormGezintiYardimcisi.KartAltSurguPaneliOlustur(
                "Simülasyon Hedef Yatay Konumu (X)", "25 – 290 px", 25, 290, 235, out trkHedefX, out _);
            trkHedefX.Scroll += (s, e) =>
            {
                rozetK2.MetinGuncelle($"X: {trkHedefX.Value}");
                if (!programatikDegisim)
                    SimulasyonKaresiCizVeHesapla();
            };

            Panel pnlFarkAlt = FormGezintiYardimcisi.KartAltSurguPaneliOlustur(
                "Hareket Algılama Eşik Değeri", "10 – 120", 10, 120, 30, out trkEsik, out _);
            trkEsik.Scroll += (s, e) =>
            {
                rozetFark.MetinGuncelle($"Eşik: {trkEsik.Value}");
                HedefiBulVeHesapla();
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
                tbl, 0, 0, "1. Kare (t-1 / Referans)", null, null, rozetK1);
            picKare2 = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 1, 0, "2. Kare (t / Hareketli)", null, pnlK2Alt, rozetK2);
            picFark = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 0, 1, "Hareket Maskesi & Eksenler (+)", null, pnlFarkAlt, rozetFark);
            picNisan = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 1, 1, "Kamera/Silah Ekseni & Takip Vektörü", null, null, rozetNisan);

            ModernKartPanel altSerit = FormGezintiYardimcisi.AltAnalizSeridiOlustur(
                "Analiz hazır.",
                out lblAltAnaliz,
                "Eksen Takip");
            tbl.Controls.Add(altSerit, 0, 2);
            tbl.SetColumnSpan(altSerit, 2);

            Controls.Add(tbl);
            FormGezintiYardimcisi.YerlesimiDuzenle(this, pnlUst, pnlSag, tbl);
        }

        private void BtnOtoSimulasyon_Click(object sender, EventArgs e)
        {
            if (aktifAkis == AkisModu.OtoSimulasyon)
            {
                AkisiDurdur();
                return;
            }

            AkisiDurdur();
            ArkaPlanOnbelleginiHazirla();
            kare1?.Dispose();
            kare1 = (Bitmap)arkaPlanOnbellek.Clone();
            ResmiAta(picKare1, (Bitmap)kare1.Clone());
            rozetK1.MetinGuncelle("Sabit Zemin");

            aktifAkis = AkisModu.OtoSimulasyon;
            simAdim = 0;
            btnOtoSimulasyon.Text = "Simülasyonu Durdur";
            akisZamanlayici.Start();
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

            videoYakala = yeni;
            kare1?.Dispose(); kare1 = null;
            kare2?.Dispose(); kare2 = null;
            aktifAkis = AkisModu.CanliKamera;
            btnKamera.Text = "Kamerayı Durdur";
            akisZamanlayici.Start();
        }

        private void BtnVideoSec_Click(object sender, EventArgs e)
        {
            using OpenFileDialog dlg = new OpenFileDialog
            {
                Title = "Hareket Algılama İçin Video Seç",
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

            videoYakala = yeni;
            kare1?.Dispose(); kare1 = null;
            kare2?.Dispose(); kare2 = null;
            aktifAkis = AkisModu.VideoDosyasi;
            akisZamanlayici.Start();
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
            aktifAkis = AkisModu.Statik;
            if (btnOtoSimulasyon != null) btnOtoSimulasyon.Text = "Otomatik Hedef Hareketini Başlat";
            if (btnKamera != null) btnKamera.Text = "Canlı Kamera Hareket Sensörü";
        }

        private void AkisZamanlayici_Tick(object sender, EventArgs e)
        {
            if (IsDisposed || kareIsleniyor || aktifAkis == AkisModu.Statik) return;

            kareIsleniyor = true;
            try
            {
                if (aktifAkis == AkisModu.OtoSimulasyon)
                {
                    double aci = simAdim * 0.14;
                    simAdim++;
                    int hx = (int)Math.Round(160 + 100 * Math.Cos(aci));
                    int hy = (int)Math.Round(110 + 62 * Math.Sin(aci * 1.3));

                    programatikDegisim = true;
                    try
                    {
                        trkHedefX.Value = Math.Clamp(hx, trkHedefX.Minimum, trkHedefX.Maximum);
                        trkHedefY.Value = Math.Clamp(hy, trkHedefY.Minimum, trkHedefY.Maximum);
                        rozetK2.MetinGuncelle($"({trkHedefX.Value},{trkHedefY.Value})");
                    }
                    finally
                    {
                        programatikDegisim = false;
                    }

                    SimulasyonKaresiCizVeHesapla();
                    return;
                }

                if (videoYakala == null || !videoYakala.IsOpened()) return;

                using OpenCvSharp.Mat mat = new OpenCvSharp.Mat();
                bool ok = videoYakala.Read(mat);
                if ((!ok || mat.Empty()) && aktifAkis == AkisModu.VideoDosyasi)
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
                Bitmap yeniKare = BitmapYardimcisi.BoyutSinirla(ham, 280);

                if (kare1 == null)
                {
                    kare1 = yeniKare;
                    ResmiAta(picKare1, (Bitmap)kare1.Clone());
                    rozetK1.MetinGuncelle("t-1 Kare");
                    return;
                }

                if (kare2 != null)
                {
                    kare1.Dispose();
                    kare1 = kare2;
                    ResmiAta(picKare1, (Bitmap)kare1.Clone());
                }

                kare2 = yeniKare;
                ResmiAta(picKare2, (Bitmap)kare2.Clone());
                rozetK1.MetinGuncelle("t-1 Kare");
                rozetK2.MetinGuncelle("t Kare");
                HedefiBulVeHesapla();
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

        private void ArkaPlanOnbelleginiHazirla()
        {
            if (arkaPlanOnbellek != null) return;

            const int w = 320, h = 220;
            Bitmap zemin = new Bitmap(w, h);
            using Bitmap arkaKaynak = BitmapYardimcisi.OrnekGorselYukle("manzara_arkaplan.jpg");
            using (Graphics g = Graphics.FromImage(zemin))
            {
                if (arkaKaynak != null)
                {
                    g.DrawImage(arkaKaynak, 0, 0, w, h);
                }
                else
                {
                    g.Clear(Color.FromArgb(135, 185, 225));
                    using Brush yer = new SolidBrush(Color.FromArgb(76, 135, 72));
                    g.FillRectangle(yer, 0, 140, w, 80);
                }
            }
            arkaPlanOnbellek = zemin;
        }

        private void OrnekKareleriYukle()
        {
            AkisiDurdur();
            ArkaPlanOnbelleginiHazirla();

            programatikDegisim = true;
            try
            {
                trkHedefX.Value = 235;
                trkHedefY.Value = 65;
                rozetK1.MetinGuncelle("Sabit");
                rozetK2.MetinGuncelle("X: 235");
            }
            finally
            {
                programatikDegisim = false;
            }

            kare1?.Dispose();
            kare1 = (Bitmap)arkaPlanOnbellek.Clone();
            ResmiAta(picKare1, (Bitmap)kare1.Clone());
            SimulasyonKaresiCizVeHesapla();
        }

        private void SimulasyonKaresiCizVeHesapla()
        {
            ArkaPlanOnbelleginiHazirla();
            if (kare1 == null)
            {
                kare1 = (Bitmap)arkaPlanOnbellek.Clone();
                ResmiAta(picKare1, (Bitmap)kare1.Clone());
            }

            Bitmap yeniKare2 = (Bitmap)arkaPlanOnbellek.Clone();
            int hx = trkHedefX.Value;
            int hy = trkHedefY.Value;

            using (Graphics g = Graphics.FromImage(yeniKare2))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using Brush bHedef = new SolidBrush(Color.FromArgb(220, 38, 38));
                using Pen pCember = new Pen(Color.White, 2);
                g.FillEllipse(bHedef, hx - 16, hy - 16, 32, 32);
                g.DrawEllipse(pCember, hx - 16, hy - 16, 32, 32);
            }

            kare2?.Dispose();
            kare2 = yeniKare2;
            ResmiAta(picKare2, (Bitmap)kare2.Clone());
            HedefiBulVeHesapla();
        }

        private void Temizle()
        {
            AkisiDurdur();
            kare1?.Dispose(); kare1 = null;
            kare2?.Dispose(); kare2 = null;

            ResmiAta(picKare1, null);
            ResmiAta(picKare2, null);
            ResmiAta(picFark, null);
            ResmiAta(picNisan, null);

            rozetK1.MetinGuncelle("Boş");
            rozetK2.MetinGuncelle("Boş");
            rozetNisan.MetinGuncelle("Bekliyor");
            lblHesap.Text = "Görseller temizlendi.\nSimülasyon başlatın veya 1. ve 2. kareleri seçin.";
            lblAltAnaliz.Text = "Analiz bekleniyor.";
        }

        private static void ResmiAta(PictureBox pic, Bitmap yeni)
        {
            Image onceki = pic.Image;
            pic.Image = yeni;
            onceki?.Dispose();
        }

        private void HedefiBulVeHesapla()
        {
            if (kare1 == null || kare2 == null) return;

            HareketHedefSonucu sonuc = HareketVeEksenHesapla(
                kare1,
                kare2,
                trkEsik.Value,
                (double)numFovYatay.Value,
                (double)numFovDikey.Value,
                chkGurultuTemizle.Checked);

            ResmiAta(picFark, sonuc.FarkMaskesi);
            ResmiAta(picNisan, sonuc.NisanResmi);

            if (sonuc.HedefBulundu)
            {
                bool hizali = Math.Abs(sonuc.FarkX) <= 2 && Math.Abs(sonuc.FarkY) <= 2;
                rozetNisan.MetinGuncelle(hizali
                    ? "KİLİTLENDİ"
                    : $"{sonuc.YatayDonusAci:+0.0;-0.0;0.0}° / {sonuc.DikeyDonusAci:+0.0;-0.0;0.0}°");

                lblHesap.Text = $"HAREKET VE EKSEN HESABI\n\n" +
                                $"• Kamera Merkezi: ({sonuc.KameraMerkezX}, {sonuc.KameraMerkezY})\n" +
                                $"• Hedef Merkezi : ({sonuc.HedefMerkezX}, {sonuc.HedefMerkezY})\n" +
                                $"• Hareket Alanı : {sonuc.HareketPikselSayisi} px\n\n" +
                                $"DÖNÜŞ MİKTARI ({(hizali ? "KİLİTLENDİ" : "TAKİPTE")})\n" +
                                $"• Yatay (dx) : {Math.Abs(sonuc.FarkX)} px ({sonuc.YatayYon})\n" +
                                $"• Dikey (dy) : {Math.Abs(sonuc.FarkY)} px ({sonuc.DikeyYon})\n" +
                                $"• Yatay Açı  : {sonuc.YatayDonusAci:+0.00;-0.00;0.00}°\n" +
                                $"• Dikey Açı  : {sonuc.DikeyDonusAci:+0.00;-0.00;0.00}°";

                lblAltAnaliz.Text = $"Hedef: ({sonuc.HedefMerkezX}, {sonuc.HedefMerkezY}) • Kamera Ekseni Dönüşü -> Yatay: {sonuc.YatayDonusAci:+0.00;-0.00;0.00}° ({sonuc.YatayYon}) | Dikey: {sonuc.DikeyDonusAci:+0.00;-0.00;0.00}° ({sonuc.DikeyYon})";
            }
            else
            {
                rozetNisan.MetinGuncelle("Hedef Yok");
                lblHesap.Text = "Hareket eden bir hedef tespit edilemedi.\nEşik değerini azaltmayı deneyin.";
                lblAltAnaliz.Text = "Hareket tespit edilemedi.";
            }
        }

        public static HareketHedefSonucu HareketVeEksenHesapla(
            Bitmap kare1,
            Bitmap kare2,
            int esikDegeri,
            double fovYatayDerece,
            double fovDikeyDerece,
            bool gurultuTemizle)
        {
            HareketHedefSonucu sonuc = new HareketHedefSonucu();
            if (kare1 == null || kare2 == null) return sonuc;

            int w = kare1.Width;
            int h = kare1.Height;
            bool ayniBoyut = kare2.Width == w && kare2.Height == h;
            using Bitmap k2Olcekli = ayniBoyut ? null : new Bitmap(kare2, w, h);
            Bitmap k2Kaynak = ayniBoyut ? kare2 : k2Olcekli;

            bool[,] maske = new bool[w, h];
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    Color c1 = kare1.GetPixel(x, y);
                    Color c2 = k2Kaynak.GetPixel(x, y);
                    int fark = (Math.Abs(c1.R - c2.R) + Math.Abs(c1.G - c2.G) + Math.Abs(c1.B - c2.B)) / 3;
                    maske[x, y] = fark >= esikDegeri;
                }
            }

            if (gurultuTemizle)
                maske = BitmapYardimcisi.MaskeAcmaKapama(maske, w, h, acmaTekrar: 1, kapamaTekrar: 1);

            Bitmap farkMaske = new Bitmap(w, h);
            long hamTopX = 0, hamTopY = 0;
            int hamAdet = 0;
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    if (maske[x, y])
                    {
                        farkMaske.SetPixel(x, y, Color.White);
                        hamTopX += x;
                        hamTopY += y;
                        hamAdet++;
                    }
                    else
                    {
                        farkMaske.SetPixel(x, y, Color.Black);
                    }
                }
            }

            // Kırmızı (+) çizimlerinden ÖNCE en büyük hareket bölgesini tespit et
            using Bitmap renklendirilmis = BitmapYardimcisi.BolgeBulVeRenklendir(
                farkMaske, 20, out List<BitmapYardimcisi.BolgeBilgisi> bolgeler, out _);

            BitmapYardimcisi.BolgeBilgisi anaBolge = null;
            foreach (var b in bolgeler)
            {
                if (anaBolge == null || b.PikselSayisi > anaBolge.PikselSayisi)
                    anaBolge = b;
            }

            int camMerkezX = w / 2;
            int camMerkezY = h / 2;
            sonuc.KameraMerkezX = camMerkezX;
            sonuc.KameraMerkezY = camMerkezY;

            Bitmap nisanBmp = new Bitmap(k2Kaynak);

            if (anaBolge != null || hamAdet > 5)
            {
                int hedefX = anaBolge != null ? (int)Math.Round(anaBolge.MerkezX) : (int)(hamTopX / hamAdet);
                int hedefY = anaBolge != null ? (int)Math.Round(anaBolge.MerkezY) : (int)(hamTopY / hamAdet);
                int hedefAlan = anaBolge != null ? anaBolge.PikselSayisi : hamAdet;

                int dx = hedefX - camMerkezX;
                int dy = hedefY - camMerkezY;

                double fX = (w / 2.0) / Math.Tan(Math.Clamp(fovYatayDerece, 5.0, 170.0) * Math.PI / 360.0);
                double fY = (h / 2.0) / Math.Tan(Math.Clamp(fovDikeyDerece, 5.0, 170.0) * Math.PI / 360.0);
                double aciX = Math.Atan2(dx, fX) * (180.0 / Math.PI);
                double aciY = Math.Atan2(-dy, fY) * (180.0 / Math.PI);

                sonuc.HedefBulundu = true;
                sonuc.HedefMerkezX = hedefX;
                sonuc.HedefMerkezY = hedefY;
                sonuc.FarkX = dx;
                sonuc.FarkY = dy;
                sonuc.YatayDonusAci = aciX;
                sonuc.DikeyDonusAci = aciY;
                sonuc.HareketPikselSayisi = hedefAlan;
                sonuc.YatayYon = Math.Abs(dx) <= 2 ? "Hizalı" : (dx > 0 ? "Sağa" : "Sola");
                sonuc.DikeyYon = Math.Abs(dy) <= 2 ? "Hizalı" : (dy < 0 ? "Yukarı" : "Aşağı");

                // Slayt diyagramına uygun olarak fark maskesine kamera ekseni (büyük +) ve hedef merkezi (küçük +) çiz
                using (Graphics gf = Graphics.FromImage(farkMaske))
                using (Pen kirmiziCam = new Pen(Color.Red, 2.5f))
                using (Pen kirmiziHedef = new Pen(Color.Red, 2.0f))
                {
                    gf.DrawLine(kirmiziCam, camMerkezX - 24, camMerkezY, camMerkezX + 24, camMerkezY);
                    gf.DrawLine(kirmiziCam, camMerkezX, camMerkezY - 24, camMerkezX, camMerkezY + 24);
                    gf.DrawLine(kirmiziHedef, hedefX - 12, hedefY, hedefX + 12, hedefY);
                    gf.DrawLine(kirmiziHedef, hedefX, hedefY - 12, hedefX, hedefY + 12);
                }

                using (Graphics gn = Graphics.FromImage(nisanBmp))
                {
                    gn.SmoothingMode = SmoothingMode.AntiAlias;
                    using Pen camPen = new Pen(Color.DeepSkyBlue, 2);
                    gn.DrawLine(camPen, camMerkezX - 14, camMerkezY, camMerkezX + 14, camMerkezY);
                    gn.DrawLine(camPen, camMerkezX, camMerkezY - 14, camMerkezX, camMerkezY + 14);

                    using Pen okPen = new Pen(Color.Yellow, 2);
                    okPen.CustomEndCap = new AdjustableArrowCap(4, 5);
                    gn.DrawLine(okPen, camMerkezX, camMerkezY, hedefX, hedefY);

                    using Pen hedefPen = new Pen(Color.Lime, 2.2f);
                    if (anaBolge != null)
                        gn.DrawRectangle(hedefPen, anaBolge.MinX, anaBolge.MinY, anaBolge.Genislik, anaBolge.Yukseklik);
                    gn.DrawLine(hedefPen, hedefX - 12, hedefY, hedefX + 12, hedefY);
                    gn.DrawLine(hedefPen, hedefX, hedefY - 12, hedefX, hedefY + 12);
                    gn.DrawEllipse(hedefPen, hedefX - 18, hedefY - 18, 36, 36);

                    string etiket = $"Pan: {aciX:+0.0;-0.0;0.0}° | Tilt: {aciY:+0.0;-0.0;0.0}°";
                    using Font f = new Font("Segoe UI", 8.0f, FontStyle.Bold);
                    using Brush bg = new SolidBrush(Color.FromArgb(185, 15, 23, 42));
                    SizeF sz = gn.MeasureString(etiket, f);
                    gn.FillRectangle(bg, 6, 6, sz.Width + 6, 17);
                    gn.DrawString(etiket, f, Brushes.Gold, 9, 7);
                }
            }
            else
            {
                using Graphics gf = Graphics.FromImage(farkMaske);
                using Graphics gn = Graphics.FromImage(nisanBmp);
                using Pen kirmiziCam = new Pen(Color.Red, 2.5f);
                using Pen maviCam = new Pen(Color.DeepSkyBlue, 2);
                gf.DrawLine(kirmiziCam, camMerkezX - 24, camMerkezY, camMerkezX + 24, camMerkezY);
                gf.DrawLine(kirmiziCam, camMerkezX, camMerkezY - 24, camMerkezX, camMerkezY + 24);
                gn.DrawLine(maviCam, camMerkezX - 14, camMerkezY, camMerkezX + 14, camMerkezY);
                gn.DrawLine(maviCam, camMerkezX, camMerkezY - 14, camMerkezX, camMerkezY + 14);
            }

            sonuc.FarkMaskesi = farkMaske;
            sonuc.NisanResmi = nisanBmp;
            return sonuc;
        }
    }
}
