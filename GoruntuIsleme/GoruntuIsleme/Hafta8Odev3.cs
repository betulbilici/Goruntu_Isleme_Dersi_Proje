using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GoruntuIsleme
{
    public enum MorfolojiIslemTuru
    {
        BeyaziGenislet,
        BeyaziDaralt,
        Acma,
        Kapama,
        AcmaVeKapama
    }

    public class Hafta8Odev3 : Form
    {
        private PictureBox picGiris;
        private PictureBox picKareSonuc;
        private PictureBox picArtiSonuc;

        private ComboBox cmbSablonSecimi;
        private NumericUpDown numTekrar;

        private RadioButton rbGenislet;
        private RadioButton rbAsindir;
        private RadioButton rbAcma;
        private RadioButton rbKapama;
        private RadioButton rbAcmaKapama;

        private NumericUpDown numMinR, numMaxR;
        private NumericUpDown numMinG, numMaxG;
        private NumericUpDown numMinB, numMaxB;
        private CheckBox chkRenkAraligiKullan;

        private ModernHapRozet rozetGiris, rozetKare, rozetArti;
        private Label lblYorum;

        private Bitmap orijinalResim;
        private Bitmap temizReferans;
        private bool programatikDegisim = false;

        public Hafta8Odev3()
        {
            ArayuzuOlustur();
            OrnekTuzBiberYukle();
        }

        private void ArayuzuOlustur()
        {
            FormGezintiYardimcisi.FormuHazirla(this);
            this.Text = "Hafta 8 - Ödev 3: Morfolojik Gürültü Temizleme ve Kenar Kapatma (9'luk Kare & 5'lik Artı)";

            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                this,
                "Hafta 8 - Ödev 3: 9'luk Kare & 5'lik Artı Şablonlu Morfolojik İşlemler",
                "< Ödev 2", () => FormGezintiYardimcisi.FormGecisYap(this, new Hafta8Odev2()),
                "Ödev 4 >", () => FormGezintiYardimcisi.FormGecisYap(this, new Hafta8Odev4()),
                false,
                "HAFTA 8 • ÖDEV 3"
            );

            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 305);
            pnlSag.AutoScroll = true;

            int genislik = 262;
            int x = 16;
            int y = 14;

            ModernHapButon btnResimSec = FormGezintiYardimcisi.ModernButonOlustur("Bilgisayardan Resim Yükle", false, genislik, 36);
            btnResimSec.Location = new Point(x, y);
            btnResimSec.Click += BtnResimSec_Click;
            y += 42;

            ModernHapButon btnTuzBiber = FormGezintiYardimcisi.ModernButonOlustur("Örnek 1: Tuz-Biber Gürültüsü (3a)", false, genislik, 34);
            btnTuzBiber.Location = new Point(x, y);
            btnTuzBiber.Click += (s, e) => OrnekTuzBiberYukle();
            y += 40;

            ModernHapButon btnKopukKenar = FormGezintiYardimcisi.ModernButonOlustur("Örnek 2: Kopuk Kenarlar (3b)", false, genislik, 34);
            btnKopukKenar.Location = new Point(x, y);
            btnKopukKenar.Click += (s, e) => OrnekKopukKenarYukle();
            y += 40;

            ModernHapButon btnRgbOrnek = FormGezintiYardimcisi.ModernButonOlustur("Örnek 3: Renkli RGB Aralığı (3b)", false, genislik, 34);
            btnRgbOrnek.Location = new Point(x, y);
            btnRgbOrnek.Click += (s, e) => OrnekRgbAraligiYukle();
            y += 40;

            ModernHapButon btnTemizle = FormGezintiYardimcisi.ModernButonOlustur("Temizle", false, genislik, 32);
            btnTemizle.Location = new Point(x, y);
            btnTemizle.Click += (s, e) => Temizle();
            y += 40;

            ModernKartPanel kartIslem = new ModernKartPanel
            {
                Location = new Point(x, y),
                Size = new Size(genislik, 246),
                KartArkaPlan = Color.White,
                KenarlikRengi = FormGezintiYardimcisi.KartKenar,
                KoseYaricapi = 12
            };

            Label lblSablonSec = new Label
            {
                Text = "Şablon Seçimi:",
                Location = new Point(12, 10),
                Size = new Size(238, 18),
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi
            };

            cmbSablonSecimi = new ComboBox
            {
                Location = new Point(12, 30),
                Size = new Size(238, 24),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 8.5F)
            };
            cmbSablonSecimi.Items.Add("Karşılaştır (9'luk Kare & 5'lik Artı)");
            cmbSablonSecimi.Items.Add("Yalnızca 9'luk Kare (3×3)");
            cmbSablonSecimi.Items.Add("Yalnızca 5'lik Artı (+)");
            cmbSablonSecimi.SelectedIndex = 0;
            cmbSablonSecimi.SelectedIndexChanged += AyarDegisti_Olayi;

            Label lblTekrar = new Label
            {
                Text = "Tekrar Sayısı (Adım):",
                Location = new Point(12, 62),
                Size = new Size(155, 18),
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi
            };

            numTekrar = new NumericUpDown
            {
                Location = new Point(182, 59),
                Size = new Size(68, 23),
                Minimum = 1,
                Maximum = 4,
                Value = 1,
                Font = new Font("Segoe UI", 8.5F)
            };
            numTekrar.ValueChanged += AyarDegisti_Olayi;

            Label lblIslemBaslik = new Label
            {
                Text = "Morfolojik İşlem Seçimi:",
                Location = new Point(12, 90),
                Size = new Size(238, 18),
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi
            };

            rbGenislet = new RadioButton { Text = "Genişletme (Dilation)", Location = new Point(14, 112), Size = new Size(236, 22), Font = new Font("Segoe UI", 8.5F) };
            rbAsindir = new RadioButton { Text = "Daraltma / Aşındırma (Erosion)", Location = new Point(14, 136), Size = new Size(236, 22), Font = new Font("Segoe UI", 8.5F) };
            rbAcma = new RadioButton { Text = "Açma (Opening)", Location = new Point(14, 160), Size = new Size(236, 22), Font = new Font("Segoe UI", 8.5F) };
            rbKapama = new RadioButton { Text = "Kapama (Closing)", Location = new Point(14, 184), Size = new Size(236, 22), Font = new Font("Segoe UI", 8.5F) };
            rbAcmaKapama = new RadioButton { Text = "Açma + Kapama (Tuz-Biber Temizle)", Location = new Point(14, 208), Size = new Size(236, 24), Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), ForeColor = FormGezintiYardimcisi.BirincilMavi, Checked = true };

            rbGenislet.CheckedChanged += AyarDegisti_Olayi;
            rbAsindir.CheckedChanged += AyarDegisti_Olayi;
            rbAcma.CheckedChanged += AyarDegisti_Olayi;
            rbKapama.CheckedChanged += AyarDegisti_Olayi;
            rbAcmaKapama.CheckedChanged += AyarDegisti_Olayi;

            kartIslem.Controls.AddRange(new Control[]
            {
                lblSablonSec, cmbSablonSecimi,
                lblTekrar, numTekrar,
                lblIslemBaslik, rbGenislet, rbAsindir, rbAcma, rbKapama, rbAcmaKapama
            });
            y += 256;

            ModernKartPanel kartRgb = new ModernKartPanel
            {
                Location = new Point(x, y),
                Size = new Size(genislik, 148),
                KartArkaPlan = Color.White,
                KenarlikRengi = FormGezintiYardimcisi.KartKenar,
                KoseYaricapi = 12
            };

            chkRenkAraligiKullan = new CheckBox
            {
                Text = "Özel RGB Kanal Aralığı Kullan (3b)",
                Location = new Point(12, 10),
                Size = new Size(240, 22),
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi
            };
            chkRenkAraligiKullan.CheckedChanged += AyarDegisti_Olayi;

            numMinR = RgbNumericOlustur(new Point(42, 38), 50);
            numMaxR = RgbNumericOlustur(new Point(132, 38), 120);
            numMinG = RgbNumericOlustur(new Point(42, 68), 0);
            numMaxG = RgbNumericOlustur(new Point(132, 68), 200);
            numMinB = RgbNumericOlustur(new Point(42, 98), 150);
            numMaxB = RgbNumericOlustur(new Point(132, 98), 180);

            kartRgb.Controls.Add(chkRenkAraligiKullan);
            kartRgb.Controls.Add(new Label { Text = "R:", Location = new Point(14, 41), Size = new Size(24, 18), Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            kartRgb.Controls.Add(numMinR);
            kartRgb.Controls.Add(new Label { Text = "..", Location = new Point(112, 41), Size = new Size(16, 18) });
            kartRgb.Controls.Add(numMaxR);

            kartRgb.Controls.Add(new Label { Text = "G:", Location = new Point(14, 71), Size = new Size(24, 18), Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            kartRgb.Controls.Add(numMinG);
            kartRgb.Controls.Add(new Label { Text = "..", Location = new Point(112, 71), Size = new Size(16, 18) });
            kartRgb.Controls.Add(numMaxG);

            kartRgb.Controls.Add(new Label { Text = "B:", Location = new Point(14, 101), Size = new Size(24, 18), Font = new Font("Segoe UI", 8.5F, FontStyle.Bold) });
            kartRgb.Controls.Add(numMinB);
            kartRgb.Controls.Add(new Label { Text = "..", Location = new Point(112, 101), Size = new Size(16, 18) });
            kartRgb.Controls.Add(numMaxB);

            Label lblRgbIpucu = new Label
            {
                Text = "Örn: 50<R<120, 0<G<200, 150<B<180",
                Location = new Point(14, 126),
                Size = new Size(236, 16),
                Font = new Font("Segoe UI", 7.8F),
                ForeColor = FormGezintiYardimcisi.AcikYazi
            };
            kartRgb.Controls.Add(lblRgbIpucu);
            y += 158;

            ModernHapButon btnUygula = FormGezintiYardimcisi.ModernButonOlustur("İşlemi Uygula ve Karşılaştır", true, genislik, 40);
            btnUygula.Location = new Point(x, y);
            btnUygula.Margin = new Padding(0, 0, 0, 16);
            btnUygula.Click += (s, e) => KarsilastirmaliUygula(kullaniciTetikledi: true);

            pnlSag.Controls.AddRange(new Control[]
            {
                btnResimSec, btnTuzBiber, btnKopukKenar, btnRgbOrnek, btnTemizle,
                kartIslem, kartRgb, btnUygula
            });

            Panel pnlOrta = new Panel { Dock = DockStyle.Fill, BackColor = FormGezintiYardimcisi.TemaZemin, Padding = new Padding(12) };

            TableLayoutPanel tablo = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                BackColor = Color.Transparent
            };
            tablo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tablo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            tablo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));
            tablo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            rozetGiris = new ModernHapRozet { Text = "Giriş" };
            rozetKare = new ModernHapRozet { Text = "9'luk Kare (3×3)" };
            rozetArti = new ModernHapRozet { Text = "5'lik Artı (+)" };

            picGiris = FormGezintiYardimcisi.KartliPictureBoxOlustur(tablo, 0, 0, "Giriş Görüntüsü", null, null, rozetGiris);
            picKareSonuc = FormGezintiYardimcisi.KartliPictureBoxOlustur(tablo, 1, 0, "1. Yöntem: 9'luk Kare (3×3) Şablon", null, null, rozetKare);
            picArtiSonuc = FormGezintiYardimcisi.KartliPictureBoxOlustur(tablo, 2, 0, "2. Yöntem: 5'lik Artı (+) Şablon", null, null, rozetArti);

            ModernKartPanel altSerit = FormGezintiYardimcisi.AltAnalizSeridiOlustur("Analiz: Morfolojik karşılaştırma hazır.", out lblYorum, "Morfoloji");

            pnlOrta.Controls.Add(tablo);
            pnlOrta.Controls.Add(altSerit);

            FormGezintiYardimcisi.YerlesimiDuzenle(this, pnlUst, pnlSag, pnlOrta);
        }

        private NumericUpDown RgbNumericOlustur(Point konum, int varsayilan)
        {
            NumericUpDown num = new NumericUpDown
            {
                Location = konum,
                Size = new Size(64, 22),
                Minimum = 0,
                Maximum = 255,
                Value = varsayilan,
                Font = new Font("Segoe UI", 8.5F)
            };
            num.ValueChanged += (s, e) =>
            {
                if (!programatikDegisim && chkRenkAraligiKullan.Checked)
                    KarsilastirmaliUygula();
            };
            return num;
        }

        private void AyarDegisti_Olayi(object sender, EventArgs e)
        {
            if (programatikDegisim) return;
            if (sender is RadioButton rb && !rb.Checked) return;
            KarsilastirmaliUygula();
        }

        private void BtnResimSec_Click(object sender, EventArgs e)
        {
            string dosya = FormGezintiYardimcisi.ResimDosyasiSec();
            if (string.IsNullOrEmpty(dosya)) return;

            temizReferans?.Dispose();
            temizReferans = null;
            orijinalResim?.Dispose();

            using Bitmap ham = new Bitmap(dosya);
            orijinalResim = BitmapYardimcisi.BoyutSinirla(ham, 380);
            KarsilastirmaliUygula();
        }

        private void OrnekTuzBiberYukle()
        {
            programatikDegisim = true;
            try
            {
                rbAcmaKapama.Checked = true;
                chkRenkAraligiKullan.Checked = false;
                cmbSablonSecimi.SelectedIndex = 0;
                numTekrar.Value = 1;
            }
            finally
            {
                programatikDegisim = false;
            }

            temizReferans?.Dispose();
            orijinalResim?.Dispose();

            Bitmap temiz = new Bitmap(320, 240);
            using (Graphics g = Graphics.FromImage(temiz))
            {
                g.SmoothingMode = SmoothingMode.None;
                g.Clear(Color.Black);
                g.FillEllipse(Brushes.White, 40, 35, 130, 150);
                g.FillRectangle(Brushes.Black, 85, 85, 40, 45);
                using Pen p = new Pen(Color.White, 7);
                g.DrawLine(p, 195, 50, 285, 190);
                g.DrawLine(p, 195, 185, 285, 55);
            }
            temizReferans = temiz;

            Bitmap gurultulu = (Bitmap)temiz.Clone();
            Random rnd = new Random(42);
            for (int i = 0; i < 450; i++)
            {
                gurultulu.SetPixel(rnd.Next(gurultulu.Width), rnd.Next(gurultulu.Height), Color.White);
                gurultulu.SetPixel(rnd.Next(gurultulu.Width), rnd.Next(gurultulu.Height), Color.Black);
            }
            // Bazı ikili gürültü öbekleri ekle ki 9'luk ve 5'lik farkı gözlemlenebilsin
            for (int i = 0; i < 45; i++)
            {
                int nx = rnd.Next(1, gurultulu.Width - 2);
                int ny = rnd.Next(1, gurultulu.Height - 2);
                gurultulu.SetPixel(nx, ny, Color.White);
                gurultulu.SetPixel(nx + 1, ny, Color.White);
                int bx = rnd.Next(45, 165);
                int by = rnd.Next(50, 175);
                gurultulu.SetPixel(bx, by, Color.Black);
                gurultulu.SetPixel(bx, by + 1, Color.Black);
            }

            orijinalResim = gurultulu;
            KarsilastirmaliUygula();
        }

        private void OrnekKopukKenarYukle()
        {
            programatikDegisim = true;
            try
            {
                rbKapama.Checked = true;
                chkRenkAraligiKullan.Checked = false;
                cmbSablonSecimi.SelectedIndex = 0;
                numTekrar.Value = 1;
            }
            finally
            {
                programatikDegisim = false;
            }

            temizReferans?.Dispose();
            temizReferans = null;
            orijinalResim?.Dispose();

            Bitmap bmp = new Bitmap(320, 240);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.None;
                g.Clear(Color.Black);
                using Pen p = new Pen(Color.White, 3);
                g.DrawRectangle(p, 35, 45, 115, 145);
                g.DrawEllipse(p, 175, 50, 110, 130);
                // Köşegen kenarlar üzerinde boşluk testi
                g.DrawLine(p, 185, 195, 275, 195);
            }

            // Yatay/dikey 2px kenar kopuklukları (her iki şablon da kapatabilir)
            for (int d = 0; d < 2; d++)
            {
                bmp.SetPixel(88 + d, 45, Color.Black);
                bmp.SetPixel(88 + d, 46, Color.Black);
                bmp.SetPixel(35, 115 + d, Color.Black);
                bmp.SetPixel(36, 115 + d, Color.Black);
                bmp.SetPixel(150, 115 + d, Color.Black);
                bmp.SetPixel(149, 115 + d, Color.Black);
            }

            // Çapraz yay üzerindeki 1px eğik kenar kopuklukları (9'luk kare çapraz komşuluğuyla daha iyi kapatır)
            for (int y = 70; y <= 160; y++)
            {
                for (int x = 175; x <= 285; x++)
                {
                    if ((x == 192 || x == 268) && Math.Abs(y - 115) > 25 && bmp.GetPixel(x, y).R > 128)
                    {
                        bmp.SetPixel(x, y, Color.Black);
                    }
                }
            }

            orijinalResim = bmp;
            KarsilastirmaliUygula();
        }

        private void OrnekRgbAraligiYukle()
        {
            programatikDegisim = true;
            try
            {
                chkRenkAraligiKullan.Checked = true;
                rbKapama.Checked = true;
                cmbSablonSecimi.SelectedIndex = 0;
                numTekrar.Value = 1;
                numMinR.Value = 50; numMaxR.Value = 120;
                numMinG.Value = 0; numMaxG.Value = 200;
                numMinB.Value = 150; numMaxB.Value = 180;
            }
            finally
            {
                programatikDegisim = false;
            }

            temizReferans?.Dispose();
            temizReferans = null;
            orijinalResim?.Dispose();

            Bitmap bmp = new Bitmap(320, 240);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.None;
                g.Clear(Color.FromArgb(20, 24, 32));

                // Hedef aralık (50<R<120, 0<G<200, 150<B<180) içindeki birbirine yakın iki ton
                Color ton1 = Color.FromArgb(75, 120, 162);
                Color ton2 = Color.FromArgb(105, 170, 175);
                using (Brush b1 = new SolidBrush(ton1))
                using (Brush b2 = new SolidBrush(ton2))
                {
                    g.FillRectangle(b1, 35, 50, 95, 130);
                    g.FillRectangle(b2, 132, 50, 90, 130); // Arada 2px boşluk (x=130, 131)
                }

                // Aralık dışı dikkat dağıtıcı renkli nesneler (kırmızı ve sarı)
                using Brush bKirmizi = new SolidBrush(Color.FromArgb(220, 40, 40));
                using Brush bSari = new SolidBrush(Color.FromArgb(230, 200, 40));
                g.FillEllipse(bKirmizi, 240, 35, 60, 75);
                g.FillEllipse(bSari, 240, 130, 60, 75);
            }

            orijinalResim = bmp;
            KarsilastirmaliUygula();
        }

        private void Temizle()
        {
            temizReferans?.Dispose();
            temizReferans = null;
            orijinalResim?.Dispose();
            orijinalResim = null;

            ResmiPictureBoxaAta(picGiris, null);
            ResmiPictureBoxaAta(picKareSonuc, null);
            ResmiPictureBoxaAta(picArtiSonuc, null);

            rozetGiris.MetinGuncelle("Boş");
            rozetKare.MetinGuncelle("Bekliyor");
            rozetArti.MetinGuncelle("Bekliyor");
            lblYorum.Text = "Görsel temizlendi. Örnek butonlarından birini seçin veya bilgisayardan görüntü yükleyin.";
        }

        private static void ResmiPictureBoxaAta(PictureBox pic, Bitmap yeni)
        {
            Image onceki = pic.Image;
            pic.Image = yeni;
            if (onceki != null)
                onceki.Dispose();
        }

        private MorfolojiIslemTuru SeciliIslemAl()
        {
            if (rbGenislet.Checked) return MorfolojiIslemTuru.BeyaziGenislet;
            if (rbAsindir.Checked) return MorfolojiIslemTuru.BeyaziDaralt;
            if (rbAcma.Checked) return MorfolojiIslemTuru.Acma;
            if (rbKapama.Checked) return MorfolojiIslemTuru.Kapama;
            return MorfolojiIslemTuru.AcmaVeKapama;
        }

        private void KarsilastirmaliUygula(bool kullaniciTetikledi = false)
        {
            if (orijinalResim == null)
            {
                if (kullaniciTetikledi)
                {
                    MessageBox.Show("Lütfen önce bir görüntü yükleyin veya örnek görsellerden birini seçin.",
                        "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                return;
            }

            ResmiPictureBoxaAta(picGiris, (Bitmap)orijinalResim.Clone());

            MorfolojiIslemTuru islem = SeciliIslemAl();
            int tekrar = (int)numTekrar.Value;
            bool rgbModu = chkRenkAraligiKullan.Checked;
            int minR = (int)numMinR.Value, maxR = (int)numMaxR.Value;
            int minG = (int)numMinG.Value, maxG = (int)numMaxG.Value;
            int minB = (int)numMinB.Value, maxB = (int)numMaxB.Value;

            int sablonSecimi = cmbSablonSecimi.SelectedIndex; // 0: İkisi, 1: Kare, 2: Artı
            bool kareCalis = sablonSecimi == 0 || sablonSecimi == 1;
            bool artiCalis = sablonSecimi == 0 || sablonSecimi == 2;

            Bitmap kareSonuc = kareCalis
                ? MorfolojiCalistir(orijinalResim, true, islem, tekrar, rgbModu, minR, maxR, minG, maxG, minB, maxB)
                : null;
            Bitmap artiSonuc = artiCalis
                ? MorfolojiCalistir(orijinalResim, false, islem, tekrar, rgbModu, minR, maxR, minG, maxG, minB, maxB)
                : null;

            ResmiPictureBoxaAta(picKareSonuc, kareSonuc);
            ResmiPictureBoxaAta(picArtiSonuc, artiSonuc);

            MetrikleriHesaplaVeYorumla(kareSonuc, artiSonuc, islem, tekrar, rgbModu, minR, maxR, minG, maxG, minB, maxB);
        }

        private void MetrikleriHesaplaVeYorumla(
            Bitmap kareSonuc, Bitmap artiSonuc,
            MorfolojiIslemTuru islem, int tekrar,
            bool rgbModu, int minR, int maxR, int minG, int maxG, int minB, int maxB)
        {
            string islemAdi = islem switch
            {
                MorfolojiIslemTuru.BeyaziGenislet => "Genişletme",
                MorfolojiIslemTuru.BeyaziDaralt => "Daraltma",
                MorfolojiIslemTuru.Acma => "Açma",
                MorfolojiIslemTuru.Kapama => "Kapama",
                _ => "Açma + Kapama"
            };

            if (temizReferans != null)
            {
                int baslangicHata = FarkliPikselSay(orijinalResim, temizReferans);
                int toplamPiksel = temizReferans.Width * temizReferans.Height;
                rozetGiris.MetinGuncelle($"Gürültü: {baslangicHata} px");

                string kareOzet = "Devre Dışı";
                string artiOzet = "Devre Dışı";
                int kareHata = int.MaxValue, artiHata = int.MaxValue;

                if (kareSonuc != null)
                {
                    kareHata = FarkliPikselSay(kareSonuc, temizReferans);
                    double dogruluk = 100.0 * (toplamPiksel - kareHata) / toplamPiksel;
                    rozetKare.MetinGuncelle($"Kalan: {kareHata} px (%{dogruluk:F2})");
                    kareOzet = $"9'luk Kare kalan hata: {kareHata} px (%{dogruluk:F2})";
                }
                else
                {
                    rozetKare.MetinGuncelle("Devre Dışı");
                }

                if (artiSonuc != null)
                {
                    artiHata = FarkliPikselSay(artiSonuc, temizReferans);
                    double dogruluk = 100.0 * (toplamPiksel - artiHata) / toplamPiksel;
                    rozetArti.MetinGuncelle($"Kalan: {artiHata} px (%{dogruluk:F2})");
                    artiOzet = $"5'lik Artı kalan hata: {artiHata} px (%{dogruluk:F2})";
                }
                else
                {
                    rozetArti.MetinGuncelle("Devre Dışı");
                }

                string kazanan = "";
                if (kareSonuc != null && artiSonuc != null)
                {
                    if (artiHata < kareHata)
                        kazanan = $" | Karar: 5'lik Artı (+), {kareHata - artiHata} px daha az kenar bozulmasıyla gürültüyü daha iyi temizledi.";
                    else if (kareHata < artiHata)
                        kazanan = $" | Karar: 9'luk Kare (3×3), {artiHata - kareHata} px daha düşük kalan gürültüyle daha iyi temizledi.";
                    else
                        kazanan = " | Karar: Her iki şablon da eşit doğruluk sağladı.";
                }

                lblYorum.Text = $"{islemAdi} (x{tekrar}) • Başlangıç gürültüsü: {baslangicHata} px | {kareOzet} | {artiOzet}{kazanan}";
                return;
            }

            // Kopuk kenar veya RGB renk aralığı modu: bağlı bileşen sayımı ile analiz
            int girisParca = BagliBilesenSay(orijinalResim, rgbModu, minR, maxR, minG, maxG, minB, maxB);
            rozetGiris.MetinGuncelle(rgbModu ? $"RGB Parça: {girisParca}" : $"Parça: {girisParca}");

            int kareParca = -1, artiParca = -1;
            if (kareSonuc != null)
            {
                kareParca = BagliBilesenSay(kareSonuc, false, 0, 255, 0, 255, 0, 255);
                rozetKare.MetinGuncelle($"Parça: {kareParca}");
            }
            else
            {
                rozetKare.MetinGuncelle("Devre Dışı");
            }

            if (artiSonuc != null)
            {
                artiParca = BagliBilesenSay(artiSonuc, false, 0, 255, 0, 255, 0, 255);
                rozetArti.MetinGuncelle($"Parça: {artiParca}");
            }
            else
            {
                rozetArti.MetinGuncelle("Devre Dışı");
            }

            string modMetni = rgbModu
                ? $"RGB ({Math.Min(minR, maxR)}..{Math.Max(minR, maxR)}, {Math.Min(minG, maxG)}..{Math.Max(minG, maxG)}, {Math.Min(minB, maxB)}..{Math.Max(minB, maxB)}) • "
                : "";

            string parcaKarsilastirma = "";
            if (kareSonuc != null && artiSonuc != null)
            {
                if (kareParca < artiParca)
                    parcaKarsilastirma = $" | Karar: 9'luk Kare (3×3), parçaları {girisParca} -> {kareParca} seviyesine indirerek boşlukları daha iyi kapattı (5'lik Artı: {artiParca}).";
                else if (artiParca < kareParca)
                    parcaKarsilastirma = $" | Karar: 5'lik Artı (+), parçaları {girisParca} -> {artiParca} seviyesine indirerek daha iyi ayırdı/kapattı (9'luk Kare: {kareParca}).";
                else
                    parcaKarsilastirma = $" | Her iki şablon da {girisParca} -> {kareParca} bileşen verdi (9'luk Kare çapraz açıklıklarda daha etkilidir).";
            }
            else if (kareSonuc != null)
            {
                parcaKarsilastirma = $" | 9'luk Kare sonucu: {girisParca} -> {kareParca} parça.";
            }
            else if (artiSonuc != null)
            {
                parcaKarsilastirma = $" | 5'lik Artı sonucu: {girisParca} -> {artiParca} parça.";
            }

            lblYorum.Text = $"{modMetni}{islemAdi} (x{tekrar}) • Giriş kopuk parça: {girisParca}{parcaKarsilastirma}";
        }

        private static int FarkliPikselSay(Bitmap a, Bitmap b)
        {
            int w = Math.Min(a.Width, b.Width);
            int h = Math.Min(a.Height, b.Height);
            int fark = 0;
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    bool ba = (a.GetPixel(x, y).R + a.GetPixel(x, y).G + a.GetPixel(x, y).B) / 3 > 128;
                    bool bb = (b.GetPixel(x, y).R + b.GetPixel(x, y).G + b.GetPixel(x, y).B) / 3 > 128;
                    if (ba != bb) fark++;
                }
            }
            return fark;
        }

        private static int BagliBilesenSay(Bitmap bmp, bool rgbMaskele, int minR, int maxR, int minG, int maxG, int minB, int maxB)
        {
            int w = bmp.Width;
            int h = bmp.Height;
            int rMin = Math.Min(minR, maxR), rMax = Math.Max(minR, maxR);
            int gMin = Math.Min(minG, maxG), gMax = Math.Max(minG, maxG);
            int bMin = Math.Min(minB, maxB), bMax = Math.Max(minB, maxB);

            using Bitmap ikili = new Bitmap(w, h);
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    Color p = bmp.GetPixel(x, y);
                    bool beyazMi = rgbMaskele
                        ? (p.R >= rMin && p.R <= rMax && p.G >= gMin && p.G <= gMax && p.B >= bMin && p.B <= bMax)
                        : ((p.R + p.G + p.B) / 3 > 128);
                    ikili.SetPixel(x, y, beyazMi ? Color.White : Color.Black);
                }
            }

            using Bitmap renklendirilmis = BitmapYardimcisi.BolgeBulVeRenklendir(ikili, 10, out List<BitmapYardimcisi.BolgeBilgisi> bolgeler, out _);
            return bolgeler.Count;
        }

        public static Bitmap MorfolojiCalistir(
            Bitmap giris,
            bool kareSablonMu,
            MorfolojiIslemTuru islem,
            int tekrarSayisi,
            bool rgbAralikKullan,
            int minR, int maxR,
            int minG, int maxG,
            int minB, int maxB)
        {
            if (giris == null) return null;

            int adim = Math.Max(1, tekrarSayisi);
            int w = giris.Width;
            int h = giris.Height;

            if (rgbAralikKullan)
            {
                int rMin = Math.Min(minR, maxR), rMax = Math.Max(minR, maxR);
                int gMin = Math.Min(minG, maxG), gMax = Math.Max(minG, maxG);
                int bMin = Math.Min(minB, maxB), bMax = Math.Max(minB, maxB);

                bool[,] maske = new bool[w, h];
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        Color p = giris.GetPixel(x, y);
                        maske[x, y] = p.R >= rMin && p.R <= rMax &&
                                      p.G >= gMin && p.G <= gMax &&
                                      p.B >= bMin && p.B <= bMax;
                    }
                }

                maske = islem switch
                {
                    MorfolojiIslemTuru.BeyaziGenislet => IkiliTekrarliUygula(maske, w, h, kareSablonMu, true, adim),
                    MorfolojiIslemTuru.BeyaziDaralt => IkiliTekrarliUygula(maske, w, h, kareSablonMu, false, adim),
                    MorfolojiIslemTuru.Acma => IkiliTekrarliUygula(IkiliTekrarliUygula(maske, w, h, kareSablonMu, false, adim), w, h, kareSablonMu, true, adim),
                    MorfolojiIslemTuru.Kapama => IkiliTekrarliUygula(IkiliTekrarliUygula(maske, w, h, kareSablonMu, true, adim), w, h, kareSablonMu, false, adim),
                    _ => IkiliTekrarliUygula(
                            IkiliTekrarliUygula(
                                IkiliTekrarliUygula(
                                    IkiliTekrarliUygula(maske, w, h, kareSablonMu, false, adim),
                                    w, h, kareSablonMu, true, adim),
                                w, h, kareSablonMu, true, adim),
                            w, h, kareSablonMu, false, adim)
                };

                Bitmap sonuc = new Bitmap(w, h);
                for (int y = 0; y < h; y++)
                    for (int x = 0; x < w; x++)
                        sonuc.SetPixel(x, y, maske[x, y] ? Color.White : Color.Black);
                return sonuc;
            }

            int[,] gri = new int[w, h];
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    Color p = giris.GetPixel(x, y);
                    gri[x, y] = (p.R + p.G + p.B) / 3;
                }
            }

            gri = islem switch
            {
                MorfolojiIslemTuru.BeyaziGenislet => GriTekrarliUygula(gri, w, h, kareSablonMu, true, adim),
                MorfolojiIslemTuru.BeyaziDaralt => GriTekrarliUygula(gri, w, h, kareSablonMu, false, adim),
                MorfolojiIslemTuru.Acma => GriTekrarliUygula(GriTekrarliUygula(gri, w, h, kareSablonMu, false, adim), w, h, kareSablonMu, true, adim),
                MorfolojiIslemTuru.Kapama => GriTekrarliUygula(GriTekrarliUygula(gri, w, h, kareSablonMu, true, adim), w, h, kareSablonMu, false, adim),
                _ => GriTekrarliUygula(
                        GriTekrarliUygula(
                            GriTekrarliUygula(
                                GriTekrarliUygula(gri, w, h, kareSablonMu, false, adim),
                                w, h, kareSablonMu, true, adim),
                            w, h, kareSablonMu, true, adim),
                        w, h, kareSablonMu, false, adim)
            };

            Bitmap griSonuc = new Bitmap(w, h);
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int v = gri[x, y];
                    griSonuc.SetPixel(x, y, Color.FromArgb(v, v, v));
                }
            }
            return griSonuc;
        }

        private static bool[,] IkiliTekrarliUygula(bool[,] kaynak, int w, int h, bool kareSablonMu, bool genisletMeMi, int tekrar)
        {
            bool[,] mevcut = kaynak;
            for (int i = 0; i < tekrar; i++)
                mevcut = IkiliMorfolojiAdimi(mevcut, w, h, kareSablonMu, genisletMeMi);
            return mevcut;
        }

        private static bool[,] IkiliMorfolojiAdimi(bool[,] kaynak, int w, int h, bool kareSablonMu, bool genisletMeMi)
        {
            bool[,] hedef = new bool[w, h];
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    bool ucDeger = !genisletMeMi; // Genişletme başlangıcı false (OR), Daraltma başlangıcı true (AND)
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        for (int dx = -1; dx <= 1; dx++)
                        {
                            if (!kareSablonMu && Math.Abs(dx) + Math.Abs(dy) > 1)
                                continue;

                            int nx = x + dx;
                            int ny = y + dy;
                            if (nx < 0 || nx >= w || ny < 0 || ny >= h)
                                continue;

                            bool deger = kaynak[nx, ny];
                            if (genisletMeMi)
                            {
                                if (deger) { ucDeger = true; break; }
                            }
                            else
                            {
                                if (!deger) { ucDeger = false; break; }
                            }
                        }
                        if ((genisletMeMi && ucDeger) || (!genisletMeMi && !ucDeger))
                            break;
                    }
                    hedef[x, y] = ucDeger;
                }
            }
            return hedef;
        }

        private static int[,] GriTekrarliUygula(int[,] kaynak, int w, int h, bool kareSablonMu, bool genisletMeMi, int tekrar)
        {
            int[,] mevcut = kaynak;
            for (int i = 0; i < tekrar; i++)
                mevcut = GriMorfolojiAdimi(mevcut, w, h, kareSablonMu, genisletMeMi);
            return mevcut;
        }

        private static int[,] GriMorfolojiAdimi(int[,] kaynak, int w, int h, bool kareSablonMu, bool genisletMeMi)
        {
            int[,] hedef = new int[w, h];
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int ucDeger = genisletMeMi ? 0 : 255;
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        for (int dx = -1; dx <= 1; dx++)
                        {
                            if (!kareSablonMu && Math.Abs(dx) + Math.Abs(dy) > 1)
                                continue;

                            int nx = x + dx;
                            int ny = y + dy;
                            if (nx < 0 || nx >= w || ny < 0 || ny >= h)
                                continue;

                            int val = kaynak[nx, ny];
                            ucDeger = genisletMeMi ? Math.Max(ucDeger, val) : Math.Min(ucDeger, val);
                        }
                    }
                    hedef[x, y] = ucDeger;
                }
            }
            return hedef;
        }
    }
}
