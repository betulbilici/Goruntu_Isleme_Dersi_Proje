namespace GoruntuIsleme
{
    partial class Hafta3Odev4
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
            picOrijinal = new PictureBox();
            picDegistirme = new PictureBox();
            picInterpolasyon = new PictureBox();
            btnResimSec = new Button();
            btnBuyut = new Button();
            numOran = new NumericUpDown();
            lblOran = new Label();
            lblBilgi = new Label();
            lnkGeri = new LinkLabel();
            linkLabel2 = new LinkLabel();
            lnkSonraki = new LinkLabel();
            ((System.ComponentModel.ISupportInitialize)picOrijinal).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picDegistirme).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picInterpolasyon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numOran).BeginInit();
            SuspendLayout();
            // 
            // picOrijinal
            // 
            picOrijinal.BorderStyle = BorderStyle.FixedSingle;
            picOrijinal.Location = new Point(40, 50);
            picOrijinal.Name = "picOrijinal";
            picOrijinal.Size = new Size(260, 260);
            picOrijinal.TabIndex = 0;
            picOrijinal.TabStop = false;
            // 
            // picDegistirme
            // 
            picDegistirme.BorderStyle = BorderStyle.FixedSingle;
            picDegistirme.Location = new Point(320, 50);
            picDegistirme.Name = "picDegistirme";
            picDegistirme.Size = new Size(260, 260);
            picDegistirme.TabIndex = 1;
            picDegistirme.TabStop = false;
            // 
            // picInterpolasyon
            // 
            picInterpolasyon.BorderStyle = BorderStyle.FixedSingle;
            picInterpolasyon.Location = new Point(692, 50);
            picInterpolasyon.Name = "picInterpolasyon";
            picInterpolasyon.Size = new Size(420, 420);
            picInterpolasyon.TabIndex = 2;
            picInterpolasyon.TabStop = false;
            // 
            // btnResimSec
            // 
            btnResimSec.BackColor = SystemColors.Highlight;
            btnResimSec.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnResimSec.ForeColor = Color.White;
            btnResimSec.Location = new Point(40, 327);
            btnResimSec.Name = "btnResimSec";
            btnResimSec.Size = new Size(202, 36);
            btnResimSec.TabIndex = 3;
            btnResimSec.Text = "📁 Küçük Resim Seç";
            btnResimSec.UseVisualStyleBackColor = false;
            // 
            // btnBuyut
            // 
            btnBuyut.BackColor = SystemColors.Highlight;
            btnBuyut.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnBuyut.ForeColor = Color.White;
            btnBuyut.Location = new Point(133, 396);
            btnBuyut.Name = "btnBuyut";
            btnBuyut.Size = new Size(167, 40);
            btnBuyut.TabIndex = 4;
            btnBuyut.Text = "🔍 Resmi Büyüt";
            btnBuyut.UseVisualStyleBackColor = false;
            // 
            // numOran
            // 
            numOran.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 162);
            numOran.Location = new Point(430, 349);
            numOran.Maximum = new decimal(new int[] { 6, 0, 0, 0 });
            numOran.Minimum = new decimal(new int[] { 2, 0, 0, 0 });
            numOran.Name = "numOran";
            numOran.Size = new Size(150, 30);
            numOran.TabIndex = 5;
            numOran.Value = new decimal(new int[] { 2, 0, 0, 0 });
            // 
            // lblOran
            // 
            lblOran.AutoSize = true;
            lblOran.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblOran.Location = new Point(266, 349);
            lblOran.Name = "lblOran";
            lblOran.Size = new Size(158, 23);
            lblOran.TabIndex = 6;
            lblOran.Text = "Büyütme Oranı (X):";
            // 
            // lblBilgi
            // 
            lblBilgi.BorderStyle = BorderStyle.Fixed3D;
            lblBilgi.Location = new Point(40, 459);
            lblBilgi.Name = "lblBilgi";
            lblBilgi.Size = new Size(570, 189);
            lblBilgi.TabIndex = 7;
            lblBilgi.Text = "label1";
            // 
            // lnkGeri
            // 
            lnkGeri.AutoSize = true;
            lnkGeri.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lnkGeri.Location = new Point(12, 671);
            lnkGeri.Name = "lnkGeri";
            lnkGeri.Size = new Size(140, 23);
            lnkGeri.TabIndex = 8;
            lnkGeri.TabStop = true;
            lnkGeri.Text = "◀ Ödev 3'e Dön";
            // 
            // linkLabel2
            // 
            linkLabel2.AutoSize = true;
            linkLabel2.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            linkLabel2.Location = new Point(920, 772);
            linkLabel2.Name = "linkLabel2";
            linkLabel2.Size = new Size(93, 23);
            linkLabel2.TabIndex = 9;
            linkLabel2.TabStop = true;
            linkLabel2.Text = "linkLabel2";
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
            lnkSonraki.Text = "Ödev 5'e Git ▶";
            // 
            // Hafta3Odev4
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1232, 703);
            Controls.Add(lnkSonraki);
            Controls.Add(linkLabel2);
            Controls.Add(lnkGeri);
            Controls.Add(lblBilgi);
            Controls.Add(lblOran);
            Controls.Add(numOran);
            Controls.Add(btnBuyut);
            Controls.Add(btnResimSec);
            Controls.Add(picInterpolasyon);
            Controls.Add(picDegistirme);
            Controls.Add(picOrijinal);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "Hafta3Odev4";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Hafta 3 - Ödev 4: Resim Büyütme ve İnterpolasyon";
            ((System.ComponentModel.ISupportInitialize)picOrijinal).EndInit();
            ((System.ComponentModel.ISupportInitialize)picDegistirme).EndInit();
            ((System.ComponentModel.ISupportInitialize)picInterpolasyon).EndInit();
            ((System.ComponentModel.ISupportInitialize)numOran).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picOrijinal;
        private PictureBox picDegistirme;
        private PictureBox picInterpolasyon;
        private Button btnResimSec;
        private Button btnBuyut;
        private NumericUpDown numOran;
        private Label lblOran;
        private Label lblBilgi;
        private LinkLabel lnkGeri;
        private LinkLabel linkLabel2;
        private LinkLabel lnkSonraki;
    }
}