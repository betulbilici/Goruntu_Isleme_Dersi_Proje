using System;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace GoruntuIsleme
{
    public partial class Hafta1Odev6 : Form
    {
        private ModernHapRozet rozetOrijinal;
        private Label lblBilgi;
        private Label lblAnaliz;

        public Hafta1Odev6()
        {
            InitializeComponent();
            ModernArayuzuUygula();
        }

        private void ModernArayuzuUygula()
        {
            // 1. Form Temel Ayarları (1250x750, Standart Kenarlık ve X ile Ana Menüye Dönüş)
            FormGezintiYardimcisi.FormuHazirla(this);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Text = "Hafta 1 - Ödev 6: RGB Bantlarının Gri Tonları";

            // Designer'daki eski etiket, buton ve linkleri gizle (PictureBox'ları koruyoruz)
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            btnResimSec.Visible = false;
            lnkGeri.Visible = false;
            lnkOdev7.Visible = false;

            // 2. Üst Gezinti Barı
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                form: this,
                baslik: "Hafta 1 - Ödev 6 | RGB Bantlarının Gri Tonları",
                oncekiYazi: "< Ödev 5",
                oncekiTikla: () =>
                {
                    Form odev5 = Application.OpenForms["Hafta1Odev5"];
                    if (odev5 != null)
                    {
                        FormGezintiYardimcisi.FormGecisYap(this, odev5);
                    }
                    else
                    {
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta1Odev5());
                    }
                },
                sonrakiYazi: "Ödev 7 >",
                sonrakiTikla: () => FormGezintiYardimcisi.FormGecisYap(this, new Hafta1Odev7())
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

            // Resim Seç Butonu
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
                "RGB Bant Ayrıştırma:\n\n" +
                "• Orijinal: Doğal RGB görüntü\n" +
                "• 1. Bant (R): (R, R, R) gri ton\n" +
                "• 2. Bant (G): (G, G, G) gri ton\n" +
                "• 3. Bant (B): (B, B, B) gri ton\n\n" +
                "Her renk kanalı tek başına ayrıştırılıp 3 kanala aynı değer atanarak gri ton yoğunluğu elde edilir.";
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

            MevcutPictureBoxKartaTasi(kartIzgara, 0, 0, "Orijinal RGB Görüntü (Doğal Renk)", picOrijinal, rozetOrijinal);
            MevcutPictureBoxKartaTasi(kartIzgara, 1, 0, "Kırmızı (R) Bandın (1. Bant) Gri-Ton Görüntüsü", picKirmiziGri);
            MevcutPictureBoxKartaTasi(kartIzgara, 0, 1, "Yeşil (G) Bandın (2. Bant) Gri-Ton Görüntüsü", picYesilGri);
            MevcutPictureBoxKartaTasi(kartIzgara, 1, 1, "Mavi (B) Bandın (3. Bant) Gri-Ton Görüntüsü", picMaviGri);

            // Alt Analiz Şeridi
            ModernKartPanel altSerit = FormGezintiYardimcisi.AltAnalizSeridiOlustur(
                "RGB bantlarının gri-ton karşılıklarını görmek için sağ panelden bir resim seçiniz.",
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
            string dosyaYolu = FormGezintiYardimcisi.ResimDosyasiSec("RGB Bant Ayrıştırma İçin Resim Seçiniz");
            if (string.IsNullOrEmpty(dosyaYolu)) return;

            using (Mat original = Cv2.ImRead(dosyaYolu, ImreadModes.Color))
            {
                if (original.Empty())
                {
                    MessageBox.Show("Resim yüklenemedi!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 1. Orijinal renkli görüntü
                picOrijinal.Image?.Dispose();
                picOrijinal.Image = BitmapConverter.ToBitmap(original);

                // BGR kanallarını ayır
                Mat[] kanallar = Cv2.Split(original);

                try
                {
                    // 2. Kırmızı bant gri-ton (R,R,R)
                    using (Mat kirmiziGriMat = new Mat())
                    {
                        Cv2.Merge(new Mat[] { kanallar[2], kanallar[2], kanallar[2] }, kirmiziGriMat);
                        picKirmiziGri.Image?.Dispose();
                        picKirmiziGri.Image = BitmapConverter.ToBitmap(kirmiziGriMat);
                    }

                    // 3. Yeşil bant gri-ton (G,G,G)
                    using (Mat yesilGriMat = new Mat())
                    {
                        Cv2.Merge(new Mat[] { kanallar[1], kanallar[1], kanallar[1] }, yesilGriMat);
                        picYesilGri.Image?.Dispose();
                        picYesilGri.Image = BitmapConverter.ToBitmap(yesilGriMat);
                    }

                    // 4. Mavi bant gri-ton (B,B,B)
                    using (Mat maviGriMat = new Mat())
                    {
                        Cv2.Merge(new Mat[] { kanallar[0], kanallar[0], kanallar[0] }, maviGriMat);
                        picMaviGri.Image?.Dispose();
                        picMaviGri.Image = BitmapConverter.ToBitmap(maviGriMat);
                    }
                }
                finally
                {
                    foreach (Mat kanal in kanallar)
                        kanal.Dispose();
                }

                rozetOrijinal.MetinGuncelle($"{original.Width} × {original.Height}");
                lblAnaliz.Text = $"Görüntü işlendi ({original.Width}×{original.Height} px) • Kırmızı (R), Yeşil (G) ve Mavi (B) bantlarının gri-ton karşılıkları oluşturuldu.";
            }
        }

        private void BtnTemizle_Click(object sender, EventArgs e)
        {
            picOrijinal.Image?.Dispose(); picOrijinal.Image = null;
            picKirmiziGri.Image?.Dispose(); picKirmiziGri.Image = null;
            picYesilGri.Image?.Dispose(); picYesilGri.Image = null;
            picMaviGri.Image?.Dispose(); picMaviGri.Image = null;

            rozetOrijinal.MetinGuncelle("Bekleniyor");
            lblAnaliz.Text = "Görseller temizlendi. Yeni bir resim seçebilirsiniz.";
        }
    }
}