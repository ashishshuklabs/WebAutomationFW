using System.Collections.Generic;
using WebAutomation.Framework.Core.Utilities;
using NUnit.Framework;

namespace WebAutomation.Framework.Core.Tests.UnitTests {
    [TestFixture()]
    [Parallelizable(ParallelScope.All)]
    [Category("Unit Tests")]
    public class WebTestContextTests {
        [Test]
        public void SetPropertyWithExistingKeyShouldOverwriteExistingValue() {
            WebTestContext.Set("a", "standard value");
            Assert.True(WebTestContext.Get<string>("a").Equals("standard value"), "Value should have persisted");
            WebTestContext.Set("a", "default value");
            Assert.True(WebTestContext.Get<string>("a").Equals("default value"), "Value should have been overwritten");
        }
        [Test]
        public void SetPropertyWithNewKeyPasses() {
            WebTestContext.Set("a", "SetPropertyWithNewKey");
            Assert.True(WebTestContext.Get<string>("a").Equals("SetPropertyWithNewKey"), "Value should have persisted");

        }

        [Test]
        public void SetPropertyWithNonExistentKeyThrows() {
            WebTestContext.Set("a", "SetPropertyWithNewKey");
            Assert.Throws<KeyNotFoundException>(() => WebTestContext.Get<string>("b"), "Key does not exist, should have thrown");

        }

        [Test]
        public void ContainsPassesForValueSavedInContext() {
            WebTestContext.Set("a", "SetPropertyWithNewKey");
            Assert.True(WebTestContext.ContainsKey("a"), "Key exists, must return true.");

        }
    }
}
