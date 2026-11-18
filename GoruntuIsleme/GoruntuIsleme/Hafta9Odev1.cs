using System;
using System.Drawing;
using System.Windows.Forms;

namespace GoruntuIsleme
{
    public class Hafta9Odev1 : Form
    {
        private PictureBox picGiris, picSabitParlaklik, picNormalize;
        private ModernSurgu trkSabit, trkAltSinir;
        private ModernHapRozet rozetGiris, rozetSabit, rozetNorm;
        private Label lblGirisAralik, lblR, lblG, lblB, lblCozunurluk;
        private Bitmap orijinalResim;

        public Hafta9Odev1()
        {
            Text = "Hafta 9 - Ödev 1 | Gece Görüş Kamerası İyileştirme";
            FormGezintiYardimcisi.FormuHazirla(this);
            ArayuzuOlustur();
            OrnekKaranlikResimOlustur();
        }

        private void ArayuzuOlustur()
        {
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                this,
                "Hafta 9 - Ödev 1 | Gece Görüş Kamerası İyileştirme",
                "< Hafta 8", () => FormGezintiYardimcisi.FormGecisYap(this, new Hafta8Odev4()),
                "Ödev 2 >", () => FormGezintiYardimcisi.FormGecisYap(this, new Hafta9Odev2()));

            // SAĞ KONTROL BAR'I
            Panel pnlSag = new Panel
            {
                Dock = DockStyle.Right,
                Width = 285,
                BackColor = Color.White,
                Padding = new Padding(18)
            };
            pnlSag.Paint += (s, e) =>
            {
                using Pen solCizgi = new Pen(FormGezintiYardimcisi.KartKenar, 1.2f);
                e.Graphics.DrawLine(solCizgi, 0, 0, 0, pnlSag.Height);
            };

            Label lblKaynakBaslik = new Label
            {
                Text = "GÖRÜNTÜ KAYNAĞI",
                Location = new Point(20, 20),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 8.8f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi
            };

            ModernHapButon btnResimSec = FormGezintiYardimcisi.ModernButonOlustur("Görüntü Dosyası Seç", true, 245, 38);
            btnResimSec.Location = new Point(20, 46);

            ModernHapButon btnOrnek = FormGezintiYardimcisi.ModernButonOlustur("Örnek Görüntü Yükle", false, 245, 38);
            btnOrnek.Location = new Point(20, 94);

            btnResimSec.Click += (s, e) =>
            {
                string yol = FormGezintiYardimcisi.ResimDosyasiSec("Gece Görüş / Karanlık Fotoğraf Seç");
                if (yol != null)
                {
                    using Bitmap ham = new Bitmap(yol);
                    orijinalResim = BitmapYardimcisi.BoyutSinirla(ham, 420);
                    picGiris.Image = orijinalResim;
                    Uygula();
                }
            };
            btnOrnek.Click += (s, e) => OrnekKaranlikResimOlustur();

            // Açık Mavi Kanal Aralıkları Kartı
            ModernKartPanel kartIstatistik = new ModernKartPanel
            {
                Location = new Point(20, 152),
                Size = new Size(245, 148),
                KartArkaPlan = FormGezintiYardimcisi.AcikMaviZemin,
                KenarlikRengi = FormGezintiYardimcisi.AcikMaviKenar,
                Padding = new Padding(14)
            };

            Label lblKatBaslik = new Label
            {
                Text = "Kanal Aralıkları",
                Location = new Point(14, 14),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 9.8f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi,
                BackColor = FormGezintiYardimcisi.AcikMaviZemin
            };

            Label lRK = new Label { Text = "Kırmızı (R)", Location = new Point(14, 48), AutoSize = true, ForeColor = FormGezintiYardimcisi.AcikYazi, BackColor = FormGezintiYardimcisi.AcikMaviZemin, Font = new Font("Segoe UI", 9.2f) };
            lblR = new Label { Text = "6 – 46", Location = new Point(155, 48), Size = new Size(74, 20), TextAlign = ContentAlignment.MiddleRight, ForeColor = FormGezintiYardimcisi.KoyuYazi, BackColor = FormGezintiYardimcisi.AcikMaviZemin, Font = new Font("Consolas", 9.5f, FontStyle.Bold) };

            Label lGK = new Label { Text = "Yeşil (G)", Location = new Point(14, 78), AutoSize = true, ForeColor = FormGezintiYardimcisi.AcikYazi, BackColor = FormGezintiYardimcisi.AcikMaviZemin, Font = new Font("Segoe UI", 9.2f) };
            lblG = new Label { Text = "10 – 48", Location = new Point(155, 78), Size = new Size(74, 20), TextAlign = ContentAlignment.MiddleRight, ForeColor = FormGezintiYardimcisi.KoyuYazi, BackColor = FormGezintiYardimcisi.AcikMaviZemin, Font = new Font("Consolas", 9.5f, FontStyle.Bold) };

