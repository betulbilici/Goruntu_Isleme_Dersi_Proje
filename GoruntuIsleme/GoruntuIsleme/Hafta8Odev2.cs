using System;
using System.Drawing;
using System.Windows.Forms;

namespace GoruntuIsleme
{
    public class Hafta8Odev2 : Form
    {
        private PictureBox picKisi, picManzara, picSonuc;
        private ModernSurgu trkTolerans;
        private ModernHapRozet rozetKisi, rozetManzara, rozetSonuc;
        private Label lblSeciliRenk;
        private Panel pnlRenkOrnek;
        private Bitmap kisiResmi, manzaraResmi;
        private Color arkaPlanRenk = Color.FromArgb(215, 222, 228);

        public Hafta8Odev2()
        {
            Text = "Hafta 8 - Ödev 2 | Arka Plan Ayrıştırma ve Manzara Montajı";
            FormGezintiYardimcisi.FormuHazirla(this);
            ArayuzuOlustur();
            OrnekResimleriOlustur();
        }

        private void ArayuzuOlustur()
        {
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                this,
                "Hafta 8 - Ödev 2 | Arka Plan Ayrıştırma ve Manzara Montajı",
                "< Ödev 1", () => FormGezintiYardimcisi.FormGecisYap(this, new Hafta8Odev1()),
                "Ödev 3 >", () => FormGezintiYardimcisi.FormGecisYap(this, new Hafta8Odev3()));

            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 290);

            Label lblKaynak = new Label
            {
                Text = "GÖRÜNTÜ KAYNAKLARI",
                Location = new Point(18, 18),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 8.8f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi
            };

            ModernHapButon btnKisiSec = FormGezintiYardimcisi.ModernButonOlustur("Kişi (Ön Plan) Seç", true, 250, 38);
            btnKisiSec.Location = new Point(18, 44);

            ModernHapButon btnManzaraSec = FormGezintiYardimcisi.ModernButonOlustur("Manzara (Arka Plan) Seç", true, 250, 38);
            btnManzaraSec.Location = new Point(18, 90);

            ModernHapButon btnOrnek = FormGezintiYardimcisi.ModernButonOlustur("Örnek Görselleri Yükle", false, 250, 38);
            btnOrnek.Location = new Point(18, 136);

            btnKisiSec.Click += (s, e) =>
            {
                string yol = FormGezintiYardimcisi.ResimDosyasiSec("Düz Duvar Önündeki Kişi Fotoğrafı Seç");
                if (yol != null)
                {
                    using Bitmap ham = new Bitmap(yol);
                    kisiResmi = BitmapYardimcisi.BoyutSinirla(ham, 420);
                    picKisi.Image = kisiResmi;
                    ArkaPlanRengiGuncelle(kisiResmi.GetPixel(2, 2));
                    Birlestir();
                }
            };

            btnManzaraSec.Click += (s, e) =>
            {
                string yol = FormGezintiYardimcisi.ResimDosyasiSec("Arka Plan Manzara Fotoğrafı Seç");
                if (yol != null)
                {
                    using Bitmap ham = new Bitmap(yol);
                    manzaraResmi = BitmapYardimcisi.BoyutSinirla(ham, 420);
                    picManzara.Image = manzaraResmi;
                    Birlestir();
                }
            };

            btnOrnek.Click += (s, e) => OrnekResimleriOlustur();

