using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace SCKRM.Compress
{
    public static class CompressFileManager
    {
        public static void CompressFile(string sourceDirectoryPath, string targetFilePath)
        {
            DirectoryInfo sourceDirectoryInfo = new DirectoryInfo(sourceDirectoryPath);

            FileStream targetFileStream = new FileStream(targetFilePath, FileMode.Create);

            ZipOutputStream zipOutputStream = new ZipOutputStream(targetFileStream);

            zipOutputStream.SetComment(sourceDirectoryPath);

            zipOutputStream.SetLevel(9);

            byte[] bufferByteArray = new byte[2048];

            foreach (FileInfo sourceFileInfo in sourceDirectoryInfo.GetFiles("*.*", SearchOption.AllDirectories))
            {
                string sourceFileName = sourceFileInfo.FullName.Substring(sourceDirectoryInfo.FullName.Length + 1);

                zipOutputStream.PutNextEntry(new ZipEntry(sourceFileName));

                using (FileStream sourceFileStream = sourceFileInfo.OpenRead())
                {
                    while (true)
                    {
                        int readCount = sourceFileStream.Read(bufferByteArray, 0, bufferByteArray.Length);

                        if (readCount == 0)
                            break;

                        zipOutputStream.Write(bufferByteArray, 0, readCount);
                    }
                }
                zipOutputStream.CloseEntry();
            }

            zipOutputStream.Finish();

            zipOutputStream.Close();
        }

        public static void DecompressFile(string sourceFilePath, string targetDirectoryPath)
        {
            DirectoryInfo targetDirectoryInfo = new DirectoryInfo(targetDirectoryPath);

            if (!targetDirectoryInfo.Exists)
                targetDirectoryInfo.Create();

            FileStream sourceFileStream = new FileStream(sourceFilePath, FileMode.Open);

            ZipInputStream zipInputStream = new ZipInputStream(sourceFileStream);

            byte[] bufferByteArray = new byte[2048];

            while (true)
            {
                ZipEntry zipEntry = zipInputStream.GetNextEntry();

                if (zipEntry == null)
                    break;

                if (zipEntry.Name.LastIndexOf('\\') > 0)
                {
                    string subdirectory = zipEntry.Name.Substring(0, zipEntry.Name.LastIndexOf('\\'));

                    if (!Directory.Exists(Path.Combine(targetDirectoryInfo.FullName, subdirectory)))
                        targetDirectoryInfo.CreateSubdirectory(subdirectory);
                }

                FileInfo targetFileInfo = new FileInfo(Path.Combine(targetDirectoryInfo.FullName, zipEntry.Name));

                using (FileStream targetFileStream = targetFileInfo.Create())
                {
                    while (true)
                    {
                        int readCount = zipInputStream.Read(bufferByteArray, 0, bufferByteArray.Length);

                        if (readCount == 0)
                            break;

                        targetFileStream.Write(bufferByteArray, 0, readCount);
                    }
                }
            }

            zipInputStream.Close();
        }
    }
}