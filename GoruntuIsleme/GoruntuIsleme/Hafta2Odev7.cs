using System;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace GoruntuIsleme
{
    public partial class Hafta2Odev7 : Form
    {
        private Mat orijinalMat = null;
        private int[] histR = new int[256];
        private int[] histG = new int[256];
        private int[] histB = new int[256];

        public Hafta2Odev7()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1250, 750);
            this.StartPosition = FormStartPosition.CenterScreen;

            // X butonu ile Form1'e güvenli dönüş
            this.FormClosing += Hafta2Odev7_FormClosing;

            // Olay bağlantıları
            if (btnResimSec != null) btnResimSec.Click += btnResimSec_Click;
            if (btnAnalizEt != null) btnAnalizEt.Click += btnAnalizEt_Click;
            if (btnYorumYap != null) btnYorumYap.Click += btnYorumYap_Click;
            if (lnkGeri != null) lnkGeri.LinkClicked += lnkGeri_LinkClicked;
            if (lnkSonraki != null) lnkSonraki.LinkClicked += lnkSonraki_LinkClicked_1;
        }

        private void Hafta2Odev7_FormClosing(object sender, FormClosingEventArgs e)
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

        // 1. AŞAMA: Sadece Resim Seçer ve Ekranı Temizler
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

                    picResim.Image?.Dispose();
                    picResim.Image = BitmapConverter.ToBitmap(orijinalMat);

                    // Yeni resim seçildiğinde önceki histogramları ve açıklamayı temizle
                    picHistR.Image?.Dispose(); picHistR.Image = null;
                    picHistG.Image?.Dispose(); picHistG.Image = null;
                    picHistB.Image?.Dispose(); picHistB.Image = null;

                    if (txtYorum != null)
                    {
                        txtYorum.Text = "Resim yüklendi. Kanalların histogramlarını çizdirmek için 'Kanalları Analiz Et' butonuna tıklayınız.";
                    }
                }
            }
        }

        // 2. AŞAMA: Butona Basılınca Histogramları Hesaplar ve Çizer
        private void btnAnalizEt_Click(object sender, EventArgs e)
        {
            if (orijinalMat == null)
            {
                MessageBox.Show("Lütfen önce bir resim seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            HistogramlariHesaplaVeCiz();

            if (txtYorum != null)
            {
                txtYorum.Text = "R, G, B kanallarının histogramları başarıyla çizildi. Bilimsel değerlendirmeyi görmek için 'Yorum ve Kullanım Alanları' butonuna tıklayabilirsiniz.";
            }
        }

        // 3. AŞAMA: Akademik Raporu Metin Kutusuna Doldurur
        private void btnYorumYap_Click(object sender, EventArgs e)
        {
            if (orijinalMat == null)
            {
                MessageBox.Show("Lütfen önce bir resim seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Histogramlar henüz çizilmediyse önce çizdirilir
            HistogramlariHesaplaVeCiz();
            AkademikRaporOlustur();
        }

        // R, G, B kanallarının histogramlarını çıkarma ve çizdirme
        private void HistogramlariHesaplaVeCiz()
        {
            if (orijinalMat == null) return;

            Array.Clear(histR, 0, 256);
            Array.Clear(histG, 0, 256);
            Array.Clear(histB, 0, 256);

            for (int y = 0; y < orijinalMat.Height; y++)
            {
                for (int x = 0; x < orijinalMat.Width; x++)
                {
                    Vec3b c = orijinalMat.At<Vec3b>(y, x);
                    histB[c.Item0]++;
                    histG[c.Item1]++;
                    histR[c.Item2]++;
                }
            }

            TekHistogramCiz(picHistR, histR, Color.Red, "Kırmızı (Red) Kanalı");
            TekHistogramCiz(picHistG, histG, Color.Lime, "Yeşil (Green) Kanalı");
            TekHistogramCiz(picHistB, histB, Color.DeepSkyBlue, "Mavi (Blue) Kanalı");
        }

        // Tek bir kanalın histogram grafiğini çizme
        private void TekHistogramCiz(PictureBox pb, int[] hist, Color renk, string kanalAdi)
        {
            if (pb == null || pb.Width <= 0 || pb.Height <= 0) return;

            int w = pb.Width;
            int h = pb.Height;
            Bitmap bmp = new Bitmap(w, h);

            int max = 1;
            for (int i = 0; i < 256; i++)
            {
                if (hist[i] > max) max = hist[i];
            }

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Black);

                // Dikey histogram sütunları
                using (Pen p = new Pen(renk, 1))
                {
                    for (int i = 0; i < 256; i++)
                    {
                        float x = (float)i / 255f * (w - 1);
                        float barH = ((float)hist[i] / max) * (h - 16);
                        g.DrawLine(p, x, h - 1, x, h - 1 - barH);
                    }
                }

                // 50'lik referans dikey kılavuz çizgileri
                using (Pen pRef = new Pen(Color.FromArgb(60, 60, 60), 1))
                {
                    for (int i = 50; i <= 250; i += 50)
                    {
                        float xRef = (float)i / 255f * (w - 1);
                        g.DrawLine(pRef, xRef, 0, xRef, h);
                    }
                }

                // Kanal İsmi
                using (Font f = new Font("Arial", 9, FontStyle.Bold))
                using (Brush b = new SolidBrush(Color.White))
                {
                    g.DrawString(kanalAdi, f, b, 8, 6);
                }
            }

            pb.Image?.Dispose();
            pb.Image = bmp;
        }

        // Akademik değerlendirme ve kullanım alanlarını oluşturan metot
        private void AkademikRaporOlustur()
        {
            if (orijinalMat == null || txtYorum == null) return;

            long toplamR = 0, toplamG = 0, toplamB = 0;
            long pikselSayisi = orijinalMat.Width * orijinalMat.Height;

            for (int i = 0; i < 256; i++)
            {
                toplamR += (long)histR[i] * i;
                toplamG += (long)histG[i] * i;
                toplamB += (long)histB[i] * i;
            }

            double ortR = (double)toplamR / pikselSayisi;
            double ortG = (double)toplamG / pikselSayisi;
            double ortB = (double)toplamB / pikselSayisi;

            string baskinKanal = "Dengeli";
            if (ortR > ortG && ortR > ortB) baskinKanal = "Kırmızı (R)";
            else if (ortG > ortR && ortG > ortB) baskinKanal = "Yeşil (G)";
            else if (ortB > ortR && ortB > ortG) baskinKanal = "Mavi (B)";

            txtYorum.Text = "RENK KANALLARI HİSTOGRAM ANALİZİ VE AKADEMİK DEĞERLENDİRME:\r\n\r\n" +
                $"1. MEVCUT GÖRSELİN İSTATİSTİKSEL DURUMU:\r\n" +
                $"• Ortalama R Değeri: {ortR:F1} | Ortalama G Değeri: {ortG:F1} | Ortalama B Değeri: {ortB:F1}\r\n" +
                $"• Baskın Renk Kanalı: {baskinKanal}\r\n\r\n" +
                "2. BU BİLGİLER GÖRÜNTÜ İŞLEMEDE NERELERDE KULLANILIR?\r\n" +
                "a) Beyaz Dengesi ve Renk Düzeltme (White Balance / Color Cast):\r\n" +
                "Kanal histogramlarının tepe noktaları karşılaştırılarak ortam ışığından kaynaklanan renk sapmaları (örneğin aşırı sarı, mavi veya yeşil ton) tespit edilir ve kanallar dengelenerek doğal renkler elde edilir.\r\n\r\n" +
                "b) Renk Tabanlı Nesne Tespiti ve Bölütleme (Color Segmentation):\r\n" +
                "Hedef bir nesnenin (örneğin cilt rengi, bitki örtüsü, trafik işaretleri) kendine özgü kanal dağılımı eşiklenerek arka plandan kolayca ayrıştırılması sağlanır.\r\n\r\n" +
                "c) Medikal ve Biyomedikal Görüntüleme:\r\n" +
                "Hücre boyamaları, doku analizleri veya retina taramalarında spesifik kanalların (örneğin kan damarlarını en iyi gösteren yeşil kanal) histogramı incelenerek hastalık teşhisi yapılır.\r\n\r\n" +
                "d) Tarımsal Görüntü İşleme ve Uydu Analizleri:\r\n" +
                "Bitkilerin klorofil oranını ölçen NDVI benzeri indekslerde kırmızı ve yeşil/kızılötesi kanalların histogram yığılmalarından bitki sağlığı haritaları çıkarılır.\r\n\r\n" +
                "e) Histogram Eşitleme ve Kontrast İyileştirme:\r\n" +
                "Her kanala bağımsız ya da HSV/Lab uzayında parlaklık kanalına histogram eşitleme uygulanarak karanlık veya aşırı aydınlık görseller optimize edilir.";
        }

        // Ödev 6'ya Geri Dön Linki
        private void lnkGeri_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta2Odev7_FormClosing;

            Hafta2Odev6 odev6 = new Hafta2Odev6();
            odev6.StartPosition = FormStartPosition.Manual;
            odev6.Location = this.Location;
            odev6.Size = this.Size;
            odev6.FormBorderStyle = this.FormBorderStyle;

            odev6.Show();
            this.Close();
        }

        // Ödev 8'e Git Linki


        private void lnkSonraki_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta2Odev7_FormClosing;

            Hafta2Odev8 odev8 = new Hafta2Odev8();
            odev8.StartPosition = FormStartPosition.Manual;
            odev8.Location = this.Location;
            odev8.Size = this.Size;
            odev8.FormBorderStyle = this.FormBorderStyle;
            odev8.Show();
            this.Close();
        }
    }
}