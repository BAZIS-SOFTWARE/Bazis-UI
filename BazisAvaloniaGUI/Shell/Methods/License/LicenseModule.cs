using ClientLogic;
using System;
using System.Net;

namespace BazisAvaloniaGUI.Shell
{
    // Из GUI/Methods/License/LicenseModule.cs перенесено подключение к серверу лицензий для пункта
    // "Лицензия → Сведения". Захват и продление лицензии модуля (LicenseModule, StartLicensing) вызываются
    // из формы лицензирования ClientGUI.ClientControl, которая реализована на WinForms и в Avalonia не перенесена.
    internal partial class MainWindow
    {
        ClientController serverConnection;

        private bool TryServerConnection()
        {
            var net = Environment.GetEnvironmentVariable("BazisServerPath", EnvironmentVariableTarget.Machine);

            if (net != null)
            {
                var iPAddress = IPAddress.Parse(net.Split(':')[0]);
                var port = int.Parse(net.Split(':')[1]);

                serverConnection = new ClientController(iPAddress, port);

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
