namespace GoruntuIsleme
{
    partial class Hafta3Odev1
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
            groupBox1 = new GroupBox();
            lblBilgi = new Label();
            btnSifirla = new Button();
            btnAynala = new Button();
            btnResimSec = new Button();
            trackAci = new TrackBar();
            lblAciDeger = new Label();
            rbAynala = new RadioButton();
            rbSurukleTasi = new RadioButton();
            rbTiklayarakTasi = new RadioButton();
            lnkSonraki = new LinkLabel();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)picCanvas).BeginInit();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackAci).BeginInit();
            SuspendLayout();
            // 
            // picCanvas
            // 
            picCanvas.BackColor = Color.Gainsboro;
            picCanvas.BorderStyle = BorderStyle.FixedSingle;
            picCanvas.Location = new Point(27, 96);
            picCanvas.Name = "picCanvas";
            picCanvas.Size = new Size(554, 448);
            picCanvas.TabIndex = 0;
            picCanvas.TabStop = false;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(lblBilgi);
            groupBox1.Controls.Add(btnSifirla);
            groupBox1.Controls.Add(btnAynala);
            groupBox1.Controls.Add(btnResimSec);
            groupBox1.Controls.Add(trackAci);
            groupBox1.Controls.Add(lblAciDeger);
            groupBox1.Controls.Add(rbAynala);
            groupBox1.Controls.Add(rbSurukleTasi);
            groupBox1.Controls.Add(rbTiklayarakTasi);
            groupBox1.Font = new Font("Segoe Print", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            groupBox1.Location = new Point(658, 40);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(541, 422);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "İşlem Kontrolleri";
            // 
            // lblBilgi
            // 
            lblBilgi.BackColor = Color.WhiteSmoke;
            lblBilgi.BorderStyle = BorderStyle.Fixed3D;
            lblBilgi.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblBilgi.Location = new Point(18, 271);
            lblBilgi.Name = "lblBilgi";
            lblBilgi.Size = new Size(443, 64);
            lblBilgi.TabIndex = 6;
            lblBilgi.Text = "Lütfen bir resim seçin ve işlem modunu belirleyin.";
            // 
            // btnSifirla
            // 
            btnSifirla.BackColor = SystemColors.Highlight;
            btnSifirla.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnSifirla.ForeColor = Color.White;
            btnSifirla.Location = new Point(388, 353);
            btnSifirla.Name = "btnSifirla";
            btnSifirla.Size = new Size(137, 51);
            btnSifirla.TabIndex = 3;
            btnSifirla.Text = "🔄 Resmi Sıfırla";
            btnSifirla.UseVisualStyleBackColor = false;
            // 
            // btnAynala
            // 
            btnAynala.BackColor = SystemColors.Highlight;
            btnAynala.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnAynala.ForeColor = Color.White;
            btnAynala.Location = new Point(324, 177);
            btnAynala.Name = "btnAynala";
            btnAynala.Size = new Size(164, 55);
            btnAynala.TabIndex = 5;
            btnAynala.Text = "Aynalamayı Uygula (Formül)";
            btnAynala.UseVisualStyleBackColor = false;
            // 
            // btnResimSec
            // 
            btnResimSec.BackColor = SystemColors.Highlight;
            btnResimSec.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnResimSec.ForeColor = Color.White;
            btnResimSec.Location = new Point(6, 353);
            btnResimSec.Name = "btnResimSec";
            btnResimSec.Size = new Size(137, 51);
            btnResimSec.TabIndex = 2;
            btnResimSec.Text = "📁 Resim Seç";
            btnResimSec.UseVisualStyleBackColor = false;
            // 
            // trackAci
            // 
            trackAci.Location = new Point(96, 135);
            trackAci.Maximum = 100;
            trackAci.Name = "trackAci";
            trackAci.Size = new Size(222, 56);
            trackAci.TabIndex = 4;
            trackAci.Value = 45;
            // 
            // lblAciDeger
            // 
            lblAciDeger.AutoSize = true;
            lblAciDeger.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lblAciDeger.Location = new Point(6, 145);
            lblAciDeger.Name = "lblAciDeger";
            lblAciDeger.Size = new Size(98, 23);
            lblAciDeger.TabIndex = 3;
            lblAciDeger.Text = "Açı (θ): 45°";
            // 
            // rbAynala
            // 
            rbAynala.AutoSize = true;
            rbAynala.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            rbAynala.Location = new Point(6, 86);
            rbAynala.Name = "rbAynala";
            rbAynala.Size = new Size(284, 27);
            rbAynala.TabIndex = 2;
            rbAynala.Text = "c) Açılı Aynalama (Formül Testi)";
            rbAynala.UseVisualStyleBackColor = true;
            // 
            // rbSurukleTasi
            // 
            rbSurukleTasi.AutoSize = true;
            rbSurukleTasi.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            rbSurukleTasi.Location = new Point(6, 56);
            rbSurukleTasi.Name = "rbSurukleTasi";
            rbSurukleTasi.Size = new Size(238, 27);
            rbSurukleTasi.TabIndex = 1;
            rbSurukleTasi.Text = "b) Basılı Tutup Sürükleme";
            rbSurukleTasi.UseVisualStyleBackColor = true;
            // 
            // rbTiklayarakTasi
            // 
            rbTiklayarakTasi.AutoSize = true;
            rbTiklayarakTasi.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            rbTiklayarakTasi.Location = new Point(6, 26);
            rbTiklayarakTasi.Name = "rbTiklayarakTasi";
            rbTiklayarakTasi.Size = new Size(275, 27);
            rbTiklayarakTasi.TabIndex = 0;
            rbTiklayarakTasi.Text = "a) Tıklayarak Taşıma (2 Nokta)";
            rbTiklayarakTasi.UseVisualStyleBackColor = true;
            // 
            // lnkSonraki
            // 
            lnkSonraki.AutoSize = true;
            lnkSonraki.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lnkSonraki.Location = new Point(1081, 671);
            lnkSonraki.Name = "lnkSonraki";
            lnkSonraki.Size = new Size(135, 23);
            lnkSonraki.TabIndex = 2;
            lnkSonraki.TabStop = true;
            lnkSonraki.Text = "Ödev 2'ye Git▶";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 162);
            label1.Location = new Point(189, 55);
            label1.Name = "label1";
            label1.Size = new Size(126, 23);
            label1.TabIndex = 3;
            label1.Text = "Çalışma Tuvali";
            // 
            // Hafta3Odev1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1232, 703);
            Controls.Add(label1);
            Controls.Add(lnkSonraki);
            Controls.Add(groupBox1);
            Controls.Add(picCanvas);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "Hafta3Odev1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Hafta 3 - Ödev 1: Resim Taşıma ve Açılı Aynalama";
            ((System.ComponentModel.ISupportInitialize)picCanvas).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackAci).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picCanvas;
        private GroupBox groupBox1;
        private RadioButton rbSurukleTasi;
        private RadioButton rbTiklayarakTasi;
        private Button btnAynala;
        private TrackBar trackAci;
        private Label lblAciDeger;
        private RadioButton rbAynala;
        private Label lblBilgi;
        private Button btnSifirla;
        private Button btnResimSec;
        private LinkLabel lnkSonraki;
        private Label label1;
    }
}