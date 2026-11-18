using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoruntuIsleme
{
    public partial class Hafta3Odev11 : Form
    {
        private Bitmap orijinalBmp = null;
        private Bitmap islenmisBmp = null;
        private const int KAYDIRMA_PIKSEL = 50; // Slayt şartı: Köşelerden 50 piksel

        public Hafta3Odev11()
        {
            InitializeComponent();
            this.FormClosing += Hafta3Odev11_FormClosing;

            if (btnResimSec != null) btnResimSec.Click += btnResimSec_Click;
            if (btnSaatYonu != null) btnSaatYonu.Click += (s, e) => KaydirmaUygula(saatYonu: true);
            if (btnSaatTersi != null) btnSaatTersi.Click += (s, e) => KaydirmaUygula(saatYonu: false);
            if (btnSifirla != null) btnSifirla.Click += btnSifirla_Click;
            if (lnkGeri != null) lnkGeri.LinkClicked += lnkGeri_LinkClicked;
            if (lnkSonraki != null) lnkSonraki.LinkClicked += lnkSonraki_LinkClicked;
        }

        private void Hafta3Odev11_FormClosing(object sender, FormClosingEventArgs e)
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
                    islenmisBmp?.Dispose();

                    using (Bitmap temp = new Bitmap(ofd.FileName))
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

                    if (lblBilgi != null)
                    {
                        lblBilgi.Text = $"Resim Yüklendi: {orijinalBmp.Width}x{orijinalBmp.Height} px\n" +
                                        $"Saat yönünde veya tersinde kaydırma komutuna basarak açı kullanmadan döndürme yapabilirsiniz.";
                    }
                }
            }
        }

        private void btnSifirla_Click(object sender, EventArgs e)
        {
            if (orijinalBmp == null) return;

            if (picKaydirilmis != null)
            {
                picKaydirilmis.Image?.Dispose();
                picKaydirilmis.Image = new Bitmap(orijinalBmp);
            }

            if (lblBilgi != null)
            {
                lblBilgi.Text = "Resim orijinal konumuna sıfırlandı.";
            }
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

                    // Ters matris çarpımı ile kaynak koordinatı bulma
                    float xs = cxSrc + ((xPrime - shx * yPrime) / det);
                    float ys = cySrc + ((-shy * xPrime + yPrime) / det);

                    int srcX = (int)Math.Round(xs);
                    int srcY = (int)Math.Round(ys);

                    if (srcX >= 0 && srcX < w && srcY >= 0 && srcY < h)
                    {
                        int sIdx = (srcY * srcStride) + (srcX * 3);
                        int dIdx = rowIdx + (xd * 3);

                        dstBuffer[dIdx] = srcBuffer[sIdx];         // Blue
                        dstBuffer[dIdx + 1] = srcBuffer[sIdx + 1]; // Green
                        dstBuffer[dIdx + 2] = srcBuffer[sIdx + 2]; // Red
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
                picKaydirilmis.Image = islenmisBmp;
            }

            if (lblBilgi != null)
            {
                lblBilgi.Text = $"İşlem: Açı kullanılmadan 50 piksel kaydırma ile döndürme yapıldı.\n" +
                                $"Yön: {(saatYonu ? "Saat Yönünde (CW)" : "Saat Yönünün Tersine (CCW)")} | Kaydırma Katsayıları: ShX={shx:F3}, ShY={shy:F3}";
            }

            this.Cursor = Cursors.Default;
        }

        private void lnkGeri_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta3Odev11_FormClosing;
            Hafta3Odev10 odev10 = new Hafta3Odev10();
            odev10.Show();
            this.Close();
        }

        private void lnkSonraki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta3Odev11_FormClosing;
            Hafta3Odev12 odev12 = new Hafta3Odev12();
            odev12.Show();
            this.Close();
        }
    }
}