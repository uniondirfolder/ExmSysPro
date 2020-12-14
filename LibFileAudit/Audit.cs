using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LibFileAudit
{
    
    public class Audit
    {
        public enum FileSizeInBit:long
        {
            GBbi=1_073_741_824,
            GBbt=8_589_934_592,
            MBbi=1_048_576,
            MBbt =8_388_608,
            KBbi = 1_024,
            KBbt = 8_192,
            Byte =8
        }
        static readonly object LockerDir = new object();
        private static bool ScanDirEnd = false;
        public static List<AuditInfo> AuditValid(string[] arrWord, FileInfo fi)
        {
            //Debug.WriteLine(fi.Name);
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
            //Debug.WriteLine(fi.Name);
            return ret;
        }
        private  void AuditValidTwo(object obj)
        {

    
                Msg msg = (Msg)obj;
                List<AuditInfo> ret = new List<AuditInfo>();

     
                    try
                    {
                        var file = File.ReadAllBytes(msg.FileInfo.FullName);
                        for (int i = 0; i < msg.BadWords.Length; i++)
                        {
                            var word = Encoding.Unicode.GetBytes(msg.BadWords[i]);
                            int numOfcoincs = 0;
                            for (int indxUp = word.Length; indxUp < file.Length; indxUp++)
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

                            ret.Add(new AuditInfo() { Count = numOfcoincs, WordIndexArr = i });
                        }
                        msg.ListSfi.Add(new SearchFileInfo() { FileInfo = msg.FileInfo, FindWords = ret });

                    }
                    catch
                    {
                        //
                    }

                
            

        }
        private static void AuditValid(object obj)
        {

            lock (LockerDir)
            {
                Msg msg = (Msg)obj;
                List<AuditInfo> ret = new List<AuditInfo>();

                if (msg.BadWords != null && msg.FileInfo != null && msg.ListSfi != null && !msg.CancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var file = File.ReadAllBytes(msg.FileInfo.FullName);
                        for (int i = 0; i < msg.BadWords.Length; i++)
                        {
                            var word = Encoding.Unicode.GetBytes(msg.BadWords[i]);
                            int numOfcoincs = 0;
                            for (int indxUp = word.Length; indxUp < file.Length; indxUp++)
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

                            ret.Add(new AuditInfo() { Count = numOfcoincs, WordIndexArr = i });
                        }
                        msg.ListSfi.Add(new SearchFileInfo() { FileInfo = msg.FileInfo, FindWords = ret });
                       
                    }
                    catch 
                    {
                        //
                    }

                }
            }
            
        }
        public static void ScanDirsAggressively(object obj)
        {
            Msg msg = (Msg) obj;

                ScanDirEnd = false;
            

            try
            {
                var dir = new DirectoryInfo(msg.Disk);
                msg.ListDir.Add(dir);

                    msg.ListDir.AddRange(dir.EnumerateDirectories());
                
                
            }
            catch 
            {
                //ACL except
            }

            if (msg.ListDir.Count > 0)
            {

                int i = 1;
                for (; i < msg.ListDir.Count && !msg.CancellationToken.IsCancellationRequested;)
                {
                    try
                    {
                        int countTh = msg.ListDir.Count - i > 10 ? 10 : msg.ListDir.Count - i;
                        Thread[] myPool = new Thread[countTh];
                        for (int j = 0; j < countTh; j++)
                        {

                            myPool[j] = new Thread(ThreadTaskDirSearch);
                            myPool[j].Start(new Msg() {ListDir = msg.ListDir, Index = i, UpdateUiSearchForm = msg.UpdateUiSearchForm});
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
                            if (msg.CancellationToken.IsCancellationRequested)
                            {
                                for (int j = 0; j < countTh; j++)
                                {
                                    if (myPool[j].IsAlive)
                                        myPool[j].Abort();
                                }

                                break;
                            }

                            Thread.Sleep(20);
                        }

                        lock (LockerDir)
                        {
                            for (int j = i; j < msg.ListDir.Count - countTh; j++, i++)
                            {
                                Msg m = new Msg() {ListDir = msg.ListDir, Index = i, UpdateUiSearchForm = msg.UpdateUiSearchForm};
                                ThreadPool.QueueUserWorkItem(ThreadTaskDirSearch, m);
                                
                            }
                            msg.UpdateUiSearchForm(new Msg() { CurrentDirectory = msg.ListDir[i].FullName, Index = i });
                        }
                    }
                    catch 
                    {
                        //ACL except
                    }

                }

                msg.UpdateUiSearchForm(new Msg() { CurrentDirectory = "Виконано", Index = i });
            }

            lock (LockerDir)
            {
                ScanDirEnd = true;
            }

        }
        static void ThreadTaskDirSearch(object obj)
        {
            Msg m = (Msg)obj;
            lock (LockerDir)
            {
                try
                {
                    var t = m.ListDir[m.Index].EnumerateDirectories();
                    m.ListDir.AddRange(t);
                    
                }
                catch
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

        public static void ScanDirsLight(object obj)
        {
            lock (LockerDir)
            {
                ScanDirEnd = false;
            }
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
                        for (int j = i; j < m.ListDir.Count && j<s; j++, i++)
                        {
                            m.ListDir.AddRange(m.ListDir[i].EnumerateDirectories());
                        }
                    }

                    m.UpdateUiSearchForm(new Msg() { CurrentDirectory = m.ListDir[i].FullName, Index = i });
                }
                catch 
                {
                    //ACL except
                }

                Thread.Sleep(2);
            }

            lock (LockerDir)
            {
                ScanDirEnd = true;
            }
            m.UpdateUiSearchForm(new Msg() {CurrentDirectory = "Виконано", Index = i});
        }

        public static void ScanFilesAggressively(object obj)
        {
            Msg m = (Msg)obj;
            if (m.UpdateUiSearchForm == null || m.ListSfi == null || m.ListDir == null)
                return;
            int i = 0;

            while (true)
            {
                FileInfo[] fi = null;
                lock (LockerDir)
                {

                    if (i < m.ListDir.Count)
                    {
                        try
                        {
                            fi = m.ListDir[i].GetFiles();
                        }
                        catch 
                        {
                            //
                        }

                    }
                }

                if (fi != null)
                {

                    for (int j = 0; j < fi.Length && !m.CancellationToken.IsCancellationRequested; j++)
                    {
                        if (m.FileSizeBr > fi[j].Length)
                        {
                            SearchFileInfo sfi = new SearchFileInfo();
                            sfi.FileInfo = fi[j];

                            Task<List<AuditInfo>> task =
                                new Task<List<AuditInfo>>(() => AuditValid(m.BadWords, sfi.FileInfo));

                            task.Start();

                            try
                            {
                                task.Wait(m.CancellationToken);
                                sfi.FindWords = task.Result;
                                lock (LockerDir)
                                {
                                    m.ListSfi.Add(sfi);
                                }

                                m.UpdateUiSearchForm(new Msg() { PrgBarFiles = i, CurrentFile = fi[j].Name });
                            }
                            catch
                            {
                                //
                            }

                        }
                    }
                }
                
                if (i == m.ListDir.Count && ScanDirEnd)
                    break;
                i++;
            }

            m.UpdateUiSearchForm(new Msg() {CurrentFile = "Виконано", PrgBarFiles = i});
        }

        public static void ScanFileLight(object obj)
        {
            Msg m = (Msg)obj;
            if (m.UpdateUiSearchForm == null || m.ListSfi == null || m.ListDir == null)
                return;
            int i = 0;
            int countTh = 15;
            if (m.ListDir.Count > 0)
            {
                Thread[] myPool = null;
                FileInfo[] fi = null;
                List<int> indexs = new List<int>();
                Audit a= new Audit();
                while (true)
                {
                    lock (LockerDir)
                    {
                        try
                        {
                            fi = m.ListDir[i].GetFiles();
                            if (fi.Length > 0)
                            {
                                int r = 0;
                                for (int j = 0; j < fi.Length; j++)
                                {
                                    if (fi.Length < m.FileSizeBr)
                                        indexs.Add(j);
                                }
                                var s = indexs.Count > countTh ? countTh : indexs.Count;
                                myPool = new Thread[s];
                                for (int j = 0; j < s; j++)
                                {
                                    myPool[j] = new Thread(a.AuditValidTwo);
                                    myPool[j].Start(new Msg() {BadWords = m.BadWords, FileInfo =fi[indexs[0]] , ListSfi = m.ListSfi, CancellationToken = m.CancellationToken});
                                    m.UpdateUiSearchForm(new Msg() {PrgBarFiles = i, CurrentFile = fi[indexs[0]].Name });
                                    indexs.RemoveAt(0);
                                }
                            }
                            else
                            {
                                fi = null;
                                goto Incr;
                                
                            }
                        }
                        catch
                        {
                            fi = null;
                            goto Incr;
                        }

                    }

                    if (myPool != null)
                    {

                        for (int j = 0; j < myPool.Length;)
                        {
                            if (myPool[j].IsAlive)
                                j = 0;
                            else
                            {
                                j++;
                            }

                            Thread.Sleep(20);
                        }

                        if (m.CancellationToken.IsCancellationRequested)
                        {
                            for (int j = 0; j < myPool.Length; j++)
                            {
                                if (myPool[j].IsAlive) myPool[j].Abort();
                            }

                        }
                    }

                    try
                    {

                        while (indexs.Count != 0)
                        {
                            Msg _m = new Msg() {BadWords = m.BadWords, FileInfo = fi[indexs[0]], ListSfi = m.ListSfi, CancellationToken = m.CancellationToken};
                            ThreadPool.QueueUserWorkItem(a.AuditValidTwo, _m);
                            indexs.RemoveAt(0);
                            Thread.Sleep(50);
                        }


                    }
                    catch
                    {
                        //
                    }

                    Incr:
                    i++;
                    //Debug.WriteLine($"{i} - {m.ListDir.Count} - {i == m.ListDir.Count}");
                    if (m.CancellationToken.IsCancellationRequested || i == m.ListDir.Count) break;
                }
                m.UpdateUiSearchForm(new Msg() { CurrentFile = "Виконано", PrgBarFiles = i });
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
        public FileInfo FileInfo;
        public long FileSizeBr;
        public int Index=-1;
        public int PrgBarFiles=-1;
        public string[] BadWords { get; set; }
        public string CurrentDirectory { get; set; }
        public string CurrentFile { get; set; }
    }
    
}