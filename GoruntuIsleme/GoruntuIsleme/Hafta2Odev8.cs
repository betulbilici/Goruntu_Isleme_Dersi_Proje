using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace GoruntuIsleme
{
    public partial class Hafta2Odev8 : Form
    {
        private class ReferansNokta
        {
            public Point Konum { get; set; }
            public Color Renk { get; set; }
            public byte GriDeger { get; set; }
        }

        private Mat griMat = null;
        private List<ReferansNokta> noktalar = new List<ReferansNokta>();
        private Color aktifRenk = Color.SkyBlue;

        private ModernHapRozet rozetGiris;
        private ModernHapRozet rozetCikis;
        private ModernHapRozet rozetNoktaSayisi;
        private Label lblAnaliz;

        public Hafta2Odev8()
        {
            InitializeComponent();
            ModernArayuzuUygula();
        }

        private void ModernArayuzuUygula()
        {
            // 1. Form Temel Ayarları (1250x750, Standart Kenarlık ve X ile Ana Menüye Dönüş)
            FormGezintiYardimcisi.FormuHazirla(this);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Text = "Hafta 2 - Ödev 8: Referans Noktalarla Siyah-Beyaz Fotoğraf Renklendirme (IDW)";

            // Designer'daki tüm eski kontrolleri gizle
            foreach (Control ctrl in this.Controls)
            {
                ctrl.Visible = false;
            }

            // 2. Üst Gezinti Barı (Ödev 7 ve Ödev 9'a geçişte eski formlar açık kalmaz)
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                form: this,
                baslik: "Hafta 2 - Ödev 8 | Referans Noktalarla Siyah-Beyaz Fotoğraf Renklendirme (IDW)",
                oncekiYazi: "< Ödev 7",
                oncekiTikla: () =>
                {
                    Form odev7 = Application.OpenForms["Hafta2Odev7"];
                    if (odev7 != null)
                        FormGezintiYardimcisi.FormGecisYap(this, odev7);
                    else
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta2Odev7());
                },
                sonrakiYazi: "Ödev 9 >",
                sonrakiTikla: () =>
                {
                    Form odev9 = Application.OpenForms["Hafta2Odev9"];
                    if (odev9 != null)
                        FormGezintiYardimcisi.FormGecisYap(this, odev9);
                    else
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta2Odev9());
                }
            );

            // 3. Sağ Kontrol Paneli
            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 290);

            Label lblKontrolBaslik = new Label
            {
                Text = "Renklendirme Kontrolü",
                Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                Location = new Point(18, 16),
                AutoSize = true
            };
            pnlSag.Controls.Add(lblKontrolBaslik);

            // Resim Seç Butonu
            ModernHapButon btnModernResimSec = FormGezintiYardimcisi.ModernButonOlustur("📁 Resim Seç", true, 252, 38);
            btnModernResimSec.Location = new Point(18, 46);
            btnModernResimSec.Click += btnResimSec_Click;
            pnlSag.Controls.Add(btnModernResimSec);

            // Renk Seç Butonu + Renk Önizleme Kutusu
            ModernHapButon btnModernRenkSec = FormGezintiYardimcisi.ModernButonOlustur("🎨 Renk Seç...", false, 198, 38);
            btnModernRenkSec.Location = new Point(18, 92);
            btnModernRenkSec.Click += btnRenkSec_Click;
            pnlSag.Controls.Add(btnModernRenkSec);

            lblRenkOnizleme.Parent?.Controls.Remove(lblRenkOnizleme);
            lblRenkOnizleme.Visible = true;
            lblRenkOnizleme.Location = new Point(224, 92);
            lblRenkOnizleme.Size = new Size(46, 38);
            lblRenkOnizleme.BorderStyle = BorderStyle.FixedSingle;
            lblRenkOnizleme.BackColor = aktifRenk;
            pnlSag.Controls.Add(lblRenkOnizleme);

            // Renklendir Butonu
            ModernHapButon btnModernRenklendir = FormGezintiYardimcisi.ModernButonOlustur("✨ Fotoğrafı Renklendir", true, 252, 40);
            btnModernRenklendir.Location = new Point(18, 138);
            btnModernRenklendir.Click += btnRenklendir_Click;
            pnlSag.Controls.Add(btnModernRenklendir);

            // Noktaları Sıfırla Butonu
            ModernHapButon btnModernTemizle = FormGezintiYardimcisi.ModernButonOlustur("🔄 Noktaları Sıfırla", false, 252, 36);
            btnModernTemizle.Location = new Point(18, 186);
            btnModernTemizle.Click += btnTemizle_Click;
            pnlSag.Controls.Add(btnModernTemizle);

            // Açık Mavi Algoritma Bilgi Kartı
            ModernKartPanel bilgiKarti = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                konum: new Point(18, 236),
                boyut: new Size(252, 320),
                icerikEtiketi: out Label lblBilgi
            );
            lblBilgi.Text =
                "IDW + Parlaklık Koruma:\n\n" +
                "1. Resim seçildikten sonra 'Renk Seç' ile hedef tonu belirleyin.\n" +
                "2. Sol resim üzerinde en fazla 10 referans noktasına tıklayın.\n" +
                "3. 'Fotoğrafı Renklendir' ile Ters Mesafe Ağırlıklandırma (IDW) ve gri ton benzerliği kullanılarak renk yayılımı hesaplanır.";
            pnlSag.Controls.Add(bilgiKarti);

            // 4. Orta Çalışma Alanı (Üst: Giriş & Çıkış Kartları | Orta: Seçilen Noktalar Listesi | Alt: Analiz Şeridi)
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
            anaYerlesim.RowStyles.Add(new RowStyle(SizeType.Percent, 68f));
            anaYerlesim.RowStyles.Add(new RowStyle(SizeType.Percent, 32f));
            anaYerlesim.RowStyles.Add(new RowStyle(SizeType.Absolute, 50f));

            // Üst Satır: Giriş ve Renklendirilmiş Çıkış Kartları
            TableLayoutPanel goruntuIzgara = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1
            };
            goruntuIzgara.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            goruntuIzgara.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            goruntuIzgara.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            rozetGiris = new ModernHapRozet(); rozetGiris.MetinGuncelle("Gri-Ton");
            rozetCikis = new ModernHapRozet(); rozetCikis.MetinGuncelle("Bekleniyor");

            MevcutPictureBoxKartaTasi(goruntuIzgara, 0, 0, "Siyah-Beyaz Görüntü (Nokta Seçimi)", picGiris, FormGezintiYardimcisi.TemaZemin, rozetGiris);
            MevcutPictureBoxKartaTasi(goruntuIzgara, 1, 0, "Renklendirilmiş Sonuç", picCikis, FormGezintiYardimcisi.TemaZemin, rozetCikis);

            picGiris.Cursor = Cursors.Cross;
            picGiris.MouseDown -= PicGiris_MouseDown;
            picGiris.MouseDown += PicGiris_MouseDown;

            // Orta Satır: Seçilen Referans Noktaları Kartı
            ModernKartPanel listeKart = new ModernKartPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(6),
                Padding = new Padding(12)
            };

            Panel pnlListeUst = new Panel
            {
                Dock = DockStyle.Top,
                Height = 26,
                BackColor = Color.White
            };

            Label lblListeBaslik = new Label
            {
                Text = "İşaretlenen Referans Noktaları (Maksimum 10 Nokta)",
                AutoSize = true,
                Location = new Point(0, 3),
                Font = new Font("Segoe UI Semibold", 9.6f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi
            };
            pnlListeUst.Controls.Add(lblListeBaslik);

            rozetNoktaSayisi = new ModernHapRozet();
            rozetNoktaSayisi.MetinGuncelle("0 / 10 Nokta");
            rozetNoktaSayisi.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            rozetNoktaSayisi.Location = new Point(pnlListeUst.Width - rozetNoktaSayisi.Width - 2, 1);
            pnlListeUst.Resize += (s, e) =>
            {
                rozetNoktaSayisi.Location = new Point(pnlListeUst.Width - rozetNoktaSayisi.Width - 2, 1);
            };
            pnlListeUst.Controls.Add(rozetNoktaSayisi);

            lstNoktalar.Parent?.Controls.Remove(lstNoktalar);
            lstNoktalar.Visible = true;
            lstNoktalar.Dock = DockStyle.Fill;
            lstNoktalar.BorderStyle = BorderStyle.None;
            lstNoktalar.Font = new Font("Segoe UI", 9.3f);
            lstNoktalar.BackColor = Color.White;

            listeKart.Controls.Add(lstNoktalar);
            listeKart.Controls.Add(pnlListeUst);

            // Alt Analiz Şeridi
            ModernKartPanel altSerit = FormGezintiYardimcisi.AltAnalizSeridiOlustur(
                "Siyah-beyaz görüntü yükleyip referans noktalarını seçtikten sonra 'Fotoğrafı Renklendir' butonuna basınız.",
                out lblAnaliz,
                "IDW Renklendirme"
            );

            anaYerlesim.Controls.Add(goruntuIzgara, 0, 0);
            anaYerlesim.Controls.Add(listeKart, 0, 1);
            anaYerlesim.Controls.Add(altSerit, 0, 2);
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
                Font = new Font("Segoe UI Semibold", 9.6f, FontStyle.Bold),
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
            string dosyaYolu = FormGezintiYardimcisi.ResimDosyasiSec("Renklendirilecek Siyah-Beyaz Görüntüyü Seçiniz");
            if (string.IsNullOrEmpty(dosyaYolu)) return;

            using (Mat yuklenen = Cv2.ImRead(dosyaYolu, ImreadModes.Color))
            {
                if (yuklenen.Empty())
                {
                    MessageBox.Show("Resim yüklenemedi!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                griMat?.Dispose();
                griMat = new Mat();
                Cv2.CvtColor(yuklenen, griMat, ColorConversionCodes.BGR2GRAY);
            }

            rozetGiris.MetinGuncelle($"{griMat.Width}×{griMat.Height}");
            btnTemizle_Click(null, null);
            lblAnaliz.Text = $"Görüntü yüklendi ({griMat.Width}×{griMat.Height} px) • Sol tık ile en fazla 10 referans noktası ekleyiniz.";
        }

        private void btnRenkSec_Click(object sender, EventArgs e)
        {
            using (ColorDialog cd = new ColorDialog())
            {
                cd.FullOpen = true;
                cd.Color = aktifRenk;
                if (cd.ShowDialog() == DialogResult.OK)
                {
                    aktifRenk = cd.Color;
                    if (lblRenkOnizleme != null) lblRenkOnizleme.BackColor = aktifRenk;
                    lblAnaliz.Text = $"Aktif fırça rengi seçildi: RGB({aktifRenk.R}, {aktifRenk.G}, {aktifRenk.B})";
                }
            }
        }

        // Zoom modunda kenar boşluklarını hesaba katarak gerçek piksel koordinatını bulur
        private bool EkranKoordinatiniResmeCevir(int mouseX, int mouseY, out int gercekX, out int gercekY)
        {
            gercekX = 0;
            gercekY = 0;
            if (griMat == null || picGiris.ClientSize.Width <= 0 || picGiris.ClientSize.Height <= 0)
                return false;

            float kutuW = picGiris.ClientSize.Width;
            float kutuH = picGiris.ClientSize.Height;
            float resimW = griMat.Width;
            float resimH = griMat.Height;

            float olcek = Math.Min(kutuW / resimW, kutuH / resimH);
            float cizilenW = resimW * olcek;
            float cizilenH = resimH * olcek;
            float boslukX = (kutuW - cizilenW) / 2f;
            float boslukY = (kutuH - cizilenH) / 2f;

            gercekX = (int)((mouseX - boslukX) / olcek);
            gercekY = (int)((mouseY - boslukY) / olcek);

            return gercekX >= 0 && gercekX < griMat.Width && gercekY >= 0 && gercekY < griMat.Height;
        }

        private void PicGiris_MouseDown(object sender, MouseEventArgs e)
        {
            if (griMat == null || picGiris.Image == null || e.Button != MouseButtons.Left) return;

            if (noktalar.Count >= 10)
            {
                MessageBox.Show("Ödev şartına göre en fazla 10 adet referans nokta belirleyebilirsiniz!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (EkranKoordinatiniResmeCevir(e.X, e.Y, out int gercekX, out int gercekY))
            {
                byte gri = griMat.At<byte>(gercekY, gercekX);

                noktalar.Add(new ReferansNokta
                {
                    Konum = new Point(gercekX, gercekY),
                    Renk = aktifRenk,
                    GriDeger = gri
                });

                NoktalariEkrandaGoster();
            }
        }

        private void NoktalariEkrandaGoster()
        {
            if (griMat == null) return;

            Bitmap bmp;
            using (Mat bgrKopya = new Mat())
            {
                Cv2.CvtColor(griMat, bgrKopya, ColorConversionCodes.GRAY2BGR);
                bmp = BitmapConverter.ToBitmap(bgrKopya);
            }

            int r = Math.Max(6, Math.Min(griMat.Width, griMat.Height) / 60);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                for (int i = 0; i < noktalar.Count; i++)
                {
                    var p = noktalar[i];
                    using (Brush b = new SolidBrush(p.Renk))
                    using (Pen cerceve = new Pen(Color.White, 2))
                    {
                        g.FillEllipse(b, p.Konum.X - r, p.Konum.Y - r, r * 2, r * 2);
                        g.DrawEllipse(cerceve, p.Konum.X - r, p.Konum.Y - r, r * 2, r * 2);

                        using (Font f = new Font("Arial", Math.Max(9, r + 2), FontStyle.Bold))
                        using (Brush yazi = new SolidBrush(Color.Yellow))
                        {
                            g.DrawString((i + 1).ToString(), f, yazi, p.Konum.X + r + 2, p.Konum.Y - r);
                        }
                    }
                }
            }

            picGiris.Image?.Dispose();
            picGiris.Image = bmp;

            lstNoktalar.Items.Clear();
            for (int i = 0; i < noktalar.Count; i++)
            {
                var p = noktalar[i];
                lstNoktalar.Items.Add($"Nokta {i + 1}: Konum=({p.Konum.X}, {p.Konum.Y}) | Renk=RGB({p.Renk.R}, {p.Renk.G}, {p.Renk.B}) | Gri Değeri={p.GriDeger}");
            }

            rozetNoktaSayisi?.MetinGuncelle($"{noktalar.Count} / 10 Nokta");
            lblAnaliz.Text = $"Referans Nokta {noktalar.Count}/10 eklendi • Renklendirmek için 'Fotoğrafı Renklendir' butonuna basınız.";
        }

        private void btnTemizle_Click(object sender, EventArgs e)
        {
            noktalar.Clear();

            if (griMat != null)
            {
                using (Mat bgrKopya = new Mat())
                {
                    Cv2.CvtColor(griMat, bgrKopya, ColorConversionCodes.GRAY2BGR);
                    picGiris.Image?.Dispose();
                    picGiris.Image = BitmapConverter.ToBitmap(bgrKopya);
                }
            }

            picCikis.Image?.Dispose();
            picCikis.Image = null;

            lstNoktalar.Items.Clear();
            rozetNoktaSayisi?.MetinGuncelle("0 / 10 Nokta");
            rozetCikis?.MetinGuncelle("Bekleniyor");
        }

        private void btnRenklendir_Click(object sender, EventArgs e)
        {
            if (griMat == null)
            {
                MessageBox.Show("Lütfen önce bir resim seçin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (noktalar.Count == 0)
            {
                MessageBox.Show("Lütfen resim üzerinden en az bir referans nokta belirleyin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.Cursor = Cursors.WaitCursor;

            int w = griMat.Width;
            int h = griMat.Height;
            using (Mat cikisMat = new Mat(h, w, MatType.CV_8UC3))
            {
                double griAgirlikKatsayisi = 0.5;

                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        byte pikselGri = griMat.At<byte>(y, x);

                        double toplamAgirlik = 0.0;
                        double toplamR = 0.0;
                        double toplamG = 0.0;
                        double toplamB = 0.0;

                        bool tamNoktaUstu = false;

                        for (int i = 0; i < noktalar.Count; i++)
                        {
                            var refNokta = noktalar[i];
                            double dx = x - refNokta.Konum.X;
                            double dy = y - refNokta.Konum.Y;
                            double mesafeKare = (dx * dx) + (dy * dy);

                            if (mesafeKare == 0)
                            {
                                cikisMat.Set(y, x, new Vec3b(refNokta.Renk.B, refNokta.Renk.G, refNokta.Renk.R));
                                tamNoktaUstu = true;
                                break;
                            }

                            double griFarki = Math.Abs(pikselGri - refNokta.GriDeger);
                            double payda = mesafeKare + (griAgirlikKatsayisi * (griFarki * griFarki)) + 1.0;
                            double agirlik = 1.0 / payda;

                            toplamAgirlik += agirlik;
                            toplamR += agirlik * refNokta.Renk.R;
                            toplamG += agirlik * refNokta.Renk.G;
                            toplamB += agirlik * refNokta.Renk.B;
                        }

                        if (!tamNoktaUstu && toplamAgirlik > 0)
                        {
                            double tahminiR = toplamR / toplamAgirlik;
                            double tahminiG = toplamG / toplamAgirlik;
                            double tahminiB = toplamB / toplamAgirlik;

                            double tahminiGri = (tahminiR * 0.299) + (tahminiG * 0.587) + (tahminiB * 0.114);
                            if (tahminiGri < 1.0) tahminiGri = 1.0;

                            double carpan = (double)pikselGri / tahminiGri;

                            int sonR = Math.Min(255, (int)(tahminiR * carpan));
                            int sonG = Math.Min(255, (int)(tahminiG * carpan));
                            int sonB = Math.Min(255, (int)(tahminiB * carpan));

                            cikisMat.Set(y, x, new Vec3b((byte)sonB, (byte)sonG, (byte)sonR));
                        }
                    }
                }

                picCikis.Image?.Dispose();
                picCikis.Image = BitmapConverter.ToBitmap(cikisMat);
            }

            rozetCikis?.MetinGuncelle("Renklendirildi");
            lblAnaliz.Text = $"Renklendirme tamamlandı • {noktalar.Count} referans noktası ile IDW + Parlaklık Koruma uygulandı.";
            this.Cursor = Cursors.Default;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            griMat?.Dispose();
            base.OnFormClosed(e);
        }
    }
}