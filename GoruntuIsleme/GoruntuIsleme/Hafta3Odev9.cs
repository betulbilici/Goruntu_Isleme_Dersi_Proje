using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace GoruntuIsleme
{
    public partial class Hafta3Odev9 : Form
    {
        private Bitmap orijinalBmp = null;

        private ModernHapRozet rozetOrijinal;
        private ModernHapRozet rozetDegistirme;
        private ModernHapRozet rozetInterpolasyon;

        private ModernSurgu surguKatsayi;
        private Label lblValKatsayi;
        private Label lblBilgiKart;
        private Label lblAnaliz;

        public Hafta3Odev9()
        {
            InitializeComponent();
            ModernArayuzuUygula();
        }

        private void ModernArayuzuUygula()
        {
            // 1. Form Temel Ayarları (1250x750, Standart Kenarlık ve X ile Ana Menüye Dönüş)
            FormGezintiYardimcisi.FormuHazirla(this);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Text = "Hafta 3 - Ödev 9: Büyütmede Piksel Değiştirme ve İnterpolasyon Karşılaştırması";

            // Designer'daki tüm eski kontrolleri gizle
            foreach (Control ctrl in this.Controls)
            {
                ctrl.Visible = false;
            }

            // 2. Üst Gezinti Barı
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                form: this,
                baslik: "Hafta 3 - Ödev 9 | Yüksek Hızlı Büyütme: Piksel Değiştirme vs Bilineer İnterpolasyon",
                oncekiYazi: "< Ödev 8",
                oncekiTikla: () =>
                {
                    Form odev8 = Application.OpenForms["Hafta3Odev8"];
                    if (odev8 != null)
                        FormGezintiYardimcisi.FormGecisYap(this, odev8);
                    else
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta3Odev8());
                },
                sonrakiYazi: "Ödev 10 >",
                sonrakiTikla: () =>
                {
                    Form odev10 = Application.OpenForms["Hafta3Odev10"];
                    if (odev10 != null)
                        FormGezintiYardimcisi.FormGecisYap(this, odev10);
                    else
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta3Odev10());
                }
            );

            // 3. Sağ Kontrol Paneli
            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 290);

            Label lblKontrolBaslik = new Label
            {
                Text = "Büyütme & Performans",
                Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                Location = new Point(18, 16),
                AutoSize = true
            };
            pnlSag.Controls.Add(lblKontrolBaslik);

            // Resim Seç Butonu
            ModernHapButon btnModernResimSec = FormGezintiYardimcisi.ModernButonOlustur("📁 Resim Seç", true, 252, 38);
            btnModernResimSec.Location = new Point(18, 48);
            btnModernResimSec.Click += btnResimSec_Click;
            pnlSag.Controls.Add(btnModernResimSec);

            // Büyütme Katsayısı Sürgüsü (2x - 8x)
            Label lblKatsayiBaslik = new Label
            {
                Text = "Büyütme Katsayısı",
                Location = new Point(18, 102),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi
            };
            pnlSag.Controls.Add(lblKatsayiBaslik);

            lblValKatsayi = new Label
            {
                Text = "2x",
                Location = new Point(210, 102),
                Size = new Size(60, 18),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi
            };
            pnlSag.Controls.Add(lblValKatsayi);

            surguKatsayi = new ModernSurgu
            {
                Minimum = 2,
                Maximum = 8,
                Value = 2,
                Location = new Point(18, 122),
                Size = new Size(252, 24)
            };
            surguKatsayi.Scroll += (s, e) =>
            {
                lblValKatsayi.Text = $"{surguKatsayi.Value}x";
                if (orijinalBmp != null) btnBuyut_Click(null, null);
            };
            pnlSag.Controls.Add(surguKatsayi);

            // Resmi Büyüt Butonu
            ModernHapButon btnModernBuyut = FormGezintiYardimcisi.ModernButonOlustur("🔍 Resmi Büyüt", true, 252, 40);
            btnModernBuyut.Location = new Point(18, 158);
            btnModernBuyut.Click += btnBuyut_Click;
            pnlSag.Controls.Add(btnModernBuyut);

            // Açık Mavi Analiz ve Teknik Bilgi Kartı
            ModernKartPanel bilgiKarti = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                konum: new Point(18, 214),
                boyut: new Size(252, 340),
                icerikEtiketi: out lblBilgiKart
            );
            lblBilgiKart.Text =
                "Algoritma & Bellek Yapısı:\n\n" +
                "• LockBits + Marshal.Copy:\n" +
                "Görüntü 24-bit RGB bayt dizisine kilitlenerek GetPixel/SetPixel yerine doğrudan bellek üzerinden işlenir.\n\n" +
                "• Parallel.For:\n" +
                "Satır bazlı çok çekirdekli paralel hesaplama ile yüksek çözünürlüklerde dahi anlık sonuç üretir.\n\n" +
                "• Sonuç Karşılaştırması:\n" +
                "Orta kartta bloklaşma, sağ kartta yumuşak kenar geçişleri gözlemlenir.";
            pnlSag.Controls.Add(bilgiKarti);

            // 4. Orta Çalışma Alanı (Üst: 3 Karşılaştırma Kartı | Alt: Analiz Şeridi)
            Panel pnlOrta = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(12),
                BackColor = FormGezintiYardimcisi.TemaZemin
            };

            TableLayoutPanel anaYerlesim = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2
            };
            anaYerlesim.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            anaYerlesim.RowStyles.Add(new RowStyle(SizeType.Absolute, 50f));

            // 3 Sütunlu Karşılaştırma Izgarası
            TableLayoutPanel goruntuIzgara = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1
            };
            goruntuIzgara.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            goruntuIzgara.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            goruntuIzgara.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34f));
            goruntuIzgara.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            rozetOrijinal = new ModernHapRozet(); rozetOrijinal.MetinGuncelle("1x");
            rozetDegistirme = new ModernHapRozet(); rozetDegistirme.MetinGuncelle("Bekleniyor");
            rozetInterpolasyon = new ModernHapRozet(); rozetInterpolasyon.MetinGuncelle("Bekleniyor");

            MevcutPictureBoxKartaTasi(goruntuIzgara, 0, 0, "Orijinal Resim", picOrijinal, FormGezintiYardimcisi.TemaZemin, rozetOrijinal);
            MevcutPictureBoxKartaTasi(goruntuIzgara, 1, 0, "Piksel Değiştirme (Nearest)", picDegistirme, FormGezintiYardimcisi.TemaZemin, rozetDegistirme);
            MevcutPictureBoxKartaTasi(goruntuIzgara, 2, 0, "Piksel İnterpolasyonu (Bilineer)", picInterpolasyon, FormGezintiYardimcisi.TemaZemin, rozetInterpolasyon);

            // Alt Analiz Şeridi
            ModernKartPanel altSerit = FormGezintiYardimcisi.AltAnalizSeridiOlustur(
                "Bir resim seçin, büyütme katsayısını (2x–8x) belirleyin ve piksel değiştirme ile bilineer interpolasyonu karşılaştırın.",
                out lblAnaliz,
                "LockBits + Parallel"
            );

            anaYerlesim.Controls.Add(goruntuIzgara, 0, 0);
            anaYerlesim.Controls.Add(altSerit, 0, 1);
            pnlOrta.Controls.Add(anaYerlesim);

            this.Controls.Add(pnlOrta);
            FormGezintiYardimcisi.YerlesimiDuzenle(this, pnlUst, pnlSag, pnlOrta);
        }

        private void MevcutPictureBoxKartaTasi(
            TableLayoutPanel tablo,
            int sutun,
            int satir,
            string baslik,
            PictureBox mevcutPb,
            Color zeminRengi,
            ModernHapRozet sagUstRozet)
        {
            ModernKartPanel kart = new ModernKartPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(6),
                Padding = new Padding(12)
            };

            Panel pnlKartUst = new Panel
            {
                Dock = DockStyle.Top,
                Height = 28,
                BackColor = Color.White
            };

            Label lbl = new Label
            {
                Text = baslik,
                UseMnemonic = false,
                AutoSize = true,
                Location = new Point(0, 4),
                Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                BackColor = Color.White
            };
            pnlKartUst.Controls.Add(lbl);

            if (sagUstRozet != null)
            {
                sagUstRozet.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                sagUstRozet.Location = new Point(pnlKartUst.Width - sagUstRozet.Width - 2, 1);
                pnlKartUst.Resize += (s, e) =>
                {
                    sagUstRozet.Location = new Point(pnlKartUst.Width - sagUstRozet.Width - 2, 1);
                };
                pnlKartUst.Controls.Add(sagUstRozet);
                sagUstRozet.BringToFront();
            }

            mevcutPb.Parent?.Controls.Remove(mevcutPb);
            mevcutPb.Visible = true;
            mevcutPb.Dock = DockStyle.Fill;
            mevcutPb.SizeMode = PictureBoxSizeMode.Zoom;
            mevcutPb.BorderStyle = BorderStyle.None;
            mevcutPb.BackColor = zeminRengi;

            kart.Controls.Add(mevcutPb);
            kart.Controls.Add(pnlKartUst);
            tablo.Controls.Add(kart, sutun, satir);
        }

        private void btnResimSec_Click(object sender, EventArgs e)
        {
            string dosyaYolu = FormGezintiYardimcisi.ResimDosyasiSec("Büyütme Karşılaştırması İçin Resim Seçiniz");
            if (string.IsNullOrEmpty(dosyaYolu)) return;

            orijinalBmp?.Dispose();
            using (Bitmap temp = new Bitmap(dosyaYolu))
            {
                orijinalBmp = new Bitmap(temp);
            }

            picOrijinal.Image?.Dispose();
            picOrijinal.SizeMode = PictureBoxSizeMode.Zoom;
            picOrijinal.Image = new Bitmap(orijinalBmp);

            picDegistirme.Image?.Dispose();
            picDegistirme.Image = null;

            picInterpolasyon.Image?.Dispose();
            picInterpolasyon.Image = null;

            rozetOrijinal.MetinGuncelle($"{orijinalBmp.Width}×{orijinalBmp.Height}");
            lblAnaliz.Text = $"Resim yüklendi ({orijinalBmp.Width}×{orijinalBmp.Height} px) • Katsayı: {surguKatsayi.Value}x";
            btnBuyut_Click(null, null);
        }

        private void btnBuyut_Click(object sender, EventArgs e)
        {
            if (orijinalBmp == null)
            {
                MessageBox.Show("Lütfen önce bir resim seçiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int katsayi = surguKatsayi.Value;
            int kaynakW = orijinalBmp.Width;
            int kaynakH = orijinalBmp.Height;

            int hedefW = kaynakW * katsayi;
            int hedefH = kaynakH * katsayi;

            this.Cursor = Cursors.WaitCursor;
            Stopwatch sw = Stopwatch.StartNew();

            // 1. Kaynak resmi 24-bit RGB formatında belleğe kilitleyip bayt dizisine alma
            Bitmap kaynak24 = new Bitmap(kaynakW, kaynakH, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(kaynak24))
            {
                g.DrawImage(orijinalBmp, 0, 0, kaynakW, kaynakH);
            }

            BitmapData srcData = kaynak24.LockBits(
                new Rectangle(0, 0, kaynakW, kaynakH),
                ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);

            int srcStride = srcData.Stride;
            byte[] srcBuffer = new byte[Math.Abs(srcStride) * kaynakH];
            Marshal.Copy(srcData.Scan0, srcBuffer, 0, srcBuffer.Length);
            kaynak24.UnlockBits(srcData);

            // Çıkış pikselleri için bayt tampon dizileri
            int hedefStride = ((hedefW * 3 + 3) / 4) * 4;
            byte[] degistirmeBuffer = new byte[hedefStride * hedefH];
            byte[] interpolasyonBuffer = new byte[hedefStride * hedefH];

            // 2. Paralel döngü: Piksel Değiştirme ve Bilinear İnterpolasyon hesabı
            Parallel.For(0, hedefH, y =>
            {
                int rowSrcY = Math.Min(y / katsayi, kaynakH - 1);
                float gy = (float)y / katsayi;
                int y1 = Math.Min((int)Math.Floor(gy), kaynakH - 1);
                int y2 = Math.Min(y1 + 1, kaynakH - 1);
                float dy = gy - y1;

                int rowDegistirmeIdx = y * hedefStride;
                int rowInterpIdx = y * hedefStride;

                for (int x = 0; x < hedefW; x++)
                {
                    // A: PİKSEL DEĞİŞTİRME (NEAREST NEIGHBOR)
                    int srcX = Math.Min(x / katsayi, kaynakW - 1);
                    int srcPixelIdx = (rowSrcY * srcStride) + (srcX * 3);

                    int dIdx = rowDegistirmeIdx + (x * 3);
                    degistirmeBuffer[dIdx] = srcBuffer[srcPixelIdx];         // Blue
                    degistirmeBuffer[dIdx + 1] = srcBuffer[srcPixelIdx + 1]; // Green
                    degistirmeBuffer[dIdx + 2] = srcBuffer[srcPixelIdx + 2]; // Red

                    // B: PİKSEL İNTERPOLASYONU (BILINEAR)
                    float gx = (float)x / katsayi;
                    int x1 = Math.Min((int)Math.Floor(gx), kaynakW - 1);
                    int x2 = Math.Min(x1 + 1, kaynakW - 1);
                    float dx = gx - x1;

                    int idx11 = (y1 * srcStride) + (x1 * 3);
                    int idx21 = (y1 * srcStride) + (x2 * 3);
                    int idx12 = (y2 * srcStride) + (x1 * 3);
                    int idx22 = (y2 * srcStride) + (x2 * 3);

                    for (int c = 0; c < 3; c++)
                    {
                        byte val11 = srcBuffer[idx11 + c];
                        byte val21 = srcBuffer[idx21 + c];
                        byte val12 = srcBuffer[idx12 + c];
                        byte val22 = srcBuffer[idx22 + c];

                        float val = val11 * (1 - dx) * (1 - dy) +
                                    val21 * dx * (1 - dy) +
                                    val12 * (1 - dx) * dy +
                                    val22 * dx * dy;

                        interpolasyonBuffer[rowInterpIdx + (x * 3) + c] = (byte)Math.Clamp((int)Math.Round(val), 0, 255);
                    }
                }
            });

            // 3. Bayt dizilerini Bitmap nesnelerine dönüştürme
            Bitmap bmpDegistirme = new Bitmap(hedefW, hedefH, PixelFormat.Format24bppRgb);
            BitmapData dstData1 = bmpDegistirme.LockBits(
                new Rectangle(0, 0, hedefW, hedefH),
                ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb);
            Marshal.Copy(degistirmeBuffer, 0, dstData1.Scan0, degistirmeBuffer.Length);
            bmpDegistirme.UnlockBits(dstData1);

            Bitmap bmpInterpolasyon = new Bitmap(hedefW, hedefH, PixelFormat.Format24bppRgb);
            BitmapData dstData2 = bmpInterpolasyon.LockBits(
                new Rectangle(0, 0, hedefW, hedefH),
                ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb);
            Marshal.Copy(interpolasyonBuffer, 0, dstData2.Scan0, interpolasyonBuffer.Length);
            bmpInterpolasyon.UnlockBits(dstData2);

            kaynak24.Dispose();
            sw.Stop();

            // 4. PictureBox'lara atama
            picDegistirme.Image?.Dispose();
            picDegistirme.SizeMode = PictureBoxSizeMode.Zoom;
            picDegistirme.Image = bmpDegistirme;

            picInterpolasyon.Image?.Dispose();
            picInterpolasyon.SizeMode = PictureBoxSizeMode.Zoom;
            picInterpolasyon.Image = bmpInterpolasyon;

            rozetDegistirme.MetinGuncelle($"{katsayi}x • {hedefW}×{hedefH}");
            rozetInterpolasyon.MetinGuncelle($"{katsayi}x • {hedefW}×{hedefH}");

            lblBilgiKart.Text =
                "Son İşlem İstatistikleri:\n\n" +
                $"• Büyütme Katsayısı: {katsayi}x\n" +
                $"• Orijinal Boyut: {kaynakW} × {kaynakH} px\n" +
                $"• Yeni Boyut: {hedefW} × {hedefH} px\n" +
                $"• İşlem Süresi: {sw.ElapsedMilliseconds} ms\n\n" +
                "Analiz: Orta resimde piksel bloklaşması oluşurken sağdaki resimde kenar geçişleri pürüzsüzleştirildi.";

            lblAnaliz.Text = $"Katsayı: {katsayi}x • Orijinal: {kaynakW}×{kaynakH} px → Yeni: {hedefW}×{hedefH} px • Süre: {sw.ElapsedMilliseconds} ms";
            this.Cursor = Cursors.Default;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            orijinalBmp?.Dispose();
            base.OnFormClosed(e);
        }
    }
}