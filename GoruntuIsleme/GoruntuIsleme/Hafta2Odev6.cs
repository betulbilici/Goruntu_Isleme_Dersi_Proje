using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace GoruntuIsleme
{
    public partial class Hafta2Odev6 : Form
    {
        private Mat orijinalMat = null;
        private List<System.Drawing.Point> poligonNoktalari = new List<System.Drawing.Point>();
        private bool poligonKapandi = false;
        private int[] roiHistogram = new int[256];

        public Hafta2Odev6()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1250, 750);
            this.StartPosition = FormStartPosition.CenterScreen;

            // X butonu kapatma olayını bağlıyoruz:
            this.FormClosing += Hafta2Odev6_FormClosing;

            // Buton ve Link olayları
            if (btnResimSec != null) btnResimSec.Click += btnResimSec_Click;
            if (btnTemizle != null) btnTemizle.Click += btnTemizle_Click;
            if (btnKapat != null) btnKapat.Click += btnKapat_Click;
            if (btncAciklama != null) btncAciklama.Click += btnAciklama_Click;
            if (lnkGeri != null) lnkGeri.LinkClicked += lnkGeri_LinkClicked;
            if (lnkSonraki != null) lnkSonraki.LinkClicked += lnkSonraki_LinkClicked;

            // Sürgüler hareket ettiğinde canlı güncelleme
            if (trackParlaklik != null) trackParlaklik.Scroll += (s, e) => IslemleriUygula();
            if (trackKontrast != null) trackKontrast.Scroll += (s, e) => IslemleriUygula();
            if (trackR != null) trackR.Scroll += (s, e) => IslemleriUygula();
            if (trackG != null) trackG.Scroll += (s, e) => IslemleriUygula();
            if (trackB != null) trackB.Scroll += (s, e) => IslemleriUygula();

            // Fare ile tıklama (Sol tık: Nokta ekle, Sağ tık: Kapat)
            if (picResim != null)
            {
                picResim.MouseDown += PicResim_MouseDown;
            }
        }

        // X butonuna basıldığında Form1'e dönen metot:
        private void Hafta2Odev6_FormClosing(object sender, FormClosingEventArgs e)
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

        // Resim Seçme Butonu
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

                    btnTemizle_Click(null, null);
                }
            }
        }

        // Fare ile poligon noktaları belirleme: Sol Tık Nokta Ekle, Sağ Tık Poligonu Kapat
        private void PicResim_MouseDown(object sender, MouseEventArgs e)
        {
            if (orijinalMat == null || poligonKapandi || picResim.Width <= 0 || picResim.Height <= 0) return;

            // Ekrandaki tıklama koordinatını resmin gerçek boyutuna ölçekle
            int gercekX = (int)((float)e.X / picResim.Width * orijinalMat.Width);
            int gercekY = (int)((float)e.Y / picResim.Height * orijinalMat.Height);

            if (e.Button == MouseButtons.Left)
            {
                poligonNoktalari.Add(new System.Drawing.Point(gercekX, gercekY));
                CizimiGuncelle();
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (poligonNoktalari.Count >= 3)
                {
                    poligonKapandi = true;
                    CizimiGuncelle();
                }
                else
                {
                    MessageBox.Show("Poligonu kapatmak için en az 3 nokta işaretlemelisiniz!");
                }
            }
        }

        // Poligonu Kapat Butonu
        private void btnKapat_Click(object sender, EventArgs e)
        {
            if (poligonNoktalari.Count >= 3)
            {
                poligonKapandi = true;
                CizimiGuncelle();
            }
            else
            {
                MessageBox.Show("Poligon oluşturmak için resim üzerinde en az 3 noktaya tıklamalısınız.");
            }
        }

        // Çizimi Sıfırla / Temizle Butonu
        private void btnTemizle_Click(object sender, EventArgs e)
        {
            poligonNoktalari.Clear();
            poligonKapandi = false;

            if (trackParlaklik != null) trackParlaklik.Value = 0;
            if (trackKontrast != null) trackKontrast.Value = 0;
            if (trackR != null) trackR.Value = 0;
            if (trackG != null) trackG.Value = 0;
            if (trackB != null) trackB.Value = 0;

            if (orijinalMat != null)
            {
                picResim.Image?.Dispose();
                picResim.Image = BitmapConverter.ToBitmap(orijinalMat);
            }

            picHistogram.Image?.Dispose();
            picHistogram.Image = null;
        }

        // (b Şıkkı): Analitik Geometri - Işın Gönderme (Ray-Casting) Algoritması
        private bool NoktaPoligonunIcindeMi(int x, int y, List<System.Drawing.Point> poligon)
        {
            bool iceride = false;
            int n = poligon.Count;

            for (int i = 0, j = n - 1; i < n; j = i++)
            {
                if (((poligon[i].Y > y) != (poligon[j].Y > y)) &&
                    (x < (poligon[j].X - poligon[i].X) * (y - poligon[i].Y) / (poligon[j].Y - poligon[i].Y) + poligon[i].X))
                {
                    iceride = !iceride;
                }
            }
            return iceride;
        }

        // Seçili Bölge İçine Filtreleri Uygulama
        private void IslemleriUygula()
        {
            if (orijinalMat == null || !poligonKapandi || poligonNoktalari.Count < 3) return;

            Mat islenmisMat = orijinalMat.Clone();
            Array.Clear(roiHistogram, 0, 256);

            int pVal = trackParlaklik != null ? trackParlaklik.Value : 0;
            double cVal = trackKontrast != null ? trackKontrast.Value : 0;
            int rVal = trackR != null ? trackR.Value : 0;
            int gVal = trackG != null ? trackG.Value : 0;
            int bVal = trackB != null ? trackB.Value : 0;

            // Slayttaki F Kontrast Faktörü Formülü
            double fKontrast = 1.0;
            if (cVal != 0)
            {
                fKontrast = (259.0 * (cVal + 255.0)) / (255.0 * (259.0 - cVal));
            }

            // Performans için Bounding Box (Sınırlayıcı Dikdörtgen) hesaplama
            int minX = int.MaxValue, maxX = int.MinValue, minY = int.MaxValue, maxY = int.MinValue;
            foreach (var pt in poligonNoktalari)
            {
                if (pt.X < minX) minX = pt.X;
                if (pt.X > maxX) maxX = pt.X;
                if (pt.Y < minY) minY = pt.Y;
                if (pt.Y > maxY) maxY = pt.Y;
            }

            minX = Math.Max(0, minX);
            maxX = Math.Min(orijinalMat.Width - 1, maxX);
            minY = Math.Max(0, minY);
            maxY = Math.Min(orijinalMat.Height - 1, maxY);

            // Sadece sınırlayıcı kutu taranır
            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    if (NoktaPoligonunIcindeMi(x, y, poligonNoktalari))
                    {
                        Vec3b bgr = orijinalMat.At<Vec3b>(y, x);

                        // 1. Parlaklık (i) ve RGB Renk Değişimi (iii)
                        double b = bgr.Item0 + pVal + bVal;
                        double g = bgr.Item1 + pVal + gVal;
                        double r = bgr.Item2 + pVal + rVal;

                        // 2. Hazır Formülle Kontrast (ii)
                        if (cVal != 0)
                        {
                            b = (fKontrast * (b - 128.0)) + 128.0;
                            g = (fKontrast * (g - 128.0)) + 128.0;
                            r = (fKontrast * (r - 128.0)) + 128.0;
                        }

                        // Taşma kırpması (Clipping)
                        b = Math.Max(0, Math.Min(255, b));
                        g = Math.Max(0, Math.Min(255, g));
                        r = Math.Max(0, Math.Min(255, r));

                        islenmisMat.Set(y, x, new Vec3b((byte)b, (byte)g, (byte)r));

                        // Seçili bölgenin gri tonlama histogramını topla (ii)
                        int griDeger = (int)(r * 0.299 + g * 0.587 + b * 0.114);
                        if (griDeger >= 0 && griDeger <= 255) roiHistogram[griDeger]++;
                    }
                }
            }

            // Resmi Bitmap'e dönüştür ve üzerine kesikli çerçeveyi çiz
            Bitmap bmp = BitmapConverter.ToBitmap(islenmisMat);
            islenmisMat.Dispose();

            using (Graphics gr = Graphics.FromImage(bmp))
            {
                using (Pen kesikliKalem = new Pen(Color.Yellow, 2))
                {
                    kesikliKalem.DashStyle = DashStyle.Dash; // Slayttaki kesikli çizgi
                    gr.DrawPolygon(kesikliKalem, poligonNoktalari.ToArray());
                }
            }

            picResim.Image?.Dispose();
            picResim.Image = bmp;

            // Seçili alanın güncel histogramını çizdir (ii)
            RoiHistogramCiz();
        }

        // Tıklanan noktaları ve kesikli çizgileri güncelle
        private void CizimiGuncelle()
        {
            if (orijinalMat == null) return;

            Bitmap bmp = BitmapConverter.ToBitmap(orijinalMat);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                using (Pen kesikliKalem = new Pen(Color.Yellow, 2))
                using (Brush noktaFircasi = new SolidBrush(Color.Red))
                {
                    kesikliKalem.DashStyle = DashStyle.Dash;

                    for (int i = 0; i < poligonNoktalari.Count; i++)
                    {
                        g.FillEllipse(noktaFircasi, poligonNoktalari[i].X - 4, poligonNoktalari[i].Y - 4, 8, 8);

                        if (i > 0)
                        {
                            g.DrawLine(kesikliKalem, poligonNoktalari[i - 1], poligonNoktalari[i]);
                        }
                    }

                    if (poligonKapandi && poligonNoktalari.Count >= 3)
                    {
                        g.DrawLine(kesikliKalem, poligonNoktalari[poligonNoktalari.Count - 1], poligonNoktalari[0]);
                    }
                }
            }

            picResim.Image?.Dispose();
            picResim.Image = bmp;

            if (poligonKapandi)
            {
                IslemleriUygula();
            }
        }

        // Seçili Bölgenin Histogram Grafiğini Çizdir
        private void RoiHistogramCiz()
        {
            if (picHistogram == null || picHistogram.Width <= 0 || picHistogram.Height <= 0) return;

            int w = picHistogram.Width;
            int h = picHistogram.Height;
            Bitmap bmp = new Bitmap(w, h);

            int max = 1;
            for (int i = 0; i < 256; i++)
            {
                if (roiHistogram[i] > max) max = roiHistogram[i];
            }

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Black);

                using (Pen p = new Pen(Color.Cyan, 1))
                {
                    for (int i = 0; i < 256; i++)
                    {
                        float x = (float)i / 255f * (w - 1);
                        float barH = ((float)roiHistogram[i] / max) * (h - 10);
                        g.DrawLine(p, x, h - 1, x, h - 1 - barH);
                    }
                }
            }

            picHistogram.Image?.Dispose();
            picHistogram.Image = bmp;
        }

        // b Şıkkı Butonuna Basıldığında Açıklamayı Yazdır
        private void btnAciklama_Click(object sender, EventArgs e)
        {
            if (txtAciklama != null)
            {
                txtAciklama.Text = "B ŞIKKI ANALİTİK GEOMETRİ ARAŞTIRMASI (RAY-CASTING):\r\n\r\n" +
                    "Kapalı bir poligonun içinde bir noktanın yer alıp almadığını tespit etmek için Jordan Eğri Teoremi'ne dayanan 'Işın Gönderme' (Ray-Casting) algoritması kullanılır.\r\n\r\n" +
                    "Çalışma Mantığı:\r\n" +
                    "1. İncelenen (x, y) pikselinden yatay doğrultuda (+X ekseninde) sonsuza bir ışın fırlatılır.\r\n" +
                    "2. Bu ışının poligonu oluşturan kenar doğrularını kestiği noktaların sayısı hesaplanır.\r\n" +
                    "3. Kesişim sayısı TEK ise nokta poligonun İÇİNDEDİR; ÇİFT veya SIFIR ise DIŞINDADIR.\r\n\r\n" +
                    "Kod içerisinde bu mantık kullanılarak sadece kapalı bölgenin pikselleri filtrelenip renklendirilmektedir.";
            }
        }

        // Ödev 5'e Geri Dön Linki
        private void lnkGeri_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta2Odev6_FormClosing;

            Hafta2Odev5 odev5 = new Hafta2Odev5();
            odev5.StartPosition = FormStartPosition.Manual;
            odev5.Location = this.Location;
            odev5.Size = this.Size;
            odev5.FormBorderStyle = this.FormBorderStyle;

            odev5.Show();
            this.Close();
        }

        // Ödev 7'ye Git Linki
        private void lnkSonraki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta2Odev6_FormClosing;

            Hafta2Odev7 odev7 = new Hafta2Odev7();
            odev7.StartPosition = FormStartPosition.Manual;
            odev7.Location = this.Location;
            odev7.Size = this.Size;
            odev7.FormBorderStyle = this.FormBorderStyle;
            odev7.Show();
            this.Close();
        }
    }
}