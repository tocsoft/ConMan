using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConMan.Providers;
using Moq;
using NUnit.Framework;

namespace ConMan.Tests
{
    [TestFixture]
    public class SettingsManagerTests
    {

        [Test]
        public void VerifiytAllProvidersCalledinRegistrationOrder()
        {

            var provider1 = new Mock<ISettingsManagerProvider>();
            var provider2 = new Mock<ISettingsManagerProvider>();

            int callOrder = 0;

            provider1.Setup(x => x.GetValue("path")).Callback(() =>
            {
                callOrder++;
                Assert.AreEqual(1, callOrder);
            });
            provider2.Setup(x => x.GetValue("path")).Callback(() =>
            {
                callOrder++;
                Assert.AreEqual(2, callOrder);
            });

            var manager = new SettingsManager();

            manager.RegisterProvider(provider1.Object);
            manager.RegisterProvider(provider2.Object);

            Assert.IsNull(manager.GetSetting("path"));
        }

        [Test]
        public void Provider1ResultsPreventProvider2BeingCalled()
        {

            var provider1 = new Mock<ISettingsManagerProvider>();
            var provider2 = new Mock<ISettingsManagerProvider>();

            int callOrder = 0;

            provider1.Setup(x => x.GetValue("path")).Callback(() =>
            {
                callOrder++;
                Assert.AreEqual(1, callOrder);
            }).Returns("value");

            var manager = new SettingsManager();

            manager.RegisterProvider(provider1.Object);
            manager.RegisterProvider(provider2.Object);

            Assert.AreEqual("value", manager.GetSetting("path"));

            provider2.Verify(x => x.GetValue("path"), Times.Never);
        }


        [Test]
        public void FirstProviderNotNullWins()
        {
            var provider1 = new Mock<ISettingsManagerProvider>();
            var provider2 = new Mock<ISettingsManagerProvider>();

            int callOrder = 0;

            provider2.Setup(x => x.GetValue("path")).Callback(() =>
            {
                callOrder++;
                Assert.AreEqual(1, callOrder);
            }).Returns("value");

            var manager = new SettingsManager();

            manager.RegisterProvider(provider1.Object);
            manager.RegisterProvider(provider2.Object);

            Assert.AreEqual("value", manager.GetSetting("path"));

            provider1.Verify(x => x.GetValue("path"), Times.Once);
            provider2.Verify(x => x.GetValue("path"), Times.Once);
        }

    }
}
