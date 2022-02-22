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
	/// ����� �������� ���������� ������� � ���������� �����������
	/// </summary>
	public class GridControl : FrameworkElement, IDisposable
	{
        #region
        /// <summary>
        /// ��� ������. �� ��������� ������ �������� ����� ������� ����� ����������
        /// </summary>
        public static readonly DependencyProperty FontNameProperty;

		/// <summary>
		/// ������ ������. �� ��������� ������ �������� ����� ������� ����� ����������
		/// </summary>
		public static readonly DependencyProperty FontSizeProperty;

		/// <summary>
		/// ����� ���� �����
		/// </summary>
		public static readonly DependencyProperty CellBackgroundProperty;

		/// <summary>
		/// ����� �������� � �������
		/// </summary>
		public static readonly DependencyProperty CellForegroundProperty;

		/// <summary>
		/// ����� ���� ��������� �������
		/// </summary>
		public static readonly DependencyProperty CaptionBackgroundProperty;

		/// <summary>
		/// ����� �������� ��������� �������
		/// </summary>
		public static readonly DependencyProperty CaptionForegroundProperty;

		/// <summary>
		/// ����� ���� �������� ������ ��� ������
		/// </summary>
		public static readonly DependencyProperty ReadOnlyBackgroundProperty;

		/// <summary>
		/// ����� �������� �������� ������ ��� ������
		/// </summary>
		public static readonly DependencyProperty ReadOnlyForegroundProperty;

		/// <summary>
		/// ����� ���� ���������� �����
		/// </summary>
		public static readonly DependencyProperty SelectionBackgroundProperty;

		/// <summary>
		/// ����� �������� ���������� �����
		/// </summary>
		public static readonly DependencyProperty SelectionForegroundProperty;

		/// <summary>
		/// ����� ����������� ����� ���� ����� � ���������� ���������
		/// </summary>
		public static readonly DependencyProperty ErrorBackgroundProperty;

		/// <summary>
		/// ����� ����������� ����� �������� ����� � ���������� ���������
		/// </summary>
		public static readonly DependencyProperty ErrorForegroundProperty;

		/// <summary>
		/// ����� ����� ����� �������
		/// </summary>
		public static readonly DependencyProperty GridLineProperty;

		/// <summary>
		/// ����� ������� �����, ����� �������� ���������, ��� ��� �������� ���������� ��������
		/// </summary>
		public static readonly DependencyProperty ErrorThresholdProperty;

		/// <summary>
		/// �������� Padding ��� ����� �������
		/// </summary>
		public static readonly DependencyProperty CellPaddingProperty;

		/// <summary>
		/// ������������ ������ �������
		/// </summary>
		public static readonly DependencyProperty MaxColumnWidthProperty;

		/// <summary>
		/// ����������� ������ �������
		/// </summary>
		public static readonly DependencyProperty MinColumnWidthProperty;

		/// <summary>
		/// ������ ������� ��-���������
		/// </summary>
		public static readonly DependencyProperty DefaultColumnWidthProperty;

		/// <summary>
		/// �������� ����� ���������� ���� � ������� ���������
		/// </summary>
		public static readonly DependencyProperty HintDelayProperty;

		/// <summary>
		/// �������� ������� ���������. ������ �� �������� ��������������
		/// ���������, ����� � ������ ��������� ������ ���� ��������� ��� �������
		/// </summary>
		public static readonly DependencyProperty ScrollIntervalProperty;

		/// <summary>
		/// ������������ �������� �����������
		/// </summary>
		/// <typeparam name="T">��� ��������</typeparam>
		/// <param name="Name">��� ��������</param>
		/// <param name="Arrange">���������� ������� �������� �� ���������� �������� �������</param>
		/// <param name="Render">���������� ������� �������� �� ����������� �������</param>
		/// <param name="Default">�������� �������� �� ���������</param>
		/// <returns>������ �������� �����������</returns>
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
		/// ����������� �����������, �������������� �������� �����������
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
        /// ������ �������
        /// </summary>
        public GridControl()
		{
			// ���������� ������� ����� ���������
			VScroll.ValueChanged += VScrollValueChanged;
			HScroll.ValueChanged += HScrollValueChanged;

			// ��������� ������������ ������ ��������� ��������� ��������� ����������
			AddVisualChild(VScroll);
			AddLogicalChild(VScroll);

			// �� �� ��� �������������� ������ ���������
			AddVisualChild(HScroll);
			AddLogicalChild(HScroll);

			Focusable = true; // ������� �����������
			FocusVisualStyle = null; // ������� ��������� �����������

			// ����� �������� ���������� �������
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
		/// ��� ������
		/// </summary>
		public string FontName { get => (string)GetValue(FontNameProperty); set => SetValue(FontNameProperty, value); }

		/// <summary>
		/// ������ ������
		/// </summary>
		public int FontSize { get => (int)GetValue(FontSizeProperty); set => SetValue(FontSizeProperty, value); }

		/// <summary>
		/// ����� ���� ���������� �����
		/// </summary>
		public Brush SelectionBackground { get => (Brush)GetValue(SelectionBackgroundProperty); set => SetValue(SelectionBackgroundProperty, value); }

		/// <summary>
		/// ����� �������� ���������� �����
		/// </summary>
		public Brush SelectionForeground { get => (Brush)GetValue(SelectionForegroundProperty); set => SetValue(SelectionForegroundProperty, value); }

		/// <summary>
		/// ����� ���� �����
		/// </summary>
		public Brush CellBackground { get => (Brush)GetValue(CellBackgroundProperty); set => SetValue(CellBackgroundProperty, value); }

		/// <summary>
		/// ����� �������� �����
		/// </summary>
		public Brush CellForeground { get => (Brush)GetValue(CellForegroundProperty); set => SetValue(CellForegroundProperty, value); }

		/// <summary>
		/// ����� ���� ���������
		/// </summary>
		public Brush CaptionBackground { get => (Brush)GetValue(CaptionBackgroundProperty); set => SetValue(CaptionBackgroundProperty, value); }

		/// <summary>
		/// ����� �������� ���������
		/// </summary>
		public Brush CaptionForeground { get => (Brush)GetValue(CaptionForegroundProperty); set => SetValue(CaptionForegroundProperty, value); }

		/// <summary>
		/// ����� ���� �������� ������ ��� ������
		/// </summary>
		public Brush ReadOnlyBackground { get => (Brush)GetValue(ReadOnlyBackgroundProperty); set => SetValue(ReadOnlyBackgroundProperty, value); }

		/// <summary>
		/// ����� �������� �������� ������ ��� ������
		/// </summary>
		public Brush ReadOnlyForeground { get => (Brush)GetValue(ReadOnlyForegroundProperty); set => SetValue(ReadOnlyForegroundProperty, value); }

		/// <summary>
		/// ����� ����� �������
		/// </summary>
		public Brush GridLine { get => (Brush)GetValue(GridLineProperty); set => SetValue(GridLineProperty, value); }

		/// <summary>
		/// ����� ����������� ����� ���� ��������� �����. 
		/// �������� ������ ������ �� ��������� ������ � ������� ���������� ������.
		/// ���������� �������� �����.
		/// </summary>
		public Func<double, bool, Brush> ErrorBackground { get => (Func<double, bool, Brush>)GetValue(ErrorBackgroundProperty); set => SetValue(ErrorBackgroundProperty, value); }

		/// <summary>
		/// ����� ����������� ����� �������� ��������� �����. 
		/// �������� ������ ������ �� ��������� ������ � ������� ���������� ������.
		/// ���������� �������� �����.
		/// </summary>
		public Func<double, bool, Brush> ErrorForeground { get => (Func<double, bool, Brush>)GetValue(ErrorForegroundProperty); set => SetValue(ErrorForegroundProperty, value); }

		/// <summary>
		/// ����� �������� ������� �����, ����� �������� ��� ��������� ����������
		/// </summary>
		public double ErrorThreshold { get => (double)GetValue(ErrorThresholdProperty); set => SetValue(ErrorThresholdProperty, value); }

		/// <summary>
		/// �������� Padding ��� ����� �������
		/// </summary>
		public int CellPadding { get => (int)GetValue(CellPaddingProperty); set => SetValue(CellPaddingProperty, value); }

		/// <summary>
		/// ������������ ������ �������
		/// </summary>
		public int MaxColumnWidth { get => (int)GetValue(MaxColumnWidthProperty); set => SetValue(MaxColumnWidthProperty, value); }

		/// <summary>
		/// ����������� ������ �������
		/// </summary>
		public int MinColumnWidth { get => (int)GetValue(MinColumnWidthProperty); set => SetValue(MinColumnWidthProperty, value); }

		/// <summary>
		/// ������ ������� �� ���������
		/// </summary>
		public int DefaultColumnWidth { get => (int)GetValue(DefaultColumnWidthProperty); set => SetValue(DefaultColumnWidthProperty, value); }

		/// <summary>
		/// �������� ����� ���������� ���� � ������� ���������
		/// </summary>
		public int HintDelay { get => (int)GetValue(HintDelayProperty); set => SetValue(HintDelayProperty, value); }

		/// <summary>
		/// �������� ������� ���������
		/// </summary>
		public int ScrollInterval { get => (int)GetValue(ScrollIntervalProperty); set => SetValue(ScrollIntervalProperty, value); }

		/// <summary>
		/// ������� ������ ������ �������
		/// </summary>
		public int RowHeight { get; private set; }
		
		/// <summary>
		/// ������� ���������� ����� � ������� ������� �������
		/// </summary>
		public int VisibleRowCount { get; private set; }

		/// <summary>
		/// ������� ��������� �������������� ������ ���������
		/// </summary>
		public int ScrollLeft { get => (int)HScroll.Value; set => HScroll.Value = value; }

		/// <summary>
		/// ������� ��������� ������������ ������ ���������
		/// </summary>
		public int ScrollTop { get => (int)VScroll.Value; set => VScroll.Value = value; }

		/// <summary>
		/// ������� �������� ������
		/// </summary>
		private TextFormattingMode TextSmoothing;

		/// <summary>
		/// �������� ������ �������
		/// </summary>
		public IGridDataSource DataSource { get; private set; }

		/// <summary>
		/// ����� ������� �������
		/// </summary>
		private class Column
		{
			/// <summary>
			/// ������ ������� �������
			/// </summary>
			/// <param name="index">����� �������</param>
			/// <param name="Caption">��������� �������</param>
			/// <param name="Width">������ �������</param>
			/// <param name="ReadOnly">������� ������� ������ ��� ������</param>
			/// <param name="OnChanged">������� ��������� �������</param>
			public Column(int index, string Caption, int Width, int ReadOnly, Action<Column> OnChanged)
			{
				Index = index;
				caption = Caption;
				width = Width;
				readOnly = ReadOnly != 0;
				ColumnChanged += OnChanged;
			}

			/// <summary>
			/// ����� �������
			/// </summary>
			public int Index { get; private set; }

			/// <summary>
			/// ������� ��������� �������
			/// </summary>
			public event Action<Column> ColumnChanged;

			/// <summary>
			/// ��������� �������
			/// </summary>
			private string caption;

			/// <summary>
			/// ��������� �������
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
			/// ������ �������
			/// </summary>
			private int width;

			/// <summary>
			/// ������ �������
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
			/// ������� ������� ������ ��� ������
			/// </summary>
			private bool readOnly;

			/// <summary>
			/// ������� ������� ������ ��� ������
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
		/// ������ �������� �������
		/// </summary>
		private readonly List<Column> Columns = new List<Column>();

		/// <summary>
		/// ��������� �������� ������� � ��������� ������
		/// </summary>
		/// <param name="ds">�������� ������</param>
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
		/// ��������� ������ ���������� ��������� ������
		/// </summary>
		public void Reset()
		{
			DataSource.Filter("", null);
			VScroll.Value = 0;
			HScroll.Value = 0;
			Refresh();
		}


		/// <summary>
		/// ��������� ��������� �������
		/// </summary>
		/// <param name="Root">������ �������� ��������</param>
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
		/// ��������� ������ ������������ �������
		/// </summary>
		/// <param name="Config">������ ������������</param>
		/// <param name="Filter">������ �������</param>
		/// <param name="Order">������� ����������</param>
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
		/// ���������� ������ ��� ���������� ���������
		/// </summary>
		/// <returns>������ ��� ����������</returns>
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
		/// ��������������� ��������� �� ���������� ������
		/// </summary>
		/// <param name="Data">������, ���������� �� GetSelectionData</param>
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
		/// �������������� �������
		/// </summary>
		public void Refresh()
		{
			InvalidateArrange();
			InvalidateVisual();
		}

		/// <summary>
		/// ���������, �������� �� ������ ����������
		/// </summary>
		/// <param name="Row">����� ������</param>
		/// <param name="Col">����� �������</param>
		/// <returns></returns>
		public bool IsCellSelected(int Row, int Col)
		{
			return SelectedCells.Inside(Row, Col);
		}

		/// <summary>
		/// ��������� ������������ ������ � ������� ������� �������
		/// </summary>
		/// <param name="Row">����� ������</param>
		/// <param name="Col">����� �������</param>
		/// <param name="r">������������� ������</param>
		/// <returns>������� ������������ ������ � ������� �������</returns>
		public bool CellRect(int Row, int Col, out Rect r)
		{
			r = new Rect(); // ������ �������������

			if (Row == -1) // ��������� � ���������
			{
				r.Y = 0;
				r.Height = RowHeight;
			}
			else if (Row >= ScrollTop && Row < ScrollTop + VisibleRowCount) // ��������� � ������� �������
			{
				r.Y = ((Row - ScrollTop) + 1) * RowHeight;
				r.Height = RowHeight;
			}
			else
				return false; // ��� ������� �������

			if (Col == 0) // ��������� � ��������� �������
			{
				r.X = 0;
				r.Width = Columns[0].Width;
				return true;
			}
			else
			{
				for (int i = 1, n = VisibleColumns.Count; i < n; i++) // ����� �� ������� ��������
				{
					if (VisibleColumns[i].Col == Col)
					{
						r.X = VisibleColumns[i - 1].Right;
						r.Width = VisibleColumns[i].Right - r.X;
						return true;
					}
				}
			}

			return false; // ������ ��� ������� �������
		}

		/// <summary>
		/// ��������� ���������� ������ �� ����������� ����� ������������ �������
		/// </summary>
		/// <param name="p">���������� �����</param>
		/// <param name="Row">����� ������ (��� ��������� ������������ -1)</param>
		/// <param name="Col">����� �������</param>
		/// <returns>������� ��������� ���������� ������� ������</returns>
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
		/// ���������� ����� �������
		/// </summary>
		public int RowCount => DataSource != null ? DataSource.GetRowCount() : 0;

		/// <summary>
		/// ���������� �������� �������
		/// </summary>
		public int ColCount => DataSource != null ? DataSource.GetColCount() : 0;

		/// <summary>
		/// ��������� ��������� �������
		/// </summary>
		/// <param name="Col">����� �������</param>
		/// <returns>��������� �������</returns>
		public string ColumnName(int Col) => Col >= 0 && Col < Columns.Count ? Columns[Col].Caption : "";

		/// <summary>
		/// ������ ������������ ��� �������� � �� �������
		/// </summary>
		private Dictionary<string, int> ColumnIndices = null;

		/// <summary>
		/// ���������� ����� ������� �� ��� �����, � ������ ���������� ����� ������������ -1 
		/// </summary>
		/// <param name="Name">��� �������</param>
		/// <returns>������</returns>
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
		/// ��������� ������ �������
		/// </summary>
		/// <param name="Id">�������������, ������������ ��������� ������</param>
		public void AddRow(string Id)
		{
			if (DataSource != null)
			{
				BeginUpdate(); // ������ ������ � ������
				try
				{
					DataSource.AddRow(Id); // �������� ������
					SelectCell(RowCount - 1, 0, true); // ������� ����� ������
					if (Changes.IsUnlocked) // ������ � ������ �������
					{
						var Index = RowCount - 1;

						Changes.Log(() => // ����� ������
						{
							DataSource.DeleteRow(Index);
						}, () => // ����� ��������
						{
							DataSource.AddRow(Id);
						});
					}
				}
				finally
				{
					EndUpdate(); // ��������� ������ � ������ �������
				}
			}
		}

		/// <summary>
		/// �������� �����, � ������� ���� ���������� ������
		/// </summary>
		public void DeleteRows()
		{
			var Rows = SelectedCells.Rows; // ���������� ������
			var n = Rows.Length;
			if (n != 0)
			{
				BeginUpdate(); // ������ ������ � ������ �������
				try
				{
					var RecoveryData = new List<(int, string)>(); // ������ ��� �������������� �����
					for (--n; n >= 0; n--)
						if (DataSource.IsRowLocked(Rows[n]) == 0)
							RecoveryData.Add((Rows[n], DataSource.DeleteRow(Rows[n])));  // ������� � ���������

					if (Rows[0] >= RowCount) // ���������� ���������
						SelectCell(RowCount - 1, 0, true);
					else if (RowCount > 0)
						SelectCell(Rows[0], 0, true);
					else
						SelectedCells.Clear();

					if (Changes.IsUnlocked) // ������ � ������ �������
					{
						Changes.Log(() => // ����� ������
						{
							for (var i = RecoveryData.Count - 1; i >= 0; i--)
							{
								var (Row, Str) = RecoveryData[i];
								DataSource.RestoreRow(Row, Str);
							}

						}, () => // ����� ��������
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
		/// ��������� ������� ���������
		/// </summary>
		public bool HasSelection
		{
			get
			{
				if (Edit.Active) // ������� ��������
				{
					var (S, E) = Edit.Selection;
					return S != E;
				}
				else // ���������
				{
					var (A, B) = SelectedCells.Current;
					return A >= 0 && B >= 0;
				}
			}
		}

		/// <summary>
		/// ����� ���������
		/// </summary>
		public string SelectedText
		{
			get
			{
				if (Edit.Active)  // ������� ��������
				{
					return Edit.SelText; // ���� ����� ���
				}
				else
				{
					var Cells = SelectedCells.Cells; // ������ ���������� �����
					if (Cells.Count != 0) // �� ������
					{
						var Str = new StringBuilder(); // ���������
						var E = Cells.GetEnumerator(); // SortedSet �� ����� ��������� []
						E.MoveNext(); // ������� ������� ����������� ������ ������, ����������
						var (Row, Col) = E.Current; // ������ ���������� ������
						Str.Append(DataSource.GetEditCell(Row, Col)); // ��������� � ��������
						while (E.MoveNext()) // ���� ���� ��� ����������
						{
							var (R, C) = E.Current; // ������� ������ 
							Str.Append(R == Row ? "\t" : "\r\n"); // �� �� ������ � ��������� Tab, ����� ������ � CrLf
							Str.Append(DataSource.GetEditCell(R, C)); // �������� ������
							Row = R; // ���������� ����� ������
						}
						return Str.ToString();
					}
					else
						return "";
				}
			}
			set
			{
				if (Edit.Active) // ������� ��������
				{ 
					Edit.Insert(value, 0); // ���������� ����
				}
				else
				{
					var Cells = SelectedCells.Cells; // ���������� ������
					var n = Cells.Count;
					if (n == 1) // ������ ���� ������
					{
						BeginUpdate();
						try
						{
							int rc = RowCount, cc = ColCount; // ������� �������
							var E = Cells.GetEnumerator(); // ���������� ���������� �����
							E.MoveNext(); // ������ �������
							var (Row0, Col0) = E.Current; // ���������� �����  
							StringToMatrix(value, (int i, int j, string Str) => // ��������� ������ � ���������� �� �������� � �������
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
								new List<(int Row, List<int> Cols)>(); // c����� ����� � �������� ���������� ����� 
			
							int Prev = -1;
							List<int> Cols = null; // ������ �������� � ������
							foreach (var (Row, Col) in Cells) // ���������� ������
							{
								if (Prev != Row)
								{
									Temp.Add((Row, Cols = new List<int>())); // �������� ������ � ������� ������ �������
									Prev = Row;
								}
								Cols.Add(Col);
							}
							n = Temp.Count;

							StringToMatrix(value, (int i, int j, string Str) => // ��������� ������ � ����������� �� �������
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

		// ������� ������� BeginUpdate / EndUpdate;
		private int UpdateCount = 0;

		/// <summary>
		/// ������� ������� ������� �� �����
		/// </summary>
		public event Action<GridControl> DisplayRequest;

		/// <summary>
		/// ����� ������ ���������� ��������� ������
		/// </summary>
		public void BeginUpdate()
		{
			if (++UpdateCount == 1) // ��������� ��������� ������
			{
				Changes.BeginGroup(); // ����� ������ �������

				if (Changes.IsUnlocked) // ������ �������������
				{
					var Snap = SelectedCells.Snap(ScrollLeft, ScrollTop); // ��������� ��������� � ������ ���������
		
					Changes.Log(() => // ����� ������
					{
						DisplayRequest?.Invoke(this); // ������� ���� ��������, ���� ��� ������

						SelectedCells.Restore(Snap); // ������������ ���������
						ScrollLeft = Snap[0];
						ScrollTop = Snap[1]; // ������������ ������ ���������
						EndUpdate();
					}, () => // ����� ��������
					{
						DisplayRequest?.Invoke(this);

						BeginUpdate();
					});
				}
			}
		}

		/// <summary>
		/// ����� ��������� ���������� ��������� ������
		/// </summary>
		public void EndUpdate()
		{
			if (--UpdateCount == 0) // ������ ���������
			{
				if (Changes.IsUnlocked) // ������ �������������
				{
					if (ScrollCell.Row >= 0 && ScrollCell.Col >= 0) // ���� �������� ������
					{
						ScrollIntoView(ScrollCell.Row, ScrollCell.Col, true);
						ScrollCell = (-2, -2);
					}
					var Snap = SelectedCells.Snap(ScrollLeft, ScrollTop);

					Changes.Log(() => // ����� ������
					{
						DisplayRequest?.Invoke(this);

						BeginUpdate();
					}, () => // ����� ��������
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
		/// ������� ��������� �������� ������������ ������ ���������
		/// </summary>
		private void VScrollValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			InvalidateVisual();
		}

		/// <summary>
		/// ������� ��������� �������� �������������� ������ ���������
		/// </summary>
		private void HScrollValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			InvalidateVisual();
		}

		/// <summary>
		/// ���������� ���������� ������� ��������� �������� �������
		/// </summary>
		private int ColumnLock = 0;

		/// <summary>
		/// ������� ��������� ������� �������
		/// </summary>
		private void OnColumnChanged(Column c)
		{
			if (ColumnLock == 0)
				Refresh();
		}

		/// <summary>
		/// ������������ ������ ���������
		/// </summary>
		private readonly ScrollBar VScroll = new ScrollBar
		{
			Orientation = Orientation.Vertical,
		};

		/// <summary>
		/// �������������� ������ ���������
		/// </summary>
		private readonly ScrollBar HScroll = new ScrollBar
		{
			Orientation = Orientation.Horizontal,
		};

		/// <summary>
		/// �������� �����
		/// </summary>
		private readonly EditControl Edit;

		/// <summary>
		/// ������ ���������� �����������
		/// </summary>
		private readonly List<Visual> Children = new List<Visual>();

		// ���������� ���������� ���������� ��������� ����������
		protected override int VisualChildrenCount => Children.Count;

		// ���������� ���������� ���������� ������� ���������� �� ��� �������
		protected override Visual GetVisualChild(int index) => Children[index];

		// ��������� ����������� ���������� ���������� ��������� ����������
		protected override Size ArrangeOverride(Size finalSize)
		{
			// ���� ��� ��������� ������, ������ �� ������ ������������
			if (DataSource != null)
			{
				// ���� �� ���������� ��������� ������, ��� ������������ �� ��������� ������� ����� ����������
				if (FontName == "" || FontSize == 0)
				{
					// �������� ������� ����� ����������
					var MainForm = Application.Current.MainWindow;
					if (MainForm != null)
					{
						// �������� ��� ������
						FontName = (MainForm.GetValue(Control.FontFamilyProperty) as FontFamily).ToString();

						// �������� ������ ������
						FontSize = (int)(double)MainForm.GetValue(Control.FontSizeProperty);
					}
					else
						return finalSize;
				}

				// �������� ������� ������ � �������� ����������� ��� ����������� ��� �������
				FormattedText ft = new FormattedText("W", CultureInfo.CurrentUICulture,
					FlowDirection.LeftToRight, new Typeface(FontName), FontSize, Brushes.Black,
					null, TextSmoothing = (TextFormattingMode)(1 - Server.GetFontSmoothing()),
					VisualTreeHelper.GetDpi(this).PixelsPerDip);

				// ���������� ������ ������
				RowHeight = (int)Math.Round(ft.Height) + CellPadding * 2;

				// ���������� ��������� ������ ����� ���������
				double X = SystemParameters.VerticalScrollBarWidth,
					Y = SystemParameters.HorizontalScrollBarHeight;

	
				// ������������� ��������� ������������ ������ ���������
				var r = new Rect(finalSize.Width - X, 0, X, finalSize.Height - Y);
				VScroll.Arrange(r);

				// ������������� ��������� �������������� ������ ���������
				r = new Rect(0, finalSize.Height - Y, finalSize.Width - X, Y);
				HScroll.Arrange(r);

				// ���������� ���������� ������� ����� �������
				VisibleRowCount = (int)((finalSize.Height - Y) / RowHeight) - 1;

				// ������������� ��������� ������������ ������ ���������
				VScroll.Minimum = 0;
				var y = DataSource.GetRowCount() - VisibleRowCount;
				VScroll.ViewportSize = VisibleRowCount; // ������ �������
				if (y > 0) // ���������� ����� ������ ��� ���������� � ����
				{
					VScroll.IsEnabled = true; // ������ ��������� �������
					VScroll.Maximum = y; // ������������ ��������

					VScroll.SmallChange = 1; // ����������� �� ������� �� �������
					VScroll.LargeChange = VScroll.ViewportSize * 3 / 4; // ����������� �� ������� �� ������ ���������
				}
				else // ����� ������ ��������� ���������
				{
					VScroll.Value = 0;
					VScroll.Maximum = 0;
					VScroll.IsEnabled = false;
				}

				// ��������� ������ ���� �������� �������, �� ����������� ��������
				double ColumnWidth = 0;
				for (int i = 1, n = Columns.Count; i < n; i++)
					ColumnWidth += Columns[i].Width;

				// ���������� ��������� �������������� ������ ���������
				HScroll.Minimum = 0;
				var x = ColumnWidth + Columns[0].Width - (finalSize.Width - X);
				HScroll.ViewportSize = finalSize.Width - X;
				if (x > 0)  // ������ �������� ������ ������� ������� �������
				{
					HScroll.IsEnabled = true;
					HScroll.Maximum = x;

					HScroll.LargeChange = Math.Round(ColumnWidth / (Columns.Count - 1));
					HScroll.SmallChange = 1;
				}
				else // ������
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
		/// �������� ��������� ������������ ����� ��� ���������
		/// </summary>
		private const double Eps = 0.01;

		/// <summary>
		/// ������ ������� �������� �������.
		/// Col � ����� �������
		/// Right � �������������� ��������� ������ ������� �������
		/// </summary>
		private readonly List<(int Col, double Right)> VisibleColumns = new List<(int Col, double Right)>();

		/// <summary>
		/// ������ ����������� ��������� ��� �����
		/// </summary>
		private readonly SortedList<(int Row, int Col), string> CellHints = new SortedList<(int Row, int Col), string>();

		// �������� ��������� �������
		protected override void OnRender(DrawingContext dc)
		{
			// ��������������� �������������
			Rect r = new Rect(0, 0, RenderSize.Width, RenderSize.Height);

			// �������������� �����
			Point p = new Point(0, 0);

			// ������������� ������� ������� ��������� �����������.
			// ����� ��� ������� �� ������� �������� ����������. 
			dc.PushClip(new RectangleGeometry(r));

			// ������� ��� ������� ������� �������
			dc.DrawRectangle(CellBackground,
				null, r);

			// ������ ��� ��������� �������
			r.Height = RowHeight;
			dc.DrawRectangle(CaptionBackground, null, r);

			// ��������� ������� ������� ������� ���� �� �������� ���������
			r.X = RenderSize.Width - (r.Width = VScroll.Width);
			r.Y = RenderSize.Height - (r.Height = HScroll.Height);
			dc.DrawRectangle(VScroll.Background, null, r);

			CellHints.Clear();  // ������� ������� ���������

			// ������ ��������� �������� 
			int nc = Columns.Count;
			if (nc != 0)
			{
				// ������� ������
				VisibleColumns.Clear();

				// ��������� �������� ���������������� ������
				var ppd = VisualTreeHelper.GetDpi(this).PixelsPerDip;
				var Cult = CultureInfo.CurrentUICulture;
				var FlowDir = FlowDirection.LeftToRight;
				var Type = new Typeface(FontName);
				var Size = FontSize;

				// ��������� ������� ������� �������
				FormattedText s = new FormattedText(Columns[0].Caption, Cult, FlowDir, Type, Size,
					CaptionForeground, null, TextSmoothing, ppd);

				r.X = 0; // ����� ������� ������
				r.Y = 0; // ������� ������� ������
				r.Width = Columns[0].Width; // ������ ������
				r.Height = RowHeight; // ������ ������

				// ���������� ������ ���������, ����� �� ������������ ���������� ������
				p.X = r.X + (r.Width - s.Width) / 2;
				p.Y = r.Y + (r.Height - s.Height) / 2;

				dc.PushClip(new RectangleGeometry(r)); // ������� ��������� ����������� (�� ������ ���� ��������� ���� ������)
				dc.DrawText(s, p); // ������ ���������
				dc.Pop(); // ��������������� ������� ���������
				VisibleColumns.Add((0, r.Right)); // ��������� ������ � ������ ����������

				// ����������� ��������� �������� � �� ���������
				double
					x0 = -Math.Round(HScroll.Value), // ������� ����� ������� 
					x1 = x0, // ������� ������ �������
					w0 = Columns[0].Width, // ������ ������� �������
					ViewportWidth = HScroll.ViewportSize - w0 + Eps, // ������ ������� ����������
					xmax = RenderSize.Width - VScroll.Width; // ������������ ������� ����������

				// ���������� �������, ���� ��� ����� ������ ������� ������� ����������
				for (int i = 1; i < nc && x0 < ViewportWidth; i++, x0 = x1)
				{
					double w = Columns[i].Width; // ������ �������
					s = new FormattedText(Columns[i].Caption, Cult, FlowDir, Type, Size,
						CaptionForeground, null, TextSmoothing, ppd); // ��������� �������

					x1 = x0 + w;
					if (x1 > -Eps) // ������ ������� ������� �������� � ������� �������
					{
						p.X = w0 + x1 - w / 2 - s.Width / 2; // ���������� ���������
						r.X = w0 + x1 - w; // ����� ������� �������
						r.Width = w; // ������ �������
						if (r.X < w0) // ������� �������� ������ ������ ��������
						{
							r.Width -= w0 - r.X; // ��������� ������� ������
							r.X = w0; // ����� �������
						}
						if (r.Right > xmax) // ������� �������� �������� �� ������ ������� ������� �������
							r.Width = xmax - r.X; // ��������� ������� ������
						if (r.Width < 1)
							continue;

						VisibleColumns.Add((i, r.Right)); // ��������� ������� � ������ �������
						dc.PushClip(new RectangleGeometry(r)); // ������� ��������� ��� ��������� ���������
						if (s.Width > r.Width - Eps)
							CellHints.Add((-1, i), s.Text); // ��������� � ���������, ���� ����� ���� �������
						dc.DrawText(s, p); // ������ ���������
						dc.Pop(); // ��������������� ������� ���������
					}
				}

				// ��������� ������� �����

				nc = VisibleColumns.Count; // ���������� ������� ��������
				r.Y = 0; // ������������ ���������� ������
				r.Height = RowHeight; // ������ ������
				int cp = CellPadding; // Padding ������

				// ���� �� ������� ������� 
				for (int i = 0, ni = VisibleRowCount, cr = ScrollTop, nr = DataSource.GetRowCount(); i < ni && cr < nr; i++, cr++)
				{
					r.Y += r.Height; // ������������ ��������
					r.X = 0; // �������������� ����������
					r.Width = 0; // ������ ������

					// ���� �� ������� ��������
					for (int j = 0; j < nc; j++)
					{
						int cc = VisibleColumns[j].Col; // ����� �������
						r.X += r.Width; // �������������� ��������
						r.Width = VisibleColumns[j].Right - r.X; // ������ ������

						if (Edit.IsCellEdited(cr, cc))
							continue;

						// �������� ��������, ������ � ��������� ������
						string Cell = DataSource.GetDisplayCell(cr, cc, out double State, out string Hint);

						// ���� ���� ���������, ���������� � � ������� ���������
						if (Hint != "")
							CellHints.Add((cr, cc), Hint);

						// ����������� ����� ���� � ������
						Brush b, f;

						bool sel = IsCellSelected(cr, cc); // ��� ������ ��������?
						if (State > ErrorThreshold) // ������ ���� ������ ������
						{
							b = ErrorBackground(State, sel);
							f = ErrorForeground(State, sel);
						}
						else if (sel) // ������ ����, �� ������ ��������
						{
							b = SelectionBackground;
							f = SelectionForeground;
						}
						else if (Columns[cc].ReadOnly) // ������ �� ��������, �� ������� ������ ��� ������
						{
							b = ReadOnlyBackground;
							f = ReadOnlyForeground;
						}
						else // ������� ������
						{
							b = CellBackground;
							f = CellForeground;
						}

						dc.DrawRectangle(b, null, r);  // ��������� ��� ������

						if (r.Width < cp + Eps)
							continue;
						r.Width -= cp;
						dc.PushClip(new RectangleGeometry(r)); // �c����������� ���� ��������� 
						r.Width += cp;

						if (Hint == "" && s.Width > r.Width - Eps)
							CellHints.Add((cr, cc), s.Text); // ���� ����� ���� ������ ��������� ���������

						// ����� ������
						s = new FormattedText(Cell, Cult, FlowDir, Type, Size,
							f, null, TextSmoothing, ppd);

						// ���������� ������
						p.X = j == 1 ? r.Right - Columns[cc].Width + cp : r.Left + cp;
						p.Y = r.Y + (r.Height - s.Height) / 2;
						dc.DrawText(s, p); // ������ �����

						dc.Pop(); // ��������������� ���� ���������


					}
				}


				// ��������� ������ �����
				Pen gp = new Pen(GridLine, 1); // ���� ��� ��������� �����
				p.Y = 1; // ������ �����
				Point p2 = new Point(0, RenderSize.Height - HScroll.Height - 1); // ����� �����

				// ������������ ����� ����� �� ������ �������� ��������
				for (int i = 0, n = VisibleColumns.Count; i < n; i++)
				{
					p.X = p2.X = VisibleColumns[i].Right;
					dc.DrawLine(gp, p, p2);
				}

				// �������������� ����� �����
				p.X = 1;
				p.Y = p2.Y = RowHeight;
				p2.X = RenderSize.Width - VScroll.Width - 1;
				for (double h = RenderSize.Height - HScroll.Height + Eps; p.Y < h; p.Y += RowHeight)
				{
					p2.Y = p.Y;
					dc.DrawLine(gp, p, p2);
				}

			}

			// ��������������� ������� ���������
			dc.Pop();

			if (Edit.Active)
				InvalidateArrange();
		}

		/// <summary>
		/// ����� ���������
		/// </summary>
		private class Selection
		{
			/// <summary>
			/// ������� ���������
			/// </summary>
			private class Spot
			{
				/// <summary>
				/// ��������� ������
				/// </summary>
				public int StartRow { get; private set; }

				/// <summary>
				/// ��������� �������
				/// </summary>
				public int StartCol { get; private set; }

				/// <summary>
				/// �������� ������
				/// </summary>
				public int EndRow { get; private set; }

				/// <summary>
				/// �������� �������
				/// </summary>
				public int EndCol { get; private set; }

				/// <summary>
				/// �������� ��������� �������
				/// </summary>
				/// <param name="Row">��������� ������</param>
				/// <param name="Col">��������� �������</param>
				public Spot(int Row, int Col)
				{
					StartRow = EndRow = Row;
					StartCol = EndCol = Col;
				}

				/// <summary>
				/// ������������� �������� ����� ������� ���������
				/// </summary>
				/// <param name="Row">����� ������</param>
				/// <param name="Col">����� �������</param>
				/// <returns>������� ����������� ����� ��������� ������</returns>
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
				/// ��������� ���������� ����� � ������� ���������
				/// </summary>
				/// <param name="Row">����� ������</param>
				/// <param name="Col">����� �������</param>
				/// <returns>������� ���������� ����� � ���������� �������</returns>
				public bool Inside(int Row, int Col)
				{
					return InsideRange(Row, StartRow, EndRow) && InsideRange(Col, StartCol, EndCol);
				}

				/// <summary>
				/// ���������, ��� ������� ������� �� ����� ������
				/// </summary>
				public bool SingleCell => StartRow == EndRow && StartCol == EndCol;
			}

			/// <summary>
			/// ������ ���������� ��������
			/// </summary>
			private readonly List<Spot> Spots = new List<Spot>();

			/// <summary>
			/// �������� ��������� �������
			/// </summary>
			/// <param name="Row">��������� ������</param>
			/// <param name="Col">��������� �������</param>
			/// <param name="Clear">������� ������� ��������� ����� ������� ����� �������</param>
			public void Start(int Row, int Col, bool Clear)
			{
				if (Clear)
					Spots.Clear();

				Spots.Add(new Spot(Row, Col));
			}

			/// <summary>
			/// ������������� ���������� �������� ����� ��������� ������� ���������
			/// </summary>
			/// <param name="Row">����� ������</param>
			/// <param name="Col">����� �������</param>
			/// <returns>������� ����������� ����� ��������� ������</returns>
			public bool Move(int Row, int Col)
			{
				int n = Spots.Count;
				if (n != 0)
					return Spots[n - 1].Move(Row, Col);
				else
					return false;
			}

			/// <summary>
			/// ������� ���������
			/// </summary>
			public void Clear()
			{
				Spots.Clear();
			}

			/// <summary>
			/// ��������� ���������� ������ � ���������� �������
			/// </summary>
			/// <param name="Row">����� ������</param>
			/// <param name="Col">����� �������</param>
			/// <returns>������� ���������� ������ � ���������� �������</returns>
			public bool Inside(int Row, int Col)
			{
				foreach (Spot s in Spots)
					if (s.Inside(Row, Col))
						return true;
				return false;
			}

			/// <summary>
			/// ���������� ������ ���������� ������
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
			/// ���������� ��������� ���������� ������
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
			/// ���������� ������� ������ (������ ������ ��������� ������� ���������)
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
			/// ���������, ��� ��������� ������� �� ������������ ������
			/// </summary>
			public bool SingleCell => Spots.Count == 1 && Spots[0].SingleCell;

			/// <summary>
			/// ���������� ������ �����, � ������� ���� ���������� ������
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
			/// ������ ��������� ���� ���������� �����
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
			/// ��������� ������ ��������� � ������� ����� �����
			/// </summary>
			/// <param name="H">��������� �������������� ������ ���������</param>
			/// <param name="V">��������� ������������ ������ ���������</param>
			/// <returns>������ ����� ����� � ������ ���������</returns>
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
			/// ��������������� ������ ��������� �� ������� ����� �����, ����������� �� ������ Snap
			/// </summary>
			/// <param name="Snap">������ ����� �����</param>
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
		/// ������ ���������� �����
		/// </summary>
		private readonly Selection SelectedCells = new Selection();

		/// <summary>
		/// ������� �������������� ���������� �����
		/// </summary>
		public Action<SortedSet<(int Row, int Col)>> Populate { get; set; }

		/// <summary>
		/// ������������� ������������� �������� ���������� �����
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
		/// ������� ������� ����� ������ ����
		/// </summary>
		private bool IsMouseLeftButtonDown = false;

		// ���������� ������� �� ������� ������ ����
		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			// ���������� �������
			if (!IsFocused)
				Focus();

			// ����������� ��� ������ ����� ������ ���� � ��� �� ���� ������ �����
			if (e.LeftButton == MouseButtonState.Pressed && IsMouseLeftButtonDown == false && CoordToCell(e.GetPosition(this), out int Row, out int Col) && Row >= 0)
			{
				if (e.ClickCount == 2 && !Columns[Col].ReadOnly && DataSource.IsRowLocked(Row) == 0)
				{
					Edit.Show(Row, Col, DataSource.GetEditCell(Row, Col));
				}
				var km = Keyboard.Modifiers; // ��������� ������-��������������
				switch (km)
				{
					case 0: // ������ �� ������
					case ModifierKeys.Control: // ����� Ctrl
						SelectedCells.Start(Row, Col, km == 0); // �������� ����� ������� ��������� (��� ���������� Ctrl ��� � ������� ������ �������)
						IsMouseLeftButtonDown = true; // ���������� ��������� ����� ������ ����
						ScrollIntoView(-1, Col);
						Mouse.Capture(this); // ������ ����, ����� ��������� ��������� �� ���� �� ��������� ������� (��� ���������)
						e.Handled = true; // ������� ����������
						break;
					case ModifierKeys.Shift: // ����� Shift
						var (SR, SC) = SelectedCells.Current;
						if (SR >= 0 && SC >= 0)
							SetSelectionEnd(Row, Col);
						e.Handled = true;
						break;
				}

			}
		}

		/// <summary>
		/// �������� ���� ������ �������
		/// </summary>
		/// <param name="Row">����� ������</param>
		/// <param name="Col">����� �������</param>
		/// <param name="Short">������� ����, ��� ������ ������������ ������ ��� �����, � �� � ����� ������ �������</param>
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
		/// ������, � ������� ��������� ������� ������ � ������ EndUpdate
		/// </summary>
		private (int Row, int Col) ScrollCell = (-2, -2);

		/// <summary>
		/// ���������� ��������� ������ �� ������
		/// </summary>
		/// <param name="Row">����� ������</param>
		/// <param name="Col">����� �������</param>
		/// <param name="Short">������� ����, ��� ������ ������������ ������ ��� �����, � �� � ����� ������ �������</param>
		public void ScrollIntoView(int Row, int Col, bool Short = false)
		{
			// ����� ������ ��������������� � ��������� ��� ������� �������
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

			// ��� ��������������� ������ �������
			if (Col > 0)
			{
				// ��������� ����� � ������ ������� �������
				int Left = 0, Right = 0;
				for (int i = 1; i <= Col; i++)
				{
					Left = Right;
					Right += Columns[i].Width;
				}

				if (Left < ScrollLeft) // ������ ����� �������
					HScroll.Value = Left;
				else
				{
					var xmax = ActualWidth - VScroll.Width - Columns[0].Width; // ������ ������� �������
					if (Right - HScroll.Value > xmax) // ������ ������ �������
						HScroll.Value = Right - xmax;
				}

			}
			InvalidateVisual();
		}

		/// <summary>
		/// ������������� �������� ������ ���������
		/// </summary>
		/// <param name="Row">����� ������</param>
		/// <param name="Col">����� �������</param>
		private void SetSelectionEnd(int Row, int Col)
		{
			if (SelectedCells.Move(Row, Col)) // ������������� ����� ���������
			{
				if (Row < ScrollTop) // ������ ���� ������� ���������
					ScrollTop = Row;
				else if (Row >= ScrollTop + VisibleRowCount) // ����� ���� ������� ���������
					ScrollTop = Row - VisibleRowCount + 1;

				if (Col == 0 && SelectedCells.First.Col > 0) // ��������� ������ ������ � ������ �������
					ScrollLeft = 0;
				else
					ScrollIntoView(-1, Col); // ���������� ������ �������

				InvalidateVisual();
			}
		}

		// ������� ���������� ������ ����
		protected override void OnMouseUp(MouseButtonEventArgs e)
		{
			// ���� ����� ������ ���� ������ � ������ ��������
			if (IsMouseLeftButtonDown && e.LeftButton == MouseButtonState.Released)
			{
				Mouse.Capture(null); // ������������� ���������� ����
				SelectionTimer.Enabled = false;
				IsMouseLeftButtonDown = false;
			}

		}

		/// <summary>
		/// ������, ���������� ����� ��� ������� ����� ������ ����
		/// ������ ��������� ��� ������� ���������. ����� ��� ����,
		/// ����� ����������� �������������� ���������
		/// </summary>
		private System.Timers.Timer SelectionTimer = new System.Timers.Timer()
		{
			Enabled = false,
			AutoReset = true,
		};

		/// <summary>
		/// ������ ����������� ���������
		/// </summary>
		private System.Timers.Timer HintTimer = new System.Timers.Timer()
		{
			Enabled = false,
			AutoReset = false
		};

		// ������� ������� ���������
		private void SelectionTimerElapsed(object sender, ElapsedEventArgs e)
		{
			SyncCtx.Post(_ =>
			{
				var (Row, Col) = SelectedCells.Last; // ��������� ���������� ������
				var P = Mouse.GetPosition(this); // ������� ����
				if (P.Y < RowHeight && Row > 0) // ���� �������
					SetSelectionEnd(Row - 1, Col);
				else if (P.Y > ActualHeight - HScroll.Height && Row < RowCount - 1) // ���� �������
					SetSelectionEnd(Row + 1, Col);
				else if (P.X > ActualWidth - VScroll.Width && Col < ColCount - 1) // ������ �������
					SetSelectionEnd(Row, Col + 1);
			}, this);
		}

		/// <summary>
		/// �����, � ������� ����� ������ ����
		/// </summary>
		private Point HoverPoint;

		/// <summary>
		/// ������, ��� ������� ����� ������ ����
		/// </summary>
		private (int Row, int Col) HoverCell;

		/// <summary>
		/// ����������� ���������
		/// </summary>
		private readonly ToolTip Hint;

		// ������� ������� ���������
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
		/// �������, ������ �������� ���������� ��� ������ ���� � ��������� ������� ����
		/// </summary>
		private (int Index, int Pos) ResizedColumn = (-1, -1);

		// ������� �������� ����
		protected override void OnMouseMove(MouseEventArgs e)
		{
			var Pos = e.GetPosition(this); // ������� ������� ����


			if (IsMouseLeftButtonDown) // ���� ������ ����� ������ ����
			{
				// ���������, ��� ������ ��������� �� ��������� �������
				if (Pos.X > ActualWidth - VScroll.Width ||
					Pos.Y < RowHeight || Pos.Y > ActualHeight - HScroll.Height)
				{
					// ������ ������, ��������� �������� �������
					if (Pos.Y < -RowHeight / 2 || Pos.Y > ActualHeight + HScroll.ActualHeight / 2 ||
						Pos.X > ActualWidth + VScroll.Width / 2)
						SelectionTimer.Interval = 30;
					else
						SelectionTimer.Interval = 100;

					// �������� ������
					if (!SelectionTimer.Enabled)
						SelectionTimer.Enabled = true;
				}
				else
				{
					// ��������� ������
					SelectionTimer.Enabled = false;

					if (CoordToCell(Pos, out int Row, out int Col) && Row >= 0) // �������� ��� �������
					{
						SetSelectionEnd(Row, Col);
						e.Handled = true; // ������� ����������
					}
				}
			}
			else
			{
				if (CoordToCell(Pos, out int Row, out int Col)) // �������� ������ ������
				{
					if (Row != HoverCell.Row || Col != HoverCell.Col)
					{
						HideHint(); // ������ ���� �� ������, ��� ������� �������� ���������
					}

					if (CellHints.ContainsKey((Row, Col)) && !(Hint.IsOpen && Row == HoverCell.Row && Col == HoverCell.Col)) // ������� ��������� ��� ������
					{
						Hint.IsOpen = false; // �������� ���������
						HintTimer.Enabled = false; // ������������� ������, ���� �� �������
						HintTimer.Interval = HintDelay; // ������������� ��������
						HoverPoint = Pos; // ���������� ��������� ����
						HintTimer.Enabled = true; // ��������� ������
					}

					if (Row == -1 && !Edit.Active) // ��������� � ���������, �������������� �� ����������
					{
						// �� ������ ������� � ���������� �������
						if (ResizedColumn.Index < 0)
						{
							// ���������� ������� �������
							for (int i = 0, nc = VisibleColumns.Count - 1; i < nc; i++)
							{
								// ������ ������ � ������� ��������
								if (Math.Abs(Pos.X - VisibleColumns[i].Right) < 4)
								{
									Cursor = Cursors.SizeWE; // ������ ���������
									ResizedColumn = (i, Columns[VisibleColumns[i].Col].Width - (int)Pos.X); // ���������� �������
									e.Handled = true; // ������� ����������
									return;
								}
							}
						}
						else // ������� ������
						{
							if (e.LeftButton == MouseButtonState.Pressed) // ������ ����� ������ ����
							{
								var w = (int)Pos.X + ResizedColumn.Pos; // ��������� ����� ������ �������
								w = Math.Max(32, Math.Min(512, w)); // ������ �����������
								Columns[VisibleColumns[ResizedColumn.Index].Col].Width = w; // ������������� ������
								e.Handled = true; // ������� ����������
							}
							else if (Math.Abs(Pos.X - VisibleColumns[ResizedColumn.Index].Right) > 4) // ������ ���� �� �������
							{
								Cursor = null; // ������ �� ���������
								ResizedColumn = (-1, -1); // ���������� ���������� � ������� �������
								e.Handled = true; // ������� ����������
							}
						}
					}
					else if (ResizedColumn.Index >= 0) // ������ ���� �� ���������
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

		// ������� ��������� ������ ����
		protected override void OnMouseWheel(MouseWheelEventArgs e)
		{
			var Pos = e.GetPosition(this); // ������� �������
			if (Pos.Y > ActualHeight - HScroll.Height) // ��� �������������� ������� ���������
				HScroll.Value -= Math.Sign(e.Delta) * HScroll.SmallChange * 10;
			else // � ������ �����
				VScroll.Value -= Math.Sign(e.Delta) * VScroll.SmallChange * 2;

		}

		/// <summary>
		/// �������� ��������� ��� ������
		/// </summary>
		private void HideHint()
		{
			HintTimer.Enabled = false;
			Hint.IsOpen = false;
			HoverCell = (-2, -2);
		}

		// ������� ����� ����
		protected override void OnMouseLeave(MouseEventArgs e)
		{
			HideHint();
		}

		// ������� ������� �������
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (ColCount != 0 && RowCount != 0 && KeyActions.TryGetValue(e.Key, out Func<GridControl, ModifierKeys, bool> A))
				e.Handled = A(this, e.KeyboardDevice.Modifiers);
		}

		// ������ �� ������� ������� Tab
		private static bool MoveTab(GridControl Grid, ModifierKeys Shift)
		{
			switch (Shift)
			{
				case ModifierKeys.None: // ������ Tab
					Grid.Edit.Hide(); // ��������� ��������������
					return MoveRight(Grid, 0); // ���������� ������
				case ModifierKeys.Shift: // Shift + Tab
					Grid.Edit.Hide(); // ��������� ��������������
					return MoveLeft(Grid, 0); // ���������� �����
			}
			return false;
		}

		// ������ �� ������� ������� Enter
		private static bool MoveEnter(GridControl Grid, ModifierKeys Shift)
		{
			switch (Shift)
			{
				case ModifierKeys.None: // ������ Enter
					Grid.Edit.Hide(); // ��������� ��������������
					return MoveDown(Grid, 0); // ���������� ����
				case ModifierKeys.Shift: // Shift + Enter
					Grid.Edit.Hide(); // ��������� ��������������
					return MoveUp(Grid, 0); // ���������� �����
				case ModifierKeys.Control: // Ctrl + Enter
					if (!Grid.Edit.Active) // �������� �� �������
						Grid.PopulateSelection(); // ���������� ���������
					return true;
			}
			return false;
		}

		/// <summary>
		/// ������ �� ������� ������ ���������
		/// </summary>
		/// <param name="Row">���������� ������</param>
		/// <param name="Col">���������� �������</param>
		/// <param name="Shift">��������� ������-��������������</param>
		/// <returns>������� ������������� �������</returns>
		private bool Arrow(int Row, int Col, ModifierKeys Shift)
		{
			if ((Shift & ~(ModifierKeys.Control | ModifierKeys.Shift)) != 0)
				return false; // ���� ������ �������-������������� ����� Ctrl ��� Shift

			var (R, C) = SelectedCells.Last; // ��������� ���������� ������
			 
			if (R >= 0 || C >= 0) // ���� ��� ����
			{
				if ((Shift & ModifierKeys.Control) != 0) // ����� Ctrl
				{
					if (Row < 0)     // �������c� �� ����� �����
						R = 0;
					else if (Row > 0)  // ����   
						R = RowCount - 1;
					if (Col < 0)   // �����
					{
						C = 0;
						ScrollLeft = 0;
					}
					else if (Col > 0) // ������
						C = ColCount - 1;

				}
				else // ������ �� ������
				{
					R += Row; // ������ ���������
					C += Col;
				}

				if ((Shift & ModifierKeys.Shift) == 0) // Shift �� �����
					SelectCell(R, C, true); // �������� ���� ������
				else // ���� �����
					SetSelectionEnd(R, C); // ���������� ��������� 
			}

			return true;
		}

		// ������ �� ������� ������� �����
		private static bool MoveLeft(GridControl Grid, ModifierKeys Shift)
		{
			return Grid.Arrow(0, -1, Shift);
		}

		// ������ �� ������� ������� ������
		private static bool MoveRight(GridControl Grid, ModifierKeys Shift)
		{
			return Grid.Arrow(0, 1, Shift);
		}

		// ������ �� ������� ������� �����
		private static bool MoveUp(GridControl Grid, ModifierKeys Shift)
		{
			return Grid.Arrow(-1, 0, Shift);
		}

		// ������ �� ������� ������� ����
		private static bool MoveDown(GridControl Grid, ModifierKeys Shift)
		{
			return Grid.Arrow(1, 0, Shift);
		}

		// ������ �� ������� PageUp
		private static bool MovePageUp(GridControl Grid, ModifierKeys Shift)
		{
			if ((Shift & ~ModifierKeys.Shift) == 0) // ����� ���� ����� ������ Shift
				return Grid.Arrow(-(int)Grid.VScroll.LargeChange, 0, Shift);
			else
				return false;
		}

		// ������ �� ������� PageDown
		private static bool MovePageDown(GridControl Grid, ModifierKeys Shift)
		{
			if ((Shift & ~ModifierKeys.Shift) == 0) // ����� ���� ����� ������ Shift
				return Grid.Arrow((int)Grid.VScroll.LargeChange, 0, Shift);
			else
				return false;
		}

		// ������ �� ������� Home
		private static bool MoveHome(GridControl Grid, ModifierKeys Shift)
		{
			switch (Shift)
			{
				case ModifierKeys.None: // �������� �� ���������� �� Ctrl + Left
				case ModifierKeys.Shift:
					return Grid.Arrow(0, -1, Shift | ModifierKeys.Control);
				case ModifierKeys.Control: // ������� � ����� ������� ����� ����
					Grid.SelectCell(0, 0, true);
					return true;
				case ModifierKeys.Shift | ModifierKeys.Control: // ������� � �������� ������
					var (Row, Col) = Grid.SelectedCells.Last;
					if (Row >= 0 && Col >= 0)
						Grid.SetSelectionEnd(0, 0);
					return true;
				default:
					return false;
			}
		}

		// ������ �� ������� End
		private static bool MoveEnd(GridControl Grid, ModifierKeys Shift)
		{
			switch (Shift)
			{
				case ModifierKeys.None: // �������� �� ���������� �� Ctrl + Right
				case ModifierKeys.Shift:
					return Grid.Arrow(0, 1, Shift | ModifierKeys.Control);
				case ModifierKeys.Control: // ������� � ����� ������ ������ ����
					Grid.SelectCell(Grid.RowCount - 1, Grid.ColCount - 1, true);
					return true;
				case ModifierKeys.Shift | ModifierKeys.Control: // ������� � ��������
					var (Row, Col) = Grid.SelectedCells.Last;
					if (Row >= 0 && Col >= 0)
						Grid.SetSelectionEnd(Grid.RowCount - 1, Grid.ColCount - 1);
					return true;
				default:
					return false;
			}
		}

		// ������ �� ������� Esc
		private static bool MoveEscape(GridControl Grid, ModifierKeys Shift)
		{
			if (Shift == 0) // ������ �� ������
			{
				Grid.HideHint(); // �������� ���������
				return true;
			}
			return false;
		}

		/// <summary>
		/// ������ ������� ������� �� ������� ������
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


		// ������� ����� ������. ���� ������������ ��� ����� �� �������� ���������
		protected override void OnTextInput(TextCompositionEventArgs e)
		{
			var (Row, Col) = SelectedCells.Current; // ������� ������
			if (Row >= 0 && Col >= 0 && !Columns[Col].ReadOnly && DataSource.IsRowLocked(Row) == 0)
			{
				if (!SelectedCells.SingleCell) // ���������� ���������
					SelectCell(Row, Col);
				else
					ScrollIntoView(Row, Col); // ������������� �� ������� ������

				// �������� ��������������. ���� ������������ ����� ������, ����� ������ �� ���������
				Edit.Show(Row, Col, e.Text == " " ? DataSource.GetEditCell(Row, Col) : e.Text);
			}
			e.Handled = true;
		}

		/// <summary>
		/// ���������� ����� ������
		/// </summary>
		/// <param name="Row">����� ������</param>
		/// <param name="Col">����� �������</param>
		/// <returns>����� ������</returns>
		public string GetCell(int Row, int Col)
		{
			return DataSource.GetEditCell(Row, Col);
		}

		/// <summary>
		/// ������������� ����� ������
		/// </summary>
		/// <param name="Row">����� ������</param>
		/// <param name="Col">����� �������</param>
		/// <param name="Text">����� ������</param>
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
		/// ������� ������ �������� ���������� ������������ ��������
		/// </summary>
		public event Action<Object, long> StartWaiting;

		/// <summary>
		/// ������� ���������� �������� ������������ ��������.
		/// �������� ��� � ��������� �� ������
		/// </summary>
		public event Action<Object, long, int, string> EndWaiting;

		/// <summary>
		/// �������� ����������� ����� ���������� ��� ����������
		/// </summary>
		/// <param name="Str">��������� ��� ���������� ��� ����������</param>
		/// <param name="Method">����� ���������� ��� ����������</param>
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
		/// ���������� ���������� ����� �������. � ������ ������ ���������� ������� StartWaiting
		/// ������� ������ ���� ������������� �� ��������� ������� EndWaiting
		/// </summary>
		/// <param name="Order">������� ����������</param>
		public void Sort(string Order)
		{
			AsyncProcess(Order, DataSource.Sort);
		}

		/// <summary>
		/// ���������� ���������� ����� �������. � ������ ������ ���������� ������� StartWaiting
		/// ������� ������ ���� ������������� �� ��������� ������� EndWaiting
		/// </summary>
		/// <param name="Exp">��������� �������</param>
		public void Filter(string Exp)
		{
			AsyncProcess(Exp, DataSource.Filter);
		}

		// ����������� ������� ��������� ������ �������
		void IDisposable.Dispose()
		{
			Marshal.ReleaseComObject(DataSource);
		}


		/// <summary>
		/// �������� ������ � ������� 
		/// </summary>
		private class EditControl : FrameworkElement
		{
			/// <summary>
			/// ���������� ��������� ��������
			/// </summary>
			public static readonly DependencyProperty OffsetProperty;

			/// <summary>
			/// ������ ���������
			/// </summary>
			public static readonly DependencyProperty SelectionStartProperty;

			/// <summary>
			/// ����� ���������
			/// </summary>
			public static readonly DependencyProperty SelectionEndProperty;

			/// <summary>
			/// �������� ������� �����������
			/// </summary>
			/// <typeparam name="T">��� ��������</typeparam>
			/// <param name="Name">��� ��������</param>
			/// <param name="Default">�������� �� ���������</param>
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
			/// �������� ������� ������������
			/// </summary>
			static EditControl()
			{
				OffsetProperty = PropReg("Offset", 0);
				SelectionStartProperty = PropReg("SelectionStart", 0);
				SelectionEndProperty = PropReg("SelectionEnd", 0);
			}

			/// <summary>
			/// �������
			/// </summary>
			private readonly GridControl Master;

			/// <summary>
			/// ������� (��������� �������)
			/// </summary>
			private readonly CaretControl Caret;

			/// <summary>
			/// ������ �������� ������
			/// </summary>
			/// <param name="master">�������</param>
			public EditControl(GridControl master)
			{
				Master = master;  // ��������� �������

				// ����� ���������� ������ �����
				SnapsToDevicePixels = true;
				SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);

				Focusable = true; // �������� ����� �������� �����
				FocusVisualStyle = null; // �� �� ����� ��� ����������

				Caret = new CaretControl(this); // ������ �������

				Master.AddLogicalChild(this); // ������������� � �������
				Master.AddVisualChild(this);

				Visibility = Visibility.Hidden; // �������� ���������� �����
				Active = false;
				Cursor = Cursors.IBeam;
			}

			/// <summary>
			/// ����� ������ ������������� ������
			/// </summary>
			public int Row { get; private set; }

			/// <summary>
			/// ����� ������� ������������� ������
			/// </summary>
			public int Col { get; private set; }

			/// <summary>
			/// ������� ���������� ���������
			/// </summary>
			public bool Active { get; private set; }

			/// <summary>
			/// �������� �������������� ������
			/// </summary>
			/// <param name="row">����� ������</param>
			/// <param name="col">����� �������</param>
			/// <param name="text">������������� �����</param>
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
			/// ���������� �������������� ������
			/// </summary>
			/// <param name="Esc">������� ������ ��� ����������� ������</param>
			public void Hide(bool Esc = false)
			{
				if (Active)
				{
					Escaped = Esc;
					Master.Focus();
				}
			}

			/// <summary>
			/// ���������, ��� ��������� ������ �������������
			/// </summary>
			/// <param name="row">����� ������</param>
			/// <param name="col">����� �������</param>
			/// <returns>������� ������������� ������</returns>
			public bool IsCellEdited(int row, int col) => Active && Row == row && Col == col;


			/// <summary>
			/// ������ �������� �������������� ������
			/// </summary>
			private readonly List<FormattedText> Characters = new List<FormattedText>();

			/// <summary>
			/// ���������� ��������� ��������
			/// </summary>
			public int Offset
			{
				get => (int)GetValue(OffsetProperty);
				set => SetValue(OffsetProperty, Math.Max(0, Math.Min(value, Characters.Count - 4)));
			}

			/// <summary>
			/// ������ ���������
			/// </summary>
			public int SelStart
			{
				get => (int)GetValue(SelectionStartProperty);
				set => SetValue(SelectionStartProperty, Math.Max(0, Math.Min(value, Characters.Count)));
			}

			/// <summary>
			/// ����� ���������. ��� �� ���������� ��������� �������
			/// </summary>
			public int SelEnd
			{
				get => (int)GetValue(SelectionEndProperty);
				set => SetValue(SelectionEndProperty, Math.Max(0, Math.Min(value, Characters.Count)));
			}

			/// <summary>
			/// ������ � ����� ��������� (���������������)
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
			/// ���������� �����
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
			/// ������������� �����
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

					// ��������� ����� �� ��������� �������
					// ������-��� ��� ������� ����������� ��������
					// ������ ���������� �������� �� ������� FormattedText
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
			/// ���������� ������� ������� �� ����� �� ������ ��� -1, ���� ����� ��� �������
			/// </summary>
			/// <param name="x">���������� �� ����� �� ����� ����� ��������</param>
			/// <returns>������� �������</returns>
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
			/// ���������� ��������� ������� �� ������ ��� -1 ��� �������� ��� ������� �������
			/// </summary>
			/// <param name="Index">����� �������</param>
			/// <returns>��������� ������� �� ������</returns>
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

			// ���������� ��������� �������
			protected override Size ArrangeOverride(Size finalSize)
			{
				if (SelEnd < Offset) // ������� �� ����� �����
					Offset = Math.Max(0, SelEnd - 4); // �������� �

				// ��������� ��������� ������� � ������ ��������
				double p = Master.CellPadding, x = p, w = finalSize.Width - 2 * p - 1;
				for (int i = Offset; i < SelEnd; i++)
					x += Characters[i].WidthIncludingTrailingWhitespace; // �����-�� ��� ���������� ������ ��� ��������

				// ������� �� ����� ������
				if (x > w)
				{
					int Delta = 0, i = SelEnd - 1;
					while (x > w) // ���� ������� �� �����, ����������� �������� 
					{
						Delta++;
						x -= Characters[i--].WidthIncludingTrailingWhitespace;
					}
					for (int j = 0; j < 4; j++) // � ��� �� ������ �������
						x -= Characters[i--].WidthIncludingTrailingWhitespace;

					Offset += Delta + 4;
					InvalidateVisual();
				}

				Caret.Arrange(new Rect(x, p, 1, finalSize.Height - 2 * p));
				return finalSize;
			}

			// ���������� �������� ����������
			protected override int VisualChildrenCount => 1;

			// ���� ������ �������
			protected override Visual GetVisualChild(int index) => Caret;

			// ��� �������� ��������� �� ���������� ���������� ������ ������ � ��������� ������
			private bool Escaped = false;

			// ������� ��������� ������
			protected override void OnGotFocus(RoutedEventArgs e)
			{
				Escaped = false;
				Caret.Active = true; // ���������� �������
				e.Handled = true;
			}

			// ������� ������ ������
			protected override void OnLostFocus(RoutedEventArgs e)
			{
				Active = false;
				Visibility = Visibility.Hidden;
				Caret.Active = false;  // ������� �������
				if (!Escaped) // ���� ��������� ��������
					Master.SetCell(Row, Col, Text); // ���������
				else
					Master.Refresh(); // ��� ������ ����������������
				e.Handled = true;
			}

			// ������� �����������
			protected override void OnRender(DrawingContext dc)
			{
				Rect 
					r = new Rect(Master.CellPadding, 0, RenderSize.Width - Master.CellPadding * 2, RenderSize.Height - 1), // �������� �������� ����������
					r1 = new Rect(r.Left, r.Left, 0, r.Height - 2 * r.Left); // ������� �������� �������

				dc.PushClip(new RectangleGeometry(r)); // ���� ���������
				dc.DrawRectangle(Master.CellBackground, null, r); // ������� ���

				int n = Characters.Count;
				if (n != 0)
				{
					var (ss, se) = Selection; // ������� ���������

					// �����
					Brush cb = Master.CellBackground,
						sb = Master.SelectionBackground,
						cf = Master.CellForeground,
						sf = Master.SelectionForeground;

					// ������ �������. 
					// ���� ��������, ��� ���� ����������� � �������� ������������� �������
					// �� ������ ��������������� ����� ��������. ����� ��� � �����������,
					// ������� ���������� �������������� �������-������������, ������ ������� ��������
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

			// ������� �������� ����
			protected override void OnMouseMove(MouseEventArgs e)
			{
				if (e.LeftButton == MouseButtonState.Pressed && Mouse.Captured == this) // ������ ����� ������ � ���� ���������
				{
					var x = e.GetPosition(this).X; // ��������������� �������
					if (x < Master.CellPadding) // ������ �����
						SelEnd = 0; // ��������� ����� �� �����
					else if (x > ActualWidth - Master.CellPadding) // ������ ������
						SelEnd = Characters.Count; // ������ �� �����
					else
						SelEnd = PointToIndex(e.GetPosition(this).X); // ������������� ������� ������
				}
			}

			// ������� ������� ������ ����
			protected override void OnMouseDown(MouseButtonEventArgs e)
			{
				if (e.LeftButton == MouseButtonState.Pressed) //����� ������
				{
					SelStart = SelEnd = PointToIndex(e.GetPosition(this).X); // ������������� ������� ������
					Mouse.Capture(this); // ������ ����. � ����������� WinAPI ����� ����, � ������� ������ ������ ����, �������� � ������ ��� ������ ����������
				}
				e.Handled = true;
			}

			// ������� ���������� ������ ����
			protected override void OnMouseUp(MouseButtonEventArgs e)
			{
				if (e.LeftButton == MouseButtonState.Released && Mouse.Captured == this)
					Mouse.Capture(null);
				e.Handled = true;
			}

			// ������� ������� �������
			protected override void OnKeyDown(KeyEventArgs e)
			{
				if (KeyboardActions.TryGetValue(e.Key, out Func<EditControl, ModifierKeys, bool> A))
				{
					e.Handled = A(this, e.KeyboardDevice.Modifiers);
				}
			}

			// ������� ����� ������
			protected override void OnTextInput(TextCompositionEventArgs e)
			{
				Insert(e.Text, 0);
				e.Handled = true;
			}

			/// <summary>
			/// ������ �� ������� ������� ������
			/// </summary>
			/// <param name="Edit">�������� ������</param>
			/// <param name="Shift">���������� ������-��������������</param>
			/// <returns>������� ��������� �������</returns>
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
			/// ������ �� ������� ������� �����
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
			/// ������ ������ �� ������� �������
			/// </summary>
			private static bool MoveNone(EditControl Edit, ModifierKeys Shift)
			{
				return Shift == 0;
			}

			/// <summary>
			/// ������ �� ������� ������� Home
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
			/// ������ �� ������� ������� End
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
			/// ������ �� ������� ������� Delete
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
			/// ������ �� ������� ������� Backspace
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
			/// ������ �� ������� ������� Escape
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
			/// ��������� ������� ������� ������� ������. �� ��������� �������� � �� ��������� ����� � ������ OnKeyDown
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
			/// ���������� ����� � ��������
			/// </summary>
			/// <param name="Str">����������� �����</param>
			/// <param name="Delete">0 � �������, +1 � �������� ������, -1 � �������� �����</param>
			public void Insert(string Str, int Delete)
			{
				if (Delete == 0) // ������� ������
				{
					var ppd = VisualTreeHelper.GetDpi(this).PixelsPerDip; // ��������� FormattedText (�� �� ����� �� �� ����������)
					var Cult = CultureInfo.CurrentUICulture;
					var FlowDir = FlowDirection.LeftToRight;
					var Type = new Typeface(Master.FontName);
					var Size = Master.FontSize;
					var Br = Master.CellForeground;

					var (S, E) = Selection; // ���������
					if (S != E) // ���� ���������
						Characters.RemoveRange(S, E - S); // ������ ���

					var f = new List<FormattedText>();
					foreach (var ch in Str) // ������ ��������� ������ �� ������ �����
						f.Add(new FormattedText(ch.ToString(), Cult, FlowDir, Type, Size, Br, null,
							Master.TextSmoothing, ppd));

					Characters.InsertRange(S, f); // ��������� � ����� ������
					SelStart = SelEnd = S + Str.Length; // ���������� �������
				}
				else if (SelStart != SelEnd) // ������� ���������
				{
					var (S, E) = Selection;
					Characters.RemoveRange(S, E - S);
					SelStart = SelEnd = S;
				}
				else if (Delete > 0) // �������� �������� �������
				{
					if (SelEnd < Characters.Count)
					{
						Characters.RemoveAt(SelEnd);
						InvalidateArrange();
						InvalidateVisual();
					}
				}
				else if (SelEnd != 0)// �������� ����������� �������
				{
					Characters.RemoveAt(SelEnd - 1);
					SelStart = --SelEnd;
				}
			}

			/// <summary>
			/// ����� �������
			/// </summary>
			private class CaretControl : FrameworkElement
			{
				/// <summary>
				/// �������� ������ 
				/// </summary>
				private readonly EditControl Master;

				/// <summary>
				/// ������ ��������
				/// </summary>
				private System.Timers.Timer Timer;

				/// <summary>
				/// ������ �������
				/// </summary>
				/// <param name="master">�������� ������</param>
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
				/// ������� ���������� �������
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

				// ������� �������
				private void TimerElapsed(object sender, ElapsedEventArgs e)
				{
					Master.Master.SyncCtx.Post(_ =>
					{
						// �������� ������� ��������� �� ���������������
						Visibility = (Visibility)(((int)Visibility + 1) & 1);
					}, this);
				}

				// ����� ���������
				protected override void OnRender(DrawingContext dc)
				{
					Pen p = new Pen(Master.Master.CellForeground, 1);
					dc.DrawLine(p, new Point(0, 0), new Point(0, RenderSize.Height));
				}
			}
		}
	}




}