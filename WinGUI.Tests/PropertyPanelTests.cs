using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;
using System.Diagnostics;
using static TestGUI.TestProvider;

namespace TestGUI
{

    public class PropertyPanelTests
    {
        [Test(Description = "Последовательное развертывание корней 'Объекты', 'ГруппыОбъектов' и 'Данные'")]
        public void SelectingTreeElementsTest()
        {
            var wd = LoadProject();
            try
            {
                var actions = new Actions(wd);

                TestProvider.GetElement(wd, "Модули", SearchWay.Name).Click();
                TestProvider.GetElement(wd, "Сварка", SearchWay.Name).Click();
                TestProvider.GetElement(wd, "Объекты", SearchWay.Name).Click();

                ExpandElement(wd, actions, "Элементы2D");
                ExpandElement(wd, actions, "Элементы3D");
                ExpandElement(wd, actions, "Группы объектов");
                ExpandElement(wd, actions, "Данные");
                ExpandElement(wd, actions, "Данные");

                actions.Perform();

                TestProvider.GetElement(wd, "Элемент2D : 7384", SearchWay.Name).Click();
                TestProvider.GetElement(wd, "Элемент3D : 23258", SearchWay.Name).Click();
                TestProvider.GetElement(wd, "refLine", SearchWay.Name).Click();
                TestProvider.GetElement(wd, "Load", SearchWay.Name).Click();
                TestProvider.GetElement(wd, "Среда : Элемент2D_2 2 31 2 15 *", SearchWay.Name).Click();
                TestProvider.GetElement(wd, "Нагрев : ARC;4;100;25 Load 0 10 baseLine|refLine;10;startNodes;startNodes;0|0|0|0", SearchWay.Name).Click();
            }
            catch (Exception e) { wd.CloseApp(); Assert.Fail(e.Message); }

            finally { wd.CloseApp(); }
        }

        [Test(Description = "Изменение имени объекта - попытка присвоения некорректного имени и ввод валидных значений")]
        [TestCase("Элемент2D : 7384")]
        [TestCase("refLine")]
        public void RenameTreeElementsTest(string element)
        {
            var wd = LoadProject();
            try
            {
                var actions = new Actions(wd);

                TestProvider.GetElement(wd, "Модули", SearchWay.Name).Click();
                TestProvider.GetElement(wd, "Сварка", SearchWay.Name).Click();

                ExpandElement(wd, actions, "Элементы2D");
                ExpandElement(wd, actions, "Элементы3D");
                ExpandElement(wd, actions, "Группы объектов");
                actions.Perform();

                TestProvider.GetElement(wd, element, SearchWay.Name).Click();

                Rename(wd, "name@1 231", true);
                Rename(wd, "     name@1231", true);
                Rename(wd, "name@1231     ", true);
                Rename(wd, "new_nameElement", false);
                Rename(wd, "newNameElement232425!@#$%^&*()_+=", false);
            }
            catch (Exception e) { wd.CloseApp(); Assert.Fail(e.Message); }

            finally { wd.CloseApp(); }
        }

        [Test(Description = "Изменение цвета и последовательное изменение режима отображения объекта")]
        [TestCase("Элемент2D : 7384")]
        [TestCase("Элемент3D : 23258")]
        public void SelectColorAndTypeModeTest(string element)
        {
            var wd = LoadProject();
            try
            {
                var actions = new Actions(wd);

                TestProvider.GetElement(wd, "Модули", SearchWay.Name).Click();
                TestProvider.GetElement(wd, "Сварка", SearchWay.Name).Click();

                //Собираю в actions действия которые нужно выполнить 
                ExpandElement(wd, actions, "Элементы2D");
                ExpandElement(wd, actions, "Элементы3D");
                actions.Perform();

                //Ячейка имя 
                TestProvider.GetElement(wd, element, SearchWay.Name).Click();

                ChangeColor(wd, -10, 100);
                ChangeColor(wd, -125, 100);

                ChangeViewMode(wd, 1);
                ChangeViewMode(wd, 2);
                ChangeViewMode(wd, 3);
            }
            catch (Exception e) { wd.CloseApp(); Assert.Fail(e.Message); }

            finally { wd.CloseApp(); }
        }

        [Test(Description = "Изменение группы у элементов ValuableData")]
        public void ChangeDataGroupTest()
        {
            var wd = LoadProject();
            try
            {
                TestProvider.GetElement(wd, "Модули", SearchWay.Name).Click();
                TestProvider.GetElement(wd, "Сварка", SearchWay.Name).Click();
                TestProvider.GetElement(wd, "Данные", SearchWay.Name).Click();

                EnumerateTree(wd, ChangeGroup);

                Thread.Sleep(1000);
            }
            catch (Exception e) { wd.CloseApp(); Assert.Fail(e.Message); }

            finally { wd.CloseApp(); }
        }

