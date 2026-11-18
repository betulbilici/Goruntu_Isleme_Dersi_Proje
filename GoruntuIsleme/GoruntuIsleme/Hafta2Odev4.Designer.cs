namespace GoruntuIsleme
{
    partial class Hafta2Odev4
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Hafta2Odev4));
            picGiris = new PictureBox();
            picCikis = new PictureBox();
            picHistogram = new PictureBox();
            btnResimSec = new Button();
            btnKontrast = new Button();
            txtX1 = new TextBox();
            txtX2 = new TextBox();
            txtY1 = new TextBox();
            txtY2 = new TextBox();
            lblBilgi = new Label();
            lnkGeri = new LinkLabel();
            lnkSonraki = new LinkLabel();
            pictureBox1 = new PictureBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            ((System.ComponentModel.ISupportInitialize)picGiris).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picCikis).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picHistogram).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // picGiris
            // 
            picGiris.BorderStyle = BorderStyle.FixedSingle;
            picGiris.Location = new Point(10, 258);
            picGiris.Name = "picGiris";
            picGiris.Size = new Size(260, 240);
            picGiris.SizeMode = PictureBoxSizeMode.StretchImage;
            picGiris.TabIndex = 0;
            picGiris.TabStop = false;
            // 
            // picCikis
            // 
            picCikis.BorderStyle = BorderStyle.FixedSingle;
            picCikis.Location = new Point(290, 258);
            picCikis.Name = "picCikis";
            picCikis.Size = new Size(260, 240);
            picCikis.SizeMode = PictureBoxSizeMode.StretchImage;
            picCikis.TabIndex = 1;
            picCikis.TabStop = false;
            // 
            // picHistogram
            // 
            picHistogram.Location = new Point(570, 258);
            picHistogram.Name = "picHistogram";
            picHistogram.Size = new Size(320, 240);
            picHistogram.TabIndex = 2;
            picHistogram.TabStop = false;
            // 
            // btnResimSec
            // 
            btnResimSec.BackColor = SystemColors.Highlight;
            btnResimSec.FlatStyle = FlatStyle.Flat;
            btnResimSec.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnResimSec.ForeColor = Color.White;
            btnResimSec.Location = new Point(10, 518);
            btnResimSec.Name = "btnResimSec";
            btnResimSec.Size = new Size(110, 35);
            btnResimSec.TabIndex = 3;
            btnResimSec.Text = "Resim Seç";
            btnResimSec.UseVisualStyleBackColor = false;
            // 
            // btnKontrast
            // 
            btnKontrast.BackColor = SystemColors.Highlight;
            btnKontrast.FlatStyle = FlatStyle.Flat;
            btnKontrast.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnKontrast.ForeColor = Color.White;
            btnKontrast.Location = new Point(290, 518);
            btnKontrast.Name = "btnKontrast";
            btnKontrast.Size = new Size(140, 59);
            btnKontrast.TabIndex = 4;
            btnKontrast.Text = "Kontrast Uygula";
            btnKontrast.UseVisualStyleBackColor = false;
            // 
            // txtX1
            // 
            txtX1.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            txtX1.Location = new Point(630, 518);
            txtX1.Name = "txtX1";
            txtX1.Size = new Size(125, 30);
            txtX1.TabIndex = 5;
            txtX1.Text = "0";
            // 
            // txtX2
            // 
            txtX2.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            txtX2.Location = new Point(630, 553);
            txtX2.Name = "txtX2";
            txtX2.Size = new Size(125, 30);
            txtX2.TabIndex = 6;
            txtX2.Text = "255";
            // 
            // txtY1
            // 
            txtY1.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            txtY1.Location = new Point(851, 518);
            txtY1.Name = "txtY1";
            txtY1.ReadOnly = true;
            txtY1.Size = new Size(125, 30);
            txtY1.TabIndex = 7;
            txtY1.Text = "0";
            // 
            // txtY2
            // 
            txtY2.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            txtY2.Location = new Point(851, 553);
            txtY2.Name = "txtY2";
            txtY2.ReadOnly = true;
            txtY2.Size = new Size(125, 27);
            txtY2.TabIndex = 8;
            txtY2.Text = "255";
            // 
            // lblBilgi
            // 
            lblBilgi.AutoSize = true;
            lblBilgi.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblBilgi.Location = new Point(570, 593);
            lblBilgi.Name = "lblBilgi";
            lblBilgi.Size = new Size(344, 23);
            lblBilgi.TabIndex = 9;
            lblBilgi.Text = "Histogram üzerinde Sol Tık: X1 | Sağ Tık: X2";
            // 
            // lnkGeri
            // 
            lnkGeri.AutoSize = true;
            lnkGeri.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lnkGeri.Location = new Point(12, 671);
            lnkGeri.Name = "lnkGeri";
            lnkGeri.Size = new Size(140, 23);
            lnkGeri.TabIndex = 10;
            lnkGeri.TabStop = true;
            lnkGeri.Text = "◀ Ödev 3'e Dön";
            // 
            // lnkSonraki
            // 
            lnkSonraki.AutoSize = true;
            lnkSonraki.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lnkSonraki.Location = new Point(1081, 671);
            lnkSonraki.Name = "lnkSonraki";
            lnkSonraki.Size = new Size(131, 23);
            lnkSonraki.TabIndex = 11;
            lnkSonraki.TabStop = true;
            lnkSonraki.Text = "Ödev 5'e Git ▶";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(3, 2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(851, 230);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 12;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label1.Location = new Point(572, 524);
            label1.Name = "label1";
            label1.Size = new Size(54, 23);
            label1.TabIndex = 13;
            label1.Text = "X1-->";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label2.Location = new Point(572, 560);
            label2.Name = "label2";
            label2.Size = new Size(56, 23);
            label2.TabIndex = 14;
            label2.Text = "X2-->";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label3.Location = new Point(794, 524);
            label3.Name = "label3";
            label3.Size = new Size(53, 23);
            label3.TabIndex = 15;
            label3.Text = "Y1-->";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label4.Location = new Point(792, 560);
            label4.Name = "label4";
            label4.Size = new Size(55, 23);
            label4.TabIndex = 16;
            label4.Text = "Y2-->";
            // 
            // Hafta2Odev4
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1232, 703);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            Controls.Add(lnkSonraki);
            Controls.Add(lnkGeri);
            Controls.Add(lblBilgi);
            Controls.Add(txtY2);
            Controls.Add(txtY1);
            Controls.Add(txtX2);
            Controls.Add(txtX1);
            Controls.Add(btnKontrast);
            Controls.Add(btnResimSec);
            Controls.Add(picHistogram);
            Controls.Add(picCikis);
            Controls.Add(picGiris);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "Hafta2Odev4";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Hafta2Odev4";
            ((System.ComponentModel.ISupportInitialize)picGiris).EndInit();
            ((System.ComponentModel.ISupportInitialize)picCikis).EndInit();
            ((System.ComponentModel.ISupportInitialize)picHistogram).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picGiris;
        private PictureBox picCikis;
        private PictureBox picHistogram;
        private Button btnResimSec;
        private Button btnKontrast;
        private TextBox txtX1;
        private TextBox txtX2;
        private TextBox txtY1;
        private TextBox txtY2;
        private Label lblBilgi;
        private LinkLabel lnkGeri;
        private LinkLabel lnkSonraki;
        private PictureBox pictureBox1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
    }
}