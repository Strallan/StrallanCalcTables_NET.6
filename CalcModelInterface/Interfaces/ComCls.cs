using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Threading;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Interop;
using System.IO;
using Strallan.Controls;
using System.Windows.Media;
using System.Xml.Linq;
using System.Text;

namespace Strallan
{
	/// <summary>
	/// ����� ���������� �������
	/// </summary>
	public class Library : IDisposable
	{
		private const string RealFmtConfig = "real.format";
		private const string EpsilonConfig = "real.epsilon";
		private const string SeparatorConfig = "separator.respect";
		private const string ChangeConfig = "undo.limit";
		private const string ControlLeftConfig = "control.left";

		/// <summary>
		/// ��������� �� ��������� �������
		/// </summary>
		private IServer server;

		/// <summary>
		/// ��������� �� ��������� ������
		/// </summary>
		private IModel model;

		/// <summary>
		/// ������ ��������
		/// </summary>
		private Config config;

		/// <summary>
		/// ������ ���������
		/// </summary>
		private ChangeLog changes;

		/// <summary>
		///  ������ ���������� ��������
		/// </summary>
		private ScriptProject project;

		/// <summary>
		/// ������� ������ �������
		/// </summary>
		[ComImport, Guid("CC3ECC3E-A6D6-4662-90E1-EEA4C28EC785")] private class Factory { }

		private Library()
		{
			server = (IServer)new Factory();
			model = server.CreateModel();
			config = new Config(server, "settings.xml");
			changes = new ChangeLog();
			project = new ScriptProject();

			InitSetting(RealFmtConfig, "6 R", (string Str) =>
			{
				var v = Str.Split(' ');
				if (v.Length == 2)
					server.SetDefaultRealFormat(".", int.Parse(v[0]), v[1]);
			}, () =>
			{
				RealSettingChanged?.Invoke();
			});

			InitSetting(EpsilonConfig, "1e-8", (string Str) =>
			{
				server.SetEpsilon(server.StringToReal(Str));
			}, () =>
			{
				RealSettingChanged?.Invoke();
			});

			InitSetting(ControlLeftConfig, "0", (string Str) =>
			{
				config[ControlLeftConfig] = Str == "1" ? "1" : "0";
			}, () =>
			{
				ControlLeftChanged?.Invoke();
			});

			InitSetting(SeparatorConfig, "1", (string Str) =>
			{
				config[SeparatorConfig] = Str == "1" ? "1" : "0";
			}, () => { });

			InitSetting(ChangeConfig, "8190", (string Str) =>
			{
				changes.Limit = int.Parse(Str);
			}, () => { });

			changes.UndoLimitChanged += (object Obj, int Value) =>
			{
				config[ChangeConfig] = Value.ToString();
			};
		}

		/// <summary>
		/// ��������� ������������� ����������
		/// </summary>
		public static void InitializeStrallanLibrary()
		{
			if (ComObject == null)
			{
				ComObject = new Library();
			}
		}

		/// <summary>
		/// ����������� ������� ����������
		/// </summary>
		public static void FinalizeStrallanLibrary()
		{
			if (ComObject != null)
			{
				(ComObject as IDisposable).Dispose();
				ComObject = null;
			}
		}

		/// <summary>
		/// ����������� ������� ����������
		/// </summary>
		void IDisposable.Dispose()
		{
			Marshal.ReleaseComObject(model);
			Marshal.ReleaseComObject(server);
			(project as IDisposable).Dispose();
			(config as IDisposable).Dispose();
		}

		/// <summary>
		/// Com-������ ����������
		/// </summary>
		public static Library ComObject { get; private set; } = null;

		/// <summary>
		/// ������ �������
		/// </summary>
		public static IServer Server => ComObject.server;

		/// <summary>
		/// ������ ������
		/// </summary>
		public static IModel Model => ComObject.model;

		/// <summary>
		/// ������ ��������
		/// </summary>
		public static Config Settings => ComObject.config;

		/// <summary>
		/// ������ ���������
		/// </summary>
		public static ChangeLog Changes => ComObject.changes;

		/// <summary>
		/// ������ ���������� ��������
		/// </summary>
		public static ScriptProject Project => ComObject.project;

		/// <summary>
		/// �������� ��������� ������������ �����
		/// </summary>
		public static double Epsilon
		{
			get => ComObject.server.GetEpsilon();
			set => ComObject.config[EpsilonConfig] = ComObject.server.RealToStringFmt(value, ".", 16, "E");
		}

		/// <summary>
		/// ������ �������������� ������������ ����� � ������
		/// </summary>
		public static (int Order, string Style) RealFormat
		{
			get
			{
				ComObject.server.GetDefaultRealFormat(out string Sep, out int Order, out string Style);
				return (Order, Style);
			}
			set
			{
				ComObject.server.SetDefaultRealFormat(".", value.Order, value.Style);
				ComObject.config[RealFmtConfig] = value.Order.ToString() + " " + value.Style;
			}
		}

		/// <summary>
		/// ����������, ��������� �� ������ ����������� ����������� ��� ����������� ������ � ����� ������
		/// </summary>
		public static bool RespectDecimalSeparator
		{
			get => ComObject.config[SeparatorConfig] == "1";
			set => ComObject.config[SeparatorConfig] = value ? "1" : "0";
		}

		/// <summary>
		/// ����������, ��� ������������ ����������� ������ ����� �� ����������� ��������
		/// </summary>
		public static bool ControlLeft
		{
			get => ComObject.config[ControlLeftConfig] == "1";
			set => ComObject.config[ControlLeftConfig] = value ? "1" : "0";
		}

		/// <summary>
		/// ������� ��������� ������������ ����������� �������
		/// </summary>
		public event Action ControlLeftChanged;

		/// <summary>
		/// �������������� �������� ���������
		/// </summary>
		/// <param name="Name">���� � ���������</param>
		/// <param name="Init">����� ������������� ��������</param>
		/// <param name="OnChange">������� ��������� ��������</param>
		/// <returns>������� �������� �������������</returns>
		private bool InitSetting(string Name, string Default, Action<string> Init, Action OnChange)
		{
			if (OnChange != null)
			{
				config.Subscribe(Name, (string a, string b, string NewValue) =>
				{
					Init(NewValue);
					OnChange();
				});
			}

			string Str = config[Name].Trim();
			if (Str == "")
				config[Name] = Str = Default;
			try
			{
				Init(Str);
				return true;
			}
			catch (Exception)
			{
				return false;
			}

		}

