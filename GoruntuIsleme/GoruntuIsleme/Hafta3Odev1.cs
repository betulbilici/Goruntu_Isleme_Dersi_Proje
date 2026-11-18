using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace GoruntuIsleme
{
    public partial class Hafta3Odev1 : Form
    {
        private Mat orijinalMat = null;
        private Mat guncelMat = null;

        // Taşıma koordinatları (Resmin Canvas üzerindeki konumu)
        private System.Drawing.Point resimKonum = new System.Drawing.Point(50, 50);

        // a) Tıklayarak taşıma için noktalar
        private System.Drawing.Point? ilkTikNokta = null;
        private System.Drawing.Point? ikinciTikNokta = null;

        // b) Basılı tutarak taşıma (Drag & Drop) kontrolü
        private bool surukleniyor = false;
        private System.Drawing.Point suruklemeBaslangicFare;
        private System.Drawing.Point suruklemeBaslangicResim;

        public Hafta3Odev1()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1250, 750);
            this.StartPosition = FormStartPosition.CenterScreen;

            // X butonu ile güvenli dönüş
            this.FormClosing += Hafta3Odev1_FormClosing;

            // Olay bağlantıları
            if (btnResimSec != null) btnResimSec.Click += btnResimSec_Click;
            if (btnSifirla != null) btnSifirla.Click += btnSifirla_Click;
            if (btnAynala != null) btnAynala.Click += btnAynala_Click;

            // Yalnızca Ödev 2'ye geçiş linki
            if (lnkSonraki != null) lnkSonraki.LinkClicked += lnkSonraki_LinkClicked;

            if (trackAci != null)
            {
                trackAci.Scroll += (s, e) =>
                {
                    if (lblAciDeger != null) lblAciDeger.Text = $"Açı (θ): {trackAci.Value}°";
                    TuvaliCiz();
                };
            }

            if (picCanvas != null)
            {
                picCanvas.MouseDown += PicCanvas_MouseDown;
                picCanvas.MouseMove += PicCanvas_MouseMove;
                picCanvas.MouseUp += PicCanvas_MouseUp;
                picCanvas.Paint += PicCanvas_Paint;
            }
        }

        private void Hafta3Odev1_FormClosing(object sender, FormClosingEventArgs e)
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
                    orijinalMat?.Dispose();
                    guncelMat?.Dispose();

                    orijinalMat = Cv2.ImRead(ofd.FileName, ImreadModes.Color);
                    guncelMat = orijinalMat.Clone();

                    resimKonum = new System.Drawing.Point(50, 50);
                    ilkTikNokta = null;
                    ikinciTikNokta = null;

                    TuvaliCiz();
                }
            }
        }

        private void btnSifirla_Click(object sender, EventArgs e)
        {
            if (orijinalMat == null) return;

            guncelMat?.Dispose();
            guncelMat = orijinalMat.Clone();
            resimKonum = new System.Drawing.Point(50, 50);
            ilkTikNokta = null;
            ikinciTikNokta = null;

            TuvaliCiz();
            if (lblBilgi != null) lblBilgi.Text = "Resim ve konum sıfırlandı.";
        }

        // Tuval Üzerinde Mouse İşlemleri
        private void PicCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (guncelMat == null) return;

            // MOD 1: a) Tıklayarak Resmi Taşıma (2 Nokta Seçimi)
            if (rbTiklayarakTasi != null && rbTiklayarakTasi.Checked)
            {
                if (!ilkTikNokta.HasValue)
                {
                    Rectangle resimAlani = new Rectangle(resimKonum.X, resimKonum.Y, guncelMat.Width, guncelMat.Height);
                    if (resimAlani.Contains(e.Location))
                    {
                        ilkTikNokta = e.Location;
                        if (lblBilgi != null)
                            lblBilgi.Text = $"1. Nokta seçildi: ({e.X}, {e.Y}). Şimdi taşımak istediğiniz 2. konuma tıklayın.";
                        TuvaliCiz();
                    }
                    else
                    {
                        MessageBox.Show("Lütfen ilk noktayı resmin üzerinden seçiniz!", "Uyarı");
                    }
                }
                else
                {
                    ikinciTikNokta = e.Location;

                    int dx = ikinciTikNokta.Value.X - ilkTikNokta.Value.X;
                    int dy = ikinciTikNokta.Value.Y - ilkTikNokta.Value.Y;

                    resimKonum = new System.Drawing.Point(resimKonum.X + dx, resimKonum.Y + dy);

                    if (lblBilgi != null)
                        lblBilgi.Text = $"Resim taşındı! Öteleme: dx={dx}, dy={dy}.";

                    ilkTikNokta = null;
                    ikinciTikNokta = null;
                    TuvaliCiz();
                }
            }
            // MOD 2: b) Basılı Tutup Sürükleyerek Taşıma
            else if (rbSurukleTasi != null && rbSurukleTasi.Checked)
            {
                Rectangle resimAlani = new Rectangle(resimKonum.X, resimKonum.Y, guncelMat.Width, guncelMat.Height);
                if (resimAlani.Contains(e.Location))
                {
                    surukleniyor = true;
                    suruklemeBaslangicFare = e.Location;
                    suruklemeBaslangicResim = resimKonum;
                }
            }
        }

        private void PicCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (surukleniyor && guncelMat != null && rbSurukleTasi.Checked)
            {
                int dx = e.X - suruklemeBaslangicFare.X;
                int dy = e.Y - suruklemeBaslangicFare.Y;

                resimKonum = new System.Drawing.Point(suruklemeBaslangicResim.X + dx, suruklemeBaslangicResim.Y + dy);
                TuvaliCiz();

                if (lblBilgi != null) lblBilgi.Text = $"Taşınıyor... Konum: ({resimKonum.X}, {resimKonum.Y})";
            }
        }

        private void PicCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (surukleniyor)
            {
                surukleniyor = false;
                if (lblBilgi != null) lblBilgi.Text = $"Taşıma tamamlandı: ({resimKonum.X}, {resimKonum.Y})";
            }
        }

        // c) Formül ile Açılı Aynalama (Slayt 2'deki Δ, x2, y2 formülleri)
        private void btnAynala_Click(object sender, EventArgs e)
        {
            if (orijinalMat == null) return;

            this.Cursor = Cursors.WaitCursor;

            int w = orijinalMat.Width;
            int h = orijinalMat.Height;

            double x0 = w / 2.0;
            double y0 = h / 2.0;

            double thetaDerece = trackAci != null ? trackAci.Value : 45.0;
            double thetaRadyan = thetaDerece * Math.PI / 180.0;

            double sinTheta = Math.Sin(thetaRadyan);
            double cosTheta = Math.Cos(thetaRadyan);

            Mat aynalanmisMat = new Mat(h, w, MatType.CV_8UC3, new Scalar(30, 30, 30));

            for (int y2 = 0; y2 < h; y2++)
            {
                for (int x2 = 0; x2 < w; x2++)
                {
                    double delta = ((x2 - x0) * sinTheta) - ((y2 - y0) * cosTheta);

                    int x1 = (int)Math.Round(x2 + (2.0 * delta * (-sinTheta)));
                    int y1 = (int)Math.Round(y2 + (2.0 * delta * (cosTheta)));

                    if (x1 >= 0 && x1 < w && y1 >= 0 && y1 < h)
                    {
                        aynalanmisMat.Set(y2, x2, orijinalMat.At<Vec3b>(y1, x1));
                    }
                }
            }

            guncelMat?.Dispose();
            guncelMat = aynalanmisMat;

            if (lblBilgi != null)
                lblBilgi.Text = $"Formül başarıyla uygulandı! Merkez: ({x0}, {y0}), Açı: {thetaDerece}°";

            TuvaliCiz();
            this.Cursor = Cursors.Default;
        }

        // Tuvalin Çizimi
        private void TuvaliCiz()
        {
            picCanvas.Invalidate();
        }

        private void PicCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (guncelMat != null)
            {
                using (Bitmap bmp = BitmapConverter.ToBitmap(guncelMat))
                {
                    g.DrawImage(bmp, resimKonum.X, resimKonum.Y, guncelMat.Width, guncelMat.Height);
                }

                using (Pen pCerceve = new Pen(Color.LightGray, 1))
                {
                    g.DrawRectangle(pCerceve, resimKonum.X, resimKonum.Y, guncelMat.Width, guncelMat.Height);
                }

                if (rbAynala != null && rbAynala.Checked && trackAci != null)
                {
                    double x0 = resimKonum.X + (guncelMat.Width / 2.0);
                    double y0 = resimKonum.Y + (guncelMat.Height / 2.0);
                    double aciRad = trackAci.Value * Math.PI / 180.0;

                    float dx = (float)(Math.Cos(aciRad) * 200);
                    float dy = (float)(Math.Sin(aciRad) * 200);

                    using (Pen pAyna = new Pen(Color.Cyan, 2) { DashStyle = DashStyle.Dash })
                    {
                        g.DrawLine(pAyna, (float)x0 - dx, (float)y0 - dy, (float)x0 + dx, (float)y0 + dy);
                    }
                }
            }

            if (ilkTikNokta.HasValue)
            {
                using (Brush b = new SolidBrush(Color.Red))
                using (Pen p = new Pen(Color.White, 2))
                {
                    int k = 10;
                    g.FillRectangle(b, ilkTikNokta.Value.X - (k / 2), ilkTikNokta.Value.Y - (k / 2), k, k);
                    g.DrawRectangle(p, ilkTikNokta.Value.X - (k / 2), ilkTikNokta.Value.Y - (k / 2), k, k);
                }
            }
        }

        // Hafta 3 Ödev 2'ye Geçiş Linki
        private void lnkSonraki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta3Odev1_FormClosing;

            Hafta3Odev2 odev2 = new Hafta3Odev2();
            odev2.StartPosition = FormStartPosition.Manual;
            odev2.Location = this.Location;
            odev2.Size = this.Size;
            odev2.FormBorderStyle = this.FormBorderStyle;

            odev2.Show();
            this.Close();
        }
    }
}