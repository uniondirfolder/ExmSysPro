using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace LibFileAudit
{
    public static class Audit
    {
        public static List<AuditInfo> IsAuditValid(string[] arrWord, FileInfo fi)
        {
            List <AuditInfo> ret = new List<AuditInfo>();
            if (arrWord == null || fi == null)
                return ret;
            try
            {
                var file = File.ReadAllBytes(fi.FullName);
                for (int i = 0; i < arrWord.Length; i++)
                {
                    var word = Encoding.Unicode.GetBytes(arrWord[i]);
                    int numOfcoincs = 0;
                    for (int indxUp = word.Length, w=0; indxUp < file.Length; indxUp++)
                    {
                        if (file[indxUp] == word[0])
                        {
                            bool find = true;
                            for (int indxL = indxUp - word.Length+1, indxArr = 1; indxL < indxUp; indxL++, indxArr++)
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
                if(ret.Count>0)
                    ret.Clear();
                return ret;
            }
            return ret;
        }
        public static List<DirectoryInfo> GetDirsOnDisk(string diskRoot)
        {
            List<DirectoryInfo> di = new List<DirectoryInfo>();
            if (diskRoot == null)
                return di;
            try
            {
                var dir = new DirectoryInfo(diskRoot);
                di.AddRange(dir.EnumerateDirectories());
            }
            catch (Exception e)
            {
                //ACL except
            }

            if (di.Count > 0)
            {
                for (int i = 1; i < di.Count; i++)
                {
                    try
                    {
                        di.AddRange(di[i].EnumerateDirectories());
                    }
                    catch (Exception e)
                    {
                        //ACL except
                    }

                }
            }

            return di;
        }

        public static List<string> GetWordsFromString(string text)
        {
            List<string> ret = new List<string>();
            if (text == null)
                return ret;
            for (int i = 0; i < text.Length;i++)
            {
                string add = "";
                while (i<text.Length)
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
                if (add.Length > 0) { ret.Add(add); }
            }

            return ret;
        }
    }
}