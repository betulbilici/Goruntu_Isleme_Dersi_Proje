using System;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace GoruntuIsleme
{
    public partial class Hafta1Odev3 : Form
    {
        private Mat orijinalMat = null;

        public Hafta1Odev3()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            // X butonu kapatma olayını bağlıyoruz:
            this.FormClosing += Hafta1Odev3_FormClosing;

            // ComboBox ilk seçimleri
            if (cmbOrnekleme != null && cmbOrnekleme.Items.Count > 0) cmbOrnekleme.SelectedIndex = 0;
            if (cmbNicemleme != null && cmbNicemleme.Items.Count > 0) cmbNicemleme.SelectedIndex = 0;

            // Olayları güvenli bağlama
            if (btnResimSec != null)
            {
                btnResimSec.Click -= btnResimSec_Click;
                btnResimSec.Click += btnResimSec_Click;
            }

            if (btnUygula != null)
            {
                btnUygula.Click -= btnUygula_Click;
                btnUygula.Click += btnUygula_Click;
            }

            if (lnkGeri != null)
            {
                lnkGeri.LinkClicked -= lnkGeri_LinkClicked;
                lnkGeri.LinkClicked += lnkGeri_LinkClicked;
            }

            if (lnkOdev4 != null)
            {
                lnkOdev4.LinkClicked -= lnkOdev4_LinkClicked;
                lnkOdev4.LinkClicked += lnkOdev4_LinkClicked;
            }
        }

        // Sağ üstteki 'X' butonuna basıldığında Form1'e dönme
        private void Hafta1Odev3_FormClosing(object sender, FormClosingEventArgs e)
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

        // 1. Resim Seçme ve 1000x1000 Boyutuna Getirme
        private void btnResimSec_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Mat yuklenen = Cv2.ImRead(ofd.FileName, ImreadModes.Color);

                    if (yuklenen.Empty())
                    {
                        MessageBox.Show("Resim yüklenemedi!");
                        return;
                    }

                    // Slayt kuralı: 1000x1000 piksel boyutuna ölçekleme
                    orijinalMat?.Dispose();
                    orijinalMat = new Mat();
                    Cv2.Resize(yuklenen, orijinalMat, new OpenCvSharp.Size(1000, 1000));
                    yuklenen.Dispose();

                    // Orijinal resmi göster
                    picOrijinal.Image?.Dispose();
                    picOrijinal.Image = BitmapConverter.ToBitmap(orijinalMat);

                    // Başlangıçta sonucu da orijinalle göster
                    picSonuc.Image?.Dispose();
                    picSonuc.Image = BitmapConverter.ToBitmap(orijinalMat);
                }
            }
        }

        // 2. Örnekleme ve Renk Nicemleme
        private void btnUygula_Click(object sender, EventArgs e)
        {
            if (orijinalMat == null)
            {
                MessageBox.Show("Lütfen önce bir resim yükleyin!");
                return;
            }

            // Seçilen örnekleme adımı (2, 5, 10, 25, 50)
            int adim = 1;
            if (cmbOrnekleme != null && cmbOrnekleme.SelectedItem != null)
            {
                string secim = cmbOrnekleme.SelectedItem.ToString().Split(' ')[0];
                int.TryParse(secim, out adim);
                if (adim <= 0) adim = 1;
            }

            // Seçilen nicemleme adımı (10, 50, 100)
            int qAdim = 0;
            if (cmbNicemleme != null && cmbNicemleme.SelectedItem != null)
            {
                string secim = cmbNicemleme.SelectedItem.ToString().Split(' ')[0];
                int.TryParse(secim, out qAdim);
            }

            Mat sonucMat = orijinalMat.Clone();

            // Adım aralıklarıyla blok blok örnekleme
            for (int y = 0; y < 1000; y += adim)
            {
                for (int x = 0; x < 1000; x += adim)
                {
                    // Bloğun sol üst piksel rengi (BGR)
                    Vec3b renk = orijinalMat.At<Vec3b>(y, x);

                    // Renk Nicemlemesi (En yakın katına yuvarlama: 63 -> 60)
                    if (qAdim > 0)
                    {
                        renk.Item0 = Yuvarla(renk.Item0, qAdim); // B (Mavi)
                        renk.Item1 = Yuvarla(renk.Item1, qAdim); // G (Yeşil)
                        renk.Item2 = Yuvarla(renk.Item2, qAdim); // R (Kırmızı)
                    }

                    // Bloğun içindeki tüm pikselleri bu renkle doldur (Örnekleme etkisi)
                    int maxY = Math.Min(y + adim, 1000);
                    int maxX = Math.Min(x + adim, 1000);

                    for (int by = y; by < maxY; by++)
                    {
                        for (int bx = x; bx < maxX; bx++)
                        {
                            sonucMat.Set(by, bx, renk);
                        }
                    }
                }
            }

            picSonuc.Image?.Dispose();
            picSonuc.Image = BitmapConverter.ToBitmap(sonucMat);
            sonucMat.Dispose();
        }

        // En yakın katına yuvarlama fonksiyonu (Örn: 63 -> 60)
        private byte Yuvarla(byte deger, int adim)
        {
            int yuvarlanmis = (int)(Math.Round((double)deger / adim) * adim);
            if (yuvarlanmis > 255) yuvarlanmis = 255;
            if (yuvarlanmis < 0) yuvarlanmis = 0;
            return (byte)yuvarlanmis;
        }

        // Ödev 2'ye Geri Dön
        private void lnkGeri_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta1Odev3_FormClosing;

            Form odev2 = Application.OpenForms["Hafta1Odev2"];
            if (odev2 != null)
            {
                odev2.Show();
            }
            else
            {
                Hafta1Odev2 yeniOdev2 = new Hafta1Odev2();
                yeniOdev2.StartPosition = FormStartPosition.Manual;
                yeniOdev2.Location = this.Location;
                yeniOdev2.Show();
            }

            this.Close();
        }

        // Ödev 4'e Git
        private void lnkOdev4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Ödev 4'e geçerken Form1'in tetiklenmesini engelliyoruz:
            this.FormClosing -= Hafta1Odev3_FormClosing;

            Hafta1Odev4 odev4 = new Hafta1Odev4();
            odev4.StartPosition = FormStartPosition.Manual;
            odev4.Location = this.Location;
            odev4.Size = this.Size;
            odev4.FormBorderStyle = this.FormBorderStyle;

            odev4.Show();
            this.Close();
        }
    }
}