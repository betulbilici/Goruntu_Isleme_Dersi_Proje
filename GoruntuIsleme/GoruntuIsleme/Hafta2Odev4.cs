using System;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace GoruntuIsleme
{
    public partial class Hafta2Odev4 : Form
    {
        private Mat griMat = null;
        private int[] histogramVerisi = new int[256];
        private int x1 = 0;
        private int x2 = 255;

        public Hafta2Odev4()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            this.StartPosition = FormStartPosition.CenterScreen;

            // X butonu kapatma olayını bağlıyoruz:
            this.FormClosing += Hafta2Odev4_FormClosing;

            // Kutuların başlangıç değerleri
            if (txtX1 != null) txtX1.Text = "0";
            if (txtX2 != null) txtX2.Text = "255";
            if (txtY1 != null) txtY1.Text = "0";
            if (txtY2 != null) txtY2.Text = "255";

            // Olay bağlantıları
            if (btnResimSec != null) btnResimSec.Click += btnResimSec_Click;
            if (btnKontrast != null) btnKontrast.Click += btnKontrast_Click;
            if (lnkGeri != null) lnkGeri.LinkClicked += lnkGeri_LinkClicked;
            if (lnkSonraki != null) lnkSonraki.LinkClicked += lnkSonraki_LinkClicked;

            // Histogram kutusu üzerinde Sol ve Sağ Tık olayı
            if (picHistogram != null)
            {
                picHistogram.MouseDown += PicHistogram_MouseDown;
            }
        }

        // X butonuna basıldığında Form1'e dönen metot:
        private void Hafta2Odev4_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Form1 anaForm = (Form1)Application.OpenForms["Form1"];
                if (anaForm != null)
                {
                    anaForm.StartPosition = FormStartPosition.Manual;
                    anaForm.Location = this.Location;
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

        // Resim Seçme
        private void btnResimSec_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    using (Mat temp = Cv2.ImRead(ofd.FileName, ImreadModes.Color))
                    {
                        if (temp.Empty())
                        {
                            MessageBox.Show("Resim yüklenemedi!");
                            return;
                        }

                        griMat?.Dispose();
                        griMat = new Mat();
                        // Gri resim üzerinde kontrast istendiği için griye dönüştürülür:
                        Cv2.CvtColor(temp, griMat, ColorConversionCodes.BGR2GRAY);
                    }

                    picGiris.Image?.Dispose();
                    picGiris.Image = BitmapConverter.ToBitmap(griMat);

                    HistogramHesapla();
                    HistogramCiz();
                }
            }
        }

        // Histogram Kutusu Üzerinde Mouse Tıklamaları (Sol: X1, Sağ: X2)
        private void PicHistogram_MouseDown(object sender, MouseEventArgs e)
        {
            if (griMat == null || picHistogram.Width <= 0) return;

            // Ölçekleme yaparak tıklanan X pikselini 0-255 renk aralığına çeviriyoruz[cite: 13]:
            int tiklananRenk = (int)((float)e.X / picHistogram.Width * 255f);

            // Sınır kontrolleri (0 ile 255 arasında tut)
            if (tiklananRenk < 0) tiklananRenk = 0;
            if (tiklananRenk > 255) tiklananRenk = 255;

            if (e.Button == MouseButtons.Left)
            {
                x1 = tiklananRenk;
                if (txtX1 != null) txtX1.Text = x1.ToString();
            }
            else if (e.Button == MouseButtons.Right)
            {
                x2 = tiklananRenk;
                if (txtX2 != null) txtX2.Text = x2.ToString();
            }

            // Seçilen noktaları ve kırmızı çizgileri histogram üzerinde göster
            HistogramCiz();
        }

        // Histogram Verisini Çıkarma
        private void HistogramHesapla()
        {
            if (griMat == null) return;
            Array.Clear(histogramVerisi, 0, histogramVerisi.Length);

            for (int y = 0; y < griMat.Height; y++)
            {
                for (int x = 0; x < griMat.Width; x++)
                {
                    byte val = griMat.At<byte>(y, x);
                    histogramVerisi[val]++;
                }
            }
        }

        // Histogramı ve Seçilen X1, X2 Noktalarını Çizme
        private void HistogramCiz()
        {
            if (picHistogram == null || griMat == null) return;

            int w = picHistogram.Width;
            int h = picHistogram.Height;
            if (w <= 0 || h <= 0) return;

            Bitmap bmp = new Bitmap(w, h);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Black);

                // Normalizasyon için en yüksek piksel adedi
                int maxSayi = 1;
                for (int i = 0; i < 256; i++)
                {
                    if (histogramVerisi[i] > maxSayi)
                        maxSayi = histogramVerisi[i];
                }

                // Sarı renk histogram çubukları
                using (Pen penSari = new Pen(Color.Yellow, 1))
                {
                    for (int i = 0; i < 256; i++)
                    {
                        float x = (float)i / 255f * (w - 1);
                        float barBoyu = ((float)histogramVerisi[i] / maxSayi) * (h - 20);
                        g.DrawLine(penSari, x, h - 1, x, h - 1 - barBoyu);
                    }
                }

                // X1 ve X2 noktalarını ve doğrusal gerdirme eğimini gösteren Kırmızı Çizgi[cite: 14]
                using (Pen penKirmizi = new Pen(Color.Red, 2))
                using (Brush brushNokta = new SolidBrush(Color.Red))
                {
                    float pos1 = (float)x1 / 255f * (w - 1);
                    float pos2 = (float)x2 / 255f * (w - 1);

                    // X1 dikey çizgisi ve noktası
                    g.DrawLine(penKirmizi, pos1, 0, pos1, h);
                    g.FillEllipse(brushNokta, pos1 - 4, h - 15, 8, 8);

                    // X2 dikey çizgisi ve noktası
                    g.DrawLine(penKirmizi, pos2, 0, pos2, h);
                    g.FillEllipse(brushNokta, pos2 - 4, h - 15, 8, 8);

                    // İki nokta arasındaki doğrusal gerdirme çizgisi
                    g.DrawLine(penKirmizi, pos1, h - 15, pos2, 10);
                }
            }

            picHistogram.Image?.Dispose();
            picHistogram.Image = bmp;
        }

        // Kontrast Uygulama Butonu
        private void btnKontrast_Click(object sender, EventArgs e)
        {
            if (griMat == null) return;

            // Textbox'lardan değerleri al
            int.TryParse(txtX1.Text, out x1);
            int.TryParse(txtX2.Text, out x2);

            if (x1 >= x2)
            {
                MessageBox.Show("X1 değeri X2 değerinden küçük olmalıdır!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Mat sonucMat = new Mat(griMat.Size(), MatType.CV_8UC1);

            int genislik = griMat.Width;
            int yukseklik = griMat.Height;

            // Kontrast germe formülü: (Piksel - X1) * 255 / (X2 - X1)
            for (int y = 0; y < yukseklik; y++)
            {
                for (int x = 0; x < genislik; x++)
                {
                    byte piksel = griMat.At<byte>(y, x);

                    int yeniRenk;
                    if (piksel <= x1)
                    {
                        yeniRenk = 0;
                    }
                    else if (piksel >= x2)
                    {
                        yeniRenk = 255;
                    }
                    else
                    {
                        yeniRenk = (int)(((float)(piksel - x1) / (x2 - x1)) * 255);
                    }

                    if (yeniRenk < 0) yeniRenk = 0;
                    if (yeniRenk > 255) yeniRenk = 255;

                    sonucMat.Set<byte>(y, x, (byte)yeniRenk);
                }
            }

            picCikis.Image?.Dispose();
            picCikis.Image = BitmapConverter.ToBitmap(sonucMat);
            sonucMat.Dispose();
        }

        // Ödev 3'e Geri Dön Linki
        private void lnkGeri_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta2Odev4_FormClosing;

            Hafta2Odev3 odev3 = new Hafta2Odev3();
            odev3.StartPosition = FormStartPosition.Manual;
            odev3.Location = this.Location;
            odev3.Size = this.Size;
            odev3.FormBorderStyle = this.FormBorderStyle;

            odev3.Show();
            this.Close();
        }

        // Ödev 5'e Geçiş Linki
        private void lnkSonraki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta2Odev4_FormClosing;

            Hafta2Odev5 odev5 = new Hafta2Odev5();
            odev5.StartPosition = FormStartPosition.Manual;
            odev5.Location = this.Location;
            odev5.Size = this.Size;
            odev5.FormBorderStyle = this.FormBorderStyle;

            odev5.Show();
            this.Close();
        }
    }
}