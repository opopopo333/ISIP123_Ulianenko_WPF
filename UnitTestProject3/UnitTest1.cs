using Microsoft.VisualStudio.TestTools.UnitTesting;
using PR14.Pages;
using System;

namespace UnitTestProject3
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void RegisterTest()
        {
            var page = new RegisterPage();
            string uniqueLogin = "User_" + DateTime.Now.Ticks.ToString();
            Assert.IsTrue(page.RegisterUser("Тестовый Пользователь", uniqueLogin, "Password123"));

            Assert.IsFalse(page.RegisterUser("Тестовый Пользователь", uniqueLogin, "Password123"));

            Assert.IsFalse(page.RegisterUser("", "", ""));
            Assert.IsFalse(page.RegisterUser("Имя", "", "123"));

            
        }
    }
}