		/// <summary>
		/// ������� ��������� �������� ����������� ������������ �����
		/// </summary>
		public event Action RealSettingChanged;

		/// <summary>
		/// ��������� ������� ���������� ����� � �������� ��������� (������� �������)
		/// </summary>
		/// <param name="v">����������� �����</param>
		/// <param name="a">������ ������� ���������</param>
		/// <param name="b">������ ������� ���������</param>
		/// <returns>������� ���������� ����� � ���������</returns>
		public static bool InsideRange(int v, int a, int b)
		{
			return a > b ? v >= b && v <= a : v >= a && v <= b;
		}

		/// <summary>
		/// ��������� ������ �� ���������
		/// </summary>
		/// <param name="Str">������� ������</param>
		/// <param name="Func">�������, ���������� ���������� �������� ���������� ����� � ������ ������,
		/// ���������� �������� ��������� ����� ���������� ����������� ����� � ���������</param>
		public static void StringToMatrix(string Str, Func<int, int, string, bool> Func)
		{
			int p = 0, n = Str.Length, i = 0, r = 0, c = 0;
			for (; i < n; i++)
			{
				switch (Str[i])
				{
					case '\t':
						if (!Func(r, c++, Str.Substring(p, i - p)))
							return;
						p = i + 1;
						break;
					case '\r':
						if (!Func(r++, c, Str.Substring(p, i - p)))
							return;
						c = 0;
						if (i + 1 < n && Str[i + 1] == '\n')
							i++;
						p = i + 1;
						break;
					case '\n':
						if (!Func(r++, c, Str.Substring(p, i - p)))
							return;
						c = 0;
						if (i + 1 < n && Str[i + 1] == '\r')
							i++;
						p = i + 1;
						break;
				}
			}
			if (i > p)
				Func(r, c, Str.Substring(p, i - p));
		}

		/// <summary>
		/// ������ �����
		/// </summary>
		/// <param name="p">������ ����������</param>
		public static void Hole(params object[] p) { }

		/// <summary>
		/// ���������� ���������� �����������, ��������� � ������� ������ �������
		/// </summary>
		[DllImport("kernel32.dll"), PreserveSig()]
		public static extern ulong GetTickCount64();

		/// <summary>
		/// ���������� ���������� ����� ����� �������
		/// </summary>
		/// <param name="a">������ �����</param>
		/// <param name="b">������ �����</param>
		public static double Distance(Point a, Point b)
		{
			double x = (a.X - b.X), y = (a.Y - b.Y);
			return Math.Sqrt(x * x + y * y);
		}

		/// <summary>
		/// ���������� ����������� ���������� �� ����� �� ������� �������, �������� ���������
		/// </summary>
		/// <param name="a">�����</param>
		/// <param name="Width">������ �������</param>
		/// <param name="Height">������ �������</param>
		/// <returns>����������</returns>
		public static double Distance(Point a, double Width, double Height)
		{
			return Math.Min(Math.Min(a.X, Width - a.X), Math.Min(a.Y, Height - a.Y));
		}

		/// <summary>
		/// ���������� ����������� ���������� �� ����� �� ������ ��������������
		/// </summary>
		/// <param name="a">�����</param>
		/// <param name="b">�������������</param>
		/// <returns>����������</returns>
		public static double Distance(Point a, Rect b)
		{
			return Math.Min(Math.Min(a.Y - b.Top, b.Bottom - a.Y),
				Math.Min(a.X - b.Left, b.Right - a.X));
		}

		/// <summary>
		/// ���������� ����������� ���������� �� ����� �� ������ ��������������
		/// </summary>
		/// <param name="a">�����</param>
		/// <param name="Left">����� �������</param>
		/// <param name="Top">������� �������</param>
		/// <param name="Right">������ �������</param>
		/// <param name="Bottom">������ �������</param>
		/// <returns>����������</returns>
		public static double Distance(Point a, double Left, double Top, double Right, double Bottom)
		{
			return Math.Min(Math.Min(a.Y - Top, Bottom - a.Y),
				Math.Min(a.X - Left, Right - a.X));
		}

		/// <summary>
		/// ���������, ��� ������ �������� ���������� � ��������������� ��������
		/// </summary>
		/// <param name="Ch">������</param>
		/// <returns>������� ������������</returns>
		public static bool IsValidIdentChar(char Ch)
		{
			return Char.IsLetterOrDigit(Ch) || Ch == '_' || Ch == '$';
		}

		/// <summary>
		/// ���������� ������ ���� Color �� �������������� �������� � ������� RGB
		/// </summary>
		/// <param name="C">��� �����</param>
		/// <returns>������ ������ Color</returns>
		public static Color IntToColor(int C)
		{
			return Color.FromRgb((byte)(C & 0xFF),
				(byte)((C >> 8) & 0xFF),
				(byte)((C >> 16) & 0xFF));
		}

		/// <summary>
		/// ���������� ������ ���� Color �� ����� � ������� HSL
		/// </summary>
		/// <param name="Hue">������� (0..255)</param>
		/// <param name="Saturation">������������ (0..255)</param>
		/// <param name="Lightness">������������ (0..255</param>
		/// <returns></returns>
		public static Color HslToColor(int Hue, int Saturation, int Lightness)
		{
			if (Hue < 0)
				Hue = int.MaxValue + Hue;

			Hue &= 255;
			Saturation = Math.Max(0, Math.Min(Saturation, 255));
			Lightness = Math.Max(0, Math.Min(Lightness, 255));

			if (Saturation == 0) // ����� ����
				return Color.FromRgb((byte)Lightness, (byte)Lightness, (byte)Lightness);
			else // �� ����� ����
			{
				double
					H = Hue / 255.0,
					S = Saturation / 255.0,
					L = Lightness / 255.0,
					V2 = L < 0.5 ? L * (1 + S) : (L + S) - (S * L),
					V1 = 2.0 * L - V2;

				byte f(double VH)
				{
					byte Check(double V) => (byte)((V < 0.0) ? 0 : (V > 1.0 ? 255 : (int)(V * 255.0)));

					if (VH < 0) VH = VH + 1;
					if (VH > 1) VH = VH - 1;
					if (6 * VH < 1) return Check(V1 + (V2 - V1) * 6.0 * VH);
					if (2 * VH < 1) return Check(V2);
					if (3 * VH < 2) return Check(V1 + (V2 - V1) * (2.0 / 3.0 - VH) * 6.0);
					return Check(V1);
				};

				return Color.FromRgb(f(H + 1.0 / 3.0), f(H), f(H - 1.0 / 3.0));

			}
		}

