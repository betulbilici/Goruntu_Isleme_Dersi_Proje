using System;
using System.Drawing;
using System.Windows.Forms;

namespace GoruntuIsleme
{
    public class Hafta9Odev4 : Form
    {
        private PictureBox picBantBos, picBantUrun, picFark, picIkiliSonuc;
        private ModernSurgu trkEsik;
        private ModernHapRozet rozetBos, rozetUrun, rozetFark, rozetMaske;
        private CheckBox chkIsikDengele;
        private Bitmap bosBant, urunluBant;

        public Hafta9Odev4()
        {
            Text = "Hafta 9 - Ödev 4 | Değişken Işıklı Yürüyen Bantta Ürün Ayırma";
            FormGezintiYardimcisi.FormuHazirla(this);
            ArayuzuOlustur();
            OrnekBantResimleriOlustur();
        }

        private void ArayuzuOlustur()
        {
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                this,
                "Hafta 9 - Ödev 4 | Değişken Işıklı Yürüyen Bantta Ürün Ayırma (İkili Siyah-Beyaz Maske)",
                "< Ödev 3", () => FormGezintiYardimcisi.FormGecisYap(this, new Hafta9Odev3()),
                "Ödev 5 >", () => FormGezintiYardimcisi.FormGecisYap(this, new Hafta9Odev5()));

            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 290);

            Label lblKaynak = new Label
            {
                Text = "GÖRÜNTÜ KAYNAKLARI",
                Location = new Point(18, 18),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 8.8f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi
            };

            ModernHapButon btnBosSec = FormGezintiYardimcisi.ModernButonOlustur("Boş Gölgeli Bant Seç", true, 250, 38);
            btnBosSec.Location = new Point(18, 44);

            ModernHapButon btnUrunSec = FormGezintiYardimcisi.ModernButonOlustur("Ürünlü Bant Seç", true, 250, 38);
            btnUrunSec.Location = new Point(18, 90);

            ModernHapButon btnOrnek = FormGezintiYardimcisi.ModernButonOlustur("Örnek Bant & Ürün Yükle", false, 250, 38);
            btnOrnek.Location = new Point(18, 136);

            btnBosSec.Click += (s, e) =>
            {
                string yol = FormGezintiYardimcisi.ResimDosyasiSec("Gölgeli Boş Arka Plan (Bant/Kağıt) Seç");
                if (yol != null)
                {
                    using Bitmap ham = new Bitmap(yol);
                    bosBant = BitmapYardimcisi.BoyutSinirla(ham, 380);
                    picBantBos.Image = bosBant;
                    Uygula();
                }
            };
            btnUrunSec.Click += (s, e) =>
            {
                string yol = FormGezintiYardimcisi.ResimDosyasiSec("Üzerinde Ürün Bulunan Bant/Kağıt Seç");
                if (yol != null)
                {
                    using Bitmap ham = new Bitmap(yol);
                    urunluBant = BitmapYardimcisi.BoyutSinirla(ham, 380);
                    picBantUrun.Image = urunluBant;
                    Uygula();
                }
            };
            btnOrnek.Click += (s, e) => OrnekBantResimleriOlustur();

            chkIsikDengele = new CheckBox
            {
                Text = "Ortam Işığı Değişimini Dengele\n(Ortalama Parlaklık Farkını Çıkar)",
                Location = new Point(18, 190),
                Size = new Size(250, 42),
                Font = new Font("Segoe UI", 9f),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                Checked = true
            };
            chkIsikDengele.CheckedChanged += (s, e) => Uygula();

