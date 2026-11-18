namespace GoruntuIsleme
{
    partial class Hafta3Odev3
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
            picCanvas = new PictureBox();
            lnkGeri = new LinkLabel();
            lnkSonraki = new LinkLabel();
            grpKontrol = new GroupBox();
            lblBilgi = new Label();
            label2 = new Label();
            label1 = new Label();
            btnAynala = new Button();
            btnSifirla = new Button();
            btnResimSec = new Button();
            ((System.ComponentModel.ISupportInitialize)picCanvas).BeginInit();
            grpKontrol.SuspendLayout();
            SuspendLayout();
            // 
            // picCanvas
            // 
            picCanvas.BackColor = Color.WhiteSmoke;
            picCanvas.BorderStyle = BorderStyle.FixedSingle;
            picCanvas.Location = new Point(25, 47);
            picCanvas.Name = "picCanvas";
            picCanvas.Size = new Size(555, 444);
            picCanvas.TabIndex = 0;
            picCanvas.TabStop = false;
            // 
            // lnkGeri
            // 
            lnkGeri.AutoSize = true;
            lnkGeri.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lnkGeri.Location = new Point(12, 671);
            lnkGeri.Name = "lnkGeri";
            lnkGeri.Size = new Size(149, 23);
            lnkGeri.TabIndex = 5;
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
            lnkSonraki.TabIndex = 6;
            lnkSonraki.TabStop = true;
            lnkSonraki.Text = "Ödev 4'e Git ▶";
            // 
            // grpKontrol
            // 
            grpKontrol.Controls.Add(lblBilgi);
            grpKontrol.Controls.Add(label2);
            grpKontrol.Controls.Add(label1);
            grpKontrol.Controls.Add(btnAynala);
            grpKontrol.Controls.Add(btnSifirla);
            grpKontrol.Controls.Add(btnResimSec);
            grpKontrol.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            grpKontrol.Location = new Point(655, 12);
            grpKontrol.Name = "grpKontrol";
            grpKontrol.Size = new Size(430, 600);
            grpKontrol.TabIndex = 7;
            grpKontrol.TabStop = false;
            grpKontrol.Text = "İşlem ve Analiz Paneli";
            // 
            // lblBilgi
            // 
            lblBilgi.BorderStyle = BorderStyle.Fixed3D;
            lblBilgi.Location = new Point(33, 360);
            lblBilgi.Name = "lblBilgi";
            lblBilgi.Size = new Size(328, 173);
            lblBilgi.TabIndex = 5;
            lblBilgi.Text = "Eksen ve formül değerleri burada hesaplanıp listelenecektir.";
            // 
            // label2
            // 
            label2.Location = new Point(33, 238);
            label2.Name = "label2";
            label2.Size = new Size(308, 122);
            label2.TabIndex = 4;
            label2.Text = "1. Resim yükleyin.\r\n2.Tuvalde eksen için 2 noktaya(A ve B)tıklayın.\r\n3.Aynalama butonuna basın.";
            // 
            // label1
            // 
            label1.Location = new Point(33, 211);
            label1.Name = "label1";
            label1.Size = new Size(308, 27);
            label1.TabIndex = 3;
            label1.Text = "Aynalama Adımları:";
            // 
            // btnAynala
            // 
            btnAynala.BackColor = SystemColors.Highlight;
            btnAynala.ForeColor = Color.White;
            btnAynala.Location = new Point(20, 149);
            btnAynala.Name = "btnAynala";
            btnAynala.Size = new Size(263, 44);
            btnAynala.TabIndex = 2;
            btnAynala.Text = "⚡ Eksen Çizgisine Göre Aynala";
            btnAynala.UseVisualStyleBackColor = false;
            // 
            // btnSifirla
            // 
            btnSifirla.BackColor = SystemColors.Highlight;
            btnSifirla.ForeColor = Color.White;
            btnSifirla.Location = new Point(20, 89);
            btnSifirla.Name = "btnSifirla";
            btnSifirla.Size = new Size(146, 34);
            btnSifirla.TabIndex = 1;
            btnSifirla.Text = "🔄 Resmi Sıfırla";
            btnSifirla.UseVisualStyleBackColor = false;
            // 
            // btnResimSec
            // 
            btnResimSec.BackColor = SystemColors.Highlight;
            btnResimSec.ForeColor = Color.White;
            btnResimSec.Location = new Point(20, 30);
            btnResimSec.Name = "btnResimSec";
            btnResimSec.Size = new Size(138, 34);
            btnResimSec.TabIndex = 0;
            btnResimSec.Text = "📁 Resim Seç";
            btnResimSec.UseVisualStyleBackColor = false;
            // 
            // Hafta3Odev3
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1232, 703);
            Controls.Add(grpKontrol);
            Controls.Add(lnkSonraki);
            Controls.Add(lnkGeri);
            Controls.Add(picCanvas);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "Hafta3Odev3";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Hafta 3 - Ödev 3: İki Nokta Arasındaki Eksen Etrafında Aynalama";
            ((System.ComponentModel.ISupportInitialize)picCanvas).EndInit();
            grpKontrol.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picCanvas;
        private LinkLabel lnkGeri;
        private LinkLabel lnkSonraki;
        private GroupBox grpKontrol;
        private Label label1;
        private Button btnAynala;
        private Button btnSifirla;
        private Button btnResimSec;
        private Label lblBilgi;
        private Label label2;
    }
}