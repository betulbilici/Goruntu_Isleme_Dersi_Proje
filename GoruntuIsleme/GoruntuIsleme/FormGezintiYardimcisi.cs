using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GoruntuIsleme
{
    /// <summary>
    /// Yuvarlatılmış köşeli ve ince açık mavi kenarlıklı modern kart paneli.
    /// </summary>
    public class ModernKartPanel : Panel
    {
        public int KoseYaricapi { get; set; } = 14;
        public Color KenarlikRengi { get; set; } = Color.FromArgb(219, 234, 254); // #DBEAFE
        public Color KartArkaPlan { get; set; } = Color.White;

        public ModernKartPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            BackColor = Color.White;
        }

        private Color DisZeminRengiBul()
        {
            Control p = Parent;
            while (p != null)
            {
                if (p.BackColor != Color.Transparent && p.BackColor.A == 255)
                    return p.BackColor;
                p = p.Parent;
            }
            return FormGezintiYardimcisi.TemaZemin;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Köşe dışındaki alanları ebeveyn zemin rengiyle temizle (köşe artığını önler)
            e.Graphics.Clear(DisZeminRengiBul());
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            Rectangle rect = new Rectangle(1, 1, Width - 3, Height - 3);
            if (rect.Width <= 0 || rect.Height <= 0) return;

            using GraphicsPath yol = YuvarlakKoseYolu(rect, KoseYaricapi);
            using SolidBrush firca = new SolidBrush(KartArkaPlan);
            using Pen kalem = new Pen(KenarlikRengi, 1.3f);

            e.Graphics.FillPath(firca, yol);
            e.Graphics.DrawPath(kalem, yol);
        }

        public static GraphicsPath YuvarlakKoseYolu(Rectangle r, int yaricap)
        {
            GraphicsPath path = new GraphicsPath();
            int cap = Math.Min(yaricap * 2, Math.Min(r.Width, r.Height));
            if (cap <= 0)
            {
                path.AddRectangle(r);
                return path;
            }
            path.AddArc(r.X, r.Y, cap, cap, 180, 90);
            path.AddArc(r.Right - cap, r.Y, cap, cap, 270, 90);
            path.AddArc(r.Right - cap, r.Bottom - cap, cap, cap, 0, 90);
            path.AddArc(r.X, r.Bottom - cap, cap, cap, 90, 90);
            path.CloseFigure();
            return path;
        }
    }

    /// <summary>
    /// Yuvarlak hap (pill) şeklinde modern bilgi rozeti (Badge).
    /// </summary>
    public class ModernHapRozet : Control
    {
        public Color RozetArkaPlan { get; set; } = Color.FromArgb(224, 242, 254); // #E0F2FE
        public Color RozetYaziRengi { get; set; } = Color.FromArgb(2, 132, 199);  // #0284C7

        public ModernHapRozet()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            Font = new Font("Segoe UI Semibold", 8.8f, FontStyle.Bold);
            Height = 24;
            Width = 76;
        }

        private Color DisZeminRengiBul()
        {
            Control p = Parent;
            while (p != null)
            {
                if (p is ModernKartPanel mkp) return mkp.KartArkaPlan;
                if (p.BackColor != Color.Transparent && p.BackColor.A == 255) return p.BackColor;
                p = p.Parent;
            }
            return Color.White;
        }

        public void MetinGuncelle(string yeniMetin)
        {
            Text = yeniMetin;
            using Bitmap tmp = new Bitmap(1, 1);
            using Graphics g = Graphics.FromImage(tmp);
            SizeF boyut = g.MeasureString(yeniMetin, Font);
            int yeniGenislik = Math.Max(52, (int)boyut.Width + 20);
            if (Parent != null && (Anchor & AnchorStyles.Right) == AnchorStyles.Right)
            {
                Location = new Point(Parent.Width - yeniGenislik - 2, Top);
            }
            Width = yeniGenislik;
            Invalidate();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            e.Graphics.Clear(DisZeminRengiBul());
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            Rectangle rect = new Rectangle(1, 1, Width - 3, Height - 3);
            if (rect.Width <= 0 || rect.Height <= 0) return;

            using GraphicsPath yol = ModernKartPanel.YuvarlakKoseYolu(rect, rect.Height / 2);
            using SolidBrush arkaFirca = new SolidBrush(RozetArkaPlan);
            e.Graphics.FillPath(arkaFirca, yol);

            TextRenderer.DrawText(
                e.Graphics,
                Text,
                Font,
                rect,
                RozetYaziRengi,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
        }
    }

    /// <summary>
    /// Windows'un standart Button köşe izlerini tamamen ortadan kaldıran,
    /// Control tabanlı pürüzsüz yuvarlak hap (pill) buton.
    /// </summary>
    public class ModernHapButon : Control
    {
        public Color NormalArkaPlan { get; set; } = Color.FromArgb(14, 165, 233); // #0EA5E9
        public Color HoverArkaPlan { get; set; } = Color.FromArgb(2, 132, 199);   // #0284C7
        public Color YaziRengi { get; set; } = Color.White;
        public Color KenarlikRengi { get; set; } = Color.Transparent;
        private bool uzerindeMi = false;

        public ModernHapButon()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            Font = new Font("Segoe UI Semibold", 9.2f, FontStyle.Bold);
            Cursor = Cursors.Hand;
            Size = new Size(120, 36);
        }

        private Color DisZeminRengiBul()
        {
            Control p = Parent;
            while (p != null)
            {
                if (p is ModernKartPanel mkp) return mkp.KartArkaPlan;
                if (p.BackColor != Color.Transparent && p.BackColor.A == 255) return p.BackColor;
                p = p.Parent;
            }
            return Color.White;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            uzerindeMi = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            uzerindeMi = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Köşelerde hiçbir siyah/mavi dikdörtgen artığı kalmaması için dış zemini temizle
            e.Graphics.Clear(DisZeminRengiBul());
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            Rectangle rect = new Rectangle(1, 1, Width - 3, Height - 3);
            if (rect.Width <= 0 || rect.Height <= 0) return;

            using GraphicsPath yol = ModernKartPanel.YuvarlakKoseYolu(rect, rect.Height / 2);
            using SolidBrush firca = new SolidBrush(uzerindeMi ? HoverArkaPlan : NormalArkaPlan);
            e.Graphics.FillPath(firca, yol);

            if (KenarlikRengi != Color.Transparent)
            {
                using Pen kalem = new Pen(KenarlikRengi, 1.3f);
                e.Graphics.DrawPath(kalem, yol);
            }

            TextRenderer.DrawText(
                e.Graphics,
                Text,
                Font,
                rect,
                YaziRengi,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
        }
    }

    /// <summary>
    /// Windows'un eski TrackBar kontrolü yerine geçen, GDI+ ile çizilmiş
    /// ince yuvarlak hap izli ve beyaz halkalı mavi tutamaçlı modern sürgü.
    /// </summary>
    public class ModernSurgu : Control
    {
        private int minDeger = 0;
        private int maxDeger = 100;
        private int deger = 50;
        private bool surukleniyor = false;

        public Color IzArkaRengi { get; set; } = Color.FromArgb(224, 242, 254); // #E0F2FE
        public Color DolguRengi { get; set; } = Color.FromArgb(14, 165, 233);   // #0EA5E9
        public Color TutamacRengi { get; set; } = Color.FromArgb(2, 132, 199);  // #0284C7
        public int TickFrequency { get; set; } = 10;

        public event EventHandler Scroll;
        public event EventHandler ValueChanged;

        public int Minimum
        {
            get => minDeger;
            set
            {
                minDeger = value;
                if (maxDeger <= minDeger) maxDeger = minDeger + 1;
                if (deger < minDeger) deger = minDeger;
                Invalidate();
            }
        }

        public int Maximum
        {
            get => maxDeger;
            set
            {
                maxDeger = value;
                if (minDeger >= maxDeger) minDeger = maxDeger - 1;
                if (deger > maxDeger) deger = maxDeger;
                Invalidate();
            }
        }

        public int Value
        {
            get => deger;
            set
            {
                int alt = Math.Min(minDeger, maxDeger);
                int ust = Math.Max(minDeger, maxDeger);
                int yeni = Math.Clamp(value, alt, ust);
                if (deger != yeni)
                {
                    deger = yeni;
                    Invalidate();
                    ValueChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public ModernSurgu()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            BackColor = Color.White;
            Height = 26;
            Cursor = Cursors.Hand;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                surukleniyor = true;
                KonumdanDegerGuncelle(e.X);
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (surukleniyor)
            {
                KonumdanDegerGuncelle(e.X);
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            surukleniyor = false;
            base.OnMouseUp(e);
        }

        private void KonumdanDegerGuncelle(int mouseX)
        {
            int kenarBosluk = 10;
            int kullanilabilirGenislik = Math.Max(1, Width - kenarBosluk * 2);
            double oran = Math.Clamp((double)(mouseX - kenarBosluk) / kullanilabilirGenislik, 0.0, 1.0);
            int yeniDeger = minDeger + (int)Math.Round(oran * (maxDeger - minDeger));
            if (yeniDeger != deger)
            {
                deger = yeniDeger;
                Invalidate();
                Scroll?.Invoke(this, EventArgs.Empty);
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            Color zemin = Parent?.BackColor ?? Color.White;
            if (zemin == Color.Transparent) zemin = Color.White;
            e.Graphics.Clear(zemin);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            int kenarBosluk = 10;
            int izYukseklik = 6;
            int izY = (Height - izYukseklik) / 2;
            int izGenislik = Math.Max(4, Width - kenarBosluk * 2);

            Rectangle izRect = new Rectangle(kenarBosluk, izY, izGenislik, izYukseklik);
            using (GraphicsPath izYol = ModernKartPanel.YuvarlakKoseYolu(izRect, 3))
            using (SolidBrush izFirca = new SolidBrush(IzArkaRengi))
            {
                e.Graphics.FillPath(izFirca, izYol);
            }

            double oran = (double)(deger - minDeger) / Math.Max(1, maxDeger - minDeger);
            int dolguGenislik = Math.Max(4, (int)(izGenislik * oran));
            Rectangle dolguRect = new Rectangle(kenarBosluk, izY, dolguGenislik, izYukseklik);
            using (GraphicsPath dolguYol = ModernKartPanel.YuvarlakKoseYolu(dolguRect, 3))
            using (SolidBrush dolguFirca = new SolidBrush(DolguRengi))
            {
                e.Graphics.FillPath(dolguFirca, dolguYol);
            }

            int thumbX = kenarBosluk + (int)(izGenislik * oran);
            int thumbY = Height / 2;
            int r = 8;

            Rectangle thumbRect = new Rectangle(thumbX - r, thumbY - r, r * 2, r * 2);
            using (SolidBrush tutamacFirca = new SolidBrush(TutamacRengi))
            using (Pen beyazHalka = new Pen(Color.White, 2.5f))
            {
                e.Graphics.FillEllipse(tutamacFirca, thumbRect);
                e.Graphics.DrawEllipse(beyazHalka, thumbRect);
            }
        }
    }

    public static class FormGezintiYardimcisi
    {
        // Profesyonel Açık Mavi Tema Paleti
        public static readonly Color TemaZemin = Color.FromArgb(244, 248, 253);       // #F4F8FD yumuşak buz mavisi-beyaz
        public static readonly Color AcikMaviZemin = Color.FromArgb(240, 249, 255);   // #F0F9FF açık mavi kart/banner zemini
        public static readonly Color AcikMaviKenar = Color.FromArgb(186, 230, 253);   // #BAE6FD zarif açık mavi kenarlık
        public static readonly Color KartKenar = Color.FromArgb(219, 234, 254);       // #DBEAFE
        public static readonly Color BirincilMavi = Color.FromArgb(14, 165, 233);     // #0EA5E9 profesyonel gökyüzü mavisi
        public static readonly Color KoyuMaviYazi = Color.FromArgb(3, 105, 161);      // #0369A1
        public static readonly Color KoyuYazi = Color.FromArgb(15, 23, 42);           // #0F172A
        public static readonly Color AcikYazi = Color.FromArgb(100, 116, 139);        // #64748B
        public static readonly Color IkincilGri = Color.FromArgb(240, 249, 255);

        public static void FormuHazirla(Form form)
        {
            form.AutoScaleMode = AutoScaleMode.None;
            form.ClientSize = new Size(1250, 750);
            form.MinimumSize = new Size(1050, 680);
            form.StartPosition = FormStartPosition.CenterScreen;
            form.BackColor = TemaZemin;

            form.FormClosing += (s, e) =>
            {
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    AnaFormaDon(form);
                }
            };
        }

        public static Panel UstBarOlustur(
            Form form,
            string baslik,
            string oncekiYazi,
            Action oncekiTikla,
            string sonrakiYazi,
            Action sonrakiTikla,
            bool sonrakiYesil = false,
            string haftaRozetMetni = null)
        {
            Panel pnlUst = new Panel
            {
                Dock = DockStyle.Top,
                Height = 62,
                BackColor = Color.White
            };

            pnlUst.Paint += (s, e) =>
            {
                using Pen altCizgi = new Pen(KartKenar, 1.2f);
                e.Graphics.DrawLine(altCizgi, 0, pnlUst.Height - 1, pnlUst.Width, pnlUst.Height - 1);
            };

            string rozetYazi = haftaRozetMetni;
            string temizBaslik = baslik;
            if (rozetYazi == null && baslik.Contains("|"))
            {
                string[] parcalar = baslik.Split('|', 2);
                rozetYazi = parcalar[0].Trim().Replace("-", "•");
                temizBaslik = parcalar[1].Trim();
            }

            int baslikX = 22;
            if (!string.IsNullOrEmpty(rozetYazi))
            {
                ModernHapRozet rozet = new ModernHapRozet
                {
                    Location = new Point(20, 19),
                    Height = 25
                };
                rozet.MetinGuncelle(rozetYazi);
                pnlUst.Controls.Add(rozet);
                baslikX = rozet.Right + 12;
            }

            Label lblBaslik = new Label
            {
                Text = temizBaslik,
                UseMnemonic = false,
                ForeColor = KoyuYazi,
                Font = new Font("Segoe UI Semibold", 11.5f, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(baslikX, 20)
            };

            FlowLayoutPanel pnlButonlar = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(0, 14, 18, 0),
                BackColor = Color.White
            };

            if (!string.IsNullOrEmpty(oncekiYazi) && oncekiTikla != null)
            {
                ModernHapButon btnOnceki = UstHapButonOlustur(oncekiYazi.Replace("<", "←"), Color.White, Color.FromArgb(51, 65, 85), KartKenar);
                btnOnceki.Click += (s, e) => oncekiTikla();
                pnlButonlar.Controls.Add(btnOnceki);
            }

            ModernHapButon btnAnaMenu = UstHapButonOlustur("Ana Menü", AcikMaviZemin, KoyuMaviYazi, AcikMaviKenar);
            btnAnaMenu.Click += (s, e) => AnaFormaDon(form);
            pnlButonlar.Controls.Add(btnAnaMenu);

            if (!string.IsNullOrEmpty(sonrakiYazi) && sonrakiTikla != null)
            {
                ModernHapButon btnSonraki = UstHapButonOlustur(sonrakiYazi.Replace(">", "→"), BirincilMavi, Color.White, Color.Transparent);
                btnSonraki.HoverArkaPlan = Color.FromArgb(2, 132, 199);
                btnSonraki.Click += (s, e) => sonrakiTikla();
                pnlButonlar.Controls.Add(btnSonraki);
            }

            pnlUst.Controls.Add(lblBaslik);
            pnlUst.Controls.Add(pnlButonlar);
            form.Controls.Add(pnlUst);
            return pnlUst;
        }

        public static Panel SagKontrolPaneliOlustur(Form form, int genislik = 290)
        {
            Panel pnlSag = new Panel
            {
                Dock = DockStyle.Right,
                Width = genislik,
                BackColor = Color.White,
                Padding = new Padding(18),
                AutoScroll = true
            };
            pnlSag.Paint += (s, e) =>
            {
                using Pen solCizgi = new Pen(KartKenar, 1.2f);
                e.Graphics.DrawLine(solCizgi, 0, 0, 0, pnlSag.Height);
            };
            form.Controls.Add(pnlSag);
            return pnlSag;
        }

        public static ModernKartPanel AcikMaviBilgiKartiOlustur(Point konum, Size boyut, out Label icerikEtiketi)
        {
            ModernKartPanel kart = new ModernKartPanel
            {
                Location = konum,
                Size = boyut,
                KartArkaPlan = AcikMaviZemin,
                KenarlikRengi = AcikMaviKenar,
                Padding = new Padding(14)
            };

            icerikEtiketi = new Label
            {
                Dock = DockStyle.Fill,
                BackColor = AcikMaviZemin,
                ForeColor = KoyuYazi,
                Font = new Font("Segoe UI", 9.3f),
                UseMnemonic = false
            };
            kart.Controls.Add(icerikEtiketi);
            return kart;
        }

        public static Panel KartAltSurguPaneliOlustur(string solEtiket, string sagAralik, int min, int max, int deger, out ModernSurgu surgu, out Label solLabel)
        {
            Panel pnl = new Panel { Height = 56, Padding = new Padding(2, 6, 2, 0), BackColor = Color.White };
            Panel pnlUst = new Panel { Dock = DockStyle.Top, Height = 20, BackColor = Color.White };
            solLabel = new Label
            {
                Text = solEtiket,
                Location = new Point(0, 1),
                AutoSize = true,
                Font = new Font("Segoe UI", 8.8f),
                ForeColor = AcikYazi,
                BackColor = Color.White
            };
            Label sagLabel = new Label
            {
                Text = sagAralik,
                Dock = DockStyle.Right,
                AutoSize = true,
                Font = new Font("Segoe UI", 8.8f),
                ForeColor = AcikYazi,
                BackColor = Color.White
            };
            pnlUst.Controls.Add(solLabel);
            pnlUst.Controls.Add(sagLabel);

            surgu = new ModernSurgu { Dock = DockStyle.Top, Minimum = min, Maximum = max, Value = deger };
            pnl.Controls.Add(surgu);
            pnl.Controls.Add(pnlUst);
            return pnl;
        }

        public static ModernKartPanel AltAnalizSeridiOlustur(string metin, out Label metinLabel, string durumMetni = "Aktif")
        {
            ModernKartPanel kart = new ModernKartPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(8, 4, 8, 6),
                Padding = new Padding(16, 10, 16, 10),
                KartArkaPlan = AcikMaviZemin,
                KenarlikRengi = AcikMaviKenar
            };

            ModernHapRozet rozetAktif = new ModernHapRozet
            {
                Dock = DockStyle.Right,
                RozetArkaPlan = Color.White,
                RozetYaziRengi = BirincilMavi
            };
            rozetAktif.MetinGuncelle(durumMetni);

            metinLabel = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = AcikMaviZemin,
                ForeColor = KoyuMaviYazi,
                Font = new Font("Segoe UI Semibold", 9.3f),
                UseMnemonic = false,
                Text = metin
            };
            kart.Controls.Add(metinLabel);
            kart.Controls.Add(rozetAktif);
            return kart;
        }

        public static ModernHapButon ModernButonOlustur(string yazi, bool birincilMi, int genislik = 250, int yukseklik = 38)
        {
            return new ModernHapButon
            {
                Text = yazi,
                Size = new Size(genislik, yukseklik),
                NormalArkaPlan = birincilMi ? Color.FromArgb(30, 58, 95) : AcikMaviZemin,
                HoverArkaPlan = birincilMi ? BirincilMavi : Color.FromArgb(224, 242, 254),
                YaziRengi = birincilMi ? Color.White : KoyuMaviYazi,
                KenarlikRengi = birincilMi ? Color.Transparent : AcikMaviKenar
            };
        }

        private static ModernHapButon UstHapButonOlustur(string yazi, Color arka, Color yaziRenk, Color kenarRenk)
        {
            return new ModernHapButon
            {
                Text = yazi,
                Size = new Size(108, 34),
                Margin = new Padding(8, 0, 0, 0),
                NormalArkaPlan = arka,
                HoverArkaPlan = arka == Color.White ? AcikMaviZemin : Color.FromArgb(224, 242, 254),
                YaziRengi = yaziRenk,
                KenarlikRengi = kenarRenk
            };
        }

        public static void YerlesimiDuzenle(Form form, Panel pnlUst, Panel pnlYan, Control pnlOrta)
        {
            pnlOrta.BringToFront();
            pnlUst.SendToBack();
        }

        public static PictureBox KartliPictureBoxOlustur(
            TableLayoutPanel tablo,
            int sutun,
            int satir,
            string baslik,
            Color? arkaRenk = null,
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
                Location = new Point(0, 4),
                Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold),
                ForeColor = KoyuYazi,
                BackColor = Color.White
            };
            pnlKartUst.Controls.Add(lbl);

            if (sagUstRozet != null)
            {
                sagUstRozet.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                sagUstRozet.Location = new Point(pnlKartUst.Width - sagUstRozet.Width - 2, 2);
                pnlKartUst.Resize += (s, e) =>
                {
                    sagUstRozet.Location = new Point(pnlKartUst.Width - sagUstRozet.Width - 2, 2);
                };
                pnlKartUst.Controls.Add(sagUstRozet);
            }

            PictureBox pb = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = arkaRenk ?? Color.FromArgb(244, 248, 253)
            };

            if (altKontrolAlani != null)
            {
                altKontrolAlani.Dock = DockStyle.Bottom;
                altKontrolAlani.BackColor = Color.White;
                kart.Controls.Add(altKontrolAlani);
            }

            kart.Controls.Add(pb);
            kart.Controls.Add(pnlKartUst);
            tablo.Controls.Add(kart, sutun, satir);
            return pb;
        }

        public static void FormGecisYap(Form mevcutForm, Form hedefForm)
        {
            hedefForm.StartPosition = FormStartPosition.Manual;
            hedefForm.Location = mevcutForm.Location;
            hedefForm.Size = mevcutForm.Size;
            hedefForm.WindowState = mevcutForm.WindowState;
            hedefForm.Show();
            mevcutForm.Hide();
        }

        public static void AnaFormaDon(Form mevcutForm)
        {
            Form anaForm = Application.OpenForms["Form1"];
            if (anaForm != null)
            {
                anaForm.StartPosition = FormStartPosition.Manual;
                anaForm.Location = mevcutForm.Location;
                anaForm.Size = mevcutForm.Size;
                anaForm.Show();
                anaForm.BringToFront();
            }
            else
            {
                Form1 yeniForm = new Form1
                {
                    StartPosition = FormStartPosition.Manual,
                    Location = mevcutForm.Location,
                    Size = mevcutForm.Size
                };
                yeniForm.Show();
            }
            mevcutForm.Hide();
        }

        public static string ResimDosyasiSec(string baslik = "Resim Seçiniz")
        {
            using OpenFileDialog ofd = new OpenFileDialog
            {
                Title = baslik,
                Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.bmp;*.tif"
            };
            return ofd.ShowDialog() == DialogResult.OK ? ofd.FileName : null;
        }
    }
}
