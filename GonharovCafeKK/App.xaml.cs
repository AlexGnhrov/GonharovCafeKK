using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace GonharovCafeKK
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {

        internal static class NativeMethods
        {
            [DllImport("user32.dll")]
            internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

            [DllImport("user32.dll")]
            internal static extern bool SetForegroundWindow(IntPtr hWnd);

        }

        private Mutex _mutex;

        App()
        {

            string mutexName = $"Local\\{Assembly.GetExecutingAssembly().GetType().GUID}";

            // Создание Mutex
            _mutex = new Mutex(true, mutexName, out bool createdNew);

            if (!createdNew)
            {

                Process currentProcess = Process.GetCurrentProcess();

                foreach (Process process in Process.GetProcessesByName(currentProcess.ProcessName))
                {
                    if (process.Id != currentProcess.Id)
                    {
                        NativeMethods.ShowWindow(process.MainWindowHandle, 5);
                        NativeMethods.SetForegroundWindow(process.MainWindowHandle); // Установить фокус на главное окно процесса
                        break;
                    }
                }

                Current.Shutdown();
                return;
            }

        }




    }
}
