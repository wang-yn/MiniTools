using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace MiniTools.Libs.Common
{
    /// <summary>
    /// 注册表操作帮助类。
    /// </summary>
    public static class RegistryHelper
    {
        /// <summary>
        /// 获取注册表的ClassesRoot节点
        /// 如果是64位系统，不管当前进程是32位还是64位，只使用64位注册表地址。
        /// </summary>
        private static RegistryKey ClassesRoot {
            get {
                if( Environment.Is64BitOperatingSystem )
                    return RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry64);

                return Registry.ClassesRoot;
            }
        }

        /// <summary>
        /// 写入注册表信息。
        /// </summary>
        /// <param name="path">注册表路径。</param>
        /// <param name="dict">数据集合。</param>
        public static void Write(string path, Dictionary<string, string> dict)
        {
            if( string.IsNullOrEmpty(path) ) {
                throw new ArgumentNullException(nameof(path));
            }
            if( dict == null || dict.Count == 0 ) {
                throw new ArgumentException(nameof(dict));
            }

            using( RegistryKey registKey = ClassesRoot ) {
                RegistryKey subKey = registKey.OpenSubKey(path, true);
                if( subKey == null ) {
                    subKey = registKey.CreateSubKey(path);
                }

                foreach( var item in dict ) {
                    subKey.SetValue(item.Key, item.Value);
                }
                subKey.Close();
            }
        }

        /// <summary>
        /// 向指定路径写入指定名称的值。
        /// </summary>
        /// <param name="path">注册表路径。</param>
        /// <param name="name">注册表名称。</param>
        /// <param name="value">注册表值。</param>
        public static void Write(string path, string name, object value)
        {
            if( string.IsNullOrEmpty(path) )
                throw new ArgumentNullException(nameof(path));

            using( RegistryKey registKey = ClassesRoot ) {
                RegistryKey subKey = registKey.OpenSubKey(path, true);
                if( subKey == null ) {
                    subKey = registKey.CreateSubKey(path);
                }

                subKey.SetValue(name, value);
                subKey.Close();
            }
        }

        /// <summary>
        /// 读取注册表指定名称的字符串值。
        /// </summary>
        /// <param name="path">注册表路径。</param>
        /// <param name="name">注册表名称。</param>
        /// <returns></returns>
        public static string ReadString(string path, string name)
        {
            if( string.IsNullOrEmpty(path) )
                throw new ArgumentNullException(nameof(path));

            using( RegistryKey registKey = ClassesRoot.OpenSubKey(path) ) {
                if( registKey == null )
                    return string.Empty;
                return registKey.GetValue(name, string.Empty).ToString();
            }
        }

        /// <summary>
        /// 获取所有子项。
        /// </summary>
        /// <param name="path">注册表路径。</param>
        /// <returns></returns>
        public static string[] GetSubKeyNames(string path)
        {
            if( string.IsNullOrEmpty(path) )
                throw new ArgumentNullException(nameof(path));

            using( RegistryKey registKey = ClassesRoot.OpenSubKey(path) ) {
                // 根目录没有，返回空数组。
                if( registKey == null ) {
                    return new string[] { };
                }
                else {
                    return registKey.GetSubKeyNames();
                }

            }
        }

        /// <summary>
        /// 删除指定路径的注册表子项。
        /// </summary>
        /// <param name="path">注册表路径。</param>
        public static void Delete(string path)
        {
            if( string.IsNullOrEmpty(path) ) {
                throw new ArgumentNullException(nameof(path));
            }
            using( RegistryKey key = ClassesRoot ) {
                key.DeleteSubKey(path);
            }
        }
    }
}
