using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoruntuIsleme
{
    public partial class Hafta3Odev9 : Form
    {
        private Bitmap orijinalBmp = null;

        public Hafta3Odev9()
        {
            InitializeComponent();
            this.FormClosing += Hafta3Odev9_FormClosing;

            if (btnResimSec != null) btnResimSec.Click += btnResimSec_Click;
            if (btnBuyut != null) btnBuyut.Click += btnBuyut_Click;
            if (lnkGeri != null) lnkGeri.LinkClicked += lnkGeri_LinkClicked;
            if (lnkSonraki != null) lnkSonraki.LinkClicked += lnkSonraki_LinkClicked;
        }

        private void Hafta3Odev9_FormClosing(object sender, FormClosingEventArgs e)
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

                    if (picDegistirme != null) picDegistirme.Image = null;
                    if (picInterpolasyon != null) picInterpolasyon.Image = null;

                    if (lblBilgi != null)
                    {
                        lblBilgi.Text = $"Resim Yüklendi: {orijinalBmp.Width}x{orijinalBmp.Height} px\n" +
                                        $"Büyütme katsayısını belirleyip 'Resmi Büyüt' butonuna basınız.";
                    }
                }
            }
        }

        private void btnBuyut_Click(object sender, EventArgs e)
        {
            if (orijinalBmp == null)
            {
                MessageBox.Show("Lütfen önce bir resim seçiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int katsayi = (int)numKatsayi.Value;
            int kaynakW = orijinalBmp.Width;
            int kaynakH = orijinalBmp.Height;

            int hedefW = kaynakW * katsayi;
            int hedefH = kaynakH * katsayi;

            this.Cursor = Cursors.WaitCursor;

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
                    // --- A: PİKSEL DEĞİŞTİRME (NEAREST NEIGHBOR) ---
                    int srcX = Math.Min(x / katsayi, kaynakW - 1);
                    int srcPixelIdx = (rowSrcY * srcStride) + (srcX * 3);

                    int dIdx = rowDegistirmeIdx + (x * 3);
                    degistirmeBuffer[dIdx] = srcBuffer[srcPixelIdx];         // Blue
                    degistirmeBuffer[dIdx + 1] = srcBuffer[srcPixelIdx + 1]; // Green
                    degistirmeBuffer[dIdx + 2] = srcBuffer[srcPixelIdx + 2]; // Red

                    // --- B: PİKSEL İNTERPOLASYONU (BILINEAR) ---
                    float gx = (float)x / katsayi;
                    int x1 = Math.Min((int)Math.Floor(gx), kaynakW - 1);
                    int x2 = Math.Min(x1 + 1, kaynakW - 1);
                    float dx = gx - x1;

                    int idx11 = (y1 * srcStride) + (x1 * 3);
                    int idx21 = (y1 * srcStride) + (x2 * 3);
                    int idx12 = (y2 * srcStride) + (x1 * 3);
                    int idx22 = (y2 * srcStride) + (x2 * 3);

                    for (int c = 0; c < 3; c++) // c = 0: B, 1: G, 2: R
                    {
                        byte val11 = srcBuffer[idx11 + c];
                        byte val21 = srcBuffer[idx21 + c];
                        byte val12 = srcBuffer[idx12 + c];
                        byte val22 = srcBuffer[idx22 + c];

                        float val = val11 * (1 - dx) * (1 - dy) +
                                    val21 * dx * (1 - dy) +
                                    val12 * (1 - dx) * dy +
                                    val22 * dx * dy;

                        interpolasyonBuffer[rowInterpIdx + (x * 3) + c] = (byte)Math.Max(0, Math.Min(255, (int)Math.Round(val)));
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

            // 4. PictureBox'lara atama
            if (picDegistirme != null)
            {
                picDegistirme.Image?.Dispose();
                picDegistirme.SizeMode = PictureBoxSizeMode.Zoom;
                picDegistirme.Image = bmpDegistirme;
            }

            if (picInterpolasyon != null)
            {
                picInterpolasyon.Image?.Dispose();
                picInterpolasyon.SizeMode = PictureBoxSizeMode.Zoom;
                picInterpolasyon.Image = bmpInterpolasyon;
            }

            if (lblBilgi != null)
            {
                lblBilgi.Text = $"İşlem Tamamlandı:\n" +
                                $"• Büyütme Katsayısı: {katsayi}x\n" +
                                $"• Orijinal Boyut   : {kaynakW}x{kaynakH} px\n" +
                                $"• Yeni Boyut       : {hedefW}x{hedefH} px\n" +
                                $"• Analiz: Orta resimde piksel bloklaşması oluşurken sağdaki resimde kenar geçişleri pürüzsüzleştirildi.";
            }

            this.Cursor = Cursors.Default;
        }

        private void lnkGeri_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta3Odev9_FormClosing;
            Hafta3Odev8 odev8 = new Hafta3Odev8();
            odev8.Show();
            this.Close();
        }

        private void lnkSonraki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta3Odev9_FormClosing;
            Hafta3Odev10 odev10 = new Hafta3Odev10();
            odev10.Show();
            this.Close();
        }
    }
}