using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConMan.Providers;
using ConMan.Wrappers;
using Moq;
using NUnit.Framework;

namespace ConMan.Tests
{
    [TestFixture]
    public class AppSettingsManagerProviderTests
    {
        [Test]

        public void ReadDotDeliminated()
        {
            var provider = new AppSettingsSettingsManagerProvider();
            Assert.AreEqual("dot", provider.GetValue("Dot.Deliminated.Path"));
        }

        [Test]
        public void ReadColonDeliminated()
        {
            var provider = new AppSettingsSettingsManagerProvider();
            Assert.AreEqual("colon", provider.GetValue("Colon:Deliminated:Path"));
        }

        [Test]

        public void ReadSwitchToDotDeliminated()
        {
            var provider = new AppSettingsSettingsManagerProvider();
            Assert.AreEqual("colon", provider.GetValue("Colon.Deliminated.Path"));
        }

        [Test]
        public void DoesNotSwitchToDotDeliminated()
        {
            var provider = new AppSettingsSettingsManagerProvider();
            Assert.IsNull(provider.GetValue("Dot:Deliminated:Path"));
        }

        [Test]
        public void ColonDelimitedWinsInAppConfig()
        {
            var provider = new AppSettingsSettingsManagerProvider();
            Assert.AreEqual("colon", provider.GetValue("Priority.Deliminated"));
        }
    }
}
