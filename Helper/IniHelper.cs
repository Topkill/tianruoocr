using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace TrOCR.Helper
{

	/// <summary>
	/// 提供对INI配置文件的读写操作的辅助类
	/// </summary>
	public static class IniHelper
	{

		/// <summary>
		/// 从INI配置文件中读取指定节和键的字符串值
		/// </summary>
		/// <param name="sectionName">INI节名称</param>
		/// <param name="key">键名</param>
		/// <param name="defaultValue">默认值，当找不到指定的节或键时返回该值</param>
		/// <param name="returnBuffer">用于接收读取数据的字节数组缓冲区</param>
		/// <param name="size">缓冲区大小</param>
		/// <param name="filePath">INI文件完整路径</param>
		/// <returns>返回实际读取到的字符数</returns>
		[DllImport("kernel32")]
		public static extern int GetPrivateProfileString(string sectionName, string key, string defaultValue, byte[] returnBuffer, int size, string filePath);

		/// <summary>
		/// 将字符串写入INI配置文件中指定的节和键
		/// </summary>
		/// <param name="sectionName">INI节名称</param>
		/// <param name="key">键名</param>
		/// <param name="value">要写入的值</param>
		/// <param name="filePath">INI文件完整路径</param>
		/// <returns>操作成功返回非零值，失败返回0</returns>
		[DllImport("kernel32")]
		public static extern long WritePrivateProfileString(string sectionName, string key, string value, string filePath);

		/// <summary>
		/// 从配置文件中获取指定节和键的值
		/// </summary>
		/// <param name="sectionName">节名称</param>
		/// <param name="key">键名称</param>
		/// <returns>返回获取到的值，如果发生错误则返回"发生错误"</returns>
		public static string GetValue(string sectionName, string key)
		{
			var text = AppDomain.CurrentDomain.BaseDirectory + "Data\\config.ini";
			var flag = !File.Exists(text);
			var flag2 = flag;
			var flag3 = flag2;
			var flag4 = flag3;
			var flag5 = flag4;
			var flag6 = flag5;
			if (flag6)
			{
				using (File.Create(text))
				{
				}
			}
			var array = new byte[2048];
			// 调用Windows API函数从INI配置文件中读取指定节和键的值
			// GetPrivateProfileString()参数说明:
			// sectionName: 节名称
			// key: 键名称
			// "发生错误": 默认值，当读取失败时返回此值
			// array: 用于接收返回值的字节数组缓冲区
			// 999: 缓冲区大小
			// text: INI配置文件的完整路径
			var privateProfileString = GetPrivateProfileString(sectionName, key, "发生错误", array, 999, text);
			return Encoding.Default.GetString(array, 0, privateProfileString);
		}

		/// <summary>
		/// 设置配置文件中指定节和键的值
		/// </summary>
		/// <param name="sectionName">节名称</param>
		/// <param name="key">键名称</param>
		/// <param name="value">要设置的值</param>
		/// <returns>设置成功返回true，否则返回false</returns>
		public static bool SetValue(string sectionName, string key, string value)
		{
			var text = AppDomain.CurrentDomain.BaseDirectory + "Data\\config.ini";
			var flag = !File.Exists(text);
			var flag2 = flag;
			var flag3 = flag2;
			var flag4 = flag3;
			var flag5 = flag4;
			var flag6 = flag5;
			if (flag6)
			{
				using (File.Create(text))
				{
				}
			}
			bool result;
			try
			{
				result = ((int)WritePrivateProfileString(sectionName, key, value, text) > 0);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		/// <summary>
		/// 从配置文件中移除指定节
		/// </summary>
		/// <param name="sectionName">要移除的节名称</param>
		/// <param name="filePath">配置文件路径</param>
		/// <returns>移除成功返回true，否则返回false</returns>
		public static bool RemoveSection(string sectionName, string filePath)
		{
			bool result;
			try
			{
				result = ((int)WritePrivateProfileString(sectionName, null, "", filePath) > 0);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		/// <summary>
		/// 从配置文件中移除指定节中的键
		/// </summary>
		/// <param name="sectionName">节名称</param>
		/// <param name="key">要移除的键名称</param>
		/// <param name="filePath">配置文件路径</param>
		/// <returns>移除成功返回true，否则返回false</returns>
		public static bool RemoveKey(string sectionName, string key, string filePath)
		{
			bool result;
			try
			{
				result = ((int)WritePrivateProfileString(sectionName, key, null, filePath) > 0);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}
	}
}