using System;
using System.Collections.Generic;
using UnityEngine;

namespace unitils.cfs
{
    [Serializable]
    public class IndexFileChunk
    {
        public int src = -1;
        public int offset = -1;
        public int dstOffset= -1;
        public int size= -1;
    }

    [Serializable]
    public class IndexFileEntry
    {
        public string id = null;
        public IndexFileChunk[] chunks = null;
        public string sha1Hash = null;
        public string md5Hash = null;
        public int size= -1;
    }

    [Serializable]
    public class IndexSource
    {
        public string filename = null;
    }

    [Serializable]
    public class Index
    {
        public IndexFileEntry[] files = null;

        private Dictionary<string, int> idmap;

        public IndexSource[] sources = null;

        public void Load(string jsonData)
        {
            JsonUtility.FromJsonOverwrite(jsonData, this);
            if(files != null)
            {
                for(int i = 0; i < files.Length; i++)
                {
                    idmap.Add(files[i].id, i);
                }
            }
        }

        public IndexFileEntry GetFileEntry(string id)
        {
            try
            {
                return files[idmap[id]];
            }
            catch (KeyNotFoundException e)
            {
                throw new KeyNotFoundException("'" + id + "' doesn't exists in this index.", e);
            }
        }
    }
}
