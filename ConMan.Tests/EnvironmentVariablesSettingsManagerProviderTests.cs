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
    public class EnvironmentVariablesSettingsManagerProviderTests
    {
        [Test]
        public void ReplaceDotWithUnderscore()
        {
            var env = new Mock<IEnvironment>();

            env.Setup(x => x.GetEnvironmentVariable(It.IsAny<string>()))
                .Callback<string>(s =>
                {
                    Assert.AreEqual("DOT_DELIMINATED_PATH", s);
                })
                .Returns("value");

            var provider = new EnvironmentVariablesSettingsManagerProvider(env.Object);
            Assert.AreEqual("value", provider.GetValue("DOT.DELIMINATED.PATH"));

            env.VerifyAll();
        }

        [Test]
        public void MakeKeyUppercase()
        {
            var env = new Mock<IEnvironment>();

            env.Setup(x => x.GetEnvironmentVariable(It.IsAny<string>()))
                .Callback<string>(s =>
                {
                    Assert.AreEqual("DOT_DELIMINATED_PATH", s);
                })
                .Returns("value");

            var provider = new EnvironmentVariablesSettingsManagerProvider(env.Object);
            Assert.AreEqual("value", provider.GetValue("Dot_Deliminated_Path"));

            env.VerifyAll();
        }

    }
}
