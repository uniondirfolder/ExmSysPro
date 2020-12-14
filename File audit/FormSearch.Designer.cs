
namespace File_audit
{
    partial class FormSearch
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
            this.progressBar_Directory = new System.Windows.Forms.ProgressBar();
            this.label_CurentDir = new System.Windows.Forms.Label();
            this.button_Start = new System.Windows.Forms.Button();
            this.button_Stop = new System.Windows.Forms.Button();
            this.checkBox_BurnProc = new System.Windows.Forms.CheckBox();
            this.progressBarFileScan = new System.Windows.Forms.ProgressBar();
            this.button_OneFolder = new System.Windows.Forms.Button();
            this.label_SizeFile = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.numericUpDown_FileSizeIgnore = new System.Windows.Forms.NumericUpDown();
            this.label_FileScan = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_FileSizeIgnore)).BeginInit();
            this.SuspendLayout();
            // 
            // progressBar_Directory
            // 
            this.progressBar_Directory.Location = new System.Drawing.Point(20, 41);
            this.progressBar_Directory.Name = "progressBar_Directory";
            this.progressBar_Directory.Size = new System.Drawing.Size(733, 23);
            this.progressBar_Directory.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar_Directory.TabIndex = 0;
            // 
            // label_CurentDir
            // 
            this.label_CurentDir.AutoSize = true;
            this.label_CurentDir.Location = new System.Drawing.Point(17, 25);
            this.label_CurentDir.Name = "label_CurentDir";
            this.label_CurentDir.Size = new System.Drawing.Size(18, 13);
            this.label_CurentDir.TabIndex = 3;
            this.label_CurentDir.Text = "../";
            this.label_CurentDir.TextChanged += new System.EventHandler(this.label_CurentDir_TextChanged);
            // 
            // button_Start
            // 
            this.button_Start.Location = new System.Drawing.Point(13, 13);
            this.button_Start.Name = "button_Start";
            this.button_Start.Size = new System.Drawing.Size(75, 23);
            this.button_Start.TabIndex = 4;
            this.button_Start.Text = "Пошук";
            this.button_Start.UseVisualStyleBackColor = true;
            this.button_Start.Click += new System.EventHandler(this.button_Start_Click);
            // 
            // button_Stop
            // 
            this.button_Stop.Location = new System.Drawing.Point(94, 13);
            this.button_Stop.Name = "button_Stop";
            this.button_Stop.Size = new System.Drawing.Size(75, 23);
            this.button_Stop.TabIndex = 5;
            this.button_Stop.Text = "Запинити";
            this.button_Stop.UseVisualStyleBackColor = true;
            this.button_Stop.Click += new System.EventHandler(this.button_Stop_Click);
            // 
            // checkBox_BurnProc
            // 
            this.checkBox_BurnProc.AutoSize = true;
            this.checkBox_BurnProc.Location = new System.Drawing.Point(9, 19);
            this.checkBox_BurnProc.Name = "checkBox_BurnProc";
            this.checkBox_BurnProc.Size = new System.Drawing.Size(200, 17);
            this.checkBox_BurnProc.TabIndex = 6;
            this.checkBox_BurnProc.Text = "Агресивне сканування директорій";
            this.checkBox_BurnProc.UseVisualStyleBackColor = true;
            // 
            // progressBarFileScan
            // 
            this.progressBarFileScan.Location = new System.Drawing.Point(20, 42);
            this.progressBarFileScan.MarqueeAnimationSpeed = 0;
            this.progressBarFileScan.Name = "progressBarFileScan";
            this.progressBarFileScan.Size = new System.Drawing.Size(733, 23);
            this.progressBarFileScan.TabIndex = 7;
            // 
            // button_OneFolder
            // 
            this.button_OneFolder.Location = new System.Drawing.Point(15, 42);
            this.button_OneFolder.Name = "button_OneFolder";
            this.button_OneFolder.Size = new System.Drawing.Size(154, 23);
            this.button_OneFolder.TabIndex = 8;
            this.button_OneFolder.Text = "Пошук в окрмій теці";
            this.button_OneFolder.UseVisualStyleBackColor = true;
            this.button_OneFolder.Click += new System.EventHandler(this.button1_Click);
            // 
            // label_SizeFile
            // 
            this.label_SizeFile.AutoSize = true;
            this.label_SizeFile.Location = new System.Drawing.Point(6, 48);
            this.label_SizeFile.Name = "label_SizeFile";
            this.label_SizeFile.Size = new System.Drawing.Size(192, 13);
            this.label_SizeFile.TabIndex = 9;
            this.label_SizeFile.Text = "Ігнорувати файли розміром (MByte)>";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label_CurentDir);
            this.groupBox1.Controls.Add(this.progressBar_Directory);
            this.groupBox1.Location = new System.Drawing.Point(15, 99);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(759, 75);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Отримання списку директорій";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label_FileScan);
            this.groupBox2.Controls.Add(this.progressBarFileScan);
            this.groupBox2.Location = new System.Drawing.Point(15, 187);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(759, 75);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Сканування файлів";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.numericUpDown_FileSizeIgnore);
            this.groupBox3.Controls.Add(this.checkBox_BurnProc);
            this.groupBox3.Controls.Add(this.label_SizeFile);
            this.groupBox3.Location = new System.Drawing.Point(211, 13);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(273, 80);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Додаткові налаштування";
            // 
            // numericUpDown_FileSizeIgnore
            // 
            this.numericUpDown_FileSizeIgnore.Location = new System.Drawing.Point(204, 46);
            this.numericUpDown_FileSizeIgnore.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_FileSizeIgnore.Name = "numericUpDown_FileSizeIgnore";
            this.numericUpDown_FileSizeIgnore.Size = new System.Drawing.Size(60, 20);
            this.numericUpDown_FileSizeIgnore.TabIndex = 10;
            this.numericUpDown_FileSizeIgnore.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label_FileScan
            // 
            this.label_FileScan.AutoSize = true;
            this.label_FileScan.Location = new System.Drawing.Point(17, 26);
            this.label_FileScan.Name = "label_FileScan";
            this.label_FileScan.Size = new System.Drawing.Size(18, 13);
            this.label_FileScan.TabIndex = 8;
            this.label_FileScan.Text = "../";
            this.label_FileScan.TextChanged += new System.EventHandler(this.label_FileScan_TextChanged);
            // 
            // FormSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_OneFolder);
            this.Controls.Add(this.button_Stop);
            this.Controls.Add(this.button_Start);
            this.Name = "FormSearch";
            this.Text = "FormSearch";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_FileSizeIgnore)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar_Directory;
        private System.Windows.Forms.Label label_CurentDir;
        private System.Windows.Forms.Button button_Start;
        private System.Windows.Forms.Button button_Stop;
        private System.Windows.Forms.CheckBox checkBox_BurnProc;
        private System.Windows.Forms.ProgressBar progressBarFileScan;
        private System.Windows.Forms.Button button_OneFolder;
        private System.Windows.Forms.Label label_SizeFile;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label_FileScan;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.NumericUpDown numericUpDown_FileSizeIgnore;
    }
}