		/// <summary>
		/// ���������� ������ ��������� Xml ��������� � �������� ������
		/// </summary>
		/// <param name="Root">��������� �������</param>
		/// <param name="Name">���</param>
		/// <returns>������ ��������� ���������</returns>
		public static List<XElement> FindXmlNodes(XElement Root, string Name)
		{
			List<XElement> Res = new List<XElement>();
			if (Root != null)
			{
				for (XNode i = Root.FirstNode; i != null; i = i.NextNode)
				{
					if (i.NodeType == System.Xml.XmlNodeType.Element)
					{
						XElement e = i as XElement;
						if (e.Name == Name)
						{
							Res.Add(e);
						}
					}
				}
			}
			return Res;
		}

		/// <summary>
		/// ���������� ������������ ��������� Xml ������� � �������� ������
		/// </summary>
		/// <param name="Root">��������� �������</param>
		/// <param name="Name">���</param>
		/// <returns>��������� �������</returns>
		public static XElement FindXmlNode(XElement Root, string Name)
		{
			var L = FindXmlNodes(Root, Name);
			return L.Count == 1 ? L[0] : null;
		}

		/// <summary>
		/// ���������� ��������� �������� ��������
		/// </summary>
		/// <param name="Node">Xml �������</param>
		/// <param name="Name">��� ��������</param>
		/// <param name="Default">�������� �� ���������</param>
		/// <returns>�������� ��������</returns>
		public static string FindXmlAttr(XElement Node, string Name, string Default = "")
		{
			if (Node != null)
			{
				for (XAttribute a = Node.FirstAttribute; a != null; a = a.NextAttribute)
				{
					if (a.Name == Name)
						return a.Value;
				}
			}
			return Default;
		}

		/// <summary>
		/// ���������� ������������ �������� ��������
		/// </summary>
		/// <param name="Node">Xml �������</param>
		/// <param name="Name">��� ��������</param>
		/// <param name="Default">�������� �� ���������</param>
		/// <returns>�������� ��������</returns>
		public static double FindXmlAttrDouble(XElement Node, string Name, double Default = double.NaN)
        {
			if (Node != null)
			{
				for (XAttribute a = Node.FirstAttribute; a != null; a = a.NextAttribute)
				{
					if (a.Name == Name)
					{
						if (double.TryParse(a.Value, out double v))
							return v;
					}
				}
			}
			return Default;
		}

		/// <summary>
		/// ���������� ������������� �������� ��������
		/// </summary>
		/// <param name="Node">Xml �������</param>
		/// <param name="Name">��� ��������</param>
		/// <param name="Default">�������� �� ���������</param>
		/// <returns>�������� ��������</returns>
		public static int FindXmlAttrInt(XElement Node, string Name, int Default = 0)
        {
			if (Node != null)
			{
				for (XAttribute a = Node.FirstAttribute; a != null; a = a.NextAttribute)
				{
					if (a.Name == Name)
					{
						if (int.TryParse(a.Value, out int v))
							return v;
					}
				}
			}
			return Default;
		}

		/// <summary>
		/// ���������� ���������� �������� ��������
		/// </summary>
		/// <param name="Node">Xml �������</param>
		/// <param name="Name">��� ��������</param>
		/// <param name="Default">�������� �� ���������</param>
		/// <returns>�������� ��������</returns>
		public static bool FindXmlAttrBool(XElement Node, string Name, bool Default = false)
		{
			if (Node != null)
			{
				for (XAttribute a = Node.FirstAttribute; a != null; a = a.NextAttribute)
				{
					if (a.Name == Name)
					{
						if (bool.TryParse(a.Value, out bool v))
							return v;
					}
				}
			}
			return Default;
		}



	}

	/// <summary>
	/// �����, ����������� ��������� �������� �������� ��������� ������
	/// </summary>
	class AddrList : ILongEnum
	{
		/// <summary>
		/// ������ ������� ��������
		/// </summary>
		private readonly List<long> Addr = new List<long>();

		// ������������� ����������� ������
		void ILongEnum.SetCapacity(int Capacity)
		{
			Addr.Clear();
			Addr.Capacity = Capacity;
		}

		/// <summary>
		/// �������� ����� � ������
		/// </summary>
		int ILongEnum.Enum(long N)
		{
			Addr.Add(N);
			return 0;
		}

		/// <summary>
		/// ���������� ������� ������
		/// </summary>
		public int Count => Addr.Count;

		/// <summary>
		/// ���������� ����� �� �����
		/// </summary>
		/// <param name="Index">������� � ������</param>
		/// <returns>�����</returns>
		public long this[int Index] => Addr[Index];

		/// <summary>
		/// ����������� ������ � ������ ����� �����.
		/// </summary>
		/// <returns></returns>
		public long[] ToArray() => Addr.ToArray();
	};

    /// <summary>
    /// ����� ������� ���������� ������������ ��������
    /// </summary>
    class FinishEvent : IFinishEvent
    {
        /// <summary>
        /// ������� ���������� ������������ ��������
        /// </summary>
        private readonly Action<int, string> Finish;

        /// <summary>
        /// �������� �������������
        /// </summary>
        private readonly SynchronizationContext SyncCtx;

        /// <summary>
        /// ������ ������ �������
        /// </summary>
        /// <param name="finish">������� ���������� ������������ ��������</param>
        public FinishEvent(Action<int, string> finish)
        {
            Finish = finish;
            SyncCtx = SynchronizationContext.Current;
        }

        void IFinishEvent.OnFinish(int Code, string Message)
        {
            SyncCtx.Post(_ =>
            {
                Finish?.Invoke(Code, Message);
            }, this);
        }
    }
		
