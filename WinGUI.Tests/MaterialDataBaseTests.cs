using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static TestGUI.TestProvider;

namespace TestGUI
{
    public class MaterialDataBaseTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test(Description = "Построение металлургических диаграмм")]
        [TestCase("CCT", TestName = "CCT диаграмма")]
        [TestCase("TTT", TestName = "TTT диаграмма")]
        [TestCase("Hardening", TestName = "Диаграмма упрочнения")]
        public void DiagramOperationTests(string key)
        {
            var wd = CreateWinDriver($"--mat {Path.GetFullPath(@".\..\..\..\Materials\Materials_v6.jsf")}");

            try
            {                
                GetElement(wd, "Сталь_20ХМ_Св", SearchWay.Name).Click();
                ClickByOffset(wd, 0, 0, ClickType.LeftDouble);

                if (key == "CCT" || key == "TTT")
                    OpenMetallurgy(wd, key);            
                else if (key == "Hardening")
                    CreateHardeningControl(wd);

                Thread.Sleep(3000);
            }
            catch (Exception e) { wd.CloseApp(); Assert.Fail(e.Message); }
            finally { wd.Quit(); }                      
        }

        private void OpenMetallurgy(WindowsDriver<WindowsElement> wd, string key)
        {
            GetElement(wd, "Металлургия", SearchWay.Name).Click();
            ClickByOffset(wd, 0, 0, ClickType.RightOne);
            GetElement(wd, "Рассчитать диаграмму", SearchWay.Name).Click();            

            if (key == "CCT")
                CreateCCTDiagramm(wd);
            else if (key == "TTT")
                CreateTTTDiagramm(wd);
        }
        private void CreateCCTDiagramm(WindowsDriver<WindowsElement> wd)
        {
            GetElement(wd, "InitialPhase", SearchWay.Name).Click();
            ClickByOffset(wd, 30, 5, ClickType.LeftOne);
            ClickByOffset(wd, -10, 20, ClickType.LeftOne);          
            GetElement(wd, "Рассчитать", SearchWay.Name).Click();
        }

        private void CreateTTTDiagramm(WindowsDriver<WindowsElement> wd)
        {
            GetElement(wd, "TTT", SearchWay.Name).Click();
            GetElement(wd, "InitialPhase", SearchWay.Name).Click();

            ClickByOffset(wd, 30, 5, ClickType.LeftOne);
            ClickByOffset(wd, -10, 20, ClickType.LeftOne);

            SendKey(wd, "Время", "0000");            
            GetElement(wd, "Рассчитать", SearchWay.Name).Click();           
        } 
        
        private void CreateHardeningControl(WindowsDriver<WindowsElement> wd)
        {
            GetElement(wd, "Механические свойства", SearchWay.Name).Click();
            ClickByOffset(wd, 0, 0, ClickType.RightOne);            

            GetElement(wd, "Рассчитать упрочнение", SearchWay.Name).Click();
            GetElement(wd, "Phases", SearchWay.Name).Click();

            ClickByOffset(wd, 30, 5, ClickType.LeftOne);
            ClickByOffset(wd, -10, 70, ClickType.LeftOne);           

            GetElement(wd, "Рассчитать", SearchWay.Name).Click();
            GetElement(wd, "Указать температуру", SearchWay.Name).Click();
            SendKey(wd, "Temp", "200");            
            GetElement(wd, "Рассчитать", SearchWay.Name).Click();            
        }        

        [TestCase(TestName = "Добавление и копирование нового материала")]
        public void AddNewMaterialTests()
        {
            var wd = CreateWinDriver("");

            try
            {
                GetElement(wd, "Добавить раздел", SearchWay.Name).Click();
                GetElement(wd, "Новый_материал_0", SearchWay.Name).Click();
                ClickByOffset(wd, 0, 0, ClickType.LeftDouble);

                GetElement(wd, "Общие сведения", SearchWay.Name).Click();
                ClickByOffset(wd, 0, 0, ClickType.LeftDouble);

                GetElement(wd, "Структура,Фаза-Масс.доли", SearchWay.Name).Click();
                GetElement(wd, "Добавить ряд", SearchWay.Name).Click();

                SendKey(wd, "Фаза Строка 0", "Аустенит");
                SendKey(wd, "Фаза Строка 1", "Мартенсит");

                GetElement(wd, "Тепловые свойства", SearchWay.Name).Click();
                ClickByOffset(wd, 0, 0, ClickType.LeftDouble);

                GetElement(wd, "Теплоемкость,Дж-C°", SearchWay.Name).Click();

                SendKey(wd, "Температура Строка 0", "100");
                SendKey(wd, "Аустенит Строка 0", "1");
                SendKey(wd, "Мартенсит Строка 0", "2");

                GetElement(wd, "Новый_материал_0", SearchWay.Name).Click();
                GetElement(wd, "Создать копию", SearchWay.Name).Click();
                GetElement(wd, "Новый_материал_0_копия", SearchWay.Name).Click();

                Thread.Sleep(3000);
            }
            catch (Exception e) { wd.CloseApp(); Assert.Fail(e.Message); }
            finally { wd.Quit(); }
        }

        [TestCase(TestName = "Добавление и копирование новой функции")]
        public void AddNewFunctionTests()
        {
            var wd = CreateWinDriver("");

            try
            {
                GetElement(wd, "Функции", SearchWay.Name).Click();
                GetElement(wd, "Добавить раздел", SearchWay.Name).Click();
                GetElement(wd, "Новая_функция_0,\" \"-\" \"", SearchWay.Name).Click();

                SendKey(wd, "X Строка 0", "100");
                SendKey(wd, "Y Строка 0", "200");

                GetElement(wd, "Добавить ряд", SearchWay.Name).Click();

                SendKey(wd, "X Строка 1", "300");
                SendKey(wd, "Y Строка 1", "400");

                GetElement(wd, "Создать копию", SearchWay.Name).Click();
                GetElement(wd, "Новая_функция_0_копия,\" \"-\" \"", SearchWay.Name).Click();

                Thread.Sleep(3000);
            }
            catch (Exception e) { wd.CloseApp(); Assert.Fail(e.Message); }
            finally { wd.Quit(); }     
        }
    }
}
