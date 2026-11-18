namespace GoruntuIsleme
{
    partial class Hafta3Odev11
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
            lblBaslikKaydirilmis = new Label();
            picOrijinal = new PictureBox();
            picKaydirilmis = new PictureBox();
            btnResimSec = new Button();
            btnSaatYonu = new Button();
            btnSaatTersi = new Button();
            btnSifirla = new Button();
            lblBilgi = new Label();
            lnkGeri = new LinkLabel();
            lnkSonraki = new LinkLabel();
            ((System.ComponentModel.ISupportInitialize)picOrijinal).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picKaydirilmis).BeginInit();
            SuspendLayout();
            // 
            // lblBaslikOrijinal
            // 
            lblBaslikOrijinal.AutoSize = true;
            lblBaslikOrijinal.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblBaslikOrijinal.Location = new Point(40, 20);
            lblBaslikOrijinal.Name = "lblBaslikOrijinal";
            lblBaslikOrijinal.Size = new Size(115, 23);
            lblBaslikOrijinal.TabIndex = 0;
            lblBaslikOrijinal.Text = "Orijinal Resim";
            // 
            // lblBaslikKaydirilmis
            // 
            lblBaslikKaydirilmis.AutoSize = true;
            lblBaslikKaydirilmis.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblBaslikKaydirilmis.Location = new Point(640, 20);
            lblBaslikKaydirilmis.Name = "lblBaslikKaydirilmis";
            lblBaslikKaydirilmis.Size = new Size(371, 23);
            lblBaslikKaydirilmis.TabIndex = 1;
            lblBaslikKaydirilmis.Text = "Kaydırma ile Döndürülmüş Resim (50 px Shear)";
            // 
            // picOrijinal
            // 
            picOrijinal.BorderStyle = BorderStyle.FixedSingle;
            picOrijinal.Location = new Point(40, 48);
            picOrijinal.Name = "picOrijinal";
            picOrijinal.Size = new Size(560, 480);
            picOrijinal.SizeMode = PictureBoxSizeMode.Zoom;
            picOrijinal.TabIndex = 2;
            picOrijinal.TabStop = false;
            // 
            // picKaydirilmis
            // 
            picKaydirilmis.BorderStyle = BorderStyle.FixedSingle;
            picKaydirilmis.Location = new Point(640, 48);
            picKaydirilmis.Name = "picKaydirilmis";
            picKaydirilmis.Size = new Size(560, 480);
            picKaydirilmis.TabIndex = 3;
            picKaydirilmis.TabStop = false;
            // 
            // btnResimSec
            // 
            btnResimSec.BackColor = SystemColors.Highlight;
            btnResimSec.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnResimSec.ForeColor = Color.White;
            btnResimSec.Location = new Point(40, 545);
            btnResimSec.Name = "btnResimSec";
            btnResimSec.Size = new Size(170, 40);
            btnResimSec.TabIndex = 4;
            btnResimSec.Text = "📁 Resim Seç";
            btnResimSec.UseVisualStyleBackColor = false;
            // 
            // btnSaatYonu
            // 
            btnSaatYonu.BackColor = SystemColors.Highlight;
            btnSaatYonu.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnSaatYonu.ForeColor = Color.White;
            btnSaatYonu.Location = new Point(220, 545);
            btnSaatYonu.Name = "btnSaatYonu";
            btnSaatYonu.Size = new Size(224, 40);
            btnSaatYonu.TabIndex = 5;
            btnSaatYonu.Text = "↻ Saat Yönünde (50 px)";
            btnSaatYonu.UseVisualStyleBackColor = false;
            // 
            // btnSaatTersi
            // 
            btnSaatTersi.BackColor = SystemColors.Highlight;
            btnSaatTersi.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnSaatTersi.ForeColor = Color.White;
            btnSaatTersi.Location = new Point(450, 545);
            btnSaatTersi.Name = "btnSaatTersi";
            btnSaatTersi.Size = new Size(224, 40);
            btnSaatTersi.TabIndex = 6;
            btnSaatTersi.Text = "↺ Saat Yönü Tersi (50 px)";
            btnSaatTersi.UseVisualStyleBackColor = false;
            // 
            // btnSifirla
            // 
            btnSifirla.BackColor = SystemColors.Highlight;
            btnSifirla.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnSifirla.ForeColor = Color.White;
            btnSifirla.Location = new Point(680, 545);
            btnSifirla.Name = "btnSifirla";
            btnSifirla.Size = new Size(130, 40);
            btnSifirla.TabIndex = 7;
            btnSifirla.Text = "🔄 Sıfırla";
            btnSifirla.UseVisualStyleBackColor = false;
            // 
            // lblBilgi
            // 
            lblBilgi.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblBilgi.Location = new Point(40, 588);
            lblBilgi.Name = "lblBilgi";
            lblBilgi.Size = new Size(1160, 60);
            lblBilgi.TabIndex = 8;
            // 
            // lnkGeri
            // 
            lnkGeri.AutoSize = true;
            lnkGeri.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lnkGeri.Location = new Point(12, 671);
            lnkGeri.Name = "lnkGeri";
            lnkGeri.Size = new Size(150, 23);
            lnkGeri.TabIndex = 9;
            lnkGeri.TabStop = true;
            lnkGeri.Text = "◀ Ödev 10'a Dön";
            // 
            // lnkSonraki
            // 
            lnkSonraki.AutoSize = true;
            lnkSonraki.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lnkSonraki.Location = new Point(1071, 674);
            lnkSonraki.Name = "lnkSonraki";
            lnkSonraki.Size = new Size(150, 23);
            lnkSonraki.TabIndex = 10;
            lnkSonraki.TabStop = true;
            lnkSonraki.Text = "Ödev 12'ye Git ▶";
            // 
            // Hafta3Odev11
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1232, 703);
            Controls.Add(lnkSonraki);
            Controls.Add(lnkGeri);
            Controls.Add(lblBilgi);
            Controls.Add(btnSifirla);
            Controls.Add(btnSaatTersi);
            Controls.Add(btnSaatYonu);
            Controls.Add(btnResimSec);
            Controls.Add(picKaydirilmis);
            Controls.Add(picOrijinal);
            Controls.Add(lblBaslikKaydirilmis);
            Controls.Add(lblBaslikOrijinal);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "Hafta3Odev11";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Hafta 3 - Ödev 11: Açı Olmadan Kaydırma ile Döndürme";
            ((System.ComponentModel.ISupportInitialize)picOrijinal).EndInit();
            ((System.ComponentModel.ISupportInitialize)picKaydirilmis).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblBaslikOrijinal;
        private Label lblBaslikKaydirilmis;
        private PictureBox picOrijinal;
        private PictureBox picKaydirilmis;
        private Button btnResimSec;
        private Button btnSaatYonu;
        private Button btnSaatTersi;
        private Button btnSifirla;
        private Label lblBilgi;
        private LinkLabel lnkGeri;
        private LinkLabel lnkSonraki;
    }
}