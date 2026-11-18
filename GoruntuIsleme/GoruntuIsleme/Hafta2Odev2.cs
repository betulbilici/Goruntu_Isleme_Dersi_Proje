using System;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace GoruntuIsleme
{
    public partial class Hafta2Odev2 : Form
    {
        private Mat orijinalMat = null;
        private int[] histogramVerisi = new int[256];

        public Hafta2Odev2()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            this.StartPosition = FormStartPosition.CenterScreen;

            // X butonuna basınca Form1'e güvenli dönüş
            this.FormClosing += Hafta2Odev2_FormClosing;

            // ComboBox başlangıç
            if (cmbMod != null)
            {
                if (cmbMod.Items.Count == 0)
                {
                    cmbMod.Items.Add("Gri-Ton Eşikleme");
                    cmbMod.Items.Add("Renkli Eşikleme (RGB)");
                }
                cmbMod.SelectedIndex = 0;
                cmbMod.SelectedIndexChanged += (s, e) => EsiklemeUygula();
            }

            // TrackBar ayarları
            if (trackMin != null && trackMax != null)
            {
                trackMin.Minimum = 0; trackMin.Maximum = 255; trackMin.Value = 50;
                trackMax.Minimum = 0; trackMax.Maximum = 255; trackMax.Value = 200;

                trackMin.Scroll += TrackBar_Scroll;
                trackMax.Scroll += TrackBar_Scroll;
            }

            // Olay bağlantıları
            if (btnResimSec != null) btnResimSec.Click += btnResimSec_Click;
            if (lnkGeri != null) lnkGeri.LinkClicked += lnkGeri_LinkClicked;
            if (lnkSonraki != null) lnkSonraki.LinkClicked += lnkSonraki_LinkClicked;
        }

        // X butonuna basıldığında Form1'e dönüş
        private void Hafta2Odev2_FormClosing(object sender, FormClosingEventArgs e)
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
                    orijinalMat?.Dispose();
                    orijinalMat = Cv2.ImRead(ofd.FileName, ImreadModes.Color);

                    if (orijinalMat.Empty())
                    {
                        MessageBox.Show("Resim yüklenemedi!");
                        return;
                    }

                    picGiris.Image?.Dispose();
                    picGiris.Image = BitmapConverter.ToBitmap(orijinalMat);

                    HistogramHesapla();
                    HistogramCiz();
                    EsiklemeUygula();
                }
            }
        }

        // Sürgüler hareket ettikçe sınırları kontrol et ve anlık işle
        private void TrackBar_Scroll(object sender, EventArgs e)
        {
            // Min, Max'ı geçemez
            if (trackMin.Value > trackMax.Value)
            {
                if (sender == trackMin) trackMax.Value = trackMin.Value;
                else trackMin.Value = trackMax.Value;
            }

            if (lblAralik != null)
                lblAralik.Text = $"Aralık: [{trackMin.Value} - {trackMax.Value}]";

            HistogramCiz();    // Sürgü çizgilerini histogram üzerinde güncelle
            EsiklemeUygula();  // Görüntüyü anlık filtrele
        }

        // 1. Histogram Verilerini Hesaplama
        private void HistogramHesapla()
        {
            if (orijinalMat == null) return;
            Array.Clear(histogramVerisi, 0, histogramVerisi.Length);

            using (Mat gri = new Mat())
            {
                Cv2.CvtColor(orijinalMat, gri, ColorConversionCodes.BGR2GRAY);
                for (int y = 0; y < gri.Height; y++)
                {
                    for (int x = 0; x < gri.Width; x++)
                    {
                        byte val = gri.At<byte>(y, x);
                        histogramVerisi[val]++;
                    }
                }
            }
        }

        // 2. Histogram Çizimi ve Kırmızı Eşik Sınır Çizgileri
        private void HistogramCiz()
        {
            if (picHistogram == null || orijinalMat == null) return;

            int w = picHistogram.Width;
            int h = picHistogram.Height;
            if (w <= 0 || h <= 0) return;

            Bitmap bmp = new Bitmap(w, h);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Black);

                // En yüksek piksel sayısını bul (Normalizasyon için)
                int maxSayi = 1;
                for (int i = 0; i < 256; i++)
                {
                    if (histogramVerisi[i] > maxSayi)
                        maxSayi = histogramVerisi[i];
                }

                // Sarı renk tonlarıyla histogram çubuklarını çiz
                using (Pen penSari = new Pen(Color.Yellow, 1))
                {
                    for (int i = 0; i < 256; i++)
                    {
                        float x = (float)i / 255f * (w - 1);
                        float barBoyu = ((float)histogramVerisi[i] / maxSayi) * (h - 20);
                        g.DrawLine(penSari, x, h - 1, x, h - 1 - barBoyu);
                    }
                }

                // Sürgülerin eşik sınırlarını gösteren Kırmızı Dikey Çizgiler
                using (Pen penKirmizi = new Pen(Color.Red, 2))
                {
                    float xMin = (float)trackMin.Value / 255f * (w - 1);
                    float xMax = (float)trackMax.Value / 255f * (w - 1);

                    g.DrawLine(penKirmizi, xMin, 0, xMin, h);
                    g.DrawLine(penKirmizi, xMax, 0, xMax, h);
                }
            }

            picHistogram.Image?.Dispose();
            picHistogram.Image = bmp;
        }

        // 3. Eşikleme İşlemi (Gri veya Renkli)
        private void EsiklemeUygula()
        {
            if (orijinalMat == null) return;

            int minT = trackMin.Value;
            int maxT = trackMax.Value;
            bool griMod = (cmbMod != null && cmbMod.SelectedIndex == 0);

            if (griMod)
            {
                using (Mat gri = new Mat())
                {
                    Cv2.CvtColor(orijinalMat, gri, ColorConversionCodes.BGR2GRAY);
                    using (Mat sonucGri = new Mat(gri.Size(), MatType.CV_8UC1))
                    {
                        for (int y = 0; y < gri.Height; y++)
                        {
                            for (int x = 0; x < gri.Width; x++)
                            {
                                byte val = gri.At<byte>(y, x);
                                // Sürgü değerleri arasındaysa değeri koru, dışındaysa siyah (0) yap
                                if (val >= minT && val <= maxT)
                                    sonucGri.Set<byte>(y, x, val);
                                else
                                    sonucGri.Set<byte>(y, x, 0);
                            }
                        }

                        picCikis.Image?.Dispose();
                        picCikis.Image = BitmapConverter.ToBitmap(sonucGri);
                    }
                }
            }
            else
            {
                // Renkli (RGB) mod: Her kanal bağımsız incelenir
                using (Mat sonucRenkli = new Mat(orijinalMat.Size(), MatType.CV_8UC3))
                {
                    for (int y = 0; y < orijinalMat.Height; y++)
                    {
                        for (int x = 0; x < orijinalMat.Width; x++)
                        {
                            Vec3b bgr = orijinalMat.At<Vec3b>(y, x);

                            byte B = (bgr.Item0 >= minT && bgr.Item0 <= maxT) ? bgr.Item0 : (byte)0;
                            byte G = (bgr.Item1 >= minT && bgr.Item1 <= maxT) ? bgr.Item1 : (byte)0;
                            byte R = (bgr.Item2 >= minT && bgr.Item2 <= maxT) ? bgr.Item2 : (byte)0;

                            sonucRenkli.Set(y, x, new Vec3b(B, G, R));
                        }
                    }

                    picCikis.Image?.Dispose();
                    picCikis.Image = BitmapConverter.ToBitmap(sonucRenkli);
                }
            }
        }

        // Ödev 1'e Geri Dön Linki
        private void lnkGeri_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta2Odev2_FormClosing;

            Hafta2Odev1 odev1 = new Hafta2Odev1();
            odev1.StartPosition = FormStartPosition.Manual;
            odev1.Location = this.Location;
            odev1.Size = this.Size;
            odev1.Show();

            this.Close();
        }

        // Sonraki Ödeve Geçiş (Ödev 3 varsa)
        private void lnkSonraki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta2Odev2_FormClosing;

            Hafta2Odev3 odev3 = new Hafta2Odev3();
            odev3.StartPosition = FormStartPosition.Manual;
            odev3.Location = this.Location;
            odev3.Size = this.Size;
            odev3.FormBorderStyle = this.FormBorderStyle;

            odev3.Show();
            this.Close();
        }
    }
}