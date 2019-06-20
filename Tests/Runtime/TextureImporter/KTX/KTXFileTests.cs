using System;
using System.IO;
using NUnit.Framework;
using unitils.ktx;
using UnityEditor;
using UnityEngine;

namespace unitils.Tests
{
    internal class KTXFileTests
    {
        [Test]
        public void CanLoad()
        {
            string imgTestPath = Path.GetFullPath("Packages/com.arsenstudio.unitils/Tests/Runtime/textures/lenna.ktx");
            byte[] imgData = File.ReadAllBytes(imgTestPath);
            var ktxFile = new KTXFile(imgData);

            AssertLennaImg(ktxFile);
        }

        private void AssertLennaImg(KTXFile file)
        {
            Assert.That(file.Header, Is.Not.Null);
            Assert.That(file.Header.Identifier, Is.EqualTo(new byte[]{ (byte)'«', (byte)'K', (byte)'T', (byte)'X', (byte)' ', (byte)'1', (byte)'1', (byte)'»', (byte)'\r', (byte)'\n', (byte)'\x1A', (byte)'\n' }));
            Assert.That(file.Header.Endianess, Is.EqualTo(0x04030201));
            Assert.That(file.Header.GlType, Is.Zero);
            Assert.That(file.Header.GlTypeSize, Is.EqualTo(1));

            Assert.That((UInt32) file.Header.GlFormat, Is.Zero);
            Assert.That(file.Header.GlInternalFormat, Is.EqualTo(KTXPixelFormat.COMPRESSED_RGB8_ETC2));
            Assert.That(file.Header.GlBaseInternalFormat, Is.EqualTo(KTXPixelFormat.RGB));

            Assert.That(file.Header.Width, Is.EqualTo(512));
            Assert.That(file.Header.Height, Is.EqualTo(512));
            Assert.That(file.Header.Depth, Is.EqualTo(0));
            Assert.That(file.Header.NumArrayElements, Is.EqualTo(0));
            Assert.That(file.Header.NumFaces, Is.EqualTo(1));
            Assert.That(file.Header.MipMapCount, Is.EqualTo(10));

            Assert.That(file.Header.KeyValuePairs, Is.Not.Null);
            Assert.That(file.Header.KeyValuePairs.Count, Is.EqualTo(1));
            Assert.That(file.Header.KeyValuePairs, Contains.Key("KTXOrientation"));
            Assert.That(file.Header.KeyValuePairs["KTXOrientation"], Is.EqualTo("S=r,T=d,R=i"));

            Assert.That(file.Header.DataOffset, Is.EqualTo(96));


        }
    }
}
