using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace GoruntuIsleme
{
    public partial class Hafta3Odev5 : Form
    {
        private Bitmap orijinalBmp = null;
        private Bitmap kirpilanBmp = null;

        // Ekran koordinatlarında seçim noktaları
        private Point? nokta1 = null;
        private Point? nokta2 = null;
        private bool fareSurukleniyor = false;

        // Kaynak resmin PictureBox içindeki orantılı çizim dikdörtgeni
        private Rectangle cizimAlani = Rectangle.Empty;

        private ModernHapRozet rozetKaynak;
        private ModernHapRozet rozetKirpilan;
        private Label lblSecimKart;
        private Label lblAnaliz;

        public Hafta3Odev5()
        {
            InitializeComponent();
            ModernArayuzuUygula();
        }

        private void ModernArayuzuUygula()
        {
            // 1. Form Temel Ayarları (1250x750, Standart Kenarlık ve X ile Ana Menüye Dönüş)
            FormGezintiYardimcisi.FormuHazirla(this);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Text = "Hafta 3 - Ödev 5: İki Nokta ile Dikdörtgen Kırpma (Crop)";

            // Designer'daki tüm eski kontrolleri gizle
            foreach (Control ctrl in this.Controls)
            {
                ctrl.Visible = false;
            }

            // 2. Üst Gezinti Barı
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                form: this,
                baslik: "Hafta 3 - Ödev 5 | İki Nokta ile Dikdörtgen Kırpma (ROI / Crop İşlemi)",
                oncekiYazi: "< Ödev 4",
                oncekiTikla: () =>
                {
                    Form odev4 = Application.OpenForms["Hafta3Odev4"];
                    if (odev4 != null)
                        FormGezintiYardimcisi.FormGecisYap(this, odev4);
                    else
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta3Odev4());
                },
                sonrakiYazi: "Ödev 6 >",
                sonrakiTikla: () =>
                {
                    Form odev6 = Application.OpenForms["Hafta3Odev6"];
                    if (odev6 != null)
                        FormGezintiYardimcisi.FormGecisYap(this, odev6);
                    else
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta3Odev6());
                }
            );

            // 3. Sağ Kontrol Paneli
            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 290);

            Label lblKontrolBaslik = new Label
            {
                Text = "Kırpma (Crop) İşlemleri",
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
            btnModernSifirla.Location = new Point(18, 146);
            btnModernSifirla.Click += btnSifirla_Click;
            pnlSag.Controls.Add(btnModernSifirla);

            // Açık Mavi Seçim ve Koordinat Bilgi Kartı
            ModernKartPanel bilgiKarti = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                konum: new Point(18, 198),
                boyut: new Size(252, 340),
                icerikEtiketi: out lblSecimKart
            );
            lblSecimKart.Text =
                "Kırpma Adımları:\n\n" +
                "1. 'Resim Seç' ile görüntüyü yükleyin.\n" +
                "2. Sol kaynak görüntü üzerinde farenin sol tuşuna basılı tutup sürükleyerek bir dikdörtgen alan belirleyin.\n" +
                "3. 'Seçilen Alanı Kırp' butonuna basarak seçili bölgenin piksellerini yeni matrise aktarın.\n\n" +
                "Seçim Bilgisi:\n• Henüz alan seçilmedi.";
            pnlSag.Controls.Add(bilgiKarti);

            // 4. Orta Çalışma Alanı (Üst: Kaynak & Kırpılan Kartları | Alt: Analiz Şeridi)
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
            rozetKirpilan = new ModernHapRozet(); rozetKirpilan.MetinGuncelle("Bekleniyor");

            MevcutPictureBoxKartaTasi(goruntuIzgara, 0, 0, "Kaynak Görüntü (Sürükleyerek Alan Seçin)", picKaynak, FormGezintiYardimcisi.TemaZemin, rozetKaynak);
            MevcutPictureBoxKartaTasi(goruntuIzgara, 1, 0, "Kırpılmış Görüntü (Crop Çıktısı)", picKirpilan, FormGezintiYardimcisi.TemaZemin, rozetKirpilan);

            picKaynak.Cursor = Cursors.Cross;
            picKaynak.MouseDown -= PicKaynak_MouseDown;
            picKaynak.MouseMove -= PicKaynak_MouseMove;
            picKaynak.MouseUp -= PicKaynak_MouseUp;
            picKaynak.Paint -= PicKaynak_Paint;

            picKaynak.MouseDown += PicKaynak_MouseDown;
            picKaynak.MouseMove += PicKaynak_MouseMove;
            picKaynak.MouseUp += PicKaynak_MouseUp;
            picKaynak.Paint += PicKaynak_Paint;
            picKaynak.Resize += (s, e) => picKaynak.Invalidate();

            picKirpilan.SizeMode = PictureBoxSizeMode.Zoom;

            // Alt Analiz Şeridi
            ModernKartPanel altSerit = FormGezintiYardimcisi.AltAnalizSeridiOlustur(
                "Bir resim seçin ve sol görüntü üzerinde farenin sol tuşuyla sürükleyerek kırpılacak dikdörtgen bölgeyi belirleyin.",
                out lblAnaliz,
                "ROI Kırpma"
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
            string dosyaYolu = FormGezintiYardimcisi.ResimDosyasiSec("Kırpma (Crop) İşlemi İçin Resim Seçiniz");
            if (string.IsNullOrEmpty(dosyaYolu)) return;

            orijinalBmp?.Dispose();
            kirpilanBmp?.Dispose();
            kirpilanBmp = null;
            nokta1 = null;
            nokta2 = null;

            using (Bitmap temp = new Bitmap(dosyaYolu))
            {
                orijinalBmp = new Bitmap(temp);
            }

            if (picKirpilan != null)
            {
                picKirpilan.Image?.Dispose();
                picKirpilan.Image = null;
            }

            rozetKaynak.MetinGuncelle($"{orijinalBmp.Width}×{orijinalBmp.Height}");
            rozetKirpilan.MetinGuncelle("Bekleniyor");
            lblAnaliz.Text = $"Resim yüklendi ({orijinalBmp.Width}×{orijinalBmp.Height} px) • Sol tuşla sürükleyerek kırpma alanını seçiniz.";
            picKaynak.Invalidate();
        }

        private void btnSifirla_Click(object sender, EventArgs e)
        {
            nokta1 = null;
            nokta2 = null;
            kirpilanBmp?.Dispose();
            kirpilanBmp = null;

            if (picKirpilan != null)
            {
                picKirpilan.Image?.Dispose();
                picKirpilan.Image = null;
            }

            rozetKirpilan.MetinGuncelle("Bekleniyor");
            lblSecimKart.Text =
                "Kırpma Adımları:\n\n" +
                "1. 'Resim Seç' ile görüntüyü yükleyin.\n" +
                "2. Sol kaynak görüntü üzerinde farenin sol tuşuna basılı tutup sürükleyerek bir dikdörtgen alan belirleyin.\n" +
                "3. 'Seçilen Alanı Kırp' butonuna basarak seçili bölgenin piksellerini yeni matrise aktarın.\n\n" +
                "Seçim Bilgisi:\n• Seçim sıfırlandı.";
            lblAnaliz.Text = "Seçim alanı sıfırlandı • Yeni bir dikdörtgen alan çizebilirsiniz.";
            picKaynak.Invalidate();
        }

        // Kaynak resmin PictureBox içine orantılı yerleştiği dikdörtgeni hesaplar
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

        private Point ResimSinirinaKirp(Point p)
        {
            if (cizimAlani.IsEmpty) return p;
            int x = Math.Clamp(p.X, cizimAlani.Left, cizimAlani.Right);
            int y = Math.Clamp(p.Y, cizimAlani.Top, cizimAlani.Bottom);
            return new Point(x, y);
        }

        private void PicKaynak_MouseDown(object sender, MouseEventArgs e)
        {
            if (orijinalBmp == null || e.Button != MouseButtons.Left) return;

            cizimAlani = CizimAlaniniHesapla();
            if (!cizimAlani.Contains(e.Location)) return;

            nokta1 = ResimSinirinaKirp(e.Location);
            nokta2 = nokta1;
            fareSurukleniyor = true;

            picKaynak.Invalidate();
        }

        private void PicKaynak_MouseMove(object sender, MouseEventArgs e)
        {
            if (!fareSurukleniyor || nokta1 == null || orijinalBmp == null) return;

            nokta2 = ResimSinirinaKirp(e.Location);
            picKaynak.Invalidate();
        }

        private void PicKaynak_MouseUp(object sender, MouseEventArgs e)
        {
            if (!fareSurukleniyor || orijinalBmp == null) return;

            fareSurukleniyor = false;
            nokta2 = ResimSinirinaKirp(e.Location);

            Rectangle gercekSecim = GercekResimSecimAlani();
            if (gercekSecim.Width > 0 && gercekSecim.Height > 0)
            {
                lblSecimKart.Text =
                    "Seçilen Bölge (ROI) Bilgileri:\n\n" +
                    $"• Başlangıç (X, Y): ({gercekSecim.X}, {gercekSecim.Y})\n" +
                    $"• Bitiş (X₂, Y₂): ({gercekSecim.Right}, {gercekSecim.Bottom})\n" +
                    $"• Boyut: {gercekSecim.Width} × {gercekSecim.Height} px\n\n" +
                    "Kırpmayı tamamlamak için 'Seçilen Alanı Kırp' butonuna tıklayınız.";

                lblAnaliz.Text = $"Seçim Alanı: X={gercekSecim.X}, Y={gercekSecim.Y} • Genişlik={gercekSecim.Width} px, Yükseklik={gercekSecim.Height} px";
            }

            picKaynak.Invalidate();
        }

        // Ekrandaki kırmızı dikdörtgen seçim
        private Rectangle EkranSecimDikdortgeniAl()
        {
            if (!nokta1.HasValue || !nokta2.HasValue) return Rectangle.Empty;

            int x = Math.Min(nokta1.Value.X, nokta2.Value.X);
            int y = Math.Min(nokta1.Value.Y, nokta2.Value.Y);
            int w = Math.Abs(nokta1.Value.X - nokta2.Value.X);
            int h = Math.Abs(nokta1.Value.Y - nokta2.Value.Y);

            return new Rectangle(x, y, w, h);
        }

        // Ekran seçimini orijinal resmin gerçek piksel koordinatlarına dönüştürür
        private Rectangle GercekResimSecimAlani()
        {
            Rectangle ekranSecim = EkranSecimDikdortgeniAl();
            if (orijinalBmp == null || cizimAlani.Width <= 0 || cizimAlani.Height <= 0 || ekranSecim.Width <= 0 || ekranSecim.Height <= 0)
                return Rectangle.Empty;

            double olcekX = (double)orijinalBmp.Width / cizimAlani.Width;
            double olcekY = (double)orijinalBmp.Height / cizimAlani.Height;

            int gercekX = (int)Math.Round((ekranSecim.X - cizimAlani.X) * olcekX);
            int gercekY = (int)Math.Round((ekranSecim.Y - cizimAlani.Y) * olcekY);
            int gercekW = (int)Math.Round(ekranSecim.Width * olcekX);
            int gercekH = (int)Math.Round(ekranSecim.Height * olcekY);

            gercekX = Math.Clamp(gercekX, 0, orijinalBmp.Width - 1);
            gercekY = Math.Clamp(gercekY, 0, orijinalBmp.Height - 1);
            gercekW = Math.Clamp(gercekW, 1, orijinalBmp.Width - gercekX);
            gercekH = Math.Clamp(gercekH, 1, orijinalBmp.Height - gercekY);

            return new Rectangle(gercekX, gercekY, gercekW, gercekH);
        }

        private void btnKirp_Click(object sender, EventArgs e)
        {
            if (orijinalBmp == null)
            {
                MessageBox.Show("Lütfen önce bir resim seçiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Rectangle secim = GercekResimSecimAlani();
            if (secim.Width < 3 || secim.Height < 3)
            {
                MessageBox.Show("Lütfen kaynak resim üzerinde fare ile sürükleyerek geçerli bir alan seçiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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

            rozetKirpilan.MetinGuncelle($"{secim.Width}×{secim.Height}");
            lblAnaliz.Text = $"Kırpma tamamlandı • Yeni boyut: {secim.Width}×{secim.Height} px (Başlangıç konumu: {secim.X}, {secim.Y})";

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
            }

            Rectangle secim = EkranSecimDikdortgeniAl();
            if (secim.Width > 0 && secim.Height > 0)
            {
                using (Brush b = new SolidBrush(Color.FromArgb(45, 239, 68, 68)))
                {
                    g.FillRectangle(b, secim);
                }
                using (Pen p = new Pen(Color.FromArgb(220, 38, 38), 2f))
                {
                    p.DashStyle = DashStyle.Dash;
                    g.DrawRectangle(p, secim);
                }
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            orijinalBmp?.Dispose();
            kirpilanBmp?.Dispose();
            base.OnFormClosed(e);
        }
    }
}