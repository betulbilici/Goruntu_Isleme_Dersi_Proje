using System;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace GoruntuIsleme
{
    public partial class Hafta2Odev1 : Form
    {
        private Mat orijinalMat = null;
        private ModernHapRozet rozetOrijinal;
        private ModernHapRozet rozetSonuc;
        private ModernSurgu modernSurguParlaklik;
        private Label lblSurguSol;
        private Label lblAnaliz;

        public Hafta2Odev1()
        {
            InitializeComponent();
            ModernArayuzuUygula();
        }

        private void ModernArayuzuUygula()
        {
            // 1. Form Temel Ayarları (1250x750, Standart Kenarlık ve X ile Ana Menüye Dönüş)
            FormGezintiYardimcisi.FormuHazirla(this);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Text = "Hafta 2 - Ödev 1: Parlaklık Ayarlama (Brightness)";

            // Designer'daki eski kontrolleri gizle
            foreach (Control ctrl in this.Controls)
            {
                ctrl.Visible = false;
            }

            // 2. Üst Gezinti Barı
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                form: this,
                baslik: "Hafta 2 - Ödev 1 | Görüntü Parlaklığı Ayarlama (R ± b, G ± b, B ± b)",
                oncekiYazi: "< Hafta 1",
                oncekiTikla: () =>
                {
                    Form hafta1Odev8 = Application.OpenForms["Hafta1Odev8"];
                    if (hafta1Odev8 != null)
                        FormGezintiYardimcisi.FormGecisYap(this, hafta1Odev8);
                    else
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta1Odev8());
                },
                sonrakiYazi: "Ödev 2 >",
                sonrakiTikla: () => FormGezintiYardimcisi.FormGecisYap(this, new Hafta2Odev2())
            );

            // 3. Sağ Kontrol Paneli
            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 290);

            Label lblKontrolBaslik = new Label
            {
                Text = "Parlaklık Kontrolleri",
                Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi,
                Location = new System.Drawing.Point(18, 18),
                AutoSize = true
            };
            pnlSag.Controls.Add(lblKontrolBaslik);

            // Resim Seç Butonu
            ModernHapButon btnModernResimSec = FormGezintiYardimcisi.ModernButonOlustur("📁 Resim Seç", true, 252, 40);
            btnModernResimSec.Location = new System.Drawing.Point(18, 54);
            btnModernResimSec.Click += btnResimSec_Click;
            pnlSag.Controls.Add(btnModernResimSec);

            // Parlaklığı Sıfırla (b = 0) Butonu
            ModernHapButon btnSifirla = FormGezintiYardimcisi.ModernButonOlustur("🔄 Sıfırla (b = 0)", false, 252, 38);
            btnSifirla.Location = new System.Drawing.Point(18, 104);
            btnSifirla.Click += (s, e) =>
            {
                if (modernSurguParlaklik != null)
                {
                    modernSurguParlaklik.Value = 0;
                    ParlaklikGuncelle(0);
                }
            };
            pnlSag.Controls.Add(btnSifirla);

            // Hızlı Ön Ayar Butonları (-60 ve +60)
            ModernHapButon btnKaranlik = FormGezintiYardimcisi.ModernButonOlustur("🌙 -60", false, 121, 36);
            btnKaranlik.Location = new System.Drawing.Point(18, 152);
            btnKaranlik.Click += (s, e) =>
            {
                if (modernSurguParlaklik != null)
                {
                    modernSurguParlaklik.Value = -60;
                    ParlaklikGuncelle(-60);
                }
            };
            pnlSag.Controls.Add(btnKaranlik);

            ModernHapButon btnAydinlik = FormGezintiYardimcisi.ModernButonOlustur("☀️ +60", false, 121, 36);
            btnAydinlik.Location = new System.Drawing.Point(149, 152);
            btnAydinlik.Click += (s, e) =>
            {
                if (modernSurguParlaklik != null)
                {
                    modernSurguParlaklik.Value = 60;
                    ParlaklikGuncelle(60);
                }
            };
            pnlSag.Controls.Add(btnAydinlik);

            // Açık Mavi Formül ve Açıklama Kartı
            ModernKartPanel bilgiKarti = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                konum: new System.Drawing.Point(18, 206),
                boyut: new System.Drawing.Size(252, 240),
                icerikEtiketi: out Label lblBilgi
            );
            lblBilgi.Text =
                "Parlaklık Dönüşümü:\n\n" +
                "f(x, y) = g(x, y) ± b\n" +
                "• R' = Clamp(R + b, 0, 255)\n" +
                "• G' = Clamp(G + b, 0, 255)\n" +
                "• B' = Clamp(B + b, 0, 255)\n\n" +
                "b > 0 görüntüyü aydınlatır, b < 0 görüntüyü karartır. Sağ kart altındaki sürgü ile anlık değiştirebilirsiniz.";
            pnlSag.Controls.Add(bilgiKarti);

            // 4. Orta Çalışma Alanı (Yan Yana 2 Kart + Alt Analiz Şeridi)
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
                RowCount = 1
            };
            kartIzgara.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            kartIzgara.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            kartIzgara.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            rozetOrijinal = new ModernHapRozet();
            rozetOrijinal.MetinGuncelle("Bekleniyor");

            rozetSonuc = new ModernHapRozet();
            rozetSonuc.MetinGuncelle("b = 0");

            // Sağ Kart Altına Modern Parlaklık Sürgüsü (-255 .. +255)
            Panel pnlSurgu = FormGezintiYardimcisi.KartAltSurguPaneliOlustur(
                solEtiket: "Parlaklık Değeri (b): 0",
                sagAralik: "-255  ..  +255",
                min: -255,
                max: 255,
                deger: 0,
                surgu: out modernSurguParlaklik,
                solLabel: out lblSurguSol
            );

            modernSurguParlaklik.Scroll += (s, e) => ParlaklikGuncelle(modernSurguParlaklik.Value);
            modernSurguParlaklik.ValueChanged += (s, e) => ParlaklikGuncelle(modernSurguParlaklik.Value);

            MevcutPictureBoxKartaTasi(kartIzgara, 0, 0, "Orijinal Görüntü", picOrijinal, null, rozetOrijinal);
            MevcutPictureBoxKartaTasi(kartIzgara, 1, 0, "Parlaklık Sonucu", picSonuc, pnlSurgu, rozetSonuc);

            // Alt Analiz Şeridi
            ModernKartPanel altSerit = FormGezintiYardimcisi.AltAnalizSeridiOlustur(
                "Parlaklık dönüşümü uygulamak için sağ panelden bir resim seçiniz.",
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
            Control altKontrolAlani = null,
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
                sagUstRozet.BringToFront();
            }

            if (altKontrolAlani != null)
            {
                altKontrolAlani.Dock = DockStyle.Bottom;
                altKontrolAlani.BackColor = Color.White;
                kart.Controls.Add(altKontrolAlani);
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

        private void ParlaklikGuncelle(int bDegeri)
        {
            string isaretli = bDegeri > 0 ? $"+{bDegeri}" : bDegeri.ToString();
            if (lblSurguSol != null) lblSurguSol.Text = $"Parlaklık Değeri (b): {isaretli}";
            rozetSonuc?.MetinGuncelle($"b = {isaretli}");

            if (orijinalMat == null || orijinalMat.Empty()) return;

            ParlaklikUygula(bDegeri);
            if (lblAnaliz != null)
            {
                string durum = bDegeri > 0 ? "Aydınlatma (+)" : (bDegeri < 0 ? "Karartma (-)" : "Orijinal Seviye");
                lblAnaliz.Text = $"Boyut: {orijinalMat.Width}×{orijinalMat.Height} px • Parlaklık Sabiti (b): {isaretli} • Durum: {durum}";
            }
        }

        // Resim Seçme
        private void btnResimSec_Click(object sender, EventArgs e)
        {
            string dosyaYolu = FormGezintiYardimcisi.ResimDosyasiSec("Parlaklık Ayarı İçin Resim Seçiniz");
            if (string.IsNullOrEmpty(dosyaYolu)) return;

            orijinalMat?.Dispose();
            orijinalMat = Cv2.ImRead(dosyaYolu, ImreadModes.Color);

            if (orijinalMat.Empty())
            {
                MessageBox.Show("Resim yüklenemedi!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            picOrijinal.Image?.Dispose();
            picOrijinal.Image = BitmapConverter.ToBitmap(orijinalMat);

            rozetOrijinal.MetinGuncelle($"{orijinalMat.Width} × {orijinalMat.Height}");

            if (modernSurguParlaklik != null)
                modernSurguParlaklik.Value = 0;

            ParlaklikGuncelle(0);
        }

        // Slayttaki formüle göre parlaklık değiştirme metodu:
        // f(x, y) = g(x, y)(R ± b, G ± b, B ± b)
        private void ParlaklikUygula(int b)
        {
            if (orijinalMat == null) return;

            using (Mat sonucMat = new Mat(orijinalMat.Size(), MatType.CV_8UC3))
            {
                int genislik = orijinalMat.Width;
                int yukseklik = orijinalMat.Height;

                for (int y = 0; y < yukseklik; y++)
                {
                    for (int x = 0; x < genislik; x++)
                    {
                        Vec3b okunanRenk = orijinalMat.At<Vec3b>(y, x);

                        int B = Math.Clamp(okunanRenk.Item0 + b, 0, 255);
                        int G = Math.Clamp(okunanRenk.Item1 + b, 0, 255);
                        int R = Math.Clamp(okunanRenk.Item2 + b, 0, 255);

                        sonucMat.Set(y, x, new Vec3b((byte)B, (byte)G, (byte)R));
                    }
                }

                picSonuc.Image?.Dispose();
                picSonuc.Image = BitmapConverter.ToBitmap(sonucMat);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            orijinalMat?.Dispose();
            base.OnFormClosed(e);
        }
    }
}