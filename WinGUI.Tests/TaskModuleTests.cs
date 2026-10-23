using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using System.Diagnostics;

namespace TestGUI
{
    public class TaskModuleTests
    {
        [SetUp]
        public void Setup()
        {
            //var myProcess = new Process();

            //myProcess.StartInfo.FileName = $@"c:\BazisGUI\GUI\bin\x64\Debug\BazisGUI.exe";

            //var argStr = string.Join(" ", new string[] { @"..\Debug\Projects", "proj.bpf" });

            //myProcess.StartInfo.Arguments = argStr;
            //myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            //myProcess.Start();
        }
        [Test(Description = "Мастер постановки технологических задач. Действия: показать, скрыть, обновить, удалить, добавить")]
        [TestCase("Сварка", "Дуговая сварка", "Материалы", "  a_m", "  d_m", "  r_m", "  s_m", "  h_m", TestName = "Дуговая сварка Материалы")]
        [TestCase("Сварка", "Дуговая сварка", "Закрепления", "  a_c", "  d_c", "  r_c", "  s_c", "  h_c", TestName = "Дуговая сварка Закрепления")]
        [TestCase("Сварка", "Дуговая сварка", "Режим сварки", "  a_h", "  d_h", "  r_h", "  s_h", "  h_h", TestName = "Дуговая сварка Режим сварки")]
        [TestCase("Сварка", "Дуговая сварка", "Среда", "  a_m", "  d_m", "  r_m", "  s_m", "  h_m", TestName = "Дуговая сварка Среда сварки")]
        //[TestCase("Сварка", "Дуговая сварка", "Планировщик", "  a_p", "  d_p", "  r_p", "  _", "  _", TestName = "Дуговая сварка Планировщик")]
        [TestCase("Сварка", "Трением с перемешиванием", "Материалы", "  a_m", "  d_m", "  r_m", "  s_m", "  h_m", TestName = "FSW Материалы")]
        [TestCase("Сварка", "Трением с перемешиванием", "Режим сварки", "  a_h", "  d_h", "  r_h", "  s_h", "  h_h", TestName = "FSW Режим сварки")]
        [TestCase("Сварка", "Трением с перемешиванием", "Среда", "  a_m", "  d_m", "  r_m", "  s_m", "  h_m", TestName = "FSW Среда сварки")]
        [TestCase("Сварка", "Лазерная сварка", "Материалы", "  a_m", "  d_m", "  r_m", "  s_m", "  h_m", TestName = "LW Материалы")]
        [TestCase("Сварка", "Лазерная сварка", "Режим сварки", "  a_h", "  d_h", "  r_h", "  s_h", "  h_h", TestName = "LW Режим сварки")]
        [TestCase("Сварка", "Лазерная сварка", "Среда", "  a_m", "  d_m", "  r_m", "  s_m", "  h_m", TestName = "LW Среда сварки")]
        [TestCase("Термообработка", "Нагрев", "Материалы", "  a_m", "  d_m", "  r_m", "  s_m", "  h_m", TestName = "Нагрев Материалы")]
        [TestCase("Термообработка", "Нагрев", "Закрепления", "  a_c", "  d_c", "  r_c", "  s_c", "  h_c", TestName = "Нагрев Закрепления")]
        [TestCase("Термообработка", "Нагрев", "Режим ТО", "  a_h", "  d_h", "  r_h", "  s_h", "  h_h", TestName = "Нагрев Режим ТО")]
        //[TestCase("Термообработка", "Нагрев", "Планировщик", "  a_p", "  d_p", "  r_p", "  _", "  _", TestName = "Нагрев Планировщик")]
        [TestCase("Термообработка", "Отпуск | Отжиг | Старение", "Материалы", "  a_m", "  d_m", "  r_m", "  s_m", "  h_m", TestName = "Отпуск | Отжиг | Старение Материалы")]
        [TestCase("Термообработка", "Отпуск | Отжиг | Старение", "Закрепления", "  a_c", "  d_c", "  r_c", "  s_c", "  h_c", TestName = "Отпуск | Отжиг | Старение Закрепления")]
        [TestCase("Термообработка", "Отпуск | Отжиг | Старение", "Режим ТО", "  a_h", "  d_h", "  r_h", "  s_h", "  h_h", TestName = "Отпуск | Отжиг | Старение Режим ТО")]
        [TestCase("Термообработка", "Отпуск | Отжиг | Старение", "Планировщик", "  a_p", "  d_p", "  r_p", "  _", "  _", TestName = "Отпуск | Отжиг | Старение Планировщик")]
        [TestCase("Термообработка", "Закалка", "Материалы", "  a_m", "  d_m", "  r_m", "  s_m", "  h_m", TestName = "Закалка Материалы")]
        [TestCase("Термообработка", "Закалка", "Закрепления", "  a_c", "  d_c", "  r_c", "  s_c", "  h_c", TestName = "Закалка Закрепления")]
        [TestCase("Термообработка", "Закалка", "Режим ТО", "  a_h", "  d_h", "  r_h", "  s_h", "  h_h", TestName = "Закалка Режим ТО")]
        //[TestCase("Термообработка", "Закалка", "Планировщик", "  a_p", "  d_p", "  r_p", "  _", "  _", TestName = "Закалка Планировщик")]
        public void TaskModuleOperationsTest(string module, string process, string category, string btnAdd, string btnDel, string btnRef, string btnShow, string btnHide)
        {
            string args;
            if (process == "Дуговая сварка")
                args = @"-proj c:\BazisGUI\GUI\Projects\Welding\Arc\proj.bpf";
            else if (process == "Трением с перемешиванием")
                args = @"-proj c:\BazisGUI\GUI\Projects\Welding\FSW\proj.bpf";
            else if (process == "Лазерная сварка")
                args = @"-proj c:\BazisGUI\GUI\Projects\Welding\Lazer\proj.bpf";
            else if (process == "Нагрев")
                args = @"-proj c:\BazisGUI\GUI\Projects\HeatTreatment\Heating\2D_axi\2D_val.bpf";
            else if (process == "Закалка")
                args = @"-proj c:\BazisGUI\GUI\Projects\HeatTreatment\Quenching\2D_axi\2D_val.bpf";
            else
                args = @"-proj c:\BazisGUI\GUI\Projects\HeatTreatment\Tempering\2D_axi\2D_val.bpf";

            WindowsDriver<WindowsElement> wd;

            var opt = new AppiumOptions();

            opt.AddAdditionalCapability("app", Path.GetFullPath(@".\..\..\..\..\GUI\bin\x64\Debug\BazisGUI.exe"));
            opt.AddAdditionalCapability("ms:waitForAppLaunch", "3");
            opt.AddAdditionalCapability("appArguments", args);
            opt.PlatformName = "Windows11x64";
            var url = new Uri("http://127.0.0.1:4723");
            wd = new WindowsDriver<WindowsElement>(url, opt);

            var moduls = wd.FindElement(By.Name("Модули"));
            moduls.Click();
            var modulW = wd.FindElement(By.Name(module));
            modulW.Click();
            var tasks = wd.FindElement(By.Name("Задачи"));
            tasks.Click();
            var taskArcW = wd.FindElement(By.Name(process));
            taskArcW.Click();
            var arcWMat = wd.FindElement(By.Name(category));
            arcWMat.Click();
            var strWMat = wd.FindElement(By.Name("Строка 0"));
            strWMat.Click();

            if (btnShow != "  _")
                wd.FindElement(By.Name(btnShow)).Click();
            if (btnHide != "  _")
                wd.FindElement(By.Name(btnHide)).Click();

            var refBtn = wd.FindElement(By.Name(btnRef));
            refBtn.Click();
            var delBtn = wd.FindElement(By.Name(btnDel));
            delBtn.Click();
            var addBtn = wd.FindElement(By.Name(btnAdd));
            addBtn.Click();

            Thread.Sleep(3000);

            //возврат лицензии на модуль сварка
            SwithModule(wd, moduls, "Построение сетки");

            Thread.Sleep(3000);
            wd.CloseApp();
        }       

        

            public static void SwithModule(WindowsDriver<WindowsElement> wd, WindowsElement moduls, string moduleName)
        {
            //возврат лицензии на модуль сварка
            moduls.Click();
            var modulM = wd.FindElement(By.Name(moduleName));
            modulM.Click();
        }
    }
}