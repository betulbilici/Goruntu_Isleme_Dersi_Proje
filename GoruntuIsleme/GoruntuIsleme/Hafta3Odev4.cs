using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GoruntuIsleme
{
    public partial class Hafta3Odev4 : Form
    {
        private Bitmap orijinalBmp = null;

        public Hafta3Odev4()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1250, 750);
            this.StartPosition = FormStartPosition.CenterScreen;

            this.FormClosing += Hafta3Odev4_FormClosing;

            if (btnResimSec != null) btnResimSec.Click += btnResimSec_Click;
            if (btnBuyut != null) btnBuyut.Click += btnBuyut_Click;
            if (lnkGeri != null) lnkGeri.LinkClicked += lnkGeri_LinkClicked;
            if (lnkSonraki != null) lnkSonraki.LinkClicked += lnkSonraki_LinkClicked;

            if (lblBilgi != null)
            {
                lblBilgi.Text = "1. Küçük bir resim seçin.\n\n" +
                                "2. Büyütme oranını seçip (örn: 2x veya 5x) 'Resmi Büyüt' butonuna basın.\n\n" +
                                "3. Piksel değiştirme ile interpolasyon arasındaki farkı inceleyin.";
            }
        }

        private void Hafta3Odev4_FormClosing(object sender, FormClosingEventArgs e)
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
                        // Slayttaki piksel bloklarını net görebilmek için orijinali küçük bir boyuta alıyoruz (örn: 80x95 px civarı)
                        int maxBoyut = 90;
                        if (temp.Width > maxBoyut || temp.Height > maxBoyut)
                        {
                            double oran = Math.Min((double)maxBoyut / temp.Width, (double)maxBoyut / temp.Height);
                            int yeniW = Math.Max(1, (int)(temp.Width * oran));
                            int yeniH = Math.Max(1, (int)(temp.Height * oran));
                            orijinalBmp = new Bitmap(temp, yeniW, yeniH);
                        }
                        else
                        {
                            orijinalBmp = new Bitmap(temp);
                        }
                    }

                    if (picOrijinal != null)
                    {
                        picOrijinal.Image?.Dispose();
                        picOrijinal.SizeMode = PictureBoxSizeMode.CenterImage;
                        picOrijinal.Image = new Bitmap(orijinalBmp);
                    }

                    if (lblBilgi != null)
                    {
                        lblBilgi.Text = $"Resim Yüklendi:\n" +
                                        $"Orijinal Boyut: {orijinalBmp.Width}x{orijinalBmp.Height} px\n\n" +
                                        $"Büyütme oranını seçip butona basabilirsiniz.";
                    }
                }
            }
        }

        private void btnBuyut_Click(object sender, EventArgs e)
        {
            if (orijinalBmp == null)
            {
                MessageBox.Show("Lütfen önce bir resim seçiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int olcek = (int)numOran.Value;
            int kaynakW = orijinalBmp.Width;
            int kaynakH = orijinalBmp.Height;

            int hedefW = kaynakW * olcek;
            int hedefH = kaynakH * olcek;

            this.Cursor = Cursors.WaitCursor;

            // --- YÖNTEM A: PİKSEL DEĞİŞTİRME (SLAYT a - NEAREST NEIGHBOR / BLOK KOPYALAMA) ---
            // Slayttaki [2, 2, 5, 5] mantığı: Her piksel olcek kadar yatay ve dikey çoğaltılır
            Bitmap bmpDegistirme = new Bitmap(hedefW, hedefH);
            for (int y = 0; y < hedefH; y++)
            {
                int srcY = Math.Min(y / olcek, kaynakH - 1);
                for (int x = 0; x < hedefW; x++)
                {
                    int srcX = Math.Min(x / olcek, kaynakW - 1);
                    bmpDegistirme.SetPixel(x, y, orijinalBmp.GetPixel(srcX, srcY));
                }
            }

            // --- YÖNTEM B: PİKSEL İNTERPOLASYONU (SLAYT b - BİLİNEER / KADEMELİ GEÇİŞ) ---
            Bitmap bmpInterpolasyon = new Bitmap(hedefW, hedefH);
            for (int y = 0; y < hedefH; y++)
            {
                float gy = (float)y / olcek;
                int y1 = (int)Math.Floor(gy);
                int y2 = Math.Min(y1 + 1, kaynakH - 1);
                float dy = gy - y1;

                for (int x = 0; x < hedefW; x++)
                {
                    float gx = (float)x / olcek;
                    int x1 = (int)Math.Floor(gx);
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

            // --- PICTUREBOX GÖRÜNÜMÜ: ORAN FARKLILIKLARINI GÖZLE GÖRÜLEBİLİR KILMA ---
            // CenterImage kullanarak 2x iken kutu içinde küçük, 5x iken kutuyu tamamen dolduran gerçek boyut gösterimi
            if (picDegistirme != null)
            {
                picDegistirme.Image?.Dispose();
                picDegistirme.SizeMode = PictureBoxSizeMode.CenterImage;
                picDegistirme.Image = bmpDegistirme;
                picDegistirme.Refresh();
            }

            if (picInterpolasyon != null)
            {
                picInterpolasyon.Image?.Dispose();
                picInterpolasyon.SizeMode = PictureBoxSizeMode.CenterImage;
                picInterpolasyon.Image = bmpInterpolasyon;
                picInterpolasyon.Refresh();
            }

            if (lblBilgi != null)
            {
                lblBilgi.Text = $"İşlem Tamamlandı!\n\n" +
                                $"• Orijinal Boyut : {kaynakW}x{kaynakH} px\n" +
                                $"• Büyütülmüş     : {hedefW}x{hedefH} px ({olcek}x)\n\n" +
                                $"Ortadaki Resim : Piksel Değiştirme (Bloklu / Keskin Pikseller)\n\n" +
                                $"Sağdaki Resim  : İnterpolasyon (Yumuşak Geçiş / Anti-aliasing)";
            }

            this.Cursor = Cursors.Default;
        }

        private void lnkGeri_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta3Odev4_FormClosing;

            Hafta3Odev3 odev3 = new Hafta3Odev3();
            odev3.StartPosition = FormStartPosition.Manual;
            odev3.Location = this.Location;
            odev3.Size = this.Size;
            odev3.FormBorderStyle = this.FormBorderStyle;

            odev3.Show();
            this.Close();
        }

        private void lnkSonraki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta3Odev4_FormClosing;

            Hafta3Odev5 odev5 = new Hafta3Odev5();
            odev5.StartPosition = FormStartPosition.Manual;
            odev5.Location = this.Location;
            odev5.Size = this.Size;
            odev5.FormBorderStyle = this.FormBorderStyle;
            odev5.Show();
            this.Close();
        }
    }
}