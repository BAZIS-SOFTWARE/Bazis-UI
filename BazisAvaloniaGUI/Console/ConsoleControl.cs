using Avalonia;
using Avalonia.Automation;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using BazisAvaloniaGUI.Localization;
using BazisAvaloniaGUI.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Resources;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Color = System.Drawing.Color;

namespace BazisAvaloniaGUI.Console
{
    internal sealed class ConsoleControl : UserControl
    {
        // Avalonia: перекрывает StyledElement.Resources, чтобы Resources.X ссылался на строковые ресурсы.
        private new class Resources : Localization.Resources { }

        public bool CheckPrintElemsInfo { get; set; }
        public bool CheckPrintNodesInfo { get; set; }
        public bool ShowTaskInfo { get; private set; }
        public int SessionNumber { get; set; }
        public string GetSessionLogPath
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                     SessionNumber.ToString() + "bazis.session.txt");
            }
        }

        public event Func<string, Task<string>> ConsoleCommandEnteredEvent;
        public event Action CommandsListRequestedEvent;
        // PinnedPage: щелчок по крестику в заголовке (в BaseForm у консоли не подписан).
        public event Action ControlCollapseEvent;

        // Avalonia: вместо RichTextBox — цветные строки вывода, последняя строка поля — ввод команды.
        internal ObservableCollection<ConsoleMessage> Messages { get; } = new();
        private readonly TextBox rtxbInput = new() { AcceptsReturn = false };
        private readonly ScrollViewer tlscOut;
        private readonly Border rtxbField;
        private readonly Button link;
        private readonly ConsoleMessage sessionHeader;

        public ConsoleControl()
        {
            InitializeComponent(out tlscOut, out rtxbField, out link);

            var path = $" > {Resources.CurrentSession} ";

            sessionHeader = new ConsoleMessage(path, Color.Green);
            Messages.Add(sessionHeader);
            Loaded += ConsoleControl_Load;
        }

        public void PrintInfo(string str, Color color)
        {
            if (!Dispatcher.UIThread.CheckAccess())
            {
                Dispatcher.UIThread.Post(() => PrintInfo(str, color));
                return;
            }

            Messages.Add(new ConsoleMessage($" > {str}", color));

            var path = GetSessionLogPath;
            using (StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default))
                sw.WriteLine(str); // каждое сообщение — отдельной строкой журнала (Write склеивал их в одну)

            rtxbInput.Focus();
            Dispatcher.UIThread.Post(() => tlscOut.ScrollToEnd());
        }

        public void PrintHistory(string str)
        {
            rtxbInput.Text = str;
            rtxbInput.CaretIndex = str.Length;
        }

        private async void Link_LinkClicked(object sender, RoutedEventArgs e)
        {
            var launcher = TopLevel.GetTopLevel(this)?.Launcher;
            if (launcher != null)
                await launcher.LaunchUriAsync(new Uri(GetSessionLogPath));
        }

        private void btnDictionary_Click(object sender, RoutedEventArgs e) => CommandsListRequestedEvent?.Invoke();

        private void ConsoleControl_Load(object sender, RoutedEventArgs e)
        {
            var rnd = new Random();
            SessionNumber = rnd.Next(0, 10000);

            link.Content = new TextBlock { Text = GetSessionLogPath, TextDecorations = TextDecorations.Underline };
        }

        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            var sessionPath = Messages[0];
            Messages.Clear();
            Messages.Add(sessionPath);
        }

        private async void btnBackGroundInfo_Click(object sender, RoutedEventArgs e)
        {
            if (TopLevel.GetTopLevel(this) is not Window owner)
                return;

            var current = rtxbField.Background is SolidColorBrush brush
                ? Color.FromArgb(brush.Color.A, brush.Color.R, brush.Color.G, brush.Color.B) : Color.White;
            var colorDialog = await new ColorSelectionDialog(current).ShowDialog<Color?>(owner);

            if (colorDialog == null)
                return;

            rtxbField.Background = PropertiesPanelControl.ColorBrush(colorDialog.Value);
        }

        private async Task ExecuteCmdFile(string cmdFileName)
        {
            if (System.IO.File.Exists(cmdFileName))
            {
                var variables = new Dictionary<string, string>();
                var cmdLines = File.ReadAllLines(cmdFileName);
                foreach (var line in cmdLines)
                {
                    if (line.StartsWith("//"))
                        continue;
                    var matches = Regex.Matches(line, @"\$(\w+)");
                    try
                    {
                        if (matches.Count > 0)
                        {
                            foreach (System.Text.RegularExpressions.Match match in matches)
                            {
                                var name = match.Groups[1].Value;

                                if (!variables.ContainsKey(name))
                                    variables[name] = "default";
                            }
                            await ProcessCommandLine(line, variables);
                        }
                        else
                            await ConsoleCommandEnteredEvent(line);
                    }
                    catch (OperationCanceledException)
                    {
                        // Отмена пользователем (Esc) — не ошибка: останавливаем макрос без красного текста.
                        PrintInfo($"{Resources.UtilityToolStrip_SelectObjectAsync_OperationCanceled_Message}: {line}", Color.Black);
                        break;
                    }
                    catch (Exception ex)
                    {
                        PrintInfo($"{ex.Message} in line: {line}", Color.Red);
                        break;
                    }
                }
            }
            else throw new Exception($"\n > {Resources.ExecuteCMDFileMissing}");
        }

        private async Task ProcessCommandLine(string line, Dictionary<string, string> variables)
        {
            var variableName = string.Empty;
            var newLine = string.Empty;

            var parts = line.Split('=', 2);

            if (parts.Length == 2)
            {
                variableName = parts[0].Trim().TrimStart('$');
                newLine = parts[1].Trim();
            }
            else newLine = line;

            //Заменяем переменные в строке вида "$name" значением из словаря
            foreach (var pair in variables)
                newLine = newLine.Replace("$" + pair.Key, pair.Value.ToString());

            var returnValue = await ConsoleCommandEnteredEvent(newLine);
            //Записываем результат "number" в Value словаря с ключом variableName
            SetValue(returnValue, variableName, variables);
        }

        private void SetValue(string returnValue, string variableName, Dictionary<string, string> variables)
        {
            if (variableName != string.Empty)
                variables[variableName] = returnValue;
        }

        private async void btnStartMacro_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TopLevel.GetTopLevel(this) is not { } top)
                    return;
                var files = await top.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
                {
                    AllowMultiple = false,
                    FileTypeFilter = [new FilePickerFileType("Bazis command file(*.tcf)") { Patterns = ["*.tcf"] }, FilePickerFileTypes.All]
                });

                if (files.Count == 0)
                    return;

                // Файл не из локальной ФС (например, из облачного провайдера) — пути нет.
                var path = files[0].TryGetLocalPath();
                if (string.IsNullOrEmpty(path))
                {
                    PrintInfo(Resources.ExecuteCMDFileMissing, Color.Red);
                    return;
                }

                await ExecuteCmdFile(path);
            }
            catch (Exception ex)
            {
                PrintInfo(ex.Message, Color.Red);
            }
        }

        private async void KeyDownEventHadler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Avalonia: команда вводится в отдельной строке, после ввода она остаётся в выводе,
                // как последняя строка RichTextBox в WinForms.
                e.Handled = true;
                var cmds = rtxbInput.Text ?? string.Empty;
                rtxbInput.Text = string.Empty;
                Messages.Add(new ConsoleMessage(cmds, Color.Black));
                try
                {
                    await ConsoleCommandEnteredEvent(cmds);
                }
                catch (OperationCanceledException)
                {
                    PrintInfo(Resources.UtilityToolStrip_SelectObjectAsync_OperationCanceled_Message, Color.Black);
                }
                catch (Exception ex)
                {
                    PrintInfo($"{ex.Message} in line: {cmds}", Color.Red);
                }
            }
            else if (e.Key == Key.Up)
            {
                e.Handled = true;
                PrintHistory(ConsoleHistory.GetPreviousCommand());
            }
            else if (e.Key == Key.Down)
            {
                e.Handled = true;
                PrintHistory(ConsoleHistory.GetNextCommand());
            }
        }

        // ---- Avalonia: визуальное дерево (аналог InitializeComponent из ConsoleControl.Designer.cs) ----

        private void InitializeComponent(out ScrollViewer output, out Border field, out Button logLink)
        {
            FontFamily = new FontFamily("Microsoft Sans Serif");
            FontSize = 11;

            // PinnedPage: заголовок высотой Padding.Top (в BaseForm console.Padding = 0,15,0,0),
            // название с x = 15 и крестик 8x8 в (Width - 15, Padding.Top / 2 - 4).
            var root = new Grid { RowDefinitions = new RowDefinitions("15,*"), ColumnDefinitions = new ColumnDefinitions("*,Auto") };
            var header = new Grid { Background = Brushes.Gainsboro };
            header.Children.Add(new TextBlock { Text = Resources.ConsoleControl_headerName_text, Foreground = Brushes.Black, Margin = new Thickness(15, 0, 0, 0) });
            var close = new Button
            {
                Name = "ControlCollapse",
                Classes = { "console-close" },
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 3, 6, 0),
                Content = new Avalonia.Controls.Shapes.Path
                {
                    Data = Avalonia.Media.Geometry.Parse("M0.5,0.5 H8.5 V8.5 H0.5 Z M1.5,1.5 L7.5,7.5 M1.5,7.5 L7.5,1.5"),
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                }
            };
            close.Click += (_, _) => ControlCollapseEvent?.Invoke();
            header.Children.Add(close);
            Grid.SetColumnSpan(header, 2);
            root.Children.Add(header);

            // rtxbField: строки вывода и строка ввода в одном поле без рамки (BorderStyle.None, SystemColors.Control).
            logLink = new Button { Classes = { "console-log" }, Cursor = new Cursor(StandardCursorType.Hand) };
            logLink.Click += Link_LinkClicked;

            var lines = new ItemsControl
            {
                ItemsSource = Messages,
                ItemTemplate = new FuncDataTemplate<ConsoleMessage>((message, _) => CreateLine(message))
            };
            rtxbInput.Classes.Add("console-input");
            AutomationProperties.SetName(rtxbInput, "Команда консоли");
            rtxbInput.KeyDown += KeyDownEventHadler;
            output = new ScrollViewer
            {
                Content = new StackPanel { Children = { lines, rtxbInput } },
                HorizontalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto,
                VerticalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto
            };
            field = new Border { Background = Brush.Parse("#F0F0F0"), Cursor = new Cursor(StandardCursorType.Ibeam), Child = output };
            // В RichTextBox щелчок в любом месте поля ставит каретку; здесь — переводит фокус на строку ввода.
            field.PointerPressed += (_, _) => rtxbInput.Focus();
            Grid.SetRow(field, 1);
            root.Children.Add(field);

            // toolStripEx1 в RightToolStripPanel: кнопки 25x25, картинки 16x16 из ConsoleControl.resx.
            var resources = new ResourceManager(typeof(ConsoleControl));
            var actions = new StackPanel { Background = Brush.Parse("#F0F0F0") };
            actions.Children.Add(ActionButton(resources, "spbDictionary", "dictionary.png", btnDictionary_Click));
            actions.Children.Add(ActionButton(resources, "toolStripButton1", "background.png", btnBackGroundInfo_Click));
            actions.Children.Add(ActionButton(resources, "toolStripButton2", "clear.png", ClearAll_Click));
            actions.Children.Add(ActionButton(resources, "btnStartMacro", "start-macro.png", btnStartMacro_Click));
            Grid.SetRow(actions, 1);
            Grid.SetColumn(actions, 1);
            root.Children.Add(actions);
            Content = new Border { BorderBrush = Brushes.DarkGray, BorderThickness = new Thickness(1), Child = root };
        }

        // Аналог HighlightPhrase: заголовок сессии зелёный целиком, у PrintInfo цветом выделяется только сообщение.
        private Control CreateLine(ConsoleMessage message)
        {
            var line = new SelectableTextBlock { Foreground = Brushes.Black, TextWrapping = TextWrapping.NoWrap };
            if (message == null)
                return line;

            var inlines = new InlineCollection();
            var colored = message.Text;
            if (!ReferenceEquals(message, sessionHeader) && colored.StartsWith(" > "))
            {
                inlines.Add(new Run(" > "));
                colored = colored.Substring(3);
            }
            inlines.Add(new Run(colored) { Foreground = PropertiesPanelControl.ColorBrush(message.Color) });
            line.Inlines = inlines;

            if (!ReferenceEquals(message, sessionHeader))
                return line;

            // LinkLabel с путём журнала стоит в первой строке сразу после " > Текущая сессия ".
            (link.Parent as Panel)?.Children.Remove(link);
            return new StackPanel { Orientation = Orientation.Horizontal, Children = { line, link } };
        }

        private static Button ActionButton(ResourceManager resources, string name, string image, EventHandler<RoutedEventArgs> handler)
        {
            using var stream = AssetLoader.Open(new Uri($"avares://BazisAvaloniaGUI/Console/Assets/{image}"));
            var text = resources.GetString(name + ".Text");
            var button = new Button
            {
                Name = name,
                Classes = { "console-action" },
                Content = new Image { Source = new Bitmap(stream), Width = 16, Height = 16, Stretch = Stretch.Fill }
            };
            AutomationProperties.SetName(button, text);
            ToolTip.SetTip(button, text);
            button.Click += handler;
            return button;
        }
    }

    /// <summary>Строка вывода консоли (аналог фрагмента текста RichTextBox с цветом).</summary>
    // Объявлена после ConsoleControl: ресурсы ConsoleControl.resx получают имя по первому типу файла.
    internal sealed record ConsoleMessage(string Text, Color Color);
}
