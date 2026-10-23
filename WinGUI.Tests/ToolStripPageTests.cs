using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Interactions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.Formats.Tar;

namespace TestGUI
{
    public class ToolStripPageTests
    {
        [Test(Description = "Тест продвинутого выбора. Действия: показать, переключение радиокнопок")]
        public void AdvanceModuleTest()
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
            var modulW = wd.FindElement(By.Name("Построение сетки"));
            modulW.Click();
            var advSelection = wd.FindElement(By.Name("toolStripButton1"));
            advSelection.Click();

            wd.FindElement(By.Name("Элементы")).Click();
            wd.FindElement(By.Name("Узлы")).Click();
            wd.FindElement(By.Name("По направлению")).Click();
            
            wd.FindElement(By.Name("Выбрать")).Click();

            Thread.Sleep(1000);

            //возврат фокуса на главную форму
            advSelection.Click();

            Thread.Sleep(1000);

            var a = new Actions(wd);
            a.SendKeys(Keys.Escape).Perform();

            Thread.Sleep(1000);

            //возврат лицензии на модуль сварка
            TaskModuleTests.SwithModule(wd, moduls, "Построение сетки");

            Thread.Sleep(3000);
            wd.CloseApp();
        }


        [Test(Description = "Тест модуля измерений. Действия: измерить, переключение радиокнопок")]
        public void MeasureModuleTest()
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
            var modulW = wd.FindElement(By.Name("Построение сетки"));
            modulW.Click();
            var measureForm = wd.FindElement(By.Name("toolStripButton14"));
            measureForm.Click();


            var measurebtn = wd.FindElement(By.Name("Измерить"));

            wd.FindElement(By.Name("Расстояние, мм")).Click();
            measurebtn.Click();
            wd.FindElement(By.Name("Объем, мм^3")).Click();
            measurebtn.Click();
            wd.FindElement(By.Name("Площадь, мм^2")).Click();
            measurebtn.Click();
            wd.FindElement(By.Name("Путь, мм")).Click();
            measurebtn.Click();

            Thread.Sleep(1000);

            //возврат фокуса на главную форму
            measureForm.Click();

            Thread.Sleep(1000);

            var a = new Actions(wd);
            a.SendKeys(Keys.Escape).Perform();

            Thread.Sleep(1000);

            //возврат лицензии на модуль сварка
            TaskModuleTests.SwithModule(wd, moduls, "Термообработка");