        [Test(Description = "Изменение времени начала и конца")]
        [TestCase("0!150", true)]
        [TestCase("200!150", true)]
        [TestCase("d23!#50", false)]
        public void ChangeStartAndEndTimeTest(string value, bool isError)
        {
            var wd = LoadProject();
            try
            {
                TestProvider.GetElement(wd, "Модули", SearchWay.Name).Click();
                TestProvider.GetElement(wd, "Сварка", SearchWay.Name).Click();
                TestProvider.GetElement(wd, "Данные", SearchWay.Name).Click();

                EnumerateTree(wd, ChangeTime, value, isError);

                Thread.Sleep(1000);
            }
            catch (Exception e) { wd.CloseApp(); Assert.Fail(e.Message); }

            finally { wd.CloseApp(); }
        }

        [Test(Description = "Тестирование изменения параметров источника")]
        [TestCase("0", true)]
        [TestCase("10", true)]
        [TestCase("12.5", true)]
        [TestCase("15. 4", true)]
        [TestCase("-15.4", false)]
        [TestCase("q12", false)]
        public void SetSourceValueTest(string data, bool isError)
        {
            var wd = LoadProject();
            try
            {
                TestProvider.GetElement(wd, "Модули", SearchWay.Name).Click();
                TestProvider.GetElement(wd, "Сварка", SearchWay.Name).Click();
                TestProvider.GetElement(wd, "Данные", SearchWay.Name).Click();
                var value = "Нагрев!" + data;
                EnumerateTree(wd, ChangeSource, value, isError);
            }
            catch (Exception e) { wd.CloseApp(); Assert.Fail(e.Message); }
            finally { wd.CloseApp(); }
        }

        [Test(Description = "Тестирование изменения параметров MediaData")]
        public void MediaDataChange()
        {
            var wd = LoadProject();
            try
            {
                TestProvider.GetElement(wd, "Модули", SearchWay.Name).Click();
                TestProvider.GetElement(wd, "Сварка", SearchWay.Name).Click();
                TestProvider.GetElement(wd, "Данные", SearchWay.Name).Click();
                var value = "Среда!";
                EnumerateTree(wd, ChangeSource, value);
            }
            catch (Exception e) { wd.CloseApp(); Assert.Fail(e.Message); }
            finally { wd.CloseApp(); }
        }
        private static WindowsDriver<WindowsElement> LoadProject()
        {
            var opt = new AppiumOptions();
            opt.AddAdditionalCapability("app", Path.GetFullPath(@".\..\..\..\..\GUI\bin\x64\Debug\BazisGUI.exe"));
            opt.AddAdditionalCapability("ms:waitForAppLaunch", "5");
            opt.AddAdditionalCapability("appArguments", $"-proj {Path.GetFullPath(@".\..\..\..\..\GUI\Projects\Welding\Arc\proj.bpf")}");
            opt.PlatformName = "Windows11x64";
            var url = new Uri("http://127.0.0.1:4723");
            var wd = new WindowsDriver<WindowsElement>(url, opt);

            return wd;
        }

        private static void Rename(WindowsDriver<WindowsElement> wd, string newName, bool isError)
        {
            WindowsElement exept = null;
            ClickCell(wd, " Строка 0, Не отсортировано.");
            wd.Keyboard.SendKeys(newName + OpenQA.Selenium.Keys.Enter);
            try
            {
                exept = wd.FindElement(By.Name("FormatException"));
                Thread.Sleep(1000);
            }
            catch (OpenQA.Selenium.WebDriverException ex) { }

            if (isError)
            {
                Assert.That(!(exept is null));
                wd.Keyboard.SendKeys(OpenQA.Selenium.Keys.Enter);
            }
            else
            {
                if (exept is not null)
                {
                    wd.Keyboard.SendKeys(newName + OpenQA.Selenium.Keys.Enter);
                    throw new Exception("FormatException: ");
                }
            }
        }

