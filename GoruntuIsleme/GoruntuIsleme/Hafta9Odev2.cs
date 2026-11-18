using System;
using System.Drawing;
using System.Windows.Forms;

namespace GoruntuIsleme
{
    public class Hafta9Odev2 : Form
    {
        private PictureBox picBelge, picArkaPlan, picFark, picSonuc;
        private ModernSurgu trkEsik;
        private ModernHapRozet rozetBelge, rozetArka, rozetFark, rozetSonuc;
        private CheckBox chkOtomatikArkaPlan;
        private Bitmap belgeResmi, arkaPlanResmi;

        public Hafta9Odev2()
        {
            Text = "Hafta 9 - Ödev 2 | Arka Plan Çıkarma ile Gölgeli Belge Okuma";
            FormGezintiYardimcisi.FormuHazirla(this);
            ArayuzuOlustur();
            OrnekGolgeliBelgeOlustur();
        }

        private void ArayuzuOlustur()
        {
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                this,
                "Hafta 9 - Ödev 2 | Gölgeli Kağıttan Arka Plan Çıkarma ile Yazı Okuma",
                "< Ödev 1", () => FormGezintiYardimcisi.FormGecisYap(this, new Hafta9Odev1()),
                "Ödev 3 >", () => FormGezintiYardimcisi.FormGecisYap(this, new Hafta9Odev3()));

            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 290);

            Label lblKaynak = new Label
            {
                Text = "GÖRÜNTÜ KAYNAKLARI",
                Location = new Point(18, 18),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 8.8f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi
            };

            ModernHapButon btnBelgeSec = FormGezintiYardimcisi.ModernButonOlustur("Gölgeli Belge Seç", true, 250, 38);
            btnBelgeSec.Location = new Point(18, 44);

            ModernHapButon btnArkaPlanSec = FormGezintiYardimcisi.ModernButonOlustur("Boş Gölgeli Arka Plan Seç", true, 250, 38);
            btnArkaPlanSec.Location = new Point(18, 90);

            ModernHapButon btnOrnek = FormGezintiYardimcisi.ModernButonOlustur("Örnek Gölgeli Belge Yükle", false, 250, 38);
            btnOrnek.Location = new Point(18, 136);

            btnBelgeSec.Click += (s, e) =>
            {
                string yol = FormGezintiYardimcisi.ResimDosyasiSec("Gölgeli Yazılı Belge Seç");
                if (yol != null)
                {
                    using Bitmap ham = new Bitmap(yol);
                    belgeResmi = BitmapYardimcisi.BoyutSinirla(ham, 380);
                    picBelge.Image = belgeResmi;
                    Uygula();
                }
            };
            btnArkaPlanSec.Click += (s, e) =>
            {
                string yol = FormGezintiYardimcisi.ResimDosyasiSec("Aynı Işıktaki Boş Kağıt (Arka Plan) Seç");
                if (yol != null)
                {
                    using Bitmap ham = new Bitmap(yol);
                    arkaPlanResmi = BitmapYardimcisi.BoyutSinirla(ham, 380);
                    chkOtomatikArkaPlan.Checked = false;
                    picArkaPlan.Image = arkaPlanResmi;
                    Uygula();
                }
            };
            btnOrnek.Click += (s, e) => OrnekGolgeliBelgeOlustur();

            chkOtomatikArkaPlan = new CheckBox
            {
                Text = "Tek Fotoğraftan Arka Plan Tahmin Et\n(Boş kağıt görüntüsü yoksa)",
                Location = new Point(18, 190),
                Size = new Size(250, 42),
                Font = new Font("Segoe UI", 9f),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                Checked = false
            };
            chkOtomatikArkaPlan.CheckedChanged += (s, e) => Uygula();

