using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TestGUI.TestProvider;

namespace TestGUI
{
    public class TestWeldingModule
    {
        [Test(Description = "Thermal")]
        public void TestThermalTask()
        {
            var opt = new AppiumOptions();
            opt.AddAdditionalCapability("app", Path.GetFullPath(@".\..\..\..\..\GUI\bin\x64\Debug\BazisGUI.exe"));
            opt.AddAdditionalCapability("ms:waitForAppLaunch", "5");
            opt.AddAdditionalCapability("appArguments", $"-proj {Path.GetFullPath(@".\..\..\..\..\GUI\Projects\Welding\Arc\proj.bpf")}");
            opt.PlatformName = "Windows11x64";

            var url = new Uri("http://127.0.0.1:4723");

            var wd = new WindowsDriver<WindowsElement>(url, opt);

            try
            {
                TestProvider.GetElement(wd, "Модули", SearchWay.Name).Click();
                TestProvider.GetElement(wd, "Сварка", SearchWay.Name).Click();
                TestProvider.GetElement(wd, "Задачи", SearchWay.Name).Click();
                TestProvider.GetElement(wd, "Дуговая сварка", SearchWay.Name).Click();

                FillingData(wd, "Материалы", "  d_m", "  a_m", true);
                FillingData(wd, "Среда", "  d_m", "  a_m", false);
                FillingData(wd, "Режим сварки", "  d_h", "  a_h", false);

                TestProvider.GetElement(wd, "Планировщик", SearchWay.Name).Click();

                TestProvider.GetElement(wd, "Термическая", SearchWay.Name).Click();
                TestProvider.GetElement(wd, "Создать *.tsf", SearchWay.Name).Click();
                TestProvider.ClickByOffset(wd, 100, 0, ClickType.LeftDouble);

                ValidateAndDeleeteData();
                Thread.Sleep(5000);
            }

            catch (Exception e) { wd.CloseApp(); Assert.Fail(e.Message); }

            finally { wd.CloseApp(); }
        }

        [Test(Description = "Mechanical")]
        public void TestMechanicalTask()
        {
            var opt = new AppiumOptions();
            opt.AddAdditionalCapability("app", Path.GetFullPath(@".\..\..\..\..\GUI\bin\x64\Debug\BazisGUI.exe"));
            opt.AddAdditionalCapability("ms:waitForAppLaunch", "5");
            opt.AddAdditionalCapability("appArguments", $"-proj {Path.GetFullPath(@".\..\..\..\..\GUI\Projects\Welding\Arc\proj.bpf")}");
            opt.PlatformName = "Windows11x64";

            var url = new Uri("http://127.0.0.1:4723");

            var wd = new WindowsDriver<WindowsElement>(url, opt);

            try
            {
                TestProvider.GetElement(wd, "Модули", SearchWay.Name).Click();
                TestProvider.GetElement(wd, "Сварка", SearchWay.Name).Click();
                TestProvider.GetElement(wd, "Задачи", SearchWay.Name).Click();
                TestProvider.GetElement(wd, "Дуговая сварка", SearchWay.Name).Click();

                FillingData(wd, "Материалы", "  d_m", "  a_m", true);
                FillingData(wd, "Закрепления", "  d_c", "  a_c", false);

                TestProvider.GetElement(wd, "Планировщик", SearchWay.Name).Click();

                TestProvider.GetElement(wd, "Механическая", SearchWay.Name).Click();
                TestProvider.GetElement(wd, "Создать *.tsf", SearchWay.Name).Click();
                TestProvider.ClickByOffset(wd, 100, 0, ClickType.LeftDouble);

                ValidateAndDeleeteData();
                Thread.Sleep(1000);
            }
            catch (Exception e) { wd.CloseApp(); Assert.Fail(e.Message); }

            finally { wd.CloseApp(); }
        }
        private void FillingData(WindowsDriver<WindowsElement> wd, string tab, string nameDeleteBut, string nameCreateBut, bool restartData)
        {
            TestProvider.GetElement(wd, tab, SearchWay.Name).Click();
            TestProvider.GetElement(wd, "Строка 0", SearchWay.Name).Click();
            TestProvider.ClickByOffset(wd, -310, 0, ClickType.LeftDouble);
            if (restartData == true)
            {
                TestProvider.GetElement(wd, nameDeleteBut, SearchWay.Name).Click();
                TestProvider.GetElement(wd, nameCreateBut, SearchWay.Name).Click();
            }
        }

        private void ValidateAndDeleeteData()
        {

            Assert.That(File.Exists(Path.GetFullPath(@".\..\..\..\..\GUI\Projects\Welding\Arc\ComputationData\computation.tcf")));
            string pathComputationData = Path.GetFullPath(@".\..\..\..\..\GUI\Projects\Welding\Arc\ComputationData");
            string pathInputData = Path.GetFullPath(@".\..\..\..\..\GUI\Projects\Welding\Arc\InputData");
            if (Directory.Exists(pathComputationData))
            {
                Directory.Delete(pathComputationData, true);
            }

            if (Directory.Exists(pathInputData))
            {
                Directory.Delete(pathInputData, true);
            }
        }
    }
}
