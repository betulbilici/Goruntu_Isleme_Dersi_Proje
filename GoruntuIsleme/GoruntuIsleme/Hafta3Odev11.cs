using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace GoruntuIsleme
{
    public partial class Hafta3Odev11 : Form
    {
        private Bitmap orijinalBmp = null;
        private Bitmap islenmisBmp = null;
        private const int KAYDIRMA_PIKSEL = 50; // Köşelerden 50 piksel

        private ModernHapRozet rozetOrijinal;
        private ModernHapRozet rozetKaydirilmis;
        private Label lblKatsayiKart;
        private Label lblAnaliz;

        public Hafta3Odev11()
        {
            InitializeComponent();
            ModernArayuzuUygula();
        }

        private void ModernArayuzuUygula()
        {
            // 1. Form Temel Ayarları (1250x750, Standart Kenarlık ve X ile Ana Menüye Dönüş)
            FormGezintiYardimcisi.FormuHazirla(this);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Text = "Hafta 3 - Ödev 11: Açı Olmadan Kaydırma (Shear) ile Döndürme";

            // Designer'daki tüm eski kontrolleri gizle
            foreach (Control ctrl in this.Controls)
            {
                ctrl.Visible = false;
            }

            // 2. Üst Gezinti Barı
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                form: this,
                baslik: "Hafta 3 - Ödev 11 | Açı Kullanmadan 50 px Kaydırma (Shear) ile Döndürme",
                oncekiYazi: "< Ödev 10",
                oncekiTikla: () =>
                {
                    Form odev10 = Application.OpenForms["Hafta3Odev10"];
                    if (odev10 != null)
                        FormGezintiYardimcisi.FormGecisYap(this, odev10);
                    else
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta3Odev10());
                },
                sonrakiYazi: "Ödev 12 >",
                sonrakiTikla: () =>
                {
                    Form odev12 = Application.OpenForms["Hafta3Odev12"];
                    if (odev12 != null)
                        FormGezintiYardimcisi.FormGecisYap(this, odev12);
                    else
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta3Odev12());
                }
            );

            // 3. Sağ Kontrol Paneli
            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 290);

            Label lblKontrolBaslik = new Label
            {
                Text = "Shear Döndürme Kontrolleri",
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

            // Saat Yönünde (50 px) Butonu
            ModernHapButon btnModernSaatYonu = FormGezintiYardimcisi.ModernButonOlustur("↻ Saat Yönünde (50 px)", true, 252, 40);
            btnModernSaatYonu.Location = new Point(18, 96);
            btnModernSaatYonu.Click += (s, e) => KaydirmaUygula(saatYonu: true);
            pnlSag.Controls.Add(btnModernSaatYonu);

            // Saat Yönü Tersi (50 px) Butonu
            ModernHapButon btnModernSaatTersi = FormGezintiYardimcisi.ModernButonOlustur("↺ Saat Yönü Tersi (50 px)", false, 252, 40);
            btnModernSaatTersi.Location = new Point(18, 144);
            btnModernSaatTersi.Click += (s, e) => KaydirmaUygula(saatYonu: false);
            pnlSag.Controls.Add(btnModernSaatTersi);

            // Sıfırla Butonu
            ModernHapButon btnModernSifirla = FormGezintiYardimcisi.ModernButonOlustur("🔄 Orijinale Sıfırla", false, 252, 36);
            btnModernSifirla.Location = new Point(18, 192);
            btnModernSifirla.Click += btnSifirla_Click;
            pnlSag.Controls.Add(btnModernSifirla);

            // Açık Mavi Matris & Kaydırma Bilgi Kartı
            ModernKartPanel bilgiKarti = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                konum: new Point(18, 242),
                boyut: new Size(252, 310),
                icerikEtiketi: out lblKatsayiKart
            );
            lblKatsayiKart.Text =
                "Yöntem & Matris Bilgisi:\n\n" +
                "• Trigonometrik açı (sin/cos) yerine yatay ve dikey 50 px kaydırma katsayıları kullanılır.\n\n" +
                "• Shx = ± 50 / (H / 2)\n" +
                "• Shy = ∓ 50 / (W / 2)\n\n" +
                "• Ters Matris (Inverse Mapping) determinantı det = 1 - Shx·Shy ile boş piksel oluşmadan dönüşüm tamamlanır.";
            pnlSag.Controls.Add(bilgiKarti);

            // 4. Orta Çalışma Alanı (Üst: Orijinal & Kaydırılmış Kartları | Alt: Analiz Şeridi)
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

            // 2 Sütunlu Görüntü Izgarası
            TableLayoutPanel goruntuIzgara = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1
            };
            goruntuIzgara.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            goruntuIzgara.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            goruntuIzgara.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            rozetOrijinal = new ModernHapRozet(); rozetOrijinal.MetinGuncelle("Kaynak");
            rozetKaydirilmis = new ModernHapRozet(); rozetKaydirilmis.MetinGuncelle("50 px Shear");

            MevcutPictureBoxKartaTasi(goruntuIzgara, 0, 0, "Orijinal Resim", picOrijinal, FormGezintiYardimcisi.TemaZemin, rozetOrijinal);
            MevcutPictureBoxKartaTasi(goruntuIzgara, 1, 0, "Kaydırma ile Döndürülmüş Resim (50 px Shear)", picKaydirilmis, FormGezintiYardimcisi.TemaZemin, rozetKaydirilmis);

            // Alt Analiz Şeridi
            ModernKartPanel altSerit = FormGezintiYardimcisi.AltAnalizSeridiOlustur(
                "Bir resim seçin ve sağ panelden 'Saat Yönünde' veya 'Saat Yönü Tersi' butonlarıyla 50 px kaydırmalı döndürme uygulayın.",
                out lblAnaliz,
                "50 px Shear"
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
                Font = new Font("Segoe UI Semibold", 9.6f, FontStyle.Bold),
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
            string dosyaYolu = FormGezintiYardimcisi.ResimDosyasiSec("Kaydırma (Shear) ile Döndürme İçin Resim Seçiniz");
            if (string.IsNullOrEmpty(dosyaYolu)) return;

            orijinalBmp?.Dispose();
            islenmisBmp?.Dispose();
            islenmisBmp = null;

            using (Bitmap temp = new Bitmap(dosyaYolu))
            {
                orijinalBmp = new Bitmap(temp);
            }

            if (picOrijinal != null)
            {
                picOrijinal.Image?.Dispose();
                picOrijinal.SizeMode = PictureBoxSizeMode.Zoom;
                picOrijinal.Image = new Bitmap(orijinalBmp);
            }

            if (picKaydirilmis != null)
            {
                picKaydirilmis.Image?.Dispose();
                picKaydirilmis.SizeMode = PictureBoxSizeMode.Zoom;
                picKaydirilmis.Image = new Bitmap(orijinalBmp);
            }

            rozetOrijinal.MetinGuncelle($"{orijinalBmp.Width}×{orijinalBmp.Height}");
            rozetKaydirilmis.MetinGuncelle("Hazır");
            lblAnaliz.Text = $"Resim yüklendi ({orijinalBmp.Width}×{orijinalBmp.Height} px) • Saat yönünde veya tersinde 50 px kaydırma seçiniz.";
        }

        private void btnSifirla_Click(object sender, EventArgs e)
        {
            if (orijinalBmp == null) return;

            if (picKaydirilmis != null)
            {
                picKaydirilmis.Image?.Dispose();
                picKaydirilmis.Image = new Bitmap(orijinalBmp);
            }

            rozetKaydirilmis.MetinGuncelle("Sıfırlandı");
            lblAnaliz.Text = "Resim orijinal konumuna sıfırlandı.";
        }

        // Açı olmadan kaydırma yöntemiyle döndürme (Shear Mapping)
        private void KaydirmaUygula(bool saatYonu)
        {
            if (orijinalBmp == null)
            {
                MessageBox.Show("Lütfen önce bir resim seçiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.Cursor = Cursors.WaitCursor;

            int w = orijinalBmp.Width;
            int h = orijinalBmp.Height;

            // Kayma sonrası taşmaları kapsamak için tuval boyutları
            int pad = KAYDIRMA_PIKSEL * 2;
            int hedefW = w + pad;
            int hedefH = h + pad;

            // 1. Kaynak resmi 24-bit RGB olarak belleğe kilitleme
            Bitmap kaynak24 = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(kaynak24))
            {
                g.DrawImage(orijinalBmp, 0, 0, w, h);
            }

            BitmapData srcData = kaynak24.LockBits(
                new Rectangle(0, 0, w, h),
                ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);

            int srcStride = srcData.Stride;
            byte[] srcBuffer = new byte[Math.Abs(srcStride) * h];
            Marshal.Copy(srcData.Scan0, srcBuffer, 0, srcBuffer.Length);
            kaynak24.UnlockBits(srcData);

            // 2. Hedef bellek alanı
            islenmisBmp?.Dispose();
            islenmisBmp = new Bitmap(hedefW, hedefH, PixelFormat.Format24bppRgb);

            BitmapData dstData = islenmisBmp.LockBits(
                new Rectangle(0, 0, hedefW, hedefH),
                ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb);

            int dstStride = dstData.Stride;
            byte[] dstBuffer = new byte[Math.Abs(dstStride) * hedefH];

            // Beyaz arka plan doldurma
            for (int i = 0; i < dstBuffer.Length; i++) dstBuffer[i] = 255;

            // Kaydırma katsayıları: Saat yönünde veya tersinde yön değişimi
            float sign = saatYonu ? 1.0f : -1.0f;
            float shx = sign * ((float)KAYDIRMA_PIKSEL / (h / 2f));
            float shy = -sign * ((float)KAYDIRMA_PIKSEL / (w / 2f));

            float cxSrc = w / 2f;
            float cySrc = h / 2f;
            float cxDst = hedefW / 2f;
            float cyDst = hedefH / 2f;

            // Ters dönüşüm determinantı: [1 shx; shy 1] matrisinin tersi
            float det = 1.0f - (shx * shy);

            // 3. Paralel ters haritalama (Inverse Mapping)
            Parallel.For(0, hedefH, yd =>
            {
                int rowIdx = yd * dstStride;
                float yPrime = yd - cyDst;

                for (int xd = 0; xd < hedefW; xd++)
                {
                    float xPrime = xd - cxDst;

                    float xs = cxSrc + ((xPrime - shx * yPrime) / det);
                    float ys = cySrc + ((-shy * xPrime + yPrime) / det);

                    int srcX = (int)Math.Round(xs);
                    int srcY = (int)Math.Round(ys);

                    if (srcX >= 0 && srcX < w && srcY >= 0 && srcY < h)
                    {
                        int sIdx = (srcY * srcStride) + (srcX * 3);
                        int dIdx = rowIdx + (xd * 3);

                        dstBuffer[dIdx] = srcBuffer[sIdx];
                        dstBuffer[dIdx + 1] = srcBuffer[sIdx + 1];
                        dstBuffer[dIdx + 2] = srcBuffer[sIdx + 2];
                    }
                }
            });

            Marshal.Copy(dstBuffer, 0, dstData.Scan0, dstBuffer.Length);
            islenmisBmp.UnlockBits(dstData);
            kaynak24.Dispose();

            if (picKaydirilmis != null)
            {
                picKaydirilmis.Image?.Dispose();
                picKaydirilmis.SizeMode = PictureBoxSizeMode.Zoom;
                picKaydirilmis.Image = new Bitmap(islenmisBmp);
            }

            string yonMetni = saatYonu ? "Saat Yönünde (CW)" : "Saat Yönünün Tersine (CCW)";
            rozetKaydirilmis.MetinGuncelle(saatYonu ? "CW • 50 px" : "CCW • 50 px");

            lblKatsayiKart.Text =
                "Hesaplanan Shear Değerleri:\n\n" +
                $"• Yön: {yonMetni}\n" +
                $"• Kaydırma Miktarı: {KAYDIRMA_PIKSEL} px\n" +
                $"• Yatay Katsayı (Shx): {shx:F3}\n" +
                $"• Dikey Katsayı (Shy): {shy:F3}\n" +
                $"• Determinant (det): {det:F4}\n" +
                $"• Çıktı Tuvali: {hedefW} × {hedefH} px";

            lblAnaliz.Text = $"Yön: {yonMetni} • Kaydırma: {KAYDIRMA_PIKSEL} px • ShX = {shx:F3}, ShY = {shy:F3} (Açı kullanılmadan döndürüldü)";
            this.Cursor = Cursors.Default;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            orijinalBmp?.Dispose();
            islenmisBmp?.Dispose();
            base.OnFormClosed(e);
        }
    }
}