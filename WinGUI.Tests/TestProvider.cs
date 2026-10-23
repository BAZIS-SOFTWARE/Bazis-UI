using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WindowsInput;

namespace TestGUI
{

    public enum SearchWay
    {
        Name,
        ID,
        TagName,
        XPath
    }

    public enum ClickType
    {
        LeftOne,
        LeftDouble,
        LeftClickAndHold,
        RightOne,
        RightClickAndHold,
        MiddleOne,
        MiddleClickAndHold
    }

    public static class TestProvider
    {

        [StructLayout(LayoutKind.Sequential)]
        struct INPUT
        {
            public int type;
            public MOUSEINPUT mi;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public int mouseData;
            public int dwFlags;
            public int time;
            public nint dwExtraInfo;
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        private const int LEFTDOWN = 0x0002;
        private const int LEFTUP = 0x0004;
        private const int RIGHTDOWN = 0x0008;
        private const int RIGHTUP = 0x0010;
        private const int MIDDLEDOWN = 0x0020;
        private const int MIDDLEUP = 0x0040;

        public static WindowsDriver<WindowsElement> CreateWinDriver(string args)
        {
            WindowsDriver<WindowsElement> wd;

            var opt = new AppiumOptions();
            if (args == "") 
                args = "Без загрузки";

            opt.AddAdditionalCapability("app", Path.GetFullPath(@".\..\..\..\..\PropertiesDataBases\bin\Debug\PropertiesDataBases.exe"));
            opt.AddAdditionalCapability("ms:waitForAppLaunch", "10");
            opt.AddAdditionalCapability("appArguments", args);
            opt.PlatformName = "Windows11x64";
            var url = new Uri("http://127.0.0.1:4723");
            return new WindowsDriver<WindowsElement>(url, opt);
        }

        public static WindowsElement GetElement(WindowsDriver<WindowsElement> wd, string searchArg, SearchWay search = SearchWay.Name, double timeOut = 10)
        {
            WindowsElement element = null;
            var wait = new DefaultWait<WindowsDriver<WindowsElement>>(wd)
            {
                Timeout = TimeSpan.FromSeconds(timeOut),
                Message = $"Element by searching argument \"{searchArg}\" not found."
            };

            wait.IgnoreExceptionTypes(typeof(WebDriverException));
            try
            {
                wait.Until(wd =>
                {
                    if (search == SearchWay.Name)
                        element = wd.FindElement(By.Name(searchArg));
                    else if (search == SearchWay.ID)
                        element = wd.FindElement(By.Id(searchArg));
                    else if (search == SearchWay.ID)
                        element = wd.FindElement(By.TagName(searchArg));
                    else
                        element = wd.FindElement(By.XPath(searchArg));
                    return element;
                });
            }
            catch (WebDriverTimeoutException ex) { Assert.Fail(ex.Message); }
            return element;
        }