	/// <summary>
    /// ����� �������� ����������
    /// </summary>
    public class Config : IDisposable, ISettingClient
    {
        /// <summary>
        /// ������ ������ ��������
        /// </summary>
        /// <param name="Server">��������� �� ��������� ������� Strallan</param>
        /// <param name="AppName">��� ����������</param>
        /// <param name="AppVersion">������ ����������</param>
        /// <param name="AppGuid">GUID ����������</param>
        /// <param name="FileName">��� ����� ��������</param>
        /// <param name="Local">������� ���������� ����� �������� � ��������� ������� ������������</param>
        public Config(IServer Server,
            string AppName, string AppVersion, string AppGuid, string FileName, bool Local = true)
        {
            Srv = Server;
            SyncCtx = SynchronizationContext.Current;
            Cfg = Server.LoadSettings(AppName, AppVersion, AppGuid, FileName, Local ? 1 : 0);
            Cfg.SetMessageClient(this);
        }

        /// <summary>
        /// ������ ������ �������� � �������������� ������ Assembly Information
        /// </summary>
        /// <param name="Server">��������� �� ��������� ������� Strallan</param>
        /// <param name="FileName">��� ����� ��������</param>
        /// <param name="Local">������� ���������� ����� �������� � ��������� ������� ������������</param>
        public Config(IServer Server, string FileName, bool Local = true)
        {
            var asm = Assembly.GetExecutingAssembly();
            var AppName = asm.GetName().Name;
            var ver = asm.GetName().Version;
            string AppVersion = ver.Major + "." + ver.Minor;
			var AppGuid = asm.GetCustomAttribute<GuidAttribute>().Value.ToUpper();
			//var AppGuid = "1111111";
			FileName = "Version " + AppVersion + "\\" + FileName;
            typeof(Config).GetConstructors()[0].Invoke(this, new object[] { Server, AppName, AppVersion, AppGuid, FileName, Local });
			ProgVersion = AppName + " " + AppVersion;
		}

		/// <summary>
		/// ��� � ������ ����������
		/// </summary>
		public string ProgVersion { get; private set; }


		/// <summary>
		/// �������� ��������� ��������
		/// </summary>
		/// <param name="Path">���� � ��������� (����� ��������� ���������� ����������� �������)</param>
		/// <returns>�������� ��������� ��������</returns>
		public string this[string Path]
        {
            get => Cfg.GetValue(Path);
            set => Cfg.SetValue(Path, value);
        }

		// ����������� ������ ���������� � ������ � ������������� - �������
		private static string ParamsToPath(params string[] Params)
		{
			for (int i = 1, n = Params.Length; i < n; i++)
				Params[0] += "." + Params[i];
			return Params[0];
		}

		/// <summary>
		/// �������� ��������� ��������
		/// </summary>
		/// <param name="Path">���� � ��������� (������������������ ���)</param>
		/// <returns>�������� ��������� ��������</returns>
		public string this[params string[] Params]
		{
			get => Cfg.GetValue(ParamsToPath(Params));
			set => Cfg.SetValue(ParamsToPath(Params), value);
		}

        /// <summary>
        /// ���������� ������������� �������� ���������
        /// </summary>
        /// <param name="Path">���� � ���������</param>
        /// <param name="Default">�������� �� ���������</param>
        /// <returns>�������� ���������</returns>
        public int GetInt(string Path, int Default = 0)
        {
            string Str = Cfg.GetValue(Path).Trim();
            if (int.TryParse(Str, out int v))
                return v;
            else
                return Default;
        }

		/// <summary>
		/// ��� ������� ��������� �������� ��������� ��������
		/// </summary>
		/// <param name="KeyName">��� ���������</param>
		/// <param name="OldValue">������ ��������</param>
		/// <param name="NewValue">����� ��������</param>
		public delegate void OnValueChangedEventType(string KeyName, string OldValue, string NewValue);

        /// <summary>
        /// ��� ������� ������� ����� ����� ����������
        /// </summary>
        /// <param name="Handle">���������� ����������</param>
        /// <param name="Stamp">��������� ����� ����������</param>
        public delegate void OnNewInstanceEventType(long Handle, long Stamp);

        /// <summary>
        /// ��� ������� ��������� ��������� �� ������ ����� ����������
        /// </summary>
        /// <param name="From">���������� ����������-���������</param>
        /// <param name="Msg">����� ���������</param>
        /// <param name="Reply">��� ������</param>
        public delegate void OnMessageEventType(long From, string Msg, out long Reply);

        /// <summary>
        /// ������������� �� ��������� ���� ��������� ���������� ���������� ���������
        /// </summary>
        /// <param name="Key">��� ���������</param>
        /// <param name="Func">������� ��������� �������� ���������</param>
        /// <returns>��� ������ ��� ������ �� ��������</returns>
        public long Subscribe(string Key, OnValueChangedEventType Func) => Cfg.Subscribe(Key, new ConfigClient(Func, SyncCtx));

        /// <summary>
        /// ������� �������� �� ������� ��������� �������� ���������
        /// </summary>
        /// <param name="Key">��� ���������</param>
        /// <param name="Code">��� ������, ��������� ��� ������ Subscribe</param>
        public void Unsubscribe(string Key, long Code) => Cfg.Unsubscribe(Key, Code);

        /// <summary>
        /// ������� ������� ����� ����� ����������
        /// </summary>
        public event OnNewInstanceEventType OnNewInstanceEvent;

        /// <summary>
        /// ������� ��������� ��������� �� ������������ ����� ����������
        /// </summary>
        public event OnMessageEventType OnMessageEvent;

        /// <summary>
        /// ��������� ��������� ���������� ��������� (�� ������������ ����� �������� ����������)
        /// </summary>
        /// <param name="Key">��� ���������</param>
        /// <returns>������� ����������� ���������</returns>
        public bool IsProvisional(string Key) => Cfg.GetProvisional(Key) != 0;

        /// <summary>
        /// ������������� ��������� ����������
        /// </summary>
        /// <param name="Key">��� ���������</param>
        /// <param name="Flag">������� �����������</param>
        public void SetProvisional(string Key, bool Flag) => Cfg.SetProvisional(Key, Flag ? 1 : 0);

