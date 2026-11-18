using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

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

        public Hafta3Odev10()
        {
            InitializeComponent();
            this.FormClosing += Hafta3Odev10_FormClosing;

            if (btnResimSec != null) btnResimSec.Click += btnResimSec_Click;
            if (btnSifirla != null) btnSifirla.Click += btnSifirla_Click;
            if (lnkGeri != null) lnkGeri.LinkClicked += lnkGeri_LinkClicked;
            if (lnkSonraki != null) lnkSonraki.LinkClicked += lnkSonraki_LinkClicked;

            if (picCanvas != null)
            {
                picCanvas.MouseDown += PicCanvas_MouseDown;
                picCanvas.MouseMove += PicCanvas_MouseMove;
                picCanvas.MouseUp += PicCanvas_MouseUp;
                picCanvas.Paint += PicCanvas_Paint;
            }
        }

        private void Hafta3Odev10_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Form1 anaForm = (Form1)Application.OpenForms["Form1"];
                if (anaForm != null)
                {
                    anaForm.Show();
                    anaForm.BringToFront();
                }
                else
                {
                    new Form1().Show();
                }
            }
        }

        private void btnResimSec_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    orijinalBmp?.Dispose();
                    dondurulmusBmp?.Dispose();

                    using (Bitmap temp = new Bitmap(ofd.FileName))
                    {
                        orijinalBmp = new Bitmap(temp);
                    }

                    // Resmi çizim kutusunun ortasına yerleştir
                    float w = Math.Min(orijinalBmp.Width, picCanvas.Width * 0.5f);
                    float h = w * ((float)orijinalBmp.Height / orijinalBmp.Width);

                    float x = (picCanvas.Width - w) / 2f;
                    float y = (picCanvas.Height - h) / 2f;

                    resimRect = new RectangleF(x, y, w, h);
                    merkezNokta = new PointF(resimRect.X + resimRect.Width / 2f, resimRect.Y + resimRect.Height / 2f);

                    anlikAci = 0f;
                    sonSabitAci = 0f;
                    dondurulmusBmp = new Bitmap(orijinalBmp);

                    GuncelleBilgi();
                    picCanvas.Invalidate();
                }
            }
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
            float derece = radyan * (180f / (float)Math.PI);
            return derece;
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

            // Slayt şartı: Fare basılıyken 4 kenar çizgisi fareyi takip edecek şekilde döner
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

            // Slayt şartı: Mouse bırakıldığı anda resim en son dikdörtgenin içinde dönmüş olarak gözüksün
            ResmiDondurVeUygula(sonSabitAci);
            GuncelleBilgi();
            picCanvas.Invalidate();
        }

        // Resmi merkez etrafında verilen açıyla döndürür (Ters dönüşüm mantığı ile)
        private void ResmiDondurVeUygula(float aciDerece)
        {
            if (orijinalBmp == null) return;

            dondurulmusBmp?.Dispose();
            dondurulmusBmp = new Bitmap(orijinalBmp.Width, orijinalBmp.Height, PixelFormat.Format24bppRgb);

            float aciRadyan = aciDerece * ((float)Math.PI / 180f);
            float cosA = (float)Math.Cos(-aciRadyan); // Ters dönüşüm için negatif açı
            float sinA = (float)Math.Sin(-aciRadyan);

            float cx = orijinalBmp.Width / 2f;
            float cy = orijinalBmp.Height / 2f;

            // Kaynak resmi kilitleme
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

            // Hedef resmi kilitleme
            BitmapData dstData = dondurulmusBmp.LockBits(
                new Rectangle(0, 0, dondurulmusBmp.Width, dondurulmusBmp.Height),
                ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb);

            int dstStride = dstData.Stride;
            byte[] dstBuffer = new byte[Math.Abs(dstStride) * dondurulmusBmp.Height];

            int w = dondurulmusBmp.Width;
            int h = dondurulmusBmp.Height;

            System.Threading.Tasks.Parallel.For(0, h, y =>
            {
                int rowIdx = y * dstStride;
                for (int x = 0; x < w; x++)
                {
                    // Ters dönüşüm formülü: Hedef pikselden kaynak piksele gitme
                    float dx = x - cx;
                    float dy = y - cy;

                    int srcX = (int)Math.Round(cx + (dx * cosA - dy * sinA));
                    int srcY = (int)Math.Round(cy + (dx * sinA + dy * cosA));

                    int dIdx = rowIdx + (x * 3);

                    // Resmin sınırları içinde mi kontrolü
                    if (srcX >= 0 && srcX < w && srcY >= 0 && srcY < h)
                    {
                        int sIdx = (srcY * srcStride) + (srcX * 3);
                        dstBuffer[dIdx] = srcBuffer[sIdx];         // Blue
                        dstBuffer[dIdx + 1] = srcBuffer[sIdx + 1]; // Green
                        dstBuffer[dIdx + 2] = srcBuffer[sIdx + 2]; // Red
                    }
                    else
                    {
                        // Sınır dışı kalan yerler beyaz arka plan
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
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            if (orijinalBmp == null) return;

            // 1. Resmi Çizme
            if (dondurulmusBmp != null)
            {
                g.DrawImage(dondurulmusBmp, resimRect);
            }

            // 2. Merkez Kırmızı Nokta
            using (Brush bKirmizi = new SolidBrush(Color.Red))
            {
                g.FillEllipse(bKirmizi, merkezNokta.X - 4, merkezNokta.Y - 4, 8, 8);
            }

            // 3. Slayt Şartı: Fare basılı tutulurken dönen 4 kenar çizgisi (Önizleme Tel Çerçevesi)
            if (fareBasildi)
            {
                GraphicsState state = g.Save();

                // Çizim matrisini merkez noktaya taşı ve anlık açı kadar döndür
                g.TranslateTransform(merkezNokta.X, merkezNokta.Y);
                g.RotateTransform(anlikAci);

                // Merkez (0,0) olduğu için çerçeve yerel koordinatlara göre çizilir
                float yerelW = resimRect.Width;
                float yerelH = resimRect.Height;
                RectangleF yerelRect = new RectangleF(-yerelW / 2f, -yerelH / 2f, yerelW, yerelH);

                using (Pen penCizgi = new Pen(Color.FromArgb(0, 120, 215), 2f))
                {
                    penCizgi.DashStyle = DashStyle.Dash;
                    g.DrawRectangle(penCizgi, yerelRect.X, yerelRect.Y, yerelRect.Width, yerelRect.Height);
                }

                // Döndürme kolu (Merkezden fareye doğru çizgi)
                using (Pen penKol = new Pen(Color.OrangeRed, 1.5f))
                {
                    g.DrawLine(penKol, 0, 0, yerelW / 2f + 25, 0);
                }

                g.Restore(state);
            }
        }

        private void GuncelleBilgi()
        {
            if (lblBilgi == null || orijinalBmp == null) return;

            float gosterilenAci = fareBasildi ? anlikAci : sonSabitAci;
            // Açıyı 0-360 derece arasına normalize etme
            float modAci = (gosterilenAci % 360f + 360f) % 360f;

            lblBilgi.Text = $"• Merkez Noktası : ({Math.Round(merkezNokta.X)}, {Math.Round(merkezNokta.Y)})\n" +
                            $"• Dönme Açısı   : {Math.Round(modAci, 1)}°\n" +
                            $"• Durum         : {(fareBasildi ? "Döndürülüyor (Tel çerçeve fareyi takip ediyor...)" : "Sabitlendi (Resim son açıya göre yeniden hesaplandı)")}";
        }

        private void lnkGeri_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta3Odev10_FormClosing;
            Hafta3Odev9 odev9 = new Hafta3Odev9();
            odev9.Show();
            this.Close();
        }

        private void lnkSonraki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta3Odev10_FormClosing;
            Hafta3Odev11 odev11 = new Hafta3Odev11();
            odev11.Show();
            this.Close();
        }
    }
}