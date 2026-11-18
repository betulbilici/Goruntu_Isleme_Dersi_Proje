using System;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace GoruntuIsleme
{
    public partial class Hafta1Odev7 : Form
    {
        public Hafta1Odev7()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            this.StartPosition = FormStartPosition.CenterScreen;

            // X butonu kapatma olayını bağlıyoruz:
            this.FormClosing += Hafta1Odev7_FormClosing;

            // Olayları güvenli bağlama
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

            if (lnkOdev8 != null)
            {
                lnkOdev8.LinkClicked -= lnkOdev8_LinkClicked;
                lnkOdev8.LinkClicked += lnkOdev8_LinkClicked;
            }
        }

        // X butonuna basıldığında Form1'e dönen metot:
        private void Hafta1Odev7_FormClosing(object sender, FormClosingEventArgs e)
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

        private void btnResimSec_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    using (Mat original = Cv2.ImRead(ofd.FileName, ImreadModes.Color))
                    {
                        if (original.Empty())
                        {
                            MessageBox.Show("Resim yüklenemedi!");
                            return;
                        }

                        // 1. Bant Birleşimi (1-2-3): Doğal Renk (RGB)
                        picOrijinal.Image?.Dispose();
                        picOrijinal.Image = BitmapConverter.ToBitmap(original);

                        // OpenCV kanalları BGR sırasındadır:
                        // kanallar[0] = Mavi (Bant 3)
                        // kanallar[1] = Yeşil (Bant 2)
                        // kanallar[2] = Kırmızı (Bant 1)
                        Mat[] kanallar = Cv2.Split(original);

                        // 2. Bant Birleşimi (2-1-3): Yapay Renk (GRB)
                        // BGR sırasıyla: (B, R, G)
                        using (Mat mat213 = new Mat())
                        {
                            Mat[] bgr213 = new Mat[] { kanallar[0], kanallar[2], kanallar[1] };
                            Cv2.Merge(bgr213, mat213);

                            picBant213.Image?.Dispose();
                            picBant213.Image = BitmapConverter.ToBitmap(mat213);
                        }

                        // 3. Bant Birleşimi (1-3-2): Yapay Renk (RBG)
                        // BGR sırasıyla: (G, B, R)
                        using (Mat mat132 = new Mat())
                        {
                            Mat[] bgr132 = new Mat[] { kanallar[1], kanallar[0], kanallar[2] };
                            Cv2.Merge(bgr132, mat132);

                            picBant132.Image?.Dispose();
                            picBant132.Image = BitmapConverter.ToBitmap(mat132);
                        }

                        // 4. Bant Birleşimi (2-3-1): Yapay Renk (GBR)
                        // BGR sırasıyla: (R, B, G)
                        using (Mat mat231 = new Mat())
                        {
                            Mat[] bgr231 = new Mat[] { kanallar[2], kanallar[0], kanallar[1] };
                            Cv2.Merge(bgr231, mat231);

                            picBant231.Image?.Dispose();
                            picBant231.Image = BitmapConverter.ToBitmap(mat231);
                        }

                        // Kanalları bellekten serbest bırak
                        for (int i = 0; i < kanallar.Length; i++)
                        {
                            kanallar[i].Dispose();
                        }
                    }
                }
            }
        }

        // Ödev 6'ya Geri Dön
        private void lnkGeri_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta1Odev7_FormClosing;

            Form odev6 = Application.OpenForms["Hafta1Odev6"];
            if (odev6 != null)
            {
                odev6.Show();
            }
            else
            {
                Hafta1Odev6 yeniOdev6 = new Hafta1Odev6();
                yeniOdev6.StartPosition = FormStartPosition.Manual;
                yeniOdev6.Location = this.Location;
                yeniOdev6.Size = this.Size;
                yeniOdev6.Show();
            }

            this.Close();
        }

        // Varsa Ödev 8'e Git Linki
        private void lnkOdev8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta1Odev7_FormClosing;

            Hafta1Odev8 odev8 = new Hafta1Odev8();
            odev8.StartPosition = FormStartPosition.Manual;
            odev8.Location = this.Location;
            odev8.Size = this.Size;
            odev8.FormBorderStyle = this.FormBorderStyle;

            odev8.Show();
            this.Close();
        }
    }
}