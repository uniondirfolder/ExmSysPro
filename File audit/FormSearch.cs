using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
                    progressBarFileScan.Maximum = m.Index;
                    progressBar_Directory.Value = 1;
                }
                else if (m.PrgBarFiles > -1)
                {
                    progressBarFileScan.Value = m.PrgBarFiles;

                }
            }
        }

        private void Start()
        {
            Task<List<DirectoryInfo>> t = new Task<List<DirectoryInfo>>(()=> Audit.ScanDirsOnDisk(_disk,_token,_updateUi));
            t.Start();
        }

        private void button_Start_Click(object sender, EventArgs e)
        {
            if (checkBox_BurnProc.Checked == true)
            {
                Start();
            }
            else
            {
                Msg m = new Msg() { ListDir = _ldi, CancellationToken = _token, UpdateUiSearchForm = _updateUi, Disk = _disk };
                ThreadPool.QueueUserWorkItem(_audit.ScanDir, m);
            }
            Thread.Sleep(1000);
            ScanFile();
        }

        private void button_Stop_Click(object sender, EventArgs e)
        {
            _cancelTokenSource.Cancel();
        }

        private void ScanFile()
        {
 
            Msg _m = new Msg() { ListDir = Tm.ListDir, CancellationToken = Tm.CancellationToken, UpdateUiSearchForm = Tm.UpdateUiSearchForm, ListSfi = this.Tm.ListSfi, BadWords = Tm.BadWords };
            ThreadPool.QueueUserWorkItem(_audit.ScanFiles, _m);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog f = new FolderBrowserDialog();
            if (f.ShowDialog() == DialogResult.OK)
            {

                Msg _m = new Msg() { ListDir = Tm.ListDir, CancellationToken = Tm.CancellationToken, UpdateUiSearchForm = Tm.UpdateUiSearchForm, Disk = f.SelectedPath, ListSfi = this.Tm.ListSfi, BadWords = Tm.BadWords};
                ThreadPool.QueueUserWorkItem(_audit.ScanDir, _m);
                Thread.Sleep(1000);
                ScanFile();
            }
        }
    }

   
}
