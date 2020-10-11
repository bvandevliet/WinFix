using System;
using System.ServiceProcess;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Linq;

class Service : IDisposable
{
    private readonly ServiceController _serviceController = null;

    public string ServiceName
    {
        get => _serviceController.ServiceName;
    }

    public ServiceStartMode DefaultStartMode
    {
        get;
    }

    public ServiceControllerStatus Status
    {
        get => GetStatus(_serviceController);
    }

    public ServiceStartMode StartType
    {
        get => GetStartType(_serviceController);
    }

    public Service(string ServiceName, ServiceStartMode DefaultStartMode)
    {
        _serviceController = new ServiceController(ServiceName);
        this.DefaultStartMode = DefaultStartMode;
    }

    public static ServiceControllerStatus GetStatus(string ServiceName)
    {
        using (ServiceController serviceController = new ServiceController(ServiceName))
        {
            return GetStatus(serviceController);
        }
    }
    public static ServiceControllerStatus GetStatus(ServiceController serviceController)
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

    public static ServiceStartMode GetStartType(string ServiceName)
    {
        using (ServiceController serviceController = new ServiceController(ServiceName))
        {
            return GetStartType(serviceController);
        }
    }
    public static ServiceStartMode GetStartType(ServiceController serviceController)
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

    public bool Enable()
    {
        return EnableDisable(true);
    }

    public bool Disable()
    {
        return EnableDisable(false);
    }

    public bool EnableDisable(bool Enable, bool destroy = false)
    {
        return EnableDisable(_serviceController, Enable, DefaultStartMode, destroy);
    }

    public static bool EnableDisable(string ServiceName, bool Enable, ServiceStartMode DefaultStartMode, bool destroy = false)
    {
        using (ServiceController serviceController = new ServiceController(ServiceName))
        {
            return EnableDisable(serviceController, Enable, DefaultStartMode, destroy);
        }
    }

    public static bool EnableDisable(ServiceController serviceController, bool Enable, ServiceStartMode DefaultStartMode, bool destroy = false)
    {
        /**
         * Return early if the service does not exist.
         */
        try
        {
            if (serviceController.DisplayName == null)
            {
                return true;
            }
        }
        catch (Exception)
        {
            return true;
        }

        /**
         * Try to change start mode, attempt to fallback to registry hack if failed.
         */
        if (!ServiceHelper.ChangeStartMode(serviceController, Enable ? DefaultStartMode : ServiceStartMode.Disabled))
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey($@"SYSTEM\CurrentControlSet\Services\{serviceController.ServiceName}", true))
                {
                    if (key != null)
                    {
                        key.SetValue("Start", Enable ? (uint)DefaultStartMode : (uint)ServiceStartMode.Disabled, RegistryValueKind.DWord);

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
                if (GetStatus(serviceController) != ServiceControllerStatus.Running && (uint)DefaultStartMode <= 2)
                {
                    try
                    {
                        serviceController.Start();
                        //serviceController.WaitForStatus(ServiceControllerStatus.Running, StartStopTimeout);
                    }
                    catch (Exception)
                    {
                    }
                }

                if (GetStartType(serviceController) == DefaultStartMode)
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


    public static bool IsEnabled(params string[] ServiceNames)
    {
        foreach (string ServiceName in ServiceNames)
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

    public static bool IsEnabled(ServiceController serviceController)
    {
        if (
            GetStartType(serviceController) != ServiceStartMode.Disabled &&
            null != Registry.GetValue($@"HKEY_LOCAL_MACHINE\System\CurrentControlSet\Services\{serviceController.ServiceName}", "ImagePath", null)
        )
        {
            return true;
        }

        return false;
    }


    ~Service()
    {
        Dispose();
    }

    public void Dispose()
    {
        _serviceController?.Close();
        _serviceController?.Dispose();
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
        }

        return true;
    }
}