using System;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace GoruntuIsleme
{
    public partial class Hafta1Odev2 : Form
    {
        private ModernHapRozet rozetBoyut;
        private ModernHapRozet rozetYaricap;
        private Label lblAnaliz;

        public Hafta1Odev2()
        {
            InitializeComponent();
            ModernArayuzuUygula();

            // Form açıldığında varsayılan değerlerle (N=512, r=100) daireyi oluştur
            btnOlustur_Click(this, EventArgs.Empty);
        }

        private void ModernArayuzuUygula()
        {
            // 1. Form Temel Ayarları (1250x750, Standart Kenarlık ve X ile Ana Menüye Dönüş)
            FormGezintiYardimcisi.FormuHazirla(this);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Text = "Hafta 1 - Ödev 2: Siyah Zemin Üzerine Beyaz Daire Oluşturma";

            // Designer'daki eski etiket, buton ve linkleri gizle
            pictureBox1.Visible = false;
            btnOlustur.Visible = false;
            lnkOdev1.Visible = false;
            lnkOdev3.Visible = false;
            label1.Visible = false;
            label2.Visible = false;

            // 2. Üst Gezinti Barı
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                form: this,
                baslik: "Hafta 1 - Ödev 2 | N×N Siyah Matris Üzerine r Yarıçaplı Beyaz Daire",
                oncekiYazi: "< Ödev 1",
                oncekiTikla: () =>
                {
                    Form odev1 = Application.OpenForms["Hafta1Odev1"];
                    if (odev1 != null)
                        FormGezintiYardimcisi.FormGecisYap(this, odev1);
                    else
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta1Odev1());
                },
                sonrakiYazi: "Ödev 3 >",
                sonrakiTikla: () => FormGezintiYardimcisi.FormGecisYap(this, new Hafta1Odev3())
            );

            // 3. Sağ Kontrol Paneli
            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 290);

            Label lblKontrolBaslik = new Label
            {
                Text = "Daire & Matris Parametreleri",
                Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                Location = new Point(18, 18),
                AutoSize = true
            };
            pnlSag.Controls.Add(lblKontrolBaslik);

            // Matris Boyutu (N)
            Label lblBoyut = new Label
            {
                Text = "Matris Boyutu (N × N):",
                Font = new Font("Segoe UI Semibold", 9.2f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                Location = new Point(18, 62),
                AutoSize = true
            };
            pnlSag.Controls.Add(lblBoyut);

            numBoyut.Parent?.Controls.Remove(numBoyut);
            numBoyut.Location = new Point(18, 84);
            numBoyut.Size = new Size(252, 32);
            numBoyut.Font = new Font("Segoe UI", 10f);
            numBoyut.Minimum = 100;
            numBoyut.Maximum = 1024;
            numBoyut.Value = 512;
            pnlSag.Controls.Add(numBoyut);

            // Daire Yarıçapı (r)
            Label lblYaricap = new Label
            {
                Text = "Daire Yarıçapı (r piksel):",
                Font = new Font("Segoe UI Semibold", 9.2f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                Location = new Point(18, 130),
                AutoSize = true
            };
            pnlSag.Controls.Add(lblYaricap);

            numYaricap.Parent?.Controls.Remove(numYaricap);
            numYaricap.Location = new Point(18, 152);
            numYaricap.Size = new Size(252, 32);
            numYaricap.Font = new Font("Segoe UI", 10f);
            numYaricap.Minimum = 10;
            numYaricap.Maximum = 500;
            numYaricap.Value = 100;
            pnlSag.Controls.Add(numYaricap);

            // Görüntüyü Oluştur Butonu
            ModernHapButon btnModernOlustur = FormGezintiYardimcisi.ModernButonOlustur("⚪ Görüntüyü Oluştur", true, 252, 40);
            btnModernOlustur.Location = new Point(18, 200);
            btnModernOlustur.Click += btnOlustur_Click;
            pnlSag.Controls.Add(btnModernOlustur);

            // Sıfırla Butonu
            ModernHapButon btnSifirla = FormGezintiYardimcisi.ModernButonOlustur("🔄 Varsayılan (512 / 100)", false, 252, 36);
            btnSifirla.Location = new Point(18, 248);
            btnSifirla.Click += (s, e) =>
            {
                numBoyut.Value = 512;
                numYaricap.Value = 100;
                btnOlustur_Click(this, EventArgs.Empty);
            };
            pnlSag.Controls.Add(btnSifirla);

            // Açık Mavi Matematiksel Açıklama Kartı
            ModernKartPanel bilgiKarti = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                konum: new Point(18, 300),
                boyut: new Size(252, 235),
                icerikEtiketi: out Label lblBilgi
            );
            lblBilgi.Text =
                "Öklid Uzaklığı Formülü:\n\n" +
                "Merkez: (cx, cy) = (N/2, N/2)\n" +
                "d = √((i - cy)² + (j - cx)²)\n\n" +
                "• d < r ise piksel = 255 (Beyaz)\n" +
                "• d ≥ r ise piksel = 0 (Siyah)\n\n" +
                "8-bit tek kanallı (CV_8UC1) ikili (binary) matris üzerinde pikseller doğrudan hesaplanır.";
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
                Text = "Oluşturulan Siyah-Beyaz Görüntü (CV_8UC1 Matris)",
                UseMnemonic = false,
                AutoSize = true,
                Location = new Point(0, 5),
                Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                BackColor = Color.White
            };
            pnlKartUst.Controls.Add(sonucBaslik);

            rozetBoyut = new ModernHapRozet();
            rozetBoyut.MetinGuncelle("512 × 512");

            rozetYaricap = new ModernHapRozet();
            rozetYaricap.MetinGuncelle("r = 100 px");

            void RozetleriHizala()
            {
                rozetYaricap.Location = new Point(pnlKartUst.Width - rozetYaricap.Width - 2, 3);
                rozetBoyut.Location = new Point(rozetYaricap.Left - rozetBoyut.Width - 8, 3);
            }

            pnlKartUst.Resize += (s, e) => RozetleriHizala();
            pnlKartUst.Controls.Add(rozetBoyut);
            pnlKartUst.Controls.Add(rozetYaricap);
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
                "Matris boyutu (N) ve yarıçap (r) değerlerini ayarlayıp 'Görüntüyü Oluştur' butonuna tıklayınız.",
                out lblAnaliz,
                "Hazır"
            );

            anaYerlesim.Controls.Add(sonucKart, 0, 0);
            anaYerlesim.Controls.Add(altSerit, 0, 1);
            pnlOrta.Controls.Add(anaYerlesim);

            this.Controls.Add(pnlOrta);
            FormGezintiYardimcisi.YerlesimiDuzenle(this, pnlUst, pnlSag, pnlOrta);
        }

        // Orijinal görüntü işleme algoritması
        private void btnOlustur_Click(object sender, EventArgs e)
        {
            try
            {
                if (numBoyut == null || picGoruntu == null)
                    return;

                int N = (int)numBoyut.Value;
                if (N < 1) N = 1;

                double R = numYaricap != null
                    ? (double)numYaricap.Value
                    : 100.0;

                double merkezX = N / 2.0;
                double merkezY = N / 2.0;

                using (Mat matris = new Mat(N, N, MatType.CV_8UC1))
                {
                    for (int i = 0; i < N; i++)
                    {
                        for (int j = 0; j < N; j++)
                        {
                            double mesafe = Math.Sqrt(
                                Math.Pow(i - merkezY, 2) +
                                Math.Pow(j - merkezX, 2));

                            if (mesafe < R)
                                matris.Set<byte>(i, j, 255);
                            else
                                matris.Set<byte>(i, j, 0);
                        }
                    }

                    picGoruntu.Image?.Dispose();
                    picGoruntu.Image = BitmapConverter.ToBitmap(matris);
                    picGoruntu.SizeMode = PictureBoxSizeMode.Zoom;
                }

                rozetBoyut?.MetinGuncelle($"{N} × {N}");
                rozetYaricap?.MetinGuncelle($"r = {R} px");
                if (lblAnaliz != null)
                {
                    lblAnaliz.Text = $"Matris oluşturuldu: {N}×{N} px • Merkez: ({merkezX:0.#}, {merkezY:0.#}) • Daire Yarıçapı: r = {R} px";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Çizim sırasında hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}