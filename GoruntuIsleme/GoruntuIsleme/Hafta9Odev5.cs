using System;
using System.Drawing;
using System.Windows.Forms;

namespace GoruntuIsleme
{
    public class Hafta9Odev5 : Form
    {
        private PictureBox picKare1, picKare2, picFark, picNisan;
        private ModernSurgu trkEsik, trkHedefX, trkHedefY;
        private ModernHapRozet rozetK1, rozetK2, rozetFark, rozetNisan;
        private Label lblHesap;
        private NumericUpDown numFovYatay, numFovDikey;
        private Bitmap kare1, kare2;

        public Hafta9Odev5()
        {
            Text = "Hafta 9 - Ödev 5 | Hareket Sensörü ve Kamera Ekseni Dönüş Hesabı";
            FormGezintiYardimcisi.FormuHazirla(this);
            ArayuzuOlustur();
            OrnekKareleriOlustur();
        }

        private void ArayuzuOlustur()
        {
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                this,
                "Hafta 9 - Ödev 5 | Hareket Sensörü ile Hedef Merkezi ve Kamera Ekseni Dönme Hesabı",
                "< Ödev 4", () => FormGezintiYardimcisi.FormGecisYap(this, new Hafta9Odev4()),
                "Ödev 6 >", () => FormGezintiYardimcisi.FormGecisYap(this, new Hafta9Odev6()));

            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 295);

            Label lblKaynak = new Label
            {
                Text = "GÖRÜNTÜ VE KAMERA AYARLARI",
                Location = new Point(18, 16),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 8.8f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi
            };

            ModernHapButon btnKare1Sec = FormGezintiYardimcisi.ModernButonOlustur("1. Kareyi Seç (Referans)", true, 255, 36);
            btnKare1Sec.Location = new Point(18, 40);

            ModernHapButon btnKare2Sec = FormGezintiYardimcisi.ModernButonOlustur("2. Kareyi Seç (Hareketli)", true, 255, 36);
            btnKare2Sec.Location = new Point(18, 84);

            ModernHapButon btnOrnek = FormGezintiYardimcisi.ModernButonOlustur("Örnek Simülasyonu Sıfırla", false, 255, 34);
            btnOrnek.Location = new Point(18, 126);

            btnKare1Sec.Click += (s, e) =>
            {
                string yol = FormGezintiYardimcisi.ResimDosyasiSec("1. Kareyi (Referans) Seç");
                if (yol != null)
                {
                    using Bitmap ham = new Bitmap(yol);
                    kare1 = BitmapYardimcisi.BoyutSinirla(ham, 380);
                    picKare1.Image = kare1;
                    HedefiBulVeHesapla();
                }
            };
            btnKare2Sec.Click += (s, e) =>
            {
                string yol = FormGezintiYardimcisi.ResimDosyasiSec("2. Kareyi (Hareketli) Seç");
                if (yol != null)
                {
                    using Bitmap ham = new Bitmap(yol);
                    kare2 = BitmapYardimcisi.BoyutSinirla(ham, 380);
                    picKare2.Image = kare2;
                    HedefiBulVeHesapla();
                }
            };
            btnOrnek.Click += (s, e) => OrnekKareleriOlustur();

            Label lblHedefY = new Label { Text = "Simülasyon Hedef Dikey Konum (Y):", Location = new Point(18, 174), AutoSize = true, Font = new Font("Segoe UI", 8.8f), ForeColor = FormGezintiYardimcisi.AcikYazi };
            trkHedefY = new ModernSurgu { Location = new Point(18, 194), Width = 255, Minimum = 25, Maximum = 190, Value = 65 };
            trkHedefY.Scroll += (s, e) => OrnekKareleriOlustur();

            Label lblFov = new Label { Text = "Kamera Görüş Açısı (Yatay / Dikey FOV °):", Location = new Point(18, 232), AutoSize = true, Font = new Font("Segoe UI", 8.8f), ForeColor = FormGezintiYardimcisi.AcikYazi };
            numFovYatay = new NumericUpDown { Location = new Point(18, 254), Width = 122, Minimum = 10, Maximum = 150, Value = 60 };
            numFovDikey = new NumericUpDown { Location = new Point(150, 254), Width = 122, Minimum = 10, Maximum = 120, Value = 45 };
            numFovYatay.ValueChanged += (s, e) => HedefiBulVeHesapla();
            numFovDikey.ValueChanged += (s, e) => HedefiBulVeHesapla();

