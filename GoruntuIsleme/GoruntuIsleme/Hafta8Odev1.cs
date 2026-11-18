using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GoruntuIsleme
{
    public class Hafta8Odev1 : Form
    {
        private PictureBox picGiris, picEsik, picSonuc;
        private ModernSurgu trkEsik, trkMinAlan, trkCevizEsik;
        private ModernHapRozet rozetGiris, rozetEsik, rozetSinif;
        private Label lblMinAlan, lblSonucOzet, lblAltBilgi;
        private CheckBox chkAcikZemin;
        private ComboBox cmbTestSenaryo;
        private Bitmap orijinalResim;

        public Hafta8Odev1()
        {
            Text = "Hafta 8 - Ödev 1 | Ceviz ve Fındık Sayma";
            FormGezintiYardimcisi.FormuHazirla(this);
            ArayuzuOlustur();
            OrnekSenaryoYukle(0);
        }

        private void ArayuzuOlustur()
        {
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                this,
                "Hafta 8 - Ödev 1 | Ceviz ve Fındık Tespiti (Bölge Bulma & Alan Sınıflandırma)",
                null, null,
                "Ödev 2 >", () => FormGezintiYardimcisi.FormGecisYap(this, new Hafta8Odev2()));

            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 290);

            Label lblKaynak = new Label
            {
                Text = "GÖRÜNTÜ VE SENARYO",
                Location = new Point(18, 18),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 8.8f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi
            };

            ModernHapButon btnResimSec = FormGezintiYardimcisi.ModernButonOlustur("Fotoğraf Dosyası Seç", true, 250, 38);
            btnResimSec.Location = new Point(18, 42);
            btnResimSec.Click += (s, e) =>
            {
                string yol = FormGezintiYardimcisi.ResimDosyasiSec("Ceviz / Fındık Fotoğrafı Seç");
                if (yol != null)
                {
                    using Bitmap ham = new Bitmap(yol);
                    orijinalResim = BitmapYardimcisi.BoyutSinirla(ham, 420);
                    picGiris.Image = orijinalResim;
                    AnalizEt();
                }
            };

            Label lblSenaryo = new Label
            {
                Text = "Örnek Test Görüntüsü Seç:",
                Location = new Point(18, 92),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 9f),
                ForeColor = FormGezintiYardimcisi.AcikYazi
            };

            cmbTestSenaryo = new ComboBox
            {
                Location = new Point(18, 114),
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9f)
            };
            cmbTestSenaryo.Items.AddRange(new object[]
            {
                "Senaryo 1: 3 Ceviz + 4 Fındık (Karışık)",
                "Senaryo 2: 2 Ceviz + 7 Fındık (Karışık)",
                "Senaryo 3: 5 Ceviz + 2 Fındık (Karışık)",
                "Senaryo 4: Sadece 6 Fındık (Karışık)",
                "Senaryo 5: 4 Ceviz + 5 Fındık (Gölgeli)"
            });
            cmbTestSenaryo.SelectedIndex = 0;
            cmbTestSenaryo.SelectedIndexChanged += (s, e) => OrnekSenaryoYukle(cmbTestSenaryo.SelectedIndex);

            chkAcikZemin = new CheckBox
            {
                Text = "Açık Renkli Zemin (Ters Eşikleme)",
                Checked = true,
                Location = new Point(18, 150),
                AutoSize = true,
                Font = new Font("Segoe UI", 9f),
                ForeColor = FormGezintiYardimcisi.KoyuYazi
            };
            chkAcikZemin.CheckedChanged += (s, e) => AnalizEt();

            lblMinAlan = new Label
            {
                Text = "Min Gürültü Filtresi: 180 px",
                Location = new Point(18, 184),
                AutoSize = true,
                Font = new Font("Segoe UI", 8.8f),
                ForeColor = FormGezintiYardimcisi.AcikYazi
            };
            trkMinAlan = new ModernSurgu
            {
                Location = new Point(18, 204),
                Width = 250,
                Minimum = 20,
                Maximum = 600,
                Value = 180
            };
            trkMinAlan.Scroll += (s, e) =>
            {
                lblMinAlan.Text = $"Min Gürültü Filtresi: {trkMinAlan.Value} px";
                AnalizEt();
            };

            ModernKartPanel kartOzet = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                new Point(18, 248), new Size(250, 210), out lblSonucOzet);

            pnlSag.Controls.AddRange(new Control[]
            {
                lblKaynak, btnResimSec, lblSenaryo, cmbTestSenaryo,
                chkAcikZemin, lblMinAlan, trkMinAlan, kartOzet
            });

            // Kart Sağ Üst Rozetleri ve Alt Sürgüleri
            rozetGiris = new ModernHapRozet();
            rozetGiris.MetinGuncelle("Giriş");

            rozetEsik = new ModernHapRozet();
            rozetEsik.MetinGuncelle("Eşik: 210");

            rozetSinif = new ModernHapRozet();
            rozetSinif.MetinGuncelle("Sınır: 2200 px");

            Panel pnlEsikAlt = FormGezintiYardimcisi.KartAltSurguPaneliOlustur(
                "İkili Eşik Değeri", "30 – 245", 30, 245, 210, out trkEsik, out _);
            trkEsik.Scroll += (s, e) =>
            {
                rozetEsik.MetinGuncelle($"Eşik: {trkEsik.Value}");
                AnalizEt();
            };

            Panel pnlCevizAlt = FormGezintiYardimcisi.KartAltSurguPaneliOlustur(
                "Ceviz / Fındık Alan Sınırı", "300 – 6500 px", 300, 6500, 2200, out trkCevizEsik, out _);
            trkCevizEsik.Scroll += (s, e) =>
            {
                rozetSinif.MetinGuncelle($"Sınır: {trkCevizEsik.Value} px");
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
                tbl, 0, 0, "Giriş Fotoğrafı", null, null, rozetGiris);
            picEsik = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 1, 0, "Siyah-Beyaz Maske", null, pnlEsikAlt, rozetEsik);
            picSonuc = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 2, 0, "Bölge Etiketleme & Sınıflandırma", null, pnlCevizAlt, rozetSinif);

            ModernKartPanel altSerit = FormGezintiYardimcisi.AltAnalizSeridiOlustur(
                "Bölge Bulma Özeti: İkili görüntüdeki bağlantılı beyaz pikseller 8-komşulukla etiketlenir; piksel alanına göre ceviz ve fındık sayısı otomatik hesaplanır.",
                out lblAltBilgi);
            tbl.Controls.Add(altSerit, 0, 1);
            tbl.SetColumnSpan(altSerit, 3);

            Controls.Add(tbl);
            FormGezintiYardimcisi.YerlesimiDuzenle(this, pnlUst, pnlSag, tbl);
        }

        private void OrnekSenaryoYukle(int senaryoIndex)
        {
            string dosyaAdi = $"ceviz_findik_{senaryoIndex + 1}.jpg";
            Bitmap yuklenen = BitmapYardimcisi.OrnekGorselYukle(dosyaAdi);
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

            Bitmap esikResim = BitmapYardimcisi.EsiklemeYap(orijinalResim, trkEsik.Value, chkAcikZemin.Checked);
            picEsik.Image = esikResim;

            Bitmap etiketli = BitmapYardimcisi.BolgeBulVeRenklendir(esikResim, trkMinAlan.Value, out var bolgeler, out _);

            int cevizAdet = 0;
            int findikAdet = 0;
            int cevizEsik = trkCevizEsik.Value;

            using (Graphics g = Graphics.FromImage(etiketli))
            using (Font f = new Font("Segoe UI", 8f, FontStyle.Bold))
            {
                foreach (var b in bolgeler)
                {
                    bool cevizMi = b.PikselSayisi >= cevizEsik;
                    if (cevizMi) cevizAdet++; else findikAdet++;

                    string etiket = cevizMi ? $"Ceviz ({b.PikselSayisi})" : $"Fındık ({b.PikselSayisi})";
                    g.DrawRectangle(Pens.Yellow, b.MinX, b.MinY, b.Genislik, b.Yukseklik);
                    g.DrawString(etiket, f, Brushes.White, b.MinX, Math.Max(0, b.MinY - 14));
                }
            }

            picSonuc.Image = etiketli;
            lblSonucOzet.Text = $"SINIFLANDIRMA SONUCU\n\n" +
                                $"• Toplam Nesne : {bolgeler.Count}\n" +
                                $"• Ceviz Sayısı : {cevizAdet}\n" +
                                $"• Fındık Sayısı: {findikAdet}\n\n" +
                                $"Alanı {cevizEsik} px ve üzeri olan\n" +
                                $"bölgeler Ceviz olarak ayrılmıştır.";
        }
    }
}
