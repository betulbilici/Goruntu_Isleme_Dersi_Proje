using System;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace GoruntuIsleme
{
    public partial class Hafta1Odev1 : Form
    {
        public Hafta1Odev1()
        {
            InitializeComponent();

            // Buton tıklamasını kod üzerinden garantiye alıyoruz:
            this.btnOlustur.Click += new EventHandler(this.btnOlustur_Click);

            // PictureBox'ın boyutu ve çerçevesi:
            this.picGoruntu.SizeMode = PictureBoxSizeMode.Zoom;
            this.picGoruntu.BorderStyle = BorderStyle.FixedSingle;
        }

        private void btnOlustur_Click(object sender, EventArgs e)
        {
            if (numGenislik == null || picGoruntu == null) return;

            // Sorudaki '255' sayısını değişkene (N) bağlıyoruz:
            int N = (int)numGenislik.Value;

            // Sütun ve satır sayısı: 0'dan N'e kadar -> (N + 1)
            int genislik = N + 1;
            int yukseklik = N + 1;

            // OpenCvSharp ile CV_8UC1 (Gri seviye) matris oluşturma
            using (Mat matris = new Mat(yukseklik, genislik, MatType.CV_8UC1))
            {
                for (int y = 0; y < yukseklik; y++)
                {
                    for (int x = 0; x < genislik; x++)
                    {
                        // Sorudaki matris: Her sütundaki değer doğrudan sütun indeksidir (x)!
                        // Asla (x * 255) / N gibi bir oranlama YOKTUR.
                        matris.Set<byte>(y, x, (byte)x);
                    }
                }

                // Matrisi PictureBox'a aktarıyoruz
                picGoruntu.Image?.Dispose();
                picGoruntu.Image = BitmapConverter.ToBitmap(matris);
            }
        }

        private void lnkOdev2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Hafta1Odev2 form2 = new Hafta1Odev2();
            form2.StartPosition = FormStartPosition.CenterScreen;
            form2.Show();

            // Ödev 1'i gizliyoruz (kapatmıyoruz ki geri dönebilelim)
            this.Hide();

            // Eğer Ödev 2 kapatılırsa (sağ üst X'ten), Ödev 1 de kapansın:
            form2.FormClosed += (s, args) =>
            {
                // Eğer kullanıcı geriye dönmediyse ve formu kapattıysa Form1'e dönsün
                if (!this.Visible) this.Close();
            };
        }
    }
}