        /// <summary>
        /// ��������� ��������� ���������� ��������� (������� ��������� �������� �� ���������� ������ ������ ����������)
        /// </summary>
        /// <param name="Key">��� ���������</param>
        /// <returns>������� ����������� ���������</returns>
        public bool IsLocal(string Key) => Cfg.GetLocal(Key) != 0;

        /// <summary>
        /// ������������� ��������� ���������� ���������
        /// </summary>
        /// <param name="Key">��� ���������</param>
        /// <param name="Flag">������� �����������</param>
        public void SetLocal(string Key, bool Flag) => Cfg.SetLocal(Key, Flag ? 1 : 0);

        /// <summary>
        /// ������������� ��������� ��������� � ����
        /// </summary>
        public void Dump() => Cfg.Dump();

        /// <summary>
        /// ��������������� ��������� �� �����
        /// </summary>
        public void Reload() => Cfg.Reload();

        /// <summary>
        /// ���������� ������ �������� ����� ����������
        /// </summary>
        /// <returns>������ �������� ����� (���������� � ��������� �����)</returns>
        public (long Handle, long Stamp)[] GetActiveInstances()
        {
            List<(long Handle, long Stamp)> L = new List<(long Handle, long Stamp)>();

            var S = Cfg.GetInstanceList().Split(' ');
            for (int i = 0; i < S.Length; i += 2)
                L.Add((Int64.Parse(S[i]), Int64.Parse(S[i + 1])));

            return L.ToArray();
        }

        /// <summary>
        /// �������� ��������� ���� �������� ������ ����������
        /// </summary>
        /// <param name="Msg">����� ���������</param>
        /// <returns>������ ���������� ����� (����������, ��������� �����, ��� ������)</returns>
        public (long Handle, long Stamp, long Reply)[] BroadcastMessage(string Msg)
        {
            List<(long Handle, long Stamp, long Reply)> L = new List<(long Handle, long Stamp, long Reply)>();

            var S = Cfg.BroadcastMessage(Msg).Split(' ');
            for (int i = 0; i < S.Length; i += 3)
                L.Add((Int64.Parse(S[i]), Int64.Parse(S[i + 1]), Int64.Parse(S[i + 2])));

            return L.ToArray();
        }

        /// <summary>
        /// �������� ��������� ������ ����� ����������
        /// </summary>
        /// <param name="Handle">���������� ����������</param>
        /// <param name="Msg">����� ���������</param>
        public long SendMessage(long Handle, string Msg) => Cfg.SendMessage(Handle, Msg);

        /// <summary>
        /// ��������� ������� ����
        /// </summary>
        /// <param name="Key">��� ��������� ����������</param>
        /// <param name="Wnd">���� ����������</param>
        public void SaveWindowPlacement(string Key, Window Wnd) =>
            this[Key] = Srv.GetWindowPlacement((long)(new WindowInteropHelper(Wnd).Handle));

        /// <summary>
        /// ��������������� ������� ����. ���� ���� ��������� ��� �������� ��������� ���
        /// ������, ��� ����� ��������� ���������� � ������� �������
        /// </summary>
        /// <param name="Key">��� ��������� ����������</param>
        /// <param name="Wnd">���� ����������</param>
        public void RestoreWindowPlacement(string Key, Window Wnd)
        {
            SetLocal(Key, true);
            Srv.AdjustWindowPlacement(this[Key], out int Left, out int Top, out int Width, out int Height, out int State);
            Wnd.Left = Left;
            Wnd.Top = Top;
            Wnd.Width = Width;
            Wnd.Height = Height;
            Wnd.WindowState = (WindowState)State;
        }

        // ����� ���������� IDisposable
        void IDisposable.Dispose()
        {
            Cfg.SetMessageClient(null);
            Marshal.ReleaseComObject(Cfg);
        }

        // ��������� �� ��������� ������������
        private readonly ISettings Cfg;

        // ��������� �� �������� �������
        private readonly IServer Srv;


        // ��������� �� �������� �������������
        private readonly SynchronizationContext SyncCtx;

        // ������ ���������� ISettingClient

        void ISettingClient.OnSettingChanged(string KeyName, string OldValue, string NewValue) { }

        void ISettingClient.OnNewInstance(long Handle, long Stamp)
        {
            if (OnNewInstanceEvent != null)
            {
                SyncCtx.Post(_ =>
                {
                    OnNewInstanceEvent(Handle, Stamp);
                }, this);
            }
        }

        long ISettingClient.OnMessage(long From, string Msg)
        {
            long Result = 0;
            if (OnMessageEvent != null)
            {
                SyncCtx.Send(_ =>
                {
                    OnMessageEvent(From, Msg, out Result);
                }, this);
            }
            return Result;
        }


        // ���������� ���������� ���������� �� �������
        private class ConfigClient : ISettingClient
        {
            // ������� ��������� �������� �����
            private readonly OnValueChangedEventType OnValueChanged;

            // �������� �������������
            private readonly SynchronizationContext SyncCtx;

            // ������ ������
            public ConfigClient(OnValueChangedEventType Func, SynchronizationContext Ctx)
            {
                OnValueChanged = Func;
                SyncCtx = Ctx;
            }

            // ������ ���������� IConfigClient
            void ISettingClient.OnSettingChanged(string KeyName, string OldValue, string NewValue)
            {
                if (OnValueChanged != null)
                {
                    SyncCtx.Post(_ =>
                    {
                        OnValueChanged(KeyName, OldValue, NewValue);
                    }, this);
                }
            }

            void ISettingClient.OnNewInstance(long Handle, long Stamp) { }

            long ISettingClient.OnMessage(long From, string Msg) => 0;

        }
    }

	/// <summary>
	/// ����� �������� �����
	/// </summary>
	public class EnumStrings : IStringEnum
	{
		/// <summary>
		/// ������ �����
		/// </summary>
		private readonly List<string> Strings = new List<string>();

		void IStringEnum.SetCapacity(int Capacity)
		{
			Strings.Capacity = Capacity;
		}

		int IStringEnum.Enum(string Str)
		{
			Strings.Add(Str);
			return 0;
		}

