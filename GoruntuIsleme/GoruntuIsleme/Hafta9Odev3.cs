using System;
using System.Drawing;
using System.Windows.Forms;

namespace GoruntuIsleme
{
    public class ToplamaAnalizSonucu
    {
        public Bitmap ManuelSonuc { get; set; }
        public Bitmap OtomatikSonuc { get; set; }
        public Bitmap NormalizeSonuc { get; set; }
        public double Ort1 { get; set; }
        public double Ort2 { get; set; }
        public double OtoK1 { get; set; }
        public double OtoK2 { get; set; }
        public int MaksHamToplam { get; set; }
        public double ManuelTasmaYuzdesi { get; set; }
        public double ManuelBeyazlamaYuzdesi { get; set; }
        public double OtoTasmaYuzdesi { get; set; }
        public double OtoBeyazlamaYuzdesi { get; set; }
    }

    public class Hafta9Odev3 : Form
    {
        private PictureBox picResim1, picResim2, picManuel, picOtomatik, picNormalize;
        private ModernSurgu trkOlcek1, trkOlcek2;
        private ModernHapRozet rozetR1, rozetR2, rozetManuel, rozetOto, rozetNorm;
        private Label lblOlcek1, lblOlcek2, lblYorum;

        private Bitmap resim1, resim2;
        private Color[,] c1Onbellek, c2Onbellek;
        private int onbellekW, onbellekH;
        private double sonOrt1, sonOrt2, sonOtoK1, sonOtoK2, sonOtoBeyaz;
        private int sonMaksHam;
        private bool programatikDegisim = false;

        public Hafta9Odev3()
        {
            Text = "Hafta 9 - Ödev 3 | İki Resmi Ölçekli Toplama ve Normalizasyon";
            FormGezintiYardimcisi.FormuHazirla(this);
            ArayuzuOlustur();
            OrnekResimleriYukle();

            this.FormClosing += (s, e) =>
            {
                resim1?.Dispose();
                resim1 = null;
                resim2?.Dispose();
                resim2 = null;
            };
        }

        private void ArayuzuOlustur()
        {
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                this,
                "Hafta 9 - Ödev 3 | İki Resmi Ölçekli Toplama (Manuel Sürgü, Otomatik Oran & Normalizasyon)",
                "< Ödev 2", () => FormGezintiYardimcisi.FormGecisYap(this, new Hafta9Odev2()),
                "Ödev 4 >", () => FormGezintiYardimcisi.FormGecisYap(this, new Hafta9Odev4()),
                false,
                "HAFTA 9 • ÖDEV 3");

            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 305);
            pnlSag.AutoScroll = true;

            int x = 16;
            int w = 262;
            int y = 12;

            Label lblKaynak = new Label
            {
                Text = "GÖRÜNTÜ KAYNAKLARI",
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi
            };
            y += 20;

            ModernHapButon btnSec1 = FormGezintiYardimcisi.ModernButonOlustur("1. Resim Seç (Koyu)", true, w, 34);
            btnSec1.Location = new Point(x, y);
            y += 40;

            ModernHapButon btnSec2 = FormGezintiYardimcisi.ModernButonOlustur("2. Resim Seç (Açık)", true, w, 34);
            btnSec2.Location = new Point(x, y);
            y += 40;

            ModernHapButon btnOrnek = FormGezintiYardimcisi.ModernButonOlustur("Örnek Görselleri Yükle", false, w, 32);
            btnOrnek.Location = new Point(x, y);
            y += 38;

            ModernHapButon btnTemizle = FormGezintiYardimcisi.ModernButonOlustur("Temizle", false, w, 30);
            btnTemizle.Location = new Point(x, y);
            y += 38;

