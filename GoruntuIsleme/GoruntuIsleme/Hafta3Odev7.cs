using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace GoruntuIsleme
{
    public partial class Hafta3Odev7 : Form
    {
        private Bitmap orijinalBmp = null;
        private Point? donmeMerkezi = null;
        private bool artiGoster = false;

        // Kaynak resmin PictureBox içindeki orantılı çizim alanı
        private Rectangle cizimAlani = Rectangle.Empty;

        private ModernHapRozet rozetKaynak;
        private ModernHapRozet rozetHedef;
        private ModernSurgu surguAci;
        private Label lblMerkezKart;
        private Label lblAnaliz;

        public Hafta3Odev7()
        {
            InitializeComponent();
            ModernArayuzuUygula();
        }

        private void ModernArayuzuUygula()
        {
            // 1. Form Temel Ayarları (1250x750, Standart Kenarlık ve X ile Ana Menüye Dönüş)
            FormGezintiYardimcisi.FormuHazirla(this);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Text = "Hafta 3 - Ödev 7: Belirlenen Merkez Etrafında Döndürme (Rotation)";

            // Designer'daki tüm eski kontrolleri gizle
            foreach (Control ctrl in this.Controls)
            {
                ctrl.Visible = false;
            }

            // 2. Üst Gezinti Barı
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                form: this,
                baslik: "Hafta 3 - Ödev 7 | Belirlenen Merkez (x₀, y₀) Etrafında Görüntü Döndürme",
                oncekiYazi: "< Ödev 6",
                oncekiTikla: () =>
                {
                    Form odev6 = Application.OpenForms["Hafta3Odev6"];
                    if (odev6 != null)
                        FormGezintiYardimcisi.FormGecisYap(this, odev6);
                    else
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta3Odev6());
                },
                sonrakiYazi: "Ödev 8 >",
                sonrakiTikla: () =>
                {
                    Form odev8 = Application.OpenForms["Hafta3Odev8"];
                    if (odev8 != null)
                        FormGezintiYardimcisi.FormGecisYap(this, odev8);
                    else
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta3Odev8());
                }
            );

            // 3. Sağ Kontrol Paneli
            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 290);

            Label lblKontrolBaslik = new Label
            {
                Text = "Döndürme (Rotation) Ayarları",
                Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                Location = new Point(18, 16),
                AutoSize = true
            };
            pnlSag.Controls.Add(lblKontrolBaslik);

            // Resim Seç Butonu
            ModernHapButon btnModernResimSec = FormGezintiYardimcisi.ModernButonOlustur("📁 Resim Seç", true, 252, 38);
            btnModernResimSec.Location = new Point(18, 48);
            btnModernResimSec.Click += btnResimSec_Click;
            pnlSag.Controls.Add(btnModernResimSec);

            // Dönme Açısı Etiketi ve Sayısal Giriş Kutusu (txtAci)
            Label lblAciBaslik = new Label
            {
                Text = "Dönme Açısı (°):",
                Location = new Point(18, 104),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 9.2f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi
            };
            pnlSag.Controls.Add(lblAciBaslik);

            txtAci.Parent?.Controls.Remove(txtAci);
            txtAci.Visible = true;
            txtAci.Text = "45";
            txtAci.Location = new Point(185, 100);
            txtAci.Size = new Size(85, 26);
            txtAci.Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold);
            txtAci.TextAlign = HorizontalAlignment.Center;
            txtAci.TextChanged += (s, e) =>
            {
                if (double.TryParse(txtAci.Text, out double val))
                {
                    int clamped = Math.Clamp((int)Math.Round(val), -180, 180);
                    if (surguAci != null && surguAci.Value != clamped)
                        surguAci.Value = clamped;
                }
            };
            pnlSag.Controls.Add(txtAci);

            // Modern Sürgü (-180° ile +180° arası hızlı açı seçimi)
            surguAci = new ModernSurgu
            {
                Minimum = -180,
                Maximum = 180,
                Value = 45,
                Location = new Point(18, 132),
                Size = new Size(252, 24)
            };
            surguAci.Scroll += (s, e) =>
            {
                txtAci.Text = surguAci.Value.ToString();
            };
            pnlSag.Controls.Add(surguAci);

            // Resmi Döndür Butonu
            ModernHapButon btnModernDondur = FormGezintiYardimcisi.ModernButonOlustur("🔄 Resmi Döndür", true, 252, 40);
            btnModernDondur.Location = new Point(18, 168);
            btnModernDondur.Click += btnDondur_Click;
            pnlSag.Controls.Add(btnModernDondur);

            // Açık Mavi Bilgi ve Ters Dönüşüm Kartı
            ModernKartPanel bilgiKarti = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                konum: new Point(18, 224),
                boyut: new Size(252, 330),
                icerikEtiketi: out lblMerkezKart
            );
            lblMerkezKart.Text =
                "Kullanım Adımları:\n\n" +
                "1. 'Resim Seç' ile bir görsel yükleyin.\n" +
                "2. Sol kaynak görüntü üzerinde dönme merkezini (+) tıklayarak belirleyin.\n" +
                "3. Açıyı girip 'Resmi Döndür' butonuna basın.\n\n" +
                "Not: Ters dönüşüm (Inverse Mapping) ile boş piksel oluşumu engellenir ve döndürme sonrası (+) işareti kaldırılır.";
            pnlSag.Controls.Add(bilgiKarti);

            // 4. Orta Çalışma Alanı (Üst: Kaynak & Hedef Kartları | Alt: Analiz Şeridi)
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

            // 2 Sütunlu Görüntü Izgarası
            TableLayoutPanel goruntuIzgara = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1
            };
            goruntuIzgara.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            goruntuIzgara.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            goruntuIzgara.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            rozetKaynak = new ModernHapRozet(); rozetKaynak.MetinGuncelle("Kaynak");
            rozetHedef = new ModernHapRozet(); rozetHedef.MetinGuncelle("Bekleniyor");

            MevcutPictureBoxKartaTasi(goruntuIzgara, 0, 0, "Kaynak Resim (Merkez Seçimi İçin Tıklayın)", picKaynak, FormGezintiYardimcisi.TemaZemin, rozetKaynak);
            MevcutPictureBoxKartaTasi(goruntuIzgara, 1, 0, "Döndürülmüş Çıktı (Inverse Mapping)", picHedef, FormGezintiYardimcisi.TemaZemin, rozetHedef);

            picKaynak.Cursor = Cursors.Cross;
            picKaynak.MouseDown -= PicKaynak_MouseDown;
            picKaynak.Paint -= PicKaynak_Paint;

            picKaynak.MouseDown += PicKaynak_MouseDown;
            picKaynak.Paint += PicKaynak_Paint;
            picKaynak.Resize += (s, e) => picKaynak.Invalidate();

            picHedef.SizeMode = PictureBoxSizeMode.Zoom;

            // Alt Analiz Şeridi
            ModernKartPanel altSerit = FormGezintiYardimcisi.AltAnalizSeridiOlustur(
                "Resim seçip sol görüntü üzerinden dönme merkezini (+) belirleyin, açıyı girin ve 'Resmi Döndür' butonuna basın.",
                out lblAnaliz,
                "Ters Dönüşüm"
            );

            anaYerlesim.Controls.Add(goruntuIzgara, 0, 0);
            anaYerlesim.Controls.Add(altSerit, 0, 1);
            pnlOrta.Controls.Add(anaYerlesim);

            this.Controls.Add(pnlOrta);
            FormGezintiYardimcisi.YerlesimiDuzenle(this, pnlUst, pnlSag, pnlOrta);
        }

        private void MevcutPictureBoxKartaTasi(
            TableLayoutPanel tablo,
            int sutun,
            int satir,
            string baslik,
            PictureBox mevcutPb,
            Color zeminRengi,
            ModernHapRozet sagUstRozet)
        {
            ModernKartPanel kart = new ModernKartPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(6),
                Padding = new Padding(12)
            };

            Panel pnlKartUst = new Panel
            {
                Dock = DockStyle.Top,
                Height = 28,
                BackColor = Color.White
            };

            Label lbl = new Label
            {
                Text = baslik,
                UseMnemonic = false,
                AutoSize = true,
                Location = new Point(0, 4),
                Font = new Font("Segoe UI Semibold", 9.6f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                BackColor = Color.White
            };
            pnlKartUst.Controls.Add(lbl);

            if (sagUstRozet != null)
            {
                sagUstRozet.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                sagUstRozet.Location = new Point(pnlKartUst.Width - sagUstRozet.Width - 2, 1);
                pnlKartUst.Resize += (s, e) =>
                {
                    sagUstRozet.Location = new Point(pnlKartUst.Width - sagUstRozet.Width - 2, 1);
                };
                pnlKartUst.Controls.Add(sagUstRozet);
                sagUstRozet.BringToFront();
            }

            mevcutPb.Parent?.Controls.Remove(mevcutPb);
            mevcutPb.Visible = true;
            mevcutPb.Dock = DockStyle.Fill;
            mevcutPb.BorderStyle = BorderStyle.None;
            mevcutPb.BackColor = zeminRengi;

            kart.Controls.Add(mevcutPb);
            kart.Controls.Add(pnlKartUst);
            tablo.Controls.Add(kart, sutun, satir);
        }

        private Rectangle CizimAlaniniHesapla()
        {
            if (orijinalBmp == null || picKaynak.ClientSize.Width <= 0 || picKaynak.ClientSize.Height <= 0)
                return Rectangle.Empty;

            float kutuW = picKaynak.ClientSize.Width;
            float kutuH = picKaynak.ClientSize.Height;
            float olcek = Math.Min(kutuW / orijinalBmp.Width, kutuH / orijinalBmp.Height);

            int yeniW = (int)Math.Round(orijinalBmp.Width * olcek);
            int yeniH = (int)Math.Round(orijinalBmp.Height * olcek);
            int x = (int)((kutuW - yeniW) / 2f);
            int y = (int)((kutuH - yeniH) / 2f);

            return new Rectangle(x, y, yeniW, yeniH);
        }

        private void btnResimSec_Click(object sender, EventArgs e)
        {
            string dosyaYolu = FormGezintiYardimcisi.ResimDosyasiSec("Döndürme İşlemi İçin Resim Seçiniz");
            if (string.IsNullOrEmpty(dosyaYolu)) return;

            orijinalBmp?.Dispose();
            using (Bitmap temp = new Bitmap(dosyaYolu))
            {
                orijinalBmp = new Bitmap(temp);
            }

            // Varsayılan dönme merkezi resmin tam ortası
            donmeMerkezi = new Point(orijinalBmp.Width / 2, orijinalBmp.Height / 2);
            artiGoster = true;

            if (picHedef != null)
            {
                picHedef.Image?.Dispose();
                picHedef.Image = null;
            }

            rozetKaynak.MetinGuncelle($"{orijinalBmp.Width}×{orijinalBmp.Height}");
            rozetHedef.MetinGuncelle("Bekleniyor");

            lblMerkezKart.Text =
                "Aktif Dönme Parametreleri:\n\n" +
                $"• Resim Boyutu: {orijinalBmp.Width} × {orijinalBmp.Height} px\n" +
                $"• Merkez Noktası: ({donmeMerkezi.Value.X}, {donmeMerkezi.Value.Y})\n" +
                "• İşaret Durumu: Görünür (+)\n\n" +
                "Farklı bir merkez belirlemek için sol görüntü üzerinde istediğiniz noktaya tıklayabilirsiniz.";

            lblAnaliz.Text = $"Resim yüklendi ({orijinalBmp.Width}×{orijinalBmp.Height} px) • Varsayılan merkez: ({donmeMerkezi.Value.X}, {donmeMerkezi.Value.Y})";
            picKaynak.Invalidate();
        }

        private void PicKaynak_MouseDown(object sender, MouseEventArgs e)
        {
            if (orijinalBmp == null || e.Button != MouseButtons.Left) return;

            cizimAlani = CizimAlaniniHesapla();
            if (!cizimAlani.Contains(e.Location) || cizimAlani.Width <= 0 || cizimAlani.Height <= 0) return;

            int gercekX = (int)Math.Round((double)(e.X - cizimAlani.X) / cizimAlani.Width * orijinalBmp.Width);
            int gercekY = (int)Math.Round((double)(e.Y - cizimAlani.Y) / cizimAlani.Height * orijinalBmp.Height);

            gercekX = Math.Clamp(gercekX, 0, orijinalBmp.Width - 1);
            gercekY = Math.Clamp(gercekY, 0, orijinalBmp.Height - 1);

            donmeMerkezi = new Point(gercekX, gercekY);
            artiGoster = true;

            lblMerkezKart.Text =
                "Aktif Dönme Parametreleri:\n\n" +
                $"• Merkez Noktası (x₀, y₀): ({gercekX}, {gercekY})\n" +
                $"• Hedef Açı: {txtAci.Text}°\n" +
                "• İşaret Durumu: Görünür (+)\n\n" +
                "Açıyı onaylayıp 'Resmi Döndür' butonuna basınız.";

            lblAnaliz.Text = $"Yeni dönme merkezi seçildi: ({gercekX}, {gercekY}) • Açıyı girip 'Resmi Döndür' butonuna basınız.";
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
                        sonucBmp.SetPixel(x, y, Color.White);
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

            rozetHedef.MetinGuncelle($"{aci}° Döndürüldü");
            lblMerkezKart.Text =
                "Döndürme Tamamlandı:\n\n" +
                $"• Dönme Açısı (θ): {aci}°\n" +
                $"• Merkez (x₀, y₀): ({x0}, {y0})\n" +
                "• İşaret Durumu: Kaldırıldı\n\n" +
                "Yeni bir merkez seçmek için sol görüntüye tekrar tıklayabilirsiniz.";

            lblAnaliz.Text = $"Döndürme tamamlandı • Açı: {aci}° • Merkez: ({x0}, {y0}) • Artı (+) işareti kaldırıldı.";
            this.Cursor = Cursors.Default;
        }

        private void PicKaynak_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (orijinalBmp != null)
            {
                cizimAlani = CizimAlaniniHesapla();
                g.DrawImage(orijinalBmp, cizimAlani);

                using (Pen pCerceve = new Pen(Color.FromArgb(148, 163, 184), 1.2f))
                {
                    g.DrawRectangle(pCerceve, cizimAlani);
                }

                // Tıklanan yerde kırmızı artı (+) işareti çizimi
                if (artiGoster && donmeMerkezi.HasValue && cizimAlani.Width > 0 && cizimAlani.Height > 0)
                {
                    int ekranX = cizimAlani.X + (int)Math.Round((double)donmeMerkezi.Value.X / orijinalBmp.Width * cizimAlani.Width);
                    int ekranY = cizimAlani.Y + (int)Math.Round((double)donmeMerkezi.Value.Y / orijinalBmp.Height * cizimAlani.Height);
                    int kol = 10;

                    using (Pen penBeyaz = new Pen(Color.White, 4f))
                    using (Pen penKirmizi = new Pen(Color.FromArgb(220, 38, 38), 2.2f))
                    {
                        g.DrawLine(penBeyaz, ekranX - kol, ekranY, ekranX + kol, ekranY);
                        g.DrawLine(penBeyaz, ekranX, ekranY - kol, ekranX, ekranY + kol);

                        g.DrawLine(penKirmizi, ekranX - kol, ekranY, ekranX + kol, ekranY);
                        g.DrawLine(penKirmizi, ekranX, ekranY - kol, ekranX, ekranY + kol);
                    }
                }
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            orijinalBmp?.Dispose();
            base.OnFormClosed(e);
        }
    }
}