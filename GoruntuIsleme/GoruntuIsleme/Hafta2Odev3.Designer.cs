namespace GoruntuIsleme
{
    partial class Hafta2Odev3
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Hafta2Odev3));
            picOrijinal = new PictureBox();
            picYeni = new PictureBox();
            picHistOrijinal = new PictureBox();
            picHistYeni = new PictureBox();
            trackAlt = new TrackBar();
            trackUst = new TrackBar();
            lblAralik = new Label();
            txtPikselBilgi = new TextBox();
            btnResimSec = new Button();
            lnkGeri = new LinkLabel();
            lnkSonraki = new LinkLabel();
            label1 = new Label();
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)picOrijinal).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picYeni).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picHistOrijinal).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picHistYeni).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackAlt).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackUst).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // picOrijinal
            // 
            picOrijinal.BorderStyle = BorderStyle.FixedSingle;
            picOrijinal.Location = new Point(30, 40);
            picOrijinal.Name = "picOrijinal";
            picOrijinal.Size = new Size(260, 220);
            picOrijinal.SizeMode = PictureBoxSizeMode.StretchImage;
            picOrijinal.TabIndex = 0;
            picOrijinal.TabStop = false;
            // 
            // picYeni
            // 
            picYeni.BorderStyle = BorderStyle.FixedSingle;
            picYeni.Location = new Point(330, 40);
            picYeni.Name = "picYeni";
            picYeni.Size = new Size(260, 220);
            picYeni.SizeMode = PictureBoxSizeMode.StretchImage;
            picYeni.TabIndex = 1;
            picYeni.TabStop = false;
            // 
            // picHistOrijinal
            // 
            picHistOrijinal.BackColor = Color.Black;
            picHistOrijinal.BorderStyle = BorderStyle.FixedSingle;
            picHistOrijinal.Location = new Point(30, 290);
            picHistOrijinal.Name = "picHistOrijinal";
            picHistOrijinal.Size = new Size(260, 180);
            picHistOrijinal.TabIndex = 2;
            picHistOrijinal.TabStop = false;
            // 
            // picHistYeni
            // 
            picHistYeni.BackColor = Color.Black;
            picHistYeni.BorderStyle = BorderStyle.FixedSingle;
            picHistYeni.Location = new Point(330, 290);
            picHistYeni.Name = "picHistYeni";
            picHistYeni.Size = new Size(260, 180);
            picHistYeni.TabIndex = 3;
            picHistYeni.TabStop = false;
            // 
            // trackAlt
            // 
            trackAlt.Location = new Point(330, 480);
            trackAlt.Maximum = 255;
            trackAlt.Name = "trackAlt";
            trackAlt.Size = new Size(260, 56);
            trackAlt.TabIndex = 4;
            // 
            // trackUst
            // 
            trackUst.Location = new Point(330, 515);
            trackUst.Maximum = 255;
            trackUst.Name = "trackUst";
            trackUst.Size = new Size(260, 56);
            trackUst.TabIndex = 5;
            trackUst.Value = 255;
            // 
            // lblAralik
            // 
            lblAralik.AutoSize = true;
            lblAralik.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblAralik.Location = new Point(330, 555);
            lblAralik.Name = "lblAralik";
            lblAralik.Size = new Size(210, 23);
            lblAralik.TabIndex = 6;
            lblAralik.Text = "Sıkıştırma Aralığı: [0 - 255]";
            // 
            // txtPikselBilgi
            // 
            txtPikselBilgi.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            txtPikselBilgi.Location = new Point(330, 580);
            txtPikselBilgi.Name = "txtPikselBilgi";
            txtPikselBilgi.ReadOnly = true;
            txtPikselBilgi.Size = new Size(363, 30);
            txtPikselBilgi.TabIndex = 7;
            // 
            // btnResimSec
            // 
            btnResimSec.BackColor = SystemColors.Highlight;
            btnResimSec.FlatStyle = FlatStyle.Flat;
            btnResimSec.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnResimSec.ForeColor = Color.White;
            btnResimSec.Location = new Point(30, 490);
            btnResimSec.Name = "btnResimSec";
            btnResimSec.Size = new Size(110, 35);
            btnResimSec.TabIndex = 8;
            btnResimSec.Text = "Resim Seç";
            btnResimSec.UseVisualStyleBackColor = false;
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
            lnkGeri.Text = "◀ Ödev 2'ye Dön";
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
            lnkSonraki.Text = "Ödev 4'e Git ▶";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label1.Location = new Point(151, 583);
            label1.Name = "label1";
            label1.Size = new Size(163, 23);
            label1.TabIndex = 11;
            label1.Text = "Seçilen Piksel Bilgisi:";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(644, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(576, 248);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 12;
            pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(644, 266);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(576, 245);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 13;
            pictureBox2.TabStop = false;
            // 
            // Hafta2Odev3
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1232, 703);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox1);
            Controls.Add(label1);
            Controls.Add(lnkSonraki);
            Controls.Add(lnkGeri);
            Controls.Add(btnResimSec);
            Controls.Add(txtPikselBilgi);
            Controls.Add(lblAralik);
            Controls.Add(trackUst);
            Controls.Add(trackAlt);
            Controls.Add(picHistYeni);
            Controls.Add(picHistOrijinal);
            Controls.Add(picYeni);
            Controls.Add(picOrijinal);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "Hafta2Odev3";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Hafta2Odev3";
            ((System.ComponentModel.ISupportInitialize)picOrijinal).EndInit();
            ((System.ComponentModel.ISupportInitialize)picYeni).EndInit();
            ((System.ComponentModel.ISupportInitialize)picHistOrijinal).EndInit();
            ((System.ComponentModel.ISupportInitialize)picHistYeni).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackAlt).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackUst).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picOrijinal;
        private PictureBox picYeni;
        private PictureBox picHistOrijinal;
        private PictureBox picHistYeni;
        private TrackBar trackAlt;
        private TrackBar trackUst;
        private Label lblAralik;
        private TextBox txtPikselBilgi;
        private Button btnResimSec;
        private LinkLabel lnkGeri;
        private LinkLabel lnkSonraki;
        private Label label1;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
    }
}