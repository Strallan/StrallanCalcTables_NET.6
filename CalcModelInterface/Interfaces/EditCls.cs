using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Timers;
using static Strallan.Library;


namespace Strallan.Controls
{
	/// <summary>
	/// Класс стилей подсветки синтаксиса
	/// </summary>
	public class HighlightStyles : IDisposable
	{
		private const string HighlightKey = "editor.highlight";
		private const string FontKey = "font";
		private const string NameKey = "name";
		private const string SizeKey = "size";
		private const string BackgroundKey = "background";
		private const string ForegroundKey = "foreground";
		private const string BoldKey = "bold";
		private const string ItalicKey = "italic";
		private const string UnderlineKey = "underline";
		private const string RightMarginKey = "rightmargin";
		private const string TabWidthKey = "tabwidth";
		private const string TagKey = "tag";


		/// <summary>
		/// Событие изменения стилей
		/// </summary>
		public event Action<HighlightStyles> Changed;

		/// <summary>
		/// Имя шрифта
		/// </summary>
		private string FontNameField;

		/// <summary>
		/// Имя шрифта
		/// </summary>
		public string FontName
		{
			get => FontNameField;
			set => Settings[HighlightKey, FontKey, NameKey] = value;
		}

		/// <summary>
		/// Размер шрифта
		/// </summary>
		private int FontSizeField;

		/// <summary>
		/// Размер шрифта
		/// </summary>
		public double FontSize
		{
			get => FontSizeField * (96.0 / 72.0);
			set => Settings[HighlightKey, FontKey, SizeKey] = ((int)value).ToString();
		}

		/// <summary>
		/// Расположение вертикальной линии справа от текста
		/// </summary>
		private int RightMarginField;

		/// <summary>
		/// Расположение вертикальной линии справа от текста
		/// </summary>
		public int RightMargin
		{
			get => RightMarginField;
			set => Settings[HighlightKey, "rightmargin"] = value.ToString();
		}

		/// <summary>
		/// Количество пробелов для замены знаков табуляции
		/// </summary>
		private int TabWidthField;

		/// <summary>
		/// Количество пробелов для замены знаков табуляции
		/// </summary>
		public int TabWidth
		{
			get => TabWidthField;
			set => Settings[HighlightKey, "tabwidth"] = value.ToString();
		}


		/// <summary>
		/// Код подписки на изменение настроек
		/// </summary>
		private readonly long SubscribeCode;

		/// <summary>
		/// Создаёт объект стилей
		/// </summary>
		public HighlightStyles()
		{
			Styles.Add(new Style("space", Colors.Yellow, Colors.Navy, false, false, false,
				new Style("", Colors.Black, Colors.White, false, false, false)));

			Styles.Add(new Style("delimiter", Colors.Yellow, Colors.Navy, false, false, false,
				new Style("", Colors.Black, Colors.White, false, false, false)));

			Styles.Add(new Style("block", Colors.LightSkyBlue, Colors.Navy, false, false, false,
				new Style("", Colors.Black, Colors.White, true, false, false)));

			Styles.Add(new Style("constant", Colors.Cornsilk, Colors.Navy, false, false, false,
				new Style("", Colors.Black, Colors.White, true, false, false)));

			Styles.Add(new Style("directive", Colors.Aquamarine, Colors.Navy, false, false, false,
				new Style("", Colors.Black, Colors.White, true, false, false)));

			Styles.Add(new Style("function", Colors.Cornsilk, Colors.Navy, false, false, false,
				new Style("", Colors.Black, Colors.White, true, false, false)));

			Styles.Add(new Style("type", Colors.PapayaWhip, Colors.Navy, false, false, false,
				new Style("", Colors.Black, Colors.White, true, false, false)));

			Styles.Add(new Style("bracket", Colors.LightGreen, Colors.Navy, false, false, false,
				new Style("", Colors.Black, Colors.White, false, false, false)));

			Styles.Add(new Style("identifier", Colors.Yellow, Colors.Navy, false, false, false,
				new Style("", Colors.Black, Colors.White, false, false, false)));

			Styles.Add(new Style("itemid", Colors.Gold, Colors.Navy, false, false, false,
				new Style("", Colors.Black, Colors.White, false, false, false)));

			Styles.Add(new Style("comment", Colors.Lime, Colors.Navy, false, false, false,
				new Style("", Color.FromRgb(140, 140, 140), Colors.White, false, true, false)));

			Styles.Add(new Style("operator", Colors.LightYellow, Colors.Navy, false, false, false,
				new Style("", Colors.Black, Colors.White, true, false, false)));

			Styles.Add(new Style("string", Colors.Gold, Colors.Navy, false, false, false,
				new Style("", Colors.Black, Colors.White, false, false, false)));

			Styles.Add(new Style("number", Colors.Yellow, Colors.Navy, false, false, false,
				new Style("", Colors.Black, Colors.White, false, false, false)));

			Styles.Add(new Style("selection", Color.FromRgb(24, 24, 24), Colors.Silver, false, false, false,
				null));

			Styles.Add(new Style("bracematch", Colors.White, Colors.Black, false, false, false,
				null));

			Styles.Add(new Style("foundmatch", Color.FromRgb(0, 0, 24), Colors.DeepSkyBlue, false, false, false,
				null));

			Styles.Add(new Style("errorline", Colors.White, Colors.Red, false, false, false,
				null));

			Styles.Add(new Style("linenumbers", Colors.Silver, Colors.Navy, false, false, false,
				new Style("", Colors.Black, Colors.White, false, false, false)));

			LineNumbers.TagField = 2;

			UpdateSettings("", "", "");
			SubscribeCode = Settings.Subscribe(HighlightKey, UpdateSettings);
		}

		/// <summary>
		/// Обновляет стили в ответ на изменение конфигурации
		/// </summary>
		private void UpdateSettings(string Key, string OldValue, string NewValue)
		{
			FontNameField = Settings[HighlightKey, FontKey, NameKey];
			EnumStrings List = new EnumStrings();
			Server.EnumFixedPitchFonts(List);

			if (FontNameField == "" || List.IndexOf(FontNameField) == -1)
			{
				int Index = List.IndexOf("Courier New");
				if (Index != -1)
					FontNameField = List[Index];
				else if ((Index = List.IndexOf("Consolas")) != -1)
					FontNameField = List[Index];
				else if ((Index = List.IndexOf("Lucida Console")) != -1)
					FontNameField = List[Index];
				else
					FontNameField = List[0];

				CurrentTypeface = new Typeface(FontNameField);
			}

			if (!int.TryParse(Settings[HighlightKey, FontKey, SizeKey], out FontSizeField))
				FontSizeField = 10;
			else
				FontSizeField = Math.Max(6, Math.Min(32, FontSizeField));


			if (!int.TryParse(Settings[HighlightKey, RightMarginKey], out RightMarginField))
				RightMarginField = 80;
			else
				RightMarginField = Math.Max(0, Math.Min(512, RightMarginField));

			if (!int.TryParse(Settings[HighlightKey, TabWidthKey], out TabWidthField))
				TabWidthField = 2;
			else
				TabWidthField = Math.Max(1, Math.Min(64, TabWidthField));

			foreach (var s in Styles)
				s.UpdateStyle();
				
			Changed?.Invoke(this);
		}

		/// <summary>
		/// Освобождение ресурсов
		/// </summary>
		void IDisposable.Dispose()
		{
			Settings.Unsubscribe(HighlightKey, SubscribeCode);
		}

		/// <summary>
		/// Стиль простого текста
		/// </summary>
		public Style Space => Styles[Highlight.Space];

		/// <summary>
		/// Стиль разделителей
		/// </summary>
		public Style Delimiter => Styles[Highlight.Delimiter];

		/// <summary>
		/// Стиль блоков
		/// </summary>
		public Style Block => Styles[Highlight.Block];

		/// <summary>
		/// Стиль констант
		/// </summary>
		public Style Constant => Styles[Highlight.Constant];

		/// <summary>
		/// Стиль директив
		/// </summary>
		public Style Directive => Styles[Highlight.Directive];

		/// <summary>
		/// Стиль функций
		/// </summary>
		public Style Function => Styles[Highlight.Function];

		/// <summary>
		/// Стиль типов
		/// </summary>
		public Style Type => Styles[Highlight.Type];

		/// <summary>
		/// Стиль скобок
		/// </summary>
		public Style Bracket => Styles[Highlight.Bracket];

		/// <summary>
		/// Стиль идентификаторов
		/// </summary>
		public Style Identifier => Styles[Highlight.Identifier];

