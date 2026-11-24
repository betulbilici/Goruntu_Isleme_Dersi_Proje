using System;
using System.Drawing;
using System.Windows.Forms;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace GoruntuIsleme
{
    public partial class Hafta3Odev6 : Form
    {
        private Bitmap orijinalBmp = null;

        private ModernHapRozet rozetOrijinal;
        private ModernHapRozet rozetDegistirme;
        private ModernHapRozet rozetInterpolasyon;

        private ModernSurgu surguOran;
        private Label lblValOran;
        private Label lblAnaliz;

        public Hafta3Odev6()
        {
            InitializeComponent();
            ModernArayuzuUygula();
        }

        private void ModernArayuzuUygula()
        {
            // 1. Form Temel Ayarları (1250x750, Standart Kenarlık ve X ile Ana Menüye Dönüş)
            FormGezintiYardimcisi.FormuHazirla(this);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Text = "Hafta 3 - Ödev 6: Oransal Resim Küçültme (Piksel Değiştirme ve Bilineer İnterpolasyon)";

            // Designer'daki tüm eski kontrolleri gizle
            foreach (Control ctrl in this.Controls)
            {
                ctrl.Visible = false;
            }

            // 2. Üst Gezinti Barı
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                form: this,
                baslik: "Hafta 3 - Ödev 6 | Oransal Resim Küçültme: Piksel Değiştirme vs Bilineer İnterpolasyon",
                oncekiYazi: "< Ödev 5",
                oncekiTikla: () =>
                {
                    Form odev5 = Application.OpenForms["Hafta3Odev5"];
                    if (odev5 != null)
                        FormGezintiYardimcisi.FormGecisYap(this, odev5);
                    else
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta3Odev5());
                },
                sonrakiYazi: "Ödev 7 >",
                sonrakiTikla: () =>
                {
                    Form odev7 = Application.OpenForms["Hafta3Odev7"];
                    if (odev7 != null)
                        FormGezintiYardimcisi.FormGecisYap(this, odev7);
                    else
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta3Odev7());
                }
            );

            // 3. Sağ Kontrol Paneli
            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 290);

            Label lblKontrolBaslik = new Label
            {
                Text = "Oransal Küçültme",
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

            // Küçültme Oranı (%) Modern Sürgüsü (%5 - %95)
            Label lblOranBaslik = new Label
            {
                Text = "Küçültme Oranı (%)",
                Location = new Point(18, 102),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi
            };
            pnlSag.Controls.Add(lblOranBaslik);

            lblValOran = new Label
            {
                Text = "%50",
                Location = new Point(210, 102),
                Size = new Size(60, 18),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi
            };
            pnlSag.Controls.Add(lblValOran);

            surguOran = new ModernSurgu
            {
                Minimum = 5,
                Maximum = 95,
                Value = 50,
                Location = new Point(18, 122),
                Size = new Size(252, 24)
            };
            surguOran.Scroll += (s, e) =>
            {
                lblValOran.Text = $"%{surguOran.Value}";
                if (orijinalBmp != null) btnKucult_Click(null, null);
            };
            pnlSag.Controls.Add(surguOran);

            // Resmi Küçült Butonu
            ModernHapButon btnModernKucult = FormGezintiYardimcisi.ModernButonOlustur("📉 Resmi Küçült ve Karşılaştır", true, 252, 40);
            btnModernKucult.Location = new Point(18, 158);
            btnModernKucult.Click += btnKucult_Click;
            pnlSag.Controls.Add(btnModernKucult);

            // Açık Mavi Bilgi Kartı
            ModernKartPanel bilgiKarti = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                konum: new Point(18, 214),
                boyut: new Size(252, 340),
                icerikEtiketi: out Label lblBilgiKart
            );
            lblBilgiKart.Text =
                "Küçültme Yöntemleri:\n\n" +
                "• Tuval boyutu orijinal görüntüyle aynı tutulur; küçültülen resim sol üst köşeye yerleştirilir.\n\n" +
                "1. Piksel Değiştirme (Subsampling):\n" +
                "Belirli adımlarla piksel atlayarak örnek alır. Hızlıdır ancak keskin kenarlarda basamaklanma yapabilir.\n\n" +
                "2. Bilineer İnterpolasyon:\n" +
                "Komşu 4 pikselin ağırlıklı ortalamasını hesaplayarak daha pürüzsüz küçültme sağlar.";
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
            goruntuIzgara.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            goruntuIzgara.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            goruntuIzgara.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34f));
            goruntuIzgara.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            rozetOrijinal = new ModernHapRozet(); rozetOrijinal.MetinGuncelle("%100");
            rozetDegistirme = new ModernHapRozet(); rozetDegistirme.MetinGuncelle("Bekleniyor");
            rozetInterpolasyon = new ModernHapRozet(); rozetInterpolasyon.MetinGuncelle("Bekleniyor");

            MevcutPictureBoxKartaTasi(goruntuIzgara, 0, 0, "Orijinal Resim", picOrijinal, FormGezintiYardimcisi.TemaZemin, rozetOrijinal);
            MevcutPictureBoxKartaTasi(goruntuIzgara, 1, 0, "Piksel Değiştirme", picDegistirme, FormGezintiYardimcisi.TemaZemin, rozetDegistirme);
            MevcutPictureBoxKartaTasi(goruntuIzgara, 2, 0, "Bilineer İnterpolasyon", picInterpolasyon, FormGezintiYardimcisi.TemaZemin, rozetInterpolasyon);

            // Alt Analiz Şeridi
            ModernKartPanel altSerit = FormGezintiYardimcisi.AltAnalizSeridiOlustur(
                "Bir resim seçin, küçültme yüzdesini (%5–%95) belirleyin ve iki küçültme yöntemini yan yana inceleyin.",
                out lblAnaliz,
                "Oransal Küçültme"
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
                Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold),
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
            mevcutPb.SizeMode = PictureBoxSizeMode.Zoom;
            mevcutPb.BorderStyle = BorderStyle.None;
            mevcutPb.BackColor = zeminRengi;

            kart.Controls.Add(mevcutPb);
            kart.Controls.Add(pnlKartUst);
            tablo.Controls.Add(kart, sutun, satir);
        }

        private void btnResimSec_Click(object sender, EventArgs e)
        {
            string dosyaYolu = FormGezintiYardimcisi.ResimDosyasiSec("Oransal Küçültme İçin Resim Seçiniz");
            if (string.IsNullOrEmpty(dosyaYolu)) return;

            orijinalBmp?.Dispose();
            using (Bitmap temp = new Bitmap(dosyaYolu))
            {
                orijinalBmp = new Bitmap(temp);
            }

            picOrijinal.Image?.Dispose();
            picOrijinal.SizeMode = PictureBoxSizeMode.Zoom;
            picOrijinal.Image = new Bitmap(orijinalBmp);

            rozetOrijinal.MetinGuncelle($"{orijinalBmp.Width}×{orijinalBmp.Height}");
            lblAnaliz.Text = $"Resim yüklendi ({orijinalBmp.Width}×{orijinalBmp.Height} px) • Küçültme oranı: %{surguOran.Value}";
            btnKucult_Click(null, null);
        }

        private void btnKucult_Click(object sender, EventArgs e)
        {
            if (orijinalBmp == null)
            {
                MessageBox.Show("Lütfen önce bir resim seçiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            double yuzde = surguOran.Value;
            double oran = yuzde / 100.0;

            int kaynakW = orijinalBmp.Width;
            int kaynakH = orijinalBmp.Height;

            int hedefW = Math.Max(1, (int)Math.Round(kaynakW * oran));
            int hedefH = Math.Max(1, (int)Math.Round(kaynakH * oran));

            this.Cursor = Cursors.WaitCursor;

            // Tuval orijinal boyutta kalır, küçülen resim sol üste oturur
            Bitmap bmpDegistirme = new Bitmap(kaynakW, kaynakH);
            using (Graphics g = Graphics.FromImage(bmpDegistirme))
            {
                g.Clear(Color.FromArgb(203, 213, 225));
            }

            Bitmap bmpInterpolasyon = new Bitmap(kaynakW, kaynakH);
            using (Graphics g = Graphics.FromImage(bmpInterpolasyon))
            {
                g.Clear(Color.FromArgb(203, 213, 225));
            }

            // YÖNTEM A: PİKSEL DEĞİŞTİRME
            for (int y = 0; y < hedefH; y++)
            {
                int srcY = Math.Min((int)Math.Floor(y / oran), kaynakH - 1);
                for (int x = 0; x < hedefW; x++)
                {
                    int srcX = Math.Min((int)Math.Floor(x / oran), kaynakW - 1);
                    bmpDegistirme.SetPixel(x, y, orijinalBmp.GetPixel(srcX, srcY));
                }
            }

            // YÖNTEM B: PİKSEL İNTERPOLASYONU
            for (int y = 0; y < hedefH; y++)
            {
                float gy = (float)(y / oran);
                int y1 = Math.Min((int)Math.Floor(gy), kaynakH - 1);
                int y2 = Math.Min(y1 + 1, kaynakH - 1);
                float dy = gy - y1;

                for (int x = 0; x < hedefW; x++)
                {
                    float gx = (float)(x / oran);
                    int x1 = Math.Min((int)Math.Floor(gx), kaynakW - 1);
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
            picDegistirme.SizeMode = PictureBoxSizeMode.Zoom;
            picDegistirme.Image = bmpDegistirme;

            picInterpolasyon.Image?.Dispose();
            picInterpolasyon.SizeMode = PictureBoxSizeMode.Zoom;
            picInterpolasyon.Image = bmpInterpolasyon;

            rozetDegistirme.MetinGuncelle($"%{yuzde} • {hedefW}×{hedefH}");
            rozetInterpolasyon.MetinGuncelle($"%{yuzde} • {hedefW}×{hedefH}");
            lblAnaliz.Text = $"Küçültme Oranı: %{yuzde} • Orijinal: {kaynakW}×{kaynakH} px → Yeni Boyut: {hedefW}×{hedefH} px";

            this.Cursor = Cursors.Default;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            orijinalBmp?.Dispose();
            base.OnFormClosed(e);
        }
    }
}