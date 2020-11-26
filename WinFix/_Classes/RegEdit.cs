using System;
using Microsoft.Win32;

/**
 * Make global RegistryKey (and also pass to ServiceHelper()) instead of "string RegPath" !!
 */

static class RegEdit
{
    public static bool IsValue(string keyName, string valueName, dynamic value)
    {
        try
        {
            dynamic current_value = Registry.GetValue(keyName, valueName, null);

            return current_value == value;
        }
        catch (Exception)
        {
            //Console.WriteLine(Ex.Message);
        }

        return false;
    }

    public static void SetValue(string keyName, string valueName, dynamic value)
    {
        try
        {
            Registry.SetValue(keyName, valueName, value);
        }
        catch (Exception Ex)
        {
            //Console.WriteLine(Ex.Message);
        }
    }
}