		/// <summary>
		/// Стиль идентификаторов оъектов расчётной модели
		/// </summary>
		public Style ItemId => Styles[Highlight.ItemId];

		/// <summary>
		/// Стиль комментариев
		/// </summary>
		public Style Comment => Styles[Highlight.Comment];

		/// <summary>
		/// Стиль операторов
		/// </summary>
		public Style Operator => Styles[Highlight.Operator];

		/// <summary>
		/// Стиль строк
		/// </summary>
		public Style String => Styles[Highlight.String];

		/// <summary>
		/// Стиль чисел
		/// </summary>
		public Style Number => Styles[Highlight.Number];

		/// <summary>
		/// Стиль выделения
		/// </summary>
		public Style Selection => Styles[Highlight.Selection];

		/// <summary>
		/// Стиль выделения парных скобок
		/// </summary>
		public Style BraceMatch => Styles[Highlight.BraceMatch];

		/// <summary>
		/// Стиль выделения найденного текста
		/// </summary>
		public Style FoundMatch => Styles[Highlight.FoundMatch];

		/// <summary>
		/// Стиль выделения ошибок
		/// </summary>
		public Style ErrorLine => Styles[Highlight.ErrorLine];

		/// <summary>
		/// Стиль номеров строк
		/// </summary>
		public Style LineNumbers => Styles[Highlight.LineNumbers];

		/// <summary>
		/// Массив стилей
		/// </summary>
		private readonly List<Style> Styles = new List<Style>();

		/// <summary>
		/// Массив стилей
		/// </summary>
		/// <param name="Index">Номер стиля</param>
		/// <returns>Стиль подсветки</returns>
		public Style this[int Index] => Styles[Index];

		/// <summary>
		/// Признак размытия текста
		/// </summary>
		internal TextFormattingMode TextSmoothing { get; set; }

		/// <summary>
		/// Значение масштаба пикелей
		/// </summary>
		internal double PixelsPerDip { get; set; }

		/// <summary>
		/// Текущий шрифт
		/// </summary>
		private Typeface CurrentTypeface;

		/// <summary>
		/// Возвращает форматированный текст 
		/// </summary>
		/// <param name="Str">Строка текста</param>
		/// <param name="Style">Номер стиля</param>
		/// <param name="bg">Кисть фона</param>
		/// <returns>Форматированный текст</returns>
		public FormattedText ReadyText(string Str, int Style, out Brush bg)
		{
			var S = Styles[Style];
			bg = S.Background;

			return ReadyText(Str, S);
		}

		/// <summary>
		/// Возвращает форматированный текст
		/// </summary>
		/// <param name="Str">Строка текста</param>
		/// <param name="S">Стиль</param>
		/// <returns>Форматированный текст</returns>
		public FormattedText ReadyText(string Str, Style S)
		{
			FormattedText ft = new FormattedText(Str, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight,
				CurrentTypeface, FontSize, S.Foreground, null, TextSmoothing, PixelsPerDip);

			if (S.Bold)
				ft.SetFontWeight(FontWeights.Bold);

			if (S.Italic)
				ft.SetFontStyle(FontStyles.Italic);

			if (S.Underline)
				ft.SetTextDecorations(TextDecorations.Underline);

			return ft;
		}

		/// <summary>
		/// Класс стиля
		/// </summary>
		public class Style
		{
			/// <summary>
			/// Имя стиля
			/// </summary>
			private string Name;

			/// <summary>
			/// Стиль печати или экпорта
			/// </summary>
			public Style Print { get; private set; }

			/// <summary>
			/// Создаёт стиль
			/// </summary>
			/// <param name="name">Имя стиля</param>
			/// <param name="background">Цвет фона</param>
			/// <param name="foreground">Цвет символов</param>
			/// <param name="bold">Признак полужирного начертания</param>
			/// <param name="italic">Признак курсивного начертания</param>
			/// <param name="underline">Признак подчёркивания</param>
			/// <param name="print">Стиль печати</param>
			internal Style(string name, Color foreground, Color background, 
				bool bold, bool italic, bool underline, Style print = null)
			{
				Name = name;
				if (print != null)
				{
					Print = print;
					Print.Name = Name + "." + "print";
				}

				Background = new SolidColorBrush(BackgroundField = background);
				Foreground = new SolidColorBrush(ForegroundField = foreground);

				if (bold)
					FontStyle |= 1;
				if (italic)
					FontStyle |= 2;
				if (underline)
					FontStyle |= 4;
			}

			/// <summary>
			/// Обновляет стиль из настроек
			/// </summary>
			internal void UpdateStyle()
			{
				string s = Settings[HighlightKey, Name, BackgroundKey];
				try
				{
					BackgroundField = (Color)ColorConverter.ConvertFromString(s);
					Background = new SolidColorBrush(BackgroundField);
				}
				catch (Exception)
				{
				}

				s = Settings[HighlightKey, Name, ForegroundKey];
				try
				{
					ForegroundField = (Color)ColorConverter.ConvertFromString(s);
					Foreground = new SolidColorBrush(ForegroundField);
				}
				catch (Exception)
				{
				}

				s = Settings[HighlightKey, Name, BoldKey];
				if (s == "1")
					FontStyle |= 1;
				else if (s != "")
					FontStyle &= ~1;

				s = Settings[HighlightKey, Name, ItalicKey];
				if (s == "1")
					FontStyle |= 2;
				else if (s != "")
					FontStyle &= ~2;

				s = Settings[HighlightKey, Name, UnderlineKey];
				if (s == "1")
					FontStyle |= 4;
				else if (s != "")
					FontStyle &= ~4;

				s = Settings[HighlightKey, Name, TagKey];
				if (s != "")
					int.TryParse(s, out TagField);


				Print?.UpdateStyle();
			}

			/// <summary>
			/// Цвет фона
			/// </summary>
			private Color BackgroundField;

			/// <summary>
			/// Цвет фона
			/// </summary>
			public Color BackgroundColor
			{
				get => BackgroundField;
				set => Settings[HighlightKey, Name, BackgroundKey] = value.ToString();
			}

			/// <summary>
			/// Кисть фона
			/// </summary>
			public Brush Background { get; private set; }

			/// <summary>
			/// Цвет символов
			/// </summary>
			private Color ForegroundField;

			/// <summary>
			/// Цвет символов
			/// </summary>
			public Color ForegroundColor
			{
				get => ForegroundField;
				set => Settings[HighlightKey, Name, ForegroundKey] = value.ToString();
			}

			/// <summary>
			/// Кисть символов
			/// </summary>
			public Brush Foreground { get; private set; }

			/// <summary>
			/// Стиль шрифта
			/// </summary>
			private int FontStyle = 0;

			/// <summary>
			/// Признак полужирного начертания
			/// </summary>
			public bool Bold
			{
				get => (FontStyle & 1) != 0; 
				set => Settings[HighlightKey, Name, BoldKey] = value ? "1" : "0";
			}

			/// <summary>
			/// Признак курсивного начертания
			/// </summary>
			public bool Italic
			{
				get => (FontStyle & 2) != 0;
				set => Settings[HighlightKey, Name, ItalicKey] = value ? "1" : "0";
			}

			/// <summary>
			/// Признак подчёркивания символов
			/// </summary>
			public bool Underline
			{
				get => (FontStyle & 4) != 0;
				set => Settings[HighlightKey, Name, UnderlineKey] = value ? "1" : "0";
			}

			/// <summary>
			/// Тег стиля
			/// </summary>
			internal int TagField;

			/// <summary>
			/// Тег стиля
			/// </summary>
			public int Tag
			{
				get => TagField;
				set => Settings[HighlightKey, Name, TagKey] = value.ToString();
			}

		}
	}

	/// <summary>
	/// Класс редактора с подсветкой и выделением
	/// </summary>
	public class EditControl : FrameworkElement, IDisposable, IEditorClient
	{
		/// <summary>
		/// Стили подсветки
		/// </summary>
		public readonly static HighlightStyles Styles = new HighlightStyles();

		/// <summary>
		/// Счётчик количества активных редакторов
		/// </summary>
		private static int InstanceCounter = 0;

		/// <summary>
		/// Интерфейс редактора
		/// </summary>
		public IScriptEditor Editor { get; private set; }

		/// <summary>
		/// Код регистрации клиента на сервере
		/// </summary>
		private readonly long ServerRegistration;

		/// <summary>
		/// Контекст синхронизации
		/// </summary>
		private readonly SynchronizationContext SyncCtx = SynchronizationContext.Current;

