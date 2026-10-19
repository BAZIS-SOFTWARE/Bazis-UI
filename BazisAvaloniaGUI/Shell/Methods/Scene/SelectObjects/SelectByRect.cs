namespace BazisAvaloniaGUI.Shell
{
    // Подбор объектов рамкой выполняет SceneView; из SelectByRect.cs перенесено склонение для сообщения в консоль.
    internal partial class MainWindow
    {
        private string Declination(int input)
        {
            string s = Resources.SelectByRect_Declination_Type1;
            if (input % 10 == 1) s = Resources.SelectByRect_Declination_Type2;
            if (input % 10 >= 2 && input % 10 <= 4) s = Resources.SelectByRect_Declination_Type3;

            return s;
        }
    }
}
