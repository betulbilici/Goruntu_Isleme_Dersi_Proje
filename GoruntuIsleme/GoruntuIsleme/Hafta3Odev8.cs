using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GoruntuIsleme
{
    public partial class Hafta3Odev8 : Form
    {
        private Bitmap orijinalBmp = null;
        private RectangleF resimRect; // Ölçeklenen resmin dinamik sınırları
        private bool olceklemeModuAktif = false;
        private const int TUTAMAC_BOYUT = 6; // Slayttaki siyah küçük kutucuklar

        private enum TutamacTipi
        {
            Yok,
            SolUst, SagUst, SolAlt, SagAlt,     // Köşeler (Orantılı)
            UstOrta, AltOrta, SolOrta, SagOrta  // Kenarlar (Orantısız Uzama)
        }

        private TutamacTipi aktifTutamac = TutamacTipi.Yok;
        private PointF sonFareKonumu;
        private float orijinalOran = 1.0f;

        public Hafta3Odev8()
        {
            InitializeComponent();
            this.FormClosing += Hafta3Odev8_FormClosing;

            if (btnResimSec != null) btnResimSec.Click += btnResimSec_Click;
            if (btnOlcekleModu != null) btnOlcekleModu.Click += btnOlcekleModu_Click;
            if (btnSifirla != null) btnSifirla.Click += btnSifirla_Click;
            if (lnkGeri != null) lnkGeri.LinkClicked += lnkGeri_LinkClicked;
            if (lnkSonraki != null) lnkSonraki.LinkClicked += lnkSonraki_LinkClicked;

            if (picCanvas != null)
            {
                picCanvas.MouseDown += PicCanvas_MouseDown;
                picCanvas.MouseMove += PicCanvas_MouseMove;
                picCanvas.MouseUp += PicCanvas_MouseUp;
                picCanvas.Paint += PicCanvas_Paint;
            }
        }

        private void Hafta3Odev8_FormClosing(object sender, FormClosingEventArgs e)
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

                    orijinalOran = (float)orijinalBmp.Width / orijinalBmp.Height;

                    // Resmi çizim kutusunun sol üst köşesinden başlatıyoruz (slayttaki gibi)
                    float baslangicW = Math.Min(orijinalBmp.Width, picCanvas.Width * 0.8f);
                    float baslangicH = baslangicW / orijinalOran;

                    resimRect = new RectangleF(15, 15, baslangicW, baslangicH);
                    olceklemeModuAktif = true;

                    GuncelleBilgi();
                    picCanvas.Invalidate();
                }
            }
        }

        private void btnOlcekleModu_Click(object sender, EventArgs e)
        {
            olceklemeModuAktif = !olceklemeModuAktif;
            picCanvas.Invalidate();
        }

        private void btnSifirla_Click(object sender, EventArgs e)
        {
            if (orijinalBmp == null) return;

            float baslangicW = Math.Min(orijinalBmp.Width, picCanvas.Width * 0.8f);
            float baslangicH = baslangicW / orijinalOran;
            resimRect = new RectangleF(15, 15, baslangicW, baslangicH);

            GuncelleBilgi();
            picCanvas.Invalidate();
        }

        // Slayttaki 8 adet tutamacın koordinatları
        private RectangleF[] TutamacKutulariniAl()
        {
            float x = resimRect.X;
            float y = resimRect.Y;
            float w = resimRect.Width;
            float h = resimRect.Height;
            float r = TUTAMAC_BOYUT / 2f;

            return new RectangleF[]
            {
                new RectangleF(x - r, y - r, TUTAMAC_BOYUT, TUTAMAC_BOYUT),             // Sol-Üst
                new RectangleF(x + w - r, y - r, TUTAMAC_BOYUT, TUTAMAC_BOYUT),         // Sağ-Üst
                new RectangleF(x - r, y + h - r, TUTAMAC_BOYUT, TUTAMAC_BOYUT),         // Sol-Alt
                new RectangleF(x + w - r, y + h - r, TUTAMAC_BOYUT, TUTAMAC_BOYUT),     // Sağ-Alt
                new RectangleF(x + w/2f - r, y - r, TUTAMAC_BOYUT, TUTAMAC_BOYUT),     // Üst-Orta
                new RectangleF(x + w/2f - r, y + h - r, TUTAMAC_BOYUT, TUTAMAC_BOYUT), // Alt-Orta
                new RectangleF(x - r, y + h/2f - r, TUTAMAC_BOYUT, TUTAMAC_BOYUT),     // Sol-Orta
                new RectangleF(x + w - r, y + h/2f - r, TUTAMAC_BOYUT, TUTAMAC_BOYUT)  // Sağ-Orta
            };
        }

        private TutamacTipi FareHangiTutamaçta(PointF p)
        {
            if (!olceklemeModuAktif || orijinalBmp == null) return TutamacTipi.Yok;

            RectangleF[] kutular = TutamacKutulariniAl();
            if (kutular[0].Contains(p)) return TutamacTipi.SolUst;
            if (kutular[1].Contains(p)) return TutamacTipi.SagUst;
            if (kutular[2].Contains(p)) return TutamacTipi.SolAlt;
            if (kutular[3].Contains(p)) return TutamacTipi.SagAlt;
            if (kutular[4].Contains(p)) return TutamacTipi.UstOrta;
            if (kutular[5].Contains(p)) return TutamacTipi.AltOrta;
            if (kutular[6].Contains(p)) return TutamacTipi.SolOrta;
            if (kutular[7].Contains(p)) return TutamacTipi.SagOrta;

            return TutamacTipi.Yok;
        }

        private void PicCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || !olceklemeModuAktif || orijinalBmp == null) return;

            aktifTutamac = FareHangiTutamaçta(e.Location);
            sonFareKonumu = e.Location;
        }

        private void PicCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!olceklemeModuAktif || orijinalBmp == null) return;

            if (aktifTutamac == TutamacTipi.Yok)
            {
                TutamacTipi uzerindeki = FareHangiTutamaçta(e.Location);
                switch (uzerindeki)
                {
                    case TutamacTipi.SolUst:
                    case TutamacTipi.SagAlt:
                        picCanvas.Cursor = Cursors.SizeNWSE;
                        break;
                    case TutamacTipi.SagUst:
                    case TutamacTipi.SolAlt:
                        picCanvas.Cursor = Cursors.SizeNESW;
                        break;
                    case TutamacTipi.UstOrta:
                    case TutamacTipi.AltOrta:
                        picCanvas.Cursor = Cursors.SizeNS;
                        break;
                    case TutamacTipi.SolOrta:
                    case TutamacTipi.SagOrta:
                        picCanvas.Cursor = Cursors.SizeWE;
                        break;
                    default:
                        picCanvas.Cursor = Cursors.Default;
                        break;
                }
                return;
            }

            float dx = e.X - sonFareKonumu.X;
            float dy = e.Y - sonFareKonumu.Y;

            float yeniX = resimRect.X;
            float yeniY = resimRect.Y;
            float yeniW = resimRect.Width;
            float yeniH = resimRect.Height;

            switch (aktifTutamac)
            {
                // KÖŞE TUTAMAÇLARI: Orantılı (Aspect Ratio Sabit) Ölçekleme
                case TutamacTipi.SagAlt:
                    yeniW = Math.Max(30, resimRect.Width + dx);
                    yeniH = yeniW / orijinalOran;
                    break;

                case TutamacTipi.SagUst:
                    yeniW = Math.Max(30, resimRect.Width + dx);
                    yeniH = yeniW / orijinalOran;
                    yeniY = resimRect.Bottom - yeniH;
                    break;

                case TutamacTipi.SolAlt:
                    yeniW = Math.Max(30, resimRect.Width - dx);
                    yeniH = yeniW / orijinalOran;
                    yeniX = resimRect.Right - yeniW;
                    break;

                case TutamacTipi.SolUst:
                    yeniW = Math.Max(30, resimRect.Width - dx);
                    yeniH = yeniW / orijinalOran;
                    yeniX = resimRect.Right - yeniW;
                    yeniY = resimRect.Bottom - yeniH;
                    break;

                // KENAR ORTA TUTAMAÇLARI: Orantısız (Yatay veya Dikey Uzama/Basma)
                case TutamacTipi.SagOrta:
                    yeniW = Math.Max(30, resimRect.Width + dx); // Sadece yatay genişler
                    break;

                case TutamacTipi.SolOrta:
                    yeniW = Math.Max(30, resimRect.Width - dx);
                    yeniX = resimRect.Right - yeniW;
                    break;

                case TutamacTipi.AltOrta:
                    yeniH = Math.Max(30, resimRect.Height + dy); // Sadece dikey uzar
                    break;

                case TutamacTipi.UstOrta:
                    yeniH = Math.Max(30, resimRect.Height - dy);
                    yeniY = resimRect.Bottom - yeniH;
                    break;
            }

            resimRect = new RectangleF(yeniX, yeniY, yeniW, yeniH);
            sonFareKonumu = e.Location;

            GuncelleBilgi();
            picCanvas.Invalidate();
        }

        private void PicCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            aktifTutamac = TutamacTipi.Yok;
            picCanvas.Cursor = Cursors.Default;
        }

        private void PicCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            // Slayttaki gibi şeffaf/arka plan dama deseni (HatchBrush)
            using (HatchBrush damaFirca = new HatchBrush(HatchStyle.LargeCheckerBoard, Color.LightGray, Color.White))
            {
                g.FillRectangle(damaFirca, picCanvas.ClientRectangle);
            }

            if (orijinalBmp != null)
            {
                // Boyutlandırılmış resmin çizimi
                g.DrawImage(orijinalBmp, resimRect);

                if (olceklemeModuAktif)
                {
                    // İnce dış sınır çizgisi
                    using (Pen penSinir = new Pen(Color.Black, 1f))
                    {
                        g.DrawRectangle(penSinir, resimRect.X, resimRect.Y, resimRect.Width, resimRect.Height);
                    }

                    // Slayttaki gibi siyah küçük tutamaç kutucukları (8 adet)
                    RectangleF[] kutular = TutamacKutulariniAl();
                    using (Brush fircaSiyah = new SolidBrush(Color.Black))
                    {
                        for (int i = 0; i < kutular.Length; i++)
                        {
                            g.FillRectangle(fircaSiyah, kutular[i]);
                        }

                        // Slaytta görünen tam ortadaki küçük merkez noktası
                        float cx = resimRect.X + resimRect.Width / 2f - 1.5f;
                        float cy = resimRect.Y + resimRect.Height / 2f - 1.5f;
                        g.FillRectangle(fircaSiyah, cx, cy, 3, 3);
                    }
                }
            }
        }

        private void GuncelleBilgi()
        {
            if (lblBilgi == null || orijinalBmp == null) return;

            float anlikOran = resimRect.Width / resimRect.Height;
            bool orantiKorundu = Math.Abs(anlikOran - orijinalOran) < 0.03f;

            lblBilgi.Text = $"Orijinal Boyut : {orijinalBmp.Width}x{orijinalBmp.Height} px\n" +
                            $"Mevcut Boyut   : {Math.Round(resimRect.Width)}x{Math.Round(resimRect.Height)} px\n" +
                            $"Durum          : {(orantiKorundu ? "En/Boy Orantısı Korundu (Köşeden Küçültme/Büyütme)" : "Yatay/Dikey Esnetildi (Orantı Bozuldu)")}";
        }

        private void lnkGeri_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta3Odev8_FormClosing;
            Hafta3Odev7 odev7 = new Hafta3Odev7();
            odev7.Show();
            this.Close();
        }

        private void lnkSonraki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta3Odev8_FormClosing;
            Hafta3Odev9 odev9 = new Hafta3Odev9();
            odev9.Show();
            this.Close();
        }
    }
}