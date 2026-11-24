using System;
using System.Drawing;
using System.Windows.Forms;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace GoruntuIsleme
{
    public partial class Hafta3Odev4 : Form
    {
        private Bitmap orijinalBmp = null;

        private ModernHapRozet rozetOrijinal;
        private ModernHapRozet rozetDegistirme;
        private ModernHapRozet rozetInterpolasyon;

        private ModernSurgu surguOran;
        private Label lblValOran;
        private Label lblAnaliz;

        public Hafta3Odev4()
        {
            InitializeComponent();
            ModernArayuzuUygula();
        }

        private void ModernArayuzuUygula()
        {
            // 1. Form Temel Ayarları (1250x750, Standart Kenarlık ve X ile Ana Menüye Dönüş)
            FormGezintiYardimcisi.FormuHazirla(this);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Text = "Hafta 3 - Ödev 4: Resim Büyütme (Piksel Değiştirme ve Bilineer İnterpolasyon)";

            // Designer'daki tüm eski kontrolleri gizle
            foreach (Control ctrl in this.Controls)
            {
                ctrl.Visible = false;
            }

            // 2. Üst Gezinti Barı
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                form: this,
                baslik: "Hafta 3 - Ödev 4 | Resim Büyütme: Piksel Değiştirme (Nearest Neighbor) vs Bilineer İnterpolasyon",
                oncekiYazi: "< Ödev 3",
                oncekiTikla: () =>
                {
                    Form odev3 = Application.OpenForms["Hafta3Odev3"];
                    if (odev3 != null)
                        FormGezintiYardimcisi.FormGecisYap(this, odev3);
                    else
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta3Odev3());
                },
                sonrakiYazi: "Ödev 5 >",
                sonrakiTikla: () =>
                {
                    Form odev5 = Application.OpenForms["Hafta3Odev5"];
                    if (odev5 != null)
                        FormGezintiYardimcisi.FormGecisYap(this, odev5);
                    else
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta3Odev5());
                }
            );

            // 3. Sağ Kontrol Paneli
            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 290);

            Label lblKontrolBaslik = new Label
            {
                Text = "Ölçekleme & Büyütme",
                Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                Location = new Point(18, 16),
                AutoSize = true
            };
            pnlSag.Controls.Add(lblKontrolBaslik);

            // Küçük Resim Seç Butonu
            ModernHapButon btnModernResimSec = FormGezintiYardimcisi.ModernButonOlustur("📁 Küçük Resim Seç", true, 252, 38);
            btnModernResimSec.Location = new Point(18, 48);
            btnModernResimSec.Click += btnResimSec_Click;
            pnlSag.Controls.Add(btnModernResimSec);

            // Büyütme Oranı Modern Sürgüsü (2x - 6x)
            Label lblOranBaslik = new Label
            {
                Text = "Büyütme Oranı (Ölçek)",
                Location = new Point(18, 102),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi
            };
            pnlSag.Controls.Add(lblOranBaslik);

            lblValOran = new Label
            {
                Text = "2x",
                Location = new Point(210, 102),
                Size = new Size(60, 18),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi
            };
            pnlSag.Controls.Add(lblValOran);

            surguOran = new ModernSurgu
            {
                Minimum = 2,
                Maximum = 6,
                Value = 2,
                Location = new Point(18, 122),
                Size = new Size(252, 24)
            };
            surguOran.Scroll += (s, e) =>
            {
                lblValOran.Text = $"{surguOran.Value}x";
                if (orijinalBmp != null) btnBuyut_Click(null, null);
            };
            pnlSag.Controls.Add(surguOran);

            // Resmi Büyüt Butonu
            ModernHapButon btnModernBuyut = FormGezintiYardimcisi.ModernButonOlustur("🔍 Resmi Büyüt ve Karşılaştır", true, 252, 40);
            btnModernBuyut.Location = new Point(18, 158);
            btnModernBuyut.Click += btnBuyut_Click;
            pnlSag.Controls.Add(btnModernBuyut);

            // Açık Mavi Yöntem Karşılaştırma Kartı
            ModernKartPanel bilgiKarti = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                konum: new Point(18, 214),
                boyut: new Size(252, 340),
                icerikEtiketi: out Label lblBilgiKart
            );
            lblBilgiKart.Text =
                "Yöntem Karşılaştırması:\n\n" +
                "1. Orijinal Görüntü: Blok yapısını net gözlemlemek için maksimum 90×90 px boyutuna ölçeklenir.\n\n" +
                "2. Piksel Değiştirme (a Şıkkı):\n" +
                "Nearest Neighbor yöntemiyle her piksel ölçek katı kadar kopyalanır; keskin ama bloklu (pikselli) sonuç üretir.\n\n" +
                "3. Bilineer İnterpolasyon (b Şıkkı):\n" +
                "Komşu 4 pikselin ağırlıklı ortalamasını alarak yumuşak renk geçişleri oluşturur.";
            pnlSag.Controls.Add(bilgiKarti);

            // 4. Orta Çalışma Alanı (Üst: 3 Karşılaştırma Kartı | Alt: Analiz Şeridi)
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

            // 3 Sütunlu Karşılaştırma Izgarası
            TableLayoutPanel goruntuIzgara = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1
            };
            goruntuIzgara.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 26f));
            goruntuIzgara.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 37f));
            goruntuIzgara.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 37f));
            goruntuIzgara.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            rozetOrijinal = new ModernHapRozet(); rozetOrijinal.MetinGuncelle("1x");
            rozetDegistirme = new ModernHapRozet(); rozetDegistirme.MetinGuncelle("a) Bloklu");
            rozetInterpolasyon = new ModernHapRozet(); rozetInterpolasyon.MetinGuncelle("b) Yumuşak");

            MevcutPictureBoxKartaTasi(goruntuIzgara, 0, 0, "Orijinal Küçük Resim", picOrijinal, FormGezintiYardimcisi.TemaZemin, rozetOrijinal);
            MevcutPictureBoxKartaTasi(goruntuIzgara, 1, 0, "Piksel Değiştirme (Nearest)", picDegistirme, FormGezintiYardimcisi.TemaZemin, rozetDegistirme);
            MevcutPictureBoxKartaTasi(goruntuIzgara, 2, 0, "Bilineer İnterpolasyon", picInterpolasyon, FormGezintiYardimcisi.TemaZemin, rozetInterpolasyon);

            // Alt Analiz Şeridi
            ModernKartPanel altSerit = FormGezintiYardimcisi.AltAnalizSeridiOlustur(
                "Küçük bir resim seçin, büyütme oranını belirleyin (2x–6x) ve iki büyütme yöntemini yan yana karşılaştırın.",
                out lblAnaliz,
                "Ölçekleme"
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
                Font = new Font("Segoe UI Semibold", 9.4f, FontStyle.Bold),
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
            mevcutPb.SizeMode = PictureBoxSizeMode.CenterImage;
            mevcutPb.BorderStyle = BorderStyle.None;
            mevcutPb.BackColor = zeminRengi;

            kart.Controls.Add(mevcutPb);
            kart.Controls.Add(pnlKartUst);
            tablo.Controls.Add(kart, sutun, satir);
        }

        private void btnResimSec_Click(object sender, EventArgs e)
        {
            string dosyaYolu = FormGezintiYardimcisi.ResimDosyasiSec("Büyütme İşlemi İçin Küçük Resim Seçiniz");
            if (string.IsNullOrEmpty(dosyaYolu)) return;

            orijinalBmp?.Dispose();
            using (Bitmap temp = new Bitmap(dosyaYolu))
            {
                int maxBoyut = 90;
                if (temp.Width > maxBoyut || temp.Height > maxBoyut)
                {
                    double oran = Math.Min((double)maxBoyut / temp.Width, (double)maxBoyut / temp.Height);
                    int yeniW = Math.Max(1, (int)(temp.Width * oran));
                    int yeniH = Math.Max(1, (int)(temp.Height * oran));
                    orijinalBmp = new Bitmap(temp, yeniW, yeniH);
                }
                else
                {
                    orijinalBmp = new Bitmap(temp);
                }
            }

            picOrijinal.Image?.Dispose();
            picOrijinal.SizeMode = PictureBoxSizeMode.CenterImage;
            picOrijinal.Image = new Bitmap(orijinalBmp);

            picDegistirme.Image?.Dispose();
            picDegistirme.Image = null;

            picInterpolasyon.Image?.Dispose();
            picInterpolasyon.Image = null;

            rozetOrijinal.MetinGuncelle($"{orijinalBmp.Width}×{orijinalBmp.Height}");
            rozetDegistirme.MetinGuncelle("Hazır");
            rozetInterpolasyon.MetinGuncelle("Hazır");

            lblAnaliz.Text = $"Küçük resim yüklendi ({orijinalBmp.Width}×{orijinalBmp.Height} px) • Büyütme oranını seçip karşılaştırabilirsiniz.";
            btnBuyut_Click(null, null);
        }

        private void btnBuyut_Click(object sender, EventArgs e)
        {
            if (orijinalBmp == null)
            {
                MessageBox.Show("Lütfen önce bir resim seçiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int olcek = surguOran.Value;
            int kaynakW = orijinalBmp.Width;
            int kaynakH = orijinalBmp.Height;

            int hedefW = kaynakW * olcek;
            int hedefH = kaynakH * olcek;

            this.Cursor = Cursors.WaitCursor;

            // YÖNTEM A: PİKSEL DEĞİŞTİRME (NEAREST NEIGHBOR / BLOK KOPYALAMA)
            Bitmap bmpDegistirme = new Bitmap(hedefW, hedefH);
            for (int y = 0; y < hedefH; y++)
            {
                int srcY = Math.Min(y / olcek, kaynakH - 1);
                for (int x = 0; x < hedefW; x++)
                {
                    int srcX = Math.Min(x / olcek, kaynakW - 1);
                    bmpDegistirme.SetPixel(x, y, orijinalBmp.GetPixel(srcX, srcY));
                }
            }

            // YÖNTEM B: PİKSEL İNTERPOLASYONU (BİLİNEER / KADEMELİ GEÇİŞ)
            Bitmap bmpInterpolasyon = new Bitmap(hedefW, hedefH);
            for (int y = 0; y < hedefH; y++)
            {
                float gy = (float)y / olcek;
                int y1 = (int)Math.Floor(gy);
                int y2 = Math.Min(y1 + 1, kaynakH - 1);
                float dy = gy - y1;

                for (int x = 0; x < hedefW; x++)
                {
                    float gx = (float)x / olcek;
                    int x1 = (int)Math.Floor(gx);
                    int x2 = Math.Min(x1 + 1, kaynakW - 1);
                    float dx = gx - x1;

                    Color c11 = orijinalBmp.GetPixel(x1, y1);
                    Color c21 = orijinalBmp.GetPixel(x2, y1);
                    Color c12 = orijinalBmp.GetPixel(x1, y2);
                    Color c22 = orijinalBmp.GetPixel(x2, y2);

                    int r = (int)(c11.R * (1 - dx) * (1 - dy) + c21.R * dx * (1 - dy) + c12.R * (1 - dx) * dy + c22.R * dx * dy);
                    int gVal = (int)(c11.G * (1 - dx) * (1 - dy) + c21.G * dx * (1 - dy) + c12.G * (1 - dx) * dy + c22.G * dx * dy);
                    int b = (int)(c11.B * (1 - dx) * (1 - dy) + c21.B * dx * (1 - dy) + c12.B * (1 - dx) * dy + c22.B * dx * dy);

                    bmpInterpolasyon.SetPixel(x, y, Color.FromArgb(
                        Math.Clamp(r, 0, 255),
                        Math.Clamp(gVal, 0, 255),
                        Math.Clamp(b, 0, 255)
                    ));
                }
            }

            picDegistirme.Image?.Dispose();
            picDegistirme.SizeMode = PictureBoxSizeMode.CenterImage;
            picDegistirme.Image = bmpDegistirme;

            picInterpolasyon.Image?.Dispose();
            picInterpolasyon.SizeMode = PictureBoxSizeMode.CenterImage;
            picInterpolasyon.Image = bmpInterpolasyon;

            rozetDegistirme.MetinGuncelle($"{olcek}x • {hedefW}×{hedefH}");
            rozetInterpolasyon.MetinGuncelle($"{olcek}x • {hedefW}×{hedefH}");
            lblAnaliz.Text = $"Orijinal: {kaynakW}×{kaynakH} px → Büyütülmüş ({olcek}x): {hedefW}×{hedefH} px • Bloklu vs Yumuşak Geçiş karşılaştırılıyor.";

            this.Cursor = Cursors.Default;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            orijinalBmp?.Dispose();
            base.OnFormClosed(e);
        }
    }
}