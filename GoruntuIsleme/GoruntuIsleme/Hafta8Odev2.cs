using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace GoruntuIsleme
{
    public class Hafta8Odev2 : Form
    {
        private PictureBox picKisi, picMaske, picManzara, picSonuc;
        private ModernSurgu trkTolerans;
        private ModernHapRozet rozetKisi, rozetMaske, rozetManzara, rozetSonuc;
        private CheckBox chkBolgeBuyutme, chkKenarYumusat, chkMaskeTemizle;
        private Label lblSeciliRenk, lblIstatistik, lblAltBilgi;
        private Panel pnlRenkOrnek;
        private Bitmap kisiResmi, manzaraResmi;
        private Color arkaPlanRenk = Color.FromArgb(215, 222, 228);
        private Point? seciliTohumNoktasi = null;

        public Hafta8Odev2()
        {
            Text = "Hafta 8 - Ödev 2 | Arka Plan Bölgesini Çıkarma ve Manzara Montajı";
            FormGezintiYardimcisi.FormuHazirla(this);
            ArayuzuOlustur();
            OrnekResimleriOlustur();
        }

        private void ArayuzuOlustur()
        {
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                this,
                "Hafta 8 - Ödev 2 | Arka Plan Bölgesini Çıkarma ve Manzara Montajı",
                "< Ödev 1", () => FormGezintiYardimcisi.FormGecisYap(this, new Hafta8Odev1()),
                "Ödev 3 >", () => FormGezintiYardimcisi.FormGecisYap(this, new Hafta8Odev3()));

            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 295);

            Label lblKaynak = new Label
            {
                Text = "GÖRÜNTÜ KAYNAKLARI",
                Location = new Point(18, 14),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 8.8f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi
            };

            ModernHapButon btnKisiSec = FormGezintiYardimcisi.ModernButonOlustur("Kişi (Ön Plan) Seç", true, 255, 36);
            btnKisiSec.Location = new Point(18, 38);

            ModernHapButon btnManzaraSec = FormGezintiYardimcisi.ModernButonOlustur("Manzara (Arka Plan) Seç", true, 255, 36);
            btnManzaraSec.Location = new Point(18, 82);

            ModernHapButon btnBirlestir = FormGezintiYardimcisi.ModernButonOlustur("Birleştir", true, 155, 36);
            btnBirlestir.NormalArkaPlan = FormGezintiYardimcisi.BirincilMavi;
            btnBirlestir.HoverArkaPlan = Color.FromArgb(2, 132, 199);
            btnBirlestir.Location = new Point(18, 126);

            ModernHapButon btnTemizle = FormGezintiYardimcisi.ModernButonOlustur("Temizle", false, 92, 36);
            btnTemizle.Location = new Point(181, 126);

            ModernHapButon btnOrnek = FormGezintiYardimcisi.ModernButonOlustur("Örnek Görselleri Yükle", false, 255, 34);
            btnOrnek.Location = new Point(18, 170);

            btnKisiSec.Click += (s, e) =>
            {
                string yol = FormGezintiYardimcisi.ResimDosyasiSec("Düz Duvar Önündeki Kişi Fotoğrafı Seç");
                if (yol != null)
                {
                    using Bitmap ham = new Bitmap(yol);
                    kisiResmi = BitmapYardimcisi.BoyutSinirla(ham, 420);
                    picKisi.Image = kisiResmi;
                    rozetKisi.MetinGuncelle($"{kisiResmi.Width}×{kisiResmi.Height}");
                    seciliTohumNoktasi = null;
                    ArkaPlanRengiGuncelle(kisiResmi.GetPixel(2, 2));
                    if (manzaraResmi != null) ResimleriBirlestir(false);
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
                    rozetManzara.MetinGuncelle($"{manzaraResmi.Width}×{manzaraResmi.Height}");
                    if (kisiResmi != null) ResimleriBirlestir(false);
                }
            };

            btnBirlestir.Click += (s, e) => ResimleriBirlestir(kullaniciTetikledi: true);
            btnTemizle.Click += (s, e) => Temizle();
            btnOrnek.Click += (s, e) => OrnekResimleriOlustur();

            lblSeciliRenk = new Label
            {
                Text = "Seçili Duvar Rengi (1. Resme Tıkla):",
                Location = new Point(18, 214),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 9f),
                ForeColor = FormGezintiYardimcisi.KoyuYazi
            };
            pnlRenkOrnek = new Panel
            {
                Location = new Point(18, 236),
                Size = new Size(255, 26),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = arkaPlanRenk
            };

            chkBolgeBuyutme = new CheckBox
            {
                Text = "Bölge Büyütme (Kıyafeti Koru)",
                Checked = true,
                Location = new Point(18, 274),
                AutoSize = true,
                Font = new Font("Segoe UI", 9f),
                ForeColor = FormGezintiYardimcisi.KoyuYazi
            };
            chkBolgeBuyutme.CheckedChanged += (s, e) => { if (kisiResmi != null && manzaraResmi != null) ResimleriBirlestir(false); };

            chkKenarYumusat = new CheckBox
            {
                Text = "Kenar Yumuşatma (Doğal Geçiş)",
                Checked = true,
                Location = new Point(18, 298),
                AutoSize = true,
                Font = new Font("Segoe UI", 9f),
                ForeColor = FormGezintiYardimcisi.KoyuYazi
            };
            chkKenarYumusat.CheckedChanged += (s, e) => { if (kisiResmi != null && manzaraResmi != null) ResimleriBirlestir(false); };

            chkMaskeTemizle = new CheckBox
            {
                Text = "Maske Gürültü Temizliği",
                Checked = true,
                Location = new Point(18, 322),
                AutoSize = true,
                Font = new Font("Segoe UI", 9f),
                ForeColor = FormGezintiYardimcisi.KoyuYazi
            };
            chkMaskeTemizle.CheckedChanged += (s, e) => { if (kisiResmi != null && manzaraResmi != null) ResimleriBirlestir(false); };

            ModernKartPanel kartIstatistik = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                new Point(18, 354), new Size(255, 200), out lblIstatistik);

            pnlSag.Controls.AddRange(new Control[]
            {
                lblKaynak, btnKisiSec, btnManzaraSec, btnBirlestir, btnTemizle, btnOrnek,
                lblSeciliRenk, pnlRenkOrnek,
                chkBolgeBuyutme, chkKenarYumusat, chkMaskeTemizle,
                kartIstatistik
            });

            rozetKisi = new ModernHapRozet();
            rozetKisi.MetinGuncelle("Ön Plan");

            rozetMaske = new ModernHapRozet();
            rozetMaske.MetinGuncelle("%0 Kişi");

            rozetManzara = new ModernHapRozet();
            rozetManzara.MetinGuncelle("Manzara");

            rozetSonuc = new ModernHapRozet();
            rozetSonuc.MetinGuncelle("Tolerans: 50");

            Panel pnlToleransAlt = FormGezintiYardimcisi.KartAltSurguPaneliOlustur(
                "Renk Benzerlik Toleransı", "5 – 150", 5, 150, 50, out trkTolerans, out _);
            trkTolerans.Scroll += (s, e) =>
            {
                rozetSonuc.MetinGuncelle($"Tolerans: {trkTolerans.Value}");
                if (kisiResmi != null && manzaraResmi != null) ResimleriBirlestir(false);
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
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 44f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 44f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 12f));

            picKisi = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 0, 0, "Ön Plan - Kişi (Duvar Rengi İçin Tıkla)", null, null, rozetKisi);
            picMaske = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 1, 0, "Ayrıştırılan Kişi Maskesi (Bölge Büyütme)", null, null, rozetMaske);
            picManzara = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 0, 1, "Yeni Arka Plan (Manzara)", null, null, rozetManzara);
            picSonuc = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 1, 1, "Birleştirilmiş Manzara Montajı", null, pnlToleransAlt, rozetSonuc);

            picKisi.Cursor = Cursors.Cross;
            picKisi.MouseClick += PicKisi_MouseClick;

            ModernKartPanel altSerit = FormGezintiYardimcisi.AltAnalizSeridiOlustur(
                "Segmentasyon Özeti: Sınır ve duvar tohumlarından BFS Bölge Büyütme ile yalnızca arka plan çıkarılır; kişinin açık renkli kıyafetleri korunur.",
                out lblAltBilgi);
            tbl.Controls.Add(altSerit, 0, 2);
            tbl.SetColumnSpan(altSerit, 2);

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
                seciliTohumNoktasi = new Point(bmpX, bmpY);
                ArkaPlanRengiGuncelle(kisiResmi.GetPixel(bmpX, bmpY));
                if (manzaraResmi != null) ResimleriBirlestir(false);
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
                rozetKisi.MetinGuncelle($"{kisiResmi.Width}×{kisiResmi.Height}");
                rozetManzara.MetinGuncelle($"{manzaraResmi.Width}×{manzaraResmi.Height}");
                seciliTohumNoktasi = null;
                ArkaPlanRengiGuncelle(kisiResmi.GetPixel(8, 8));
                ResimleriBirlestir(false);
            }
        }

        public static double RenkMesafesi(Color a, Color b)
        {
            int dr = a.R - b.R;
            int dg = a.G - b.G;
            int db = a.B - b.B;
            return Math.Sqrt(dr * dr + dg * dg + db * db);
        }

        /// <summary>
        /// Bölge Büyütme (4-komşuluk BFS) veya global renk eşiklemesiyle arka plan piksellerini tespit eder.
        /// Üst, sol ve sağ kenar tohumlarından büyütme yapıldığı için kişinin beyaz tişört/diş gibi iç açık renkleri korunur.
        /// </summary>
        public static bool[,] ArkaPlanBolgesiniBul(
            Color[,] pikseller,
            int w,
            int h,
            Color arkaPlanRenk,
            int tolerans,
            bool bolgeBuyutmeKullan,
            Point? seciliTohumNoktasi = null)
        {
            bool[,] arkaPlanMi = new bool[w, h];

            if (!bolgeBuyutmeKullan)
            {
                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        if (RenkMesafesi(pikseller[x, y], arkaPlanRenk) <= tolerans)
                            arkaPlanMi[x, y] = true;
                    }
                }
                return arkaPlanMi;
            }

            Queue<(int x, int y)> kuyruk = new Queue<(int x, int y)>();

            void DeneyipEkle(int x, int y)
            {
                if (!arkaPlanMi[x, y] && RenkMesafesi(pikseller[x, y], arkaPlanRenk) <= tolerans)
                {
                    arkaPlanMi[x, y] = true;
                    kuyruk.Enqueue((x, y));
                }
            }

            // Üst kenar tohumları
            for (int x = 0; x < w; x++)
                DeneyipEkle(x, 0);

            // Sol ve sağ kenar tohumları (alt kenar kişinin gövdesini korumak için tohumlanmaz)
            for (int y = 0; y < h; y++)
            {
                DeneyipEkle(0, y);
                DeneyipEkle(w - 1, y);
            }

            if (seciliTohumNoktasi.HasValue)
            {
                int sx = Math.Clamp(seciliTohumNoktasi.Value.X, 0, w - 1);
                int sy = Math.Clamp(seciliTohumNoktasi.Value.Y, 0, h - 1);
                DeneyipEkle(sx, sy);
            }

            int[] dx = { -1, 1, 0, 0 };
            int[] dy = { 0, 0, -1, 1 };

            while (kuyruk.Count > 0)
            {
                var p = kuyruk.Dequeue();
                for (int k = 0; k < 4; k++)
                {
                    int nx = p.x + dx[k];
                    int ny = p.y + dy[k];
                    if (nx >= 0 && nx < w && ny >= 0 && ny < h &&
                        !arkaPlanMi[nx, ny] &&
                        RenkMesafesi(pikseller[nx, ny], arkaPlanRenk) <= tolerans)
                    {
                        arkaPlanMi[nx, ny] = true;
                        kuyruk.Enqueue((nx, ny));
                    }
                }
            }

            return arkaPlanMi;
        }

        private static double[,] OnPlanAlfasiniHesapla(
            bool[,] arkaPlanMi,
            int w,
            int h,
            bool maskeTemizle,
            bool kenarYumusat,
            out int onPlanPikselSayisi)
        {
            bool[,] onPlan = new bool[w, h];
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    onPlan[x, y] = !arkaPlanMi[x, y];

            if (maskeTemizle)
                onPlan = BitmapYardimcisi.MaskeAcmaKapama(onPlan, w, h, acmaTekrar: 1, kapamaTekrar: 1);

            onPlanPikselSayisi = 0;
            double[,] alfa = new double[w, h];
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    if (onPlan[x, y])
                    {
                        alfa[x, y] = 1.0;
                        onPlanPikselSayisi++;
                    }
                }
            }

            if (kenarYumusat)
            {
                for (int pass = 0; pass < 2; pass++)
                {
                    double[,] yeniAlfa = (double[,])alfa.Clone();
                    for (int x = 1; x < w - 1; x++)
                    {
                        for (int y = 1; y < h - 1; y++)
                        {
                            bool sifirVar = false;
                            bool birVar = false;
                            double toplam = 0;

                            for (int i = -1; i <= 1; i++)
                            {
                                for (int j = -1; j <= 1; j++)
                                {
                                    double deger = alfa[x + i, y + j];
                                    toplam += deger;
                                    if (deger < 0.99) sifirVar = true;
                                    if (deger > 0.01) birVar = true;
                                }
                            }

                            if (sifirVar && birVar)
                                yeniAlfa[x, y] = toplam / 9.0;
                        }
                    }
                    alfa = yeniAlfa;
                }
            }

            return alfa;
        }

        private void ResimleriBirlestir(bool kullaniciTetikledi)
        {
            if (kisiResmi == null || manzaraResmi == null)
            {
                if (kullaniciTetikledi)
                {
                    MessageBox.Show(
                        "Lütfen hem kişi (ön plan) hem de manzara (arka plan) fotoğrafını seçin.",
                        "Eksik Görüntü",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                return;
            }

            Stopwatch sw = Stopwatch.StartNew();

            int w = kisiResmi.Width;
            int h = kisiResmi.Height;
            Color[,] pikseller = new Color[w, h];
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    pikseller[x, y] = kisiResmi.GetPixel(x, y);

            bool[,] arkaPlanMi = ArkaPlanBolgesiniBul(
                pikseller, w, h, arkaPlanRenk, trkTolerans.Value,
                chkBolgeBuyutme.Checked, seciliTohumNoktasi);

            double[,] alfa = OnPlanAlfasiniHesapla(
                arkaPlanMi, w, h, chkMaskeTemizle.Checked, chkKenarYumusat.Checked,
                out int onPlanPikselSayisi);

            using Bitmap olcekliManzara = new Bitmap(manzaraResmi, w, h);
            Bitmap maskeBmp = new Bitmap(w, h);
            Bitmap sonucBmp = new Bitmap(w, h);

            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    double a = alfa[x, y];
                    int maskeDeger = (int)Math.Round(a * 255);
                    maskeBmp.SetPixel(x, y, Color.FromArgb(maskeDeger, maskeDeger, maskeDeger));

                    Color kisi = pikseller[x, y];
                    Color manzara = olcekliManzara.GetPixel(x, y);
                    int r = Math.Clamp((int)(kisi.R * a + manzara.R * (1.0 - a)), 0, 255);
                    int g = Math.Clamp((int)(kisi.G * a + manzara.G * (1.0 - a)), 0, 255);
                    int b = Math.Clamp((int)(kisi.B * a + manzara.B * (1.0 - a)), 0, 255);
                    sonucBmp.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }

            sw.Stop();

            picMaske.Image = maskeBmp;
            picSonuc.Image = sonucBmp;

            int toplamPiksel = w * h;
            double kisiYuzde = 100.0 * onPlanPikselSayisi / Math.Max(1, toplamPiksel);
            double duvarYuzde = 100.0 - kisiYuzde;
            string yontem = chkBolgeBuyutme.Checked ? "Bölge Büyütme (BFS)" : "Global Renk Eşiği";

            rozetMaske.MetinGuncelle($"%{kisiYuzde:0.0} Kişi");

            lblIstatistik.Text = $"SEGMENTASYON ÖZETİ\n\n" +
                                 $"• Yöntem       : {yontem}\n" +
                                 $"• Kişi Alanı   : %{kisiYuzde:0.0} ({onPlanPikselSayisi} px)\n" +
                                 $"• Çıkarılan Duvar: %{duvarYuzde:0.0}\n" +
                                 $"• Tolerans     : {trkTolerans.Value}\n" +
                                 $"• Çözünürlük   : {w} × {h}\n" +
                                 $"• İşlem Süresi : {sw.ElapsedMilliseconds} ms";

            lblAltBilgi.Text = $"Montaj Tamamlandı: {yontem} ile arka plan (%{duvarYuzde:0.0}) temizlendi; " +
                               $"kişi silüeti (%{kisiYuzde:0.0}) yeni manzara üzerine birleştirildi ({sw.ElapsedMilliseconds} ms).";
        }

        private void Temizle()
        {
            kisiResmi = null;
            manzaraResmi = null;
            seciliTohumNoktasi = null;
            picKisi.Image = null;
            picMaske.Image = null;
            picManzara.Image = null;
            picSonuc.Image = null;

            rozetKisi.MetinGuncelle("Boş");
            rozetMaske.MetinGuncelle("%0 Kişi");
            rozetManzara.MetinGuncelle("Boş");

            lblIstatistik.Text = "SEGMENTASYON ÖZETİ\n\n" +
                                 "Görüntüler temizlendi.\n" +
                                 "Ön plan ve manzara fotoğrafı seçip\n" +
                                 "'Birleştir' butonuna basın.";

            lblAltBilgi.Text = "Ekran temizlendi. Kişi ve manzara fotoğrafı seçin.";
        }
    }
}
