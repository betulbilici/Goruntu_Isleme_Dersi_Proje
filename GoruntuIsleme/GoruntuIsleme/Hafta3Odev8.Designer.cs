namespace GoruntuIsleme
{
    partial class Hafta3Odev8
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
            btnResimSec = new Button();
            btnOlcekleModu = new Button();
            btnSifirla = new Button();
            lblBilgi = new Label();
            lnkGeri = new LinkLabel();
            lnkSonraki = new LinkLabel();
            ((System.ComponentModel.ISupportInitialize)picCanvas).BeginInit();
            SuspendLayout();
            // 
            // picCanvas
            // 
            picCanvas.BorderStyle = BorderStyle.FixedSingle;
            picCanvas.Location = new Point(40, 40);
            picCanvas.Name = "picCanvas";
            picCanvas.Size = new Size(1170, 520);
            picCanvas.TabIndex = 0;
            picCanvas.TabStop = false;
            // 
            // btnResimSec
            // 
            btnResimSec.BackColor = SystemColors.Highlight;
            btnResimSec.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnResimSec.ForeColor = Color.White;
            btnResimSec.Location = new Point(40, 580);
            btnResimSec.Name = "btnResimSec";
            btnResimSec.Size = new Size(131, 36);
            btnResimSec.TabIndex = 1;
            btnResimSec.Text = "📁 Resim Seç";
            btnResimSec.UseVisualStyleBackColor = false;
            // 
            // btnOlcekleModu
            // 
            btnOlcekleModu.BackColor = SystemColors.Highlight;
            btnOlcekleModu.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnOlcekleModu.ForeColor = Color.White;
            btnOlcekleModu.Location = new Point(225, 580);
            btnOlcekleModu.Name = "btnOlcekleModu";
            btnOlcekleModu.Size = new Size(209, 36);
            btnOlcekleModu.TabIndex = 2;
            btnOlcekleModu.Text = "🔍 Ölçekleme Modu";
            btnOlcekleModu.UseVisualStyleBackColor = false;
            // 
            // btnSifirla
            // 
            btnSifirla.BackColor = SystemColors.Highlight;
            btnSifirla.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnSifirla.ForeColor = Color.White;
            btnSifirla.Location = new Point(420, 580);
            btnSifirla.Name = "btnSifirla";
            btnSifirla.Size = new Size(107, 36);
            btnSifirla.TabIndex = 3;
            btnSifirla.Text = "🔄 Sıfırla";
            btnSifirla.UseVisualStyleBackColor = false;
            // 
            // lblBilgi
            // 
            lblBilgi.BorderStyle = BorderStyle.FixedSingle;
            lblBilgi.Location = new Point(569, 580);
            lblBilgi.Name = "lblBilgi";
            lblBilgi.Size = new Size(620, 75);
            lblBilgi.TabIndex = 4;
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
            lnkGeri.Text = "◀ Ödev 7'ye Dön";
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
            lnkSonraki.Text = "Ödev 9'a Git ▶";
            // 
            // Hafta3Odev8
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1232, 703);
            Controls.Add(lnkSonraki);
            Controls.Add(lnkGeri);
            Controls.Add(lblBilgi);
            Controls.Add(btnSifirla);
            Controls.Add(btnOlcekleModu);
            Controls.Add(btnResimSec);
            Controls.Add(picCanvas);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "Hafta3Odev8";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Hafta 3 - Ödev 8: Tutamaçlarla Ölçekleme";
            ((System.ComponentModel.ISupportInitialize)picCanvas).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picCanvas;
        private Button btnResimSec;
        private Button btnOlcekleModu;
        private Button btnSifirla;
        private Label lblBilgi;
        private LinkLabel lnkGeri;
        private LinkLabel lnkSonraki;
    }
}