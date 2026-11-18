namespace GoruntuIsleme
{
    partial class Hafta2Odev9
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
            picGiris = new PictureBox();
            picCikis = new PictureBox();
            picHistogram = new PictureBox();
            trackEsik = new TrackBar();
            trackMin = new TrackBar();
            trackYaricap = new TrackBar();
            chkYesilMod = new CheckBox();
            btnResimSec = new Button();
            btnSifirlaBolge = new Button();
            lblEsikBilgi = new Label();
            lblMinBilgi = new Label();
            lnkGeri = new LinkLabel();
            lnkSonraki = new LinkLabel();
            lblYaricapBilgi = new Label();
            ((System.ComponentModel.ISupportInitialize)picGiris).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picCikis).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picHistogram).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackEsik).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackMin).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackYaricap).BeginInit();
            SuspendLayout();
            // 
            // picGiris
            // 
            picGiris.BorderStyle = BorderStyle.FixedSingle;
            picGiris.Location = new Point(87, 12);
            picGiris.Name = "picGiris";
            picGiris.Size = new Size(480, 360);
            picGiris.SizeMode = PictureBoxSizeMode.StretchImage;
            picGiris.TabIndex = 0;
            picGiris.TabStop = false;
            // 
            // picCikis
            // 
            picCikis.BorderStyle = BorderStyle.FixedSingle;
            picCikis.Location = new Point(634, 12);
            picCikis.Name = "picCikis";
            picCikis.Size = new Size(480, 360);
            picCikis.SizeMode = PictureBoxSizeMode.StretchImage;
            picCikis.TabIndex = 1;
            picCikis.TabStop = false;
            // 
            // picHistogram
            // 
            picHistogram.BackColor = Color.Black;
            picHistogram.Location = new Point(40, 378);
            picHistogram.Name = "picHistogram";
            picHistogram.Size = new Size(1120, 140);
            picHistogram.TabIndex = 2;
            picHistogram.TabStop = false;
            // 
            // trackEsik
            // 
            trackEsik.Location = new Point(198, 524);
            trackEsik.Maximum = 240;
            trackEsik.Minimum = 10;
            trackEsik.Name = "trackEsik";
            trackEsik.Size = new Size(340, 56);
            trackEsik.TabIndex = 3;
            trackEsik.Value = 128;
            // 
            // trackMin
            // 
            trackMin.Location = new Point(198, 579);
            trackMin.Maximum = 100;
            trackMin.Name = "trackMin";
            trackMin.Size = new Size(340, 56);
            trackMin.TabIndex = 4;
            trackMin.Value = 15;
            // 
            // trackYaricap
            // 
            trackYaricap.Location = new Point(706, 529);
            trackYaricap.Maximum = 250;
            trackYaricap.Minimum = 20;
            trackYaricap.Name = "trackYaricap";
            trackYaricap.Size = new Size(280, 56);
            trackYaricap.TabIndex = 5;
            trackYaricap.Value = 80;
            // 
            // chkYesilMod
            // 
            chkYesilMod.AutoSize = true;
            chkYesilMod.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            chkYesilMod.ForeColor = Color.Green;
            chkYesilMod.Location = new Point(689, 632);
            chkYesilMod.Name = "chkYesilMod";
            chkYesilMod.Size = new Size(258, 27);
            chkYesilMod.TabIndex = 6;
            chkYesilMod.Text = "Yeşil Fosfor Gece Görüş Efekti";
            chkYesilMod.UseVisualStyleBackColor = true;
            // 
            // btnResimSec
            // 
            btnResimSec.BackColor = SystemColors.Highlight;
            btnResimSec.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnResimSec.ForeColor = Color.White;
            btnResimSec.Location = new Point(1070, 529);
            btnResimSec.Name = "btnResimSec";
            btnResimSec.Size = new Size(150, 38);
            btnResimSec.TabIndex = 7;
            btnResimSec.Text = "Gece Resmi Seç";
            btnResimSec.UseVisualStyleBackColor = false;
            // 
            // btnSifirlaBolge
            // 
            btnSifirlaBolge.BackColor = SystemColors.Highlight;
            btnSifirlaBolge.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnSifirlaBolge.ForeColor = Color.White;
            btnSifirlaBolge.Location = new Point(1070, 579);
            btnSifirlaBolge.Name = "btnSifirlaBolge";
            btnSifirlaBolge.Size = new Size(150, 38);
            btnSifirlaBolge.TabIndex = 8;
            btnSifirlaBolge.Text = "Bölgesel Işığı Sıfırla";
            btnSifirlaBolge.UseVisualStyleBackColor = false;
            // 
            // lblEsikBilgi
            // 
            lblEsikBilgi.AutoSize = true;
            lblEsikBilgi.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblEsikBilgi.Location = new Point(64, 529);
            lblEsikBilgi.Name = "lblEsikBilgi";
            lblEsikBilgi.Size = new Size(128, 20);
            lblEsikBilgi.TabIndex = 9;
            lblEsikBilgi.Text = "Kırılma (Mid): 128";
            // 
            // lblMinBilgi
            // 
            lblMinBilgi.AutoSize = true;
            lblMinBilgi.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblMinBilgi.Location = new Point(106, 579);
            lblMinBilgi.Name = "lblMinBilgi";
            lblMinBilgi.Size = new Size(86, 20);
            lblMinBilgi.TabIndex = 10;
            lblMinBilgi.Text = "Alt Sınır: 15";
            // 
            // lnkGeri
            // 
            lnkGeri.AutoSize = true;
            lnkGeri.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lnkGeri.Location = new Point(12, 671);
            lnkGeri.Name = "lnkGeri";
            lnkGeri.Size = new Size(140, 23);
            lnkGeri.TabIndex = 11;
            lnkGeri.TabStop = true;
            lnkGeri.Text = "◀ Ödev 8'e Dön";
            // 
            // lnkSonraki
            // 
            lnkSonraki.AutoSize = true;
            lnkSonraki.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lnkSonraki.Location = new Point(1081, 671);
            lnkSonraki.Name = "lnkSonraki";
            lnkSonraki.Size = new Size(141, 23);
            lnkSonraki.TabIndex = 12;
            lnkSonraki.TabStop = true;
            lnkSonraki.Text = "Ödev 10'a Git ▶";
            // 
            // lblYaricapBilgi
            // 
            lblYaricapBilgi.AutoSize = true;
            lblYaricapBilgi.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblYaricapBilgi.Location = new Point(566, 532);
            lblYaricapBilgi.Name = "lblYaricapBilgi";
            lblYaricapBilgi.Size = new Size(134, 20);
            lblYaricapBilgi.TabIndex = 13;
            lblYaricapBilgi.Text = "Işık Yarıçapı: 80 px";
            // 
            // Hafta2Odev9
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1232, 703);
            Controls.Add(lblYaricapBilgi);
            Controls.Add(lnkSonraki);
            Controls.Add(lnkGeri);
            Controls.Add(lblMinBilgi);
            Controls.Add(lblEsikBilgi);
            Controls.Add(btnSifirlaBolge);
            Controls.Add(btnResimSec);
            Controls.Add(chkYesilMod);
            Controls.Add(trackYaricap);
            Controls.Add(trackMin);
            Controls.Add(trackEsik);
            Controls.Add(picHistogram);
            Controls.Add(picCikis);
            Controls.Add(picGiris);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "Hafta2Odev9";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Hafta2Odev9";
            ((System.ComponentModel.ISupportInitialize)picGiris).EndInit();
            ((System.ComponentModel.ISupportInitialize)picCikis).EndInit();
            ((System.ComponentModel.ISupportInitialize)picHistogram).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackEsik).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackMin).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackYaricap).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picGiris;
        private PictureBox picCikis;
        private PictureBox picHistogram;
        private TrackBar trackEsik;
        private TrackBar trackMin;
        private TrackBar trackYaricap;
        private CheckBox chkYesilMod;
        private Button btnResimSec;
        private Button btnSifirlaBolge;
        private Label lblEsikBilgi;
        private Label lblMinBilgi;
        private LinkLabel lnkGeri;
        private LinkLabel lnkSonraki;
        private Label lblYaricapBilgi;
    }
}