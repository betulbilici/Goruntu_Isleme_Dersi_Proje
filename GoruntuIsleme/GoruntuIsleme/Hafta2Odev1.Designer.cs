namespace GoruntuIsleme
{
    partial class Hafta2Odev1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Hafta2Odev1));
            picOrijinal = new PictureBox();
            picSonuc = new PictureBox();
            trackParlaklik = new TrackBar();
            label1 = new Label();
            label2 = new Label();
            lblDeger = new Label();
            btnResimSec = new Button();
            lnkOdev2 = new LinkLabel();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)picOrijinal).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picSonuc).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackParlaklik).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // picOrijinal
            // 
            picOrijinal.BorderStyle = BorderStyle.FixedSingle;
            picOrijinal.Location = new Point(15, 297);
            picOrijinal.Name = "picOrijinal";
            picOrijinal.Size = new Size(300, 300);
            picOrijinal.SizeMode = PictureBoxSizeMode.StretchImage;
            picOrijinal.TabIndex = 0;
            picOrijinal.TabStop = false;
            // 
            // picSonuc
            // 
            picSonuc.BorderStyle = BorderStyle.FixedSingle;
            picSonuc.Location = new Point(366, 297);
            picSonuc.Name = "picSonuc";
            picSonuc.Size = new Size(300, 300);
            picSonuc.SizeMode = PictureBoxSizeMode.StretchImage;
            picSonuc.TabIndex = 1;
            picSonuc.TabStop = false;
            // 
            // trackParlaklik
            // 
            trackParlaklik.Location = new Point(309, 637);
            trackParlaklik.Maximum = 255;
            trackParlaklik.Minimum = -255;
            trackParlaklik.Name = "trackParlaklik";
            trackParlaklik.Size = new Size(380, 56);
            trackParlaklik.TabIndex = 2;
            trackParlaklik.TickFrequency = 25;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label1.Location = new Point(92, 271);
            label1.Name = "label1";
            label1.Size = new Size(115, 23);
            label1.TabIndex = 3;
            label1.Text = "Orijinal Resim";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label2.Location = new Point(406, 271);
            label2.Name = "label2";
            label2.Size = new Size(220, 23);
            label2.TabIndex = 4;
            label2.Text = "Parlaklığı Değiştirilen Resim";
            // 
            // lblDeger
            // 
            lblDeger.AutoSize = true;
            lblDeger.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblDeger.Location = new Point(130, 637);
            lblDeger.Name = "lblDeger";
            lblDeger.Size = new Size(173, 25);
            lblDeger.TabIndex = 5;
            lblDeger.Text = "Parlaklık Değeri (b):";
            // 
            // btnResimSec
            // 
            btnResimSec.BackColor = SystemColors.Highlight;
            btnResimSec.FlatStyle = FlatStyle.Flat;
            btnResimSec.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            btnResimSec.ForeColor = Color.White;
            btnResimSec.Location = new Point(694, 542);
            btnResimSec.Name = "btnResimSec";
            btnResimSec.Size = new Size(132, 55);
            btnResimSec.TabIndex = 6;
            btnResimSec.Text = "Resim Seç";
            btnResimSec.UseVisualStyleBackColor = false;
            // 
            // lnkOdev2
            // 
            lnkOdev2.AutoSize = true;
            lnkOdev2.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lnkOdev2.Location = new Point(1081, 671);
            lnkOdev2.Name = "lnkOdev2";
            lnkOdev2.Size = new Size(140, 23);
            lnkOdev2.TabIndex = 7;
            lnkOdev2.TabStop = true;
            lnkOdev2.Text = "Ödev 2'ye Git ▶";
            lnkOdev2.LinkClicked += lnkOdev2_LinkClicked;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(12, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(616, 246);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 8;
            pictureBox1.TabStop = false;
            // 
            // Hafta2Odev1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1232, 703);
            Controls.Add(pictureBox1);
            Controls.Add(lnkOdev2);
            Controls.Add(btnResimSec);
            Controls.Add(lblDeger);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(trackParlaklik);
            Controls.Add(picSonuc);
            Controls.Add(picOrijinal);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "Hafta2Odev1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Hafta2Odev1";
            ((System.ComponentModel.ISupportInitialize)picOrijinal).EndInit();
            ((System.ComponentModel.ISupportInitialize)picSonuc).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackParlaklik).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picOrijinal;
        private PictureBox picSonuc;
        private TrackBar trackParlaklik;
        private Label label1;
        private Label label2;
        private Label lblDeger;
        private Button btnResimSec;
        private LinkLabel lnkOdev2;
        private PictureBox pictureBox1;
    }
}