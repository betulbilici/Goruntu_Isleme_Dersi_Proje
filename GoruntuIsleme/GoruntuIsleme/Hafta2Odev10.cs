using System;
using System.Drawing;
using System.Windows.Forms;

namespace GoruntuIsleme
{
    public partial class Hafta2Odev10 : Form
    {
        private Bitmap bmpAnaRenk;
        private Bitmap bmpTonSkala;
        private Color secilenTemelRenk = Color.Red;
        private Color secilenSonRenk = Color.Red;

        // Sol taraftaki 48 adet özel renk kutucuğu
        private PictureBox[] ozelRenkKutulari = new PictureBox[48];
        private int secilenKutuIndex = -1; // Tıklanan veya silinen hedef kutuyu takip eder

        public Hafta2Odev10()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1250, 750);
            this.StartPosition = FormStartPosition.CenterScreen;

            // X butonu ile güvenli dönüş
            this.FormClosing += Hafta2Odev10_FormClosing;

            // Olay bağlantıları
            if (btnOzelEkle != null) btnOzelEkle.Click += btnOzelEkle_Click;
            if (lnkGeri != null) lnkGeri.LinkClicked += lnkGeri_LinkClicked;
            if (lnkAnaMenu != null) lnkAnaMenu.LinkClicked += lnkAnaMenu_LinkClicked;
            if (btnPaletiTemizle != null) btnPaletiTemizle.Click += btnPaletiTemizle_Click;

            if (picAnaRenk != null)
            {
                picAnaRenk.MouseDown += PicAnaRenk_MouseDown;
                picAnaRenk.MouseMove += PicAnaRenk_MouseMove;
            }

            if (picTonSkala != null)
            {
                picTonSkala.MouseDown += PicTonSkala_MouseDown;
                picTonSkala.MouseMove += PicTonSkala_MouseMove;
            }

