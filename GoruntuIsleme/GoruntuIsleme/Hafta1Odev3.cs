using System;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace GoruntuIsleme
{
    public partial class Hafta1Odev3 : Form
    {
        private Mat? orijinalMat;
        private ModernHapRozet rozetOrijinal;
        private ModernHapRozet rozetSonuc;

        public Hafta1Odev3()
        {
            InitializeComponent();
            ModernArayuzuUygula();

            if (cmbOrnekleme.Items.Count > 0) cmbOrnekleme.SelectedIndex = 0;
            if (cmbNicemleme.Items.Count > 0) cmbNicemleme.SelectedIndex = 0;

            FormClosed += (s, e) =>
            {
                orijinalMat?.Dispose();
            };
        }

        private void ModernArayuzuUygula()
        {
            // 1. Form Temel Ayarları (1250x750, Standart Kenarlık ve X ile Ana Menüye Dönüş)
            FormGezintiYardimcisi.FormuHazirla(this);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Text = "Hafta 1 - Ödev 3: Örnekleme ve Renk Nicemleme";

            // Designer'daki tüm eski kontrolleri gizle (aktif kontrolleri aşağıda taşıyacağız)
            foreach (Control ctrl in this.Controls)
            {
                ctrl.Visible = false;
            }

            // 2. Üst Gezinti Barı
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                form: this,
                baslik: "Hafta 1 - Ödev 3 | Örnekleme (Sampling) ve Renk Nicemleme (Quantization)",
                oncekiYazi: "< Ödev 2",
                oncekiTikla: () =>
                {
                    Form odev2 = Application.OpenForms["Hafta1Odev2"];
                    if (odev2 != null)
                        FormGezintiYardimcisi.FormGecisYap(this, odev2);
                    else
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta1Odev2());
                },
                sonrakiYazi: "Ödev 4 >",
                sonrakiTikla: () => FormGezintiYardimcisi.FormGecisYap(this, new Hafta1Odev4())
            );

            // 3. Sağ Kontrol Paneli
            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 290);

            Label lblKontrolBaslik = new Label
            {
                Text = "Örnekleme & Nicemleme",
                Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                Location = new Point(18, 18),
                AutoSize = true
            };
            pnlSag.Controls.Add(lblKontrolBaslik);

            // Resim Yükle Butonu
            ModernHapButon btnModernResimSec = FormGezintiYardimcisi.ModernButonOlustur("📁 Resim Yükle", true, 252, 40);
            btnModernResimSec.Location = new Point(18, 52);
            btnModernResimSec.Click += btnResimSec_Click;
            pnlSag.Controls.Add(btnModernResimSec);

            // Örnekleme Adımı
            Label lblOrnekleme = new Label
            {
                Text = "Örnekleme Adımı (Uzamsal):",
                Font = new Font("Segoe UI Semibold", 9.2f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                Location = new Point(18, 108),
                AutoSize = true
            };
            pnlSag.Controls.Add(lblOrnekleme);

            cmbOrnekleme.Parent?.Controls.Remove(cmbOrnekleme);
            cmbOrnekleme.Visible = true;
            cmbOrnekleme.Location = new Point(18, 130);
            cmbOrnekleme.Size = new Size(252, 32);
            cmbOrnekleme.Font = new Font("Segoe UI", 9.8f);
            cmbOrnekleme.DropDownStyle = ComboBoxStyle.DropDownList;
            pnlSag.Controls.Add(cmbOrnekleme);

            // Renk Nicemleme
            Label lblNicemleme = new Label
            {
                Text = "Renk Nicemleme (Ton Adımı):",
                Font = new Font("Segoe UI Semibold", 9.2f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                Location = new Point(18, 176),
                AutoSize = true
            };
            pnlSag.Controls.Add(lblNicemleme);

            cmbNicemleme.Parent?.Controls.Remove(cmbNicemleme);
            cmbNicemleme.Visible = true;
            cmbNicemleme.Location = new Point(18, 198);
            cmbNicemleme.Size = new Size(252, 32);
            cmbNicemleme.Font = new Font("Segoe UI", 9.8f);
            cmbNicemleme.DropDownStyle = ComboBoxStyle.DropDownList;
            pnlSag.Controls.Add(cmbNicemleme);

            // Uygula / Dönüştür Butonu
            ModernHapButon btnModernUygula = FormGezintiYardimcisi.ModernButonOlustur("⚡ Uygula / Dönüştür", false, 252, 40);
            btnModernUygula.Location = new Point(18, 244);
            btnModernUygula.Click += btnUygula_Click;
            pnlSag.Controls.Add(btnModernUygula);

            // Açık Mavi Bilgi Kartı
            ModernKartPanel bilgiKarti = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                konum: new Point(18, 300),
                boyut: new Size(252, 230),
                icerikEtiketi: out Label lblBilgi
            );
            lblBilgi.Text =
                "İşlem Özeti:\n\n" +
                "• Boyutlandırma: Seçilen görüntü otomatik olarak 1000 × 1000 piksele ölçeklenir.\n" +
                "• Örnekleme: Belirlenen N×N piksel blokları tek örnek renkle doldurulur.\n" +
                "• Nicemleme: RGB kanal değerleri seçilen adım katlarına yuvarlanır.";
            pnlSag.Controls.Add(bilgiKarti);

            // 4. Orta Çalışma Alanı (Yan Yana 2 Tam Boy Modern Kart)
            Panel pnlOrta = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(14),
                BackColor = FormGezintiYardimcisi.TemaZemin
            };

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
            rozetOrijinal.MetinGuncelle("1000 × 1000");

            rozetSonuc = new ModernHapRozet();
            rozetSonuc.MetinGuncelle("Bekleniyor");

            MevcutPictureBoxKartaTasi(kartIzgara, 0, 0, "Orijinal Görüntü (1000 × 1000)", picOrijinal, rozetOrijinal);
            MevcutPictureBoxKartaTasi(kartIzgara, 1, 0, "İşlenmiş Görüntü", picSonuc, rozetSonuc);

            pnlOrta.Controls.Add(kartIzgara);
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
                Height = 32,
                BackColor = Color.White
            };

            Label lbl = new Label
            {
                Text = baslik,
                UseMnemonic = false,
                AutoSize = true,
                Location = new Point(0, 5),
                Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                BackColor = Color.White
            };
            pnlKartUst.Controls.Add(lbl);

            if (sagUstRozet != null)
            {
                sagUstRozet.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                sagUstRozet.Location = new Point(pnlKartUst.Width - sagUstRozet.Width - 2, 3);
                pnlKartUst.Resize += (s, e) =>
                {
                    sagUstRozet.Location = new Point(pnlKartUst.Width - sagUstRozet.Width - 2, 3);
                };
                pnlKartUst.Controls.Add(sagUstRozet);
                sagUstRozet.BringToFront();
            }

            mevcutPb.Parent?.Controls.Remove(mevcutPb);
            mevcutPb.Visible = true;
            mevcutPb.Dock = DockStyle.Fill;
            mevcutPb.SizeMode = PictureBoxSizeMode.Zoom;
            mevcutPb.BorderStyle = BorderStyle.None;
            mevcutPb.BackColor = FormGezintiYardimcisi.TemaZemin;

            kart.Controls.Add(mevcutPb);
            kart.Controls.Add(pnlKartUst);
            tablo.Controls.Add(kart, sutun, satir);
        }

        // RESİM YÜKLEME
        private void btnResimSec_Click(object sender, EventArgs e)
        {
            using OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.bmp"
            };

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                using Mat yuklenen = Cv2.ImRead(ofd.FileName, ImreadModes.Color);

                if (yuklenen.Empty())
                {
                    MessageBox.Show("Resim yüklenemedi!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Mat yeniMat = new Mat();

                try
                {
                    Cv2.Resize(yuklenen, yeniMat, new OpenCvSharp.Size(1000, 1000));

                    Bitmap yeniOrijinal = BitmapConverter.ToBitmap(yeniMat);
                    Bitmap yeniSonuc;

                    try
                    {
                        yeniSonuc = BitmapConverter.ToBitmap(yeniMat);
                    }
                    catch
                    {
                        yeniOrijinal.Dispose();
                        throw;
                    }

                    orijinalMat?.Dispose();
                    orijinalMat = yeniMat;
                    yeniMat = null!;

                    var eskiOrijinal = picOrijinal.Image;
                    var eskiSonuc = picSonuc.Image;

                    picOrijinal.Image = yeniOrijinal;
                    picSonuc.Image = yeniSonuc;

                    eskiOrijinal?.Dispose();
                    eskiSonuc?.Dispose();

                    rozetOrijinal.MetinGuncelle("1000 × 1000");
                    rozetSonuc.MetinGuncelle("Hazır");
                }
                finally
                {
                    yeniMat?.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Resim yüklenirken hata oluştu:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ÖRNEKLEME VE RENK NİCEMLEME
        private void btnUygula_Click(object sender, EventArgs e)
        {
            if (orijinalMat == null || orijinalMat.Empty())
            {
                MessageBox.Show("Lütfen önce bir resim yükleyin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int adim = 1;
            if (cmbOrnekleme.SelectedItem != null)
            {
                string secim = cmbOrnekleme.SelectedItem.ToString()!.Split(' ')[0];
                if (!int.TryParse(secim, out adim) || adim <= 0)
                    adim = 1;
            }

            int qAdim = 0;
            if (cmbNicemleme.SelectedItem != null)
            {
                string secim = cmbNicemleme.SelectedItem.ToString()!.Split(' ')[0];
                int.TryParse(secim, out qAdim);
            }

            try
            {
                using Mat sonucMat = orijinalMat.Clone();

                int yukseklik = orijinalMat.Rows;
                int genislik = orijinalMat.Cols;

                for (int y = 0; y < yukseklik; y += adim)
                {
                    for (int x = 0; x < genislik; x += adim)
                    {
                        Vec3b renk = orijinalMat.At<Vec3b>(y, x);

                        if (qAdim > 0)
                        {
                            renk.Item0 = Yuvarla(renk.Item0, qAdim);
                            renk.Item1 = Yuvarla(renk.Item1, qAdim);
                            renk.Item2 = Yuvarla(renk.Item2, qAdim);
                        }

                        int maxY = Math.Min(y + adim, yukseklik);
                        int maxX = Math.Min(x + adim, genislik);

                        for (int by = y; by < maxY; by++)
                        {
                            for (int bx = x; bx < maxX; bx++)
                            {
                                sonucMat.Set(by, bx, renk);
                            }
                        }
                    }
                }

                Bitmap yeniGoruntu = BitmapConverter.ToBitmap(sonucMat);
                var eskiGoruntu = picSonuc.Image;

                picSonuc.Image = yeniGoruntu;
                picSonuc.SizeMode = PictureBoxSizeMode.Zoom;

                eskiGoruntu?.Dispose();

                string qMetin = qAdim > 0 ? $" | Q:{qAdim}" : "";
                rozetSonuc.MetinGuncelle($"Adım: {adim}px{qMetin}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Görüntü işlenirken hata oluştu:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // RENK YUVARLAMA
        private byte Yuvarla(byte deger, int adim)
        {
            int yuvarlanmis = (int)(Math.Round((double)deger / adim) * adim);
            yuvarlanmis = Math.Clamp(yuvarlanmis, 0, 255);
            return (byte)yuvarlanmis;
        }
    }
}