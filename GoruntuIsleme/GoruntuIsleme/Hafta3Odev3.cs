using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

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

        private ModernHapRozet rozetTuval;
        private Label lblEksenKart;
        private Label lblAnaliz;

        public Hafta3Odev3()
        {
            InitializeComponent();
            ModernArayuzuUygula();
        }

        private void ModernArayuzuUygula()
        {
            // 1. Form Temel Ayarları (1250x750, Standart Kenarlık ve X ile Ana Menüye Dönüş)
            FormGezintiYardimcisi.FormuHazirla(this);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Text = "Hafta 3 - Ödev 3: İki Nokta Arasındaki Eksen Etrafında Aynalama";

            // Designer'daki tüm eski kontrolleri gizle
            foreach (Control ctrl in this.Controls)
            {
                ctrl.Visible = false;
            }

            // 2. Üst Gezinti Barı
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                form: this,
                baslik: "Hafta 3 - Ödev 3 | İki Nokta (A - B) Arasındaki Eksen Etrafında Aynalama",
                oncekiYazi: "< Ödev 2",
                oncekiTikla: () =>
                {
                    Form odev2 = Application.OpenForms["Hafta3Odev2"];
                    if (odev2 != null)
                        FormGezintiYardimcisi.FormGecisYap(this, odev2);
                    else
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta3Odev2());
                },
                sonrakiYazi: "Ödev 4 >",
                sonrakiTikla: () =>
                {
                    Form odev4 = Application.OpenForms["Hafta3Odev4"];
                    if (odev4 != null)
                        FormGezintiYardimcisi.FormGecisYap(this, odev4);
                    else
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta3Odev4());
                }
            );

            // 3. Sağ Kontrol Paneli
            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 300);

            Label lblKontrolBaslik = new Label
            {
                Text = "Eksen & Aynalama Kontrolü",
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

            // Eksen Çizgisine Göre Aynala Butonu
            ModernHapButon btnModernAynala = FormGezintiYardimcisi.ModernButonOlustur("⚡ Eksen Çizgisine Göre Aynala", true, 262, 42);
            btnModernAynala.Location = new Point(18, 94);
            btnModernAynala.Click += btnAynala_Click;
            pnlSag.Controls.Add(btnModernAynala);

            // Aynalama Adımları Kartı
            ModernKartPanel adimKarti = new ModernKartPanel
            {
                Location = new Point(18, 148),
                Size = new Size(262, 126),
                Padding = new Padding(12)
            };

            Label lblAdimBaslik = new Label
            {
                Text = "Aynalama Adımları",
                Location = new Point(12, 10),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 9.3f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi
            };
            adimKarti.Controls.Add(lblAdimBaslik);

            Label lblAdimMetin = new Label
            {
                Text =
                    "1. 'Resim Seç' ile görüntüyü yükleyin.\n" +
                    "2. Tuvalde eksen doğrusu için A ve B noktalarına tıklayın.\n" +
                    "3. 'Eksen Çizgisine Göre Aynala' butonuna basın.",
                Location = new Point(12, 34),
                Size = new Size(238, 82),
                Font = new Font("Segoe UI", 8.9f),
                ForeColor = FormGezintiYardimcisi.KoyuYazi
            };
            adimKarti.Controls.Add(lblAdimMetin);
            pnlSag.Controls.Add(adimKarti);

            // Açık Mavi Eksen ve Formül Değerleri Kartı
            ModernKartPanel eksenBilgiKarti = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                konum: new Point(18, 286),
                boyut: new Size(262, 275),
                icerikEtiketi: out lblEksenKart
            );
            lblEksenKart.Text =
                "Eksen ve Formül Değerleri:\n\n" +
                "• A Noktası: Seçilmedi\n" +
                "• B Noktası: Seçilmedi\n" +
                "• Eğim (m): —\n" +
                "• Açı (θ): —\n\n" +
                "Formül (Backward Mapping):\n" +
                "Δ = (x₂ - x₀)·sin(θ) - (y₂ - y₀)·cos(θ)\n" +
                "x₁ = x₂ - 2·Δ·sin(θ)\n" +
                "y₁ = y₂ + 2·Δ·cos(θ)";
            pnlSag.Controls.Add(eksenBilgiKarti);

            // 4. Orta Çalışma Alanı (Üst: Çalışma Tuvali | Alt: Analiz Şeridi)
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
                Text = "Çalışma Tuvali (Eksen Doğrusu İçin A ve B Noktalarını İşaretleyin)",
                UseMnemonic = false,
                AutoSize = true,
                Location = new Point(0, 4),
                Font = new Font("Segoe UI Semibold", 9.8f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi
            };
            pnlTuvalUst.Controls.Add(lblTuvalBaslik);

            rozetTuval = new ModernHapRozet();
            rozetTuval.MetinGuncelle("A & B Bekleniyor");
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
            picCanvas.Cursor = Cursors.Cross;
            picCanvas.MouseDown -= PicCanvas_MouseDown;
            picCanvas.Paint -= PicCanvas_Paint;

            picCanvas.MouseDown += PicCanvas_MouseDown;
            picCanvas.Paint += PicCanvas_Paint;

            tuvalKart.Controls.Add(picCanvas);
            tuvalKart.Controls.Add(pnlTuvalUst);

            // Alt Analiz Şeridi
            ModernKartPanel altSerit = FormGezintiYardimcisi.AltAnalizSeridiOlustur(
                "Resim seçip tuval üzerinde aynalama eksenini tanımlayan 2 noktaya (A ve B) tıklayınız.",
                out lblAnaliz,
                "Eksen Aynalama"
            );

            anaYerlesim.Controls.Add(tuvalKart, 0, 0);
            anaYerlesim.Controls.Add(altSerit, 0, 1);
            pnlOrta.Controls.Add(anaYerlesim);

            this.Controls.Add(pnlOrta);
            FormGezintiYardimcisi.YerlesimiDuzenle(this, pnlUst, pnlSag, pnlOrta);
        }

        // Resim Seçme
        private void btnResimSec_Click(object sender, EventArgs e)
        {
            string dosyaYolu = FormGezintiYardimcisi.ResimDosyasiSec("Eksen Etrafında Aynalama İçin Resim Seçiniz");
            if (string.IsNullOrEmpty(dosyaYolu)) return;

            orijinalBmp?.Dispose();
            aynalanmisBmp?.Dispose();
            aynalanmisBmp = null;

            using (Bitmap temp = new Bitmap(dosyaYolu))
            {
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

            resimKonum = new Point(60, 60);
            noktaA = null;
            noktaB = null;

            rozetTuval.MetinGuncelle("A & B Bekleniyor");
            lblAnaliz.Text = $"Resim yüklendi ({orijinalBmp.Width}×{orijinalBmp.Height} px) • Tuval üzerinde 1. noktaya (A) tıklayınız.";
            picCanvas.Invalidate();
        }

        // Sıfırlama
        private void btnSifirla_Click(object sender, EventArgs e)
        {
            aynalanmisBmp?.Dispose();
            aynalanmisBmp = null;
            noktaA = null;
            noktaB = null;

            rozetTuval.MetinGuncelle("A & B Bekleniyor");
            lblEksenKart.Text =
                "Eksen ve Formül Değerleri:\n\n" +
                "• A Noktası: Seçilmedi\n" +
                "• B Noktası: Seçilmedi\n" +
                "• Eğim (m): —\n" +
                "• Açı (θ): —\n\n" +
                "Formül (Backward Mapping):\n" +
                "Δ = (x₂ - x₀)·sin(θ) - (y₂ - y₀)·cos(θ)\n" +
                "x₁ = x₂ - 2·Δ·sin(θ)\n" +
                "y₁ = y₂ + 2·Δ·cos(θ)";
            lblAnaliz.Text = "Sıfırlandı • Tuval üzerinden yeni A ve B noktaları seçebilirsiniz.";
            picCanvas.Invalidate();
        }

        // Tuvalde 2 Nokta Seçme
        private void PicCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            if (!noktaA.HasValue)
            {
                noktaA = e.Location;
                noktaB = null;

                rozetTuval.MetinGuncelle("A Seçildi • B Bekleniyor");
                lblAnaliz.Text = $"A Noktası seçildi: ({noktaA.Value.X}, {noktaA.Value.Y}) • Şimdi eksenin geçeceği 2. noktaya (B) tıklayınız.";
            }
            else if (!noktaB.HasValue)
            {
                if (e.Location == noktaA.Value) return;
                noktaB = e.Location;

                double dx = noktaB.Value.X - noktaA.Value.X;
                double dy = noktaB.Value.Y - noktaA.Value.Y;

                egim_m = dx != 0 ? (dy / dx) : double.PositiveInfinity;
                radyanAci = Math.Atan2(dy, dx);
                dereceAci = radyanAci * 180.0 / Math.PI;

                string egimMetni = double.IsInfinity(egim_m) ? "Dikey (∞)" : egim_m.ToString("F3");

                rozetTuval.MetinGuncelle($"θ = {dereceAci:F1}°");
                lblEksenKart.Text =
                    "Eksen ve Formül Değerleri:\n\n" +
                    $"• A Noktası: ({noktaA.Value.X}, {noktaA.Value.Y})\n" +
                    $"• B Noktası: ({noktaB.Value.X}, {noktaB.Value.Y})\n" +
                    $"• Eğim (m): {egimMetni}\n" +
                    $"• Açı (θ): {dereceAci:F2}°\n\n" +
                    "Aynalamayı başlatmak için 'Eksen Çizgisine Göre Aynala' butonuna basınız.";
                lblAnaliz.Text = $"Eksen belirlendi • A({noktaA.Value.X}, {noktaA.Value.Y}) – B({noktaB.Value.X}, {noktaB.Value.Y}) • Eğim m={egimMetni}, θ={dereceAci:F2}°";
            }
            else
            {
                noktaA = e.Location;
                noktaB = null;
                aynalanmisBmp?.Dispose();
                aynalanmisBmp = null;

                rozetTuval.MetinGuncelle("A Seçildi • B Bekleniyor");
                lblAnaliz.Text = $"Yeni A Noktası seçildi: ({noktaA.Value.X}, {noktaA.Value.Y}) • B noktasını seçiniz.";
            }

            picCanvas.Invalidate();
        }

        // Backward Mapping ile Tüm Tuval Boyutunda Kayıpsız Aynalama
        private void btnAynala_Click(object sender, EventArgs e)
        {
            if (orijinalBmp == null || !noktaA.HasValue || !noktaB.HasValue)
            {
                MessageBox.Show("Lütfen bir resim yükleyip eksen için 2 noktaya (A ve B) tıklayınız!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.Cursor = Cursors.WaitCursor;

            int cw = picCanvas.ClientSize.Width;
            int ch = picCanvas.ClientSize.Height;

            aynalanmisBmp?.Dispose();
            aynalanmisBmp = new Bitmap(cw, ch);

            double x0 = noktaA.Value.X;
            double y0 = noktaA.Value.Y;

            double sinTheta = Math.Sin(radyanAci);
            double cosTheta = Math.Cos(radyanAci);

            int imgW = orijinalBmp.Width;
            int imgH = orijinalBmp.Height;

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

            string egimMetni = double.IsInfinity(egim_m) ? "Dikey (∞)" : egim_m.ToString("F3");
            lblEksenKart.Text =
                "Aynalama Tamamlandı:\n\n" +
                $"• Referans (x₀, y₀): ({x0}, {y0})\n" +
                $"• Eğim (m): {egimMetni}\n" +
                $"• Eksen Açısı (θ): {dereceAci:F2}°\n\n" +
                "Uygulanan Formül:\n" +
                "Δ = (x - x₀)sinθ - (y - y₀)cosθ\n" +
                "x₁ = x₂ - 2Δsinθ, y₁ = y₂ + 2Δcosθ";

            lblAnaliz.Text = $"Aynalama tamamlandı • Referans: ({x0}, {y0}) • Eksen Açısı θ = {dereceAci:F2}°";
            picCanvas.Invalidate();
            this.Cursor = Cursors.Default;
        }

        // Çizim Metodu (Izgara, Orijinal Resim, Aynalanmış Resim, Eksen ve A-B Noktaları)
        private void PicCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Arka plan koordinat ızgarası
            using (Pen pIzgara = new Pen(Color.FromArgb(226, 232, 240), 1))
            {
                for (int x = 0; x < picCanvas.Width; x += 40)
                    g.DrawLine(pIzgara, x, 0, x, picCanvas.Height);
                for (int y = 0; y < picCanvas.Height; y += 40)
                    g.DrawLine(pIzgara, 0, y, picCanvas.Width, y);
            }

            // 1. Orijinal Resim
            if (orijinalBmp != null)
            {
                g.DrawImage(orijinalBmp, resimKonum.X, resimKonum.Y, orijinalBmp.Width, orijinalBmp.Height);
                using (Pen p = new Pen(Color.FromArgb(148, 163, 184), 1.5f))
                {
                    g.DrawRectangle(p, resimKonum.X, resimKonum.Y, orijinalBmp.Width, orijinalBmp.Height);
                }
            }

            // 2. Aynalanmış Resim
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

                    PointF baslangic = new PointF(noktaA.Value.X - (ux * 1500), noktaA.Value.Y - (uy * 1500));
                    PointF bitis = new PointF(noktaA.Value.X + (ux * 1500), noktaA.Value.Y + (uy * 1500));

                    using (Pen pEksen = new Pen(Color.FromArgb(14, 165, 233), 2.5f) { DashStyle = DashStyle.Dash })
                    {
                        g.DrawLine(pEksen, baslangic, bitis);
                    }
                }
            }

            // 4. A Noktası (Kırmızı Kare)
            if (noktaA.HasValue)
            {
                int k = 10;
                using (Brush b = new SolidBrush(Color.FromArgb(239, 68, 68)))
                using (Pen p = new Pen(Color.White, 2))
                {
                    g.FillRectangle(b, noktaA.Value.X - k / 2, noktaA.Value.Y - k / 2, k, k);
                    g.DrawRectangle(p, noktaA.Value.X - k / 2, noktaA.Value.Y - k / 2, k, k);
                }
                using (Font f = new Font("Segoe UI", 9, FontStyle.Bold))
                using (Brush bYazi = new SolidBrush(Color.FromArgb(220, 38, 38)))
                {
                    g.DrawString("A (x₁, y₁)", f, bYazi, noktaA.Value.X + 8, noktaA.Value.Y - 8);
                }
            }

            // 5. B Noktası (Mavi Kare)
            if (noktaB.HasValue)
            {
                int k = 10;
                using (Brush b = new SolidBrush(Color.FromArgb(37, 99, 235)))
                using (Pen p = new Pen(Color.White, 2))
                {
                    g.FillRectangle(b, noktaB.Value.X - k / 2, noktaB.Value.Y - k / 2, k, k);
                    g.DrawRectangle(p, noktaB.Value.X - k / 2, noktaB.Value.Y - k / 2, k, k);
                }
                using (Font f = new Font("Segoe UI", 9, FontStyle.Bold))
                using (Brush bYazi = new SolidBrush(Color.FromArgb(29, 78, 216)))
                {
                    g.DrawString("B (x₂, y₂)", f, bYazi, noktaB.Value.X + 8, noktaB.Value.Y - 8);
                }
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            orijinalBmp?.Dispose();
            aynalanmisBmp?.Dispose();
            base.OnFormClosed(e);
        }
    }
}