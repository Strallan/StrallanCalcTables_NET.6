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
	/// Класс библиотеки сервера
	/// </summary>
	public class Library : IDisposable
	{
		private const string RealFmtConfig = "real.format";
		private const string EpsilonConfig = "real.epsilon";
		private const string SeparatorConfig = "separator.respect";
		private const string ChangeConfig = "undo.limit";
		private const string ControlLeftConfig = "control.left";

		/// <summary>
		/// Указатель на интерфейс сервера
		/// </summary>
		private IServer server;

		/// <summary>
		/// Указатель на интерфейс модели
		/// </summary>
		private IModel model;

		/// <summary>
		/// Объект настроек
		/// </summary>
		private Config config;

		/// <summary>
		/// Журнал изменений
		/// </summary>
		private ChangeLog changes;

		/// <summary>
		///  Список редакторов скриптов
		/// </summary>
		private ScriptProject project;

		/// <summary>
		/// Фабрика класса сервера
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
		/// Прозводит инициализацию библиотеки
		/// </summary>
		public static void InitializeStrallanLibrary()
		{
			if (ComObject == null)
			{
				ComObject = new Library();
			}
		}

		/// <summary>
		/// Освобождает ресурсы библиотеки
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
		/// Освобождает ресурсы библиотеки
		/// </summary>
		void IDisposable.Dispose()
		{
			Marshal.ReleaseComObject(model);
			Marshal.ReleaseComObject(server);
			(project as IDisposable).Dispose();
			(config as IDisposable).Dispose();
		}

		/// <summary>
		/// Com-объект библиотеки
		/// </summary>
		public static Library ComObject { get; private set; } = null;

		/// <summary>
		/// Объект сервера
		/// </summary>
		public static IServer Server => ComObject.server;

		/// <summary>
		/// Объект модели
		/// </summary>
		public static IModel Model => ComObject.model;

		/// <summary>
		/// Объект настроек
		/// </summary>
		public static Config Settings => ComObject.config;

		/// <summary>
		/// Журнал изменений
		/// </summary>
		public static ChangeLog Changes => ComObject.changes;

		/// <summary>
		/// Список редакторов скриптов
		/// </summary>
		public static ScriptProject Project => ComObject.project;

		/// <summary>
		/// Точность сравнения вещественных чисел
		/// </summary>
		public static double Epsilon
		{
			get => ComObject.server.GetEpsilon();
			set => ComObject.config[EpsilonConfig] = ComObject.server.RealToStringFmt(value, ".", 16, "E");
		}

		/// <summary>
		/// Формат преобразования вещественных чисел в строку
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
		/// Определяет, требуется ли замена десятичного разделителя при копировании данных в буфер обмена
		/// </summary>
		public static bool RespectDecimalSeparator
		{
			get => ComObject.config[SeparatorConfig] == "1";
			set => ComObject.config[SeparatorConfig] = value ? "1" : "0";
		}

		/// <summary>
		/// Определяет, что расположение контрольной панели слева от содержимого закладок
		/// </summary>
		public static bool ControlLeft
		{
			get => ComObject.config[ControlLeftConfig] == "1";
			set => ComObject.config[ControlLeftConfig] = value ? "1" : "0";
		}

		/// <summary>
		/// Событие изменения расположения контрольных панелей
		/// </summary>
		public event Action ControlLeftChanged;

		/// <summary>
		/// Инициализирует параметр настройки
		/// </summary>
		/// <param name="Name">Путь к параметру</param>
		/// <param name="Init">Метод инициализации значения</param>
		/// <param name="OnChange">Событие изменения значения</param>
		/// <returns>Признак успешной инициализации</returns>
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
		/// Событие изменения настроек отображения вещественных чисел
		/// </summary>
		public event Action RealSettingChanged;

		/// <summary>
		/// Проверяет условие нахождения числа в заданном диапазоне (включая границы)
		/// </summary>
		/// <param name="v">Проверяемое число</param>
		/// <param name="a">Первая граница диапазона</param>
		/// <param name="b">Вторая граница диапазона</param>
		/// <returns>Признак нахождения числа в диапазоне</returns>
		public static bool InsideRange(int v, int a, int b)
		{
			return a > b ? v >= b && v <= a : v >= a && v <= b;
		}

		/// <summary>
		/// Разбивает строку на подстроки
		/// </summary>
		/// <param name="Str">Исходня строка</param>
		/// <param name="Func">Функция, получающая количество символов разделение строк с начала строки,
		/// количество символов табуляции после последнего разделителя строк и подстроку</param>
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
		/// Пустой метод
		/// </summary>
		/// <param name="p">Список аргументов</param>
		public static void Hole(params object[] p) { }

		/// <summary>
		/// Возвращает количество миллисекунд, прошедших с момента старта системы
		/// </summary>
		[DllImport("kernel32.dll"), PreserveSig()]
		public static extern ulong GetTickCount64();

		/// <summary>
		/// Возвращает расстояние между двумя точками
		/// </summary>
		/// <param name="a">Первая точка</param>
		/// <param name="b">Вторая точка</param>
		public static double Distance(Point a, Point b)
		{
			double x = (a.X - b.X), y = (a.Y - b.Y);
			return Math.Sqrt(x * x + y * y);
		}

		/// <summary>
		/// Возвращает минимальное расстояние от точки до границы области, заданной размерами
		/// </summary>
		/// <param name="a">Точка</param>
		/// <param name="Width">Ширина области</param>
		/// <param name="Height">Высота области</param>
		/// <returns>Расстояние</returns>
		public static double Distance(Point a, double Width, double Height)
		{
			return Math.Min(Math.Min(a.X, Width - a.X), Math.Min(a.Y, Height - a.Y));
		}

		/// <summary>
		/// Возвращает минимальное расстояние от точки до границ прямоугольника
		/// </summary>
		/// <param name="a">Точка</param>
		/// <param name="b">Прямоугольник</param>
		/// <returns>Расстояние</returns>
		public static double Distance(Point a, Rect b)
		{
			return Math.Min(Math.Min(a.Y - b.Top, b.Bottom - a.Y),
				Math.Min(a.X - b.Left, b.Right - a.X));
		}

		/// <summary>
		/// Возвращает минимальное расстояние от точки до границ прямоугольника
		/// </summary>
		/// <param name="a">Точка</param>
		/// <param name="Left">Левая граница</param>
		/// <param name="Top">Верхняя граница</param>
		/// <param name="Right">Правая граница</param>
		/// <param name="Bottom">Нижняя граница</param>
		/// <returns>Расстояние</returns>
		public static double Distance(Point a, double Left, double Top, double Right, double Bottom)
		{
			return Math.Min(Math.Min(a.Y - Top, Bottom - a.Y),
				Math.Min(a.X - Left, Right - a.X));
		}

		/// <summary>
		/// Проверяет, что символ является допустимым в идентификаторах скриптов
		/// </summary>
		/// <param name="Ch">Символ</param>
		/// <returns>Признак допустимости</returns>
		public static bool IsValidIdentChar(char Ch)
		{
			return Char.IsLetterOrDigit(Ch) || Ch == '_' || Ch == '$';
		}

		/// <summary>
		/// Возвращает объект типа Color из целочисленного значения в формате RGB
		/// </summary>
		/// <param name="C">Код цвета</param>
		/// <returns>Объект класса Color</returns>
		public static Color IntToColor(int C)
		{
			return Color.FromRgb((byte)(C & 0xFF),
				(byte)((C >> 8) & 0xFF),
				(byte)((C >> 16) & 0xFF));
		}

		/// <summary>
		/// Возвращает объект типа Color из цвета в формате HSL
		/// </summary>
		/// <param name="Hue">Оттенок (0..255)</param>
		/// <param name="Saturation">Насыщенность (0..255)</param>
		/// <param name="Lightness">Освещеённость (0..255</param>
		/// <returns></returns>
		public static Color HslToColor(int Hue, int Saturation, int Lightness)
		{
			if (Hue < 0)
				Hue = int.MaxValue + Hue;

			Hue &= 255;
			Saturation = Math.Max(0, Math.Min(Saturation, 255));
			Lightness = Math.Max(0, Math.Min(Lightness, 255));

			if (Saturation == 0) // серый цвет
				return Color.FromRgb((byte)Lightness, (byte)Lightness, (byte)Lightness);
			else // не серый цвет
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
		/// Возвращает список вложенных Xml элементов с заданным именем
		/// </summary>
		/// <param name="Root">Начальный элемент</param>
		/// <param name="Name">Имя</param>
		/// <returns>Список вложенных элементов</returns>
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
		/// Возвращает единственный вложенный Xml элемент с заданным именем
		/// </summary>
		/// <param name="Root">Начальный элемент</param>
		/// <param name="Name">Имя</param>
		/// <returns>Вложенный элемент</returns>
		public static XElement FindXmlNode(XElement Root, string Name)
		{
			var L = FindXmlNodes(Root, Name);
			return L.Count == 1 ? L[0] : null;
		}

		/// <summary>
		/// Возвращает строковое значение атрибута
		/// </summary>
		/// <param name="Node">Xml элемент</param>
		/// <param name="Name">Имя атрибута</param>
		/// <param name="Default">Значение по умолчанию</param>
		/// <returns>Значение атрибута</returns>
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
		/// Возвращает вещественное значение атрибута
		/// </summary>
		/// <param name="Node">Xml элемент</param>
		/// <param name="Name">Имя атрибута</param>
		/// <param name="Default">Значение по умолчанию</param>
		/// <returns>Значение атрибута</returns>
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
		/// Возвращает целочисленное значение атрибута
		/// </summary>
		/// <param name="Node">Xml элемент</param>
		/// <param name="Name">Имя атрибута</param>
		/// <param name="Default">Значение по умолчанию</param>
		/// <returns>Значение атрибута</returns>
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
		/// Возвращает логическое значение атрибута
		/// </summary>
		/// <param name="Node">Xml элемент</param>
		/// <param name="Name">Имя атрибута</param>
		/// <param name="Default">Значение по умолчанию</param>
		/// <returns>Значение атрибута</returns>
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
	/// Класс, реализующий интерфейс перебора объектов расчётной модели
	/// </summary>
	class AddrList : ILongEnum
	{
		/// <summary>
		/// Список адресов объектов
		/// </summary>
		private readonly List<long> Addr = new List<long>();

		// Устанавливает вместимость списка
		void ILongEnum.SetCapacity(int Capacity)
		{
			Addr.Clear();
			Addr.Capacity = Capacity;
		}

		/// <summary>
		/// Добаляет адрес в список
		/// </summary>
		int ILongEnum.Enum(long N)
		{
			Addr.Add(N);
			return 0;
		}

		/// <summary>
		/// Количество адресов списка
		/// </summary>
		public int Count => Addr.Count;

		/// <summary>
		/// Возвращает адрес из спика
		/// </summary>
		/// <param name="Index">Позиция в списке</param>
		/// <returns>Адрес</returns>
		public long this[int Index] => Addr[Index];

		/// <summary>
		/// Преобразует список в массив целых чисел.
		/// </summary>
		/// <returns></returns>
		public long[] ToArray() => Addr.ToArray();
	};

    /// <summary>
    /// Класс события завершения асинхронного процесса
    /// </summary>
    class FinishEvent : IFinishEvent
    {
        /// <summary>
        /// Событие завершения асинхронного процесса
        /// </summary>
        private readonly Action<int, string> Finish;

        /// <summary>
        /// Контекст синхронизации
        /// </summary>
        private readonly SynchronizationContext SyncCtx;

        /// <summary>
        /// Создаёт объект события
        /// </summary>
        /// <param name="finish">Событие завершения асинхронного процесса</param>
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
    /// Класс настроек приложения
    /// </summary>
    public class Config : IDisposable, ISettingClient
    {
        /// <summary>
        /// Создаёт объект настроек
        /// </summary>
        /// <param name="Server">Указатель на интерфейс сервера Strallan</param>
        /// <param name="AppName">Имя приложения</param>
        /// <param name="AppVersion">Версия приложения</param>
        /// <param name="AppGuid">GUID приложения</param>
        /// <param name="FileName">Имя файла настроек</param>
        /// <param name="Local">Признак размещения файла настроек в локальном профиле пользователя</param>
        public Config(IServer Server,
            string AppName, string AppVersion, string AppGuid, string FileName, bool Local = true)
        {
            Srv = Server;
            SyncCtx = SynchronizationContext.Current;
            Cfg = Server.LoadSettings(AppName, AppVersion, AppGuid, FileName, Local ? 1 : 0);
            Cfg.SetMessageClient(this);
        }

        /// <summary>
        /// Создаёт объект настроек с использованием данных Assembly Information
        /// </summary>
        /// <param name="Server">Указатель на интерфейс сервера Strallan</param>
        /// <param name="FileName">Имя файла настроек</param>
        /// <param name="Local">Признак размещения файла настроек в локальном профиле пользователя</param>
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
		/// Имя и версия приложения
		/// </summary>
		public string ProgVersion { get; private set; }


		/// <summary>
		/// Значение параметра настроек
		/// </summary>
		/// <param name="Path">Путь к параметру (имена вложенных параметров разделяются точками)</param>
		/// <returns>Значение параметра настроек</returns>
		public string this[string Path]
        {
            get => Cfg.GetValue(Path);
            set => Cfg.SetValue(Path, value);
        }

		// Преобразует массив аргументов в строку с разделителями - точками
		private static string ParamsToPath(params string[] Params)
		{
			for (int i = 1, n = Params.Length; i < n; i++)
				Params[0] += "." + Params[i];
			return Params[0];
		}

		/// <summary>
		/// Значение параметра настроек
		/// </summary>
		/// <param name="Path">Путь к параметру (последовательность имён)</param>
		/// <returns>Значение параметра настроек</returns>
		public string this[params string[] Params]
		{
			get => Cfg.GetValue(ParamsToPath(Params));
			set => Cfg.SetValue(ParamsToPath(Params), value);
		}

        /// <summary>
        /// Возвращает целочисленное значение параметра
        /// </summary>
        /// <param name="Path">Путь к параметру</param>
        /// <param name="Default">Значение по умолчанию</param>
        /// <returns>Значение параметра</returns>
        public int GetInt(string Path, int Default = 0)
        {
            string Str = Cfg.GetValue(Path).Trim();
            if (int.TryParse(Str, out int v))
                return v;
            else
                return Default;
        }

		/// <summary>
		/// Тип события изменения значения параметра настроек
		/// </summary>
		/// <param name="KeyName">Имя параметра</param>
		/// <param name="OldValue">Старое значение</param>
		/// <param name="NewValue">Новое значение</param>
		public delegate void OnValueChangedEventType(string KeyName, string OldValue, string NewValue);

        /// <summary>
        /// Тип события запуска новой копии приложения
        /// </summary>
        /// <param name="Handle">Дескриптор приложения</param>
        /// <param name="Stamp">Временная метка приложения</param>
        public delegate void OnNewInstanceEventType(long Handle, long Stamp);

        /// <summary>
        /// Тип события получения сообщения от другой копии приложения
        /// </summary>
        /// <param name="From">Дескриптор приложения-источника</param>
        /// <param name="Msg">Текст сообщения</param>
        /// <param name="Reply">Код ответа</param>
        public delegate void OnMessageEventType(long From, string Msg, out long Reply);

        /// <summary>
        /// Подписывается на изменение всех вложенных параметров указанного параметра
        /// </summary>
        /// <param name="Key">Имя параметра</param>
        /// <param name="Func">Событие изменения значения параметра</param>
        /// <returns>Код ответа для отказа от подписки</returns>
        public long Subscribe(string Key, OnValueChangedEventType Func) => Cfg.Subscribe(Key, new ConfigClient(Func, SyncCtx));

        /// <summary>
        /// Снимает подписку на события изменения значения параметра
        /// </summary>
        /// <param name="Key">Имя параметра</param>
        /// <param name="Code">Код ответа, полученый при вызове Subscribe</param>
        public void Unsubscribe(string Key, long Code) => Cfg.Unsubscribe(Key, Code);

        /// <summary>
        /// Событие запуска новой копии приложения
        /// </summary>
        public event OnNewInstanceEventType OnNewInstanceEvent;

        /// <summary>
        /// Событие получения сообщения от параллельной копии приложения
        /// </summary>
        public event OnMessageEventType OnMessageEvent;

        /// <summary>
        /// Проверяет состояние временного параметра (не сохраняемого между сессиями приложения)
        /// </summary>
        /// <param name="Key">Имя параметра</param>
        /// <returns>Признак временности параметра</returns>
        public bool IsProvisional(string Key) => Cfg.GetProvisional(Key) != 0;

        /// <summary>
        /// Устанавливает состояние временного
        /// </summary>
        /// <param name="Key">Имя параметра</param>
        /// <param name="Flag">Признак временности</param>
        public void SetProvisional(string Key, bool Flag) => Cfg.SetProvisional(Key, Flag ? 1 : 0);

        /// <summary>
        /// Проверяет состояние локального параметра (события изменения которого не передаются другим копиям приложения)
        /// </summary>
        /// <param name="Key">Имя параметра</param>
        /// <returns>Признак локальности параметра</returns>
        public bool IsLocal(string Key) => Cfg.GetLocal(Key) != 0;

        /// <summary>
        /// Устанавливает состояние локального параметра
        /// </summary>
        /// <param name="Key">Имя параметра</param>
        /// <param name="Flag">Признак локальности</param>
        public void SetLocal(string Key, bool Flag) => Cfg.SetLocal(Key, Flag ? 1 : 0);

        /// <summary>
        /// Принудительно сохраняет настройки в файл
        /// </summary>
        public void Dump() => Cfg.Dump();

        /// <summary>
        /// Восстанавливает настройки из файла
        /// </summary>
        public void Reload() => Cfg.Reload();

        /// <summary>
        /// Возвращает список активных копий приложения
        /// </summary>
        /// <returns>Список активных копий (дескриптор и временная метка)</returns>
        public (long Handle, long Stamp)[] GetActiveInstances()
        {
            List<(long Handle, long Stamp)> L = new List<(long Handle, long Stamp)>();

            var S = Cfg.GetInstanceList().Split(' ');
            for (int i = 0; i < S.Length; i += 2)
                L.Add((Int64.Parse(S[i]), Int64.Parse(S[i + 1])));

            return L.ToArray();
        }

        /// <summary>
        /// Посылает сообщение всем активным копиям приложения
        /// </summary>
        /// <param name="Msg">Текст сообщения</param>
        /// <returns>Список ответивших копий (дескриптор, временная метка, код ответа)</returns>
        public (long Handle, long Stamp, long Reply)[] BroadcastMessage(string Msg)
        {
            List<(long Handle, long Stamp, long Reply)> L = new List<(long Handle, long Stamp, long Reply)>();

            var S = Cfg.BroadcastMessage(Msg).Split(' ');
            for (int i = 0; i < S.Length; i += 3)
                L.Add((Int64.Parse(S[i]), Int64.Parse(S[i + 1]), Int64.Parse(S[i + 2])));

            return L.ToArray();
        }

        /// <summary>
        /// Посылает сообщение другой копии приложения
        /// </summary>
        /// <param name="Handle">Дескриптор приложения</param>
        /// <param name="Msg">Текст сообщения</param>
        public long SendMessage(long Handle, string Msg) => Cfg.SendMessage(Handle, Msg);

        /// <summary>
        /// Сохраняет позицию окна
        /// </summary>
        /// <param name="Key">Имя параметра сохранения</param>
        /// <param name="Wnd">Окно приложения</param>
        public void SaveWindowPlacement(string Key, Window Wnd) =>
            this[Key] = Srv.GetWindowPlacement((long)(new WindowInteropHelper(Wnd).Handle));

        /// <summary>
        /// Восстанавливает позицию окна. Если окно полностью или частично находится вне
        /// экрана, оно будет полностью перенесено в видимую область
        /// </summary>
        /// <param name="Key">Имя параметра сохранения</param>
        /// <param name="Wnd">Окно приложения</param>
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

        // Метод интерфейса IDisposable
        void IDisposable.Dispose()
        {
            Cfg.SetMessageClient(null);
            Marshal.ReleaseComObject(Cfg);
        }

        // Указатель на интерфейс конфигурации
        private readonly ISettings Cfg;

        // Указатель на интефейс сервера
        private readonly IServer Srv;


        // Указатель на контекст синхронизации
        private readonly SynchronizationContext SyncCtx;

        // Методы интерфейса ISettingClient

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


        // Реализация интерфейса подписчика на события
        private class ConfigClient : ISettingClient
        {
            // Событие изменения значения ключа
            private readonly OnValueChangedEventType OnValueChanged;

            // Контекст синхронизации
            private readonly SynchronizationContext SyncCtx;

            // Создаёт объект
            public ConfigClient(OnValueChangedEventType Func, SynchronizationContext Ctx)
            {
                OnValueChanged = Func;
                SyncCtx = Ctx;
            }

            // Методы интерфейса IConfigClient
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
	/// Класс перебора строк
	/// </summary>
	public class EnumStrings : IStringEnum
	{
		/// <summary>
		/// Список строк
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
		/// Возвращает строку списка
		/// </summary>
		/// <param name="Index">Номер строки</param>
		/// <returns>Текст строки</returns>
		public string this[int Index] => Strings[Index];

		/// <summary>
		/// Количество строк списка
		/// </summary>
		public int Count => Strings.Count;

		/// <summary>
		/// Сортирует строки 
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
    /// Класс журнала изменений
    /// </summary>
    public class ChangeLog
    {
        /// <summary>
        /// Создаёт журнал изменений
        /// </summary>
        public ChangeLog()
        {
            UndoQueue = new List<IRecord>();
            RedoQueue = new List<IRecord>();
            Stack = new Stack<List<IRecord>>();
            Stack.Push(UndoQueue);
        }

        /// <summary>
        /// Начинает группу записей, обрабатываемых за одну команду отмены/возврата
        /// </summary>
        /// <returns>true – группа начата, false – журнал заблокирован</returns>
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
        /// Вносит в журнал запись об изменениях
        /// </summary>
        /// <param name="Undo">Метод отмены изменений</param>
        /// <param name="Redo">Метод возврата отмены</param>
        /// <returns>true – запись внесена, false – журнал заблокирован</returns>
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
        /// Завершает группу записей. Метод должен быть вызван в любом контексте после
        /// метода BeginGroup, вернувшего true
        /// </summary>
        public void EndGroup()
        {
            if (LockCount == 0 && Stack.Count > 1)
                Stack.Pop();
        }

        /// <summary>
        /// Проверяет возможность вызова метода отмены изменений
        /// </summary>
        public bool CanUndo => UndoQueue.Count > 0 && Stack.Count == 1;

        /// <summary>
        /// Метод отмены изменений. Может быть вызван только после CanUndo, вернувшего true
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
        /// Проверяет возможность вызова метода возврата отмены
        /// </summary>
        public bool CanRedo => RedoQueue.Count > 0 && Stack.Count == 1;

        /// <summary>
        /// Метод отмены возврата. Может быть вызван только после CanRedo, вернувшего true
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
        /// Проверяет, что журнал не заблокирован и готов принимать записи
        /// </summary>
        public bool IsUnlocked => LockCount == 0;

        /// <summary>
        /// Ограничение на количество записей в журнале. При неположительном значении журнал является бесконечным
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
        /// Очищает журнал
        /// </summary>
        public void Clear()
        {
            UndoQueue.Clear();
            RedoQueue.Clear();
            Stack.Clear();
            Stack.Push(UndoQueue);
        }

		/// <summary>
		/// Событие изменения ограничения количества записей
		/// </summary>
		public event Action<object, int> UndoLimitChanged;

        /// <summary>
        /// Счётчик блокировок журнала
        /// </summary>
        private int LockCount = 0;

        /// <summary>
        /// Ограничение на количество записей в журнал
        /// </summary>
        private int QueueLimit = 0;

        /// <summary>
        /// Применяет органичение количества записей, удаляя лишние из журнала
        /// </summary>
        private void ApplyLimit()
        {
            if (QueueLimit > 0 && UndoQueue.Count > QueueLimit)
                    UndoQueue.RemoveRange(0, UndoQueue.Count - QueueLimit);
        }

		/// <summary>
		/// Блокирует журнал
		/// </summary>
		public void Lock()
		{
			LockCount++;
		}

		/// <summary>
		/// Разблокирует журнал
		/// </summary>
		public void Unlock()
		{
			LockCount--;
		}

        /// <summary>
        /// Интерфейс записи журнала
        /// </summary>
        private interface IRecord
        {
            void Undo();
            void Redo();
        }

        /// <summary>
        /// Очередь отмены изменений
        /// </summary>
        private readonly List<IRecord> UndoQueue;

        /// <summary>
        /// Очередь возврата отмены
        /// </summary>
        private readonly List<IRecord> RedoQueue;

        /// <summary>
        /// Стек очередей отмены
        /// </summary>
        private readonly Stack<List<IRecord>> Stack;

        /// <summary>
        /// Класс записи журнала
        /// </summary>
        private class Record : IRecord
        {
            /// <summary>
            /// Метод отмены изменений
            /// </summary>
            private readonly Action UndoAction;

            /// <summary>
            /// Метод возврата изменений
            /// </summary>
            private readonly Action RedoAction;

            /// <summary>
            /// Создаёт запись
            /// </summary>
            /// <param name="undo">Метод отмены</param>
            /// <param name="redo">Метод возврата</param>
            public Record(Action undo, Action redo)
            {
                UndoAction = undo;
                RedoAction = redo;
            }
            void IRecord.Redo() => RedoAction();

            void IRecord.Undo() => UndoAction();
        }

        /// <summary>
        /// Класс группы изменений
        /// </summary>
        private class Group : IRecord
        {
            /// <summary>
            /// Список записей группы
            /// </summary>
            private readonly List<IRecord> Queue;

            /// <summary>
            /// Создаёт групповую запись
            /// </summary>
            /// <param name="queue">Очередь записей группы</param>
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
	/// Класс списка указателей на интерфейсы редакторов
	/// </summary>
	public class ScriptProject : IScriptClient, IDisposable
	{
		/// <summary>
		/// Список указателей
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
		/// Возвращает объект по индексу
		/// </summary>
		/// <param name="Index">Индекс</param>
		/// <returns>Объект</returns>
		public EditControl this[int Index] => Editors[Index];

		/// <summary>
		/// Возвращает полное имя файла
		/// </summary>
		/// <param name="FileName">Относительное имя</param>
		/// <returns>Абсолютное имя</returns>
		private string FullName(string FileName)
		{
			var f = new FileInfo(FileName);
			return f.FullName;
		}

		/// <summary>
		/// Возвращает индекс объекта по имени файла
		/// </summary>
		/// <param name="FileName">Имя файла</param>
		/// <returns>Индекс объекта (-1 если файл не найден)</returns>
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
		/// Создаёт новый объект редактора
		/// </summary>
		/// <returns>Объект редактора</returns>
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
		/// Событие измнения редактора
		/// </summary>
		/// <param name="obj"></param>
		private void Changed(EditControl obj)
		{
			EditorChanged?.Invoke(obj);
		}

		/// <summary>
		/// Событие запроса на показ редактора
		/// </summary>
		private void EditorDisplayRequest(EditControl obj)
		{
			DisplayRequest?.Invoke(obj);
		}

		/// <summary>
		/// Событие запроса указателя на интерфейс редактора по имени файла
		/// </summary>
		private void EditorRequest(string FileName, ref IScriptEditor Editor)
		{
			var E = CheckFile(FileName, true, false);
			if (E != null)
				Editor = E.Editor;
		}

		/// <summary>
		/// Возвращает объект редактора по имени файла
		/// </summary>
		/// <param name="FileName">Имя файла</param>
		/// <param name="Open">Признак необходимости открыть файл</param>
		/// <param name="Create">Признак необходимости создать новый файл</param>
		/// <returns>Объект редактора</returns>
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
		/// Закрывает файл
		/// </summary>
		/// <param name="Index">Индекс редактора</param>
		public void CloseFile(int Index)
		{
			if (Editors[Index] != null)
			{
				(Editors[Index] as IDisposable).Dispose();
				Editors[Index] = null;
			}
		}

		/// <summary>
		/// Закрывает файл
		/// </summary>
		/// <param name="FileName">Имя файла</param>
		public void CloseFile(string FileName)
		{
			var Index = IndexOf(FileName);
			if (Index >= 0)
				CloseFile(Index);
		}

		/// <summary>
		/// Закрывает файл
		/// </summary>
		/// <param name="Editor">Интерфейс редактора</param>
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
		/// Контекст синхронизации
		/// </summary>
		private readonly SynchronizationContext SyncCtx = SynchronizationContext.Current;

		/// <summary>
		/// Событие создания объекта редактора
		/// </summary>
		public event Action<ScriptProject, EditControl> CreateEditor;

		/// <summary>
		/// Событие запроса на отображение редактора
		/// </summary>
		public event Action<EditControl> DisplayRequest;

		/// <summary>
		/// Событие изменения состояния редактора
		/// </summary>
		public event Action<EditControl> EditorChanged;

		/// <summary>
		/// Событие начала работы скрипта
		/// </summary>
		public event Action Start;

		/// <summary>
		/// Признак работы скрипта
		/// </summary>
		public bool IsRunning { get; private set; } = false;

		/// <summary>
		/// Событие окончания разбора скрипта
		/// </summary>
		public event Action EndParsing;

		/// <summary>
		/// Событие начала записи в журнал
		/// </summary>
		public event Action<string> StartLog;

		/// <summary>
		/// Событие записи в журнал
		/// </summary>
		public event Action<string> WriteLog;

		/// <summary>
		/// Событие окончания записи в журнал
		/// </summary>
		public event Action EndLog;

		/// <summary>
		/// Событие записи в поток вывода
		/// </summary>
		public event Action<string> WriteOutput;

		/// <summary>
		/// Событие начала ожидания завершения внешнего процесса
		/// </summary>
		public event Action StartWaiting;

		/// <summary>
		/// Событие окончания ожидания завершения внешнего процесса
		/// </summary>
		public event Action EndWaiting;

		/// <summary>
		/// Признак ожидания завершения внешнего процесса
		/// </summary>
		public bool IsWaiting { get; private set; } = false;

		/// <summary>
		/// Событие приостановки выполнения скрипта
		/// </summary>
		public event Action<int, int, string> Suspended;

		/// <summary>
		/// Событие возобновления выполнения скрипта
		/// </summary>
		public event Action Resumed;

		/// <summary>
		/// Признак приостановленной работы скрипта
		/// </summary>
		public bool IsSuspended { get; private set; } = false;

		/// <summary>
		/// Событие начала работы вычислительного модуля
		/// </summary>
		public event Action StartAnalysis;

		/// <summary>
		/// Событие завершения работы вычислительного модуля
		/// </summary>
		public event Action EndAnalysis;

		/// <summary>
		/// Признак работы вычислительного модуля
		/// </summary>
		public bool IsPerformingAnalysis { get; private set; } = false;

		/// <summary>
		/// Событие завершения работы скрипта
		/// </summary>
		public event Action Finish;

		/// <summary>
		/// Признак режима трассировки
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