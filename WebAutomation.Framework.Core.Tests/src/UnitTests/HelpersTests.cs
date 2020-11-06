using WebAutomation.Framework.Core.Default;
using WebAutomation.Framework.Core.Utilities;
using NUnit.Framework;
using System;

namespace WebAutomation.Framework.Core.Tests.UnitTests {
    [TestFixture]
    [Category("Unit Tests")]
    public class HelpersTests {
        [Test]
        public void InvalidBrowserNameThrows() {
            Assert.Throws<NotSupportedException>(() => Helpers.SetBrowserType("Moby"), "Unsupported browsers should throw.");
        }
        [Test]
        public void ValidBrowserNameDoesNotThrow() {
            Assert.DoesNotThrow(() => Helpers.SetBrowserType("edge"), "Unsupported browsers should throw.");
        }
        [Test]
        public void ValidBrowserNameReturnsCorrectValue() {
            Assert.DoesNotThrow(() => Helpers.SetBrowserType("ie"), "Unsupported browsers should throw.");
            Assert.AreEqual(BrowserType.IE, Helpers.SetBrowserType("ie"), "Should have returned IE browser.");

        }
    }
}
