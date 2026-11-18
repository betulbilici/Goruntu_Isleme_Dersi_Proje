namespace GoruntuIsleme
{
    partial class Hafta3Odev9
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
            lblBaslikOrijinal = new Label();
            lblBaslikDegistirme = new Label();
            lblBaslikInterpolasyon = new Label();
            picOrijinal = new PictureBox();
            picDegistirme = new PictureBox();
            picInterpolasyon = new PictureBox();
            btnResimSec = new Button();
            lblKatsayi = new Label();
            numKatsayi = new NumericUpDown();
            btnBuyut = new Button();
            lblBilgi = new Label();
            lnkGeri = new LinkLabel();
            lnkSonraki = new LinkLabel();
            ((System.ComponentModel.ISupportInitialize)picOrijinal).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picDegistirme).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picInterpolasyon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numKatsayi).BeginInit();
            SuspendLayout();
            // 
            // lblBaslikOrijinal
            // 
            lblBaslikOrijinal.AutoSize = true;
            lblBaslikOrijinal.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblBaslikOrijinal.Location = new Point(40, 18);
            lblBaslikOrijinal.Name = "lblBaslikOrijinal";
            lblBaslikOrijinal.Size = new Size(115, 23);
            lblBaslikOrijinal.TabIndex = 0;
            lblBaslikOrijinal.Text = "Orijinal Resim";
            // 
            // lblBaslikDegistirme
            // 
            lblBaslikDegistirme.AutoSize = true;
            lblBaslikDegistirme.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblBaslikDegistirme.Location = new Point(440, 18);
            lblBaslikDegistirme.Name = "lblBaslikDegistirme";
            lblBaslikDegistirme.Size = new Size(140, 23);
            lblBaslikDegistirme.TabIndex = 1;
            lblBaslikDegistirme.Text = "Piksel Değiştirme";
            // 
            // lblBaslikInterpolasyon
            // 
            lblBaslikInterpolasyon.AutoSize = true;
            lblBaslikInterpolasyon.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblBaslikInterpolasyon.Location = new Point(840, 18);
            lblBaslikInterpolasyon.Name = "lblBaslikInterpolasyon";
            lblBaslikInterpolasyon.Size = new Size(173, 23);
            lblBaslikInterpolasyon.TabIndex = 2;
            lblBaslikInterpolasyon.Text = "Piksel İnterpolasyonu";
            // 
            // picOrijinal
            // 
            picOrijinal.BorderStyle = BorderStyle.FixedSingle;
            picOrijinal.Location = new Point(40, 45);
            picOrijinal.Name = "picOrijinal";
            picOrijinal.Size = new Size(360, 420);
            picOrijinal.SizeMode = PictureBoxSizeMode.Zoom;
            picOrijinal.TabIndex = 3;
            picOrijinal.TabStop = false;
            // 
            // picDegistirme
            // 
            picDegistirme.BorderStyle = BorderStyle.FixedSingle;
            picDegistirme.Location = new Point(440, 45);
            picDegistirme.Name = "picDegistirme";
            picDegistirme.Size = new Size(360, 420);
            picDegistirme.SizeMode = PictureBoxSizeMode.Zoom;
            picDegistirme.TabIndex = 4;
            picDegistirme.TabStop = false;
            // 
            // picInterpolasyon
            // 
            picInterpolasyon.BorderStyle = BorderStyle.FixedSingle;
            picInterpolasyon.Location = new Point(840, 45);
            picInterpolasyon.Name = "picInterpolasyon";
            picInterpolasyon.Size = new Size(360, 420);
            picInterpolasyon.SizeMode = PictureBoxSizeMode.Zoom;
            picInterpolasyon.TabIndex = 5;
            picInterpolasyon.TabStop = false;
            // 
            // btnResimSec
            // 
            btnResimSec.BackColor = SystemColors.Highlight;
            btnResimSec.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnResimSec.ForeColor = Color.White;
            btnResimSec.Location = new Point(40, 490);
            btnResimSec.Name = "btnResimSec";
            btnResimSec.Size = new Size(133, 37);
            btnResimSec.TabIndex = 6;
            btnResimSec.Text = "📁 Resim Seç";
            btnResimSec.UseVisualStyleBackColor = false;
            // 
            // lblKatsayi
            // 
            lblKatsayi.AutoSize = true;
            lblKatsayi.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblKatsayi.Location = new Point(40, 545);
            lblKatsayi.Name = "lblKatsayi";
            lblKatsayi.Size = new Size(153, 23);
            lblKatsayi.TabIndex = 7;
            lblKatsayi.Text = "Büyütme Katsayısı:";
            // 
            // numKatsayi
            // 
            numKatsayi.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 162);
            numKatsayi.Location = new Point(199, 541);
            numKatsayi.Maximum = new decimal(new int[] { 8, 0, 0, 0 });
            numKatsayi.Minimum = new decimal(new int[] { 2, 0, 0, 0 });
            numKatsayi.Name = "numKatsayi";
            numKatsayi.Size = new Size(85, 30);
            numKatsayi.TabIndex = 8;
            numKatsayi.Value = new decimal(new int[] { 2, 0, 0, 0 });
            // 
            // btnBuyut
            // 
            btnBuyut.BackColor = SystemColors.Highlight;
            btnBuyut.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnBuyut.ForeColor = Color.White;
            btnBuyut.Location = new Point(40, 585);
            btnBuyut.Name = "btnBuyut";
            btnBuyut.Size = new Size(220, 42);
            btnBuyut.TabIndex = 9;
            btnBuyut.Text = "🔍 Resmi Büyüt";
            btnBuyut.UseVisualStyleBackColor = false;
            // 
            // lblBilgi
            // 
            lblBilgi.BorderStyle = BorderStyle.FixedSingle;
            lblBilgi.Location = new Point(371, 490);
            lblBilgi.Name = "lblBilgi";
            lblBilgi.Size = new Size(798, 137);
            lblBilgi.TabIndex = 10;
            lblBilgi.Text = "label1";
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
            // Hafta3Odev9
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1232, 703);
            Controls.Add(lnkSonraki);
            Controls.Add(lnkGeri);
            Controls.Add(lblBilgi);
            Controls.Add(btnBuyut);
            Controls.Add(numKatsayi);
            Controls.Add(lblKatsayi);
            Controls.Add(btnResimSec);
            Controls.Add(picInterpolasyon);
            Controls.Add(picDegistirme);
            Controls.Add(picOrijinal);
            Controls.Add(lblBaslikInterpolasyon);
            Controls.Add(lblBaslikDegistirme);
            Controls.Add(lblBaslikOrijinal);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "Hafta3Odev9";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Hafta 3-Ödev 9: Büyütmede Piksel Değiştirme ve İnterpolasyon Karşılaştırması";
            ((System.ComponentModel.ISupportInitialize)picOrijinal).EndInit();
            ((System.ComponentModel.ISupportInitialize)picDegistirme).EndInit();
            ((System.ComponentModel.ISupportInitialize)picInterpolasyon).EndInit();
            ((System.ComponentModel.ISupportInitialize)numKatsayi).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblBaslikOrijinal;
        private Label lblBaslikDegistirme;
        private Label lblBaslikInterpolasyon;
        private PictureBox picOrijinal;
        private PictureBox picDegistirme;
        private PictureBox picInterpolasyon;
        private Button btnResimSec;
        private Label lblKatsayi;
        private NumericUpDown numKatsayi;
        private Button btnBuyut;
        private Label lblBilgi;
        private LinkLabel lnkGeri;
        private LinkLabel lnkSonraki;
    }
}