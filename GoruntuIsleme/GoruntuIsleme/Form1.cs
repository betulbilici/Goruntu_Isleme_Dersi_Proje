using System;
using System.Drawing;
using System.Windows.Forms;

namespace GoruntuIsleme
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            this.ClientSize = new Size(1250, 750);
            this.MinimumSize = new Size(1050, 680);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Görüntü İşleme Ders Projesi - Ana Menü";

            this.Load += Form1_Load;
            this.Resize += (s, e) => MenuyuHizala();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ModernLinkLabelMenusuAyarla();
        }

        private void ModernLinkLabelMenusuAyarla()
        {
            LinkLabel[] linkler = new LinkLabel[]
            {
                linkLabel1, linkLabel2, linkLabel3, linkLabel4, linkLabel5,
                linkLabel6, linkLabel7, linkLabel8, linkLabel9, linkLabel10
            };

            Color normalZemin = Color.FromArgb(210, 18, 32, 54);   // Belirgin koyu lacivert kart
            Color normalYazi = Color.FromArgb(230, 245, 255);     // Net beyaz / buz mavisi
            Color hoverZemin = Color.FromArgb(240, 14, 165, 233); // Tema ile uyumlu canlı gökyüzü mavisi
            Color hoverYazi = Color.White;

            for (int i = 0; i < linkler.Length; i++)
            {
                LinkLabel lnk = linkler[i];
                if (lnk == null) continue;

                lnk.Text = $"  ▶  Hafta {i + 1}";
                lnk.Tag = i + 1;

                lnk.AutoSize = false;
                lnk.Size = new Size(165, 34);
                lnk.TextAlign = ContentAlignment.MiddleLeft;

                lnk.Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold);
                lnk.BackColor = normalZemin;
                lnk.LinkColor = normalYazi;
                lnk.ActiveLinkColor = Color.FromArgb(0, 255, 200);
                lnk.VisitedLinkColor = normalYazi;
                lnk.LinkBehavior = LinkBehavior.NeverUnderline;

                lnk.MouseEnter -= Lnk_MouseEnter;
                lnk.MouseEnter += Lnk_MouseEnter;

                lnk.MouseLeave -= Lnk_MouseLeave;
                lnk.MouseLeave += Lnk_MouseLeave;

                lnk.Click -= HaftaLinki_Click;
                lnk.Click += HaftaLinki_Click;
            }

            MenuyuHizala();

            void Lnk_MouseEnter(object s, EventArgs ev)
            {
                LinkLabel l = (LinkLabel)s;
                l.BackColor = hoverZemin;
                l.LinkColor = hoverYazi;
                l.Cursor = Cursors.Hand;
            }

            void Lnk_MouseLeave(object s, EventArgs ev)
            {
                LinkLabel l = (LinkLabel)s;
                l.BackColor = normalZemin;
                l.LinkColor = normalYazi;
            }
        }

        private void MenuyuHizala()
        {
            LinkLabel[] linkler = new LinkLabel[]
            {
                linkLabel1, linkLabel2, linkLabel3, linkLabel4, linkLabel5,
                linkLabel6, linkLabel7, linkLabel8, linkLabel9, linkLabel10
            };

            int baslangicX = this.ClientSize.Width - 190;
            int baslangicY = 140;
            int aralikY = 40;

            for (int i = 0; i < linkler.Length; i++)
            {
                if (linkler[i] != null)
                {
                    linkler[i].Location = new Point(baslangicX, baslangicY + (i * aralikY));
                }
            }
        }

        private void HaftaLinki_Click(object sender, EventArgs e)
        {
            LinkLabel tiklanan = sender as LinkLabel;
            if (tiklanan?.Tag == null) return;

            int haftaNo = Convert.ToInt32(tiklanan.Tag);

            switch (haftaNo)
            {
                case 1:
                    FormGezintiYardimcisi.FormGecisYap(this, new Hafta1Odev1());
                    break;
                case 2:
                    FormGezintiYardimcisi.FormGecisYap(this, new Hafta2Odev1());
                    break;
                case 3:
                    FormGezintiYardimcisi.FormGecisYap(this, new Hafta3Odev1());
                    break;
                case 8:
                    FormGezintiYardimcisi.FormGecisYap(this, new Hafta8Odev1());
                    break;
                case 9:
                    FormGezintiYardimcisi.FormGecisYap(this, new Hafta9Odev1());
                    break;
                default:
                    MessageBox.Show($"Hafta {haftaNo} ödevi henüz eklenmedi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }
        }
    }
}