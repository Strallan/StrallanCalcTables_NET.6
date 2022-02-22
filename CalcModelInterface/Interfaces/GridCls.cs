using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;
using System.Xml.Linq;
using static Strallan.Library;

namespace Strallan.Controls
{
	/// <summary>
	/// Класс элемента управления таблицы с отрисовкой содержимого
	/// </summary>
	public class GridControl : FrameworkElement, IDisposable
	{
        #region
        /// <summary>
        /// Имя шрифта. По умолчанию берётся свойство стиля главной формы приложения
        /// </summary>
        public static readonly DependencyProperty FontNameProperty;

		/// <summary>
		/// Размер шрифта. По умолчанию берётся свойство стиля главной формы приложения
		/// </summary>
		public static readonly DependencyProperty FontSizeProperty;

		/// <summary>
		/// Кисть фона ячеек
		/// </summary>
		public static readonly DependencyProperty CellBackgroundProperty;

		/// <summary>
		/// Кисть символов в ячейках
		/// </summary>
		public static readonly DependencyProperty CellForegroundProperty;

		/// <summary>
		/// Кисть фона заголовка таблицы
		/// </summary>
		public static readonly DependencyProperty CaptionBackgroundProperty;

		/// <summary>
		/// Кисть символов заголовка таблицы
		/// </summary>
		public static readonly DependencyProperty CaptionForegroundProperty;

		/// <summary>
		/// Кисть фона столбцов только для чтения
		/// </summary>
		public static readonly DependencyProperty ReadOnlyBackgroundProperty;

		/// <summary>
		/// Кисть символов столбцов только для чтения
		/// </summary>
		public static readonly DependencyProperty ReadOnlyForegroundProperty;

		/// <summary>
		/// Кисть фона выделенных ячеек
		/// </summary>
		public static readonly DependencyProperty SelectionBackgroundProperty;

		/// <summary>
		/// Кисть символов выделенных ячеек
		/// </summary>
		public static readonly DependencyProperty SelectionForegroundProperty;

		/// <summary>
		/// Метод определения кисти фона ячеек с «ошибочным» значением
		/// </summary>
		public static readonly DependencyProperty ErrorBackgroundProperty;

		/// <summary>
		/// Метод определения кисти символов ячеек с «ошибочным» значением
		/// </summary>
		public static readonly DependencyProperty ErrorForegroundProperty;

		/// <summary>
		/// Кисть сетки ячеек таблицы
		/// </summary>
		public static readonly DependencyProperty GridLineProperty;

		/// <summary>
		/// Порог статуса ячеек, после которого считается, что они содержат «ошибочное» значение
		/// </summary>
		public static readonly DependencyProperty ErrorThresholdProperty;

		/// <summary>
		/// Свойство Padding для ячеек таблицы
		/// </summary>
		public static readonly DependencyProperty CellPaddingProperty;

		/// <summary>
		/// Максимальная ширина столбца
		/// </summary>
		public static readonly DependencyProperty MaxColumnWidthProperty;

		/// <summary>
		/// Минимальная ширина столбца
		/// </summary>
		public static readonly DependencyProperty MinColumnWidthProperty;

		/// <summary>
		/// Ширина столбца по-умолчанию
		/// </summary>
		public static readonly DependencyProperty DefaultColumnWidthProperty;

		/// <summary>
		/// Интервал между остановкой мыши и показом подсказки
		/// </summary>
		public static readonly DependencyProperty HintDelayProperty;

		/// <summary>
		/// Интервал таймера прокрутки. Влияет на скорость автоматической
		/// прокрутки, когда в режиме выделения курсор мыши находится вне таблицы
		/// </summary>
		public static readonly DependencyProperty ScrollIntervalProperty;

		/// <summary>
		/// Регистрирует свойство зависимости
		/// </summary>
		/// <typeparam name="T">Тип свойства</typeparam>
		/// <param name="Name">Имя свойства</param>
		/// <param name="Arrange">Определяет влияние свойства на внутреннюю разметку таблицы</param>
		/// <param name="Render">Определяет влияние свойства на перерисовку таблицы</param>
		/// <param name="Default">Значение свойства по умолчанию</param>
		/// <returns>Объект свойства зависимости</returns>
		private static DependencyProperty PropReg<T>(string Name, bool Arrange, bool Render, T Default)
		{
			FrameworkPropertyMetadata fpm = new FrameworkPropertyMetadata
			{
				AffectsArrange = Arrange,
				AffectsMeasure = false,
				AffectsRender = Render,
				DefaultValue = Default,
			};

			return DependencyProperty.Register(Name, typeof(T), typeof(GridControl), fpm);
		}

		/// <summary>
		/// Статический конструктор, регистрирующий свойства зависимости
		/// </summary>
		static GridControl()
		{
			FontNameProperty = PropReg("FontName", true, true, "");
			FontSizeProperty = PropReg("FontSize", true, true, 0);
			CellBackgroundProperty = PropReg("CellBackground", false, true, (Brush)new SolidColorBrush(Colors.White));
			CellForegroundProperty = PropReg("CellForeground", false, true, (Brush)new SolidColorBrush(Colors.Black));
			CaptionForegroundProperty = PropReg("CaptionForeground", false, true, (Brush)new SolidColorBrush(Colors.Black));
			ReadOnlyForegroundProperty = PropReg("ReadOnlyForeground", false, true, (Brush)new SolidColorBrush(Colors.Black));
			CaptionBackgroundProperty = PropReg("CaptionBackground", false, true, (Brush)new SolidColorBrush(Color.FromRgb(223, 219, 215)));
			ReadOnlyBackgroundProperty = PropReg("ReadOnlyBackground", false, true, (Brush)new SolidColorBrush(Color.FromRgb(252, 251, 250)));
			SelectionBackgroundProperty = PropReg("SelectionBackground", false, true, (Brush)new SolidColorBrush(Colors.ForestGreen));
			SelectionForegroundProperty = PropReg("SelectionForeground", false, true, (Brush)new SolidColorBrush(Colors.White));
			GridLineProperty = PropReg("GridLine", false, true, (Brush)new SolidColorBrush(Color.FromRgb(184, 181, 178)));

			Func<double, bool, Brush> f1 = (double State1, bool Selected1) =>
			{
				return new SolidColorBrush(Selected1 ? Color.FromRgb(0xd0, 0x00, 0x00) : Color.FromRgb(0xff, 0xf5, 0xf5));
			};
			ErrorBackgroundProperty = PropReg("ErrorBackground", false, true, f1);

			Func<double, bool, Brush> f2 = (double State2, bool Selected2) =>
			{
				return new SolidColorBrush(Selected2 ? Colors.Yellow : Colors.Red);
			};
			ErrorForegroundProperty = PropReg("ErrorForeground", false, true, f2);
			ErrorThresholdProperty = PropReg("ErrorThreshold", false, true, 0.95);
			CellPaddingProperty = PropReg("CellPadding", true, true, 3);
			MaxColumnWidthProperty = PropReg("MaxColumnWidth", false, false, 512);
			MinColumnWidthProperty = PropReg("MinColumnWidth", false, false, 32);
			DefaultColumnWidthProperty = PropReg("DefaultColumnWidth", false, false, 64);
			HintDelayProperty = PropReg("HintDelay", false, false, 500);
			ScrollIntervalProperty = PropReg("ScrollInterval", false, false, 120);
		}

		private readonly SynchronizationContext SyncCtx = SynchronizationContext.Current;
#endregion

        /// <summary>
        /// Создаёт таблицу
        /// </summary>
        public GridControl()
		{
			// Назначение событий полос прокрутки
			VScroll.ValueChanged += VScrollValueChanged;
			HScroll.ValueChanged += HScrollValueChanged;

			// Объявляем вертикальную полосу прокрутки вложенным элементов управления
			AddVisualChild(VScroll);
			AddLogicalChild(VScroll);

			// То же для горизонтальной полосы прокрутки
			AddVisualChild(HScroll);
			AddLogicalChild(HScroll);

			Focusable = true; // таблица фокусируема
			FocusVisualStyle = null; // убираем артефакты фокусировки

			// Линии рисуются кристально ровными
			SnapsToDevicePixels = true;
			SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);

			SelectionTimer.Elapsed += SelectionTimerElapsed;
			HintTimer.Elapsed += HintTimerElapsed;

			Hint = new ToolTip()
			{
				Placement = PlacementMode.Relative,
				PlacementTarget = this,
			};

			Children.Add(VScroll);
			Children.Add(HScroll);
			Children.Add(Edit = new EditControl(this));

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
		public Brush CellBackground { get => (Brush)GetValue(CellBackgroundProperty); set => SetValue(CellBackgroundProperty, value); }

		/// <summary>
		/// Кисть символов ячеек
		/// </summary>
		public Brush CellForeground { get => (Brush)GetValue(CellForegroundProperty); set => SetValue(CellForegroundProperty, value); }

		/// <summary>
		/// Кисть фона заголовка
		/// </summary>
		public Brush CaptionBackground { get => (Brush)GetValue(CaptionBackgroundProperty); set => SetValue(CaptionBackgroundProperty, value); }

		/// <summary>
		/// Кисть символов заголовка
		/// </summary>
		public Brush CaptionForeground { get => (Brush)GetValue(CaptionForegroundProperty); set => SetValue(CaptionForegroundProperty, value); }

		/// <summary>
		/// Кисть фона столбцов только для чтения
		/// </summary>
		public Brush ReadOnlyBackground { get => (Brush)GetValue(ReadOnlyBackgroundProperty); set => SetValue(ReadOnlyBackgroundProperty, value); }

		/// <summary>
		/// Кисть символов столбцов только для чтения
		/// </summary>
		public Brush ReadOnlyForeground { get => (Brush)GetValue(ReadOnlyForegroundProperty); set => SetValue(ReadOnlyForegroundProperty, value); }

		/// <summary>
		/// Кисть сетки таблицы
		/// </summary>
		public Brush GridLine { get => (Brush)GetValue(GridLineProperty); set => SetValue(GridLineProperty, value); }

		/// <summary>
		/// Метод определения кисти фона ошибочных ячеек. 
		/// Получает статус ячейки от источника данных и признак выделенной ячейки.
		/// Возвращает значение кисти.
		/// </summary>
		public Func<double, bool, Brush> ErrorBackground { get => (Func<double, bool, Brush>)GetValue(ErrorBackgroundProperty); set => SetValue(ErrorBackgroundProperty, value); }

		/// <summary>
		/// Метод определения кисти символов ошибочных ячеек. 
		/// Получает статус ячейки от источника данных и признак выделенной ячейки.
		/// Возвращает значение кисти.
		/// </summary>
		public Func<double, bool, Brush> ErrorForeground { get => (Func<double, bool, Brush>)GetValue(ErrorForegroundProperty); set => SetValue(ErrorForegroundProperty, value); }

		/// <summary>
		/// Порог значения статуса ячеек, после которого они считаются ошибочными
		/// </summary>
		public double ErrorThreshold { get => (double)GetValue(ErrorThresholdProperty); set => SetValue(ErrorThresholdProperty, value); }

		/// <summary>
		/// Свойство Padding для ячеек таблицы
		/// </summary>
		public int CellPadding { get => (int)GetValue(CellPaddingProperty); set => SetValue(CellPaddingProperty, value); }

		/// <summary>
		/// Максимальная ширина столбца
		/// </summary>
		public int MaxColumnWidth { get => (int)GetValue(MaxColumnWidthProperty); set => SetValue(MaxColumnWidthProperty, value); }

		/// <summary>
		/// Минимальная ширина столбца
		/// </summary>
		public int MinColumnWidth { get => (int)GetValue(MinColumnWidthProperty); set => SetValue(MinColumnWidthProperty, value); }

		/// <summary>
		/// Ширина столбца по умолчанию
		/// </summary>
		public int DefaultColumnWidth { get => (int)GetValue(DefaultColumnWidthProperty); set => SetValue(DefaultColumnWidthProperty, value); }

		/// <summary>
		/// Интервал между остановкой мыши и показом подсказки
		/// </summary>
		public int HintDelay { get => (int)GetValue(HintDelayProperty); set => SetValue(HintDelayProperty, value); }

