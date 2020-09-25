using System;
using System.Diagnostics;
using System.Management.Automation;

static class Commands
{
    public static Process _getInstance()
    {
        Process proc = new Process()
        {
            StartInfo = new ProcessStartInfo()
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                Verb = "runas",
            }
        };

        return proc;
    }

    public static void _runInstance(Process proc)
    {
        /**
         * Wait max 5 minutes to complete.
         */
        proc.Start();
        proc.WaitForExit(300000);
    }

    public static void taskkill(string process)
    {
        using (Process proc = _getInstance())
        {
            proc.StartInfo.FileName = "taskkill.exe";
            proc.StartInfo.Arguments = $"/f /t /im {process}.exe";

            _runInstance(proc);

            proc.Close();
        }
    }

    public static void dism(string feature, bool Enable)
    {
        using (Process proc = _getInstance())
        {
            proc.StartInfo.FileName = "dism.exe";
            proc.StartInfo.Arguments = $"/Online /{(Enable ? "Enable" : "Disable")}-Feature /FeatureName:{feature}";

            _runInstance(proc);

            proc.Close();
        }
    }

    public static void schtasks(string task, bool Enable)
    {
        using (Process proc = _getInstance())
        {
            proc.StartInfo.FileName = "schtasks.exe";
            proc.StartInfo.Arguments = $"/Change /TN \"{task}\" /{(Enable ? "Enable" : "Disable")}";

            _runInstance(proc);

            proc.Close();
        }
    }

    public static void regimport(string regFile)
    {
        using (Process proc = _getInstance())
        {
            proc.StartInfo.FileName = "reg.exe";

            proc.StartInfo.Arguments = $"import \"{regFile}\"";

            _runInstance(proc);

            proc.StartInfo.Arguments = $"import \"{regFile}\" /reg:64";

            _runInstance(proc);

            proc.Close();
        }
    }

    public static void InvokePS(string command)
    {
        using (PowerShell ps = PowerShell.Create())
        {
            ps.AddScript(command);

            ps.Invoke();
        }
    }
}

static class TakeOwnership
{
    public static void File(string path)
    {
        using (Process proc = Commands._getInstance())
        {
            proc.StartInfo.FileName = "cmd.exe";
            proc.StartInfo.Arguments = $"/c takeown /f \"{path}\" /d Y && icacls \"{path}\" /grant administrators:F /c";

            Commands._runInstance(proc);

            proc.Close();
        }
    }

    public static void Folder(string path)
    {
        using (Process proc = Commands._getInstance())
        {
            proc.StartInfo.FileName = "cmd.exe";
            proc.StartInfo.Arguments = $"/c takeown /f \"{path}\" /r /d Y && icacls \"{path}\" /grant Administrators:F /t /c";

            Commands._runInstance(proc);

            proc.Close();
        }
    }
}