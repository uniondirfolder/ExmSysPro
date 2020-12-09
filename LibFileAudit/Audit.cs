using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LibFileAudit
{

    public class Audit
    {
        static readonly object LockerDir = new object();

        public static List<AuditInfo> AuditValid(string[] arrWord, FileInfo fi)
        {
            List<AuditInfo> ret = new List<AuditInfo>();
            if (arrWord == null || fi == null)
                return ret;
            try
            {
                var file = File.ReadAllBytes(fi.FullName);
                for (int i = 0; i < arrWord.Length; i++)
                {
                    var word = Encoding.Unicode.GetBytes(arrWord[i]);
                    int numOfcoincs = 0;
                    for (int indxUp = word.Length, w = 0; indxUp < file.Length; indxUp++)
                    {
                        if (file[indxUp] == word[0])
                        {
                            bool find = true;
                            for (int indxL = indxUp - word.Length + 1, indxArr = 1; indxL < indxUp; indxL++, indxArr++)
                            {
                                if (word[indxArr] == file[indxL])
                                    continue;
                                find = false;
                                break;
                            }

                            if (find)
                                numOfcoincs++;
                        }
                    }

                    ret.Add(new AuditInfo() {Count = numOfcoincs, WordIndexArr = i});
                }
            }
            catch (Exception e)
            {
                if (ret.Count > 0)
                    ret.Clear();
                return ret;
            }

            return ret;
        }

        public static List<DirectoryInfo> ScanDirsOnDisk(string diskRoot, CancellationToken ct, UpdateUiSearchForm del)
        {

            CancellationToken _ct = ct;
            UpdateUiSearchForm _del = del;

            List<DirectoryInfo> listDir = new List<DirectoryInfo>();
            if (diskRoot == null)
                return listDir;
            try
            {
                var dir = new DirectoryInfo(diskRoot);

                listDir.AddRange(dir.EnumerateDirectories());
            }
            catch (Exception e)
            {
                //ACL except
            }

            if (listDir.Count > 0)
            {

                int i = 1;
                for (; i < listDir.Count && !_ct.IsCancellationRequested;)
                {
                    try
                    {
                        int countTh = listDir.Count - i > 100 ? 100 : listDir.Count - i;
                        Thread[] myPool = new Thread[countTh];
                        for (int j = 0; j < countTh; j++)
                        {

                            myPool[j] = new Thread(ThreadTaskDirSearch);
                            myPool[j].Start(new Msg() {ListDir = listDir, Index = i, UpdateUiSearchForm = _del});
                            i++;
                        }

                        while (true)
                        {
                            int countLiveThr = countTh;
                            for (int j = 0; j < countTh; j++)
                            {
                                if (!myPool[j].IsAlive)
                                    countLiveThr--;
                            }

                            if (countLiveThr == 0)
                                break;
                            if (_ct.IsCancellationRequested)
                            {
                                for (int j = 0; j < countTh; j++)
                                {
                                    if (myPool[j].IsAlive)
                                        myPool[j].Abort();
                                }

                                break;
                            }

                            Thread.Sleep(100);
                        }

                        lock (LockerDir)
                        {
                            for (int j = i; j < listDir.Count - countTh; j++, i++)
                            {
                                Msg m = new Msg() {ListDir = listDir, Index = i, UpdateUiSearchForm = _del};
                                ThreadPool.QueueUserWorkItem(ThreadTaskDirSearch, m);
                            }

                            _del(new Msg() {CurrentDirectory = listDir[i].FullName, Index = i});
                        }


                        //di.AddRange(di[i].EnumerateDirectories());
                        //_del(new Msg(){CurrentDirectory = di[i].FullName});
                    }
                    catch (Exception e)
                    {
                        //ACL except
                    }


                }

                _del(new Msg() {CurrentDirectory = $"Завершено {i}"});
            }

            return listDir;
        }

        static void ThreadTaskDirSearch(object obj)
        {
            lock (LockerDir)
            {
                Msg m = (Msg) obj;
                try
                {
                    if (m.ListDir != null)
                    {
                        m.ListDir.AddRange(m.ListDir[m.Index].EnumerateDirectories());
                    }
                }
                catch (Exception e)
                {

                    //ACL except
                }
            }
        }

        public static List<string> GetWordsFromString(string text)
        {
            List<string> ret = new List<string>();
            if (text == null)
                return ret;
            for (int i = 0; i < text.Length; i++)
            {
                string add = "";
                while (i < text.Length)
                {
                    if (char.IsLetterOrDigit(text[i]) && !char.IsWhiteSpace(text[i]))
                    {
                        add += text[i];
                    }
                    else
                    {
                        break;
                    }

                    i++;
                }

                if (add.Length > 0)
                {
                    ret.Add(add);
                }
            }

            return ret;
        }

        public void ScanDir(object obj)
        {
            Msg m = (Msg) obj;
            if (m.Disk == null || m.UpdateUiSearchForm == null || m.ListDir == null)
                return;
            try
            {
                
                var dir = new DirectoryInfo(m.Disk);
                m.ListDir.Add(dir);
                m.ListDir.AddRange(dir.EnumerateDirectories());
            }
            catch (Exception e)
            {
                //ACL except
            }

            int i = 1;
            for (; i < m.ListDir.Count && !m.CancellationToken.IsCancellationRequested; i++)
            {
                try
                {
                    lock (LockerDir)
                    {
                        int s = i + 50;
                        for (int j = i; j < m.ListDir.Count && j<s; j++)
                        {
                            m.ListDir.AddRange(m.ListDir[i].EnumerateDirectories());
                        }
                    }

                    m.UpdateUiSearchForm(new Msg() { CurrentDirectory = m.ListDir[i].FullName, Index = i });
                }
                catch (Exception e)
                {
                    //ACL except
                }

                Thread.Sleep(2);
            }

            m.UpdateUiSearchForm(new Msg() {CurrentDirectory = "Завершено", Index = i});
        }

        public void ScanFiles(object obj)
        {
            Msg m = (Msg)obj;
            if (m.UpdateUiSearchForm == null || m.ListSfi == null || m.ListDir == null)
                return;
            int i = 0;
            try
            {
                while (true)
                {
                    FileInfo[] fi = null;
                    lock (LockerDir)
                    {
                        if (i < m.ListDir.Count)
                        {
                            fi = m.ListDir[i].GetFiles("*.*");
                        }
                    }

                    if (i < m.ListDir.Count && fi != null)
                    {
                        i++;
                        for (int j = 0; j < fi.Length && !m.CancellationToken.IsCancellationRequested; j++)
                        {
                            SearchFileInfo sfi = new SearchFileInfo();
                            sfi.FileInfo = fi[j];

                            Task<List<AuditInfo>> task =
                                new Task<List<AuditInfo>>(() => AuditValid(m.BadWords, sfi.FileInfo));
                            task.Start();
                            task.Wait(m.CancellationToken);
                            sfi.FindWords = task.Result;
                            m.ListSfi.Add(sfi);
                        }
                    }
                    else
                    {
                        break;
                    }
                    m.UpdateUiSearchForm(new Msg(){PrgBarFiles = i});
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    public delegate void UpdateUiSearchForm(object obj);
    public class Msg
    {
        public string Disk;
        public UpdateUiSearchForm UpdateUiSearchForm;
        public CancellationToken CancellationToken;
        public List<DirectoryInfo> ListDir;
        public List<SearchFileInfo> ListSfi;
        public int Index=-1;
        public int PrgBarFiles=-1;
        public string[] BadWords { get; set; }
        public string CurrentDirectory { get; set; }
    }
    
}