            this.Load += Hafta2Odev10_Load;
        }

        private void Hafta2Odev10_Load(object sender, EventArgs e)
        {
            OzelRenkKutulariniOlustur();
            AnaRenkSpektrumunuCiz();
            TonSkalasiniCiz(secilenTemelRenk);
            RengiGuncelle(secilenTemelRenk);
        }

        private void Hafta2Odev10_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Form1 anaForm = (Form1)Application.OpenForms["Form1"];
                if (anaForm != null)
                {
                    anaForm.StartPosition = FormStartPosition.Manual;
                    anaForm.Location = this.Location;
                    anaForm.Size = this.Size;
                    anaForm.Show();
                    anaForm.BringToFront();
                }
                else
                {
                    Form1 yeniForm = new Form1();
                    yeniForm.StartPosition = FormStartPosition.Manual;
                    yeniForm.Location = this.Location;
                    yeniForm.Size = this.Size;
                    yeniForm.Show();
                }
            }
        }

        // Sol taraftaki 6x8'lik 48 küçük renk kutucuğunu dinamik oluşturma ve ortalama
        private void OzelRenkKutulariniOlustur()
        {
            if (pnlOzelRenkler == null) return;
            pnlOzelRenkler.Controls.Clear();

            int sutunSayisi = 8;
            int satirSayisi = 6;
            int kutuW = 32;
            int kutuH = 32;
            int bosluk = 6;

            // Panelin içinde tam ortalanması için sol ve üst boşluk hesabı:
            int toplamKutuGenisligi = (sutunSayisi * kutuW) + ((sutunSayisi - 1) * bosluk);
            int toplamKutuYuksekligi = (satirSayisi * kutuH) + ((satirSayisi - 1) * bosluk);
            int baslangicX = Math.Max(10, (pnlOzelRenkler.ClientSize.Width - toplamKutuGenisligi) / 2);
            int baslangicY = Math.Max(10, (pnlOzelRenkler.ClientSize.Height - toplamKutuYuksekligi) / 2);

            for (int i = 0; i < 48; i++)
            {
                int satir = i / sutunSayisi;
                int sutun = i % sutunSayisi;

                PictureBox pb = new PictureBox
                {
                    Size = new Size(kutuW, kutuH),
                    Location = new Point(baslangicX + sutun * (kutuW + bosluk), baslangicY + satir * (kutuH + bosluk)),
                    BorderStyle = BorderStyle.Fixed3D,
                    BackColor = Color.White,
                    Cursor = Cursors.Hand
                };

                int index = i;

                pb.MouseDown += (s, e) =>
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        // Sol tık: Bu kutuyu hedef yap ve içindeki rengi önizlemeye al
                        secilenKutuIndex = index;
                        if (pb.BackColor != Color.White)
                        {
                            RengiGuncelle(pb.BackColor);
                        }
                    }
                    else if (e.Button == MouseButtons.Right)
                    {
                        // Sağ tık: Kutuyu sil ve bir sonraki eklenecek rengin hedefi yap
                        pb.BackColor = Color.White;
                        secilenKutuIndex = index;
                    }
                };

                ozelRenkKutulari[i] = pb;
                pnlOzelRenkler.Controls.Add(pb);
            }
        }

        // 16 Milyon Renk Tabanı: Hue (0-360) ve Saturation (0-1) Gradyanını Çizdirme
        private void AnaRenkSpektrumunuCiz()
        {
            if (picAnaRenk == null || picAnaRenk.Width <= 0 || picAnaRenk.Height <= 0) return;

            int w = picAnaRenk.Width;
            int h = picAnaRenk.Height;
            bmpAnaRenk = new Bitmap(w, h);

            for (int x = 0; x < w; x++)
            {
                double hue = (double)x / w * 360.0; // X ekseni renk tonu (Hue)

                for (int y = 0; y < h; y++)
                {
                    double sat = 1.0 - ((double)y / h); // Y ekseni doygunluk (Saturation)
                    Color c = HsvToRgb(hue, sat, 1.0);
                    bmpAnaRenk.SetPixel(x, y, c);
                }
            }

            picAnaRenk.Image = bmpAnaRenk;
        }

        // Seçilen rengin açıktan koyuya ton skalasını (Value/Luminance şeridini) çizdirme
        private void TonSkalasiniCiz(Color temelRenk)
        {
            if (picTonSkala == null || picTonSkala.Width <= 0 || picTonSkala.Height <= 0) return;

            int w = picTonSkala.Width;
            int h = picTonSkala.Height;
            bmpTonSkala = new Bitmap(w, h);

            for (int y = 0; y < h; y++)
            {
                // Üst kısım en açık/parlak (1.0), alt kısım en koyu/siyah (0.0)
                double factor = 1.0 - ((double)y / (h - 1));

                int r = (int)(temelRenk.R * factor);
                int g = (int)(temelRenk.G * factor);
                int b = (int)(temelRenk.B * factor);

                Color tonRengi = Color.FromArgb(r, g, b);

                for (int x = 0; x < w; x++)
                {
                    bmpTonSkala.SetPixel(x, y, tonRengi);
                }
            }

            picTonSkala.Image?.Dispose();
            picTonSkala.Image = bmpTonSkala;
        }

        // Ortadaki Ana Renk tablosuna tıklandığında / sürüklendiğinde
        private void PicAnaRenk_MouseDown(object sender, MouseEventArgs e)
        {
            AnaRenkSec(e);
        }

        private void PicAnaRenk_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                AnaRenkSec(e);
            }
        }

        private void AnaRenkSec(MouseEventArgs e)
        {
            if (bmpAnaRenk == null || e.X < 0 || e.X >= bmpAnaRenk.Width || e.Y < 0 || e.Y >= bmpAnaRenk.Height) return;

            secilenTemelRenk = bmpAnaRenk.GetPixel(e.X, e.Y);
            TonSkalasiniCiz(secilenTemelRenk);
            RengiGuncelle(secilenTemelRenk);
        }

        // Sağdaki Ton şeridine tıklandığında / sürüklendiğinde
        private void PicTonSkala_MouseDown(object sender, MouseEventArgs e)
        {
            TonSec(e);
        }

        private void PicTonSkala_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                TonSec(e);
            }
        }

        private void TonSec(MouseEventArgs e)
        {
            if (bmpTonSkala == null || e.X < 0 || e.X >= bmpTonSkala.Width || e.Y < 0 || e.Y >= bmpTonSkala.Height) return;

            Color sonRenk = bmpTonSkala.GetPixel(e.X, e.Y);
            RengiGuncelle(sonRenk);
        }

        // Seçilen rengin RGB değerlerini ve önizlemesini güncelleme
        private void RengiGuncelle(Color c)
        {
            secilenSonRenk = c;

            if (picOnizleme != null) picOnizleme.BackColor = c;
            if (txtR != null) txtR.Text = c.R.ToString();
            if (txtG != null) txtG.Text = c.G.ToString();
            if (txtB != null) txtB.Text = c.B.ToString();
        }

        // "Özel Renklere Ekle" Butonu (Akıllı hedefleme: seçili kutu, yoksa ilk boş kutu)
        private void btnOzelEkle_Click(object sender, EventArgs e)
        {
            int hedefIndex = -1;

            // 1. Kullanıcı spesifik bir kutuya tıklamışsa doğrudan oraya ata
            if (secilenKutuIndex >= 0 && secilenKutuIndex < 48)
            {
                hedefIndex = secilenKutuIndex;
            }
            else
            {
                // 2. Tıklanmamışsa ilk boş (beyaz) kutuyu bul
                for (int i = 0; i < 48; i++)
                {
                    if (ozelRenkKutulari[i] != null && ozelRenkKutulari[i].BackColor == Color.White)
                    {
                        hedefIndex = i;
                        break;
                    }
                }
            }

            // Hiç boş kutu kalmamışsa ve seçili kutu da yoksa palet doludur
            if (hedefIndex == -1)
            {
                MessageBox.Show("Kişiye özel renk paletiniz tamamen dolu. Yeni renk eklemek için bir kutuyu sağ tık ile silebilir veya değiştirmek istediğiniz kutuya sol tıklayabilirsiniz.",
                                "Palet Dolu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Hedef kutuya rengi kaydet
            if (ozelRenkKutulari[hedefIndex] != null)
            {
                ozelRenkKutulari[hedefIndex].BackColor = secilenSonRenk;
                secilenKutuIndex = -1; // Seçimi sıfırla ki bir sonraki işlem ilk boş kutuya devam edebilsin
            }
        }

        // "Paleti Temizle" Butonu: Tüm kutucukları sıfırlar
        private void btnPaletiTemizle_Click(object sender, EventArgs e)
        {
            DialogResult secim = MessageBox.Show("Özel renk paletini tamamen temizlemek istiyor musunuz?",
                                                 "Paleti Temizle", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (secim == DialogResult.Yes)
            {
                for (int i = 0; i < 48; i++)
                {
                    if (ozelRenkKutulari[i] != null)
                    {
                        ozelRenkKutulari[i].BackColor = Color.White;
                    }
                }
                secilenKutuIndex = -1;
            }
        }

        // HSV renk uzayından standart RGB'ye matematiksel dönüşüm
        private Color HsvToRgb(double h, double s, double v)
        {
            int hi = (int)Math.Floor(h / 60.0) % 6;
            double f = (h / 60.0) - Math.Floor(h / 60.0);

            v = v * 255.0;
            byte vVal = (byte)v;
            byte p = (byte)(v * (1.0 - s));
            byte q = (byte)(v * (1.0 - (f * s)));
            byte t = (byte)(v * (1.0 - ((1.0 - f) * s)));

            switch (hi)
            {
                case 0: return Color.FromArgb(vVal, t, p);
                case 1: return Color.FromArgb(q, vVal, p);
                case 2: return Color.FromArgb(p, vVal, t);
                case 3: return Color.FromArgb(p, q, vVal);
                case 4: return Color.FromArgb(t, p, vVal);
                default: return Color.FromArgb(vVal, p, q);
            }
        }

        // Ödev 9'a Geri Dön Linki
        private void lnkGeri_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta2Odev10_FormClosing;

            Hafta2Odev9 odev9 = new Hafta2Odev9();
            odev9.StartPosition = FormStartPosition.Manual;
            odev9.Location = this.Location;
            odev9.Size = this.Size;
            odev9.FormBorderStyle = this.FormBorderStyle;

            odev9.Show();
            this.Close();
        }

        // Ana Menüye (Form1) Dön Linki
        private void lnkAnaMenu_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta2Odev10_FormClosing;

            Form1 anaForm = (Form1)Application.OpenForms["Form1"];
            if (anaForm != null)
            {
                anaForm.StartPosition = FormStartPosition.Manual;
                anaForm.Location = this.Location;
                anaForm.Size = this.Size;
                anaForm.Show();
                anaForm.BringToFront();
            }
            else
            {
                Form1 yeniForm = new Form1();
                yeniForm.StartPosition = FormStartPosition.Manual;
                yeniForm.Location = this.Location;
                yeniForm.Size = this.Size;
                yeniForm.Show();
            }
            this.Close();
        }
    }
}