using System;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace GoruntuIsleme
{
    public partial class Hafta1Odev7 : Form
    {
        private ModernHapRozet rozetOrijinal;
        private Label lblBilgi;
        private Label lblAnaliz;

        public Hafta1Odev7()
        {
            InitializeComponent();
            ModernArayuzuUygula();
        }

        private void ModernArayuzuUygula()
        {
            // 1. Form Temel Ayarları (1250x750, Standart Kenarlık ve X ile Ana Menüye Dönüş)
            FormGezintiYardimcisi.FormuHazirla(this);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Text = "Hafta 1 - Ödev 7: Bant Birleşimleri (RGB - GRB - RBG - GBR)";

            // Designer'daki eski etiket, buton ve linkleri gizle (PictureBox'ları koruyoruz)
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            btnResimSec.Visible = false;
            lnkGeri.Visible = false;
            lnkOdev8.Visible = false;
            linkLabel1.Visible = false;

            // 2. Üst Gezinti Barı
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                form: this,
                baslik: "Hafta 1 - Ödev 7 | Bant Birleşimleri (Doğal ve Yapay Renk Kanalları)",
                oncekiYazi: "< Ödev 6",
                oncekiTikla: () =>
                {
                    Form odev6 = Application.OpenForms["Hafta1Odev6"];
                    if (odev6 != null)
                    {
                        FormGezintiYardimcisi.FormGecisYap(this, odev6);
                    }
                    else
                    {
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta1Odev6());
                    }
                },
                sonrakiYazi: "Ödev 8 >",
                sonrakiTikla: () => FormGezintiYardimcisi.FormGecisYap(this, new Hafta1Odev8())
            );

            // 3. Sağ Kontrol Paneli
            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 290);

            Label lblKontrolBaslik = new Label
            {
                Text = "İşlem Kontrolleri",
                Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                Location = new System.Drawing.Point(18, 18),
                AutoSize = true
            };
            pnlSag.Controls.Add(lblKontrolBaslik);

            // Resim Seç Butonu (Mevcut btnResimSec_Click metoduna bağlı)
            ModernHapButon btnModernResimSec = FormGezintiYardimcisi.ModernButonOlustur("📁 Resim Seç", true, 252, 40);
            btnModernResimSec.Location = new System.Drawing.Point(18, 52);
            btnModernResimSec.Click += btnResimSec_Click;
            pnlSag.Controls.Add(btnModernResimSec);

            // Temizle Butonu
            ModernHapButon btnTemizle = FormGezintiYardimcisi.ModernButonOlustur("🔄 Temizle", false, 252, 38);
            btnTemizle.Location = new System.Drawing.Point(18, 102);
            btnTemizle.Click += BtnTemizle_Click;
            pnlSag.Controls.Add(btnTemizle);

            // Designer'daki pictureBox1 (Slayt/Formül Referans Görseli) varsa sağ panele kart içinde yerleştir
            int bilgiKartiY = 158;
            if (pictureBox1 != null && pictureBox1.Image != null)
            {
                ModernKartPanel ornekKart = new ModernKartPanel
                {
                    Location = new System.Drawing.Point(18, 154),
                    Size = new System.Drawing.Size(252, 130),
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

                bilgiKartiY = 296;
            }

            // Açık Mavi Açıklama Kartı
            ModernKartPanel bilgiKarti = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                konum: new System.Drawing.Point(18, bilgiKartiY),
                boyut: new System.Drawing.Size(252, 240),
                icerikEtiketi: out lblBilgi
            );
            lblBilgi.Text =
                "Bant Birleşim Sıraları:\n\n" +
                "• (1-2-3): Doğal Renk (RGB)\n" +
                "• (2-1-3): Yapay Renk (GRB)\n" +
                "• (1-3-2): Yapay Renk (RBG)\n" +
                "• (2-3-1): Yapay Renk (GBR)\n\n" +
                "Not: OpenCV matrisleri bellekte BGR sırasıyla tutulduğu için Cv2.Split ve Cv2.Merge ile bant sıraları yeniden birleştirilir.";
            pnlSag.Controls.Add(bilgiKarti);

            // 4. Orta Çalışma Alanı (Designer'daki 4 PictureBox'ı Modern Kartlara Taşıma)
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
                RowCount = 2
            };
            kartIzgara.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            kartIzgara.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            kartIzgara.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            kartIzgara.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));

            rozetOrijinal = new ModernHapRozet();
            rozetOrijinal.MetinGuncelle("Bekleniyor");

            MevcutPictureBoxKartaTasi(kartIzgara, 0, 0, "Doğal Renkli Görüntü • Bant Birleşimi (1-2-3)", picOrijinal, rozetOrijinal);
            MevcutPictureBoxKartaTasi(kartIzgara, 1, 0, "Yapay Renkli Görüntü • Bant Birleşimi (2-1-3)", picBant213);
            MevcutPictureBoxKartaTasi(kartIzgara, 0, 1, "Yapay Renkli Görüntü • Bant Birleşimi (1-3-2)", picBant132);
            MevcutPictureBoxKartaTasi(kartIzgara, 1, 1, "Yapay Renkli Görüntü • Bant Birleşimi (2-3-1)", picBant231);

            // Alt Analiz Şeridi
            ModernKartPanel altSerit = FormGezintiYardimcisi.AltAnalizSeridiOlustur(
                "Bant birleşimlerini görmek için sağ panelden bir resim seçiniz.",
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

            // Designer'daki mevcut PictureBox'ı formdan alıp kartın içine oturtuyoruz
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
            string dosyaYolu = FormGezintiYardimcisi.ResimDosyasiSec("Bant Birleşimi İçin Resim Seçiniz");
            if (string.IsNullOrEmpty(dosyaYolu)) return;

            using (Mat original = Cv2.ImRead(dosyaYolu, ImreadModes.Color))
            {
                if (original.Empty())
                {
                    MessageBox.Show("Resim yüklenemedi!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 1. Bant Birleşimi (1-2-3): Doğal Renk (RGB)
                picOrijinal.Image?.Dispose();
                picOrijinal.Image = BitmapConverter.ToBitmap(original);

                // OpenCV kanalları BGR sırasındadır:
                // kanallar[0] = Mavi (Bant 3), kanallar[1] = Yeşil (Bant 2), kanallar[2] = Kırmızı (Bant 1)
                Mat[] kanallar = Cv2.Split(original);

                try
                {
                    // 2. Bant Birleşimi (2-1-3): Yapay Renk (GRB) -> BGR sırası: (B, R, G)
                    using (Mat mat213 = new Mat())
                    {
                        Cv2.Merge(new Mat[] { kanallar[0], kanallar[2], kanallar[1] }, mat213);
                        picBant213.Image?.Dispose();
                        picBant213.Image = BitmapConverter.ToBitmap(mat213);
                    }

                    // 3. Bant Birleşimi (1-3-2): Yapay Renk (RBG) -> BGR sırası: (G, B, R)
                    using (Mat mat132 = new Mat())
                    {
                        Cv2.Merge(new Mat[] { kanallar[1], kanallar[0], kanallar[2] }, mat132);
                        picBant132.Image?.Dispose();
                        picBant132.Image = BitmapConverter.ToBitmap(mat132);
                    }

                    // 4. Bant Birleşimi (2-3-1): Yapay Renk (GBR) -> BGR sırası: (R, B, G)
                    using (Mat mat231 = new Mat())
                    {
                        Cv2.Merge(new Mat[] { kanallar[2], kanallar[0], kanallar[1] }, mat231);
                        picBant231.Image?.Dispose();
                        picBant231.Image = BitmapConverter.ToBitmap(mat231);
                    }
                }
                finally
                {
                    for (int i = 0; i < kanallar.Length; i++)
                    {
                        kanallar[i]?.Dispose();
                    }
                }

                rozetOrijinal.MetinGuncelle($"{original.Width} × {original.Height}");
                lblAnaliz.Text = $"Görüntü işlendi ({original.Width}×{original.Height} px) • Bant birleşimleri (1-2-3), (2-1-3), (1-3-2) ve (2-3-1) oluşturuldu.";
            }
        }

        private void BtnTemizle_Click(object sender, EventArgs e)
        {
            picOrijinal.Image?.Dispose(); picOrijinal.Image = null;
            picBant213.Image?.Dispose(); picBant213.Image = null;
            picBant132.Image?.Dispose(); picBant132.Image = null;
            picBant231.Image?.Dispose(); picBant231.Image = null;

            rozetOrijinal.MetinGuncelle("Bekleniyor");
            lblAnaliz.Text = "Görseller temizlendi. Yeni bir resim seçebilirsiniz.";
        }
    }
}