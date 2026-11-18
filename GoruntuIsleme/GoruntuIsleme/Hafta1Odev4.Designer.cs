namespace GoruntuIsleme
{
    partial class Hafta1Odev4
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Hafta1Odev4));
            picGri = new PictureBox();
            picMozaik = new PictureBox();
            cmbMozaikBoyut = new ComboBox();
            numI = new NumericUpDown();
            numJ = new NumericUpDown();
            lblPikselDegeri = new Label();
            btnResimSec = new Button();
            btnMozaikOlustur = new Button();
            lnkGeri = new LinkLabel();
            lnkOdev5 = new LinkLabel();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)picGri).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picMozaik).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numI).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numJ).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // picGri
            // 
            picGri.BorderStyle = BorderStyle.FixedSingle;
            picGri.Location = new Point(21, 292);
            picGri.Name = "picGri";
            picGri.Size = new Size(320, 320);
            picGri.SizeMode = PictureBoxSizeMode.StretchImage;
            picGri.TabIndex = 0;
            picGri.TabStop = false;
            // 
            // picMozaik
            // 
            picMozaik.BorderStyle = BorderStyle.FixedSingle;
            picMozaik.Location = new Point(392, 292);
            picMozaik.Name = "picMozaik";
            picMozaik.Size = new Size(320, 320);
            picMozaik.SizeMode = PictureBoxSizeMode.StretchImage;
            picMozaik.TabIndex = 1;
            picMozaik.TabStop = false;
            // 
            // cmbMozaikBoyut
            // 
            cmbMozaikBoyut.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMozaikBoyut.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 162);
            cmbMozaikBoyut.FormattingEnabled = true;
            cmbMozaikBoyut.Items.AddRange(new object[] { "2", "5", "10", "25" });
            cmbMozaikBoyut.Location = new Point(910, 325);
            cmbMozaikBoyut.Name = "cmbMozaikBoyut";
            cmbMozaikBoyut.Size = new Size(151, 31);
            cmbMozaikBoyut.TabIndex = 2;
            // 
            // numI
            // 
            numI.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 162);
            numI.Location = new Point(910, 411);
            numI.Name = "numI";
            numI.Size = new Size(150, 30);
            numI.TabIndex = 3;
            // 
            // numJ
            // 
            numJ.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 162);
            numJ.Location = new Point(910, 489);
            numJ.Name = "numJ";
            numJ.Size = new Size(150, 30);
            numJ.TabIndex = 4;
            // 
            // lblPikselDegeri
            // 
            lblPikselDegeri.AutoSize = true;
            lblPikselDegeri.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblPikselDegeri.ForeColor = Color.Maroon;
            lblPikselDegeri.Location = new Point(738, 589);
            lblPikselDegeri.Name = "lblPikselDegeri";
            lblPikselDegeri.Size = new Size(293, 23);
            lblPikselDegeri.TabIndex = 5;
            lblPikselDegeri.Text = "Piksel gri seviye değeri f_gray(i, j) = -";
            // 
            // btnResimSec
            // 
            btnResimSec.BackColor = SystemColors.Highlight;
            btnResimSec.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            btnResimSec.ForeColor = Color.White;
            btnResimSec.Location = new Point(1088, 325);
            btnResimSec.Name = "btnResimSec";
            btnResimSec.Size = new Size(114, 58);
            btnResimSec.TabIndex = 6;
            btnResimSec.Text = "Resim Seç";
            btnResimSec.UseVisualStyleBackColor = false;
            // 
            // btnMozaikOlustur
            // 
            btnMozaikOlustur.BackColor = SystemColors.Highlight;
            btnMozaikOlustur.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            btnMozaikOlustur.ForeColor = Color.White;
            btnMozaikOlustur.Location = new Point(1088, 411);
            btnMozaikOlustur.Name = "btnMozaikOlustur";
            btnMozaikOlustur.Size = new Size(114, 76);
            btnMozaikOlustur.TabIndex = 7;
            btnMozaikOlustur.Text = "Mozaik Oluştur";
            btnMozaikOlustur.UseVisualStyleBackColor = false;
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
            lnkGeri.Text = "◀ Ödev 3'e Dön";
            // 
            // lnkOdev5
            // 
            lnkOdev5.AutoSize = true;
            lnkOdev5.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lnkOdev5.Location = new Point(1081, 671);
            lnkOdev5.Name = "lnkOdev5";
            lnkOdev5.Size = new Size(131, 23);
            lnkOdev5.TabIndex = 9;
            lnkOdev5.TabStop = true;
            lnkOdev5.Text = "Ödev 5'e Git ▶";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label1.Location = new Point(738, 299);
            label1.Name = "label1";
            label1.Size = new Size(187, 23);
            label1.TabIndex = 10;
            label1.Text = "Mozaik Boyutu (Adım):";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label2.Location = new Point(738, 387);
            label2.Name = "label2";
            label2.Size = new Size(153, 23);
            label2.TabIndex = 11;
            label2.Text = "Satır Koordinatı (i):";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label3.Location = new Point(738, 464);
            label3.Name = "label3";
            label3.Size = new Size(166, 23);
            label3.TabIndex = 12;
            label3.Text = "Sütun Koordinatı (j):";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(12, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(849, 248);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 13;
            pictureBox1.TabStop = false;
            // 
            // Hafta1Odev4
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1232, 703);
            Controls.Add(pictureBox1);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(lnkOdev5);
            Controls.Add(lnkGeri);
            Controls.Add(btnMozaikOlustur);
            Controls.Add(btnResimSec);
            Controls.Add(lblPikselDegeri);
            Controls.Add(numJ);
            Controls.Add(numI);
            Controls.Add(cmbMozaikBoyut);
            Controls.Add(picMozaik);
            Controls.Add(picGri);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "Hafta1Odev4";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Hafta1Odev4";
            ((System.ComponentModel.ISupportInitialize)picGri).EndInit();
            ((System.ComponentModel.ISupportInitialize)picMozaik).EndInit();
            ((System.ComponentModel.ISupportInitialize)numI).EndInit();
            ((System.ComponentModel.ISupportInitialize)numJ).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picGri;
        private PictureBox picMozaik;
        private ComboBox cmbMozaikBoyut;
        private NumericUpDown numI;
        private NumericUpDown numJ;
        private Label lblPikselDegeri;
        private Button btnResimSec;
        private Button btnMozaikOlustur;
        private LinkLabel lnkGeri;
        private LinkLabel lnkOdev5;
        private Label label1;
        private Label label2;
        private Label label3;
        private PictureBox pictureBox1;
    }
}