		/// <summary>
		/// Интервал таймера прокрутки
		/// </summary>
		public int ScrollInterval { get => (int)GetValue(ScrollIntervalProperty); set => SetValue(ScrollIntervalProperty, value); }

		/// <summary>
		/// Текущая высота строки таблицы
		/// </summary>
		public int RowHeight { get; private set; }
		
		/// <summary>
		/// Текущее количество строк в видимой области таблицы
		/// </summary>
		public int VisibleRowCount { get; private set; }

		/// <summary>
		/// Текущее положение горизонтальной полосы прокрутки
		/// </summary>
		public int ScrollLeft { get => (int)HScroll.Value; set => HScroll.Value = value; }

		/// <summary>
		/// Текущее положение вертикальной полосы прокрутки
		/// </summary>
		public int ScrollTop { get => (int)VScroll.Value; set => VScroll.Value = value; }

		/// <summary>
		/// Признак размытия шрифта
		/// </summary>
		private TextFormattingMode TextSmoothing;

		/// <summary>
		/// Источник данных таблицы
		/// </summary>
		public IGridDataSource DataSource { get; private set; }

		/// <summary>
		/// Класс столбца таблицы
		/// </summary>
		private class Column
		{
			/// <summary>
			/// Создаёт столбец таблицы
			/// </summary>
			/// <param name="index">Номер столбца</param>
			/// <param name="Caption">Заголовок столбца</param>
			/// <param name="Width">Ширина столбца</param>
			/// <param name="ReadOnly">Признак столбца только для чтения</param>
			/// <param name="OnChanged">Событие изменения столбца</param>
			public Column(int index, string Caption, int Width, int ReadOnly, Action<Column> OnChanged)
			{
				Index = index;
				caption = Caption;
				width = Width;
				readOnly = ReadOnly != 0;
				ColumnChanged += OnChanged;
			}

			/// <summary>
			/// Номер столбца
			/// </summary>
			public int Index { get; private set; }

			/// <summary>
			/// Событие изменения столбца
			/// </summary>
			public event Action<Column> ColumnChanged;

			/// <summary>
			/// Заголовок столбца
			/// </summary>
			private string caption;

			/// <summary>
			/// Заголовок столбца
			/// </summary>
			public string Caption
			{
				get => caption;
				set
				{
					caption = value;
					ColumnChanged?.Invoke(this);
				}
			}

			/// <summary>
			/// Ширина столбца
			/// </summary>
			private int width;

			/// <summary>
			/// Ширина столбца
			/// </summary>
			public int Width
			{
				get => width;
				set
				{
					width = value;
					ColumnChanged?.Invoke(this);
				}
			}

			/// <summary>
			/// Признак столбца только для чтения
			/// </summary>
			private bool readOnly;

			/// <summary>
			/// Признак столбца только для чтения
			/// </summary>
			public bool ReadOnly
			{
				get => readOnly;
				set
				{
					readOnly = value;
					ColumnChanged?.Invoke(this);
				}
			}
		}

		/// <summary>
		/// Список столбцов таблицы
		/// </summary>
		private readonly List<Column> Columns = new List<Column>();

		/// <summary>
		/// Выполняет привязку таблицы к источнику данных
		/// </summary>
		/// <param name="ds">Источник данных</param>
		public void BindDataSource(IGridDataSource ds)
		{
			if (DataSource == null)
			{
				DataSource = ds;

				for (int i = 0, n = DataSource.GetColCount(); i < n; i++)
				{
					string Caption = DataSource.GetColHeader(i, out int ReadOnly);

					Columns.Add(new Column(i, Caption, 64, ReadOnly, OnColumnChanged));
				}

				DataSource.Filter("", null);
			}
		}



		/// <summary>
		/// Выполняет полное обновление источника данных
		/// </summary>
		public void Reset()
		{
			DataSource.Filter("", null);
			VScroll.Value = 0;
			HScroll.Value = 0;
			Refresh();
		}


		/// <summary>
		/// Сохраняет настройки таблицы
		/// </summary>
		/// <param name="Root">Объект хранения настроек</param>
		public void SaveSettings(XElement Root)
		{
			XElement cols = new XElement("Columns");
			for (int i = 0, n = Columns.Count; i < n; i++)
			{
				XElement col = new XElement(Columns[i].Caption);
				XAttribute w = new XAttribute("Width", Columns[i].Width);
				col.Add(w);
				cols.Add(col);
			}
			XElement sel = new XElement("Selection");
			sel.Value = GetSelectionData();

			Root.Add(cols);
			Root.Add(sel);
		}

		/// <summary>
		/// Загружает данные конфигурации таблицы
		/// </summary>
		/// <param name="Config">Объект конфигурации</param>
		/// <param name="Filter">Строка фильтра</param>
		/// <param name="Order">Порядок сортировки</param>
		public void LoadSettings(XElement Config, string Filter, string Order)
		{
			ColumnLock++;
			try
			{
				XElement cols = FindXmlNode(Config, "Columns");

				if (cols != null)
				{
					for (int i = 0, n = DataSource.GetColCount(); i < n; i++)
					{
						string Caption = Columns[i].Caption;
						int Width = -1;

						for (XNode c = cols.FirstNode; c != null; c = c.NextNode)
						{
							if (c.NodeType == XmlNodeType.Element)
							{
								XElement e = c as XElement;
								if (e.Name == Caption)
								{
									var a = FindXmlAttr(e, "Width");
									if (int.TryParse(a, out int w))
										Width = w;
									break;
								}
							}
						}

						if (Width > 0)
						{
							Width = Math.Min(Math.Max(Width, MinColumnWidth), MaxColumnWidth);

							Columns[i].Width = Width;
						}
					}
				}
			}
			finally
			{
				ColumnLock--;
				OnColumnChanged(null);
			}

			XElement sel = FindXmlNode(Config, "Selection");

			var Snap = Server.Rdtsc();

			DataSource.Filter(Filter.Trim(), new FinishEvent((int Code, string Message) =>
			{
				if (Code == 0)
				{
					if ((Order = Order.Trim()) != "")
					{
						DataSource.Sort(Order, new FinishEvent((int Code1, string Message1) =>
						{
							Message = Message1;
							Code = Code1;

						}));
					}
					if (Code == 0 && sel != null)
						RestoreSelection(sel.Value);
				}
				EndWaiting?.Invoke(this, Snap, Code, Message);
			}));

			StartWaiting?.Invoke(this, Snap);
		}

		/// <summary>
		/// Возвращает данные для сохранения выделения
		/// </summary>
		/// <returns>Данные для сохранения</returns>
		public string GetSelectionData()
		{
			var Snap = SelectedCells.Snap(ScrollLeft, ScrollTop);
			StringBuilder sb = new StringBuilder();
			foreach (var i in Snap)
			{
				sb.Append(Snap[i].ToString());
				sb.Append(" ");
			}
			return sb.ToString().Trim();
		}

		/// <summary>
		/// Восстанавливает выделение из сохранённых данных
		/// </summary>
		/// <param name="Data">Строка, полученная от GetSelectionData</param>
		public void RestoreSelection(string Data = "")
		{
			if (DataSource != null)
			{

				if (Data != "")
				{
					var S = Data.Split(' ');
					List<int> L = new List<int>();
					foreach (var s in S)
					{
						if (int.TryParse(s, out int v))
							L.Add(v);
						else
							return;
					}
					SelectedCells.Restore(L);
				}

			}
		}

		/// <summary>
		/// Перерисовывает таблицу
		/// </summary>
		public void Refresh()
		{
			InvalidateArrange();
			InvalidateVisual();
		}

		/// <summary>
		/// Проверяет, является ли ячейка выделенной
		/// </summary>
		/// <param name="Row">Номер строки</param>
		/// <param name="Col">Номер столбца</param>
		/// <returns></returns>
		public bool IsCellSelected(int Row, int Col)
		{
			return SelectedCells.Inside(Row, Col);
		}

		/// <summary>
		/// Вычисляет расположение ячейки в видимой области таблицы
		/// </summary>
		/// <param name="Row">Номер строки</param>
		/// <param name="Col">Номер столбца</param>
		/// <param name="r">Прямоугольник ячейки</param>
		/// <returns>Признак расположения ячейки в видимой области</returns>
		public bool CellRect(int Row, int Col, out Rect r)
		{
			r = new Rect(); // создаём прямоугольник

			if (Row == -1) // находимся в заголовке
			{
				r.Y = 0;
				r.Height = RowHeight;
			}
			else if (Row >= ScrollTop && Row < ScrollTop + VisibleRowCount) // находимся в видимой области
			{
				r.Y = ((Row - ScrollTop) + 1) * RowHeight;
				r.Height = RowHeight;
			}
			else
				return false; // вне видимой области

			if (Col == 0) // находимся в начальном столбце
			{
				r.X = 0;
				r.Width = Columns[0].Width;
				return true;
			}
			else
			{
				for (int i = 1, n = VisibleColumns.Count; i < n; i++) // поиск по видимым столбцам
				{
					if (VisibleColumns[i].Col == Col)
					{
						r.X = VisibleColumns[i - 1].Right;
						r.Width = VisibleColumns[i].Right - r.X;
						return true;
					}
				}
			}

			return false; // ячейка вне видимой области
		}

		/// <summary>
		/// Вычисляет координаты ячейки по координатам точки относительно таблицы
		/// </summary>
		/// <param name="p">Координаты точки</param>
		/// <param name="Row">Номер строки (Для заголовка возвращается -1)</param>
		/// <param name="Col">Номер столбца</param>
		/// <returns>Признак успешного вычисления позиции ячейки</returns>
		public bool CoordToCell(Point p, out int Row, out int Col)
		{
			Row = 0;
			Col = 0;
			if (p.X < 0 || p.Y > ActualWidth - VScroll.Width ||
				p.Y < 0 || p.Y > ActualHeight - VScroll.Height)
				return false;

			Row = (int)Math.Round(p.Y) / RowHeight - 1;
			if (Row >= 0)
				Row += ScrollTop;

			if (Row >= RowCount)
				return false;

			int n = VisibleColumns.Count;
			if (n == 0)
				return false;

			if (p.X < VisibleColumns[0].Right)
				return true;

			for (int i = 1; i < n; i++)
			{
				if (p.X > VisibleColumns[i - 1].Right - Eps &&
					p.X < VisibleColumns[i].Right + Eps)
				{
					Col = VisibleColumns[i].Col;
					return true;
				}
			}

			return false;

		}

		/// <summary>
		/// Количество строк таблицы
		/// </summary>
		public int RowCount => DataSource != null ? DataSource.GetRowCount() : 0;

		/// <summary>
		/// Количество столбцов таблицы
		/// </summary>
		public int ColCount => DataSource != null ? DataSource.GetColCount() : 0;

		/// <summary>
		/// Возващает заголовок столбца
		/// </summary>
		/// <param name="Col">Номер столбца</param>
		/// <returns>Заголовок столбца</returns>
		public string ColumnName(int Col) => Col >= 0 && Col < Columns.Count ? Columns[Col].Caption : "";

		/// <summary>
		/// Список соответствия имён столбцов и их номеров
		/// </summary>
		private Dictionary<string, int> ColumnIndices = null;

		/// <summary>
		/// Возвращает номер столбца по его имени, в случае отсутствия имени возвращается -1 
		/// </summary>
		/// <param name="Name">Имя столбца</param>
		/// <returns>Индекс</returns>
		public int ColumnIndex(string Name)
        {
			if (ColumnIndices == null)
            {
				ColumnIndices = new Dictionary<string, int>();
				for (int i = 0, n = Columns.Count; i < n; i++)
					ColumnIndices.Add(Columns[i].Caption, i);
            }

			if (ColumnIndices.TryGetValue(Name, out int Index))
				return Index;
			else
				return -1;
        }

