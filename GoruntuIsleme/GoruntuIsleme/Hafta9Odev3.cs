using System;
using System.Drawing;
using System.Windows.Forms;

namespace GoruntuIsleme
{
    public class Hafta9Odev3 : Form
    {
        private PictureBox picResim1, picResim2, picManuel, picOtomatik, picNormalize;
        private ModernSurgu trkOlcek1, trkOlcek2;
        private ModernHapRozet rozetR1, rozetR2, rozetManuel, rozetOto, rozetNorm;
        private Label lblYorum;
        private Bitmap resim1, resim2;

        public Hafta9Odev3()
        {
            Text = "Hafta 9 - Ödev 3 | İki Resmi Ölçekli Toplama ve Normalizasyon";
            FormGezintiYardimcisi.FormuHazirla(this);
            ArayuzuOlustur();
            OrnekResimleriOlustur();
        }

        private void ArayuzuOlustur()
        {
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                this,
                "Hafta 9 - Ödev 3 | İki Resmi Toplama (Manuel Ölçek, Otomatik Oran & Normalizasyon)",
                "< Ödev 2", () => FormGezintiYardimcisi.FormGecisYap(this, new Hafta9Odev2()),
                "Ödev 4 >", () => FormGezintiYardimcisi.FormGecisYap(this, new Hafta9Odev4()));

            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 290);

            Label lblKaynak = new Label
            {
                Text = "GÖRÜNTÜ KAYNAKLARI",
                Location = new Point(18, 18),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 8.8f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi
            };

            ModernHapButon btnSec1 = FormGezintiYardimcisi.ModernButonOlustur("1. Resim Seç (Koyu)", true, 250, 38);
            btnSec1.Location = new Point(18, 44);

            ModernHapButon btnSec2 = FormGezintiYardimcisi.ModernButonOlustur("2. Resim Seç (Açık)", true, 250, 38);
            btnSec2.Location = new Point(18, 90);

            ModernHapButon btnOrnek = FormGezintiYardimcisi.ModernButonOlustur("Örnek Görselleri Yükle", false, 250, 38);
            btnOrnek.Location = new Point(18, 136);

            btnSec1.Click += (s, e) =>
            {
                string yol = FormGezintiYardimcisi.ResimDosyasiSec("1. Resmi Seç");
                if (yol != null)
                {
                    using Bitmap ham = new Bitmap(yol);
                    resim1 = BitmapYardimcisi.BoyutSinirla(ham, 340);
                    picResim1.Image = resim1;
                    TumYontemleriHesapla();
                }
            };
            btnSec2.Click += (s, e) =>
            {
                string yol = FormGezintiYardimcisi.ResimDosyasiSec("2. Resmi Seç");
                if (yol != null)
                {
                    using Bitmap ham = new Bitmap(yol);
                    resim2 = BitmapYardimcisi.BoyutSinirla(ham, 340);
                    picResim2.Image = resim2;
                    TumYontemleriHesapla();
                }
            };
            btnOrnek.Click += (s, e) => OrnekResimleriOlustur();

