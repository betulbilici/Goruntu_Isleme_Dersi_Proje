using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace GoruntuIsleme
{
    public partial class Hafta3Odev2 : Form
    {
        // Dışarı taşan piksellerin kaybolmaması için tutulan dizi yapısı
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

        // Eğme katsayıları (b katsayısı)
        private double egmeKatsayisiX = 0.0;
        private double egmeKatsayisiY = 0.0;

        // Fare ile kenarlardan tutma takibi
        private enum KenarTipi { Yok, Ust, Alt, Sol, Sag }
        private KenarTipi secilenKenar = KenarTipi.Yok;
        private Point fareBaslangicKonum;
        private double baslangicKatsayiX = 0.0;
        private double baslangicKatsayiY = 0.0;

        private ModernHapRozet rozetTuval;
        private ModernSurgu surguShx;
        private ModernSurgu surguShy;
        private Label lblValShx;
        private Label lblValShy;
        private Label lblFormulKart;
        private Label lblAnaliz;

        public Hafta3Odev2()
        {
            InitializeComponent();
            ModernArayuzuUygula();
        }

        private void ModernArayuzuUygula()
        {
            // 1. Form Temel Ayarları (1250x750, Standart Kenarlık ve X ile Ana Menüye Dönüş)
            FormGezintiYardimcisi.FormuHazirla(this);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Text = "Hafta 3 - Ödev 2: İnteraktif Resim Eğme / Kaydırma (Shear)";

            // Designer'daki tüm eski kontrolleri gizle
            foreach (Control ctrl in this.Controls)
            {
                ctrl.Visible = false;
            }

            // 2. Üst Gezinti Barı
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                form: this,
                baslik: "Hafta 3 - Ödev 2 | İnteraktif Resim Eğme / Kaydırma (Shear Dönüşümü)",
                oncekiYazi: "< Ödev 1",
                oncekiTikla: () =>
                {
                    Form odev1 = Application.OpenForms["Hafta3Odev1"];
                    if (odev1 != null)
                        FormGezintiYardimcisi.FormGecisYap(this, odev1);
                    else
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta3Odev1());
                },
                sonrakiYazi: "Ödev 3 >",
                sonrakiTikla: () =>
                {
                    Form odev3 = Application.OpenForms["Hafta3Odev3"];
                    if (odev3 != null)
                        FormGezintiYardimcisi.FormGecisYap(this, odev3);
                    else
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta3Odev3());
                }
            );

            // 3. Sağ Kontrol Paneli
            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 300);

            Label lblKontrolBaslik = new Label
            {
                Text = "Shear (Eğme) Kontrolleri",
                Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                Location = new Point(18, 16),
                AutoSize = true
            };
            pnlSag.Controls.Add(lblKontrolBaslik);

            // Resim Seç & Sıfırla Butonları
            ModernHapButon btnModernResimSec = FormGezintiYardimcisi.ModernButonOlustur("📁 Resim Seç", true, 132, 38);
            btnModernResimSec.Location = new Point(18, 46);
            btnModernResimSec.Click += btnResimSec_Click;
            pnlSag.Controls.Add(btnModernResimSec);

            ModernHapButon btnModernSifirla = FormGezintiYardimcisi.ModernButonOlustur("🔄 Sıfırla", false, 122, 38);
            btnModernSifirla.Location = new Point(158, 46);
            btnModernSifirla.Click += btnSifirla_Click;
            pnlSag.Controls.Add(btnModernSifirla);

            // Fare ile Kenardan Tut & Sürükle Seçeneği
            rbFareyleSurukle.Parent?.Controls.Remove(rbFareyleSurukle);
            rbFareyleSurukle.Visible = true;
            rbFareyleSurukle.Checked = true;
            rbFareyleSurukle.Location = new Point(18, 96);
            rbFareyleSurukle.Font = new Font("Segoe UI Semibold", 9.3f, FontStyle.Bold);
            rbFareyleSurukle.ForeColor = FormGezintiYardimcisi.KoyuYazi;
            rbFareyleSurukle.Text = "Fare ile Kenardan Tut & Sürükle";
            pnlSag.Controls.Add(rbFareyleSurukle);

            // Yatay Eğme (Shx) Sürgüsü (-120 ile +120 arası -> -1.20 ile +1.20)
            surguShx = SurguEkle(pnlSag, "Yatay Eğme (Shx)", -120, 120, 0, 134, out lblValShx, (val) =>
            {
                egmeKatsayisiX = val / 100.0;
                lblValShx.Text = $"{egmeKatsayisiX:F2}";
                BilgiMetniniGuncelle();
                picCanvas.Invalidate();
            });

            // Dikey Eğme (Shy) Sürgüsü
            surguShy = SurguEkle(pnlSag, "Dikey Eğme (Shy)", -120, 120, 0, 192, out lblValShy, (val) =>
            {
                egmeKatsayisiY = val / 100.0;
                lblValShy.Text = $"{egmeKatsayisiY:F2}";
                BilgiMetniniGuncelle();
                picCanvas.Invalidate();
            });

            // Açık Mavi Formül ve Piksel Dizisi Kartı
            ModernKartPanel bilgiKarti = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                konum: new Point(18, 256),
                boyut: new Size(262, 310),
                icerikEtiketi: out lblFormulKart
            );
            lblFormulKart.Text =
                "Uygulanan Formüller:\n" +
                "• Yatay: x₂ = x₁ + 0.00 · y₁\n" +
                "• Dikey: y₂ = +0.00 · x₁ + y₁\n\n" +
                "Kullanım:\n" +
                "• Kenar orta noktalarındaki siyah tutamaçlardan tutup sürükleyebilir veya yukarıdaki sürgüleri kullanabilirsiniz.\n" +
                "• Tüm piksel koordinat ve renkleri bellekteki dizide korunur.";
            pnlSag.Controls.Add(bilgiKarti);

            // 4. Orta Çalışma Alanı (Üst: Ana Tuval Kartı | Alt: Analiz Şeridi)
            Panel pnlOrta = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(12),
                BackColor = FormGezintiYardimcisi.TemaZemin
            };

            TableLayoutPanel anaYerlesim = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2
            };
            anaYerlesim.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            anaYerlesim.RowStyles.Add(new RowStyle(SizeType.Absolute, 50f));

            // Tuval Kartı
            ModernKartPanel tuvalKart = new ModernKartPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(6),
                Padding = new Padding(12)
            };

            Panel pnlTuvalUst = new Panel
            {
                Dock = DockStyle.Top,
                Height = 28,
                BackColor = Color.White
            };

            Label lblTuvalBaslik = new Label
            {
                Text = "Ana Tuval (Kenar Tutamaçlarını Sürükleyerek Eğme Uygulayın)",
                UseMnemonic = false,
                AutoSize = true,
                Location = new Point(0, 4),
                Font = new Font("Segoe UI Semibold", 9.8f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi
            };
            pnlTuvalUst.Controls.Add(lblTuvalBaslik);

            rozetTuval = new ModernHapRozet();
            rozetTuval.MetinGuncelle("Shx: 0.00 | Shy: 0.00");
            rozetTuval.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            rozetTuval.Location = new Point(pnlTuvalUst.Width - rozetTuval.Width - 2, 1);
            pnlTuvalUst.Resize += (s, e) =>
            {
                rozetTuval.Location = new Point(pnlTuvalUst.Width - rozetTuval.Width - 2, 1);
            };
            pnlTuvalUst.Controls.Add(rozetTuval);

            picCanvas.Parent?.Controls.Remove(picCanvas);
            picCanvas.Visible = true;
            picCanvas.Dock = DockStyle.Fill;
            picCanvas.BorderStyle = BorderStyle.None;
            picCanvas.BackColor = Color.FromArgb(241, 245, 249);
            picCanvas.MouseDown -= PicCanvas_MouseDown;
            picCanvas.MouseMove -= PicCanvas_MouseMove;
            picCanvas.MouseUp -= PicCanvas_MouseUp;
            picCanvas.Paint -= PicCanvas_Paint;

            picCanvas.MouseDown += PicCanvas_MouseDown;
            picCanvas.MouseMove += PicCanvas_MouseMove;
            picCanvas.MouseUp += PicCanvas_MouseUp;
            picCanvas.Paint += PicCanvas_Paint;

            tuvalKart.Controls.Add(picCanvas);
            tuvalKart.Controls.Add(pnlTuvalUst);

            // Alt Analiz Şeridi
            ModernKartPanel altSerit = FormGezintiYardimcisi.AltAnalizSeridiOlustur(
                "Lütfen bir resim seçip kenarlardaki tutamaçlardan sürükleyin veya sürgüleri hareket ettirin.",
                out lblAnaliz,
                "Shear Dönüşümü"
            );

            anaYerlesim.Controls.Add(tuvalKart, 0, 0);
            anaYerlesim.Controls.Add(altSerit, 0, 1);
            pnlOrta.Controls.Add(anaYerlesim);

            this.Controls.Add(pnlOrta);
            FormGezintiYardimcisi.YerlesimiDuzenle(this, pnlUst, pnlSag, pnlOrta);
        }

        private ModernSurgu SurguEkle(
            Panel pnl,
            string baslik,
            int min,
            int max,
            int deger,
            int y,
            out Label lblDeger,
            Action<int> degistiginde)
        {
            Label lblAd = new Label
            {
                Text = baslik,
                Location = new Point(18, y),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi
            };
            pnl.Controls.Add(lblAd);

            lblDeger = new Label
            {
                Text = $"{deger / 100.0:F2}",
                Location = new Point(215, y),
                Size = new Size(65, 18),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi
            };
            pnl.Controls.Add(lblDeger);

            ModernSurgu surgu = new ModernSurgu
            {
                Minimum = min,
                Maximum = max,
                Value = deger,
                Location = new Point(18, y + 20),
                Size = new Size(262, 24)
            };
            surgu.Scroll += (s, e) => degistiginde?.Invoke(surgu.Value);
            pnl.Controls.Add(surgu);
            return surgu;
        }

        // 1. Resim Seçme ve Tüm Pikselleri Diziye Kaydetme
        private void btnResimSec_Click(object sender, EventArgs e)
        {
            string dosyaYolu = FormGezintiYardimcisi.ResimDosyasiSec("Eğme (Shear) Dönüşümü İçin Resim Seçiniz");
            if (string.IsNullOrEmpty(dosyaYolu)) return;

            orijinalBmp?.Dispose();
            using (Bitmap temp = new Bitmap(dosyaYolu))
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
            surguShx.Value = 0;
            surguShy.Value = 0;
            lblValShx.Text = "0.00";
            lblValShy.Text = "0.00";

            BilgiMetniniGuncelle();
            picCanvas.Invalidate();
        }

        private void btnSifirla_Click(object sender, EventArgs e)
        {
            egmeKatsayisiX = 0.0;
            egmeKatsayisiY = 0.0;
            surguShx.Value = 0;
            surguShy.Value = 0;
            lblValShx.Text = "0.00";
            lblValShy.Text = "0.00";

            BilgiMetniniGuncelle();
            picCanvas.Invalidate();
        }

        // 2. Fare Olayları: Kenarları Yakalama
        private void PicCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (pikselDizisi == null || (rbFareyleSurukle != null && !rbFareyleSurukle.Checked)) return;

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
            if (pikselDizisi == null || (rbFareyleSurukle != null && !rbFareyleSurukle.Checked))
            {
                picCanvas.Cursor = Cursors.Default;
                return;
            }

            KenarTipi k = HangiKenarUzerinde(e.Location);
            if (k == KenarTipi.Ust || k == KenarTipi.Alt)
                picCanvas.Cursor = Cursors.SizeWE;
            else if (k == KenarTipi.Sol || k == KenarTipi.Sag)
                picCanvas.Cursor = Cursors.SizeNS;
            else if (secilenKenar == KenarTipi.Yok)
                picCanvas.Cursor = Cursors.Default;

            if (e.Button == MouseButtons.Left && secilenKenar != KenarTipi.Yok)
            {
                int dx = e.X - fareBaslangicKonum.X;
                int dy = e.Y - fareBaslangicKonum.Y;

                if (secilenKenar == KenarTipi.Ust)
                    egmeKatsayisiX = baslangicKatsayiX + ((double)dx / resimYuksekligi);
                else if (secilenKenar == KenarTipi.Alt)
                    egmeKatsayisiX = baslangicKatsayiX - ((double)dx / resimYuksekligi);
                else if (secilenKenar == KenarTipi.Sag)
                    egmeKatsayisiY = baslangicKatsayiY + ((double)dy / resimGenisligi);
                else if (secilenKenar == KenarTipi.Sol)
                    egmeKatsayisiY = baslangicKatsayiY - ((double)dy / resimGenisligi);

                egmeKatsayisiX = Math.Clamp(egmeKatsayisiX, -1.2, 1.2);
                egmeKatsayisiY = Math.Clamp(egmeKatsayisiY, -1.2, 1.2);

                surguShx.Value = (int)Math.Round(egmeKatsayisiX * 100);
                surguShy.Value = (int)Math.Round(egmeKatsayisiY * 100);
                lblValShx.Text = $"{egmeKatsayisiX:F2}";
                lblValShy.Text = $"{egmeKatsayisiY:F2}";

                BilgiMetniniGuncelle();
                picCanvas.Invalidate();
            }
        }

        private void PicCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            secilenKenar = KenarTipi.Yok;
        }

        private void BilgiMetniniGuncelle()
        {
            rozetTuval?.MetinGuncelle($"Shx: {egmeKatsayisiX:F2} | Shy: {egmeKatsayisiY:F2}");

            if (pikselDizisi == null)
            {
                lblAnaliz.Text = "Lütfen bir resim seçiniz.";
                return;
            }

            lblFormulKart.Text =
                "Uygulanan Formüller:\n" +
                $"• Yatay: x₂ = x₁ {(egmeKatsayisiX >= 0 ? "+" : "-")} {Math.Abs(egmeKatsayisiX):F2} · y₁\n" +
                $"• Dikey: y₂ = {(egmeKatsayisiY >= 0 ? "+" : "-")}{Math.Abs(egmeKatsayisiY):F2} · x₁ + y₁\n\n" +
                $"Dizide Korunan Piksel:\n• {pikselDizisi.Length:N0} piksel ({resimGenisligi}×{resimYuksekligi} px)\n\n" +
                "İpucu: Siyah kenar tutamaçlarını sürükleyerek eğmeyi canlı değiştirebilirsiniz.";

            lblAnaliz.Text = $"Shx = {egmeKatsayisiX:F2}, Shy = {egmeKatsayisiY:F2} • Dizide korunan piksel: {pikselDizisi.Length:N0}";
        }

        private KenarTipi HangiKenarUzerinde(Point p)
        {
            int ox = (picCanvas.Width - resimGenisligi) / 2;
            int oy = (picCanvas.Height - resimYukseklikHesapla()) / 2;

            PointF pUst = new PointF(ox + (resimGenisligi / 2f) + (float)(egmeKatsayisiX * (-resimYuksekligi / 2f)), oy);
            PointF pAlt = new PointF(ox + (resimGenisligi / 2f) + (float)(egmeKatsayisiX * (resimYuksekligi / 2f)), oy + resimYuksekligi);
            PointF pSol = new PointF(ox, oy + (resimYuksekligi / 2f) - (float)(egmeKatsayisiY * (resimGenisligi / 2f)));
            PointF pSag = new PointF(ox + resimGenisligi, oy + (resimYuksekligi / 2f) + (float)(egmeKatsayisiY * (resimGenisligi / 2f)));

            int tol = 18;
            if (Math.Abs(p.X - pUst.X) <= tol && Math.Abs(p.Y - pUst.Y) <= tol) return KenarTipi.Ust;
            if (Math.Abs(p.X - pAlt.X) <= tol && Math.Abs(p.Y - pAlt.Y) <= tol) return KenarTipi.Alt;
            if (Math.Abs(p.X - pSol.X) <= tol && Math.Abs(p.Y - pSol.Y) <= tol) return KenarTipi.Sol;
            if (Math.Abs(p.X - pSag.X) <= tol && Math.Abs(p.Y - pSag.Y) <= tol) return KenarTipi.Sag;

            return KenarTipi.Yok;
        }

        private int resimYukseklikHesapla() => resimYuksekligi > 0 ? resimYuksekligi : 200;

        // 3. Orijinal Mavi Kutu + Eğilmiş Resim ve Tutamaç Çizimi
        private void PicCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Arka plana hafif koordinat ızgarası çiz
            using (Pen pIzgara = new Pen(Color.FromArgb(226, 232, 240), 1))
            {
                for (int x = 0; x < picCanvas.Width; x += 40)
                    g.DrawLine(pIzgara, x, 0, x, picCanvas.Height);
                for (int y = 0; y < picCanvas.Height; y += 40)
                    g.DrawLine(pIzgara, 0, y, picCanvas.Width, y);
            }

            if (pikselDizisi == null || orijinalBmp == null) return;

            int ox = (picCanvas.Width - resimGenisligi) / 2;
            int oy = (picCanvas.Height - resimYuksekligi) / 2;

            // 1. Orijinal Mavi Çerçeve (Sabit Referans Dikdörtgen)
            Rectangle orijinalKutu = new Rectangle(ox, oy, resimGenisligi, resimYuksekligi);
            using (Pen pMavi = new Pen(Color.FromArgb(14, 165, 233), 1.5f))
            {
                g.DrawRectangle(pMavi, orijinalKutu);
            }

            Point[] maviKoseler =
            {
                new Point(ox, oy),
                new Point(ox + resimGenisligi, oy),
                new Point(ox + resimGenisligi, oy + resimYuksekligi),
                new Point(ox, oy + resimYuksekligi)
            };

            int maviK = 6;
            using (Brush bMavi = new SolidBrush(Color.FromArgb(56, 189, 248)))
            {
                foreach (var mk in maviKoseler)
                {
                    g.FillRectangle(bMavi, mk.X - maviK / 2, mk.Y - maviK / 2, maviK, maviK);
                }
            }

            // 2. Eğilmiş Resim ve Damalı Arka Plan
            PointF kSolUst = FormulleDonustur(0, 0, ox, oy);
            PointF kSagUst = FormulleDonustur(resimGenisligi, 0, ox, oy);
            PointF kSagAlt = FormulleDonustur(resimGenisligi, resimYuksekligi, ox, oy);
            PointF kSolAlt = FormulleDonustur(0, resimYuksekligi, ox, oy);
            PointF[] koseler = { kSolUst, kSagUst, kSagAlt, kSolAlt };

            using (HatchBrush damaBrush = new HatchBrush(HatchStyle.LargeCheckerBoard, Color.White, Color.Gainsboro))
            {
                g.FillPolygon(damaBrush, koseler);
            }

            using (Matrix mat = new Matrix())
            {
                mat.Translate(ox + resimGenisligi / 2f, oy + resimYuksekligi / 2f);
                mat.Shear((float)egmeKatsayisiX, (float)egmeKatsayisiY);
                mat.Translate(-(ox + resimGenisligi / 2f), -(oy + resimYuksekligi / 2f));

                g.Transform = mat;
                g.DrawImage(orijinalBmp, ox, oy, resimGenisligi, resimYuksekligi);
                g.ResetTransform();
            }

            // 3. Eğilmiş Resmin Çerçevesi ve Siyah Tutamaçları
            using (Pen cercevePen = new Pen(Color.FromArgb(30, 41, 59), 1.4f))
            {
                g.DrawPolygon(cercevePen, koseler);
            }

            PointF ustOrta = new PointF((kSolUst.X + kSagUst.X) / 2f, (kSolUst.Y + kSagUst.Y) / 2f);
            PointF altOrta = new PointF((kSolAlt.X + kSagAlt.X) / 2f, (kSolAlt.Y + kSagAlt.Y) / 2f);
            PointF solOrta = new PointF((kSolUst.X + kSolAlt.X) / 2f, (kSolUst.Y + kSolAlt.Y) / 2f);
            PointF sagOrta = new PointF((kSagUst.X + kSagAlt.X) / 2f, (kSagUst.Y + kSagAlt.Y) / 2f);
            PointF merkezNokta = new PointF(ox + resimGenisligi / 2f, oy + resimYuksekligi / 2f);

            PointF[] siyahTutamaclar = { kSolUst, kSagUst, kSagAlt, kSolAlt, ustOrta, altOrta, solOrta, sagOrta, merkezNokta };

            int k = 8;
            using (Brush bSiyah = new SolidBrush(Color.FromArgb(15, 23, 42)))
            using (Pen pBeyaz = new Pen(Color.White, 1.3f))
            {
                foreach (var pt in siyahTutamaclar)
                {
                    RectangleF r = new RectangleF(pt.X - k / 2f, pt.Y - k / 2f, k, k);
                    g.FillRectangle(bSiyah, r);
                    g.DrawRectangle(pBeyaz, r.X, r.Y, r.Width, r.Height);
                }
            }
        }

        private PointF FormulleDonustur(int x1, int y1, int ox, int oy)
        {
            double relX = x1 - (resimGenisligi / 2.0);
            double relY = y1 - (resimYuksekligi / 2.0);

            double x2 = relX + (egmeKatsayisiX * relY);
            double y2 = relY + (egmeKatsayisiY * relX);

            return new PointF(
                (float)(ox + (resimGenisligi / 2.0) + x2),
                (float)(oy + (resimYuksekligi / 2.0) + y2)
            );
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            orijinalBmp?.Dispose();
            base.OnFormClosed(e);
        }
    }
}