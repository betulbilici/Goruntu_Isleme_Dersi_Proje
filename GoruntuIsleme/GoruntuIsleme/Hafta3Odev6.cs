using System;
using System.Drawing;
using System.Windows.Forms;

namespace GoruntuIsleme
{
    public partial class Hafta3Odev6 : Form
    {
        private Bitmap orijinalBmp = null;

        public Hafta3Odev6()
        {
            InitializeComponent();
            this.FormClosing += Hafta3Odev6_FormClosing;

            if (btnResimSec != null) btnResimSec.Click += btnResimSec_Click;
            if (btnKucult != null) btnKucult.Click += btnKucult_Click;
            if (lnkGeri != null) lnkGeri.LinkClicked += lnkGeri_LinkClicked;
            if (lnkSonraki != null) lnkSonraki.LinkClicked += lnkSonraki_LinkClicked;
        }

        private void Hafta3Odev6_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Form1 anaForm = (Form1)Application.OpenForms["Form1"];
                if (anaForm != null)
                {
                    anaForm.Show();
                    anaForm.BringToFront();
                }
                else
                {
                    new Form1().Show();
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
                    orijinalBmp?.Dispose();
                    using (Bitmap temp = new Bitmap(ofd.FileName))
                    {
                        orijinalBmp = new Bitmap(temp);
                    }

                    if (picOrijinal != null)
                    {
                        picOrijinal.Image?.Dispose();
                        picOrijinal.Image = new Bitmap(orijinalBmp);
                    }

                    if (lblBilgi != null)
                    {
                        lblBilgi.Text = $"Resim Yüklendi: {orijinalBmp.Width}x{orijinalBmp.Height} px\nKüçültme yüzdesini girip butona basınız.";
                    }
                }
            }
        }

        private void btnKucult_Click(object sender, EventArgs e)
        {
            if (orijinalBmp == null)
            {
                MessageBox.Show("Lütfen önce bir resim seçiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            double yuzde = (double)numOran.Value;
            double oran = yuzde / 100.0;

            int kaynakW = orijinalBmp.Width;
            int kaynakH = orijinalBmp.Height;

            int hedefW = Math.Max(1, (int)Math.Round(kaynakW * oran));
            int hedefH = Math.Max(1, (int)Math.Round(kaynakH * oran));

            this.Cursor = Cursors.WaitCursor;

            // Hocanın slaytındaki gibi: Tuval orijinal boyutta kalır, küçülen resim sol üste oturur
            Bitmap bmpDegistirme = new Bitmap(kaynakW, kaynakH);
            using (Graphics g = Graphics.FromImage(bmpDegistirme))
            {
                g.Clear(Color.LightGray); // Arka plan belli olsun diye gri tuval
            }

            Bitmap bmpInterpolasyon = new Bitmap(kaynakW, kaynakH);
            using (Graphics g = Graphics.FromImage(bmpInterpolasyon))
            {
                g.Clear(Color.LightGray);
            }

            // --- YÖNTEM A: PİKSEL DEĞİŞTİRME ---
            for (int y = 0; y < hedefH; y++)
            {
                int srcY = Math.Min((int)Math.Floor(y / oran), kaynakH - 1);
                for (int x = 0; x < hedefW; x++)
                {
                    int srcX = Math.Min((int)Math.Floor(x / oran), kaynakW - 1);
                    bmpDegistirme.SetPixel(x, y, orijinalBmp.GetPixel(srcX, srcY));
                }
            }

            // --- YÖNTEM B: PİKSEL İNTERPOLASYONU ---
            for (int y = 0; y < hedefH; y++)
            {
                float gy = (float)(y / oran);
                int y1 = Math.Min((int)Math.Floor(gy), kaynakH - 1);
                int y2 = Math.Min(y1 + 1, kaynakH - 1);
                float dy = gy - y1;

                for (int x = 0; x < hedefW; x++)
                {
                    float gx = (float)(x / oran);
                    int x1 = Math.Min((int)Math.Floor(gx), kaynakW - 1);
                    int x2 = Math.Min(x1 + 1, kaynakW - 1);
                    float dx = gx - x1;

                    Color c11 = orijinalBmp.GetPixel(x1, y1);
                    Color c21 = orijinalBmp.GetPixel(x2, y1);
                    Color c12 = orijinalBmp.GetPixel(x1, y2);
                    Color c22 = orijinalBmp.GetPixel(x2, y2);

                    int r = (int)(c11.R * (1 - dx) * (1 - dy) + c21.R * dx * (1 - dy) + c12.R * (1 - dx) * dy + c22.R * dx * dy);
                    int gVal = (int)(c11.G * (1 - dx) * (1 - dy) + c21.G * dx * (1 - dy) + c12.G * (1 - dx) * dy + c22.G * dx * dy);
                    int b = (int)(c11.B * (1 - dx) * (1 - dy) + c21.B * dx * (1 - dy) + c12.B * (1 - dx) * dy + c22.B * dx * dy);

                    bmpInterpolasyon.SetPixel(x, y, Color.FromArgb(
                        Math.Max(0, Math.Min(255, r)),
                        Math.Max(0, Math.Min(255, gVal)),
                        Math.Max(0, Math.Min(255, b))
                    ));
                }
            }

            if (picDegistirme != null)
            {
                picDegistirme.Image?.Dispose();
                picDegistirme.SizeMode = PictureBoxSizeMode.Zoom;
                picDegistirme.Image = bmpDegistirme;
            }

            if (picInterpolasyon != null)
            {
                picInterpolasyon.Image?.Dispose();
                picInterpolasyon.SizeMode = PictureBoxSizeMode.Zoom;
                picInterpolasyon.Image = bmpInterpolasyon;
            }

            if (lblBilgi != null)
            {
                lblBilgi.Text = $"Küçültme Oranı: %{yuzde}\n" +
                                $"Orijinal Boyut : {kaynakW}x{kaynakH} px\n" +
                                $"Yeni Boyut     : {hedefW}x{hedefH} px\n" +
                                $"Sol: Orijinal | Orta: Piksel Değiştirme | Sağ: İnterpolasyon";
            }

            this.Cursor = Cursors.Default;
        }

        private void lnkGeri_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta3Odev6_FormClosing;
            Hafta3Odev5 odev5 = new Hafta3Odev5();
            odev5.Show();
            this.Close();
        }

        private void lnkSonraki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta3Odev6_FormClosing;
            Hafta3Odev7 odev7 = new Hafta3Odev7();
            odev7.Show();
            this.Close();
        }
    }
}