		/// <summary>
		/// ���������� ������ ������
		/// </summary>
		/// <param name="Index">����� ������</param>
		/// <returns>����� ������</returns>
		public string this[int Index] => Strings[Index];

		/// <summary>
		/// ���������� ����� ������
		/// </summary>
		public int Count => Strings.Count;

		/// <summary>
		/// ��������� ������ 
		/// </summary>
		public void Sort() => Strings.Sort();

		public int IndexOf(string Str)
		{
			return Strings.FindIndex((string S) =>
			{
				return Str.Equals(S, StringComparison.OrdinalIgnoreCase);
			});
		}

	}

    /// <summary>
    /// ����� ������� ���������
    /// </summary>
    public class ChangeLog
    {
        /// <summary>
        /// ������ ������ ���������
        /// </summary>
        public ChangeLog()
        {
            UndoQueue = new List<IRecord>();
            RedoQueue = new List<IRecord>();
            Stack = new Stack<List<IRecord>>();
            Stack.Push(UndoQueue);
        }

        /// <summary>
        /// �������� ������ �������, �������������� �� ���� ������� ������/��������
        /// </summary>
        /// <returns>true � ������ ������, false � ������ ������������</returns>
        public bool BeginGroup()
        {
            if (LockCount == 0)
            {
                RedoQueue.Clear();
                var Rec = new Group(out List<IRecord> Queue);
                Stack.Push(Queue);
                UndoQueue.Add(Rec);
                return true;
            }
            return false;
        }

        /// <summary>
        /// ������ � ������ ������ �� ����������
        /// </summary>
        /// <param name="Undo">����� ������ ���������</param>
        /// <param name="Redo">����� �������� ������</param>
        /// <returns>true � ������ �������, false � ������ ������������</returns>
        public bool Log(Action Undo, Action Redo)
        {
            if (LockCount == 0)
            {
                RedoQueue.Clear();
                Stack.Peek().Add(new Record(Undo, Redo));
                ApplyLimit();
                return true;
            }
            return false;
        }

        /// <summary>
        /// ��������� ������ �������. ����� ������ ���� ������ � ����� ��������� �����
        /// ������ BeginGroup, ���������� true
        /// </summary>
        public void EndGroup()
        {
            if (LockCount == 0 && Stack.Count > 1)
                Stack.Pop();
        }

        /// <summary>
        /// ��������� ����������� ������ ������ ������ ���������
        /// </summary>
        public bool CanUndo => UndoQueue.Count > 0 && Stack.Count == 1;

        /// <summary>
        /// ����� ������ ���������. ����� ���� ������ ������ ����� CanUndo, ���������� true
        /// </summary>
        public void Undo()
        {
            LockCount++;
            try
            {
                var n = UndoQueue.Count - 1;
                var Rec = UndoQueue[n];
                Rec.Undo();
                RedoQueue.Add(Rec);
                UndoQueue.RemoveAt(n);
            }
            finally
            {
                LockCount--;
            }
        }

        /// <summary>
        /// ��������� ����������� ������ ������ �������� ������
        /// </summary>
        public bool CanRedo => RedoQueue.Count > 0 && Stack.Count == 1;

        /// <summary>
        /// ����� ������ ��������. ����� ���� ������ ������ ����� CanRedo, ���������� true
        /// </summary>
        public void Redo()
        {
            LockCount++;
            try
            {
                var n = RedoQueue.Count - 1;
                var Rec = RedoQueue[n];
                Rec.Redo();
                UndoQueue.Add(Rec);
                RedoQueue.RemoveAt(n);
            }
            finally
            {
                LockCount--;
            }
        }

        /// <summary>
        /// ���������, ��� ������ �� ������������ � ����� ��������� ������
        /// </summary>
        public bool IsUnlocked => LockCount == 0;

        /// <summary>
        /// ����������� �� ���������� ������� � �������. ��� ��������������� �������� ������ �������� �����������
        /// </summary>
        public int Limit
        {
            get => QueueLimit;
            set
            {
				if (value != QueueLimit)
				{
					QueueLimit = value;
					ApplyLimit();
					UndoLimitChanged?.Invoke(this, value);
				}
            }
        }

        /// <summary>
        /// ������� ������
        /// </summary>
        public void Clear()
        {
            UndoQueue.Clear();
            RedoQueue.Clear();
            Stack.Clear();
            Stack.Push(UndoQueue);
        }

		/// <summary>
		/// ������� ��������� ����������� ���������� �������
		/// </summary>
		public event Action<object, int> UndoLimitChanged;

        /// <summary>
        /// ������� ���������� �������
        /// </summary>
        private int LockCount = 0;

        /// <summary>
        /// ����������� �� ���������� ������� � ������
        /// </summary>
        private int QueueLimit = 0;

        /// <summary>
        /// ��������� ����������� ���������� �������, ������ ������ �� �������
        /// </summary>
        private void ApplyLimit()
        {
            if (QueueLimit > 0 && UndoQueue.Count > QueueLimit)
                    UndoQueue.RemoveRange(0, UndoQueue.Count - QueueLimit);
        }

		/// <summary>
		/// ��������� ������
		/// </summary>
		public void Lock()
		{
			LockCount++;
		}

		/// <summary>
		/// ������������ ������
		/// </summary>
		public void Unlock()
		{
			LockCount--;
		}

        /// <summary>
        /// ��������� ������ �������
        /// </summary>
        private interface IRecord
        {
            void Undo();
            void Redo();
        }

        /// <summary>
        /// ������� ������ ���������
        /// </summary>
        private readonly List<IRecord> UndoQueue;

        /// <summary>
        /// ������� �������� ������
        /// </summary>
        private readonly List<IRecord> RedoQueue;

        /// <summary>
        /// ���� �������� ������
        /// </summary>
        private readonly Stack<List<IRecord>> Stack;

        /// <summary>
        /// ����� ������ �������
        /// </summary>
        private class Record : IRecord
        {
            /// <summary>
            /// ����� ������ ���������
            /// </summary>
            private readonly Action UndoAction;

            /// <summary>
            /// ����� �������� ���������
            /// </summary>
            private readonly Action RedoAction;