		/// <summary>
		/// Создаёт редактор
		/// </summary>
		public EditControl(IScriptEditor editor = null)
		{
			Styles.PixelsPerDip = VisualTreeHelper.GetDpi(this).PixelsPerDip;
			Styles.TextSmoothing = (TextFormattingMode)(1 - Server.GetFontSmoothing());
			InstanceCounter++;
			Styles.Changed += StylesChanged;

			Editor = editor ?? Server.CreateEditor();
			ServerRegistration = Editor.RegisterClient(this);
			Editor.SetTabWidth(Styles.TabWidth);

			VScroll.ValueChanged += VScrollValueChanged;
			HScroll.ValueChanged += HScrollValueChanged;

			AddVisualChild(VScroll);
			AddLogicalChild(VScroll);
			Children.Add(VScroll);

			AddVisualChild(HScroll);
			AddLogicalChild(HScroll);
			Children.Add(HScroll);

			Children.Add(Caret = new CaretControl(this));

			Focusable = true;
			FocusVisualStyle = null;

			SnapsToDevicePixels = true;
			SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);

			ScrollTimer.Elapsed += ScrollTimerElapsed;
            HoverTimer.Elapsed += HoverTimerElapsed;

			Hint = new ToolTip()
			{
				Placement = PlacementMode.Relative,
				PlacementTarget = this,
			};

			IdentPopup = new Popup()
			{
				Placement = PlacementMode.Relative,
				PlacementTarget = this,
			};

		}

        /// <summary>
        /// Текущее положение каретки
        /// </summary>
        public (int Line, int Char) CaretPos
		{
			get
			{
				Editor.GetCurrentPos(out int Line, out int Char, 0);
				return (Line + 1, Char + 1);
			}
		}

		/// <summary>
		/// Событие начала изменения редактора
		/// </summary>
		public event Action<EditControl> Changing;

		/// <summary>
		/// Событие окончания изменения редактора
		/// </summary>
		public event Action<EditControl> Changed;

