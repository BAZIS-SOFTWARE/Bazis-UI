using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;
using System;
using static TestGUI.TestProvider;


namespace TestGUI
{
    public class TreeViewTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase(TestName = "Добавление реакции")]

        public void TreeViewOperationTests()
        {
            var wd = CreateWinDriver($"--mat {Path.GetFullPath(@".\..\..\..\Materials\Materials_v6.jsf")}");

            try
            {                
                GetElement(wd, "Сталь_20ХМ_Св", SearchWay.Name).Click();
                ClickByOffset(wd, 0, 0, ClickType.LeftDouble);

                GetElement(wd, "Металлургия", SearchWay.Name).Click();
                ClickByOffset(wd, 0, 0, ClickType.RightOne);
                GetElement(wd, "Добавить реакцию", SearchWay.Name).Click();
                GetElement(wd, "Металлургия", SearchWay.Name).Click();
                ClickByOffset(wd, 0, 0, ClickType.LeftDouble);
                GetElement(wd, "Реакция R1-R2,Масс.Доли-°C", SearchWay.Name).Click();
                ClickByOffset(wd, 0, 0, ClickType.RightOne);

                GetElement(wd, "Редактировать", SearchWay.Name).Click();
                GetElement(wd, "InitialPhase", SearchWay.Name).Click();
                ClickByOffset(wd, 70, 3, ClickType.LeftOne);
                ClickByOffset(wd, -20, 60, ClickType.LeftOne);

                GetElement(wd, "FinalPhase", SearchWay.Name).Click();
                ClickByOffset(wd, 70, 3, ClickType.LeftOne);
                ClickByOffset(wd, -20, 50, ClickType.LeftOne);

                GetElement(wd, "PhaseName", SearchWay.Name).Click();
                ClickByOffset(wd, 70, 3, ClickType.LeftOne);
                ClickByOffset(wd, -20, 20, ClickType.LeftOne);

                SendKey(wd, "Температура Строка 0", "300");
                SendKey(wd, "Масс.Доли Строка 0", $"0.9");
                SendKey(wd, "Температура Строка 1", "375");
                SendKey(wd, "Масс.Доли Строка 1", $"0.5");

                ClickByOffset(wd, 50, -200, ClickType.LeftOne);

                Thread.Sleep(3000);
            }
            catch (Exception e) { wd.CloseApp(); Assert.Fail(e.Message); }
            finally { wd.CloseApp(); }        
        }
       
    }
}
