using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GoruntuIsleme
{
    public class BantUrunSonucu
    {
        public Bitmap FarkResmi { get; set; }
        public Bitmap IkiliYsaMaskesi { get; set; }
        public bool[,] UrunMaskesi { get; set; }
        public int MedyanIsikFarki { get; set; }
        public int UrunPikselSayisi { get; set; }
        public double UrunAlaniYuzde { get; set; }
        public int BastirilanGolgePikseli { get; set; }
    }

    public class Hafta9Odev4 : Form
    {
        private enum AkisModu
        {
            Statik,
            Simulasyon,
            Kamera
        }

        private PictureBox picBantBos, picBantUrun, picFark, picIkiliSonuc;
        private ModernSurgu trkEsik;
        private ModernHapRozet rozetBos, rozetUrun, rozetFark, rozetMaske;
        private ModernHapButon btnSimulasyon, btnKamera, btnBosKaydet;
        private CheckBox chkUyarlamaliArkaPlan, chkIsikDengele, chkGolgeBastir, chkMorfoloji;
        private Label lblAltAnaliz;

        private Bitmap bosBant, urunluBant;
        private OpenCvSharp.VideoCapture videoYakala;
        private System.Windows.Forms.Timer akisZamanlayici;
        private AkisModu aktifAkis = AkisModu.Statik;
        private bool kareIsleniyor = false;
        private int simKareNo = 0;

        public Hafta9Odev4()
        {
            Text = "Hafta 9 - Ödev 4 | Değişken Işıklı Yürüyen Bantta Ürün Ayırma";
            FormGezintiYardimcisi.FormuHazirla(this);
            akisZamanlayici = new System.Windows.Forms.Timer { Interval = 85 };
            akisZamanlayici.Tick += AkisZamanlayici_Tick;

            ArayuzuOlustur();
            OrnekBantResimleriYukle();

            this.FormClosing += (s, e) =>
            {
                AkisiDurdur();
                bosBant?.Dispose();
                bosBant = null;
                urunluBant?.Dispose();
                urunluBant = null;
            };
        }

        private void ArayuzuOlustur()
        {
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                this,
                "Hafta 9 - Ödev 4 | Değişken Işıklı Yürüyen Bantta Ürün Ayırma (YSA İçin Siyah-Beyaz)",
                "< Ödev 3", () => { AkisiDurdur(); FormGezintiYardimcisi.FormGecisYap(this, new Hafta9Odev3()); },
                "Ödev 5 >", () => { AkisiDurdur(); FormGezintiYardimcisi.FormGecisYap(this, new Hafta9Odev5()); },
                false,
                "HAFTA 9 • ÖDEV 4");

            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 305);
            pnlSag.AutoScroll = true;

            int x = 16;
            int w = 262;
            int y = 12;

            Label lblCanli = new Label
            {
                Text = "CANLI BANT / KAMERA AKIŞI",
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi
            };
            y += 20;

            btnSimulasyon = FormGezintiYardimcisi.ModernButonOlustur("Bant Simülasyonu (Gündüz → Akşam)", true, w, 34);
            btnSimulasyon.Location = new Point(x, y);
            btnSimulasyon.Click += BtnSimulasyon_Click;
            y += 40;

            btnKamera = FormGezintiYardimcisi.ModernButonOlustur("Sabit Kamera Başlat", false, w, 34);
            btnKamera.Location = new Point(x, y);
            btnKamera.Click += BtnKamera_Click;
            y += 40;

            btnBosKaydet = FormGezintiYardimcisi.ModernButonOlustur("Anlık Kareyi Boş Bant Yap", false, w, 30);
            btnBosKaydet.Location = new Point(x, y);
            btnBosKaydet.Enabled = false;
            btnBosKaydet.Click += BtnBosKaydet_Click;
            y += 38;

            Label lblKaynak = new Label
            {
                Text = "FOTOĞRAF / ÖRNEK KAYNAKLAR",
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi
            };
            y += 20;

            ModernHapButon btnBosSec = FormGezintiYardimcisi.ModernButonOlustur("Boş Gölgeli Bant/Kağıt Seç", false, w, 32);
            btnBosSec.Location = new Point(x, y);
            y += 38;

            ModernHapButon btnUrunSec = FormGezintiYardimcisi.ModernButonOlustur("Ürünlü Bant/Kaşık Seç", false, w, 32);
            btnUrunSec.Location = new Point(x, y);
            y += 38;

            ModernHapButon btnOrnek = FormGezintiYardimcisi.ModernButonOlustur("Örnek: Gölgeli Kağıt + Kaşık", false, w, 32);
            btnOrnek.Location = new Point(x, y);
            y += 38;

            ModernHapButon btnTemizle = FormGezintiYardimcisi.ModernButonOlustur("Temizle", false, w, 30);
            btnTemizle.Location = new Point(x, y);
            y += 38;

            btnBosSec.Click += (s, e) =>
            {
                string yol = FormGezintiYardimcisi.ResimDosyasiSec("Gölgeli Boş Arka Plan (Bant/Kağıt) Seç");
                if (yol == null) return;
                AkisiDurdur();
                using Bitmap ham = new Bitmap(yol);
                bosBant?.Dispose();
                bosBant = BitmapYardimcisi.BoyutSinirla(ham, 380);
                ResmiAta(picBantBos, (Bitmap)bosBant.Clone());
                rozetBos.MetinGuncelle("Seçilen Zemin");
                Uygula();
            };
            btnUrunSec.Click += (s, e) =>
            {
                string yol = FormGezintiYardimcisi.ResimDosyasiSec("Üzerinde Ürün Bulunan Bant/Kağıt Seç");
                if (yol == null) return;
                AkisiDurdur();
                using Bitmap ham = new Bitmap(yol);
                urunluBant?.Dispose();
                urunluBant = BitmapYardimcisi.BoyutSinirla(ham, 380);
                ResmiAta(picBantUrun, (Bitmap)urunluBant.Clone());
                rozetUrun.MetinGuncelle("Seçilen Ürün");
                Uygula();
            };
            btnOrnek.Click += (s, e) => OrnekBantResimleriYukle();
            btnTemizle.Click += (s, e) => Temizle();

            chkUyarlamaliArkaPlan = new CheckBox
            {
                Text = "Uyarlamalı Arka Plan (B_t Güncelle)",
                Location = new Point(x, y),
                Size = new Size(w, 22),
                Font = new Font("Segoe UI", 8.4f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi,
                Checked = true
            };
            y += 24;

            chkIsikDengele = new CheckBox
            {
                Text = "Medyan Işık Farkını Dengele",
                Location = new Point(x, y),
                Size = new Size(w, 22),
                Font = new Font("Segoe UI", 8.4f),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                Checked = true
            };
            chkIsikDengele.CheckedChanged += (s, e) => Uygula();
            y += 24;

            chkGolgeBastir = new CheckBox
            {
                Text = "Ürün Gölgesini Bastır (Kromatiklik)",
                Location = new Point(x, y),
                Size = new Size(w, 22),
                Font = new Font("Segoe UI", 8.4f),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                Checked = true
            };
            chkGolgeBastir.CheckedChanged += (s, e) => Uygula();
            y += 24;

            chkMorfoloji = new CheckBox
            {
                Text = "Gürültü Temizle & İç Delik Doldur (YSA)",
                Location = new Point(x, y),
                Size = new Size(w, 22),
                Font = new Font("Segoe UI", 8.4f),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                Checked = true
            };
            chkMorfoloji.CheckedChanged += (s, e) => Uygula();
            y += 30;

            ModernKartPanel kartBilgi = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                new Point(x, y), new Size(w, 178), out Label lblBilgi);
            kartBilgi.Margin = new Padding(0, 0, 0, 14);
            lblBilgi.Font = new Font("Segoe UI", 8.2f);
            lblBilgi.Text = "Fabrika & YSA Algılama Mantığı\n\n" +
                            "1. Gündüz/akşam ve hareket eden gölgeler yalnızca ürün olmayan piksellerde B_t=(1-α)B+αI ile güncellenir.\n\n" +
                            "2. Medyan fark + RGB kromatiklik oranı ürünün kendi gölgesini bastırır.\n\n" +
                            "3. İç delikler doldurulup YSA için kesin 2 renkli (Siyah-Beyaz) maske üretilir.";

            pnlSag.Controls.AddRange(new Control[]
            {
                lblCanli, btnSimulasyon, btnKamera, btnBosKaydet,
                lblKaynak, btnBosSec, btnUrunSec, btnOrnek, btnTemizle,
                chkUyarlamaliArkaPlan, chkIsikDengele, chkGolgeBastir, chkMorfoloji,
                kartBilgi
            });

            rozetBos = new ModernHapRozet();
            rozetBos.MetinGuncelle("Referans");

            rozetUrun = new ModernHapRozet();
            rozetUrun.MetinGuncelle("Ürünlü");

            rozetFark = new ModernHapRozet();
            rozetFark.MetinGuncelle("Medyan: 0");

            rozetMaske = new ModernHapRozet();
            rozetMaske.MetinGuncelle("Eşik: 30");

            Panel pnlMaskeAlt = FormGezintiYardimcisi.KartAltSurguPaneliOlustur(
                "Ayırma Eşik Değeri", "5 – 120", 5, 120, 30, out trkEsik, out _);
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
                RowCount = 3,
                BackColor = FormGezintiYardimcisi.TemaZemin
            };
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 43f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 45f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 12f));

            picBantBos = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 0, 0, "Boş Bant / Arka Plan (B_t)", null, null, rozetBos);
            picBantUrun = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 1, 0, "Anlık Bant / Kaşık Görüntüsü (I_t)", null, null, rozetUrun);
            picFark = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 0, 1, "Medyan Işık Dengeli Fark", null, null, rozetFark);
            picIkiliSonuc = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 1, 1, "YSA İçin 2 Renkli (Siyah-Beyaz) Maske", null, pnlMaskeAlt, rozetMaske);

            ModernKartPanel altSerit = FormGezintiYardimcisi.AltAnalizSeridiOlustur(
                "Analiz hazır.",
                out lblAltAnaliz,
                "YSA Maske");
            tbl.Controls.Add(altSerit, 0, 2);
            tbl.SetColumnSpan(altSerit, 2);

            Controls.Add(tbl);
            FormGezintiYardimcisi.YerlesimiDuzenle(this, pnlUst, pnlSag, tbl);
        }

        private void BtnSimulasyon_Click(object sender, EventArgs e)
        {
            if (aktifAkis == AkisModu.Simulasyon)
            {
                AkisiDurdur();
                return;
            }

            AkisiDurdur();
            aktifAkis = AkisModu.Simulasyon;
            simKareNo = 0;

            bosBant?.Dispose();
            bosBant = BantSimulasyonKaresiUret(0, out string donem, out _);
            ResmiAta(picBantBos, (Bitmap)bosBant.Clone());
            rozetBos.MetinGuncelle(chkUyarlamaliArkaPlan.Checked ? $"B_t ({donem})" : "Sabit Gündüz");

            btnSimulasyon.Text = "Simülasyonu Durdur";
            btnBosKaydet.Enabled = true;
            akisZamanlayici.Start();
        }

        private void BtnKamera_Click(object sender, EventArgs e)
        {
            if (aktifAkis == AkisModu.Kamera)
            {
                AkisiDurdur();
                return;
            }

            AkisiDurdur();
            OpenCvSharp.VideoCapture yeni = new OpenCvSharp.VideoCapture(0);
            if (!yeni.IsOpened())
            {
                yeni.Dispose();
                MessageBox.Show("Sistemde kullanılabilir bir sabit kamera bulunamadı.",
                    "Kamera Açılamadı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            videoYakala = yeni;
            aktifAkis = AkisModu.Kamera;
            btnKamera.Text = "Kamerayı Durdur";
            btnBosKaydet.Enabled = true;
            akisZamanlayici.Start();
        }

        private void BtnBosKaydet_Click(object sender, EventArgs e)
        {
            if (urunluBant == null) return;
            bosBant?.Dispose();
            bosBant = (Bitmap)urunluBant.Clone();
            ResmiAta(picBantBos, (Bitmap)bosBant.Clone());
            rozetBos.MetinGuncelle("Güncel Bant");
            Uygula();
        }

        private void AkisiDurdur()
        {
            akisZamanlayici?.Stop();
            if (videoYakala != null)
            {
                videoYakala.Release();
                videoYakala.Dispose();
                videoYakala = null;
            }
            aktifAkis = AkisModu.Statik;
            if (btnSimulasyon != null) btnSimulasyon.Text = "Bant Simülasyonu (Gündüz → Akşam)";
            if (btnKamera != null) btnKamera.Text = "Sabit Kamera Başlat";
            if (btnBosKaydet != null) btnBosKaydet.Enabled = false;
        }

        private void AkisZamanlayici_Tick(object sender, EventArgs e)
        {
            if (IsDisposed || kareIsleniyor || aktifAkis == AkisModu.Statik) return;

            kareIsleniyor = true;
            try
            {
                Bitmap yeniKare = null;
                string etiket = "Kamera";

                if (aktifAkis == AkisModu.Simulasyon)
                {
                    yeniKare = BantSimulasyonKaresiUret(simKareNo++, out etiket, out bool urunVar);
                    rozetUrun.MetinGuncelle($"{etiket} {(urunVar ? "• Ürün" : "• Boş")}");
                }
                else if (videoYakala != null && videoYakala.IsOpened())
                {
                    using OpenCvSharp.Mat mat = new OpenCvSharp.Mat();
                    if (!videoYakala.Read(mat) || mat.Empty())
                    {
                        AkisiDurdur();
                        return;
                    }
                    using Bitmap ham = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(mat);
                    yeniKare = BitmapYardimcisi.BoyutSinirla(ham, 320);
                    if (bosBant == null)
                        bosBant = (Bitmap)yeniKare.Clone();
                    rozetUrun.MetinGuncelle("Canlı Kare");
                }

                if (yeniKare == null || bosBant == null) return;

                urunluBant?.Dispose();
                urunluBant = yeniKare;
                ResmiAta(picBantUrun, (Bitmap)urunluBant.Clone());

                BantUrunSonucu sonuc = Uygula();
                if (sonuc != null && chkUyarlamaliArkaPlan.Checked)
                {
                    Bitmap guncelBos = UyarlamaliArkaPlanGuncelle(bosBant, urunluBant, sonuc.UrunMaskesi, 0.22);
                    bosBant.Dispose();
                    bosBant = guncelBos;
                    ResmiAta(picBantBos, (Bitmap)bosBant.Clone());
                    rozetBos.MetinGuncelle($"Uyarlamalı ({etiket})");
                }
                else if (aktifAkis == AkisModu.Simulasyon)
                {
                    rozetBos.MetinGuncelle("Sabit Referans");
                }
            }
            catch
            {
                AkisiDurdur();
            }
            finally
            {
                kareIsleniyor = false;
            }
        }

        private void OrnekBantResimleriYukle()
        {
            AkisiDurdur();
            Bitmap bos = BitmapYardimcisi.OrnekGorselYukle("bant_bos.jpg");
            Bitmap urun = BitmapYardimcisi.OrnekGorselYukle("bant_kasik.jpg");
            if (bos == null || urun == null)
            {
                bos?.Dispose();
                urun?.Dispose();
                bos = BantSimulasyonKaresiUret(0, out _, out _);
                urun = BantSimulasyonKaresiUret(15, out _, out _);
            }

            bosBant?.Dispose();
            bosBant = bos;
            urunluBant?.Dispose();
            urunluBant = urun;

            ResmiAta(picBantBos, (Bitmap)bosBant.Clone());
            ResmiAta(picBantUrun, (Bitmap)urunluBant.Clone());
            rozetBos.MetinGuncelle("Gölgeli Boş Zemin");
            rozetUrun.MetinGuncelle("Kaşık & Gölge");
            Uygula();
        }

        private void Temizle()
        {
            AkisiDurdur();
            bosBant?.Dispose();
            bosBant = null;
            urunluBant?.Dispose();
            urunluBant = null;

            ResmiAta(picBantBos, null);
            ResmiAta(picBantUrun, null);
            ResmiAta(picFark, null);
            ResmiAta(picIkiliSonuc, null);

            rozetBos.MetinGuncelle("Boş");
            rozetUrun.MetinGuncelle("Boş");
            rozetFark.MetinGuncelle("Medyan: 0");
            lblAltAnaliz.Text = "Görseller temizlendi. Simülasyon başlatın veya kaşık/bant görsellerini seçin.";
        }

        private static void ResmiAta(PictureBox pic, Bitmap yeni)
        {
            Image onceki = pic.Image;
            pic.Image = yeni;
            onceki?.Dispose();
        }

        private BantUrunSonucu Uygula()
        {
            if (bosBant == null || urunluBant == null) return null;

            BantUrunSonucu sonuc = BanttanUrunAyir(
                bosBant,
                urunluBant,
                trkEsik.Value,
                chkIsikDengele.Checked,
                chkGolgeBastir.Checked,
                chkMorfoloji.Checked);

            ResmiAta(picFark, sonuc.FarkResmi);
            ResmiAta(picIkiliSonuc, sonuc.IkiliYsaMaskesi);

            rozetFark.MetinGuncelle($"Medyan: {(sonuc.MedyanIsikFarki >= 0 ? "+" : "")}{sonuc.MedyanIsikFarki}");
            lblAltAnaliz.Text = $"YSA 2-Renk Maske: {sonuc.UrunPikselSayisi} ürün px (%{sonuc.UrunAlaniYuzde:F2}) | Bastırılan gölge: {sonuc.BastirilanGolgePikseli} px | Medyan ışık kayması: {sonuc.MedyanIsikFarki}";
            return sonuc;
        }

        public static BantUrunSonucu BanttanUrunAyir(
            Bitmap bosBant,
            Bitmap urunluBant,
            int esikDegeri,
            bool isikDengele,
            bool golgeBastir,
            bool morfolojiVeDelikDoldur)
        {
            BantUrunSonucu sonuc = new BantUrunSonucu();
            if (bosBant == null || urunluBant == null) return sonuc;

            int w = bosBant.Width;
            int h = bosBant.Height;
            bool ayniBoyut = urunluBant.Width == w && urunluBant.Height == h;
            using Bitmap urunOlcekli = ayniBoyut ? null : new Bitmap(urunluBant, w, h);
            Bitmap urunKaynak = ayniBoyut ? urunluBant : urunOlcekli;

            Color[,] cBos = new Color[w, h];
            Color[,] cUrun = new Color[w, h];
            byte[,] gBos = new byte[w, h];
            byte[,] gUrun = new byte[w, h];

            int[] farkHist = isikDengele ? new int[511] : null;
            int ornekSay = 0;

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    Color cb = bosBant.GetPixel(x, y);
                    Color cu = urunKaynak.GetPixel(x, y);
                    cBos[x, y] = cb;
                    cUrun[x, y] = cu;
                    byte gb = (byte)((cb.R + cb.G + cb.B) / 3);
                    byte gu = (byte)((cu.R + cu.G + cu.B) / 3);
                    gBos[x, y] = gb;
                    gUrun[x, y] = gu;

                    if (farkHist != null)
                    {
                        farkHist[gu - gb + 255]++;
                        ornekSay++;
                    }
                }
            }

            int medyanFark = 0;
            if (farkHist != null && ornekSay > 0)
            {
                int birikimli = 0;
                int hedef = ornekSay / 2;
                for (int i = 0; i < farkHist.Length; i++)
                {
                    birikimli += farkHist[i];
                    if (birikimli >= hedef)
                    {
                        medyanFark = i - 255;
                        break;
                    }
                }
            }

            Bitmap farkBmp = new Bitmap(w, h);
            bool[,] maske = new bool[w, h];
            int bastirilanGolge = 0;

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int guDengeli = Math.Clamp(gUrun[x, y] - medyanFark, 0, 255);
                    int fark = Math.Abs(guDengeli - gBos[x, y]);
                    farkBmp.SetPixel(x, y, Color.FromArgb(fark, fark, fark));

                    bool onPlan = fark >= esikDegeri;
                    if (onPlan && golgeBastir)
                    {
                        double oran = guDengeli / Math.Max(1.0, (double)gBos[x, y]);
                        Color cb = cBos[x, y];
                        Color cu = cUrun[x, y];
                        double sb = Math.Max(1.0, cb.R + cb.G + cb.B);
                        double su = Math.Max(1.0, cu.R + cu.G + cu.B);
                        double kromaFark = Math.Abs(cb.R / sb - cu.R / su)
                                         + Math.Abs(cb.G / sb - cu.G / su)
                                         + Math.Abs(cb.B / sb - cu.B / su);

                        if (oran >= 0.52 && oran <= 0.94 && kromaFark <= 0.055)
                        {
                            onPlan = false;
                            bastirilanGolge++;
                        }
                    }

                    maske[x, y] = onPlan;
                }
            }

            if (morfolojiVeDelikDoldur)
            {
                maske = BitmapYardimcisi.MaskeAcmaKapama(maske, w, h, acmaTekrar: 1, kapamaTekrar: 1);
                IcDelikleriDoldur(maske, w, h);
            }

            Bitmap ikiliBmp = new Bitmap(w, h);
            int urunPiksel = 0;
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    if (maske[x, y])
                    {
                        urunPiksel++;
                        ikiliBmp.SetPixel(x, y, Color.White);
                    }
                    else
                    {
                        ikiliBmp.SetPixel(x, y, Color.Black);
                    }
                }
            }

            sonuc.FarkResmi = farkBmp;
            sonuc.IkiliYsaMaskesi = ikiliBmp;
            sonuc.UrunMaskesi = maske;
            sonuc.MedyanIsikFarki = medyanFark;
            sonuc.UrunPikselSayisi = urunPiksel;
            sonuc.UrunAlaniYuzde = 100.0 * urunPiksel / Math.Max(1, w * h);
            sonuc.BastirilanGolgePikseli = bastirilanGolge;
            return sonuc;
        }

        private static void IcDelikleriDoldur(bool[,] maske, int w, int h)
        {
            bool[,] disZemin = new bool[w, h];
            Queue<(int x, int y)> q = new Queue<(int x, int y)>();

            for (int x = 0; x < w; x++)
            {
                if (!maske[x, 0] && !disZemin[x, 0]) { disZemin[x, 0] = true; q.Enqueue((x, 0)); }
                if (!maske[x, h - 1] && !disZemin[x, h - 1]) { disZemin[x, h - 1] = true; q.Enqueue((x, h - 1)); }
            }
            for (int y = 1; y < h - 1; y++)
            {
                if (!maske[0, y] && !disZemin[0, y]) { disZemin[0, y] = true; q.Enqueue((0, y)); }
                if (!maske[w - 1, y] && !disZemin[w - 1, y]) { disZemin[w - 1, y] = true; q.Enqueue((w - 1, y)); }
            }

            int[] dx = { -1, 1, 0, 0 };
            int[] dy = { 0, 0, -1, 1 };

            while (q.Count > 0)
            {
                var p = q.Dequeue();
                for (int k = 0; k < 4; k++)
                {
                    int nx = p.x + dx[k];
                    int ny = p.y + dy[k];
                    if (nx >= 0 && nx < w && ny >= 0 && ny < h && !maske[nx, ny] && !disZemin[nx, ny])
                    {
                        disZemin[nx, ny] = true;
                        q.Enqueue((nx, ny));
                    }
                }
            }

            for (int y = 1; y < h - 1; y++)
                for (int x = 1; x < w - 1; x++)
                    if (!maske[x, y] && !disZemin[x, y])
                        maske[x, y] = true;
        }

        public static Bitmap UyarlamaliArkaPlanGuncelle(
            Bitmap mevcutArkaPlan,
            Bitmap yeniKare,
            bool[,] urunMaskesi,
            double ogrenmeOrani)
        {
            if (mevcutArkaPlan == null) return yeniKare != null ? (Bitmap)yeniKare.Clone() : null;
            if (yeniKare == null) return (Bitmap)mevcutArkaPlan.Clone();

            int w = mevcutArkaPlan.Width;
            int h = mevcutArkaPlan.Height;
            double alpha = Math.Clamp(ogrenmeOrani, 0.01, 1.0);
            double birEksi = 1.0 - alpha;

            bool ayni = yeniKare.Width == w && yeniKare.Height == h;
            using Bitmap olcekli = ayni ? null : new Bitmap(yeniKare, w, h);
            Bitmap kaynak = ayni ? yeniKare : olcekli;

            Bitmap guncel = new Bitmap(w, h);
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    Color cb = mevcutArkaPlan.GetPixel(x, y);
                    if (urunMaskesi != null && urunMaskesi[x, y])
                    {
                        guncel.SetPixel(x, y, cb);
                        continue;
                    }

                    Color cu = kaynak.GetPixel(x, y);
                    int r = Math.Clamp((int)Math.Round(birEksi * cb.R + alpha * cu.R), 0, 255);
                    int g = Math.Clamp((int)Math.Round(birEksi * cb.G + alpha * cu.G), 0, 255);
                    int b = Math.Clamp((int)Math.Round(birEksi * cb.B + alpha * cu.B), 0, 255);
                    guncel.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
            return guncel;
        }

        public static Bitmap BantSimulasyonKaresiUret(int kareNo, out string donemEtiketi, out bool urunVar)
        {
            const int w = 260;
            const int h = 200;
            int k = ((kareNo % 80) + 80) % 80;

            // Gündüz (210) -> Akşam (105) -> Sabah (210) döngüsü ve kayan fabrika gölgesi
            double evre;
            if (k <= 34)
            {
                evre = k / 34.0;
                donemEtiketi = k < 16 ? "Gündüz" : (k < 28 ? "İkindi" : "Akşam");
            }
            else if (k <= 49)
            {
                evre = 1.0;
                donemEtiketi = "Akşam";
            }
            else
            {
                evre = 1.0 - (k - 49) / 31.0;
                donemEtiketi = k < 66 ? "Akşam" : "Sabah";
            }

            double tabanAydinlik = 212.0 - 106.0 * evre; // 212 -> 106
            double golgeMerkezX = 45.0 + 170.0 * evre;   // Gündüz soldan akşam sağa kayan gölge

            Bitmap bmp = new Bitmap(w, h);
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    double eksen = (x - golgeMerkezX) * 0.75 + (y - 100) * 0.45;
                    double golgeCarpan = 1.0 - 0.42 * Math.Exp(-(eksen * eksen) / 2800.0);
                    double serit = (y < 14 || y > h - 15) ? 0.68 : 1.0;

                    int v = Math.Clamp((int)Math.Round(tabanAydinlik * golgeCarpan * serit), 0, 255);
                    bmp.SetPixel(x, y, Color.FromArgb(v, v, (int)Math.Max(0, v - 4)));
                }
            }

            // Ürün geçiş pencereleri: k=[5..27] (Ürün 1) ve k=[40..64] (Ürün 2); k=[0..4] ve [28..39] boş bant
            urunVar = (k >= 5 && k <= 27) || (k >= 40 && k <= 64);
            if (urunVar)
            {
                using Graphics g = Graphics.FromImage(bmp);
                g.SmoothingMode = SmoothingMode.AntiAlias;

                if (k <= 27)
                {
                    int ux = 25 + (k - 5) * 8;
                    using Brush bKutu = new SolidBrush(Color.FromArgb(40, 82, 165));
                    using Brush bDetay = new SolidBrush(Color.FromArgb(20, 48, 105));
                    g.FillRectangle(bKutu, ux, 70, 46, 58);
                    g.FillRectangle(bDetay, ux + 10, 82, 26, 34);
                }
                else
                {
                    int ux = 25 + (k - 40) * 8;
                    using Brush bKapak = new SolidBrush(Color.FromArgb(175, 42, 42));
                    using Brush bIc = new SolidBrush(Color.FromArgb(115, 24, 24));
                    g.FillEllipse(bKapak, ux, 74, 52, 52);
                    g.FillEllipse(bIc, ux + 14, 88, 24, 24);
                }
            }

            return bmp;
        }
    }
}
