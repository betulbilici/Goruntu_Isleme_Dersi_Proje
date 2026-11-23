using System;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace GoruntuIsleme
{
    public partial class Hafta1Odev1 : Form
    {
        private ModernHapRozet rozetBoyut;
        private ModernHapRozet rozetAralik;
        private Label lblAnaliz;

        public Hafta1Odev1()
        {
            InitializeComponent();
            ModernArayuzuUygula();

            // Form açıldığında varsayılan değerle (N = 255) gradyan matrisini oluştur
            btnOlustur_Click(this, EventArgs.Empty);
        }

        private void ModernArayuzuUygula()
        {
            // 1. Form Temel Ayarları (1250x750, Standart Kenarlık ve X ile Ana Menüye Dönüş)
            FormGezintiYardimcisi.FormuHazirla(this);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Text = "Hafta 1 - Ödev 1: Gri Seviye Gradyan Matrisi Oluşturma";

            // Designer'daki eski etiket, buton ve linkleri gizle
            pictureBox1.Visible = false;
            btnOlustur.Visible = false;
            label1.Visible = false;
            lnkOdev2.Visible = false;

            // 2. Üst Gezinti Barı
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                form: this,
                baslik: "Hafta 1 - Ödev 1 | (N+1)×(N+1) Gri Seviye Gradyan Matrisi",
                oncekiYazi: null,
                oncekiTikla: null,
                sonrakiYazi: "Ödev 2 >",
                sonrakiTikla: () => FormGezintiYardimcisi.FormGecisYap(this, new Hafta1Odev2())
            );

            // 3. Sağ Kontrol Paneli
            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 290);

            Label lblKontrolBaslik = new Label
            {
                Text = "Matris Parametreleri",
                Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                Location = new Point(18, 18),
                AutoSize = true
            };
            pnlSag.Controls.Add(lblKontrolBaslik);

            // Maksimum Gri Değeri (N)
            Label lblN = new Label
            {
                Text = "Maksimum Gri Değeri (N):",
                Font = new Font("Segoe UI Semibold", 9.2f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                Location = new Point(18, 62),
                AutoSize = true
            };
            pnlSag.Controls.Add(lblN);

            numGenislik.Parent?.Controls.Remove(numGenislik);
            numGenislik.Location = new Point(18, 84);
            numGenislik.Size = new Size(252, 32);
            numGenislik.Font = new Font("Segoe UI", 10f);
            numGenislik.Minimum = 1;
            numGenislik.Maximum = 255;
            numGenislik.Value = 255;
            pnlSag.Controls.Add(numGenislik);

            // Görüntüyü Oluştur Butonu
            ModernHapButon btnModernOlustur = FormGezintiYardimcisi.ModernButonOlustur("⬛ Görüntüyü Oluştur", true, 252, 40);
            btnModernOlustur.Location = new Point(18, 134);
            btnModernOlustur.Click += btnOlustur_Click;
            pnlSag.Controls.Add(btnModernOlustur);

            // Varsayılan (N = 255) Butonu
            ModernHapButon btnVarsayilan = FormGezintiYardimcisi.ModernButonOlustur("🔄 Tam Skala (N = 255)", false, 252, 36);
            btnVarsayilan.Location = new Point(18, 182);
            btnVarsayilan.Click += (s, e) =>
            {
                numGenislik.Value = 255;
                btnOlustur_Click(this, EventArgs.Empty);
            };
            pnlSag.Controls.Add(btnVarsayilan);

            // Açık Mavi Teorik Bilgi Kartı
            ModernKartPanel bilgiKarti = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                konum: new Point(18, 236),
                boyut: new Size(252, 235),
                icerikEtiketi: out Label lblBilgi
            );
            lblBilgi.Text =
                "Matris Oluşturma Mantığı:\n\n" +
                "• Boyut: (N + 1) × (N + 1) piksel\n" +
                "• Piksel Değeri: f(y, x) = x\n\n" +
                "Soldan (0 = Tam Siyah) sağa (N = En Açık Gri / 255 Tam Beyaz) doğru yatay eksende doğrusal parlaklık geçişi üretilir.";
            pnlSag.Controls.Add(bilgiKarti);

            // 4. Orta Çalışma Alanı (Tek Geniş Kart + Alt Analiz Şeridi)
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

            ModernKartPanel sonucKart = new ModernKartPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(8),
                Padding = new Padding(14)
            };

            Panel pnlKartUst = new Panel
            {
                Dock = DockStyle.Top,
                Height = 32,
                BackColor = Color.White
            };

            Label sonucBaslik = new Label
            {
                Text = "Oluşturulan Gri Seviye Matris Görüntüsü (CV_8UC1)",
                UseMnemonic = false,
                AutoSize = true,
                Location = new Point(0, 5),
                Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                BackColor = Color.White
            };
            pnlKartUst.Controls.Add(sonucBaslik);

            rozetBoyut = new ModernHapRozet();
            rozetBoyut.MetinGuncelle("256 × 256");

            rozetAralik = new ModernHapRozet();
            rozetAralik.MetinGuncelle("Ton: 0..255");

            void RozetleriHizala()
            {
                rozetAralik.Location = new Point(pnlKartUst.Width - rozetAralik.Width - 2, 3);
                rozetBoyut.Location = new Point(rozetAralik.Left - rozetBoyut.Width - 8, 3);
            }

            pnlKartUst.Resize += (s, e) => RozetleriHizala();
            pnlKartUst.Controls.Add(rozetBoyut);
            pnlKartUst.Controls.Add(rozetAralik);
            RozetleriHizala();

            picGoruntu.Parent?.Controls.Remove(picGoruntu);
            picGoruntu.Dock = DockStyle.Fill;
            picGoruntu.BorderStyle = BorderStyle.None;
            picGoruntu.SizeMode = PictureBoxSizeMode.Zoom;
            picGoruntu.BackColor = FormGezintiYardimcisi.TemaZemin;

            sonucKart.Controls.Add(picGoruntu);
            sonucKart.Controls.Add(pnlKartUst);

            // Alt Analiz Şeridi
            ModernKartPanel altSerit = FormGezintiYardimcisi.AltAnalizSeridiOlustur(
                "Maksimum gri değerini (N) belirleyip 'Görüntüyü Oluştur' butonuna tıklayınız.",
                out lblAnaliz,
                "Hazır"
            );

            anaYerlesim.Controls.Add(sonucKart, 0, 0);
            anaYerlesim.Controls.Add(altSerit, 0, 1);
            pnlOrta.Controls.Add(anaYerlesim);

            this.Controls.Add(pnlOrta);
            FormGezintiYardimcisi.YerlesimiDuzenle(this, pnlUst, pnlSag, pnlOrta);
        }

        private void btnOlustur_Click(object sender, EventArgs e)
        {
            if (numGenislik == null || picGoruntu == null)
                return;

            int N = (int)numGenislik.Value;
            int genislik = N + 1;
            int yukseklik = N + 1;

            using (Mat matris = new Mat(yukseklik, genislik, MatType.CV_8UC1))
            {
                for (int y = 0; y < yukseklik; y++)
                {
                    for (int x = 0; x < genislik; x++)
                    {
                        matris.Set<byte>(y, x, (byte)x);
                    }
                }

                picGoruntu.Image?.Dispose();
                picGoruntu.Image = BitmapConverter.ToBitmap(matris);
            }

            rozetBoyut?.MetinGuncelle($"{genislik} × {yukseklik}");
            rozetAralik?.MetinGuncelle($"Ton: 0..{N}");
            if (lblAnaliz != null)
            {
                lblAnaliz.Text = $"Matris oluşturuldu: ({N}+1)×({N}+1) = {genislik}×{yukseklik} px • Piksel yoğunluk aralığı: [0, {N}]";
            }
        }
    }
}