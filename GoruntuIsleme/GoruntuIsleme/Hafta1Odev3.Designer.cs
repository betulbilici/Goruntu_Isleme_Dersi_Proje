namespace GoruntuIsleme
{
    partial class Hafta1Odev3
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Hafta1Odev3));
            picOrijinal = new PictureBox();
            picSonuc = new PictureBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            cmbOrnekleme = new ComboBox();
            label4 = new Label();
            cmbNicemleme = new ComboBox();
            btnResimSec = new Button();
            btnUygula = new Button();
            lnkGeri = new LinkLabel();
            lnkOdev4 = new LinkLabel();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)picOrijinal).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picSonuc).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // picOrijinal
            // 
            picOrijinal.BorderStyle = BorderStyle.FixedSingle;
            picOrijinal.Location = new Point(21, 274);
            picOrijinal.Name = "picOrijinal";
            picOrijinal.Size = new Size(350, 350);
            picOrijinal.SizeMode = PictureBoxSizeMode.StretchImage;
            picOrijinal.TabIndex = 0;
            picOrijinal.TabStop = false;
            // 
            // picSonuc
            // 
            picSonuc.BorderStyle = BorderStyle.FixedSingle;
            picSonuc.Location = new Point(400, 274);
            picSonuc.Name = "picSonuc";
            picSonuc.Size = new Size(350, 350);
            picSonuc.SizeMode = PictureBoxSizeMode.StretchImage;
            picSonuc.TabIndex = 1;
            picSonuc.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label1.Location = new Point(73, 248);
            label1.Name = "label1";
            label1.Size = new Size(209, 23);
            label1.TabIndex = 2;
            label1.Text = "Orijinal Resim (1000x1000)";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label2.Location = new Point(490, 248);
            label2.Name = "label2";
            label2.Size = new Size(122, 23);
            label2.TabIndex = 3;
            label2.Text = "İşlenmiş Resim";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label3.Location = new Point(780, 326);
            label3.Name = "label3";
            label3.Size = new Size(147, 23);
            label3.TabIndex = 4;
            label3.Text = "Örnekleme Adımı:";
            // 
            // cmbOrnekleme
            // 
            cmbOrnekleme.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbOrnekleme.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 162);
            cmbOrnekleme.FormattingEnabled = true;
            cmbOrnekleme.Items.AddRange(new object[] { "1 (Orijinal)", "2 piksel", "5 piksel", "10 piksel", "25 piksel", "50 piksel" });
            cmbOrnekleme.Location = new Point(949, 326);
            cmbOrnekleme.Name = "cmbOrnekleme";
            cmbOrnekleme.Size = new Size(151, 31);
            cmbOrnekleme.TabIndex = 5;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label4.Location = new Point(780, 408);
            label4.Name = "label4";
            label4.Size = new Size(144, 23);
            label4.TabIndex = 6;
            label4.Text = "Renk Nicemleme:";
            // 
            // cmbNicemleme
            // 
            cmbNicemleme.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbNicemleme.FormattingEnabled = true;
            cmbNicemleme.Items.AddRange(new object[] { "0 (Yok)", "10 katları", "50 katları", "100 katları" });
            cmbNicemleme.Location = new Point(949, 403);
            cmbNicemleme.Name = "cmbNicemleme";
            cmbNicemleme.Size = new Size(151, 28);
            cmbNicemleme.TabIndex = 7;
            // 
            // btnResimSec
            // 
            btnResimSec.BackColor = SystemColors.Highlight;
            btnResimSec.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnResimSec.ForeColor = Color.White;
            btnResimSec.Location = new Point(780, 522);
            btnResimSec.Name = "btnResimSec";
            btnResimSec.Size = new Size(109, 61);
            btnResimSec.TabIndex = 8;
            btnResimSec.Text = "Resim Yükle";
            btnResimSec.UseVisualStyleBackColor = false;
            // 
            // btnUygula
            // 
            btnUygula.BackColor = SystemColors.Highlight;
            btnUygula.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold);
            btnUygula.ForeColor = Color.White;
            btnUygula.Location = new Point(991, 523);
            btnUygula.Name = "btnUygula";
            btnUygula.Size = new Size(109, 61);
            btnUygula.TabIndex = 9;
            btnUygula.Text = "Uygula / Dönüştür";
            btnUygula.UseVisualStyleBackColor = false;
            // 
            // lnkGeri
            // 
            lnkGeri.AutoSize = true;
            lnkGeri.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lnkGeri.Location = new Point(12, 671);
            lnkGeri.Name = "lnkGeri";
            lnkGeri.Size = new Size(149, 23);
            lnkGeri.TabIndex = 10;
            lnkGeri.TabStop = true;
            lnkGeri.Text = "◀ Ödev 2'ye Dön";
            // 
            // lnkOdev4
            // 
            lnkOdev4.AutoSize = true;
            lnkOdev4.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lnkOdev4.Location = new Point(1081, 671);
            lnkOdev4.Name = "lnkOdev4";
            lnkOdev4.Size = new Size(131, 23);
            lnkOdev4.TabIndex = 11;
            lnkOdev4.TabStop = true;
            lnkOdev4.Text = "Ödev 4'e Git ▶";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(12, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(1054, 179);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 12;
            pictureBox1.TabStop = false;
            // 
            // Hafta1Odev3
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1232, 703);
            Controls.Add(pictureBox1);
            Controls.Add(lnkOdev4);
            Controls.Add(lnkGeri);
            Controls.Add(btnUygula);
            Controls.Add(btnResimSec);
            Controls.Add(cmbNicemleme);
            Controls.Add(label4);
            Controls.Add(cmbOrnekleme);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(picSonuc);
            Controls.Add(picOrijinal);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "Hafta1Odev3";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Hafta1Odev3";
            ((System.ComponentModel.ISupportInitialize)picOrijinal).EndInit();
            ((System.ComponentModel.ISupportInitialize)picSonuc).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picOrijinal;
        private PictureBox picSonuc;
        private Label label1;
        private Label label2;
        private Label label3;
        private ComboBox cmbOrnekleme;
        private Label label4;
        private ComboBox cmbNicemleme;
        private Button btnResimSec;
        private Button btnUygula;
        private LinkLabel lnkGeri;
        private LinkLabel lnkOdev4;
        private PictureBox pictureBox1;
    }
}