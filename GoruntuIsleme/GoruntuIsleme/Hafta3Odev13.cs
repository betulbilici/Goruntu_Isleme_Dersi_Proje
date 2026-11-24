using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

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

        private ModernHapRozet rozetOrijinal;
        private ModernHapRozet rozetKirpilan;
        private Label lblBilgiKart;
        private Label lblAnaliz;

        public Hafta3Odev13()
        {
            InitializeComponent();
            ModernArayuzuUygula();
        }

        private void ModernArayuzuUygula()
        {
            // 1. Form Temel Ayarları (1250x750, Standart Kenarlık ve X ile Ana Menüye Dönüş)
            FormGezintiYardimcisi.FormuHazirla(this);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Text = "Hafta 3 - Ödev 13: Mouse Takip Eden Çizgi ile Kırpma";

            // Designer'daki tüm eski kontrolleri gizle
            foreach (Control ctrl in this.Controls)
            {
                ctrl.Visible = false;
            }

            // 2. Üst Gezinti Barı
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                form: this,
                baslik: "Hafta 3 - Ödev 13 | Fareyi Takip Eden Çizgi ve Tutamaçlarla Kırpma",
                oncekiYazi: "< Ödev 12",
                oncekiTikla: () =>
                {
                    Form odev12 = Application.OpenForms["Hafta3Odev12"];
                    if (odev12 != null)
                        FormGezintiYardimcisi.FormGecisYap(this, odev12);
                    else
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta3Odev12());
                },
                sonrakiYazi: "Ana Menü >",
                sonrakiTikla: () =>
                {
                    Form anaForm = Application.OpenForms["Form1"];
                    if (anaForm != null)
                        FormGezintiYardimcisi.FormGecisYap(this, anaForm);
                    else
                        FormGezintiYardimcisi.FormGecisYap(this, new Form1());
                }
            );

            // 3. Sağ Kontrol Paneli
            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 290);

            Label lblKontrolBaslik = new Label
            {
                Text = "Kırpma & Seçim Kontrolleri",
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

            // Seçilen Alanı Kırp Butonu
            ModernHapButon btnModernKirp = FormGezintiYardimcisi.ModernButonOlustur("✂️ Seçilen Alanı Kırp", true, 252, 40);
            btnModernKirp.Location = new Point(18, 96);
            btnModernKirp.Click += btnKirp_Click;
            pnlSag.Controls.Add(btnModernKirp);

            // Sıfırla Butonu
            ModernHapButon btnModernSifirla = FormGezintiYardimcisi.ModernButonOlustur("🔄 Seçimi Sıfırla", false, 252, 36);
            btnModernSifirla.Location = new Point(18, 144);
            btnModernSifirla.Click += btnSifirla_Click;
            pnlSag.Controls.Add(btnModernSifirla);

            // Açık Mavi Kırpma Bilgi Kartı
            ModernKartPanel bilgiKarti = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                konum: new Point(18, 196),
                boyut: new Size(252, 355),
                icerikEtiketi: out lblBilgiKart
            );
            lblBilgiKart.Text =
                "Kullanım & Kırpma Detayı:\n\n" +
                "1. 'Resim Seç' ile görüntüyü yükleyin.\n\n" +
                "2. Sol kaynak görüntü üzerinde farenin sol tuşuna basılı tutup sürükleyin; kesikli çerçeve ve 8 tutamaç noktası anlık çizilir.\n\n" +
                "3. Fare bırakıldığında (veya 'Seçilen Alanı Kırp' ile) seçim dikdörtgeni Zoom oranına göre gerçek piksel koordinatlarına eşlenip kırpılır.";
            pnlSag.Controls.Add(bilgiKarti);

            // 4. Orta Çalışma Alanı (Üst: Orijinal & Kırpılan Kartları | Alt: Analiz Şeridi)
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

            rozetOrijinal = new ModernHapRozet(); rozetOrijinal.MetinGuncelle("Kaynak");
            rozetKirpilan = new ModernHapRozet(); rozetKirpilan.MetinGuncelle("Bekleniyor");

            MevcutPictureBoxKartaTasi(goruntuIzgara, 0, 0, "Orijinal Resim (Sürükleyerek Alan Seçin)", picOrijinal, FormGezintiYardimcisi.TemaZemin, rozetOrijinal);
            MevcutPictureBoxKartaTasi(goruntuIzgara, 1, 0, "Kırpılan Sonuç", picKirpilan, FormGezintiYardimcisi.TemaZemin, rozetKirpilan);

            picOrijinal.Cursor = Cursors.Cross;
            picOrijinal.MouseDown -= picOrijinal_MouseDown;
            picOrijinal.MouseMove -= picOrijinal_MouseMove;
            picOrijinal.MouseUp -= picOrijinal_MouseUp;
            picOrijinal.Paint -= picOrijinal_Paint;

            picOrijinal.MouseDown += picOrijinal_MouseDown;
            picOrijinal.MouseMove += picOrijinal_MouseMove;
            picOrijinal.MouseUp += picOrijinal_MouseUp;
            picOrijinal.Paint += picOrijinal_Paint;
            picOrijinal.Resize += (s, e) => picOrijinal.Invalidate();

            picKirpilan.SizeMode = PictureBoxSizeMode.Zoom;

            // Alt Analiz Şeridi
            ModernKartPanel altSerit = FormGezintiYardimcisi.AltAnalizSeridiOlustur(
                "Bir resim seçin ve sol görüntü üzerinde farenin sol tuşuna basılı tutarak kırpmak istediğiniz alanı belirleyin.",
                out lblAnaliz,
                "Çizgi Kırpma"
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

        private void btnResimSec_Click(object sender, EventArgs e)
        {
            string dosyaYolu = FormGezintiYardimcisi.ResimDosyasiSec("Kırpma İşlemi İçin Resim Seçiniz");
            if (string.IsNullOrEmpty(dosyaYolu)) return;

            orijinalBmp?.Dispose();
            kirpilanBmp?.Dispose();
            kirpilanBmp = null;
            secimDikdortgeni = Rectangle.Empty;

            using (Bitmap temp = new Bitmap(dosyaYolu))
            {
                orijinalBmp = new Bitmap(temp);
            }

            if (picOrijinal != null)
            {
                picOrijinal.Image?.Dispose();
                picOrijinal.SizeMode = PictureBoxSizeMode.Zoom;
                picOrijinal.Image = new Bitmap(orijinalBmp);
            }

            if (picKirpilan != null)
            {
                picKirpilan.Image?.Dispose();
                picKirpilan.Image = null;
            }

            rozetOrijinal.MetinGuncelle($"{orijinalBmp.Width}×{orijinalBmp.Height}");
            rozetKirpilan.MetinGuncelle("Bekleniyor");

            lblBilgiKart.Text =
                "Görüntü Yüklendi:\n\n" +
                $"• Orijinal Boyut: {orijinalBmp.Width} × {orijinalBmp.Height} px\n\n" +
                "Sol kaynak görüntü üzerinde farenin sol tuşuna basılı tutup sürükleyerek kırpmak istediğiniz alanı belirleyin.";

            lblAnaliz.Text = $"Resim yüklendi: {orijinalBmp.Width}×{orijinalBmp.Height} px • Fare sol tuşuna basılı tutarak kırpma alanını seçiniz.";
            picOrijinal.Invalidate();
        }

        private void btnSifirla_Click(object sender, EventArgs e)
        {
            secimDikdortgeni = Rectangle.Empty;
            kirpilanBmp?.Dispose();
            kirpilanBmp = null;

            if (picKirpilan != null)
            {
                picKirpilan.Image?.Dispose();
                picKirpilan.Image = null;
            }

            if (picOrijinal != null) picOrijinal.Invalidate();

            rozetKirpilan.MetinGuncelle("Bekleniyor");
            lblBilgiKart.Text =
                "Seçim Sıfırlandı:\n\n" +
                "• Kırpılan görüntü ve seçim dikdörtgeni temizlendi.\n\n" +
                "Yeni bir bölge seçmek için kaynak görüntü üzerinde farenin sol tuşunu sürükleyin.";

            lblAnaliz.Text = "Seçim ve kırpılan görüntü temizlendi.";
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

            int x = Math.Min(baslangicNoktasi.X, e.X);
            int y = Math.Min(baslangicNoktasi.Y, e.Y);
            int genislik = Math.Abs(baslangicNoktasi.X - e.X);
            int yukseklik = Math.Abs(baslangicNoktasi.Y - e.Y);

            secimDikdortgeni = new Rectangle(x, y, genislik, yukseklik);
            picOrijinal.Invalidate();
        }

        private void picOrijinal_MouseUp(object sender, MouseEventArgs e)
        {
            if (!fareBasildi || orijinalBmp == null) return;

            fareBasildi = false;

            if (secimDikdortgeni.Width > 5 && secimDikdortgeni.Height > 5)
            {
                KirpmaIsleminiYap();
            }
        }

        // Kesikli çizgi ve tutma noktalarını ekrana çizdirme
        private void picOrijinal_Paint(object sender, PaintEventArgs e)
        {
            if (secimDikdortgeni.Width <= 0 || secimDikdortgeni.Height <= 0) return;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (Brush dolguFirca = new SolidBrush(Color.FromArgb(35, 37, 99, 235)))
            {
                e.Graphics.FillRectangle(dolguFirca, secimDikdortgeni);
            }

            using (Pen kesikliKalem = new Pen(Color.FromArgb(15, 23, 42), 1.6f))
            {
                kesikliKalem.DashStyle = DashStyle.Dash;
                e.Graphics.DrawRectangle(kesikliKalem, secimDikdortgeni);
            }

            int kareBoyut = 6;
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

            using (Brush firca = new SolidBrush(Color.FromArgb(15, 23, 42)))
            using (Pen kenarKalem = new Pen(Color.White, 1.2f))
            {
                foreach (Point p in noktalar)
                {
                    Rectangle tutamac = new Rectangle(p.X - kareBoyut / 2, p.Y - kareBoyut / 2, kareBoyut, kareBoyut);
                    e.Graphics.FillRectangle(firca, tutamac);
                    e.Graphics.DrawRectangle(kenarKalem, tutamac);
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

            Rectangle gercekResimAlani = GetGercekResimDikdortgeni(picOrijinal);
            if (!gercekResimAlani.IntersectsWith(secimDikdortgeni)) return;

            Rectangle kesisim = Rectangle.Intersect(gercekResimAlani, secimDikdortgeni);

            float oranX = (float)orijinalBmp.Width / gercekResimAlani.Width;
            float oranY = (float)orijinalBmp.Height / gercekResimAlani.Height;

            int srcX = (int)((kesisim.X - gercekResimAlani.X) * oranX);
            int srcY = (int)((kesisim.Y - gercekResimAlani.Y) * oranY);
            int srcW = (int)(kesisim.Width * oranX);
            int srcH = (int)(kesisim.Height * oranY);

            srcX = Math.Clamp(srcX, 0, orijinalBmp.Width - 1);
            srcY = Math.Clamp(srcY, 0, orijinalBmp.Height - 1);
            srcW = Math.Clamp(srcW, 1, orijinalBmp.Width - srcX);
            srcH = Math.Clamp(srcH, 1, orijinalBmp.Height - srcY);

            if (srcW <= 0 || srcH <= 0) return;

            kirpilanBmp?.Dispose();
            kirpilanBmp = new Bitmap(srcW, srcH, PixelFormat.Format24bppRgb);

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

            rozetKirpilan.MetinGuncelle($"{srcW}×{srcH}");

            lblBilgiKart.Text =
                "Kırpma Sonucu:\n\n" +
                $"• Seçilen Konum (X, Y): ({srcX}, {srcY})\n" +
                $"• Kırpılan Boyut: {srcW} × {srcH} px\n" +
                $"• Ekran Seçimi: {secimDikdortgeni.Width} × {secimDikdortgeni.Height} px\n\n" +
                "Farklı bir bölge seçmek için kaynak resimde yeniden sürükleyebilirsiniz.";

            lblAnaliz.Text = $"Kırpma başarılı • Seçilen Konum: ({srcX}, {srcY}) • Kırpılan Çözünürlük: {srcW}×{srcH} px";
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

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            orijinalBmp?.Dispose();
            kirpilanBmp?.Dispose();
            base.OnFormClosed(e);
        }
    }
}