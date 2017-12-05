using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

[StandardModule]
internal sealed class Class120
{
	internal enum Enum11
	{

	}

	internal static Class120.Enum11 enum11_0;

	private static Class323.Class325<string> class325_0 = new Class323.Class325<string>(100000);

	private static Class323.Class325<string> class325_1 = new Class323.Class325<string>(100000);

	private static Class323.Class325<string> class325_2 = new Class323.Class325<string>(100000);

	private static Class323.Class325<string> class325_3 = new Class323.Class325<string>(100000);

	private static Class323.Class325<string> class325_4 = new Class323.Class325<string>(100000);

	internal static string string_0 = string.Empty;

	private static void smethod_0(Class323.Class325<string> class325_5, string string_1)
	{
		checked
		{
			using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("WindowsApplication1." + string_1 + ".bin"))
			{
				byte[] array = new byte[(int)(manifestResourceStream.Length - 1L) + 1];
				manifestResourceStream.Read(array, 0, array.Length);
				int num = array.Length / 16 - 1;
				for (int i = 0; i <= num; i++)
				{
					StringBuilder stringBuilder = new StringBuilder(32);
					int num2 = 0;
					do
					{
						stringBuilder.AppendFormat("{0:X2}", array[i * 16 + num2]);
						num2++;
					}
					while (num2 <= 15);
					class325_5.method_1(stringBuilder.ToString().ToLower());
				}
			}
		}
	}

	internal static void smethod_1()
	{
		Class120.smethod_0(Class120.class325_0, "_NORMAL");
		Class120.smethod_0(Class120.class325_1, "_TECH");
		RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\ZAR\\X", false);
		if (registryKey == null || registryKey.GetValue("kkk") == null)
		{
			registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\ZAR", false);
		}
		Class120.string_0 = string.Empty;
		try
		{
			Class120.string_0 = Conversions.ToString(registryKey.GetValue("kkk"));
		}
		catch (Exception arg_77_0)
		{
			ProjectData.SetProjectError(arg_77_0);
			Class120.string_0 = string.Empty;
			ProjectData.ClearProjectError();
		}
		Class120.smethod_5(Class120.string_0);
	}

	private static void smethod_2(Class323.Class325<string> class325_5, string string_1)
	{
		StreamWriter streamWriter = new StreamWriter(string_1);
		try
		{
			IEnumerator<string> enumerator = class325_5.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				streamWriter.WriteLine(current);
			}
		}
		finally
		{
			IEnumerator<string> enumerator;
			if (enumerator != null)
			{
				enumerator.Dispose();
			}
		}
		streamWriter.Close();
	}

	internal static string smethod_3(Class120.Enum11 enum11_1)
	{
		string result;
		switch (enum11_1)
		{
		case (Class120.Enum11)1:
			result = "Single user license";
			break;
		case (Class120.Enum11)2:
			result = "Technician edition";
			break;
		case (Class120.Enum11)3:
			result = "Standard license";
			break;
		case (Class120.Enum11)4:
			result = "Standard Plus license";
			break;
		case (Class120.Enum11)5:
			result = "Professional license";
			break;
		default:
			result = "Limited demo version";
			break;
		}
		return result;
	}

	internal static Class120.Enum11 smethod_4(string string_1)
	{
		Class120.Enum11 @enum = (Class120.Enum11)0;
		string_1 = string_1.ToUpper().Trim();
		string_1 += "-";
		checked
		{
			Class120.Enum11 result;
			if (string_1.Length != 30)
			{
				result = @enum;
			}
			else
			{
				int num = 0;
				while (true)
				{
					IL_9E:
					int num2 = 0;
					while (Class323.smethod_13((byte)Strings.Asc(string_1[num * 6 + num2])) | Versioned.IsNumeric(Strings.Asc(string_1[num * 6 + num2])))
					{
						num2++;
						if (num2 > 4)
						{
							if (Operators.CompareString(Conversions.ToString(string_1[num * 6 + 5]), "-", false) != 0)
							{
								goto IL_A9;
							}
							num++;
							if (num <= 4)
							{
								goto IL_9E;
							}
							goto IL_B0;
						}
					}
					break;
				}
				result = @enum;
				return result;
				IL_A9:
				result = @enum;
				return result;
				IL_B0:
				string_1 = Strings.Left(string_1, string_1.Length - 1);
				MD5 mD = MD5.Create();
				byte[] array = mD.ComputeHash(Encoding.ASCII.GetBytes(string_1));
				string text = string.Empty;
				int num3 = array.Length - 1;
				for (int i = 0; i <= num3; i++)
				{
					text += array[i].ToString("X2");
				}
				text = text.ToLower();
				if (Class120.class325_0.method_0(text))
				{
					result = (Class120.Enum11)1;
				}
				else if (Class120.class325_1.method_0(text))
				{
					result = (Class120.Enum11)2;
				}
				else
				{
					result = @enum;
				}
			}
			return result;
		}
	}

	public static bool smethod_5(string string_1)
	{
		bool flag = false;
		Class120.enum11_0 = (Class120.Enum11)0;
		bool result;
		if (string_1 == null || string_1.Length == 0)
		{
			result = flag;
		}
		else
		{
			Class120.enum11_0 = Class120.smethod_4(string_1);
			if (Class120.enum11_0 == (Class120.Enum11)0)
			{
				result = flag;
			}
			else
			{
				flag = true;
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\ZAR\\X", true);
				if (registryKey == null)
				{
					registryKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\ZAR\\X");
				}
				registryKey.SetValue("kkk", string_1);
				result = flag;
			}
		}
		return result;
	}
}
