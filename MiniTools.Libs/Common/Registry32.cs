using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace MiniTools.Libs.Common
{
    /// <summary>
	/// 一个帮助类，允许32位程序访问64位注册表
	/// </summary>
	public static class Registry32
    {
        // copy from https://leonax.net/p/2889/accessing-64-bit-registry-from-32-bit-application/

        private enum RegSAM
        {
            QueryValue = 0x0001,
            SetValue = 0x0002,
            CreateSubKey = 0x0004,
            EnumerateSubKeys = 0x0008,
            Notify = 0x0010,
            CreateLink = 0x0020,
            WOW64_32Key = 0x0200,
            WOW64_64Key = 0x0100,
            WOW64_Res = 0x0300,
            Read = 0x00020019,
            Write = 0x00020006,
            Execute = 0x00020019,
            AllAccess = 0x000f003f
        }

        private static class RegHive
        {
            public static UIntPtr HKEY_LOCAL_MACHINE = new UIntPtr(0x80000002u);
            public static UIntPtr HKEY_CURRENT_USER = new UIntPtr(0x80000001u);
        }


        [DllImport("Advapi32.dll")]
        private static extern uint RegOpenKeyEx(
          UIntPtr hKey,
          string lpSubKey,
          uint ulOptions,
          int samDesired,
          out int phkResult);

        [DllImport("Advapi32.dll")]
        private static extern uint RegCloseKey(int hKey);

        [DllImport("advapi32.dll", EntryPoint = "RegQueryValueEx")]
        private static extern int RegQueryValueEx(
          int hKey, string lpValueName,
          int lpReserved,
          ref uint lpType,
          System.Text.StringBuilder lpData,
          ref uint lpcbData);

        public static string GetLocalMachineValue(String regPath, String inPropertyName)
        {
            return GetRegKey64(RegHive.HKEY_LOCAL_MACHINE, regPath, inPropertyName);
        }


        public static string GetCurrentUserValue(String regPath, String inPropertyName)
        {
            return GetRegKey64(RegHive.HKEY_CURRENT_USER, regPath, inPropertyName);
        }

        private static string GetRegKey64(UIntPtr hKey, String regPath, String name)
        {
            int hkey = 0;

            try {
                uint lResult = RegOpenKeyEx(hKey, regPath,
                    0, (int)RegSAM.QueryValue | (int)RegSAM.WOW64_64Key, out hkey);

                if( 0 != lResult )
                    return null;

                uint lpType = 0;
                uint lpcbData = 1024;
                StringBuilder buffer = new StringBuilder(1024);

                RegQueryValueEx(hkey, name, 0, ref lpType, buffer, ref lpcbData);
                return buffer.ToString();
            }
            finally {
                if( 0 != hkey )
                    RegCloseKey(hkey);
            }
        }



    }
}
