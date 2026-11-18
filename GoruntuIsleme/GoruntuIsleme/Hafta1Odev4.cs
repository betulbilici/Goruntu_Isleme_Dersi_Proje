using System;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace GoruntuIsleme
{
    public partial class Hafta1Odev4 : Form
    {
        private Mat griMat = null;     // Griye dönüştürülmüş görsel
        private Mat mozaikMat = null;  // Ortalama alınarak küçültülmüş yeni görsel
        private int blokBoyutu = 10;   // Seçilen adım aralığı

        public Hafta1Odev4()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            this.StartPosition = FormStartPosition.CenterScreen;

            // X butonu kapatma olayını bağlıyoruz:
            this.FormClosing += Hafta1Odev4_FormClosing;

            if (cmbMozaikBoyut != null && cmbMozaikBoyut.Items.Count > 0)
                cmbMozaikBoyut.SelectedIndex = 2; // Varsayılan: 10

            // Olayları bağlama
            if (btnResimSec != null) btnResimSec.Click += btnResimSec_Click;
            if (btnMozaikOlustur != null) btnMozaikOlustur.Click += btnMozaikOlustur_Click;
            if (lnkGeri != null) lnkGeri.LinkClicked += lnkGeri_LinkClicked;
            if (lnkOdev5 != null) lnkOdev5.LinkClicked += lnkOdev5_LinkClicked;
            if (numI != null) numI.ValueChanged += KoordinatDegeriGuncelle;
            if (numJ != null) numJ.ValueChanged += KoordinatDegeriGuncelle;
        }

        // X butonuna basıldığında Form1'e dönen metot:
        private void Hafta1Odev4_FormClosing(object sender, FormClosingEventArgs e)
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

        // 1. Resim Seçme ve Griye Çevirme
        private void btnResimSec_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    using (Mat yuklenen = Cv2.ImRead(ofd.FileName, ImreadModes.Color))
                    {
                        if (yuklenen.Empty()) return;

                        griMat?.Dispose();
                        griMat = new Mat();

                        // Renkli resmi gri renge dönüştürme (BGR -> GRAY)
                        Cv2.CvtColor(yuklenen, griMat, ColorConversionCodes.BGR2GRAY);

                        picGri.Image?.Dispose();
                        picGri.Image = BitmapConverter.ToBitmap(griMat);
                    }
                }
            }
        }

        // 2. Mozaik Alanların Ortalama Rengini Bularak Yeni Küçük Resim Oluşturma
        private void btnMozaikOlustur_Click(object sender, EventArgs e)
        {
            if (griMat == null)
            {
                MessageBox.Show("Lütfen önce bir resim seçin!");
                return;
            }

            // Seçilen mozaik boyutu (2, 5, 10, 25)
            blokBoyutu = 10;
            if (cmbMozaikBoyut != null && cmbMozaikBoyut.SelectedItem != null)
            {
                int.TryParse(cmbMozaikBoyut.SelectedItem.ToString(), out blokBoyutu);
                if (blokBoyutu <= 0) blokBoyutu = 1;
            }

            int yeniGenislik = griMat.Width / blokBoyutu;
            int yeniYukseklik = griMat.Height / blokBoyutu;

            if (yeniGenislik <= 0 || yeniYukseklik <= 0) return;

            mozaikMat?.Dispose();
            mozaikMat = new Mat(yeniYukseklik, yeniGenislik, MatType.CV_8UC1);

            // Her mozaik bloğunu tara ve ortalamasını ata
            for (int i = 0; i < yeniYukseklik; i++)
            {
                for (int j = 0; j < yeniGenislik; j++)
                {
                    int baslangicY = i * blokBoyutu;
                    int baslangicX = j * blokBoyutu;

                    long toplamGri = 0;
                    int sayac = 0;

                    for (int y = baslangicY; y < baslangicY + blokBoyutu && y < griMat.Height; y++)
                    {
                        for (int x = baslangicX; x < baslangicX + blokBoyutu && x < griMat.Width; x++)
                        {
                            toplamGri += griMat.At<byte>(y, x);
                            sayac++;
                        }
                    }

                    byte ortalamaGri = (byte)(toplamGri / sayac);
                    mozaikMat.Set<byte>(i, j, ortalamaGri);
                }
            }

            // Koordinat sınırlarını yeni küçük resmin sınırlarına çek
            numI.Maximum = Math.Max(0, yeniYukseklik - 1);
            numJ.Maximum = Math.Max(0, yeniGenislik - 1);

            // Görseli güncelle ve seçili bloğu işaretle
            KoordinatDegeriGuncelle(null, null);
        }

        // Slayttaki (i, j) koordinatındaki piksel değerini ekrana yazdırma ve bloğu işaretleme
        private void KoordinatDegeriGuncelle(object sender, EventArgs e)
        {
            if (mozaikMat == null || griMat == null) return;

            int i = (int)numI.Value;
            int j = (int)numJ.Value;

            if (i < mozaikMat.Height && j < mozaikMat.Width)
            {
                byte deger = mozaikMat.At<byte>(i, j);
                lblPikselDegeri.Text = $"Piksel gri seviye değeri f_gray({i}, {j}) = {deger}";

                // Mozaik resmi orijinal boyutuna yakınlaştırıp seçilen bloğu kırmızı çerçeveyle göster
                using (Mat gosterimMat = new Mat())
                {
                    Cv2.Resize(mozaikMat, gosterimMat, new OpenCvSharp.Size(griMat.Width, griMat.Height), 0, 0, InterpolationFlags.Nearest);

                    using (Mat renkliGosterim = new Mat())
                    {
                        Cv2.CvtColor(gosterimMat, renkliGosterim, ColorConversionCodes.GRAY2BGR);

                        Rect secilenAlan = new Rect(j * blokBoyutu, i * blokBoyutu, blokBoyutu, blokBoyutu);
                        Cv2.Rectangle(renkliGosterim, secilenAlan, new Scalar(0, 0, 255), 2); // Kırmızı kutu

                        picMozaik.Image?.Dispose();
                        picMozaik.Image = BitmapConverter.ToBitmap(renkliGosterim);
                    }
                }
            }
        }

        // Ödev 3'e Geri Dön Linki
        private void lnkGeri_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta1Odev4_FormClosing;

            Form odev3 = Application.OpenForms["Hafta1Odev3"];
            if (odev3 != null)
            {
                odev3.Show();
            }
            else
            {
                Hafta1Odev3 yeniOdev3 = new Hafta1Odev3();
                yeniOdev3.StartPosition = FormStartPosition.Manual;
                yeniOdev3.Location = this.Location;
                yeniOdev3.Size = this.Size;
                yeniOdev3.Show();
            }

            this.Close();
        }

        // Ödev 5'e Git Linki
        private void lnkOdev5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta1Odev4_FormClosing;

            Hafta1Odev5 odev4 = new Hafta1Odev5();
            odev4.StartPosition = FormStartPosition.Manual;
            odev4.Location = this.Location;
            odev4.Size = this.Size;
            odev4.FormBorderStyle = this.FormBorderStyle;

            odev4.Show();
            this.Close();
        }
    }
}