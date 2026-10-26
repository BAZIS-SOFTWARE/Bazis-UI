using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Appium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium;

namespace TestGUI
{
    public class SceneOperationsTests
    {
        [Test(Description = "Действия: выбрать узлы, задать окно выбора, выбрать, создать группу")]
        public void CreateNewGroup()
        {
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

            moduls.Click();
            wd.FindElement(By.Name("Сварка")).Click();

            var a = new Actions(wd);
            var select = wd.FindElement(By.Name("Выбор"));
            a.MoveToElement(select).MoveByOffset(15, 0).Click().Build().Perform();

            var b = new Actions(wd);
            b.MoveByOffset(450, 200).ClickAndHold().MoveByOffset(600, -50).Click().Build().Perform();

            var с = new Actions(wd);
            с.ContextClick().MoveByOffset(0, 10).Click().Build().Perform();

            Thread.Sleep(3000);
            TaskModuleTests.SwithModule(wd, moduls, "Построение сетки");

            Thread.Sleep(3000);
            wd.CloseApp();
        }
    }
}
