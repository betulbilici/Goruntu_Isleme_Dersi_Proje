using System;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace GoruntuIsleme
{
    public partial class Hafta1Odev8 : Form
    {
        private Mat yuklenenMat = null;
        private ModernHapRozet rozetDurum;
        private Label lblBilgi;
        private Label lblAnaliz;

        public Hafta1Odev8()
        {
            InitializeComponent();
            ModernArayuzuUygula();
        }

        private void ModernArayuzuUygula()
        {
            // 1. Form Temel Ayarları (1250x750, Standart Kenarlık ve X ile Ana Menüye Dönüş)
            FormGezintiYardimcisi.FormuHazirla(this);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Text = "Hafta 1 - Ödev 8: Bit Derinliği ve Gri Seviye Çözünürlüğü (m = 8..1 bit)";

            // Designer'daki eski etiket, buton ve linkleri gizle
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            btnResimSec.Visible = false;
            btnUygula.Visible = false;
            lnkGeri.Visible = false;
            lnkAnaMenu.Visible = false;

            // 2. Üst Gezinti Barı
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                form: this,
                baslik: "Hafta 1 - Ödev 8 | Bit Derinliği Azaltma (m = 8..1 bit, G = 256..2)",
                oncekiYazi: "< Ödev 7",
                oncekiTikla: () =>
                {
                    Form odev7 = Application.OpenForms["Hafta1Odev7"];
                    if (odev7 != null)
                    {
                        FormGezintiYardimcisi.FormGecisYap(this, odev7);
                    }
                    else
                    {
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta1Odev7());
                    }
                },
                sonrakiYazi: "Hafta 2 >",
                sonrakiTikla: () => FormGezintiYardimcisi.AnaFormaDon(this)
            );

            // 3. Sağ Kontrol Paneli
            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 290);

            Label lblKontrolBaslik = new Label
            {
                Text = "Kuantizasyon Ayarları",
                Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                Location = new System.Drawing.Point(18, 18),
                AutoSize = true
            };
            pnlSag.Controls.Add(lblKontrolBaslik);

            // Resim Seç Butonu
            ModernHapButon btnModernResimSec = FormGezintiYardimcisi.ModernButonOlustur("📁 Resim Seç", true, 252, 40);
            btnModernResimSec.Location = new System.Drawing.Point(18, 52);
            btnModernResimSec.Click += BtnResimSec_Click;
            pnlSag.Controls.Add(btnModernResimSec);

            // Renk Modu Etiketi ve Designer'daki cmbMod
            Label lblMod = new Label
            {
                Text = "İşlem Modu:",
                Font = new Font("Segoe UI Semibold", 9.2f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                Location = new System.Drawing.Point(18, 106),
                AutoSize = true
            };
            pnlSag.Controls.Add(lblMod);

            cmbMod.Parent?.Controls.Remove(cmbMod);
            cmbMod.Location = new System.Drawing.Point(18, 130);
            cmbMod.Size = new System.Drawing.Size(252, 32);
            cmbMod.Font = new Font("Segoe UI", 9.8f);
            if (cmbMod.Items.Count > 0 && cmbMod.SelectedIndex < 0)
            {
                cmbMod.SelectedIndex = 0; // Varsayılan: Gri-Ton (Grayscale)
            }
            cmbMod.SelectedIndexChanged += (s, e) => { if (yuklenenMat != null) TumBitleriOlustur(); };
            pnlSag.Controls.Add(cmbMod);

            // Tüm Bitleri Oluştur Butonu
            ModernHapButon btnModernUygula = FormGezintiYardimcisi.ModernButonOlustur("⚡ Tüm Bitleri Oluştur (8)", false, 252, 40);
            btnModernUygula.Location = new System.Drawing.Point(18, 174);
            btnModernUygula.Click += BtnUygula_Click;
            pnlSag.Controls.Add(btnModernUygula);

            // Temizle Butonu
            ModernHapButon btnTemizle = FormGezintiYardimcisi.ModernButonOlustur("🔄 Temizle", false, 252, 36);
            btnTemizle.Location = new System.Drawing.Point(18, 222);
            btnTemizle.Click += BtnTemizle_Click;
            pnlSag.Controls.Add(btnTemizle);

            // Açık Mavi Teorik Açıklama Kartı (Designer'daki label9 metnini de destekler)
            ModernKartPanel bilgiKarti = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                konum: new System.Drawing.Point(18, 272),
                boyut: new System.Drawing.Size(252, 310),
                icerikEtiketi: out lblBilgi
            );
            lblBilgi.Text = !string.IsNullOrWhiteSpace(label9.Text)
                ? label9.Text
                : "Bit Derinliği Formülü: G = 2^m\n\n" +
                  "• m = 8 bit → 256 gri seviye\n" +
                  "• m = 7 bit → 128 gri seviye\n" +
                  "• m = 6 bit → 64 gri seviye\n" +
                  "• m = 5 bit → 32 gri seviye\n" +
                  "• m = 4 bit → 16 gri seviye\n" +
                  "• m = 3 bit → 8 gri seviye\n" +
                  "• m = 2 bit → 4 gri seviye\n" +
                  "• m = 1 bit → 2 seviye (Siyah-Beyaz)\n\n" +
                  "Bit sayısı azaldıkça ton geçişlerinde yapay konturlanma (false contouring) oluşur.";
            pnlSag.Controls.Add(bilgiKarti);

            // 4. Orta Çalışma Alanı (4x2 = 8 Kartlı Izgara + Alt Analiz Şeridi)
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
            anaYerlesim.RowStyles.Add(new RowStyle(SizeType.Absolute, 52f));

            TableLayoutPanel kartIzgara = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 2
            };
            for (int c = 0; c < 4; c++)
                kartIzgara.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            for (int r = 0; r < 2; r++)
                kartIzgara.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));

            rozetDurum = new ModernHapRozet();
            rozetDurum.MetinGuncelle("256 Ton");

            // Üst Satır: m = 8, 7, 6, 5 bit
            MevcutPictureBoxKartaTasi(kartIzgara, 0, 0, "m = 8 bit (G = 256)", picBit8, rozetDurum);
            MevcutPictureBoxKartaTasi(kartIzgara, 1, 0, "m = 7 bit (G = 128)", picBit7);
            MevcutPictureBoxKartaTasi(kartIzgara, 2, 0, "m = 6 bit (G = 64)", picBit6);
            MevcutPictureBoxKartaTasi(kartIzgara, 3, 0, "m = 5 bit (G = 32)", picBit5);

            // Alt Satır: m = 4, 3, 2, 1 bit
            MevcutPictureBoxKartaTasi(kartIzgara, 0, 1, "m = 4 bit (G = 16)", picBit4);
            MevcutPictureBoxKartaTasi(kartIzgara, 1, 1, "m = 3 bit (G = 8)", picBit3);
            MevcutPictureBoxKartaTasi(kartIzgara, 2, 1, "m = 2 bit (G = 4)", picBit2);
            MevcutPictureBoxKartaTasi(kartIzgara, 3, 1, "m = 1 bit (G = 2)", picBit1);

            // Alt Analiz Şeridi
            ModernKartPanel altSerit = FormGezintiYardimcisi.AltAnalizSeridiOlustur(
                "8 farklı bit derinliğini (m = 8..1) karşılaştırmak için sağ panelden bir resim seçiniz.",
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
                Margin = new Padding(5),
                Padding = new Padding(10)
            };

            Panel pnlKartUst = new Panel
            {
                Dock = DockStyle.Top,
                Height = 26,
                BackColor = Color.White
            };

            Label lbl = new Label
            {
                Text = baslik,
                UseMnemonic = false,
                AutoSize = true,
                Location = new System.Drawing.Point(0, 3),
                Font = new Font("Segoe UI Semibold", 9.2f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                BackColor = Color.White
            };
            pnlKartUst.Controls.Add(lbl);

            if (sagUstRozet != null)
            {
                sagUstRozet.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                sagUstRozet.Location = new System.Drawing.Point(pnlKartUst.Width - sagUstRozet.Width - 2, 1);
                pnlKartUst.Resize += (s, e) =>
                {
                    sagUstRozet.Location = new System.Drawing.Point(pnlKartUst.Width - sagUstRozet.Width - 2, 1);
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

        private void BtnResimSec_Click(object sender, EventArgs e)
        {
            string dosyaYolu = FormGezintiYardimcisi.ResimDosyasiSec("Bit Derinliği Analizi İçin Resim Seçiniz");
            if (string.IsNullOrEmpty(dosyaYolu)) return;

            yuklenenMat?.Dispose();
            yuklenenMat = Cv2.ImRead(dosyaYolu, ImreadModes.Color);

            if (yuklenenMat.Empty())
            {
                MessageBox.Show("Resim yüklenemedi!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                yuklenenMat = null;
                return;
            }

            TumBitleriOlustur();
        }

        private void BtnUygula_Click(object sender, EventArgs e)
        {
            if (yuklenenMat == null || yuklenenMat.Empty())
            {
                MessageBox.Show("Lütfen önce bir resim seçiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TumBitleriOlustur();
        }

        private void TumBitleriOlustur()
        {
            if (yuklenenMat == null || yuklenenMat.Empty()) return;

            bool griModMu = cmbMod.SelectedIndex == 0;
            PictureBox[] kutular = { picBit8, picBit7, picBit6, picBit5, picBit4, picBit3, picBit2, picBit1 };

            using (Mat temelMat = new Mat())
            {
                if (griModMu)
                    Cv2.CvtColor(yuklenenMat, temelMat, ColorConversionCodes.BGR2GRAY);
                else
                    yuklenenMat.CopyTo(temelMat);

                for (int i = 0; i < 8; i++)
                {
                    int bitSayisi = 8 - i; // 8, 7, 6, 5, 4, 3, 2, 1
                    using (Mat kuantizeMat = BitDerinligiUygula(temelMat, bitSayisi))
                    {
                        kutular[i].Image?.Dispose();
                        kutular[i].Image = BitmapConverter.ToBitmap(kuantizeMat);
                    }
                }
            }

            string modMetni = griModMu ? "Gri-Ton (Grayscale)" : "Renkli (RGB)";
            rozetDurum.MetinGuncelle($"{yuklenenMat.Width}×{yuklenenMat.Height}");
            lblAnaliz.Text = $"Mod: {modMetni} • m = 8 bit (G=256) seviyesinden m = 1 bit (G=2) seviyesine tüm kuantizasyon adımları oluşturuldu.";
        }

        private Mat BitDerinligiUygula(Mat kaynak, int bitSayisi)
        {
            if (bitSayisi >= 8)
                return kaynak.Clone();

            int kaydirma = 8 - bitSayisi;
            int maxDeger = (1 << bitSayisi) - 1;
            double olcek = 255.0 / maxDeger;

            // 0..255 arası lookup table (LUT) ile yüksek hızlı kuantizasyon
            byte[] lutVeri = new byte[256];
            for (int v = 0; v < 256; v++)
            {
                int seviye = v >> kaydirma;
                lutVeri[v] = (byte)Math.Round(seviye * olcek);
            }

            Mat sonuc = new Mat();
            using (Mat lutMat = Mat.FromPixelData(1, 256, MatType.CV_8UC1, lutVeri))
            {
                Cv2.LUT(kaynak, lutMat, sonuc);
            }
            return sonuc;
        }

        private void BtnTemizle_Click(object sender, EventArgs e)
        {
            yuklenenMat?.Dispose();
            yuklenenMat = null;

            PictureBox[] kutular = { picBit8, picBit7, picBit6, picBit5, picBit4, picBit3, picBit2, picBit1 };
            foreach (PictureBox pb in kutular)
            {
                pb.Image?.Dispose();
                pb.Image = null;
            }

            rozetDurum.MetinGuncelle("256 Ton");
            lblAnaliz.Text = "Görseller temizlendi. Yeni bir resim seçebilirsiniz.";
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            yuklenenMat?.Dispose();
            base.OnFormClosed(e);
        }
    }
}