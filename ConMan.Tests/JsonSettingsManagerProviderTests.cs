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
    public class JsonSettingsManagerProviderTests
    {
        [Test]

        public void LoadSettingsFromFile()
        {
            var fs = new Mock<IFileSystem>();
            
            fs.Setup(f => f.ReadAllText("path")).Returns("{ value : { path : 'data'} }");

            var provider = new JsonSettingsManagerProvider("path", fs.Object);

            var actual = provider.GetValue("value.path");

            fs.VerifyAll();
            Assert.AreEqual("data", actual);
        }

        [Test]
        public void ValueCachedUntilFileChanges()
        {
            var fs = new Mock<IFileSystem>();
            string json = "{ value : { path : 'data'} }";
            fs.Setup(f => f.ReadAllText("path")).Returns(() => json);

            //this is the call back that should be called to allert system of file changes
            Action callbackToInvalidateData = null;
            fs.Setup(f => f.MonitorFileChanges("path", It.IsAny<Action>())).Callback<string, Action>((path, act) => {
                callbackToInvalidateData = act;
            });

            var provider = new JsonSettingsManagerProvider("path", fs.Object);


            Assert.AreEqual("data", provider.GetValue("value.path"));
            
            //change the response
            json = "{ value : { path : 'data2'} }";
            var actual2 = provider.GetValue("value.path");

            Assert.AreEqual("data", actual2);


            //only read the file fom disk once
            fs.Verify(x => x.ReadAllText("path"), Times.Exactly(1));
        }

        [Test]
        public void ValueCachedInvalidatedWhenFileChanges()
        {
            var fs = new Mock<IFileSystem>();
            string json = "{ value : { path : 'data'} }";
            fs.Setup(f => f.ReadAllText("path")).Returns(() => json);

            //this is the call back that should be called to allert system of file changes
            Action callbackToInvalidateData = null;
            fs.Setup(f => f.MonitorFileChanges("path", It.IsAny<Action>())).Callback<string, Action>((path, act) => {
                callbackToInvalidateData = act;
            });

            var provider = new JsonSettingsManagerProvider("path", fs.Object);
            
            Assert.AreEqual("data", provider.GetValue("value.path"));

            //invalidate data
            callbackToInvalidateData();
            //change the response
            json = "{ value : { path : 'data2'} }";
            var actual2 = provider.GetValue("value.path");
            Assert.AreEqual("data2", actual2);

            //read it twice because file invalidated
            fs.Verify(x => x.ReadAllText("path"), Times.Exactly(2));
        }
    }
}
