using System;
using System.Drawing;
using System.Windows.Forms;

namespace GoruntuIsleme
{
    public partial class Hafta3Odev5 : Form
    {
        private Bitmap orijinalBmp = null;
        private Bitmap kirpilanBmp = null;

        private Point? nokta1 = null;
        private Point? nokta2 = null;
        private bool fareSurukleniyor = false;

        public Hafta3Odev5()
        {
            InitializeComponent();
            this.FormClosing += Hafta3Odev5_FormClosing;

            if (btnResimSec != null) btnResimSec.Click += btnResimSec_Click;
            if (btnKirp != null) btnKirp.Click += btnKirp_Click;
            if (btnSifirla != null) btnSifirla.Click += btnSifirla_Click;
            if (lnkGeri != null) lnkGeri.LinkClicked += lnkGeri_LinkClicked;
            if (lnkSonraki != null) lnkSonraki.LinkClicked += lnkSonraki_LinkClicked;

            if (picKaynak != null)
            {
                picKaynak.MouseDown += PicKaynak_MouseDown;
                picKaynak.MouseMove += PicKaynak_MouseMove;
                picKaynak.MouseUp += PicKaynak_MouseUp;
                picKaynak.Paint += PicKaynak_Paint;
            }
        }

        private void Hafta3Odev5_FormClosing(object sender, FormClosingEventArgs e)
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
                    kirpilanBmp?.Dispose();
                    kirpilanBmp = null;
                    nokta1 = null;
                    nokta2 = null;

                    using (Bitmap temp = new Bitmap(ofd.FileName))
                    {
                        orijinalBmp = new Bitmap(temp);
                    }

                    if (picKirpilan != null) picKirpilan.Image = null;
                    picKaynak.Invalidate();
                }
            }
        }

        private void btnSifirla_Click(object sender, EventArgs e)
        {
            nokta1 = null;
            nokta2 = null;
            kirpilanBmp?.Dispose();
            kirpilanBmp = null;

            if (picKirpilan != null) picKirpilan.Image = null;
            picKaynak.Invalidate();
        }

        private void PicKaynak_MouseDown(object sender, MouseEventArgs e)
        {
            if (orijinalBmp == null || e.Button != MouseButtons.Left) return;

            int x = Math.Max(0, Math.Min(e.X, orijinalBmp.Width));
            int y = Math.Max(0, Math.Min(e.Y, orijinalBmp.Height));

            nokta1 = new Point(x, y);
            nokta2 = nokta1;
            fareSurukleniyor = true;

            picKaynak.Invalidate();
        }

        private void PicKaynak_MouseMove(object sender, MouseEventArgs e)
        {
            if (!fareSurukleniyor || nokta1 == null || orijinalBmp == null) return;

            int x = Math.Max(0, Math.Min(e.X, orijinalBmp.Width));
            int y = Math.Max(0, Math.Min(e.Y, orijinalBmp.Height));

            nokta2 = new Point(x, y);
            picKaynak.Invalidate();
        }

        private void PicKaynak_MouseUp(object sender, MouseEventArgs e)
        {
            if (!fareSurukleniyor || orijinalBmp == null) return;

            fareSurukleniyor = false;

            int x = Math.Max(0, Math.Min(e.X, orijinalBmp.Width));
            int y = Math.Max(0, Math.Min(e.Y, orijinalBmp.Height));
            nokta2 = new Point(x, y);

            Rectangle secim = SecimDikdortgeniAl();
            if (lblBilgi != null && secim.Width > 0 && secim.Height > 0)
            {
                lblBilgi.Text = $"Seçim Alanı: X={secim.X}, Y={secim.Y}, Genişlik={secim.Width}, Yükseklik={secim.Height}";
            }

            picKaynak.Invalidate();
        }

        private Rectangle SecimDikdortgeniAl()
        {
            if (!nokta1.HasValue || !nokta2.HasValue) return Rectangle.Empty;

            int x = Math.Min(nokta1.Value.X, nokta2.Value.X);
            int y = Math.Min(nokta1.Value.Y, nokta2.Value.Y);
            int w = Math.Abs(nokta1.Value.X - nokta2.Value.X);
            int h = Math.Abs(nokta1.Value.Y - nokta2.Value.Y);

            return new Rectangle(x, y, w, h);
        }

        private void btnKirp_Click(object sender, EventArgs e)
        {
            if (orijinalBmp == null) return;

            Rectangle secim = SecimDikdortgeniAl();
            if (secim.Width < 5 || secim.Height < 5) return;

            this.Cursor = Cursors.WaitCursor;

            kirpilanBmp?.Dispose();
            kirpilanBmp = new Bitmap(secim.Width, secim.Height);

            for (int y = 0; y < secim.Height; y++)
            {
                for (int x = 0; x < secim.Width; x++)
                {
                    Color piksel = orijinalBmp.GetPixel(secim.X + x, secim.Y + y);
                    kirpilanBmp.SetPixel(x, y, piksel);
                }
            }

            if (picKirpilan != null)
            {
                picKirpilan.Image?.Dispose();
                picKirpilan.Image = new Bitmap(kirpilanBmp);
            }

            if (lblBilgi != null)
            {
                lblBilgi.Text = $"Kırpıldı: {secim.Width}x{secim.Height} px (Konum: {secim.X}, {secim.Y})";
            }

            this.Cursor = Cursors.Default;
        }

        private void PicKaynak_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            if (orijinalBmp != null)
            {
                g.DrawImage(orijinalBmp, 0, 0, orijinalBmp.Width, orijinalBmp.Height);
            }

            Rectangle secim = SecimDikdortgeniAl();
            if (secim.Width > 0 && secim.Height > 0)
            {
                using (Brush b = new SolidBrush(Color.FromArgb(40, Color.Red)))
                {
                    g.FillRectangle(b, secim);
                }
                using (Pen p = new Pen(Color.Red, 2f))
                {
                    g.DrawRectangle(p, secim);
                }
            }
        }

        private void lnkGeri_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta3Odev5_FormClosing;
            Hafta3Odev4 odev4 = new Hafta3Odev4();
            odev4.Show();
            this.Close();
        }

        private void lnkSonraki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta3Odev5_FormClosing;
            Hafta3Odev6 odev6 = new Hafta3Odev6();
            odev6.Show();
            this.Close();
        }
    }
}