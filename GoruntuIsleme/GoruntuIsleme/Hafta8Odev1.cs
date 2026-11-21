using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace GoruntuIsleme
{
    public class Hafta8Odev1 : Form
    {
        public class TespitEdilenNesne
        {
            public int EtiketNo { get; set; }
            public int PikselAlani { get; set; }
            public int MinX { get; set; }
            public int MaxX { get; set; }
            public int MinY { get; set; }
            public int MaxY { get; set; }
            public int Genislik { get; set; }
            public int Yukseklik { get; set; }
            public double EnBoyOrani { get; set; }
            public double MerkezX { get; set; }
            public double MerkezY { get; set; }
            public double OrtalamaParlaklik { get; set; }
            public string Sinif { get; set; }
            public Color SinifRengi { get; set; }
        }

        private PictureBox picOrijinal, picIslenmis, picKenar, picSiniflandirilmis;
        private ModernSurgu trkEsik, trkKenarEsik, trkMinAlan, trkCevizEsik;
        private ModernHapRozet rozetOrijinal, rozetIslenmis, rozetKenar, rozetSinif;
        private Label lblMinAlan, lblSonucOzet, lblAltBilgi;
        private CheckBox chkOtoEsik, chkOtoSinif, chkTemasAyir, chkAcikZemin;
        private ComboBox cmbTestSenaryo;
        private Bitmap orijinalResim;

        private static readonly Color CevizRengi = Color.FromArgb(245, 158, 11);  // #F59E0B Amber
        private static readonly Color FindikRengi = Color.FromArgb(16, 185, 129); // #10B981 Zümrüt Yeşili

        public Hafta8Odev1()
        {
            Text = "Hafta 8 - Ödev 1 | Ceviz ve Fındık Tanıma ve Sayma Sistemi";
            FormGezintiYardimcisi.FormuHazirla(this);
            ArayuzuOlustur();
            OrnekSenaryoYukle(0);
        }

        private void ArayuzuOlustur()
        {
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                this,
                "Hafta 8 - Ödev 1 | Ceviz ve Fındık Tanıma ve Sayma Sistemi",
                null, null,
                "Ödev 2 >", () => FormGezintiYardimcisi.FormGecisYap(this, new Hafta8Odev2()));

            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 300);

            Label lblKaynak = new Label
            {
                Text = "GÖRÜNTÜ VE İŞLEMLER",
                Location = new Point(18, 14),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 8.8f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi
            };

            ModernHapButon btnFotoSec = FormGezintiYardimcisi.ModernButonOlustur("Fotoğraf Seç", true, 255, 36);
            btnFotoSec.Location = new Point(18, 38);
            btnFotoSec.Click += (s, e) => LoadImage();

            ModernHapButon btnIsle = FormGezintiYardimcisi.ModernButonOlustur("Görüntüyü İşle", true, 160, 36);
            btnIsle.NormalArkaPlan = FormGezintiYardimcisi.BirincilMavi;
            btnIsle.HoverArkaPlan = Color.FromArgb(2, 132, 199);
            btnIsle.Location = new Point(18, 82);
            btnIsle.Click += (s, e) => GoruntuyuIsle(kullaniciTetikledi: true);

            ModernHapButon btnTemizle = FormGezintiYardimcisi.ModernButonOlustur("Temizle", false, 88, 36);
            btnTemizle.Location = new Point(185, 82);
            btnTemizle.Click += (s, e) => Temizle();

            Label lblSenaryo = new Label
            {
                Text = "Örnek Test Fotoğrafı Seç:",
                Location = new Point(18, 128),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 9f),
                ForeColor = FormGezintiYardimcisi.AcikYazi
            };

            cmbTestSenaryo = new ComboBox
            {
                Location = new Point(18, 150),
                Width = 255,
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

            chkOtoEsik = new CheckBox
            {
                Text = "Otomatik Eşik (Otsu)",
                Checked = true,
                Location = new Point(18, 184),
                AutoSize = true,
                Font = new Font("Segoe UI", 9f),
                ForeColor = FormGezintiYardimcisi.KoyuYazi
            };
            chkOtoEsik.CheckedChanged += (s, e) => { if (orijinalResim != null) GoruntuyuIsle(false); };

            chkOtoSinif = new CheckBox
            {
                Text = "Otomatik Alan Sınıflandırma",
                Checked = true,
                Location = new Point(18, 208),
                AutoSize = true,
                Font = new Font("Segoe UI", 9f),
                ForeColor = FormGezintiYardimcisi.KoyuYazi
            };
            chkOtoSinif.CheckedChanged += (s, e) => { if (orijinalResim != null) GoruntuyuIsle(false); };

            chkTemasAyir = new CheckBox
            {
                Text = "Temas Eden Nesneleri Ayır",
                Checked = true,
                Location = new Point(18, 232),
                AutoSize = true,
                Font = new Font("Segoe UI", 9f),
                ForeColor = FormGezintiYardimcisi.KoyuYazi
            };
            chkTemasAyir.CheckedChanged += (s, e) => { if (orijinalResim != null) GoruntuyuIsle(false); };

            chkAcikZemin = new CheckBox
            {
                Text = "Açık Zemin (Ters Eşikleme)",
                Checked = true,
                Location = new Point(18, 256),
                AutoSize = true,
                Font = new Font("Segoe UI", 9f),
                ForeColor = FormGezintiYardimcisi.KoyuYazi
            };
            chkAcikZemin.CheckedChanged += (s, e) => { if (orijinalResim != null) GoruntuyuIsle(false); };

            lblMinAlan = new Label
            {
                Text = "Min Gürültü Alanı: 180 px",
                Location = new Point(18, 286),
                AutoSize = true,
                Font = new Font("Segoe UI", 8.8f),
                ForeColor = FormGezintiYardimcisi.AcikYazi
            };
            trkMinAlan = new ModernSurgu
            {
                Location = new Point(18, 306),
                Width = 255,
                Minimum = 20,
                Maximum = 600,
                Value = 180
            };
            trkMinAlan.Scroll += (s, e) =>
            {
                lblMinAlan.Text = $"Min Gürültü Alanı: {trkMinAlan.Value} px";
                if (orijinalResim != null) GoruntuyuIsle(false);
            };

            ModernKartPanel kartOzet = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                new Point(18, 342), new Size(255, 235), out lblSonucOzet);

            pnlSag.Controls.AddRange(new Control[]
            {
                lblKaynak, btnFotoSec, btnIsle, btnTemizle, lblSenaryo, cmbTestSenaryo,
                chkOtoEsik, chkOtoSinif, chkTemasAyir, chkAcikZemin,
                lblMinAlan, trkMinAlan, kartOzet
            });

            // 4 Görüntü Kartı (2x2) + Alt Analiz Şeridi
            rozetOrijinal = new ModernHapRozet();
            rozetOrijinal.MetinGuncelle("Orijinal");

            rozetIslenmis = new ModernHapRozet();
            rozetIslenmis.MetinGuncelle("Otsu");

            rozetKenar = new ModernHapRozet();
            rozetKenar.MetinGuncelle("Üst: 80");

            rozetSinif = new ModernHapRozet();
            rozetSinif.MetinGuncelle("Otomatik");

            Panel pnlIslenmisAlt = FormGezintiYardimcisi.KartAltSurguPaneliOlustur(
                "İkili Eşik Değeri", "30 – 245", 30, 245, 200, out trkEsik, out _);
            trkEsik.Scroll += (s, e) =>
            {
                chkOtoEsik.Checked = false;
                rozetIslenmis.MetinGuncelle($"Eşik: {trkEsik.Value}");
                if (orijinalResim != null) GoruntuyuIsle(false);
            };

            Panel pnlKenarAlt = FormGezintiYardimcisi.KartAltSurguPaneliOlustur(
                "Canny Üst Kenar Eşiği", "30 – 180", 30, 180, 80, out trkKenarEsik, out _);
            trkKenarEsik.Scroll += (s, e) =>
            {
                rozetKenar.MetinGuncelle($"Üst: {trkKenarEsik.Value}");
                if (orijinalResim != null) GoruntuyuIsle(false);
            };

            Panel pnlSinifAlt = FormGezintiYardimcisi.KartAltSurguPaneliOlustur(
                "Ceviz / Fındık Alan Eşiği", "300 – 6500 px", 300, 6500, 2200, out trkCevizEsik, out _);
            trkCevizEsik.Scroll += (s, e) =>
            {
                chkOtoSinif.Checked = false;
                rozetSinif.MetinGuncelle($"Manuel: {trkCevizEsik.Value} px");
                if (orijinalResim != null) GoruntuyuIsle(false);
            };

            TableLayoutPanel tbl = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(12),
                ColumnCount = 2,
                RowCount = 3,
                BackColor = FormGezintiYardimcisi.TemaZemin
            };
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 43f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 45f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 12f));

            picOrijinal = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 0, 0, "Orijinal Görüntü", null, null, rozetOrijinal);
            picIslenmis = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 1, 0, "İşlenmiş Görüntü (Temizlenmiş Zemin)", null, pnlIslenmisAlt, rozetIslenmis);
            picKenar = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 0, 1, "Kenar Tespiti (Canny)", null, pnlKenarAlt, rozetKenar);
            picSiniflandirilmis = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 1, 1, "Sınıflandırılmış Görüntü", null, pnlSinifAlt, rozetSinif);

            ModernKartPanel altSerit = FormGezintiYardimcisi.AltAnalizSeridiOlustur(
                "İşlem Hattı: Gri Tonlama -> Gaussian Blur -> Otsu/Eşikleme + Morfolojik Açma/Kapama -> Canny Kenar -> Bölge Ayırma -> Adaptif Sınıflandırma.",
                out lblAltBilgi);
            tbl.Controls.Add(altSerit, 0, 2);
            tbl.SetColumnSpan(altSerit, 2);

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
                picOrijinal.Image = orijinalResim;
                rozetOrijinal.MetinGuncelle($"{orijinalResim.Width}×{orijinalResim.Height}");
                GoruntuyuIsle(kullaniciTetikledi: false);
            }
        }

        public void LoadImage()
        {
            string yol = FormGezintiYardimcisi.ResimDosyasiSec("Ceviz / Fındık Fotoğrafı Seç");
            if (yol == null) return;

            using Bitmap ham = new Bitmap(yol);
            orijinalResim = BitmapYardimcisi.BoyutSinirla(ham, 420);
            picOrijinal.Image = orijinalResim;
            rozetOrijinal.MetinGuncelle($"{orijinalResim.Width}×{orijinalResim.Height}");
            GoruntuyuIsle(kullaniciTetikledi: false);
        }

        public Bitmap ConvertToGray(Bitmap kaynak)
        {
            return BitmapYardimcisi.GriTonaDonustur(kaynak);
        }

        public Bitmap RemoveNoise(Bitmap griResim)
        {
            return BitmapYardimcisi.GaussianBlurUygula(griResim);
        }

        public Bitmap ThresholdImage(Bitmap bulanikGri, out int kullanilanEsik, out Bitmap temizlenmisGoruntu)
        {
            kullanilanEsik = chkOtoEsik.Checked
                ? BitmapYardimcisi.OtsuEsikHesapla(bulanikGri)
                : trkEsik.Value;

            if (chkOtoEsik.Checked)
            {
                trkEsik.Value = kullanilanEsik;
                rozetIslenmis.MetinGuncelle($"Otsu: {kullanilanEsik}");
            }

            using Bitmap hamIkili = BitmapYardimcisi.EsiklemeYap(bulanikGri, kullanilanEsik, chkAcikZemin.Checked);
            Bitmap temizMaske = BitmapYardimcisi.MorfolojiAcmaKapama(hamIkili, acmaTekrar: 1, kapamaTekrar: 1);

            // Arka planı saf beyaza temizleyerek "İşlenmiş Görüntü" çıktısını üret
            int w = orijinalResim.Width;
            int h = orijinalResim.Height;
            temizlenmisGoruntu = new Bitmap(w, h);
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    if (temizMaske.GetPixel(x, y).R > 128)
                        temizlenmisGoruntu.SetPixel(x, y, orijinalResim.GetPixel(x, y));
                    else
                        temizlenmisGoruntu.SetPixel(x, y, Color.White);
                }
            }

            return temizMaske;
        }

        public Bitmap DetectEdges(Bitmap bulanikGri)
        {
            int ust = trkKenarEsik.Value;
            int alt = Math.Max(10, ust * 2 / 5);
            return BitmapYardimcisi.CannyKenarBul(bulanikGri, alt, ust);
        }

        public List<TespitEdilenNesne> FindContours(Bitmap ikiliMaske, out int[,] etiketMatrisi)
        {
            List<BitmapYardimcisi.BolgeBilgisi> hamBolgeler;
            using (Bitmap renkliGecici = chkTemasAyir.Checked
                ? BitmapYardimcisi.TemasEdenBolgeleriAyir(ikiliMaske, trkMinAlan.Value, out hamBolgeler, out etiketMatrisi)
                : BitmapYardimcisi.BolgeBulVeRenklendir(ikiliMaske, trkMinAlan.Value, out hamBolgeler, out etiketMatrisi))
            {
                // yalnızca etiketMatrisi ve hamBolgeler kullanılacak
            }

            List<TespitEdilenNesne> nesneler = new List<TespitEdilenNesne>();
            foreach (var b in hamBolgeler)
            {
                nesneler.Add(new TespitEdilenNesne
                {
                    EtiketNo = b.EtiketNo,
                    PikselAlani = b.PikselSayisi,
                    MinX = b.MinX,
                    MaxX = b.MaxX,
                    MinY = b.MinY,
                    MaxY = b.MaxY,
                    Genislik = b.Genislik,
                    Yukseklik = b.Yukseklik,
                    EnBoyOrani = (double)b.Genislik / Math.Max(1, b.Yukseklik),
                    MerkezX = b.MerkezX,
                    MerkezY = b.MerkezY
                });
            }
            return nesneler;
        }

        public void CalculateArea(List<TespitEdilenNesne> nesneler, int[,] etiketMatrisi, Bitmap orijinal)
        {
            if (nesneler.Count == 0) return;

            Dictionary<int, (int alan, long parlaklikToplam)> istatistik = new Dictionary<int, (int, long)>();
            foreach (var n in nesneler)
                istatistik[n.EtiketNo] = (0, 0);

            int w = orijinal.Width;
            int h = orijinal.Height;
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    int et = etiketMatrisi[x, y];
                    if (et != 0 && istatistik.TryGetValue(et, out var veri))
                    {
                        Color c = orijinal.GetPixel(x, y);
                        int gri = (c.R + c.G + c.B) / 3;
                        istatistik[et] = (veri.alan + 1, veri.parlaklikToplam + gri);
                    }
                }
            }

            foreach (var n in nesneler)
            {
                var veri = istatistik[n.EtiketNo];
                n.PikselAlani = veri.alan;
                n.OrtalamaParlaklik = veri.alan > 0 ? (double)veri.parlaklikToplam / veri.alan : 0;
            }
        }

        public void ClassifyObjects(List<TespitEdilenNesne> nesneler, int resimAlani, out int kullanilanAlanEsigi)
        {
            if (nesneler.Count == 0)
            {
                kullanilanAlanEsigi = trkCevizEsik.Value;
                return;
            }

            if (chkOtoSinif.Checked)
            {
                int minAlan = int.MaxValue;
                int maxAlan = int.MinValue;
                foreach (var n in nesneler)
                {
                    if (n.PikselAlani < minAlan) minAlan = n.PikselAlani;
                    if (n.PikselAlani > maxAlan) maxAlan = n.PikselAlani;
                }

                if (nesneler.Count >= 2 && (double)maxAlan / Math.Max(1, minAlan) >= 1.85)
                {
                    // 1D 2-kümeli K-Means ile fındık ve ceviz küme merkezlerini bul
                    double cFindik = minAlan;
                    double cCeviz = maxAlan;

                    for (int iter = 0; iter < 10; iter++)
                    {
                        double topF = 0, topC = 0;
                        int sayF = 0, sayC = 0;
                        foreach (var n in nesneler)
                        {
                            if (Math.Abs(n.PikselAlani - cFindik) <= Math.Abs(n.PikselAlani - cCeviz))
                            {
                                topF += n.PikselAlani;
                                sayF++;
                            }
                            else
                            {
                                topC += n.PikselAlani;
                                sayC++;
                            }
                        }
                        if (sayF > 0) cFindik = topF / sayF;
                        if (sayC > 0) cCeviz = topC / sayC;
                    }
                    kullanilanAlanEsigi = (int)((cFindik + cCeviz) / 2.0);
                }
                else
                {
                    // Tek tür varsa görüntü alanına göre çözünürlükten bağımsız eşik kullan
                    kullanilanAlanEsigi = Math.Max(400, (int)(resimAlani * 0.018));
                }

                trkCevizEsik.Value = kullanilanAlanEsigi;
                rozetSinif.MetinGuncelle($"Oto: {kullanilanAlanEsigi} px");
            }
            else
            {
                kullanilanAlanEsigi = trkCevizEsik.Value;
                rozetSinif.MetinGuncelle($"Manuel: {kullanilanAlanEsigi} px");
            }

            foreach (var n in nesneler)
            {
                bool cevizMi = n.PikselAlani >= kullanilanAlanEsigi;
                n.Sinif = cevizMi ? "Ceviz" : "Fındık";
                n.SinifRengi = cevizMi ? CevizRengi : FindikRengi;
            }
        }

        public Bitmap ColorObjects(Bitmap orijinal, List<TespitEdilenNesne> nesneler, int[,] etiketMatrisi)
        {
            int w = orijinal.Width;
            int h = orijinal.Height;
            Bitmap sonuc = new Bitmap(orijinal);

            Dictionary<int, TespitEdilenNesne> sozluk = new Dictionary<int, TespitEdilenNesne>();
            foreach (var n in nesneler)
                sozluk[n.EtiketNo] = n;

            for (int x = 1; x < w - 1; x++)
            {
                for (int y = 1; y < h - 1; y++)
                {
                    int et = etiketMatrisi[x, y];
                    if (et == 0 || !sozluk.TryGetValue(et, out var nesne)) continue;

                    bool sinirMi = etiketMatrisi[x - 1, y] != et ||
                                   etiketMatrisi[x + 1, y] != et ||
                                   etiketMatrisi[x, y - 1] != et ||
                                   etiketMatrisi[x, y + 1] != et;

                    if (sinirMi)
                    {
                        sonuc.SetPixel(x, y, nesne.SinifRengi);
                    }
                    else
                    {
                        Color c = orijinal.GetPixel(x, y);
                        int r = (int)(c.R * 0.58 + nesne.SinifRengi.R * 0.42);
                        int g = (int)(c.G * 0.58 + nesne.SinifRengi.G * 0.42);
                        int b = (int)(c.B * 0.58 + nesne.SinifRengi.B * 0.42);
                        sonuc.SetPixel(x, y, Color.FromArgb(r, g, b));
                    }
                }
            }

            using (Graphics g = Graphics.FromImage(sonuc))
            using (Font f = new Font("Segoe UI", 7.8f, FontStyle.Bold))
            {
                int cevizNo = 0, findikNo = 0;
                foreach (var n in nesneler)
                {
                    int sira = n.Sinif == "Ceviz" ? ++cevizNo : ++findikNo;
                    string etiket = $"{n.Sinif} #{sira} ({n.PikselAlani} px)";

                    using Pen kutuKalem = new Pen(n.SinifRengi, 1.5f);
                    g.DrawRectangle(kutuKalem, n.MinX, n.MinY, n.Genislik, n.Yukseklik);

                    SizeF yaziBoyut = g.MeasureString(etiket, f);
                    int yaziX = Math.Clamp(n.MinX, 2, Math.Max(2, w - (int)yaziBoyut.Width - 4));
                    int yaziY = n.MinY >= 16 ? n.MinY - 15 : Math.Min(h - 16, n.MaxY + 2);

                    using SolidBrush arka = new SolidBrush(Color.FromArgb(215, 15, 23, 42));
                    g.FillRectangle(arka, yaziX - 1, yaziY - 1, yaziBoyut.Width + 2, yaziBoyut.Height);
                    g.DrawString(etiket, f, Brushes.White, yaziX, yaziY);
                }
            }

            return sonuc;
        }

        public void CountObjects(
            List<TespitEdilenNesne> nesneler,
            out int cevizSayisi,
            out int findikSayisi,
            out int toplamSayisi)
        {
            cevizSayisi = 0;
            findikSayisi = 0;
            HashSet<int> sayilanEtiketler = new HashSet<int>();

            foreach (var n in nesneler)
            {
                if (!sayilanEtiketler.Add(n.EtiketNo)) continue;
                if (n.Sinif == "Ceviz") cevizSayisi++;
                else findikSayisi++;
            }

            toplamSayisi = cevizSayisi + findikSayisi;
        }

        public void DisplayResults(
            Bitmap temizlenmisGoruntu,
            Bitmap kenarGoruntu,
            Bitmap siniflandirilmisGoruntu,
            List<TespitEdilenNesne> nesneler,
            int cevizSayisi,
            int findikSayisi,
            int toplamSayisi,
            int kullanilanEsik,
            int alanEsigi,
            long sureMs)
        {
            picIslenmis.Image = temizlenmisGoruntu;
            picKenar.Image = kenarGoruntu;
            picSiniflandirilmis.Image = siniflandirilmisGoruntu;

            double topCevizAlan = 0, topFindikAlan = 0;
            foreach (var n in nesneler)
            {
                if (n.Sinif == "Ceviz") topCevizAlan += n.PikselAlani;
                else topFindikAlan += n.PikselAlani;
            }

            string ortCeviz = cevizSayisi > 0 ? $"{topCevizAlan / cevizSayisi:0} px" : "-";
            string ortFindik = findikSayisi > 0 ? $"{topFindikAlan / findikSayisi:0} px" : "-";

            lblSonucOzet.Text = $"SINIFLANDIRMA SONUCU\n\n" +
                                $"• Ceviz Sayısı  : {cevizSayisi}\n" +
                                $"• Fındık Sayısı : {findikSayisi}\n" +
                                $"• Toplam Nesne  : {toplamSayisi}\n\n" +
                                $"• Ort. Ceviz Alanı : {ortCeviz}\n" +
                                $"• Ort. Fındık Alanı: {ortFindik}\n\n" +
                                $"• Eşik Değeri  : {kullanilanEsik}\n" +
                                $"• Alan Sınırı  : {alanEsigi} px\n" +
                                $"• İşlem Süresi : {sureMs} ms";

            lblAltBilgi.Text = toplamSayisi == 0
                ? "Hiç nesne tespit edilemedi. Eşik veya minimum gürültü alanı değerini ayarlayarak tekrar deneyin."
                : $"Tespit Tamamlandı: {cevizSayisi} Ceviz (Amber) ve {findikSayisi} Fındık (Yeşil) ayrıştırıldı; " +
                  $"Otsu/İkili Eşik = {kullanilanEsik}, Adaptif Alan Sınırı = {alanEsigi} px ({sureMs} ms).";
        }

        private void GoruntuyuIsle(bool kullaniciTetikledi)
        {
            if (orijinalResim == null)
            {
                if (kullaniciTetikledi)
                {
                    MessageBox.Show(
                        "Lütfen önce bir fotoğraf seçin veya örnek senaryo yükleyin.",
                        "Fotoğraf Gerekli",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                return;
            }

            Stopwatch sw = Stopwatch.StartNew();

            using Bitmap griResim = ConvertToGray(orijinalResim);
            using Bitmap bulanikGri = RemoveNoise(griResim);
            using Bitmap ikiliMaske = ThresholdImage(bulanikGri, out int kullanilanEsik, out Bitmap temizlenmisGoruntu);
            Bitmap kenarGoruntu = DetectEdges(bulanikGri);

            List<TespitEdilenNesne> nesneler = FindContours(ikiliMaske, out int[,] etiketMatrisi);
            CalculateArea(nesneler, etiketMatrisi, orijinalResim);
            ClassifyObjects(nesneler, orijinalResim.Width * orijinalResim.Height, out int alanEsigi);
            Bitmap siniflandirilmisGoruntu = ColorObjects(orijinalResim, nesneler, etiketMatrisi);
            CountObjects(nesneler, out int cevizSayisi, out int findikSayisi, out int toplamSayisi);

            sw.Stop();

            DisplayResults(
                temizlenmisGoruntu,
                kenarGoruntu,
                siniflandirilmisGoruntu,
                nesneler,
                cevizSayisi,
                findikSayisi,
                toplamSayisi,
                kullanilanEsik,
                alanEsigi,
                sw.ElapsedMilliseconds);
        }

        private void Temizle()
        {
            orijinalResim = null;
            picOrijinal.Image = null;
            picIslenmis.Image = null;
            picKenar.Image = null;
            picSiniflandirilmis.Image = null;

            rozetOrijinal.MetinGuncelle("Boş");
            rozetIslenmis.MetinGuncelle("Otsu");
            rozetSinif.MetinGuncelle("Otomatik");

            lblSonucOzet.Text = "SINIFLANDIRMA SONUCU\n\n" +
                                "• Ceviz Sayısı  : 0\n" +
                                "• Fındık Sayısı : 0\n" +
                                "• Toplam Nesne  : 0\n\n" +
                                "Devam etmek için bir fotoğraf seçin\n" +
                                "veya 'Görüntüyü İşle' butonuna basın.";

            lblAltBilgi.Text = "Ekran temizlendi. Fotoğraf seçip 'Görüntüyü İşle' butonuna basın.";
        }
    }
}
