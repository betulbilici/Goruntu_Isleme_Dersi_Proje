using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace GoruntuIsleme
{
    public partial class Hafta3Odev10 : Form
    {
        private Bitmap orijinalBmp = null;
        private Bitmap dondurulmusBmp = null;

        private RectangleF resimRect;
        private PointF merkezNokta;

        private bool fareBasildi = false;
        private float baslangicAci = 0f;
        private float anlikAci = 0f;
        private float sonSabitAci = 0f;

        private ModernHapRozet rozetTuval;
        private Label lblDurumKart;
        private Label lblAnaliz;

        public Hafta3Odev10()
        {
            InitializeComponent();
            ModernArayuzuUygula();
        }

        private void ModernArayuzuUygula()
        {
            // 1. Form Temel Ayarları (1250x750, Standart Kenarlık ve X ile Ana Menüye Dönüş)
            FormGezintiYardimcisi.FormuHazirla(this);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Text = "Hafta 3 - Ödev 10: Mouse ile İnteraktif Resim Döndürme";

            // Designer'daki tüm eski kontrolleri gizle
            foreach (Control ctrl in this.Controls)
            {
                ctrl.Visible = false;
            }

            // 2. Üst Gezinti Barı
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                form: this,
                baslik: "Hafta 3 - Ödev 10 | Fare ile İnteraktif Döndürme (Tel Çerçeve Önizleme & Ters Dönüşüm)",
                oncekiYazi: "< Ödev 9",
                oncekiTikla: () =>
                {
                    Form odev9 = Application.OpenForms["Hafta3Odev9"];
                    if (odev9 != null)
                        FormGezintiYardimcisi.FormGecisYap(this, odev9);
                    else
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta3Odev9());
                },
                sonrakiYazi: "Ödev 11 >",
                sonrakiTikla: () =>
                {
                    Form odev11 = Application.OpenForms["Hafta3Odev11"];
                    if (odev11 != null)
                        FormGezintiYardimcisi.FormGecisYap(this, odev11);
                    else
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta3Odev11());
                }
            );

            // 3. Sağ Kontrol Paneli
            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 290);

            Label lblKontrolBaslik = new Label
            {
                Text = "İnteraktif Döndürme",
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

            // Açıyı Sıfırla Butonu
            ModernHapButon btnModernSifirla = FormGezintiYardimcisi.ModernButonOlustur("🔄 Açıyı Sıfırla (0°)", false, 252, 38);
            btnModernSifirla.Location = new Point(18, 96);
            btnModernSifirla.Click += btnSifirla_Click;
            pnlSag.Controls.Add(btnModernSifirla);

            // Açık Mavi Durum ve Kullanım Bilgi Kartı
            ModernKartPanel bilgiKarti = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                konum: new Point(18, 150),
                boyut: new Size(252, 395),
                icerikEtiketi: out lblDurumKart
            );
            lblDurumKart.Text =
                "Çalışma Mantığı:\n\n" +
                "1. 'Resim Seç' ile görüntüyü tuval merkezine yerleştirin.\n\n" +
                "2. Tuval üzerinde sol tuşa basılı tutup fareyi merkez etrafında çevirin: 4 kenarlı tel çerçeve ve açı kolu fareyi takip eder.\n\n" +
                "3. Fare tuşunu bıraktığınız anda görüntü, son dikdörtgen sınırları içinde ters dönüşüm (Inverse Mapping) ile yeniden hesaplanır.";
            pnlSag.Controls.Add(bilgiKarti);

            // 4. Orta Çalışma Alanı (Üst: İnteraktif Tuval Kartı | Alt: Analiz Şeridi)
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

            // Tuval Kartı
            ModernKartPanel tuvalKart = new ModernKartPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(6),
                Padding = new Padding(12)
            };

            Panel pnlTuvalUst = new Panel
            {
                Dock = DockStyle.Top,
                Height = 28,
                BackColor = Color.White
            };

            Label lblTuvalBaslik = new Label
            {
                Text = "İnteraktif Döndürme Alanı (Sol Tuşa Basılı Tutup Fareyi Merkez Etrafında Çevirin)",
                UseMnemonic = false,
                AutoSize = true,
                Location = new Point(0, 4),
                Font = new Font("Segoe UI Semibold", 9.8f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi
            };
            pnlTuvalUst.Controls.Add(lblTuvalBaslik);

            rozetTuval = new ModernHapRozet();
            rozetTuval.MetinGuncelle("Açı: 0.0°");
            rozetTuval.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            rozetTuval.Location = new Point(pnlTuvalUst.Width - rozetTuval.Width - 2, 1);
            pnlTuvalUst.Resize += (s, e) =>
            {
                rozetTuval.Location = new Point(pnlTuvalUst.Width - rozetTuval.Width - 2, 1);
            };
            pnlTuvalUst.Controls.Add(rozetTuval);

            picCanvas.Parent?.Controls.Remove(picCanvas);
            picCanvas.Visible = true;
            picCanvas.Dock = DockStyle.Fill;
            picCanvas.BorderStyle = BorderStyle.None;
            picCanvas.BackColor = Color.FromArgb(241, 245, 249);
            picCanvas.MouseDown -= PicCanvas_MouseDown;
            picCanvas.MouseMove -= PicCanvas_MouseMove;
            picCanvas.MouseUp -= PicCanvas_MouseUp;
            picCanvas.Paint -= PicCanvas_Paint;

            picCanvas.MouseDown += PicCanvas_MouseDown;
            picCanvas.MouseMove += PicCanvas_MouseMove;
            picCanvas.MouseUp += PicCanvas_MouseUp;
            picCanvas.Paint += PicCanvas_Paint;
            picCanvas.Resize += (s, e) =>
            {
                ResimKonumunuOrtala();
                picCanvas.Invalidate();
            };

            tuvalKart.Controls.Add(picCanvas);
            tuvalKart.Controls.Add(pnlTuvalUst);

            // Alt Analiz Şeridi
            ModernKartPanel altSerit = FormGezintiYardimcisi.AltAnalizSeridiOlustur(
                "Bir resim seçin; fareye basılı tutup çevirdiğinizde tel çerçeve açıyı gösterir, bıraktığınızda görüntü döndürülür.",
                out lblAnaliz,
                "İnteraktif Açı"
            );

            anaYerlesim.Controls.Add(tuvalKart, 0, 0);
            anaYerlesim.Controls.Add(altSerit, 0, 1);
            pnlOrta.Controls.Add(anaYerlesim);

            this.Controls.Add(pnlOrta);
            FormGezintiYardimcisi.YerlesimiDuzenle(this, pnlUst, pnlSag, pnlOrta);
        }

        private void ResimKonumunuOrtala()
        {
            if (orijinalBmp == null || picCanvas.ClientSize.Width <= 0 || picCanvas.ClientSize.Height <= 0) return;

            float w = Math.Min(orijinalBmp.Width, picCanvas.ClientSize.Width * 0.55f);
            float h = w * ((float)orijinalBmp.Height / orijinalBmp.Width);
            if (h > picCanvas.ClientSize.Height * 0.65f)
            {
                h = picCanvas.ClientSize.Height * 0.65f;
                w = h * ((float)orijinalBmp.Width / orijinalBmp.Height);
            }

            float x = (picCanvas.ClientSize.Width - w) / 2f;
            float y = (picCanvas.ClientSize.Height - h) / 2f;

            resimRect = new RectangleF(x, y, w, h);
            merkezNokta = new PointF(resimRect.X + resimRect.Width / 2f, resimRect.Y + resimRect.Height / 2f);
        }

        private void btnResimSec_Click(object sender, EventArgs e)
        {
            string dosyaYolu = FormGezintiYardimcisi.ResimDosyasiSec("İnteraktif Döndürme İçin Resim Seçiniz");
            if (string.IsNullOrEmpty(dosyaYolu)) return;

            orijinalBmp?.Dispose();
            dondurulmusBmp?.Dispose();

            using (Bitmap temp = new Bitmap(dosyaYolu))
            {
                orijinalBmp = new Bitmap(temp);
            }

            ResimKonumunuOrtala();

            anlikAci = 0f;
            sonSabitAci = 0f;
            dondurulmusBmp = new Bitmap(orijinalBmp);

            GuncelleBilgi();
            picCanvas.Invalidate();
        }

        private void btnSifirla_Click(object sender, EventArgs e)
        {
            if (orijinalBmp == null) return;

            anlikAci = 0f;
            sonSabitAci = 0f;

            dondurulmusBmp?.Dispose();
            dondurulmusBmp = new Bitmap(orijinalBmp);

            GuncelleBilgi();
            picCanvas.Invalidate();
        }

        // Fare ile merkez arasındaki açıyı derece cinsinden hesaplar
        private float FareninMerkezeAcisi(PointF fareKonumu)
        {
            float dx = fareKonumu.X - merkezNokta.X;
            float dy = fareKonumu.Y - merkezNokta.Y;
            float radyan = (float)Math.Atan2(dy, dx);
            return radyan * (180f / (float)Math.PI);
        }

        private void PicCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || orijinalBmp == null) return;

            fareBasildi = true;
            baslangicAci = FareninMerkezeAcisi(e.Location) - sonSabitAci;
            picCanvas.Cursor = Cursors.Hand;
        }

        private void PicCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!fareBasildi || orijinalBmp == null) return;

            // Fare basılıyken 4 kenar çizgisi fareyi takip edecek şekilde döner
            anlikAci = FareninMerkezeAcisi(e.Location) - baslangicAci;
            GuncelleBilgi();
            picCanvas.Invalidate();
        }

        private void PicCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (!fareBasildi || orijinalBmp == null) return;

            fareBasildi = false;
            sonSabitAci = anlikAci;
            picCanvas.Cursor = Cursors.Default;

            // Mouse bırakıldığı anda resim en son dikdörtgenin içinde dönmüş olarak hesaplanır
            ResmiDondurVeUygula(sonSabitAci);
            GuncelleBilgi();
            picCanvas.Invalidate();
        }

        // Resmi merkez etrafında verilen açıyla döndürür (Ters dönüşüm + LockBits + Parallel)
        private void ResmiDondurVeUygula(float aciDerece)
        {
            if (orijinalBmp == null) return;

            dondurulmusBmp?.Dispose();
            dondurulmusBmp = new Bitmap(orijinalBmp.Width, orijinalBmp.Height, PixelFormat.Format24bppRgb);

            float aciRadyan = aciDerece * ((float)Math.PI / 180f);
            float cosA = (float)Math.Cos(-aciRadyan);
            float sinA = (float)Math.Sin(-aciRadyan);

            float cx = orijinalBmp.Width / 2f;
            float cy = orijinalBmp.Height / 2f;

            Bitmap kaynak24 = new Bitmap(orijinalBmp.Width, orijinalBmp.Height, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(kaynak24))
            {
                g.DrawImage(orijinalBmp, 0, 0, orijinalBmp.Width, orijinalBmp.Height);
            }

            BitmapData srcData = kaynak24.LockBits(
                new Rectangle(0, 0, kaynak24.Width, kaynak24.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);

            int srcStride = srcData.Stride;
            byte[] srcBuffer = new byte[Math.Abs(srcStride) * kaynak24.Height];
            Marshal.Copy(srcData.Scan0, srcBuffer, 0, srcBuffer.Length);
            kaynak24.UnlockBits(srcData);

            BitmapData dstData = dondurulmusBmp.LockBits(
                new Rectangle(0, 0, dondurulmusBmp.Width, dondurulmusBmp.Height),
                ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb);

            int dstStride = dstData.Stride;
            byte[] dstBuffer = new byte[Math.Abs(dstStride) * dondurulmusBmp.Height];

            int w = dondurulmusBmp.Width;
            int h = dondurulmusBmp.Height;

            Parallel.For(0, h, y =>
            {
                int rowIdx = y * dstStride;
                for (int x = 0; x < w; x++)
                {
                    float dx = x - cx;
                    float dy = y - cy;

                    int srcX = (int)Math.Round(cx + (dx * cosA - dy * sinA));
                    int srcY = (int)Math.Round(cy + (dx * sinA + dy * cosA));

                    int dIdx = rowIdx + (x * 3);

                    if (srcX >= 0 && srcX < w && srcY >= 0 && srcY < h)
                    {
                        int sIdx = (srcY * srcStride) + (srcX * 3);
                        dstBuffer[dIdx] = srcBuffer[sIdx];
                        dstBuffer[dIdx + 1] = srcBuffer[sIdx + 1];
                        dstBuffer[dIdx + 2] = srcBuffer[sIdx + 2];
                    }
                    else
                    {
                        dstBuffer[dIdx] = 255;
                        dstBuffer[dIdx + 1] = 255;
                        dstBuffer[dIdx + 2] = 255;
                    }
                }
            });

            Marshal.Copy(dstBuffer, 0, dstData.Scan0, dstBuffer.Length);
            dondurulmusBmp.UnlockBits(dstData);
            kaynak24.Dispose();
        }

        private void PicCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            // Arka plan koordinat ızgarası
            using (Pen pIzgara = new Pen(Color.FromArgb(226, 232, 240), 1))
            {
                for (int x = 0; x < picCanvas.Width; x += 40)
                    g.DrawLine(pIzgara, x, 0, x, picCanvas.Height);
                for (int y = 0; y < picCanvas.Height; y += 40)
                    g.DrawLine(pIzgara, 0, y, picCanvas.Width, y);
            }

            if (orijinalBmp == null) return;

            // 1. Resmi Çizme
            if (dondurulmusBmp != null)
            {
                g.DrawImage(dondurulmusBmp, resimRect);
                using (Pen pSabitSinir = new Pen(Color.FromArgb(148, 163, 184), 1.2f))
                {
                    g.DrawRectangle(pSabitSinir, resimRect.X, resimRect.Y, resimRect.Width, resimRect.Height);
                }
            }

            // 2. Merkez Kırmızı Nokta
            using (Brush bKirmizi = new SolidBrush(Color.FromArgb(220, 38, 38)))
            using (Pen pBeyaz = new Pen(Color.White, 1.5f))
            {
                g.FillEllipse(bKirmizi, merkezNokta.X - 5, merkezNokta.Y - 5, 10, 10);
                g.DrawEllipse(pBeyaz, merkezNokta.X - 5, merkezNokta.Y - 5, 10, 10);
            }

            // 3. Fare basılı tutulurken dönen 4 kenar çizgisi (Önizleme Tel Çerçevesi)
            if (fareBasildi)
            {
                GraphicsState state = g.Save();

                g.TranslateTransform(merkezNokta.X, merkezNokta.Y);
                g.RotateTransform(anlikAci);

                float yerelW = resimRect.Width;
                float yerelH = resimRect.Height;
                RectangleF yerelRect = new RectangleF(-yerelW / 2f, -yerelH / 2f, yerelW, yerelH);

                using (Pen penCizgi = new Pen(Color.FromArgb(14, 165, 233), 2.2f))
                {
                    penCizgi.DashStyle = DashStyle.Dash;
                    g.DrawRectangle(penCizgi, yerelRect.X, yerelRect.Y, yerelRect.Width, yerelRect.Height);
                }

                // Döndürme kolu (Merkezden sağ kenara doğru yön göstergesi)
                using (Pen penKol = new Pen(Color.FromArgb(249, 115, 22), 2f))
                {
                    g.DrawLine(penKol, 0, 0, yerelW / 2f + 26, 0);
                }

                g.Restore(state);
            }
        }

        private void GuncelleBilgi()
        {
            if (orijinalBmp == null) return;

            float gosterilenAci = fareBasildi ? anlikAci : sonSabitAci;
            float modAci = (gosterilenAci % 360f + 360f) % 360f;
            string durumMetni = fareBasildi
                ? "Döndürülüyor (Tel çerçeve fareyi takip ediyor)"
                : "Sabitlendi (Resim son açıya göre hesaplandı)";

            rozetTuval.MetinGuncelle($"Açı: {modAci:F1}°");

            lblDurumKart.Text =
                "Canlı Döndürme Durumu:\n\n" +
                $"• Resim Boyutu: {orijinalBmp.Width} × {orijinalBmp.Height} px\n" +
                $"• Merkez Noktası: ({Math.Round(merkezNokta.X)}, {Math.Round(merkezNokta.Y)})\n" +
                $"• Dönme Açısı: {modAci:F1}°\n" +
                $"• Durum: {durumMetni}\n\n" +
                "Fareyi basılı tutup çevirirken önizleme çerçevesi görünür; bıraktığınızda piksel matrisi güncellenir.";

            lblAnaliz.Text = $"Merkez: ({Math.Round(merkezNokta.X)}, {Math.Round(merkezNokta.Y)}) • Dönme Açısı: {modAci:F1}° • {durumMetni}";
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            orijinalBmp?.Dispose();
            dondurulmusBmp?.Dispose();
            base.OnFormClosed(e);
        }
    }
}