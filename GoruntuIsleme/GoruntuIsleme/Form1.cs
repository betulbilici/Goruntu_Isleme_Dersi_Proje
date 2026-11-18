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
            this.Load += Form1_Load;
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

            // Konumlandırma ayarları:
            // Başlangıç Y değerini 130 yaparak menüyü biraz daha aşağı kaydırdık
            int baslangicX = this.ClientSize.Width - 185;
            int baslangicY = 130;
            int aralikY = 38;

            // Tema renkleri (Arka plana kontrast sağlayan koyu ve canlı tonlar)
            Color normalZemin = Color.FromArgb(210, 18, 32, 54);     // Belirgin koyu lacivert kart
            Color normalYazi = Color.FromArgb(230, 245, 255);       // Net beyaz / buz mavisi
            Color hoverZemin = Color.FromArgb(240, 0, 140, 230);     // Üzerine gelince canlı neon mavi
            Color hoverYazi = Color.White;

            for (int i = 0; i < linkler.Length; i++)
            {
                LinkLabel lnk = linkler[i];
                if (lnk == null) continue;

                lnk.Text = $"  ▶  Hafta {i + 1}";
                lnk.Tag = i + 1;

                // Boyut ve Konum
                lnk.Location = new Point(baslangicX, baslangicY + (i * aralikY));
                lnk.AutoSize = false;
                lnk.Size = new Size(160, 32);
                lnk.TextAlign = ContentAlignment.MiddleLeft;

                // Tipografi ve Stil
                lnk.Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold);
                lnk.BackColor = normalZemin;
                lnk.LinkColor = normalYazi;
                lnk.ActiveLinkColor = Color.FromArgb(0, 255, 200);
                lnk.VisitedLinkColor = normalYazi;
                lnk.LinkBehavior = LinkBehavior.NeverUnderline;

                // Hover Efektleri
                lnk.MouseEnter += (s, ev) =>
                {
                    LinkLabel l = (LinkLabel)s;
                    l.BackColor = hoverZemin;
                    l.LinkColor = hoverYazi;
                    l.Cursor = Cursors.Hand;
                };

                lnk.MouseLeave += (s, ev) =>
                {
                    LinkLabel l = (LinkLabel)s;
                    l.BackColor = normalZemin;
                    l.LinkColor = normalYazi;
                };

                lnk.Click += HaftaLinki_Click;
            }
        }

        private void HaftaLinki_Click(object sender, EventArgs e)
        {
            LinkLabel tiklanan = sender as LinkLabel;
            if (tiklanan?.Tag == null) return;

            int haftaNo = Convert.ToInt32(tiklanan.Tag);

            if (haftaNo == 1)
            {
                Hafta1Odev1 hafta1Form = new Hafta1Odev1();
                hafta1Form.StartPosition = FormStartPosition.Manual;
                hafta1Form.Location = this.Location;
                hafta1Form.Size = this.Size;
                hafta1Form.Show();
                this.Hide();
            }
            else if (haftaNo == 2)
            {
                Hafta2Odev1 hafta2Form = new Hafta2Odev1();
                hafta2Form.StartPosition = FormStartPosition.Manual;
                hafta2Form.Location = this.Location;
                hafta2Form.Size = this.Size;
                hafta2Form.Show();
                this.Hide();
            }
            else if (haftaNo == 3)
            {
                Hafta3Odev1 hafta3Form = new Hafta3Odev1();
                hafta3Form.StartPosition = FormStartPosition.Manual;
                hafta3Form.Location = this.Location;
                hafta3Form.Size = this.Size;
                hafta3Form.Show();
                this.Hide();
            }
            else if (haftaNo == 8)
            {
                Hafta8Odev1 hafta8Form = new Hafta8Odev1();
                FormGezintiYardimcisi.FormGecisYap(this, hafta8Form);
            }
            else if (haftaNo == 9)
            {
                Hafta9Odev1 hafta9Form = new Hafta9Odev1();
                FormGezintiYardimcisi.FormGecisYap(this, hafta9Form);
            }
            else
            {
                MessageBox.Show($"Hafta {haftaNo} ödevi henüz eklenmedi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}