using System;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace GoruntuIsleme
{
    public partial class Hafta1Odev5 : Form
    {
        public Hafta1Odev5()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            this.StartPosition = FormStartPosition.CenterScreen;

            // X butonu kapatma olayını bağlıyoruz:
            this.FormClosing += Hafta1Odev5_FormClosing;

            // Olayları bağlama
            if (btnResimSec != null)
            {
                btnResimSec.Click -= btnResimSec_Click;
                btnResimSec.Click += btnResimSec_Click;
            }

            if (lnkGeri != null)
            {
                lnkGeri.LinkClicked -= lnkGeri_LinkClicked;
                lnkGeri.LinkClicked += lnkGeri_LinkClicked;
            }

            if (lnkOdev6 != null)
            {
                lnkOdev6.LinkClicked -= lnkOdev6_LinkClicked;
                lnkOdev6.LinkClicked += lnkOdev6_LinkClicked;
            }
        }

        // Sağ üstteki 'X' butonuna basıldığında Form1'e dönme
        private void Hafta1Odev5_FormClosing(object sender, FormClosingEventArgs e)
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

        // Resim Seçme ve Kanalları Ayrıştırma (R00, 0G0, 00B)
        private void btnResimSec_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    using (Mat orijinal = Cv2.ImRead(ofd.FileName, ImreadModes.Color))
                    {
                        if (orijinal.Empty())
                        {
                            MessageBox.Show("Resim yüklenemedi!");
                            return;
                        }

                        // Orijinal renkli görseli göster
                        picOrijinal.Image?.Dispose();
                        picOrijinal.Image = BitmapConverter.ToBitmap(orijinal);

                        // OpenCV kanalları BGR sırasındadır: [0]=Blue, [1]=Green, [2]=Red
                        Mat[] kanallar = Cv2.Split(orijinal);

                        // Sıfır değerli boş kanal (Siyah / 0)
                        using (Mat sifirKanal = Mat.Zeros(orijinal.Size(), MatType.CV_8UC1))
                        {
                            // 1. Kırmızı Kanal Görüntüsü (R00): B=0, G=0, R=orijinal_R
                            using (Mat kirmiziMat = new Mat())
                            {
                                Mat[] bgrKirmizi = new Mat[] { sifirKanal, sifirKanal, kanallar[2] };
                                Cv2.Merge(bgrKirmizi, kirmiziMat);

                                picKirmizi.Image?.Dispose();
                                picKirmizi.Image = BitmapConverter.ToBitmap(kirmiziMat);
                            }

                            // 2. Yeşil Kanal Görüntüsü (0G0): B=0, G=orijinal_G, R=0
                            using (Mat yesilMat = new Mat())
                            {
                                Mat[] bgrYesil = new Mat[] { sifirKanal, kanallar[1], sifirKanal };
                                Cv2.Merge(bgrYesil, yesilMat);

                                picYesil.Image?.Dispose();
                                picYesil.Image = BitmapConverter.ToBitmap(yesilMat);
                            }

                            // 3. Mavi Kanal Görüntüsü (00B): B=orijinal_B, G=0, R=0
                            using (Mat maviMat = new Mat())
                            {
                                Mat[] bgrMavi = new Mat[] { kanallar[0], sifirKanal, sifirKanal };
                                Cv2.Merge(bgrMavi, maviMat);

                                picMavi.Image?.Dispose();
                                picMavi.Image = BitmapConverter.ToBitmap(maviMat);
                            }
                        }

                        // Kanalları bellekten temizle
                        for (int i = 0; i < kanallar.Length; i++)
                        {
                            kanallar[i].Dispose();
                        }
                    }
                }
            }
        }

        // Ödev 4'e Geri Dön Linki
        private void lnkGeri_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta1Odev5_FormClosing;

            Form odev4 = Application.OpenForms["Hafta1Odev4"];
            if (odev4 != null)
            {
                odev4.Show();
            }
            else
            {
                Hafta1Odev4 yeniOdev4 = new Hafta1Odev4();
                yeniOdev4.StartPosition = FormStartPosition.Manual;
                yeniOdev4.Location = this.Location;
                yeniOdev4.Size = this.Size;
                yeniOdev4.Show();
            }

            this.Close();
        }

        // Ödev 6'ya Git Linki
        private void lnkOdev6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Ödev 6'ya geçerken Form1'in tetiklenmesini önlüyoruz:
            this.FormClosing -= Hafta1Odev5_FormClosing;

            Hafta1Odev6 odev6 = new Hafta1Odev6();
            odev6.StartPosition = FormStartPosition.Manual;
            odev6.Location = this.Location;
            odev6.Size = this.Size;
            odev6.FormBorderStyle = this.FormBorderStyle;

            odev6.Show();
            this.Close();
        }
    }
}