        /// <summary>
        /// Исполнение клика/нажатия на определенную кнопку мыши с возможностью смещения координат от исходного положения курсора
        /// </summary>
        /// <param name="wd">Экземпляр сессии WindowsApplicationDriver</param>
        /// <param name="x">смещение по X в компьютерных координатах</param>
        /// <param name="y">смещение по Y в компьютерных координатах</param>
        /// <param name="clickType">Тип клика, определен по умолчанию</param>
        /// <param name="timeOut">Время ожидания, определено по умолчанию</param>
        public static void ClickByOffset(WindowsDriver<WindowsElement> wd, int x = 0, int y = 0, ClickType clickType = ClickType.LeftOne, int delayAfterInMilliSeconds = 0, double timeOut = 10)
        {
            var wait = new DefaultWait<WindowsDriver<WindowsElement>>(wd)
            {
                Timeout = TimeSpan.FromSeconds(timeOut),
                Message = $"Clicking on Element by offset ({x}, {y}) turn into error: could not click by offset"
            };
            try
            {
                const int INPUT_MOUSE = 0;
                void MouseDownButton(ClickType clickType)
                {
                    INPUT[] input = new INPUT[1];
                    input[0].type = INPUT_MOUSE;
                    if (clickType == ClickType.LeftOne || clickType == ClickType.LeftDouble || clickType == ClickType.LeftClickAndHold)
                    {
                        input[0].mi.dwFlags = LEFTDOWN;
                    }
                    else if (clickType == ClickType.MiddleOne || clickType == ClickType.MiddleClickAndHold)
                    {
                        input[0].mi.dwFlags = MIDDLEDOWN;
                    }
                    else if (clickType == ClickType.RightOne || clickType == ClickType.RightClickAndHold)
                    {
                        input[0].mi.dwFlags = RIGHTDOWN;
                    }
                    SendInput(1, input, Marshal.SizeOf(typeof(INPUT)));
                }

                void MouseUpButton(ClickType clickType)
                {
                    INPUT[] input = new INPUT[1];
                    input[0].type = INPUT_MOUSE;
                    if (clickType == ClickType.LeftOne || clickType == ClickType.LeftDouble || clickType == ClickType.LeftClickAndHold)
                    {
                        input[0].mi.dwFlags = LEFTUP;
                    }
                    else if (clickType == ClickType.MiddleOne || clickType == ClickType.MiddleClickAndHold)
                    {
                        input[0].mi.dwFlags = MIDDLEUP;
                    }
                    else if (clickType == ClickType.RightOne || clickType == ClickType.RightClickAndHold)
                    {
                        input[0].mi.dwFlags = RIGHTUP;
                    }
                    SendInput(1, input, Marshal.SizeOf(typeof(INPUT)));
                }

                void Offset()
                {
                    var CursorX = Cursor.Position.X;
                    var CursorY = Cursor.Position.Y;
                    var EndPointX = CursorX + x;
                    var EndPointY = CursorY + y;

                    int steps = 20;
                    for (int i = 0; i <= steps; i++)
                    {
                        int newX = CursorX + (EndPointX - CursorX) * i / steps;
                        int newY = CursorY + (EndPointY - CursorY) * i / steps;
                        SetCursorPos(newX, newY);
                        Thread.Sleep(16);
                    }
                }

                void Click()
                {
                    MouseDownButton(clickType);
                    MouseUpButton(clickType);
                }

                if (clickType == ClickType.LeftOne || clickType == ClickType.MiddleOne || clickType == ClickType.RightOne)
                {
                    Offset();
                    Click();
                }
                else if (clickType == ClickType.LeftDouble)
                {
                    Offset();
                    Click();
                    Click();
                }
                else
                {
                    MouseDownButton(clickType);
                    Offset();
                    MouseUpButton(clickType);
                }
                Thread.Sleep(delayAfterInMilliSeconds);
                return;
            }
            catch (WebDriverTimeoutException ex) { Assert.Fail(ex.Message); }
        }

        public static void SendKey(WindowsDriver<WindowsElement> wd, string name, string value)
        {
            wd.FindElement(By.Name(name)).Click();

            wd.FindElement(By.Name(name)).SendKeys(value);
        }

        /// <summary>
        /// Передвигает курсор на выбранные координаты, относительно начала координат
        /// По умолчанию, отправляет курсор в начало координат
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void MoveFromStartCoordinates(int x = 0, int y = 0)
        {
            try
            {
                var CursorX = Cursor.Position.X;
                var CursorY = Cursor.Position.Y;
                Cursor.Position = new System.Drawing.Point(x, y);
            }
            catch (WebDriverTimeoutException ex) { Assert.Fail(ex.Message); }
        }

        /// <summary>
        /// Передвигает курсор в центр экрана
        /// </summary>
        public static void MoveCursorToCenter()
        {
            var screenWidth = Screen.PrimaryScreen.Bounds.Width;
            var screenHeight = Screen.PrimaryScreen.Bounds.Height;
            MoveFromStartCoordinates(screenWidth / 2, screenHeight / 2);
        }

