namespace GoruntuIsleme
{
    partial class Hafta2Odev6
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            picResim = new PictureBox();
            btnResimSec = new Button();
            btnTemizle = new Button();
            btnKapat = new Button();
            trackParlaklik = new TrackBar();
            lblParlaklik = new Label();
            trackKontrast = new TrackBar();
            lblKontrast = new Label();
            trackR = new TrackBar();
            trackG = new TrackBar();
            trackB = new TrackBar();
            picHistogram = new PictureBox();
            txtAciklama = new TextBox();
            lnkGeri = new LinkLabel();
            lnkSonraki = new LinkLabel();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            btncAciklama = new Button();
            ((System.ComponentModel.ISupportInitialize)picResim).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackParlaklik).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackKontrast).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackR).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackG).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackB).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picHistogram).BeginInit();
            SuspendLayout();
            // 
            // picResim
            // 
            picResim.BorderStyle = BorderStyle.FixedSingle;
            picResim.Location = new Point(12, 12);
            picResim.Name = "picResim";
            picResim.Size = new Size(621, 342);
            picResim.SizeMode = PictureBoxSizeMode.StretchImage;
            picResim.TabIndex = 0;
            picResim.TabStop = false;
            // 
            // btnResimSec
            // 
            btnResimSec.BackColor = SystemColors.Highlight;
            btnResimSec.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold);
            btnResimSec.ForeColor = Color.White;
            btnResimSec.Location = new Point(683, 316);
            btnResimSec.Name = "btnResimSec";
            btnResimSec.Size = new Size(130, 38);
            btnResimSec.TabIndex = 1;
            btnResimSec.Text = "Resim Seç";
            btnResimSec.UseVisualStyleBackColor = false;
            // 
            // btnTemizle
            // 
            btnTemizle.BackColor = SystemColors.Highlight;
            btnTemizle.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold);
            btnTemizle.ForeColor = Color.White;
            btnTemizle.Location = new Point(828, 316);
            btnTemizle.Name = "btnTemizle";
            btnTemizle.Size = new Size(130, 38);
            btnTemizle.TabIndex = 2;
            btnTemizle.Text = "Çizimi Temizle";
            btnTemizle.UseVisualStyleBackColor = false;
            // 
            // btnKapat
            // 
            btnKapat.BackColor = SystemColors.Highlight;
            btnKapat.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold);
            btnKapat.ForeColor = Color.White;
            btnKapat.Location = new Point(973, 316);
            btnKapat.Name = "btnKapat";
            btnKapat.Size = new Size(140, 38);
            btnKapat.TabIndex = 3;
            btnKapat.Text = "Poligon Oluştur";
            btnKapat.UseVisualStyleBackColor = false;
            // 
            // trackParlaklik
            // 
            trackParlaklik.Location = new Point(829, 12);
            trackParlaklik.Maximum = 100;
            trackParlaklik.Minimum = -100;
            trackParlaklik.Name = "trackParlaklik";
            trackParlaklik.Size = new Size(325, 56);
            trackParlaklik.TabIndex = 4;
            // 
            // lblParlaklik
            // 
            lblParlaklik.AutoSize = true;
            lblParlaklik.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblParlaklik.Location = new Point(734, 12);
            lblParlaklik.Name = "lblParlaklik";
            lblParlaklik.Size = new Size(79, 23);
            lblParlaklik.TabIndex = 5;
            lblParlaklik.Text = "Parlaklık:";
            // 
            // trackKontrast
            // 
            trackKontrast.Location = new Point(829, 67);
            trackKontrast.Maximum = 100;
            trackKontrast.Minimum = -100;
            trackKontrast.Name = "trackKontrast";
            trackKontrast.Size = new Size(325, 56);
            trackKontrast.TabIndex = 6;
            // 
            // lblKontrast
            // 
            lblKontrast.AutoSize = true;
            lblKontrast.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblKontrast.Location = new Point(709, 67);
            lblKontrast.Name = "lblKontrast";
            lblKontrast.Size = new Size(104, 23);
            lblKontrast.TabIndex = 7;
            lblKontrast.Text = "Kontrast (F):";
            // 
            // trackR
            // 
            trackR.Location = new Point(829, 122);
            trackR.Maximum = 100;
            trackR.Minimum = -100;
            trackR.Name = "trackR";
            trackR.Size = new Size(325, 56);
            trackR.TabIndex = 8;
            // 
            // trackG
            // 
            trackG.Location = new Point(829, 177);
            trackG.Maximum = 100;
            trackG.Minimum = -100;
            trackG.Name = "trackG";
            trackG.Size = new Size(325, 56);
            trackG.TabIndex = 9;
            // 
            // trackB
            // 
            trackB.Location = new Point(829, 232);
            trackB.Maximum = 100;
            trackB.Minimum = -100;
            trackB.Name = "trackB";
            trackB.Size = new Size(325, 56);
            trackB.TabIndex = 10;
            // 
            // picHistogram
            // 
            picHistogram.BackColor = Color.Black;
            picHistogram.Location = new Point(734, 413);
            picHistogram.Name = "picHistogram";
            picHistogram.Size = new Size(443, 221);
            picHistogram.TabIndex = 11;
            picHistogram.TabStop = false;
            // 
            // txtAciklama
            // 
            txtAciklama.Location = new Point(148, 413);
            txtAciklama.Multiline = true;
            txtAciklama.Name = "txtAciklama";
            txtAciklama.ReadOnly = true;
            txtAciklama.ScrollBars = ScrollBars.Vertical;
            txtAciklama.Size = new Size(512, 221);
            txtAciklama.TabIndex = 12;
            // 
            // lnkGeri
            // 
            lnkGeri.AutoSize = true;
            lnkGeri.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lnkGeri.Location = new Point(12, 671);
            lnkGeri.Name = "lnkGeri";
            lnkGeri.Size = new Size(140, 23);
            lnkGeri.TabIndex = 13;
            lnkGeri.TabStop = true;
            lnkGeri.Text = "◀ Ödev 5'e Dön";
            
            // 
            // lnkSonraki
            // 
            lnkSonraki.AutoSize = true;
            lnkSonraki.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lnkSonraki.Location = new Point(1081, 671);
            lnkSonraki.Name = "lnkSonraki";
            lnkSonraki.Size = new Size(140, 23);
            lnkSonraki.TabIndex = 14;
            lnkSonraki.TabStop = true;
            lnkSonraki.Text = "Ödev 7'ye Git ▶";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label1.Location = new Point(720, 122);
            label1.Name = "label1";
            label1.Size = new Size(93, 23);
            label1.TabIndex = 15;
            label1.Text = "Kırmızı (R):";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label2.Location = new Point(738, 177);
            label2.Name = "label2";
            label2.Size = new Size(75, 23);
            label2.TabIndex = 16;
            label2.Text = "Yeşil (G):";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label3.Location = new Point(734, 232);
            label3.Name = "label3";
            label3.Size = new Size(79, 23);
            label3.TabIndex = 17;
            label3.Text = "Mavi (B):";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label4.Location = new Point(844, 366);
            label4.Name = "label4";
            label4.Size = new Size(209, 23);
            label4.TabIndex = 18;
            label4.Text = "Seçili Bölgenin Histogramı";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label5.Location = new Point(232, 366);
            label5.Name = "label5";
            label5.Size = new Size(338, 23);
            label5.TabIndex = 19;
            label5.Text = "Analitik Geometri / Ray-Casting Açıklaması";
            // 
            // btncAciklama
            // 
            btncAciklama.BackColor = SystemColors.Highlight;
            btncAciklama.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btncAciklama.ForeColor = Color.White;
            btncAciklama.Location = new Point(12, 463);
            btncAciklama.Name = "btncAciklama";
            btncAciklama.Size = new Size(130, 82);
            btncAciklama.TabIndex = 20;
            btncAciklama.Text = "Yöntem Açıklaması\r\n(b Şıkkı)";
            btncAciklama.UseVisualStyleBackColor = false;
            // 
            // Hafta2Odev6
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1232, 703);
            Controls.Add(btncAciklama);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(lnkSonraki);
            Controls.Add(lnkGeri);
            Controls.Add(txtAciklama);
            Controls.Add(picHistogram);
            Controls.Add(trackB);
            Controls.Add(trackG);
            Controls.Add(trackR);
            Controls.Add(lblKontrast);
            Controls.Add(trackKontrast);
            Controls.Add(lblParlaklik);
            Controls.Add(trackParlaklik);
            Controls.Add(btnKapat);
            Controls.Add(btnTemizle);
            Controls.Add(btnResimSec);
            Controls.Add(picResim);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "Hafta2Odev6";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Hafta2Odev6";
            ((System.ComponentModel.ISupportInitialize)picResim).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackParlaklik).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackKontrast).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackR).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackG).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackB).EndInit();
            ((System.ComponentModel.ISupportInitialize)picHistogram).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picResim;
        private Button btnResimSec;
        private Button btnTemizle;
        private Button btnKapat;
        private TrackBar trackParlaklik;
        private Label lblParlaklik;
        private TrackBar trackKontrast;
        private Label lblKontrast;
        private TrackBar trackR;
        private TrackBar trackG;
        private TrackBar trackB;
        private PictureBox picHistogram;
        private TextBox txtAciklama;
        private LinkLabel lnkGeri;
        private LinkLabel lnkSonraki;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Button btncAciklama;
    }
}