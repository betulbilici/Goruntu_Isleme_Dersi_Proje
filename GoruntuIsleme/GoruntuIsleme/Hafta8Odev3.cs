using System;
using System.Drawing;
using System.Windows.Forms;

namespace GoruntuIsleme
{
    public class Hafta8Odev3 : Form
    {
        private PictureBox picGiris, picKareSonuc, picArtiSonuc;
        private RadioButton rbGenislet, rbAsindir, rbAcma, rbKapama;
        private NumericUpDown numMinR, numMaxR, numMinG, numMaxG, numMinB, numMaxB;
        private CheckBox chkRenkAraligiKullan;
        private ModernHapRozet rozetGiris, rozetKare, rozetArti;
        private Label lblYorum;
        private Bitmap orijinalResim;

        public Hafta8Odev3()
        {
            Text = "Hafta 8 - Ödev 3 | Morfolojik Şablonlar ve RGB Renk Aralığı";
            FormGezintiYardimcisi.FormuHazirla(this);
            ArayuzuOlustur();
            OrnekTuzBiberYukle();
        }

        private void ArayuzuOlustur()
        {
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                this,
                "Hafta 8 - Ödev 3 | Morfolojik İşlemler (9'luk Kare vs 5'lik Artı Şablon)",
                "< Ödev 2", () => FormGezintiYardimcisi.FormGecisYap(this, new Hafta8Odev2()),
                "Ödev 4 >", () => FormGezintiYardimcisi.FormGecisYap(this, new Hafta8Odev4()));

            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 295);

            Label lblKaynak = new Label
            {
                Text = "GÖRÜNTÜ VE ÖRNEKLER",
                Location = new Point(18, 16),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 8.8f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi
            };

            ModernHapButon btnResimSec = FormGezintiYardimcisi.ModernButonOlustur("Görüntü Dosyası Seç", true, 255, 36);
            btnResimSec.Location = new Point(18, 40);

            ModernHapButon btnTuzBiber = FormGezintiYardimcisi.ModernButonOlustur("Örnek 1: Tuz-Biber Gürültüsü", false, 255, 34);
            btnTuzBiber.Location = new Point(18, 84);

            ModernHapButon btnKopukKenar = FormGezintiYardimcisi.ModernButonOlustur("Örnek 2: Kopuk Kenarlı Şekil", false, 255, 34);
            btnKopukKenar.Location = new Point(18, 124);

            btnResimSec.Click += (s, e) =>
            {
                string yol = FormGezintiYardimcisi.ResimDosyasiSec();
                if (yol != null)
                {
                    using Bitmap ham = new Bitmap(yol);
                    orijinalResim = BitmapYardimcisi.BoyutSinirla(ham, 380);
                    picGiris.Image = orijinalResim;
                    KarsilastirmaliUygula();
                }
            };
            btnTuzBiber.Click += (s, e) => OrnekTuzBiberYukle();
            btnKopukKenar.Click += (s, e) => OrnekKopukKenarYukle();