            ModernKartPanel kartHesap = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                new Point(18, 294), new Size(255, 250), out lblHesap);

            pnlSag.Controls.AddRange(new Control[]
            {
                lblKaynak, btnKare1Sec, btnKare2Sec, btnOrnek,
                lblHedefY, trkHedefY, lblFov, numFovYatay, numFovDikey, kartHesap
            });

            rozetK1 = new ModernHapRozet();
            rozetK1.MetinGuncelle("Sabit");

            rozetK2 = new ModernHapRozet();
            rozetK2.MetinGuncelle("X: 235");

            rozetFark = new ModernHapRozet();
            rozetFark.MetinGuncelle("Eşik: 30");

            rozetNisan = new ModernHapRozet();
            rozetNisan.MetinGuncelle("Aktif");

            Panel pnlK2Alt = FormGezintiYardimcisi.KartAltSurguPaneliOlustur(
                "Simülasyon Hedef Yatay Konumu (X)", "25 – 290 px", 25, 290, 235, out trkHedefX, out _);
            trkHedefX.Scroll += (s, e) =>
            {
                rozetK2.MetinGuncelle($"X: {trkHedefX.Value}");
                OrnekKareleriOlustur();
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
                RowCount = 2,
                BackColor = FormGezintiYardimcisi.TemaZemin
            };
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));

            picKare1 = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 0, 0, "1. Kare (Sabit Arka Plan)", null, null, rozetK1);
            picKare2 = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 1, 0, "2. Kare (Hareket Eden Hedef)", null, pnlK2Alt, rozetK2);
            picFark = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 0, 1, "İkili Hareket Fark Maskesi", null, pnlFarkAlt, rozetFark);
            picNisan = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 1, 1, "Kamera Ekseni (+) & Hedef Takip Vektörü", null, null, rozetNisan);

            Controls.Add(tbl);
            FormGezintiYardimcisi.YerlesimiDuzenle(this, pnlUst, pnlSag, tbl);
        }

        private void OrnekKareleriOlustur()
        {
            int w = 320, h = 220;
            using Bitmap arkaKaynak = BitmapYardimcisi.OrnekGorselYukle("manzara_arkaplan.jpg");

            Action<Bitmap, bool, int, int> sahneCiz = (bmp, hedefVarMi, hx, hy) =>
            {
                using Graphics g = Graphics.FromImage(bmp);
                if (arkaKaynak != null)
                {
                    g.DrawImage(arkaKaynak, 0, 0, w, h);
                }
                else
                {
                    g.Clear(Color.FromArgb(135, 185, 225));
                    g.FillRectangle(new SolidBrush(Color.FromArgb(76, 135, 72)), 0, 140, w, 80);
                }

                if (hedefVarMi)
                {
                    g.FillEllipse(new SolidBrush(Color.FromArgb(220, 38, 38)), hx - 16, hy - 16, 32, 32);
                    g.DrawEllipse(new Pen(Color.White, 2), hx - 16, hy - 16, 32, 32);
                }
            };

            kare1 = new Bitmap(w, h);
            kare2 = new Bitmap(w, h);
            sahneCiz(kare1, false, 0, 0);
            sahneCiz(kare2, true, trkHedefX.Value, trkHedefY.Value);

            picKare1.Image = kare1;
            picKare2.Image = kare2;
            HedefiBulVeHesapla();
        }

        private void HedefiBulVeHesapla()
        {
            if (kare1 == null || kare2 == null) return;

            int w = kare1.Width, h = kare1.Height;
            using Bitmap k2Olcekli = new Bitmap(kare2, w, h);
            Bitmap farkMaske = new Bitmap(w, h);
            Bitmap nisanBmp = new Bitmap(k2Olcekli);

            int esik = trkEsik.Value;
            long toplamX = 0, toplamY = 0, adet = 0;

            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    Color c1 = kare1.GetPixel(x, y);
                    Color c2 = k2Olcekli.GetPixel(x, y);
                    int fark = (Math.Abs(c1.R - c2.R) + Math.Abs(c1.G - c2.G) + Math.Abs(c1.B - c2.B)) / 3;

                    if (fark >= esik)
                    {
                        farkMaske.SetPixel(x, y, Color.White);
                        toplamX += x;
                        toplamY += y;
                        adet++;
                    }
                    else
                    {
                        farkMaske.SetPixel(x, y, Color.Black);
                    }
                }
            }

            picFark.Image = farkMaske;

            int camMerkezX = w / 2;
            int camMerkezY = h / 2;

            using (Graphics g = Graphics.FromImage(nisanBmp))
            {
                using Pen camPen = new Pen(Color.DeepSkyBlue, 2);
                g.DrawLine(camPen, camMerkezX - 14, camMerkezY, camMerkezX + 14, camMerkezY);
                g.DrawLine(camPen, camMerkezX, camMerkezY - 14, camMerkezX, camMerkezY + 14);

                if (adet > 5)
                {
                    int hedefX = (int)(toplamX / adet);
                    int hedefY = (int)(toplamY / adet);

                    using Pen okPen = new Pen(Color.Yellow, 2);
                    g.DrawLine(okPen, camMerkezX, camMerkezY, hedefX, hedefY);

                    using Pen hedefPen = new Pen(Color.Lime, 2.5f);
                    g.DrawLine(hedefPen, hedefX - 12, hedefY, hedefX + 12, hedefY);
                    g.DrawLine(hedefPen, hedefX, hedefY - 12, hedefX, hedefY + 12);
                    g.DrawEllipse(hedefPen, hedefX - 18, hedefY - 18, 36, 36);

                    int dx = hedefX - camMerkezX;
                    int dy = hedefY - camMerkezY;

                    double aciX = dx * ((double)numFovYatay.Value / w);
                    double aciY = -dy * ((double)numFovDikey.Value / h);

                    string yonX = dx >= 0 ? "Sağa" : "Sola";
                    string yonY = dy >= 0 ? "Aşağı" : "Yukarı";

                    rozetNisan.MetinGuncelle($"{aciX:+0.0;-0.0;0.0}° / {aciY:+0.0;-0.0;0.0}°");

                    lblHesap.Text = $"HAREKET VE EKSEN HESABI\n\n" +
                                    $"• Kamera Merkezi: ({camMerkezX}, {camMerkezY})\n" +
                                    $"• Hedef Merkezi : ({hedefX}, {hedefY})\n" +
                                    $"• Hareket Alanı : {adet} piksel\n\n" +
                                    $"DÖNÜŞ MİKTARI\n" +
                                    $"• Yatay (dx) : {Math.Abs(dx)} px ({yonX})\n" +
                                    $"• Dikey (dy) : {Math.Abs(dy)} px ({yonY})\n" +
                                    $"• Yatay Açı  : {aciX:+0.0;-0.0;0.0}°\n" +
                                    $"• Dikey Açı  : {aciY:+0.0;-0.0;0.0}°";
                }
                else
                {
                    lblHesap.Text = "Hareket eden bir hedef tespit edilemedi.";
                }
            }

            picNisan.Image = nisanBmp;
        }
    }
}
