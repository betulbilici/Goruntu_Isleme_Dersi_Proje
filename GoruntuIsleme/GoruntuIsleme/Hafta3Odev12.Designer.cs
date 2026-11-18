namespace GoruntuIsleme
{
    partial class Hafta3Odev12
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
            picA = new PictureBox();
            picB = new PictureBox();
            lblBaslikOrijinal = new Label();
            lblBaslikA = new Label();
            lblBaslikB = new Label();
            btnResimSec = new Button();
            lblAci = new Label();
            numAci = new NumericUpDown();
            btnIslemA = new Button();
            btnIslemB = new Button();
            lblBilgi = new Label();
            lnkGeri = new LinkLabel();
            lnkSonraki = new LinkLabel();
            btnSifirla = new Button();
            ((System.ComponentModel.ISupportInitialize)picOrijinal).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picA).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picB).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numAci).BeginInit();
            SuspendLayout();
            // 
            // picOrijinal
            // 
            picOrijinal.BorderStyle = BorderStyle.FixedSingle;
            picOrijinal.Location = new Point(40, 45);
            picOrijinal.Name = "picOrijinal";
            picOrijinal.Size = new Size(360, 420);
            picOrijinal.SizeMode = PictureBoxSizeMode.Zoom;
            picOrijinal.TabIndex = 0;
            picOrijinal.TabStop = false;
            // 
            // picA
            // 
            picA.BorderStyle = BorderStyle.FixedSingle;
            picA.Location = new Point(440, 45);
            picA.Name = "picA";
            picA.Size = new Size(360, 420);
            picA.SizeMode = PictureBoxSizeMode.Zoom;
            picA.TabIndex = 1;
            picA.TabStop = false;
            // 
            // picB
            // 
            picB.BorderStyle = BorderStyle.FixedSingle;
            picB.Location = new Point(840, 45);
            picB.Name = "picB";
            picB.Size = new Size(360, 420);
            picB.SizeMode = PictureBoxSizeMode.Zoom;
            picB.TabIndex = 2;
            picB.TabStop = false;
            // 
            // lblBaslikOrijinal
            // 
            lblBaslikOrijinal.AutoSize = true;
            lblBaslikOrijinal.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblBaslikOrijinal.Location = new Point(40, 18);
            lblBaslikOrijinal.Name = "lblBaslikOrijinal";
            lblBaslikOrijinal.Size = new Size(115, 23);
            lblBaslikOrijinal.TabIndex = 3;
            lblBaslikOrijinal.Text = "Orijinal Resim";
            // 
            // lblBaslikA
            // 
            lblBaslikA.AutoSize = true;
            lblBaslikA.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblBaslikA.Location = new Point(440, 18);
            lblBaslikA.Name = "lblBaslikA";
            lblBaslikA.Size = new Size(291, 23);
            lblBaslikA.TabIndex = 4;
            lblBaslikA.Text = "a) Alias Giderme (Komşu Ortalaması)";
            // 
            // lblBaslikB
            // 
            lblBaslikB.AutoSize = true;
            lblBaslikB.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblBaslikB.Location = new Point(840, 18);
            lblBaslikB.Name = "lblBaslikB";
            lblBaslikB.Size = new Size(240, 23);
            lblBaslikB.TabIndex = 5;
            lblBaslikB.Text = "b) Köşeden Eğerek Döndürme";
            // 
            // btnResimSec
            // 
            btnResimSec.BackColor = SystemColors.Highlight;
            btnResimSec.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnResimSec.ForeColor = Color.White;
            btnResimSec.Location = new Point(40, 485);
            btnResimSec.Name = "btnResimSec";
            btnResimSec.Size = new Size(180, 38);
            btnResimSec.TabIndex = 6;
            btnResimSec.Text = "📁 Resim Seç";
            btnResimSec.UseVisualStyleBackColor = false;
            // 
            // lblAci
            // 
            lblAci.AutoSize = true;
            lblAci.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblAci.Location = new Point(40, 535);
            lblAci.Name = "lblAci";
            lblAci.Size = new Size(132, 23);
            lblAci.TabIndex = 7;
            lblAci.Text = "Dönme Açısı (°):";
            // 
            // numAci
            // 
            numAci.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 162);
            numAci.Location = new Point(178, 535);
            numAci.Maximum = new decimal(new int[] { 360, 0, 0, 0 });
            numAci.Minimum = new decimal(new int[] { 360, 0, 0, int.MinValue });
            numAci.Name = "numAci";
            numAci.Size = new Size(150, 30);
            numAci.TabIndex = 8;
            numAci.Value = new decimal(new int[] { 30, 0, 0, 0 });
            // 
            // btnIslemA
            // 
            btnIslemA.BackColor = SystemColors.Highlight;
            btnIslemA.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnIslemA.ForeColor = Color.White;
            btnIslemA.Location = new Point(440, 485);
            btnIslemA.Name = "btnIslemA";
            btnIslemA.Size = new Size(360, 38);
            btnIslemA.TabIndex = 9;
            btnIslemA.Text = "a) Geometrik Döndür & Delik Doldur";
            btnIslemA.UseVisualStyleBackColor = false;
            // 
            // btnIslemB
            // 
            btnIslemB.BackColor = SystemColors.Highlight;
            btnIslemB.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnIslemB.ForeColor = Color.White;
            btnIslemB.Location = new Point(840, 485);
            btnIslemB.Name = "btnIslemB";
            btnIslemB.Size = new Size(360, 38);
            btnIslemB.TabIndex = 10;
            btnIslemB.Text = "b) Köşeden Eğerek Döndür";
            btnIslemB.UseVisualStyleBackColor = false;
            // 
            // lblBilgi
            // 
            lblBilgi.BorderStyle = BorderStyle.Fixed3D;
            lblBilgi.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblBilgi.Location = new Point(40, 580);
            lblBilgi.Name = "lblBilgi";
            lblBilgi.Size = new Size(1160, 80);
            lblBilgi.TabIndex = 11;
            // 
            // lnkGeri
            // 
            lnkGeri.AutoSize = true;
            lnkGeri.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lnkGeri.Location = new Point(12, 671);
            lnkGeri.Name = "lnkGeri";
            lnkGeri.Size = new Size(150, 23);
            lnkGeri.TabIndex = 12;
            lnkGeri.TabStop = true;
            lnkGeri.Text = "◀ Ödev 11'e Dön";
            // 
            // lnkSonraki
            // 
            lnkSonraki.AutoSize = true;
            lnkSonraki.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lnkSonraki.Location = new Point(1080, 671);
            lnkSonraki.Name = "lnkSonraki";
            lnkSonraki.Size = new Size(141, 23);
            lnkSonraki.TabIndex = 14;
            lnkSonraki.TabStop = true;
            lnkSonraki.Text = "Ödev 13'e Git ▶";
            // 
            // btnSifirla
            // 
            btnSifirla.BackColor = SystemColors.Highlight;
            btnSifirla.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnSifirla.ForeColor = Color.White;
            btnSifirla.Location = new Point(763, 532);
            btnSifirla.Name = "btnSifirla";
            btnSifirla.Size = new Size(110, 33);
            btnSifirla.TabIndex = 15;
            btnSifirla.Text = "🔄 Sıfırla";
            btnSifirla.UseVisualStyleBackColor = false;
            // 
            // Hafta3Odev12
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1232, 703);
            Controls.Add(btnSifirla);
            Controls.Add(lnkSonraki);
            Controls.Add(lnkGeri);
            Controls.Add(lblBilgi);
            Controls.Add(btnIslemB);
            Controls.Add(btnIslemA);
            Controls.Add(numAci);
            Controls.Add(lblAci);
            Controls.Add(btnResimSec);
            Controls.Add(lblBaslikB);
            Controls.Add(lblBaslikA);
            Controls.Add(lblBaslikOrijinal);
            Controls.Add(picB);
            Controls.Add(picA);
            Controls.Add(picOrijinal);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "Hafta3Odev12";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Hafta 3 - Ödev 12: Alias Giderme ve Köşeden Kaydırarak Döndürme";
            ((System.ComponentModel.ISupportInitialize)picOrijinal).EndInit();
            ((System.ComponentModel.ISupportInitialize)picA).EndInit();
            ((System.ComponentModel.ISupportInitialize)picB).EndInit();
            ((System.ComponentModel.ISupportInitialize)numAci).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picOrijinal;
        private PictureBox picA;
        private PictureBox picB;
        private Label lblBaslikOrijinal;
        private Label lblBaslikA;
        private Label lblBaslikB;
        private Button btnResimSec;
        private Label lblAci;
        private NumericUpDown numAci;
        private Button btnIslemA;
        private Button btnIslemB;
        private Label lblBilgi;
        private LinkLabel lnkGeri;
        private LinkLabel lnkSonraki;
        private Button btnSifirla;
    }
}