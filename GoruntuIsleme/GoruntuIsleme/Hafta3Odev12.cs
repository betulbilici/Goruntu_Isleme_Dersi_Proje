using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace GoruntuIsleme
{
    public partial class Hafta3Odev12 : Form
    {
        private Bitmap orijinalBmp = null;

        private ModernHapRozet rozetOrijinal;
        private ModernHapRozet rozetA;
        private ModernHapRozet rozetB;

        private ModernSurgu surguAci;
        private Label lblValAci;
        private Label lblBilgiKart;
        private Label lblAnaliz;

        public Hafta3Odev12()
        {
            InitializeComponent();
            ModernArayuzuUygula();
        }

        private void ModernArayuzuUygula()
        {
            // 1. Form Temel Ayarları (1250x750, Standart Kenarlık ve X ile Ana Menüye Dönüş)
            FormGezintiYardimcisi.FormuHazirla(this);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Text = "Hafta 3 - Ödev 12: Alias Giderme ve Köşeden Kaydırarak (3-Shear) Döndürme";

            // Designer'daki tüm eski kontrolleri gizle
            foreach (Control ctrl in this.Controls)
            {
                ctrl.Visible = false;
            }

            // 2. Üst Gezinti Barı
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                form: this,
                baslik: "Hafta 3 - Ödev 12 | Alias Delik Doldurma (3×3) vs Köşeden Eğerek Döndürme (3-Shear)",
                oncekiYazi: "< Ödev 11",
                oncekiTikla: () =>
                {
                    Form odev11 = Application.OpenForms["Hafta3Odev11"];
                    if (odev11 != null)
                        FormGezintiYardimcisi.FormGecisYap(this, odev11);
                    else
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta3Odev11());
                },
                sonrakiYazi: "Ödev 13 >",
                sonrakiTikla: () =>
                {
                    Form odev13 = Application.OpenForms["Hafta3Odev13"];
                    if (odev13 != null)
                        FormGezintiYardimcisi.FormGecisYap(this, odev13);
                    else
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta3Odev13());
                }
            );

            // 3. Sağ Kontrol Paneli
            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 290);

            Label lblKontrolBaslik = new Label
            {
                Text = "Döndürme & Alias Kontrolleri",
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

            // Dönme Açısı Sürgüsü (-180° ile +180°)
            Label lblAciBaslik = new Label
            {
                Text = "Dönme Açısı (°)",
                Location = new Point(18, 100),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi
            };
            pnlSag.Controls.Add(lblAciBaslik);

            lblValAci = new Label
            {
                Text = "30°",
                Location = new Point(210, 100),
                Size = new Size(60, 18),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuMaviYazi
            };
            pnlSag.Controls.Add(lblValAci);

            surguAci = new ModernSurgu
            {
                Minimum = -180,
                Maximum = 180,
                Value = 30,
                Location = new Point(18, 120),
                Size = new Size(252, 24)
            };
            surguAci.Scroll += (s, e) =>
            {
                lblValAci.Text = $"{surguAci.Value}°";
            };
            pnlSag.Controls.Add(surguAci);

            // a) Geometrik Döndür & Delik Doldur Butonu
            ModernHapButon btnModernA = FormGezintiYardimcisi.ModernButonOlustur("a) Geometrik Döndür & Delik Doldur", true, 252, 38);
            btnModernA.Location = new Point(18, 154);
            btnModernA.Click += btnIslemA_Click;
            pnlSag.Controls.Add(btnModernA);

            // b) Köşeden Eğerek Döndür Butonu
            ModernHapButon btnModernB = FormGezintiYardimcisi.ModernButonOlustur("b) Köşeden Eğerek Döndür", true, 252, 38);
            btnModernB.Location = new Point(18, 200);
            btnModernB.Click += btnIslemB_Click;
            pnlSag.Controls.Add(btnModernB);

            // Sıfırla Butonu
            ModernHapButon btnModernSifirla = FormGezintiYardimcisi.ModernButonOlustur("🔄 Temizle & Sıfırla", false, 252, 34);
            btnModernSifirla.Location = new Point(18, 246);
            btnModernSifirla.Click += btnSifirla_Click;
            pnlSag.Controls.Add(btnModernSifirla);

            // Açık Mavi Algoritma Açıklama Kartı
            ModernKartPanel bilgiKarti = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                konum: new Point(18, 292),
                boyut: new Size(252, 275),
                icerikEtiketi: out lblBilgiKart
            );
            lblBilgiKart.Text =
                "Yöntem Özeti:\n\n" +
                "a) İleri Haritalama & 3×3 Delik Doldurma:\n" +
                "Forward mapping sonucu oluşan boş pikseller, dolu 3×3 komşuların RGB ortalaması ile doldurulur.\n\n" +
                "b) 3-Shear (Yana & Aşağı Eğme):\n" +
                "α = -tan(θ/2) ve β = sin(θ) katsayılarıyla 3 adımlı kaydırma ters haritalama ile uygulanır.";
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

            rozetOrijinal = new ModernHapRozet(); rozetOrijinal.MetinGuncelle("Kaynak");
            rozetA = new ModernHapRozet(); rozetA.MetinGuncelle("a) 3×3 Ortalama");
            rozetB = new ModernHapRozet(); rozetB.MetinGuncelle("b) 3-Shear");

            MevcutPictureBoxKartaTasi(goruntuIzgara, 0, 0, "Orijinal Resim", picOrijinal, FormGezintiYardimcisi.TemaZemin, rozetOrijinal);
            MevcutPictureBoxKartaTasi(goruntuIzgara, 1, 0, "a) Alias Giderme (Komşu Ortalaması)", picA, FormGezintiYardimcisi.TemaZemin, rozetA);
            MevcutPictureBoxKartaTasi(goruntuIzgara, 2, 0, "b) Köşeden Eğerek Döndürme", picB, FormGezintiYardimcisi.TemaZemin, rozetB);

            // Alt Analiz Şeridi
            ModernKartPanel altSerit = FormGezintiYardimcisi.AltAnalizSeridiOlustur(
                "Bir resim seçin, dönme açısını ayarlayın ve sağ panelden 'a' veya 'b' yöntemlerini çalıştırarak karşılaştırın.",
                out lblAnaliz,
                "Alias & 3-Shear"
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
            mevcutPb.SizeMode = PictureBoxSizeMode.Zoom;
            mevcutPb.BorderStyle = BorderStyle.None;
            mevcutPb.BackColor = zeminRengi;

            kart.Controls.Add(mevcutPb);
            kart.Controls.Add(pnlKartUst);
            tablo.Controls.Add(kart, sutun, satir);
        }

        private void btnResimSec_Click(object sender, EventArgs e)
        {
            string dosyaYolu = FormGezintiYardimcisi.ResimDosyasiSec("Alias Giderme ve 3-Shear Döndürme İçin Resim Seçiniz");
            if (string.IsNullOrEmpty(dosyaYolu)) return;

            orijinalBmp?.Dispose();
            using (Bitmap temp = new Bitmap(dosyaYolu))
            {
                orijinalBmp = new Bitmap(temp);
            }

            picOrijinal.Image?.Dispose();
            picOrijinal.SizeMode = PictureBoxSizeMode.Zoom;
            picOrijinal.Image = new Bitmap(orijinalBmp);

            picA.Image?.Dispose();
            picA.Image = null;

            picB.Image?.Dispose();
            picB.Image = null;

            rozetOrijinal.MetinGuncelle($"{orijinalBmp.Width}×{orijinalBmp.Height}");
            rozetA.MetinGuncelle("Hazır");
            rozetB.MetinGuncelle("Hazır");
            lblAnaliz.Text = $"Resim yüklendi ({orijinalBmp.Width}×{orijinalBmp.Height} px) • Dönme açısını belirleyip 'a' veya 'b' butonuna tıklayınız.";
        }

        private void btnSifirla_Click(object sender, EventArgs e)
        {
            if (orijinalBmp == null) return;

            picA.Image?.Dispose();
            picA.Image = null;

            picB.Image?.Dispose();
            picB.Image = null;

            surguAci.Value = 30;
            lblValAci.Text = "30°";

            rozetA.MetinGuncelle("Temizlendi");
            rozetB.MetinGuncelle("Temizlendi");
            lblAnaliz.Text = "Sonuç kutuları temizlendi ve dönme açısı 30° olarak sıfırlandı.";
        }

        // a) GEOMETRİK DÖNÜŞÜMLE DÖNDÜRME VE ALIAS / DELİK DOLDURMA
        private void btnIslemA_Click(object sender, EventArgs e)
        {
            if (orijinalBmp == null)
            {
                MessageBox.Show("Lütfen önce bir resim seçiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.Cursor = Cursors.WaitCursor;

            double aciDerece = surguAci.Value;
            double radyan = aciDerece * Math.PI / 180.0;
            double cos = Math.Cos(radyan);
            double sin = Math.Sin(radyan);

            int w = orijinalBmp.Width;
            int h = orijinalBmp.Height;

            int hedefW = (int)Math.Ceiling(Math.Abs(w * cos) + Math.Abs(h * sin));
            int hedefH = (int)Math.Ceiling(Math.Abs(w * sin) + Math.Abs(h * cos));

            Bitmap cikisBmp = new Bitmap(hedefW, hedefH, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(cikisBmp))
            {
                g.Clear(Color.FromArgb(240, 240, 240));
            }

            bool[,] pikselDoluMu = new bool[hedefW, hedefH];

            double cxSrc = w / 2.0;
            double cySrc = h / 2.0;
            double cxDst = hedefW / 2.0;
            double cyDst = hedefH / 2.0;

            // 1. İleri Haritalama (Forward Mapping) -> Bazı pikseller atlandığı için boş delikler kalır
            for (int x1 = 0; x1 < w; x1++)
            {
                for (int y1 = 0; y1 < h; y1++)
                {
                    Color c = orijinalBmp.GetPixel(x1, y1);

                    double xRel = x1 - cxSrc;
                    double yRel = y1 - cySrc;

                    int x2 = (int)Math.Round(cxDst + (xRel * cos - yRel * sin));
                    int y2 = (int)Math.Round(cyDst + (xRel * sin + yRel * cos));

                    if (x2 >= 0 && x2 < hedefW && y2 >= 0 && y2 < hedefH)
                    {
                        cikisBmp.SetPixel(x2, y2, c);
                        pikselDoluMu[x2, y2] = true;
                    }
                }
            }

            // 2. Alias Boşluklarını Doldurma: Boş kalan noktaların 3x3 komşu ortalamasını alma
            int doldurulanDelikSayisi = 0;
            Bitmap filtrelenmisBmp = new Bitmap(cikisBmp);
            for (int x = 1; x < hedefW - 1; x++)
            {
                for (int y = 1; y < hedefH - 1; y++)
                {
                    if (!pikselDoluMu[x, y])
                    {
                        int rToplam = 0, gToplam = 0, bToplam = 0;
                        int sayac = 0;

                        for (int kx = -1; kx <= 1; kx++)
                        {
                            for (int ky = -1; ky <= 1; ky++)
                            {
                                if (kx == 0 && ky == 0) continue;
                                if (pikselDoluMu[x + kx, y + ky])
                                {
                                    Color kRenk = cikisBmp.GetPixel(x + kx, y + ky);
                                    rToplam += kRenk.R;
                                    gToplam += kRenk.G;
                                    bToplam += kRenk.B;
                                    sayac++;
                                }
                            }
                        }

                        if (sayac >= 2)
                        {
                            Color ortalamaRenk = Color.FromArgb(rToplam / sayac, gToplam / sayac, bToplam / sayac);
                            filtrelenmisBmp.SetPixel(x, y, ortalamaRenk);
                            doldurulanDelikSayisi++;
                        }
                    }
                }
            }

            cikisBmp.Dispose();

            picA.Image?.Dispose();
            picA.SizeMode = PictureBoxSizeMode.Zoom;
            picA.Image = filtrelenmisBmp;

            rozetA.MetinGuncelle($"{aciDerece}° • {doldurulanDelikSayisi} delik");
            lblBilgiKart.Text =
                "a) İleri Haritalama & Alias Giderme:\n\n" +
                $"• Dönme Açısı: {aciDerece}°\n" +
                $"• Doldurulan Alias Boşluğu: {doldurulanDelikSayisi} px\n\n" +
                "En İyi Algoritma: Ters Dönüşüm (Inverse Mapping) ve Bilineer İnterpolasyondur.";

            lblAnaliz.Text = $"a) İleri haritalama boşlukları 3×3 komşu ortalamasıyla dolduruldu ({doldurulanDelikSayisi} piksel) • En iyi yöntem: Inverse Mapping + Bilineer İnterpolasyon.";
            this.Cursor = Cursors.Default;
        }

        // b) RESMİN KÖŞESİNDEN YANA VE AŞAĞI KAYDIRARAK (3-SHEAR İLE) DÖNDÜRME
        private void btnIslemB_Click(object sender, EventArgs e)
        {
            if (orijinalBmp == null)
            {
                MessageBox.Show("Lütfen önce bir resim seçiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.Cursor = Cursors.WaitCursor;

            double aciDerece = surguAci.Value;
            if (Math.Abs(aciDerece % 180) == 90)
            {
                MessageBox.Show("Kaydırma yöntemi 90 derece için uygun değildir. Lütfen 15, 30, 45 gibi bir açı giriniz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Cursor = Cursors.Default;
                return;
            }

            double radyan = aciDerece * Math.PI / 180.0;
            int w = orijinalBmp.Width;
            int h = orijinalBmp.Height;

            // 1. Yana ve aşağı eğme katsayıları
            double alpha = -Math.Tan(radyan / 2.0); // Yana kaydırma
            double beta = Math.Sin(radyan);         // Aşağı kaydırma

            // 2. Resmin dışına çıkan kısımların kaybolmaması için nihai tuval boyutları
            int hedefW = (int)Math.Ceiling(Math.Abs(w * Math.Cos(radyan)) + Math.Abs(h * Math.Sin(radyan))) + 4;
            int hedefH = (int)Math.Ceiling(Math.Abs(w * Math.Sin(radyan)) + Math.Abs(h * Math.Cos(radyan))) + 4;

            Bitmap cikisBmp = new Bitmap(hedefW, hedefH, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(cikisBmp))
            {
                g.Clear(Color.FromArgb(240, 240, 240));
            }

            double cxDst = hedefW / 2.0;
            double cyDst = hedefH / 2.0;
            double cxSrc = w / 2.0;
            double cySrc = h / 2.0;

            // 3. Yana ve aşağı kaydırmanın ters adımları
            for (int yd = 0; yd < hedefH; yd++)
            {
                double yRel = yd - cyDst;
                for (int xd = 0; xd < hedefW; xd++)
                {
                    double xRel = xd - cxDst;

                    double x2 = xRel - alpha * yRel;
                    double y1 = yRel - beta * x2;
                    double x0 = x2 - alpha * y1;

                    int xs = (int)Math.Round(x0 + cxSrc);
                    int ys = (int)Math.Round(y1 + cySrc);

                    if (xs >= 0 && xs < w && ys >= 0 && ys < h)
                    {
                        cikisBmp.SetPixel(xd, yd, orijinalBmp.GetPixel(xs, ys));
                    }
                }
            }

            picB.Image?.Dispose();
            picB.SizeMode = PictureBoxSizeMode.Zoom;
            picB.Image = cikisBmp;

            rozetB.MetinGuncelle($"{aciDerece}° • 3-Shear");
            lblBilgiKart.Text =
                "b) 3-Shear (Yana & Aşağı Eğme):\n\n" +
                $"• Dönme Açısı: {aciDerece}°\n" +
                $"• α = -tan(θ/2) = {alpha:F3}\n" +
                $"• β = sin(θ) = {beta:F3}\n" +
                $"• Hedef Tuval: {hedefW} × {hedefH} px\n\n" +
                "Taşmayı önlemek için sınır açılarıyla tuval boyutu hesaplanıp ofsetle merkezlendi.";

            lblAnaliz.Text = $"b) 3-Shear formülleriyle döndürüldü (θ={aciDerece}°, α={alpha:F3}, β={beta:F3}) • Taşma ofset ile önlendi.";
            this.Cursor = Cursors.Default;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            orijinalBmp?.Dispose();
            base.OnFormClosed(e);
        }
    }
}