using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace unitils.cfs
{
    public class FileSystem
    {
        private Index Index;

        public bool EnableSHA1;

        public bool EnableMD5;

        public FileSystem(string indexFilename)
        {
            Index = new Index();
            Index.Load(File.ReadAllText(indexFilename));
        }

        public FileSystem(Index chunkIndex)
        {
            Index = chunkIndex;
        }

        public byte[] ReadAllBytes(string path)
        {
            return RebuildFile(path);
        }

        public string ReadAllText(string path)
        {
            return Encoding.UTF8.GetString(RebuildFile(path));
        }

        private byte[] RebuildFile(string id)
        {
            IndexFileEntry fileEntry = Index.GetFileEntry(id);

            byte[] fileData = new byte[fileEntry.size];

            for(int i = 0; i < fileEntry.chunks.Length; i++)
            {
                var chunk = fileEntry.chunks[i];
                var source = Index.sources[chunk.src];
                int numBytesToRead = chunk.size;
                byte[] chunkBytes = new byte[numBytesToRead];
                using (FileStream fsSource = new FileStream(source.filename, FileMode.Open, FileAccess.Read))
                {
                    fsSource.Read(chunkBytes, chunk.offset, numBytesToRead);
                }

                Buffer.BlockCopy(chunkBytes, 0, fileData, chunk.dstOffset, chunk.size);
            }

            if(EnableSHA1)
            {
                SHA1 sha = new SHA1CryptoServiceProvider();
                if(fileEntry.sha1Hash != ToHexString(sha.ComputeHash(fileData)))
                {
                    new Exception("SHA1 Hash Mismatch.");
                }
            }

            if(EnableMD5)
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                if(fileEntry.md5Hash != ToHexString(md5.ComputeHash(fileData)))
                {
                    new Exception("MD5 Hash Mismatch.");
                }
            }

            return fileData;
        }

        private static char ToHexDigit(int i)
        {
            if (i < 10)
            {
                return (char)(i + '0');
            }
            return (char)(i - 10 + 'A');
        }
        public static string ToHexString(byte[] bytes)
        {
            var chars = new char[bytes.Length * 2 + 2];

            chars[0] = '0';
            chars[1] = 'x';

            for (int i = 0; i < bytes.Length; i++)
            {
                chars[2 * i + 2] = ToHexDigit(bytes[i] / 16);
                chars[2 * i + 3] = ToHexDigit(bytes[i] % 16);
            }

            return new string(chars);
        }
    }
}
