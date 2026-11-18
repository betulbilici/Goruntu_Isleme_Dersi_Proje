using System;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace GoruntuIsleme
{
    public partial class Hafta2Odev1 : Form
    {
        private Mat orijinalMat = null;

        public Hafta2Odev1()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            this.StartPosition = FormStartPosition.CenterScreen;

            // X butonu kapatma olayını bağlıyoruz:
            this.FormClosing -= Hafta2Odev1_FormClosing;
            this.FormClosing += Hafta2Odev1_FormClosing;

            // TrackBar ayarları (-255 ile +255 arası)
            if (trackParlaklik != null)
            {
                trackParlaklik.Minimum = -255;
                trackParlaklik.Maximum = 255;
                trackParlaklik.Value = 0;
                trackParlaklik.Scroll -= trackParlaklik_Scroll;
                trackParlaklik.Scroll += trackParlaklik_Scroll;
            }

            // Olay bağlantıları
            if (btnResimSec != null)
            {
                btnResimSec.Click -= btnResimSec_Click;
                btnResimSec.Click += btnResimSec_Click;
            }

            if (lnkOdev2 != null)
            {
                lnkOdev2.LinkClicked -= lnkOdev2_LinkClicked;
                lnkOdev2.LinkClicked += lnkOdev2_LinkClicked;
            }
        }

        // X butonuna basıldığında Form1'e dönen metot:
        private void Hafta2Odev1_FormClosing(object sender, FormClosingEventArgs e)
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
                    orijinalMat?.Dispose();
                    orijinalMat = Cv2.ImRead(ofd.FileName, ImreadModes.Color);

                    if (orijinalMat.Empty())
                    {
                        MessageBox.Show("Resim yüklenemedi!");
                        return;
                    }

                    picOrijinal.Image?.Dispose();
                    picOrijinal.Image = BitmapConverter.ToBitmap(orijinalMat);

                    if (trackParlaklik != null) trackParlaklik.Value = 0;
                    if (lblDeger != null) lblDeger.Text = "Parlaklık Değeri (b): 0";
                    ParlaklikUygula(0);
                }
            }
        }

        // Sürgü hareket ettikçe anlık (dinamik) parlaklık değişimi
        private void trackParlaklik_Scroll(object sender, EventArgs e)
        {
            if (orijinalMat == null) return;
            int bDegeri = trackParlaklik.Value;
            if (lblDeger != null) lblDeger.Text = $"Parlaklık Değeri (b): {bDegeri}";
            ParlaklikUygula(bDegeri);
        }

        // Slayttaki formüle göre parlaklık değiştirme metodu:
        // f(x, y) = g(x, y)(R ± b, G ± b, B ± b)
        private void ParlaklikUygula(int b)
        {
            if (orijinalMat == null) return;

            Mat sonucMat = new Mat(orijinalMat.Size(), MatType.CV_8UC3);

            int genislik = orijinalMat.Width;
            int yukseklik = orijinalMat.Height;

            for (int y = 0; y < yukseklik; y++)
            {
                for (int x = 0; x < genislik; x++)
                {
                    Vec3b okunanRenk = orijinalMat.At<Vec3b>(y, x);

                    // B, G, R kanallarına b eklenir/çıkarılır
                    int B = okunanRenk.Item0 + b;
                    int G = okunanRenk.Item1 + b;
                    int R = okunanRenk.Item2 + b;

                    // 0 ile 255 arasına sınırlandırma:
                    if (R > 255) R = 255;
                    if (R < 0) R = 0;

                    if (G > 255) G = 255;
                    if (G < 0) G = 0;

                    if (B > 255) B = 255;
                    if (B < 0) B = 0;

                    sonucMat.Set(y, x, new Vec3b((byte)B, (byte)G, (byte)R));
                }
            }

            picSonuc.Image?.Dispose();
            picSonuc.Image = BitmapConverter.ToBitmap(sonucMat);
            sonucMat.Dispose();
        }

        // Ödev 2'ye Git Linki
        private void lnkOdev2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta2Odev1_FormClosing;

            Hafta2Odev2 odev2 = new Hafta2Odev2();
            odev2.StartPosition = FormStartPosition.Manual;
            odev2.Location = this.Location;
            odev2.Size = this.Size;
            odev2.FormBorderStyle = this.FormBorderStyle;

            odev2.Show();
            this.Close();
        }
    }
}