            btnSec1.Click += (s, e) =>
            {
                string yol = FormGezintiYardimcisi.ResimDosyasiSec("1. Resmi Seç (Koyu)");
                if (yol == null) return;
                resim1?.Dispose();
                using Bitmap ham = new Bitmap(yol);
                resim1 = BitmapYardimcisi.BoyutSinirla(ham, 340);
                ResmiAta(picResim1, (Bitmap)resim1.Clone());
                TumunuYenidenHesapla();
            };
            btnSec2.Click += (s, e) =>
            {
                string yol = FormGezintiYardimcisi.ResimDosyasiSec("2. Resmi Seç (Açık)");
                if (yol == null) return;
                resim2?.Dispose();
                using Bitmap ham = new Bitmap(yol);
                resim2 = BitmapYardimcisi.BoyutSinirla(ham, 340);
                ResmiAta(picResim2, (Bitmap)resim2.Clone());
                TumunuYenidenHesapla();
            };
            btnOrnek.Click += (s, e) => OrnekResimleriYukle();
            btnTemizle.Click += (s, e) => Temizle();

            ModernKartPanel kartOlcek = new ModernKartPanel
            {
                Location = new Point(x, y),
                Size = new Size(w, 172),
                KartArkaPlan = Color.White,
                KenarlikRengi = FormGezintiYardimcisi.KartKenar,
                KoseYaricapi = 12
            };

            Label lblOlcekBaslik = new Label
            {
                Text = "MANUEL ÖLÇEK SÜRGÜLERİ (a)",
                Location = new Point(12, 10),
                Size = new Size(238, 17),
                Font = new Font("Segoe UI Semibold", 8.3f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi
            };

            lblOlcek1 = new Label
            {
                Text = "1. Resim Çarpanı (Koyu): 1,60×",
                Location = new Point(12, 32),
                Size = new Size(238, 16),
                Font = new Font("Segoe UI", 8.3f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi
            };
            trkOlcek1 = new ModernSurgu
            {
                Location = new Point(12, 50),
                Width = 238,
                Minimum = 0,
                Maximum = 300,
                Value = 160
            };

            lblOlcek2 = new Label
            {
                Text = "2. Resim Çarpanı (Açık): 0,55×",
                Location = new Point(12, 80),
                Size = new Size(238, 16),
                Font = new Font("Segoe UI", 8.3f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi
            };
            trkOlcek2 = new ModernSurgu
            {
                Location = new Point(12, 98),
                Width = 238,
                Minimum = 0,
                Maximum = 300,
                Value = 55
            };

            trkOlcek1.Scroll += (s, e) => { if (!programatikDegisim) ManuelGuncelle(); };
            trkOlcek2.Scroll += (s, e) => { if (!programatikDegisim) ManuelGuncelle(); };

            ModernHapButon btnOtoAktar = FormGezintiYardimcisi.ModernButonOlustur("Otomatik Dengeli Ölçekleri Uygula", false, 238, 30);
            btnOtoAktar.Location = new Point(12, 130);
            btnOtoAktar.Click += (s, e) =>
            {
                if (sonOtoK1 <= 0 && sonOtoK2 <= 0) return;
                programatikDegisim = true;
                try
                {
                    trkOlcek1.Value = Math.Clamp((int)Math.Round(sonOtoK1 * 100.0), trkOlcek1.Minimum, trkOlcek1.Maximum);
                    trkOlcek2.Value = Math.Clamp((int)Math.Round(sonOtoK2 * 100.0), trkOlcek2.Minimum, trkOlcek2.Maximum);
                }
                finally
                {
                    programatikDegisim = false;
                }
                ManuelGuncelle();
            };

            kartOlcek.Controls.AddRange(new Control[]
            {
                lblOlcekBaslik, lblOlcek1, trkOlcek1, lblOlcek2, trkOlcek2, btnOtoAktar
            });
            y += 182;

            ModernKartPanel kartYorum = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                new Point(x, y), new Size(w, 240), out lblYorum);
            kartYorum.Margin = new Padding(0, 0, 0, 14);
            lblYorum.Font = new Font("Segoe UI", 8.4f);

            pnlSag.Controls.AddRange(new Control[]
            {
                lblKaynak, btnSec1, btnSec2, btnOrnek, btnTemizle,
                kartOlcek, kartYorum
            });

            rozetR1 = new ModernHapRozet();
            rozetR1.MetinGuncelle("1,60×");

            rozetR2 = new ModernHapRozet();
            rozetR2.MetinGuncelle("0,55×");

            rozetManuel = new ModernHapRozet();
            rozetManuel.MetinGuncelle("Beyazlama: %0,0");

            rozetOto = new ModernHapRozet();
            rozetOto.MetinGuncelle("Dengeli");

            rozetNorm = new ModernHapRozet();
            rozetNorm.MetinGuncelle("Normalize");

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
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));

