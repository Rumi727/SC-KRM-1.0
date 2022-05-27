using System;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using SCKRM.Threads;
using UnityEngine;

namespace SCKRM.Compress
{
    public static class CompressFileManager
    {
        public static bool CompressZipFile(string sourceDirectory, string zipFilePath, string password = "", ThreadMetaData threadMetaData = null)
        {
            bool retVal = false;

            //폴더가 존재하는 경우에만 수행
            if (Directory.Exists(sourceDirectory))
            {
                //압축 대상 폴더의 파일 목록
                List<string> fileList = GenerateFileList(sourceDirectory);

                //압축 대상 폴더 경로의 길이 + 1
                int TrimLength = (Directory.GetParent(sourceDirectory)).ToString().Length + 1;

                //find number of chars to remove. from orginal file path. remove '\'
                FileStream ostream;
                byte[] obuffer;
                string outPath = zipFilePath;

                //ZIP 스트림 생성
                using ZipOutputStream oZipStream = new ZipOutputStream(File.Create(outPath));

                try
                {
                    //패스워드가 있는 경우 패스워드 지정
                    if (password != null && password != string.Empty)
                        oZipStream.Password = password;

                    oZipStream.SetLevel(9); //암호화 레벨 (최대 압축)

                    if (threadMetaData != null)
                    {
                        threadMetaData.name = "compress_file_manager.compress";
                        threadMetaData.info = "";

                        threadMetaData.progress = 0;
                        threadMetaData.maxProgress = fileList.Count;
                    }

                    ZipEntry oZipEntry;
                    for (int i = 0; i < fileList.Count; i++)
                    {
                        string Fil = fileList[i];
                        oZipEntry = new ZipEntry(Fil.Remove(0, TrimLength));
                        oZipStream.PutNextEntry(oZipEntry);

                        //파일인 경우
                        if (!Fil.EndsWith(@"/"))
                        {
                            ostream = File.OpenRead(Fil);
                            obuffer = new byte[ostream.Length];
                            ostream.Read(obuffer, 0, obuffer.Length);
                            oZipStream.Write(obuffer, 0, obuffer.Length);
                        }

                        if (threadMetaData != null)
                        {
                            threadMetaData.info = fileList[i];
                            threadMetaData.progress = i + 1;
                        }
                    }

                    retVal = true;
                }
                catch (Exception e)
                {
                    retVal = false;

                    //오류가 난 경우 생성 했던 파일을 삭제
                    if (File.Exists(outPath))
                        File.Delete(outPath);

                    Debug.LogException(e);
                }
                finally
                {
                    threadMetaData.info = "";

                    //압축 종료
                    oZipStream.Finish();
                    oZipStream.Close();
                }
            }
            return retVal;
        }

        private static List<string> GenerateFileList(string Dir)
        {
            List<string> fils = new List<string>();
            bool Empty = true;

            //폴더 내의 파일 추가
            string[] filePaths = Directory.GetFiles(Dir);
            for (int i = 0; i < filePaths.Length; i++)
            {
                string file = filePaths[i];
                fils.Add(file);
                Empty = false;
            }

            //파일이 없고, 폴더도 없는 경우 자신의 폴더 추가
            if (Empty && Directory.GetDirectories(Dir).Length == 0)
                fils.Add(Dir + @"/");

            //폴더 내 폴더 목록.
            string[] paths = Directory.GetDirectories(Dir);
            for (int i = 0; i < paths.Length; i++)
            {
                string dirs = paths[i];
                //해당 폴더로 다시 GenerateFileList 재귀 호출
                List<string> generateFileList = GenerateFileList(dirs);
                for (int i1 = 0; i1 < generateFileList.Count; i1++)
                    //해당 폴더 내의 파일, 폴더 추가.
                    fils.Add(generateFileList[i1]);
            }

            return fils;
        }

        public static bool DecompressZipFile(string zipFilePath, string targetDirectory, string password = "", ThreadMetaData threadMetaData = null)
        {
            bool retVal = false;

            //ZIP 파일이 있는 경우만 수행
            if (File.Exists(zipFilePath))
            {
                using (ZipFile zipFile = new ZipFile(File.OpenRead(zipFilePath)))
                {
                    if (threadMetaData != null)
                    {
                        threadMetaData.name = "compress_file_manager.decompress";
                        threadMetaData.info = "";

                        threadMetaData.progress = 0;
                        threadMetaData.maxProgress = zipFile.Count;
                    }
                }

                //ZIP 스트림 생성.
                using ZipInputStream zipInputStream = new ZipInputStream(File.OpenRead(zipFilePath));

                //패스워드가 있는 경우 패스워드 지정
                if (password != null && password != string.Empty)
                    zipInputStream.Password = password;

                try
                {
                    ZipEntry theEntry;
                    //반복하며 파일을 가져옴.
                    while ((theEntry = zipInputStream.GetNextEntry()) != null)
                    {
                        //폴더
                        string directoryName = Path.GetDirectoryName(theEntry.Name);
                        string fileName = Path.GetFileName(theEntry.Name); // 파일

                        //폴더 생성
                        Directory.CreateDirectory(PathTool.Combine(targetDirectory, directoryName));

                        //파일 이름이 있는 경우
                        if (fileName != string.Empty)
                        {
                            //파일 스트림 생성 (파일생성)
                            using FileStream streamWriter = File.Create(PathTool.Combine(targetDirectory, theEntry.Name));

                            int size = 2048;
                            byte[] data = new byte[2048];

                            //파일 복사
                            while (true)
                            {
                                size = zipInputStream.Read(data, 0, data.Length);

                                if (size > 0)
                                    streamWriter.Write(data, 0, size);
                                else
                                    break;
                            }

                            //파일스트림 종료
                            streamWriter.Close();

                            if (threadMetaData != null)
                            {
                                threadMetaData.info = theEntry.Name;
                                threadMetaData.progress++;
                            }
                        }
                    }

                    retVal = true;
                }
                catch (Exception e)
                {
                    retVal = false;
                    Debug.LogException(e);
                }
                finally
                {
                    threadMetaData.info = "";

                    //ZIP 파일 스트림 종료
                    zipInputStream.Close();
                }
            }

            return retVal;
        }
    }
}