		/// <summary>
		/// Событие изменения значения горизонтальной полосы прокрутки
		/// </summary>
		private void HScrollValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			Editor.SetLeftChar((int)HScroll.Value);
		}

		/// <summary>
		/// Событие изменения значения горизонтальной полосы прокрутки
		/// </summary>
		private void VScrollValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			Editor.SetTopLine((int)VScroll.Value);
		}

		/// <summary>
		/// Список внутренних компонентов
		/// </summary>
		private readonly List<Visual> Children = new List<Visual>();

		/// <summary>
		/// Количество внутренних визуальных элементов управления
		/// </summary>
		protected override int VisualChildrenCount => Children.Count;

		/// <summary>
		/// Возвращает внутренний визуальный элемент управления по его индексу
		/// </summary>
		protected override Visual GetVisualChild(int index) => Children[index];

		/// <summary>
		/// Выполняет перерисовку в ответ на изменение стилей
		/// </summary>
		private void StylesChanged(HighlightStyles obj)
		{
			Editor.SetTabWidth(obj.TabWidth);
			InvalidateArrange();
			InvalidateVisual();
		}

		/// <summary>
		/// Вертикальная полоса прокрутки
		/// </summary>
		private readonly ScrollBar VScroll = new ScrollBar
		{
			Orientation = Orientation.Vertical,
		};

		/// <summary>
		/// Горизонтальная полоса прокрутки
		/// </summary>
		private readonly ScrollBar HScroll = new ScrollBar
		{
			Orientation = Orientation.Horizontal,
		};

		/// <summary>
		/// Каретка
		/// </summary>
		private readonly CaretControl Caret;

		/// <summary>
		/// Всплывающая подсказка
		/// </summary>
		private readonly ToolTip Hint;

		/// <summary>
		/// Область, для которой показывается подсказка
		/// </summary>
		private Rect HintArea = new Rect();


		/// <summary>
		/// Освобождает ресурсы
		/// </summary>
		void IDisposable.Dispose()
		{
			Styles.Changed -= StylesChanged;

			Editor.UnregisterClient(ServerRegistration);
			Marshal.ReleaseComObject(Editor);
			Editor = null;

			if (--InstanceCounter == 0)
				(Styles as IDisposable).Dispose();
		}

		void IEditorClient.OnChanging()
		{
			SyncCtx.Send(_ =>
			{
				if (!IsFocused)
					Focus();

				Hint.IsOpen = false;
				Changing?.Invoke(this);
			}, this);
		}

		void IEditorClient.OnChanged()
		{
			SyncCtx.Post(_ =>
			{
				InvalidateArrange();
				InvalidateVisual();
				Changed?.Invoke(this);
				if (IdentPopup.IsOpen)
					(IdentPopup.Child as IdentListControl)?.Filter(CurrentWord());
			}, this);
		}

		void IEditorClient.ContextMenuRequest(int X, int Y)
		{
			Hole(this);
		}

		void IEditorClient.HintRequest(int X, int Y, int Width, int Height, int Away, string Str)
		{
			SyncCtx.Post(_ =>
			{
				if (Away == 0)
				{
					HintArea.X = X;
					HintArea.Y = Y;
					HintArea.Width = Width;
					HintArea.Height = Height;
				}
				else
				{
					var p = Mouse.GetPosition(this);
					var d = Distance(Mouse.GetPosition(this),
						X, Y, X + Width, Y + Height);
					if (d > 0)
					{
						HintArea.X = X;
						HintArea.Y = Y;
						HintArea.Width = Width;
						HintArea.Height = Height;
					}
					else
					{
						HintArea.X = p.X - CharHeight / 2;
						HintArea.Y = p.Y - CharHeight / 2;
						HintArea.Width = HintArea.Height = CharHeight;
						Mouse.Capture(this);
					}
				}

				Hint.IsOpen = false;

				Hint.Content = Str;
				Hint.PlacementRectangle = new Rect(X + Height * 0.618, Y + Height * 1.05, 0, 0);
				Hint.IsOpen = true;
			}, this);
		}

		/// <summary>
		/// Всплывающее окно со списком идентификаторов
		/// </summary>
		private readonly Popup IdentPopup;

		/// <summary>
		/// Закрывает список идентификаторов
		/// </summary>
		private void ClosePopup()
		{
			if (IdentPopup.IsOpen)
			{
				LockMouseMove = true;
				IdentPopup.IsOpen = false;
				(IdentPopup.Child as IDisposable).Dispose();
				IdentPopup.Child = null;
				
				
			}
		}

		/// <summary>
		/// Признак ввода текста из списка идентификаторов.
		/// Блокирует событие движения мыши при закрытии списка идентификаторов
		/// </summary>
		private bool LockMouseMove = false;

		/// <summary>
		/// Событие ввода идентификатора из списка
		/// </summary>
		/// <param name="List">Список</param>
		/// <param name="Ident">Идентификатор</param>
		private void InputPopupIdent(IdentListControl List, string Ident)
		{
			Editor.ReplaceCurrentWord(Ident);
		}

		/// <summary>
		/// Возвращает текущее слово в редакторе
		/// </summary>
		/// <returns></returns>
		private string CurrentWord()
		{
			return Editor.GetWordAt(Editor.GetCurrentLine(1), Editor.GetCurrentChar(1), 1, out int Pos, out int Style);
		}

		void IEditorClient.IdentListRequest(int X, int Y, IIdentList Idents)
		{
			var List = new IdentListControl
			{
				CloseRequest = ClosePopup,
				InputIdent = InputPopupIdent,

			};
			List.Bind(Idents);
			IdentPopup.Child = List;
			IdentPopup.PlacementRectangle = new Rect(X + CharHeight * 0.618, Y + CharHeight * 1.05, 0, 0);
			IdentPopup.IsOpen = true;


		}

		/// <summary>
		/// Тип события запроса указателя на интерфейс редактора по имени файла
		/// </summary>
		/// <param name="FileName">Имя файла</param>
		/// <param name="Editor">Указатель на интерфейс редактора</param>
		public delegate void EditorRequestType(string FileName, ref IScriptEditor Editor);

		/// <summary>
		/// Событие запроса указателя на интерфейс редактора по имени файла
		/// </summary>
		public event EditorRequestType EditorRequest;

		IScriptEditor IEditorClient.GetEditor(string FileName)
		{
			IScriptEditor Res = null;
			EditorRequest?.Invoke(FileName, ref Res);
			return Res;
		}

		/// <summary>
		/// Событие запроса на отображение редактора 
		/// </summary>
		public event Action<EditControl> DisplayRequest;

		/// <summary>
		/// Показывает строку с ошибкой выполнения скрипта
		/// </summary>
		/// <param name="Line">Номер строки</param>
		/// <param name="Char">Позиция символа</param>
		void IEditorClient.DisplayError(int Line, int Char)
		{
			DisplayRequest?.Invoke(this);
			ErrorLine = Line;
			Editor.SetCurrentPos(Line, Char, 0);
			Editor.ScrollCaretIntoView();
		}

		/// <summary>
		/// Позиция ошибки выполнения скрипта
		/// </summary>
		private int ErrorLine = -2;

		/// <summary>
		/// Показывает точку останова
		/// </summary>
		/// <param name="Line">Номер строки</param>
		/// <param name="Char">Позиция символа</param>
		void IEditorClient.DisplayTrace(int Line, int Char)
		{
			TraceLine = Line;
			if (Line >= 0)
			{
				DisplayRequest?.Invoke(this);
				Editor.SetCurrentPos(Line, Char, 0);
				Editor.ScrollCaretIntoView();
			}
			else
			{
				InvalidateArrange();
				InvalidateVisual();
			}
		}

		/// <summary>
		/// Точка трассировки
		/// </summary>
		private int TraceLine = -2;

		/// <summary>
		/// Событие получения фокуса
		/// </summary>
		protected override void OnGotFocus(RoutedEventArgs e)
		{
			if (Editor == null)
				e.Handled = true;
			else
			{
				e.Handled = Editor.OnFocused() != 0;
				InvalidateArrange();
				InvalidateVisual();
			}
		}

		/// <summary>
		/// Событие потери фокуса
		/// </summary>
		protected override void OnLostFocus(RoutedEventArgs e)
		{
			if (Editor == null)
				e.Handled = true;
			else
			{
				e.Handled = Editor.OnDefocused() != 0;

				InvalidateArrange();
				InvalidateVisual();
			}
			Hint.IsOpen = false;
			ClosePopup();
		}

		/// <summary>
		/// Текущая ширина символа
		/// </summary>
		private int CharWidth;

		/// <summary>
		/// Текущая высота символа
		/// </summary>
		private int CharHeight;

		/// <summary>
		/// Количество видимых строк текста
		/// </summary>
		private int VisibleLineCount;

		/// <summary>
		/// Количество видимых символов строки
		/// </summary>
		private int VisibleCharCount;

		/// <summary>
		/// Ширина правого поля
		/// </summary>
		private int LeftPadding;

		/// <summary>
		/// Ширина поля номеров строк
		/// </summary>
		private int RulerWidth;

		/// <summary>
		/// Выполняет расстановку внутренних визуальных элементов управления
		/// </summary>
		protected override Size ArrangeOverride(Size finalSize)
		{
			FormattedText ft = new FormattedText("W", CultureInfo.CurrentUICulture,
				FlowDirection.LeftToRight, new Typeface(Styles.FontName), Styles.FontSize, Brushes.Black,
				null, Styles.TextSmoothing = (TextFormattingMode)(1 - Server.GetFontSmoothing()),
			Styles.PixelsPerDip = VisualTreeHelper.GetDpi(this).PixelsPerDip);

			CharWidth = (int)Math.Ceiling(ft.Width);
			CharHeight = (int)Math.Ceiling(ft.Height);

			double X = SystemParameters.VerticalScrollBarWidth,
				Y = SystemParameters.HorizontalScrollBarHeight;

			var r = new Rect(finalSize.Width - X, 0, X, finalSize.Height - Y);
			VScroll.Arrange(r);

			r = new Rect(0, finalSize.Height - Y, finalSize.Width - X, Y);
			HScroll.Arrange(r);

			VisibleLineCount = (int)((finalSize.Height - Y)) / CharHeight;

			LeftPadding = (int)X;

			int LineCount = Editor.GetLineCount();

			RulerWidth = Styles.LineNumbers.Tag != 0 && LineCount != 0 ? (int)Math.Truncate(Math.Log10(Editor.GetLineCount()) + 1) + 1 : 1;

			VisibleCharCount = (int)((finalSize.Width - 2 * X) / CharWidth) - RulerWidth;

			RulerWidth *= CharWidth;
			RulerWidth += 1;

			Editor.GetMaxPos(out int MaxLeft, out int MaxTop);

			VScroll.Minimum = 0;
			if (MaxTop > 0) 
			{
				VScroll.IsEnabled = true;
				VScroll.Maximum = MaxTop; 
				VScroll.ViewportSize = VisibleLineCount;
				VScroll.SmallChange = 1;
				VScroll.LargeChange = VScroll.ViewportSize;
				VScroll.Value = Editor.GetTopLine();
			}
			else
			{
				VScroll.Value = 0;
				VScroll.Maximum = 0;
				VScroll.IsEnabled = false;
			}

			HScroll.Minimum = 0;
			if (MaxLeft > 0)
			{
				HScroll.IsEnabled = true;
				HScroll.Maximum = MaxLeft;
				HScroll.ViewportSize = VisibleCharCount + 1;
				HScroll.LargeChange = HScroll.ViewportSize;
				HScroll.SmallChange = 1;
				HScroll.Value = Editor.GetLeftChar();
			}
			else
			{
				HScroll.Value = 0;
				HScroll.Maximum = 0;
				HScroll.IsEnabled = false;
			}

			Editor.GeometryReport((int)RenderSize.Width, (int)RenderSize.Height,
				LeftPadding, RulerWidth, (int)X, (int)Y, CharWidth, CharHeight, VisibleLineCount, VisibleCharCount);


			if (IsFocused)
			{
				Editor.GetCurrentPos(out int Line, out int Char, 1);

				if (InsideRange(Char, 0, VisibleCharCount) && InsideRange(Line, 0, VisibleLineCount))
				{
					Caret.Active = true;
					r.Y = Line * CharHeight;
					r.X = Char * CharWidth + LeftPadding + RulerWidth;
					r.Width = CharWidth;
					r.Height = CharHeight;
					Caret.Arrange(r);
					Caret.RectangleMode = Editor.GetInsertMode() == 0;
				}
				else
					Caret.Active = false;
			}

			return finalSize;
		}

		/// <summary>
		/// Видимые блоки подсветки
		/// </summary>
		private List<(int Y, int X, FormattedText Str, Brush Background)> TextBlocks =
			new List<(int Y, int X, FormattedText Str, Brush Background)>();

		/// <summary>
		/// Координаты левого верхнего символа
		/// </summary>
		private int LeftChar, TopLine;

		/// <summary>
		/// Перечисляет блоки подсветки
		/// </summary>
		void IEditorClient.HighlightEnum(int Line, int Char, int Style, string Str)
		{
			var ft = Styles.ReadyText(Str, Line != ErrorLine ? Style : Highlight.ErrorLine, out Brush b);
			TextBlocks.Add(((Line - TopLine) * CharHeight, (Char - LeftChar) * CharWidth + LeftPadding + RulerWidth, ft, b));
		}

		/// <summary>
		/// Картинка-красный кружок для точек прерывания
		/// </summary>
		private BitmapSource Breakpoint;

		/// <summary>
		/// Последнее значение размера точки прерывания
		/// </summary>
		private int RecentBreakpointSize = 0;

		/// <summary>
		/// Последнеe значение цвета точки прерывания
		/// </summary>
		private Color RecentBreakpointColor;

		private int RecentTracepointSize = 0;

		private BitmapSource Tracepoint;

		private BitmapSource DrawTracepoint(int Size)
		{
			if (Size != RecentTracepointSize )
			{
				RecentTracepointSize = Size;

				DrawingVisual dv = new DrawingVisual();
				DrawingContext dc = dv.RenderOpen();

				double v = Size;
				var g = new PathGeometry();
				var f = new PathFigure();
				f.StartPoint = new Point(1, v / 3);
				var s = new PolyLineSegment();
				s.IsStroked = true;
				s.IsSmoothJoin = true;
				s.Points.Add(new Point(v / 2, v / 3));
				s.Points.Add(new Point(v / 2, 0));
				s.Points.Add(new Point(v, v / 2));
				s.Points.Add(new Point(v / 2, v));
				s.Points.Add(new Point(v / 2, 2 * v / 3));
				s.Points.Add(new Point(1, 2 * v / 3));
				s.Points.Add(new Point(1, v / 3));
				f.Segments.Add(s);
				g.Figures.Add(f);

				dc.DrawGeometry(new SolidColorBrush(Colors.Yellow), 
					new Pen(new SolidColorBrush(Colors.DarkOrange), 1), g);
				dc.Close();

				var dpi = VisualTreeHelper.GetDpi(this);
				RenderTargetBitmap bmp = new RenderTargetBitmap(Size, Size, dpi.PixelsPerInchX, dpi.PixelsPerInchY, PixelFormats.Default);

				bmp.Render(dv);
				bmp.Freeze();

				Tracepoint = bmp;
			}
			return Tracepoint;
		}

		/// <summary>
		/// Возвращает картинку для точки прерывания
		/// Они не смогли обеспечить возмножности рисовать чёткие горизонтальные и вертикальные линии
		/// при размытых других примитивах. RenderOptions действуют глобально. Отсюда необходимость
		/// рисовать кружок с размытием границы отдельно, а потом отображать его как Bitmap
		/// </summary>
		/// <param name="Size">Размер</param>
		/// <param name="c">Цвет</param>
		/// <returns>Картинка</returns>
		private BitmapSource DrawBreakpoint(int Size, Color c)
		{
			if (Size != RecentBreakpointSize || c != RecentBreakpointColor)
			{
				RecentBreakpointSize = Size;
				RecentBreakpointColor = c;

				DrawingVisual dv = new DrawingVisual();
				DrawingContext dc = dv.RenderOpen();

				double x = Size / 2, s = x - 2.5;
				dc.DrawEllipse(new SolidColorBrush(c), null, new Point(x, x), s, s);
				dc.Close();

				var dpi = VisualTreeHelper.GetDpi(this);
				RenderTargetBitmap bmp = new RenderTargetBitmap(Size, Size, dpi.PixelsPerInchX, dpi.PixelsPerInchY, PixelFormats.Default);
				
				bmp.Render(dv);
				bmp.Freeze();

				Breakpoint = bmp;

				//BitmapSource image = bmp;
				//using (var fileStream = new FileStream(@"\\spb.gpsm.ru\dfs\groups1\strallan\Sample\dot.png", FileMode.Create))
				//{
				//	BitmapEncoder encoder = new PngBitmapEncoder();
				//	encoder.Frames.Add(BitmapFrame.Create(image));
				//	encoder.Save(fileStream);
				//}

			}
			return Breakpoint;
		}

		/// <summary>
		/// Рисует редактор
		/// </summary>
		/// <param name="dc"></param>
		protected override void OnRender(DrawingContext dc)
		{
			int n = TextBlocks.Count;
			TextBlocks.Clear();
			TextBlocks.Capacity = Math.Max(40, n * 2);

			Editor.GetTopLeftPos(out TopLine, out LeftChar);
			Editor.GetCurrentPos(out int LinePos, out int CharPos, 1);

			int LineCount = Editor.GetLineCount();

			Editor.HighlightEnum(LeftChar, TopLine, VisibleCharCount, VisibleLineCount, this);

			Rect r = new Rect(0, 0, RenderSize.Width, RenderSize.Height);

			Point p = new Point(0, 0), p2 = new Point(0, 0);

			dc.PushClip(new RectangleGeometry(r));

			dc.DrawRectangle(Styles.Space.Background, null, r);

			r.Width = LeftPadding;

			dc.DrawRectangle(HScroll.Background, null, r);

			if (LinePos >= 0 && LinePos < VisibleLineCount)
			{
				Rect r1 = new Rect(2, LinePos * CharHeight + 2, LeftPadding - 4, CharHeight - 4);
				var c = SystemColors.ScrollBarBrush.Color;
				c.A = 128;
				var Br = new SolidColorBrush(c);
				dc.DrawRectangle(Br, null, r1);
			}


			{
				var bs = Math.Min(LeftPadding, CharHeight);
				var bp = DrawBreakpoint(bs, Styles.ErrorLine.BackgroundColor);
				var bt = DrawTracepoint(bs * 3 / 4);
				
				var r2 = new Rect(0, (CharHeight - bs) / 2, bs, bs);
				for (int i = 0; i < VisibleLineCount; i++, r2.Y += CharHeight)
				{
					if (Editor.IsBreakpoint(i, 1) != 0)
					{
						dc.DrawImage(bp, r2);
					}
					if (TopLine + i == TraceLine)
					{
						var r4 = new Rect
						{
							X = r2.X + (r2.Width - bt.PixelWidth) / 2,
							Y = r2.Y + (r2.Height - bt.PixelHeight) / 2,
							Width = bt.PixelWidth,
							Height = bt.PixelHeight
						};

						dc.DrawImage(bt, r4);
					}
					if (TopLine + i == ErrorLine)
					{
						var r3 = new Rect();
						r3.X = LeftPadding + RulerWidth;
						r3.Y = i * CharHeight;
						r3.Width = RenderSize.Width - r3.X - VScroll.Width;
						r3.Height = CharHeight;
						dc.DrawRectangle(Styles.ErrorLine.Background, null, r3);
						ErrorLine = -2;
					}
				}
			}


			r.X = r.Width;
			r.Width = RulerWidth;
			dc.DrawRectangle(Styles.LineNumbers.Background, null, r);

			Pen gp = new Pen(Styles.LineNumbers.Foreground, 1);
			p.X = p2.X = r.Right - 2;
			p.Y = 1;
			p2.Y = RenderSize.Height - HScroll.Height - 1;
			dc.DrawLine(gp, p, p2);

			if (Styles.LineNumbers.Tag != 0)
			{
				p.Y = 0;
				for (int i = TopLine, nl = Math.Min(LineCount, TopLine + VisibleLineCount); i < nl; i++, p.Y += CharHeight)
				{
					string s = "·";
					var j = i + 1;
					if (Styles.LineNumbers.Tag == 1 || (j % 10) == 0 || i == LineCount - 1 || i == TopLine + LinePos)
						s = j.ToString();
					else if (Styles.LineNumbers.Tag == 2 && (j % 5) == 0)
						s = "–";

					var ft = Styles.ReadyText(s, Styles.LineNumbers);
					p.X = r.Right - CharWidth / 2 - ft.Width - 1;
					dc.DrawText(ft, p);
				}
			}

			r.X = RenderSize.Width - (r.Width = VScroll.Width);
			r.Y = RenderSize.Height - (r.Height = HScroll.Height);
			dc.DrawRectangle(VScroll.Background, null, r);

			foreach ((int Y, int X, FormattedText Str, Brush Background) in TextBlocks)
			{
				r.X = X;
				if (Str.Text == "")
				{
					r.X -= 1;
					r.Width = 1;
				}
				else
					r.Width = Str.WidthIncludingTrailingWhitespace;
				r.Y = Y;
				
				r.Height = CharHeight;
				dc.DrawRectangle(Background, null, r);
				dc.DrawText(Str, r.TopLeft);
			}

	

			var m = Styles.RightMargin - LeftChar;
			if (m > 0 && m < VisibleCharCount)
			{
				Pen gp1 = new Pen(Styles.LineNumbers.Foreground, 1)
				{
					DashStyle = new DashStyle(new double[] { 1, 3 }, 0),
					DashCap = PenLineCap.Flat,
				};
				gp1.Brush.SetValue(OpacityProperty, 0.1);
				p.X = p2.X = m * CharWidth + LeftPadding + RulerWidth;
				p.Y = 1;
				p2.Y = RenderSize.Height - HScroll.Height - 1;
				dc.DrawLine(gp1, p, p2);
			}
		}


		/// <summary> 
		/// Состояние клавиш-переключателей
		/// </summary>
		internal static int ShiftState
		{
			get
			{
				int Shift = (int)Keyboard.Modifiers;
				if (Mouse.LeftButton == MouseButtonState.Pressed)
					Shift |= ShiftKey.MouseLeft;
				if (Mouse.RightButton == MouseButtonState.Pressed)
					Shift |= ShiftKey.MouseRight;
				if (Mouse.MiddleButton == MouseButtonState.Pressed)
					Shift |= ShiftKey.MouseMiddle;
				if (Mouse.XButton1 == MouseButtonState.Pressed)
					Shift |= ShiftKey.MouseX1;
				if (Mouse.XButton2 == MouseButtonState.Pressed)
					Shift |= ShiftKey.MouseX2;
				return Shift;
			}
		}

		/// <summary>
		/// Событие нажатия клавиши
		/// </summary>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			var Shift = ShiftState;
			if (IdentPopup.IsOpen)
			{
				(IdentPopup.Child as IdentListControl)?.HostKeyDown(e, Shift);
				if (e.Handled)
					return;
			}
			if (Hint.IsOpen && Mouse.Captured == this)
				Mouse.Capture(null);
			Hint.IsOpen = false;
			e.Handled = Editor.OnKeyDown((int)(e.SystemKey != 0 ? e.SystemKey : e.Key), Shift) != 0;

		}

		/// <summary>
		/// Событие отпускания клавиши
		/// </summary>
		protected override void OnKeyUp(KeyEventArgs e)
		{
			e.Handled = Editor.OnKeyUp((int)e.Key, ShiftState) != 0;
		}

		/// <summary>
		/// Событие ввода текста
		/// </summary>
		protected override void OnTextInput(TextCompositionEventArgs e)
		{
			string str = e.Text;
			if (IdentPopup.IsOpen)
			{
				for (int i = 0, n = str.Length; i < n; i++)
				{
					if (!IsValidIdentChar(str[i]))
					{
						ClosePopup();
						break;
					}
				}
			}
			e.Handled = Editor.OnTextInput(str, ShiftState) != 0;
		}

		/// <summary>
		/// Событие нажатия кнопки мыши
		/// </summary>
		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			ClosePopup();

			if (!IsFocused)
				Focus();
			var p = e.GetPosition(this);
			var d = Distance(p, LeftPadding + RulerWidth, 0, Width - VScroll.Width, Height - HScroll.Height);
			var d1 = Distance(p, LeftPadding, Height - HScroll.Height);
			if (d > 0 || d1 > 0)
			{
				Mouse.Capture(this);

				e.Handled = Editor.OnMouseDown((ShiftKey.MouseLeft << (int)e.ChangedButton) | (e.ClickCount << 16),
					(int)p.X, (int)p.Y, ShiftState) != 0;
			}

		}

		/// <summary>
		/// Cобытие отпускания кнопки мыши
		/// </summary>
		protected override void OnMouseUp(MouseButtonEventArgs e)
		{
			Mouse.Capture(null);
			var p = e.GetPosition(this);
			e.Handled = Editor.OnMouseUp(ShiftKey.MouseLeft << (int)e.ChangedButton, (int)p.X, (int)p.Y, ShiftState) != 0;
		}

		/// <summary>
		/// Координаты последнего движения мыши
		/// </summary>
		private Point RecentMouseMovePoint = new Point(0, 0);

		/// <summary>
		/// Счётчик времени в момент последнего движения мыши
		/// </summary>
		private ulong RecentMouseMoveTick;

        /// <summary>
        /// Таймер зависания мыши
        /// </summary>
        private readonly System.Timers.Timer HoverTimer = new System.Timers.Timer
        {
            Enabled = false,
            Interval = 250,
            AutoReset = true
        };

        /// <summary>
        /// Событие таймера зависания мыши
        /// </summary>
        private void HoverTimerElapsed(object sender, ElapsedEventArgs e)
        {
            SyncCtx.Post(_ =>
            {
                if (GetTickCount64() - RecentMouseMoveTick > 1000)
                {
                    var p = Mouse.GetPosition(this);
                    if (Distance(p, this.RecentMouseMovePoint) < 3)
                    {
                        Editor.OnMouseHover((int)p.X, (int)p.Y, ShiftState, 
							Project.IsRunning && Project.IsSuspended ? Model : null);
                        RecentMouseMovePoint.X = -1000;
                    }
                }
            }, this);
        }



		/// <summary>
		/// Событие движения мыши
		/// </summary>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (LockMouseMove)
			{
				LockMouseMove = false;
				e.Handled = true;
				return;
			}

			var p = e.GetPosition(this);

			if (Hint.IsOpen && Distance(p, HintArea) < 0)
			{
				Hint.IsOpen = false;
				Mouse.Capture(null);
			}

			if (Mouse.Captured != this)
			{
				if (p.X > LeftPadding + RulerWidth && p.X < ActualWidth - VScroll.Width &&
					p.Y > 0 && p.Y < ActualHeight - HScroll.Height)
				{
					Cursor = Cursors.IBeam;

					RecentMouseMovePoint.X = p.X;
					RecentMouseMovePoint.Y = p.Y;
					RecentMouseMoveTick = GetTickCount64();
				}
				else
				{
					Cursor = null;
					ScrollTimer.Enabled = false;
				}
			}
			else
			{
				var d = Distance(p, ActualWidth, ActualHeight);
				if (d < 0)
				{
					ScrollTimer.Interval = -d > CharHeight * 2 ? 30 : 100;
					ScrollTimer.Enabled = true;
					e.Handled = true;
					return;
				}
			}
			e.Handled = e.Source == this && Editor.OnMouseMove((int)p.X, (int)p.Y, ShiftState) != 0;
		}

		/// <summary>
		/// Cобытие обработки колеса мыши
		/// </summary>
		protected override void OnMouseWheel(MouseWheelEventArgs e)
		{
			if (IdentPopup.IsOpen)
			{
				(IdentPopup.Child as IdentListControl)?.HostMouseWheel(e);
			}
			else
			{
				RecentMouseMoveTick = GetTickCount64();
				var p = e.GetPosition(this);
				e.Handled = Editor.OnMouseWheel(-Math.Sign(e.Delta), (int)p.X, (int)p.Y, ShiftState) != 0;
			}
		}

		/// <summary>
		/// Событие прихода курсора мыши
		/// </summary>
		protected override void OnMouseEnter(MouseEventArgs e)
		{
            HoverTimer.Enabled = true;
			e.Handled = Editor.OnMouseEnter(ShiftState) != 0;
		}

		/// <summary>
		/// Событие ухода курсора мыши
		/// </summary>
		protected override void OnMouseLeave(MouseEventArgs e)
		{
			e.Handled = Editor.OnMouseLeave(ShiftState) != 0;
			ScrollTimer.Enabled = false;
            HoverTimer.Enabled = false;
		}

		/// <summary>
		/// Таймер прокрутки, использующийся когда курсор мыши с нажатой левой кнопкой уходит за границы окна
		/// </summary>
		private readonly System.Timers.Timer ScrollTimer = new System.Timers.Timer()
		{
			AutoReset = true,
			Enabled = false,
		};

		private void ScrollTimerElapsed(object sender, ElapsedEventArgs e)
		{
			SyncCtx.Post(_ =>
			{
				var n = Mouse.GetPosition(this);
				var d = Distance(n, ActualWidth, ActualHeight);
				if (d > 0)
				{
					ScrollTimer.Enabled = false;
				}
				else
				{
					ScrollTimer.Interval = -d > CharHeight * 2 ? 30 : 100;
					ScrollTimer.Enabled = true;

					Editor.OnMouseMove((int)n.X, (int)n.Y, ShiftState);
				}
				
			}, this);
		}

		/// <summary>
		/// Класс каретки
		/// </summary>
		private class CaretControl : FrameworkElement
		{
			/// <summary>
			/// Редактор текста 
			/// </summary>
			private readonly EditControl Master;

			/// <summary>
			/// Таймер моргания
			/// </summary>
			private System.Timers.Timer Timer;

			/// <summary>
			/// Создаёт каретку
			/// </summary>
			/// <param name="master">Редактор текста</param>
			public CaretControl(EditControl master)
			{
				Master = master;
				
				Master.AddVisualChild(this);
				Master.AddLogicalChild(this);

				Timer = new System.Timers.Timer()
				{
					Interval = 187.5,
					AutoReset = true,
					Enabled = false,
				};

				Timer.Elapsed += TimerElapsed;
	

				SnapsToDevicePixels = true;
				SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);

				Visibility = Visibility.Hidden;
			}

			/// <summary>
			/// Признак активности каретки
			/// </summary>
			public bool Active
			{
				get => Timer.Enabled;
				set
				{
					if (Timer.Enabled && value)
						Timer.Enabled = false;
					if (Timer.Enabled != value)
					{
						if (Timer.Enabled = value)
							Visibility = Visibility.Visible;
						else
							Visibility = Visibility.Hidden;
					}
				}
			}

			/// <summary>
			/// Определяет, что каретка рисуется прямоугольником вокруг текущего символа
			/// </summary>
			public bool RectangleModeField = false;

			/// <summary>
			/// Определяет, что каретка рисуется прямоугольником вокруг текущего символа
			/// </summary>
			public bool RectangleMode
			{
				get => RectangleModeField;
				set
				{
					if (RectangleModeField != value)
					{
						RectangleModeField = value;
						InvalidateVisual();
					}
				}
			}

			/// <summary>
			/// Счётчик таймера
			/// </summary>
			private ulong Counter = 0;

			/// <summary>
			/// Событие таймера
			/// </summary>
			private void TimerElapsed(object sender, ElapsedEventArgs e)
			{
				Master.SyncCtx.Post(_ =>
				{
					if ((++Counter & 3) == 0)
						Visibility = (Visibility)(((int)Visibility + 1) & 1);

				}, this);
			}

			// Метод отрисовки
			protected override void OnRender(DrawingContext dc)
			{
				Pen p = new Pen(Styles.Space.Foreground, 1);
				Point p1 = new Point(0, 0), p2 = new Point(0, RenderSize.Height);
				dc.DrawLine(p, p1, p2);
				if (RectangleMode)
				{
					p1.X = p2.X;
					p1.Y = p2.Y;
					p2.X = RenderSize.Width + 1;
					dc.DrawLine(p, p1, p2);

					p1.X = p2.X;
					p1.Y = p2.Y;
					p2.Y = 1;
					dc.DrawLine(p, p1, p2);

					p1.X = p2.X;
					p1.Y = p2.Y;
					p2.X = 0;
					dc.DrawLine(p, p1, p2);
				}
			}
		}


	}

	/// <summary>
	/// Класс всплывающего списка доступных идентификаторов
	/// </summary>
	public class IdentListControl : FrameworkElement, IDisposable
	{
		/// <summary>
		/// Имя шрифта. По умолчанию берётся свойство стиля главной формы приложения
		/// </summary>
		public static readonly DependencyProperty FontNameProperty;

		/// <summary>
		/// Размер шрифта. По умолчанию берётся свойство стиля главной формы приложения
		/// </summary>
		public static readonly DependencyProperty FontSizeProperty;

		/// <summary>
		/// Цвет фона
		/// </summary>
		public static readonly DependencyProperty BackgroundProperty;

		/// <summary>
		/// Цвет шрифта
		/// </summary>
		public static readonly DependencyProperty ForegroundProperty;

		/// <summary>
		/// Цвет фона выделения
		/// </summary>
		public static readonly DependencyProperty SelectionBackgroundProperty;

		/// <summary>
		/// Цвет символов выделения
		/// </summary>
		public static readonly DependencyProperty SelectionForegroundProperty;

		/// <summary>
		/// Свойство Padding для строк списка
		/// </summary>
		public static readonly DependencyProperty LinePaddingProperty;

		/// <summary>
		/// Количество видимых строк списка
		/// </summary>
		public static readonly DependencyProperty VisibleRowCountProperty;

		/// <summary>
		/// Текущий номер строки
		/// </summary>
		public static readonly DependencyProperty CurrentIndexProperty;

		/// <summary>
		/// Регистрирует свойство зависимости
		/// </summary>
		/// <typeparam name="T">Тип свойства</typeparam>
		/// <param name="Name">Имя свойства</param>
		/// <param name="Arrange">Определяет влияние свойства на внутреннюю разметку таблицы</param>
		/// <param name="Render">Определяет влияние свойства на перерисовку таблицы</param>
		/// <param name="Default">Значение свойства по умолчанию</param>
		/// <returns>Объект свойства зависимости</returns>
		private static DependencyProperty PropReg<T>(string Name, bool Measure, bool Arrange, bool Render, T Default)
		{
			FrameworkPropertyMetadata fpm = new FrameworkPropertyMetadata
			{
				AffectsArrange = Arrange,
				AffectsMeasure = Measure,
				AffectsRender = Render,
				DefaultValue = Default,
			};

			return DependencyProperty.Register(Name, typeof(T), typeof(IdentListControl), fpm);
		}

		/// <summary>
		/// Статический конструктор, регистрирующий свойства зависимости
		/// </summary>
		static IdentListControl()
		{
			FontNameProperty = PropReg("FontName", true, true, true, "");
			FontSizeProperty = PropReg("FontSize", true, true, true, 0);
			BackgroundProperty = PropReg("Background", false, false, true, (Brush)SystemColors.WindowBrush);
			ForegroundProperty = PropReg("Foreground", false, false, true, (Brush)SystemColors.WindowTextBrush);
			SelectionBackgroundProperty = PropReg("SelectionBackground", false, false, true, (Brush)SystemColors.HighlightBrush);
			SelectionForegroundProperty = PropReg("SelectionForeground", false, false, true, (Brush)SystemColors.HighlightTextBrush);
			LinePaddingProperty = PropReg("LinePadding", true, true, true, 3);
			VisibleRowCountProperty = PropReg("VisibleRowCount", true, true, true, 5);
			CurrentIndexProperty = PropReg("CurrentIndexProperty", false, false, true, 0);
		}

		/// <summary>
		/// Имя шрифта
		/// </summary>
		public string FontName { get => (string)GetValue(FontNameProperty); set => SetValue(FontNameProperty, value); }

		/// <summary>
		/// Размер шрифта
		/// </summary>
		public int FontSize { get => (int)GetValue(FontSizeProperty); set => SetValue(FontSizeProperty, value); }

		/// <summary>
		/// Кисть фона выделенных ячеек
		/// </summary>
		public Brush SelectionBackground { get => (Brush)GetValue(SelectionBackgroundProperty); set => SetValue(SelectionBackgroundProperty, value); }

		/// <summary>
		/// Кисть символов выделенных ячеек
		/// </summary>
		public Brush SelectionForeground { get => (Brush)GetValue(SelectionForegroundProperty); set => SetValue(SelectionForegroundProperty, value); }

		/// <summary>
		/// Кисть фона ячеек
		/// </summary>
		public Brush Background { get => (Brush)GetValue(BackgroundProperty); set => SetValue(BackgroundProperty, value); }

		/// <summary>
		/// Кисть символов ячеек
		/// </summary>
		public Brush Foreground { get => (Brush)GetValue(ForegroundProperty); set => SetValue(ForegroundProperty, value); }

		/// <summary>
		/// Padding для строк таблицы
		/// </summary>
		public int LinePadding { get => (int)GetValue(LinePaddingProperty); set => SetValue(LinePaddingProperty, value); }

		/// <summary>
		/// Количество видимых строк списка
		/// </summary>
		public int VisibleRowCount { get => (int)GetValue(VisibleRowCountProperty); set => SetValue(VisibleRowCountProperty, value); }

		/// <summary>
		/// Текущий номер строки
		/// </summary>
		public int CurrentIndex
		{
			get => (int)GetValue(CurrentIndexProperty);
			set
			{
				int C = Math.Max(0, Math.Min(IdentList.GetCount(), value));

				if (C < ScrollTop)
					ScrollTop = C - 2;

				if (C >= ScrollTop + VisibleRowCount)
					ScrollTop = C - VisibleRowCount + 3;

				SetValue(CurrentIndexProperty, C);
			}
		}
		
		/// <summary>
		/// Вертикальная полоса прокрутки
		/// </summary>
		private readonly ScrollBar VScroll = new ScrollBar
		{
			Orientation = Orientation.Vertical,
		};

		/// <summary>
		/// Список внутренних компонентов
		/// </summary>
		private readonly List<Visual> Children = new List<Visual>();

		// Количество внутренних визуальных элементов управления
		protected override int VisualChildrenCount => Children.Count;

		// Возвращает внутренний визуальный элемент управления по его индексу
		protected override Visual GetVisualChild(int index) => Children[index];

		/// <summary>
		/// Создаёт список
		/// </summary>
		public IdentListControl()
		{
			// Назначение событий полос прокрутки
			VScroll.ValueChanged += VScrollValueChanged;


			// Объявляем вертикальную полосу прокрутки вложенным элементов управления
			AddVisualChild(VScroll);
			AddLogicalChild(VScroll);


			Focusable = false;
			FocusVisualStyle = null;

			// Линии рисуются кристально ровными
			SnapsToDevicePixels = true;
			SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);

			Children.Add(VScroll);

		}

		/// <summary>
		/// Событие изменения значения вертикальной полосы прокрутки
		/// </summary>
		private void VScrollValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			InvalidateArrange();
			InvalidateVisual();
		}

		/// <summary>
		///	Интерфейс списка идентификаторов
		/// </summary>
		public IIdentList IdentList { get; private set; }

		/// <summary>
		/// Связывает список с источником данных
		/// </summary>
		/// <param name="List">Интерфейс источника данных</param>
		public void Bind(IIdentList List)
		{
			IdentList = List;
			Filter(List.GetCurrent(out int Char));
		}

		/// <summary>
		/// Признак размытия шрифта
		/// </summary>
		private TextFormattingMode TextSmoothing;

		/// <summary>
		/// Текущая высота строки таблицы
		/// </summary>
		public int RowHeight { get; private set; }


		/// <summary>
		/// Верхняя видимая строка
		/// </summary>
		public int ScrollTop { get => (int)Math.Round(VScroll.Value); set => VScroll.Value = Math.Max(VScroll.Minimum, Math.Min(VScroll.Maximum, value)); }

		// Уставнавливает размеры окна
		protected override Size MeasureOverride(Size availableSize)
		{
			if (IdentList != null)
			{
				// Если не определены параметры шрифта, они определяются по свойствам главной формы приложения
				if (FontName == "" || FontSize == 0)
				{
					// Получаем главную форму приложения
					var MainForm = Application.Current.MainWindow;
					if (MainForm != null)
					{
						// Получаем имя шрифта
						FontName = (MainForm.GetValue(Control.FontFamilyProperty) as FontFamily).ToString();

						// Получаем размер шрифта
						FontSize = (int)(double)MainForm.GetValue(Control.FontSizeProperty);
					}
					else
						return availableSize;
				}

				// Получаем образец текста с текущими параметрами для определения его размера
				FormattedText ft = new FormattedText("W", CultureInfo.CurrentUICulture,
					FlowDirection.LeftToRight, new Typeface(FontName), FontSize, Brushes.Black,
					null, TextSmoothing = (TextFormattingMode)(1 - Server.GetFontSmoothing()),
					VisualTreeHelper.GetDpi(this).PixelsPerDip);

				// Определяем высоту строки
				RowHeight = (int)Math.Round(ft.Height) + LinePadding * 2;

				var m = RowHeight * VisibleRowCount + 2;

				return new Size(m * 3.5, m);
			}
			else
				return new Size(240, 100);
		}

		// Выполняет расстановку внутренних визуальных элементов управления
		protected override Size ArrangeOverride(Size finalSize)
		{
			if (IdentList != null)
			{

				// Определяем системный размер полос прокрутки
				double X = SystemParameters.VerticalScrollBarWidth;

				// Устанавливаем положение вертикальной полосы прокрутки
				var r = new Rect(finalSize.Width - X - 1, 1, X, finalSize.Height - 2);
				VScroll.Arrange(r);

				// Устанавливаем параметры вертикальной полосы прокрутки
				VScroll.Minimum = 0;
				var n = IdentList.GetCount();

				var y = n - VisibleRowCount;
				if (y > 0) // количество строк больше чем помещается в окне
				{
					VScroll.IsEnabled = true; // полоса прокрутки активна
					VScroll.Maximum = y; // максимальное значение
					VScroll.ViewportSize = VisibleRowCount; // размер бегунка
					VScroll.SmallChange = 1; // перемещение от нажатия на стрелку
					VScroll.LargeChange = VScroll.ViewportSize * 3 / 4; // перемещение от нажатия на полосу прокрутки
				}
				else // иначе полоса прокрутки неактивна
				{
					VScroll.Value = 0;
					VScroll.Maximum = 0;
					VScroll.IsEnabled = false;
				}

			}
			return finalSize;
		}

		// Выполяет отрисовку 
		protected override void OnRender(DrawingContext dc)
		{
			// Вспомогательный прямоугольник
			Rect r = new Rect(1, 1, RenderSize.Width - 1, RenderSize.Height - 1);

			// Вспомогательная точка
			Point p = new Point(0, 0);


			// Очищаем всю видимую область таблицы
			dc.DrawRectangle(Background,
				new Pen(Foreground, 1), r);


			// Устанавливаем размеры области отсечения изображения.
			// Иначе оно вылезет за размеры элемента управления. 
			dc.PushClip(new RectangleGeometry(r));

			r.X = 1;
			r.Y = 1;
			r.Width = RenderSize.Width - VScroll.Width - 2;
			r.Height = RowHeight;

			// Аргументы создания форматированного текста
			var ppd = VisualTreeHelper.GetDpi(this).PixelsPerDip;
			var Cult = CultureInfo.CurrentUICulture;
			var FlowDir = FlowDirection.LeftToRight;
			var Type = new Typeface(FontName);
			var Size = FontSize;

			p.X = LinePadding * 2;
			for (int i = ScrollTop, n = Math.Min(IdentList.GetCount(), i + VisibleRowCount); i < n; i++)
			{
				FormattedText s = new FormattedText(IdentList.GetDeclaration(i), Cult, FlowDir, Type, Size,
					i == CurrentIndex ? SelectionForeground : Foreground, null, TextSmoothing, ppd);

				if (i == CurrentIndex)
				{
					dc.DrawRectangle(SelectionBackground, null, r);
				}
				dc.PushClip(new RectangleGeometry(r));
	

				p.Y = r.Y + LinePadding - 1;
				dc.DrawText(s, p);

				r.Y += r.Height;
				dc.Pop();
			}

			dc.Pop();
		}

		// Реакция на нажатие кнопки мыши
		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left && e.LeftButton == MouseButtonState.Pressed)
			{
				var p = e.GetPosition(this);
				CurrentIndex = ScrollTop + (int)p.Y / RowHeight;
				if (e.ClickCount == 2)
				{
					var C = CurrentIndex;
					if (C >= 0 && C < IdentList.GetCount())
						InputIdent?.Invoke(this, IdentList.GetName(C));
					CloseRequest?.Invoke();
				}
				e.Handled = true;
			}
		}

		// Реакция на вращение колеса мыши
		protected override void OnMouseWheel(MouseWheelEventArgs e)
		{
			Roll(-Math.Sign(e.Delta));
			e.Handled = true;
		}

		/// <summary>
		/// Реакция на вращение колеса мыши в редакторе
		/// </summary>
		/// <param name="e">Аргументы события</param>
		public void HostMouseWheel(MouseWheelEventArgs e) => OnMouseWheel(e);


		/// <summary>
		/// Реакция на нажатие клавиши в редакторе
		/// </summary>
		/// <param name="e"></param>
		public void HostKeyDown(KeyEventArgs e, int Shift)
		{
			void HandledKeyDown(Action f)
			{
				if (Shift == 0)
				{
					f();
					e.Handled = true;
				}
				else
					CloseRequest?.Invoke();
			};


			switch (e.Key)
			{
				case Key.Down:
					HandledKeyDown(() => { Roll(1); });
					return;
				case Key.Up:
					HandledKeyDown(() => { Roll(-1); });
					return;
				case Key.PageDown:
					HandledKeyDown(() => { Roll(VisibleRowCount); });
					return;
				case Key.PageUp:
					HandledKeyDown(() => { Roll(-VisibleRowCount); });
					return;
				case Key.Home:
					HandledKeyDown(() => { Roll(-CurrentIndex); });
					return;
				case Key.End:
					HandledKeyDown(() => { Roll(IdentList.GetCount() - CurrentIndex); });
					return;
				case Key.Left:
				case Key.Right:
				case Key.Escape:
				case Key.Delete:
				case Key.Space:
					CloseRequest?.Invoke();
					e.Handled = false;
					return;
				case Key.Enter:
					HandledKeyDown(() =>
					{
						var C = CurrentIndex;
						if (C >= 0 && C < IdentList.GetCount())
							InputIdent?.Invoke(this, IdentList.GetName(C));
						CloseRequest?.Invoke();
					});
					return;
				case Key.Back:
					if (RecentCurrentWord.Length == 0 || !(IsValidIdentChar(RecentCurrentWord[RecentCurrentWord.Length - 1])))
						CloseRequest?.Invoke();
					return;
			}
		}

		/// <summary>
		/// Запрос на закрытие окна
		/// </summary>
		public Action CloseRequest;

		/// <summary>
		/// Событие ввода идентификатора
		/// </summary>
		public Action<IdentListControl, string> InputIdent;

		/// <summary>
		/// Сдвигает текущую строку
		/// </summary>
		/// <param name="Delta">Сдвижка</param>
		private void Roll(int Delta)
		{
			CurrentIndex = CurrentIndex + Delta;
		}

		/// <summary>
		/// Фильтрует список идентификаторов по текущему вводу
		/// </summary>
		public void Filter(string CurrentWord)
		{
			CurrentIndex = IdentList.Filter(RecentCurrentWord = CurrentWord);
			InvalidateVisual();
		}

		/// <summary>
		/// Последнее значение текущего слова
		/// </summary>
		private string RecentCurrentWord;

		void IDisposable.Dispose()
		{
			if (IdentList != null)
				Marshal.ReleaseComObject(IdentList);
		}
	}


}