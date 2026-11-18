namespace GoruntuIsleme
{
    partial class Hafta3Odev7
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
            picHedef = new PictureBox();
            btnResimSec = new Button();
            lblAci = new Label();
            txtAci = new TextBox();
            btnDondur = new Button();
            lnkGeri = new LinkLabel();
            lnkSonraki = new LinkLabel();
            label1 = new Label();
            label2 = new Label();
            lblBilgi = new Label();
            ((System.ComponentModel.ISupportInitialize)picKaynak).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picHedef).BeginInit();
            SuspendLayout();
            // 
            // picKaynak
            // 
            picKaynak.BorderStyle = BorderStyle.FixedSingle;
            picKaynak.Location = new Point(40, 50);
            picKaynak.Name = "picKaynak";
            picKaynak.Size = new Size(548, 413);
            picKaynak.TabIndex = 0;
            picKaynak.TabStop = false;
            // 
            // picHedef
            // 
            picHedef.BorderStyle = BorderStyle.FixedSingle;
            picHedef.Location = new Point(640, 50);
            picHedef.Name = "picHedef";
            picHedef.Size = new Size(548, 413);
            picHedef.TabIndex = 1;
            picHedef.TabStop = false;
            // 
            // btnResimSec
            // 
            btnResimSec.BackColor = SystemColors.Highlight;
            btnResimSec.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnResimSec.ForeColor = Color.White;
            btnResimSec.Location = new Point(66, 496);
            btnResimSec.Name = "btnResimSec";
            btnResimSec.Size = new Size(157, 47);
            btnResimSec.TabIndex = 2;
            btnResimSec.Text = "📁 Resim Seç";
            btnResimSec.UseVisualStyleBackColor = false;
            // 
            // lblAci
            // 
            lblAci.AutoSize = true;
            lblAci.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblAci.Location = new Point(259, 512);
            lblAci.Name = "lblAci";
            lblAci.Size = new Size(132, 23);
            lblAci.TabIndex = 3;
            lblAci.Text = "Dönme Açısı (°):";
            // 
            // txtAci
            // 
            txtAci.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 162);
            txtAci.Location = new Point(397, 513);
            txtAci.Name = "txtAci";
            txtAci.Size = new Size(125, 30);
            txtAci.TabIndex = 5;
            txtAci.Text = "45";
            // 
            // btnDondur
            // 
            btnDondur.BackColor = SystemColors.Highlight;
            btnDondur.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnDondur.ForeColor = Color.White;
            btnDondur.Location = new Point(66, 549);
            btnDondur.Name = "btnDondur";
            btnDondur.Size = new Size(198, 47);
            btnDondur.TabIndex = 6;
            btnDondur.Text = "🔄 Resmi Döndür";
            btnDondur.UseVisualStyleBackColor = false;
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
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label1.Location = new Point(143, 24);
            label1.Name = "label1";
            label1.Size = new Size(245, 23);
            label1.TabIndex = 9;
            label1.Text = "Kaynak Resim (Merkez Seçimi)";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label2.Location = new Point(819, 24);
            label2.Name = "label2";
            label2.Size = new Size(153, 23);
            label2.TabIndex = 10;
            label2.Text = "Döndürülmüş Çıktı";
            // 
            // lblBilgi
            // 
            lblBilgi.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblBilgi.Location = new Point(628, 496);
            lblBilgi.Name = "lblBilgi";
            lblBilgi.Size = new Size(560, 95);
            lblBilgi.TabIndex = 11;
            lblBilgi.Text = "'Resim Seç' ile bir görsel yükleyin.\r\nSoldaki resim üzerinde dönme merkezini \r\nfare ile tıklayarak belirleyin (+ işareti çıkar).\r\nAçıyı girip 'Resmi Döndür' butonuna basın.";
            // 
            // Hafta3Odev7
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1232, 703);
            Controls.Add(lblBilgi);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(lnkSonraki);
            Controls.Add(lnkGeri);
            Controls.Add(btnDondur);
            Controls.Add(txtAci);
            Controls.Add(lblAci);
            Controls.Add(btnResimSec);
            Controls.Add(picHedef);
            Controls.Add(picKaynak);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "Hafta3Odev7";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Hafta 3 - Ödev 7: Belirlenen Merkez Etrafında Döndürme";
            ((System.ComponentModel.ISupportInitialize)picKaynak).EndInit();
            ((System.ComponentModel.ISupportInitialize)picHedef).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picKaynak;
        private PictureBox picHedef;
        private Button btnResimSec;
        private Label lblAci;
        private TextBox txtAci;
        private Button btnDondur;
        private LinkLabel lnkGeri;
        private LinkLabel lnkSonraki;
        private Label label1;
        private Label label2;
        private Label lblBilgi;
    }
}