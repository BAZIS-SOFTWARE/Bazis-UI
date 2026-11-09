using ClientLogic;
using System;
using System.Net;

namespace BazisGUI.Avalonia.Shell
{
    // Из GUI/Methods/License/LicenseModule.cs перенесено подключение к серверу лицензий для пункта
    // "Лицензия → Сведения". Захват и продление лицензии модуля (LicenseModule, StartLicensing) вызываются
    // из формы лицензирования ClientGUI.ClientControl, которая реализована на WinForms и в Avalonia не перенесена.
    internal partial class MainWindow
    {
        ClientController serverConnection;

        private bool TryServerConnection()
        {
            var net = Environment.GetEnvironmentVariable("BazisServerPath");

            if (!string.IsNullOrWhiteSpace(net))
            {
                var endpoint = IPEndPoint.Parse(net);
                serverConnection = new ClientController(endpoint.Address, endpoint.Port);

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
