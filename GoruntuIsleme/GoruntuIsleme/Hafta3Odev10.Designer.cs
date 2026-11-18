namespace GoruntuIsleme
{
    partial class Hafta3Odev10
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
            lblBaslik = new Label();
            picCanvas = new PictureBox();
            btnResimSec = new Button();
            btnSifirla = new Button();
            lblBilgi = new Label();
            lnkGeri = new LinkLabel();
            lnkSonraki = new LinkLabel();
            ((System.ComponentModel.ISupportInitialize)picCanvas).BeginInit();
            SuspendLayout();
            // 
            // lblBaslik
            // 
            lblBaslik.AutoSize = true;
            lblBaslik.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblBaslik.Location = new Point(40, 15);
            lblBaslik.Name = "lblBaslik";
            lblBaslik.Size = new Size(503, 23);
            lblBaslik.TabIndex = 0;
            lblBaslik.Text = "İnteraktif Döndürme Alanı (Resme Basılı Tutup Fareyi Döndürün)";
            // 
            // picCanvas
            // 
            picCanvas.BorderStyle = BorderStyle.FixedSingle;
            picCanvas.Location = new Point(40, 45);
            picCanvas.Name = "picCanvas";
            picCanvas.Size = new Size(1160, 520);
            picCanvas.TabIndex = 1;
            picCanvas.TabStop = false;
            // 
            // btnResimSec
            // 
            btnResimSec.BackColor = SystemColors.Highlight;
            btnResimSec.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnResimSec.ForeColor = Color.White;
            btnResimSec.Location = new Point(40, 580);
            btnResimSec.Name = "btnResimSec";
            btnResimSec.Size = new Size(180, 42);
            btnResimSec.TabIndex = 2;
            btnResimSec.Text = "📁 Resim Seç";
            btnResimSec.UseVisualStyleBackColor = false;
            // 
            // btnSifirla
            // 
            btnSifirla.BackColor = SystemColors.Highlight;
            btnSifirla.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnSifirla.ForeColor = Color.White;
            btnSifirla.Location = new Point(235, 580);
            btnSifirla.Name = "btnSifirla";
            btnSifirla.Size = new Size(180, 42);
            btnSifirla.TabIndex = 3;
            btnSifirla.Text = "🔄 Açıyı Sıfırla (0°)";
            btnSifirla.UseVisualStyleBackColor = false;
            // 
            // lblBilgi
            // 
            lblBilgi.BorderStyle = BorderStyle.Fixed3D;
            lblBilgi.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblBilgi.Location = new Point(430, 580);
            lblBilgi.Name = "lblBilgi";
            lblBilgi.Size = new Size(770, 75);
            lblBilgi.TabIndex = 4;
            // 
            // lnkGeri
            // 
            lnkGeri.AutoSize = true;
            lnkGeri.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lnkGeri.Location = new Point(12, 671);
            lnkGeri.Name = "lnkGeri";
            lnkGeri.Size = new Size(140, 23);
            lnkGeri.TabIndex = 5;
            lnkGeri.TabStop = true;
            lnkGeri.Text = "◀ Ödev 9'a Dön";
            // 
            // lnkSonraki
            // 
            lnkSonraki.AutoSize = true;
            lnkSonraki.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lnkSonraki.Location = new Point(1080, 674);
            lnkSonraki.Name = "lnkSonraki";
            lnkSonraki.Size = new Size(141, 23);
            lnkSonraki.TabIndex = 6;
            lnkSonraki.TabStop = true;
            lnkSonraki.Text = "Ödev 11'e Git ▶";
            // 
            // Hafta3Odev10
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1232, 703);
            Controls.Add(lnkSonraki);
            Controls.Add(lnkGeri);
            Controls.Add(lblBilgi);
            Controls.Add(btnSifirla);
            Controls.Add(btnResimSec);
            Controls.Add(picCanvas);
            Controls.Add(lblBaslik);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "Hafta3Odev10";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Hafta 3 - Ödev 10: Mouse ile İnteraktif Resim Döndürme";
            ((System.ComponentModel.ISupportInitialize)picCanvas).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblBaslik;
        private PictureBox picCanvas;
        private Button btnResimSec;
        private Button btnSifirla;
        private Label lblBilgi;
        private LinkLabel lnkGeri;
        private LinkLabel lnkSonraki;
    }
}