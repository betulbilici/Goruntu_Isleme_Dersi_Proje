using System;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace GoruntuIsleme
{
    public partial class Hafta1Odev5 : Form
    {
        private ModernHapRozet rozetOrijinal;
        private Label lblBilgi;
        private Label lblAnaliz;

        public Hafta1Odev5()
        {
            InitializeComponent();
            ModernArayuzuUygula();
        }

        private void ModernArayuzuUygula()
        {
            // 1. Form Temel Ayarları (1250x750, Standart Kenarlık ve X ile Ana Menüye Dönüş)
            FormGezintiYardimcisi.FormuHazirla(this);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Text = "Hafta 1 - Ödev 5: RGB Renk Kanalları (Yapay Renklendirme)";

            // Designer'daki eski etiket, buton ve linkleri gizle (PictureBox'ları koruyoruz)
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            btnResimSec.Visible = false;
            lnkGeri.Visible = false;
            lnkOdev6.Visible = false;

            // 2. Üst Gezinti Barı
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                form: this,
                baslik: "Hafta 1 - Ödev 5 | RGB Renk Kanalları (R00 - 0G0 - 00B)",
                oncekiYazi: "< Ödev 4",
                oncekiTikla: () =>
                {
                    Form odev4 = Application.OpenForms["Hafta1Odev4"];
                    if (odev4 != null)
                    {
                        FormGezintiYardimcisi.FormGecisYap(this, odev4);
                    }
                    else
                    {
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta1Odev4());
                    }
                },
                sonrakiYazi: "Ödev 6 >",
                sonrakiTikla: () => FormGezintiYardimcisi.FormGecisYap(this, new Hafta1Odev6())
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

            // Designer'daki pictureBox4 (Slayt/Formül Referans Görseli) varsa sağ panele kart içinde yerleştir
            int bilgiKartiY = 158;
            if (pictureBox4 != null && pictureBox4.Image != null)
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

                pictureBox4.Parent?.Controls.Remove(pictureBox4);
                pictureBox4.Dock = DockStyle.Fill;
                pictureBox4.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox4.BorderStyle = BorderStyle.None;
                pictureBox4.BackColor = Color.White;

                ornekKart.Controls.Add(pictureBox4);
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
                "RGB Kanal Ayrıştırma:\n\n" +
                "• Doğal (RGB): Orijinal görüntü\n" +
                "• Kırmızı (R00): (R, 0, 0)\n" +
                "• Yeşil (0G0): (0, G, 0)\n" +
                "• Mavi (00B): (0, 0, B)\n\n" +
                "Diğer iki renk kanalına sıfır matrisi (Mat.Zeros) atanarak yalnızca ilgili bandın renk katkısı izole edilir.";
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

            MevcutPictureBoxKartaTasi(kartIzgara, 0, 0, "Doğal Renkli Görüntü (RGB)", picOrijinal, rozetOrijinal);
            MevcutPictureBoxKartaTasi(kartIzgara, 1, 0, "Kırmızı Yapay Renklendirme (R00)", picKirmizi);
            MevcutPictureBoxKartaTasi(kartIzgara, 0, 1, "Yeşil Yapay Renklendirme (0G0)", picYesil);
            MevcutPictureBoxKartaTasi(kartIzgara, 1, 1, "Mavi Yapay Renklendirme (00B)", picMavi);

            // Alt Analiz Şeridi
            ModernKartPanel altSerit = FormGezintiYardimcisi.AltAnalizSeridiOlustur(
                "RGB renk kanallarını ayrı ayrı incelemek için sağ panelden bir resim seçiniz.",
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
            string dosyaYolu = FormGezintiYardimcisi.ResimDosyasiSec("RGB Kanal Ayrıştırma İçin Resim Seçiniz");
            if (string.IsNullOrEmpty(dosyaYolu)) return;

            using (Mat orijinal = Cv2.ImRead(dosyaYolu, ImreadModes.Color))
            {
                if (orijinal.Empty())
                {
                    MessageBox.Show("Resim yüklenemedi!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 1. Orijinal renkli görüntü
                picOrijinal.Image?.Dispose();
                picOrijinal.Image = BitmapConverter.ToBitmap(orijinal);

                // BGR kanallarını ayır
                Mat[] kanallar = Cv2.Split(orijinal);

                try
                {
                    using (Mat sifirKanal = Mat.Zeros(orijinal.Size(), MatType.CV_8UC1))
                    {
                        // 2. Kırmızı Kanal (R00) -> BGR sırası: (0, 0, R)
                        using (Mat kirmiziMat = new Mat())
                        {
                            Cv2.Merge(new Mat[] { sifirKanal, sifirKanal, kanallar[2] }, kirmiziMat);
                            picKirmizi.Image?.Dispose();
                            picKirmizi.Image = BitmapConverter.ToBitmap(kirmiziMat);
                        }

                        // 3. Yeşil Kanal (0G0) -> BGR sırası: (0, G, 0)
                        using (Mat yesilMat = new Mat())
                        {
                            Cv2.Merge(new Mat[] { sifirKanal, kanallar[1], sifirKanal }, yesilMat);
                            picYesil.Image?.Dispose();
                            picYesil.Image = BitmapConverter.ToBitmap(yesilMat);
                        }

                        // 4. Mavi Kanal (00B) -> BGR sırası: (B, 0, 0)
                        using (Mat maviMat = new Mat())
                        {
                            Cv2.Merge(new Mat[] { kanallar[0], sifirKanal, sifirKanal }, maviMat);
                            picMavi.Image?.Dispose();
                            picMavi.Image = BitmapConverter.ToBitmap(maviMat);
                        }
                    }
                }
                finally
                {
                    foreach (Mat kanal in kanallar)
                        kanal.Dispose();
                }

                rozetOrijinal.MetinGuncelle($"{orijinal.Width} × {orijinal.Height}");
                lblAnaliz.Text = $"Görüntü işlendi ({orijinal.Width}×{orijinal.Height} px) • Kırmızı (R00), Yeşil (0G0) ve Mavi (00B) kanalları ayrıştırıldı.";
            }
        }

        private void BtnTemizle_Click(object sender, EventArgs e)
        {
            picOrijinal.Image?.Dispose(); picOrijinal.Image = null;
            picKirmizi.Image?.Dispose(); picKirmizi.Image = null;
            picYesil.Image?.Dispose(); picYesil.Image = null;
            picMavi.Image?.Dispose(); picMavi.Image = null;

            rozetOrijinal.MetinGuncelle("Bekleniyor");
            lblAnaliz.Text = "Görseller temizlendi. Yeni bir resim seçebilirsiniz.";
        }
    }
}