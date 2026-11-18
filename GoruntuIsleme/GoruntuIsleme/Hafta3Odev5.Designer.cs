namespace GoruntuIsleme
{
    partial class Hafta3Odev5
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
            picKaynak = new PictureBox();
            picKirpilan = new PictureBox();
            btnResimSec = new Button();
            btnKirp = new Button();
            btnSifirla = new Button();
            lblBilgi = new Label();
            lnkGeri = new LinkLabel();
            lnkSonraki = new LinkLabel();
            ((System.ComponentModel.ISupportInitialize)picKaynak).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picKirpilan).BeginInit();
            SuspendLayout();
            // 
            // picKaynak
            // 
            picKaynak.BackColor = Color.White;
            picKaynak.BorderStyle = BorderStyle.FixedSingle;
            picKaynak.Location = new Point(12, 25);
            picKaynak.Name = "picKaynak";
            picKaynak.Size = new Size(450, 450);
            picKaynak.SizeMode = PictureBoxSizeMode.CenterImage;
            picKaynak.TabIndex = 0;
            picKaynak.TabStop = false;
            // 
            // picKirpilan
            // 
            picKirpilan.BackColor = Color.White;
            picKirpilan.BorderStyle = BorderStyle.FixedSingle;
            picKirpilan.Location = new Point(478, 25);
            picKirpilan.Name = "picKirpilan";
            picKirpilan.Size = new Size(450, 450);
            picKirpilan.SizeMode = PictureBoxSizeMode.CenterImage;
            picKirpilan.TabIndex = 1;
            picKirpilan.TabStop = false;
            // 
            // btnResimSec
            // 
            btnResimSec.BackColor = SystemColors.Highlight;
            btnResimSec.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnResimSec.ForeColor = Color.White;
            btnResimSec.Location = new Point(1012, 52);
            btnResimSec.Name = "btnResimSec";
            btnResimSec.Size = new Size(181, 48);
            btnResimSec.TabIndex = 2;
            btnResimSec.Text = "📁 Resim Seç";
            btnResimSec.UseVisualStyleBackColor = false;
            // 
            // btnKirp
            // 
            btnKirp.BackColor = SystemColors.Highlight;
            btnKirp.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnKirp.ForeColor = Color.White;
            btnKirp.Location = new Point(1012, 110);
            btnKirp.Name = "btnKirp";
            btnKirp.Size = new Size(181, 48);
            btnKirp.TabIndex = 3;
            btnKirp.Text = "✂️ Seçilen Alanı Kırp";
            btnKirp.UseVisualStyleBackColor = false;
            // 
            // btnSifirla
            // 
            btnSifirla.BackColor = SystemColors.Highlight;
            btnSifirla.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnSifirla.ForeColor = Color.White;
            btnSifirla.Location = new Point(1012, 164);
            btnSifirla.Name = "btnSifirla";
            btnSifirla.Size = new Size(181, 48);
            btnSifirla.TabIndex = 4;
            btnSifirla.Text = "🔄 Sıfırla";
            btnSifirla.UseVisualStyleBackColor = false;
            // 
            // lblBilgi
            // 
            lblBilgi.BackColor = Color.White;
            lblBilgi.BorderStyle = BorderStyle.Fixed3D;
            lblBilgi.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblBilgi.Location = new Point(947, 235);
            lblBilgi.Name = "lblBilgi";
            lblBilgi.Size = new Size(273, 264);
            lblBilgi.TabIndex = 5;
            // 
            // lnkGeri
            // 
            lnkGeri.AutoSize = true;
            lnkGeri.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lnkGeri.Location = new Point(12, 671);
            lnkGeri.Name = "lnkGeri";
            lnkGeri.Size = new Size(140, 23);
            lnkGeri.TabIndex = 6;
            lnkGeri.TabStop = true;
            lnkGeri.Text = "◀ Ödev 4'e Dön";
            // 
            // lnkSonraki
            // 
            lnkSonraki.AutoSize = true;
            lnkSonraki.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lnkSonraki.Location = new Point(1081, 671);
            lnkSonraki.Name = "lnkSonraki";
            lnkSonraki.Size = new Size(140, 23);
            lnkSonraki.TabIndex = 7;
            lnkSonraki.TabStop = true;
            lnkSonraki.Text = "Ödev 6'ya Git ▶";
            // 
            // Hafta3Odev5
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
            Controls.Add(picKaynak);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "Hafta3Odev5";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Hafta 3 - Ödev 5: İki Nokta ile Dikdörtgen Kırpma (Crop)";
            ((System.ComponentModel.ISupportInitialize)picKaynak).EndInit();
            ((System.ComponentModel.ISupportInitialize)picKirpilan).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picKaynak;
        private PictureBox picKirpilan;
        private Button btnResimSec;
        private Button btnKirp;
        private Button btnSifirla;
        private Label lblBilgi;
        private LinkLabel lnkGeri;
        private LinkLabel lnkSonraki;
    }
}