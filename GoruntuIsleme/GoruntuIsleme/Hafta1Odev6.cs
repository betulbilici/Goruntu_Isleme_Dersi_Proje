using System;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace GoruntuIsleme
{
    public partial class Hafta1Odev6 : Form
    {
        public Hafta1Odev6()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            this.StartPosition = FormStartPosition.CenterScreen;

            // X butonu kapatma olayını bağlıyoruz:
            this.FormClosing += Hafta1Odev6_FormClosing;

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

            if (lnkOdev7 != null)
            {
                lnkOdev7.LinkClicked -= lnkOdev7_LinkClicked;
                lnkOdev7.LinkClicked += lnkOdev7_LinkClicked;
            }
        }

        // X butonuna basıldığında Form1'e dönen metot:
        private void Hafta1Odev6_FormClosing(object sender, FormClosingEventArgs e)
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

                        // 1. Orijinal Renkli Görüntü
                        picOrijinal.Image?.Dispose();
                        picOrijinal.Image = BitmapConverter.ToBitmap(original);

                        // Kanalları BGR sırasıyla ayır: kanallar[0]=Blue, kanallar[1]=Green, kanallar[2]=Red
                        Mat[] kanallar = Cv2.Split(original);

                        // 2. Kırmızı (R) Bant Gri-Ton: (R, R, R)
                        using (Mat kirmiziGriMat = new Mat())
                        {
                            Mat[] rrr = new Mat[] { kanallar[2], kanallar[2], kanallar[2] };
                            Cv2.Merge(rrr, kirmiziGriMat);

                            picKirmiziGri.Image?.Dispose();
                            picKirmiziGri.Image = BitmapConverter.ToBitmap(kirmiziGriMat);
                        }

                        // 3. Yeşil (G) Bant Gri-Ton: (G, G, G)
                        using (Mat yesilGriMat = new Mat())
                        {
                            Mat[] ggg = new Mat[] { kanallar[1], kanallar[1], kanallar[1] };
                            Cv2.Merge(ggg, yesilGriMat);

                            picYesilGri.Image?.Dispose();
                            picYesilGri.Image = BitmapConverter.ToBitmap(yesilGriMat);
                        }

                        // 4. Mavi (B) Bant Gri-Ton: (B, B, B)
                        using (Mat maviGriMat = new Mat())
                        {
                            Mat[] bbb = new Mat[] { kanallar[0], kanallar[0], kanallar[0] };
                            Cv2.Merge(bbb, maviGriMat);

                            picMaviGri.Image?.Dispose();
                            picMaviGri.Image = BitmapConverter.ToBitmap(maviGriMat);
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

        // Ödev 5'e Geri Dön Linki
        private void lnkGeri_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta1Odev6_FormClosing;

            Form odev5 = Application.OpenForms["Hafta1Odev5"];
            if (odev5 != null)
            {
                odev5.Show();
            }
            else
            {
                Hafta1Odev5 yeniOdev5 = new Hafta1Odev5();
                yeniOdev5.StartPosition = FormStartPosition.Manual;
                yeniOdev5.Location = this.Location;
                yeniOdev5.Size = this.Size;
                yeniOdev5.Show();
            }

            this.Close();
        }

        // Ödev 7'ye Git Linki
        private void lnkOdev7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta1Odev6_FormClosing;

            Hafta1Odev7 odev7 = new Hafta1Odev7();
            odev7.StartPosition = FormStartPosition.Manual;
            odev7.Location = this.Location;
            odev7.Size = this.Size;
            odev7.FormBorderStyle = this.FormBorderStyle;

            odev7.Show();
            this.Close();
        }
    }
}