namespace GoruntuIsleme
{
    partial class Hafta3Odev13
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
            lblBaslikKirpilan = new Label();
            picOrijinal = new PictureBox();
            picKirpilan = new PictureBox();
            btnResimSec = new Button();
            btnKirp = new Button();
            btnSifirla = new Button();
            lblBilgi = new Label();
            lnkGeri = new LinkLabel();
            lnkSonraki = new LinkLabel();
            ((System.ComponentModel.ISupportInitialize)picOrijinal).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picKirpilan).BeginInit();
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
            // lblBaslikKirpilan
            // 
            lblBaslikKirpilan.AutoSize = true;
            lblBaslikKirpilan.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblBaslikKirpilan.Location = new Point(640, 20);
            lblBaslikKirpilan.Name = "lblBaslikKirpilan";
            lblBaslikKirpilan.Size = new Size(119, 23);
            lblBaslikKirpilan.TabIndex = 1;
            lblBaslikKirpilan.Text = "Kırpılan Sonuç";
            // 
            // picOrijinal
            // 
            picOrijinal.BorderStyle = BorderStyle.FixedSingle;
            picOrijinal.Location = new Point(40, 45);
            picOrijinal.Name = "picOrijinal";
            picOrijinal.Size = new Size(560, 480);
            picOrijinal.SizeMode = PictureBoxSizeMode.Zoom;
            picOrijinal.TabIndex = 2;
            picOrijinal.TabStop = false;
            // 
            // picKirpilan
            // 
            picKirpilan.BorderStyle = BorderStyle.FixedSingle;
            picKirpilan.Location = new Point(640, 48);
            picKirpilan.Name = "picKirpilan";
            picKirpilan.Size = new Size(560, 480);
            picKirpilan.SizeMode = PictureBoxSizeMode.Zoom;
            picKirpilan.TabIndex = 3;
            picKirpilan.TabStop = false;
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
            // btnKirp
            // 
            btnKirp.BackColor = SystemColors.Highlight;
            btnKirp.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnKirp.ForeColor = Color.White;
            btnKirp.Location = new Point(220, 545);
            btnKirp.Name = "btnKirp";
            btnKirp.Size = new Size(200, 40);
            btnKirp.TabIndex = 5;
            btnKirp.Text = "✂️ Seçilen Alanı Kırp";
            btnKirp.UseVisualStyleBackColor = false;
            // 
            // btnSifirla
            // 
            btnSifirla.BackColor = SystemColors.Highlight;
            btnSifirla.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnSifirla.ForeColor = Color.White;
            btnSifirla.Location = new Point(430, 545);
            btnSifirla.Name = "btnSifirla";
            btnSifirla.Size = new Size(140, 40);
            btnSifirla.TabIndex = 6;
            btnSifirla.Text = "🔄 Sıfırla";
            btnSifirla.UseVisualStyleBackColor = false;
            // 
            // lblBilgi
            // 
            lblBilgi.BorderStyle = BorderStyle.Fixed3D;
            lblBilgi.Location = new Point(40, 600);
            lblBilgi.Name = "lblBilgi";
            lblBilgi.Size = new Size(1160, 55);
            lblBilgi.TabIndex = 7;
            // 
            // lnkGeri
            // 
            lnkGeri.AutoSize = true;
            lnkGeri.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lnkGeri.Location = new Point(12, 671);
            lnkGeri.Name = "lnkGeri";
            lnkGeri.Size = new Size(159, 23);
            lnkGeri.TabIndex = 8;
            lnkGeri.TabStop = true;
            lnkGeri.Text = "◀ Ödev 12'ye Dön";
            // 
            // lnkSonraki
            // 
            lnkSonraki.AutoSize = true;
            lnkSonraki.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lnkSonraki.Location = new Point(1081, 671);
            lnkSonraki.Name = "lnkSonraki";
            lnkSonraki.Size = new Size(151, 23);
            lnkSonraki.TabIndex = 9;
            lnkSonraki.TabStop = true;
            lnkSonraki.Text = "Ana Sayfaya Dön ";
            // 
            // Hafta3Odev13
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1232, 703);
            Controls.Add(lnkSonraki);
            Controls.Add(lnkGeri);
            Controls.Add(lblBilgi);
            Controls.Add(btnSifirla);
            Controls.Add(btnKirp);
            Controls.Add(btnResimSec);
            Controls.Add(picKirpilan);
            Controls.Add(picOrijinal);
            Controls.Add(lblBaslikKirpilan);
            Controls.Add(lblBaslikOrijinal);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "Hafta3Odev13";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Hafta 3 - Ödev 13: Mouse Takip Eden Çizgi ile Kırpma";
            ((System.ComponentModel.ISupportInitialize)picOrijinal).EndInit();
            ((System.ComponentModel.ISupportInitialize)picKirpilan).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblBaslikOrijinal;
        private Label lblBaslikKirpilan;
        private PictureBox picOrijinal;
        private PictureBox picKirpilan;
        private Button btnResimSec;
        private Button btnKirp;
        private Button btnSifirla;
        private Label lblBilgi;
        private LinkLabel lnkGeri;
        private LinkLabel lnkSonraki;
    }
}