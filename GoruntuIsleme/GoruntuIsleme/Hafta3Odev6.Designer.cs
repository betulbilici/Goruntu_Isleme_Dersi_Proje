namespace GoruntuIsleme
{
    partial class Hafta3Odev6
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
            picOrijinal = new PictureBox();
            picDegistirme = new PictureBox();
            picInterpolasyon = new PictureBox();
            numOran = new NumericUpDown();
            btnResimSec = new Button();
            btnKucult = new Button();
            lblBilgi = new Label();
            lnkGeri = new LinkLabel();
            lnkSonraki = new LinkLabel();
            lblOran = new Label();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)picOrijinal).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picDegistirme).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picInterpolasyon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numOran).BeginInit();
            SuspendLayout();
            // 
            // picOrijinal
            // 
            picOrijinal.BackColor = Color.White;
            picOrijinal.BorderStyle = BorderStyle.FixedSingle;
            picOrijinal.Location = new Point(40, 50);
            picOrijinal.Name = "picOrijinal";
            picOrijinal.Size = new Size(360, 420);
            picOrijinal.SizeMode = PictureBoxSizeMode.Zoom;
            picOrijinal.TabIndex = 0;
            picOrijinal.TabStop = false;
            // 
            // picDegistirme
            // 
            picDegistirme.BackColor = Color.White;
            picDegistirme.BorderStyle = BorderStyle.FixedSingle;
            picDegistirme.Location = new Point(440, 50);
            picDegistirme.Name = "picDegistirme";
            picDegistirme.Size = new Size(360, 420);
            picDegistirme.SizeMode = PictureBoxSizeMode.Zoom;
            picDegistirme.TabIndex = 1;
            picDegistirme.TabStop = false;
            // 
            // picInterpolasyon
            // 
            picInterpolasyon.BackColor = Color.White;
            picInterpolasyon.BorderStyle = BorderStyle.FixedSingle;
            picInterpolasyon.Location = new Point(840, 50);
            picInterpolasyon.Name = "picInterpolasyon";
            picInterpolasyon.Size = new Size(360, 420);
            picInterpolasyon.SizeMode = PictureBoxSizeMode.Zoom;
            picInterpolasyon.TabIndex = 2;
            picInterpolasyon.TabStop = false;
            // 
            // numOran
            // 
            numOran.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 162);
            numOran.Increment = new decimal(new int[] { 5, 0, 0, 0 });
            numOran.Location = new Point(220, 543);
            numOran.Maximum = new decimal(new int[] { 95, 0, 0, 0 });
            numOran.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
            numOran.Name = "numOran";
            numOran.Size = new Size(150, 30);
            numOran.TabIndex = 3;
            numOran.Value = new decimal(new int[] { 50, 0, 0, 0 });
            // 
            // btnResimSec
            // 
            btnResimSec.BackColor = SystemColors.Highlight;
            btnResimSec.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnResimSec.ForeColor = Color.White;
            btnResimSec.Location = new Point(40, 490);
            btnResimSec.Name = "btnResimSec";
            btnResimSec.Size = new Size(198, 38);
            btnResimSec.TabIndex = 4;
            btnResimSec.Text = "📁 Resim Seç";
            btnResimSec.UseVisualStyleBackColor = false;
            // 
            // btnKucult
            // 
            btnKucult.BackColor = SystemColors.Highlight;
            btnKucult.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold);
            btnKucult.ForeColor = Color.White;
            btnKucult.Location = new Point(40, 585);
            btnKucult.Name = "btnKucult";
            btnKucult.Size = new Size(198, 36);
            btnKucult.TabIndex = 5;
            btnKucult.Text = "📉 Resmi Küçült";
            btnKucult.UseVisualStyleBackColor = false;
            // 
            // lblBilgi
            // 
            lblBilgi.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblBilgi.Location = new Point(461, 498);
            lblBilgi.Name = "lblBilgi";
            lblBilgi.Size = new Size(339, 137);
            lblBilgi.TabIndex = 6;
            // 
            // lnkGeri
            // 
            lnkGeri.AutoSize = true;
            lnkGeri.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lnkGeri.Location = new Point(12, 671);
            lnkGeri.Name = "lnkGeri";
            lnkGeri.Size = new Size(140, 23);
            lnkGeri.TabIndex = 7;
            lnkGeri.TabStop = true;
            lnkGeri.Text = "◀ Ödev 5'e Dön";
            // 
            // lnkSonraki
            // 
            lnkSonraki.AutoSize = true;
            lnkSonraki.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lnkSonraki.Location = new Point(1081, 671);
            lnkSonraki.Name = "lnkSonraki";
            lnkSonraki.Size = new Size(140, 23);
            lnkSonraki.TabIndex = 8;
            lnkSonraki.TabStop = true;
            lnkSonraki.Text = "Ödev 7'ye Git ▶";
            // 
            // lblOran
            // 
            lblOran.AutoSize = true;
            lblOran.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblOran.Location = new Point(40, 545);
            lblOran.Name = "lblOran";
            lblOran.Size = new Size(164, 23);
            lblOran.TabIndex = 9;
            lblOran.Text = "Küçültme Oranı (%):";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label1.Location = new Point(136, 23);
            label1.Name = "label1";
            label1.Size = new Size(115, 23);
            label1.TabIndex = 10;
            label1.Text = "Orijinal Resim";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label2.Location = new Point(537, 23);
            label2.Name = "label2";
            label2.Size = new Size(140, 23);
            label2.TabIndex = 11;
            label2.Text = "Piksel Değiştirme";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label3.Location = new Point(968, 23);
            label3.Name = "label3";
            label3.Size = new Size(115, 23);
            label3.TabIndex = 12;
            label3.Text = "İnterpolasyon";
            // 
            // Hafta3Odev6
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1232, 703);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(lblOran);
            Controls.Add(lnkSonraki);
            Controls.Add(lnkGeri);
            Controls.Add(lblBilgi);
            Controls.Add(btnKucult);
            Controls.Add(btnResimSec);
            Controls.Add(numOran);
            Controls.Add(picInterpolasyon);
            Controls.Add(picDegistirme);
            Controls.Add(picOrijinal);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "Hafta3Odev6";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Hafta 3 - Ödev 6: Oransal Resim Küçültme";
            ((System.ComponentModel.ISupportInitialize)picOrijinal).EndInit();
            ((System.ComponentModel.ISupportInitialize)picDegistirme).EndInit();
            ((System.ComponentModel.ISupportInitialize)picInterpolasyon).EndInit();
            ((System.ComponentModel.ISupportInitialize)numOran).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picOrijinal;
        private PictureBox picDegistirme;
        private PictureBox picInterpolasyon;
        private NumericUpDown numOran;
        private Button btnResimSec;
        private Button btnKucult;
        private Label lblBilgi;
        private LinkLabel lnkGeri;
        private LinkLabel lnkSonraki;
        private Label lblOran;
        private Label label1;
        private Label label2;
        private Label label3;
    }
}