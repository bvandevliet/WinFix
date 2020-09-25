using System;
using System.ServiceProcess;
using Microsoft.Win32;
using System.Runtime.InteropServices;

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

    public bool Start()
    {
        return StartStop(true);
    }

    public bool Stop()
    {
        return StartStop(false);
    }

    public bool StartStop(bool Enable, bool destroy = false)
    {
        return StartStop(_serviceController, Enable, DefaultStartMode, destroy);
    }

    public static bool StartStop(string ServiceName, bool Enable, ServiceStartMode DefaultStartMode, bool destroy = false)
    {
        using (ServiceController serviceController = new ServiceController(ServiceName))
        {
            return StartStop(serviceController, Enable, DefaultStartMode, destroy);
        }
    }

    public static bool StartStop(ServiceController serviceController, bool Enable, ServiceStartMode DefaultStartMode, bool destroy = false)
    {
        TimeSpan StartStopTimeout = TimeSpan.FromSeconds(15);

        try
        {
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey($@"SYSTEM\CurrentControlSet\Services\{serviceController.ServiceName}", true))
            {
                if (Enable)
                {
                    bool success = ServiceHelper.ChangeStartMode(serviceController, DefaultStartMode);

                    if (key != null)
                    {
                        try
                        {
                            key.SetValue("Start", (uint)DefaultStartMode, RegistryValueKind.DWord);
                        }
                        catch (Exception)
                        {
                        }

                        try
                        {
                            // Restore ImagePath ..
                            string imagePath = (string)key.GetValue("ImagePath.bac");
                            if (imagePath != null)
                            {
                                key.SetValue("ImagePath", imagePath);
                                key.DeleteValue("ImagePath.bac");
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }

                    if (GetStatus(serviceController) != ServiceControllerStatus.Running && (uint)DefaultStartMode <= 2)
                    {
                        serviceController.Start();
                        serviceController.WaitForStatus(ServiceControllerStatus.Running, StartStopTimeout);
                    }

                    if (GetStartType(serviceController) == DefaultStartMode)
                    {
                        return success;
                    }
                    else
                    {
                        Console.WriteLine($"Failed to enable service \"{serviceController.ServiceName}\"");
                    }
                }
                else
                {
                    bool success = ServiceHelper.ChangeStartMode(serviceController, ServiceStartMode.Disabled);

                    if (key != null)
                    {
                        try
                        {
                            key.SetValue("Start", (uint)ServiceStartMode.Disabled, RegistryValueKind.DWord);
                        }
                        catch (Exception)
                        {
                        }

                        if (destroy)
                        {
                            try
                            {
                                // Remove ImagePath ..
                                string imagePath = (string)key.GetValue("ImagePath");
                                if (imagePath != null)
                                {
                                    key.SetValue("ImagePath.bac", imagePath);
                                    key.DeleteValue("ImagePath");
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }

                    }

                    if (GetStatus(serviceController) != ServiceControllerStatus.Stopped)
                    {
                        serviceController.Stop();
                        serviceController.WaitForStatus(ServiceControllerStatus.Stopped, StartStopTimeout);
                    }

                    if (!IsEnabled(serviceController))
                    {
                        return success;
                    }
                    else
                    {
                        if (destroy)
                        {
                            Console.WriteLine($"Failed to disable service \"{serviceController.ServiceName}\", but should be disabled after a reboot.");
                        }
                        else
                        {
                            Console.WriteLine($"Failed to disable service \"{serviceController.ServiceName}\"");
                        }
                    }
                }

                key?.Close();
            }
        }
        catch (Exception)
        {
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
        var scManagerHandle = OpenSCManager(null, null, SC_MANAGER_ALL_ACCESS);
        if (scManagerHandle == IntPtr.Zero)
        {
            return false;

            //throw new ExternalException("Open Service Manager Error");
        }

        var serviceHandle = OpenService(
            scManagerHandle,
            serviceController.ServiceName,
            SERVICE_QUERY_CONFIG | SERVICE_CHANGE_CONFIG);

        if (serviceHandle == IntPtr.Zero)
        {
            return false;

            //throw new ExternalException("Open Service Error");
        }

        var result =
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

        if (result == false)
        {
            return false;

            //int nError = Marshal.GetLastWin32Error();
            //var win32Exception = new Win32Exception(nError);
            //throw new ExternalException("Could not change service start type: " + win32Exception.Message);
        }

        CloseServiceHandle(serviceHandle);
        CloseServiceHandle(scManagerHandle);

        return true;
    }
}