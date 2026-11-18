using System;
using System.Drawing;
using System.Windows.Forms;

namespace GoruntuIsleme
{
    public partial class Hafta3Odev7 : Form
    {
        private Bitmap orijinalBmp = null;
        private Point? donmeMerkezi = null;
        private bool artiGoster = false;

        public Hafta3Odev7()
        {
            InitializeComponent();
            this.FormClosing += Hafta3Odev7_FormClosing;

            if (btnResimSec != null) btnResimSec.Click += btnResimSec_Click;
            if (btnDondur != null) btnDondur.Click += btnDondur_Click;
            if (lnkGeri != null) lnkGeri.LinkClicked += lnkGeri_LinkClicked;
            if (lnkSonraki != null) lnkSonraki.LinkClicked += lnkSonraki_LinkClicked;

            if (picKaynak != null)
            {
                picKaynak.MouseDown += PicKaynak_MouseDown;
                picKaynak.Paint += PicKaynak_Paint;
            }
        }

        private void Hafta3Odev7_FormClosing(object sender, FormClosingEventArgs e)
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

                    // Varsayılan dönme merkezi resmin tam ortası
                    donmeMerkezi = new Point(orijinalBmp.Width / 2, orijinalBmp.Height / 2);
                    artiGoster = true;

                    if (picHedef != null) picHedef.Image = null;
                    if (lblBilgi != null)
                    {
                        lblBilgi.Text = $"Resim Yüklendi ({orijinalBmp.Width}x{orijinalBmp.Height} px).\n" +
                                        $"Dönme Merkezi: X={donmeMerkezi.Value.X}, Y={donmeMerkezi.Value.Y}\n" +
                                        $"Farklı bir merkez için resme tıklayabilirsiniz.";
                    }

                    picKaynak.Invalidate();
                }
            }
        }

        private void PicKaynak_MouseDown(object sender, MouseEventArgs e)
        {
            if (orijinalBmp == null || e.Button != MouseButtons.Left) return;

            // Tıklanan noktanın resim sınırları içinde kalmasını sağla
            int x = Math.Max(0, Math.Min(e.X, orijinalBmp.Width - 1));
            int y = Math.Max(0, Math.Min(e.Y, orijinalBmp.Height - 1));

            donmeMerkezi = new Point(x, y);
            artiGoster = true;

            if (lblBilgi != null)
            {
                lblBilgi.Text = $"Dönme Merkezi Belirlendi: ({x}, {y})\n" +
                                $"Açıyı girip 'Döndür' butonuna basınız.";
            }

            picKaynak.Invalidate();
        }

        private void btnDondur_Click(object sender, EventArgs e)
        {
            if (orijinalBmp == null)
            {
                MessageBox.Show("Lütfen önce bir resim seçiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!double.TryParse(txtAci.Text, out double aci))
            {
                MessageBox.Show("Lütfen geçerli bir sayısal açı değeri giriniz! (Örn: 45, 90, -30)", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Point merkez = donmeMerkezi ?? new Point(orijinalBmp.Width / 2, orijinalBmp.Height / 2);

            this.Cursor = Cursors.WaitCursor;

            int w = orijinalBmp.Width;
            int h = orijinalBmp.Height;
            Bitmap sonucBmp = new Bitmap(w, h);

            // Açı radyana çevrilir
            double radyan = aci * Math.PI / 180.0;
            double cos = Math.Cos(radyan);
            double sin = Math.Sin(radyan);

            int x0 = merkez.X;
            int y0 = merkez.Y;

            // Ters dönüşüm (Inverse Mapping): Hedef pikselden kaynak piksele eşleme
            for (int y = 0; y < h; y++)
            {
                int dy = y - y0;
                for (int x = 0; x < w; x++)
                {
                    int dx = x - x0;

                    int srcX = (int)Math.Round(x0 + (dx * cos + dy * sin));
                    int srcY = (int)Math.Round(y0 + (-dx * sin + dy * cos));

                    if (srcX >= 0 && srcX < w && srcY >= 0 && srcY < h)
                    {
                        sonucBmp.SetPixel(x, y, orijinalBmp.GetPixel(srcX, srcY));
                    }
                    else
                    {
                        sonucBmp.SetPixel(x, y, Color.White); // Resim dışı alanlar beyaz kalır
                    }
                }
            }

            // Döndürme sonrası soru şartı: Artı işareti kalkmalı
            artiGoster = false;
            picKaynak.Invalidate();

            if (picHedef != null)
            {
                picHedef.Image?.Dispose();
                picHedef.Image = sonucBmp;
            }

            if (lblBilgi != null)
            {
                lblBilgi.Text = $"İşlem Tamamlandı:\n" +
                                $"• Dönme Açısı   : {aci}°\n" +
                                $"• Merkez Noktası: ({x0}, {y0})\n" +
                                $"• Artı işareti kaldırıldı.";
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

            // Tıklanan yerde küçük bir artı (+) işareti çizimi
            if (artiGoster && donmeMerkezi.HasValue)
            {
                int cx = donmeMerkezi.Value.X;
                int cy = donmeMerkezi.Value.Y;
                int kol = 8; // Artı işaretinin kol uzunluğu

                using (Pen penKirmizi = new Pen(Color.Red, 2f))
                {
                    g.DrawLine(penKirmizi, cx - kol, cy, cx + kol, cy);
                    g.DrawLine(penKirmizi, cx, cy - kol, cx, cy + kol);
                }
            }
        }

        private void lnkGeri_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta3Odev7_FormClosing;
            Hafta3Odev6 odev6 = new Hafta3Odev6();
            odev6.Show();
            this.Close();
        }

        private void lnkSonraki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta3Odev7_FormClosing;
            Hafta3Odev8 odev8 = new Hafta3Odev8();
            odev8.Show();
            this.Close();
        }
    }
}