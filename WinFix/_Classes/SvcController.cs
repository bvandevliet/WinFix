using System;
using System.ServiceProcess;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Linq;

static class Service
{
    public static ServiceControllerStatus GetStatus(string serviceName)
    {
        using (ServiceController serviceController = new ServiceController(serviceName))
        {
            return GetStatus(serviceController);
        }
    }
    private static ServiceControllerStatus GetStatus(ServiceController serviceController)
    {
        try
        {
            return serviceController.Status;
        }
        catch (Exception)
        {
            return ServiceControllerStatus.Stopped;
        }
    }

    public static ServiceStartMode GetStartType(string serviceName)
    {
        using (ServiceController serviceController = new ServiceController(serviceName))
        {
            return GetStartType(serviceController);
        }
    }
    private static ServiceStartMode GetStartType(ServiceController serviceController)
    {
        try
        {
            return serviceController.StartType;
        }
        catch (Exception)
        {
            return ServiceStartMode.Disabled;
        }
    }

    public static bool EnableDisable(string serviceName, bool Enable, ServiceStartMode defaultStartMode, bool destroy = false)
    {
        /**
         * Verify if the service exists or bail early ..
         */
        ServiceController serviceController =
            ServiceController.GetServices().FirstOrDefault(sc => sc.ServiceName == serviceName);

        if (serviceController == null)
        {
            return true;
        }

        using (serviceController)
        {
            return EnableDisable(serviceController, Enable, defaultStartMode, destroy);
        }
    }

    private static bool EnableDisable(ServiceController serviceController, bool Enable, ServiceStartMode defaultStartMode, bool destroy = false)
    {
        /**
         * Try to change start mode, attempt to fallback to registry hack if failed.
         */
        if (!ServiceHelper.ChangeStartMode(serviceController, Enable ? defaultStartMode : ServiceStartMode.Disabled))
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey($@"SYSTEM\CurrentControlSet\Services\{serviceController.ServiceName}", true))
                {
                    if (key != null)
                    {
                        key.SetValue("Start", Enable ? (uint)defaultStartMode : (uint)ServiceStartMode.Disabled, RegistryValueKind.DWord);

                        if (Enable)
                        {
                            // Restore ImagePath ..
                            string imagePath = (string)key.GetValue("ImagePath.bac");
                            if (imagePath != null)
                            {
                                key.SetValue("ImagePath", imagePath);
                                key.DeleteValue("ImagePath.bac");
                            }
                        }
                        else if (destroy)
                        {
                            // Remove ImagePath ..
                            string imagePath = (string)key.GetValue("ImagePath");
                            if (imagePath != null)
                            {
                                key.SetValue("ImagePath.bac", imagePath);
                                key.DeleteValue("ImagePath");
                            }
                        }
                    }

                    key?.Close();
                }
            }
            catch (Exception)
            {
            }

            /**
             * Start or stop the service and verify start mode change succeeded.
             */
            //TimeSpan StartStopTimeout = TimeSpan.FromSeconds(15);
            if (Enable)
            {
                if (GetStatus(serviceController) != ServiceControllerStatus.Running && (uint)defaultStartMode <= 2)
                {
                    try
                    {
                        serviceController.Start();
                        //serviceController.WaitForStatus(ServiceControllerStatus.Running, StartStopTimeout);
                    }
                    catch (Exception)
                    {
                        //Console.WriteLine($"\nCould not start \"{serviceController.DisplayName}\" service.");
                    }
                }

                if (GetStartType(serviceController) == defaultStartMode)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine($"Failed to enable service \"{serviceController.ServiceName}\"");
                }
            }
            else
            {
                if (GetStatus(serviceController) != ServiceControllerStatus.Stopped)
                {
                    try
                    {
                        serviceController.Stop();
                        //serviceController.WaitForStatus(ServiceControllerStatus.Stopped, StartStopTimeout);
                    }
                    catch (Exception)
                    {
                        //Console.WriteLine($"\nCould not stop \"{serviceController.DisplayName}\" service.");
                    }
                }

                if (!IsEnabled(serviceController))
                {
                    return true;
                }
                else
                {
                    Console.WriteLine($"Failed to disable service \"{serviceController.ServiceName}\"");
                }
            }
        }

        return false;
    }


    public static bool IsEnabled(params string[] serviceNames)
    {
        foreach (string ServiceName in serviceNames)
        {
            using (ServiceController serviceController = new ServiceController(ServiceName))
            {
                if (IsEnabled(serviceController))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private static bool IsEnabled(ServiceController serviceController)
    {
        return
            GetStartType(serviceController) != ServiceStartMode.Disabled &&
            null != Registry.GetValue($@"HKEY_LOCAL_MACHINE\System\CurrentControlSet\Services\{serviceController.ServiceName}", "ImagePath", null);
    }
}

static class ServiceHelper
{
    [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    static extern bool ChangeServiceConfig(
        IntPtr hService,
        uint nServiceType,
        uint nStartType,
        uint nErrorControl,
        string lpBinaryPathName,
        string lpLoadOrderGroup,
        IntPtr lpdwTagId,
        [In] char[] lpDependencies,
        string lpServiceStartName,
        string lpPassword,
        string lpDisplayName);

    [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern IntPtr OpenService(
        IntPtr hSCManager, string lpServiceName, uint dwDesiredAccess);

    [DllImport("advapi32.dll", EntryPoint = "OpenSCManagerW", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
    static extern IntPtr OpenSCManager(
        string machineName, string databaseName, uint dwAccess);

    [DllImport("advapi32.dll", EntryPoint = "CloseServiceHandle")]
    static extern int CloseServiceHandle(IntPtr hSCObject);

    private const uint SERVICE_NO_CHANGE = 0xFFFFFFFF;
    private const uint SERVICE_QUERY_CONFIG = 0x00000001;
    private const uint SERVICE_CHANGE_CONFIG = 0x00000002;
    private const uint SC_MANAGER_ALL_ACCESS = 0x000F003F;

    public static bool ChangeStartMode(ServiceController serviceController, ServiceStartMode StartMode)
    {
        IntPtr scManagerHandle = IntPtr.Zero;
        try
        {
            scManagerHandle = OpenSCManager(null, null, SC_MANAGER_ALL_ACCESS);
        }
        catch (Exception)
        {
        }
        if (scManagerHandle == IntPtr.Zero)
        {
            return false;

            //throw new ExternalException("Open Service Manager Error");
        }

        IntPtr serviceHandle = IntPtr.Zero;
        try
        {
            serviceHandle = OpenService(
                scManagerHandle,
                serviceController.ServiceName,
                SERVICE_QUERY_CONFIG | SERVICE_CHANGE_CONFIG);
        }
        catch (Exception)
        {
        }
        if (serviceHandle == IntPtr.Zero)
        {
            return false;

            //throw new ExternalException("Open Service Error");
        }

        var result = false;
        try
        {
            result =
            ChangeServiceConfig(
                serviceHandle,
                SERVICE_NO_CHANGE,
                (uint)StartMode,
                SERVICE_NO_CHANGE,
                null,
                null,
                IntPtr.Zero,
                null,
                null,
                null,
                null);
        }
        catch (Exception)
        {
        }
        if (result == false)
        {
            return false;

            //int nError = Marshal.GetLastWin32Error();
            //var win32Exception = new Win32Exception(nError);
            //throw new ExternalException("Could not change service start type: " + win32Exception.Message);
        }

        try
        {
            CloseServiceHandle(serviceHandle);
            CloseServiceHandle(scManagerHandle);
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }
}