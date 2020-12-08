using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibFileAudit;

namespace File_audit
{
    public partial class MainF : Form
    {
        public MainF()
        {
            InitializeComponent();
            RefreshDiskInfo();
        }

        private void button_RefreshDiskInfo_Click(object sender, EventArgs e)
        {
            RefreshDiskInfo();
        }

        private void RefreshDiskInfo()
        {
            comboBox_DiskInfo.Items.Clear();
            comboBox_DiskInfo.Items.AddRange(items: DriveInfo.GetDrives());
            if (comboBox_DiskInfo.Items.Count > 0)
            {
                comboBox_DiskInfo.Items.Add("Усі");
            }
            else
            {
                comboBox_DiskInfo.Items.Add("Відсутні носії");
            }
            comboBox_DiskInfo.SelectedIndex = 0;
        }

        private void button_UserAddWord_Click(object sender, EventArgs e)
        {
            if (textBox_WordForAudit.Text.Length > 0)
                listBox_UserWordsManual.Items.Add(textBox_WordForAudit.Text);
        }

        private void tsm_DeleteFromList_Click(object sender, EventArgs e)
        {
            if (listBox_UserWordsManual.Items.Count > 0 && listBox_UserWordsManual.SelectedIndex > -1)
            {
                listBox_UserWordsManual.Items.RemoveAt(listBox_UserWordsManual.SelectedIndex);
            }
        }

        private void button_AddWordsFromFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    listBox_UserWordsManual.Items.AddRange(items: Audit.GetWordsFromString(File.ReadAllText(ofd.FileName)).ToArray());
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    throw;
                }
            }
        }
    }
}