		/// <summary>
		/// Добавляет строку таблицы
		/// </summary>
		/// <param name="Id">Идентификатор, передаваемый источнику данных</param>
		public void AddRow(string Id)
		{
			if (DataSource != null)
			{
				BeginUpdate(); // начали запись в журнал
				try
				{
					DataSource.AddRow(Id); // добавили строку
					SelectCell(RowCount - 1, 0, true); // выбрали новую ячейку
					if (Changes.IsUnlocked) // запись в журнал событий
					{
						var Index = RowCount - 1;

						Changes.Log(() => // метод отмены
						{
							DataSource.DeleteRow(Index);
						}, () => // метод возврата
						{
							DataSource.AddRow(Id);
						});
					}
				}
				finally
				{
					EndUpdate(); // закончили запись в журнал событий
				}
			}
		}

		/// <summary>
		/// Удаление строк, в которых есть выделенные ячейки
		/// </summary>
		public void DeleteRows()
		{
			var Rows = SelectedCells.Rows; // выделенные строки
			var n = Rows.Length;
			if (n != 0)
			{
				BeginUpdate(); // начали запись в журнал событий
				try
				{
					var RecoveryData = new List<(int, string)>(); // данные для восстановления строк
					for (--n; n >= 0; n--)
						if (DataSource.IsRowLocked(Rows[n]) == 0)
							RecoveryData.Add((Rows[n], DataSource.DeleteRow(Rows[n])));  // удалили и запомнили

					if (Rows[0] >= RowCount) // установили выделение
						SelectCell(RowCount - 1, 0, true);
					else if (RowCount > 0)
						SelectCell(Rows[0], 0, true);
					else
						SelectedCells.Clear();

					if (Changes.IsUnlocked) // запись в журнал событий
					{
						Changes.Log(() => // метод отмены
						{
							for (var i = RecoveryData.Count - 1; i >= 0; i--)
							{
								var (Row, Str) = RecoveryData[i];
								DataSource.RestoreRow(Row, Str);
							}

						}, () => // метод возврата
						{
						
							for (int i = 0, nc = RecoveryData.Count; i < nc; i++)
							{
								var (Row, Str) = RecoveryData[i];
								DataSource.DeleteRow(Row);
							}
						});
					}

				}
				finally
				{
					EndUpdate();
				}
			}
		}

		/// <summary>
		/// Проверяет наличие выделения
		/// </summary>
		public bool HasSelection
		{
			get
			{
				if (Edit.Active) // активен редактор
				{
					var (S, E) = Edit.Selection;
					return S != E;
				}
				else // неактивен
				{
					var (A, B) = SelectedCells.Current;
					return A >= 0 && B >= 0;
				}
			}
		}

		/// <summary>
		/// Текст выделения
		/// </summary>
		public string SelectedText
		{
			get
			{
				if (Edit.Active)  // активен редактор
				{
					return Edit.SelText; // берём текст там
				}
				else
				{
					var Cells = SelectedCells.Cells; // список выделенных ячеек
					if (Cells.Count != 0) // не пустой
					{
						var Str = new StringBuilder(); // результат
						var E = Cells.GetEnumerator(); // SortedSet не имеет оператора []
						E.MoveNext(); // нулевой элемент энумератора всегда пустой, сдвигаемся
						var (Row, Col) = E.Current; // первая выделенная ячейка
						Str.Append(DataSource.GetEditCell(Row, Col)); // добавляем её значение
						while (E.MoveNext()) // пока есть что перебирать
						{
							var (R, C) = E.Current; // текущая ячейка 
							Str.Append(R == Row ? "\t" : "\r\n"); // та же строка – добавляем Tab, новая строка – CrLf
							Str.Append(DataSource.GetEditCell(R, C)); // значение ячейки
							Row = R; // запоминаем номер строки
						}
						return Str.ToString();
					}
					else
						return "";
				}
			}
			set
			{
				if (Edit.Active) // активен редактор
				{ 
					Edit.Insert(value, 0); // записываем туда
				}
				else
				{
					var Cells = SelectedCells.Cells; // выделенные ячейки
					var n = Cells.Count;
					if (n == 1) // только одна ячейка
					{
						BeginUpdate();
						try
						{
							int rc = RowCount, cc = ColCount; // размеры таблицы
							var E = Cells.GetEnumerator(); // энумератор выделенных ячеек
							E.MoveNext(); // первый элемент
							var (Row0, Col0) = E.Current; // координаты ячеек  
							StringToMatrix(value, (int i, int j, string Str) => // разбиваем строку и записываем по кусочкам в ячеечки
							{
								if ((i += Row0) < rc && (j += Col0) < cc && !Columns[j].ReadOnly && DataSource.IsRowLocked(i) == 0)
								{
									SetCell(i, j, Str);
									return true;
								}
								return false;
							});
						}
						finally
						{
							EndUpdate();
						}
					}
					else if (n != 0)
					{
						BeginUpdate();
						try
						{
							List<(int Row, List<int> Cols)> Temp = 
								new List<(int Row, List<int> Cols)>(); // cписок строк и столбцов выделенных ячеек 
			
							int Prev = -1;
							List<int> Cols = null; // список столбцов в строке
							foreach (var (Row, Col) in Cells) // перебираем ячейки
							{
								if (Prev != Row)
								{
									Temp.Add((Row, Cols = new List<int>())); // получаем список в немного другом формате
									Prev = Row;
								}
								Cols.Add(Col);
							}
							n = Temp.Count;

							StringToMatrix(value, (int i, int j, string Str) => // разбиваем строку и раскидываем по ячейкам
							{
								if (i < n)
								{
									var c = Temp[i];
									if (j < c.Cols.Count)
									{
										int Row = c.Row, Col = c.Cols[j];
										if (!Columns[Col].ReadOnly && DataSource.IsRowLocked(Row) == 0)
											SetCell(Row, Col, Str);
									}
									return true;
								}
								return false;
							});
						}
						finally
						{
							EndUpdate();
						}
					}
				}
			}
		}

		// Счётчик вызовов BeginUpdate / EndUpdate;
		private int UpdateCount = 0;

		/// <summary>
		/// Событие запроса таблицы на показ
		/// </summary>
		public event Action<GridControl> DisplayRequest;

		/// <summary>
		/// Метод начала группового изменения данных
		/// </summary>
		public void BeginUpdate()
		{
			if (++UpdateCount == 1) // произошла первичная запись
			{
				Changes.BeginGroup(); // новая группа записей

				if (Changes.IsUnlocked) // журнал разблокирован
				{
					var Snap = SelectedCells.Snap(ScrollLeft, ScrollTop); // сохраняем выделение и полосы прокрутки
		
					Changes.Log(() => // метод отмены
					{
						DisplayRequest?.Invoke(this); // таблицу надо показать, если она скрыта

						SelectedCells.Restore(Snap); // восстановили выделение
						ScrollLeft = Snap[0];
						ScrollTop = Snap[1]; // восстановили полосы прокрутки
						EndUpdate();
					}, () => // метод возврата
					{
						DisplayRequest?.Invoke(this);

						BeginUpdate();
					});
				}
			}
		}

		/// <summary>
		/// Метод окончания группового изменения данных
		/// </summary>
		public void EndUpdate()
		{
			if (--UpdateCount == 0) // группа завершена
			{
				if (Changes.IsUnlocked) // журнал разблокирован
				{
					if (ScrollCell.Row >= 0 && ScrollCell.Col >= 0) // надо выделить ячейку
					{
						ScrollIntoView(ScrollCell.Row, ScrollCell.Col, true);
						ScrollCell = (-2, -2);
					}
					var Snap = SelectedCells.Snap(ScrollLeft, ScrollTop);

					Changes.Log(() => // метод отмены
					{
						DisplayRequest?.Invoke(this);

						BeginUpdate();
					}, () => // метод возврата
					{
						DisplayRequest?.Invoke(this);

						SelectedCells.Restore(Snap);
						ScrollLeft = Snap[0];
						ScrollTop = Snap[1];
						EndUpdate();
					});

				}
				Changes.EndGroup();
				Refresh();
			}
		}

		/// <summary>
		/// Событие изменения значения вертикальной полосы прокрутки
		/// </summary>
		private void VScrollValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			InvalidateVisual();
		}

