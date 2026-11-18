namespace GoruntuIsleme
{
    partial class Hafta1Odev2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Hafta1Odev2));
            picGoruntu = new PictureBox();
            numBoyut = new NumericUpDown();
            numYaricap = new NumericUpDown();
            btnOlustur = new Button();
            lnkOdev1 = new LinkLabel();
            lnkOdev3 = new LinkLabel();
            label1 = new Label();
            label2 = new Label();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)picGoruntu).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numBoyut).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numYaricap).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // picGoruntu
            // 
            picGoruntu.BorderStyle = BorderStyle.FixedSingle;
            picGoruntu.Location = new Point(686, 215);
            picGoruntu.Name = "picGoruntu";
            picGoruntu.Size = new Size(425, 375);
            picGoruntu.SizeMode = PictureBoxSizeMode.StretchImage;
            picGoruntu.TabIndex = 0;
            picGoruntu.TabStop = false;
            // 
            // numBoyut
            // 
            numBoyut.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 162);
            numBoyut.Location = new Point(306, 354);
            numBoyut.Maximum = new decimal(new int[] { 1024, 0, 0, 0 });
            numBoyut.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
            numBoyut.Name = "numBoyut";
            numBoyut.Size = new Size(150, 30);
            numBoyut.TabIndex = 1;
            numBoyut.Value = new decimal(new int[] { 512, 0, 0, 0 });
            // 
            // numYaricap
            // 
            numYaricap.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 162);
            numYaricap.Location = new Point(306, 466);
            numYaricap.Maximum = new decimal(new int[] { 500, 0, 0, 0 });
            numYaricap.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            numYaricap.Name = "numYaricap";
            numYaricap.Size = new Size(150, 30);
            numYaricap.TabIndex = 2;
            numYaricap.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // btnOlustur
            // 
            btnOlustur.BackColor = SystemColors.Highlight;
            btnOlustur.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnOlustur.ForeColor = Color.White;
            btnOlustur.Location = new Point(533, 546);
            btnOlustur.Name = "btnOlustur";
            btnOlustur.Size = new Size(137, 65);
            btnOlustur.TabIndex = 3;
            btnOlustur.Text = "Görüntüyü Oluştur";
            btnOlustur.UseVisualStyleBackColor = false;
            // 
            // lnkOdev1
            // 
            lnkOdev1.AutoSize = true;
            lnkOdev1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lnkOdev1.Location = new Point(12, 671);
            lnkOdev1.Name = "lnkOdev1";
            lnkOdev1.Size = new Size(140, 23);
            lnkOdev1.TabIndex = 4;
            lnkOdev1.TabStop = true;
            lnkOdev1.Text = "◀ Ödev 1'e Dön";
            // 
            // lnkOdev3
            // 
            lnkOdev3.AutoSize = true;
            lnkOdev3.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            lnkOdev3.Location = new Point(1081, 671);
            lnkOdev3.Name = "lnkOdev3";
            lnkOdev3.Size = new Size(131, 23);
            lnkOdev3.TabIndex = 5;
            lnkOdev3.TabStop = true;
            lnkOdev3.Text = "Ödev 3'e Git ▶";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label1.Location = new Point(148, 354);
            label1.Name = "label1";
            label1.Size = new Size(152, 23);
            label1.TabIndex = 6;
            label1.Text = "Matris Boyutu (N):";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label2.Location = new Point(160, 466);
            label2.Name = "label2";
            label2.Size = new Size(140, 23);
            label2.TabIndex = 7;
            label2.Text = "Daire Yarıçapı (r):";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(12, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(668, 247);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 8;
            pictureBox1.TabStop = false;
            // 
            // Hafta1Odev2
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1232, 703);
            Controls.Add(pictureBox1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(lnkOdev3);
            Controls.Add(lnkOdev1);
            Controls.Add(btnOlustur);
            Controls.Add(numYaricap);
            Controls.Add(numBoyut);
            Controls.Add(picGoruntu);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "Hafta1Odev2";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Hafta1Odev2";
            ((System.ComponentModel.ISupportInitialize)picGoruntu).EndInit();
            ((System.ComponentModel.ISupportInitialize)numBoyut).EndInit();
            ((System.ComponentModel.ISupportInitialize)numYaricap).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picGoruntu;
        private NumericUpDown numBoyut;
        private NumericUpDown numYaricap;
        private Button btnOlustur;
        private LinkLabel lnkOdev1;
        private LinkLabel lnkOdev3;
        private Label label1;
        private Label label2;
        private PictureBox pictureBox1;
    }
}