using System;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace GoruntuIsleme
{
    public partial class Hafta2Odev3 : Form
    {
        private Mat orijinalGri = null;
        private Mat islenmisGri = null;

        public Hafta2Odev3()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1250, 750);
            this.StartPosition = FormStartPosition.CenterScreen;

            this.FormClosing += Hafta2Odev3_FormClosing;

            // Sürgü sınır ayarları (0-255)
            if (trackAlt != null && trackUst != null)
            {
                trackAlt.Minimum = 0; trackAlt.Maximum = 255; trackAlt.Value = 0;
                trackUst.Minimum = 0; trackUst.Maximum = 255; trackUst.Value = 255;

                trackAlt.Scroll += TrackBar_Scroll;
                trackUst.Scroll += TrackBar_Scroll;
            }

            // Olay bağlantıları
            if (btnResimSec != null) btnResimSec.Click += btnResimSec_Click;
            if (lnkGeri != null) lnkGeri.LinkClicked += lnkGeri_LinkClicked;
            if (lnkSonraki != null) lnkSonraki.LinkClicked += lnkSonraki_LinkClicked;

            // c Şıkkı: MouseMove ve MouseDown olayları
            if (picYeni != null)
            {
                picYeni.MouseMove += PicYeni_MouseOlayi;
                picYeni.MouseDown += PicYeni_MouseOlayi;
            }
        }

        private void Hafta2Odev3_FormClosing(object sender, FormClosingEventArgs e)
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

                        orijinalGri?.Dispose();
                        orijinalGri = new Mat();
                        // Slaytta açıkça siyah-beyaz / gri istendiği için griye çeviriyoruz:
                        Cv2.CvtColor(yuklenen, orijinalGri, ColorConversionCodes.BGR2GRAY);
                    }

                    picOrijinal.Image?.Dispose();
                    picOrijinal.Image = BitmapConverter.ToBitmap(orijinalGri);

                    // Başlangıç histogramını çiz
                    HistogramCiz(orijinalGri, picHistOrijinal, Color.Yellow);

                    if (trackAlt != null) trackAlt.Value = 0;
                    if (trackUst != null) trackUst.Value = 255;

                    ResmiSikistirVeHistogramGuncelle();
                }
            }
        }

        // Çift sürgü kaydırma olayı
        private void TrackBar_Scroll(object sender, EventArgs e)
        {
            if (trackAlt == null || trackUst == null) return;

            // Alt sürgü üst sürgüyü geçemesin
            if (trackAlt.Value > trackUst.Value)
            {
                if (sender == trackAlt) trackUst.Value = trackAlt.Value;
                else trackAlt.Value = trackUst.Value;
            }

            if (lblAralik != null)
            {
                lblAralik.Text = $"Sıkıştırma Aralığı: [{trackAlt.Value} - {trackUst.Value}]";
            }

            ResmiSikistirVeHistogramGuncelle();
        }

        // a ve b Şıkları: Pikselleri [alt, ust] sınırına ölçekleme
        private void ResmiSikistirVeHistogramGuncelle()
        {
            if (orijinalGri == null) return;

            int alt = trackAlt != null ? trackAlt.Value : 0;
            int ust = trackUst != null ? trackUst.Value : 255;

            islenmisGri?.Dispose();
            islenmisGri = new Mat(orijinalGri.Size(), MatType.CV_8UC1);

            int w = orijinalGri.Width;
            int h = orijinalGri.Height;

            // Slayttaki ölçekleme formülü: alt + (piksel * (ust - alt)) / 255
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    byte eskiRenk = orijinalGri.At<byte>(y, x);
                    int yeniRenk = alt + (int)(((float)eskiRenk / 255f) * (ust - alt));

                    if (yeniRenk < 0) yeniRenk = 0;
                    if (yeniRenk > 255) yeniRenk = 255;

                    islenmisGri.Set<byte>(y, x, (byte)yeniRenk);
                }
            }

            picYeni.Image?.Dispose();
            picYeni.Image = BitmapConverter.ToBitmap(islenmisGri);

            // Sıkıştırılmış resmin histogramını yenile
            HistogramCiz(islenmisGri, picHistYeni, Color.Cyan);
        }

        // Histogram Çizim Metodu
        private void HistogramCiz(Mat griMat, PictureBox hedefBox, Color sutunRengi)
        {
            if (griMat == null || hedefBox == null) return;

            int w = hedefBox.Width;
            int h = hedefBox.Height;
            if (w <= 0 || h <= 0) return;

            int[] pikselSayilari = new int[256];
            for (int y = 0; y < griMat.Height; y++)
            {
                for (int x = 0; x < griMat.Width; x++)
                {
                    byte val = griMat.At<byte>(y, x);
                    pikselSayilari[val]++;
                }
            }

            int maksPiksel = 1;
            for (int i = 0; i < 256; i++)
            {
                if (pikselSayilari[i] > maksPiksel)
                    maksPiksel = pikselSayilari[i];
            }

            Bitmap bmp = new Bitmap(w, h);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Black);

                using (Pen kalem = new Pen(sutunRengi, 1))
                {
                    for (int i = 0; i < 256; i++)
                    {
                        float x = (float)i / 255f * (w - 1);
                        float barBoyu = ((float)pikselSayilari[i] / maksPiksel) * (h - 15);
                        g.DrawLine(kalem, x, h - 1, x, h - 1 - barBoyu);
                    }
                }

                // 50'lik referans dikey çizgiler
                using (Pen refKalem = new Pen(Color.FromArgb(70, 70, 70), 1))
                {
                    for (int i = 50; i <= 250; i += 50)
                    {
                        float xRef = (float)i / 255f * (w - 1);
                        g.DrawLine(refKalem, xRef, 0, xRef, h);
                    }
                }
            }

            hedefBox.Image?.Dispose();
            hedefBox.Image = bmp;
        }

        // c Şıkkı: MouseMove ve MouseDown Olayı
        private void PicYeni_MouseOlayi(object sender, MouseEventArgs e)
        {
            if (islenmisGri == null || picYeni.Image == null || picYeni.Width <= 0 || picYeni.Height <= 0) return;

            // PictureBox koordinatını resmin gerçek piksel boyutuna ölçekle
            int pikselX = (int)((float)e.X / picYeni.Width * islenmisGri.Width);
            int pikselY = (int)((float)e.Y / picYeni.Height * islenmisGri.Height);

            if (pikselX >= 0 && pikselX < islenmisGri.Width && pikselY >= 0 && pikselY < islenmisGri.Height)
            {
                byte gri = islenmisGri.At<byte>(pikselY, pikselX);

                // Gri resimde R=G=B eşittir
                if (txtPikselBilgi != null)
                {
                    txtPikselBilgi.Text = $"X: {pikselX}, Y: {pikselY} | R: {gri}, G: {gri}, B: {gri} | Gri: {gri}";
                }
            }
        }

        private void lnkGeri_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta2Odev3_FormClosing;
            try
            {
                Type odev2Turu = Type.GetType("GoruntuIsleme.Hafta2Odev2");
                if (odev2Turu != null)
                {
                    Form odev2 = (Form)Activator.CreateInstance(odev2Turu);
                    odev2.StartPosition = FormStartPosition.Manual;
                    odev2.Location = this.Location;
                    odev2.Size = this.Size;
                    odev2.Show();
                    this.Close();
                }
            }
            catch { }
        }

        private void lnkSonraki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta2Odev3_FormClosing;

            Hafta2Odev4 odev4 = new Hafta2Odev4();
            odev4.StartPosition = FormStartPosition.Manual;
            odev4.Location = this.Location;
            odev4.Size = this.Size;
            odev4.Show();
            this.Close();
        }
    }
}