            Label lBK = new Label { Text = "Mavi (B)", Location = new Point(14, 108), AutoSize = true, ForeColor = FormGezintiYardimcisi.AcikYazi, BackColor = FormGezintiYardimcisi.AcikMaviZemin, Font = new Font("Segoe UI", 9.2f) };
            lblB = new Label { Text = "8 – 36", Location = new Point(155, 108), Size = new Size(74, 20), TextAlign = ContentAlignment.MiddleRight, ForeColor = FormGezintiYardimcisi.KoyuYazi, BackColor = FormGezintiYardimcisi.AcikMaviZemin, Font = new Font("Consolas", 9.5f, FontStyle.Bold) };

            kartIstatistik.Controls.AddRange(new Control[] { lblKatBaslik, lRK, lblR, lGK, lblG, lBK, lblB });

            // Alt Çözünürlük Hap Kartı
            ModernKartPanel kartCozunurluk = new ModernKartPanel
            {
                Dock = DockStyle.Bottom,
                Height = 36,
                KoseYaricapi = 18,
                KartArkaPlan = FormGezintiYardimcisi.AcikMaviZemin,
                KenarlikRengi = FormGezintiYardimcisi.AcikMaviKenar
            };
            lblCozunurluk = new Label
            {
                Text = "Çözünürlük: 320 × 260 px",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi,
                BackColor = Color.Transparent
            };
            kartCozunurluk.Controls.Add(lblCozunurluk);

            pnlSag.Controls.AddRange(new Control[] { lblKaynakBaslik, btnResimSec, btnOrnek, kartIstatistik, kartCozunurluk });
            Controls.Add(pnlSag);

            // 1. Kart Alt Bilgisi
            Panel pnlGirisAlt = new Panel { Height = 52, Padding = new Padding(2, 14, 2, 0) };
            lblGirisAralik = new Label
            {
                Text = "Parlaklık aralığı: 6 – 48",
                Dock = DockStyle.Top,
                Height = 24,
                Font = new Font("Segoe UI", 9.2f),
                ForeColor = FormGezintiYardimcisi.AcikYazi
            };
            pnlGirisAlt.Controls.Add(lblGirisAralik);

            // 2. Kart Alt Kontrolü (Modern Mavi Sürgü)
            Panel pnlSabitKontrol = new Panel { Height = 56, Padding = new Padding(2, 6, 2, 0) };
            Panel pnlSabitBaslik = new Panel { Dock = DockStyle.Top, Height = 20 };
            Label lSabitSol = new Label { Text = "Parlaklık Ekle", Location = new Point(0, 1), AutoSize = true, Font = new Font("Segoe UI", 8.8f), ForeColor = FormGezintiYardimcisi.AcikYazi };
            Label lSabitSag = new Label { Text = "0 – 200", Dock = DockStyle.Right, AutoSize = true, Font = new Font("Segoe UI", 8.8f), ForeColor = FormGezintiYardimcisi.AcikYazi };
            pnlSabitBaslik.Controls.Add(lSabitSol);
            pnlSabitBaslik.Controls.Add(lSabitSag);

            trkSabit = new ModernSurgu { Dock = DockStyle.Top, Minimum = 0, Maximum = 200, Value = 80 };
            trkSabit.Scroll += (s, e) =>
            {
                rozetSabit.MetinGuncelle($"+{trkSabit.Value}");
                Uygula();
            };
            pnlSabitKontrol.Controls.Add(trkSabit);
            pnlSabitKontrol.Controls.Add(pnlSabitBaslik);

            // 3. Kart Alt Kontrolü (Modern Mavi Sürgü)
            Panel pnlNormKontrol = new Panel { Height = 56, Padding = new Padding(2, 6, 2, 0) };
            Panel pnlNormBaslik = new Panel { Dock = DockStyle.Top, Height = 20 };
            Label lNormSol = new Label { Text = "Alt Sınır Hedefi", Location = new Point(0, 1), AutoSize = true, Font = new Font("Segoe UI", 8.8f), ForeColor = FormGezintiYardimcisi.AcikYazi };
            Label lNormSag = new Label { Text = "0 – 180", Dock = DockStyle.Right, AutoSize = true, Font = new Font("Segoe UI", 8.8f), ForeColor = FormGezintiYardimcisi.AcikYazi };
            pnlNormBaslik.Controls.Add(lNormSol);
            pnlNormBaslik.Controls.Add(lNormSag);

            trkAltSinir = new ModernSurgu { Dock = DockStyle.Top, Minimum = 0, Maximum = 180, Value = 100 };
            trkAltSinir.Scroll += (s, e) =>
            {
                rozetNorm.MetinGuncelle($"Min: {trkAltSinir.Value}");
                Uygula();
            };
            pnlNormKontrol.Controls.Add(trkAltSinir);
            pnlNormKontrol.Controls.Add(pnlNormBaslik);

            // Kart Sağ Üst Rozetleri
            rozetGiris = new ModernHapRozet();
            rozetGiris.MetinGuncelle("Giriş");

