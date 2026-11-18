using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GoruntuIsleme
{
    public class Hafta9Odev6 : Form
    {
        private PictureBox picKare1, picKare2, picMaske, picTakip;
        private ModernSurgu trkEsik, trkMinAlan;
        private ModernHapRozet rozetK1, rozetK2, rozetMaske, rozetTakip;
        private Label lblSonuc;
        private ModernHapButon btnSimulasyon;
        private System.Windows.Forms.Timer animTimer;
        private Bitmap kare1, kare2;
        private int animAdim = 0;

        public Hafta9Odev6()
        {
            Text = "Hafta 9 - Ödev 6 | Ağaç ve İnsan Ayrımı ile Artikel (+) Takibi";
            FormGezintiYardimcisi.FormuHazirla(this);
            ArayuzuOlustur();

            animTimer = new System.Windows.Forms.Timer { Interval = 250 };
            animTimer.Tick += (s, e) =>
            {
                animAdim = (animAdim + 1) % 24;
                SimulasyonKaresiUret(animAdim);
            };
            FormClosing += (s, e) => animTimer?.Stop();

            SimulasyonKaresiUret(3);
        }

        private void ArayuzuOlustur()
        {
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                this,
                "Hafta 9 - Ödev 6 | Sallanan Ağaç ve Yürüyen İnsan Ayrımı ile Artikel (+) Takibi",
                "< Ödev 5", () => { animTimer.Stop(); FormGezintiYardimcisi.FormGecisYap(this, new Hafta9Odev5()); },
                "Ana Menü >", () => { animTimer.Stop(); FormGezintiYardimcisi.AnaFormaDon(this); },
                sonrakiYesil: false);

            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 295);

            Label lblKaynak = new Label
            {
                Text = "DENEY ORTAMI VE KAYNAKLAR",
                Location = new Point(18, 16),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 8.8f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi
            };

            btnSimulasyon = FormGezintiYardimcisi.ModernButonOlustur("Canlı Deney Ortamını Başlat", true, 255, 38);
            btnSimulasyon.Location = new Point(18, 42);
            btnSimulasyon.Click += (s, e) =>
            {
                if (animTimer.Enabled)
                {
                    animTimer.Stop();
                    btnSimulasyon.Text = "Canlı Deney Ortamını Başlat";
                    btnSimulasyon.Invalidate();
                }
                else
                {
                    animTimer.Start();
                    btnSimulasyon.Text = "Deney Ortamını Durdur";
                    btnSimulasyon.Invalidate();
                }
            };

            ModernHapButon btnKare1Sec = FormGezintiYardimcisi.ModernButonOlustur("1. Kareyi Dosyadan Seç", false, 255, 36);
            btnKare1Sec.Location = new Point(18, 88);

            ModernHapButon btnKare2Sec = FormGezintiYardimcisi.ModernButonOlustur("2. Kareyi Dosyadan Seç", false, 255, 36);
            btnKare2Sec.Location = new Point(18, 132);

            btnKare1Sec.Click += (s, e) =>
            {
                animTimer.Stop();
                btnSimulasyon.Text = "Canlı Deney Ortamını Başlat";
                btnSimulasyon.Invalidate();
                string yol = FormGezintiYardimcisi.ResimDosyasiSec("1. Kareyi Seç");
                if (yol != null)
                {
                    using Bitmap ham = new Bitmap(yol);
                    kare1 = BitmapYardimcisi.BoyutSinirla(ham, 340);
                    picKare1.Image = kare1;
                    AgacVeInsanAyirtEt();
                }
            };
            btnKare2Sec.Click += (s, e) =>
            {
                animTimer.Stop();
                btnSimulasyon.Text = "Canlı Deney Ortamını Başlat";
                btnSimulasyon.Invalidate();
                string yol = FormGezintiYardimcisi.ResimDosyasiSec("2. Kareyi Seç");
                if (yol != null)
                {
                    using Bitmap ham = new Bitmap(yol);
                    kare2 = BitmapYardimcisi.BoyutSinirla(ham, 340);
                    picKare2.Image = kare2;
                    AgacVeInsanAyirtEt();
                }
            };

            ModernKartPanel kartSonuc = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                new Point(18, 186), new Size(255, 340), out lblSonuc);

            pnlSag.Controls.AddRange(new Control[]
            {
                lblKaynak, btnSimulasyon, btnKare1Sec, btnKare2Sec, kartSonuc
            });

            rozetK1 = new ModernHapRozet();
            rozetK1.MetinGuncelle("t - 1");

            rozetK2 = new ModernHapRozet();
            rozetK2.MetinGuncelle("Anlık (t)");

            rozetMaske = new ModernHapRozet();
            rozetMaske.MetinGuncelle("Eşik: 28");

            rozetTakip = new ModernHapRozet();
            rozetTakip.MetinGuncelle("Min: 220 px");

            Panel pnlMaskeAlt = FormGezintiYardimcisi.KartAltSurguPaneliOlustur(
                "Hareket Fark Eşiği", "10 – 100", 10, 100, 28, out trkEsik, out _);
            trkEsik.Scroll += (s, e) =>
            {
                rozetMaske.MetinGuncelle($"Eşik: {trkEsik.Value}");
                AgacVeInsanAyirtEt();
            };

            Panel pnlTakipAlt = FormGezintiYardimcisi.KartAltSurguPaneliOlustur(
                "Min İnsan Kütle Alanı", "50 – 1500 px", 50, 1500, 220, out trkMinAlan, out _);
            trkMinAlan.Scroll += (s, e) =>
            {
                rozetTakip.MetinGuncelle($"Min: {trkMinAlan.Value} px");
                AgacVeInsanAyirtEt();
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
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 48f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 52f));

            picKare1 = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 0, 0, "1. Kare (Önceki Zaman Adımı)", null, null, rozetK1);
            picKare2 = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 1, 0, "2. Kare (Sallanan Ağaç & Yürüyen İnsan)", null, null, rozetK2);
            picMaske = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 0, 1, "Fark & Bölge Etiketleme", null, pnlMaskeAlt, rozetMaske);
            picTakip = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 1, 1, "Hedef Sınıflandırma ve Artikel (+) Takibi", null, pnlTakipAlt, rozetTakip);

            Controls.Add(tbl);
            FormGezintiYardimcisi.YerlesimiDuzenle(this, pnlUst, pnlSag, tbl);
        }

        private Bitmap SahneCiz(int adim, bool oncekiKare)
        {
            int w = 280, h = 190;
            Bitmap bmp = new Bitmap(w, h);
            using Graphics g = Graphics.FromImage(bmp);

            using Bitmap arkaKaynak = BitmapYardimcisi.OrnekGorselYukle("manzara_arkaplan.jpg");
            if (arkaKaynak != null)
            {
                g.DrawImage(arkaKaynak, 0, 0, w, h);
            }
            else
            {
                g.Clear(Color.FromArgb(145, 195, 235));
                g.FillRectangle(new SolidBrush(Color.FromArgb(80, 145, 78)), 0, 125, w, 65);
            }

            int salinim = oncekiKare ? -4 : 4;
            g.FillRectangle(new SolidBrush(Color.FromArgb(100, 68, 40)), 52, 85, 14, 52);
            g.FillEllipse(new SolidBrush(Color.FromArgb(30, 95, 34)), 26 + salinim, 38, 42, 36);
            g.FillEllipse(new SolidBrush(Color.FromArgb(36, 110, 40)), 48 - salinim, 32, 38, 38);
            g.FillEllipse(new SolidBrush(Color.FromArgb(25, 84, 30)), 42 + salinim / 2, 54, 44, 34);

            int insanX = 110 + adim * 6;
            if (oncekiKare) insanX -= 12;
            insanX = Math.Clamp(insanX, 95, w - 35);

            g.FillEllipse(new SolidBrush(Color.FromArgb(235, 190, 160)), insanX + 3, 76, 14, 14);
            g.FillRectangle(new SolidBrush(Color.FromArgb(220, 38, 38)), insanX, 91, 20, 28);
            g.FillRectangle(new SolidBrush(Color.FromArgb(30, 41, 59)), insanX + 2, 119, 7, 22);
            g.FillRectangle(new SolidBrush(Color.FromArgb(30, 41, 59)), insanX + 11, 119, 7, 22);

            return bmp;
        }

        private void SimulasyonKaresiUret(int adim)
        {
            kare1 = SahneCiz(adim, true);
            kare2 = SahneCiz(adim, false);
            picKare1.Image = kare1;
            picKare2.Image = kare2;
            AgacVeInsanAyirtEt();
        }

        private void AgacVeInsanAyirtEt()
        {
            if (kare1 == null || kare2 == null) return;

            int w = kare1.Width, h = kare1.Height;
            using Bitmap k2Olcekli = new Bitmap(kare2, w, h);
            Bitmap farkIkili = new Bitmap(w, h);
            int esik = trkEsik.Value;

            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    Color c1 = kare1.GetPixel(x, y);
                    Color c2 = k2Olcekli.GetPixel(x, y);
                    int fark = (Math.Abs(c1.R - c2.R) + Math.Abs(c1.G - c2.G) + Math.Abs(c1.B - c2.B)) / 3;
                    farkIkili.SetPixel(x, y, fark >= esik ? Color.White : Color.Black);
                }
            }

            Bitmap bolgeli = BitmapYardimcisi.BolgeBulVeRenklendir(farkIkili, 25, out var bolgeler, out _);
            picMaske.Image = bolgeli;

            Bitmap takipBmp = new Bitmap(k2Olcekli);
            int insanSay = 0;
            int agacYaprakSay = 0;

            using (Graphics g = Graphics.FromImage(takipBmp))
            using (Font f = new Font("Segoe UI", 8.5f, FontStyle.Bold))
            {
                foreach (var b in bolgeler)
                {
                    double enBoyOrani = (double)b.Yukseklik / Math.Max(1, b.Genislik);
                    double doluluk = (double)b.PikselSayisi / Math.Max(1, b.Genislik * b.Yukseklik);

                    bool insanMi = b.PikselSayisi >= trkMinAlan.Value && enBoyOrani >= 1.1 && doluluk >= 0.30;

                    if (insanMi)
                    {
                        insanSay++;
                        int mx = (int)b.MerkezX;
                        int my = (int)b.MerkezY;

                        using Pen pInsan = new Pen(Color.Red, 2.5f);
                        g.DrawRectangle(pInsan, b.MinX, b.MinY, b.Genislik, b.Yukseklik);
                        g.DrawLine(pInsan, mx - 12, my, mx + 12, my);
                        g.DrawLine(pInsan, mx, my - 12, mx, my + 12);
                        g.DrawString("İNSAN (+)", f, Brushes.Red, b.MinX, Math.Max(0, b.MinY - 15));
                    }
                    else
                    {
                        agacYaprakSay++;
                        using Pen pAgac = new Pen(Color.Orange, 1);
                        g.DrawRectangle(pAgac, b.MinX, b.MinY, b.Genislik, b.Yukseklik);
                    }
                }
            }

            picTakip.Image = takipBmp;

            lblSonuc.Text = $"SINIFLANDIRMA VE TAKİP\n\n" +
                            $"• Tespit Edilen İnsan : {insanSay} (Kırmızı +)\n" +
                            $"• Elenen Yaprak/Dal   : {agacYaprakSay} parça\n\n" +
                            $"Ayırt Etme Kriterleri:\n" +
                            $"1. Kütle Alanı: Sallanan yapraklar küçük ve parçalı bölgeler oluşturur.\n\n" +
                            $"2. En-Boy Oranı: İnsan silüeti dikey (Yükseklik / Genişlik >= 1.1) ve bütünleşik kütledir.\n\n" +
                            $"3. Artikel (+): Yalnızca insan kriterini sağlayan hedefin ağırlık merkezine yerleştirilir.";
        }
    }
}
