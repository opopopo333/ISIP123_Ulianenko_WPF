using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using PR14.Pages;

namespace UnitTestProject2
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void AuthTest()
        {
            var page = new LoginPage();
            Assert.IsTrue(page.Auth("test1", "test1"));
            Assert.IsFalse(page.Auth("user12", "123452"));
            Assert.IsFalse(page.Auth("", ""));
            Assert.IsFalse(page.Auth("", ""));
        }

        [TestMethod]
        public void AuthTestSuccess()
        {
            var page = new LoginPage();
            Assert.IsTrue(page.Auth("test", "test"));
            Assert.IsTrue(page.Auth("user1", "12345"));
        }


        [TestMethod]
        public void AuthTestFail()
        {
            var page = new LoginPage();
            Assert.IsFalse(page.Auth("test", ""));
            Assert.IsFalse(page.Auth("user1", "12456345"));
            Assert.IsFalse(page.Auth("", "test"));
            Assert.IsFalse(page.Auth("dfg32", "12345"));
        }
    }
}