            picResim1 = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 0, 0, "1. Giriş (Koyu Görüntü)", null, null, rozetR1);
            picResim2 = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 1, 0, "2. Giriş (Açık Görüntü)", null, null, rozetR2);
            picManuel = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 2, 0, "a) Manuel Ölçekli Toplama", null, null, rozetManuel);
            picOtomatik = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 0, 1, "b) Otomatik Dengeli Toplama", null, null, rozetOto);
            picNormalize = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 1, 1, "c) Doğrudan Toplama + Normalizasyon", null, null, rozetNorm);

            ModernKartPanel kartKarsilastirma = new ModernKartPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(8),
                Padding = new Padding(16),
                KartArkaPlan = FormGezintiYardimcisi.AcikMaviZemin,
                KenarlikRengi = FormGezintiYardimcisi.AcikMaviKenar
            };
            Label lblKarsilastirma = new Label
            {
                Dock = DockStyle.Fill,
                BackColor = FormGezintiYardimcisi.AcikMaviZemin,
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi,
                Font = new Font("Segoe UI", 9.0f),
                UseMnemonic = false,
                Text = "Yöntemlerin Karşılaştırması\n\n" +
                       "• Manuel Ölçekleme (a): Sağdaki sürgülerle koyu resim daha büyük, açık resim daha küçük katsayıyla çarpılıp beyazlama olmadan eşit görünürlük sağlanır.\n\n" +
                       "• Otomatik Dengeli Oran (b): Ortalama parlaklığa göre iki görüntüyü ~64'er katkıya ölçekleyip P98 tavanıyla taşmayı önler.\n\n" +
                       "• Normalizasyon (c): 255 üzerini aşağı ölçekler ancak koyu görüntü zayıf kalır."
            };
            kartKarsilastirma.Controls.Add(lblKarsilastirma);
            tbl.Controls.Add(kartKarsilastirma, 2, 1);

            Controls.Add(tbl);
            FormGezintiYardimcisi.YerlesimiDuzenle(this, pnlUst, pnlSag, tbl);
        }

        private void OrnekResimleriYukle()
        {
            Bitmap r1 = BitmapYardimcisi.OrnekGorselYukle("toplama_koyu.jpg");
            Bitmap r2 = BitmapYardimcisi.OrnekGorselYukle("toplama_acik.jpg");
            if (r1 == null || r2 == null)
            {
                r1?.Dispose();
                r2?.Dispose();
                r1 = SentetikGorselOlustur(koyuMu: true);
                r2 = SentetikGorselOlustur(koyuMu: false);
            }

            programatikDegisim = true;
            try
            {
                trkOlcek1.Value = 160;
                trkOlcek2.Value = 55;
            }
            finally
            {
                programatikDegisim = false;
            }

            resim1?.Dispose();
            resim1 = r1;
            resim2?.Dispose();
            resim2 = r2;
            ResmiAta(picResim1, (Bitmap)resim1.Clone());
            ResmiAta(picResim2, (Bitmap)resim2.Clone());
            TumunuYenidenHesapla();
        }

        private static Bitmap SentetikGorselOlustur(bool koyuMu)
        {
            Bitmap bmp = new Bitmap(320, 240);
            using Graphics g = Graphics.FromImage(bmp);
            if (koyuMu)
            {
                g.Clear(Color.FromArgb(8, 12, 20));
                using Brush ev = new SolidBrush(Color.FromArgb(52, 22, 22));
                g.FillRectangle(ev, 100, 90, 120, 90);
            }
            else
            {
                g.Clear(Color.FromArgb(135, 185, 230));
                using Brush tepe = new SolidBrush(Color.FromArgb(65, 130, 85));
                g.FillRectangle(tepe, 0, 145, 320, 95);
            }
            return bmp;
        }

        private void Temizle()
        {
            resim1?.Dispose();
            resim1 = null;
            resim2?.Dispose();
            resim2 = null;
            c1Onbellek = null;
            c2Onbellek = null;

            ResmiAta(picResim1, null);
            ResmiAta(picResim2, null);
            ResmiAta(picManuel, null);
            ResmiAta(picOtomatik, null);
            ResmiAta(picNormalize, null);

            rozetManuel.MetinGuncelle("Boş");
            rozetOto.MetinGuncelle("Boş");
            lblYorum.Text = "Görseller temizlendi. Sağdan 1. ve 2. resmi seçin veya örnek görselleri yükleyin.";
        }

        private static void ResmiAta(PictureBox pic, Bitmap yeni)
        {
            Image onceki = pic.Image;
            pic.Image = yeni;
            onceki?.Dispose();
        }

        private void TumunuYenidenHesapla()
        {
            if (resim1 == null || resim2 == null) return;

            ColorDizilerineOku(resim1, resim2, out onbellekW, out onbellekH, out c1Onbellek, out c2Onbellek);

            double k1 = trkOlcek1.Value / 100.0;
            double k2 = trkOlcek2.Value / 100.0;

            ToplamaAnalizSonucu sonuc = TumYontemleriHesapla(resim1, resim2, k1, k2);
            sonOrt1 = sonuc.Ort1;
            sonOrt2 = sonuc.Ort2;
            sonOtoK1 = sonuc.OtoK1;
            sonOtoK2 = sonuc.OtoK2;
            sonOtoBeyaz = sonuc.OtoBeyazlamaYuzdesi;
            sonMaksHam = sonuc.MaksHamToplam;

            ResmiAta(picManuel, sonuc.ManuelSonuc);
            ResmiAta(picOtomatik, sonuc.OtomatikSonuc);
            ResmiAta(picNormalize, sonuc.NormalizeSonuc);

            rozetOto.MetinGuncelle($"{sonOtoK1:0.00}× / {sonOtoK2:0.00}×");
            ManuelEtiketleriniGuncelle(k1, k2, sonuc.ManuelTasmaYuzdesi, sonuc.ManuelBeyazlamaYuzdesi);
        }

        private void ManuelGuncelle()
        {
            if (c1Onbellek == null || c2Onbellek == null) return;

            double k1 = trkOlcek1.Value / 100.0;
            double k2 = trkOlcek2.Value / 100.0;

            Bitmap manuelBmp = OlcekliTopla(c1Onbellek, c2Onbellek, onbellekW, onbellekH, k1, k2, out double tasma, out double beyaz);
            ResmiAta(picManuel, manuelBmp);
            ManuelEtiketleriniGuncelle(k1, k2, tasma, beyaz);
        }

        private void ManuelEtiketleriniGuncelle(double k1, double k2, double tasma, double beyaz)
        {
            lblOlcek1.Text = $"1. Resim Çarpanı (Koyu): {k1:0.00}×";
            lblOlcek2.Text = $"2. Resim Çarpanı (Açık): {k2:0.00}×";
            rozetR1.MetinGuncelle($"{k1:0.00}×");
            rozetR2.MetinGuncelle($"{k2:0.00}×");
            rozetManuel.MetinGuncelle($"Beyazlama: %{beyaz:0.0}");

            double katki1 = k1 * sonOrt1;
            double katki2 = k2 * sonOrt2;
            lblYorum.Text = $"ÖLÇEK VE BEYAZLAMA ANALİZİ\n\n" +
                            $"• 1. Ort / 2. Ort       : {sonOrt1:0.0} / {sonOrt2:0.0}\n" +
                            $"• Manuel Katkı (1 vs 2) : {katki1:0.0} vs {katki2:0.0}\n" +
                            $"• Manuel Taşma / Beyaz  : %{tasma:0.1} / %{beyaz:0.1}\n\n" +
                            $"• Otomatik Ölçekler (b) : {sonOtoK1:0.00}× ve {sonOtoK2:0.00}×\n" +
                            $"• Otomatik Beyazlama    : %{sonOtoBeyaz:0.1}\n" +
                            $"• Ham Toplam Tepe Değer : {sonMaksHam}";
        }

        public static Bitmap ManuelOlcekliTopla(
            Bitmap resim1,
            Bitmap resim2,
            double k1,
            double k2,
            out double tasmaYuzdesi,
            out double beyazlamaYuzdesi)
        {
            tasmaYuzdesi = 0;
            beyazlamaYuzdesi = 0;
            if (resim1 == null || resim2 == null) return null;

            ColorDizilerineOku(resim1, resim2, out int w, out int h, out Color[,] c1Dizi, out Color[,] c2Dizi);
            return OlcekliTopla(c1Dizi, c2Dizi, w, h, k1, k2, out tasmaYuzdesi, out beyazlamaYuzdesi);
        }

        public static ToplamaAnalizSonucu TumYontemleriHesapla(Bitmap resim1, Bitmap resim2, double manuelK1, double manuelK2)
        {
            ToplamaAnalizSonucu sonuc = new ToplamaAnalizSonucu();
            if (resim1 == null || resim2 == null) return sonuc;

            ColorDizilerineOku(resim1, resim2, out int w, out int h, out Color[,] c1Dizi, out Color[,] c2Dizi);
            int pikselSayisi = Math.Max(1, w * h);

            double top1 = 0, top2 = 0;
            int[,] hamR = new int[w, h];
            int[,] hamG = new int[w, h];
            int[,] hamB = new int[w, h];
            int maxHam = 1;

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    Color c1 = c1Dizi[x, y];
                    Color c2 = c2Dizi[x, y];
                    top1 += (c1.R + c1.G + c1.B) / 3.0;
                    top2 += (c2.R + c2.G + c2.B) / 3.0;

                    int hr = c1.R + c2.R;
                    int hg = c1.G + c2.G;
                    int hb = c1.B + c2.B;
                    hamR[x, y] = hr;
                    hamG[x, y] = hg;
                    hamB[x, y] = hb;

                    if (hr > maxHam) maxHam = hr;
                    if (hg > maxHam) maxHam = hg;
                    if (hb > maxHam) maxHam = hb;
                }
            }

            double ort1 = Math.Max(1.0, top1 / pikselSayisi);
            double ort2 = Math.Max(1.0, top2 / pikselSayisi);

            // Dengeli katkı: her iki görüntü de yaklaşık 64 orta parlaklık katkısı verir (~128 ortalama), P98 taşma korumasıyla beyazlama önlenir
            double rawK1 = 64.0 / ort1;
            double rawK2 = 64.0 / ort2;

            int[] tepeHist = new int[1024];
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    Color c1 = c1Dizi[x, y];
                    Color c2 = c2Dizi[x, y];
                    double sr = c1.R * rawK1 + c2.R * rawK2;
                    double sg = c1.G * rawK1 + c2.G * rawK2;
                    double sb = c1.B * rawK1 + c2.B * rawK2;
                    int tepe = Math.Clamp((int)Math.Ceiling(Math.Max(sr, Math.Max(sg, sb))), 0, 1023);
                    tepeHist[tepe]++;
                }
            }

            int hedefSayi = (int)Math.Ceiling(pikselSayisi * 0.98);
            int birikimli = 0;
            int p98 = 255;
            for (int i = 0; i < tepeHist.Length; i++)
            {
                birikimli += tepeHist[i];
                if (birikimli >= hedefSayi)
                {
                    p98 = i;
                    break;
                }
            }

            double sinirlama = p98 > 255 ? (255.0 / p98) : 1.0;
            double otoK1 = rawK1 * sinirlama;
            double otoK2 = rawK2 * sinirlama;

            sonuc.Ort1 = ort1;
            sonuc.Ort2 = ort2;
            sonuc.OtoK1 = otoK1;
            sonuc.OtoK2 = otoK2;
            sonuc.MaksHamToplam = maxHam;

            sonuc.ManuelSonuc = OlcekliTopla(c1Dizi, c2Dizi, w, h, manuelK1, manuelK2, out double manTasma, out double manBeyaz);
            sonuc.ManuelTasmaYuzdesi = manTasma;
            sonuc.ManuelBeyazlamaYuzdesi = manBeyaz;

            sonuc.OtomatikSonuc = OlcekliTopla(c1Dizi, c2Dizi, w, h, otoK1, otoK2, out double otoTasma, out double otoBeyaz);
            sonuc.OtoTasmaYuzdesi = otoTasma;
            sonuc.OtoBeyazlamaYuzdesi = otoBeyaz;

            Bitmap normBmp = new Bitmap(w, h);
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int nr = Math.Clamp((int)Math.Round(hamR[x, y] * 255.0 / maxHam), 0, 255);
                    int ng = Math.Clamp((int)Math.Round(hamG[x, y] * 255.0 / maxHam), 0, 255);
                    int nb = Math.Clamp((int)Math.Round(hamB[x, y] * 255.0 / maxHam), 0, 255);
                    normBmp.SetPixel(x, y, Color.FromArgb(nr, ng, nb));
                }
            }
            sonuc.NormalizeSonuc = normBmp;
            return sonuc;
        }

        private static void ColorDizilerineOku(Bitmap resim1, Bitmap resim2, out int w, out int h, out Color[,] c1Dizi, out Color[,] c2Dizi)
        {
            w = resim1.Width;
            h = resim1.Height;
            c1Dizi = new Color[w, h];
            c2Dizi = new Color[w, h];

            bool ayniBoyut = resim2.Width == w && resim2.Height == h;
            using Bitmap r2Olcekli = ayniBoyut ? null : new Bitmap(resim2, w, h);
            Bitmap kaynak2 = ayniBoyut ? resim2 : r2Olcekli;

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    c1Dizi[x, y] = resim1.GetPixel(x, y);
                    c2Dizi[x, y] = kaynak2.GetPixel(x, y);
                }
            }
        }

        private static Bitmap OlcekliTopla(
            Color[,] c1Dizi,
            Color[,] c2Dizi,
            int w,
            int h,
            double k1,
            double k2,
            out double tasmaYuzdesi,
            out double beyazlamaYuzdesi)
        {
            Bitmap sonuc = new Bitmap(w, h);
            int tasmaSay = 0;
            int beyazSay = 0;
            int pikselSayisi = Math.Max(1, w * h);

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    Color c1 = c1Dizi[x, y];
                    Color c2 = c2Dizi[x, y];

                    double sr = c1.R * k1 + c2.R * k2;
                    double sg = c1.G * k1 + c2.G * k2;
                    double sb = c1.B * k1 + c2.B * k2;

                    if (sr > 255.0 || sg > 255.0 || sb > 255.0)
                        tasmaSay++;

                    int r = Math.Clamp((int)Math.Round(sr), 0, 255);
                    int g = Math.Clamp((int)Math.Round(sg), 0, 255);
                    int b = Math.Clamp((int)Math.Round(sb), 0, 255);

                    if (r >= 250 && g >= 250 && b >= 250)
                        beyazSay++;

                    sonuc.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }

            tasmaYuzdesi = 100.0 * tasmaSay / pikselSayisi;
            beyazlamaYuzdesi = 100.0 * beyazSay / pikselSayisi;
            return sonuc;
        }
    }
}