            Thread.Sleep(3000);
            wd.CloseApp();
        }

        [Test(Description = "Тест модуля сечений. Действия: построить, переключение радиокнопок")]
        public void CrossSectionModuleTest()
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
            var modulW = wd.FindElement(By.Name("Построение сетки"));
            modulW.Click();
            var crossForm = wd.FindElement(By.Name("toolStripButton15"));
            crossForm.Click();


            var createbtn = wd.FindElement(By.Name("Построить"));
            var deletebtn = wd.FindElement(By.Name("Удалить"));

            wd.FindElement(By.Name("XY")).Click();
            createbtn.Click();
            wd.FindElement(By.Name("XZ")).Click();
            createbtn.Click();
            wd.FindElement(By.Name("YZ")).Click();
            createbtn.Click();

            deletebtn.Click();

            Thread.Sleep(1000);

            //возврат фокуса на главную форму
            crossForm.Click();

            Thread.Sleep(1000);

            var a = new Actions(wd);
            a.SendKeys(Keys.Escape).Perform();

            Thread.Sleep(1000);

            //возврат лицензии на модуль сварка
            TaskModuleTests.SwithModule(wd, moduls, "Построение сетки");

            Thread.Sleep(3000);
            wd.CloseApp();
        }

        [Test(Description = "Тест модуля скрыть сечением. Действия: переключение радиокнопок")]
        public void HideSectionModuleTest()
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
            var modulW = wd.FindElement(By.Name("Построение сетки"));
            modulW.Click();
            var crossForm = wd.FindElement(By.Name("btnClipPlane"));
            crossForm.Click();

            wd.FindElement(By.Name("YZ")).Click();
            wd.FindElement(By.Name("ZY")).Click();
            wd.FindElement(By.Name("ZX")).Click();
            wd.FindElement(By.Name("XZ")).Click();
            wd.FindElement(By.Name("XY")).Click();
            wd.FindElement(By.Name("YX")).Click();

            wd.FindElement(By.Name("Сброс")).Click();

            Thread.Sleep(1000);

            //возврат фокуса на главную форму
            crossForm.Click();

            Thread.Sleep(1000);

            //возврат лицензии на модуль сварка
            TaskModuleTests.SwithModule(wd, moduls, "Сварка");

            Thread.Sleep(3000);
            wd.CloseApp();
        }

        [Test(Description = "Тест модуля отражения. Действия: переключение радиокнопок")]
        public void ReflectModuleTest()
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
            var modulW = wd.FindElement(By.Name("Построение сетки"));
            modulW.Click();
            var crossForm = wd.FindElement(By.Name("btnReflect"));
            crossForm.Click();
            wd.FindElement(By.Name("YZ")).Click();
            wd.FindElement(By.Name("Задать")).Click();
            wd.FindElement(By.Name("ZX")).Click();
            wd.FindElement(By.Name("Задать")).Click();
            wd.FindElement(By.Name("XY")).Click();
            wd.FindElement(By.Name("Задать")).Click();

            wd.FindElement(By.Name("Сброс")).Click();

            Thread.Sleep(1000);

            //возврат фокуса на главную форму
            crossForm.Click();

            Thread.Sleep(1000);

            //возврат лицензии на модуль сварка
            TaskModuleTests.SwithModule(wd, moduls, "Сварка");

            Thread.Sleep(3000);
            wd.CloseApp();
        }


        [Test(Description = "Тест модуля сечений. Действия: построить, переключение радиокнопок")]
        public void SwitchPlaneAndAxisTest()
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
            var modulW = wd.FindElement(By.Name("Построение сетки"));
            modulW.Click();

            wd.FindElement(By.Name("toolStripButton5")).Click();
            wd.FindElement(By.Name("toolStripButton6")).Click();
            wd.FindElement(By.Name("toolStripButton7")).Click();
            wd.FindElement(By.Name("toolStripButton8")).Click(); 
            wd.FindElement(By.Name("toolStripButton9")).Click();
            wd.FindElement(By.Name("toolStripButton10")).Click();
            wd.FindElement(By.Name("toolStripButton11")).Click();
            wd.FindElement(By.Name("toolStripButton12")).Click();
            wd.FindElement(By.Name("toolStripButton13")).Click();

            Thread.Sleep(1000);

            var a = new Actions(wd);
            a.SendKeys(Keys.Escape).Perform();

            Thread.Sleep(1000);

            //возврат лицензии на модуль сварка
            TaskModuleTests.SwithModule(wd, moduls, "Сварка");

            Thread.Sleep(3000);
            wd.CloseApp();
        }

        [Test(Description = "Тест контрола настроек")]
        [TestCase(@"c:\BazisComponents\WeldingCADMerge\model7v3.stp","Сварка", TestName = "Открыть около сцены")]
        [TestCase(@"c:\BazisComponents\WeldingCADMerge\model7v3.stp", "", TestName = "Открыть в верхнем левом углу")]
        public void GeneralSettingsTest(string filePath, string module)
        {
            WindowsDriver<WindowsElement> wd;

            var opt = new AppiumOptions();

            opt.AddAdditionalCapability("app", @"c:\BazisGUI\GUI\bin\x64\Debug\BazisGUI.exe");
            opt.AddAdditionalCapability("ms:waitForAppLaunch", "3");
            var args = string.Join(" ", new string[] {
                "-cad", filePath });

            opt.AddAdditionalCapability("appArguments", args);
            opt.PlatformName = "Windows11x64";
            var url = new Uri("http://127.0.0.1:4723");

            wd = new WindowsDriver<WindowsElement>(url, opt);

            var moduls = wd.FindElement(By.Name("Модули"));
            moduls.Click();

            if (module != "")
                wd.FindElement(By.Name(module)).Click();

            var settings = wd.FindElement(By.Name("Настройки"));
            settings.Click();
            var objects = wd.FindElement(By.Name("Объекты"));
            objects.Click();
            var modulR = wd.FindElement(By.Name("Решатель"));
            modulR.Click();

            Thread.Sleep(3000);
            wd.CloseApp();
        }
    }
}