            ModernKartPanel kartIslem = new ModernKartPanel
            {
                Location = new Point(18, 170),
                Size = new Size(255, 132),
                KartArkaPlan = FormGezintiYardimcisi.AcikMaviZemin,
                KenarlikRengi = FormGezintiYardimcisi.AcikMaviKenar,
                Padding = new Padding(12)
            };
            Label lblIslemBaslik = new Label { Text = "Morfolojik İşlem Türü", Location = new Point(12, 10), AutoSize = true, Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold), ForeColor = FormGezintiYardimcisi.KoyuMaviYazi, BackColor = FormGezintiYardimcisi.AcikMaviZemin };
            rbGenislet = new RadioButton { Text = "Genişletme (Dilation)", Location = new Point(12, 34), AutoSize = true, BackColor = FormGezintiYardimcisi.AcikMaviZemin };
            rbAsindir = new RadioButton { Text = "Aşındırma (Erosion)", Location = new Point(12, 56), AutoSize = true, BackColor = FormGezintiYardimcisi.AcikMaviZemin };
            rbAcma = new RadioButton { Text = "Açma (Aşındır → Genişlet)", Location = new Point(12, 78), AutoSize = true, Checked = true, BackColor = FormGezintiYardimcisi.AcikMaviZemin };
            rbKapama = new RadioButton { Text = "Kapama (Genişlet → Aşındır)", Location = new Point(12, 100), AutoSize = true, BackColor = FormGezintiYardimcisi.AcikMaviZemin };

            rbGenislet.CheckedChanged += (s, e) => { if (rbGenislet.Checked) KarsilastirmaliUygula(); };
            rbAsindir.CheckedChanged += (s, e) => { if (rbAsindir.Checked) KarsilastirmaliUygula(); };
            rbAcma.CheckedChanged += (s, e) => { if (rbAcma.Checked) KarsilastirmaliUygula(); };
            rbKapama.CheckedChanged += (s, e) => { if (rbKapama.Checked) KarsilastirmaliUygula(); };

            kartIslem.Controls.AddRange(new Control[] { lblIslemBaslik, rbGenislet, rbAsindir, rbAcma, rbKapama });

            ModernKartPanel kartRgb = new ModernKartPanel
            {
                Location = new Point(18, 314),
                Size = new Size(255, 168),
                KartArkaPlan = FormGezintiYardimcisi.AcikMaviZemin,
                KenarlikRengi = FormGezintiYardimcisi.AcikMaviKenar,
                Padding = new Padding(12)
            };
            chkRenkAraligiKullan = new CheckBox
            {
                Text = "Özel RGB Aralığını Filtrele",
                Location = new Point(12, 12),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi,
                BackColor = FormGezintiYardimcisi.AcikMaviZemin
            };
            chkRenkAraligiKullan.CheckedChanged += (s, e) => KarsilastirmaliUygula();

            Label lR = new Label { Text = "R (Min–Max):", Location = new Point(12, 46), AutoSize = true, BackColor = FormGezintiYardimcisi.AcikMaviZemin };
            numMinR = new NumericUpDown { Location = new Point(105, 44), Width = 64, Minimum = 0, Maximum = 255, Value = 50 };
            numMaxR = new NumericUpDown { Location = new Point(178, 44), Width = 64, Minimum = 0, Maximum = 255, Value = 120 };

            Label lG = new Label { Text = "G (Min–Max):", Location = new Point(12, 82), AutoSize = true, BackColor = FormGezintiYardimcisi.AcikMaviZemin };
            numMinG = new NumericUpDown { Location = new Point(105, 80), Width = 64, Minimum = 0, Maximum = 255, Value = 0 };
            numMaxG = new NumericUpDown { Location = new Point(178, 80), Width = 64, Minimum = 0, Maximum = 255, Value = 200 };

            Label lB = new Label { Text = "B (Min–Max):", Location = new Point(12, 118), AutoSize = true, BackColor = FormGezintiYardimcisi.AcikMaviZemin };
            numMinB = new NumericUpDown { Location = new Point(105, 116), Width = 64, Minimum = 0, Maximum = 255, Value = 150 };
            numMaxB = new NumericUpDown { Location = new Point(178, 116), Width = 64, Minimum = 0, Maximum = 255, Value = 180 };

            kartRgb.Controls.AddRange(new Control[] { chkRenkAraligiKullan, lR, numMinR, numMaxR, lG, numMinG, numMaxG, lB, numMinB, numMaxB });

            ModernHapButon btnUygula = FormGezintiYardimcisi.ModernButonOlustur("Şablonları Karşılaştır", true, 255, 38);
            btnUygula.Location = new Point(18, 496);
            btnUygula.Click += (s, e) => KarsilastirmaliUygula();

            pnlSag.Controls.AddRange(new Control[] { lblKaynak, btnResimSec, btnTuzBiber, btnKopukKenar, kartIslem, kartRgb, btnUygula });

            rozetGiris = new ModernHapRozet();
            rozetGiris.MetinGuncelle("Giriş");

            rozetKare = new ModernHapRozet();
            rozetKare.MetinGuncelle("3×3 Kare");

            rozetArti = new ModernHapRozet();
            rozetArti.MetinGuncelle("5'lik Artı (+)");

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
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 85f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 15f));

            picGiris = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 0, 0, "Orijinal Giriş Görüntüsü", null, null, rozetGiris);
            picKareSonuc = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 1, 0, "9'luk Kare Şablon Sonucu", null, null, rozetKare);
            picArtiSonuc = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 2, 0, "5'lik Artı (+) Şablon Sonucu", null, null, rozetArti);

            ModernKartPanel altSerit = FormGezintiYardimcisi.AltAnalizSeridiOlustur(
                "Şablon Karşılaştırması: 9'luk kare şablon çapraz komşuları da içerdiği için noktasal gürültü ve kopuklukları daha güçlü kapatır; 5'lik artı şablon ise ince köşeleri daha iyi korur.",
                out lblYorum);
            tbl.Controls.Add(altSerit, 0, 1);
            tbl.SetColumnSpan(altSerit, 3);

            Controls.Add(tbl);
            FormGezintiYardimcisi.YerlesimiDuzenle(this, pnlUst, pnlSag, tbl);
        }

        private void OrnekTuzBiberYukle()
        {
            Bitmap bmp = new Bitmap(280, 220);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Black);
                g.FillEllipse(Brushes.White, 55, 45, 170, 130);
                g.FillRectangle(Brushes.Black, 105, 85, 70, 50);
            }
            Random rnd = new Random(7);
            for (int i = 0; i < 450; i++)
            {
                int x = rnd.Next(0, bmp.Width);
                int y = rnd.Next(0, bmp.Height);
                bmp.SetPixel(x, y, i % 2 == 0 ? Color.White : Color.Black);
            }
            orijinalResim = bmp;
            picGiris.Image = orijinalResim;
            rbAcma.Checked = true;
            chkRenkAraligiKullan.Checked = false;
            KarsilastirmaliUygula();
        }

        private void OrnekKopukKenarYukle()
        {
            Bitmap bmp = new Bitmap(280, 220);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Black);
                using Pen p = new Pen(Color.White, 3);
                g.DrawRectangle(p, 45, 40, 190, 140);
                g.DrawEllipse(p, 95, 65, 90, 90);
                using Pen kesik = new Pen(Color.Black, 3);
                for (int x = 55; x < 230; x += 22)
                    g.DrawLine(kesik, x, 38, x + 2, 44);
                for (int y = 50; y < 175; y += 20)
                    g.DrawLine(kesik, 43, y, 48, y + 2);
            }
            orijinalResim = bmp;
            picGiris.Image = orijinalResim;
            rbKapama.Checked = true;
            chkRenkAraligiKullan.Checked = false;
            KarsilastirmaliUygula();
        }

        private bool PikselHedefAraliktaMi(Color c)
        {
            if (chkRenkAraligiKullan.Checked)
            {
                return c.R >= numMinR.Value && c.R <= numMaxR.Value &&
                       c.G >= numMinG.Value && c.G <= numMaxG.Value &&
                       c.B >= numMinB.Value && c.B <= numMaxB.Value;
            }
            return (c.R + c.G + c.B) / 3 > 128;
        }

        private Bitmap MorfolojiUygula(Bitmap giris, bool kareSablonMu, bool genisletMeMi)
        {
            int w = giris.Width;
            int h = giris.Height;
            Bitmap cikis = new Bitmap(w, h);

            int[,] artiOfset = { { 0, 0 }, { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 } };

            for (int x = 1; x < w - 1; x++)
            {
                for (int y = 1; y < h - 1; y++)
                {
                    int enYuksekGri = 0;
                    int enDusukGri = 255;
                    bool komsudaHedefVar = false;
                    bool tumKomsularHedef = true;

                    if (kareSablonMu)
                    {
                        for (int i = -1; i <= 1; i++)
                        {
                            for (int j = -1; j <= 1; j++)
                            {
                                Color c = giris.GetPixel(x + i, y + j);
                                int gri = (c.R + c.G + c.B) / 3;
                                if (gri > enYuksekGri) enYuksekGri = gri;
                                if (gri < enDusukGri) enDusukGri = gri;

                                bool hedef = PikselHedefAraliktaMi(c);
                                if (hedef) komsudaHedefVar = true;
                                else tumKomsularHedef = false;
                            }
                        }
                    }
                    else
                    {
                        for (int k = 0; k < 5; k++)
                        {
                            Color c = giris.GetPixel(x + artiOfset[k, 0], y + artiOfset[k, 1]);
                            int gri = (c.R + c.G + c.B) / 3;
                            if (gri > enYuksekGri) enYuksekGri = gri;
                            if (gri < enDusukGri) enDusukGri = gri;

                            bool hedef = PikselHedefAraliktaMi(c);
                            if (hedef) komsudaHedefVar = true;
                            else tumKomsularHedef = false;
                        }
                    }

                    if (chkRenkAraligiKullan.Checked)
                    {
                        bool secili = genisletMeMi ? komsudaHedefVar : tumKomsularHedef;
                        cikis.SetPixel(x, y, secili ? Color.White : Color.Black);
                    }
                    else
                    {
                        int sonucDeger = genisletMeMi ? enYuksekGri : enDusukGri;
                        cikis.SetPixel(x, y, Color.FromArgb(sonucDeger, sonucDeger, sonucDeger));
                    }
                }
            }
            return cikis;
        }

        private Bitmap IslemCalistir(Bitmap kaynak, bool kareSablonMu)
        {
            if (rbGenislet.Checked)
                return MorfolojiUygula(kaynak, kareSablonMu, true);
            if (rbAsindir.Checked)
                return MorfolojiUygula(kaynak, kareSablonMu, false);
            if (rbAcma.Checked)
            {
                using Bitmap asindirilmis = MorfolojiUygula(kaynak, kareSablonMu, false);
                return MorfolojiUygula(asindirilmis, kareSablonMu, true);
            }
            using Bitmap genisletilmis = MorfolojiUygula(kaynak, kareSablonMu, true);
            return MorfolojiUygula(genisletilmis, kareSablonMu, false);
        }

        private void KarsilastirmaliUygula()
        {
            if (orijinalResim == null) return;

            picKareSonuc.Image = IslemCalistir(orijinalResim, true);
            picArtiSonuc.Image = IslemCalistir(orijinalResim, false);
        }
    }
}
