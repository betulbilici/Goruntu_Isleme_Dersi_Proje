using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GoruntuIsleme
{
    public partial class Hafta1Odev2 : Form
    {
        public Hafta1Odev2()
        {
            InitializeComponent();

            // Ekranın ortasında açılsın
            this.StartPosition = FormStartPosition.CenterScreen;

            // Olayları (Events) koddan doğrudan bağlıyoruz:
            this.btnOlustur.Click -= btnOlustur_Click; // Çift tetiklemeyi önle
            this.btnOlustur.Click += btnOlustur_Click;

            // Ödev 1 Link Bağlantısı
            if (this.lnkOdev1 != null)
            {
                this.lnkOdev1.LinkClicked -= lnkOdev1_LinkClicked;
                this.lnkOdev1.LinkClicked += lnkOdev1_LinkClicked;
            }

            // Ödev 3 Link Bağlantısı (Eksik olan kısım bağlandı)
            if (this.lnkOdev3 != null)
            {
                this.lnkOdev3.LinkClicked -= lnkOdev3_LinkClicked;
                this.lnkOdev3.LinkClicked += lnkOdev3_LinkClicked;
            }

            // NumericUpDown ayarları
            if (this.numBoyut != null)
            {
                this.numBoyut.Minimum = 100;
                this.numBoyut.Maximum = 1024;
                this.numBoyut.Value = 512;
            }

            if (this.numYaricap != null)
            {
                this.numYaricap.Minimum = 10;
                this.numYaricap.Maximum = 500;
                this.numYaricap.Value = 100;
            }

            // Buton stili
            this.btnOlustur.FlatStyle = FlatStyle.Flat;
            this.btnOlustur.FlatAppearance.BorderSize = 0;
            this.btnOlustur.BackColor = Color.FromArgb(0, 122, 255);
            this.btnOlustur.ForeColor = Color.White;
            this.btnOlustur.Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold);
            this.btnOlustur.Cursor = Cursors.Hand;
        }

        private void btnOlustur_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Kontrollerin kontrolü
                if (numBoyut == null || picGoruntu == null) return;

                // 2. Değişkenleri arayüzden alma (Soruda istenen 512 ve 100)
                int N = (int)numBoyut.Value;
                if (N < 1) N = 1;

                // numYaricap kontrolü varsa değerini al, yoksa varsayılan 100 yap
                double R = (numYaricap != null) ? (double)numYaricap.Value : 100.0;

                // Çember merkez koordinatları (N/2)
                double merkezX = N / 2.0;
                double merkezY = N / 2.0;

                // 3. OpenCvSharp ile N x N gri seviye (CV_8UC1) matris oluşturma
                using (Mat matris = new Mat(N, N, MatType.CV_8UC1))
                {
                    // Öklid uzaklığı hesabı: sqrt((i - merkezY)^2 + (j - merkezX)^2)
                    for (int i = 0; i < N; i++)
                    {
                        for (int j = 0; j < N; j++)
                        {
                            double mesafe = Math.Sqrt(Math.Pow(i - merkezY, 2) + Math.Pow(j - merkezX, 2));

                            if (mesafe < R)
                            {
                                matris.Set<byte>(i, j, 255); // Çember içi Beyaz (1)
                            }
                            else
                            {
                                matris.Set<byte>(i, j, 0);   // Dışı Siyah (0)
                            }
                        }
                    }

                    // 4. Matrisi Bitmap'e çevirip PictureBox'a tam kaplayacak şekilde aktar
                    picGoruntu.Image?.Dispose();
                    picGoruntu.Image = BitmapConverter.ToBitmap(matris);
                    picGoruntu.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Çizim sırasında hata oluştu: " + ex.Message);
            }
        }

        // Ödev 1'e Geri Dön Linki:
        private void lnkOdev1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form odev1 = Application.OpenForms["Hafta1Odev1"];
            if (odev1 != null)
            {
                odev1.Show();
            }
            else
            {
                Hafta1Odev1 yeniOdev1 = new Hafta1Odev1();
                yeniOdev1.StartPosition = FormStartPosition.CenterScreen;
                yeniOdev1.Show();
            }

            this.Dispose();
        }

        // Ödev 3'e Git Linki:
        private void lnkOdev3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Hafta1Odev3 odev3 = new Hafta1Odev3();
            odev3.StartPosition = FormStartPosition.CenterScreen;
            odev3.Show();

            this.Hide();
        }
    }
}