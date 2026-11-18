using System;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace GoruntuIsleme
{
    public partial class Hafta2Odev9 : Form
    {
        private Mat orijinalMat = null;
        private int[] griHistogram = new int[256];
        private System.Drawing.Point? bolgeselIsikMerkezi = null;

        public Hafta2Odev9()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1250, 750);
            this.StartPosition = FormStartPosition.CenterScreen;

            this.FormClosing += Hafta2Odev9_FormClosing;

            // Olay bağlantıları
            if (btnResimSec != null) btnResimSec.Click += btnResimSec_Click;
            if (btnSifirlaBolge != null) btnSifirlaBolge.Click += btnSifirlaBolge_Click;
            if (lnkGeri != null) lnkGeri.LinkClicked += lnkGeri_LinkClicked;
            // Constructor (Hafta2Odev9()) içine ekle:
            if (lnkSonraki != null) lnkSonraki.LinkClicked += lnkSonraki_LinkClicked;

            if (trackEsik != null) trackEsik.Scroll += (s, e) => { GuncelleEtiketler(); AlgoritmayiUygula(); };
            if (trackMin != null) trackMin.Scroll += (s, e) => { GuncelleEtiketler(); AlgoritmayiUygula(); };
            if (trackYaricap != null) trackYaricap.Scroll += (s, e) => { GuncelleEtiketler(); AlgoritmayiUygula(); };
            if (chkYesilMod != null) chkYesilMod.CheckedChanged += (s, e) => AlgoritmayiUygula();

            // Sol resme tıklayarak belli bir bölgeye ışık verme
            if (picGiris != null)
            {
                picGiris.MouseDown += PicGiris_MouseDown;
            }
        }

        private void Hafta2Odev9_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Form1 anaForm = (Form1)Application.OpenForms["Form1"];
                if (anaForm != null)
                {
                    anaForm.StartPosition = FormStartPosition.Manual;
                    anaForm.Location = this.Location;
                    anaForm.Size = this.Size;
                    anaForm.Show();
                    anaForm.BringToFront();
                }
                else
                {
                    Form1 yeniForm = new Form1();
                    yeniForm.StartPosition = FormStartPosition.Manual;
                    yeniForm.Location = this.Location;
                    yeniForm.Size = this.Size;
                    yeniForm.Show();
                }
            }
        }

        private void GuncelleEtiketler()
        {
            if (lblEsikBilgi != null && trackEsik != null) lblEsikBilgi.Text = $"Kırılma (Mid): {trackEsik.Value}";
            if (lblMinBilgi != null && trackMin != null) lblMinBilgi.Text = $"Alt Sınır: {trackMin.Value}";
            if (lblYaricapBilgi != null && trackYaricap != null) lblYaricapBilgi.Text = $"Işık Yarıçapı: {trackYaricap.Value} px";
        }

        // Resim Seçme
        private void btnResimSec_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    orijinalMat?.Dispose();
                    orijinalMat = Cv2.ImRead(ofd.FileName, ImreadModes.Color);

                    if (orijinalMat.Empty())
                    {
                        MessageBox.Show("Resim yüklenemedi!");
                        return;
                    }

                    bolgeselIsikMerkezi = null;
                    picGiris.Image?.Dispose();
                    picGiris.Image = BitmapConverter.ToBitmap(orijinalMat);

                    HistogramHesapla();
                    AlgoritmayiUygula();
                }
            }
        }

        // Resim üzerinde tıklanan noktayı bölgesel ışık merkezi yapma
        private void PicGiris_MouseDown(object sender, MouseEventArgs e)
        {
            if (orijinalMat == null || picGiris.Width <= 0 || picGiris.Height <= 0) return;

            int rx = (int)((float)e.X / picGiris.Width * orijinalMat.Width);
            int ry = (int)((float)e.Y / picGiris.Height * orijinalMat.Height);

            bolgeselIsikMerkezi = new System.Drawing.Point(rx, ry);
            AlgoritmayiUygula();
        }

        private void btnSifirlaBolge_Click(object sender, EventArgs e)
        {
            bolgeselIsikMerkezi = null;
            AlgoritmayiUygula();
        }

        // Orijinal görüntünün gri tonlama histogramını çıkarma
        private void HistogramHesapla()
        {
            if (orijinalMat == null) return;
            Array.Clear(griHistogram, 0, 256);

            for (int y = 0; y < orijinalMat.Height; y++)
            {
                for (int x = 0; x < orijinalMat.Width; x++)
                {
                    Vec3b c = orijinalMat.At<Vec3b>(y, x);
                    int gri = (int)(c.Item2 * 0.299 + c.Item1 * 0.587 + c.Item0 * 0.114);
                    griHistogram[Math.Max(0, Math.Min(255, gri))]++;
                }
            }
        }

        // Slayttaki Histogram Grafiğini ve Mavi Kırılma Sınırını Çizdirme
        private void HistogramCiz(int kirilmaNoktasi)
        {
            if (picHistogram == null || picHistogram.Width <= 0 || picHistogram.Height <= 0) return;

            int w = picHistogram.Width;
            int h = picHistogram.Height;
            Bitmap bmp = new Bitmap(w, h);

            int max = 1;
            for (int i = 0; i < 256; i++)
            {
                if (griHistogram[i] > max) max = griHistogram[i];
            }

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Black);

                // Histogram barları (Sarı ton - slayttaki gibi)
                using (Pen pBar = new Pen(Color.FromArgb(240, 220, 50), 1))
                {
                    for (int i = 0; i < 256; i++)
                    {
                        float x = (float)i / 255f * (w - 1);
                        float barH = ((float)griHistogram[i] / max) * (h - 15);
                        g.DrawLine(pBar, x, h - 1, x, h - 1 - barH);
                    }
                }

                // Kılavuz referans dikey çizgileri (Slayttaki kırmızı çizgiler)
                using (Pen pRed = new Pen(Color.FromArgb(180, 50, 50), 1))
                {
                    for (int i = 32; i < 256; i += 32)
                    {
                        float xRef = (float)i / 255f * (w - 1);
                        g.DrawLine(pRed, xRef, 0, xRef, h);
                    }
                }

                // Slaytta belirtilen hareketli mavi sınır çizgisi
                float xMavi = (float)kirilmaNoktasi / 255f * (w - 1);
                using (Pen pBlue = new Pen(Color.DeepSkyBlue, 3))
                {
                    g.DrawLine(pBlue, xMavi, 5, xMavi, h - 5);
                }

                using (Font f = new Font("Arial", 9, FontStyle.Bold))
                using (Brush bYazi = new SolidBrush(Color.DeepSkyBlue))
                {
                    g.DrawString($"Eşik: {kirilmaNoktasi}", f, bYazi, Math.Max(5, xMavi - 30), 8);
                }
            }

            picHistogram.Image?.Dispose();
            picHistogram.Image = bmp;
        }

        // Ödev 9 Algoritması: Dinamik Kontrast Germe, Kırılma ve Bölgesel Işık Artırma
        private void AlgoritmayiUygula()
        {
            if (orijinalMat == null) return;

            int mid = trackEsik != null ? trackEsik.Value : 128;
            int minVal = trackMin != null ? trackMin.Value : 15;
            int yaricap = trackYaricap != null ? trackYaricap.Value : 80;
            bool yesilMod = chkYesilMod != null && chkYesilMod.Checked;

            HistogramCiz(mid);

            Mat islenmis = new Mat(orijinalMat.Height, orijinalMat.Width, MatType.CV_8UC3);

            // Lookup Table (LUT) - Parçalı Doğrusal Kontrast Germe:
            // 0 - Mid arası: Koyu pikseller hızla açılarak ayrıştırılır (0 - 180 aralığına gerilir).
            // Mid - 255 arası: Parlak pikseller patlamasın diye birbirine yaklaştırılır (180 - 255 aralığına sıkıştırılır).
            byte[] lut = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                if (i <= minVal)
                {
                    lut[i] = 0;
                }
                else if (i <= mid)
                {
                    double oran = (double)(i - minVal) / Math.Max(1, (mid - minVal));
                    lut[i] = (byte)Math.Min(255, Math.Pow(oran, 0.6) * 180.0); // Gamma düzeltmesiyle koyu yerleri aç
                }
                else
                {
                    double oran = (double)(i - mid) / Math.Max(1, (255 - mid));
                    lut[i] = (byte)Math.Min(255, 180.0 + (oran * 75.0)); // Parlak yerleri birbirine yaklaştır
                }
            }

            double yaricapKare = yaricap * yaricap;

            for (int y = 0; y < orijinalMat.Height; y++)
            {
                for (int x = 0; x < orijinalMat.Width; x++)
                {
                    Vec3b bgr = orijinalMat.At<Vec3b>(y, x);

                    // 1. Dinamik LUT dönüşümü
                    double b = lut[bgr.Item0];
                    double g = lut[bgr.Item1];
                    double r = lut[bgr.Item2];

                    // 2. Belli bölgelerde ışık artırma işlemi (Radyal Spot Işık Maskesi)
                    if (bolgeselIsikMerkezi.HasValue)
                    {
                        double dx = x - bolgeselIsikMerkezi.Value.X;
                        double dy = y - bolgeselIsikMerkezi.Value.Y;
                        double dKare = (dx * dx) + (dy * dy);

                        if (dKare < yaricapKare)
                        {
                            double isikFaktoru = 1.0 + (1.2 * (1.0 - Math.Sqrt(dKare) / yaricap));
                            b *= isikFaktoru;
                            g *= isikFaktoru;
                            r *= isikFaktoru;
                        }
                    }

                    // 3. Gece Görüş Dürbünü Efekti (Yeşil kanal baskınlığı)
                    if (yesilMod)
                    {
                        double griDeger = (r * 0.299 + g * 0.587 + b * 0.114);
                        b = griDeger * 0.15;
                        g = Math.Min(255, griDeger * 1.35); // Gece görüş fosfor yeşili
                        r = griDeger * 0.20;
                    }

                    b = Math.Max(0, Math.Min(255, b));
                    g = Math.Max(0, Math.Min(255, g));
                    r = Math.Max(0, Math.Min(255, r));

                    islenmis.Set(y, x, new Vec3b((byte)b, (byte)g, (byte)r));
                }
            }

            // Bölgesel ışık merkezinin gösterimi
            Bitmap bmpCikis = BitmapConverter.ToBitmap(islenmis);
            islenmis.Dispose();

            if (bolgeselIsikMerkezi.HasValue)
            {
                using (Graphics gr = Graphics.FromImage(bmpCikis))
                using (Pen cemberKalem = new Pen(Color.Cyan, 2))
                {
                    cemberKalem.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                    gr.DrawEllipse(cemberKalem,
                        bolgeselIsikMerkezi.Value.X - yaricap,
                        bolgeselIsikMerkezi.Value.Y - yaricap,
                        yaricap * 2, yaricap * 2);
                }
            }

            picCikis.Image?.Dispose();
            picCikis.Image = bmpCikis;
        }

        // Ödev 8'e Geri Dön Linki
        private void lnkGeri_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta2Odev9_FormClosing;

            Hafta2Odev8 odev8 = new Hafta2Odev8();
            odev8.StartPosition = FormStartPosition.Manual;
            odev8.Location = this.Location;
            odev8.Size = this.Size;
            odev8.FormBorderStyle = this.FormBorderStyle;

            odev8.Show();
            this.Close();
        }

       
        // Ödev 10'a Git Linki
        private void lnkSonraki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Form1'in arka planda açılmasını engellemek için kapanış aboneliğini kaldırıyoruz:
            this.FormClosing -= Hafta2Odev9_FormClosing;

            Hafta2Odev10 odev10 = new Hafta2Odev10();
            odev10.StartPosition = FormStartPosition.Manual;
            odev10.Location = this.Location;
            odev10.Size = this.Size;
            odev10.FormBorderStyle = this.FormBorderStyle;

            odev10.Show();
            this.Close();
        }
    }
}