        /// <summary>
        /// Краткое нажатие клавиши.
        /// </summary>
        /// <param name="key">Клавиша для нажатия</param>
        public static void PressKey(WindowsInput.Native.VirtualKeyCode key)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(key);
        }

        /// <summary>
        /// Краткое нажатие группы клавиш
        /// </summary>
        /// <param name="keys">Набор клавиш для краткого нажатия</param>
        public static void PressKey(WindowsInput.Native.VirtualKeyCode[] keys)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyPress(keys);
        }

        /// <summary>
        /// Зажатие клавиши
        /// Клавиша необязательно должна быть свободной для выполнения метода
        /// </summary>
        /// <param name="key"></param>
        public static void DownKey(WindowsInput.Native.VirtualKeyCode key)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyDown(key);
        }

        /// <summary>
        /// Освобождение зажатой клавиши.
        /// Освобождение клавиши, зажатой командной DownKey.
        /// Клавиша необязательно должна быть зажатой для выполнения метода
        /// </summary>
        /// <param name="key"></param>
        public static void UpKey(WindowsInput.Native.VirtualKeyCode key)
        {
            var sim = new InputSimulator();
            sim.Keyboard.KeyUp(key);
        }

        /// <summary>
        /// Сохраняет строку в буфер обмена, с помощью библиотеки InputSimulator происходит 
        /// имитация нажатия комбинации клавиш "ctrl + V" для передачи строки в поле ввода
        /// </summary>
        /// <param name="wd"></param>
        /// <param name="valueString">Строка, которую необходимо сохранить в буфер обмена и затем вставить</param>
        public static void SetClipboardAndPaste(WindowsDriver<WindowsElement> wd, string valueString, int delayAfterInMilliseconds = 0)
        {
            var thread = new Thread(() =>
            {
                Clipboard.SetText(valueString);
            });
            thread.SetApartmentState(ApartmentState.STA); // Clipboard требует однопоточного режима (STA)
            thread.Start();
            thread.Join(); // Дожидается завершения потока перед продолжением (гарантирована передает строку в буфер обмена)
            Thread.Sleep(500);//Без паузы не успевает вставить строку 
            var simulatorInput = new InputSimulator();
            simulatorInput.Keyboard.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.CONTROL, WindowsInput.Native.VirtualKeyCode.VK_A);
            simulatorInput.Keyboard.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.CONTROL, WindowsInput.Native.VirtualKeyCode.VK_V);
            simulatorInput.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.RETURN);
            Thread.Sleep(delayAfterInMilliseconds);
        }

        /// <summary>
        /// Метод для чтения консоли приложения и поиска в ней сообщений об ошибках (с пометкой error) с выводом в лог.
        /// При наличии ошибок тест считается не пройденным. 
        /// </summary>
        /// <param name="wd"></param>
        public static void SearchNotifications(WindowsDriver<WindowsElement> wd)
        {
            Thread.Sleep(1500);
            var console = wd.FindElementByAccessibilityId("rtxbField");
            SearchConsoleErrorMatches(wd, console.Text);
        }

        public static IEnumerable<string> GetConsoleLines(WindowsDriver<WindowsElement> wd, bool shouldClearConsole)
        {
            Thread.Sleep(1500);
            var console = wd.FindElementByAccessibilityId("rtxbField");
            SearchConsoleErrorMatches(wd, console.Text);
            var text = console.Text;

            if (shouldClearConsole)
                GetElement(wd, "Очистить консоль").Click();

            return text.Split("\r");
        }

        private static void SearchConsoleErrorMatches(WindowsDriver<WindowsElement> wd, string lines)
        {
            var matches = Regex.Matches(lines, @"error >([^\r]+)");

            foreach (Match match in matches)
                TestContext.WriteLine($"{match.Groups[1]}");

            if (matches.Count > 0)
            {
                try { Assert.Fail("Тест не пройден - в консоли есть ошибки"); }
                finally { wd.CloseApp(); }
            }
        }
    }
}
