using System.IO;
using NUnit.Framework;
using UnityEditor;

namespace UnityEngine.Tests
{
    internal class TextureLoaderTests
    {
        [Test]
        public void CanLoad()
        {
            string imgTestPath = Path.GetFullPath("Packages/com.arsenstudio.unitils/Tests/Runtime/tex.dds");
            byte[] imgData = File.ReadAllBytes(imgTestPath);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadDXTImage(imgData);

            Assert.That(tex.width, Is.EqualTo(64));
            Assert.That(tex.height, Is.EqualTo(64));
        }
    }
}
