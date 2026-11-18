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

        public Hafta1Odev8()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            this.StartPosition = FormStartPosition.CenterScreen;

            // X butonu kapatma olayını bağlıyoruz:
            this.FormClosing += Hafta1Odev8_FormClosing;

            if (cmbMod != null && cmbMod.Items.Count > 0)
                cmbMod.SelectedIndex = 0; // Varsayılan: Gri-Ton

            // Olay bağlantıları
            if (btnResimSec != null) btnResimSec.Click += btnResimSec_Click;
            if (btnUygula != null) btnUygula.Click += btnUygula_Click;
            if (lnkGeri != null) lnkGeri.LinkClicked += lnkGeri_LinkClicked;
            if (lnkAnaMenu != null) lnkAnaMenu.LinkClicked += lnkAnaMenu_LinkClicked;
        }

        // X butonuna basıldığında Form1'e dönen metot:
        private void Hafta1Odev8_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Form1 anaForm = (Form1)Application.OpenForms["Form1"];

                if (anaForm != null)
                {
                    anaForm.StartPosition = FormStartPosition.Manual;
                    anaForm.Location = this.Location;
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

        // Resim Seçme
        private void btnResimSec_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    yuklenenMat?.Dispose();
                    yuklenenMat = Cv2.ImRead(ofd.FileName, ImreadModes.Color);

                    if (yuklenenMat.Empty())
                    {
                        MessageBox.Show("Resim yüklenemedi!");
                        return;
                    }

                    // Başlangıçta 8-bit olan ilk kutuya orijinali yerleştir
                    picBit8.Image?.Dispose();
                    picBit8.Image = BitmapConverter.ToBitmap(yuklenenMat);
                }
            }
        }

        // 8 Farklı Bit Seviyesini Tek Seferde Hesaplayıp Gösterme
        private void btnUygula_Click(object sender, EventArgs e)
        {
            if (yuklenenMat == null)
            {
                MessageBox.Show("Lütfen önce bir resim seçin!");
                return;
            }

            bool griMod = (cmbMod != null && cmbMod.SelectedIndex == 0);

            // PictureBox'ları sırayla diziye alıyoruz (8 bit -> 1 bit)
            PictureBox[] kutular = new PictureBox[]
            {
                picBit8, picBit7, picBit6, picBit5,
                picBit4, picBit3, picBit2, picBit1
            };

            // 8'den 1'e kadar her bit için görüntüyü oluştur
            for (int bit = 8; bit >= 1; bit--)
            {
                PictureBox hedefKutu = kutular[8 - bit];
                if (hedefKutu == null) continue;

                if (griMod)
                {
                    using (Mat gri = new Mat())
                    {
                        Cv2.CvtColor(yuklenenMat, gri, ColorConversionCodes.BGR2GRAY);
                        using (Mat sonucGri = new Mat(gri.Size(), MatType.CV_8UC1))
                        {
                            for (int y = 0; y < gri.Height; y++)
                            {
                                for (int x = 0; x < gri.Width; x++)
                                {
                                    byte val = gri.At<byte>(y, x);
                                    sonucGri.Set<byte>(y, x, Quantize(val, bit));
                                }
                            }

                            hedefKutu.Image?.Dispose();
                            hedefKutu.Image = BitmapConverter.ToBitmap(sonucGri);
                        }
                    }
                }
                else
                {
                    // Renkli Mod (RGB kanallarının tümüne uygulama)
                    using (Mat sonucRenkli = new Mat(yuklenenMat.Size(), MatType.CV_8UC3))
                    {
                        for (int y = 0; y < yuklenenMat.Height; y++)
                        {
                            for (int x = 0; x < yuklenenMat.Width; x++)
                            {
                                Vec3b bgr = yuklenenMat.At<Vec3b>(y, x);
                                bgr.Item0 = Quantize(bgr.Item0, bit); // Mavi
                                bgr.Item1 = Quantize(bgr.Item1, bit); // Yeşil
                                bgr.Item2 = Quantize(bgr.Item2, bit); // Kırmızı
                                sonucRenkli.Set(y, x, bgr);
                            }
                        }

                        hedefKutu.Image?.Dispose();
                        hedefKutu.Image = BitmapConverter.ToBitmap(sonucRenkli);
                    }
                }
            }
        }

        // Piksel değerini m-bit aralığına indirgeyip normalize etme
        private byte Quantize(byte deger, int bit)
        {
            if (bit >= 8) return deger;
            if (bit <= 0) return 0;

            int kaydirma = 8 - bit;
            int maxYeniSeviye = (1 << bit) - 1; // 2^bit - 1

            int indirgenmis = deger >> kaydirma;
            int sonuc = (indirgenmis * 255) / maxYeniSeviye;

            return (byte)sonuc;
        }

        // Ödev 7'ye Geri Dön Linki
        private void lnkGeri_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta1Odev8_FormClosing;

            Form odev7 = Application.OpenForms["Hafta1Odev7"];
            if (odev7 != null)
            {
                odev7.Show();
            }
            else
            {
                Hafta1Odev7 yeniOdev7 = new Hafta1Odev7();
                yeniOdev7.StartPosition = FormStartPosition.Manual;
                yeniOdev7.Location = this.Location;
                yeniOdev7.Size = this.Size;
                yeniOdev7.Show();
            }

            this.Close();
        }

        // Ana Menüye Dön
        private void lnkAnaMenu_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.FormClosing -= Hafta1Odev8_FormClosing;

            Form1 anaForm = (Form1)Application.OpenForms["Form1"];
            if (anaForm != null)
            {
                anaForm.StartPosition = FormStartPosition.Manual;
                anaForm.Location = this.Location;
                anaForm.Show();
                anaForm.BringToFront();
            }
            else
            {
                Form1 yeniForm = new Form1();
                yeniForm.StartPosition = FormStartPosition.Manual;
                yeniForm.Location = this.Location;
                yeniForm.Show();
            }

            this.Close();
        }
    }
}