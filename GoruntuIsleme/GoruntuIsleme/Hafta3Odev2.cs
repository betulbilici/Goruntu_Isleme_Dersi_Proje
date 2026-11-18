using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GoruntuIsleme
{
    public partial class Hafta3Odev2 : Form
    {
        // Slayt İsteği: Dışarı taşan piksellerin kaybolmaması için tutulan dizi yapısı
        private struct PikselBilgi
        {
            public int X;
            public int Y;
            public Color Renk;
        }

        private Bitmap orijinalBmp = null;
        private PikselBilgi[] pikselDizisi = null;
        private int resimGenisligi = 0;
        private int resimYuksekligi = 0;

        // Hocanın notlarındaki EgmeKatsayisi (b katsayısı)
        private double egmeKatsayisiX = 0.0; // X yönündeki eğme (+b veya -b)
        private double egmeKatsayisiY = 0.0; // Y yönündeki eğme (+b veya -b)

        // Fare ile kenarlardan tutma takibi
        private enum KenarTipi { Yok, Ust, Alt, Sol, Sag }
        private KenarTipi secilenKenar = KenarTipi.Yok;
        private Point fareBaslangicKonum;
        private double baslangicKatsayiX = 0.0;
        private double baslangicKatsayiY = 0.0;

        public Hafta3Odev2()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1250, 750);
            this.StartPosition = FormStartPosition.CenterScreen;

            this.FormClosing += Hafta3Odev2_FormClosing;

            // Bilgi etiketinin çok satırlı metni kesilmeden gösterebilmesi için ayarlar
            if (lblBilgi != null)
            {
                lblBilgi.AutoSize = false;
                lblBilgi.Width = 380;
                lblBilgi.Height = 110;
                lblBilgi.Text = "Lütfen bir resim seçip kenarlardaki tutamaçlardan sürükleyin.";
            }

            if (btnResimSec != null) btnResimSec.Click += btnResimSec_Click;
            if (btnSifirla != null) btnSifirla.Click += btnSifirla_Click;
            if (lnkGeri != null) lnkGeri.LinkClicked += lnkGeri_LinkClicked;
            if (lnkSonraki != null) lnkSonraki.LinkClicked += lnkSonraki_LinkClicked;

            // TrackBar (Sürgü) etkileşimleri
            if (trackShx != null)
            {
                trackShx.Scroll += (s, e) =>
                {
                    egmeKatsayisiX = trackShx.Value / 100.0;
                    BilgiMetniniGuncelle();
                    picCanvas.Invalidate();
                };
            }

            if (trackShy != null)
            {
                trackShy.Scroll += (s, e) =>
                {
                    egmeKatsayisiY = trackShy.Value / 100.0;
                    BilgiMetniniGuncelle();
                    picCanvas.Invalidate();
                };
            }

            // PictureBox olayları
            if (picCanvas != null)
            {
                picCanvas.MouseDown += PicCanvas_MouseDown;
                picCanvas.MouseMove += PicCanvas_MouseMove;
                picCanvas.MouseUp += PicCanvas_MouseUp;
                picCanvas.Paint += PicCanvas_Paint;
            }
        }

        private void Hafta3Odev2_FormClosing(object sender, FormClosingEventArgs e)
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

        // 1. Resim Seçme ve Tüm Pikselleri Diziye Kaydetme
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
                        int hedefW = temp.Width;
                        int hedefH = temp.Height;
                        if (hedefW > 400 || hedefH > 400)
                        {
                            double oran = Math.Min(350.0 / hedefW, 350.0 / hedefH);
                            hedefW = (int)(hedefW * oran);
                            hedefH = (int)(hedefH * oran);
                        }
                        orijinalBmp = new Bitmap(temp, hedefW, hedefH);
                    }

                    resimGenisligi = orijinalBmp.Width;
                    resimYuksekligi = orijinalBmp.Height;

                    // Slayt İsteği: "tüm resim üzerindeki piksel değerlerinin hem koordinatları hem de renk bilgileri dizide tutulabilir."
                    pikselDizisi = new PikselBilgi[resimGenisligi * resimYuksekligi];
                    int index = 0;
                    for (int y1 = 0; y1 < resimYuksekligi; y1++)
                    {
                        for (int x1 = 0; x1 < resimGenisligi; x1++)
                        {
                            pikselDizisi[index++] = new PikselBilgi
                            {
                                X = x1,
                                Y = y1,
                                Renk = orijinalBmp.GetPixel(x1, y1)
                            };
                        }
                    }

                    egmeKatsayisiX = 0.0;
                    egmeKatsayisiY = 0.0;
                    if (trackShx != null) trackShx.Value = 0;
                    if (trackShy != null) trackShy.Value = 0;

                    BilgiMetniniGuncelle();
                    picCanvas.Invalidate();
                }
            }
        }

        private void btnSifirla_Click(object sender, EventArgs e)
        {
            egmeKatsayisiX = 0.0;
            egmeKatsayisiY = 0.0;
            if (trackShx != null) trackShx.Value = 0;
            if (trackShy != null) trackShy.Value = 0;

            BilgiMetniniGuncelle();
            picCanvas.Invalidate();
        }

        // 2. Fare Olayları: Kenarları Yakalama
        private void PicCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (pikselDizisi == null) return;

            secilenKenar = HangiKenarUzerinde(e.Location);
            if (secilenKenar != KenarTipi.Yok)
            {
                fareBaslangicKonum = e.Location;
                baslangicKatsayiX = egmeKatsayisiX;
                baslangicKatsayiY = egmeKatsayisiY;
            }
        }

        private void PicCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (pikselDizisi == null) return;

            // İmleç ikonunu ayarla
            KenarTipi k = HangiKenarUzerinde(e.Location);
            if (k == KenarTipi.Ust || k == KenarTipi.Alt)
                picCanvas.Cursor = Cursors.SizeWE; // Yatay kaydırma imleci
            else if (k == KenarTipi.Sol || k == KenarTipi.Sag)
                picCanvas.Cursor = Cursors.SizeNS; // Dikey kaydırma imleci
            else
                picCanvas.Cursor = Cursors.Default;

            // Sürükleme esnasında katsayıyı hesapla
            if (e.Button == MouseButtons.Left && secilenKenar != KenarTipi.Yok)
            {
                int dx = e.X - fareBaslangicKonum.X;
                int dy = e.Y - fareBaslangicKonum.Y;

                // Üst kenardan sağa sürüklenirse +X yönü, sola sürüklenirse -X yönü formülü
                if (secilenKenar == KenarTipi.Ust)
                {
                    egmeKatsayisiX = baslangicKatsayiX + ((double)dx / resimYuksekligi);
                }
                // Alt kenardan çekilirse ters yön
                else if (secilenKenar == KenarTipi.Alt)
                {
                    egmeKatsayisiX = baslangicKatsayiX - ((double)dx / resimYuksekligi);
                }
                // Sağ veya Sol kenardan çekilirse +Y / -Y formülü
                else if (secilenKenar == KenarTipi.Sag)
                {
                    egmeKatsayisiY = baslangicKatsayiY + ((double)dy / resimGenisligi);
                }
                else if (secilenKenar == KenarTipi.Sol)
                {
                    egmeKatsayisiY = baslangicKatsayiY - ((double)dy / resimGenisligi);
                }

                // Aşırı taşmayı engellemek için sınırlandırma
                egmeKatsayisiX = Math.Max(-1.2, Math.Min(1.2, egmeKatsayisiX));
                egmeKatsayisiY = Math.Max(-1.2, Math.Min(1.2, egmeKatsayisiY));

                if (trackShx != null) trackShx.Value = (int)(egmeKatsayisiX * 100);
                if (trackShy != null) trackShy.Value = (int)(egmeKatsayisiY * 100);

                BilgiMetniniGuncelle();
                picCanvas.Invalidate();
            }
        }

        private void PicCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            secilenKenar = KenarTipi.Yok;
        }

        // Bilgi Etiketini Güncelleyen Yardımcı Metot
        private void BilgiMetniniGuncelle()
        {
            // TrackBar başlıklarını anlık değerle güncelle
            if (lblBaslikShx != null)
                lblBaslikShx.Text = $"Yatay Eğme (Shx): {egmeKatsayisiX:F2}";

            if (lblBaslikShy != null)
                lblBaslikShy.Text = $"Dikey Eğme (Shy): {egmeKatsayisiY:F2}";

            if (lblBilgi == null) return;

            if (pikselDizisi == null)
            {
                lblBilgi.Text = "Lütfen bir resim seçiniz.";
                return;
            }

            lblBilgi.Text = $"Uygulanan Formül Katsayıları:\n" +
                            $"• Yatay: x2 = x1 {(egmeKatsayisiX >= 0 ? "+" : "-")} {Math.Abs(egmeKatsayisiX):F2} * y1\n" +
                            $"• Dikey: y2 = {(egmeKatsayisiY >= 0 ? "+" : "-")}{Math.Abs(egmeKatsayisiY):F2} * x1 + y1\n" +
                            $"Dizide korunan piksel sayısı: {pikselDizisi.Length:N0}";
        }

        // Kenar ve Tutamaç Alanı Tespiti
        private KenarTipi HangiKenarUzerinde(Point p)
        {
            int ox = (picCanvas.Width - resimGenisligi) / 2;
            int oy = (picCanvas.Height - resimYukseklikHesapla()) / 2;

            PointF pUst = new PointF(ox + (resimGenisligi / 2f) + (float)(egmeKatsayisiX * (-resimYuksekligi / 2f)), oy);
            PointF pAlt = new PointF(ox + (resimGenisligi / 2f) + (float)(egmeKatsayisiX * (resimYuksekligi / 2f)), oy + resimYuksekligi);
            PointF pSol = new PointF(ox, oy + (resimYuksekligi / 2f) - (float)(egmeKatsayisiY * (resimGenisligi / 2f)));
            PointF pSag = new PointF(ox + resimGenisligi, oy + (resimYuksekligi / 2f) + (float)(egmeKatsayisiY * (resimGenisligi / 2f)));

            int tol = 16;
            if (Math.Abs(p.X - pUst.X) <= tol && Math.Abs(p.Y - pUst.Y) <= tol) return KenarTipi.Ust;
            if (Math.Abs(p.X - pAlt.X) <= tol && Math.Abs(p.Y - pAlt.Y) <= tol) return KenarTipi.Alt;
            if (Math.Abs(p.X - pSol.X) <= tol && Math.Abs(p.Y - pSol.Y) <= tol) return KenarTipi.Sol;
            if (Math.Abs(p.X - pSag.X) <= tol && Math.Abs(p.Y - pSag.Y) <= tol) return KenarTipi.Sag;

            return KenarTipi.Yok;
        }

        private int resimYukseklikHesapla() => resimYuksekligi > 0 ? resimYuksekligi : 200;

        // 3. Slayttaki Formüllere Göre Çizim Yapma Metodu
        // 3. Slayttaki Formüllere Göre Çizim Yapma Metodu (Orijinal Mavi Kutu + Eğilmiş Resim)
        private void PicCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (pikselDizisi == null || orijinalBmp == null) return;

            int ox = (picCanvas.Width - resimGenisligi) / 2;
            int oy = (picCanvas.Height - resimYuksekligi) / 2;

            // --- 1. SLAYTTAKİ ORİJİNAL MAVİ ÇERÇEVE (SABİT DİKDÖRTGEN) ---
            Rectangle orijinalKutu = new Rectangle(ox, oy, resimGenisligi, resimYuksekligi);

            // Mavi ince çerçeve
            using (Pen pMavi = new Pen(Color.FromArgb(0, 160, 240), 1.5f))
            {
                g.DrawRectangle(pMavi, orijinalKutu);
            }

            // Mavi çerçevenin 4 köşesindeki küçük açık mavi tutamaçlar
            Point[] maviKoseler = new Point[]
            {
        new Point(ox, oy),
        new Point(ox + resimGenisligi, oy),
        new Point(ox + resimGenisligi, oy + resimYuksekligi),
        new Point(ox, oy + resimYuksekligi)
            };

            int maviK = 6;
            using (Brush bMavi = new SolidBrush(Color.FromArgb(0, 170, 255)))
            {
                foreach (var mk in maviKoseler)
                {
                    g.FillRectangle(bMavi, mk.X - maviK / 2, mk.Y - maviK / 2, maviK, maviK);
                }
            }

            // --- 2. EĞİLMİŞ RESİM VE DAMALI ARKA PLAN (PARALELKENAR) ---
            PointF kSolUst = FormulleDonustur(0, 0, ox, oy);
            PointF kSagUst = FormulleDonustur(resimGenisligi, 0, ox, oy);
            PointF kSagAlt = FormulleDonustur(resimGenisligi, resimYuksekligi, ox, oy);
            PointF kSolAlt = FormulleDonustur(0, resimYuksekligi, ox, oy);
            PointF[] koseler = { kSolUst, kSagUst, kSagAlt, kSolAlt };

            // Slayttaki şeffaf arka plan daması
            using (HatchBrush damaBrush = new HatchBrush(HatchStyle.LargeCheckerBoard, Color.White, Color.Gainsboro))
            {
                g.FillPolygon(damaBrush, koseler);
            }

            // Eğilmiş resmi çiz (Shear matrisi ile)
            using (Matrix mat = new Matrix())
            {
                mat.Translate(ox + resimGenisligi / 2f, oy + resimYuksekligi / 2f);
                mat.Shear((float)egmeKatsayisiX, (float)egmeKatsayisiY);
                mat.Translate(-(ox + resimGenisligi / 2f), -(oy + resimYuksekligi / 2f));

                g.Transform = mat;
                g.DrawImage(orijinalBmp, ox, oy, resimGenisligi, resimYuksekligi);
                g.ResetTransform();
            }

            // --- 3. EĞİLMİŞ RESMİN SİYAH ÇERÇEVESİ VE SİYAH TUTAMAÇLARI ---
            using (Pen cercevePen = new Pen(Color.FromArgb(40, 40, 40), 1.2f))
            {
                g.DrawPolygon(cercevePen, koseler);
            }

            // Kenar ve köşe orta noktaları
            PointF ustOrta = new PointF((kSolUst.X + kSagUst.X) / 2f, (kSolUst.Y + kSagUst.Y) / 2f);
            PointF altOrta = new PointF((kSolAlt.X + kSagAlt.X) / 2f, (kSolAlt.Y + kSagAlt.Y) / 2f);
            PointF solOrta = new PointF((kSolUst.X + kSolAlt.X) / 2f, (kSolUst.Y + kSolAlt.Y) / 2f);
            PointF sagOrta = new PointF((kSagUst.X + kSagAlt.X) / 2f, (kSagUst.Y + kSagAlt.Y) / 2f);
            PointF merkezNokta = new PointF(ox + resimGenisligi / 2f, oy + resimYuksekligi / 2f);

            PointF[] siyahTutamaclar = { kSolUst, kSagUst, kSagAlt, kSolAlt, ustOrta, altOrta, solOrta, sagOrta, merkezNokta };

            int k = 7;
            using (Brush bSiyah = new SolidBrush(Color.Black))
            using (Pen pBeyaz = new Pen(Color.White, 1.2f))
            {
                foreach (var pt in siyahTutamaclar)
                {
                    RectangleF r = new RectangleF(pt.X - k / 2f, pt.Y - k / 2f, k, k);
                    g.FillRectangle(bSiyah, r);
                    g.DrawRectangle(pBeyaz, r.X, r.Y, r.Width, r.Height);
                }
            }
        }

        // Slayttaki Matematiksel Formül Çeviricisi
        private PointF FormulleDonustur(int x1, int y1, int ox, int oy)
        {
            double relX = x1 - (resimGenisligi / 2.0);
            double relY = y1 - (resimYuksekligi / 2.0);

            double x2 = relX + (egmeKatsayisiX * relY);
            double y2 = relY + (egmeKatsayisiY * relX);

            return new PointF((float)(ox + (resimGenisligi / 2.0) + x2),
                              (float)(oy + (resimYuksekligi / 2.0) + y2));
        }

        // Ödev 1'e Dön Linki
        private void lnkGeri_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta3Odev2_FormClosing;

            Hafta3Odev1 odev1 = new Hafta3Odev1();
            odev1.StartPosition = FormStartPosition.Manual;
            odev1.Location = this.Location;
            odev1.Size = this.Size;
            odev1.FormBorderStyle = this.FormBorderStyle;

            odev1.Show();
            this.Close();
        }

        // Ödev 3'e Git Linki
        private void lnkSonraki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta3Odev2_FormClosing;

            Hafta3Odev3 odev3 = new Hafta3Odev3();
            odev3.StartPosition = FormStartPosition.Manual;
            odev3.Location = this.Location;
            odev3.Size = this.Size;
            odev3.FormBorderStyle = this.FormBorderStyle;

            odev3.Show();
            this.Close();
        }
    }
}