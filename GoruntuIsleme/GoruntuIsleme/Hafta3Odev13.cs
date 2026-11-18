using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace GoruntuIsleme
{
    public partial class Hafta3Odev13 : Form
    {
        private Bitmap orijinalBmp = null;
        private Bitmap kirpilanBmp = null;

        // Mouse ile seçim takibi değişkenleri
        private bool fareBasildi = false;
        private Point baslangicNoktasi;
        private Rectangle secimDikdortgeni = Rectangle.Empty;

        public Hafta3Odev13()
        {
            InitializeComponent();
            this.FormClosing += Hafta3Odev13_FormClosing;

            if (btnResimSec != null) btnResimSec.Click += btnResimSec_Click;
            if (btnKirp != null) btnKirp.Click += btnKirp_Click;
            if (btnSifirla != null) btnSifirla.Click += btnSifirla_Click;
            if (lnkGeri != null) lnkGeri.LinkClicked += lnkGeri_LinkClicked;
            if (lnkSonraki != null) lnkSonraki.LinkClicked += lnkSonraki_LinkClicked;

            // Mouse olaylarını picOrijinal üzerine bağlıyoruz
            if (picOrijinal != null)
            {
                picOrijinal.MouseDown += picOrijinal_MouseDown;
                picOrijinal.MouseMove += picOrijinal_MouseMove;
                picOrijinal.MouseUp += picOrijinal_MouseUp;
                picOrijinal.Paint += picOrijinal_Paint;
            }
        }

        private void Hafta3Odev13_FormClosing(object sender, FormClosingEventArgs e)
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
                    secimDikdortgeni = Rectangle.Empty;

                    using (Bitmap temp = new Bitmap(ofd.FileName))
                    {
                        orijinalBmp = new Bitmap(temp);
                    }

                    if (picOrijinal != null)
                    {
                        picOrijinal.Image?.Dispose();
                        picOrijinal.SizeMode = PictureBoxSizeMode.Zoom;
                        picOrijinal.Image = new Bitmap(orijinalBmp);
                    }

                    if (picKirpilan != null) picKirpilan.Image = null;

                    if (lblBilgi != null)
                    {
                        lblBilgi.Text = $"Resim Yüklendi: {orijinalBmp.Width}x{orijinalBmp.Height} px. " +
                                        $"Mouse sol tuşuna basılı tutarak kırpmak istediğiniz alanı seçiniz.";
                    }
                }
            }
        }

        private void btnSifirla_Click(object sender, EventArgs e)
        {
            secimDikdortgeni = Rectangle.Empty;
            kirpilanBmp?.Dispose();
            kirpilanBmp = null;

            if (picKirpilan != null) picKirpilan.Image = null;
            if (picOrijinal != null) picOrijinal.Invalidate();

            if (lblBilgi != null)
            {
                lblBilgi.Text = "Seçim ve kırpılan görüntü temizlendi.";
            }
        }

        // --- MOUSE İLE ÇİZGİ ÇİZDİREREK SEÇİM ALANI BELİRLEME ---

        private void picOrijinal_MouseDown(object sender, MouseEventArgs e)
        {
            if (orijinalBmp == null || e.Button != MouseButtons.Left) return;

            fareBasildi = true;
            baslangicNoktasi = e.Location;
            secimDikdortgeni = new Rectangle(e.X, e.Y, 0, 0);
            picOrijinal.Invalidate();
        }

        private void picOrijinal_MouseMove(object sender, MouseEventArgs e)
        {
            if (!fareBasildi || orijinalBmp == null) return;

            // Her yöne çekilebilen dikdörtgen hesabı
            int x = Math.Min(baslangicNoktasi.X, e.X);
            int y = Math.Min(baslangicNoktasi.Y, e.Y);
            int genislik = Math.Abs(baslangicNoktasi.X - e.X);
            int yukseklik = Math.Abs(baslangicNoktasi.Y - e.Y);

            secimDikdortgeni = new Rectangle(x, y, genislik, yukseklik);
            picOrijinal.Invalidate(); // Çizgiyi anlık güncellemek için Paint'i tetikler
        }

        private void picOrijinal_MouseUp(object sender, MouseEventArgs e)
        {
            if (!fareBasildi || orijinalBmp == null) return;

            fareBasildi = false;

            if (secimDikdortgeni.Width > 5 && secimDikdortgeni.Height > 5)
            {
                // Mouse bırakıldığında doğrudan kırpma işlemini de otomatik tetikleyebiliriz
                KirpmaIsleminiYap();
            }
        }

        // Slayttaki gibi kesikli çizgi ve tutma noktalarını ekrana çizdirme
        private void picOrijinal_Paint(object sender, PaintEventArgs e)
        {
            if (secimDikdortgeni.Width <= 0 || secimDikdortgeni.Height <= 0) return;

            // Slayttaki gibi siyah-beyaz belirgin kesikli çizgi (DashDot / Dash)
            using (Pen kesikliKalem = new Pen(Color.Black, 1.5f))
            {
                kesikliKalem.DashStyle = DashStyle.Dash;
                e.Graphics.DrawRectangle(kesikliKalem, secimDikdortgeni);
            }

            // Köşelere slayttaki küçük tutamaç kareleri (Handles)
            int kareBoyut = 4;
            Point[] noktalar = new Point[]
            {
                new Point(secimDikdortgeni.Left, secimDikdortgeni.Top),
                new Point(secimDikdortgeni.Right, secimDikdortgeni.Top),
                new Point(secimDikdortgeni.Left, secimDikdortgeni.Bottom),
                new Point(secimDikdortgeni.Right, secimDikdortgeni.Bottom),
                new Point(secimDikdortgeni.Left + secimDikdortgeni.Width / 2, secimDikdortgeni.Top),
                new Point(secimDikdortgeni.Left + secimDikdortgeni.Width / 2, secimDikdortgeni.Bottom),
                new Point(secimDikdortgeni.Left, secimDikdortgeni.Top + secimDikdortgeni.Height / 2),
                new Point(secimDikdortgeni.Right, secimDikdortgeni.Top + secimDikdortgeni.Height / 2)
            };

            using (Brush firca = new SolidBrush(Color.Black))
            {
                foreach (Point p in noktalar)
                {
                    e.Graphics.FillRectangle(firca, p.X - kareBoyut / 2, p.Y - kareBoyut / 2, kareBoyut, kareBoyut);
                }
            }
        }

        private void btnKirp_Click(object sender, EventArgs e)
        {
            KirpmaIsleminiYap();
        }

        // --- KIRPMA ALGORİTMASI ---
        private void KirpmaIsleminiYap()
        {
            if (orijinalBmp == null)
            {
                MessageBox.Show("Lütfen önce bir resim seçiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (secimDikdortgeni.Width < 5 || secimDikdortgeni.Height < 5)
            {
                MessageBox.Show("Lütfen resim üzerinde geçerli bir alanı mouse ile sürükleyerek seçiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // PictureBox Zoom modunda olduğundan ekrandaki koordinatları gerçek resim koordinatlarına eşleme
            Rectangle gercekResimAlani = GetGercekResimDikdortgeni(picOrijinal);
            if (!gercekResimAlani.IntersectsWith(secimDikdortgeni)) return;

            Rectangle kesisim = Rectangle.Intersect(gercekResimAlani, secimDikdortgeni);

            float oranX = (float)orijinalBmp.Width / gercekResimAlani.Width;
            float oranY = (float)orijinalBmp.Height / gercekResimAlani.Height;

            int srcX = (int)((kesisim.X - gercekResimAlani.X) * oranX);
            int srcY = (int)((kesisim.Y - gercekResimAlani.Y) * oranY);
            int srcW = (int)(kesisim.Width * oranX);
            int srcH = (int)(kesisim.Height * oranY);

            // Sınır güvenliği
            srcX = Math.Max(0, Math.Min(srcX, orijinalBmp.Width - 1));
            srcY = Math.Max(0, Math.Min(srcY, orijinalBmp.Height - 1));
            srcW = Math.Min(srcW, orijinalBmp.Width - srcX);
            srcH = Math.Min(srcH, orijinalBmp.Height - srcY);

            if (srcW <= 0 || srcH <= 0) return;

            kirpilanBmp?.Dispose();
            kirpilanBmp = new Bitmap(srcW, srcH, PixelFormat.Format24bppRgb);

            // Seçilen bölgeyi piksel döngüsüyle kopyalama
            for (int x = 0; x < srcW; x++)
            {
                for (int y = 0; y < srcH; y++)
                {
                    Color c = orijinalBmp.GetPixel(srcX + x, srcY + y);
                    kirpilanBmp.SetPixel(x, y, c);
                }
            }

            if (picKirpilan != null)
            {
                picKirpilan.Image?.Dispose();
                picKirpilan.SizeMode = PictureBoxSizeMode.Zoom;
                picKirpilan.Image = new Bitmap(kirpilanBmp);
            }

            if (lblBilgi != null)
            {
                lblBilgi.Text = $"Kırpma Başarılı! Seçilen Konum: ({srcX}, {srcY}) | Kırpılan Çözünürlük: {srcW}x{srcH} px";
            }
        }

        // PictureBoxSizeMode.Zoom modunda resmin ekranda kapladığı gerçek dikdörtgeni bulur
        private Rectangle GetGercekResimDikdortgeni(PictureBox pBox)
        {
            if (pBox.Image == null) return Rectangle.Empty;

            float pBoxOran = (float)pBox.ClientSize.Width / pBox.ClientSize.Height;
            float resimOran = (float)pBox.Image.Width / pBox.Image.Height;

            int w, h, x, y;

            if (resimOran > pBoxOran)
            {
                w = pBox.ClientSize.Width;
                h = (int)(w / resimOran);
                x = 0;
                y = (pBox.ClientSize.Height - h) / 2;
            }
            else
            {
                h = pBox.ClientSize.Height;
                w = (int)(h * resimOran);
                y = 0;
                x = (pBox.ClientSize.Width - w) / 2;
            }

            return new Rectangle(x, y, w, h);
        }

        private void lnkGeri_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta3Odev13_FormClosing;
            Hafta3Odev12 odev12 = new Hafta3Odev12();
            odev12.Show();
            this.Close();
        }

        private void lnkSonraki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta3Odev13_FormClosing;

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
            this.Close();
        }
    }
}