            ModernKartPanel kartBilgi = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                new Point(18, 246), new Size(250, 235), out Label lblBilgi);
            lblBilgi.Text = "Çalışma Mantığı\n\n" +
                            "1. Aynı ışık altında çekilmiş boş arka plan görüntüsünden yazılı belge çıkarılır: |ArkaPlan - Belge|.\n\n" +
                            "2. Her iki resimde ortak olan gölge tonları birbirini götürür; geriye yalnızca yazı pikselleri kalır.\n\n" +
                            "3. 4. kartın altındaki eşik sürgüsüyle net okunabilir metin elde edilir.";

            pnlSag.Controls.AddRange(new Control[]
            {
                lblKaynak, btnBelgeSec, btnArkaPlanSec, btnOrnek, chkOtomatikArkaPlan, kartBilgi
            });

            rozetBelge = new ModernHapRozet();
            rozetBelge.MetinGuncelle("Belge");

            rozetArka = new ModernHapRozet();
            rozetArka.MetinGuncelle("Arka Plan");

            rozetFark = new ModernHapRozet();
            rozetFark.MetinGuncelle("|A - B|");

            rozetSonuc = new ModernHapRozet();
            rozetSonuc.MetinGuncelle("Eşik: 25");

            Panel pnlSonucAlt = FormGezintiYardimcisi.KartAltSurguPaneliOlustur(
                "Okuma Eşik Değeri", "5 – 120", 5, 120, 25, out trkEsik, out _);
            trkEsik.Scroll += (s, e) =>
            {
                rozetSonuc.MetinGuncelle($"Eşik: {trkEsik.Value}");
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

            picBelge = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 0, 0, "Gölgeli Belge", null, null, rozetBelge);
            picArkaPlan = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 1, 0, "Boş Gölgeli Arka Plan", null, null, rozetArka);
            picFark = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 0, 1, "Fark Görüntüsü |ArkaPlan - Belge|", null, null, rozetFark);
            picSonuc = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 1, 1, "Eşiklenmiş Okunabilir Metin", null, pnlSonucAlt, rozetSonuc);

            Controls.Add(tbl);
            FormGezintiYardimcisi.YerlesimiDuzenle(this, pnlUst, pnlSag, tbl);
        }

        private void OrnekGolgeliBelgeOlustur()
        {
            Bitmap belge = BitmapYardimcisi.OrnekGorselYukle("golgeli_belge.jpg");
            Bitmap arka = BitmapYardimcisi.OrnekGorselYukle("golgeli_belge_bos.jpg");
            if (belge != null && arka != null)
            {
                belgeResmi = belge;
                arkaPlanResmi = arka;
                picBelge.Image = belgeResmi;
                picArkaPlan.Image = arkaPlanResmi;
                Uygula();
                return;
            }
        }

        private Bitmap ArkaPlanTahminEt(Bitmap belge)
        {
            int w = belge.Width, h = belge.Height;
            Bitmap tahmin = new Bitmap(w, h);
            int r = 6;
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    int enAcik = 0;
                    for (int i = -r; i <= r; i += 2)
                    {
                        for (int j = -r; j <= r; j += 2)
                        {
                            int nx = Math.Clamp(x + i, 0, w - 1);
                            int ny = Math.Clamp(y + j, 0, h - 1);
                            Color c = belge.GetPixel(nx, ny);
                            int gri = (c.R + c.G + c.B) / 3;
                            if (gri > enAcik) enAcik = gri;
                        }
                    }
                    tahmin.SetPixel(x, y, Color.FromArgb(enAcik, enAcik, enAcik));
                }
            }
            return tahmin;
        }

        private void Uygula()
        {
            if (belgeResmi == null) return;

            Bitmap kullanilanArka = chkOtomatikArkaPlan.Checked || arkaPlanResmi == null
                ? ArkaPlanTahminEt(belgeResmi)
                : new Bitmap(arkaPlanResmi, belgeResmi.Width, belgeResmi.Height);

            picArkaPlan.Image = kullanilanArka;

            int w = belgeResmi.Width;
            int h = belgeResmi.Height;
            Bitmap farkBmp = new Bitmap(w, h);
            Bitmap sonucBmp = new Bitmap(w, h);
            int esik = trkEsik.Value;

            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    Color cb = belgeResmi.GetPixel(x, y);
                    Color ca = kullanilanArka.GetPixel(x, y);

                    int griBelge = (cb.R + cb.G + cb.B) / 3;
                    int griArka = (ca.R + ca.G + ca.B) / 3;

                    int fark = Math.Abs(griArka - griBelge);
                    farkBmp.SetPixel(x, y, Color.FromArgb(fark, fark, fark));

                    Color netRenk = fark >= esik ? Color.Black : Color.White;
                    sonucBmp.SetPixel(x, y, netRenk);
                }
            }

            picFark.Image = farkBmp;
            picSonuc.Image = sonucBmp;
        }
    }
}