            lblSeciliRenk = new Label
            {
                Text = "Seçili Duvar Rengi (1. Resme Tıkla):",
                Location = new Point(18, 192),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 9f),
                ForeColor = FormGezintiYardimcisi.KoyuYazi
            };
            pnlRenkOrnek = new Panel
            {
                Location = new Point(18, 216),
                Size = new Size(250, 32),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = arkaPlanRenk
            };

            ModernKartPanel kartBilgi = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                new Point(18, 266), new Size(250, 190), out Label lblBilgiSag);
            lblBilgiSag.Text = "Kullanım İpucu\n\n" +
                               "• 1. görüntüdeki duvar üzerine tıklayarak çıkarılacak arka plan rengini seçebilirsiniz.\n\n" +
                               "• 3. kartın altındaki sürgüyle renk benzerlik toleransını ayarlayabilirsiniz.";

            pnlSag.Controls.AddRange(new Control[]
            {
                lblKaynak, btnKisiSec, btnManzaraSec, btnOrnek,
                lblSeciliRenk, pnlRenkOrnek, kartBilgi
            });

            rozetKisi = new ModernHapRozet();
            rozetKisi.MetinGuncelle("Ön Plan");

            rozetManzara = new ModernHapRozet();
            rozetManzara.MetinGuncelle("Manzara");

            rozetSonuc = new ModernHapRozet();
            rozetSonuc.MetinGuncelle("Tolerans: 45");

            Panel pnlToleransAlt = FormGezintiYardimcisi.KartAltSurguPaneliOlustur(
                "Renk Benzerlik Toleransı", "5 – 150", 5, 150, 45, out trkTolerans, out _);
            trkTolerans.Scroll += (s, e) =>
            {
                rozetSonuc.MetinGuncelle($"Tolerans: {trkTolerans.Value}");
                Birlestir();
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

            picKisi = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 0, 0, "Ön Plan (Renk Seçmek İçin Tıkla)", null, null, rozetKisi);
            picManzara = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 1, 0, "Yeni Arka Plan Manzarası", null, null, rozetManzara);
            picSonuc = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 2, 0, "Birleştirilmiş Montaj", null, pnlToleransAlt, rozetSonuc);

            picKisi.Cursor = Cursors.Cross;
            picKisi.MouseClick += PicKisi_MouseClick;

            ModernKartPanel altSerit = FormGezintiYardimcisi.AltAnalizSeridiOlustur(
                "Segmentasyon Özeti: Ön plan piksellerinin seçilen duvar rengine olan RGB Öklid mesafesi hesaplanarak arka plan pikselleri manzara ile değiştirilir.",
                out _);
            tbl.Controls.Add(altSerit, 0, 1);
            tbl.SetColumnSpan(altSerit, 3);

            Controls.Add(tbl);
            FormGezintiYardimcisi.YerlesimiDuzenle(this, pnlUst, pnlSag, tbl);
        }

        private void PicKisi_MouseClick(object sender, MouseEventArgs e)
        {
            if (kisiResmi == null || picKisi.Width == 0 || picKisi.Height == 0) return;

            double oran = Math.Min((double)picKisi.Width / kisiResmi.Width, (double)picKisi.Height / kisiResmi.Height);
            int cizilenW = (int)(kisiResmi.Width * oran);
            int cizilenH = (int)(kisiResmi.Height * oran);
            int offsetX = (picKisi.Width - cizilenW) / 2;
            int offsetY = (picKisi.Height - cizilenH) / 2;

            int bmpX = (int)((e.X - offsetX) / oran);
            int bmpY = (int)((e.Y - offsetY) / oran);

            if (bmpX >= 0 && bmpX < kisiResmi.Width && bmpY >= 0 && bmpY < kisiResmi.Height)
            {
                ArkaPlanRengiGuncelle(kisiResmi.GetPixel(bmpX, bmpY));
                Birlestir();
            }
        }

        private void ArkaPlanRengiGuncelle(Color renk)
        {
            arkaPlanRenk = renk;
            pnlRenkOrnek.BackColor = renk;
            lblSeciliRenk.Text = $"Duvar Rengi: ({renk.R}, {renk.G}, {renk.B})";
        }

        private void OrnekResimleriOlustur()
        {
            Bitmap kisi = BitmapYardimcisi.OrnekGorselYukle("kisi_onplan.jpg");
            Bitmap manzara = BitmapYardimcisi.OrnekGorselYukle("manzara_arkaplan.jpg");
            if (kisi != null && manzara != null)
            {
                kisiResmi = kisi;
                manzaraResmi = manzara;
                picKisi.Image = kisiResmi;
                picManzara.Image = manzaraResmi;
                ArkaPlanRengiGuncelle(kisiResmi.GetPixel(8, 8));
                Birlestir();
                return;
            }
        }

        private void Birlestir()
        {
            if (kisiResmi == null || manzaraResmi == null) return;

            int genislik = kisiResmi.Width;
            int yukseklik = kisiResmi.Height;
            using Bitmap olcekliManzara = new Bitmap(manzaraResmi, genislik, yukseklik);
            Bitmap sonuc = new Bitmap(genislik, yukseklik);
            int tolerans = trkTolerans.Value;

            for (int x = 0; x < genislik; x++)
            {
                for (int y = 0; y < yukseklik; y++)
                {
                    Color c = kisiResmi.GetPixel(x, y);
                    double mesafe = Math.Sqrt(
                        Math.Pow(c.R - arkaPlanRenk.R, 2) +
                        Math.Pow(c.G - arkaPlanRenk.G, 2) +
                        Math.Pow(c.B - arkaPlanRenk.B, 2));

                    if (mesafe <= tolerans)
                        sonuc.SetPixel(x, y, olcekliManzara.GetPixel(x, y));
                    else
                        sonuc.SetPixel(x, y, c);
                }
            }

            picSonuc.Image = sonuc;
        }
    }
}