        private static void EnumerateTree(
            WindowsDriver<WindowsElement> wd,
            Action<WindowsDriver<WindowsElement>, string, bool> action,
            string value = "",
            bool isError = false)
        {
            while (true)
            {
                var previous = wd.SwitchTo().ActiveElement();
                previous.SendKeys(OpenQA.Selenium.Keys.ArrowDown);
                var current = wd.SwitchTo().ActiveElement();

                if (previous.Equals(current)) break;

                var namePrevious = previous.Text;
                var nameCurrent = current.Text;

                action(wd, nameCurrent + "!" + value, isError);

                var elements = wd.FindElements(By.Name(namePrevious));
                if (elements.Count > 1)
                {
                    var elem = elements[^1].Text;
                    elements[^1].Click();
                }
                else TestProvider.GetElement(wd, namePrevious, SearchWay.Name).Click();

                wd.Keyboard.SendKeys(OpenQA.Selenium.Keys.ArrowDown);
            }
        }

        private static void ChangeSource(WindowsDriver<WindowsElement> wd, string value, bool isError)
        {
            var data = value.Split('!');

            if (data[0].Contains("Нагрев") && data[1].Contains("Нагрев"))
            {
                var length = (data[0].Contains("Нагрев : ARC") || data[0].Contains("Нагрев : FSWShoulder")) ? 3 : 4;
                for (int i = 0; i < length; i++)
                {
                    TestProvider.GetElement(wd, $" Строка {i}, Не отсортировано.", SearchWay.Name).Click();
                    TestProvider.ClickByOffset(wd, 265, 0, ClickType.LeftOne);
                    TestProvider.ClickByOffset(wd, 0, 0, ClickType.LeftOne);
                    wd.Keyboard.SendKeys(data[1] + OpenQA.Selenium.Keys.Enter);
                    CheckError(wd, isError);
                }
            }

            else if (data[0].Contains("Среда") && data[1].Contains("Среда"))
            {
                var elementActive = wd.SwitchTo().ActiveElement();
                var nameActive = elementActive.Text;
                var numberLines = 1;
                TestProvider.GetElement(wd, $" Строка 0, Не отсортировано.", SearchWay.Name).Click();
                for (; ; )
                {
                    var previous = wd.SwitchTo().ActiveElement();
                    var namePrevious = previous.Text;
                    previous.SendKeys(OpenQA.Selenium.Keys.ArrowDown);
                    var current = wd.SwitchTo().ActiveElement();
                    var nameCurrent = current.Text;
                    if (previous.Text.Equals(current.Text)) break;
                    numberLines++;
                }

                var excludeLastCount = 2;
                for (int i = 0; i < numberLines - excludeLastCount; i++)
                {
                    TestProvider.GetElement(wd, $" Строка {i}, Не отсортировано.", SearchWay.Name).Click();
                    if(i == 0)
                    {
                        TestProvider.ClickByOffset(wd, 265, 0, ClickType.LeftOne);
                        TestProvider.ClickByOffset(wd, 0, 0, ClickType.LeftOne);
                        wd.Keyboard.SendKeys(OpenQA.Selenium.Keys.ArrowDown);
                        wd.Keyboard.SendKeys(OpenQA.Selenium.Keys.Enter);
                        wd.Keyboard.SendKeys(OpenQA.Selenium.Keys.Tab);
                    }
                    else
                    {
                        TestProvider.ClickByOffset(wd, 240, 0, ClickType.LeftOne);
                        TestProvider.ClickByOffset(wd, 0, 0, ClickType.LeftOne);
                        var testString = "32";
                        wd.Keyboard.SendKeys(testString + OpenQA.Selenium.Keys.Enter);

                        TestProvider.ClickByOffset(wd, 25, 0, ClickType.LeftOne);
                        TestProvider.ClickByOffset(wd, 0, 0, ClickType.LeftOne);
                        wd.Keyboard.SendKeys(OpenQA.Selenium.Keys.ArrowDown);
                        wd.Keyboard.SendKeys(OpenQA.Selenium.Keys.Enter);
                        wd.Keyboard.SendKeys(OpenQA.Selenium.Keys.Tab);
                    }
                }
            }
        }

