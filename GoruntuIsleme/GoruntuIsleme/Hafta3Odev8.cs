using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace GoruntuIsleme
{
    public partial class Hafta3Odev8 : Form
    {
        private Bitmap orijinalBmp = null;
        private RectangleF resimRect; // Ölçeklenen resmin dinamik sınırları
        private bool olceklemeModuAktif = false;
        private const int TUTAMAC_BOYUT = 8; // Siyah küçük tutamaç kutucukları

        private enum TutamacTipi
        {
            Yok,
            SolUst, SagUst, SolAlt, SagAlt,     // Köşeler (Orantılı)
            UstOrta, AltOrta, SolOrta, SagOrta  // Kenarlar (Orantısız Uzama)
        }

        private TutamacTipi aktifTutamac = TutamacTipi.Yok;
        private PointF sonFareKonumu;
        private float orijinalOran = 1.0f;

        private ModernHapRozet rozetTuval;
        private ModernHapButon btnModernOlcekleModu;
        private Label lblBoyutKart;
        private Label lblAnaliz;

        public Hafta3Odev8()
        {
            InitializeComponent();
            ModernArayuzuUygula();
        }

        private void ModernArayuzuUygula()
        {
            // 1. Form Temel Ayarları (1250x750, Standart Kenarlık ve X ile Ana Menüye Dönüş)
            FormGezintiYardimcisi.FormuHazirla(this);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Text = "Hafta 3 - Ödev 8: 8 Tutamaç ile İnteraktif Ölçekleme";

            // Designer'daki tüm eski kontrolleri gizle
            foreach (Control ctrl in this.Controls)
            {
                ctrl.Visible = false;
            }

            // 2. Üst Gezinti Barı
            Panel pnlUst = FormGezintiYardimcisi.UstBarOlustur(
                form: this,
                baslik: "Hafta 3 - Ödev 8 | 8 Tutamaç ile Orantılı & Orantısız İnteraktif Ölçekleme",
                oncekiYazi: "< Ödev 7",
                oncekiTikla: () =>
                {
                    Form odev7 = Application.OpenForms["Hafta3Odev7"];
                    if (odev7 != null)
                        FormGezintiYardimcisi.FormGecisYap(this, odev7);
                    else
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta3Odev7());
                },
                sonrakiYazi: "Ödev 9 >",
                sonrakiTikla: () =>
                {
                    Form odev9 = Application.OpenForms["Hafta3Odev9"];
                    if (odev9 != null)
                        FormGezintiYardimcisi.FormGecisYap(this, odev9);
                    else
                        FormGezintiYardimcisi.FormGecisYap(this, new Hafta3Odev9());
                }
            );

            // 3. Sağ Kontrol Paneli
            Panel pnlSag = FormGezintiYardimcisi.SagKontrolPaneliOlustur(this, 290);

            Label lblKontrolBaslik = new Label
            {
                Text = "Tutamaç & Ölçekleme",
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

            // Ölçekleme Modu Aç/Kapat Butonu
            btnModernOlcekleModu = FormGezintiYardimcisi.ModernButonOlustur("🔍 Ölçekleme Modu: Açık", false, 252, 38);
            btnModernOlcekleModu.Location = new Point(18, 96);
            btnModernOlcekleModu.Click += btnOlcekleModu_Click;
            pnlSag.Controls.Add(btnModernOlcekleModu);

            // Sıfırla Butonu
            ModernHapButon btnModernSifirla = FormGezintiYardimcisi.ModernButonOlustur("🔄 Boyutu Sıfırla", false, 252, 36);
            btnModernSifirla.Location = new Point(18, 144);
            btnModernSifirla.Click += btnSifirla_Click;
            pnlSag.Controls.Add(btnModernSifirla);

            // Açık Mavi Boyut ve Kullanım Bilgi Kartı
            ModernKartPanel bilgiKarti = FormGezintiYardimcisi.AcikMaviBilgiKartiOlustur(
                konum: new Point(18, 196),
                boyut: new Size(252, 355),
                icerikEtiketi: out lblBoyutKart
            );
            lblBoyutKart.Text =
                "Tutamaç Kuralları:\n\n" +
                "• 4 Köşe Tutamacı:\n" +
                "En/boy oranını (Aspect Ratio) sabit tutarak orantılı büyütme veya küçültme yapar.\n\n" +
                "• 4 Kenar Orta Tutamacı:\n" +
                "Yalnızca yatay veya dikey yönde orantısız esnetme/basma uygular.\n\n" +
                "• Merkez Noktası:\n" +
                "Görüntünün geometrik ağırlık merkezini gösterir.";
            pnlSag.Controls.Add(bilgiKarti);

            // 4. Orta Çalışma Alanı (Üst: Çalışma Tuvali | Alt: Analiz Şeridi)
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

            // Tuval Kartı
            ModernKartPanel tuvalKart = new ModernKartPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(6),
                Padding = new Padding(12)
            };

            Panel pnlTuvalUst = new Panel
            {
                Dock = DockStyle.Top,
                Height = 28,
                BackColor = Color.White
            };

            Label lblTuvalBaslik = new Label
            {
                Text = "Çalışma Tuvali (Siyah Tutamaçlardan Tutup Sürükleyerek Ölçekleyin)",
                UseMnemonic = false,
                AutoSize = true,
                Location = new Point(0, 4),
                Font = new Font("Segoe UI Semibold", 9.8f, FontStyle.Bold),
                ForeColor = FormGezintiYardimcisi.KoyuYazi
            };
            pnlTuvalUst.Controls.Add(lblTuvalBaslik);

            rozetTuval = new ModernHapRozet();
            rozetTuval.MetinGuncelle("Hazır");
            rozetTuval.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            rozetTuval.Location = new Point(pnlTuvalUst.Width - rozetTuval.Width - 2, 1);
            pnlTuvalUst.Resize += (s, e) =>
            {
                rozetTuval.Location = new Point(pnlTuvalUst.Width - rozetTuval.Width - 2, 1);
            };
            pnlTuvalUst.Controls.Add(rozetTuval);

            picCanvas.Parent?.Controls.Remove(picCanvas);
            picCanvas.Visible = true;
            picCanvas.Dock = DockStyle.Fill;
            picCanvas.BorderStyle = BorderStyle.None;
            picCanvas.MouseDown -= PicCanvas_MouseDown;
            picCanvas.MouseMove -= PicCanvas_MouseMove;
            picCanvas.MouseUp -= PicCanvas_MouseUp;
            picCanvas.Paint -= PicCanvas_Paint;

            picCanvas.MouseDown += PicCanvas_MouseDown;
            picCanvas.MouseMove += PicCanvas_MouseMove;
            picCanvas.MouseUp += PicCanvas_MouseUp;
            picCanvas.Paint += PicCanvas_Paint;
            picCanvas.Resize += (s, e) => picCanvas.Invalidate();

            tuvalKart.Controls.Add(picCanvas);
            tuvalKart.Controls.Add(pnlTuvalUst);

            // Alt Analiz Şeridi
            ModernKartPanel altSerit = FormGezintiYardimcisi.AltAnalizSeridiOlustur(
                "Bir resim seçin; köşe tutamaçlarıyla orantılı, kenar orta tutamaçlarıyla tek eksende ölçekleme yapın.",
                out lblAnaliz,
                "8 Tutamaç"
            );

            anaYerlesim.Controls.Add(tuvalKart, 0, 0);
            anaYerlesim.Controls.Add(altSerit, 0, 1);
            pnlOrta.Controls.Add(anaYerlesim);

            this.Controls.Add(pnlOrta);
            FormGezintiYardimcisi.YerlesimiDuzenle(this, pnlUst, pnlSag, pnlOrta);
        }

        private void btnResimSec_Click(object sender, EventArgs e)
        {
            string dosyaYolu = FormGezintiYardimcisi.ResimDosyasiSec("Tutamaçlarla Ölçekleme İçin Resim Seçiniz");
            if (string.IsNullOrEmpty(dosyaYolu)) return;

            orijinalBmp?.Dispose();
            using (Bitmap temp = new Bitmap(dosyaYolu))
            {
                orijinalBmp = new Bitmap(temp);
            }

            orijinalOran = (float)orijinalBmp.Width / orijinalBmp.Height;

            float tuvalW = Math.Max(200, picCanvas.ClientSize.Width);
            float tuvalH = Math.Max(200, picCanvas.ClientSize.Height);
            float baslangicW = Math.Min(orijinalBmp.Width, tuvalW * 0.65f);
            float baslangicH = baslangicW / orijinalOran;
            if (baslangicH > tuvalH * 0.75f)
            {
                baslangicH = tuvalH * 0.75f;
                baslangicW = baslangicH * orijinalOran;
            }

            resimRect = new RectangleF(24, 24, baslangicW, baslangicH);
            olceklemeModuAktif = true;
            btnModernOlcekleModu.Text = "🔍 Ölçekleme Modu: Açık";

            GuncelleBilgi();
            picCanvas.Invalidate();
        }

        private void btnOlcekleModu_Click(object sender, EventArgs e)
        {
            olceklemeModuAktif = !olceklemeModuAktif;
            btnModernOlcekleModu.Text = olceklemeModuAktif ? "🔍 Ölçekleme Modu: Açık" : "🔍 Ölçekleme Modu: Kapalı";
            picCanvas.Invalidate();
        }

        private void btnSifirla_Click(object sender, EventArgs e)
        {
            if (orijinalBmp == null) return;

            float tuvalW = Math.Max(200, picCanvas.ClientSize.Width);
            float tuvalH = Math.Max(200, picCanvas.ClientSize.Height);
            float baslangicW = Math.Min(orijinalBmp.Width, tuvalW * 0.65f);
            float baslangicH = baslangicW / orijinalOran;
            if (baslangicH > tuvalH * 0.75f)
            {
                baslangicH = tuvalH * 0.75f;
                baslangicW = baslangicH * orijinalOran;
            }

            resimRect = new RectangleF(24, 24, baslangicW, baslangicH);
            GuncelleBilgi();
            picCanvas.Invalidate();
        }

        // 8 adet tutamacın koordinatları (yakalama toleransı dahil)
        private RectangleF[] TutamacKutulariniAl(float ekTolerans = 0f)
        {
            float x = resimRect.X;
            float y = resimRect.Y;
            float w = resimRect.Width;
            float h = resimRect.Height;
            float boyut = TUTAMAC_BOYUT + ekTolerans * 2f;
            float r = boyut / 2f;

            return new RectangleF[]
            {
                new RectangleF(x - r, y - r, boyut, boyut),             // Sol-Üst
                new RectangleF(x + w - r, y - r, boyut, boyut),         // Sağ-Üst
                new RectangleF(x - r, y + h - r, boyut, boyut),         // Sol-Alt
                new RectangleF(x + w - r, y + h - r, boyut, boyut),     // Sağ-Alt
                new RectangleF(x + w / 2f - r, y - r, boyut, boyut),    // Üst-Orta
                new RectangleF(x + w / 2f - r, y + h - r, boyut, boyut),// Alt-Orta
                new RectangleF(x - r, y + h / 2f - r, boyut, boyut),    // Sol-Orta
                new RectangleF(x + w - r, y + h / 2f - r, boyut, boyut) // Sağ-Orta
            };
        }

        private TutamacTipi FareHangiTutamaçta(PointF p)
        {
            if (!olceklemeModuAktif || orijinalBmp == null) return TutamacTipi.Yok;

            RectangleF[] kutular = TutamacKutulariniAl(4f);
            if (kutular[0].Contains(p)) return TutamacTipi.SolUst;
            if (kutular[1].Contains(p)) return TutamacTipi.SagUst;
            if (kutular[2].Contains(p)) return TutamacTipi.SolAlt;
            if (kutular[3].Contains(p)) return TutamacTipi.SagAlt;
            if (kutular[4].Contains(p)) return TutamacTipi.UstOrta;
            if (kutular[5].Contains(p)) return TutamacTipi.AltOrta;
            if (kutular[6].Contains(p)) return TutamacTipi.SolOrta;
            if (kutular[7].Contains(p)) return TutamacTipi.SagOrta;

            return TutamacTipi.Yok;
        }

        private void PicCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || !olceklemeModuAktif || orijinalBmp == null) return;

            aktifTutamac = FareHangiTutamaçta(e.Location);
            sonFareKonumu = e.Location;
        }

        private void PicCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!olceklemeModuAktif || orijinalBmp == null)
            {
                picCanvas.Cursor = Cursors.Default;
                return;
            }

            if (aktifTutamac == TutamacTipi.Yok)
            {
                TutamacTipi uzerindeki = FareHangiTutamaçta(e.Location);
                switch (uzerindeki)
                {
                    case TutamacTipi.SolUst:
                    case TutamacTipi.SagAlt:
                        picCanvas.Cursor = Cursors.SizeNWSE;
                        break;
                    case TutamacTipi.SagUst:
                    case TutamacTipi.SolAlt:
                        picCanvas.Cursor = Cursors.SizeNESW;
                        break;
                    case TutamacTipi.UstOrta:
                    case TutamacTipi.AltOrta:
                        picCanvas.Cursor = Cursors.SizeNS;
                        break;
                    case TutamacTipi.SolOrta:
                    case TutamacTipi.SagOrta:
                        picCanvas.Cursor = Cursors.SizeWE;
                        break;
                    default:
                        picCanvas.Cursor = Cursors.Default;
                        break;
                }
                return;
            }

            float dx = e.X - sonFareKonumu.X;
            float dy = e.Y - sonFareKonumu.Y;

            float yeniX = resimRect.X;
            float yeniY = resimRect.Y;
            float yeniW = resimRect.Width;
            float yeniH = resimRect.Height;

            switch (aktifTutamac)
            {
                // KÖŞE TUTAMAÇLARI: Orantılı (Aspect Ratio Sabit) Ölçekleme
                case TutamacTipi.SagAlt:
                    yeniW = Math.Max(30, resimRect.Width + dx);
                    yeniH = yeniW / orijinalOran;
                    break;

                case TutamacTipi.SagUst:
                    yeniW = Math.Max(30, resimRect.Width + dx);
                    yeniH = yeniW / orijinalOran;
                    yeniY = resimRect.Bottom - yeniH;
                    break;

                case TutamacTipi.SolAlt:
                    yeniW = Math.Max(30, resimRect.Width - dx);
                    yeniH = yeniW / orijinalOran;
                    yeniX = resimRect.Right - yeniW;
                    break;

                case TutamacTipi.SolUst:
                    yeniW = Math.Max(30, resimRect.Width - dx);
                    yeniH = yeniW / orijinalOran;
                    yeniX = resimRect.Right - yeniW;
                    yeniY = resimRect.Bottom - yeniH;
                    break;

                // KENAR ORTA TUTAMAÇLARI: Orantısız (Yatay veya Dikey Uzama/Basma)
                case TutamacTipi.SagOrta:
                    yeniW = Math.Max(30, resimRect.Width + dx);
                    break;

                case TutamacTipi.SolOrta:
                    yeniW = Math.Max(30, resimRect.Width - dx);
                    yeniX = resimRect.Right - yeniW;
                    break;

                case TutamacTipi.AltOrta:
                    yeniH = Math.Max(30, resimRect.Height + dy);
                    break;

                case TutamacTipi.UstOrta:
                    yeniH = Math.Max(30, resimRect.Height - dy);
                    yeniY = resimRect.Bottom - yeniH;
                    break;
            }

            resimRect = new RectangleF(yeniX, yeniY, yeniW, yeniH);
            sonFareKonumu = e.Location;

            GuncelleBilgi();
            picCanvas.Invalidate();
        }

        private void PicCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            aktifTutamac = TutamacTipi.Yok;
            picCanvas.Cursor = Cursors.Default;
        }

        private void PicCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            // Arka plan dama deseni
            using (HatchBrush damaFirca = new HatchBrush(HatchStyle.LargeCheckerBoard, Color.FromArgb(226, 232, 240), Color.White))
            {
                g.FillRectangle(damaFirca, picCanvas.ClientRectangle);
            }

            if (orijinalBmp != null)
            {
                g.DrawImage(orijinalBmp, resimRect);

                if (olceklemeModuAktif)
                {
                    using (Pen penSinir = new Pen(Color.FromArgb(15, 23, 42), 1.3f))
                    {
                        g.DrawRectangle(penSinir, resimRect.X, resimRect.Y, resimRect.Width, resimRect.Height);
                    }

                    RectangleF[] kutular = TutamacKutulariniAl();
                    using (Brush fircaSiyah = new SolidBrush(Color.FromArgb(15, 23, 42)))
                    using (Pen penBeyaz = new Pen(Color.White, 1.2f))
                    {
                        for (int i = 0; i < kutular.Length; i++)
                        {
                            g.FillRectangle(fircaSiyah, kutular[i]);
                            g.DrawRectangle(penBeyaz, kutular[i].X, kutular[i].Y, kutular[i].Width, kutular[i].Height);
                        }

                        // Merkez noktası
                        float cx = resimRect.X + resimRect.Width / 2f - 2f;
                        float cy = resimRect.Y + resimRect.Height / 2f - 2f;
                        g.FillRectangle(fircaSiyah, cx, cy, 4, 4);
                    }
                }
            }
        }

        private void GuncelleBilgi()
        {
            if (orijinalBmp == null) return;

            float anlikOran = resimRect.Width / resimRect.Height;
            bool orantiKorundu = Math.Abs(anlikOran - orijinalOran) < 0.03f;
            string durumMetni = orantiKorundu ? "En/Boy Orantısı Korundu" : "Orantısız Esnetildi";

            rozetTuval.MetinGuncelle($"{Math.Round(resimRect.Width)}×{Math.Round(resimRect.Height)} px");

            lblBoyutKart.Text =
                "Canlı Ölçekleme Bilgileri:\n\n" +
                $"• Orijinal Boyut: {orijinalBmp.Width} × {orijinalBmp.Height} px\n" +
                $"• Mevcut Boyut: {Math.Round(resimRect.Width)} × {Math.Round(resimRect.Height)} px\n" +
                $"• Orijinal Oran: {orijinalOran:F2}\n" +
                $"• Güncel Oran: {anlikOran:F2}\n" +
                $"• Durum: {durumMetni}\n\n" +
                "Köşe tutamaçları oranı korur, kenar orta tutamaçları tek yönde esnetir.";

            lblAnaliz.Text = $"Orijinal: {orijinalBmp.Width}×{orijinalBmp.Height} px → Mevcut: {Math.Round(resimRect.Width)}×{Math.Round(resimRect.Height)} px • {durumMetni}";
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            orijinalBmp?.Dispose();
            base.OnFormClosed(e);
        }
    }
}