            rozetSabit = new ModernHapRozet();
            rozetSabit.MetinGuncelle("+80");

            rozetNorm = new ModernHapRozet();
            rozetNorm.MetinGuncelle("Min: 100");

            // ORTA ÇALIŞMA ALANI
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
                tbl, 0, 0, "Orijinal Görüntü", gorselZemin, pnlGirisAlt, rozetGiris);

            picSabitParlaklik = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 1, 0, "Sabit Parlaklık", Color.FromArgb(42, 52, 65), pnlSabitKontrol, rozetSabit);

            picNormalize = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 2, 0, "Dinamik Normalize", Color.FromArgb(86, 99, 112), pnlNormKontrol, rozetNorm);

            // Alt İnce Açık Mavi Analiz Şeridi
            ModernKartPanel kartAciklama = new ModernKartPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(8, 4, 8, 6),
                Padding = new Padding(16, 10, 16, 10),
                KartArkaPlan = FormGezintiYardimcisi.AcikMaviZemin,
                KenarlikRengi = FormGezintiYardimcisi.AcikMaviKenar
            };

            ModernHapRozet rozetAktif = new ModernHapRozet
            {
                Dock = DockStyle.Right,
                RozetArkaPlan = Color.White,
                RozetYaziRengi = FormGezintiYardimcisi.BirincilMavi
            };
            rozetAktif.MetinGuncelle("Aktif");

            Label lblBilgi = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent,
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi,
                Font = new Font("Segoe UI Semibold", 9.5f),
                UseMnemonic = false,
                Text = "Analiz Özeti: Dinamik normalizasyon pikselleri [Alt Sınır – 255] aralığına orantılı yayarak doyumu önler ve detay kontrastını korur."
            };
            kartAciklama.Controls.Add(lblBilgi);
            kartAciklama.Controls.Add(rozetAktif);

            tbl.Controls.Add(kartAciklama, 0, 1);
            tbl.SetColumnSpan(kartAciklama, 3);

            Controls.Add(tbl);
            FormGezintiYardimcisi.YerlesimiDuzenle(this, pnlUst, pnlSag, tbl);
        }

        private void OrnekKaranlikResimOlustur()
        {
            Bitmap yuklenen = BitmapYardimcisi.OrnekGorselYukle("gece_gorus.jpg");
            if (yuklenen != null)
            {
                orijinalResim = yuklenen;
                picGiris.Image = orijinalResim;
                Uygula();
                return;
            }
        }

        private void Uygula()
        {
            if (orijinalResim == null) return;

            int w = orijinalResim.Width;
            int h = orijinalResim.Height;
            int sabit = trkSabit.Value;
            int altSinir = trkAltSinir.Value;

            Bitmap sabitBmp = new Bitmap(w, h);
            Bitmap normBmp = new Bitmap(w, h);

            int minR = 255, maxR = 0, minG = 255, maxG = 0, minB = 255, maxB = 0;

            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    Color c = orijinalResim.GetPixel(x, y);
                    if (c.R < minR) minR = c.R; if (c.R > maxR) maxR = c.R;
                    if (c.G < minG) minG = c.G; if (c.G > maxG) maxG = c.G;
                    if (c.B < minB) minB = c.B; if (c.B > maxB) maxB = c.B;

                    int nr = Math.Min(255, c.R + sabit);
                    int ng = Math.Min(255, c.G + sabit);
                    int nb = Math.Min(255, c.B + sabit);
                    sabitBmp.SetPixel(x, y, Color.FromArgb(nr, ng, nb));
                }
            }

            int farkR = Math.Max(1, maxR - minR);
            int farkG = Math.Max(1, maxG - minG);
            int farkB = Math.Max(1, maxB - minB);

            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    Color c = orijinalResim.GetPixel(x, y);
                    int nr = altSinir + (c.R - minR) * (255 - altSinir) / farkR;
                    int ng = altSinir + (c.G - minG) * (255 - altSinir) / farkG;
                    int nb = altSinir + (c.B - minB) * (255 - altSinir) / farkB;
                    normBmp.SetPixel(x, y, Color.FromArgb(
                        Math.Clamp(nr, 0, 255),
                        Math.Clamp(ng, 0, 255),
                        Math.Clamp(nb, 0, 255)));
                }
            }

            picSabitParlaklik.Image = sabitBmp;
            picNormalize.Image = normBmp;

            int genelMin = Math.Min(minR, Math.Min(minG, minB));
            int genelMax = Math.Max(maxR, Math.Max(maxG, maxB));
            lblGirisAralik.Text = $"Parlaklık aralığı: {genelMin} – {genelMax}";
            lblR.Text = $"{minR} – {maxR}";
            lblG.Text = $"{minG} – {maxG}";
            lblB.Text = $"{minB} – {maxB}";
            lblCozunurluk.Text = $"Çözünürlük: {w} × {h} px";
        }
    }
}
