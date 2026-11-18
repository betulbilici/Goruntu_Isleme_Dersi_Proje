using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace GoruntuIsleme
{
    public partial class Hafta3Odev12 : Form
    {
        private Bitmap orijinalBmp = null;

        public Hafta3Odev12()
        {
            InitializeComponent();
            this.FormClosing += Hafta3Odev12_FormClosing;

            if (btnResimSec != null) btnResimSec.Click += btnResimSec_Click;
            if (btnIslemA != null) btnIslemA.Click += btnIslemA_Click;
            if (btnIslemB != null) btnIslemB.Click += btnIslemB_Click;
            if (btnSifirla != null) btnSifirla.Click += btnSifirla_Click;
            if (lnkGeri != null) lnkGeri.LinkClicked += lnkGeri_LinkClicked;
            if (lnkSonraki != null) lnkSonraki.LinkClicked += lnkSonraki_LinkClicked;
        }

        private void Hafta3Odev12_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Form1 anaForm = (Form1)Application.OpenForms["Form1"];
                if (anaForm != null)
                {
                    anaForm.Show();
                    anaForm.BringToFront();
                }
                else
                {
                    new Form1().Show();
                }
            }
        }

        private void btnResimSec_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    orijinalBmp?.Dispose();
                    using (Bitmap temp = new Bitmap(ofd.FileName))
                    {
                        orijinalBmp = new Bitmap(temp);
                    }

                    if (picOrijinal != null)
                    {
                        picOrijinal.Image?.Dispose();
                        picOrijinal.SizeMode = PictureBoxSizeMode.Zoom;
                        picOrijinal.Image = new Bitmap(orijinalBmp);
                    }

                    if (picA != null) picA.Image = null;
                    if (picB != null) picB.Image = null;

                    if (lblBilgi != null)
                    {
                        lblBilgi.Text = $"Resim Yüklendi: {orijinalBmp.Width}x{orijinalBmp.Height} px\n" +
                                        $"Dönme açısını belirleyip 'a' veya 'b' butonuna tıklayınız.";
                    }
                }
            }
        }

        private void btnSifirla_Click(object sender, EventArgs e)
        {
            if (orijinalBmp == null) return;

            if (picA != null) picA.Image = null;
            if (picB != null) picB.Image = null;
            numAci.Value = 30;

            if (lblBilgi != null)
            {
                lblBilgi.Text = "Sonuç kutuları temizlendi ve açı 30° olarak sıfırlandı.";
            }
        }

        // --- a) GEOMETRİK DÖNÜŞÜMLE DÖNDÜRME VE ALIAS / DELİK DOLDURMA ---
        private void btnIslemA_Click(object sender, EventArgs e)
        {
            if (orijinalBmp == null)
            {
                MessageBox.Show("Lütfen önce bir resim seçiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.Cursor = Cursors.WaitCursor;

            double aciDerece = (double)numAci.Value;
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
                        }
                    }
                }
            }

            cikisBmp.Dispose();

            if (picA != null)
            {
                picA.Image?.Dispose();
                picA.SizeMode = PictureBoxSizeMode.Zoom;
                picA.Image = filtrelenmisBmp;
            }

            if (lblBilgi != null)
            {
                lblBilgi.Text = "a) İLERİ HARİTALAMA İLE OLUŞAN BOŞLUKLAR 3x3 KOMŞU PİKSEL ORTALAMASIYLA DOLDURULDU.\n" +
                                "EN İYİ ALGORİTMA: TERS DÖNÜŞÜM (INVERSE MAPPING) VE BİLİNEAR İNTERPOLASYONDUR.";
            }

            this.Cursor = Cursors.Default;
        }

        // --- b) RESMİN ORTASINA GÖRE DEĞİL, SOL ÜST KÖŞESİNE (0,0) GÖRE EĞEREK DÖNDÜRME ---
        // --- b) RESMİN KÖŞESİNDEN YANA VE AŞAĞI KAYDIRARAK (SHEARING İLE) DÖNDÜRME ---
        private void btnIslemB_Click(object sender, EventArgs e)
        {
            if (orijinalBmp == null)
            {
                MessageBox.Show("Lütfen önce bir resim seçiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.Cursor = Cursors.WaitCursor;

            double aciDerece = (double)numAci.Value;
            if (Math.Abs(aciDerece % 180) == 90)
            {
                MessageBox.Show("Kaydırma yöntemi 90 derece için uygun değildir. Lütfen 15, 30, 45 gibi bir açı giriniz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Cursor = Cursors.Default;
                return;
            }

            double radyan = aciDerece * Math.PI / 180.0;
            int w = orijinalBmp.Width;
            int h = orijinalBmp.Height;

            // 1. Hocanın slaytındaki yana ve aşağı eğme katsayıları
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

            // Köşeden taşmayı önleyen merkez/ofset dengelemesi
            double cxDst = hedefW / 2.0;
            double cyDst = hedefH / 2.0;
            double cxSrc = w / 2.0;
            double cySrc = h / 2.0;

            // 3. Yana ve aşağı kaydırmanın ters adımları (Piksel deliklerini engellemek için)
            for (int yd = 0; yd < hedefH; yd++)
            {
                double yRel = yd - cyDst;
                for (int xd = 0; xd < hedefW; xd++)
                {
                    double xRel = xd - cxDst;

                    // Adım 3'ün tersi: Son yana kaydırmadan geri gel
                    double x2 = xRel - alpha * yRel;

                    // Adım 2'nin tersi: Aşağı kaydırmadan geri gel (Yana ve aşağı kaydırma kuralı)
                    double y1 = yRel - beta * x2;

                    // Adım 1'in tersi: İlk yana kaydırmadan geri gel
                    double x0 = x2 - alpha * y1;

                    int xs = (int)Math.Round(x0 + cxSrc);
                    int ys = (int)Math.Round(y1 + cySrc);

                    if (xs >= 0 && xs < w && ys >= 0 && ys < h)
                    {
                        cikisBmp.SetPixel(xd, yd, orijinalBmp.GetPixel(xs, ys));
                    }
                }
            }

            if (picB != null)
            {
                picB.Image?.Dispose();
                picB.SizeMode = PictureBoxSizeMode.Zoom;
                picB.Image = cikisBmp;
            }

            if (lblBilgi != null)
            {
                lblBilgi.Text = "b) RESİM KÖŞESİNDEN AÇIYA BAĞLI YANA VE AŞAĞI KAYDIRMA (3-SHEAR) FORMÜLLERİYLE DÖNDÜRÜLDÜ.\n" +
                                "TAŞMAYI ÖNLEME: HEDEF TUVAL BOYUTU SINIR AÇILARIYLA HESAPLANDI VE OFSETLE MERKEZLENDİ.";
            }

            this.Cursor = Cursors.Default;
        }

        private void lnkGeri_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta3Odev12_FormClosing;
            Hafta3Odev11 odev11 = new Hafta3Odev11();
            odev11.Show();
            this.Close();
        }

        private void lnkSonraki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta3Odev12_FormClosing;
            Hafta3Odev13 odev13 = new Hafta3Odev13();
            odev13.Show();
            this.Close();
        }
    }
}