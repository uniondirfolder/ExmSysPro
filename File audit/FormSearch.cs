using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibFileAudit;

namespace File_audit
{
    public partial class FormSearch : Form
    {
        private string _disk = "";
        private UpdateUiSearchForm _updateUi;
        private CancellationTokenSource _cancelTokenSource;
        private CancellationToken _token;
        private List<DirectoryInfo> _ldi;
        private List<SearchFileInfo> _sfi;
        private readonly Audit _audit;
        private string[] _arrWords;
        private Msg Tm;
        public FormSearch(string disk, string[]arrWords)
        {
            _cancelTokenSource = new CancellationTokenSource();
            _token = _cancelTokenSource.Token;
            _updateUi = UpdateFace;
            _disk = disk;
            _ldi = new List<DirectoryInfo>();
            _audit = new Audit();
            _sfi = new List<SearchFileInfo>();
            _arrWords = arrWords;
          
            InitializeComponent();
            Tm = new Msg() { ListDir = _ldi, CancellationToken = _token, UpdateUiSearchForm = _updateUi, ListSfi = _sfi, BadWords = _arrWords.ToArray() };
           
        }

        private void UpdateFace(object obj)
        {
            if (InvokeRequired)
            {
                Invoke(_updateUi, obj);
            }
            else
            {
                Msg m = (Msg) obj;
                if (m.CurrentDirectory != null && m.Index > -1)
                {
                    label_CurentDir.Text = "";
                    label_CurentDir.Text = m.Index + " | " + m.CurrentDirectory;
                }
                else if (m.PrgBarFiles > -1)
                {

                        progressBarFileScan.Value = m.PrgBarFiles;
                    
                    
                    label_FileScan.Text = "";
                    label_FileScan.Text = m.PrgBarFiles + " | " + m.CurrentFile;
                }

            }
        }

        private void button_Start_Click(object sender, EventArgs e)
        {
            progressBar_Directory.Style= ProgressBarStyle.Marquee;
            Msg m = new Msg()
            {
                Disk = _disk,
                ListDir = Tm.ListDir,
                CancellationToken = Tm.CancellationToken,
                UpdateUiSearchForm = Tm.UpdateUiSearchForm,
                ListSfi = this.Tm.ListSfi,
                BadWords = Tm.BadWords
            };
            if (checkBox_BurnProc.Checked)
            {
                ThreadPool.QueueUserWorkItem(Audit.ScanDirsAggressively, m);
            }
            else
            {
                ThreadPool.QueueUserWorkItem(Audit.ScanDirsLight, m);
            }
        }

        private void button_Stop_Click(object sender, EventArgs e)
        {
            _cancelTokenSource.Cancel();
            progressBar_Directory.Style = ProgressBarStyle.Continuous;
            progressBar_Directory.Value = 0;
            progressBarFileScan.Value = 0;
            
        }

        private void ScanFileLight()
        {
            progressBarFileScan.Maximum = _ldi.Count;
            progressBar_Directory.Value = 0;

            Msg _m = new Msg()
            {
                ListDir = Tm.ListDir,
                CancellationToken = Tm.CancellationToken,
                UpdateUiSearchForm = Tm.UpdateUiSearchForm,
                ListSfi = this.Tm.ListSfi,
                BadWords = Tm.BadWords,
                FileSizeBr = long.Parse(numericUpDown_FileSizeIgnore.Value.ToString(CultureInfo.InvariantCulture)) * (long)Audit.FileSizeInBit.MBbt
            };
            ThreadPool.QueueUserWorkItem(Audit.ScanFileLight, _m);
            

        }
        private void ScanFileAggressively()
        {
            progressBarFileScan.Maximum = _ldi.Count;
            progressBar_Directory.Value = 0;
            Msg _m = new Msg()
            {
                ListDir = Tm.ListDir, 
                CancellationToken = Tm.CancellationToken,
                UpdateUiSearchForm = Tm.UpdateUiSearchForm,
                ListSfi = this.Tm.ListSfi,
                BadWords = Tm.BadWords,
                FileSizeBr = long.Parse(numericUpDown_FileSizeIgnore.Value.ToString(CultureInfo.InvariantCulture))*(long)Audit.FileSizeInBit.MBbt
            };
            ThreadPool.QueueUserWorkItem(Audit.ScanFilesAggressively, _m);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog f = new FolderBrowserDialog();
            if (f.ShowDialog() == DialogResult.OK)
            {
                progressBar_Directory.Style = ProgressBarStyle.Marquee;
                Msg _m = new Msg()
                {
                    ListDir = Tm.ListDir, 
                    CancellationToken = Tm.CancellationToken,
                    UpdateUiSearchForm = Tm.UpdateUiSearchForm,
                    Disk = f.SelectedPath,
                    ListSfi = this.Tm.ListSfi,
                    BadWords = Tm.BadWords
                };
                if (checkBox_BurnProc.Checked) { ThreadPool.QueueUserWorkItem(Audit.ScanDirsAggressively, _m); }
                else { ThreadPool.QueueUserWorkItem(Audit.ScanDirsLight, _m); }
                
            }
        }

        private void label_CurentDir_TextChanged(object sender, EventArgs e)
        {

            if (label_CurentDir.Text.Contains("Виконано"))
            {
                progressBar_Directory.Enabled = false;
                progressBar_Directory.Style = ProgressBarStyle.Continuous;
                progressBar_Directory.Value = 0;
                if (checkBox_BurnProc.Checked)
                {
                    ScanFileAggressively();
                }
                else
                {
                   ScanFileLight();
                }
            }
        }

        private void label_FileScan_TextChanged(object sender, EventArgs e)
        {
            if(label_FileScan.Text.Contains("Виконано"))
            {
                progressBarFileScan.Value = 0;
            }
        }
    }

   
}
