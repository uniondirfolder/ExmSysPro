
namespace File_audit
{
    partial class MainF
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.listBox_UserWordsManual = new System.Windows.Forms.ListBox();
            this.groupBox_Properties = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_RefreshDiskInfo = new System.Windows.Forms.Button();
            this.comboBox_DiskInfo = new System.Windows.Forms.ComboBox();
            this.button_AddWordsFromFile = new System.Windows.Forms.Button();
            this.button_UserAddWord = new System.Windows.Forms.Button();
            this.textBox_WordForAudit = new System.Windows.Forms.TextBox();
            this.splitContainer_Main = new System.Windows.Forms.SplitContainer();
            this.contextMenuStrip_ListWords = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsm_DeleteFromList = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox_Properties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Main)).BeginInit();
            this.splitContainer_Main.Panel1.SuspendLayout();
            this.splitContainer_Main.SuspendLayout();
            this.contextMenuStrip_ListWords.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox_UserWordsManual
            // 
            this.listBox_UserWordsManual.FormattingEnabled = true;
            this.listBox_UserWordsManual.Location = new System.Drawing.Point(5, 45);
            this.listBox_UserWordsManual.Name = "listBox_UserWordsManual";
            this.listBox_UserWordsManual.Size = new System.Drawing.Size(120, 108);
            this.listBox_UserWordsManual.TabIndex = 0;
            // 
            // groupBox_Properties
            // 
            this.groupBox_Properties.Controls.Add(this.groupBox1);
            this.groupBox_Properties.Controls.Add(this.label1);
            this.groupBox_Properties.Controls.Add(this.button_RefreshDiskInfo);
            this.groupBox_Properties.Controls.Add(this.comboBox_DiskInfo);
            this.groupBox_Properties.Controls.Add(this.button_AddWordsFromFile);
            this.groupBox_Properties.Controls.Add(this.button_UserAddWord);
            this.groupBox_Properties.Controls.Add(this.textBox_WordForAudit);
            this.groupBox_Properties.Controls.Add(this.listBox_UserWordsManual);
            this.groupBox_Properties.Location = new System.Drawing.Point(12, 12);
            this.groupBox_Properties.Name = "groupBox_Properties";
            this.groupBox_Properties.Size = new System.Drawing.Size(422, 221);
            this.groupBox_Properties.TabIndex = 1;
            this.groupBox_Properties.TabStop = false;
            this.groupBox_Properties.Text = "Налаштування пошуку ";
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(154, 45);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(245, 108);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Загальна інформація";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(151, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Диск";
            // 
            // button_RefreshDiskInfo
            // 
            this.button_RefreshDiskInfo.Location = new System.Drawing.Point(324, 15);
            this.button_RefreshDiskInfo.Name = "button_RefreshDiskInfo";
            this.button_RefreshDiskInfo.Size = new System.Drawing.Size(75, 23);
            this.button_RefreshDiskInfo.TabIndex = 5;
            this.button_RefreshDiskInfo.Text = "Оновити";
            this.button_RefreshDiskInfo.UseVisualStyleBackColor = true;
            this.button_RefreshDiskInfo.Click += new System.EventHandler(this.button_RefreshDiskInfo_Click);
            // 
            // comboBox_DiskInfo
            // 
            this.comboBox_DiskInfo.FormattingEnabled = true;
            this.comboBox_DiskInfo.Items.AddRange(new object[] {
            "Усі"});
            this.comboBox_DiskInfo.Location = new System.Drawing.Point(197, 17);
            this.comboBox_DiskInfo.Name = "comboBox_DiskInfo";
            this.comboBox_DiskInfo.Size = new System.Drawing.Size(121, 21);
            this.comboBox_DiskInfo.TabIndex = 4;
            // 
            // button_AddWordsFromFile
            // 
            this.button_AddWordsFromFile.Location = new System.Drawing.Point(5, 189);
            this.button_AddWordsFromFile.Name = "button_AddWordsFromFile";
            this.button_AddWordsFromFile.Size = new System.Drawing.Size(120, 23);
            this.button_AddWordsFromFile.TabIndex = 3;
            this.button_AddWordsFromFile.Text = "Додати з файлу";
            this.button_AddWordsFromFile.UseVisualStyleBackColor = true;
            this.button_AddWordsFromFile.Click += new System.EventHandler(this.button_AddWordsFromFile_Click);
            // 
            // button_UserAddWord
            // 
            this.button_UserAddWord.Location = new System.Drawing.Point(6, 160);
            this.button_UserAddWord.Name = "button_UserAddWord";
            this.button_UserAddWord.Size = new System.Drawing.Size(119, 23);
            this.button_UserAddWord.TabIndex = 2;
            this.button_UserAddWord.Text = "Додати слово";
            this.button_UserAddWord.UseVisualStyleBackColor = true;
            this.button_UserAddWord.Click += new System.EventHandler(this.button_UserAddWord_Click);
            // 
            // textBox_WordForAudit
            // 
            this.textBox_WordForAudit.Location = new System.Drawing.Point(6, 19);
            this.textBox_WordForAudit.Name = "textBox_WordForAudit";
            this.textBox_WordForAudit.Size = new System.Drawing.Size(119, 20);
            this.textBox_WordForAudit.TabIndex = 1;
            // 
            // splitContainer_Main
            // 
            this.splitContainer_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_Main.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_Main.Name = "splitContainer_Main";
            this.splitContainer_Main.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_Main.Panel1
            // 
            this.splitContainer_Main.Panel1.Controls.Add(this.groupBox_Properties);
            this.splitContainer_Main.Size = new System.Drawing.Size(800, 450);
            this.splitContainer_Main.SplitterDistance = 295;
            this.splitContainer_Main.TabIndex = 2;
            // 
            // contextMenuStrip_ListWords
            // 
            this.contextMenuStrip_ListWords.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsm_DeleteFromList});
            this.contextMenuStrip_ListWords.Name = "contextMenuStrip_Main";
            this.contextMenuStrip_ListWords.Size = new System.Drawing.Size(164, 26);
            // 
            // tsm_DeleteFromList
            // 
            this.tsm_DeleteFromList.Name = "tsm_DeleteFromList";
            this.tsm_DeleteFromList.Size = new System.Drawing.Size(180, 22);
            this.tsm_DeleteFromList.Text = "Вилучити слово";
            this.tsm_DeleteFromList.Click += new System.EventHandler(this.tsm_DeleteFromList_Click);
            // 
            // MainF
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.ContextMenuStrip = this.contextMenuStrip_ListWords;
            this.Controls.Add(this.splitContainer_Main);
            this.Name = "MainF";
            this.Text = "Пошук заборонених слів";
            this.groupBox_Properties.ResumeLayout(false);
            this.groupBox_Properties.PerformLayout();
            this.splitContainer_Main.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Main)).EndInit();
            this.splitContainer_Main.ResumeLayout(false);
            this.contextMenuStrip_ListWords.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox_UserWordsManual;
        private System.Windows.Forms.GroupBox groupBox_Properties;
        private System.Windows.Forms.Button button_AddWordsFromFile;
        private System.Windows.Forms.Button button_UserAddWord;
        private System.Windows.Forms.TextBox textBox_WordForAudit;
        private System.Windows.Forms.Button button_RefreshDiskInfo;
        private System.Windows.Forms.ComboBox comboBox_DiskInfo;
        private System.Windows.Forms.SplitContainer splitContainer_Main;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_ListWords;
        private System.Windows.Forms.ToolStripMenuItem tsm_DeleteFromList;
    }
}

