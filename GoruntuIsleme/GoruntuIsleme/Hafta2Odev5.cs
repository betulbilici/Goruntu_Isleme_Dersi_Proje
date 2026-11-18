using System;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace GoruntuIsleme
{
    public partial class Hafta2Odev5 : Form
    {
        private Mat orijinalMat = null;
        private int[] histR = new int[256];
        private int[] histG = new int[256];
        private int[] histB = new int[256];

        public Hafta2Odev5()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1250, 750);
            this.StartPosition = FormStartPosition.CenterScreen;

            // X butonu ile güvenli kapatma / ana forma dönüş
            this.FormClosing += Hafta2Odev5_FormClosing;

            // Başlangıç değerleri
            if (txtC != null && string.IsNullOrEmpty(txtC.Text)) txtC.Text = "50";
            if (txtRMin != null) txtRMin.Text = "0";
            if (txtRMax != null) txtRMax.Text = "255";
            if (txtGMin != null) txtGMin.Text = "0";
            if (txtGMax != null) txtGMax.Text = "255";
            if (txtBMin != null) txtBMin.Text = "0";
            if (txtBMax != null) txtBMax.Text = "255";

            // Buton ve Link olay bağlantıları
            if (btnResimSec != null) btnResimSec.Click += btnResimSec_Click;
            if (btnKontrastUygula != null) btnKontrastUygula.Click += btnKontrastUygula_Click;
            if (lnkGeri != null) lnkGeri.LinkClicked += lnkGeri_LinkClicked;
            if (lnkSonraki != null) lnkSonraki.LinkClicked += lnkSonraki_LinkClicked;

            // Histogram fare tıklama olayları (Sol Tık: Min, Sağ Tık: Max)
            if (picHistR != null) picHistR.MouseDown += (s, e) => HistMouseDown(e, txtRMin, txtRMax, 0);
            if (picHistG != null) picHistG.MouseDown += (s, e) => HistMouseDown(e, txtGMin, txtGMax, 1);
            if (picHistB != null) picHistB.MouseDown += (s, e) => HistMouseDown(e, txtBMin, txtBMax, 2);
        }

        // X butonuna basıldığında Form1'e dönen metot
        private void Hafta2Odev5_FormClosing(object sender, FormClosingEventArgs e)
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
                    orijinalMat?.Dispose();
                    orijinalMat = Cv2.ImRead(ofd.FileName, ImreadModes.Color);

                    if (orijinalMat.Empty())
                    {
                        MessageBox.Show("Resim yüklenemedi!");
                        return;
                    }

                    picOrijinal.Image?.Dispose();
                    picOrijinal.Image = BitmapConverter.ToBitmap(orijinalMat);

                    HistogramlariHesaplaVeCiz();
                }
            }
        }

        // Histogram Kutusu Üzerinde Mouse Tıklamaları (Sol: Min, Sağ: Max)
        private void HistMouseDown(MouseEventArgs e, TextBox txtMin, TextBox txtMax, int kanalIdx)
        {
            PictureBox pBox = (kanalIdx == 0) ? picHistR : (kanalIdx == 1 ? picHistG : picHistB);
            if (orijinalMat == null || pBox.Width <= 0) return;

            // X koordinatını 0-255 renk aralığına ölçekleme
            int val = (int)((float)e.X / pBox.Width * 255f);
            if (val < 0) val = 0;
            if (val > 255) val = 255;

            if (e.Button == MouseButtons.Left)
                txtMin.Text = val.ToString();
            else if (e.Button == MouseButtons.Right)
                txtMax.Text = val.ToString();

            TekHistogramCiz(pBox, kanalIdx == 0 ? histR : (kanalIdx == 1 ? histG : histB),
                            kanalIdx == 0 ? Color.Red : (kanalIdx == 1 ? Color.Lime : Color.DeepSkyBlue),
                            txtMin, txtMax);
        }

        // R, G, B kanallarının ayrı ayrı histogramlarını hesapla
        private void HistogramlariHesaplaVeCiz()
        {
            if (orijinalMat == null) return;
            Array.Clear(histR, 0, 256);
            Array.Clear(histG, 0, 256);
            Array.Clear(histB, 0, 256);

            for (int y = 0; y < orijinalMat.Height; y++)
            {
                for (int x = 0; x < orijinalMat.Width; x++)
                {
                    Vec3b c = orijinalMat.At<Vec3b>(y, x);
                    histB[c.Item0]++;
                    histG[c.Item1]++;
                    histR[c.Item2]++;
                }
            }

            TekHistogramCiz(picHistR, histR, Color.Red, txtRMin, txtRMax);
            TekHistogramCiz(picHistG, histG, Color.Lime, txtGMin, txtGMax);
            TekHistogramCiz(picHistB, histB, Color.DeepSkyBlue, txtBMin, txtBMax);
        }

        // Tek bir kanalın histogram grafiğini ve sınırlarını çizme
        private void TekHistogramCiz(PictureBox pb, int[] hist, Color renk, TextBox tMin, TextBox tMax)
        {
            if (pb == null || pb.Width <= 0 || pb.Height <= 0) return;

            int w = pb.Width;
            int h = pb.Height;
            Bitmap bmp = new Bitmap(w, h);

            int max = 1;
            for (int i = 0; i < 256; i++)
            {
                if (hist[i] > max) max = hist[i];
            }

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Black);

                using (Pen p = new Pen(renk, 1))
                {
                    for (int i = 0; i < 256; i++)
                    {
                        float x = (float)i / 255f * (w - 1);
                        float barH = ((float)hist[i] / max) * (h - 10);
                        g.DrawLine(p, x, h - 1, x, h - 1 - barH);
                    }
                }

                // 50'lik referans dikey çizgileri (Slaytta belirtilen çizgiler)
                using (Pen pRef = new Pen(Color.FromArgb(70, 70, 70), 1))
                {
                    for (int i = 50; i <= 250; i += 50)
                    {
                        float xRef = (float)i / 255f * (w - 1);
                        g.DrawLine(pRef, xRef, 0, xRef, h);
                    }
                }

                // Seçilen sınır dikey çizgileri
                int.TryParse(tMin?.Text, out int minVal);
                int.TryParse(tMax?.Text, out int maxVal);
                using (Pen pLimit = new Pen(Color.White, 1))
                {
                    float xMin = (float)minVal / 255f * (w - 1);
                    float xMax = (float)maxVal / 255f * (w - 1);
                    g.DrawLine(pLimit, xMin, 0, xMin, h);
                    g.DrawLine(pLimit, xMax, 0, xMax, h);
                }
            }

            pb.Image?.Dispose();
            pb.Image = bmp;
        }

        // Kontrast Uygulama Butonu
        private void btnKontrastUygula_Click(object sender, EventArgs e)
        {
            if (orijinalMat == null) return;

            // -------------------------------------------------------------
            // YÖNTEM A: 6 Sınır Değeriyle Bağımsız Kanal Germe (a şıkkı)
            // -------------------------------------------------------------
            int.TryParse(txtRMin?.Text, out int rMin); int.TryParse(txtRMax?.Text, out int rMax);
            int.TryParse(txtGMin?.Text, out int gMin); int.TryParse(txtGMax?.Text, out int gMax);
            int.TryParse(txtBMin?.Text, out int bMin); int.TryParse(txtBMax?.Text, out int bMax);

            if (rMin >= rMax || gMin >= gMax || bMin >= bMax)
            {
                MessageBox.Show("Seçilen minimum değerler maksimum değerlerden küçük olmalıdır!");
                return;
            }

            Mat sonuc1 = new Mat(orijinalMat.Size(), MatType.CV_8UC3);
            for (int y = 0; y < orijinalMat.Height; y++)
            {
                for (int x = 0; x < orijinalMat.Width; x++)
                {
                    Vec3b bgr = orijinalMat.At<Vec3b>(y, x);

                    byte b = GermeUygula(bgr.Item0, bMin, bMax);
                    byte g = GermeUygula(bgr.Item1, gMin, gMax);
                    byte r = GermeUygula(bgr.Item2, rMin, rMax);

                    sonuc1.Set(y, x, new Vec3b(b, g, r));
                }
            }
            picSonuc1.Image?.Dispose();
            picSonuc1.Image = BitmapConverter.ToBitmap(sonuc1);
            sonuc1.Dispose();

            // -------------------------------------------------------------
            // YÖNTEM B: HOCANIN SLAYTINDAKİ HAZIR FORMÜL (b şıkkı)
            // F = (259 * (C + 255)) / (255 * (259 - C))
            // -------------------------------------------------------------
            double C_KontrastSeviyesi = 50;
            if (txtC != null) double.TryParse(txtC.Text, out C_KontrastSeviyesi);

            if (C_KontrastSeviyesi < -255) C_KontrastSeviyesi = -255;
            if (C_KontrastSeviyesi > 255) C_KontrastSeviyesi = 255;

            // Slayttaki F Kontrast Faktörü formülü:
            double F_KontrastFaktoru = (259.0 * (C_KontrastSeviyesi + 255.0)) / (255.0 * (259.0 - C_KontrastSeviyesi));

            Mat sonuc2 = new Mat(orijinalMat.Size(), MatType.CV_8UC3);

            for (int y = 0; y < orijinalMat.Height; y++)
            {
                for (int x = 0; x < orijinalMat.Width; x++)
                {
                    Vec3b okunanRenk = orijinalMat.At<Vec3b>(y, x);

                    // Slayttaki 128 etrafında öteleme formülü:
                    int B = (int)((F_KontrastFaktoru * (okunanRenk.Item0 - 128)) + 128);
                    int G = (int)((F_KontrastFaktoru * (okunanRenk.Item1 - 128)) + 128);
                    int R = (int)((F_KontrastFaktoru * (okunanRenk.Item2 - 128)) + 128);

                    // Slayttaki 0-255 aralığına kırpma kontrolleri:
                    if (R > 255) R = 255;
                    if (G > 255) G = 255;
                    if (B > 255) B = 255;

                    if (R < 0) R = 0;
                    if (G < 0) G = 0;
                    if (B < 0) B = 0;

                    sonuc2.Set(y, x, new Vec3b((byte)B, (byte)G, (byte)R));
                }
            }

            picSonuc2.Image?.Dispose();
            picSonuc2.Image = BitmapConverter.ToBitmap(sonuc2);
            sonuc2.Dispose();

            // -------------------------------------------------------------
            // SLAYTTAKİ YORUMLAMA METNİ (Slayt metnine birebir uygun akademik rapor)
            // -------------------------------------------------------------
            if (txtYorum != null)
            {
                txtYorum.Text = "HOCANIN DERS NOTLARINA GÖRE KARŞILAŞTIRMA VE YORUM:\r\n\r\n" +
                    "1. Histogram Sınırları ile Ayrı Kanal Germe (a Şıkkı):\r\n" +
                    "Histogram üzerinden R, G ve B renklerinin yığıldığı gerçek sınırlar seçilerek uygulandığında resim detayları daha iyi korunur. Fakat üç kanal birbirinden bağımsız oranlarda gerdirildiğinde pikseller arasındaki orijinal renk dengesi bozulabilir ve hafif renk sapmaları (renk tonu kayması) gözlemlenebilir.\r\n\r\n" +
                    "2. Hazır Formül Kullanımı (b Şıkkı - Slayt Formülü):\r\n" +
                    "Bu yöntemde temel mantık 128 değerinin üzerindeki renkleri yukarı 255'e, altındaki renkleri 0'a doğru ötelemektir. F katsayısı öteleme miktarını belirler. Pratik bir yöntemdir fakat renk değerleri 0'ın altına veya 255'in üzerine taştığında sınırlarda kırpma (clipping) yapıldığı için aşırı kontrast durumunda siyah ve beyaz alanlar belirgin şekilde artar ve resim doğallığını kaybedebilir.\r\n\r\n" +
                    "Sonuç olarak: Histogram yöntemi hangi sınırların nereye çekileceğini görerek karar vermeyi sağladığı için kontrollü kontrastta daha başarılı sonuç verir; formül yöntemi ise hızlı ve pratik uygulamalar için uygundur.";
            }
        }

        // Doğrusal aralık germe formülü: (deger - min) * 255 / (max - min)
        private byte GermeUygula(byte val, int min, int max)
        {
            if (val <= min) return 0;
            if (val >= max) return 255;
            return (byte)(((float)(val - min) / (max - min)) * 255f);
        }

        // Ödev 4'e Geri Dön Linki
        private void lnkGeri_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta2Odev5_FormClosing;

            Hafta2Odev4 odev4 = new Hafta2Odev4();
            odev4.StartPosition = FormStartPosition.Manual;
            odev4.Location = this.Location;
            odev4.Size = this.Size;
            odev4.FormBorderStyle = this.FormBorderStyle;

            odev4.Show();
            this.Close();
        }

        // Ödev 6'ya Git Linki
        // Ödev 6'ya Geçiş Linki
        private void lnkSonraki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Form1'in arka planda açılmasını engellemek için FormClosing aboneliğini kaldırıyoruz:
            this.FormClosing -= Hafta2Odev5_FormClosing;

            Hafta2Odev6 odev6 = new Hafta2Odev6();
            odev6.StartPosition = FormStartPosition.Manual;
            odev6.Location = this.Location;
            odev6.Size = this.Size;
            odev6.FormBorderStyle = this.FormBorderStyle;

            odev6.Show();
            this.Close();
        }
    }
}
