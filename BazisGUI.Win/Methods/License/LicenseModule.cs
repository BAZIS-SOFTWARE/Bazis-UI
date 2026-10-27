using Model.Interfaces.ObjectsCollections;
using Model.Interfaces;
using System.Collections.Generic;
using BazisGUI.Win.Extensions;
using ClientLogic;
using System.Net;
using System.Windows.Forms;
using System;
using System.Threading;

namespace BazisGUI.Win
{
    public partial class BaseForm
    {
        private void StartLicensing(string moduleName)
        {

            serverConnectionPing = new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        lock (serverConnection)
                        {
                            serverConnection.RequestServer(moduleName + " Работа");
                            if (serverConnection.Answer != "Работай")
                            {
                                throw new AccidentServerDisconnectionException();
                            }

                        }
                        Thread.Sleep(3000);
                    }

                }
                catch (Exception ex)
                {
                    if (ex is AccidentServerDisconnectionException)
                    {
                        Invoke(new Action(() =>
                        {
                            MessageBox.Show(this, "Внимание! Лицензирование прервано. Приложение будет заблокировано. Проверьте сервер лицензий.");
                            //Application.ExitThread();
                            UnBlockGeneralMenuInterface(moduleName, false);
                        }));
                    }
                }
            });
            serverConnectionPing.Start();

        }

        private void StopServerPing()
        {
            if (serverConnectionPing != null)
            {
                while (true)
                {
                    if (serverConnectionPing.ThreadState == System.Threading.ThreadState.WaitSleepJoin |
                        serverConnectionPing.ThreadState == System.Threading.ThreadState.Running
                        )
                        serverConnectionPing.Abort();
                    if (serverConnectionPing.ThreadState == System.Threading.ThreadState.Aborted |
                        serverConnectionPing.ThreadState == System.Threading.ThreadState.Stopped
                        )
                        break;
                }
            }
        }
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

        private void DisconnectWithServer(string moduleName)
        {
            //if (module != null)
            //{
            StopServerPing();
            serverConnection?.RequestServer(moduleName + " Отдать");
            //}
        }

        private void LicenseModule(string moduleName)
        {
            serverConnection?.RequestServer(moduleName + " Взять");

            if (serverConnection?.Answer == "можно")
            {
                UnBlockGeneralMenuInterface(moduleName, true);
                StartLicensing(moduleName);
            }

            else StartLisenceForm(moduleName + " Взять");
        }
    }
}
