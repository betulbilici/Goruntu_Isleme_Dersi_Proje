namespace GoruntuIsleme
{
    partial class Hafta2Odev7
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
            btnAnalizEt = new Button();
            picHistR = new PictureBox();
            picHistG = new PictureBox();
            picHistB = new PictureBox();
            txtYorum = new TextBox();
            lnkGeri = new LinkLabel();
            lnkSonraki = new LinkLabel();
            btnYorumYap = new Button();
            ((System.ComponentModel.ISupportInitialize)picResim).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picHistR).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picHistG).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picHistB).BeginInit();
            SuspendLayout();
            // 
            // picResim
            // 
            picResim.BorderStyle = BorderStyle.FixedSingle;
            picResim.Location = new Point(12, 12);
            picResim.Name = "picResim";
            picResim.Size = new Size(537, 378);
            picResim.SizeMode = PictureBoxSizeMode.StretchImage;
            picResim.TabIndex = 0;
            picResim.TabStop = false;
            // 
            // btnResimSec
            // 
            btnResimSec.BackColor = SystemColors.Highlight;
            btnResimSec.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold);
            btnResimSec.ForeColor = Color.White;
            btnResimSec.Location = new Point(140, 396);
            btnResimSec.Name = "btnResimSec";
            btnResimSec.Size = new Size(130, 38);
            btnResimSec.TabIndex = 1;
            btnResimSec.Text = "Resim Seç";
            btnResimSec.UseVisualStyleBackColor = false;
            // 
            // btnAnalizEt
            // 
            btnAnalizEt.BackColor = SystemColors.Highlight;
            btnAnalizEt.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold);
            btnAnalizEt.ForeColor = Color.White;
            btnAnalizEt.Location = new Point(1042, 417);
            btnAnalizEt.Name = "btnAnalizEt";
            btnAnalizEt.Size = new Size(160, 38);
            btnAnalizEt.TabIndex = 2;
            btnAnalizEt.Text = "Kanalları Analiz Et";
            btnAnalizEt.UseVisualStyleBackColor = false;
            // 
            // picHistR
            // 
            picHistR.BackColor = Color.Black;
            picHistR.Location = new Point(590, 11);
            picHistR.Name = "picHistR";
            picHistR.Size = new Size(630, 120);
            picHistR.TabIndex = 3;
            picHistR.TabStop = false;
            // 
            // picHistG
            // 
            picHistG.BackColor = Color.Black;
            picHistG.Location = new Point(590, 151);
            picHistG.Name = "picHistG";
            picHistG.Size = new Size(630, 120);
            picHistG.TabIndex = 4;
            picHistG.TabStop = false;
            // 
            // picHistB
            // 
            picHistB.BackColor = Color.Black;
            picHistB.Location = new Point(590, 291);
            picHistB.Name = "picHistB";
            picHistB.Size = new Size(630, 120);
            picHistB.TabIndex = 5;
            picHistB.TabStop = false;
            // 
            // txtYorum
            // 
            txtYorum.Location = new Point(19, 474);
            txtYorum.Multiline = true;
            txtYorum.Name = "txtYorum";
            txtYorum.ReadOnly = true;
            txtYorum.ScrollBars = ScrollBars.Vertical;
            txtYorum.Size = new Size(1150, 175);
            txtYorum.TabIndex = 6;
            // 
            // lnkGeri
            // 
            lnkGeri.AutoSize = true;
            lnkGeri.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lnkGeri.Location = new Point(12, 671);
            lnkGeri.Name = "lnkGeri";
            lnkGeri.Size = new Size(149, 23);
            lnkGeri.TabIndex = 7;
            lnkGeri.TabStop = true;
            lnkGeri.Text = "◀ Ödev 6'ya Dön";
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
            lnkSonraki.Text = "Ödev 8'e Git ▶";
            
            // 
            // btnYorumYap
            // 
            btnYorumYap.BackColor = SystemColors.Highlight;
            btnYorumYap.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnYorumYap.ForeColor = Color.White;
            btnYorumYap.Location = new Point(425, 426);
            btnYorumYap.Name = "btnYorumYap";
            btnYorumYap.Size = new Size(293, 29);
            btnYorumYap.TabIndex = 9;
            btnYorumYap.Text = "Yorum ve Kullanım Alanları";
            btnYorumYap.UseVisualStyleBackColor = false;
            // 
            // Hafta2Odev7
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1232, 703);
            Controls.Add(btnYorumYap);
            Controls.Add(lnkSonraki);
            Controls.Add(lnkGeri);
            Controls.Add(txtYorum);
            Controls.Add(picHistB);
            Controls.Add(picHistG);
            Controls.Add(picHistR);
            Controls.Add(btnAnalizEt);
            Controls.Add(btnResimSec);
            Controls.Add(picResim);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "Hafta2Odev7";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Hafta2Odev7";
            ((System.ComponentModel.ISupportInitialize)picResim).EndInit();
            ((System.ComponentModel.ISupportInitialize)picHistR).EndInit();
            ((System.ComponentModel.ISupportInitialize)picHistG).EndInit();
            ((System.ComponentModel.ISupportInitialize)picHistB).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picResim;
        private Button btnResimSec;
        private Button btnAnalizEt;
        private PictureBox picHistR;
        private PictureBox picHistG;
        private PictureBox picHistB;
        private TextBox txtYorum;
        private LinkLabel lnkGeri;
        private LinkLabel lnkSonraki;
        private Button btnYorumYap;
    }
}