
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
            this.label1 = new System.Windows.Forms.Label();
            this.label_CurentDir = new System.Windows.Forms.Label();
            this.button_Start = new System.Windows.Forms.Button();
            this.button_Stop = new System.Windows.Forms.Button();
            this.checkBox_BurnProc = new System.Windows.Forms.CheckBox();
            this.progressBarFileScan = new System.Windows.Forms.ProgressBar();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // progressBar_Directory
            // 
            this.progressBar_Directory.Location = new System.Drawing.Point(178, 51);
            this.progressBar_Directory.Name = "progressBar_Directory";
            this.progressBar_Directory.Size = new System.Drawing.Size(609, 23);
            this.progressBar_Directory.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Отримання списку директорій";
            // 
            // label_CurentDir
            // 
            this.label_CurentDir.AutoSize = true;
            this.label_CurentDir.Location = new System.Drawing.Point(12, 89);
            this.label_CurentDir.Name = "label_CurentDir";
            this.label_CurentDir.Size = new System.Drawing.Size(18, 13);
            this.label_CurentDir.TabIndex = 3;
            this.label_CurentDir.Text = "../";
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
            this.checkBox_BurnProc.Location = new System.Drawing.Point(175, 19);
            this.checkBox_BurnProc.Name = "checkBox_BurnProc";
            this.checkBox_BurnProc.Size = new System.Drawing.Size(80, 17);
            this.checkBox_BurnProc.TabIndex = 6;
            this.checkBox_BurnProc.Text = "Агресивно";
            this.checkBox_BurnProc.UseVisualStyleBackColor = true;
            // 
            // progressBarFileScan
            // 
            this.progressBarFileScan.Location = new System.Drawing.Point(178, 106);
            this.progressBarFileScan.MarqueeAnimationSpeed = 0;
            this.progressBarFileScan.Name = "progressBarFileScan";
            this.progressBarFileScan.Size = new System.Drawing.Size(609, 23);
            this.progressBarFileScan.TabIndex = 7;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 336);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FormSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.progressBarFileScan);
            this.Controls.Add(this.checkBox_BurnProc);
            this.Controls.Add(this.button_Stop);
            this.Controls.Add(this.button_Start);
            this.Controls.Add(this.label_CurentDir);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar_Directory);
            this.Name = "FormSearch";
            this.Text = "FormSearch";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar_Directory;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label_CurentDir;
        private System.Windows.Forms.Button button_Start;
        private System.Windows.Forms.Button button_Stop;
        private System.Windows.Forms.CheckBox checkBox_BurnProc;
        private System.Windows.Forms.ProgressBar progressBarFileScan;
        private System.Windows.Forms.Button button1;
    }
}