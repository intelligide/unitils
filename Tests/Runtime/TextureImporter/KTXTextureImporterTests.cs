using System;
using System.IO;
using NUnit.Framework;
using unitils.textures;

namespace unitils.Tests
{
    internal class KTXTextureImporterTests
    {
        [Test]
        public void CanLoad()
        {
            string imgTestPath = Path.GetFullPath("Packages/com.arsenstudio.unitils/Tests/Runtime/textures/lenna.ktx");
            byte[] imgData = File.ReadAllBytes(imgTestPath);
            var ktxTexture = new KTXTextureImporter(imgData);

            AssertLennaImg(ktxTexture);
        }

        private void AssertLennaImg(KTXTextureImporter tex)
        {
            Assert.That(tex.Identifier, Is.EqualTo(new byte[]{ (byte)'«', (byte)'K', (byte)'T', (byte)'X', (byte)' ', (byte)'1', (byte)'1', (byte)'»', (byte)'\r', (byte)'\n', (byte)'\x1A', (byte)'\n' }));
            Assert.That(tex.Endianess, Is.EqualTo(0x04030201));
            Assert.That(tex.GlType, Is.Zero);
            Assert.That(tex.GlTypeSize, Is.EqualTo(1));

            Assert.That((UInt32) tex.GlFormat, Is.Zero);
            Assert.That(tex.GlInternalFormat, Is.EqualTo(KTXPixelFormat.COMPRESSED_RGB8_ETC2));
            Assert.That(tex.GlBaseInternalFormat, Is.EqualTo(KTXPixelFormat.RGB));

            Assert.That(tex.Width, Is.EqualTo(512));
            Assert.That(tex.Height, Is.EqualTo(512));
            Assert.That(tex.Depth, Is.EqualTo(0));
            Assert.That(tex.NumArrayElements, Is.EqualTo(0));
            Assert.That(tex.NumFaces, Is.EqualTo(1));
            Assert.That(tex.MipMapCount, Is.EqualTo(10));

            Assert.That(tex.KeyValuePairs, Is.Not.Null);
            Assert.That(tex.KeyValuePairs.Count, Is.EqualTo(1));
            Assert.That(tex.KeyValuePairs, Contains.Key("KTXOrientation"));
            Assert.That(tex.KeyValuePairs["KTXOrientation"], Is.EqualTo("S=r,T=d,R=i"));

            Assert.That(tex.DataOffset, Is.EqualTo(96));


        }
    }
}
