using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace GoruntuIsleme
{
    public partial class Hafta3Odev1 : Form
    {
        private Mat orijinalMat = null;
        private Mat guncelMat = null;

        // Taşıma koordinatları (Resmin Canvas üzerindeki konumu)
        private Point resimKonum = new Point(50, 50);

        // a) Tıklayarak taşıma için noktalar
        private Point? ilkTikNokta = null;
        private Point? ikinciTikNokta = null;

        // b) Basılı tutarak taşıma (Drag & Drop) kontrolü
        private bool surukleniyor = false;
        private Point suruklemeBaslangicFare;
        private Point suruklemeBaslangicResim;

        private ModernHapRozet rozetTuval;
        private ModernSurgu surguAci;
        private Label lblValAci;
        private Label lblAnaliz;

        public Hafta3Odev1()
        {
            InitializeComponent();
            ModernArayuzuUygula();
        }

        private void ModernArayuzuUygula()
        {
            // 1. Form Temel Ayarları (1250x750, Standart Kenarlık ve X ile Ana Menüye Dönüş)
            FormGezintiYardimcisi.FormuHazirla(this);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Text = "Hafta 3 - Ödev 1: Resim Taşıma ve Açılı Aynalama";

            // Designer'daki tüm eski kontrolleri gizle
            foreach (Control ctrl in this.Controls)
            {
                ctrl.Visible = false;
            }

            // 2. Üst Gezinti Barı (Hafta 3'ün ilk ödevi olduğu için önceki linki yok)
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                form: this,
                baslik: "Hafta 3 - Ödev 1 | Resim Taşıma (2 Nokta / Sürükleme) ve Açılı Aynalama",
                oncekiYazi: "",
                oncekiTikla: null,
                sonrakiYazi: "Ödev 2 >",
                sonrakiTikla: () =>
                {
                    Form odev2 = Application.OpenForms["Hafta3Odev2"];
                    if (odev2 != null)
                        FormGezintiYardimcisi.FormGecisYap(this, odev2);
                    else
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta3Odev2());
                }
            );

            // 3. Sağ Kontrol Paneli
            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 300);

            Label lblKontrolBaslik = new Label
            {
                Text = "Taşıma & Aynalama",
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

            // İşlem Modu Seçimi Kartı (Designer RadioButton'larını içeri taşıyarak koruyoruz)
            ModernKartPanel modKart = new ModernKartPanel
            {
                Location = new Point(18, 94),
                Size = new Size(262, 124),
                Padding = new Padding(10)
            };

            Label lblModBaslik = new Label
            {
                Text = "İşlem Modu Seçimi",
                Location = new Point(12, 8),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 9.2f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi
            };
            modKart.Controls.Add(lblModBaslik);

            rbTiklayarakTasi.Parent?.Controls.Remove(rbTiklayarakTasi);
            rbTiklayarakTasi.Visible = true;
            rbTiklayarakTasi.Location = new Point(12, 32);
            rbTiklayarakTasi.Font = new Font("Segoe UI", 9.1f, FontStyle.Bold);
            rbTiklayarakTasi.ForeColor = FormGezintiYardimcisi.KoyuYazi;
            rbTiklayarakTasi.Text = "a) Tıklayarak Taşıma (2 Nokta)";
            rbTiklayarakTasi.Checked = true;
            rbTiklayarakTasi.CheckedChanged += (s, e) => { ilkTikNokta = null; ikinciTikNokta = null; TuvaliCiz(); };
            modKart.Controls.Add(rbTiklayarakTasi);

            rbSurukleTasi.Parent?.Controls.Remove(rbSurukleTasi);
            rbSurukleTasi.Visible = true;
            rbSurukleTasi.Location = new Point(12, 60);
            rbSurukleTasi.Font = new Font("Segoe UI", 9.1f, FontStyle.Bold);
            rbSurukleTasi.ForeColor = FormGezintiYardimcisi.KoyuYazi;
            rbSurukleTasi.Text = "b) Basılı Tutup Sürükleme";
            rbSurukleTasi.CheckedChanged += (s, e) => { ilkTikNokta = null; ikinciTikNokta = null; TuvaliCiz(); };
            modKart.Controls.Add(rbSurukleTasi);

            rbAynala.Parent?.Controls.Remove(rbAynala);
            rbAynala.Visible = true;
            rbAynala.Location = new Point(12, 88);
            rbAynala.Font = new Font("Segoe UI", 9.1f, FontStyle.Bold);
            rbAynala.ForeColor = FormGezintiYardimcisi.KoyuYazi;
            rbAynala.Text = "c) Açılı Aynalama (Formül)";
            rbAynala.CheckedChanged += (s, e) => { ilkTikNokta = null; ikinciTikNokta = null; TuvaliCiz(); };
            modKart.Controls.Add(rbAynala);

            pnlSag.Controls.Add(modKart);

            // Açı (θ) Modern Sürgüsü
            Label lblAciBaslik = new Label
            {
                Text = "Ayna Ekseni Açısı (θ)",
                Location = new Point(18, 232),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi
            };
            pnlSag.Controls.Add(lblAciBaslik);

            lblValAci = new Label
            {
                Text = "45°",
                Location = new Point(220, 232),
                Size = new Size(60, 18),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi
            };
            pnlSag.Controls.Add(lblValAci);

            surguAci = new ModernSurgu
            {
                Minimum = 0,
                Maximum = 180,
                Value = 45,
                Location = new Point(18, 252),
                Size = new Size(262, 24)
            };
            surguAci.Scroll += (s, e) =>
            {
                lblValAci.Text = $"{surguAci.Value}°";
                TuvaliCiz();
            };
            pnlSag.Controls.Add(surguAci);

            // Aynalamayı Uygula Butonu
            ModernHapButon btnModernAynala = FormGezintiYardimcisi.ModernButonOlustur("🪞 Aynalamayı Uygula (Formül)", true, 262, 38);
            btnModernAynala.Location = new Point(18, 286);
            btnModernAynala.Click += btnAynala_Click;
            pnlSag.Controls.Add(btnModernAynala);

            // Açık Mavi Formül ve Kullanım Kartı
            ModernKartPanel bilgiKarti = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                konum: new Point(18, 336),
                boyut: new Size(262, 240),
                icerikEtiketi: out Label lblAciklamaKart
            );
            lblAciklamaKart.Text =
                "Açılı Aynalama Formülü:\n" +
                "Δ = (x₂ - x₀)·sin(θ) - (y₂ - y₀)·cos(θ)\n" +
                "x₁ = x₂ - 2·Δ·sin(θ)\n" +
                "y₁ = y₂ + 2·Δ·cos(θ)\n\n" +
                "• a) Modu: Önce resim üstüne, sonra hedef konuma tıklayın.\n" +
                "• b) Modu: Resmi basılı tutup sürükleyin.";
            pnlSag.Controls.Add(bilgiKarti);

            // 4. Orta Çalışma Alanı (Üst: Çalışma Tuvali Kartı | Alt: Analiz Şeridi)
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
                Text = "İnteraktif Çalışma Tuvali (Öteleme & Açılı Simetri)",
                UseMnemonic = false,
                AutoSize = true,
                Location = new Point(0, 4),
                Font = new Font("Segoe UI Semibold", 9.8f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi
            };
            pnlTuvalUst.Controls.Add(lblTuvalBaslik);

            rozetTuval = new ModernHapRozet();
            rozetTuval.MetinGuncelle("Konum: (50, 50)");
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
                "Lütfen bir resim seçin ve sağ panelden işlem modunu (Tıklayarak Taşıma, Sürükleme veya Açılı Aynalama) belirleyin.",
                out lblAnaliz,
                "Geometrik Dönüşüm"
            );

            anaYerlesim.Controls.Add(tuvalKart, 0, 0);
            anaYerlesim.Controls.Add(altSerit, 0, 1);
            pnlOrta.Controls.Add(anaYerlesim);

            this.Controls.Add(pnlOrta);
            FormGezintiYardimcisi.YerlesimiDuzenle(this, pnlUst, pnlSag, pnlOrta);
        }

        private void btnResimSec_Click(object sender, EventArgs e)
        {
            string dosyaYolu = FormGezintiYardimcisi.ResimDosyasiSec("Geometrik Dönüşüm İçin Resim Seçiniz");
            if (string.IsNullOrEmpty(dosyaYolu)) return;

            orijinalMat?.Dispose();
            guncelMat?.Dispose();

            orijinalMat = Cv2.ImRead(dosyaYolu, ImreadModes.Color);
            if (orijinalMat.Empty())
            {
                MessageBox.Show("Resim yüklenemedi!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            guncelMat = orijinalMat.Clone();
            resimKonum = new Point(50, 50);
            ilkTikNokta = null;
            ikinciTikNokta = null;

            rozetTuval.MetinGuncelle($"Konum: ({resimKonum.X}, {resimKonum.Y})");
            lblAnaliz.Text = $"Resim yüklendi ({orijinalMat.Width}×{orijinalMat.Height} px) • Seçili mod ile tuval üzerinde işlem yapabilirsiniz.";
            TuvaliCiz();
        }

        private void btnSifirla_Click(object sender, EventArgs e)
        {
            if (orijinalMat == null) return;

            guncelMat?.Dispose();
            guncelMat = orijinalMat.Clone();
            resimKonum = new Point(50, 50);
            ilkTikNokta = null;
            ikinciTikNokta = null;

            rozetTuval.MetinGuncelle("Konum: (50, 50)");
            lblAnaliz.Text = "Resim ve tuval konumu başlangıç haline sıfırlandı.";
            TuvaliCiz();
        }

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
                        lblAnaliz.Text = $"1. Nokta seçildi: ({e.X}, {e.Y}) • Şimdi taşımak istediğiniz 2. hedef konuma tıklayın.";
                        TuvaliCiz();
                    }
                    else
                    {
                        MessageBox.Show("Lütfen ilk noktayı resmin üzerinden seçiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    ikinciTikNokta = e.Location;

                    int dx = ikinciTikNokta.Value.X - ilkTikNokta.Value.X;
                    int dy = ikinciTikNokta.Value.Y - ilkTikNokta.Value.Y;

                    resimKonum = new Point(resimKonum.X + dx, resimKonum.Y + dy);
                    rozetTuval.MetinGuncelle($"Konum: ({resimKonum.X}, {resimKonum.Y})");
                    lblAnaliz.Text = $"Resim taşındı • Öteleme vektörü: dx = {dx}, dy = {dy} • Yeni konum: ({resimKonum.X}, {resimKonum.Y})";

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
                    picCanvas.Cursor = Cursors.SizeAll;
                }
            }
        }

        private void PicCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (surukleniyor && guncelMat != null && rbSurukleTasi.Checked)
            {
                int dx = e.X - suruklemeBaslangicFare.X;
                int dy = e.Y - suruklemeBaslangicFare.Y;

                resimKonum = new Point(suruklemeBaslangicResim.X + dx, suruklemeBaslangicResim.Y + dy);
                rozetTuval.MetinGuncelle($"Konum: ({resimKonum.X}, {resimKonum.Y})");
                lblAnaliz.Text = $"Sürükleniyor... Güncel konum: ({resimKonum.X}, {resimKonum.Y})";
                TuvaliCiz();
            }
        }

        private void PicCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (surukleniyor)
            {
                surukleniyor = false;
                picCanvas.Cursor = Cursors.Default;
                lblAnaliz.Text = $"Sürükleme tamamlandı • Yeni konum: ({resimKonum.X}, {resimKonum.Y})";
            }
        }

        // c) Formül ile Açılı Aynalama
        private void btnAynala_Click(object sender, EventArgs e)
        {
            if (orijinalMat == null)
            {
                MessageBox.Show("Lütfen önce bir resim seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (rbAynala != null) rbAynala.Checked = true;
            this.Cursor = Cursors.WaitCursor;

            int w = orijinalMat.Width;
            int h = orijinalMat.Height;

            double x0 = w / 2.0;
            double y0 = h / 2.0;

            double thetaDerece = surguAci != null ? surguAci.Value : 45.0;
            double thetaRadyan = thetaDerece * Math.PI / 180.0;

            double sinTheta = Math.Sin(thetaRadyan);
            double cosTheta = Math.Cos(thetaRadyan);

            Mat aynalanmisMat = new Mat(h, w, MatType.CV_8UC3, new Scalar(30, 41, 59));

            for (int y2 = 0; y2 < h; y2++)
            {
                for (int x2 = 0; x2 < w; x2++)
                {
                    double delta = ((x2 - x0) * sinTheta) - ((y2 - y0) * cosTheta);

                    int x1 = (int)Math.Round(x2 + (2.0 * delta * (-sinTheta)));
                    int y1 = (int)Math.Round(y2 + (2.0 * delta * cosTheta));

                    if (x1 >= 0 && x1 < w && y1 >= 0 && y1 < h)
                    {
                        aynalanmisMat.Set(y2, x2, orijinalMat.At<Vec3b>(y1, x1));
                    }
                }
            }

            guncelMat?.Dispose();
            guncelMat = aynalanmisMat;

            lblAnaliz.Text = $"Açılı aynalama formülü uygulandı • Merkez: ({x0:F0}, {y0:F0}), Ayna Açısı θ = {thetaDerece}°";
            TuvaliCiz();
            this.Cursor = Cursors.Default;
        }

        private void TuvaliCiz()
        {
            picCanvas?.Invalidate();
        }

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

            if (guncelMat != null)
            {
                using (Bitmap bmp = BitmapConverter.ToBitmap(guncelMat))
                {
                    g.DrawImage(bmp, resimKonum.X, resimKonum.Y, guncelMat.Width, guncelMat.Height);
                }

                using (Pen pCerceve = new Pen(Color.FromArgb(148, 163, 184), 1.5f))
                {
                    g.DrawRectangle(pCerceve, resimKonum.X, resimKonum.Y, guncelMat.Width, guncelMat.Height);
                }

                if (rbAynala != null && rbAynala.Checked && surguAci != null)
                {
                    double x0 = resimKonum.X + (guncelMat.Width / 2.0);
                    double y0 = resimKonum.Y + (guncelMat.Height / 2.0);
                    double aciRad = surguAci.Value * Math.PI / 180.0;

                    float dx = (float)(Math.Cos(aciRad) * 220);
                    float dy = (float)(Math.Sin(aciRad) * 220);

                    using (Pen pAyna = new Pen(Color.FromArgb(14, 165, 233), 2.5f) { DashStyle = DashStyle.Dash })
                    {
                        g.DrawLine(pAyna, (float)x0 - dx, (float)y0 - dy, (float)x0 + dx, (float)y0 + dy);
                    }
                }
            }

            if (ilkTikNokta.HasValue)
            {
                using (Brush b = new SolidBrush(Color.FromArgb(239, 68, 68)))
                using (Pen p = new Pen(Color.White, 2))
                {
                    int k = 10;
                    g.FillEllipse(b, ilkTikNokta.Value.X - (k / 2), ilkTikNokta.Value.Y - (k / 2), k, k);
                    g.DrawEllipse(p, ilkTikNokta.Value.X - (k / 2), ilkTikNokta.Value.Y - (k / 2), k, k);
                }
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            orijinalMat?.Dispose();
            guncelMat?.Dispose();
            base.OnFormClosed(e);
        }
    }
}