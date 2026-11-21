using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GoruntuIsleme
{
    public enum BelgeCiktiModu
    {
        SiyahBeyazMetin,
        GolgesizRenkliBelge
    }

    public class BelgeOkumaSonucu
    {
        public Bitmap KullanilanArkaPlan { get; set; }
        public Bitmap FarkResmi { get; set; }
        public Bitmap SonucResmi { get; set; }
        public int TespitEdilenPikselSayisi { get; set; }
        public double TespitYuzdesi { get; set; }
        public int MaksimumFark { get; set; }
    }

    public class Hafta9Odev2 : Form
    {
        private PictureBox picBelge, picArkaPlan, picFark, picSonuc;
        private ModernSurgu trkEsik;
        private ModernHapRozet rozetBelge, rozetArka, rozetFark, rozetSonuc;
        private CheckBox chkOtomatikArkaPlan;
        private ComboBox cmbCiktiModu;
        private TextBox txtKameraKaynak;
        private ModernHapButon btnKamera, btnArkaPlanYakala;
        private Label lblAltAnaliz;

        private Bitmap belgeResmi;
        private Bitmap arkaPlanResmi;
        private Bitmap tahminiArkaPlanOnbellek;

        private OpenCvSharp.VideoCapture videoYakala;
        private System.Windows.Forms.Timer akisZamanlayici;
        private bool kameraAcik = false;
        private bool kareIsleniyor = false;
        private bool programatikDegisim = false;

        public Hafta9Odev2()
        {
            Text = "Hafta 9 - Ödev 2 | Arka Plan Çıkarma ile Gölgeli Belge Okuma";
            FormGezintiYardimcisi.FormuHazirla(this);
            akisZamanlayici = new System.Windows.Forms.Timer { Interval = 80 };
            akisZamanlayici.Tick += AkisZamanlayici_Tick;

            ArayuzuOlustur();
            OrnekGolgeliBelgeYukle();

            this.FormClosing += (s, e) =>
            {
                AkisiDurdur();
                belgeResmi?.Dispose();
                belgeResmi = null;
                arkaPlanResmi?.Dispose();
                arkaPlanResmi = null;
                tahminiArkaPlanOnbellek?.Dispose();
                tahminiArkaPlanOnbellek = null;
            };
        }

        private void ArayuzuOlustur()
        {
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                this,
                "Hafta 9 - Ödev 2 | Gölgeli Kağıttan Arka Plan Çıkarma ile Yazı ve Resim Okuma",
                "< Ödev 1", () => { AkisiDurdur(); FormGezintiYardimcisi.FormGecisYap(this, new Hafta9Odev1()); },
                "Ödev 3 >", () => { AkisiDurdur(); FormGezintiYardimcisi.FormGecisYap(this, new Hafta9Odev3()); },
                false,
                "HAFTA 9 • ÖDEV 2");

            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 305);
            pnlSag.AutoScroll = true;

            int x = 16;
            int w = 262;
            int y = 12;

            Label lblKameraBaslik = new Label
            {
                Text = "SABİT KAMERA / TELEFON AKIŞI",
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi
            };
            y += 20;

            Label lblKaynakIpucu = new Label
            {
                Text = "USB Kamera (0, 1) veya IP Webcam URL:",
                Location = new Point(x, y),
                Size = new Size(w, 16),
                Font = new Font("Segoe UI", 8.0f),
                ForeColor = FormGezintiYardimcisi.AcikYazi
            };
            y += 18;

            txtKameraKaynak = new TextBox
            {
                Location = new Point(x, y),
                Size = new Size(w, 23),
                Text = "0",
                Font = new Font("Consolas", 9.0f)
            };
            y += 29;

            btnKamera = FormGezintiYardimcisi.ModernButonOlustur("Sabit Kamerayı Başlat", true, w, 34);
            btnKamera.Location = new Point(x, y);
            btnKamera.Click += BtnKamera_Click;
            y += 40;

            btnArkaPlanYakala = FormGezintiYardimcisi.ModernButonOlustur("Anlık Kareyi Boş Arka Plan Yap", false, w, 32);
            btnArkaPlanYakala.Location = new Point(x, y);
            btnArkaPlanYakala.Enabled = false;
            btnArkaPlanYakala.Click += BtnArkaPlanYakala_Click;
            y += 40;

            Label lblKaynak = new Label
            {
                Text = "BELGE VE ÖRNEK KAYNAKLAR",
                Location = new Point(x, y),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi
            };
            y += 20;

            ModernHapButon btnBelgeSec = FormGezintiYardimcisi.ModernButonOlustur("Gölgeli Belge Seç", false, w, 34);
            btnBelgeSec.Location = new Point(x, y);
            btnBelgeSec.Click += BtnBelgeSec_Click;
            y += 40;

            ModernHapButon btnArkaPlanSec = FormGezintiYardimcisi.ModernButonOlustur("Boş Gölgeli Arka Plan Seç", false, w, 34);
            btnArkaPlanSec.Location = new Point(x, y);
            btnArkaPlanSec.Click += BtnArkaPlanSec_Click;
            y += 40;

            ModernHapButon btnOrnek1 = FormGezintiYardimcisi.ModernButonOlustur("Örnek 1: Gerçek Gölgeli Belge", false, w, 32);
            btnOrnek1.Location = new Point(x, y);
            btnOrnek1.Click += (s, e) => OrnekGolgeliBelgeYukle();
            y += 38;

            ModernHapButon btnOrnek2 = FormGezintiYardimcisi.ModernButonOlustur("Örnek 2: Yazı + Resimli Belge", false, w, 32);
            btnOrnek2.Location = new Point(x, y);
            btnOrnek2.Click += (s, e) => OrnekYaziResimliYukle();
            y += 38;

            ModernHapButon btnTemizle = FormGezintiYardimcisi.ModernButonOlustur("Temizle", false, w, 30);
            btnTemizle.Location = new Point(x, y);
            btnTemizle.Click += (s, e) => Temizle();
            y += 38;

            Label lblMod = new Label
            {
                Text = "Çıktı Modu:",
                Location = new Point(x, y),
                Size = new Size(w, 16),
                Font = new Font("Segoe UI", 8.3f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi
            };
            y += 18;

            cmbCiktiModu = new ComboBox
            {
                Location = new Point(x, y),
                Size = new Size(w, 24),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 8.5f)
            };
            cmbCiktiModu.Items.Add("Siyah-Beyaz Metin (|A - B|)");
            cmbCiktiModu.Items.Add("Gölgesiz Belge & Resim (Oransal)");
            cmbCiktiModu.SelectedIndex = 0;
            cmbCiktiModu.SelectedIndexChanged += (s, e) => { if (!programatikDegisim) Uygula(); };
            y += 30;

            chkOtomatikArkaPlan = new CheckBox
            {
                Text = "Tek Fotoğraftan Arka Plan Tahmin Et\n(Boş kağıt görüntüsü yoksa)",
                Location = new Point(x, y),
                Size = new Size(w, 36),
                Font = new Font("Segoe UI", 8.3f),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                Checked = false
            };
            chkOtomatikArkaPlan.CheckedChanged += (s, e) => { if (!programatikDegisim) Uygula(); };
            y += 42;

            ModernKartPanel kartBilgi = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                new Point(x, y), new Size(w, 186), out Label lblBilgi);
            kartBilgi.Margin = new Padding(0, 0, 0, 14);
            lblBilgi.Font = new Font("Segoe UI", 8.3f);
            lblBilgi.Text = "Sabit Kamera & Telefon Kullanımı\n\n" +
                            "1. Telefonunuzu USB (DroidCam/Iriun sanal kamera 0/1) veya Wi-Fi (IP Webcam http://IP:8080/video) ile sabitleyin.\n\n" +
                            "2. Boş kağıdı 'Anlık Kareyi Boş Arka Plan Yap' ile kaydedin, ardından kağıda yazılı/resimli belgeyi koyun; ortak gölge elenir.";

            pnlSag.Controls.AddRange(new Control[]
            {
                lblKameraBaslik, lblKaynakIpucu, txtKameraKaynak, btnKamera, btnArkaPlanYakala,
                lblKaynak, btnBelgeSec, btnArkaPlanSec, btnOrnek1, btnOrnek2, btnTemizle,
                lblMod, cmbCiktiModu, chkOtomatikArkaPlan, kartBilgi
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
                RowCount = 3,
                BackColor = FormGezintiYardimcisi.TemaZemin
            };
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 43f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 45f));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 12f));

            picBelge = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 0, 0, "Gölgeli Belge (Yazı / Resim)", null, null, rozetBelge);
            picArkaPlan = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 1, 0, "Boş Gölgeli Arka Plan", null, null, rozetArka);
            picFark = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 0, 1, "Fark Görüntüsü |ArkaPlan - Belge|", null, null, rozetFark);
            picSonuc = FormGezintiYardimcisi.KartliPictureBoxOlustur(
                tbl, 1, 1, "Eşiklenmiş Okunabilir Çıktı", null, pnlSonucAlt, rozetSonuc);

            ModernKartPanel altSerit = FormGezintiYardimcisi.AltAnalizSeridiOlustur(
                "Analiz hazır.",
                out lblAltAnaliz,
                "Yazı Okuma");
            tbl.Controls.Add(altSerit, 0, 2);
            tbl.SetColumnSpan(altSerit, 2);

            Controls.Add(tbl);
            FormGezintiYardimcisi.YerlesimiDuzenle(this, pnlUst, pnlSag, tbl);
        }

        private void BtnKamera_Click(object sender, EventArgs e)
        {
            if (kameraAcik)
            {
                AkisiDurdur();
                return;
            }

            string kaynakMetin = (txtKameraKaynak.Text ?? "0").Trim();
            OpenCvSharp.VideoCapture yeni;
            if (int.TryParse(kaynakMetin, out int indeks))
                yeni = new OpenCvSharp.VideoCapture(indeks);
            else
                yeni = new OpenCvSharp.VideoCapture(kaynakMetin);

            if (!yeni.IsOpened())
            {
                yeni.Dispose();
                MessageBox.Show(
                    "Belirtilen sabit kamera veya telefon IP/USB yayın kaynağı açılamadı.\nLütfen bağlantıyı veya kamera indeksini (0, 1 ya da http://IP:8080/video) kontrol edin.",
                    "Kamera Açılamadı",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            videoYakala = yeni;
            kameraAcik = true;
            btnKamera.Text = "Kamerayı Durdur";
            btnArkaPlanYakala.Enabled = true;
            rozetBelge.MetinGuncelle("Canlı Kamera");
            akisZamanlayici.Start();
        }

        private void BtnArkaPlanYakala_Click(object sender, EventArgs e)
        {
            if (belgeResmi == null) return;

            arkaPlanResmi?.Dispose();
            arkaPlanResmi = (Bitmap)belgeResmi.Clone();

            programatikDegisim = true;
            chkOtomatikArkaPlan.Checked = false;
            programatikDegisim = false;

            rozetArka.MetinGuncelle("Yakalandı");
            Uygula();
        }

        private void AkisZamanlayici_Tick(object sender, EventArgs e)
        {
            if (IsDisposed || !kameraAcik || kareIsleniyor || videoYakala == null || !videoYakala.IsOpened())
                return;

            kareIsleniyor = true;
            try
            {
                using OpenCvSharp.Mat mat = new OpenCvSharp.Mat();
                if (!videoYakala.Read(mat) || mat.Empty())
                {
                    AkisiDurdur();
                    return;
                }

                using Bitmap ham = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(mat);
                BelgeyiDegistir(BitmapYardimcisi.BoyutSinirla(ham, 320));
                Uygula();
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

        private void AkisiDurdur()
        {
            akisZamanlayici?.Stop();
            if (videoYakala != null)
            {
                videoYakala.Release();
                videoYakala.Dispose();
                videoYakala = null;
            }
            kameraAcik = false;
            if (btnKamera != null) btnKamera.Text = "Sabit Kamerayı Başlat";
            if (btnArkaPlanYakala != null) btnArkaPlanYakala.Enabled = false;
        }

        private void BtnBelgeSec_Click(object sender, EventArgs e)
        {
            string yol = FormGezintiYardimcisi.ResimDosyasiSec("Gölgeli Yazılı / Resimli Belge Seç");
            if (yol == null) return;

            AkisiDurdur();
            using Bitmap ham = new Bitmap(yol);
            BelgeyiDegistir(BitmapYardimcisi.BoyutSinirla(ham, 380));
            rozetBelge.MetinGuncelle("Seçilen Belge");
            Uygula();
        }

        private void BtnArkaPlanSec_Click(object sender, EventArgs e)
        {
            string yol = FormGezintiYardimcisi.ResimDosyasiSec("Aynı Işıktaki Boş Kağıt (Arka Plan) Seç");
            if (yol == null) return;

            using Bitmap ham = new Bitmap(yol);
            arkaPlanResmi?.Dispose();
            arkaPlanResmi = BitmapYardimcisi.BoyutSinirla(ham, 380);

            programatikDegisim = true;
            chkOtomatikArkaPlan.Checked = false;
            programatikDegisim = false;

            rozetArka.MetinGuncelle("Seçilen Zemin");
            Uygula();
        }

        private void OrnekGolgeliBelgeYukle()
        {
            AkisiDurdur();
            Bitmap belge = BitmapYardimcisi.OrnekGorselYukle("golgeli_belge.jpg");
            Bitmap arka = BitmapYardimcisi.OrnekGorselYukle("golgeli_belge_bos.jpg");
            if (belge == null || arka == null)
            {
                belge?.Dispose();
                arka?.Dispose();
                OrnekYaziAndResimOlusturVeAta();
                return;
            }

            programatikDegisim = true;
            try
            {
                chkOtomatikArkaPlan.Checked = false;
                cmbCiktiModu.SelectedIndex = 0;
            }
            finally
            {
                programatikDegisim = false;
            }

            BelgeyiDegistir(belge);
            arkaPlanResmi?.Dispose();
            arkaPlanResmi = arka;
            rozetBelge.MetinGuncelle("Örnek 1 Belge");
            rozetArka.MetinGuncelle("Boş Gölge");
            Uygula();
        }

        private void OrnekYaziResimliYukle()
        {
            AkisiDurdur();
            OrnekYaziAndResimOlusturVeAta();
        }

        private void OrnekYaziAndResimOlusturVeAta()
        {
            OrnekYaziVeResimliBelgeOlustur(out Bitmap belge, out Bitmap arka);
            programatikDegisim = true;
            try
            {
                chkOtomatikArkaPlan.Checked = false;
                cmbCiktiModu.SelectedIndex = 1; // Yazı + renkli şekilleri gölgesiz göster
            }
            finally
            {
                programatikDegisim = false;
            }

            BelgeyiDegistir(belge);
            arkaPlanResmi?.Dispose();
            arkaPlanResmi = arka;
            rozetBelge.MetinGuncelle("Örnek 2 (Yazı+Resim)");
            rozetArka.MetinGuncelle("Boş Gölge");
            Uygula();
        }

        private void Temizle()
        {
            AkisiDurdur();
            BelgeyiDegistir(null);
            arkaPlanResmi?.Dispose();
            arkaPlanResmi = null;
            ResmiAta(picBelge, null);
            ResmiAta(picArkaPlan, null);
            ResmiAta(picFark, null);
            ResmiAta(picSonuc, null);
            rozetBelge.MetinGuncelle("Boş");
            rozetArka.MetinGuncelle("Boş");
            rozetFark.MetinGuncelle("|A - B|");
            lblAltAnaliz.Text = "Görseller temizlendi. Örnek seçin veya kamera başlatın.";
        }

        private void BelgeyiDegistir(Bitmap yeniBelge)
        {
            belgeResmi?.Dispose();
            belgeResmi = yeniBelge;
            tahminiArkaPlanOnbellek?.Dispose();
            tahminiArkaPlanOnbellek = null;
            ResmiAta(picBelge, yeniBelge != null ? (Bitmap)yeniBelge.Clone() : null);
        }

        private static void ResmiAta(PictureBox pic, Bitmap yeni)
        {
            Image onceki = pic.Image;
            pic.Image = yeni;
            onceki?.Dispose();
        }

        private void Uygula()
        {
            if (belgeResmi == null) return;

            bool tahminModu = chkOtomatikArkaPlan.Checked || arkaPlanResmi == null;
            Bitmap kullanilacakArka = arkaPlanResmi;
            bool otomatikHesapla = false;

            if (tahminModu)
            {
                if (tahminiArkaPlanOnbellek == null)
                    tahminiArkaPlanOnbellek = ArkaPlanTahminEt(belgeResmi);
                kullanilacakArka = tahminiArkaPlanOnbellek;
                rozetArka.MetinGuncelle("Tahmini Zemin");
            }

            BelgeCiktiModu ciktiModu = cmbCiktiModu.SelectedIndex == 0
                ? BelgeCiktiModu.SiyahBeyazMetin
                : BelgeCiktiModu.GolgesizRenkliBelge;

            BelgeOkumaSonucu sonuc = GolgeliBelgeOku(
                belgeResmi,
                kullanilacakArka,
                otomatikHesapla,
                trkEsik.Value,
                ciktiModu);

            ResmiAta(picArkaPlan, sonuc.KullanilanArkaPlan);
            ResmiAta(picFark, sonuc.FarkResmi);
            ResmiAta(picSonuc, sonuc.SonucResmi);

            rozetFark.MetinGuncelle($"Maks: {sonuc.MaksimumFark}");
            lblAltAnaliz.Text = $"Tespit edilen yazı/ön plan: {sonuc.TespitEdilenPikselSayisi} px (%{sonuc.TespitYuzdesi:F2}) | Maks fark: {sonuc.MaksimumFark} | Mod: {(ciktiModu == BelgeCiktiModu.SiyahBeyazMetin ? "Siyah-Beyaz Eşikleme" : "Gölgesiz Oransal Renk")}";
        }

        public static Bitmap ArkaPlanTahminEt(Bitmap belge)
        {
            if (belge == null) return null;

            int w = belge.Width;
            int h = belge.Height;
            byte[,] gri = new byte[w, h];
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    Color c = belge.GetPixel(x, y);
                    gri[x, y] = (byte)((c.R + c.G + c.B) / 3);
                }
            }

            byte[,] arkaGri = ArkaPlanGriTahminEt(gri, w, h);
            Bitmap tahmin = new Bitmap(w, h);
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    byte v = arkaGri[x, y];
                    tahmin.SetPixel(x, y, Color.FromArgb(v, v, v));
                }
            }
            return tahmin;
        }

        private static byte[,] ArkaPlanGriTahminEt(byte[,] gri, int w, int h)
        {
            // Gri tonlamalı morfolojik Kapama (Closing: önce Max(r=7), sonra Min(r=7))
            // Yazı darbelerini silerken güneş ışığı/gölge ve kağıt kenar sınırını gerçek konumuna geri çeker
            byte[,] genislet = AyrilabilirUcFiltre(gri, w, h, 7, maksMi: true);
            byte[,] kapama = AyrilabilirUcFiltre(genislet, w, h, 7, maksMi: false);
            return AyrilabilirKutuYumusat(kapama, w, h, 2);
        }

        private static byte[,] AyrilabilirUcFiltre(byte[,] kaynak, int w, int h, int r, bool maksMi)
        {
            byte[,] yatay = new byte[w, h];
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int x0 = Math.Max(0, x - r);
                    int x1 = Math.Min(w - 1, x + r);
                    byte uc = maksMi ? (byte)0 : (byte)255;
                    for (int kx = x0; kx <= x1; kx++)
                    {
                        byte v = kaynak[kx, y];
                        if (maksMi ? (v > uc) : (v < uc)) uc = v;
                    }
                    yatay[x, y] = uc;
                }
            }

            byte[,] dikey = new byte[w, h];
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    int y0 = Math.Max(0, y - r);
                    int y1 = Math.Min(h - 1, y + r);
                    byte uc = maksMi ? (byte)0 : (byte)255;
                    for (int ky = y0; ky <= y1; ky++)
                    {
                        byte v = yatay[x, ky];
                        if (maksMi ? (v > uc) : (v < uc)) uc = v;
                    }
                    dikey[x, y] = uc;
                }
            }
            return dikey;
        }

        private static byte[,] AyrilabilirKutuYumusat(byte[,] kaynak, int w, int h, int r)
        {
            byte[,] yatay = new byte[w, h];
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int x0 = Math.Max(0, x - r);
                    int x1 = Math.Min(w - 1, x + r);
                    int top = 0;
                    for (int kx = x0; kx <= x1; kx++) top += kaynak[kx, y];
                    yatay[x, y] = (byte)(top / (x1 - x0 + 1));
                }
            }

            byte[,] sonuc = new byte[w, h];
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    int y0 = Math.Max(0, y - r);
                    int y1 = Math.Min(h - 1, y + r);
                    int top = 0;
                    for (int ky = y0; ky <= y1; ky++) top += yatay[x, ky];
                    sonuc[x, y] = (byte)(top / (y1 - y0 + 1));
                }
            }
            return sonuc;
        }

        public static BelgeOkumaSonucu GolgeliBelgeOku(
            Bitmap belge,
            Bitmap arkaPlan,
            bool otomatikArkaPlanTahmin,
            int esikDegeri,
            BelgeCiktiModu ciktiModu)
        {
            BelgeOkumaSonucu sonuc = new BelgeOkumaSonucu();
            if (belge == null) return sonuc;

            int w = belge.Width;
            int h = belge.Height;
            Bitmap farkBmp = new Bitmap(w, h);
            Bitmap sonucBmp = new Bitmap(w, h);
            int minRenkliFark = Math.Max(4, esikDegeri / 2);
            int tespitSay = 0;
            int maksFark = 0;
            Bitmap kullanilanArka;

            if (otomatikArkaPlanTahmin || arkaPlan == null)
            {
                Color[,] belgeRenk = new Color[w, h];
                byte[,] griBelge = new byte[w, h];
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        Color cb = belge.GetPixel(x, y);
                        belgeRenk[x, y] = cb;
                        griBelge[x, y] = (byte)((cb.R + cb.G + cb.B) / 3);
                    }
                }

                byte[,] arkaGri = ArkaPlanGriTahminEt(griBelge, w, h);
                kullanilanArka = new Bitmap(w, h);

                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        byte ga = arkaGri[x, y];
                        kullanilanArka.SetPixel(x, y, Color.FromArgb(ga, ga, ga));
                        SonucPikselYaz(belgeRenk[x, y], ga, ga, ga, griBelge[x, y], ga,
                            x, y, esikDegeri, minRenkliFark, ciktiModu, farkBmp, sonucBmp, ref tespitSay, ref maksFark);
                    }
                }
            }
            else
            {
                kullanilanArka = (arkaPlan.Width == w && arkaPlan.Height == h)
                    ? (Bitmap)arkaPlan.Clone()
                    : new Bitmap(arkaPlan, w, h);

                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        Color cb = belge.GetPixel(x, y);
                        Color ca = kullanilanArka.GetPixel(x, y);
                        int griBelge = (cb.R + cb.G + cb.B) / 3;
                        int griArka = (ca.R + ca.G + ca.B) / 3;
                        SonucPikselYaz(cb, ca.R, ca.G, ca.B, griBelge, griArka,
                            x, y, esikDegeri, minRenkliFark, ciktiModu, farkBmp, sonucBmp, ref tespitSay, ref maksFark);
                    }
                }
            }

            sonuc.KullanilanArkaPlan = kullanilanArka;
            sonuc.FarkResmi = farkBmp;
            sonuc.SonucResmi = sonucBmp;
            sonuc.TespitEdilenPikselSayisi = tespitSay;
            sonuc.TespitYuzdesi = 100.0 * tespitSay / Math.Max(1, w * h);
            sonuc.MaksimumFark = maksFark;
            return sonuc;
        }

        private static void SonucPikselYaz(
            Color cb, int caR, int caG, int caB,
            int griBelge, int griArka,
            int x, int y,
            int esikDegeri, int minRenkliFark, BelgeCiktiModu ciktiModu,
            Bitmap farkBmp, Bitmap sonucBmp,
            ref int tespitSay, ref int maksFark)
        {
            int fark = Math.Abs(griArka - griBelge);
            if (fark > maksFark) maksFark = fark;
            farkBmp.SetPixel(x, y, Color.FromArgb(fark, fark, fark));

            if (fark >= esikDegeri)
                tespitSay++;

            if (ciktiModu == BelgeCiktiModu.SiyahBeyazMetin)
            {
                sonucBmp.SetPixel(x, y, fark >= esikDegeri ? Color.Black : Color.White);
                return;
            }

            if (fark < minRenkliFark)
            {
                sonucBmp.SetPixel(x, y, Color.White);
                return;
            }

            int orR = Math.Clamp((int)Math.Round(cb.R * 255.0 / Math.Max(20, caR)), 0, 255);
            int orG = Math.Clamp((int)Math.Round(cb.G * 255.0 / Math.Max(20, caG)), 0, 255);
            int orB = Math.Clamp((int)Math.Round(cb.B * 255.0 / Math.Max(20, caB)), 0, 255);
            sonucBmp.SetPixel(x, y, Color.FromArgb(orR, orG, orB));
        }

        public static void OrnekYaziVeResimliBelgeOlustur(out Bitmap belge, out Bitmap bosArkaPlan)
        {
            const int w = 320;
            const int h = 260;

            bosArkaPlan = new Bitmap(w, h);
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    // Sol üst aydınlık (~238), sağ alt güçlü çapraz gölge (~115)
                    double t = Math.Clamp((x / (double)w * 0.55 + y / (double)h * 0.45 - 0.35) * 2.2, 0.0, 1.0);
                    int aydinlik = (int)Math.Round(238.0 - 123.0 * t);
                    bosArkaPlan.SetPixel(x, y, Color.FromArgb(aydinlik, aydinlik, (int)Math.Max(0, aydinlik - 4)));
                }
            }

            belge = (Bitmap)bosArkaPlan.Clone();
            using Bitmap katman = new Bitmap(w, h);
            using (Graphics gk = Graphics.FromImage(katman))
            {
                gk.SmoothingMode = SmoothingMode.AntiAlias;
                gk.Clear(Color.White);

                using Font fBaslik = new Font("Segoe UI", 12f, FontStyle.Bold);
                using Font fGovde = new Font("Segoe UI", 9.5f, FontStyle.Regular);
                using Brush koyu = new SolidBrush(Color.FromArgb(24, 28, 34));

                gk.DrawString("GÖRÜNTÜ İŞLEME NOTLARI", fBaslik, koyu, 22, 18);
                gk.DrawString("1. Arka plan çıkarma ile gölgeli\n   belgelerden yazı okunabilir.\n2. Renkli şekiller oranla korunur.", fGovde, koyu, 22, 48);

                // Gölge altında kalan renkli ev ve güneş/grafik çizimi
                using Brush maviFirca = new SolidBrush(Color.FromArgb(30, 105, 210));
                using Brush kirmiziFirca = new SolidBrush(Color.FromArgb(205, 45, 45));
                using Brush yesilFirca = new SolidBrush(Color.FromArgb(38, 155, 72));
                gk.FillRectangle(maviFirca, 185, 145, 85, 65);
                gk.FillPolygon(kirmiziFirca, new[]
                {
                    new Point(178, 145),
                    new Point(227, 105),
                    new Point(277, 145)
                });
                gk.FillEllipse(yesilFirca, 38, 138, 90, 72);
                gk.DrawString("ŞEKİL VE METİN TESTİ", fGovde, koyu, 32, 224);
            }

            // Yazı ve renkli çizimleri gölge aydınlatmasıyla çarpıştırarak gerçek kağıt fiziğini taklit et
            for (int py = 0; py < h; py++)
            {
                for (int px = 0; px < w; px++)
                {
                    Color kc = katman.GetPixel(px, py);
                    if (kc.R >= 250 && kc.G >= 250 && kc.B >= 250) continue;
                    Color bg = bosArkaPlan.GetPixel(px, py);
                    int r = Math.Clamp(kc.R * bg.R / 255, 0, 255);
                    int gr = Math.Clamp(kc.G * bg.G / 255, 0, 255);
                    int b = Math.Clamp(kc.B * bg.B / 255, 0, 255);
                    belge.SetPixel(px, py, Color.FromArgb(r, gr, b));
                }
            }
        }
    }
}
