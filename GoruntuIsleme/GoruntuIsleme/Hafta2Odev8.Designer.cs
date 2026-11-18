namespace GoruntuIsleme
{
    partial class Hafta2Odev8
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
            btnResimSec = new Button();
            btnTemizle = new Button();
            btnRenkSec = new Button();
            lblRenkOnizleme = new Label();
            btnRenklendir = new Button();
            lblDurum = new Label();
            lstNoktalar = new ListBox();
            lnkGeri = new LinkLabel();
            lnkSonraki = new LinkLabel();
            ((System.ComponentModel.ISupportInitialize)picGiris).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picCikis).BeginInit();
            SuspendLayout();
            // 
            // picGiris
            // 
            picGiris.BorderStyle = BorderStyle.FixedSingle;
            picGiris.Location = new Point(32, 12);
            picGiris.Name = "picGiris";
            picGiris.Size = new Size(550, 430);
            picGiris.SizeMode = PictureBoxSizeMode.StretchImage;
            picGiris.TabIndex = 0;
            picGiris.TabStop = false;
            // 
            // picCikis
            // 
            picCikis.BorderStyle = BorderStyle.FixedSingle;
            picCikis.Location = new Point(633, 12);
            picCikis.Name = "picCikis";
            picCikis.Size = new Size(550, 430);
            picCikis.SizeMode = PictureBoxSizeMode.StretchImage;
            picCikis.TabIndex = 1;
            picCikis.TabStop = false;
            // 
            // btnResimSec
            // 
            btnResimSec.BackColor = SystemColors.ActiveBorder;
            btnResimSec.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnResimSec.Location = new Point(80, 448);
            btnResimSec.Name = "btnResimSec";
            btnResimSec.Size = new Size(120, 36);
            btnResimSec.TabIndex = 2;
            btnResimSec.Text = "Resim Seç";
            btnResimSec.UseVisualStyleBackColor = false;
            // 
            // btnTemizle
            // 
            btnTemizle.BackColor = SystemColors.ActiveBorder;
            btnTemizle.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnTemizle.Location = new Point(210, 448);
            btnTemizle.Name = "btnTemizle";
            btnTemizle.Size = new Size(130, 36);
            btnTemizle.TabIndex = 3;
            btnTemizle.Text = "Noktaları Sıfırla";
            btnTemizle.UseVisualStyleBackColor = false;
            // 
            // btnRenkSec
            // 
            btnRenkSec.BackColor = SystemColors.ActiveBorder;
            btnRenkSec.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnRenkSec.Location = new Point(350, 448);
            btnRenkSec.Name = "btnRenkSec";
            btnRenkSec.Size = new Size(120, 36);
            btnRenkSec.TabIndex = 4;
            btnRenkSec.Text = "Renk Seç...";
            btnRenkSec.UseVisualStyleBackColor = false;
            // 
            // lblRenkOnizleme
            // 
            lblRenkOnizleme.BackColor = Color.SkyBlue;
            lblRenkOnizleme.BorderStyle = BorderStyle.Fixed3D;
            lblRenkOnizleme.Location = new Point(480, 448);
            lblRenkOnizleme.Name = "lblRenkOnizleme";
            lblRenkOnizleme.Size = new Size(36, 36);
            lblRenkOnizleme.TabIndex = 5;
            // 
            // btnRenklendir
            // 
            btnRenklendir.BackColor = Color.LightGreen;
            btnRenklendir.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnRenklendir.Location = new Point(530, 448);
            btnRenklendir.Name = "btnRenklendir";
            btnRenklendir.Size = new Size(150, 36);
            btnRenklendir.TabIndex = 6;
            btnRenklendir.Text = "🎨 Renklendir";
            btnRenklendir.UseVisualStyleBackColor = false;
            // 
            // lblDurum
            // 
            lblDurum.AutoSize = true;
            lblDurum.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblDurum.Location = new Point(700, 455);
            lblDurum.Name = "lblDurum";
            lblDurum.Size = new Size(147, 20);
            lblDurum.TabIndex = 7;
            lblDurum.Text = "Seçilen Nokta: 0 / 10";
            // 
            // lstNoktalar
            // 
            lstNoktalar.FormattingEnabled = true;
            lstNoktalar.Location = new Point(43, 505);
            lstNoktalar.Name = "lstNoktalar";
            lstNoktalar.Size = new Size(1140, 124);
            lstNoktalar.TabIndex = 8;
            // 
            // lnkGeri
            // 
            lnkGeri.AutoSize = true;
            lnkGeri.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lnkGeri.Location = new Point(12, 671);
            lnkGeri.Name = "lnkGeri";
            lnkGeri.Size = new Size(149, 23);
            lnkGeri.TabIndex = 9;
            lnkGeri.TabStop = true;
            lnkGeri.Text = "◀ Ödev 7'ye Dön";
            // 
            // lnkSonraki
            // 
            lnkSonraki.AutoSize = true;
            lnkSonraki.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lnkSonraki.Location = new Point(1081, 671);
            lnkSonraki.Name = "lnkSonraki";
            lnkSonraki.Size = new Size(131, 23);
            lnkSonraki.TabIndex = 10;
            lnkSonraki.TabStop = true;
            lnkSonraki.Text = "Ödev 9'a Git ▶";
            // 
            // Hafta2Odev8
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1232, 703);
            Controls.Add(lnkSonraki);
            Controls.Add(lnkGeri);
            Controls.Add(lstNoktalar);
            Controls.Add(lblDurum);
            Controls.Add(btnRenklendir);
            Controls.Add(lblRenkOnizleme);
            Controls.Add(btnRenkSec);
            Controls.Add(btnTemizle);
            Controls.Add(btnResimSec);
            Controls.Add(picCikis);
            Controls.Add(picGiris);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "Hafta2Odev8";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Hafta2Odev8";
            ((System.ComponentModel.ISupportInitialize)picGiris).EndInit();
            ((System.ComponentModel.ISupportInitialize)picCikis).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picGiris;
        private PictureBox picCikis;
        private Button btnResimSec;
        private Button btnTemizle;
        private Button btnRenkSec;
        private Label lblRenkOnizleme;
        private Button btnRenklendir;
        private Label lblDurum;
        private ListBox lstNoktalar;
        private LinkLabel lnkGeri;
        private LinkLabel lnkSonraki;
    }
}