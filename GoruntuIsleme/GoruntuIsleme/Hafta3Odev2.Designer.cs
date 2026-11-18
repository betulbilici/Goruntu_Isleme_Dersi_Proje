namespace GoruntuIsleme
{
    partial class Hafta3Odev2
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
            picCanvas = new PictureBox();
            btnResimSec = new Button();
            btnSifirla = new Button();
            rbFareyleSurukle = new RadioButton();
            trackShx = new TrackBar();
            trackShy = new TrackBar();
            lblBilgi = new Label();
            lnkGeri = new LinkLabel();
            lnkSonraki = new LinkLabel();
            lblBaslikShx = new Label();
            lblBaslikShy = new Label();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)picCanvas).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackShx).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackShy).BeginInit();
            SuspendLayout();
            // 
            // picCanvas
            // 
            picCanvas.BackColor = Color.FromArgb(235, 235, 235);
            picCanvas.BorderStyle = BorderStyle.FixedSingle;
            picCanvas.Location = new Point(40, 80);
            picCanvas.Name = "picCanvas";
            picCanvas.Size = new Size(595, 446);
            picCanvas.SizeMode = PictureBoxSizeMode.StretchImage;
            picCanvas.TabIndex = 0;
            picCanvas.TabStop = false;
            // 
            // btnResimSec
            // 
            btnResimSec.BackColor = SystemColors.Highlight;
            btnResimSec.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnResimSec.ForeColor = Color.White;
            btnResimSec.Location = new Point(684, 70);
            btnResimSec.Name = "btnResimSec";
            btnResimSec.Size = new Size(167, 40);
            btnResimSec.TabIndex = 1;
            btnResimSec.Text = "📁 Resim Seç";
            btnResimSec.UseVisualStyleBackColor = false;
            // 
            // btnSifirla
            // 
            btnSifirla.BackColor = SystemColors.Highlight;
            btnSifirla.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnSifirla.ForeColor = Color.White;
            btnSifirla.Location = new Point(684, 132);
            btnSifirla.Name = "btnSifirla";
            btnSifirla.Size = new Size(167, 40);
            btnSifirla.TabIndex = 2;
            btnSifirla.Text = "🔄 Resmi Sıfırla";
            btnSifirla.UseVisualStyleBackColor = false;
            // 
            // rbFareyleSurukle
            // 
            rbFareyleSurukle.AutoSize = true;
            rbFareyleSurukle.Checked = true;
            rbFareyleSurukle.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            rbFareyleSurukle.Location = new Point(663, 203);
            rbFareyleSurukle.Name = "rbFareyleSurukle";
            rbFareyleSurukle.Size = new Size(259, 27);
            rbFareyleSurukle.TabIndex = 3;
            rbFareyleSurukle.TabStop = true;
            rbFareyleSurukle.Text = "Fare ile Kenardan Tut & Sürükle";
            rbFareyleSurukle.UseVisualStyleBackColor = true;
            // 
            // trackShx
            // 
            trackShx.Location = new Point(819, 280);
            trackShx.Maximum = 150;
            trackShx.Minimum = -150;
            trackShx.Name = "trackShx";
            trackShx.Size = new Size(298, 56);
            trackShx.TabIndex = 4;
            // 
            // trackShy
            // 
            trackShy.Location = new Point(819, 361);
            trackShy.Maximum = 150;
            trackShy.Minimum = -150;
            trackShy.Name = "trackShy";
            trackShy.Size = new Size(298, 56);
            trackShy.TabIndex = 5;
            // 
            // lblBilgi
            // 
            lblBilgi.BorderStyle = BorderStyle.Fixed3D;
            lblBilgi.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblBilgi.Location = new Point(684, 441);
            lblBilgi.Name = "lblBilgi";
            lblBilgi.Size = new Size(385, 162);
            lblBilgi.TabIndex = 6;
            lblBilgi.Text = "label1";
            // 
            // lnkGeri
            // 
            lnkGeri.AutoSize = true;
            lnkGeri.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lnkGeri.Location = new Point(12, 671);
            lnkGeri.Name = "lnkGeri";
            lnkGeri.Size = new Size(140, 23);
            lnkGeri.TabIndex = 7;
            lnkGeri.TabStop = true;
            lnkGeri.Text = "◀ Ödev 1'e Dön";
            // 
            // lnkSonraki
            // 
            lnkSonraki.AutoSize = true;
            lnkSonraki.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lnkSonraki.Location = new Point(1081, 671);
            lnkSonraki.Name = "lnkSonraki";
            lnkSonraki.Size = new Size(131, 23);
            lnkSonraki.TabIndex = 8;
            lnkSonraki.TabStop = true;
            lnkSonraki.Text = "Ödev 3'e Git ▶";
            // 
            // lblBaslikShx
            // 
            lblBaslikShx.AutoSize = true;
            lblBaslikShx.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblBaslikShx.Location = new Point(663, 257);
            lblBaslikShx.Name = "lblBaslikShx";
            lblBaslikShx.Size = new Size(242, 20);
            lblBaslikShx.TabIndex = 9;
            lblBaslikShx.Text = "Yatay Eğme / Kaydırma (Shx): 0.00";
            // 
            // lblBaslikShy
            // 
            lblBaslikShy.AutoSize = true;
            lblBaslikShy.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblBaslikShy.Location = new Point(663, 338);
            lblBaslikShy.Name = "lblBaslikShy";
            lblBaslikShy.Size = new Size(244, 20);
            lblBaslikShy.TabIndex = 10;
            lblBaslikShy.Text = "Dikey Eğme / Kaydırma (Shy): 0.00";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label1.Location = new Point(276, 40);
            label1.Name = "label1";
            label1.Size = new Size(85, 23);
            label1.TabIndex = 11;
            label1.Text = "Ana Tuval";
            // 
            // Hafta3Odev2
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1232, 703);
            Controls.Add(label1);
            Controls.Add(lblBaslikShy);
            Controls.Add(lblBaslikShx);
            Controls.Add(lnkSonraki);
            Controls.Add(lnkGeri);
            Controls.Add(lblBilgi);
            Controls.Add(trackShy);
            Controls.Add(trackShx);
            Controls.Add(rbFareyleSurukle);
            Controls.Add(btnSifirla);
            Controls.Add(btnResimSec);
            Controls.Add(picCanvas);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "Hafta3Odev2";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Hafta 3 - Ödev 2: İnteraktif Resim Kaydırma (Shear)";
            ((System.ComponentModel.ISupportInitialize)picCanvas).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackShx).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackShy).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picCanvas;
        private Button btnResimSec;
        private Button btnSifirla;
        private RadioButton rbFareyleSurukle;
        private TrackBar trackShx;
        private TrackBar trackShy;
        private Label lblBilgi;
        private LinkLabel lnkGeri;
        private LinkLabel lnkSonraki;
        private Label lblBaslikShx;
        private Label lblBaslikShy;
        private Label label1;
    }
}