            /// <summary>
            /// ������ ������
            /// </summary>
            /// <param name="undo">����� ������</param>
            /// <param name="redo">����� ��������</param>
            public Record(Action undo, Action redo)
            {
                UndoAction = undo;
                RedoAction = redo;
            }
            void IRecord.Redo() => RedoAction();

            void IRecord.Undo() => UndoAction();
        }

        /// <summary>
        /// ����� ������ ���������
        /// </summary>
        private class Group : IRecord
        {
            /// <summary>
            /// ������ ������� ������
            /// </summary>
            private readonly List<IRecord> Queue;

            /// <summary>
            /// ������ ��������� ������
            /// </summary>
            /// <param name="queue">������� ������� ������</param>
            public Group(out List<IRecord> queue) => queue = Queue = new List<IRecord>();

            void IRecord.Redo()
            {
                for (int i = 0, n = Queue.Count; i < n; i++)
                    Queue[i].Redo();
            }

            void IRecord.Undo()
            {
                for (int i = Queue.Count - 1; i >= 0; i--)
                    Queue[i].Undo();
            }
        }
    }

	/// <summary>
	/// ����� ������ ���������� �� ���������� ����������
	/// </summary>
	public class ScriptProject : IScriptClient, IDisposable
	{
		/// <summary>
		/// ������ ����������
		/// </summary>
		private readonly List<EditControl> Editors =
			new List<EditControl>();

		void IDisposable.Dispose()
		{
			foreach (var Editor in Editors)
			{
				if (Editor != null)
					(Editor as IDisposable).Dispose();
			}
		}

		/// <summary>
		/// ���������� ������ �� �������
		/// </summary>
		/// <param name="Index">������</param>
		/// <returns>������</returns>
		public EditControl this[int Index] => Editors[Index];

		/// <summary>
		/// ���������� ������ ��� �����
		/// </summary>
		/// <param name="FileName">������������� ���</param>
		/// <returns>���������� ���</returns>
		private string FullName(string FileName)
		{
			var f = new FileInfo(FileName);
			return f.FullName;
		}

		/// <summary>
		/// ���������� ������ ������� �� ����� �����
		/// </summary>
		/// <param name="FileName">��� �����</param>
		/// <returns>������ ������� (-1 ���� ���� �� ������)</returns>
		public int IndexOf(string FileName)
		{
			int Index = 0;
			FileName = FullName(FileName);
			foreach (var Editor in Editors)
			{
				var Str = FullName(Editor.Editor.GetFileName(out double FileTime));
				if (Str.Equals(FileName, StringComparison.OrdinalIgnoreCase))
					return Index;
				Index++;
			}
			return -1;
		}


		/// <summary>
		/// ������ ����� ������ ���������
		/// </summary>
		/// <returns>������ ���������</returns>
		public EditControl NewEditor()
		{
			EditControl E = new EditControl(Library.Server.CreateEditor());
			E.EditorRequest += EditorRequest;
			E.DisplayRequest += EditorDisplayRequest;
			E.Changed += Changed;
			CreateEditor?.Invoke(this, E);

			for (int i = 0, n = Editors.Count; i < n; i++)
			{
				if (Editors[i] == null)
					return Editors[i] = E;
			}
			Editors.Add(E);
			return E;
		}

		/// <summary>
		/// ������� �������� ���������
		/// </summary>
		/// <param name="obj"></param>
		private void Changed(EditControl obj)
		{
			EditorChanged?.Invoke(obj);
		}

		/// <summary>
		/// ������� ������� �� ����� ���������
		/// </summary>
		private void EditorDisplayRequest(EditControl obj)
		{
			DisplayRequest?.Invoke(obj);
		}

		/// <summary>
		/// ������� ������� ��������� �� ��������� ��������� �� ����� �����
		/// </summary>
		private void EditorRequest(string FileName, ref IScriptEditor Editor)
		{
			var E = CheckFile(FileName, true, false);
			if (E != null)
				Editor = E.Editor;
		}

		/// <summary>
		/// ���������� ������ ��������� �� ����� �����
		/// </summary>
		/// <param name="FileName">��� �����</param>
		/// <param name="Open">������� ������������� ������� ����</param>
		/// <param name="Create">������� ������������� ������� ����� ����</param>
		/// <returns>������ ���������</returns>
		public EditControl CheckFile(string FileName, bool Open, bool Create)
		{
			FileName = FileName.Trim();
			if (FileName == "")
			{
				return Open && Create ? NewEditor() : null;
			}

			FileName = FullName(FileName);
			foreach (var Editor in Editors)
			{
				var Str = FullName(Editor.Editor.GetFileName(out double FileTime));
				if (Str.Equals(FileName, StringComparison.OrdinalIgnoreCase))
					return Editor;
			}

			if (Open)
			{
				if (File.Exists(FileName))
				{
					var E = NewEditor();
					E.Editor.LoadFromFile(FileName, 0);
					return E;
				}
				else if (Create)
				{
					var E = NewEditor();
					E.Editor.LoadFromFile(FileName, 1);
					return E;
				}
			}
			return null;
		}

		/// <summary>
		/// ��������� ����
		/// </summary>
		/// <param name="Index">������ ���������</param>
		public void CloseFile(int Index)
		{
			if (Editors[Index] != null)
			{
				(Editors[Index] as IDisposable).Dispose();
				Editors[Index] = null;
			}
		}

		/// <summary>
		/// ��������� ����
		/// </summary>
		/// <param name="FileName">��� �����</param>
		public void CloseFile(string FileName)
		{
			var Index = IndexOf(FileName);
			if (Index >= 0)
				CloseFile(Index);
		}

		/// <summary>
		/// ��������� ����
		/// </summary>
		/// <param name="Editor">��������� ���������</param>
		public void CloseFile(EditControl Editor)
		{
			for (int i = 0, n = Editors.Count; i < n; i++)
			{
				if (Editors[i] == Editor)
				{
					CloseFile(i);
					break;
				}
			}
		}

		/// <summary>
		/// �������� �������������
		/// </summary>
		private readonly SynchronizationContext SyncCtx = SynchronizationContext.Current;

		/// <summary>
		/// ������� �������� ������� ���������
		/// </summary>
		public event Action<ScriptProject, EditControl> CreateEditor;

		/// <summary>
		/// ������� ������� �� ����������� ���������
		/// </summary>
		public event Action<EditControl> DisplayRequest;

