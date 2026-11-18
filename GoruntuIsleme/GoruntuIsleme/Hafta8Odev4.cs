using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GoruntuIsleme
{
    public class Hafta8Odev4 : Form
    {
        private PictureBox picGiris, picKenar, picSonuc;
        private ModernSurgu trkEsik, trkYirmiBesElliSinir, trkElliBirSinir, trkYaziTuraEsik;
        private ModernHapRozet rozetGiris, rozetKenar, rozetSonuc;
        private Label lblSinir1, lblSinir2, lblOzet;
        private Bitmap orijinalResim;

        public Hafta8Odev4()
        {
            Text = "Hafta 8 - Ödev 4 | Madeni Para Sayma ve Yazı/Tura Tespiti";
            FormGezintiYardimcisi.FormuHazirla(this);
            ArayuzuOlustur();
            OrnekParaResmiOlustur();
        }

        private void ArayuzuOlustur()
        {
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                this,
                "Hafta 8 - Ödev 4 | Madeni Para (25 Kr, 50 Kr, 1 TL) Sayma & Yazı/Tura Tanıma",
                "< Ödev 3", () => FormGezintiYardimcisi.FormGecisYap(this, new Hafta8Odev3()),
                "Hafta 9 >", () => FormGezintiYardimcisi.FormGecisYap(this, new Hafta9Odev1()));

            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 295);

            Label lblKaynak = new Label
            {
                Text = "GÖRÜNTÜ VE ÇAP AYARLARI",
                Location = new Point(18, 16),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 8.8f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi
            };

            ModernHapButon btnResimSec = FormGezintiYardimcisi.ModernButonOlustur("Para Fotoğrafı Seç", true, 255, 38);
            btnResimSec.Location = new Point(18, 42);

            ModernHapButon btnOrnek = FormGezintiYardimcisi.ModernButonOlustur("Örnek Paraları Yükle", false, 255, 36);
            btnOrnek.Location = new Point(18, 88);

            btnResimSec.Click += (s, e) =>
            {
                string yol = FormGezintiYardimcisi.ResimDosyasiSec("Beyaz Kağıt Üzerinde Madeni Para Fotoğrafı Seç");
                if (yol != null)
                {
                    using Bitmap ham = new Bitmap(yol);
                    orijinalResim = BitmapYardimcisi.BoyutSinirla(ham, 420);
                    picGiris.Image = orijinalResim;
                    AnalizEt();
                }
            };
            btnOrnek.Click += (s, e) => OrnekParaResmiOlustur();

            lblSinir1 = new Label { Text = "25 Kr / 50 Kr Çap Sınırı: 75 px", Location = new Point(18, 138), AutoSize = true, Font = new Font("Segoe UI", 8.8f), ForeColor = FormGezintiYardimcisi.AcikYazi };
            trkYirmiBesElliSinir = new ModernSurgu { Location = new Point(18, 158), Width = 255, Minimum = 15, Maximum = 140, Value = 75 };
            trkYirmiBesElliSinir.Scroll += (s, e) =>
            {
                lblSinir1.Text = $"25 Kr / 50 Kr Çap Sınırı: {trkYirmiBesElliSinir.Value} px";
                AnalizEt();
            };

            lblSinir2 = new Label { Text = "50 Kr / 1 TL Çap Sınırı: 82 px", Location = new Point(18, 194), AutoSize = true, Font = new Font("Segoe UI", 8.8f), ForeColor = FormGezintiYardimcisi.AcikYazi };
            trkElliBirSinir = new ModernSurgu { Location = new Point(18, 214), Width = 255, Minimum = 20, Maximum = 160, Value = 82 };
            trkElliBirSinir.Scroll += (s, e) =>
            {
                lblSinir2.Text = $"50 Kr / 1 TL Çap Sınırı: {trkElliBirSinir.Value} px";
                AnalizEt();
            };

            ModernKartPanel kartOzet = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                new Point(18, 256), new Size(255, 220), out lblOzet);

            pnlSag.Controls.AddRange(new Control[]
            {
                lblKaynak, btnResimSec, btnOrnek,
                lblSinir1, trkYirmiBesElliSinir, lblSinir2, trkElliBirSinir, kartOzet
            });

            rozetGiris = new ModernHapRozet();
            rozetGiris.MetinGuncelle("Zemin: 225");

            rozetKenar = new ModernHapRozet();
            rozetKenar.MetinGuncelle("Yoğunluk: %24");

            rozetSonuc = new ModernHapRozet();
            rozetSonuc.MetinGuncelle("0.00 TL");

            Panel pnlGirisAlt = FormGezintiYardimcisi.KartAltSurguPaneliOlustur(
                "Arka Plan Eşik Değeri", "50 – 245", 50, 245, 225, out trkEsik, out _);
            trkEsik.Scroll += (s, e) =>
            {
                rozetGiris.MetinGuncelle($"Zemin: {trkEsik.Value}");
                AnalizEt();
            };

            Panel pnlKenarAlt = FormGezintiYardimcisi.KartAltSurguPaneliOlustur(
                "Yazı / Tura Kenar Yoğunluk Eşiği (%)", "%5 – %60", 5, 60, 24, out trkYaziTuraEsik, out _);
            trkYaziTuraEsik.Scroll += (s, e) =>
            {
                rozetKenar.MetinGuncelle($"Yoğunluk: %{trkYaziTuraEsik.Value}");
                AnalizEt();
            };

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

            picGiris = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 0, 0, "Giriş Fotoğrafı", null, pnlGirisAlt, rozetGiris);
            picKenar = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 1, 0, "Sobel Kenar Deseni (Yazı/Tura)", null, pnlKenarAlt, rozetKenar);
            picSonuc = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 2, 0, "Bölge Çapı & Tanıma Sonucu", null, null, rozetSonuc);

            ModernKartPanel altSerit = FormGezintiYardimcisi.AltAnalizSeridiOlustur(
                "Tanıma Özeti: Bölge sınırlarından (MaxX - MinX) daire çapı ölçülerek para değeri, iç bölgedeki Sobel kenar yoğunluğundan ise Yazı/Tura durumu tespit edilir.",
                out _);
            tbl.Controls.Add(altSerit, 0, 1);
            tbl.SetColumnSpan(altSerit, 3);

            Controls.Add(tbl);
            FormGezintiYardimcisi.YerlesimiDuzenle(this, pnlUst, pnlSag, tbl);
        }

        private void OrnekParaResmiOlustur()
        {
            Bitmap yuklenen = BitmapYardimcisi.OrnekGorselYukle("turk_lirasi_paralar.jpg");
            if (yuklenen != null)
            {
                orijinalResim = yuklenen;
                picGiris.Image = orijinalResim;
                AnalizEt();
                return;
            }
        }

        private void AnalizEt()
        {
            if (orijinalResim == null) return;

            using Bitmap ikili = BitmapYardimcisi.EsiklemeYap(orijinalResim, trkEsik.Value, true);
            Bitmap etiketli = BitmapYardimcisi.BolgeBulVeRenklendir(ikili, 150, out var bolgeler, out int[,] etiketler);
            Bitmap kenarResmi = BitmapYardimcisi.SobelKenarBul(orijinalResim);
            picKenar.Image = kenarResmi;

            int adet25 = 0, adet50 = 0, adet1TL = 0;
            int yaziSay = 0, turaSay = 0;

            using (Graphics g = Graphics.FromImage(etiketli))
            using (Font f = new Font("Segoe UI", 8f, FontStyle.Bold))
            {
                foreach (var b in bolgeler)
                {
                    int cap = b.Cap;
                    string degerYazi;

                    if (cap < trkYirmiBesElliSinir.Value)
                    {
                        adet25++;
                        degerYazi = "25 Kr";
                    }
                    else if (cap < trkElliBirSinir.Value)
                    {
                        adet50++;
                        degerYazi = "50 Kr";
                    }
                    else
                    {
                        adet1TL++;
                        degerYazi = "1 TL";
                    }

                    int icPiksel = 0;
                    int icKenarPiksel = 0;
                    double rIc = (cap / 2.0) * 0.72;

                    for (int x = b.MinX; x <= b.MaxX; x++)
                    {
                        for (int y = b.MinY; y <= b.MaxY; y++)
                        {
                            if (etiketler[x, y] != b.EtiketNo) continue;
                            double uzaklik = Math.Sqrt(Math.Pow(x - b.MerkezX, 2) + Math.Pow(y - b.MerkezY, 2));
                            if (uzaklik <= rIc)
                            {
                                icPiksel++;
                                if (kenarResmi.GetPixel(x, y).R > 55)
                                    icKenarPiksel++;
                            }
                        }
                    }

                    double kenarYuzdesi = icPiksel > 0 ? (100.0 * icKenarPiksel / icPiksel) : 0;
                    bool turaMi = kenarYuzdesi >= trkYaziTuraEsik.Value;
                    if (turaMi) turaSay++; else yaziSay++;

                    string durum = turaMi ? "Tura" : "Yazı";
                    g.DrawRectangle(Pens.Yellow, b.MinX, b.MinY, b.Genislik, b.Yukseklik);
                    g.DrawString($"{degerYazi} ({durum})", f, Brushes.White, b.MinX, Math.Max(0, b.MinY - 14));
                }
            }

            picSonuc.Image = etiketli;
            double toplamTL = adet25 * 0.25 + adet50 * 0.50 + adet1TL * 1.00;
            rozetSonuc.MetinGuncelle($"{toplamTL:0.00} TL");

            lblOzet.Text = $"PARA VE YÜZ TESPİTİ\n\n" +
                           $"• 25 Kuruş : {adet25} adet\n" +
                           $"• 50 Kuruş : {adet50} adet\n" +
                           $"• 1 TL     : {adet1TL} adet\n" +
                           $"• TOPLAM   : {toplamTL:0.00} TL\n\n" +
                           $"• Yazı Yüzü : {yaziSay} adet\n" +
                           $"• Tura Yüzü : {turaSay} adet";
        }
    }
}
