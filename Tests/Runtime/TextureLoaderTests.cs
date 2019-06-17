using System.IO;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace unitils.Tests
{
    internal class TextureLoaderTests
    {
        [Test]
        public void CanLoad()
        {
            string imgTestPath = Path.GetFullPath("Packages/com.arsenstudio.unitils/Tests/Runtime/tex.dds");
            byte[] imgData = File.ReadAllBytes(imgTestPath);
            Texture2D tex;
            ImageConversion.LoadDXTImage(imgData, out tex);

            Assert.NotNull(tex);
            Assert.That(tex.width, Is.EqualTo(64));
            Assert.That(tex.height, Is.EqualTo(64));
        }
    }
}