		/// <summary>
		/// Событие изменения значение горизонтальной полосы прокрутки
		/// </summary>
		private void HScrollValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			InvalidateVisual();
		}

		/// <summary>
		/// Количество блокировок события изменения столбцов таблицы
		/// </summary>
		private int ColumnLock = 0;

		/// <summary>
		/// Событие изменения столбца таблицы
		/// </summary>
		private void OnColumnChanged(Column c)
		{
			if (ColumnLock == 0)
				Refresh();
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
		/// Редактор ячеек
		/// </summary>
		private readonly EditControl Edit;

		/// <summary>
		/// Список внутренних компонентов
		/// </summary>
		private readonly List<Visual> Children = new List<Visual>();

		// Количество внутренних визуальных элементов управления
		protected override int VisualChildrenCount => Children.Count;

		// Возвращает внутренний визуальный элемент управления по его индексу
		protected override Visual GetVisualChild(int index) => Children[index];

		// Выполняет расстановку внутренних визуальных элементов управления
		protected override Size ArrangeOverride(Size finalSize)
		{
			// Если нет источника данных, ничего не должно отображаться
			if (DataSource != null)
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
						return finalSize;
				}

				// Получаем образец текста с текущими параметрами для определения его размера
				FormattedText ft = new FormattedText("W", CultureInfo.CurrentUICulture,
					FlowDirection.LeftToRight, new Typeface(FontName), FontSize, Brushes.Black,
					null, TextSmoothing = (TextFormattingMode)(1 - Server.GetFontSmoothing()),
					VisualTreeHelper.GetDpi(this).PixelsPerDip);

				// Определяем высоту строки
				RowHeight = (int)Math.Round(ft.Height) + CellPadding * 2;

				// Определяем системный размер полос прокрутки
				double X = SystemParameters.VerticalScrollBarWidth,
					Y = SystemParameters.HorizontalScrollBarHeight;

	
				// Устанавливаем положение вертикальной полосы прокрутки
				var r = new Rect(finalSize.Width - X, 0, X, finalSize.Height - Y);
				VScroll.Arrange(r);

				// Устанавливаем положение горизонтальной полосы прокрутки
				r = new Rect(0, finalSize.Height - Y, finalSize.Width - X, Y);
				HScroll.Arrange(r);

				// Определяем количество видимых строк таблицы
				VisibleRowCount = (int)((finalSize.Height - Y) / RowHeight) - 1;

				// Устанавливаем параметры вертикальной полосы прокрутки
				VScroll.Minimum = 0;
				var y = DataSource.GetRowCount() - VisibleRowCount;
				VScroll.ViewportSize = VisibleRowCount; // размер бегунка
				if (y > 0) // количество строк больше чем помещается в окне
				{
					VScroll.IsEnabled = true; // полоса прокрутки активна
					VScroll.Maximum = y; // максимальное значение

					VScroll.SmallChange = 1; // перемещение от нажатия на стрелку
					VScroll.LargeChange = VScroll.ViewportSize * 3 / 4; // перемещение от нажатия на полосу прокрутки
				}
				else // иначе полоса прокрутки неактивна
				{
					VScroll.Value = 0;
					VScroll.Maximum = 0;
					VScroll.IsEnabled = false;
				}

				// Вычисляем ширину всех столбцов таблицы, за исключением нулевого
				double ColumnWidth = 0;
				for (int i = 1, n = Columns.Count; i < n; i++)
					ColumnWidth += Columns[i].Width;

				// Определяем параметры горизонтальной полосы прокрутки
				HScroll.Minimum = 0;
				var x = ColumnWidth + Columns[0].Width - (finalSize.Width - X);
				HScroll.ViewportSize = finalSize.Width - X;
				if (x > 0)  // ширина столбцов больше размера видимой области
				{
					HScroll.IsEnabled = true;
					HScroll.Maximum = x;

					HScroll.LargeChange = Math.Round(ColumnWidth / (Columns.Count - 1));
					HScroll.SmallChange = 1;
				}
				else // меньше
				{
					HScroll.Value = 0;
					HScroll.Maximum = 0;
					HScroll.IsEnabled = false;
				}

				if (Edit.Active)
				{
					if (CellRect(Edit.Row, Edit.Col, out Rect R) && Math.Abs(R.Width - Columns[Edit.Col].Width) < Eps)
						Edit.Arrange(R);
					else
						Edit.Hide();
				}
			}

			return finalSize;
		}

		/// <summary>
		/// Точность сравнения вещественных чисел при отрисовке
		/// </summary>
		private const double Eps = 0.01;

		/// <summary>
		/// Список видимых столбцов таблицы.
		/// Col – номер столбца
		/// Right – горизонтальное положение правой границы столбца
		/// </summary>
		private readonly List<(int Col, double Right)> VisibleColumns = new List<(int Col, double Right)>();

		/// <summary>
		/// Список всплывающих подсказок для ячеек
		/// </summary>
		private readonly SortedList<(int Row, int Col), string> CellHints = new SortedList<(int Row, int Col), string>();

		// Выполяет отрисовку таблицы
		protected override void OnRender(DrawingContext dc)
		{
			// Вспомогательный прямоугольник
			Rect r = new Rect(0, 0, RenderSize.Width, RenderSize.Height);

			// Вспомогателная точка
			Point p = new Point(0, 0);

			// Устанавливаем размеры области отсечения изображения.
			// Иначе оно вылезет за размеры элемента управления. 
			dc.PushClip(new RectangleGeometry(r));

			// Очищаем всю видимую область таблицы
			dc.DrawRectangle(CellBackground,
				null, r);

			// Рисуем фон заголовка таблицы
			r.Height = RowHeight;
			dc.DrawRectangle(CaptionBackground, null, r);

			// Заполняем область правого нижнего угла за полосами прокрутки
			r.X = RenderSize.Width - (r.Width = VScroll.Width);
			r.Y = RenderSize.Height - (r.Height = HScroll.Height);
			dc.DrawRectangle(VScroll.Background, null, r);

			CellHints.Clear();  // Очистка массива подсказок

			// Рисуем заголовки столбцов 
			int nc = Columns.Count;
			if (nc != 0)
			{
				// Очистка списка
				VisibleColumns.Clear();

				// Аргументы создания форматированного текста
				var ppd = VisualTreeHelper.GetDpi(this).PixelsPerDip;
				var Cult = CultureInfo.CurrentUICulture;
				var FlowDir = FlowDirection.LeftToRight;
				var Type = new Typeface(FontName);
				var Size = FontSize;

				// Заголовок первого столбца таблицы
				FormattedText s = new FormattedText(Columns[0].Caption, Cult, FlowDir, Type, Size,
					CaptionForeground, null, TextSmoothing, ppd);

				r.X = 0; // левая граница ячейки
				r.Y = 0; // верхняя граница ячейки
				r.Width = Columns[0].Width; // ширина ячейки
				r.Height = RowHeight; // высота ячейки

				// координаты текста заголовка, чтобы он располагался посередине ячейки
				p.X = r.X + (r.Width - s.Width) / 2;
				p.Y = r.Y + (r.Height - s.Height) / 2;

				dc.PushClip(new RectangleGeometry(r)); // область отсечения изображения (на случай если заголовок шире ячейки)
				dc.DrawText(s, p); // рисуем заголовок
				dc.Pop(); // восстанавливаем область отсечения
				VisibleColumns.Add((0, r.Right)); // добавляем ячейку в список выделенных

				// Определение видимости столбцов и их отрисовка
				double
					x0 = -Math.Round(HScroll.Value), // текущая левая граница 
					x1 = x0, // текущая правая граница
					w0 = Columns[0].Width, // ширина первого столбца
					ViewportWidth = HScroll.ViewportSize - w0 + Eps, // ширина области построения
					xmax = RenderSize.Width - VScroll.Width; // максимальная видимая координата

				// Перебираем столбцы, пока они левее правой границы области построения
				for (int i = 1; i < nc && x0 < ViewportWidth; i++, x0 = x1)
				{
					double w = Columns[i].Width; // ширина столбца
					s = new FormattedText(Columns[i].Caption, Cult, FlowDir, Type, Size,
						CaptionForeground, null, TextSmoothing, ppd); // заголовок столбца

					x1 = x0 + w;
					if (x1 > -Eps) // Правая граница столбца попадает в видимую область
					{
						p.X = w0 + x1 - w / 2 - s.Width / 2; // координата заголовка
						r.X = w0 + x1 - w; // левая граница столбца
						r.Width = w; // ширина столбца
						if (r.X < w0) // столбец частично закрыт первым столбцом
						{
							r.Width -= w0 - r.X; // уменьшаем видимую ширину
							r.X = w0; // левая граница
						}
						if (r.Right > xmax) // столбец частично попадает за правую границу видимой области
							r.Width = xmax - r.X; // уменьшаем видимую ширину
						if (r.Width < 1)
							continue;

						VisibleColumns.Add((i, r.Right)); // добавляем столбец в список видимых
						dc.PushClip(new RectangleGeometry(r)); // область отсечения при рисовании заголовка
						if (s.Width > r.Width - Eps)
							CellHints.Add((-1, i), s.Text); // добавляем в подсказку, если текст шире столбца
						dc.DrawText(s, p); // рисуем заголовок
						dc.Pop(); // восстанавливаем область отсечения
					}
				}

				// Рисование видимых ячеек

				nc = VisibleColumns.Count; // количество видимых столбцов
				r.Y = 0; // вертикальная координата ячейки
				r.Height = RowHeight; // высота ячейки
				int cp = CellPadding; // Padding ячейки

				// Цикл по видимым строкам 
				for (int i = 0, ni = VisibleRowCount, cr = ScrollTop, nr = DataSource.GetRowCount(); i < ni && cr < nr; i++, cr++)
				{
					r.Y += r.Height; // вертикальное смещение
					r.X = 0; // горизонтальная координата
					r.Width = 0; // ширина ячейки

					// Цикл по видимым столбцам
					for (int j = 0; j < nc; j++)
					{
						int cc = VisibleColumns[j].Col; // номер столбца
						r.X += r.Width; // горизонтальное смещение
						r.Width = VisibleColumns[j].Right - r.X; // ширина ячейки

						if (Edit.IsCellEdited(cr, cc))
							continue;

						// Получаем значение, статус и подсказку ячейки
						string Cell = DataSource.GetDisplayCell(cr, cc, out double State, out string Hint);

						// Если есть подсказка, запоминаем её в массиве подсказок
						if (Hint != "")
							CellHints.Add((cr, cc), Hint);

						// Определение цвета фона и шрифта
						Brush b, f;

						bool sel = IsCellSelected(cr, cc); // Или ячейка выделена?
						if (State > ErrorThreshold) // статус выше порога ошибки
						{
							b = ErrorBackground(State, sel);
							f = ErrorForeground(State, sel);
						}
						else if (sel) // статус ниже, но ячейка выделена
						{
							b = SelectionBackground;
							f = SelectionForeground;
						}
						else if (Columns[cc].ReadOnly) // ячейка не выделена, но столбец только для чтения
						{
							b = ReadOnlyBackground;
							f = ReadOnlyForeground;
						}
						else // простая ячейка
						{
							b = CellBackground;
							f = CellForeground;
						}

						dc.DrawRectangle(b, null, r);  // заполняем фон ячейки

						if (r.Width < cp + Eps)
							continue;
						r.Width -= cp;
						dc.PushClip(new RectangleGeometry(r)); // уcтанавливаем зону отсечения 
						r.Width += cp;

						if (Hint == "" && s.Width > r.Width - Eps)
							CellHints.Add((cr, cc), s.Text); // если текст шире ячейки добавляем подсказку

						// Текст ячейки
						s = new FormattedText(Cell, Cult, FlowDir, Type, Size,
							f, null, TextSmoothing, ppd);

						// Координаты текста
						p.X = j == 1 ? r.Right - Columns[cc].Width + cp : r.Left + cp;
						p.Y = r.Y + (r.Height - s.Height) / 2;
						dc.DrawText(s, p); // рисуем текст

						dc.Pop(); // восстанавливаем зону отсечения


					}
				}


				// Рисование границ ячеек
				Pen gp = new Pen(GridLine, 1); // перо для рисования сетки
				p.Y = 1; // начало линии
				Point p2 = new Point(0, RenderSize.Height - HScroll.Height - 1); // конец линии

				// Вертикальные линии сетки по правым границам столбцов
				for (int i = 0, n = VisibleColumns.Count; i < n; i++)
				{
					p.X = p2.X = VisibleColumns[i].Right;
					dc.DrawLine(gp, p, p2);
				}

				// Горизонтальные линии сетки
				p.X = 1;
				p.Y = p2.Y = RowHeight;
				p2.X = RenderSize.Width - VScroll.Width - 1;
				for (double h = RenderSize.Height - HScroll.Height + Eps; p.Y < h; p.Y += RowHeight)
				{
					p2.Y = p.Y;
					dc.DrawLine(gp, p, p2);
				}

			}

			// Восстанавливаем область отсечения
			dc.Pop();

			if (Edit.Active)
				InvalidateArrange();
		}

		/// <summary>
		/// Класс выделения
		/// </summary>
		private class Selection
		{
			/// <summary>
			/// Область выделения
			/// </summary>
			private class Spot
			{
				/// <summary>
				/// Начальная строка
				/// </summary>
				public int StartRow { get; private set; }

				/// <summary>
				/// Начальный столбец
				/// </summary>
				public int StartCol { get; private set; }

				/// <summary>
				/// Конечная строка
				/// </summary>
				public int EndRow { get; private set; }

				/// <summary>
				/// Конечный столбец
				/// </summary>
				public int EndCol { get; private set; }

				/// <summary>
				/// Начинает выделение области
				/// </summary>
				/// <param name="Row">Начальная строка</param>
				/// <param name="Col">Начальный столбец</param>
				public Spot(int Row, int Col)
				{
					StartRow = EndRow = Row;
					StartCol = EndCol = Col;
				}

				/// <summary>
				/// Устанавливает конечную точку области выделения
				/// </summary>
				/// <param name="Row">Номер строки</param>
				/// <param name="Col">Номер столбца</param>
				/// <returns>Признак неравенства новых координат старым</returns>
				public bool Move(int Row, int Col)
				{
					if (EndRow != Row || EndCol != Col)
					{
						EndRow = Row;
						EndCol = Col;
						return true;
					}
					return false;
				}

				/// <summary>
				/// Проверяет нахождение точки в области выделения
				/// </summary>
				/// <param name="Row">Номер строки</param>
				/// <param name="Col">Номер столбца</param>
				/// <returns>Признак нахождения точки в выделенной области</returns>
				public bool Inside(int Row, int Col)
				{
					return InsideRange(Row, StartRow, EndRow) && InsideRange(Col, StartCol, EndCol);
				}

				/// <summary>
				/// Проверяет, что область состоит из одной ячейки
				/// </summary>
				public bool SingleCell => StartRow == EndRow && StartCol == EndCol;
			}

			/// <summary>
			/// Список выделенных областей
			/// </summary>
			private readonly List<Spot> Spots = new List<Spot>();

			/// <summary>
			/// Начинает выделение области
			/// </summary>
			/// <param name="Row">Начальная строка</param>
			/// <param name="Col">Начальный столбец</param>
			/// <param name="Clear">Признак очистки выделения перед началом новой области</param>
			public void Start(int Row, int Col, bool Clear)
			{
				if (Clear)
					Spots.Clear();

				Spots.Add(new Spot(Row, Col));
			}

			/// <summary>
			/// Устанавливает координаты конечной точки последней области выделения
			/// </summary>
			/// <param name="Row">Номер строки</param>
			/// <param name="Col">Номер столбца</param>
			/// <returns>Признак неравенства новых координат старым</returns>
			public bool Move(int Row, int Col)
			{
				int n = Spots.Count;
				if (n != 0)
					return Spots[n - 1].Move(Row, Col);
				else
					return false;
			}

			/// <summary>
			/// Очищает выделение
			/// </summary>
			public void Clear()
			{
				Spots.Clear();
			}

			/// <summary>
			/// Проверяет нахождение ячейки в выделенной области
			/// </summary>
			/// <param name="Row">Номер строки</param>
			/// <param name="Col">Номер столбца</param>
			/// <returns>Признак нахождения ячейки в выделенной области</returns>
			public bool Inside(int Row, int Col)
			{
				foreach (Spot s in Spots)
					if (s.Inside(Row, Col))
						return true;
				return false;
			}

			/// <summary>
			/// Координаты первой выделенной ячейки
			/// </summary>
			public (int Row, int Col) First
			{
				get
				{
					if (Spots.Count != 0)
					{
						var s = Spots[0];
						return (s.StartRow, s.StartCol);
					}
					else
						return (-1, -1);
				}
			}

			/// <summary>
			/// Координаты последней выделенной ячейки
			/// </summary>
			public (int Row, int Col) Last
			{
				get
				{
					var n = Spots.Count;
					if (n != 0)
					{
						var s = Spots[n - 1];
						return (s.EndRow, s.EndCol);
					}
					else
						return (-1, -1);
				}
			}

			/// <summary>
			/// Координаты текущей ячейки (первой ячейки последней области выделения)
			/// </summary>
			public (int Row, int Col) Current
			{
				get
				{
					var n = Spots.Count;
					if (n != 0)
					{
						var s = Spots[n - 1];
						return (s.StartRow, s.StartCol);
					}
					else
						return (-1, -1);
				}
			}

			/// <summary>
			/// Проверяет, что выделение состоит из единственной ячейки
			/// </summary>
			public bool SingleCell => Spots.Count == 1 && Spots[0].SingleCell;

			/// <summary>
			/// Возвращает номера строк, в которых есть выделенные ячейки
			/// </summary>
			public int[] Rows
			{
				get
				{
					var Set = new SortedSet<int>();
					foreach (Spot s in Spots)
					{
						for (int i = s.StartRow, n = s.EndRow + 1; i < n; i++)
							Set.Add(i);
					}

					{
						int i = 0, n = Set.Count;
						int[] Res = new int[n];
						foreach (var j in Set)
							Res[i++] = j;

						return Res;
					}


				}
			}

			/// <summary>
			/// Список координат всех выделенных ячеек
			/// </summary>
			public SortedSet<(int Row, int Col)> Cells
			{
				get
				{
					SortedSet<(int Row, int Col)> Res = new SortedSet<(int Row, int Col)>();
					foreach (Spot s in Spots)
					{
						int i0 = s.StartRow, i1 = s.EndRow, j0 = s.StartCol, j1 = s.EndCol;
						if (i0 > i1)
						{
							i0 = i1;
							i1 = s.StartRow;
						}
						if (j0 > j1)
						{
							j0 = j1;
							j1 = s.EndRow;
						}
						for (int i = i0; i <= i1; i++)
							for (int j = j0; j <= j1; j++)
								Res.Add((i, j));
					}
					return Res;
				}
			}

			/// <summary>
			/// Сохраняет данные выделения в массиве целых чисел
			/// </summary>
			/// <param name="H">Положение горизонтальной полосы прокрутки</param>
			/// <param name="V">Положение вертикальной полосы прокрутки</param>
			/// <returns>Массив целых чисел – данные выделения</returns>
			public List<int> Snap(int H, int V)
			{
				var Res = new List<int>
				{
					H,
					V
				};
				foreach (Spot s in Spots)
				{
					Res.Add(s.StartRow);
					Res.Add(s.StartCol);
					Res.Add(s.EndRow);
					Res.Add(s.EndCol);
				}
				return Res;
			}

			/// <summary>
			/// Восстанавливает данные выделения из массива целых чисел, полученного от метода Snap
			/// </summary>
			/// <param name="Snap">Массив целых чисел</param>
			public void Restore(List<int> Snap)
			{
				Spots.Clear();
				for (int i = 2, n = Snap.Count; i < n; i += 4)
				{
					Spot s = new Spot(Snap[i], Snap[i + 1]);
					s.Move(Snap[i + 2], Snap[i + 3]);
					Spots.Add(s);
				}
			}


		}

		/// <summary>
		/// Список выделенных ячеек
		/// </summary>
		private readonly Selection SelectedCells = new Selection();

		/// <summary>
		/// Событие автозаполнения выделенных ячеек
		/// </summary>
		public Action<SortedSet<(int Row, int Col)>> Populate { get; set; }

		/// <summary>
		/// Автоматически устанавливает значение выделенных ячеек
		/// </summary>
		public void PopulateSelection()
		{
			if (!Edit.Active)
			{
				var Cells = SelectedCells.Cells;
				if (Cells.Count > 1 && Populate != null)
					Populate(Cells);
			}
		}

		/// <summary>
		/// Признак нажатой левой кнопки мыши
		/// </summary>
		private bool IsMouseLeftButtonDown = false;

		// Обработчик событий от нажатия кнопок мыши
		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			// фокусируем таблицу
			if (!IsFocused)
				Focus();

			// опеределяем что нажата левая кпопка мыши и она не была нажата ранее
			if (e.LeftButton == MouseButtonState.Pressed && IsMouseLeftButtonDown == false && CoordToCell(e.GetPosition(this), out int Row, out int Col) && Row >= 0)
			{
				if (e.ClickCount == 2 && !Columns[Col].ReadOnly && DataSource.IsRowLocked(Row) == 0)
				{
					Edit.Show(Row, Col, DataSource.GetEditCell(Row, Col));
				}
				var km = Keyboard.Modifiers; // состояние клавиш-переключателей
				switch (km)
				{
					case 0: // ничего не нажато
					case ModifierKeys.Control: // нажат Ctrl
						SelectedCells.Start(Row, Col, km == 0); // начинает новую область выделения (при отпущенном Ctrl ещё и очищаем старую область)
						IsMouseLeftButtonDown = true; // запоминаем состояние левой кнопки мыши
						ScrollIntoView(-1, Col);
						Mouse.Capture(this); // зохват мыши, чтобы поступали сообщения от мыши за границами таблицы (для прокрутки)
						e.Handled = true; // событие обработано
						break;
					case ModifierKeys.Shift: // нажат Shift
						var (SR, SC) = SelectedCells.Current;
						if (SR >= 0 && SC >= 0)
							SetSelectionEnd(Row, Col);
						e.Handled = true;
						break;
				}

			}
		}

		/// <summary>
		/// Выделяет одну ячейки таблицы
		/// </summary>
		/// <param name="Row">Номер строки</param>
		/// <param name="Col">Номер столбца</param>
		/// <param name="Short">Признак того, что строка показывается вверху или внизу, а не в трети высоты таблицы</param>
		public void SelectCell(int Row, int Col, bool Short = false)
		{
			Row = Math.Max(0, Math.Min(Row, RowCount - 1));
			Col = Math.Max(0, Math.Min(Col, ColCount - 1));
			SelectedCells.Start(Row, Col, true);
			if (UpdateCount == 0)
				ScrollIntoView(Row, Col, Short);
			else
				ScrollCell = (Row, Col);
		}

		/// <summary>
		/// Ячейка, в которую требуется перенос фокуса в методе EndUpdate
		/// </summary>
		private (int Row, int Col) ScrollCell = (-2, -2);

		/// <summary>
		/// Показывает указанную ячейку на экране
		/// </summary>
		/// <param name="Row">Номер строки</param>
		/// <param name="Col">Номер столбца</param>
		/// <param name="Short">Признак того, что строка показывается вверху или внизу, а не в трети высоты таблицы</param>
		public void ScrollIntoView(int Row, int Col, bool Short = false)
		{
			// Номер строки неотрицательный и находится вне видимой области
			if (Row >= 0)
			{
				double v = VScroll.Value;
				if (Row < ScrollTop)
					v = Short ? Row : Row - Math.Round(VisibleRowCount * 0.392) - 1;
				else if (Row >= ScrollTop + VisibleRowCount)
				{
					v = Short ? Row - VisibleRowCount + 1 : Row - Math.Round(VisibleRowCount * 0.618);
					if (v > VScroll.Maximum)
						VScroll.Maximum = v;
				}
				v = Math.Max(VScroll.Minimum, Math.Min(v, VScroll.Maximum));
				VScroll.Value = v;
			}

			// При помоложительном номере столбца
			if (Col > 0)
			{
				// Вычисляем левую и правую границы столбца
				int Left = 0, Right = 0;
				for (int i = 1; i <= Col; i++)
				{
					Left = Right;
					Right += Columns[i].Width;
				}

				if (Left < ScrollLeft) // Скрыта левая граница
					HScroll.Value = Left;
				else
				{
					var xmax = ActualWidth - VScroll.Width - Columns[0].Width; // ширина видимой области
					if (Right - HScroll.Value > xmax) // Скрыта правая граница
						HScroll.Value = Right - xmax;
				}

			}
			InvalidateVisual();
		}

		/// <summary>
		/// Устанавливает конечную ячейку выделения
		/// </summary>
		/// <param name="Row">Номер строки</param>
		/// <param name="Col">Номер столбца</param>
		private void SetSelectionEnd(int Row, int Col)
		{
			if (SelectedCells.Move(Row, Col)) // устанавливаем конец выделения
			{
				if (Row < ScrollTop) // строка выше области видимости
					ScrollTop = Row;
				else if (Row >= ScrollTop + VisibleRowCount) // срока ниже области видимости
					ScrollTop = Row - VisibleRowCount + 1;

				if (Col == 0 && SelectedCells.First.Col > 0) // выделение пришло справа в первый столбец
					ScrollLeft = 0;
				else
					ScrollIntoView(-1, Col); // показываем нужный столбец

				InvalidateVisual();
			}
		}

		// Событие отпускания кнопки мыши
		protected override void OnMouseUp(MouseButtonEventArgs e)
		{
			// если левая кнопка была нажата и теперь отпущена
			if (IsMouseLeftButtonDown && e.LeftButton == MouseButtonState.Released)
			{
				Mouse.Capture(null); // высвобождение зохваченой мыши
				SelectionTimer.Enabled = false;
				IsMouseLeftButtonDown = false;
			}

		}

		/// <summary>
		/// Таймер, работающий когда при нажатой левой кнопке мыши
		/// курсор находится вне области прокрутки. Нужен для того,
		/// чтобы происходила автоматическая прокрутка
		/// </summary>
		private System.Timers.Timer SelectionTimer = new System.Timers.Timer()
		{
			Enabled = false,
			AutoReset = true,
		};

		/// <summary>
		/// Таймер всплывающей подсказки
		/// </summary>
		private System.Timers.Timer HintTimer = new System.Timers.Timer()
		{
			Enabled = false,
			AutoReset = false
		};

		// Событие таймера выделения
		private void SelectionTimerElapsed(object sender, ElapsedEventArgs e)
		{
			SyncCtx.Post(_ =>
			{
				var (Row, Col) = SelectedCells.Last; // последняя выделенная ячейка
				var P = Mouse.GetPosition(this); // позиция мыши
				if (P.Y < RowHeight && Row > 0) // выше таблицы
					SetSelectionEnd(Row - 1, Col);
				else if (P.Y > ActualHeight - HScroll.Height && Row < RowCount - 1) // ниже таблицы
					SetSelectionEnd(Row + 1, Col);
				else if (P.X > ActualWidth - VScroll.Width && Col < ColCount - 1) // правее таблицы
					SetSelectionEnd(Row, Col + 1);
			}, this);
		}

		/// <summary>
		/// Точка, в которой завис курсор мыши
		/// </summary>
		private Point HoverPoint;

		/// <summary>
		/// Ячейка, над которой завис курсор мыши
		/// </summary>
		private (int Row, int Col) HoverCell;

		/// <summary>
		/// Всплывающая подсказка
		/// </summary>
		private readonly ToolTip Hint;

		// Событие таймера подсказки
		private void HintTimerElapsed(object sender, ElapsedEventArgs e)
		{
			SyncCtx.Post(_ =>
			{
				if (CoordToCell(HoverPoint, out int Row, out int Col) &&
					CellHints.TryGetValue((Row, Col), out string Str) &&
					CellRect(Row, Col, out Rect R))
				{
					HoverCell = (Row, Col);
					Hint.IsOpen = false;
					Hint.Content = Str;
					Hint.PlacementRectangle = new Rect(R.Left + R.Height / 4, R.Bottom + R.Height / 4, 0, 0);
					Hint.IsOpen = true;
				}
			}, this);
		}

		/// <summary>
		/// Столбец, размер которого изменяется при помощи мыши и начальная позиция мыши
		/// </summary>
		private (int Index, int Pos) ResizedColumn = (-1, -1);

		// Событие движения мыши
		protected override void OnMouseMove(MouseEventArgs e)
		{
			var Pos = e.GetPosition(this); // позиция курсора мыши


			if (IsMouseLeftButtonDown) // если нажата левая кнопка мыши
			{
				// проверяем, что курсор находится за границами таблицы
				if (Pos.X > ActualWidth - VScroll.Width ||
					Pos.Y < RowHeight || Pos.Y > ActualHeight - HScroll.Height)
				{
					// совсем далеко, уменьшаем интервал таймера
					if (Pos.Y < -RowHeight / 2 || Pos.Y > ActualHeight + HScroll.ActualHeight / 2 ||
						Pos.X > ActualWidth + VScroll.Width / 2)
						SelectionTimer.Interval = 30;
					else
						SelectionTimer.Interval = 100;

					// включаем таймер
					if (!SelectionTimer.Enabled)
						SelectionTimer.Enabled = true;
				}
				else
				{
					// выключаем таймер
					SelectionTimer.Enabled = false;

					if (CoordToCell(Pos, out int Row, out int Col) && Row >= 0) // движемся над ячейкой
					{
						SetSelectionEnd(Row, Col);
						e.Handled = true; // событие обработано
					}
				}
			}
			else
			{
				if (CoordToCell(Pos, out int Row, out int Col)) // движение внутри ячейки
				{
					if (Row != HoverCell.Row || Col != HoverCell.Col)
					{
						HideHint(); // курсор ушёл из ячейки, для которой показана подсказка
					}

					if (CellHints.ContainsKey((Row, Col)) && !(Hint.IsOpen && Row == HoverCell.Row && Col == HoverCell.Col)) // имеется подсказка для ячейки
					{
						Hint.IsOpen = false; // скрываем подсказку
						HintTimer.Enabled = false; // останавливаем таймер, если он запущен
						HintTimer.Interval = HintDelay; // устанавливаем интервал
						HoverPoint = Pos; // запоминаем положение мыши
						HintTimer.Enabled = true; // запускаем таймер
					}

					if (Row == -1 && !Edit.Active) // находимся в заголовке, редактирование не происходит
					{
						// не выбран столбец с изменяемой шириной
						if (ResizedColumn.Index < 0)
						{
							// перебираем видимые столбцы
							for (int i = 0, nc = VisibleColumns.Count - 1; i < nc; i++)
							{
								// курсор близко к границе столбцов
								if (Math.Abs(Pos.X - VisibleColumns[i].Right) < 4)
								{
									Cursor = Cursors.SizeWE; // курсор сплиттера
									ResizedColumn = (i, Columns[VisibleColumns[i].Col].Width - (int)Pos.X); // запоминаем столбец
									e.Handled = true; // событие обработано
									return;
								}
							}
						}
						else // столбец выбран
						{
							if (e.LeftButton == MouseButtonState.Pressed) // нажата левая кнопка мыши
							{
								var w = (int)Pos.X + ResizedColumn.Pos; // вычисляем новую ширину столбца
								w = Math.Max(32, Math.Min(512, w)); // вводим ограничения
								Columns[VisibleColumns[ResizedColumn.Index].Col].Width = w; // устанавливаем ширину
								e.Handled = true; // событие обработано
							}
							else if (Math.Abs(Pos.X - VisibleColumns[ResizedColumn.Index].Right) > 4) // курсор ушёл от границы
							{
								Cursor = null; // курсор по умолчанию
								ResizedColumn = (-1, -1); // сбрасываем информацию о текущем столбце
								e.Handled = true; // событие обработано
							}
						}
					}
					else if (ResizedColumn.Index >= 0) // курсор ушёл из заголовка
					{
						Cursor = null;
						ResizedColumn = (-1, -1);
						e.Handled = true;
					}
				}
				else
				{
					Cursor = null;
					HideHint();
				}
			}
		}

		// событие обработки колеса мыши
		protected override void OnMouseWheel(MouseWheelEventArgs e)
		{
			var Pos = e.GetPosition(this); // позиция курсора
			if (Pos.Y > ActualHeight - HScroll.Height) // над горизонтальной полосой прокрутки
				HScroll.Value -= Math.Sign(e.Delta) * HScroll.SmallChange * 10;
			else // в другом месте
				VScroll.Value -= Math.Sign(e.Delta) * VScroll.SmallChange * 2;

		}

		/// <summary>
		/// Скрывает подсказку для ячейки
		/// </summary>
		private void HideHint()
		{
			HintTimer.Enabled = false;
			Hint.IsOpen = false;
			HoverCell = (-2, -2);
		}

		// Событие ухода мыши
		protected override void OnMouseLeave(MouseEventArgs e)
		{
			HideHint();
		}

		// Событие нажатия клавиши
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (ColCount != 0 && RowCount != 0 && KeyActions.TryGetValue(e.Key, out Func<GridControl, ModifierKeys, bool> A))
				e.Handled = A(this, e.KeyboardDevice.Modifiers);
		}

		// Отклик на нажатие клавиши Tab
		private static bool MoveTab(GridControl Grid, ModifierKeys Shift)
		{
			switch (Shift)
			{
				case ModifierKeys.None: // только Tab
					Grid.Edit.Hide(); // завершить редактирование
					return MoveRight(Grid, 0); // сдвинуться вправо
				case ModifierKeys.Shift: // Shift + Tab
					Grid.Edit.Hide(); // завершить редактирование
					return MoveLeft(Grid, 0); // сдвинуться влево
			}
			return false;
		}

		// Отклик на нажатие клавиши Enter
		private static bool MoveEnter(GridControl Grid, ModifierKeys Shift)
		{
			switch (Shift)
			{
				case ModifierKeys.None: // Только Enter
					Grid.Edit.Hide(); // завершить редактирование
					return MoveDown(Grid, 0); // сдвинуться вниз
				case ModifierKeys.Shift: // Shift + Enter
					Grid.Edit.Hide(); // завершить редактирование
					return MoveUp(Grid, 0); // сдвинуться вверх
				case ModifierKeys.Control: // Ctrl + Enter
					if (!Grid.Edit.Active) // редактор не активен
						Grid.PopulateSelection(); // размножить выделение
					return true;
			}
			return false;
		}

		/// <summary>
		/// Отклик на нажатие клавиш навигации
		/// </summary>
		/// <param name="Row">Приращение строки</param>
		/// <param name="Col">Приращение столбца</param>
		/// <param name="Shift">Состояние клавиш-переключателей</param>
		/// <returns>Признак обработанного события</returns>
		private bool Arrow(int Row, int Col, ModifierKeys Shift)
		{
			if ((Shift & ~(ModifierKeys.Control | ModifierKeys.Shift)) != 0)
				return false; // Если нажаты клавиши-переключатели кроме Ctrl или Shift

			var (R, C) = SelectedCells.Last; // последняя выделенная ячейка
			 
			if (R >= 0 || C >= 0) // если она есть
			{
				if ((Shift & ModifierKeys.Control) != 0) // нажат Ctrl
				{
					if (Row < 0)     // Смещаемcя до конца вверх
						R = 0;
					else if (Row > 0)  // вниз   
						R = RowCount - 1;
					if (Col < 0)   // влево
					{
						C = 0;
						ScrollLeft = 0;
					}
					else if (Col > 0) // вправо
						C = ColCount - 1;

				}
				else // ничего не нажато
				{
					R += Row; // просто смещаемся
					C += Col;
				}

				if ((Shift & ModifierKeys.Shift) == 0) // Shift НЕ нажат
					SelectCell(R, C, true); // выделяем одну ячейку
				else // таки нажат
					SetSelectionEnd(R, C); // продолжаем выделение 
			}

			return true;
		}

		// Отклик на нажатие стрелки влево
		private static bool MoveLeft(GridControl Grid, ModifierKeys Shift)
		{
			return Grid.Arrow(0, -1, Shift);
		}

		// Отклик на нажатие стрелки вправо
		private static bool MoveRight(GridControl Grid, ModifierKeys Shift)
		{
			return Grid.Arrow(0, 1, Shift);
		}

		// Отклие на нажатие стрелки вверх
		private static bool MoveUp(GridControl Grid, ModifierKeys Shift)
		{
			return Grid.Arrow(-1, 0, Shift);
		}

		// Отклик на нажатие стрелки вниз
		private static bool MoveDown(GridControl Grid, ModifierKeys Shift)
		{
			return Grid.Arrow(1, 0, Shift);
		}

		// Отклик на нажатие PageUp
		private static bool MovePageUp(GridControl Grid, ModifierKeys Shift)
		{
			if ((Shift & ~ModifierKeys.Shift) == 0) // Может быть нажат только Shift
				return Grid.Arrow(-(int)Grid.VScroll.LargeChange, 0, Shift);
			else
				return false;
		}

		// Отклик на нажатие PageDown
		private static bool MovePageDown(GridControl Grid, ModifierKeys Shift)
		{
			if ((Shift & ~ModifierKeys.Shift) == 0) // Может быть нажат только Shift
				return Grid.Arrow((int)Grid.VScroll.LargeChange, 0, Shift);
			else
				return false;
		}

		// Отклик на нажатие Home
		private static bool MoveHome(GridControl Grid, ModifierKeys Shift)
		{
			switch (Shift)
			{
				case ModifierKeys.None: // Действие не отличается от Ctrl + Left
				case ModifierKeys.Shift:
					return Grid.Arrow(0, -1, Shift | ModifierKeys.Control);
				case ModifierKeys.Control: // прыгаем в самый верхний левый угол
					Grid.SelectCell(0, 0, true);
					return true;
				case ModifierKeys.Shift | ModifierKeys.Control: // прыгаем и выделяем ячейки
					var (Row, Col) = Grid.SelectedCells.Last;
					if (Row >= 0 && Col >= 0)
						Grid.SetSelectionEnd(0, 0);
					return true;
				default:
					return false;
			}
		}

		// Отклик на нажатие End
		private static bool MoveEnd(GridControl Grid, ModifierKeys Shift)
		{
			switch (Shift)
			{
				case ModifierKeys.None: // действие не отличается от Ctrl + Right
				case ModifierKeys.Shift:
					return Grid.Arrow(0, 1, Shift | ModifierKeys.Control);
				case ModifierKeys.Control: // прыгаем в самый правый нижний угол
					Grid.SelectCell(Grid.RowCount - 1, Grid.ColCount - 1, true);
					return true;
				case ModifierKeys.Shift | ModifierKeys.Control: // прыгаем и выделяем
					var (Row, Col) = Grid.SelectedCells.Last;
					if (Row >= 0 && Col >= 0)
						Grid.SetSelectionEnd(Grid.RowCount - 1, Grid.ColCount - 1);
					return true;
				default:
					return false;
			}
		}

		// Отклик на нажатие Esc
		private static bool MoveEscape(GridControl Grid, ModifierKeys Shift)
		{
			if (Shift == 0) // ничего не нажато
			{
				Grid.HideHint(); // скрываем подсказку
				return true;
			}
			return false;
		}

		/// <summary>
		/// Список методов реакции на нажатие клавиш
		/// </summary>
		private static readonly Dictionary<Key, Func<GridControl, ModifierKeys, bool>> KeyActions =
			new Dictionary<Key, Func<GridControl, ModifierKeys, bool>>()
		{
			{ Key.Tab, MoveTab },
			{ Key.Enter, MoveEnter },
			{ Key.Left, MoveLeft },
			{ Key.Right, MoveRight },
			{ Key.Up, MoveUp },
			{ Key.Down, MoveDown },
			{ Key.PageDown, MovePageDown },
			{ Key.PageUp, MovePageUp },
			{ Key.Home, MoveHome },
			{ Key.End, MoveEnd },
			{ Key.Escape, MoveEscape },
		};


		// Событие ввода текста. Если пользователь его начал до открытия редактора
		protected override void OnTextInput(TextCompositionEventArgs e)
		{
			var (Row, Col) = SelectedCells.Current; // текущая ячейка
			if (Row >= 0 && Col >= 0 && !Columns[Col].ReadOnly && DataSource.IsRowLocked(Row) == 0)
			{
				if (!SelectedCells.SingleCell) // сбрасываем выделение
					SelectCell(Row, Col);
				else
					ScrollIntoView(Row, Col); // откручиваемся до текущей ячейки

				// Начинаем редактирование. Если пользователь нажал пробел, текст ячейки не очищается
				Edit.Show(Row, Col, e.Text == " " ? DataSource.GetEditCell(Row, Col) : e.Text);
			}
			e.Handled = true;
		}

		/// <summary>
		/// Возвращает текст ячейки
		/// </summary>
		/// <param name="Row">Номер строки</param>
		/// <param name="Col">Номер столбца</param>
		/// <returns>Текст ячейки</returns>
		public string GetCell(int Row, int Col)
		{
			return DataSource.GetEditCell(Row, Col);
		}

		/// <summary>
		/// Устанавливает текст ячейки
		/// </summary>
		/// <param name="Row">Номер строки</param>
		/// <param name="Col">Номер столбца</param>
		/// <param name="Text">Текст ячейки</param>
		public void SetCell(int Row, int Col, string Text)
		{
			if (Columns[Col].ReadOnly)
				return;

			BeginUpdate();
			try
			{
				var Old = GetCell(Row, Col);

				DataSource.SetCell(Row, Col, Text);
				if (Changes.IsUnlocked)
				{
					Changes.Log(() =>
					{
						DataSource.SetCell(Row, Col, Old);
					}, () =>
					{
						DataSource.SetCell(Row, Col, Text);
					});
				}
			}
			finally
			{
				EndUpdate();
			}
		}

		/// <summary>
		/// Событие начала ожидания завершения асинхронного процесса
		/// </summary>
		public event Action<Object, long> StartWaiting;

		/// <summary>
		/// Событие завершения ожидания асинхронного процесса.
		/// Получает код и сообщение об ошибке
		/// </summary>
		public event Action<Object, long, int, string> EndWaiting;

		/// <summary>
		/// Вызывает асинхронный метод сортировки или фильтрации
		/// </summary>
		/// <param name="Str">Выражение для сортировки или фильтрации</param>
		/// <param name="Method">Метод сортировки или фильтрации</param>
		private void AsyncProcess(string Str, Action<string, IFinishEvent> Method)
		{
			var Snap = Server.Rdtsc();


			var OldOrder = DataSource.GetRowOrder();
			var OldSelection = SelectedCells.Snap(ScrollLeft, ScrollTop);
			var CurrentCell = SelectedCells.Current;
			DataSource.MarkRow(CurrentCell.Row);

			bool Filter = Method == DataSource.Filter;

			try
			{
				Method(Str, new FinishEvent((int Code, string Message) =>
				{
					if (Code == 0)
					{
						int M = 0;
						if (RowCount > 0 && (M = DataSource.GetMarkedRow()) >= 0)
							SelectCell(M, CurrentCell.Col);
						else
							Refresh();

						var NewOrder = DataSource.GetRowOrder();
						if (Changes.IsUnlocked)
						{
							Changes.Log(() =>
							{
								DisplayRequest?.Invoke(this);

								DataSource.RestoreRowOrder(OldOrder);
								SelectedCells.Restore(OldSelection);
								ScrollLeft = OldSelection[0];
								ScrollTop = OldSelection[1];
								Refresh();

							}, () =>
							{

								DisplayRequest?.Invoke(this);

								DataSource.RestoreRowOrder(NewOrder);
								if (RowCount > 0 && M < RowCount)
									SelectCell(M, CurrentCell.Col);
								else
									Refresh();
							});
						}

					}
					EndWaiting?.Invoke(this, Snap, Code, Message);
				}));

				StartWaiting?.Invoke(this, Snap);
			}
			catch (Exception e)
			{
				EndWaiting?.Invoke(this, Snap, 53, e.Message); 
			}
		}


		/// <summary>
		/// Производит сортировку строк таблицы. В начале метода происходит событие StartWaiting
		/// Система должна быть заблокирована до получения события EndWaiting
		/// </summary>
		/// <param name="Order">Порядок сортировки</param>
		public void Sort(string Order)
		{
			AsyncProcess(Order, DataSource.Sort);
		}

		/// <summary>
		/// Производит фильтрацию строк таблицы. В начале метода происходит событие StartWaiting
		/// Система должна быть заблокирована до получения события EndWaiting
		/// </summary>
		/// <param name="Exp">Выражение фильтра</param>
		public void Filter(string Exp)
		{
			AsyncProcess(Exp, DataSource.Filter);
		}

		// Освобождает ресурсы источника данных таблицы
		void IDisposable.Dispose()
		{
			Marshal.ReleaseComObject(DataSource);
		}


		/// <summary>
		/// Редактор текста в ячейках 
		/// </summary>
		private class EditControl : FrameworkElement
		{
			/// <summary>
			/// Количество невидимых символов
			/// </summary>
			public static readonly DependencyProperty OffsetProperty;

			/// <summary>
			/// Начало выделения
			/// </summary>
			public static readonly DependencyProperty SelectionStartProperty;

			/// <summary>
			/// Конец выделения
			/// </summary>
			public static readonly DependencyProperty SelectionEndProperty;

			/// <summary>
			/// Создание свойсва зависимости
			/// </summary>
			/// <typeparam name="T">Тип свойства</typeparam>
			/// <param name="Name">Имя свойства</param>
			/// <param name="Default">Значение по умолчанию</param>
			/// <returns></returns>
			private static DependencyProperty PropReg<T>(string Name, T Default)
			{
				FrameworkPropertyMetadata fpm = new FrameworkPropertyMetadata
				{
					AffectsArrange = true,
					AffectsMeasure = false,
					AffectsRender = true,
					DefaultValue = Default,
					
				};


				return DependencyProperty.Register(Name, typeof(T), typeof(EditControl), fpm);
			}

			/// <summary>
			/// Создание свойств зависимостей
			/// </summary>
			static EditControl()
			{
				OffsetProperty = PropReg("Offset", 0);
				SelectionStartProperty = PropReg("SelectionStart", 0);
				SelectionEndProperty = PropReg("SelectionEnd", 0);
			}

			/// <summary>
			/// Таблица
			/// </summary>
			private readonly GridControl Master;

			/// <summary>
			/// Каретка (моргающая палочка)
			/// </summary>
			private readonly CaretControl Caret;

			/// <summary>
			/// Создаёт редактор ячейки
			/// </summary>
			/// <param name="master">Таблица</param>
			public EditControl(GridControl master)
			{
				Master = master;  // сохраняем таблицу

				// Нужны кристально ровные линии
				SnapsToDevicePixels = true;
				SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);

				Focusable = true; // редактор может получать фокус
				FocusVisualStyle = null; // но не хочет его показывать

				Caret = new CaretControl(this); // создаём каретку

				Master.AddLogicalChild(this); // прописываемся у мастера
				Master.AddVisualChild(this);

				Visibility = Visibility.Hidden; // редактор изначально скрыт
				Active = false;
				Cursor = Cursors.IBeam;
			}

			/// <summary>
			/// Номер строки редактируемой ячейки
			/// </summary>
			public int Row { get; private set; }

			/// <summary>
			/// Номер столбца редактируемой ячейки
			/// </summary>
			public int Col { get; private set; }

			/// <summary>
			/// Признак активности редактора
			/// </summary>
			public bool Active { get; private set; }

			/// <summary>
			/// Начинает редактирование ячейки
			/// </summary>
			/// <param name="row">Номер строки</param>
			/// <param name="col">Номер столбца</param>
			/// <param name="text">Редактируемый текст</param>
			public void Show(int row, int col, string text)
			{
				Text = text;
				Row = row;
				Col = col;
				Active = true;
				Visibility = Visibility.Visible;
				Master.InvalidateArrange();
				Master.InvalidateVisual();
				Focus();
				Offset = 0;
				SelStart = SelEnd = text.Length;
			}

			/// <summary>
			/// Прекращает редактирование ячейки
			/// </summary>
			/// <param name="Esc">Признак выхода без запоминания текста</param>
			public void Hide(bool Esc = false)
			{
				if (Active)
				{
					Escaped = Esc;
					Master.Focus();
				}
			}

			/// <summary>
			/// Проверяет, что указанная ячейка редактируется
			/// </summary>
			/// <param name="row">Номер строки</param>
			/// <param name="col">Номер столбца</param>
			/// <returns>Признак редактиования ячейки</returns>
			public bool IsCellEdited(int row, int col) => Active && Row == row && Col == col;


			/// <summary>
			/// Список символов редактируемого текста
			/// </summary>
			private readonly List<FormattedText> Characters = new List<FormattedText>();

			/// <summary>
			/// Количество невидимых символов
			/// </summary>
			public int Offset
			{
				get => (int)GetValue(OffsetProperty);
				set => SetValue(OffsetProperty, Math.Max(0, Math.Min(value, Characters.Count - 4)));
			}

			/// <summary>
			/// Начало выделения
			/// </summary>
			public int SelStart
			{
				get => (int)GetValue(SelectionStartProperty);
				set => SetValue(SelectionStartProperty, Math.Max(0, Math.Min(value, Characters.Count)));
			}

			/// <summary>
			/// Конец выделения. Так же определяет положение каретки
			/// </summary>
			public int SelEnd
			{
				get => (int)GetValue(SelectionEndProperty);
				set => SetValue(SelectionEndProperty, Math.Max(0, Math.Min(value, Characters.Count)));
			}

			/// <summary>
			/// Начало и конец выделения (отсортированные)
			/// </summary>
			public (int Start, int End) Selection
			{
				get
				{
					int ss = SelStart, se = SelEnd;
					if (ss > se)
					{
						ss = se;
						se = SelStart;
					}
					return (ss, se);
				}
			}

			/// <summary>
			/// Выделенный текст
			/// </summary>
			public string SelText
			{
				get
				{
					var (S, E) = Selection;
					StringBuilder s = new StringBuilder();
					for (; S < E; S++)
						s.Append(Characters[S].Text);
					return s.ToString();
				}
			}

			/// <summary>
			/// Редактируемый текст
			/// </summary>
			public string Text
			{
				get
				{
					StringBuilder s = new StringBuilder();
					foreach (var ch in Characters)
						s.Append(ch.Text);
					return s.ToString();
				}
				set
				{
					var ppd = VisualTreeHelper.GetDpi(this).PixelsPerDip;
					var Cult = CultureInfo.CurrentUICulture;
					var FlowDir = FlowDirection.LeftToRight;
					var Type = new Typeface(Master.FontName);
					var Size = Master.FontSize;
					var Br = Master.CellForeground;

					// Разбиваем текст на отдельные символы
					// Потому-что нет никакой возможности получить
					// ширину нескольких символов из объекта FormattedText
					Characters.Clear();
					foreach (var ch in value)
						Characters.Add(new FormattedText(ch.ToString(), Cult, FlowDir, Type, Size, Br, null,
							Master.TextSmoothing, ppd));

					Offset = 0;
					SelStart = 0;
					SelEnd = 0;
				}
			}


			/// <summary>
			/// Возвращает позицию символа по точке на экране или -1, если точка вне символа
			/// </summary>
			/// <param name="x">Расстояние от точки до левой грани элемента</param>
			/// <returns>Позиция символа</returns>
			public int PointToIndex(double x)
			{
				double p = Master.CellPadding, w = RenderSize.Width - 2 * p - 1, x1 = p;
				if (x < p || x > w)
					return -1;

				int n = Characters.Count;
				for (int i = Offset; i < n; i++)
				{
					x1 += Characters[i].WidthIncludingTrailingWhitespace;
					if (x1 > x)
						return i;
				}

				return n;
			}

			/// <summary>
			/// Возвращает положение смивола на экране или -1 для символов вне видимой области
			/// </summary>
			/// <param name="Index">Номер символа</param>
			/// <returns>Положение символа на экране</returns>
			public int IndexToPoint(int Index)
			{
				if (Index < Offset)
					return -1;

				int n = Characters.Count;
				double p = Master.CellPadding, w = RenderSize.Width - 2 * p - 1, x = p;
				for (int i = Offset; i < Index; i++)
					x += Characters[i].WidthIncludingTrailingWhitespace;

				return (int)Math.Round(x);
			}

			// Опеделение положения каретки
			protected override Size ArrangeOverride(Size finalSize)
			{
				if (SelEnd < Offset) // Каретка не видна слева
					Offset = Math.Max(0, SelEnd - 4); // показать её

				// вычисляем положение каретки и ширины символов
				double p = Master.CellPadding, x = p, w = finalSize.Width - 2 * p - 1;
				for (int i = Offset; i < SelEnd; i++)
					x += Characters[i].WidthIncludingTrailingWhitespace; // зачем-то они определяют ширину без пробелов

				// Каретка не видна справа
				if (x > w)
				{
					int Delta = 0, i = SelEnd - 1;
					while (x > w) // пока каретка не видна, увеличиваем смещение 
					{
						Delta++;
						x -= Characters[i--].WidthIncludingTrailingWhitespace;
					}
					for (int j = 0; j < 4; j++) // и ещё на четыре символа
						x -= Characters[i--].WidthIncludingTrailingWhitespace;

					Offset += Delta + 4;
					InvalidateVisual();
				}

				Caret.Arrange(new Rect(x, p, 1, finalSize.Height - 2 * p));
				return finalSize;
			}

			// Внутренние элементы управления
			protected override int VisualChildrenCount => 1;

			// Одна только каретка
			protected override Visual GetVisualChild(int index) => Caret;

			// При закрытии редактора не происходит обновление текста ячейки в источнике данных
			private bool Escaped = false;

			// Событие получения фокуса
			protected override void OnGotFocus(RoutedEventArgs e)
			{
				Escaped = false;
				Caret.Active = true; // показываем каретку
				e.Handled = true;
			}

			// Событие потери фокуса
			protected override void OnLostFocus(RoutedEventArgs e)
			{
				Active = false;
				Visibility = Visibility.Hidden;
				Caret.Active = false;  // убираем каретку
				if (!Escaped) // надо сохранить значение
					Master.SetCell(Row, Col, Text); // сохраняем
				else
					Master.Refresh(); // или просто перерисовываемся
				e.Handled = true;
			}

			// Событие перерисовки
			protected override void OnRender(DrawingContext dc)
			{
				Rect 
					r = new Rect(Master.CellPadding, 0, RenderSize.Width - Master.CellPadding * 2, RenderSize.Height - 1), // граниицы элемента управления
					r1 = new Rect(r.Left, r.Left, 0, r.Height - 2 * r.Left); // границы текущего символа

				dc.PushClip(new RectangleGeometry(r)); // зона отсечения
				dc.DrawRectangle(Master.CellBackground, null, r); // очищаем фон

				int n = Characters.Count;
				if (n != 0)
				{
					var (ss, se) = Selection; // границы выделения

					// Кисти
					Brush cb = Master.CellBackground,
						sb = Master.SelectionBackground,
						cf = Master.CellForeground,
						sf = Master.SelectionForeground;

					// Рисуем символы. 
					// Надо понимать, что наши израильские и арабские потенциальные коллеги
					// не смогут воспользоваться нашим поделием. Ровно как и европейские,
					// которые используют диакритические символы-модификаторы, вместо готовых символов
					for (int i = Offset; i < n; i++)
					{
						r1.X += r1.Width;
						r1.Width = Characters[i].WidthIncludingTrailingWhitespace;
						bool sel = i >= ss && i < se;
						Characters[i].SetForegroundBrush(sel ? sf : cf);
						dc.DrawRectangle(sel ? sb : cb, null, r1);
						dc.DrawText(Characters[i], r1.TopLeft);
					}
				}
				dc.Pop();

			}

			// Событие движения мыши
			protected override void OnMouseMove(MouseEventArgs e)
			{
				if (e.LeftButton == MouseButtonState.Pressed && Mouse.Captured == this) // нажата левая кнопка и мышь зохвачена
				{
					var x = e.GetPosition(this).X; // Горизонатальная позиция
					if (x < Master.CellPadding) // Сильно слева
						SelEnd = 0; // смещаемся влево до упора
					else if (x > ActualWidth - Master.CellPadding) // сильно справа
						SelEnd = Characters.Count; // вправо до упора
					else
						SelEnd = PointToIndex(e.GetPosition(this).X); // устанавливаем текущий символ
				}
			}

			// Событие нажатия кнопки мыши
			protected override void OnMouseDown(MouseButtonEventArgs e)
			{
				if (e.LeftButton == MouseButtonState.Pressed) //левая кнопка
				{
					SelStart = SelEnd = PointToIndex(e.GetPosition(this).X); // устанавливаем текущий символ
					Mouse.Capture(this); // зохват мыши. В стандартном WinAPI любое окно, в котором нажата кнопка мыши, получает её захват без лишних проволочек
				}
				e.Handled = true;
			}

			// Событие отпускания кнопки мыши
			protected override void OnMouseUp(MouseButtonEventArgs e)
			{
				if (e.LeftButton == MouseButtonState.Released && Mouse.Captured == this)
					Mouse.Capture(null);
				e.Handled = true;
			}

			// Событие нажатия клавиши
			protected override void OnKeyDown(KeyEventArgs e)
			{
				if (KeyboardActions.TryGetValue(e.Key, out Func<EditControl, ModifierKeys, bool> A))
				{
					e.Handled = A(this, e.KeyboardDevice.Modifiers);
				}
			}

			// Событие ввода текста
			protected override void OnTextInput(TextCompositionEventArgs e)
			{
				Insert(e.Text, 0);
				e.Handled = true;
			}

			/// <summary>
			/// Отклик на нажатие стрелки вправо
			/// </summary>
			/// <param name="Edit">Редактор текста</param>
			/// <param name="Shift">Состояение клавиш-переключателей</param>
			/// <returns>Признак обработки события</returns>
			private static bool MoveRight(EditControl Edit, ModifierKeys Shift)
			{
				switch (Shift)
				{
					case ModifierKeys.None:
						Edit.SelEnd = ++Edit.SelStart;
						return true;
					case ModifierKeys.Shift:
						Edit.SelEnd++;
						return true;
				}
				return false;
			}

			/// <summary>
			/// Отклик на нажатие стрелки влево
			/// </summary>
			private static bool MoveLeft(EditControl Edit, ModifierKeys Shift)
			{
				switch (Shift)
				{
					case ModifierKeys.None:
						Edit.SelEnd = --Edit.SelStart;
						return true;
					case ModifierKeys.Shift:
						Edit.SelEnd--;
						return true;
				}
				return false;
			}

			/// <summary>
			/// Пустой отклик на нажатие клавиши
			/// </summary>
			private static bool MoveNone(EditControl Edit, ModifierKeys Shift)
			{
				return Shift == 0;
			}

			/// <summary>
			/// Отклик на нажатие клавиши Home
			/// </summary>
			private static bool MoveHome(EditControl Edit, ModifierKeys Shift)
			{
				switch (Shift)
				{
					case ModifierKeys.Shift:
						Edit.SelEnd = Edit.SelStart;
						Edit.SelStart = 0;
						Edit.Offset = 0;
						return true;
					case ModifierKeys.None:
						Edit.Offset = 0;
						Edit.SelStart = 0;
						Edit.SelEnd = 0;
						return true;
				}
				return false;
			}

			/// <summary>
			/// Отклик на нажатие клавиши End
			/// </summary>
			private static bool MoveEnd(EditControl Edit, ModifierKeys Shift)
			{
				switch (Shift)
				{
					case ModifierKeys.Shift:
						Edit.SelEnd = Edit.Characters.Count + 1;
						return true;
					case ModifierKeys.None:
						Edit.SelStart = Edit.SelEnd = Edit.Characters.Count + 1;
						return true;
				}
				return false;
			}

			/// <summary>
			/// Отклик на нажатие клавиши Delete
			/// </summary>
			private static bool MoveDelete(EditControl Edit, ModifierKeys Shift)
			{
				if (Shift == 0)
				{
					Edit.Insert("", 1);
					return true;
				}
				return false;
			}

			/// <summary>
			/// Отклик на нажатие клавиши Backspace
			/// </summary>
			private static bool MoveBackspace(EditControl Edit, ModifierKeys Shift)
			{
				if (Shift == 0)
				{
					Edit.Insert("", -1);
					return true;
				}
				return false;
			}

			/// <summary>
			/// Отклик на нажатие клавиши Escape
			/// </summary>
			private static bool MoveEscape(EditControl Edit, ModifierKeys Shift)
			{
				if (Shift == 0)
				{
					Edit.Hide(true);
					return true;
				}
				return false;
			}

			/// <summary>
			/// Структура методов отклика нажатия клавиш. Во избежание большого и не красивого свича в методе OnKeyDown
			/// </summary>
			private static readonly Dictionary<Key, Func<EditControl, ModifierKeys, bool>> KeyboardActions =
				new Dictionary<Key, Func<EditControl, ModifierKeys, bool>>()
			{
				{ Key.Left, MoveLeft },
				{ Key.Right, MoveRight },
				{ Key.Up, MoveNone },
				{ Key.Down, MoveNone },
				{ Key.Delete, MoveDelete },
				{ Key.Back, MoveBackspace },
				{ Key.Home, MoveHome },
				{ Key.End, MoveEnd },
				{ Key.Escape, MoveEscape },
			};

			/// <summary>
			/// Выставляет текст в редактор
			/// </summary>
			/// <param name="Str">Вставляемый текст</param>
			/// <param name="Delete">0 – вставка, +1 – удаление вправо, -1 – удаление влево</param>
			public void Insert(string Str, int Delete)
			{
				if (Delete == 0) // вставка текста
				{
					var ppd = VisualTreeHelper.GetDpi(this).PixelsPerDip; // параметры FormattedText (ну не можем мы их копировать)
					var Cult = CultureInfo.CurrentUICulture;
					var FlowDir = FlowDirection.LeftToRight;
					var Type = new Typeface(Master.FontName);
					var Size = Master.FontSize;
					var Br = Master.CellForeground;

					var (S, E) = Selection; // выделение
					if (S != E) // есть выделение
						Characters.RemoveRange(S, E - S); // больше нет

					var f = new List<FormattedText>();
					foreach (var ch in Str) // создаём отдельный объект на кажную букву
						f.Add(new FormattedText(ch.ToString(), Cult, FlowDir, Type, Size, Br, null,
							Master.TextSmoothing, ppd));

					Characters.InsertRange(S, f); // вставляем в общий список
					SelStart = SelEnd = S + Str.Length; // перемещаем каретку
				}
				else if (SelStart != SelEnd) // очистка выделения
				{
					var (S, E) = Selection;
					Characters.RemoveRange(S, E - S);
					SelStart = SelEnd = S;
				}
				else if (Delete > 0) // удаление текущего символа
				{
					if (SelEnd < Characters.Count)
					{
						Characters.RemoveAt(SelEnd);
						InvalidateArrange();
						InvalidateVisual();
					}
				}
				else if (SelEnd != 0)// удаление предыдущего символа
				{
					Characters.RemoveAt(SelEnd - 1);
					SelStart = --SelEnd;
				}
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
						Interval = 750,
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
						Timer.Enabled = value;
						if (!value)
							Visibility = Visibility.Hidden;
					}
				}

				// Событие таймера
				private void TimerElapsed(object sender, ElapsedEventArgs e)
				{
					Master.Master.SyncCtx.Post(_ =>
					{
						// Изменяем признак видимости на противоположный
						Visibility = (Visibility)(((int)Visibility + 1) & 1);
					}, this);
				}

				// Метод отрисовки
				protected override void OnRender(DrawingContext dc)
				{
					Pen p = new Pen(Master.Master.CellForeground, 1);
					dc.DrawLine(p, new Point(0, 0), new Point(0, RenderSize.Height));
				}
			}
		}
	}




}