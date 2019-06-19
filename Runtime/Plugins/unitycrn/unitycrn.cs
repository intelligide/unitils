using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Assertions;

namespace unitils
{
    public static class UnityCRN
    {
        [DllImport("unitycrn", EntryPoint = "getDXTCData")]
        private static extern void _getDXTCData(byte[] dxtData, uint dxtDataSize);

        [DllImport("unitycrn", EntryPoint = "hasDXTCData")]
        private static extern bool _hasDXTCData();

        [DllImport("unitycrn", EntryPoint = "getDXTCDataLen")]
        private static extern uint _DXTCDataLen();

        [DllImport("unitycrn", EntryPoint = "dispose")]
        private static extern void _Dispose();

        [DllImport("unitycrn", EntryPoint = "transcode")]
        private static extern bool _Transcode(byte[] crnData, uint crnDataSize);

        public static byte[] Transcode(byte[] crnData)
        {
            if(!_Transcode(crnData, (uint) crnData.Length)) {
                throw new Exception("Cannot transcode CRN to DDS/DXT");
            }
            Assert.IsTrue(_hasDXTCData());
            byte[] dxtData = new byte[_DXTCDataLen()];
            _getDXTCData(dxtData, (uint) dxtData.Length);
            _Dispose();
            return dxtData;
        }

    }
}