            ModernKartPanel kartBilgi = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                new Point(18, 246), new Size(250, 240), out Label lblBilgi);
            lblBilgi.Text = "Çalışma Mantığı\n\n" +
                            "1. Yan ışıktan oluşan gölgeli boş bant görüntüsü referans alınır.\n\n" +
                            "2. Ürün geldiğinde ortamdaki genel aydınlatma değişimi ortalama farkla dengelenir ve arka plan çıkarılır.\n\n" +
                            "3. 4. kartın altındaki eşik sürgüsüyle gölgeler tamamen silinip net Siyah-Beyaz ürün silüeti üretilir.";

            pnlSag.Controls.AddRange(new Control[]
            {
                lblKaynak, btnBosSec, btnUrunSec, btnOrnek, chkIsikDengele, kartBilgi
            });

            rozetBos = new ModernHapRozet();
            rozetBos.MetinGuncelle("Referans");

            rozetUrun = new ModernHapRozet();
            rozetUrun.MetinGuncelle("Ürünlü");

            rozetFark = new ModernHapRozet();
            rozetFark.MetinGuncelle("Fark");

            rozetMaske = new ModernHapRozet();
            rozetMaske.MetinGuncelle("Eşik: 24");

            Panel pnlMaskeAlt = FormGezintiYardimcisi.KartAltSurguPaneliOlustur(
                "Ayırma Eşik Değeri", "5 – 120", 5, 120, 24, out trkEsik, out _);
            trkEsik.Scroll += (s, e) =>
            {
                rozetMaske.MetinGuncelle($"Eşik: {trkEsik.Value}");
                Uygula();
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

            picBantBos = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 0, 0, "Gölgeli Boş Bant (Referans)", null, null, rozetBos);
            picBantUrun = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 1, 0, "Ürünlü Bant (Değişken Işıklı)", null, null, rozetUrun);
            picFark = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 0, 1, "Aydınlatma Dengeli Fark Görüntüsü", null, null, rozetFark);
            picIkiliSonuc = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 1, 1, "İkili (Siyah-Beyaz) Ürün Maskesi", null, pnlMaskeAlt, rozetMaske);

            Controls.Add(tbl);
            FormGezintiYardimcisi.YerlesimiDuzenle(this, pnlUst, pnlSag, tbl);
        }

        private void OrnekBantResimleriOlustur()
        {
            Bitmap bos = BitmapYardimcisi.OrnekGorselYukle("bant_bos.jpg");
            Bitmap urun = BitmapYardimcisi.OrnekGorselYukle("bant_kasik.jpg");
            if (bos != null && urun != null)
            {
                bosBant = bos;
                urunluBant = urun;
                picBantBos.Image = bosBant;
                picBantUrun.Image = urunluBant;
                Uygula();
                return;
            }
        }

        private void Uygula()
        {
            if (bosBant == null || urunluBant == null) return;

            int w = bosBant.Width, h = bosBant.Height;
            using Bitmap urunOlcekli = new Bitmap(urunluBant, w, h);

            int isikFarki = 0;
            if (chkIsikDengele.Checked)
            {
                long topBos = 0, topUrun = 0;
                int sayac = 0;
                for (int x = 0; x < w; x += 2)
                {
                    for (int y = 0; y < h; y += 2)
                    {
                        Color cb = bosBant.GetPixel(x, y);
                        Color cu = urunOlcekli.GetPixel(x, y);
                        topBos += (cb.R + cb.G + cb.B) / 3;
                        topUrun += (cu.R + cu.G + cu.B) / 3;
                        sayac++;
                    }
                }
                isikFarki = (int)((topUrun - topBos) / Math.Max(1, sayac));
            }

            Bitmap farkBmp = new Bitmap(w, h);
            Bitmap ikiliBmp = new Bitmap(w, h);
            int esik = trkEsik.Value;

            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    Color cb = bosBant.GetPixel(x, y);
                    Color cu = urunOlcekli.GetPixel(x, y);

                    int gBos = (cb.R + cb.G + cb.B) / 3;
                    int gUrun = Math.Clamp(((cu.R + cu.G + cu.B) / 3) - isikFarki, 0, 255);

                    int fark = Math.Abs(gUrun - gBos);
                    farkBmp.SetPixel(x, y, Color.FromArgb(fark, fark, fark));
                    ikiliBmp.SetPixel(x, y, fark >= esik ? Color.White : Color.Black);
                }
            }

            picFark.Image = farkBmp;
            picIkiliSonuc.Image = ikiliBmp;
        }
    }
}