            ModernKartPanel kartYorum = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                new Point(18, 192), new Size(250, 340), out lblYorum);

            pnlSag.Controls.AddRange(new Control[] { lblKaynak, btnSec1, btnSec2, btnOrnek, kartYorum });

            rozetR1 = new ModernHapRozet();
            rozetR1.MetinGuncelle("1,40×");

            rozetR2 = new ModernHapRozet();
            rozetR2.MetinGuncelle("0,60×");

            rozetManuel = new ModernHapRozet();
            rozetManuel.MetinGuncelle("Manuel");

            rozetOto = new ModernHapRozet();
            rozetOto.MetinGuncelle("Otomatik");

            rozetNorm = new ModernHapRozet();
            rozetNorm.MetinGuncelle("Normalize");

            Panel pnlR1Alt = FormGezintiYardimcisi.KartAltSurguPaneliOlustur(
                "1. Resim Çarpanı", "0,10× – 3,00×", 10, 300, 140, out trkOlcek1, out _);
            trkOlcek1.Scroll += (s, e) =>
            {
                rozetR1.MetinGuncelle($"{trkOlcek1.Value / 100.0:0.00}×");
                ManuelToplamaHesapla();
            };

            Panel pnlR2Alt = FormGezintiYardimcisi.KartAltSurguPaneliOlustur(
                "2. Resim Çarpanı", "0,10× – 3,00×", 10, 300, 60, out trkOlcek2, out _);
            trkOlcek2.Scroll += (s, e) =>
            {
                rozetR2.MetinGuncelle($"{trkOlcek2.Value / 100.0:0.00}×");
                ManuelToplamaHesapla();
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
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 52f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 48f));

            picResim1 = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 0, 0, "1. Giriş (Koyu Görüntü)", null, pnlR1Alt, rozetR1);
            picResim2 = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 1, 0, "2. Giriş (Açık Görüntü)", null, pnlR2Alt, rozetR2);
            picManuel = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 2, 0, "a) Manuel Ölçekli Toplama", null, null, rozetManuel);
            picOtomatik = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 0, 1, "b) Otomatik Ortalama Oranlı Toplama", null, null, rozetOto);
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
                Font = new Font("Segoe UI", 9.3f),
                UseMnemonic = false,
                Text = "Yöntemlerin Karşılaştırması\n\n" +
                       "• Manuel Ölçekleme (a): 1. ve 2. kartların altındaki sürgülerle her iki resmin ağırlığı elle dengelenir.\n\n" +
                       "• Otomatik Oranlama (b): Her iki görüntünün parlaklık ortalaması hesaplanarak koyu görüntü otomatik büyütülür, açık görüntü küçültülür.\n\n" +
                       "• Normalizasyon (c): 255'i aşan beyazlamaları giderir ancak açık görüntü koyu görüntüyü bastırır."
            };
            kartKarsilastirma.Controls.Add(lblKarsilastirma);
            tbl.Controls.Add(kartKarsilastirma, 2, 1);

            Controls.Add(tbl);
            FormGezintiYardimcisi.YerlesimiDuzenle(this, pnlUst, pnlSag, tbl);
        }

        private void OrnekResimleriOlustur()
        {
            Bitmap r1 = BitmapYardimcisi.OrnekGorselYukle("toplama_koyu.jpg");
            Bitmap r2 = BitmapYardimcisi.OrnekGorselYukle("toplama_acik.jpg");
            if (r1 != null && r2 != null)
            {
                resim1 = r1;
                resim2 = r2;
                picResim1.Image = resim1;
                picResim2.Image = resim2;
                TumYontemleriHesapla();
                return;
            }
        }

        private void ManuelToplamaHesapla()
        {
            if (resim1 == null || resim2 == null) return;
            int w = resim1.Width, h = resim1.Height;
            using Bitmap r2Olcekli = new Bitmap(resim2, w, h);
            Bitmap manuel = new Bitmap(w, h);

            double k1 = trkOlcek1.Value / 100.0;
            double k2 = trkOlcek2.Value / 100.0;

            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    Color c1 = resim1.GetPixel(x, y);
                    Color c2 = r2Olcekli.GetPixel(x, y);
                    int r = Math.Clamp((int)(c1.R * k1 + c2.R * k2), 0, 255);
                    int g = Math.Clamp((int)(c1.G * k1 + c2.G * k2), 0, 255);
                    int b = Math.Clamp((int)(c1.B * k1 + c2.B * k2), 0, 255);
                    manuel.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
            picManuel.Image = manuel;
        }

        private void TumYontemleriHesapla()
        {
            if (resim1 == null || resim2 == null) return;
            ManuelToplamaHesapla();

            int w = resim1.Width, h = resim1.Height;
            using Bitmap r2Olcekli = new Bitmap(resim2, w, h);

            double top1 = 0, top2 = 0;
            int pikselSayisi = w * h;

            int[,] hamR = new int[w, h];
            int[,] hamG = new int[w, h];
            int[,] hamB = new int[w, h];
            int maxHam = 1;

            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    Color c1 = resim1.GetPixel(x, y);
                    Color c2 = r2Olcekli.GetPixel(x, y);
                    top1 += (c1.R + c1.G + c1.B) / 3.0;
                    top2 += (c2.R + c2.G + c2.B) / 3.0;

                    hamR[x, y] = c1.R + c2.R;
                    hamG[x, y] = c1.G + c2.G;
                    hamB[x, y] = c1.B + c2.B;

                    if (hamR[x, y] > maxHam) maxHam = hamR[x, y];
                    if (hamG[x, y] > maxHam) maxHam = hamG[x, y];
                    if (hamB[x, y] > maxHam) maxHam = hamB[x, y];
                }
            }

            double ort1 = Math.Max(1.0, top1 / pikselSayisi);
            double ort2 = Math.Max(1.0, top2 / pikselSayisi);

            double otoK1 = 127.5 / ort1;
            double otoK2 = 127.5 / ort2;
            rozetOto.MetinGuncelle($"{otoK1:0.0}× / {otoK2:0.0}×");

            Bitmap otoBmp = new Bitmap(w, h);
            Bitmap normBmp = new Bitmap(w, h);

            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    Color c1 = resim1.GetPixel(x, y);
                    Color c2 = r2Olcekli.GetPixel(x, y);

                    int orR = Math.Clamp((int)(c1.R * otoK1 + c2.R * otoK2), 0, 255);
                    int orG = Math.Clamp((int)(c1.G * otoK1 + c2.G * otoK2), 0, 255);
                    int orB = Math.Clamp((int)(c1.B * otoK1 + c2.B * otoK2), 0, 255);
                    otoBmp.SetPixel(x, y, Color.FromArgb(orR, orG, orB));

                    int nr = hamR[x, y] * 255 / maxHam;
                    int ng = hamG[x, y] * 255 / maxHam;
                    int nb = hamB[x, y] * 255 / maxHam;
                    normBmp.SetPixel(x, y, Color.FromArgb(nr, ng, nb));
                }
            }

            picOtomatik.Image = otoBmp;
            picNormalize.Image = normBmp;

            lblYorum.Text = $"ÖLÇEK ANALİZİ\n\n" +
                            $"• 1. Görüntü Ortalaması : {ort1:0.0}\n" +
                            $"• 2. Görüntü Ortalaması : {ort2:0.0}\n" +
                            $"• Otomatik 1. Çarpan    : {otoK1:0.00}×\n" +
                            $"• Otomatik 2. Çarpan    : {otoK2:0.00}×\n" +
                            $"• Ham Toplam Tepe Değer : {maxHam}\n\n" +
                            $"Otomatik ortalama oranlama, her iki resmi de ~127.5 parlaklık katkısıyla birleştirerek en dengeli sonucu üretir.";
        }
    }
}