		/// <summary>
		/// ������� ��������� ��������� ���������
		/// </summary>
		public event Action<EditControl> EditorChanged;

		/// <summary>
		/// ������� ������ ������ �������
		/// </summary>
		public event Action Start;

		/// <summary>
		/// ������� ������ �������
		/// </summary>
		public bool IsRunning { get; private set; } = false;

		/// <summary>
		/// ������� ��������� ������� �������
		/// </summary>
		public event Action EndParsing;

		/// <summary>
		/// ������� ������ ������ � ������
		/// </summary>
		public event Action<string> StartLog;

		/// <summary>
		/// ������� ������ � ������
		/// </summary>
		public event Action<string> WriteLog;

		/// <summary>
		/// ������� ��������� ������ � ������
		/// </summary>
		public event Action EndLog;

		/// <summary>
		/// ������� ������ � ����� ������
		/// </summary>
		public event Action<string> WriteOutput;

		/// <summary>
		/// ������� ������ �������� ���������� �������� ��������
		/// </summary>
		public event Action StartWaiting;

		/// <summary>
		/// ������� ��������� �������� ���������� �������� ��������
		/// </summary>
		public event Action EndWaiting;

		/// <summary>
		/// ������� �������� ���������� �������� ��������
		/// </summary>
		public bool IsWaiting { get; private set; } = false;

		/// <summary>
		/// ������� ������������ ���������� �������
		/// </summary>
		public event Action<int, int, string> Suspended;

		/// <summary>
		/// ������� ������������� ���������� �������
		/// </summary>
		public event Action Resumed;

		/// <summary>
		/// ������� ���������������� ������ �������
		/// </summary>
		public bool IsSuspended { get; private set; } = false;

		/// <summary>
		/// ������� ������ ������ ��������������� ������
		/// </summary>
		public event Action StartAnalysis;

		/// <summary>
		/// ������� ���������� ������ ��������������� ������
		/// </summary>
		public event Action EndAnalysis;

		/// <summary>
		/// ������� ������ ��������������� ������
		/// </summary>
		public bool IsPerformingAnalysis { get; private set; } = false;

		/// <summary>
		/// ������� ���������� ������ �������
		/// </summary>
		public event Action Finish;

		/// <summary>
		/// ������� ������ �����������
		/// </summary>
		public bool TraceMode { get; set; } = false;

		void IScriptClient.OnStart()
		{
			SyncCtx.Send(_ =>
			{
				IsRunning = true;
				Start?.Invoke();
			}, this);
		}

		void IScriptClient.OnEndParsing()
		{
			SyncCtx.Send(_ =>
			{
				EndParsing?.Invoke();
			}, this);
		}

		void IScriptClient.OnError(int Line, int Pos, string FileName, int Code, string Message)
		{
			SyncCtx.Send(_ =>
			{
				var e = CheckFile(FileName, true, false);
				e.Editor.DisplayError(Line, Pos);
				MessageBox.Show(Application.Current.MainWindow, Message, Library.Settings.ProgVersion,
					MessageBoxButton.OK, MessageBoxImage.Error);
			}, this);
		}

		int IScriptClient.DisplayMessage(string Message, int Type)
		{
			MessageBoxResult Res = 0;
			SyncCtx.Send(_ =>
			{
				Res = MessageBox.Show(Application.Current.MainWindow, Message, Library.Settings.ProgVersion,
					(MessageBoxButton)(Type & 0x0F), (MessageBoxImage)(Type & 0xF0));

			}, this);

			return (int)Res | (1 << 31);

		}

		void IScriptClient.OnStartLog(string FileName)
		{
			SyncCtx.Send(_ =>
			{
				StartLog?.Invoke(FileName);
			}, this);
		}

		void IScriptClient.OnWriteLog(string Message)
		{
			SyncCtx.Send(_ =>
			{
				WriteLog?.Invoke(Message);
			}, this);
		}

		void IScriptClient.OnEndLog()
		{
			SyncCtx.Send(_ =>
			{
				EndLog?.Invoke();
			}, this);
		}

		void IScriptClient.OnWrite(string Text)
		{
			SyncCtx.Send(_ =>
			{
				WriteOutput?.Invoke(Text);
			}, this);
		}

		void IScriptClient.OnStartWaiting()
		{
			SyncCtx.Send(_ =>
			{
				IsWaiting = true;
				StartWaiting?.Invoke();
			}, this);
		}

		void IScriptClient.OnEndWaiting()
		{
			SyncCtx.Send(_ =>
			{
				EndWaiting?.Invoke();
				IsWaiting = false;
			}, this);
		}

		int IScriptClient.OnDebugTrace(int Line, int Pos, string FileName)
		{
			int Res = 0;
			SyncCtx.Send(_ =>
			{
				if ((Library.Model.GetScriptState() & ScriptState.Terminated) != 0)
					Res = 1;
				else
				{
					var e = CheckFile(FileName, true, false);
					if (TraceMode || e.Editor.IsBreakpoint(Line, 0) != 0)
						Res = 2;
				}

			}, this);
			return Res;
		}

		void IScriptClient.OnSuspended(int Line, int Pos, string FileName)
		{
			SyncCtx.Send(_ =>
			{
				IsSuspended = true;
				Suspended?.Invoke(Line, Pos, FileName);
				var e = CheckFile(FileName, true, false);
				e.Editor.DisplayTrace(Line, Pos);
			}, this);
		}

		void IScriptClient.OnResumed()
		{
			SyncCtx.Send(_ =>
			{
				Resumed?.Invoke();
				IsSuspended = false;
			}, this);
		}

		void IScriptClient.OnStartAnalysis()
		{
			SyncCtx.Send(_ =>
			{
				IsPerformingAnalysis = true;
				StartAnalysis?.Invoke();
			}, this);
		}

		void IScriptClient.OnEndAnalysis()
		{
			SyncCtx.Send(_ =>
			{
				EndAnalysis?.Invoke();
				IsPerformingAnalysis = false;
			}, this);
		}

		void IScriptClient.OnFinish()
		{
			SyncCtx.Send(_ =>
			{
				Finish?.Invoke();
				IsRunning = false;
			}, this);
		}
	}

}