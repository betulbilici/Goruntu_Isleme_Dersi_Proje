using System;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace GoruntuIsleme
{
    public partial class Hafta1Odev4 : Form
    {
        private Mat griMat = null;
        private Mat mozaikMat = null;
        private int blokBoyutu = 10;

        private ModernHapRozet rozetOrijinal;
        private ModernHapRozet rozetPikselDegeri;
        private Label lblAnaliz;

        public Hafta1Odev4()
        {
            InitializeComponent();
            ModernArayuzuUygula();
        }

        private void ModernArayuzuUygula()
        {
            // 1. Form Temel Ayarları (1250x750, Standart Kenarlık ve X ile Ana Menüye Dönüş)
            FormGezintiYardimcisi.FormuHazirla(this);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Text = "Hafta 1 - Ödev 4: Gri Seviye Mozaik ve Koordinat Okuma";

            // Designer'daki eski etiket, buton ve linkleri gizle (aktif kontrolleri taşıyacağız)
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            lblPikselDegeri.Visible = false;
            btnResimSec.Visible = false;
            btnMozaikOlustur.Visible = false;
            lnkGeri.Visible = false;
            lnkOdev5.Visible = false;

            // 2. Üst Gezinti Barı
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                form: this,
                baslik: "Hafta 1 - Ödev 4 | Gri Seviye Mozaik ve f_gray(i, j) Koordinat Okuma",
                oncekiYazi: "< Ödev 3",
                oncekiTikla: () =>
                {
                    Form odev3 = Application.OpenForms["Hafta1Odev3"];
                    if (odev3 != null)
                        FormGezintiYardimcisi.FormGecisYap(this, odev3);
                    else
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta1Odev3());
                },
                sonrakiYazi: "Ödev 5 >",
                sonrakiTikla: () => FormGezintiYardimcisi.FormGecisYap(this, new Hafta1Odev5())
            );

            // 3. Sağ Kontrol Paneli
            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 290);

            Label lblKontrolBaslik = new Label
            {
                Text = "Mozaik & Koordinat Ayarları",
                Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                Location = new System.Drawing.Point(18, 18),
                AutoSize = true
            };
            pnlSag.Controls.Add(lblKontrolBaslik);

            // Resim Seç Butonu
            ModernHapButon btnModernResimSec = FormGezintiYardimcisi.ModernButonOlustur("📁 Resim Seç", true, 252, 40);
            btnModernResimSec.Location = new System.Drawing.Point(18, 52);
            btnModernResimSec.Click += btnResimSec_Click;
            pnlSag.Controls.Add(btnModernResimSec);

            // Mozaik Boyutu (Adım)
            Label lblAdim = new Label
            {
                Text = "Mozaik Boyutu (Adım):",
                Font = new Font("Segoe UI Semibold", 9.2f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                Location = new System.Drawing.Point(18, 106),
                AutoSize = true
            };
            pnlSag.Controls.Add(lblAdim);

            cmbMozaikBoyut.Parent?.Controls.Remove(cmbMozaikBoyut);
            cmbMozaikBoyut.Location = new System.Drawing.Point(18, 128);
            cmbMozaikBoyut.Size = new System.Drawing.Size(252, 30);
            cmbMozaikBoyut.Font = new Font("Segoe UI", 9.8f);
            if (cmbMozaikBoyut.Items.Count > 0 && cmbMozaikBoyut.SelectedIndex < 0)
                cmbMozaikBoyut.SelectedIndex = 2; // Varsayılan: 10
            pnlSag.Controls.Add(cmbMozaikBoyut);

            // Mozaik Oluştur Butonu
            ModernHapButon btnModernMozaik = FormGezintiYardimcisi.ModernButonOlustur("🧱 Mozaik Oluştur", false, 252, 38);
            btnModernMozaik.Location = new System.Drawing.Point(18, 168);
            btnModernMozaik.Click += btnMozaikOlustur_Click;
            pnlSag.Controls.Add(btnModernMozaik);

            // Satır (i) ve Sütun (j) Koordinatları (Yan Yana Düzen)
            Label lblSatir = new Label
            {
                Text = "Satır (i):",
                Font = new Font("Segoe UI Semibold", 9.2f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                Location = new System.Drawing.Point(18, 220),
                AutoSize = true
            };
            pnlSag.Controls.Add(lblSatir);

            numI.Parent?.Controls.Remove(numI);
            numI.Location = new System.Drawing.Point(18, 242);
            numI.Size = new System.Drawing.Size(118, 30);
            numI.Font = new Font("Segoe UI", 9.8f);
            numI.Minimum = 0;
            numI.Maximum = 9999;
            numI.ValueChanged -= KoordinatDegeriGuncelle;
            numI.ValueChanged += KoordinatDegeriGuncelle;
            pnlSag.Controls.Add(numI);

            Label lblSutun = new Label
            {
                Text = "Sütun (j):",
                Font = new Font("Segoe UI Semibold", 9.2f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                Location = new System.Drawing.Point(152, 220),
                AutoSize = true
            };
            pnlSag.Controls.Add(lblSutun);

            numJ.Parent?.Controls.Remove(numJ);
            numJ.Location = new System.Drawing.Point(152, 242);
            numJ.Size = new System.Drawing.Size(118, 30);
            numJ.Font = new Font("Segoe UI", 9.8f);
            numJ.Minimum = 0;
            numJ.Maximum = 9999;
            numJ.ValueChanged -= KoordinatDegeriGuncelle;
            numJ.ValueChanged += KoordinatDegeriGuncelle;
            pnlSag.Controls.Add(numJ);

            // Designer'daki pictureBox1 (Slayt/Formül Referans Görseli) varsa sağ panele ekle
            int bilgiKartiY = 288;
            if (pictureBox1 != null && pictureBox1.Image != null)
            {
                ModernKartPanel ornekKart = new ModernKartPanel
                {
                    Location = new System.Drawing.Point(18, 286),
                    Size = new System.Drawing.Size(252, 125),
                    Padding = new Padding(8)
                };

                Label lblOrnek = new Label
                {
                    Text = "Referans Şema",
                    Dock = DockStyle.Top,
                    Height = 20,
                    Font = new Font("Segoe UI Semibold", 8.8f, FontStyle.Bold),
                    ForeColor = FormGezintiYardimcisi.KoyuMaviYazi,
                    BackColor = Color.White
                };

                pictureBox1.Parent?.Controls.Remove(pictureBox1);
                pictureBox1.Dock = DockStyle.Fill;
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox1.BorderStyle = BorderStyle.None;
                pictureBox1.BackColor = Color.White;

                ornekKart.Controls.Add(pictureBox1);
                ornekKart.Controls.Add(lblOrnek);
                pnlSag.Controls.Add(ornekKart);

                bilgiKartiY = 422;
            }

            // Açık Mavi Bilgi Kartı
            ModernKartPanel bilgiKarti = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                konum: new System.Drawing.Point(18, bilgiKartiY),
                boyut: new System.Drawing.Size(252, 185),
                icerikEtiketi: out Label lblBilgi
            );
            lblBilgi.Text =
                "Mozaik & Blok Ortalama:\n\n" +
                "• Görüntü N×N piksellik bloklara ayrılır ve her bloğun gri seviye ortalaması hesaplanır.\n" +
                "• Seçilen (i, j) mozaik bloğu kırmızı çerçeve ile işaretlenir ve f_gray(i, j) değeri okunur.";
            pnlSag.Controls.Add(bilgiKarti);

            // 4. Orta Çalışma Alanı (Yan Yana 2 Kart + Alt Analiz Şeridi)
            Panel pnlOrta = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(14),
                BackColor = FormGezintiYardimcisi.TemaZemin
            };

            TableLayoutPanel anaYerlesim = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2
            };
            anaYerlesim.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            anaYerlesim.RowStyles.Add(new RowStyle(SizeType.Absolute, 54f));

            TableLayoutPanel kartIzgara = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1
            };
            kartIzgara.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            kartIzgara.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            kartIzgara.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            rozetOrijinal = new ModernHapRozet();
            rozetOrijinal.MetinGuncelle("Bekleniyor");

            rozetPikselDegeri = new ModernHapRozet();
            rozetPikselDegeri.MetinGuncelle("f_gray(i,j) = -");

            MevcutPictureBoxKartaTasi(kartIzgara, 0, 0, "Orijinal Gri Seviye Görüntü", picGri, rozetOrijinal);
            MevcutPictureBoxKartaTasi(kartIzgara, 1, 0, "Mozaik Görüntü (Seçili Blok İşaretli)", picMozaik, rozetPikselDegeri);

            // Alt Analiz Şeridi
            ModernKartPanel altSerit = FormGezintiYardimcisi.AltAnalizSeridiOlustur(
                "Gri seviye mozaik oluşturmak için sağ panelden bir resim seçiniz.",
                out lblAnaliz,
                "Hazır"
            );

            anaYerlesim.Controls.Add(kartIzgara, 0, 0);
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
            ModernHapRozet sagUstRozet = null)
        {
            ModernKartPanel kart = new ModernKartPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(8),
                Padding = new Padding(14)
            };

            Panel pnlKartUst = new Panel
            {
                Dock = DockStyle.Top,
                Height = 30,
                BackColor = Color.White
            };

            Label lbl = new Label
            {
                Text = baslik,
                UseMnemonic = false,
                AutoSize = true,
                Location = new System.Drawing.Point(0, 4),
                Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                BackColor = Color.White
            };
            pnlKartUst.Controls.Add(lbl);

            if (sagUstRozet != null)
            {
                sagUstRozet.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                sagUstRozet.Location = new System.Drawing.Point(pnlKartUst.Width - sagUstRozet.Width - 2, 2);
                pnlKartUst.Resize += (s, e) =>
                {
                    sagUstRozet.Location = new System.Drawing.Point(pnlKartUst.Width - sagUstRozet.Width - 2, 2);
                };
                pnlKartUst.Controls.Add(sagUstRozet);
            }

            mevcutPb.Parent?.Controls.Remove(mevcutPb);
            mevcutPb.Dock = DockStyle.Fill;
            mevcutPb.SizeMode = PictureBoxSizeMode.Zoom;
            mevcutPb.BorderStyle = BorderStyle.None;
            mevcutPb.BackColor = FormGezintiYardimcisi.TemaZemin;

            kart.Controls.Add(mevcutPb);
            kart.Controls.Add(pnlKartUst);
            tablo.Controls.Add(kart, sutun, satir);
        }

        private void btnResimSec_Click(object sender, EventArgs e)
        {
            string dosyaYolu = FormGezintiYardimcisi.ResimDosyasiSec("Gri Seviye Mozaik İçin Resim Seçiniz");
            if (string.IsNullOrEmpty(dosyaYolu)) return;

            using (Mat yuklenen = Cv2.ImRead(dosyaYolu, ImreadModes.Color))
            {
                if (yuklenen.Empty())
                {
                    MessageBox.Show("Resim yüklenemedi!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                griMat?.Dispose();
                griMat = new Mat();
                Cv2.CvtColor(yuklenen, griMat, ColorConversionCodes.BGR2GRAY);

                picGri.Image?.Dispose();
                picGri.Image = BitmapConverter.ToBitmap(griMat);

                mozaikMat?.Dispose();
                mozaikMat = null;

                picMozaik.Image?.Dispose();
                picMozaik.Image = null;

                rozetOrijinal.MetinGuncelle($"{griMat.Width} × {griMat.Height}");
                rozetPikselDegeri.MetinGuncelle("f_gray(i,j) = -");
                lblAnaliz.Text = $"Görüntü yüklendi ({griMat.Width}×{griMat.Height} px) • Adım boyutunu seçip 'Mozaik Oluştur' butonuna tıklayınız.";
            }
        }

        private void btnMozaikOlustur_Click(object sender, EventArgs e)
        {
            if (griMat == null)
            {
                MessageBox.Show("Lütfen önce bir resim seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            blokBoyutu = 10;
            if (cmbMozaikBoyut.SelectedItem != null)
            {
                int.TryParse(cmbMozaikBoyut.SelectedItem.ToString(), out blokBoyutu);
                if (blokBoyutu <= 0) blokBoyutu = 1;
            }

            int yeniGenislik = griMat.Width / blokBoyutu;
            int yeniYukseklik = griMat.Height / blokBoyutu;

            if (yeniGenislik <= 0 || yeniYukseklik <= 0) return;

            mozaikMat?.Dispose();
            mozaikMat = new Mat(yeniYukseklik, yeniGenislik, MatType.CV_8UC1);

            for (int i = 0; i < yeniYukseklik; i++)
            {
                for (int j = 0; j < yeniGenislik; j++)
                {
                    int baslangicY = i * blokBoyutu;
                    int baslangicX = j * blokBoyutu;

                    long toplamGri = 0;
                    int sayac = 0;

                    for (int y = baslangicY; y < baslangicY + blokBoyutu && y < griMat.Height; y++)
                    {
                        for (int x = baslangicX; x < baslangicX + blokBoyutu && x < griMat.Width; x++)
                        {
                            toplamGri += griMat.At<byte>(y, x);
                            sayac++;
                        }
                    }

                    byte ortalamaGri = (byte)(toplamGri / sayac);
                    mozaikMat.Set<byte>(i, j, ortalamaGri);
                }
            }

            numI.Maximum = Math.Max(0, yeniYukseklik - 1);
            numJ.Maximum = Math.Max(0, yeniGenislik - 1);

            KoordinatDegeriGuncelle(null, EventArgs.Empty);
        }

        private void KoordinatDegeriGuncelle(object sender, EventArgs e)
        {
            if (mozaikMat == null || griMat == null) return;

            int i = (int)numI.Value;
            int j = (int)numJ.Value;

            if (i >= mozaikMat.Height || j >= mozaikMat.Width) return;

            byte deger = mozaikMat.At<byte>(i, j);

            rozetPikselDegeri.MetinGuncelle($"f_gray({i},{j}) = {deger}");
            lblAnaliz.Text = $"Adım: {blokBoyutu}×{blokBoyutu} • Mozaik Boyutu: {mozaikMat.Width}×{mozaikMat.Height} • Seçilen Piksel Değeri: f_gray({i}, {j}) = {deger}";

            using (Mat gosterimMat = new Mat())
            {
                Cv2.Resize(
                    mozaikMat,
                    gosterimMat,
                    new OpenCvSharp.Size(griMat.Width, griMat.Height),
                    0, 0,
                    InterpolationFlags.Nearest);

                using (Mat renkliGosterim = new Mat())
                {
                    Cv2.CvtColor(gosterimMat, renkliGosterim, ColorConversionCodes.GRAY2BGR);

                    int x = j * blokBoyutu;
                    int y = i * blokBoyutu;
                    int merkezX = x + (blokBoyutu / 2);
                    int merkezY = y + (blokBoyutu / 2);

                    // Yüksek çözünürlüklü resimlerde Zoom küçültmesinde kaybolmaması için dinamik kalınlık
                    int kalinlik = Math.Max(2, griMat.Width / 300);
                    int nisanBoyu = Math.Max(18, griMat.Width / 35);
                    Scalar kirmizi = new Scalar(0, 0, 255);

                    // 1. Seçilen bloğun kendi çerçevesi
                    OpenCvSharp.Rect secilenAlan = new OpenCvSharp.Rect(x, y, blokBoyutu, blokBoyutu);
                    Cv2.Rectangle(renkliGosterim, secilenAlan, kirmizi, kalinlik);

                    // 2. Küçük blokların (ör. 2x2) yüksek çözünürlükte net seçilebilmesi için kırmızı hedef işareti
                    Cv2.Line(renkliGosterim,
                        new OpenCvSharp.Point(Math.Max(0, merkezX - nisanBoyu), merkezY),
                        new OpenCvSharp.Point(Math.Min(renkliGosterim.Width - 1, merkezX + nisanBoyu), merkezY),
                        kirmizi, kalinlik);

                    Cv2.Line(renkliGosterim,
                        new OpenCvSharp.Point(merkezX, Math.Max(0, merkezY - nisanBoyu)),
                        new OpenCvSharp.Point(merkezX, Math.Min(renkliGosterim.Height - 1, merkezY + nisanBoyu)),
                        kirmizi, kalinlik);

                    picMozaik.Image?.Dispose();
                    picMozaik.Image = BitmapConverter.ToBitmap(renkliGosterim);
                }
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            griMat?.Dispose();
            mozaikMat?.Dispose();
            base.OnFormClosed(e);
        }
    }
}