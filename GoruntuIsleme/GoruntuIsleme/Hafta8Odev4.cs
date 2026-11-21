using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GoruntuIsleme
{
    public enum ParaDegerYontemi
    {
        CapVeMetalHalkaOtomatik,
        YalnizcaCapManuel
    }

    public enum YaziTuraYontemi
    {
        KenarHistogramKumeleme,
        MerkezHalkaVeSimetri
    }

    public class ParaTespitBilgisi
    {
        public BitmapYardimcisi.BolgeBilgisi Bolge { get; set; }
        public double CapOrtalama { get; set; }
        public int CapX { get; set; }
        public int CapY { get; set; }
        public int KurusDegeri { get; set; }
        public string DegerEtiketi { get; set; }
        public bool YaziMi { get; set; }
        public double MerkezKenarOrt { get; set; }
        public double PuruzsuzMerkezYuzde { get; set; }
        public double MerkezDisOran { get; set; }
        public double YatayAsimetri { get; set; }
        public double HalkaSicaklikFarki { get; set; }
        public double[] KenarHistogram12 { get; set; }
    }

    public class ParaAnalizSonucu
    {
        public List<ParaTespitBilgisi> Paralar { get; set; } = new List<ParaTespitBilgisi>();
        public int Adet25 { get; set; }
        public int Adet50 { get; set; }
        public int Adet1TL { get; set; }
        public double ToplamTL { get; set; }
        public int YaziSayisi { get; set; }
        public int TuraSayisi { get; set; }
        public double KumeHistogramUzakligi { get; set; }
        public int KullanilanEsik { get; set; }
        public Bitmap KenarResmi { get; set; }
        public Bitmap SonucResmi { get; set; }
    }

    public class Hafta8Odev4 : Form
    {
        private PictureBox picGiris, picKenar, picSonuc;
        private ModernSurgu trkEsik, trkYirmiBesElliSinir, trkElliBirSinir, trkYaziTuraEsik;
        private ComboBox cmbDegerYontemi, cmbYaziTuraYontemi;
        private CheckBox chkOtomatikEsik;
        private ModernHapRozet rozetGiris, rozetKenar, rozetSonuc;
        private Label lblSinir1, lblSinir2, lblOzet, lblAltAnaliz;
        private Bitmap orijinalResim;
        private bool programatikDegisim = false;

        public Hafta8Odev4()
        {
            Text = "Hafta 8 - Ödev 4 | Madeni Para Sayma ve Yazı/Tura Tespiti";
            FormGezintiYardimcisi.FormuHazirla(this);
            ArayuzuOlustur();
            OrnekParaResmiYukle();
        }

        private void ArayuzuOlustur()
        {
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                this,
                "Hafta 8 - Ödev 4 | Madeni Para (25 Kr, 50 Kr, 1 TL) Sayma & Yazı/Tura Tanıma",
                "< Ödev 3", () => FormGezintiYardimcisi.FormGecisYap(this, new Hafta8Odev3()),
                "Hafta 9 >", () => FormGezintiYardimcisi.FormGecisYap(this, new Hafta9Odev1()),
                false,
                "HAFTA 8 • ÖDEV 4");

            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 305);
            pnlSag.AutoScroll = true;

            int x = 16;
            int w = 262;
            int y = 14;

            ModernHapButon btnResimSec = FormGezintiYardimcisi.ModernButonOlustur("Para Fotoğrafı Seç", true, w, 36);
            btnResimSec.Location = new Point(x, y);
            y += 42;

            ModernHapButon btnOrnekGercek = FormGezintiYardimcisi.ModernButonOlustur("Örnek 1: Türk Lirası Paralar", false, w, 34);
            btnOrnekGercek.Location = new Point(x, y);
            y += 40;

            ModernHapButon btnOrnekOlcekli = FormGezintiYardimcisi.ModernButonOlustur("Örnek 2: Ölçekli Paralar (Çap Testi)", false, w, 34);
            btnOrnekOlcekli.Location = new Point(x, y);
            y += 40;

            ModernHapButon btnTemizle = FormGezintiYardimcisi.ModernButonOlustur("Temizle", false, w, 32);
            btnTemizle.Location = new Point(x, y);
            y += 40;

            btnResimSec.Click += (s, e) =>
            {
                string yol = FormGezintiYardimcisi.ResimDosyasiSec("Beyaz Kağıt Üzerinde Madeni Para Fotoğrafı Seç");
                if (yol == null) return;
                orijinalResim?.Dispose();
                using Bitmap ham = new Bitmap(yol);
                orijinalResim = BitmapYardimcisi.BoyutSinirla(ham, 420);
                ResmiAta(picGiris, (Bitmap)orijinalResim.Clone());
                AnalizEt(kullaniciTetikledi: true);
            };
            btnOrnekGercek.Click += (s, e) => OrnekParaResmiYukle();
            btnOrnekOlcekli.Click += (s, e) => OrnekOlcekliParaYukle();
            btnTemizle.Click += (s, e) => Temizle();

            ModernKartPanel kartAyar = new ModernKartPanel
            {
                Location = new Point(x, y),
                Size = new Size(w, 240),
                KartArkaPlan = Color.White,
                KenarlikRengi = FormGezintiYardimcisi.KartKenar,
                KoseYaricapi = 12
            };

            Label lblDegerYontem = new Label
            {
                Text = "Para Değeri Sınıflandırma (4a):",
                Location = new Point(12, 10),
                Size = new Size(238, 17),
                Font = new Font("Segoe UI", 8.3f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi
            };

            cmbDegerYontemi = new ComboBox
            {
                Location = new Point(12, 28),
                Size = new Size(238, 24),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 8.3f)
            };
            cmbDegerYontemi.Items.Add("Çap + Metal Halka (Otomatik)");
            cmbDegerYontemi.Items.Add("Yalnızca Çap Eşiği (x2-x1 Manuel)");
            cmbDegerYontemi.SelectedIndex = 0;
            cmbDegerYontemi.SelectedIndexChanged += (s, e) => { if (!programatikDegisim) AnalizEt(); };

            Label lblYtYontem = new Label
            {
                Text = "Yazı / Tura Tanıma Yöntemi (4b):",
                Location = new Point(12, 58),
                Size = new Size(238, 17),
                Font = new Font("Segoe UI", 8.3f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi
            };

            cmbYaziTuraYontemi = new ComboBox
            {
                Location = new Point(12, 76),
                Size = new Size(238, 24),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 8.3f)
            };
            cmbYaziTuraYontemi.Items.Add("Kenar Deseni Histogram Kümeleme");
            cmbYaziTuraYontemi.Items.Add("Merkez/Halka Oranı + Simetri");
            cmbYaziTuraYontemi.SelectedIndex = 0;
            cmbYaziTuraYontemi.SelectedIndexChanged += (s, e) => { if (!programatikDegisim) AnalizEt(); };

            chkOtomatikEsik = new CheckBox
            {
                Text = "Otomatik Arka Plan Eşiği (Otsu)",
                Location = new Point(12, 107),
                Size = new Size(238, 22),
                Font = new Font("Segoe UI", 8.3f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi
            };
            chkOtomatikEsik.CheckedChanged += (s, e) =>
            {
                if (!programatikDegisim) AnalizEt();
            };

            lblSinir1 = new Label
            {
                Text = "25 Kr / 50 Kr Çap Sınırı: 66 px",
                Location = new Point(12, 134),
                AutoSize = true,
                Font = new Font("Segoe UI", 8.3f),
                ForeColor = FormGezintiYardimcisi.AcikYazi
            };
            trkYirmiBesElliSinir = new ModernSurgu
            {
                Location = new Point(12, 152),
                Width = 238,
                Minimum = 20,
                Maximum = 140,
                Value = 66
            };
            trkYirmiBesElliSinir.Scroll += (s, e) =>
            {
                lblSinir1.Text = $"25 Kr / 50 Kr Çap Sınırı: {trkYirmiBesElliSinir.Value} px";
                if (!programatikDegisim)
                {
                    if (cmbDegerYontemi.SelectedIndex != 1)
                    {
                        programatikDegisim = true;
                        cmbDegerYontemi.SelectedIndex = 1;
                        programatikDegisim = false;
                    }
                    AnalizEt();
                }
            };

            lblSinir2 = new Label
            {
                Text = "50 Kr / 1 TL Çap Sınırı: 78 px",
                Location = new Point(12, 184),
                AutoSize = true,
                Font = new Font("Segoe UI", 8.3f),
                ForeColor = FormGezintiYardimcisi.AcikYazi
            };
            trkElliBirSinir = new ModernSurgu
            {
                Location = new Point(12, 202),
                Width = 238,
                Minimum = 30,
                Maximum = 160,
                Value = 78
            };
            trkElliBirSinir.Scroll += (s, e) =>
            {
                lblSinir2.Text = $"50 Kr / 1 TL Çap Sınırı: {trkElliBirSinir.Value} px";
                if (!programatikDegisim)
                {
                    if (cmbDegerYontemi.SelectedIndex != 1)
                    {
                        programatikDegisim = true;
                        cmbDegerYontemi.SelectedIndex = 1;
                        programatikDegisim = false;
                    }
                    AnalizEt();
                }
            };

            kartAyar.Controls.AddRange(new Control[]
            {
                lblDegerYontem, cmbDegerYontemi,
                lblYtYontem, cmbYaziTuraYontemi,
                chkOtomatikEsik,
                lblSinir1, trkYirmiBesElliSinir,
                lblSinir2, trkElliBirSinir
            });
            y += 250;

            ModernKartPanel kartOzet = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                new Point(x, y), new Size(w, 232), out lblOzet);
            kartOzet.Margin = new Padding(0, 0, 0, 14);

            pnlSag.Controls.AddRange(new Control[]
            {
                btnResimSec, btnOrnekGercek, btnOrnekOlcekli, btnTemizle,
                kartAyar, kartOzet
            });

            rozetGiris = new ModernHapRozet();
            rozetGiris.MetinGuncelle("Zemin: 215");

            rozetKenar = new ModernHapRozet();
            rozetKenar.MetinGuncelle("Kenar Eşiği: 98");

            rozetSonuc = new ModernHapRozet();
            rozetSonuc.MetinGuncelle("0.00 TL");

            Panel pnlGirisAlt = FormGezintiYardimcisi.KartAltSurguPaneliOlustur(
                "Arka Plan Eşik Değeri", "50 – 245", 50, 245, 215, out trkEsik, out _);
            trkEsik.Scroll += (s, e) =>
            {
                if (chkOtomatikEsik.Checked)
                {
                    programatikDegisim = true;
                    chkOtomatikEsik.Checked = false;
                    programatikDegisim = false;
                }
                rozetGiris.MetinGuncelle($"Zemin: {trkEsik.Value}");
                AnalizEt();
            };

            Panel pnlKenarAlt = FormGezintiYardimcisi.KartAltSurguPaneliOlustur(
                "Yazı/Tura Merkez Kenar Eşiği", "60 – 150", 60, 150, 98, out trkYaziTuraEsik, out _);
            trkYaziTuraEsik.Scroll += (s, e) =>
            {
                rozetKenar.MetinGuncelle($"Kenar Eşiği: {trkYaziTuraEsik.Value}");
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
                tbl, 2, 0, "Koordinat Çapı (x1,x2,y1,y2) & Sonuç", null, null, rozetSonuc);

            ModernKartPanel altSerit = FormGezintiYardimcisi.AltAnalizSeridiOlustur(
                "Analiz hazır.",
                out lblAltAnaliz,
                "Para Tanıma");
            tbl.Controls.Add(altSerit, 0, 1);
            tbl.SetColumnSpan(altSerit, 3);

            Controls.Add(tbl);
            FormGezintiYardimcisi.YerlesimiDuzenle(this, pnlUst, pnlSag, tbl);
        }

        private void OrnekParaResmiYukle()
        {
            Bitmap yuklenen = BitmapYardimcisi.OrnekGorselYukle("turk_lirasi_paralar.jpg");
            if (yuklenen == null) yuklenen = OrnekOlcekliParaOlustur();

            programatikDegisim = true;
            try
            {
                cmbDegerYontemi.SelectedIndex = 0;
                chkOtomatikEsik.Checked = false;
                trkEsik.Value = 215;
                rozetGiris.MetinGuncelle("Zemin: 215");
                trkYirmiBesElliSinir.Value = 66;
                lblSinir1.Text = "25 Kr / 50 Kr Çap Sınırı: 66 px";
                trkElliBirSinir.Value = 78;
                lblSinir2.Text = "50 Kr / 1 TL Çap Sınırı: 78 px";
            }
            finally
            {
                programatikDegisim = false;
            }

            orijinalResim?.Dispose();
            orijinalResim = yuklenen;
            ResmiAta(picGiris, (Bitmap)orijinalResim.Clone());
            AnalizEt();
        }

        private void OrnekOlcekliParaYukle()
        {
            programatikDegisim = true;
            try
            {
                cmbDegerYontemi.SelectedIndex = 1; // Yalnızca çap ölçümünü doğrudan göster
                chkOtomatikEsik.Checked = false;
                trkEsik.Value = 215;
                rozetGiris.MetinGuncelle("Zemin: 215");
                trkYirmiBesElliSinir.Value = 59;
                lblSinir1.Text = "25 Kr / 50 Kr Çap Sınırı: 59 px";
                trkElliBirSinir.Value = 68;
                lblSinir2.Text = "50 Kr / 1 TL Çap Sınırı: 68 px";
            }
            finally
            {
                programatikDegisim = false;
            }

            orijinalResim?.Dispose();
            orijinalResim = OrnekOlcekliParaOlustur();
            ResmiAta(picGiris, (Bitmap)orijinalResim.Clone());
            AnalizEt();
        }

        private void Temizle()
        {
            orijinalResim?.Dispose();
            orijinalResim = null;
            ResmiAta(picGiris, null);
            ResmiAta(picKenar, null);
            ResmiAta(picSonuc, null);
            rozetSonuc.MetinGuncelle("0.00 TL");
            lblOzet.Text = "Görsel temizlendi.\nÖrnek yükleyin veya fotoğraf seçin.";
            lblAltAnaliz.Text = "Analiz bekleniyor.";
        }

        private static void ResmiAta(PictureBox pic, Bitmap yeni)
        {
            Image onceki = pic.Image;
            pic.Image = yeni;
            onceki?.Dispose();
        }

        private void AnalizEt(bool kullaniciTetikledi = false)
        {
            if (orijinalResim == null)
            {
                if (kullaniciTetikledi)
                {
                    MessageBox.Show("Lütfen önce madeni para içeren bir görsel seçin veya örnek yükleyin.",
                        "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                return;
            }

            ParaDegerYontemi degerYontemi = cmbDegerYontemi.SelectedIndex == 0
                ? ParaDegerYontemi.CapVeMetalHalkaOtomatik
                : ParaDegerYontemi.YalnizcaCapManuel;

            YaziTuraYontemi ytYontemi = cmbYaziTuraYontemi.SelectedIndex == 0
                ? YaziTuraYontemi.KenarHistogramKumeleme
                : YaziTuraYontemi.MerkezHalkaVeSimetri;

            ParaAnalizSonucu sonuc = ParalariAnalizEt(
                orijinalResim,
                trkEsik.Value,
                chkOtomatikEsik.Checked,
                degerYontemi,
                trkYirmiBesElliSinir.Value,
                trkElliBirSinir.Value,
                ytYontemi,
                trkYaziTuraEsik.Value);

            ResmiAta(picKenar, sonuc.KenarResmi);
            ResmiAta(picSonuc, sonuc.SonucResmi);

            rozetGiris.MetinGuncelle(chkOtomatikEsik.Checked
                ? $"Otsu: {sonuc.KullanilanEsik}"
                : $"Zemin: {sonuc.KullanilanEsik}");
            rozetSonuc.MetinGuncelle($"{sonuc.ToplamTL:0.00} TL");

            string ytAd = ytYontemi == YaziTuraYontemi.KenarHistogramKumeleme
                ? "Kenar Hist. Kümeleme"
                : "Merkez/Halka + Simetri";

            lblOzet.Text = $"PARA & YÜZ ANALİZİ\n\n" +
                           $"• 100 Kr (1 TL) : {sonuc.Adet1TL} adet\n" +
                           $"•  50 Kuruş    : {sonuc.Adet50} adet\n" +
                           $"•  25 Kuruş    : {sonuc.Adet25} adet\n" +
                           $"• TOPLAM TUTAR : {sonuc.ToplamTL:0.00} TL\n\n" +
                           $"• Yazı Yüzü : {sonuc.YaziSayisi} adet\n" +
                           $"• Tura Yüzü : {sonuc.TuraSayisi} adet\n" +
                           $"• Yöntem: {ytAd}";

            if (sonuc.Paralar.Count > 0)
            {
                int minCap = int.MaxValue, maxCap = 0;
                foreach (var p in sonuc.Paralar)
                {
                    if (p.CapX < minCap) minCap = p.CapX;
                    if (p.CapX > maxCap) maxCap = p.CapX;
                }
                lblAltAnaliz.Text = $"Bulunan: {sonuc.Paralar.Count} para • Ölçülen çap (x2-x1): {minCap}..{maxCap} px | Toplam: {sonuc.ToplamTL:0.00} TL | {sonuc.YaziSayisi} Yazı, {sonuc.TuraSayisi} Tura | Küme L1: {sonuc.KumeHistogramUzakligi:0.00}";
            }
            else
            {
                lblAltAnaliz.Text = "Uyarı: Eşik değerine uygun madeni para bölgesi bulunamadı. Arka plan eşiğini ayarlayın.";
            }
        }

        public static ParaAnalizSonucu ParalariAnalizEt(
            Bitmap giris,
            int arkaPlanEsik,
            bool otsuKullan,
            ParaDegerYontemi degerYontemi,
            int sinir25_50,
            int sinir50_100,
            YaziTuraYontemi ytYontemi,
            int yaziTuraEsik)
        {
            ParaAnalizSonucu sonuc = new ParaAnalizSonucu();
            if (giris == null) return sonuc;

            int w = giris.Width;
            int h = giris.Height;
            int kullanilanEsik = otsuKullan
                ? Math.Clamp(BitmapYardimcisi.OtsuEsikHesapla(giris) + 15, 50, 240)
                : arkaPlanEsik;
            sonuc.KullanilanEsik = kullanilanEsik;

            using Bitmap hamIkili = BitmapYardimcisi.EsiklemeYap(giris, kullanilanEsik, true);
            using Bitmap temizIkili = BitmapYardimcisi.MorfolojiAcmaKapama(hamIkili, acmaTekrar: 1, kapamaTekrar: 1);
            Bitmap etiketli = BitmapYardimcisi.BolgeBulVeRenklendir(temizIkili, 200, out List<BitmapYardimcisi.BolgeBilgisi> hamBolgeler, out int[,] etiketler);
            Bitmap kenarResmi = BitmapYardimcisi.SobelKenarBul(giris);

            int[,] gri = new int[w, h];
            Color[,] renkler = new Color[w, h];
            int[,] sobel = new int[w, h];
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    Color c = giris.GetPixel(x, y);
                    renkler[x, y] = c;
                    gri[x, y] = (c.R + c.G + c.B) / 3;
                    sobel[x, y] = kenarResmi.GetPixel(x, y).R;
                }
            }

            List<ParaTespitBilgisi> paralar = new List<ParaTespitBilgisi>();
            foreach (var b in hamBolgeler)
            {
                double enBoyOrani = (double)Math.Min(b.Genislik, b.Yukseklik) / Math.Max(1, Math.Max(b.Genislik, b.Yukseklik));
                double doluluk = (double)b.PikselSayisi / Math.Max(1, b.Genislik * b.Yukseklik);
                if (enBoyOrani < 0.65 || doluluk < 0.45) continue;

                ParaTespitBilgisi bilgi = ParaOzellikleriniCikar(b, etiketler, renkler, gri, sobel, w);
                paralar.Add(bilgi);
            }

            // 4a: Para değerini belirle
            int altSinir = Math.Min(sinir25_50, sinir50_100);
            int ustSinir = Math.Max(sinir25_50, sinir50_100);
            bool ciftMetalSinyaliVar = false;
            foreach (var p in paralar)
            {
                if (Math.Abs(p.HalkaSicaklikFarki) >= 15.0)
                {
                    ciftMetalSinyaliVar = true;
                    break;
                }
            }

            foreach (var p in paralar)
            {
                if (degerYontemi == ParaDegerYontemi.CapVeMetalHalkaOtomatik && ciftMetalSinyaliVar)
                {
                    if (p.HalkaSicaklikFarki >= 15.0)
                    {
                        p.KurusDegeri = 100;
                        p.DegerEtiketi = "1 TL";
                    }
                    else if (p.HalkaSicaklikFarki <= -15.0)
                    {
                        p.KurusDegeri = 50;
                        p.DegerEtiketi = "50 Kr";
                    }
                    else
                    {
                        p.KurusDegeri = 25;
                        p.DegerEtiketi = "25 Kr";
                    }
                }
                else
                {
                    if (p.CapOrtalama < altSinir)
                    {
                        p.KurusDegeri = 25;
                        p.DegerEtiketi = "25 Kr";
                    }
                    else if (p.CapOrtalama < ustSinir)
                    {
                        p.KurusDegeri = 50;
                        p.DegerEtiketi = "50 Kr";
                    }
                    else
                    {
                        p.KurusDegeri = 100;
                        p.DegerEtiketi = "1 TL";
                    }
                }

                if (p.KurusDegeri == 25) sonuc.Adet25++;
                else if (p.KurusDegeri == 50) sonuc.Adet50++;
                else sonuc.Adet1TL++;
            }

            // 4b: Yazı / Tura sınıflandırması
            sonuc.KumeHistogramUzakligi = YaziTuraSiniflandir(paralar, ytYontemi, yaziTuraEsik);
            foreach (var p in paralar)
            {
                if (p.YaziMi) sonuc.YaziSayisi++;
                else sonuc.TuraSayisi++;
            }

            sonuc.Paralar = paralar;
            sonuc.ToplamTL = sonuc.Adet25 * 0.25 + sonuc.Adet50 * 0.50 + sonuc.Adet1TL * 1.00;
            SonucGorseliniCiz(etiketli, paralar);

            sonuc.KenarResmi = kenarResmi;
            sonuc.SonucResmi = etiketli;
            return sonuc;
        }

        private static ParaTespitBilgisi ParaOzellikleriniCikar(
            BitmapYardimcisi.BolgeBilgisi b,
            int[,] etiketler,
            Color[,] renkler,
            int[,] gri,
            int[,] sobel,
            int w)
        {
            double cx = b.MerkezX;
            double cy = b.MerkezY;
            double rMax = Math.Max(4.0, b.Genislik / 2.0);

            double coreWarmSum = 0, ringWarmSum = 0;
            int coreWarmN = 0, ringWarmN = 0;

            double[] hist12 = new double[12];
            int[] ringN = new int[4];
            int[] ringE = new int[4];
            int cTot = 0, smoothC = 0, outN = 0, outE = 0, asymN = 0;
            double cSum = 0, asymSum = 0;

            for (int py = b.MinY; py <= b.MaxY; py++)
            {
                for (int px = b.MinX; px <= b.MaxX; px++)
                {
                    if (etiketler[px, py] != b.EtiketNo) continue;

                    double rn = Math.Sqrt((px - cx) * (px - cx) + (py - cy) * (py - cy)) / rMax;
                    Color col = renkler[px, py];
                    int ev = sobel[px, py];

                    if (rn <= 0.52)
                    {
                        coreWarmSum += (col.R - col.B);
                        coreWarmN++;
                    }
                    else if (rn >= 0.65 && rn <= 0.90)
                    {
                        ringWarmSum += (col.R - col.B);
                        ringWarmN++;
                    }

                    if (rn <= 0.45)
                    {
                        hist12[Math.Min(7, ev / 32)] += 1.0;
                        cTot++;
                        cSum += ev;
                        if (ev < 45) smoothC++;
                    }

                    if (rn <= 0.88)
                    {
                        int ri = Math.Min(3, (int)(rn / 0.22));
                        ringN[ri]++;
                        if (ev > 65) ringE[ri]++;
                    }

                    if (rn >= 0.65 && rn <= 0.88)
                    {
                        outN++;
                        if (ev > 65) outE++;
                    }

                    if (rn <= 0.55)
                    {
                        int mx = (int)Math.Round(2.0 * cx - px);
                        if (mx >= 0 && mx < w)
                        {
                            asymSum += Math.Abs(gri[px, py] - gri[mx, py]);
                            asymN++;
                        }
                    }
                }
            }

            for (int k = 0; k < 8; k++) hist12[k] /= Math.Max(1, cTot);
            for (int r = 0; r < 4; r++) hist12[8 + r] = (double)ringE[r] / Math.Max(1, ringN[r]);

            double cMean = cSum / Math.Max(1, cTot);
            double smoothPct = 100.0 * smoothC / Math.Max(1, cTot);
            double cEdgePct = 100.0 * (ringE[0] + ringE[1]) / Math.Max(1, ringN[0] + ringN[1]);
            double oEdgePct = 100.0 * outE / Math.Max(1, outN);
            double ratioCO = cEdgePct / (oEdgePct + 1.0);
            double asym = asymSum / Math.Max(1, asymN);

            double coreWarm = coreWarmN > 0 ? coreWarmSum / coreWarmN : 0;
            double ringWarm = ringWarmN > 0 ? ringWarmSum / ringWarmN : 0;

            return new ParaTespitBilgisi
            {
                Bolge = b,
                CapX = b.Genislik,
                CapY = b.Yukseklik,
                CapOrtalama = (b.Genislik + b.Yukseklik) / 2.0,
                MerkezKenarOrt = cMean,
                PuruzsuzMerkezYuzde = smoothPct,
                MerkezDisOran = ratioCO,
                YatayAsimetri = asym,
                HalkaSicaklikFarki = ringWarm - coreWarm,
                KenarHistogram12 = hist12
            };
        }

        private static double YaziTuraSiniflandir(List<ParaTespitBilgisi> paralar, YaziTuraYontemi ytYontemi, int yaziTuraEsik)
        {
            if (paralar.Count == 0) return 0;

            if (ytYontemi == YaziTuraYontemi.MerkezHalkaVeSimetri)
            {
                foreach (var p in paralar)
                {
                    double skor = (p.MerkezKenarOrt - yaziTuraEsik) * 0.6
                                + (p.MerkezDisOran - 0.90) * 35.0
                                + (24.0 - p.PuruzsuzMerkezYuzde) * 1.2
                                + (15.5 - p.YatayAsimetri) * 1.0;
                    p.YaziMi = skor >= 0.0;
                }
                return 0;
            }

            // Kenar Deseni Histogram Kümeleme (Slayt İpucu Yöntemi)
            if (paralar.Count < 2)
            {
                paralar[0].YaziMi = paralar[0].MerkezKenarOrt >= yaziTuraEsik;
                return 0;
            }

            int idxLow = 0, idxHigh = 0;
            for (int i = 1; i < paralar.Count; i++)
            {
                if (paralar[i].MerkezKenarOrt < paralar[idxLow].MerkezKenarOrt) idxLow = i;
                if (paralar[i].MerkezKenarOrt > paralar[idxHigh].MerkezKenarOrt) idxHigh = i;
            }

            double[] c0 = (double[])paralar[idxLow].KenarHistogram12.Clone();
            double[] c1 = (double[])paralar[idxHigh].KenarHistogram12.Clone();
            int[] atama = new int[paralar.Count];

            for (int iter = 0; iter < 15; iter++)
            {
                for (int i = 0; i < paralar.Count; i++)
                {
                    double d0 = L1Uzaklik(paralar[i].KenarHistogram12, c0);
                    double d1 = L1Uzaklik(paralar[i].KenarHistogram12, c1);
                    atama[i] = d1 < d0 ? 1 : 0;
                }

                double[] toplam0 = new double[12];
                double[] toplam1 = new double[12];
                int say0 = 0, say1 = 0;
                for (int i = 0; i < paralar.Count; i++)
                {
                    if (atama[i] == 0)
                    {
                        say0++;
                        for (int k = 0; k < 12; k++) toplam0[k] += paralar[i].KenarHistogram12[k];
                    }
                    else
                    {
                        say1++;
                        for (int k = 0; k < 12; k++) toplam1[k] += paralar[i].KenarHistogram12[k];
                    }
                }

                if (say0 > 0) for (int k = 0; k < 12; k++) c0[k] = toplam0[k] / say0;
                if (say1 > 0) for (int k = 0; k < 12; k++) c1[k] = toplam1[k] / say1;
            }

            double kumeUzakligi = L1Uzaklik(c0, c1);
            if (kumeUzakligi < 0.25)
            {
                foreach (var p in paralar)
                    p.YaziMi = p.MerkezKenarOrt >= yaziTuraEsik;
                return kumeUzakligi;
            }

            // Hangi kümenin Yazı kümesi olduğunu belirle (merkez kenar ortalaması daha yüksek küme = Yazı)
            double ort0 = 0, ort1 = 0;
            int n0 = 0, n1 = 0;
            for (int i = 0; i < paralar.Count; i++)
            {
                if (atama[i] == 0) { ort0 += paralar[i].MerkezKenarOrt; n0++; }
                else { ort1 += paralar[i].MerkezKenarOrt; n1++; }
            }
            if (n0 > 0) ort0 /= n0;
            if (n1 > 0) ort1 /= n1;
            int yaziKumesi = ort1 >= ort0 ? 1 : 0;

            for (int i = 0; i < paralar.Count; i++)
                paralar[i].YaziMi = (atama[i] == yaziKumesi);

            return kumeUzakligi;
        }

        private static double L1Uzaklik(double[] a, double[] b)
        {
            double s = 0;
            for (int i = 0; i < a.Length; i++) s += Math.Abs(a[i] - b[i]);
            return s;
        }

        private static void SonucGorseliniCiz(Bitmap etiketli, List<ParaTespitBilgisi> paralar)
        {
            if (paralar.Count == 0) return;

            using Graphics g = Graphics.FromImage(etiketli);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using Font fEtiket = new Font("Segoe UI", 7.8f, FontStyle.Bold);
            using Font fEksen = new Font("Segoe UI", 7.5f, FontStyle.Bold);
            using Pen kilavuzKalem = new Pen(Color.FromArgb(190, 220, 235, 250), 1f) { DashStyle = DashStyle.Dash };
            using Brush etiketKutu = new SolidBrush(Color.FromArgb(195, 15, 23, 42));

            // Slayt diyagramındaki (x1, x2, y1, y2) koordinat kılavuzlarını ilk para için çiz
            ParaTespitBilgisi ilk = paralar[0];
            for (int i = 1; i < paralar.Count; i++)
            {
                if (paralar[i].Bolge.MinY + paralar[i].Bolge.MinX < ilk.Bolge.MinY + ilk.Bolge.MinX)
                    ilk = paralar[i];
            }
            var ib = ilk.Bolge;
            g.DrawLine(kilavuzKalem, ib.MinX, 0, ib.MinX, ib.MaxY);
            g.DrawLine(kilavuzKalem, ib.MaxX, 0, ib.MaxX, ib.MaxY);
            g.DrawLine(kilavuzKalem, 0, ib.MinY, ib.MaxX, ib.MinY);
            g.DrawLine(kilavuzKalem, 0, ib.MaxY, ib.MaxX, ib.MaxY);

            g.DrawString("x1", fEksen, Brushes.LightSkyBlue, Math.Max(0, ib.MinX - 8), 2);
            g.DrawString("x2", fEksen, Brushes.LightSkyBlue, Math.Min(etiketli.Width - 18, ib.MaxX - 4), 2);
            g.DrawString("y1", fEksen, Brushes.LightSkyBlue, 2, Math.Max(12, ib.MinY - 12));
            g.DrawString("y2", fEksen, Brushes.LightSkyBlue, 2, Math.Min(etiketli.Height - 14, ib.MaxY - 4));

            foreach (var p in paralar)
            {
                var b = p.Bolge;
                g.DrawRectangle(Pens.Yellow, b.MinX, b.MinY, b.Genislik, b.Yukseklik);

                string yaziMetni = $"{p.DegerEtiketi} • {(p.YaziMi ? "Yazı" : "Tura")} (Ø{p.CapX})";
                SizeF sz = g.MeasureString(yaziMetni, fEtiket);
                int tx = Math.Clamp(b.MinX, 2, Math.Max(2, etiketli.Width - (int)sz.Width - 4));
                int ty = b.MinY >= 18 ? b.MinY - 16 : b.MinY + 2;

                g.FillRectangle(etiketKutu, tx, ty, sz.Width + 4, 15);
                g.DrawString(yaziMetni, fEtiket, p.YaziMi ? Brushes.LightGreen : Brushes.Gold, tx + 2, ty + 1);
            }
        }

        public static Bitmap OrnekOlcekliParaOlustur()
        {
            Bitmap bmp = new Bitmap(360, 280);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.White);

                // Üst sıra: Yazı yüzleri (1 TL d=72, 50 Kr d=64, 25 Kr d=54)
                TekParaCiz(g, 22, 22, 72, 100, yaziMi: true);
                TekParaCiz(g, 145, 26, 64, 50, yaziMi: true);
                TekParaCiz(g, 268, 31, 54, 25, yaziMi: true);

                // Alt sıra: Tura yüzleri (25 Kr d=54, 50 Kr d=64, 1 TL d=72)
                TekParaCiz(g, 31, 165, 54, 25, yaziMi: false);
                TekParaCiz(g, 145, 160, 64, 50, yaziMi: false);
                TekParaCiz(g, 259, 155, 72, 100, yaziMi: false);
            }
            return bmp;
        }

        private static void TekParaCiz(Graphics g, int x, int y, int cap, int kurus, bool yaziMi)
        {
            Color sariMetal = Color.FromArgb(196, 158, 72);
            Color gumusMetal = Color.FromArgb(172, 174, 178);
            Color koyuCizgi = Color.FromArgb(45, 45, 50);

            Color disRenk = kurus == 100 ? sariMetal : gumusMetal;
            Color icRenk = kurus == 50 ? sariMetal : gumusMetal;

            using (Brush disFirca = new SolidBrush(disRenk))
                g.FillEllipse(disFirca, x, y, cap, cap);

            int icPay = (int)(cap * 0.19);
            using (Brush icFirca = new SolidBrush(icRenk))
                g.FillEllipse(icFirca, x + icPay, y + icPay, cap - 2 * icPay, cap - 2 * icPay);

            using (Pen kenarKalem = new Pen(Color.FromArgb(95, 95, 100), 1.6f))
                g.DrawEllipse(kenarKalem, x, y, cap, cap);

            float cx = x + cap / 2f;
            float cy = y + cap / 2f;

            if (yaziMi)
            {
                // Yazı yüzü: merkezde (r<=0.45) simetrik desen çizgileri ve net yüksek kontrastlı rakam
                using Pen icCizgi = new Pen(koyuCizgi, 1.5f);
                float halkaR = cap * 0.18f;
                float tickYari = Math.Max(2.2f, cap * 0.035f);
                for (int a = 0; a < 360; a += 30)
                {
                    double rad = a * Math.PI / 180.0;
                    float x1 = cx + (float)(Math.Cos(rad) * (halkaR - tickYari));
                    float y1 = cy + (float)(Math.Sin(rad) * (halkaR - tickYari));
                    float x2 = cx + (float)(Math.Cos(rad) * (halkaR + tickYari));
                    float y2 = cy + (float)(Math.Sin(rad) * (halkaR + tickYari));
                    g.DrawLine(icCizgi, x1, y1, x2, y2);
                }

                if (kurus == 100)
                {
                    float icCemberR = cap * 0.13f;
                    g.DrawEllipse(icCizgi, cx - icCemberR, cy - icCemberR, 2 * icCemberR, 2 * icCemberR);
                }

                string metin = kurus == 100 ? "1" : kurus.ToString();
                float fontBoyut = kurus == 100 ? cap * 0.25f : cap * 0.22f;
                using Font f = new Font("Segoe UI", fontBoyut, FontStyle.Bold);
                using Brush textBrush = new SolidBrush(koyuCizgi);
                SizeF sz = g.MeasureString(metin, f);
                g.DrawString(metin, f, textBrush, cx - sz.Width / 2f, cy - sz.Height / 2f);
                return;
            }

            // Tura yüzü: dış halkada yazı noktaları (yüksek dış kenar), merkezde pürüzsüz asimetrik profil gölgesi (düşük merkez kenarı)
            Color profilRenk = kurus == 50
                ? Color.FromArgb(174, 138, 64)
                : Color.FromArgb(152, 154, 158);
            using (Brush profilFirca = new SolidBrush(profilRenk))
            {
                float pw = cap * 0.26f;
                float ph = cap * 0.34f;
                g.FillEllipse(profilFirca, cx - pw * 0.65f, cy - ph * 0.55f, pw, ph);
            }

            using Brush disHarf = new SolidBrush(koyuCizgi);
            float disYaziR = cap * 0.40f;
            for (int a = -150; a <= 150; a += 22)
            {
                if (a > 65 && a < 115) continue;
                double rad = a * Math.PI / 180.0;
                float px = cx + (float)(Math.Cos(rad) * disYaziR);
                float py = cy + (float)(Math.Sin(rad) * disYaziR);
                g.FillRectangle(disHarf, px - 1.5f, py - 1.5f, 3f, 3f);
            }
        }
    }
}
