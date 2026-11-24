using System;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace GoruntuIsleme
{
    public partial class Hafta2Odev7 : Form
    {
        private Mat orijinalMat = null;
        private int[] histR = new int[256];
        private int[] histG = new int[256];
        private int[] histB = new int[256];
        private bool histogramlarHazir = false;

        private ModernHapRozet rozetResim;
        private ModernHapRozet rozetHistR;
        private ModernHapRozet rozetHistG;
        private ModernHapRozet rozetHistB;
        private Label lblAnaliz;

        public Hafta2Odev7()
        {
            InitializeComponent();
            ModernArayuzuUygula();
        }

        private void ModernArayuzuUygula()
        {
            // 1. Form Temel Ayarları (1250x750, Standart Kenarlık ve X ile Ana Menüye Dönüş)
            FormGezintiYardimcisi.FormuHazirla(this);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Text = "Hafta 2 - Ödev 7: RGB Renk Kanalları Histogram Analizi ve Değerlendirme";

            // Designer'daki tüm eski kontrolleri gizle
            foreach (Control ctrl in this.Controls)
            {
                ctrl.Visible = false;
            }

            // 2. Üst Gezinti Barı
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                form: this,
                baslik: "Hafta 2 - Ödev 7 | RGB Renk Kanalları Histogram Analizi ve Akademik Değerlendirme",
                oncekiYazi: "< Ödev 6",
                oncekiTikla: () =>
                {
                    Form odev6 = Application.OpenForms["Hafta2Odev6"];
                    if (odev6 != null)
                        FormGezintiYardimcisi.FormGecisYap(this, odev6);
                    else
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta2Odev6());
                },
                sonrakiYazi: "Ödev 8 >",
                sonrakiTikla: () => FormGezintiYardimcisi.FormGecisYap(this, new Hafta2Odev8())
            );

            // 3. Sağ Kontrol Paneli
            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 290);

            Label lblKontrolBaslik = new Label
            {
                Text = "Kanal Analiz İşlemleri",
                Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                Location = new Point(18, 18),
                AutoSize = true
            };
            pnlSag.Controls.Add(lblKontrolBaslik);

            // 1. Aşama: Resim Seç Butonu
            ModernHapButon btnModernResimSec = FormGezintiYardimcisi.ModernButonOlustur("📁 1. Resim Seç", true, 252, 40);
            btnModernResimSec.Location = new Point(18, 52);
            btnModernResimSec.Click += btnResimSec_Click;
            pnlSag.Controls.Add(btnModernResimSec);

            // 2. Aşama: Kanalları Analiz Et Butonu
            ModernHapButon btnModernAnaliz = FormGezintiYardimcisi.ModernButonOlustur("📊 2. Kanalları Analiz Et", false, 252, 40);
            btnModernAnaliz.Location = new Point(18, 102);
            btnModernAnaliz.Click += btnAnalizEt_Click;
            pnlSag.Controls.Add(btnModernAnaliz);

            // 3. Aşama: Yorum ve Kullanım Alanları Butonu
            ModernHapButon btnModernYorum = FormGezintiYardimcisi.ModernButonOlustur("📝 3. Yorum ve Kullanım Alanları", false, 252, 40);
            btnModernYorum.Location = new Point(18, 152);
            btnModernYorum.Click += btnYorumYap_Click;
            pnlSag.Controls.Add(btnModernYorum);

            // Açık Mavi Bilgi Kartı
            ModernKartPanel bilgiKarti = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                konum: new Point(18, 210),
                boyut: new Size(252, 340),
                icerikEtiketi: out Label lblBilgi
            );
            lblBilgi.Text =
                "Analiz Adımları:\n\n" +
                "1. Resim Seç: İncelenecek renkli görüntüyü yükler.\n\n" +
                "2. Kanalları Analiz Et: Kırmızı (R), Yeşil (G) ve Mavi (B) kanallarının 0–255 arası piksel dağılımlarını hesaplayıp çizer.\n\n" +
                "3. Yorum ve Kullanım Alanları: Kanal ortalamalarını, baskın rengi ve görüntü işlemedeki uygulama alanlarını raporlar.";
            pnlSag.Controls.Add(bilgiKarti);

            // 4. Orta Çalışma Alanı (Üst: Sol Resim + Sağ 3 RGB Histogramı | Alt: Akademik Rapor | En Alt: Analiz Şeridi)
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
                RowCount = 3
            };
            anaYerlesim.RowStyles.Add(new RowStyle(SizeType.Percent, 64f));
            anaYerlesim.RowStyles.Add(new RowStyle(SizeType.Percent, 36f));
            anaYerlesim.RowStyles.Add(new RowStyle(SizeType.Absolute, 50f));

            // Üst Satır: Sol Orijinal Resim Kartı + Sağ 3 Katlı RGB Histogramları
            TableLayoutPanel ustIzgara = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1
            };
            ustIzgara.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 46f));
            ustIzgara.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 54f));
            ustIzgara.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            // Sol Kart: Orijinal Resim
            ModernKartPanel resimKart = new ModernKartPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(6),
                Padding = new Padding(12)
            };

            Panel pnlResimUst = new Panel
            {
                Dock = DockStyle.Top,
                Height = 28,
                BackColor = Color.White
            };

            Label lblResimBaslik = new Label
            {
                Text = "Orijinal Renkli Görüntü",
                AutoSize = true,
                Location = new Point(0, 4),
                Font = new Font("Segoe UI Semibold", 9.8f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi
            };
            pnlResimUst.Controls.Add(lblResimBaslik);

            rozetResim = new ModernHapRozet();
            rozetResim.MetinGuncelle("Bekleniyor");
            rozetResim.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            rozetResim.Location = new Point(pnlResimUst.Width - rozetResim.Width - 2, 1);
            pnlResimUst.Resize += (s, e) =>
            {
                rozetResim.Location = new Point(pnlResimUst.Width - rozetResim.Width - 2, 1);
            };
            pnlResimUst.Controls.Add(rozetResim);

            picResim.Parent?.Controls.Remove(picResim);
            picResim.Visible = true;
            picResim.Dock = DockStyle.Fill;
            picResim.SizeMode = PictureBoxSizeMode.Zoom;
            picResim.BorderStyle = BorderStyle.None;
            picResim.BackColor = FormGezintiYardimcisi.TemaZemin;

            resimKart.Controls.Add(picResim);
            resimKart.Controls.Add(pnlResimUst);
            ustIzgara.Controls.Add(resimKart, 0, 0);

            // Sağ Sütun: 3 Adet RGB Histogram Kartı Alt Alta
            TableLayoutPanel histSutun = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3
            };
            histSutun.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            histSutun.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33f));
            histSutun.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33f));
            histSutun.RowStyles.Add(new RowStyle(SizeType.Percent, 33.34f));

            rozetHistR = new ModernHapRozet(); rozetHistR.MetinGuncelle("R Kanalı");
            rozetHistG = new ModernHapRozet(); rozetHistG.MetinGuncelle("G Kanalı");
            rozetHistB = new ModernHapRozet(); rozetHistB.MetinGuncelle("B Kanalı");

            MevcutHistogramKartaTasi(histSutun, 0, "Kırmızı (Red) Kanal Histogramı", picHistR, rozetHistR);
            MevcutHistogramKartaTasi(histSutun, 1, "Yeşil (Green) Kanal Histogramı", picHistG, rozetHistG);
            MevcutHistogramKartaTasi(histSutun, 2, "Mavi (Blue) Kanal Histogramı", picHistB, rozetHistB);

            picHistR.Resize += (s, e) => { if (histogramlarHazir) TekHistogramCiz(picHistR, histR, Color.FromArgb(248, 113, 113)); };
            picHistG.Resize += (s, e) => { if (histogramlarHazir) TekHistogramCiz(picHistG, histG, Color.FromArgb(74, 222, 128)); };
            picHistB.Resize += (s, e) => { if (histogramlarHazir) TekHistogramCiz(picHistB, histB, Color.FromArgb(56, 189, 248)); };

            ustIzgara.Controls.Add(histSutun, 1, 0);

            // Alt Satır: Akademik Rapor Kartı
            ModernKartPanel raporKart = new ModernKartPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(6),
                Padding = new Padding(12)
            };

            Label lblRaporBaslik = new Label
            {
                Text = "Renk Kanalları İstatistiksel Değerlendirme ve Kullanım Alanları",
                Dock = DockStyle.Top,
                Height = 24,
                Font = new Font("Segoe UI Semibold", 9.6f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                BackColor = Color.White
            };

            txtYorum.Parent?.Controls.Remove(txtYorum);
            txtYorum.Visible = true;
            txtYorum.Dock = DockStyle.Fill;
            txtYorum.BorderStyle = BorderStyle.None;
            txtYorum.BackColor = Color.White;
            txtYorum.Font = new Font("Segoe UI", 9.2f);
            txtYorum.ReadOnly = true;
            txtYorum.Multiline = true;
            txtYorum.ScrollBars = ScrollBars.Vertical;
            txtYorum.Text = "Başlamak için sağ panelden '1. Resim Seç' butonuna tıklayınız.";

            raporKart.Controls.Add(txtYorum);
            raporKart.Controls.Add(lblRaporBaslik);

            // En Alt Analiz Şeridi
            ModernKartPanel altSerit = FormGezintiYardimcisi.AltAnalizSeridiOlustur(
                "RGB kanal histogramlarını çıkarmak ve istatistiksel raporu oluşturmak için sağ paneldeki adımları takip ediniz.",
                out lblAnaliz,
                "Hazır"
            );

            anaYerlesim.Controls.Add(ustIzgara, 0, 0);
            anaYerlesim.Controls.Add(raporKart, 0, 1);
            anaYerlesim.Controls.Add(altSerit, 0, 2);
            pnlOrta.Controls.Add(anaYerlesim);

            this.Controls.Add(pnlOrta);
            FormGezintiYardimcisi.YerlesimiDuzenle(this, pnlUst, pnlSag, pnlOrta);
        }

        private void MevcutHistogramKartaTasi(
            TableLayoutPanel tablo,
            int satir,
            string baslik,
            PictureBox mevcutPb,
            ModernHapRozet sagUstRozet)
        {
            ModernKartPanel kart = new ModernKartPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(6, 4, 6, 4),
                Padding = new Padding(10, 8, 10, 8)
            };

            Panel pnlKartUst = new Panel
            {
                Dock = DockStyle.Top,
                Height = 24,
                BackColor = Color.White
            };

            Label lbl = new Label
            {
                Text = baslik,
                UseMnemonic = false,
                AutoSize = true,
                Location = new Point(0, 2),
                Font = new Font("Segoe UI Semibold", 9.2f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                BackColor = Color.White
            };
            pnlKartUst.Controls.Add(lbl);

            sagUstRozet.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            sagUstRozet.Location = new Point(pnlKartUst.Width - sagUstRozet.Width - 2, 0);
            pnlKartUst.Resize += (s, e) =>
            {
                sagUstRozet.Location = new Point(pnlKartUst.Width - sagUstRozet.Width - 2, 0);
            };
            pnlKartUst.Controls.Add(sagUstRozet);

            mevcutPb.Parent?.Controls.Remove(mevcutPb);
            mevcutPb.Visible = true;
            mevcutPb.Dock = DockStyle.Fill;
            mevcutPb.BorderStyle = BorderStyle.None;
            mevcutPb.BackColor = Color.FromArgb(15, 23, 42);

            kart.Controls.Add(mevcutPb);
            kart.Controls.Add(pnlKartUst);
            tablo.Controls.Add(kart, 0, satir);
        }

        // 1. AŞAMA: Sadece Resim Seçer ve Ekranı Temizler
        private void btnResimSec_Click(object sender, EventArgs e)
        {
            string dosyaYolu = FormGezintiYardimcisi.ResimDosyasiSec("RGB Histogram Analizi İçin Resim Seçiniz");
            if (string.IsNullOrEmpty(dosyaYolu)) return;

            orijinalMat?.Dispose();
            orijinalMat = Cv2.ImRead(dosyaYolu, ImreadModes.Color);

            if (orijinalMat.Empty())
            {
                MessageBox.Show("Resim yüklenemedi!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            histogramlarHazir = false;
            picResim.Image?.Dispose();
            picResim.Image = BitmapConverter.ToBitmap(orijinalMat);
            rozetResim.MetinGuncelle($"{orijinalMat.Width}×{orijinalMat.Height}");

            picHistR.Image?.Dispose(); picHistR.Image = null;
            picHistG.Image?.Dispose(); picHistG.Image = null;
            picHistB.Image?.Dispose(); picHistB.Image = null;

            rozetHistR.MetinGuncelle("R Kanalı");
            rozetHistG.MetinGuncelle("G Kanalı");
            rozetHistB.MetinGuncelle("B Kanalı");

            txtYorum.Text = "Resim yüklendi. Kanalların histogramlarını çizdirmek için '2. Kanalları Analiz Et' butonuna tıklayınız.";
            lblAnaliz.Text = $"Görsel yüklendi ({orijinalMat.Width}×{orijinalMat.Height} px) • Histogram çizimi bekleniyor.";
        }

        // 2. AŞAMA: Butona Basılınca Histogramları Hesaplar ve Çizer
        private void btnAnalizEt_Click(object sender, EventArgs e)
        {
            if (orijinalMat == null)
            {
                MessageBox.Show("Lütfen önce bir resim seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            HistogramlariHesaplaVeCiz();

            txtYorum.Text = "R, G, B kanallarının histogramları başarıyla çizildi. Bilimsel değerlendirmeyi görmek için '3. Yorum ve Kullanım Alanları' butonuna tıklayabilirsiniz.";
            lblAnaliz.Text = "R, G, B kanal histogramları hesaplandı ve çizildi.";
        }

        // 3. AŞAMA: Akademik Raporu Metin Kutusuna Doldurur
        private void btnYorumYap_Click(object sender, EventArgs e)
        {
            if (orijinalMat == null)
            {
                MessageBox.Show("Lütfen önce bir resim seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!histogramlarHazir)
                HistogramlariHesaplaVeCiz();

            AkademikRaporOlustur();
        }

        private void HistogramlariHesaplaVeCiz()
        {
            if (orijinalMat == null) return;

            Array.Clear(histR, 0, 256);
            Array.Clear(histG, 0, 256);
            Array.Clear(histB, 0, 256);

            long toplamR = 0, toplamG = 0, toplamB = 0;
            long pikselSayisi = orijinalMat.Width * orijinalMat.Height;

            for (int y = 0; y < orijinalMat.Height; y++)
            {
                for (int x = 0; x < orijinalMat.Width; x++)
                {
                    Vec3b c = orijinalMat.At<Vec3b>(y, x);
                    histB[c.Item0]++;
                    histG[c.Item1]++;
                    histR[c.Item2]++;

                    toplamB += c.Item0;
                    toplamG += c.Item1;
                    toplamR += c.Item2;
                }
            }

            histogramlarHazir = true;

            rozetHistR.MetinGuncelle($"Ort: {(double)toplamR / pikselSayisi:F1}");
            rozetHistG.MetinGuncelle($"Ort: {(double)toplamG / pikselSayisi:F1}");
            rozetHistB.MetinGuncelle($"Ort: {(double)toplamB / pikselSayisi:F1}");

            TekHistogramCiz(picHistR, histR, Color.FromArgb(248, 113, 113));
            TekHistogramCiz(picHistG, histG, Color.FromArgb(74, 222, 128));
            TekHistogramCiz(picHistB, histB, Color.FromArgb(56, 189, 248));
        }

        private void TekHistogramCiz(PictureBox pb, int[] hist, Color renk)
        {
            if (pb == null || pb.ClientSize.Width <= 5 || pb.ClientSize.Height <= 5) return;

            int w = pb.ClientSize.Width;
            int h = pb.ClientSize.Height;
            Bitmap bmp = new Bitmap(w, h);

            int max = 1;
            for (int i = 0; i < 256; i++)
            {
                if (hist[i] > max) max = hist[i];
            }

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.FromArgb(15, 23, 42));

                using (Pen pRef = new Pen(Color.FromArgb(51, 65, 85), 1))
                {
                    for (int i = 50; i <= 250; i += 50)
                    {
                        float xRef = (float)i / 255f * (w - 1);
                        g.DrawLine(pRef, xRef, 0, xRef, h);
                    }
                }

                using (Pen p = new Pen(renk, 1.4f))
                {
                    for (int i = 0; i < 256; i++)
                    {
                        float x = (float)i / 255f * (w - 1);
                        float barH = ((float)hist[i] / max) * (h - 10);
                        g.DrawLine(p, x, h - 1, x, h - 1 - barH);
                    }
                }
            }

            pb.Image?.Dispose();
            pb.Image = bmp;
        }

        private void AkademikRaporOlustur()
        {
            if (orijinalMat == null || txtYorum == null) return;

            long toplamR = 0, toplamG = 0, toplamB = 0;
            long pikselSayisi = orijinalMat.Width * orijinalMat.Height;

            for (int i = 0; i < 256; i++)
            {
                toplamR += (long)histR[i] * i;
                toplamG += (long)histG[i] * i;
                toplamB += (long)histB[i] * i;
            }

            double ortR = (double)toplamR / pikselSayisi;
            double ortG = (double)toplamG / pikselSayisi;
            double ortB = (double)toplamB / pikselSayisi;

            string baskinKanal = "Dengeli";
            if (ortR > ortG && ortR > ortB) baskinKanal = "Kırmızı (R)";
            else if (ortG > ortR && ortG > ortB) baskinKanal = "Yeşil (G)";
            else if (ortB > ortR && ortB > ortG) baskinKanal = "Mavi (B)";

            lblAnaliz.Text = $"İstatistiksel Özet • Ort. R: {ortR:F1} | Ort. G: {ortG:F1} | Ort. B: {ortB:F1} • Baskın Kanal: {baskinKanal}";

            txtYorum.Text = "RENK KANALLARI HİSTOGRAM ANALİZİ VE AKADEMİK DEĞERLENDİRME:\r\n\r\n" +
                $"1. MEVCUT GÖRSELİN İSTATİSTİKSEL DURUMU:\r\n" +
                $"• Ortalama R Değeri: {ortR:F1} | Ortalama G Değeri: {ortG:F1} | Ortalama B Değeri: {ortB:F1}\r\n" +
                $"• Baskın Renk Kanalı: {baskinKanal}\r\n\r\n" +
                "2. BU BİLGİLER GÖRÜNTÜ İŞLEMEDE NERELERDE KULLANILIR?\r\n" +
                "a) Beyaz Dengesi ve Renk Düzeltme (White Balance / Color Cast):\r\n" +
                "Kanal histogramlarının tepe noktaları karşılaştırılarak ortam ışığından kaynaklanan renk sapmaları (örneğin aşırı sarı, mavi veya yeşil ton) tespit edilir ve kanallar dengelenerek doğal renkler elde edilir.\r\n\r\n" +
                "b) Renk Tabanlı Nesne Tespiti ve Bölütleme (Color Segmentation):\r\n" +
                "Hedef bir nesnenin (örneğin cilt rengi, bitki örtüsü, trafik işaretleri) kendine özgü kanal dağılımı eşiklenerek arka plandan kolayca ayrıştırılması sağlanır.\r\n\r\n" +
                "c) Medikal ve Biyomedikal Görüntüleme:\r\n" +
                "Hücre boyamaları, doku analizleri veya retina taramalarında spesifik kanalların (örneğin kan damarlarını en iyi gösteren yeşil kanal) histogramı incelenerek hastalık teşhisi yapılır.\r\n\r\n" +
                "d) Tarımsal Görüntü İşleme ve Uydu Analizleri:\r\n" +
                "Bitkilerin klorofil oranını ölçen NDVI benzeri indekslerde kırmızı ve yeşil/kızılötesi kanalların histogram yığılmalarından bitki sağlığı haritaları çıkarılır.\r\n\r\n" +
                "e) Histogram Eşitleme ve Kontrast İyileştirme:\r\n" +
                "Her kanala bağımsız ya da HSV/Lab uzayında parlaklık kanalına histogram eşitleme uygulanarak karanlık veya aşırı aydınlık görseller optimize edilir.";
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            orijinalMat?.Dispose();
            base.OnFormClosed(e);
        }
    }
}