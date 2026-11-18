namespace GoruntuIsleme
{
    partial class Hafta2Odev2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Hafta2Odev2));
            picGiris = new PictureBox();
            picCikis = new PictureBox();
            picHistogram = new PictureBox();
            trackMin = new TrackBar();
            trackMax = new TrackBar();
            btnResimSec = new Button();
            cmbMod = new ComboBox();
            lblAralik = new Label();
            lnkGeri = new LinkLabel();
            lnkSonraki = new LinkLabel();
            pictureBox1 = new PictureBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)picGiris).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picCikis).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picHistogram).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackMin).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackMax).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // picGiris
            // 
            picGiris.BorderStyle = BorderStyle.FixedSingle;
            picGiris.Location = new Point(13, 265);
            picGiris.Name = "picGiris";
            picGiris.Size = new Size(280, 280);
            picGiris.SizeMode = PictureBoxSizeMode.StretchImage;
            picGiris.TabIndex = 0;
            picGiris.TabStop = false;
            // 
            // picCikis
            // 
            picCikis.BorderStyle = BorderStyle.FixedSingle;
            picCikis.Location = new Point(311, 265);
            picCikis.Name = "picCikis";
            picCikis.Size = new Size(280, 280);
            picCikis.SizeMode = PictureBoxSizeMode.StretchImage;
            picCikis.TabIndex = 1;
            picCikis.TabStop = false;
            // 
            // picHistogram
            // 
            picHistogram.BackColor = Color.Black;
            picHistogram.BorderStyle = BorderStyle.FixedSingle;
            picHistogram.Location = new Point(633, 265);
            picHistogram.Name = "picHistogram";
            picHistogram.Size = new Size(320, 280);
            picHistogram.TabIndex = 2;
            picHistogram.TabStop = false;
            // 
            // trackMin
            // 
            trackMin.Location = new Point(633, 551);
            trackMin.Maximum = 250;
            trackMin.Name = "trackMin";
            trackMin.Size = new Size(320, 56);
            trackMin.TabIndex = 3;
            trackMin.TickFrequency = 25;
            trackMin.Value = 50;
            // 
            // trackMax
            // 
            trackMax.Location = new Point(633, 613);
            trackMax.Maximum = 250;
            trackMax.Name = "trackMax";
            trackMax.Size = new Size(320, 56);
            trackMax.TabIndex = 4;
            trackMax.TickFrequency = 25;
            trackMax.Value = 200;
            // 
            // btnResimSec
            // 
            btnResimSec.BackColor = SystemColors.Highlight;
            btnResimSec.FlatStyle = FlatStyle.Flat;
            btnResimSec.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnResimSec.ForeColor = Color.White;
            btnResimSec.Location = new Point(1017, 493);
            btnResimSec.Name = "btnResimSec";
            btnResimSec.Size = new Size(112, 39);
            btnResimSec.TabIndex = 5;
            btnResimSec.Text = "Resim Seç";
            btnResimSec.UseVisualStyleBackColor = false;
            // 
            // cmbMod
            // 
            cmbMod.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMod.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 162);
            cmbMod.FormattingEnabled = true;
            cmbMod.Items.AddRange(new object[] { "Gri-Ton Eşikleme", "Renkli Eşikleme (RGB)" });
            cmbMod.Location = new Point(991, 317);
            cmbMod.Name = "cmbMod";
            cmbMod.Size = new Size(151, 28);
            cmbMod.TabIndex = 6;
            // 
            // lblAralik
            // 
            lblAralik.AutoSize = true;
            lblAralik.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblAralik.Location = new Point(469, 584);
            lblAralik.Name = "lblAralik";
            lblAralik.Size = new Size(137, 23);
            lblAralik.TabIndex = 7;
            lblAralik.Text = "Aralık: [50 - 200]";
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
            lnkGeri.Text = "◀ Ödev 1'e Dön";
            // 
            // lnkSonraki
            // 
            lnkSonraki.AutoSize = true;
            lnkSonraki.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lnkSonraki.Location = new Point(1081, 671);
            lnkSonraki.Name = "lnkSonraki";
            lnkSonraki.Size = new Size(131, 23);
            lnkSonraki.TabIndex = 9;
            lnkSonraki.TabStop = true;
            lnkSonraki.Text = "Ödev 3'e Git ▶";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(12, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(636, 222);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 10;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label1.Location = new Point(81, 239);
            label1.Name = "label1";
            label1.Size = new Size(115, 23);
            label1.TabIndex = 11;
            label1.Text = "Orijinal Resim";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label2.Location = new Point(359, 240);
            label2.Name = "label2";
            label2.Size = new Size(140, 23);
            label2.TabIndex = 12;
            label2.Text = "Eşiklenmiş Sonuç";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label3.Location = new Point(724, 239);
            label3.Name = "label3";
            label3.Size = new Size(146, 23);
            label3.TabIndex = 13;
            label3.Text = "Histogram Grafiği";
            // 
            // Hafta2Odev2
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1232, 703);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            Controls.Add(lnkSonraki);
            Controls.Add(lnkGeri);
            Controls.Add(lblAralik);
            Controls.Add(cmbMod);
            Controls.Add(btnResimSec);
            Controls.Add(trackMax);
            Controls.Add(trackMin);
            Controls.Add(picHistogram);
            Controls.Add(picCikis);
            Controls.Add(picGiris);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "Hafta2Odev2";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Hafta2Odev2";
            ((System.ComponentModel.ISupportInitialize)picGiris).EndInit();
            ((System.ComponentModel.ISupportInitialize)picCikis).EndInit();
            ((System.ComponentModel.ISupportInitialize)picHistogram).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackMin).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackMax).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picGiris;
        private PictureBox picCikis;
        private PictureBox picHistogram;
        private TrackBar trackMin;
        private TrackBar trackMax;
        private Button btnResimSec;
        private ComboBox cmbMod;
        private Label lblAralik;
        private LinkLabel lnkGeri;
        private LinkLabel lnkSonraki;
        private PictureBox pictureBox1;
        private Label label1;
        private Label label2;
        private Label label3;
    }
}