namespace GoruntuIsleme
{
    partial class Hafta1Odev1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Hafta1Odev1));
            picGoruntu = new PictureBox();
            numGenislik = new NumericUpDown();
            btnOlustur = new Button();
            label1 = new Label();
            lnkOdev2 = new LinkLabel();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)picGoruntu).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numGenislik).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // picGoruntu
            // 
            picGoruntu.BorderStyle = BorderStyle.FixedSingle;
            picGoruntu.Location = new Point(69, 333);
            picGoruntu.Name = "picGoruntu";
            picGoruntu.Size = new Size(350, 350);
            picGoruntu.SizeMode = PictureBoxSizeMode.StretchImage;
            picGoruntu.TabIndex = 0;
            picGoruntu.TabStop = false;
            // 
            // numGenislik
            // 
            numGenislik.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 162);
            numGenislik.Location = new Point(669, 459);
            numGenislik.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            numGenislik.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numGenislik.Name = "numGenislik";
            numGenislik.Size = new Size(150, 30);
            numGenislik.TabIndex = 1;
            numGenislik.Value = new decimal(new int[] { 255, 0, 0, 0 });
            // 
            // btnOlustur
            // 
            btnOlustur.BackColor = SystemColors.Highlight;
            btnOlustur.Cursor = Cursors.Hand;
            btnOlustur.FlatAppearance.BorderSize = 0;
            btnOlustur.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnOlustur.ForeColor = Color.White;
            btnOlustur.Location = new Point(618, 581);
            btnOlustur.Name = "btnOlustur";
            btnOlustur.Size = new Size(163, 63);
            btnOlustur.TabIndex = 2;
            btnOlustur.Text = "Görüntüyü Oluştur";
            btnOlustur.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label1.Location = new Point(452, 461);
            label1.Name = "label1";
            label1.Size = new Size(211, 23);
            label1.TabIndex = 3;
            label1.Text = "Maksimum Gri Değeri (N):";
            // 
            // lnkOdev2
            // 
            lnkOdev2.AutoSize = true;
            lnkOdev2.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lnkOdev2.Location = new Point(1081, 671);
            lnkOdev2.Name = "lnkOdev2";
            lnkOdev2.Size = new Size(140, 23);
            lnkOdev2.TabIndex = 4;
            lnkOdev2.TabStop = true;
            lnkOdev2.Text = "Ödev 2'ye Git ▶";
            
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(12, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(807, 296);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 5;
            pictureBox1.TabStop = false;
            // 
            // Hafta1Odev1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1232, 703);
            Controls.Add(pictureBox1);
            Controls.Add(lnkOdev2);
            Controls.Add(label1);
            Controls.Add(btnOlustur);
            Controls.Add(numGenislik);
            Controls.Add(picGoruntu);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "Hafta1Odev1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Hafta1Odev1";
            ((System.ComponentModel.ISupportInitialize)picGoruntu).EndInit();
            ((System.ComponentModel.ISupportInitialize)numGenislik).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picGoruntu;
        private NumericUpDown numGenislik;
        private Button btnOlustur;
        private Label label1;
        private LinkLabel lnkOdev2;
        private PictureBox pictureBox1;
    }
}