        private static void ChangeTime(WindowsDriver<WindowsElement> wd, string value, bool isError = false)
        {
            var data = value.Split('!');
            var search = string.Empty;
            if (data[0].Contains("Закрепление")) search = " Строка 4, Не отсортировано.";
            else if (data[0].Contains("Среда"))
            {
                if (data[0].Contains("Среда : air")) search = " Строка 4, Не отсортировано.";
                else search = " Строка 3, Не отсортировано.";
            }
            else if (data[0].Contains("Нагрев"))
            {
                if (data[0].Contains("Нагрев : ARC")) search = " Строка 4, Не отсортировано.";
                else if (data[0].Contains("Нагрев : FSWShoulder")) search = " Строка 5, Не отсортировано.";
                else if (data[0].Contains("Нагрев : FSWPin")) search = " Строка 6, Не отсортировано.";
                else search = " Строка 5, Не отсортировано.";
            }
            else if (data[0].Contains("Нагрузка")) search = " Строка 6, Не отсортировано.";
            else search = " Строка 3, Не отсортировано.";
            TestProvider.GetElement(wd, search, SearchWay.Name).Click();
            TestProvider.ClickByOffset(wd, 265, 0, ClickType.LeftOne);
            TestProvider.ClickByOffset(wd, 0, 0, ClickType.LeftOne);
            wd.Keyboard.SendKeys(data[1] + OpenQA.Selenium.Keys.Enter);
            CheckError(wd, isError);
            wd.Keyboard.SendKeys(OpenQA.Selenium.Keys.ArrowDown);
            wd.Keyboard.SendKeys(OpenQA.Selenium.Keys.Enter);
            if (!data[0].Contains("Нагрев"))
            {
                wd.Keyboard.SendKeys(data[2] + OpenQA.Selenium.Keys.Enter);
                CheckError(wd, isError);
                wd.Keyboard.SendKeys(OpenQA.Selenium.Keys.Tab);
            }
        }

        private static void ChangeGroup(WindowsDriver<WindowsElement> wd, string nameCurrent, bool isError = false)
        {
            if (nameCurrent.Contains("Нагрев")) TestProvider.GetElement(wd, " Строка 0, Не отсортировано.", SearchWay.Name).Click();
            else TestProvider.GetElement(wd, " Строка 1, Не отсортировано.", SearchWay.Name).Click();
            TestProvider.ClickByOffset(wd, 265, 0, ClickType.LeftOne);
            TestProvider.ClickByOffset(wd, 0, 0, ClickType.LeftOne);
            wd.Keyboard.SendKeys(OpenQA.Selenium.Keys.ArrowDown);
            wd.Keyboard.SendKeys(OpenQA.Selenium.Keys.Enter);
            wd.Keyboard.SendKeys(OpenQA.Selenium.Keys.Tab);
        }
        private static void ChangeViewMode(WindowsDriver<WindowsElement> wd, int indexViewMode)
        {
            TestProvider.GetElement(wd, " Строка 2, Не отсортировано.", SearchWay.Name).Click();
            TestProvider.ClickByOffset(wd, 275, 0, ClickType.LeftOne);
            TestProvider.ClickByOffset(wd, 0, 0, ClickType.LeftOne);
            var selectedViewMode = new Actions(wd);
            for (int i = indexViewMode - 1; i < indexViewMode; i++)
            {
                selectedViewMode.SendKeys(OpenQA.Selenium.Keys.ArrowDown);
            }
            selectedViewMode.SendKeys(OpenQA.Selenium.Keys.Enter).Perform();
            TestProvider.ClickByOffset(wd, 100, 0, ClickType.LeftOne);
            var update = new Actions(wd);
            update.SendKeys(OpenQA.Selenium.Keys.Escape).Perform();
        }

        private static void ChangeColor(WindowsDriver<WindowsElement> wd, int offsetX, int offsetY)
        {
            ClickCell(wd, " Строка 1, Не отсортировано.");
            TestProvider.GetElement(wd, "Основные цвета:", SearchWay.Name).Click();
            TestProvider.ClickByOffset(wd, offsetX, offsetY, ClickType.LeftOne);
            TestProvider.GetElement(wd, "ОК", SearchWay.Name).Click();
            var update = new Actions(wd);
            update.SendKeys(OpenQA.Selenium.Keys.Escape).Perform();
        }

        private static void ClickCell(WindowsDriver<WindowsElement> wd, string rowName)
        {
            TestProvider.GetElement(wd, rowName, SearchWay.Name).Click();
            TestProvider.ClickByOffset(wd, 130, 0, ClickType.LeftOne);
            TestProvider.ClickByOffset(wd, 0, 0, ClickType.LeftOne);
        }

        private static void CheckError(WindowsDriver<WindowsElement> wd, bool isError)
        {
            WindowsElement exept = null;
            if (!isError)
            {
                try
                {
                    exept = wd.FindElement(By.Name("FormatException"));
                    Thread.Sleep(1000);
                }
                catch (OpenQA.Selenium.WebDriverException ex) { }

                Assert.That(!(exept is null));
                wd.Keyboard.SendKeys(OpenQA.Selenium.Keys.Enter);
            }
            else
            {
                if (exept is not null)
                {
                    throw new Exception("FormatException: ");
                }
            }
        }
        private static void ExpandElement(WindowsDriver<WindowsElement> wd, Actions actions, string nameInTree)
        {
            var element = TestProvider.GetElement(wd, nameInTree, SearchWay.Name);
            actions.DoubleClick(element);
        }
    }
}