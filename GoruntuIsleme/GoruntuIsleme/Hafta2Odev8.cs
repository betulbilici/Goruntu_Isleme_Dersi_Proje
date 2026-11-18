using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace GoruntuIsleme
{
    public partial class Hafta2Odev8 : Form
    {
        // Referans nokta modeli
        private class ReferansNokta
        {
            public System.Drawing.Point Konum { get; set; }
            public Color Renk { get; set; }
            public byte GriDeger { get; set; }
        }

        private Mat griMat = null;
        private List<ReferansNokta> noktalar = new List<ReferansNokta>();
        private Color aktifRenk = Color.SkyBlue; // Başlangıç rengi

        public Hafta2Odev8()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1250, 750);
            this.StartPosition = FormStartPosition.CenterScreen;

            // X butonu ile güvenli dönüş
            this.FormClosing += Hafta2Odev8_FormClosing;

            // Önizleme kutusu
            if (lblRenkOnizleme != null) lblRenkOnizleme.BackColor = aktifRenk;

            // Olay bağlantıları
            if (btnResimSec != null) btnResimSec.Click += btnResimSec_Click;
            if (btnTemizle != null) btnTemizle.Click += btnTemizle_Click;
            if (btnRenkSec != null) btnRenkSec.Click += btnRenkSec_Click;
            if (btnRenklendir != null) btnRenklendir.Click += btnRenklendir_Click;
            if (lnkGeri != null) lnkGeri.LinkClicked += lnkGeri_LinkClicked;
            if (lnkSonraki != null) lnkSonraki.LinkClicked += lnkSonraki_LinkClicked;

            if (picGiris != null)
            {
                picGiris.MouseDown += PicGiris_MouseDown;
            }
        }

        private void Hafta2Odev8_FormClosing(object sender, FormClosingEventArgs e)
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
                    using (Mat yuklenen = Cv2.ImRead(ofd.FileName, ImreadModes.Color))
                    {
                        if (yuklenen.Empty())
                        {
                            MessageBox.Show("Resim yüklenemedi!");
                            return;
                        }

                        griMat?.Dispose();
                        griMat = new Mat();
                        // Tarihi siyah-beyaz fotoğraf simülasyonu için griye çeviriyoruz
                        Cv2.CvtColor(yuklenen, griMat, ColorConversionCodes.BGR2GRAY);
                    }

                    btnTemizle_Click(null, null);
                }
            }
        }

        // Renk Paletinden Renk Seçme
        private void btnRenkSec_Click(object sender, EventArgs e)
        {
            using (ColorDialog cd = new ColorDialog())
            {
                cd.Color = aktifRenk;
                if (cd.ShowDialog() == DialogResult.OK)
                {
                    aktifRenk = cd.Color;
                    if (lblRenkOnizleme != null) lblRenkOnizleme.BackColor = aktifRenk;
                }
            }
        }

        // Sol Tık ile Nokta Ekleme (Maksimum 10 Nokta)
        private void PicGiris_MouseDown(object sender, MouseEventArgs e)
        {
            if (griMat == null || picGiris.Image == null || picGiris.Width <= 0 || picGiris.Height <= 0) return;

            if (noktalar.Count >= 10)
            {
                MessageBox.Show("Ödev şartına göre en fazla 10 adet referans nokta belirleyebilirsiniz!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // PictureBox boyutunu gerçek resim piksel boyutuna ölçekle
            int gercekX = (int)((float)e.X / picGiris.Width * griMat.Width);
            int gercekY = (int)((float)e.Y / picGiris.Height * griMat.Height);

            if (gercekX >= 0 && gercekX < griMat.Width && gercekY >= 0 && gercekY < griMat.Height)
            {
                byte gri = griMat.At<byte>(gercekY, gercekX);

                ReferansNokta yeniNokta = new ReferansNokta
                {
                    Konum = new System.Drawing.Point(gercekX, gercekY),
                    Renk = aktifRenk,
                    GriDeger = gri
                };

                noktalar.Add(yeniNokta);
                NoktalariEkrandaGoster();
            }
        }

        // Noktaları Çizdirme ve Listeleme (Indexed Pixel Hatası Giderilmiş Hali)
        private void NoktalariEkrandaGoster()
        {
            if (griMat == null) return;

            // 8-bit Indexed Format hatasını çözmek için gri görüntüyü geçici BGR matrise çevirip Bitmap yapıyoruz:
            Bitmap bmp;
            using (Mat bgrKopya = new Mat())
            {
                Cv2.CvtColor(griMat, bgrKopya, ColorConversionCodes.GRAY2BGR);
                bmp = BitmapConverter.ToBitmap(bgrKopya);
            }

            using (Graphics g = Graphics.FromImage(bmp))
            {
                for (int i = 0; i < noktalar.Count; i++)
                {
                    var p = noktalar[i];
                    using (Brush b = new SolidBrush(p.Renk))
                    using (Pen cerceve = new Pen(Color.White, 2))
                    {
                        g.FillEllipse(b, p.Konum.X - 6, p.Konum.Y - 6, 12, 12);
                        g.DrawEllipse(cerceve, p.Konum.X - 6, p.Konum.Y - 6, 12, 12);

                        using (Font f = new Font("Arial", 9, FontStyle.Bold))
                        using (Brush yazi = new SolidBrush(Color.Yellow))
                        {
                            g.DrawString((i + 1).ToString(), f, yazi, p.Konum.X + 7, p.Konum.Y - 6);
                        }
                    }
                }
            }

            picGiris.Image?.Dispose();
            picGiris.Image = bmp;

            if (lstNoktalar != null)
            {
                lstNoktalar.Items.Clear();
                for (int i = 0; i < noktalar.Count; i++)
                {
                    var p = noktalar[i];
                    lstNoktalar.Items.Add($"Nokta {i + 1}: Konum=({p.Konum.X}, {p.Konum.Y}) | Renk=RGB({p.Renk.R}, {p.Renk.G}, {p.Renk.B}) | Gri Değeri={p.GriDeger}");
                }
            }

            if (lblDurum != null)
            {
                lblDurum.Text = $"Seçilen Nokta: {noktalar.Count} / 10";
            }
        }

        // Noktaları Sıfırla / Temizle Butonu
        private void btnTemizle_Click(object sender, EventArgs e)
        {
            noktalar.Clear();

            if (griMat != null)
            {
                using (Mat bgrKopya = new Mat())
                {
                    Cv2.CvtColor(griMat, bgrKopya, ColorConversionCodes.GRAY2BGR);
                    picGiris.Image?.Dispose();
                    picGiris.Image = BitmapConverter.ToBitmap(bgrKopya);
                }
            }

            picCikis.Image?.Dispose();
            picCikis.Image = null;

            if (lstNoktalar != null) lstNoktalar.Items.Clear();
            if (lblDurum != null) lblDurum.Text = "Seçilen Nokta: 0 / 10";
        }

        // Ödev 8 Renklendirme Algoritması (IDW + Gri Benzerliği Harmonisi)
        private void btnRenklendir_Click(object sender, EventArgs e)
        {
            if (griMat == null)
            {
                MessageBox.Show("Lütfen önce bir resim seçin!");
                return;
            }

            if (noktalar.Count == 0)
            {
                MessageBox.Show("Lütfen resim üzerinden en az bir referans nokta belirleyin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.Cursor = Cursors.WaitCursor;

            int w = griMat.Width;
            int h = griMat.Height;
            Mat cikisMat = new Mat(h, w, MatType.CV_8UC3);

            double griAgirlikKatsayisi = 0.5;

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    byte pikselGri = griMat.At<byte>(y, x);

                    double toplamAgirlik = 0.0;
                    double toplamR = 0.0;
                    double toplamG = 0.0;
                    double toplamB = 0.0;

                    bool tamNoktaUstu = false;

                    for (int i = 0; i < noktalar.Count; i++)
                    {
                        var refNokta = noktalar[i];
                        double dx = x - refNokta.Konum.X;
                        double dy = y - refNokta.Konum.Y;
                        double mesafeKare = (dx * dx) + (dy * dy);

                        if (mesafeKare == 0)
                        {
                            cikisMat.Set(y, x, new Vec3b(refNokta.Renk.B, refNokta.Renk.G, refNokta.Renk.R));
                            tamNoktaUstu = true;
                            break;
                        }

                        double griFarki = Math.Abs(pikselGri - refNokta.GriDeger);
                        double payda = (mesafeKare) + (griAgirlikKatsayisi * (griFarki * griFarki)) + 1.0;
                        double agirlik = 1.0 / payda;

                        toplamAgirlik += agirlik;
                        toplamR += agirlik * refNokta.Renk.R;
                        toplamG += agirlik * refNokta.Renk.G;
                        toplamB += agirlik * refNokta.Renk.B;
                    }

                    if (!tamNoktaUstu && toplamAgirlik > 0)
                    {
                        double tahminiR = toplamR / toplamAgirlik;
                        double tahminiG = toplamG / toplamAgirlik;
                        double tahminiB = toplamB / toplamAgirlik;

                        double tahminiGri = (tahminiR * 0.299) + (tahminiG * 0.587) + (tahminiB * 0.114);
                        if (tahminiGri < 1.0) tahminiGri = 1.0;

                        double carpan = (double)pikselGri / tahminiGri;

                        int sonR = Math.Min(255, (int)(tahminiR * carpan));
                        int sonG = Math.Min(255, (int)(tahminiG * carpan));
                        int sonB = Math.Min(255, (int)(tahminiB * carpan));

                        cikisMat.Set(y, x, new Vec3b((byte)sonB, (byte)sonG, (byte)sonR));
                    }
                }
            }

            picCikis.Image?.Dispose();
            picCikis.Image = BitmapConverter.ToBitmap(cikisMat);
            cikisMat.Dispose();

            this.Cursor = Cursors.Default;
        }

        // Ödev 7'ye Geri Dön Linki
        private void lnkGeri_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta2Odev8_FormClosing;

            Hafta2Odev7 odev7 = new Hafta2Odev7();
            odev7.StartPosition = FormStartPosition.Manual;
            odev7.Location = this.Location;
            odev7.Size = this.Size;
            odev7.FormBorderStyle = this.FormBorderStyle;

            odev7.Show();
            this.Close();
        }

        // Ödev 9'a Git Linki
        private void lnkSonraki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta2Odev8_FormClosing;

            Hafta2Odev9 odev9 = new Hafta2Odev9();
            odev9.StartPosition = FormStartPosition.Manual;
            odev9.Location = this.Location;
            odev9.Size = this.Size;
            odev9.FormBorderStyle = this.FormBorderStyle;

            odev9.Show();
            this.Close();
        }
    }
}