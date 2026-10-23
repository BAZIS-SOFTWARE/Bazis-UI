using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace TestGUI
{
    public class RemoteAccesTests
    {

        [Test(Description = "Тест удаленного лицензирования. Выполнять от Админа!!!")]
        [TestCase("192.168.11.205:8001")]
        public void AdvanceModuleTest(string ip)
        {
            SetRemoteServerIp(ip);

            string args = @"-proj c:\BazisGUI\GUI\Projects\Welding\Arc\proj.bpf";

            WindowsDriver<WindowsElement> wd;

            var opt = new AppiumOptions();

            opt.AddAdditionalCapability("app", @"c:\BazisGUI\GUI\bin\x64\Debug\BazisGUI.exe");
            opt.AddAdditionalCapability("ms:waitForAppLaunch", "3");
            opt.AddAdditionalCapability("appArguments", args);
            opt.PlatformName = "Windows11x64";
            var url = new Uri("http://127.0.0.1:4723");
            wd = new WindowsDriver<WindowsElement>(url, opt);

            var moduls = wd.FindElement(By.Name("Модули"));
            moduls.Click();
            wd.FindElement(By.Name("Построение сетки")).Click();
            Thread.Sleep(1000);
            moduls.Click();
            wd.FindElement(By.Name("Сварка")).Click();
            Thread.Sleep(1000);          

            //возврат лицензии на модуль сварка
            TaskModuleTests.SwithModule(wd, moduls, "Построение сетки");

            Thread.Sleep(3000);

            wd.CloseApp();

            SetRemoteServerIp("127.0.0.1:8000");
        }

        private static void SetRemoteServerIp(string server_ip)
        {
            Environment.SetEnvironmentVariable("BazisServerPath", server_ip, EnvironmentVariableTarget.Machine);

            var value = Environment.GetEnvironmentVariable("BazisServerPath", EnvironmentVariableTarget.Machine);

            if (value == null)
                throw new Exception("Ошибка создания переменной");
        }
    }
}
