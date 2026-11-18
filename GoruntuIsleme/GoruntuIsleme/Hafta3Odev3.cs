using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GoruntuIsleme
{
    public partial class Hafta3Odev3 : Form
    {
        private Bitmap orijinalBmp = null;
        private Bitmap aynalanmisBmp = null;

        // Resmin tuval üzerindeki konumu
        private Point resimKonum = new Point(60, 60);

        // Kullanıcının tuvalde tıkladığı 2 nokta
        private Point? noktaA = null;
        private Point? noktaB = null;

        // Eksen matematiksel değerleri
        private double egim_m = 0.0;
        private double radyanAci = 0.0;
        private double dereceAci = 0.0;

        public Hafta3Odev3()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            this.StartPosition = FormStartPosition.CenterScreen;

            this.FormClosing += Hafta3Odev3_FormClosing;

            // Tasarımdaki boyut ve konumlara dokunmadan sadece olayları bağlıyoruz
            if (btnResimSec != null) btnResimSec.Click += btnResimSec_Click;
            if (btnSifirla != null) btnSifirla.Click += btnSifirla_Click;
            if (btnAynala != null) btnAynala.Click += btnAynala_Click;
            if (lnkGeri != null) lnkGeri.LinkClicked += lnkGeri_LinkClicked;
            if (lnkSonraki != null) lnkSonraki.LinkClicked += lnkSonraki_LinkClicked;

            if (picCanvas != null)
            {
                picCanvas.MouseDown += PicCanvas_MouseDown;
                picCanvas.Paint += PicCanvas_Paint;
            }
        }

        private void Hafta3Odev3_FormClosing(object sender, FormClosingEventArgs e)
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

        // Resim Seçme
        private void btnResimSec_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    orijinalBmp?.Dispose();
                    aynalanmisBmp?.Dispose();
                    aynalanmisBmp = null;

                    using (Bitmap temp = new Bitmap(ofd.FileName))
                    {
                        // Aynalama alanı rahat sığsın diye tuval boyutuna göre ölçekleme
                        int hedefW = temp.Width;
                        int hedefH = temp.Height;
                        if (hedefW > 350 || hedefH > 350)
                        {
                            double oran = Math.Min(300.0 / hedefW, 300.0 / hedefH);
                            hedefW = (int)(hedefW * oran);
                            hedefH = (int)(hedefH * oran);
                        }
                        orijinalBmp = new Bitmap(temp, hedefW, hedefH);
                    }

                    resimKonum = new Point(50, 50);
                    noktaA = null;
                    noktaB = null;

                    if (lblBilgi != null)
                    {
                        lblBilgi.Text = $"Resim yüklendi ({orijinalBmp.Width}x{orijinalBmp.Height}).\nLütfen tuval üzerinde eksen doğrusu için 1. noktaya (A) tıklayınız.";
                    }

                    picCanvas.Invalidate();
                }
            }
        }

        // Sıfırlama
        private void btnSifirla_Click(object sender, EventArgs e)
        {
            aynalanmisBmp?.Dispose();
            aynalanmisBmp = null;
            noktaA = null;
            noktaB = null;

            if (lblBilgi != null)
                lblBilgi.Text = "Sıfırlandı. Yeni 2 nokta seçebilirsiniz.";

            picCanvas.Invalidate();
        }

        // Tuvalde 2 Nokta Seçme (Slayt d)
        private void PicCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            if (!noktaA.HasValue)
            {
                noktaA = e.Location;
                noktaB = null;

                if (lblBilgi != null)
                    lblBilgi.Text = $"A Noktası seçildi: ({noktaA.Value.X}, {noktaA.Value.Y})\nŞimdi eksenin geçeceği 2. noktaya (B) tıklayınız.";
            }
            else if (!noktaB.HasValue)
            {
                if (e.Location == noktaA.Value) return;
                noktaB = e.Location;

                // Slayt d) Doğrunun Eğimi ve Açısı
                double dx = noktaB.Value.X - noktaA.Value.X;
                double dy = noktaB.Value.Y - noktaA.Value.Y;

                egim_m = dx != 0 ? (dy / dx) : double.PositiveInfinity;
                radyanAci = Math.Atan2(dy, dx);
                dereceAci = radyanAci * 180.0 / Math.PI;

                if (lblBilgi != null)
                {
                    lblBilgi.Text = $"Eksen Doğrusu Belirlendi!\n" +
                                    $"• A Noktası: ({noktaA.Value.X}, {noktaA.Value.Y})\n" +
                                    $"• B Noktası: ({noktaB.Value.X}, {noktaB.Value.Y})\n" +
                                    $"• Eğim (m): {(double.IsInfinity(egim_m) ? "Dikey" : egim_m.ToString("F3"))}\n" +
                                    $"• Açı (θ): {dereceAci:F2}°\n" +
                                    $"Şimdi 'Eksen Çizgisine Göre Aynala' butonuna basabilirsiniz.";
                }
            }
            else
            {
                noktaA = e.Location;
                noktaB = null;
                aynalanmisBmp?.Dispose();
                aynalanmisBmp = null;

                if (lblBilgi != null)
                    lblBilgi.Text = $"Yeni A Noktası seçildi: ({noktaA.Value.X}, {noktaA.Value.Y})\nB noktasını seçiniz.";
            }

            picCanvas.Invalidate();
        }

        // Slaytlardaki Formülü Tüm Tuval Boyutunda Kayıpsız Çalıştırma
        private void btnAynala_Click(object sender, EventArgs e)
        {
            if (orijinalBmp == null || !noktaA.HasValue || !noktaB.HasValue)
            {
                MessageBox.Show("Lütfen bir resim yükleyip eksen için 2 noktaya tıklayınız!", "Uyarı");
                return;
            }

            this.Cursor = Cursors.WaitCursor;

            int cw = picCanvas.Width;
            int ch = picCanvas.Height;

            aynalanmisBmp?.Dispose();
            aynalanmisBmp = new Bitmap(cw, ch);

            // Eksen doğrusu üzerindeki referans nokta (Tuval koordinatında x0, y0)
            double x0 = noktaA.Value.X;
            double y0 = noktaA.Value.Y;

            double sinTheta = Math.Sin(radyanAci);
            double cosTheta = Math.Cos(radyanAci);

            int imgW = orijinalBmp.Width;
            int imgH = orijinalBmp.Height;

            // Slayttaki Formül:
            // Delta = (x1 - x0)*sin(θ) - (y1 - y0)*cos(θ)
            // x2 = x1 + 2*Delta*(-sin(θ))
            // y2 = y1 + 2*Delta*(cos(θ))
            // Tuvalin tamamı taranarak piksel kaybı engellenir (Backward Mapping)
            for (int y2 = 0; y2 < ch; y2++)
            {
                for (int x2 = 0; x2 < cw; x2++)
                {
                    double delta = ((x2 - x0) * sinTheta) - ((y2 - y0) * cosTheta);

                    int x1 = (int)Math.Round(x2 + (2.0 * delta * (-sinTheta)));
                    int y1 = (int)Math.Round(y2 + (2.0 * delta * cosTheta));

                    int yerelX = x1 - resimKonum.X;
                    int yerelY = y1 - resimKonum.Y;

                    if (yerelX >= 0 && yerelX < imgW && yerelY >= 0 && yerelY < imgH)
                    {
                        aynalanmisBmp.SetPixel(x2, y2, orijinalBmp.GetPixel(yerelX, yerelY));
                    }
                }
            }

            if (lblBilgi != null)
            {
                lblBilgi.Text = $"Aynalama Tamamlandı!\n" +
                                $"Formül: Δ = (x - x0)sinθ - (y - y0)cosθ\n" +
                                $"Referans: ({x0}, {y0})\n" +
                                $"Eksen Açısı: {dereceAci:F2}°";
            }

            picCanvas.Invalidate();
            this.Cursor = Cursors.Default;
        }

        // Çizim Metodu (Orijinal Resim, Aynalanmış Resim, Eksen ve A-B Noktaları)
        private void PicCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // 1. Orijinal Resim (Slayttaki kartal gibi yerinde durur)
            if (orijinalBmp != null)
            {
                g.DrawImage(orijinalBmp, resimKonum.X, resimKonum.Y, orijinalBmp.Width, orijinalBmp.Height);
                using (Pen p = new Pen(Color.LightGray, 1))
                {
                    g.DrawRectangle(p, resimKonum.X, resimKonum.Y, orijinalBmp.Width, orijinalBmp.Height);
                }
            }

            // 2. Aynalanmış Resim (Tuval üstünde eksiksiz görünür)
            if (aynalanmisBmp != null)
            {
                g.DrawImage(aynalanmisBmp, 0, 0);
            }

            // 3. Eksen Doğrusu (Kesikli Çizgi)
            if (noktaA.HasValue && noktaB.HasValue)
            {
                float dx = noktaB.Value.X - noktaA.Value.X;
                float dy = noktaB.Value.Y - noktaA.Value.Y;
                float hipo = (float)Math.Sqrt(dx * dx + dy * dy);

                if (hipo > 0)
                {
                    float ux = dx / hipo;
                    float uy = dy / hipo;

                    PointF baslangic = new PointF(noktaA.Value.X - (ux * 1000), noktaA.Value.Y - (uy * 1000));
                    PointF bitis = new PointF(noktaA.Value.X + (ux * 1000), noktaA.Value.Y + (uy * 1000));

                    using (Pen pEksen = new Pen(Color.FromArgb(0, 180, 240), 2.5f) { DashStyle = DashStyle.Dash })
                    {
                        g.DrawLine(pEksen, baslangic, bitis);
                    }
                }
            }

            // 4. A Noktası (Kırmızı Kare)
            if (noktaA.HasValue)
            {
                int k = 10;
                using (Brush b = new SolidBrush(Color.Red))
                using (Pen p = new Pen(Color.White, 2))
                {
                    g.FillRectangle(b, noktaA.Value.X - k / 2, noktaA.Value.Y - k / 2, k, k);
                    g.DrawRectangle(p, noktaA.Value.X - k / 2, noktaA.Value.Y - k / 2, k, k);
                }
                g.DrawString("A (x1, y1)", new Font("Segoe UI", 9, FontStyle.Bold), Brushes.Red, noktaA.Value.X + 8, noktaA.Value.Y - 8);
            }

            // 5. B Noktası (Mavi Kare)
            if (noktaB.HasValue)
            {
                int k = 10;
                using (Brush b = new SolidBrush(Color.Blue))
                using (Pen p = new Pen(Color.White, 2))
                {
                    g.FillRectangle(b, noktaB.Value.X - k / 2, noktaB.Value.Y - k / 2, k, k);
                    g.DrawRectangle(p, noktaB.Value.X - k / 2, noktaB.Value.Y - k / 2, k, k);
                }
                g.DrawString("B (x2, y2)", new Font("Segoe UI", 9, FontStyle.Bold), Brushes.Blue, noktaB.Value.X + 8, noktaB.Value.Y - 8);
            }
        }

        private void lnkGeri_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta3Odev3_FormClosing;

            Hafta3Odev2 odev2 = new Hafta3Odev2();
            odev2.StartPosition = FormStartPosition.Manual;
            odev2.Location = this.Location;
            odev2.Size = this.Size;
            odev2.FormBorderStyle = this.FormBorderStyle;

            odev2.Show();
            this.Close();
        }

        private void lnkSonraki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta3Odev3_FormClosing;

            Hafta3Odev4 odev4 = new Hafta3Odev4();
            odev4.StartPosition = FormStartPosition.Manual;
            odev4.Location = this.Location;
            odev4.Size = this.Size;
            odev4.FormBorderStyle = this.FormBorderStyle;

            odev4.Show();
            this.Close();
        }
    }
}