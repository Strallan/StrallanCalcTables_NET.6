using System;
using System.Runtime.InteropServices;

namespace Strallan
{


	/// <summary>
	/// Стили подсветки
	/// </summary>
	public static class Highlight
	{
		/// <summary>
		/// Стиль простого текста
		/// </summary>
		public const int Space = 0;
		/// <summary>
		/// Стиль разделителей
		/// </summary>
		public const int Delimiter = 1;
		/// <summary>
		/// Стиль заголовков блоков
		/// </summary>
		public const int Block = 2;
		/// <summary>
		/// Стиль стандартных переменных
		/// </summary>
		public const int Constant = 3;
		/// <summary>
		/// Стиль команд
		/// </summary>
		public const int Directive = 4;
		/// <summary>
		/// Стиль имён функций
		/// </summary>
		public const int Function = 5;
		/// <summary>
		/// Стиль имён типов данных
		/// </summary>
		public const int Type = 6;
		/// <summary>
		/// Стиль скобок
		/// </summary>
		public const int Bracket = 7;
		/// <summary>
		/// Стиль идентификаторов переменных
		/// </summary>
		public const int Identifier = 8;
		/// <summary>
		/// Стиль идентификаторов объектов расчётной модели
		/// </summary>
		public const int ItemId = 9;
		/// <summary>
		/// Стиль комментариев
		/// </summary>
		public const int Comment = 10;
		/// <summary>
		/// Стиль операторов
		/// </summary>
		public const int Operator = 11;
		/// <summary>
		/// Стиль строк
		/// </summary>
		public const int String = 12;
		/// <summary>
		/// Стиль чисел
		/// </summary>
		public const int Number = 13;
		/// <summary>
		/// Стиль выделения
		/// </summary>
		public const int Selection = 14;
		/// <summary>
		/// Стиль парных скобок
		/// </summary>
		public const int BraceMatch = 15;
		/// <summary>
		/// Стиль найденного текста
		/// </summary>
		public const int FoundMatch = 16;
		/// <summary>
		/// Стиль строки с ошибкой
		/// </summary>
		public const int ErrorLine = 17;
		/// <summary>
		/// Стиль нумерации строк (используется на стороне клиента или при синтаксическом разборе)
		/// </summary>
		public const int LineNumbers = 18;
	}

	/// <summary>
	/// Режимы сохранения модели в двоичном формате
	/// </summary>
	public static class SaveMode
	{
		/// <summary>
		/// Сохранение полной модели без последних результатов расчётов (экв. 13)
		/// </summary>
		public const int FullModel = 0;
		/// <summary>
		/// Сохранение только данных об объектах модели
		/// </summary>
		public const int ItemsOnly = 1;
		/// <summary>
		/// Сохранение результатов последней стадии (внутреннее использование)
		/// </summary>
		public const int RecentResults = 3;
		/// <summary>
		/// Сохранение данных о стадийности
		/// </summary>
		public const int Stages = 5;
		/// <summary>
		/// Сохранение данных Cookie
		/// </summary>
		public const int Cookie = 9;
	}

	/// <summary>
	/// Статусы работа скрипта
	/// </summary>
	public static class ScriptState
	{
		/// <summary>
		/// Скрипт запущен
		/// </summary>
		public const long Running = unchecked((long)0x001);
		/// <summary>
		/// Скрипт приостановлен
		/// </summary>
		public const long Suspended = unchecked((long)0x002);
		/// <summary>
		/// Прекращена работа скрипта
		/// </summary>
		public const long Terminated = unchecked((long)0x004);
		/// <summary>
		/// Скрипт находится в состоянии ожидания
		/// </summary>
		public const long Waiting = unchecked((long)0x008);
		/// <summary>
		/// Скрипт находится в состоянии расчёта
		/// </summary>
		public const long Analysing = unchecked((long)0x010);
		/// <summary>
		/// Произошла ошибка выполнения скрипта
		/// </summary>
		public const long Error = unchecked((long)0x020);
		/// <summary>
		/// </summary>
		/// Скрипт находится в состоянии отладки
		public const long Debugging = unchecked((long)0x040);
	}

	/// <summary>
	/// Режимы вставки текста в буфер обмена
	/// </summary>
	public static class Clipboard
	{
		/// <summary>
		/// Копирование
		/// </summary>
		public const int Copy = 0;
		/// <summary>
		/// Объединение снизу
		/// </summary>
		public const int Bottom = 1;
		/// <summary>
		/// Объединение слева
		/// </summary>
		public const int Left = 2;
		/// <summary>
		/// Объединение справа
		/// </summary>
		public const int Right = 3;
		/// <summary>
		/// Объединение сверху
		/// </summary>
		public const int Top = 4;
		/// <summary>
		/// Замена групп пробелов на знаки табуляции
		/// </summary>
		public const int Tabs = 8;
		/// <summary>
		/// Замена десятичного разделителя
		/// </summary>
		public const int Separator = 16;
	}

	/// <summary>
	/// Константы состояния клавиш-переключателей
	/// </summary>
	public static class ShiftKey
	{
		/// <summary>
		/// Клавиша Alt
		/// </summary>
		public const int Alt = unchecked((int)0x00000001);
		/// <summary>
		/// Клавиша Ctrl
		/// </summary>
		public const int Ctrl = unchecked((int)0x00000002);
		/// <summary>
		/// Клавиша Shift
		/// </summary>
		public const int Shift = unchecked((int)0x00000004);
		/// <summary>
		/// Клавиша Win
		/// </summary>
		public const int Win = unchecked((int)0x00000008);
		/// <summary>
		/// Маска клавиш-переключателей
		/// </summary>
		public const int Keyboard = Alt | Ctrl | Shift | Win;
		/// <summary>
		/// Левая кнопка мыши
		/// </summary>
		public const int MouseLeft = unchecked((int)0x00000010);
		/// <summary>
		/// Средняя кнопка мыши
		/// </summary>
		public const int MouseMiddle = unchecked((int)0x00000020);
		/// <summary>
		/// Правая кнопка мыши
		/// </summary>
		public const int MouseRight = unchecked((int)0x00000040);
		/// <summary>
		/// Кнопка мыши X1
		/// </summary>
		public const int MouseX1 = unchecked((int)0x00000080);
		/// <summary>
		///  Кнопка мыши X2
		/// </summary>
		public const int MouseX2 = unchecked((int)0x00000100);
		/// <summary>
		/// Маска кнопок мыши
		/// </summary>
		public const int Mouse = MouseLeft | MouseMiddle | MouseRight | MouseX1 | MouseX2;
	}

	/// <summary>
	/// Константы флагов поиска
	/// </summary>
	public static class Search
	{
		/// <summary>
		/// Поиск в обратном направлении
		/// </summary>
		public const int Backward = 1;
		/// <summary>
		/// Регистрозависимый поиск
		/// </summary>
		public const int CaseSensitive = 1 << 1;
		/// <summary>
		/// Поиск по целым словам
		/// </summary>
		public const int WholeWords = 1 << 2;
		/// <summary>
		/// Поиск по выделению
		/// </summary>
		public const int Selection = 1 << 3;
		/// <summary>
		/// Поиск по идентификаторам
		/// </summary>
		public const int Identifiers = 1 << 4;
		/// <summary>
		/// Поиск вне комментариев
		/// </summary>
		public const int IgnoreComments = 1 << 5;
		/// <summary>
		/// Поиск вне строк
		/// </summary>
		public const int IgnoreStrings = 1 << 6;
	}


/// <summary>
/// Интерфейс, метод которого вызывается при завершении асинхронных процессов
/// </summary>
[ComImport, Guid("FFEA7861-14E3-4AA0-A730-75763CC57E37"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IFinishEvent
{

	/// <summary>
	/// Вызывается при завершении асинхронного процесса
	/// </summary>
	/// <param name="Code">Код ошибки</param>
	/// <param name="Message">Сообщение об ошибке</param>
	void OnFinish([In] int Code,
		[In] string Message);

}


/// <summary>
/// Интерфейс обратного вызова для перечисления массивов целых чисел
/// </summary>
[ComImport, Guid("8A197910-B69E-4BAA-AC98-B310C69D6C8D"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface ILongEnum
{

	/// <summary>
	/// Передаёт приблизительное количество чисел массива
	/// </summary>
	/// <param name="Capacity">Приблизительное количество чисел</param>
	void SetCapacity([In] int Capacity);


	/// <summary>
	/// Передаёт очередное число массива
	/// </summary>
	/// <param name="N">Значение числа</param>
	/// <returns>Признак завершения перебора. При ненулевом значении перебор прекращается</returns>
	int Enum([In] long N);

}


/// <summary>
/// Интерфейс обратного вызова для перечисления массивов вещественных чисел
/// </summary>
[ComImport, Guid("ED5C169D-2EDE-4A06-8A64-B0437F095374"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IRealEnum
{

	/// <summary>
	/// Передаёт приблизительное количество чисел массива
	/// </summary>
	/// <param name="Capacity">Приблизительное количество чисел</param>
	void SetCapacity([In] int Capacity);


	/// <summary>
	/// Передаёт очередное число массива
	/// </summary>
	/// <param name="N">Значение числа</param>
	/// <returns>Признак завершения перебора. При ненулевом значении перебор прекращается</returns>
	int Enum([In] double N);

}


/// <summary>
/// Интерфейс массива вещественных чисел для асинхронных операций
/// </summary>
[ComImport, Guid("0EB28CD9-EC9C-4EDE-8A5E-BEA2C95685C8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IRealArray
{

	/// <summary>
	/// Возвращает количество элементов массива
	/// </summary>
	/// <returns>Количество элементов</returns>
	int GetSize();


	/// <summary>
	/// Возвращает значение элемента массива
	/// </summary>
	/// <param name="Index">Номер элемента массива</param>
	/// <returns>Значение элемента массива</returns>
	double GetItem([In] int Index);


	/// <summary>
	/// Перебирает элементы массива
	/// </summary>
	/// <param name="Enum">Интерфейс обратного вызова</param>
	void Enum([In, MarshalAs(UnmanagedType.Interface)] IRealEnum Enum);

}


/// <summary>
/// Интерфейс обратного вызова для перечисления массивов строк</param>
/// </summary>
[ComImport, Guid("CFC01F65-DC2A-4C14-B2D3-A8CFAA6E5436"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IStringEnum
{

	/// <summary>
	/// Передаёт приблизительное количество строк массива
	/// </summary>
	/// <param name="Capacity">Приблизительное количество строк</param>
	void SetCapacity([In] int Capacity);


	/// <summary>
	/// Передаёт очередную строку массива
	/// </summary>
	/// <param name="N">Значение строки</param>
	/// <returns>Признак завершения перебора. При ненулевом значении перебор прекращается</returns>
	int Enum([In] string Str);

}


/// <summary>
/// Интерфейс табличных данных
/// </summary>
[ComImport, Guid("7FF7CD00-2C52-4218-A5B5-114371078423"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IGridDataSource
{

	/// <summary>
	/// Возвращает количество столбцов таблицы
	/// </summary>
	/// <returns>Количество столбцов таблицы</returns>
	int GetColCount();


	/// <summary>
	/// Возвращает заголовок столбца таблицы
	/// </summary>
	/// <param name="Col">Номер столбца</param>
	/// <param name="ReadOnly">Признак столбца только для чтения</param>
	/// <returns>Заголовок столбца</returns>
	string GetColHeader([In] int Col,
		[Out] out int ReadOnly);


	/// <summary>
	/// Возвращает количество строк таблицы
	/// </summary>
	/// <returns>Количество строк таблицы</returns>
	int GetRowCount();


	/// <summary>
	/// Возвращает видимое значение ячейки
	/// </summary>
	/// <param name="Row">Номер строки</param>
	/// <param name="Col">Номер столбца</param>
	/// <param name="State">Статус ячейки</param>
	/// <param name="Hint">Подсказка для ячейки</param>
	/// <returns>Отображаемое значение ячейки таблицы</returns>
	string GetDisplayCell([In] int Row,
		[In] int Col,
		[Out] out double State,
		[Out] out string Hint);


	/// <summary>
	/// Возвращает редактируемое значение ячейки таблицы
	/// </summary>
	/// <param name="Row">Номер строки</param>
	/// <param name="Col">Номер столбца</param>
	/// <returns>Редактируемое значение ячейки таблицы</returns>
	string GetEditCell([In] int Row,
		[In] int Col);


	/// <summary>
	/// Устанавливает значение ячейки таблицы
	/// </summary>
	/// <param name="Row">Номер строки</param>
	/// <param name="Col">Номер столбца</param>
	/// <param name="Value">Значение ячейки</param>
	void SetCell([In] int Row,
		[In] int Col,
		[In] string Value);


	/// <summary>
	/// Возвращает информацию о порядке строк таблицы
	/// </summary>
	/// <returns>Порядок строк таблицы</returns>
	string GetRowOrder();


	/// <summary>
	/// Устанавливает порядок следования строк таблицы
	/// </summary>
	/// <param name="Order">Порядок следования строк таблицы, полученный от метода GetRowOrder</param>
	void RestoreRowOrder([In] string Order);


	/// <summary>
	/// Добавляет новую строку таблицы
	/// </summary>
	/// <param name="Data">Данные новой строки</param>
	void AddRow([In] string Data);


	/// <summary>
	/// Удаляет строку таблицы
	/// </summary>
	/// <param name="Row">Номер строки</param>
	/// <returns>Информация для восстановления строки</returns>
	string DeleteRow([In] int Row);


	/// <summary>
	/// Восстанавливает строку таблицы
	/// </summary>
	/// <param name="Row">Номер строки</param>
	/// <param name="RecoveryData">Информация для восстановления, полученная от метода DeleteRow</param>
	void RestoreRow([In] int Row,
		[In] string RecoveryData);


	/// <summary>
	/// Помечает строку, чтобы её можно было найти после фильтрации или сортировки
	/// </summary>
	/// <param name="Row">Номер строки</param>
	void MarkRow([In] int Row);


	/// <summary>
	/// Возвращает позицию помеченной строки после сортировки или фильтрации
	/// </summary>
	/// <returns>Позиция строки, или отрицательное число, если строка больше не показывается</returns>
	int GetMarkedRow();


	/// <summary>
	/// Устанавливает фильтр строк таблицы
	/// </summary>
	/// <param name="Exp">Выражение для фильтра</param>
	/// <param name="Finish">Интерфейс события завершения асинхронного процесса</param>
	void Filter([In] string Exp,
		[In, MarshalAs(UnmanagedType.Interface)] IFinishEvent Finish);


	/// <summary>
	/// Проводит сортировку строк таблицы по значениям столбцов
	/// </summary>
	/// <param name="Exp">Порядок сортировки. Строка из цифр и знаков + или -.
	///   число представляет собой номер выражения,
	///  знаки + или - : порядок сортировки по возрастанию или по убыванию
	///   Например "+0 -2" означает сортировку по столбцу 0 по возрастанию при равенстве по столбцу 2 по убыванию</param>
	/// <param name="Finish">Интерфейс события завершения асинхронного процесса</param>
	void Sort([In] string Order,
		[In, MarshalAs(UnmanagedType.Interface)] IFinishEvent Finish);


	/// <summary>
	/// Проверяет наличие плохих ячеек в таблице
	/// </summary>
	/// <param name="Row">Номер строки первой плохой ячейки</param>
	/// <param name="Col">Номер столбца первой плохой ячейки</param>
	void HasBadCells([Out] out int Row,
		[Out] out int Col);


	/// <summary>
	/// Проверяет, что строка таблицы заблокирована от редактирования
	/// </summary>
	/// <param name="Row">Номер строки</param>
	/// <returns>Признак блокировки строки</returns>
	int IsRowLocked([In] int Row);


	/// <summary>
	/// Возвращает имя таблицы
	/// </summary>
	/// <returns>Имя таблицы</returns>
	string GetName();

}


/// <summary>
/// Общий интерфейс списка объектов расчётной модели
/// </summary>
[ComImport, Guid("5BD894F3-5196-496A-9FC6-ACDF0816394C"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IItemList
{

	/// <summary>
	/// Возвращает имя класса объектов
	/// </summary>
	/// <returns>Имя класса</returns>
	string ClassName();


	/// <summary>
	/// Создаёт объект расчётной модели
	/// </summary>
	/// <param name="Id">Идентификатор объекта</param>
	/// <returns>Адрес объекта</returns>
	long AddItem([In] string Id);


	/// <summary>
	/// Перечисляет адреса объектов
	/// </summary>
	/// <param name="Enum">Указатель на интерфейс перечисления</param>
	void Enum([In, MarshalAs(UnmanagedType.Interface)] ILongEnum Client);


	/// <summary>
	/// Возвращает признак валидности объектов
	/// </summary>
	/// <param name="Addr">Адрес объекта</param>
	/// <returns>Признак валидности</returns>
	int IsValid([In] long Addr);


	/// <summary>
	/// Помечает объект как удалённый
	/// </summary>
	/// <param name="Addr">Адрес объекта</param>
	/// <returns>Информация для восстановления</returns>
	string Invalidate([In] long Addr);


	/// <summary>
	/// Восстанавливает помеченный на удаление объект
	/// </summary>
	/// <param name="RecoveryData">Информация для восстановления, полученная от метода Invalidate</param>
	void RestoreInvalidated([In] string RecoveryData);


	/// <summary>
	/// Возвращает признак активности объекта
	/// </summary>
	/// <param name="Addr">Адрес узла</param>
	/// <returns>Признак активности</returns>
	int IsActive([In] long Addr);


	/// <summary>
	/// Устанавливает признак активности объекта
	/// </summary>
	/// <param name="Addr">Адрес объекта</param>
	/// <param name="Flag">Признак активности</param>
	void SetActive([In] long Addr,
		[In] int Flag);


	/// <summary>
	/// Возвращает идентификатор объекта
	/// </summary>
	/// <param name="Addr">Адрес объекта</param>
	/// <returns>Идентификатор объекта</returns>
	string GetId([In] long Addr);


	/// <summary>
	/// Устанавливает идентификатор объекта
	/// </summary>
	/// <param name="Addr">Адрес объекта</param>
	/// <param name="Id">Идентификатор объекта</param>
	void SetId([In] long Addr,
		[In] string Id);


	/// <summary>
	/// Возвращает адрес объекта по идентификатору
	/// </summary>
	/// <param name="Id">Идентификатор объекта</param>
	/// <returns>Адрес узла</returns>
	long GetAddr([In] string Id);


	/// <summary>
	/// Возвращает имя слоя объекта
	/// </summary>
	/// <param name="Addr">Адрес объекта</param>
	/// <returns>Имя слоя объекта</returns>
	string GetLayer([In] long Addr);


	/// <summary>
	/// Устанавливает имя слоя объекта
	/// </summary>
	/// <param name="Addr">Адрес объекта</param>
	/// <param name="Layer">Имя слоя объекта</param>
	void SetLayer([In] long Addr,
		[In] string Layer);


	/// <summary>
	/// Возвращает цвет объекта
	/// </summary>
	/// <param name="Addr">Адрес объекта</param>
	/// <returns>Цвет объекта</returns>
	int GetColor([In] long Addr);


	/// <summary>
	/// Устанавливает цвет объекта
	/// </summary>
	/// <param name="Addr">Адрес объекта</param>
	/// <param name="Color">Цвет объекта</param>
	void SetColor([In] long Addr,
		[In] int Color);


	/// <summary>
	/// Проверяет принадлежность объекта списку
	/// </summary>
	/// <param name="Addr">Адрес объекта</param>
	/// <returns>Признак принадлежности</returns>
	int HasItemAddr([In] long Addr);

}


/// <summary>
/// Интерфейс группы объектов расчётной модели
/// </summary>
[ComImport, Guid("8E7512D6-B838-417E-82F3-C1DD6AD3024D"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IGroup
{

	/// <summary>
	/// Возвращает количество элементов списка
	/// </summary>
	/// <returns>Количество объектов</returns>
	int GetSize();


	/// <summary>
	/// Возвращает адрес объекта с указанным индексом
	/// </summary>
	/// <param name="Index">Индекс объекта</param>
	/// <returns>Адрес объекта (для невалидных объектов возвращается нуль)</returns>
	long GetItem([In] int Index);


	/// <summary>
	/// Перечисляет объекты группы в порядке их добавления в группу
	/// </summary>
	/// <param name="Client">Указатель на интефейс обратного вызова</param>
	void Enum([In, MarshalAs(UnmanagedType.Interface)] ILongEnum Client);


	/// <summary>
	/// Очищает группу
	/// </summary>
	void Clear();


	/// <summary>
	/// Проверяет, что объект входит в группу
	/// </summary>
	/// <param name="ItemAddr">Адрес объекта</param>
	/// <returns>1 – объект входит в группу, 0 – не входит</returns>
	int HasItem([In] long ItemAddr);


	/// <summary>
	/// Добавляет объект в группу
	/// </summary>
	/// <param name="ItemAddr">Адрес объекта</param>
	void AddItem([In] long ItemAddr);


	/// <summary>
	/// Добавляет объекты в группу
	/// </summary>
	/// <param name="Group">Группа добавляемых объектов</param>
	void AddGroup([In, MarshalAs(UnmanagedType.Interface)] IGroup Group);


	/// <summary>
	/// Удаляет объект из группы
	/// </summary>
	/// <param name="ItemAddr">Адрес объекта</param>
	void RemoveItem([In] long ItemAddr);


	/// <summary>
	/// Удаляет объекты из группы
	/// </summary>
	/// <param name="Group">Группа удаляемых объектов</param>
	void RemoveGroup([In, MarshalAs(UnmanagedType.Interface)] IGroup Group);


	/// <summary>
	/// Выполняет процедуру пересечения с группой [   [ab]   ]
	/// </summary>
	/// <param name="Group">Группа объектов</param>
	void IntersectGroup([In, MarshalAs(UnmanagedType.Interface)] IGroup Group);


	/// <summary>
	/// Выполняет процедуру пересечения с группой [aaa[  ]bbb]
	/// </summary>
	/// <param name="Group">Группа объектов</param>
	void ExcludeGroup([In, MarshalAs(UnmanagedType.Interface)] IGroup Group);


	/// <summary>
	/// Возвращает развёрнутый список идентификаторов объектов
	/// </summary>
	/// <returns>Список идентификаторов</returns>
	string GetDetailedList();


	/// <summary>
	/// Возвращает сжатый список идентификаторов объектов
	/// </summary>
	/// <returns>Список идентификаторов</returns>
	string GetCondensedList();

}


/// <summary>
/// Интерфейс списка систем координат расчётной модели
/// </summary>
[ComImport, Guid("73D99120-5302-44D4-91DD-EA42639BAF69"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface ICSList
{

	/// <summary>
	/// Создаёт новую координатную систему
	/// </summary>
	/// <param name="Id">Идентификатор координатной системы</param>
	/// <returns>Адрес координатной системы</returns>
	long Add([In] string Id);


	/// <summary>
	/// Возвращает начало координатной системы
	/// </summary>
	/// <param name="Addr">Адрес координатной системы</param>
	/// <param name="X">Продольная координата</param>
	/// <param name="Y">Поперечная координата</param>
	/// <param name="Z">Вертикальная координата</param>
	void GetOrigin([In] long Addr,
		[Out] out double X,
		[Out] out double Y,
		[Out] out double Z);


	/// <summary>
	/// Устанавливает начало координатной системы
	/// </summary>
	/// <param name="Addr">Адрес координатной системы</param>
	/// <param name="X">Продольная координата</param>
	/// <param name="Y">Поперечная координата</param>
	/// <param name="Z">Вертикальная координата</param>
	void SetOrigin([In] long Addr,
		[In] double X,
		[In] double Y,
		[In] double Z);


	/// <summary>
	/// Возвращает углы поворота координатной системы
	/// </summary>
	/// <param name="Addr">Адрес координатной системы</param>
	/// <param name="RZ">Поворот вокруг глобальной оси Z</param>
	/// <param name="RY">Поворот вокруг глобальной оси Y</param>
	/// <param name="RX">Поворот вокруг глобальной оси X</param>
	void GetRotations([In] long Addr,
		[Out] out double RZ,
		[Out] out double RY,
		[Out] out double RX);


	/// <summary>
	/// Возвращает значение ячейки тензора координатной системы
	/// </summary>
	/// <param name="Addr">Адрес координатной системы</param>
	/// <param name="Row">Номер строки ячейки (0, 1, 2)</param>
	/// <param name="Col">Номер столбца ячейки (0, 1, 2)</param>
	/// <returns>Значение ячейки</returns>
	double GetTensorCell([In] long Addr,
		[In] int Row,
		[In] int Col);


	/// <summary>
	/// Устанавливает тензор поворота по двум точкам и углу поворота вокруг локальной оси X
	/// </summary>
	/// <param name="Addr">Адрес координатной системы</param>
	/// <param name="X1">Продольная координата первой точки</param>
	/// <param name="Y1">Поперечная координата первой точки</param>
	/// <param name="Z1">Вертикальная координата первой точки</param>
	/// <param name="X2">Продольная координата второй точки</param>
	/// <param name="Y2">Поперечная координата второй точки</param>
	/// <param name="Z2">Вертикальная координата второй точки</param>
	/// <param name="Beta">Угол поворота вокруг локальной оси X</param>
	void SetTensor2p([In] long Addr,
		[In] double X1,
		[In] double Y1,
		[In] double Z1,
		[In] double X2,
		[In] double Y2,
		[In] double Z2,
		[In] double Alpha);


	/// <summary>
	/// Устанавливает тензор поворота по трём точкам
	/// </summary>
	/// <param name="Addr">Адрес координатной системы</param>
	/// <param name="X1">Продольная координата первой точки</param>
	/// <param name="Y1">Поперечная координата первой точки</param>
	/// <param name="Z1">Вертикальная координата первой точки</param>
	/// <param name="X2">Продольная координата второй точки</param>
	/// <param name="Y2">Поперечная координата второй точки</param>
	/// <param name="Z2">Вертикальная координата второй точки</param>
	/// <param name="X3">Продольная координата третьей точки</param>
	/// <param name="Y3">Поперечная координата третьей точки</param>
	/// <param name="Z3">Вертикальная координата третьей точки</param>
	void SetTensor3p([In] long Addr,
		[In] double X1,
		[In] double Y1,
		[In] double Z1,
		[In] double X2,
		[In] double Y2,
		[In] double Z2,
		[In] double X3,
		[In] double Y3,
		[In] double Z3);


	/// <summary>
	/// Устанавливает тензор по трём углам поворота
	/// </summary>
	/// <param name="Addr">Адрес координатной системы</param>
	/// <param name="X0">Продольная координата начальной точки</param>
	/// <param name="Y1">Поперечная координата начальной точки</param>
	/// <param name="Z1">Вертикальная координата начальной точки</param>
	/// <param name="R1">Первый угол поворота</param>
	/// <param name="R2">Второй угол поворота</param>
	/// <param name="R3">Третий угол поворота</param>
	/// <param name="Sequence">Последовательность углов поворота.
	/// Первый символ задаёт тип поворотов (С – Кардановы вращения, E – Эйлеровы).
	/// Далее следует последовательность трём символов XYZ в порядке выполнения вращений</param>
	void SetTensorRotation([In] long Addr,
		[In] double X0,
		[In] double Y0,
		[In] double Z0,
		[In] double R1,
		[In] double R2,
		[In] double R3,
		[In] string Sequence);


	/// <summary>
	/// Поворачивает тензор на заданный угол вокруг локальной оси
	/// </summary>
	/// <param name="Addr">Адрес координатной системы</param>
	/// <param name="Axis">Ось вращения (0 – X, 1 – Y, 2 – Z)</param>
	/// <param name="Theta">Угол поворота</param>
	void RotateTensor([In] long Addr,
		[In] int Axis,
		[In] double Theta);


	/// <summary>
	/// Устанавливает тензор поворота как локальный для объекта расчётной модели
	/// </summary>
	/// <param name="Addr">Адрес координатной системы</param>
	/// <param name="Item">Адрес объекта расчётной модели</param>
	void SetTensorLocal([In] long Addr,
		[In] long Item);


	/// <summary>
	/// Устанавливает тензор поворота как узловой для объекта расчётной модели
	/// </summary>
	/// <param name="Addr">Адрес координатной системы</param>
	/// <param name="Item">Адрес объекта расчётной модели</param>
	void SetTensorNodal([In] long Addr,
		[In] long Item);


	/// <summary>
	/// Возвращает адрес объекта, используемого для определения координатной системы
	/// </summary>
	/// <param name="Addr">Адрес координатной системы</param>
	/// <param name="Local">Признак локальной системы координат (1 – локальная, 0 – узловая)</param>
	/// <returns>Адрес объекта</returns>
	long GetRefItem([In] long Addr,
		[Out] out int Local);

}


/// <summary>
/// Интерфейс списка узлов
/// </summary>
[ComImport, Guid("3A19C5FC-B841-488A-AEDB-C34E785512D2"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface INodeList
{

	/// <summary>
	/// Возвращает глобальные координаты узла
	/// </summary>
	/// <param name="Addr">Адрес узла</param>
	/// <param name="X">Продольная координата</param>
	/// <param name="Y">Поперечная координата</param>
	/// <param name="Z">Вертикальная координата</param>
	void GetGlobalCoord([In] long Addr,
		[Out] out double X,
		[Out] out double Y,
		[Out] out double Z);


	/// <summary>
	/// Устанавливает глобальные координаты узла
	/// </summary>
	/// <param name="Addr">Адрес узла</param>
	/// <param name="X">Продольная координата</param>
	/// <param name="Y">Поперечная координата</param>
	/// <param name="Z">Вертикальная координата</param>
	void SetGlobalCoord([In] long Addr,
		[In] double X,
		[In] double Y,
		[In] double Z);


	/// <summary>
	/// Возвращает локальные координаты узла
	/// </summary>
	/// <param name="Addr">Адрес узла</param>
	/// <param name="X">Продольная координата</param>
	/// <param name="Y">Поперечная координата</param>
	/// <param name="Z">Вертикальная координата</param>
	void GetLocalCoord([In] long Addr,
		[Out] out double X,
		[Out] out double Y,
		[Out] out double Z);


	/// <summary>
	/// Устанавливает локальные координаты узла
	/// </summary>
	/// <param name="Addr">Адрес узла</param>
	/// <param name="X">Продольная координата</param>
	/// <param name="Y">Поперечная координата</param>
	/// <param name="Z">Вертикальная координата</param>
	void SetLocalCoord([In] long Addr,
		[In] double X,
		[In] double Y,
		[In] double Z);


	/// <summary>
	/// Возвращает маску закреплённых степеней свободы
	/// </summary>
	/// <param name="Addr">Адрес узла</param>
	/// <returns>Маска закреплённых степеней свободы</returns>
	int GetSupport([In] long Addr);


	/// <summary>
	/// Устанавливает маску закреплённых степеней свободы
	/// </summary>
	/// <param name="Addr">Адрес узла</param>
	/// <param name="Mask">Маска закреплённых степеней свободы</param>
	void SetSupport([In] long Addr,
		[In] int Mask);


	/// <summary>
	/// Возвращает маску степеней свободы, освобождённых от жёсткого тела
	/// </summary>
	/// <param name="Addr">Адрес узла</param>
	/// <returns>Маска освобождённых степеней свободы</returns>
	int GetRelease([In] long Addr);


	/// <summary>
	/// Устанавливает маску степеней свободы, освобождённых от жёсткого тела
	/// </summary>
	/// <param name="Addr">Адрес узла</param>
	/// <param name="Mask">Маска освобождённых степеней свободы</param>
	void SetRelease([In] long Addr,
		[In] int Mask);


	/// <summary>
	/// Возвращает адреc барского узла
	/// </summary>
	/// <param name="Addr">Адрес узла</param>
	/// <returns>Адрес барского узла</returns>
	long GetMaster([In] long Addr);


	/// <summary>
	/// Устанавливает адреc барского узла
	/// </summary>
	/// <param name="Addr">Адрес узла</param>
	/// <param name="Master">Адрес барского узла</param>
	void SetMaster([In] long Addr,
		[In] long Master);


	/// <summary>
	/// Возвращает адрес локальной системы координат
	/// </summary>
	/// <param name="Addr">Адрес узла</param>
	/// <returns>Адрес локальной системы координат</returns>
	long GetLocalCS([In] long Addr);


	/// <summary>
	/// Устанавливает адрес локальной системы координат
	/// </summary>
	/// <param name="Addr">Адрес узла</param>
	/// <param name="CS">Адрес локальной системы координат</param>
	void SetLocalCS([In] long Addr,
		[In] long CS);


	/// <summary>
	/// Возвращает список инцидентных элементов
	/// </summary>
	/// <param name="Addr">Адрес узла</param>
	/// <returns>Список инцидентных элементов</returns>
	string GetIncidence([In] long Addr);

}


/// <summary>
/// Интерфейс списка материалов
/// </summary>
[ComImport, Guid("11167B80-35D5-461F-BBE2-6D67451C3C52"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IMaterialList
{

	/// <summary>
	/// Возвращает материал по умолчанию
	/// </summary>
	/// <returns>Адрес материала</returns>
	long GetDefault();


	/// <summary>
	/// Устанавливает материал по умолчанию
	/// </summary>
	/// <param name="Addr">Адрес материала</param>
	void SetDefault([In] long Addr);


	/// <summary>
	/// Возвращает модуль упругости материала
	/// </summary>
	/// <param name="Addr">Адрес материала</param>
	/// <returns>Значение модуля упругости</returns>
	double GetElast([In] long Addr);


	/// <summary>
	/// Устанавливает модуль упругости материала
	/// </summary>
	/// <param name="Addr">Адрес материала</param>
	/// <param name="Elast">Значение модуля упругости</param>
	void SetElast([In] long Addr,
		[In] double Elast);


	/// <summary>
	/// Возвращает коэффициент Пуассона материала
	/// </summary>
	/// <param name="Addr">Адрес материала</param>
	/// <returns>Значение коэффициента Пуассона</returns>
	double GetPoisson([In] long Addr);


	/// <summary>
	// Устанавливает коэффициент Пуассона материала
	/// </summary>
	/// <param name="Addr">Адрес материала</param>
	/// <param name="Poisson">Значение коэффициента Пуассона</param>
	void SetPoisson([In] long Addr,
		[In] double Poisson);


	/// <summary>
	/// Возвращает модуль сдвига материала (получение корректного значения возможно только после
	/// установки значений модуля упругости и коэффициента Пуассона
	/// </summary>
	/// <param name="Addr">Адрес материала</param>
	/// <returns>Значение модуля сдвига</returns>
	double GetShear([In] long Addr);


	/// <summary>
	/// Устанавливает коэффициент Пуассона из модуля упгругости и полученного модуля сдвига материала
	/// </summary>
	/// <param name="Addr">Адрес материала</param>
	/// <param name="Shear">Значение модуля сдвига</param>
	void SetShear([In] long Addr,
		[In] double Shear);


	/// <summary>
	/// Возвращает объёмный вес материала
	/// </summary>
	/// <param name="Addr">Адрес материала</param>
	/// <returns>Значение объёмного веса</returns>
	double GetDen([In] long Addr);


	/// <summary>
	/// Устанавливает объёмный вес материала
	/// </summary>
	/// <param name="Addr">Адрес материала</param>
	/// <param name="Den">Значение объёмного веса</param>
	void SetDen([In] long Addr,
		[In] double Den);


	/// <summary>
	/// Возвращает коэффициент температурного расширения относительно локальной оси
	/// </summary>
	/// <param name="Addr">Адрес материала</param>
	/// <param name="Axis">Ось (0 – X, 1 – Y, 2 – Z)</param>
	/// <returns>Значение коэффициента</returns>
	double GetCte([In] long Addr,
		[In] int Axis);


	/// <summary>
	/// Устанавливает коэффициент температурного расширения относительно локальной оси
	/// </summary>
	/// <param name="Addr">Адрес материала</param>
	/// <param name="Axis">Ось (0 – X, 1 – Y, 2 – Z, -1 – все оси)</param>
	/// <param name="Cte">Значение коэффициента</param>
	void SetCte([In] long Addr,
		[In] int Axis,
		[In] double Cte);


	/// <summary>
	/// Возвращает предельное значение растягивающих напряжений
	/// </summary>
	/// <param name="Addr">Адрес материала</param>
	/// <returns>Значение напряжений</returns>
	double GetMaxStress([In] long Addr);


	/// <summary>
	/// Устанавливает предельное значение растягивающих напряжений
	/// </summary>
	/// <param name="Addr">Адрес материала</param>
	/// <param name="MaxStress">Значение напряжений</param>
	void SetMaxStress([In] long Addr,
		[In] double MaxStress);


	/// <summary>
	/// Возвращает предельное значение сжимающих напряжений
	/// </summary>
	/// <param name="Addr">Адрес материала</param>
	/// <returns>Значение напряжений</returns>
	double GetMinStress([In] long Addr);


	/// <summary>
	/// Устанавливает предельное значение сжимающих напряжений
	/// </summary>
	/// <param name="Addr">Адрес материала</param>
	/// <param name="MinStress">Значение напряжений</param>
	void SetMinStress([In] long Addr,
		[In] double MinStress);

}


/// <summary>
/// Интерфейс списка сечений
/// </summary>
[ComImport, Guid("E9B0AB19-8BC3-4971-9E09-82AEE2E2B2EE"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface ISectionList
{

	/// <summary>
	/// Возвращает сечение по умолчанию
	/// </summary>
	/// <returns>Адрес cечения</returns>
	long GetDefault();


	/// <summary>
	/// Устанавливает сечение по умолчанию
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	void SetDefault([In] long Addr);


	/// <summary>
	/// Возвращает площадь поперечного сечения
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <returns>Площадь поперечного сечения</returns>
	double GetAx([In] long Addr);


	/// <summary>
	/// Устанавливает площадь поперечного сечения
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <param name="Value">Площадь поперечного сечения</param>
	void SetAx([In] long Addr,
		[In] double Value);


	/// <summary>
	/// Возвращает площадь сдвига относительно оси Y
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <returns>Площадь сдвига</returns>
	double GetAy([In] long Addr);


	/// <summary>
	/// Устанавливает площадь сдвига относительно оси Y
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <param name="Value">Площадь сдвига</param>
	void SetAy([In] long Addr,
		[In] double Value);


	/// <summary>
	/// Возвращает площадь сдвига относительно оси Z
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <returns>Площадь сдвига</returns>
	double GetAz([In] long Addr);


	/// <summary>
	/// Устанавливает площадь сдвига относительно оси Z
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <param name="Value">Площадь сдвига</param>
	void SetAz([In] long Addr,
		[In] double Value);


	/// <summary>
	/// Возвращает момент инерции при чистом кручении
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <returns>Момент инерции</returns>
	double GetIx([In] long Addr);


	/// <summary>
	/// Устанавливает момент инерции при чистом кручении
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <param name="Value">Момент инерции</param>
	void SetIx([In] long Addr,
		[In] double Value);


	/// <summary>
	/// Возвращает момент инерции при изгибе относительно оси Y
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <returns>Момент инерции</returns>
	double GetIy([In] long Addr);


	/// <summary>
	/// Устанавливает момент инерции при изгибе относительно оси Y
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <param name="Value">Момент инерции</param>
	void SetIy([In] long Addr,
		[In] double Value);


	/// <summary>
	/// Возвращает момент инерции при изгибе относительно оси Z
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <returns>Момент инерции</returns>
	double GetIz([In] long Addr);


	/// <summary>
	/// Устанавливает момент инерции при изгибе относительно оси Y
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <param name="Value">Момент инерции</param>
	void SetIz([In] long Addr,
		[In] double Value);


	/// <summary>
	/// Возвращает расстояние вдоль узловой оси Y от узла до центра тяжести
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <returns>Расстояние</returns>
	double GetYc([In] long Addr);


	/// <summary>
	/// Устанавливает расстояние вдоль узловой оси Y от узла до центра тяжести
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <param name="Value">Расстояние</param>
	void SetYc([In] long Addr,
		[In] double Value);


	/// <summary>
	/// Возвращает расстояние вдоль узловой оси Z от узла до центра тяжести
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <returns>Расстояние</returns>
	double GetZc([In] long Addr);


	/// <summary>
	/// Устанавливает расстояние вдоль узловой оси Z от узла до центра тяжести
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <param name="Value">Расстояние</param>
	void SetZc([In] long Addr,
		[In] double Value);


	/// <summary>
	/// Возвращает угол поворота локальной системы координат относительно узловой
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <returns>Угол поворота</returns>
	double GetAlpha([In] long Addr);


	/// <summary>
	/// Устанавливает угол поворота локальной системы координат относительно узловой
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <param name="Value">Угол поворота</param>
	void SetAlpha([In] long Addr,
		[In] double Value);


	/// <summary>
	/// Возвращает расстояние вдоль локальной оси Y от центра тяжести до центра кручения
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <returns>Расстояние</returns>
	double GetYs([In] long Addr);


	/// <summary>
	/// Устанавливает расстояние вдоль локальной оси Y от центра тяжести до центра кручения
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <param name="Value">Расстояние</param>
	void SetYs([In] long Addr,
		[In] double Value);


	/// <summary>
	/// Возвращает расстояние вдоль локальной оси Z от центра тяжести до центра кручения
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <returns>Расстояние</returns>
	double GetZs([In] long Addr);


	/// <summary>
	/// Устанавливает расстояние вдоль локальной оси Z от центра тяжести до центра кручения
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <param name="Value">Расстояние</param>
	void SetZs([In] long Addr,
		[In] double Value);


	/// <summary>
	/// Возвращает толщину пластины
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <returns>Толщина пластины</returns>
	double GetThickness([In] long Addr);


	/// <summary>
	/// Устанавливает толщину пластины
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <param name="Value">Толщина пластины</param>
	void SetThickness([In] long Addr,
		[In] double Value);


	/// <summary>
	/// Возвращает идентификатор теории плоских элементов
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <returns>Идентификатор теории</returns>
	string GetTheory([In] long Addr);


	/// <summary>
	/// Устанавливает идентификатор теории плоских элементов
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <param name="Theory">Идентификатор теории</param>
	void SetTheory([In] long Addr,
		[In] string Theory);


	/// <summary>
	/// Возвращает признак учёта осевых деформаций
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <returns>Признак учёта: 1 – деформации учитываются, 0 – нет</returns>
	int GetAxial([In] long Addr);


	/// <summary>
	/// Устанавливает признак учёта осевых деформаций
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <param name="Value">Признак учёта: 1 – деформации учитываются, 0 – нет</param>
	void SetAxial([In] long Addr,
		[In] int Value);


	/// <summary>
	/// Возвращает признак учёта изгиба
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <returns>Признак учёта: 1 – деформации учитываются, 0 – нет</returns>
	int GetBending([In] long Addr);


	/// <summary>
	/// Устанавливает признак учёта изгиба
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <param name="Value">Признак учёта: 1 – деформации учитываются, 0 – нет</param>
	void SetBending([In] long Addr,
		[In] int Value);


	/// <summary>
	/// Возвращает следующее сечение в цепочке переменных сечений
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <returns>Адрес следующего сечения</returns>
	long GetNext([In] long Addr);


	/// <summary>
	/// Устанавливает следующее сечение в цепочке переменных сечений
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <param name="Next">Адрес следующего сечения</param>
	void SetNext([In] long Addr,
		[In] long Next);


	/// <summary>
	/// Возвращает расстояние до следующего сечения в цепочке переменных сечений
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <returns>Расстояние</returns>
	double GetDistance([In] long Addr);


	/// <summary>
	/// Устанавливает расстояние до следующего сечения в цепочке переменных сечений
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <param name="Value">Расстояние</param>
	void SetDistance([In] long Addr,
		[In] double Value);


	/// <summary>
	/// Возвращает количество контрольных точек сечения
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <returns>Количество точек</returns>
	int GetPointCount([In] long Addr);


	/// <summary>
	/// Возвращает идентификатор точки с указанных индексом
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <param name="Index">Индекс точки</param>
	/// <returns>Идентификатор точки</returns>
	long GetPointId([In] long Addr,
		[In] int Index);


	/// <summary>
	/// Удаляет контрольную точку сечения
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <param name="PointId">Идентификатор точки</param>
	void DeletePoint([In] long Addr,
		[In] long PointId);


	/// <summary>
	/// Возвращает локальную координату Y контрольной точки
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <param name="PointId">Идентификатор точки</param>
	/// <returns>Координата</returns>
	double GetPointY([In] long Addr,
		[In] long PointId);


	/// <summary>
	/// Устанавливает локальную координату Y контрольной точки
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <param name="PointId">Идентификатор точки</param>
	/// <param name="Value">Координата</param>
	void SetPointY([In] long Addr,
		[In] long PointId,
		[In] double Value);


	/// <summary>
	/// Возвращает локальную координату Z контрольной точки
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <param name="PointId">Идентификатор точки</param>
	/// <returns>Координата</returns>
	double GetPointZ([In] long Addr,
		[In] long PointId);


	/// <summary>
	/// Устанавливает локальную координату Z контрольной точки
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <param name="PointId">Идентификатор точки</param>
	/// <param name="Value">Координата</param>
	void SetPointZ([In] long Addr,
		[In] long PointId,
		[In] double Value);


	/// <summary>
	/// Возвращает статический момент отсечённой части относительно оси Y в точке
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <param name="PointId">Идентификатор точки</param>
	/// <returns>Статический момент</returns>
	double GetPointSy([In] long Addr,
		[In] long PointId);


	/// <summary>
	/// Устанавливает статический момент отсечённой части относительно оси Y в точке
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <param name="PointId">Идентификатор точки</param>
	/// <param name="Value">Статический момент</param>
	void SetPointSy([In] long Addr,
		[In] long PointId,
		[In] double Value);


	/// <summary>
	/// Возвращает статический момент отсечённой части относительно оси Z в точке
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <param name="PointId">Идентификатор точки</param>
	/// <returns>Статический момент</returns>
	double GetPointSz([In] long Addr,
		[In] long PointId);


	/// <summary>
	/// Устанавливает статический момент отсечённой части относительно оси Z в точке
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <param name="PointId">Идентификатор точки</param>
	/// <param name="Value">Статический момент</param>
	void SetPointSz([In] long Addr,
		[In] long PointId,
		[In] double Value);


	/// <summary>
	/// Возвращает ширину сечения вдоль оси Y в точке
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <param name="PointId">Идентификатор точки</param>
	/// <returns>Ширина сечения</returns>
	double GetPointBy([In] long Addr,
		[In] long PointId);


	/// <summary>
	/// Устанавливает ширину сечения вдоль оси Y в точке
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <param name="PointId">Идентификатор точки</param>
	/// <param name="Value">Ширина сечения</param>
	void SetPointBy([In] long Addr,
		[In] long PointId,
		[In] double Value);


	/// <summary>
	/// Возвращает ширину сечения вдоль оси Z в точке
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <param name="PointId">Идентификатор точки</param>
	/// <returns>Ширина сечения</returns>
	double GetPointBz([In] long Addr,
		[In] long PointId);


	/// <summary>
	/// Устанавливает ширину сечения вдоль оси Z в точке
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <param name="PointId">Идентификатор точки</param>
	/// <param name="Value">Ширина сечения</param>
	void SetPointBz([In] long Addr,
		[In] long PointId,
		[In] double Value);


	/// <summary>
	/// Возвращает интерфейс табличных данных для контрольных точек сечения
	/// </summary>
	/// <param name="Addr">Адрес сечения</param>
	/// <returns>Указатель на интерфейс</returns>
	IGridDataSource GetPointsGridDataSource([In] long Addr);

}


/// <summary>
/// Интерфейс списка ориентаций
/// </summary>
[ComImport, Guid("D767EAF8-7588-4082-A072-A43C52A3434B"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IOrientationList
{

	/// <summary>
	/// Возвращает тип ориентации
	/// </summary>
	/// <param name="Addr">Адрес ориентации</param>
	/// <returns>0 - угол поворота, 1 - вектор, 2 - координаты, 3 - узел</returns>
	int GetType([In] long Addr);


	/// <summary>
	/// Возвращает признак того, что ориентация происходит по оси Z
	/// </summary>
	/// <param name="Addr">Адрес ориентации</param>
	/// <returns>1 – ориентация по оси Z, 0 – по оси Y</returns>
	int GetAxisZ([In] long Addr);


	/// <summary>
	/// Устанавливает признак того, что ориентация происходит по оси Z
	/// </summary>
	/// <param name="Addr">Адрес ориентации</param>
	/// <param name="Z">1 – ориентация по оси Z, 0 – по оси Y</param>
	void SetAxisZ([In] long Addr,
		[In] int Z);


	/// <summary>
	/// Возвращает угол поворота ориентации
	/// </summary>
	/// <param name="Addr">Адрес ориентации</param>
	/// <returns>Угол поворота</returns>
	double GetBeta([In] long Addr);


	/// <summary>
	/// Устанавливает угол поворота ориентации
	/// </summary>
	/// <param name="Addr">Адрес ориентации</param>
	/// <param name="Beta">Угол поворота</param>
	void SetBeta([In] long Addr,
		[In] double Beta);


	/// <summary>
	/// Возвращает вектор ориентации
	/// </summary>
	/// <param name="Addr">Адрес ориентации</param>
	/// <param name="X">Продольная координата</param>
	/// <param name="Y">Поперечная координата</param>
	/// <param name="Z">Вертикальная координата</param>
	/// <returns>Признак того, что для ориентации используется вектор</returns>
	int GetVector([In] long Addr,
		[Out] out double X,
		[Out] out double Y,
		[Out] out double Z);


	/// <summary>
	/// Устанавливает вектор ориентации
	/// </summary>
	/// <param name="Addr">Адрес ориентации</param>
	/// <param name="X">Продольная координата</param>
	/// <param name="Y">Поперечная координата</param>
	/// <param name="Z">Вертикальная координата</param>
	void SetVector([In] long Addr,
		[In] double X,
		[In] double Y,
		[In] double Z);


	/// <summary>
	/// Возвращает координаты точки ориентации
	/// </summary>
	/// <param name="Addr">Адрес ориентации</param>
	/// <param name="X">Продольная координата</param>
	/// <param name="Y">Поперечная координата</param>
	/// <param name="Z">Вертикальная координата</param>
	/// <returns>Признак того, что для ориентации используется точка</returns>
	int GetCoord([In] long Addr,
		[Out] out double X,
		[Out] out double Y,
		[Out] out double Z);


	/// <summary>
	/// Устанавливает координаты ориентации
	/// </summary>
	/// <param name="Addr">Адрес ориентации</param>
	/// <param name="X">Продольная координата</param>
	/// <param name="Y">Поперечная координата</param>
	/// <param name="Z">Вертикальная координата</param>
	void SetCoord([In] long Addr,
		[In] double X,
		[In] double Y,
		[In] double Z);


	/// <summary>
	/// Возвращает узел ориентации
	/// </summary>
	/// <param name="Addr">Адрес ориентации</param>
	/// <returns>Узел ориентации (при нулевом значении ориентация осуществляется не по узлу)</returns>
	long GetNode([In] long Addr);


	/// <summary>
	/// Устанавливает узел ориентации
	/// </summary>
	/// <param name="Addr">Адрес ориентации</param>
	/// <param name="Node">Адрес узла ориентации</param>
	void SetNode([In] long Addr,
		[In] long Node);

}


/// <summary>
/// Интерфейс списка смещений
/// </summary>
[ComImport, Guid("2D1884AD-692E-452E-9C07-0B297A85CFE5"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IOffsetList
{

	/// <summary>
	/// Возвращает начальное смещение
	/// </summary>
	/// <param name="Addr">Адрес смещения</param>
	/// <returns>Величина смещения</returns>
	double GetStart([In] long Addr);


	/// <summary>
	/// Устанавливает начальное смещение
	/// </summary>
	/// <param name="Addr">Адрес смещения</param>
	/// <param name="Value">Величина смещения</param>
	void SetStart([In] long Addr,
		[In] double Value);


	/// <summary>
	/// Возвращает конечное смещение
	/// </summary>
	/// <param name="Addr">Адрес смещения</param>
	/// <returns>Величина смещения</returns>
	double GetEnd([In] long Addr);


	/// <summary>
	/// Устанавливает конечное смещение
	/// </summary>
	/// <param name="Addr">Адрес смещения</param>
	/// <param name="Value">Величина смещения</param>
	void SetEnd([In] long Addr,
		[In] double Value);

}


/// <summary>
/// Интерфейс списка стержневых элементов
/// </summary>
[ComImport, Guid("427CE2DE-C21C-430F-85AC-4CDC9E213904"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IMemberList
{

	/// <summary>
	/// Возвращает начальный узел
	/// </summary>
	/// <param name="Addr">Адрес стержневого элемента</param>
	/// <returns>Адрес узла</returns>
	long GetStartNode([In] long Addr);


	/// <summary>
	/// Устанавливает начальный узел
	/// </summary>
	/// <param name="Addr">Адрес стержневого элемента</param>
	/// <param name="Node">Адрес узла</param>
	void SetStartNode([In] long Addr,
		[In] long Node);


	/// <summary>
	/// Возвращает конечный узел
	/// </summary>
	/// <param name="Addr">Адрес стержневого элемента</param>
	/// <returns>Адрес узла</returns>
	long GetEndNode([In] long Addr);


	/// <summary>
	/// Устанавливает конечный узел
	/// </summary>
	/// <param name="Addr">Адрес стержневого элемента</param>
	/// <param name="Node">Адрес узла</param>
	void SetEndNode([In] long Addr,
		[In] long Node);


	/// <summary>
	/// Возвращает координаты начала и конца упругой оси элемента
	/// </summary>
	/// <param name="Addr">Адрес стержневого элемента</param>
	/// <param name="Local">Признак определения координат относительно текущей локальной системы координат</param>
	/// <param name="X0">Продольная координата начальной точки</param>
	/// <param name="Y0">Поперечная координата начальной точки</param>
	/// <param name="Z0">Вертикальная координата начальной точки</param>
	/// <param name="X1">Продольная координата конечной точки</param>
	/// <param name="Y1">Поперечная координата конечной точки</param>
	/// <param name="Z1">Вертикальная координата конечной точки</param>
	void GetFlexibleAxisCoord([In] long Addr,
		[In] int Local,
		[Out] out double X0,
		[Out] out double Y0,
		[Out] out double Z0,
		[Out] out double X1,
		[Out] out double Y1,
		[Out] out double Z1);


	/// <summary>
	/// Возвращает материал элемента
	/// </summary>
	/// <param name="Addr">Адрес стержневого элемента</param>
	/// <returns>Адрес материала</returns>
	long GetMaterial([In] long Addr);


	/// <summary>
	/// Устанавливает материал элемента
	/// </summary>
	/// <param name="Addr">Адрес стержневого элемента</param>
	/// <param name="Material">Адрес материала</param>
	void SetMaterial([In] long Addr,
		[In] long Material);


	/// <summary>
	/// Возвращает сечение элемента
	/// </summary>
	/// <param name="Addr">Адрес стержневого элемента</param>
	/// <returns>Адрес сечения</returns>
	long GetSection([In] long Addr);


	/// <summary>
	/// Устанавливает сечение элемента
	/// </summary>
	/// <param name="Addr">Адрес стержневого элемента</param>
	/// <param name="Section">Адрес материала</param>
	void SetSection([In] long Addr,
		[In] long Section);


	/// <summary>
	/// Возвращает ориентацию элемента
	/// </summary>
	/// <param name="Addr">Адрес стержневого элемента</param>
	/// <returns>Адрес ориентации</returns>
	long GetOrientation([In] long Addr);


	/// <summary>
	/// Устанавливает ориентацию элемента
	/// </summary>
	/// <param name="Addr">Адрес стержневого элемента</param>
	/// <param name="Orientation">Адрес ориентации</param>
	void SetOrientation([In] long Addr,
		[In] long Orientation);


	/// <summary>
	/// Возвращает смещение элемента
	/// </summary>
	/// <param name="Addr">Адрес стержневого элемента</param>
	/// <returns>Адрес смещения</returns>
	long GetOffset([In] long Addr);


	/// <summary>
	/// Устанавливает смещение элемента
	/// </summary>
	/// <param name="Addr">Адрес стержневого элемента</param>
	/// <param name="Offset">Адрес смещения</param>
	void SetOffset([In] long Addr,
		[In] long Offset);


	/// <summary>
	/// Возвращает начальное освобождение
	/// </summary>
	/// <param name="Addr">Адрес стержневого элемента</param>
	/// <returns>Маска степеней свободы</returns>
	int GetStartRelease([In] long Addr);


	/// <summary>
	/// Устанавливает начальное освобождение
	/// </summary>
	/// <param name="Addr">Адрес стержневого элемента</param>
	/// <param name="Release">Маска степеней свободы</param>
	void SetStartRelease([In] long Addr,
		[In] int Release);


	/// <summary>
	/// Возвращает конечное освобождение
	/// </summary>
	/// <param name="Addr">Адрес стержневого элемента</param>
	/// <returns>Маска степеней свободы</returns>
	int GetEndRelease([In] long Addr);


	/// <summary>
	/// Устанавливает конечное освобождение
	/// </summary>
	/// <param name="Addr">Адрес стержневого элемента</param>
	/// <param name="Release">Маска степеней свободы</param>
	void SetEndRelease([In] long Addr,
		[In] int Release);


	/// <summary>
	/// Проверяет правильность заданных освобождений
	/// </summary>
	/// <param name="Addr">Адрес стержневого элемента</param>
	/// <returns>1 – освобождения правильные, 0 – освобождения избыточные</returns>
	int CheckRelease([In] long Addr);


	/// <summary>
	/// Возвращает положение начала элемента относительно начала цепочки переменных сечений
	/// </summary>
	/// <param name="Addr">Адрес стержневого элемента</param>
	/// <returns>Положение</returns>
	double GetLocation([In] long Addr);


	/// <summary>
	/// Устанавливает положение начала элемента относительно начала цепочки переменных сечений
	/// </summary>
	/// <param name="Addr">Адрес стержневого элемента</param>
	/// <param name="Value">Положение</param>
	void SetLocation([In] long Addr,
		[In] double Value);


	/// <summary>
	/// Возвращает длину упругой части элемента
	/// </summary>
	/// <param name="Addr">Адрес стержневого элемента</param>
	/// <returns>Длина</returns>
	double GetLength([In] long Addr);


	/// <summary>
	/// Возвращает объём упругой части элемента
	/// </summary>
	/// <param name="Addr">Адрес стержневого элемента</param>
	/// <returns>Объём</returns>
	double GetVolume([In] long Addr);


	/// <summary>
	/// Возвращает вес упругой части элемента
	/// </summary>
	/// <param name="Addr">Адрес стержневого элемента</param>
	/// <returns>Вес</returns>
	double GetWeight([In] long Addr);


	/// <summary>
	/// Возвращает отношение проецированной длины элемента к полной длине
	/// </summary>
	/// <param name="Addr">Адрес стержневого элемента</param>
	/// <param name="Dof">Глобальная ось, нормальная к плоскости проекции</param>
	/// <returns>Отношение</returns>
	double GetProjection([In] long Addr,
		[In] int Dof);

}


/// <summary>
/// Интерфейс списка жёстких тел
/// </summary>
[ComImport, Guid("779BCEC1-0A40-4E80-BB0B-96ED206B64EC"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IRigidBodyList
{

	/// <summary>
	/// Возвращает количество узлов в жёстком теле
	/// </summary>
	/// <param name="Addr">Адрес жёсткого тела</param>
	/// <returns>Количество узлов</returns>
	int GetNodeCount([In] long Addr);


	/// <summary>
	/// Возвращает адрес узла с указанным индексом
	/// </summary>
	/// <param name="Addr">Адрес жёсткого тела</param>
	/// <param name="Index">Индекс узла</param>
	/// <returns>Адрес узла</returns>
	long GetNode([In] long Addr,
		[In] int Index);


	/// <summary>
	/// Устанавливает адрес узла с указанным индексом
	/// </summary>
	/// <param name="Addr">Адрес жёсткого тела</param>
	/// <param name="Index">Индекс узла</param>
	/// <param name="Node">Адрес узла</param>
	void SetNode([In] long Addr,
		[In] int Index,
		[In] long Node);


	/// <summary>
	/// Добавляет узел к жёсткому телу
	/// </summary>
	/// <param name="Addr">Адрес жёсткого тела</param>
	/// <param name="Node">Адрес узла</param>
	void AddNode([In] long Addr,
		[In] long Node);


	/// <summary>
	/// Удаляет узел с указанным индексом
	/// </summary>
	/// <param name="Addr">Адрес жёсткого тела</param>
	/// <param name="Index">Индекс узла</param>
	void DeleteNode([In] long Addr,
		[In] int Index);


	/// <summary>
	/// Возвращает индекс узла в жёстком теле
	/// </summary>
	/// <param name="Addr">Адрес жёсткого тела</param>
	/// <param name="Node">Адрес узла</param>
	/// <returns>Индекс узла</returns>
	int IndexOf([In] long Addr,
		[In] long Node);

}


/// <summary>
/// Интерфейс списка плоских элементов
/// </summary>
[ComImport, Guid("4C350C11-B608-4918-9714-912C78D9533A"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IPlateList
{

	/// <summary>
	/// Возвращает количество узлов пластины
	/// </summary>
	/// <param name="Addr">Адрес пластины</param>
	/// <returns>Количество узлов</returns>
	int GetNodeCount([In] long Addr);


	/// <summary>
	/// Возвращает адрес узла с указанным индексом
	/// </summary>
	/// <param name="Addr">Адрес пластины</param>
	/// <param name="Index">Номер узла</param>
	/// <returns>Адрес узла</returns>
	long GetNode([In] long Addr,
		[In] int Index);


	/// <summary>
	/// Возвращает координаты точек срединной плоскости пластины в узлах
	/// </summary>
	/// <param name="Addr">Адрес пластины</param>
	/// <param name="Local">Признак возврата координат в текущей локальной системе координат</param>
	/// <param name="X1">Продольная координата первого узла</param>
	/// <param name="Y1">Поперечная координата первого узла</param>
	/// <param name="Z1">Вертикальная координата первого узла</param>
	/// <param name="X2">Продольная координата второго узла</param>
	/// <param name="Y2">Поперечная координата второго узла</param>
	/// <param name="Z2">Вертикальная координата второго узла</param>
	/// <param name="X3">Продольная координата третьего узла</param>
	/// <param name="Y3">Поперечная координата третьего узла</param>
	/// <param name="Z3">Вертикальная координата третьего узла</param>
	/// <param name="X4">Продольная координата четвёртого узла</param>
	/// <param name="Y4">Поперечная координата четвёртого узла</param>
	/// <param name="Z4">Вертикальная координата четвёртого узла</param>
	void GetFlexibleSurfaceCoord([In] long Addr,
		[In] int Local,
		[Out] out double X1,
		[Out] out double Y1,
		[Out] out double Z1,
		[Out] out double X2,
		[Out] out double Y2,
		[Out] out double Z2,
		[Out] out double X3,
		[Out] out double Y3,
		[Out] out double Z3,
		[Out] out double X4,
		[Out] out double Y4,
		[Out] out double Z4);


	/// <summary>
	/// Устанавливает адрес узла с указанным индексом
	/// </summary>
	/// <param name="Addr">Адрес пластины</param>
	/// <param name="Index">Номер узла</param>
	/// <param name="Node">Адрес узла</param>
	void SetNode([In] long Addr,
		[In] int Index,
		[In] long Node);


	/// <summary>
	/// Возвращает материал пластины
	/// </summary>
	/// <param name="Addr">Адрес пластины</param>
	/// <returns>Адрес материала</returns>
	long GetMaterial([In] long Addr);


	/// <summary>
	/// Устанавливает материал пластины
	/// </summary>
	/// <param name="Addr">Адрес пластины</param>
	/// <param name="Material">Адрес материала</param>
	void SetMaterial([In] long Addr,
		[In] long Material);


	/// <summary>
	/// Возвращает сечение пластины
	/// </summary>
	/// <param name="Addr">Адрес пластины</param>
	/// <returns>Адрес сечения</returns>
	long GetSection([In] long Addr);


	/// <summary>
	/// Устанавливает сечение пластины
	/// </summary>
	/// <param name="Addr">Адрес пластины</param>
	/// <param name="Section">Адрес сечения</param>
	void SetSection([In] long Addr,
		[In] long Section);


	/// <summary>
	/// Возвращает ориентацию пластины
	/// </summary>
	/// <param name="Addr">Адрес пластины</param>
	/// <returns>Адрес ориентации</returns>
	long GetOrientation([In] long Addr);


	/// <summary>
	/// Устанавливает ориентацию пластины
	/// </summary>
	/// <param name="Addr">Адрес пластины</param>
	/// <param name="Orientation">Адрес ориентации</param>
	void SetOrientation([In] long Addr,
		[In] long Orientation);


	/// <summary>
	/// Возвращает коэффициент неопрятности пластины
	/// </summary>
	/// <param name="Addr">Адрес пластины</param>
	/// <returns>Коэффициент неопрятности</returns>
	double GetLaxity([In] long Addr);


	/// <summary>
	/// Возвращает площадь поверхности пластины
	/// </summary>
	/// <param name="Addr">Адрес пластины</param>
	/// <returns>Площадь поверхности</returns>
	double GetArea([In] long Addr);


	/// <summary>
	/// Возвращает объём пластины
	/// </summary>
	/// <param name="Addr">Адрес пластины</param>
	/// <returns>Объём пластины</returns>
	double GetVolume([In] long Addr);


	/// <summary>
	/// Возвращает вес пластины
	/// </summary>
	/// <param name="Addr">Адрес пластины</param>
	/// <returns>Вес пластины</returns>
	double GetWeight([In] long Addr);


	/// <summary>
	/// Возвращает отношение проецированной площади элемента к полной площади
	/// </summary>
	/// <param name="Addr">Адрес пластины</param>
	/// <param name="Dof">Глобальная ось, нормальная к плоскости проекции</param>
	/// <returns>Отношение</returns>
	double GetProjection([In] long Addr,
		[In] int Dof);

}


/// <summary>
/// Интерфейс списка вантовых элементов
/// </summary>
[ComImport, Guid("5EF96F17-CAC1-4FC5-B754-EC958D7B7C6C"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface ICableList
{

	/// <summary>
	/// Возвращает начальный узел
	/// </summary>
	/// <param name="Addr">Адрес вантового элемента</param>
	/// <returns>Адрес узла</returns>
	long GetStartNode([In] long Addr);


	/// <summary>
	/// Устанавливает начальный узел
	/// </summary>
	/// <param name="Addr">Адрес вантового элемента</param>
	/// <param name="Node">Адрес узла</param>
	void SetStartNode([In] long Addr,
		[In] long Node);


	/// <summary>
	/// Возвращает конечный узел
	/// </summary>
	/// <param name="Addr">Адрес вантового элемента</param>
	/// <returns>Адрес узла</returns>
	long GetEndNode([In] long Addr);


	/// <summary>
	/// Устанавливает конечный узел
	/// </summary>
	/// <param name="Addr">Адрес вантового элемента</param>
	/// <param name="Mode">Адрес узла</param>
	void SetEndNode([In] long Addr,
		[In] long Node);


	/// <summary>
	/// Возвращает координаты начала и конца упругой оси элемента
	/// </summary>
	/// <param name="Addr">Адрес стержневого элемента</param>
	/// <param name="Local">Признак определения координат относительно текущей локальной системы координат</param>
	/// <param name="X0">Продольная координата начальной точки</param>
	/// <param name="Y0">Поперечная координата начальной точки</param>
	/// <param name="Z0">Вертикальная координата начальной точки</param>
	/// <param name="X1">Продольная координата конечной точки</param>
	/// <param name="Y1">Поперечная координата конечной точки</param>
	/// <param name="Z1">Вертикальная координата конечной точки</param>
	void GetFlexibleAxisCoord([In] long Addr,
		[In] int Local,
		[Out] out double X0,
		[Out] out double Y0,
		[Out] out double Z0,
		[Out] out double X1,
		[Out] out double Y1,
		[Out] out double Z1);


	/// <summary>
	/// Возвращает материал элемента
	/// </summary>
	/// <param name="Addr">Адрес вантового элемента</param>
	/// <returns>Адрес материала</returns>
	long GetMaterial([In] long Addr);


	/// <summary>
	/// Устанавливает материал элемента
	/// </summary>
	/// <param name="Addr">Адрес вантового элемента</param>
	/// <param name="Material">Адрес материала</param>
	void SetMaterial([In] long Addr,
		[In] long Material);


	/// <summary>
	/// Возвращает смещение элемента
	/// </summary>
	/// <param name="Addr">Адрес вантового элемента</param>
	/// <returns>Адрес смещения</returns>
	long GetOffset([In] long Addr);


	/// <summary>
	/// Устанавливает смещение элемента
	/// </summary>
	/// <param name="Addr">Адрес вантового элемента</param>
	/// <param name="Offset">Адрес смещения</param>
	void SetOffset([In] long Addr,
		[In] long Offset);


	/// <summary>
	/// Возвращает площадь одной пряди вантового элемента
	/// </summary>
	/// <param name="Addr">Адрес вантового элемента</param>
	/// <returns>Площадь пряди</returns>
	double GetStrandArea([In] long Addr);


	/// <summary>
	/// Устанавливает площадь одной пряди вантового элемента
	/// </summary>
	/// <param name="Addr">Адрес вантового элемента</param>
	/// <param name="Value">Площадь пряди</param>
	void SetStrandArea([In] long Addr,
		[In] double Value);


	/// <summary>
	/// Возвращает количество прядей  вантового элемента
	/// </summary>
	/// <param name="Addr">Адрес вантового элемента</param>
	/// <returns>Количество прядей</returns>
	int GetStrandCount([In] long Addr);


	/// <summary>
	/// Устанавливает количество прядей вантового элемента
	/// </summary>
	/// <param name="Addr">Адрес вантового элемента</param>
	/// <param name="Count">Количество прядей</param>
	void SetStrandCount([In] long Addr,
		[In] int Count);


	/// <summary>
	/// Возвращает усилие в вантовом элементе
	/// </summary>
	/// <param name="Addr">Адрес вантового элемента</param>
	/// <returns>Усилие</returns>
	double GetForce([In] long Addr);


	/// <summary>
	/// Устанавливает усилие в вантовом элементе
	/// </summary>
	/// <param name="Addr">Адрес вантового элемента</param>
	/// <param name="Value">Усилие</param>
	void SetForce([In] long Addr,
		[In] double Value);


	/// <summary>
	/// Возвращает вытяжку вантового элемента
	/// </summary>
	/// <param name="Addr">Адрес вантового элемента</param>
	/// <returns>Вытяжка</returns>
	double GetStretch([In] long Addr);


	/// <summary>
	/// Устанавливает вытяжку вантового элемента
	/// </summary>
	/// <param name="Addr">Адрес вантового элемента</param>
	/// <param name="Value">Вытяжка</param>
	void SetStretch([In] long Addr,
		[In] double Value);


	/// <summary>
	/// Возвращает эффективную площадь вантового элемента
	/// </summary>
	/// <param name="Addr">Адрес вантового элемента</param>
	/// <returns>Эффективная площадь (ф-ла Эрнста)</returns>
	double GetAef([In] long Addr);


	/// <summary>
	/// Возвращает эффективный момент инерции вантового элемента
	/// </summary>
	/// <param name="Addr">Адрес вантового элемента</param>
	/// <returns>Эффективный момент инерции (ф-ла В.И. Сливкера)</returns>
	double GetIef([In] long Addr);


	/// <summary>
	/// Возвращает отношение длины заготовки вантового элемента к расстоянию между узлами
	/// </summary>
	/// <param name="Addr">Адрес вантового элемента</param>
	/// <returns>Отношение длин</returns>
	double GetLf([In] long Addr);


	/// <summary>
	/// Возвращает разрывное усилие в вантовом элементе
	/// </summary>
	/// <param name="Addr">Адрес вантового элемента</param>
	/// <returns>Разрывное усилие</returns>
	double GetGuts([In] long Addr);


	/// <summary>
	/// Возвращает полную площадь вантового элемента
	/// </summary>
	/// <param name="Addr">Адрес вантового элемента</param>
	/// <returns>Площадь</returns>
	double GetAx([In] long Addr);


	/// <summary>
	/// Возвращает длину упругой части вантового элемента
	/// </summary>
	/// <param name="Addr">Адрес вантового элемента</param>
	/// <returns>Длина</returns>
	double GetLength([In] long Addr);


	/// <summary>
	/// Возвращает объём упругой части вантового элемента
	/// </summary>
	/// <param name="Addr">Адрес вантового элемента</param>
	/// <returns>Объём</returns>
	double GetVolume([In] long Addr);


	/// <summary>
	/// Возвращает вес упругой части вантового элемента
	/// </summary>
	/// <param name="Addr">Адрес вантового элемента</param>
	/// <returns>Вес</returns>
	double GetWeight([In] long Addr);


	/// <summary>
	/// Возвращает отношение проецированной длины элемента к полной длине
	/// </summary>
	/// <param name="Addr">Адрес вантового элемента</param>
	/// <param name="Dof">Глобальная ось, нормальная к плоскости проекции</param>
	/// <returns>Отношение</returns>
	double GetProjection([In] long Addr,
		[In] int Dof);

}


/// <summary>
/// Интерфейс списка пружин (балансиров)
/// </summary>
[ComImport, Guid("EDE1CD2E-1FAE-4DCB-ACF2-45047D220098"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface ISpringList
{

	/// <summary>
	/// Возвращает количество узлов пружины
	/// </summary>
	/// <param name="Addr">Адрес пружины</param>
	/// <returns>Количество узлов</returns>
	int GetNodeCount([In] long Addr);


	/// <summary>
	/// Возвращает адрес узла с указанным индексом
	/// </summary>
	/// <param name="Addr">Адрес пружины</param>
	/// <param name="Index">Номер узла</param>
	/// <returns>Адрес узла</returns>
	long GetNode([In] long Addr,
		[In] int Index);


	/// <summary>
	/// Устанавливает адрес узла с указанным индексом
	/// </summary>
	/// <param name="Addr">Адрес пружины</param>
	/// <param name="Index">Номер узла</param>
	/// <param name="Node">Адрес узла</param>
	void SetNode([In] long Addr,
		[In] int Index,
		[In] long Node);


	/// <summary>
	/// Добавляет узел к пружине
	/// </summary>
	/// <param name="Addr">Адрес пружины</param>
	/// <param name="Node">Адрес узла</param>
	void AddNode([In] long Addr,
		[In] long Node);


	/// <summary>
	/// Удаляет узел с указанным индексом
	/// </summary>
	/// <param name="Addr">Адрес пружины</param>
	/// <param name="Index">Номер узла</param>
	void DeleteNode([In] long Addr,
		[In] int Index);


	/// <summary>
	/// Возвращает индекс узла пружины
	/// </summary>
	/// <param name="Addr">Адрес пружины</param>
	/// <param name="Node">Адрес узла</param>
	/// <returns>Индекс узла</returns>
	int IndexOf([In] long Addr,
		[In] long Node);


	/// <summary>
	/// Возвращает жёсткость пружины
	/// </summary>
	/// <param name="Addr">Адрес пружины</param>
	/// <returns>Жёсткость</returns>
	double GetStiffness([In] long Addr);


	/// <summary>
	/// Устанавливает жёсткость пружины
	/// </summary>
	/// <param name="Addr">Адрес пружины</param>
	/// <param name="Value">Жёсткость</param>
	void SetStiffness([In] long Addr,
		[In] double Value);


	/// <summary>
	/// Возвращает степень свободы пружины
	/// </summary>
	/// <param name="Addr">Адрес пружины</param>
	/// <returns>Маска степени свободы</returns>
	int GetDof([In] long Addr);


	/// <summary>
	/// Устанавливает степень свободы пружины
	/// </summary>
	/// <param name="Addr">Адрес пружины</param>
	/// <param name="Dof">Маска степени свободы</param>
	void SetDof([In] long Addr,
		[In] int Dof);


	/// <summary>
	/// Возвращает локальную систему координат пружины
	/// </summary>
	/// <param name="Addr">Адрес пружины</param>
	/// <returns>Адрес системы координат</returns>
	long GetCS([In] long Addr);


	/// <summary>
	/// Устанавливает локальную систему координат пружины
	/// </summary>
	/// <param name="Addr">Адрес пружины</param>
	/// <param name="CS">Адрес системы координат</param>
	void SetCS([In] long Addr,
		[In] long CS);

}


/// <summary>
/// Интерфейс списка суперэлементов
/// </summary>
[ComImport, Guid("9629F831-8197-47AC-9754-F631F5F6835A"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface ISuperelementList
{

	/// <summary>
	/// Добавляет степени свободы узла к суперэлементу
	/// </summary>
	/// <param name="Addr">Адрес суперэлемента</param>
	/// <param name="Node">Адес узла</param>
	/// <param name="Dof">Маска степеней свободы</param>
	void AddDofs([In] long Addr,
		[In] long Node,
		[In] int Dof);


	/// <summary>
	/// Удаляет степени свободы узла из суперэлемента
	/// </summary>
	/// <param name="Addr">Адрес суперэлемента</param>
	/// <param name="Node">Адес узла</param>
	/// <param name="Dof">Маска степеней свободы</param>
	void RemoveDofs([In] long Addr,
		[In] long Node,
		[In] int Dof);


	/// <summary>
	/// Возвращает количество степеней свободы суперэлемента
	/// </summary>
	/// <param name="Addr">Адрес суперэлемента</param>
	/// <returns>Количество степеней свободы</returns>
	int GetDofCount([In] long Addr);


	/// <summary>
	/// Возвращает адрес узла и маску степени свободы с указанным индексом
	/// </summary>
	/// <param name="Addr">Адрес суперэлемента</param>
	/// <param name="Index">Индекс степени свободы</param>
	/// <param name="Node">Адес узла</param>
	/// <param name="Dof">Маска степеней свободы</param>
	void GetDofNodeMask([In] long Addr,
		[In] int Index,
		[Out] out long Node,
		[Out] out int Mask);


	/// <summary>
	/// Возвращает индекс степени свободы для узла и маски степени свободы
	/// </summary>
	/// <param name="Addr">Адрес суперэлемента</param>
	/// <param name="Node">Адес узла</param>
	/// <param name="Dof">Маска степеней свободы</param>
	/// <returns>Индекс степени свободы (для несуществующих возвращается -1)</returns>
	int GetDofIndex([In] long Addr,
		[In] long Node,
		[In] int Dof);


	/// <summary>
	/// Возвращает барcкий суперэлемент
	/// </summary>
	/// <param name="Addr">Адрес суперэлемента</param>
	/// <returns>Адрес барского суперэлемента</returns>
	long GetMaster([In] long Addr);


	/// <summary>
	/// Устанавливает барин суперэлемент
	/// </summary>
	/// <param name="Addr">Адрес суперэлемента</param>
	/// <param name="Master">Адрес барского суперэлемента</param>
	void SetMaster([In] long Addr,
		[In] long Master);


	/// <summary>
	/// Создаёт суперэлемент из элементов группы
	/// </summary>
	/// <param name="Addr">Адрес суперэлемента</param>
	/// <param name="Group">Группа объектов</param>
	void Build([In] long Addr,
		[In, MarshalAs(UnmanagedType.Interface)] IGroup Group);


	/// <summary>
	/// Загружает матрицу жёсткости суперэлемента из файла
	/// </summary>
	/// <param name="Addr">Адрес суперэлемента</param>
	/// <param name="FileName">Имя файла (формат определяется автоматически)</param>
	void LoadMatrixFromFile([In] long Addr,
		[In] string FileName);


	/// <summary>
	/// Сохраняет матрицу жёсткости в файл
	/// </summary>
	/// <param name="Addr">Адрес суперэлемента</param>
	/// <param name="FileName">Имя файла (расширение .txt определяет текстовый формат, .coo – формат COO, остальные – двоичный)</param>
	void SaveMatrixToFile([In] long Addr,
		[In] string FileName);


	/// <summary>
	/// Возвращает значение ячейки матрицы жёсткости
	/// </summary>
	/// <param name="Addr">Адрес суперэлемента</param>
	/// <param name="Row">Номер строки</param>
	/// <param name="Col">Номер столбца</param>
	/// <returns>Значение ячейки</returns>
	double GetCell([In] long Addr,
		[In] int Row,
		[In] int Col);


	/// <summary>
	/// Устанавливает значение ячейки матрицы жёсткости
	/// </summary>
	/// <param name="Addr">Адрес суперэлемента</param>
	/// <param name="Row">Номер строки</param>
	/// <param name="Col">Номер столбца</param>
	/// <param name="Value">Значение ячейки</param>
	void SetCell([In] long Addr,
		[In] int Row,
		[In] int Col,
		[In] double Value);


	/// <summary>
	/// Возвращает интерфейс табличных данных для матрицы жёсткости суперэлемента
	/// </summary>
	/// <param name="Addr">Адрес суперэлемента</param>
	/// <returns>Интерфейс табличных данных</returns>
	IGridDataSource GetGridDataSource([In] long Addr);

}


/// <summary>
/// Интерфейс списка наборов объектов (именованных групп)
/// </summary>
[ComImport, Guid("E151FD07-2E03-4F48-AB1D-0BC13FEC6E07"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface INamedGroupList
{

	/// <summary>
	/// Возвращает указатель на интерфейс группы набора объектов
	/// </summary>
	/// <param name="Addr">Адрес набора объектов</param>
	/// <returns>Интерфейс группы</returns>
	IGroup GetGroup([In] long Addr);

}


/// <summary>
/// Интерфейс списка загружений
/// </summary>
[ComImport, Guid("F46FDE5B-03FA-4321-B0C6-6CB2149C9464"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface ILoadingList
{

	/// <summary>
	/// Возвращает описание загружения
	/// </summary>
	/// <param name="Addr">Адрес загружения</param>
	/// <returns>Описание загружения</returns>
	string GetDescription([In] long Addr);


	/// <summary>
	/// Устанавливает описание загружения
	/// </summary>
	/// <param name="Addr">Адрес загружения</param>
	/// <param name="Description">Описание загружения</param>
	void SetDescription([In] long Addr,
		[In] string Description);


	/// <summary>
	/// Возвращает общий множитель для величин воздействий загружения
	/// </summary>
	/// <param name="Addr">Адрес загружения</param>
	/// <returns>Множитель</returns>
	double GetFactor([In] long Addr);


	/// <summary>
	/// Устанавливает общий множитель для величин воздействий загружения
	/// </summary>
	/// <param name="Addr">Адрес загружения</param>
	/// <param name="Factor">Множитель</param>
	void SetFactor([In] long Addr,
		[In] double Factor);


	/// <summary>
	/// Возвращает главный вектор и главный момент всех воздействий загружения
	/// </summary>
	/// <param name="Addr">Адрес загружения</param>
	/// <param name="Finish">Интерфейс обратного вызова, получающий результат в конце асинхронного процесса</param>
	void GetResultant([In] long Addr,
		[In, MarshalAs(UnmanagedType.Interface)] IFinishEvent Finish);


	/// <summary>
	/// Добавляет к загружению воздействия другого загружения с указанным коэффициентом
	/// </summary>
	/// <param name="Addr">Адрес загружения</param>
	/// <param name="Source">Адрес загружения-источника</param>
	/// <param name="Factor">Коэффициент</param>
	/// <param name="Finish">Интерфейс обратного вызова, получающий результат в конце асинхронного процесса</param>
	void AddLoadingActions([In] long Addr,
		[In] long Source,
		[In] double Factor,
		[In, MarshalAs(UnmanagedType.Interface)] IFinishEvent Finish);


	/// <summary>
	/// Очищает все воздействия загружения
	/// </summary>
	/// <param name="Addr">Адрес загружения</param>
	void Clear([In] long Addr);


	/// <summary>
	/// Добавляет воздействие к загружению
	/// </summary>
	/// <param name="Addr">Адрес загружения</param>
	/// <param name="ActionName">Имя класса воздействия</param>
	/// <param name="ClassName">Имя класса объектов</param>
	/// <param name="ItemList">Список объектов</param>
	/// <param name="Value">Строковое представление массива параметров воздействия</param>
	/// <param name="Dof">Cтепень свободы воздействия</param>
	/// <param name="CS">Адрес системы координат воздействия</param>
	void AddActions([In] long Addr,
		[In] string ActionName,
		[In] string ClassName,
		[In] string ItemList,
		[In] string Value,
		[In] int Dof,
		[In] long CS);


	/// <summary>
	/// Возвращает количество воздействий загружения
	/// </summary>
	/// <param name="Addr">Адрес загружения</param>
	/// <returns>Количество воздействий</returns>
	int GetActionCount([In] long Addr);


	/// <summary>
	/// Возвращает адрес воздействия по индексу
	/// </summary>
	/// <param name="Addr">Адрес загружения</param>
	/// <param name="Index">Индекс воздействия</param>
	/// <returns>Адрес воздействия</returns>
	long GetActionHandle([In] long Addr,
		[In] int Index);


	/// <summary>
	/// Перечисляет воздействия загружения
	/// </summary>
	/// <param name="Addr">Адрес загружения</param>
	/// <param name="Enum">Интерфейс обратного вызова</param>
	void EnumActions([In] long Addr,
		[In, MarshalAs(UnmanagedType.Interface)] ILongEnum Enum);


	/// <summary>
	/// Возвращает данные воздействия (для неактивных воздействий возвращается нулевые данные)
	/// </summary>
	/// <param name="Action">Адрес воздействия</param>
	/// <param name="ActionName">Имя воздействия</param>
	/// <param name="Item">Объект воздействия</param>
	/// <param name="Dof">Степень свободы воздействия</param>
	/// <param name="CS">Система координат воздействия</param>
	/// <param name="Value">Строковое представление массива данных воздействия</param>
	void GetActionData([In] long Action,
		[Out] out string ActionName,
		[Out] out long Item,
		[Out] out int Dof,
		[Out] out long CS,
		[Out] out string Value);


	/// <summary>
	/// Устанавливает данные воздействия
	/// </summary>
	/// <param name="Action">Адрес воздействия</param>
	/// <param name="Item">Объект воздействия</param>
	/// <param name="Dof">Степень свободы воздействия</param>
	/// <param name="CS">Система координат воздействия</param>
	/// <param name="Value">Строковое представление массива данных воздействия</param>
	void SetActionData([In] long Action,
		[In] long Item,
		[In] int Dof,
		[In] long CS,
		[In] string Value);


	/// <summary>
	/// Помечает воздействие как активное или неактивное
	/// </summary>
	/// <param name="Action">Адрес воздействия</param>
	/// <param name="Flag">Признак активности/неактивности воздействия</param>
	void EnableAction([In] long Action,
		[In] int Flag);


	/// <summary>
	/// Проверяет активность воздействия
	/// </summary>
	/// <param name="Action">Адрес воздействия</param>
	/// <returns>Признак активности</returns>
	int IsActionEnabled([In] long Action);


	/// <summary>
	/// Добавляет точечное силовое воздействие
	/// </summary>
	/// <param name="Addr">Адрес загружения</param>
	/// <param name="Args">Аргументы X, Y, Z, PX, PY, PZ, DX, DY, DZ (в строковом виде, разделённые пробелами)</param>
	/// <param name="Eps">Точность определения точки приложения воздействия</param>
	/// <param name="Finish">Интерфейс обратного вызова, вызываемый по окончании асинхронного процесса</param>
	void PointForce([In] long Addr,
		[In] string Args,
		[In] double Eps,
		[In, MarshalAs(UnmanagedType.Interface)] IFinishEvent Finish);


	/// <summary>
	/// Добавляет точечное силовое воздействие
	/// </summary>
	/// <param name="Addr">Адрес загружения</param>
	/// <param name="Args">Аргументы X, Y, Z, MX, MY, MZ (в строковом виде, разделённые пробелами)</param>
	/// <param name="Eps">Точность определения точки приложения воздействия</param>
	/// <param name="Finish">Интерфейс обратного вызова, вызываемый по окончании асинхронного процесса</param>
	void PointMoment([In] long Addr,
		[In] string Args,
		[In] double Eps,
		[In, MarshalAs(UnmanagedType.Interface)] IFinishEvent Finish);


	/// <summary>
	/// Добавляет точечное силовое воздействие
	/// </summary>
	/// <param name="Addr">Адрес загружения</param>
	/// <param name="Args">Аргументы X0, Y0, Z0, PX0, PY0, PZ0, DX0, DY0, DZ0, X1, Y1, Z1, PX1, PY1, PZ1, DX1, DY1, DZ1 (в строковом виде, разделённые пробелами)</param>
	/// <param name="Eps">Точность определения точки приложения воздействия</param>
	/// <param name="Finish">Интерфейс обратного вызова, вызываемый по окончании асинхронного процесса</param>
	void LineForce([In] long Addr,
		[In] string Args,
		[In] double Eps,
		[In, MarshalAs(UnmanagedType.Interface)] IFinishEvent Finish);


	/// <summary>
	/// Добавляет точечное силовое воздействие
	/// </summary>
	/// <param name="Addr">Адрес загружения</param>
	/// <param name="Args">Аргументы  X0, Y0, Z0, MX0, MY0, MZ0, X1, Y1, Z1, MX1, MY1, MZ1 (в строковом виде, разделённые пробелами)</param>
	/// <param name="Eps">Точность определения точки приложения воздействия</param>
	/// <param name="Finish">Интерфейс обратного вызова, вызываемый по окончании асинхронного процесса</param>
	void LineMoment([In] long Addr,
		[In] string Args,
		[In] double Eps,
		[In, MarshalAs(UnmanagedType.Interface)] IFinishEvent Finish);


	/// <summary>
	/// Подготавливает массив отображаемых силовых воздействий
	/// </summary>
	/// <param name="Addr">Адрес загружения</param>
	/// <param name="Finish">Интерфейс обратного вызова, вызываемый по окончании асинхронного процесса</param>
	void ReadyForces([In] long Addr,
		[In, MarshalAs(UnmanagedType.Interface)] IFinishEvent Finish);


	/// <summary>
	/// Перебирает отображаемые силовые воздействия, подготовленные ранее методом ReadyForces
	/// </summary>
	/// <param name="Enum">Указатель на интерфейс обратного вызова,
	///   асинхронно получающий массив вещественных чисел следующей структуры:
	///         1 число - количество сосредоточенных сил
	///         3 числа: координаты точки
	///         3 числа: вектор сил
	///         3 числа: вектор моментов
	///         ...
	///         1 число - количество распределённых нагрузок
	///         3 числа: координаты точки начала нагрузки
	///         3 числа: координаты точки конца нагрузки
	///         3 числа: вектор сил в начале нагрузки
	///         3 числа: вектор моментов в начале нагрузки
	///         3 числа: вектор сил в начале нагрузки
	///         3 числа: вектор моментов в конце нагрузки
	///         ...
	///         1 число – количество поверхностных нагрузок
	///         3 числа: координаты первой точки
	///         3 числа: координаты второй точки
	///         3 числа: координаты третьей точки
	///         3 числа: вектор сил
	///         3 числа: вектор моментов</param>
	void EnumForces([In, MarshalAs(UnmanagedType.Interface)] IRealEnum Enum);


	/// <summary>
	/// Возвращает указатель на интерфейс табличных данных для воздействий загружения
	/// </summary>
	/// <param name="Addr">Адрес загружения</param>
	/// <returns>Указатель на интерфейс</returns>
	IGridDataSource GetGridDataSource([In] long Addr);

}


/// <summary>
/// Основной интерфейс для взаимодействия скриптового движка с клиентским приложением
/// </summary>
[ComImport, Guid("45B0A83A-4A3D-42FF-BDEA-D8891729D31D"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IScriptClient
{

	/// <summary>
	/// Вызывается в начале работы скрипта
	/// </summary>
	void OnStart();


	/// <summary>
	/// Вызывается в конце процедуры синтаксического разбора текста скрипта
	/// </summary>
	void OnEndParsing();


	/// <summary>
	/// Вызывается при возникновении ошибки при разборе или выполнении скрипта
	/// </summary>
	/// <param name="Line">Номер строки</param>
	/// <param name="Pos">Номер символа в строке</param>
	/// <param name="FileName">Имя файла</param>
	/// <param name="Code">Код ошибки</param>
	/// <param name="Message">Сообщение об ошибке</param>
	void OnError([In] int Line,
		[In] int Pos,
		[In] string FileName,
		[In] int Code,
		[In] string Message);


	/// <summary>
	/// Блокирует вычислительный поток и отображает на экране
	/// модальное диалоговое окно с полученным сообщением
	/// </summary>
	/// <param name="Message">отображаемое сообщение</param>
	/// <param name="Type">тип диалогового сообщения. Значение задаётся так же как для функции WinAPI MessageBox</param>
	/// <returns>Код нажатой пользователем кнопки (такой же, как результат работы MessageBox)</returns>
	int DisplayMessage([In] string Message,
		[In] int Type);


	/// <summary>
	/// Вызывается в начале записи в журнал отладочных событий скрипта
	/// </summary>
	/// <param name="FileName">Имя файла журнала</param>
	void OnStartLog([In] string FileName);


	/// <summary>
	/// Вызывается при записи в журнал отладочных событий скрипта
	/// </summary>
	/// <param name="Message">Текст, записываемый в журнал отладочных событий</param>
	void OnWriteLog([In] string Message);


	/// <summary>
	/// Вызывается при завершении записи в журнал отладочных событий скрипта
	/// </summary>
	void OnEndLog();


	/// <summary>
	/// Вызывается при записи в поток вывода скрипта
	/// </summary>
	/// <param name="Text">Текст, записываемый в поток вывода</param>
	void OnWrite([In] string Text);


	/// <summary>
	/// Вызывается при переходе скрипта в режим ожидания события
	/// </summary>
	void OnStartWaiting();


	/// <summary>
	/// Вызывается при переходе скрипта в нормальный режим после ожидания события
	/// </summary>
	void OnEndWaiting();


	/// <summary>
	/// Вызывается при каждом переходе скрипта на очередную команду в режиме отладки.
	/// </summary>
	/// <param name="Line">Текущая строка исходного текста</param>
	/// <param name="Pos">Текущий символ исходного текста</param>
	/// <param name="FileName">Текущий файл исходного текста</param>
	/// <returns>0 – продолжение работы, 1 – прекращение, 2 – остановка до вызова IModel::ResumeThread</returns>
	int OnDebugTrace([In] int Line,
		[In] int Pos,
		[In] string FileName);


	/// <summary>
	/// Вызывается при приостановке работы скрипта
	/// </summary>
	/// <param name="Line">Текущая строка исходного текста</param>
	/// <param name="Pos">Текущий символ исходного текста</param>
	/// <param name="FileName">Текущий файл исходного текста</param>
	void OnSuspended([In] int Line,
		[In] int Pos,
		[In] string FileName);


	/// <summary>
	/// Вызывается при восстановлении работы скрипта
	/// </summary>
	void OnResumed();


	/// <summary>
	/// Вызывается при переходе скрипта в режим расчёта
	/// </summary>
	void OnStartAnalysis();


	/// <summary>
	/// Вызывается при завершении режима расчёта
	/// </summary>
	void OnEndAnalysis();


	/// <summary>
	/// Вызывается при завершении работы скрипта
	/// </summary>
	void OnFinish();

}


/// <summary>
/// Интерфейс многомерного массива
/// </summary>
[ComImport, Guid("3372A0A9-2652-432B-A7BE-60EAE30AAD11"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IMapObject
{

	/// <summary>
	/// Возвращает имя файла карты
	/// </summary>
	/// <returns>Имя файла</returns>
	string GetFileName();


	/// <summary>
	/// Возвращает количество уровней карты
	/// </summary>
	/// <returns>Количество уровней
	int GetDepth();


	/// <summary>
	/// Возвращает размер уровня карты
	/// </summary>
	/// <param name="Index">Номер уровня</param>
	/// <returns>Размер уровня карты</returns>
	int GetLevelSize([In] int Index);


	/// <summary>
	/// Возвращает имя идентификатора уровня
	/// </summary>
	/// <param name="Level">Номер уровня</param>
	/// <param name="Index">Номер идентификатора</param>
	/// <returns>Имя идентификатора</returns>
	string GetNameByIndex([In] int Level,
		[In] int Index);


	/// <summary>
	/// Возвращет индекс идентификатора в указанном уровне
	/// </summary>
	/// <param name="Level">Номер уровня</param>
	/// <param name="Name">Имя идентификатора</param>
	/// <returns>Индекс идентификатора</returns>
	int GetIndexByName([In] int Level,
		[In] string Name);


	/// <summary>
	/// Возвращает полное количество ячеек
	/// </summary>
	/// <returns>Количество ячеек</returns>
	int GetSize();


	/// <summary>
	/// Возвращает значение ячейки
	/// </summary>
	/// <param name="Offset">Смещение ячейки от начала карты</param>
	/// <returns>Значение ячейки</returns>
	double GetCell([In] int Offset);


	/// <summary>
	/// Устанавливает значение ячейки
	/// </summary>
	/// <param name="Offset">Смещение ячейки от начала карты</param>
	/// <param name="Value">Значение ячейки</param>
	void SetCell([In] int Offset,
		[In] double Value);


	/// <summary>
	/// Возвращает текущее смещение ячейки
	/// </summary>
	/// <returns>Смещение ячейки</returns>
	int GetCurrentOffset();


	/// <summary>
	/// Устанавливает текущий индекс в указанном уровне карты
	/// </summary>
	/// <param name="Level">Номер уровня</param>
	/// <param name="Index">Индекс ячейки</param>
	void SetCurrentOffset([In] int Level,
		[In] int Index);


	/// <summary>
	/// Возвращает значение текущей ячейки
	/// </summary>
	/// <returns>Значение текущей ячейки</returns>
	double GetCurrentCell();


	/// <summary>
	/// Устанавливает значение текущей ячейки
	/// </summary>
	/// <param name="Value">Значение ячейки</param>
	void SetCurrentCell([In] double Value);


	/// <summary>
	/// Возвращает плоскость карты в виде массива
	/// </summary>
	/// <param name="RowLevel">Уровень карты, ячейки которого будут располагаться в строках результирующего массива</param>
	/// <param name="Rows">Имена идентификаторов уровня RowLevel (разделённые пробелами)</param>
	/// <param name="ColLevel">уровень карты, ячейки которого будут располагаться в столбцах результирующего массива</param>
	/// <param name="Cols">имена идентификаторов уровня ColLevel</param>
	/// <param name="OtherLevels">имена идентификаторов других уровней карты</param>
	/// <param name="Client">Интерфейс клиента</param>
	void GetPlane([In] int RowLevel,
		[In] string Rows,
		[In] int ColLevel,
		[In] string Cols,
		[In] string OtherLevels,
		[In, MarshalAs(UnmanagedType.Interface)] IMapClient Client);


	/// <summary>
	/// Возвращает плоскость карты в виде листа таблицы
	/// </summary>
	/// <param name="RowLevel">Уровень карты, ячейки которого будут располагаться в строках результирующего массива</param>
	/// <param name="Rows">Имена идентификаторов уровня RowLevel (разделённые пробелами)</param>
	/// <param name="ColLevel">уровень карты, ячейки которого будут располагаться в столбцах результирующего массива</param>
	/// <param name="Cols">имена идентификаторов уровня ColLevel</param>
	/// <param name="OtherLevels">имена идентификаторов других уровней карты</param>
	/// <param name="Client">Интерфейс клиента</param>
	void GetTable([In] string SheetName,
		[In] int RowLevel,
		[In] string Rows,
		[In] int ColLevel,
		[In] string Cols,
		[In] string OtherLevels,
		[In, MarshalAs(UnmanagedType.Interface)] IMapClient Client);


	/// <summary>
	/// Создаёт комбинированную карту (см. документацию по скриптовому языку)
	/// </summary>
	/// <param name="FileName">Имя файла результирующей карты</param>
	/// <param name="Level">Номер комбинируемого уровня</param>
	/// <param name="Exp">Выражения для комбинирования (разделённые символами CRLF)</param>
	/// <param name="Client">Интерфейс клиента</param>
	void Combine([In] string FileName,
		[In] int Level,
		[In] string Exp,
		[In, MarshalAs(UnmanagedType.Interface)] IMapClient Client);


	/// <summary>
	/// Создаёт объединённую карту
	/// </summary>
	/// <param name="FileName">Имя файла для результирующей карты</param>
	/// <param name="b">Вторая карта</param>
	/// <param name="Client">Интерфейс клиента</param>
	void Merge([In] string FileName,
		[In, MarshalAs(UnmanagedType.Interface)] IMapObject b,
		[In, MarshalAs(UnmanagedType.Interface)] IMapClient Client);


	/// <summary>
	/// Решает задачу оптимизации (поиска коэффициентов)
	/// </summary>
	/// <param name="Conditions">Разделённые символами CRLF условия подбора</param>
	/// <param name="Level">Уровень карты, для ячеек которого подбираются коэффициенты</param>
	/// <param name="Loads">Разделённые символами CRLF идентификаторы заданного уровня</param>
	/// <param name="Client">Интерфейс клиента для вызова метода FinishArray при завершении асинхронного процесса</param>
	void Optimize([In] string Conditions,
		[In] int Level,
		[In] string Loads,
		[In, MarshalAs(UnmanagedType.Interface)] IMapClient Client);


	/// <summary>
	/// Строит огибающую по данным карты
	/// </summary>
	/// <param name="Name">Наименование уровня LoadLevel, отображаемое на листах</param>
	/// <param name="FactorLevel">Номер уровня, по ячейкам которого производится поиск максимального и минимального значений</param>
	/// <param name="Factors">Имена идентификаторов уровня FactorLevel для которых строятся огибающие и находятся соответствующие значения других факторов (если идентификатор начинается с символа # – он участвует только в выборке соответствующих)</param>
	/// <param name="LoadLevel">Номер уровня в котором находятся ячейки факторов, участвующие в выборке</param>
	/// <param name="Loads">Идентификаторы уровня LoadLevel, участвующие в выборке</param>
	/// <param name="Items">Массив идентификаторов остальных уровней карты (уровни в каждом идентификаторе разделяются знаками ::</param>
	/// <param name="Client">Интерфейс клиента</param>
	void Envelope([In] string Name,
		[In] int FactorLevel,
		[In] string Factors,
		[In] int LoadLevel,
		[In] string Loads,
		[In] string Items,
		[In, MarshalAs(UnmanagedType.Interface)] IMapClient Client);


	/// <summary>
	/// Возвращает массив данных для построения эпюр в стержневых элементах
	/// </summary>
	/// <param name="Model">Интерфейс модели</param>
	/// <param name="Members">Массив идентификаторов стержневых элементов</param>
	/// <param name="Factor">Имя фактора</param>
	/// <param name="Load">Имя загружения</param>
	/// <param name="Client">Интерфейс клиента</param>
	void Diagram([In, MarshalAs(UnmanagedType.Interface)] IModel Model,
		[In] string Members,
		[In] string Factor,
		[In] string Load,
		[In, MarshalAs(UnmanagedType.Interface)] IMapClient Client);


	/// <summary>
	/// Завершает работу с картой
	/// </summary>
	void Finish();

}


/// <summary>
/// Интерфейс обратного вызова для объединения ячеек карт
/// </summary>
[ComImport, Guid("AB3D9A02-C0E4-46DD-8219-E13DCC0E2031"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IMapClient
{

	/// <summary>
	/// Возвращает значение объединённой ячейки. Используется в функции объединения карт
	/// </summary>
	/// <param name="a">Значение ячейки первой карты</param>
	/// <param name="b">Значение ячейки второй карты</param>
	/// <returns>Объединённое значение</returns>
	double Merge([In] double a,
		[In] double b);


	/// <summary>
	/// Вызывается по завершению асинхронного процесса объединения или комбинирования
	/// </summary>
	/// <param name="Code">Код ошибки</param>
	/// <param name="Message">Сообщение об ошибке</param>
	/// <param name="Result">Результирующая карта</param>
	void Finish([In] int Code,
		[In] string Message,
		[In, MarshalAs(UnmanagedType.Interface)] IMapObject Result);


	/// <summary>
	/// Вызывается по завершению асинхронного процесса, возвращающего массив вещественных чисел
	/// </summary>
	/// <param name="Code">Код ошибки</param>
	/// <param name="Message">Сообщение об ошибке</param>
	/// <param name="Result">Результирующий массив</param>
	void FinishArray([In] int Code,
		[In] string Message,
		[In, MarshalAs(UnmanagedType.Interface)] IRealArray Result);


	/// <summary>
	/// Вызывается по завершению асинхронного процесса, возвращающего таблицу данных
	/// </summary>
	/// <param name="Code">Код ошибки</param>
	/// <param name="Message">Сообщение об ошибке</param>
	/// <param name="Result">Результирующая таблица</param>
	void FinishTable([In] int Code,
		[In] string Message,
		[In, MarshalAs(UnmanagedType.Interface)] IOutputTable Result);

}


/// <summary>
/// Интерфейс расчётной модели
/// </summary>
[ComImport, Guid("402859CD-FBA8-42A6-B762-F685AA924D84"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IModel
{

	/// <summary>
	/// Проверяет принадлежность объекта модели
	/// </summary>
	/// <param name="Addr">Адрес объекта</param>
	/// <returns>Признак принадлежности</returns>
	int HasItemAddr([In] long Addr);


	/// <summary>
	/// Возвращает идентификатор и имя класса объекта
	/// </summary>
	/// <param name="Addr">Адрес объекта</param>
	/// <param name="Id">Идентификатор объекта</param>
	/// <param name="ClassName">Имя класса объекта</param>
	void GetItemId([In] long Addr,
		[Out] out string Id,
		[Out] out string ClassName);


	/// <summary>
	/// Очищает все данные модели
	/// </summary>
	/// <param name="Description">Описание модели</param>
	void Reset([In] string Description);


	/// <summary>
	/// Возвращает строковое представление глобального идентификатора модели
	/// </summary>
	/// <returns>Глобальный идентификатор модели</returns>
	string GetGuid();


	/// <summary>
	/// Возвращает информацию об авторе модели
	/// </summary>
	/// <returns>Информация об авторе модели</returns>
	string GetAuthor();


	/// <summary>
	/// Возвращает описание модели
	/// </summary>
	/// <returns>Описание модели</returns>
	string GetDescription();


	/// <summary>
	/// Возвращает признак учёта сдвиговых деформаций
	/// </summary>
	/// <returns>Признак учёта сдвиговых деформаций: 1 – есть учёт, 0 – нет</returns>
	int GetShear();


	/// <summary>
	/// Устанавливает признак учёта сдвиговых деформаций
	/// </summary>
	/// <param name="Shear">Признак учёта сдвиговых деформаций: 1 – есть учёт, 0 – нет</param>
	void SetShear([In] int Shear);


	/// <summary>
	/// Загружает данные модели из файла
	/// </summary>
	/// <param name="FileName">Имя файла</param>
	/// <param name="Finish">Интерфейс события завершения асинхронного процесса</param>
	void LoadFromFile([In] string FileName,
		[In, MarshalAs(UnmanagedType.Interface)] IFinishEvent Finish);


	/// <summary>
	/// Сохраняет данные модели в файл
	/// </summary>
	/// <param name="FileName">Имя файла</param>
	/// <param name="Mode">Режим сохранения SaveMode</param>
	void SaveToFile([In] string FileName,
		[In] int Mode,
		[In, MarshalAs(UnmanagedType.Interface)] IFinishEvent Finish);


	/// <summary>
	/// Возвращает предположительные единицы длины и силы модели
	/// </summary>
	/// <param name="Length">Eдиницы длины</param>
	/// <param name="Force">Eдиницы силы</param>
	void GetUnits([Out] out string Length,
		[Out] out string Force);


	/// <summary>
	/// Устанавливает предположительные единицы длины и силы модели
	/// допускается передавать только правильные идентификаторы объектов модели.
	/// Масштабирование модели не происходит.
	/// </summary>
	/// <param name="Length">Eдиницы длины</param>
	/// <param name="Force">Eдиницы силы</param>
	void SetUnits([In] string Length,
		[In] string Force);


	/// <summary>
	/// Масштабирует модель
	/// </summary>
	/// <param name="Length">Коэффициент для линейных величин</param>
	/// <param name="Force">Коэффициент для силовых величин</param>
	/// <param name="Finish">Интерфейс события завершения асинхронного процесса</param>
	void Scale([In] double Length,
		[In] double Force,
		[In, MarshalAs(UnmanagedType.Interface)] IFinishEvent Finish);


	/// <summary>
	/// Возвращает значение Cookie
	/// </summary>
	/// <param name="Name">Имя Cookies</param>
	/// <param name="Default">Значение по умолчанию</param>
	/// <returns>Значение Cookie</returns>
	string GetCookie([In] string Name,
		[In] string Default);


	/// <summary>
	/// Устанавливает значение Cookie или удаляет его при пустом значении
	/// </summary>
	/// <param name="Name">Имя Cookie</param>
	/// <param name="Value">Значение Cookie</param>
	void SetCookie([In] string Name,
		[In] string Value);


	/// <summary>
	/// Возвращает список Cookie
	/// </summary>
	/// <returns>Cписок Cookies</returns>
	string ListCookies();


	/// <summary>
	/// Сохраняет модель во временном двоичном файле
	/// </summary>
	/// <returns>Имя файла</returns>
	string SaveToTemporaryFile();


	/// <summary>
	/// Восстанавливает модель из временного двоичного файла
	/// </summary>
	/// <param name="FileName">Имя файла, полученное от метода SaveToTemporary</param>
	void LoadFromTemporaryFile([In] string FileName);


	/// <summary>
	/// Возвращает идентификатор локальной системы координат
	/// </summary>
	/// <returns>Идентификатор локальной системы координат
	string GetLocalCS();


	/// <summary>
	/// Устанавливает идентификатор локальной системы координат
	/// </summary>
	/// <param name="Id">Идентификатор локальной системы координат</param>
	void SetLocalCS([In] string Id);


	/// <summary>
	/// Возвращает указетель на интерфейс списка координатных систем
	/// </summary>
	/// <returns>Указатель на список координатных систем</returns>
	ICSList GetCSList();


	/// <summary>
	/// Возвращает указатель на интерфейс списка узлов
	/// </summary>
	/// <returns>Указатель на список узлов</returns>
	INodeList GetNodeList();


	/// <summary>
	/// Возврвщает указатель на интерфейс списка материалов
	/// </summary>
	/// <returns>Указатель на список материалов</returns>
	IMaterialList GetMaterialList();


	/// <summary>
	/// Возвращает указатель на интерфейс списка сечений
	/// </summary>
	/// <returns>Указатель на список сечений</returns>
	ISectionList GetSectionList();


	/// <summary>
	/// Возвращает указатель на интерфейс списка ориентаций
	/// </summary>
	/// <returns>Указатель на список ориентаций</returns>
	IOrientationList GetOrientationList();


	/// <summary>
	/// Возвращает указатель на интерфейс списка смещений
	/// </summary>
	/// <returns>Указатель на список смещений</returns>
	IOffsetList GetOffsetList();


	/// <summary>
	/// Возвращает указатель на интерфейс списка стержневых элементов
	/// </summary>
	/// <returns>Указатель на список стержневых элементы</returns>
	IMemberList GetMemberList();


	/// <summary>
	/// Возвращает указатель на интерфейс списка жёстких тел
	/// </summary>
	/// <returns>Указатель на список жёстких тел</returns>
	IRigidBodyList GetRigidBodyList();


	/// <summary>
	/// Возвращает указатель на интерфейс списка плоских элементов
	/// </summary>
	/// <returns>Указатель на список плоских элементы</returns>
	IPlateList GetPlateList();


	/// <summary>
	/// Возвращает указатель на интерфейс списка кабельных элементов
	/// </summary>
	/// <returns>Указатель на список кабельных элементов</returns>
	ICableList GetCableList();


	/// <summary>
	/// Возвращает указатель на интерфейс списка пружинных элементов
	/// </summary>
	/// <returns>Указатель на список пружинных элементы</returns>
	ISpringList GetSpringList();


	/// <summary>
	/// Возвращает указатель на интерфейс списка суперэлементов
	/// </summary>
	/// <returns>Указатель на список суперэлементов</returns>
	ISuperelementList GetSuperelementList();


	/// <summary>
	/// Возвращает указатель на интерфейс списка нагрузок
	/// </summary>
	/// <returns>Указатель на список нагрузок
	ILoadingList GetLoadingList();


	/// <summary>
	/// Возвращает указатель на интерфейс списка наборов
	/// </summary>
	/// <returns>Указатель на список наборов</returns>
	INamedGroupList GetNamedGroupList();


	/// <summary>
	/// Создаёт группу объектов
	/// </summary>
	/// <returns>Указатель на интерфейс группы
	IGroup CreateGroup();


	/// <summary>
	/// Осуществляет разбор и выполнение скрипта
	/// </summary>
	/// <param name="Client">Указатель на интерфейс клиента. При нулевом значении скрипт выполняется синхронно</param>
	/// <param name="FileName">Имя исходного файла. Если имя начинается знаком *, это означает, что передаётся текст выполняемого скрипта</param>
	/// <param name="Param">Параметр запуска</param>
	/// <param name="Debug">Признак режима отладки. В режиме отладки клиент получает события OnDebugTrace</param>
	/// <returns>Содержимое потока вывода (при запуке без клиента)</returns>
	string RunScript([In, MarshalAs(UnmanagedType.Interface)] IScriptClient Client,
		[In] string FileName,
		[In] string Param,
		[In] int Debug);


	/// <summary>
	/// Приостанавливает работу скрипта
	/// <summary>
	void PauseScript();


	/// <summary>
	/// Продолжает работу вычислительного потока
	/// </summary>
	void ResumeScript();


	/// <summary>
	/// Вычисляет отладочное выражение в текущем контексте. Функция работает только в момент приостановленной работы скрипта
	/// после того, как IScriptClient::OnDebugTrace возвращает 2
	/// </summary>
	/// <param name="Exp">Вычисляемое выражение</param>
	/// <param name="Result">Результат вычисления или сообщение об ошибке</param>
	/// <param name="TypeName">Имя типа результата</param>
	/// <returns>Код ошибки</returns>
	int EvaluateWatchExpression([In] string Exp,
		[Out] out string Result,
		[Out] out string TypeName);


	/// <summary>
	/// Возвращает поток вывода скрипта
	/// </summary>
	/// <returns>Поток вывода</returns>
	string GetOutput();


	/// <summary>
	/// Завершает работу скрипта
	/// </summary>
	void TerminateScript();


	/// <summary>
	/// Проверяет, что скрипт находится в приостановленном состоянии
	/// </summary>
	/// <returns>Флаги скипта: бит 0 – скрипт запущен, 1 – приостановлен,
	///  2 – работа прекращена, 3 – режим ожидания, 4 – режим расчёта,
	///  5 – возникла ошибка, 6 - режим отладки</returns>
	long GetScriptState();


	/// <summary>
	/// Вычисляет выражение в контексте объекта или модели
	/// </summary>
	/// <param name="Item">Адрес объекта, при нулевом значении вычисление происходит в контексте модели</param>
	/// <param name="Exp">Вычисляемое выражение</param>
	/// <returns>Строковое представление результата</returns>
	string EvaluateExpression([In] long Item,
		[In] string Exp);


	/// <summary>
	/// Формирует карту результатов для узлов
	/// </summary>
	/// <param name="FileName">Имя файла карты</param>
	/// <param name="Nodes">Список идентификаторов узлов (разделяются пробелами)</param>
	/// <param name="Factors">Cписок идентификаторов факторов</param>
	/// <param name="Loads">Список идентификаторов загружений</param>
	/// <param name="Client">Клиент карты, получающий её интерфейс</param>
	void GetNodeResults([In] string FileName,
		[In] string Nodes,
		[In] string Factors,
		[In] string Loads,
		[In, MarshalAs(UnmanagedType.Interface)] IMapClient Client);


	/// <summary>
	/// Формирует карту результатов для стержневых элементов
	/// </summary>
	/// <param name="FileName">Имя файла карты</param>
	/// <param name="Members">Список идентификаторов стержневых элементов (разделяются пробелами)</param>
	/// <param name="Factors">Cписок идентификаторов факторов</param>
	/// <param name="Loads">Список идентификаторов загружений</param>
	/// <param name="Client">Клиент карты, получающий её интерфейс</param>
	void GetMemberResults([In] string FileName,
		[In] string Members,
		[In] string Factors,
		[In] string Loads,
		[In, MarshalAs(UnmanagedType.Interface)] IMapClient Client);


	/// <summary>
	/// Формирует карту результатов для вантовых элементов
	/// </summary>
	/// <param name="FileName">Имя файла карты</param>
	/// <param name="Nodes">Список идентификаторов вантовых элементов (разделяются пробелами)</param>
	/// <param name="Factors">Cписок идентификаторов факторов</param>
	/// <param name="Loads">Список идентификаторов загружений</param>
	/// <param name="Client">Клиент карты, получающий её интерфейс</param>
	void GetCableResults([In] string FileName,
		[In] string Cables,
		[In] string Factors,
		[In] string Loads,
		[In, MarshalAs(UnmanagedType.Interface)] IMapClient Client);

}


/// <summary>
/// Интерфейс клиента системы конфигурации и взаимодействия между копиями приложения
/// </summary>
[ComImport, Guid("A1450423-5E3B-4A39-89C9-F92B61EADD60"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface ISettingClient
{

	/// <summary>
	/// Вызывается в ответ на изменение значения параметра конфигурации
	/// </summary>
	/// <param name="Key">Имя ключа</param>
	/// <param name="OldValue">Старое значение ключа</param>
	/// <param name="NewValue">Новое значение ключа</param>
	void OnSettingChanged([In] string Key,
		[In] string OldValue,
		[In] string NewValue);


	/// <summary>
	/// Вызывается в ответ на запуск новой копии приложения
	/// </summary>
	/// <param name="Handle">Дескриптор новой копии</param>
	/// <param name="Stamp">Метка времени запуска новой копии</param>
	void OnNewInstance([In] long Handle,
		[In] long Stamp);


	/// <summary>
	/// Вызывается в ответ на получение сообщения от другой копии приложения
	/// </summary>
	/// <param name="From">Дескриптор копии приложения, пославшей сообщение</param>
	/// <param name="Msg">Текст сообщения</param>
	/// <returns>Ответ на сообщение</returns>
	long OnMessage([In] long From,
		[In] string Msg);

}


/// <summary>
/// Интерфейс системы конфигурации и взаимодействия между копиями приложения
/// </summary>
[ComImport, Guid("1F40D9F7-579A-419D-B252-85ED41DD7D90"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface ISettings
{

	/// <summary>
	/// Возвращает значение ключа конфигурации. Если ключ не существует, он создаётся автоматически
	/// </summary>
	/// <param name="Name">Имя ключа. В качестве разделителя имён подразделов используется точка</param>
	/// <returns>Значение ключа</returns>
	string GetValue([In] string Name);


	/// <summary>
	/// Устанавливает значение ключа конфигурации. Если ключ не существует, он создаётся автоматически
	/// </summary>
	/// <param name="Name">Имя ключа</param>
	/// <returns>Значение ключа</returns>
	void SetValue([In] string Name,
		[In] string Value);


	/// <summary>
	/// Подписывается на событие изменения значения ключа и всех его вложенных ключей
	/// </summary>
	/// <param name="Name">Имя ключа</param>
	/// <param name="Client">Интерфейс клиента, асинхронно получающий вызовы метода OnSettingChanged</param>
	/// <returns>Код возврата для отказа от подписки</returns>
	long Subscribe([In] string Name,
		[In, MarshalAs(UnmanagedType.Interface)] ISettingClient Client);


	/// <summary>
	/// Снимает подписку на событие изменения ключа
	/// </summary>
	/// <param name="Name">Имя ключа</param>
	/// <param name="Code">Код возврата, полученный от метода Subscribe</param>
	void Unsubscribe([In] string Name,
		[In] long Code);


	/// <summary>
	/// Возвращает список всех параметров конфигурации
	/// </summary>
	/// <returns>Список параметров</returns>
	string ListValues();


	/// <summary>
	/// Возвращает флаг временного (не сохраняемого) параметра
	/// </summary>
	/// <param name="Name">Имя ключа</param>
	/// <returns>Флаг временного параметра</returns>
	int GetProvisional([In] string Name);


	/// <summary>
	/// Устанавливает флаг временного параметра
	/// </summary>
	/// <param name="Name">Имя ключа</param>
	/// <param name="Flag">Флаг временного параметра</param>
	void SetProvisional([In] string Name,
		[In] int Flag);


	/// <summary>
	/// Возвращает флаг локального параметра. При изменении таких параметров
	/// другие копии приложения не информируются
	/// </summary>
	/// <param name="Name">Имя ключа</param>
	/// <returns>Флаг временного параметра</returns>
	int GetLocal([In] string Name);


	/// <summary>
	/// Устанавливает флаг локального параметра
	/// </summary>
	/// <param name="Name">Имя ключа</param>
	/// <param name="Flag">Флаг локального параметра</param>
	void SetLocal([In] string Name,
		[In] int Flag);


	/// <summary>
	/// Принудительно записивает всю структуру конфигурации в файл
	/// </summary>
	void Dump();


	/// <summary>
	/// Принудительно восстанавливает всю структуру конфигурации из файла
	/// </summary>
	void Reload();


	/// <summary>
	/// Устанавливает указатель на интерфейс клиента, получающего
	/// события запуска новой копии приложения и сообщения от
	/// параллельно работающих копий
	/// </summary>
	/// <param name="Client">Интерфейс клиента</param>
	void SetMessageClient([In, MarshalAs(UnmanagedType.Interface)] ISettingClient Client);


	/// <summary>
	/// Возвращает список дескрипторов и временных меток
	/// параллельно работающих копий приложения
	/// </summary>
	/// <returns>Список</returns>
	string GetInstanceList();


	/// <summary>
	/// Возвращает временную метку текущей копии приложения
	/// </summary>
	/// <returns>Временная метка</returns>
	long GetTimeStamp();


	/// <summary>
	/// Отправляет текстовое сообщение параллельно работающей
	/// копии приложения и получает от него числовой ответ
	/// </summary>
	/// <param name="Handle">Дескриптор копии</param>
	/// <param name="Msg">Сообщение</param>
	/// <returns>Ответ на сообщение</returns>
	long SendMessage([In] long Handle,
		[In] string Msg);


	/// <summary>
	/// Рассылает текстовое сообщение параллельно работающим копиям приложения
	/// </summary>
	/// <param name="Msg">Сообщение</param>
	/// <returns>Список ответов на сообщение</returns>
	string BroadcastMessage([In] string Msg);

}


/// <summary>
/// Интерфейс обратного вызова для преобразования координат чертежа при чтении
/// </summary>
[ComImport, Guid("A8B44E67-E66D-476C-8798-2B78882DF9EE"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface ICoordTransform
{

	/// <summary>
	/// Выполняет преобразование координат
	/// </summary>
	/// <param name="X0">Начальная координата X</param>
	/// <param name="Y0">Начальная координата Y</param>
	/// <param name="Z0">Начальная координата Z</param>
	/// <param name="X">Преобразованная координата X</param>
	/// <param name="Y">Преобразованная координата Y</param>
	/// <param name="Z">Преобразованная координата Z</param>
	double Transform([In] double X0,
		[In] double Y0,
		[In] double Z0,
		[Out] out double X,
		[Out] out double Y);

}


/// <summary>
/// Интерфейс обратного вызова для перечисления примитивов чертежа
/// <summary>
[ComImport, Guid("EC1FC966-9B05-48E2-8358-D3B8999E5852"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IDxfEnum
{

	/// <summary>
	/// Вызывается при перечислении окружностей
	/// </summary>
	/// <param name="X">Координата X центра окружности</param>
	/// <param name="Y">Координата Y центра окружности</param>
	/// <param name="Z">Координата Z центра окружности</param>
	/// <param name="R">Радиус окружности</param>
	/// <param name="Layer">Слой примитива</param>
	/// <param name="Color">Цвет примитива</param>
	/// <returns>Признак остановки перечисления</returns>
	int EnumCircle([In] double X,
		[In] double Y,
		[In] double Z,
		[In] double R,
		[In] string Layer,
		[In] string Color);


	/// <summary>
	/// Вызывается при перечислении текстовых полей
	/// </summary>
	/// <param name="X">Координата X точки вставки</param>
	/// <param name="Y">Координата Y точки вставки</param>
	/// <param name="Z">Координата Z точки вставки</param>
	/// <param name="Text">Содержимое текста</param>
	/// <param name="Layer">Слой примитива</param>
	/// <param name="Color">Цвет примитива</param>
	/// <returns>Признак остановки перечисления</returns>
	int EnumText([In] double X,
		[In] double Y,
		[In] double Z,
		[In] string Text,
		[In] string Layer,
		[In] string Color);


	/// <summary>
	/// Вызываетcя при перечислении линий чертежа
	/// </summary>
	/// <param name="N">Количество точек линии</param>
	/// <param name="Layer">Слой примитива</param>
	/// <param name="Color">Цвет примитива</param>
	/// <returns>Признак остановки перечисления</returns>
	int EnumLine([In] int N,
		[In] string Layer,
		[In] string Color);


	/// <summary>
	/// Вызывается при переборе точек линии
	/// </summary>
	/// <param name="X">Координата X</param>
	/// <param name="Y">Координата Y</param>
	/// <param name="Z">Координата Z</param>
	/// <returns>Признак остановки перечисления точек</param>
	int EnumVertex([In] double X,
		[In] double Y,
		[In] double Z);

}


/// <summary>
/// Интерфейс чтения данных из файла DXF
/// </summary>
[ComImport, Guid("B6B813ED-D10E-43EA-B769-40219EF2BD10"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IDxfReader
{

	/// <summary>
	/// Загружает данные из файла .dxf
	/// </summary>
	/// <param name="FileName">Имя файла</param>
	/// <param name="Eps">Точность определения совпадающих точек</param>
	/// <param name="Buldge">Тип приведения скруглений (больше нуля – по количеству точек, меньше – по длине хорды)</param>
	/// <param name="Transform">Интерфейс преобразования координат (может быть нулевым)</param>
	void LoadFromFile([In] string FileName,
		[In] double Eps,
		[In] double Buldge,
		[In, MarshalAs(UnmanagedType.Interface)] ICoordTransform Transform);


	/// <summary>
	/// Перечисляет слои чертежа
	/// </summary>
	/// </param name="Enum">Интерфейс обратного вызова для перечисления слоёв</param>
	void EnumLayers([In, MarshalAs(UnmanagedType.Interface)] IStringEnum Enum);


	/// <summary>
	/// Находит точки пересечения линий сечения
	/// </summary>
	/// <param name="Eps">Точность поиска</param>
	/// <param name="SameLayer">Признак нахождения пересекающихся линий в одном слое</param>
	/// <param name="SameColor">Признак одинакового цвета перекающихся линий</param>
	/// <param name="IgnoreLayers">Список слоёв (разделяется \r\n), в которых поиск не производится</param>
	/// <param name="Finish">Интерфейс события завершения асинхронного процесса</param>
	void FindIntersections([In] double Eps,
		[In] int SameLayer,
		[In] int SameColor,
		[In] string IgnoreLayers,
		[In, MarshalAs(UnmanagedType.Interface)] IFinishEvent Finish);


	/// <summary>
	/// Возвращает координаты крайних точек и центра чертежа
	/// <summary>
	/// <param name="XMin">Минимальная координата X</param>
	/// <param name="YMin">Минимальная координата Y</param>
	/// <param name="ZMin">Минимальная координата Z</param>
	/// <param name="XMax">Максимальная координата X</param>
	/// <param name="YMax">Максимальная координата Y</param>
	/// <param name="ZMax">Максимальная координата Z</param>
	/// <param name="XCen">Центральная координата X</param>
	/// <param name="YCen">Центральная координата Y</param>
	/// <param name="ZCen">Центральная координата Z</param>
	double GetBounds([Out] out double XMin,
		[Out] out double YMin,
		[Out] out double ZMin,
		[Out] out double XMax,
		[Out] out double YMax,
		[Out] out double ZMax,
		[Out] out double XCen,
		[Out] out double YCen);


	/// <summary>
	/// Перечисляет окружности чертежа
	/// </summary>
	/// <param name="Layers">Список слоёв (с разделением \r\n, может быть пустая строка)</param>
	/// <param name="Enum">Интерфейс обратного вызова для перечисления</param>
	void EnumCircles([In] string Layers,
		[In, MarshalAs(UnmanagedType.Interface)] IDxfEnum Enum);


	/// <summary>
	/// Перечисляет текстовые поля чертежа
	/// </summary>
	/// <param name="Layers">Список слоёв (с разделением \r\n, может быть пустая строка)</param>
	/// <param name="Enum">Интерфейс обратного вызова для перечисления</param>
	void EnumTexts([In] string Layers,
		[In, MarshalAs(UnmanagedType.Interface)] IDxfEnum Enum);


	/// <summary>
	/// Перечисляет линии и точки линий.
	/// Для каждой линии вызывает IDxfEnum::EnumLine,
	/// затем для каждой точки линии IDxfEnum::EnumVertex
	/// </summary>
	/// <param name="Layers">Список слоёв (с разделением \r\n, может быть пустая строка)</param>
	/// <param name="Enum">Интерфейс обратного вызова для перечисления</param>
	void EnumLines([In] string Layers,
		[In, MarshalAs(UnmanagedType.Interface)] IDxfEnum Enum);


	/// <summary>
	/// Перечисляет полинии чертежа, формирующие изображение стрелок.
	/// Для каждой найденной стрелки вызывает IDXfEnum::EnumLine (с пустыми значениями слоя и цвета)
	/// затем для каждой точки стрелки IDxfEnum::EnumVertex
	/// </summary>
	/// <param name="Layers">Список слоёв (с разделением \r\n, может быть пустая строка)</param>
	/// <param name="Eps">Точность поиска</param>
	/// <param name="Enum">Интерфейс обратного вызова для перечисления</param>
	void EnumArrows([In] string Layers,
		[In] double Eps,
		[In, MarshalAs(UnmanagedType.Interface)] IDxfEnum Enum);

}


/// <summary>
/// Интерфейс контурного сечения
/// </summary>
[ComImport, Guid("531BDAAA-ADAE-4401-9FD6-C2F84055C4CE"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface ISectionShape
{

	/// <summary>
	/// Загружает данные из интерфейса IDxfReader
	/// </summary>
	/// <param name="Reader">Интерфейс чтения данных чертежа</param>
	/// <param name="Layer">Имя слоя, в котором находятся примитивы сечения</param>
	void LoadFromDxfReader([In, MarshalAs(UnmanagedType.Interface)] IDxfReader Reader,
		[In] string Layer);


	/// <summary>
	/// Добавляет прямолинейный сегмент к сечению
	/// </summary>
	/// <param name="Y1">Горизонтальная координата начальной точки</param>
	/// <param name="Z1">Вертикальная координата начальной точки</param>
	/// <param name="Y2">Горизонтальная координата конечной точки</param>
	/// <param name="Z2">Вертикальная координата конечной точки</param>
	void AddSegment([In] double Y1,
		[In] double Z1,
		[In] double Y2,
		[In] double Z2);


	/// <summary>
	/// Завершает формирование cечения
	/// </summary>
	void Finish();


	/// <summary>
	/// Очищает все данные сечения
	/// </summary>
	void Reset();


	/// <summary>
	/// Возвращает количество замкнутых контуров сечения
	/// </summary>
	/// <returns>Количество контуров</returns>
	int GetContourCount();


	/// <summary>
	/// Возвращает количество вершин в контуре, для внутренних контуров (отверстий) возвращается отрицательное число
	/// </summary>
	/// <param name="Contour">Номер контура</param>
	/// <returns>Количество вершин</returns>
	int GetContourVertexCount([In] int Contour);


	/// <summary>
	/// Возвращает координаты вершины контура
	/// </summary>
	/// <param name="Contour">Номер контура</param>
	/// <param name="Vertex">Номер вершины</param>
	/// <param name="Y">Горизонтальная координата точки</param>
	/// <param name="Z">Вертикальная координата точки</param>
	void GetVertexCoord([In] int Contour,
		[In] int Point,
		[Out] out double Y,
		[Out] out double Z);


	/// <summary>
	/// Возвращает геометрические характеристики сечения
	/// </summary>
	/// <param name="Name">Имя характеристики Ax, Ay, Az, Ix, Iy, Iz, Yc, Zc, Alpha</param>
	/// <returns>Величина характеристики</returns>
	double GetProperty([In] string Name);


	/// <summary>
	/// Возвращает количество контрольных точек, взятых из DXF файла.
	/// Контрольные точки на чертеже задаются как примитивы DTEXT,
	/// точка вставки которого определяет координаты, а текст – её числовой
	/// идентификатор
	/// </summary>
	/// <returns>Количество контрольных точек</returns>
	int GetControlPointCount();


	/// <summary>
	/// Возвращает идентификатор и координаты контрольной точки из DXF файла
	/// </summary>
	/// <param name="Index">Номер контрольной точки</param>
	/// <param name="Id">Идентификатор контрольной точки</param>
	/// <param name="Y">Горизонтальная координата контрольной точки</param>
	/// <param name="Z">Вертикальная координата контрольной точки</param>
	void GetControlPointCoord([In] int Index,
		[Out] out int Id,
		[Out] out double Y,
		[Out] out double Z);


	/// <summary>
	/// Производит триангуляцию сечения
	/// </summary>
	/// <param name="Size">Приблизительный размер треугольников</param>
	/// <param name="Opt">Количество итераций оптизимации сетки по Ллойду</param>
	/// <param name="ConstraintPoints">Массив координат фиксированных точек – строка,
	///   содержащая разделённые пробелами строковые представления вещественных чисел –
	///   попарно записанные координаты</param>
	/// <param name="Finish">Интерфейс события завершения асинхронного процесса</param>
	void Mesh([In] double Size,
		[In] int Opt,
		[In] string ConstraintPoints,
		[In, MarshalAs(UnmanagedType.Interface)] IFinishEvent Finish);


	/// <summary>
	/// Возвращает количество узлов триангулированной сетки сечения
	/// </summary>
	/// <returns>Количество узлов</returns>
	int GetMeshNodeCount();


	/// <summary>
	/// Возвращает координаты узла сетки
	/// </summary>
	/// <param name="Index">Номер узла</param>
	/// <param name="Y">Горизонтальная координата</param>
	/// <param name="Z">Вертикальная координата</param>
	void GetMeshNodeCoord([In] int Index,
		[Out] out double Y,
		[Out] out double Z);


	/// <summary>
	/// Возвращает количество треугольников триангулированной сетки сечения
	/// </summary>
	/// <returns>Количество треугольников</returns>
	int GetMeshTriangleCount();


	/// <summary>
	/// Возвращает номера узлов треугольника сетки
	/// </summary>
	/// <param name="Index">Номер треугольника</param>
	/// <param name="P1">Номер первой вершины</param>
	/// <param name="P2">Номер второй вершины</param>
	/// <param name="P3">Номер третьей вершины</param>
	void GetMeshTriangleNodes([In] int Index,
		[Out] out int P0,
		[Out] out int P1,
		[Out] out int P2);


	/// <summary>
	/// Проверяет нахождение точки внутри контура сечения
	/// </summary>
	/// <param name="Y">Горизонтальная координата</param>
	/// <param name="Z">Вертикальная координата</param>
	/// <returns>Признак нахождения точки внутри контура сечения</returns>
	int Inside([In] double Y,
		[In] double Z);


	/// <summary>
	/// Проверяет нахождение точки в одном из треугольников сетки
	/// </summary>
	/// <param name="Y">Горизонтальная координата</param>
	/// <param name="Z">Вертикальная координата</param>
	/// <returns>Номер треугольника, в котором находится точка, или -1 (когда треугольник не найден)</returns>
	int InsideTriangle([In] double Y,
		[In] double Z);

}


/// <summary>
/// Интерфейс списка идентификаторов
/// </summary>
[ComImport, Guid("1AABD877-CEDD-49E2-AF64-C2486EA81BDD"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IIdentList
{

	/// <summary>
	/// Возвращает количество идентификаторов в списке
	/// </summary>
	/// <returns>Количество идентификатров</returns>
	int GetCount();


	/// <summary>
	/// Возвращает имя идентификатора
	/// </summary>
	/// <param name="Index">Номер идентификатора</param>
	/// <returns>Имя идентификатора</returns>
	string GetName([In] int Index);


	/// <summary>
	/// Возвращает определение идентификатора
	/// </summary>
	/// <param name="Index">Номер идентификатора</param>
	/// <returns>Объявление идентификатора</returns>
	string GetDeclaration([In] int Index);


	/// <summary>
	/// Возвращает имя файла, в котором объявлен идентификатор
	/// </summary>
	/// <param name="Index">Номер идентификатора</param>
	/// <returns>Имя файла</returns>
	string GetFileName([In] int Index);


	/// <summary>
	/// Возвращает позицию объявления идентификаторв
	/// </summary>
	/// <param name="Index">Номер идентификатора</param>
	/// <param name="Line">Номер строки</param>
	/// <param name="Char">Норме символа</param>
	void GetPosition([In] int Index,
		[Out] out int Line,
		[Out] out int Char);


	/// <summary>
	/// Возвращает последовательность символов, предшествующую точке сбора идентификаторов
	/// </summary>
	/// <param name="Char">Символ пунктуации перед строкой букв и цифр</param>
	/// <returns>Строка букв и цифр перед точкой сбора</returns>
	string GetCurrent([Out] out int Char);


	/// <summary>
	/// Фильтрует список идентификаторов, оставляя только содержащие указанную подстроку
	/// </summary>
	/// <param name="Str">Искомая подстрока</param>
	/// <returns>Индекс ближайшего к указанному идентификатора</returns>
	int Filter([In] string Str);

}


/// <summary>
/// Интерфейс обратного вызова, обрабатывающий события редактора
/// </summary>
[ComImport, Guid("5003C875-8BFC-479F-B367-7C0C69184022"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IEditorClient
{

	/// <summary>
	/// Вызывается при переходе редактора в режим изменения состояния
	/// </summary>
	/// <param name="Editor">Указатель на интерфейс редактора</param>
	void OnChanging();


	/// <summary>
	/// Вызывается при завершении изменения состояния
	/// </summary>
	/// <param name="Editor">Указатель на интерфейс редактора</param>
	void OnChanged();


	/// <summary>
	/// Используется для перечисления блоков подсветки
	/// </summary>
	/// <param name="Line">Строка блока</param>
	/// <param name="Char">Позиция первого символа</param>
	/// <param name="Style">Номер стиля</param>
	/// <param name="Str">Текст блока</param>
	void HighlightEnum([In] int Line,
		[In] int Char,
		[In] int Style,
		[In] string Str);


	/// <summary>
	/// Вызывается при запросе на показ контекстного меню
	/// </summary>
	/// <param name="X">Горизонтальная координата</param>
	/// <param name="Y">Вертикальная координата</param>
	void ContextMenuRequest([In] int X,
		[In] int Y);


	/// <summary>
	/// Вызывается при запросе на показ всплывающей подсказки
	/// </summary>
	/// <param name="X">Горизонтальная координата</param>
	/// <param name="Y">Вертикальная координата</param>
	/// <param name="Width">Ширина области</param>
	/// <param name="Height">Высота области</param>
	/// <param name="Away">Признак того, что подсказка не совпадает со словом, над которым находится курсор мыши
	/// <param name="Str">Текст подсказки</param>
	void HintRequest([In] int X,
		[In] int Y,
		[In] int Width,
		[In] int Height,
		[In] int Away,
		[In] string Str);


	/// <summary>
	/// Вызывается при запросе на показ списка идентификаторов
	/// </summary>
	/// <param name="X">Горизонтальная координата</param>
	/// <param name="Y">Вертикальная координата</param>
	/// <param name="Idents">Список идентификаторов</param>
	void IdentListRequest([In] int X,
		[In] int Y,
		[In, MarshalAs(UnmanagedType.Interface)] IIdentList Idents);


	/// <summary>
	/// Вызывается при запросе указателя на интерфейс редактора по имени файла
	/// </summary>
	/// <param name="FileName">Абсолютное имя файла</param>
	/// <returns>Указатель на интерфейс редактора</returns>
	IScriptEditor GetEditor([In] string FileName);


	/// <summary>
	/// Вызывается в ответ на запрос показа строки с ошибкой выполнения скрипта
	/// </summary>
	void DisplayError([In] int Line,
		[In] int Char);


	/// <summary>
	/// Вызывается в ответ запрос показа точки трассировки
	/// </summary>
	void DisplayTrace([In] int Line,
		[In] int Char);

}


/// <summary>
///	Интерфейс обратного вызова для сравнения строк
/// </summary>
[ComImport, Guid("2092E2EE-007A-4DF0-BEA3-719AE74F8B24"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface ICompareString
{

	/// <summary>
	///	Сравнивает две строки
	/// </summary>
	/// <param name="Str1">Первая строка</param>
	/// <param name="Str2">Вторая строка</param>
	/// <returns>Результат сравнения</returns>
	int CompareString([In] string Str1,
		[In] string Str2);

}


/// <summary>
/// Интерфейс редактора скриптов
/// </summary>
[ComImport, Guid("081898B1-BE4B-4A22-B398-0EBA8F7942D5"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IScriptEditor
{

	/// <summary>
	/// Тестовый метод
	/// </summary>
	void Test();


	/// <summary>
	/// Регистрирует интерфейс клиента
	/// </summary>
	/// <param name="Client">Указатель на интерфейс клиента</param>
	/// <returns>Дескриптор для отмены регистрации</returns>
	long RegisterClient([In, MarshalAs(UnmanagedType.Interface)] IEditorClient Client);


	/// <summary>
	/// Отменяет регистрацию интерфейса клиента
	/// </summary>
	/// <param name="Handle">Дескриптор, полученный через метод RegisterClient</param>
	void UnregisterClient([In] long Handle);


	/// <summary>
	/// Загружает текст из файла
	/// </summary>
	/// <param name="FileName">Имя файла</param>
	/// <param name="New">Признак создания нового файла</param>
	void LoadFromFile([In] string FileName,
		[In] int New);


	/// <summary>
	/// Сохраняет текст в файл
	/// </summary>
	/// <param name="FileName">Имя файла</param>
	void SaveToFile([In] string FileName);


	/// <summary>
	/// Возващает имя файла скрипта и время последнего его изменения
	/// </summary>
	/// <param name="DateTime">Время последнего изменения файла</param>
	/// <returns>Имя файла</returns>
	string GetFileName([Out] out double DateTime);


	/// <summary>
	/// Проверяет, что время последней записи в файл отличается от зафиксированной более чем на одну десятую секунды
	/// <summary>
	/// <returns>Признак измнения файла внешним приложением</returns>
	int CheckOutsideModification();


	/// <summary>
	/// Сообщает редактору, что клиент собирается изменить его состояние и ожидает событие OnChanging
	/// </summary>
	/// <param name="EditText">Указание на то, что будет изменяться текст</param>
	void BeginUpdate([In] int EditText);


	/// <summary>
	/// Сообщает редактору, что клиент закончил изменение состояния и ожидает событие OnChanged
	/// Вызов метода обязан следовать за любым вызовом BeginUpdate
	/// </summary>
	/// <param name="EditText">Значение, полученное в предыдущем вызове BeginUpdate</param>
	void EndUpdate([In] int EditText);


	/// <summary>
	/// Проверяет, что текст редактора был изменён с момента последнего вызова Load/Save
	/// </summary>
	/// <param name="Group">Ненулевое значение указывает на то, что клиент собирается самостоятельно изменить флаг</param>
	/// <returns>Признак изменения текста</returns>
	int IsModified([In] int Set);


	/// <summary>
	/// Возвращает количество строк редактора
	/// </summary>
	/// <returns>Количество строк</returns>
	int GetLineCount();


	/// <summary>
	/// Возвращает строку текста
	/// </summary>
	/// <param name="Index">Номер строки (для неправильных индексов возвращается пустая строка)</param>
	/// <returns>Строка текста</returns>
	string GetLineString([In] int Index);


	/// <summary>
	/// Возвращает стили символов строки текста
	/// </summary>
	/// <param name="Index">Номер строки</param>
	/// <returns>Строки стилей</returns>
	string GetLineStyle([In] int Index);


	/// <summary>
	/// Перечисляет все блоки подсветки в заданной области редактора
	/// </summary>
	/// <param name="Left">Первая позиция области видимости</param>
	/// <param name="Top">Первая строка области видимости</param>
	/// <param name="Width">Ширина области видимости</param>
	/// <param name="Height">Высота области видимости</param>
	/// <param name="Client">Указатель на интерфейс клиента</param>
	void HighlightEnum([In] int Left,
		[In] int Top,
		[In] int Width,
		[In] int Height,
		[In, MarshalAs(UnmanagedType.Interface)] IEditorClient Client);


	/// <summary>
	/// Перечисляет все блоки подсветки текста и возвращает результат в виде последовательности html-тэгов
	/// </summary>
	/// <returns>Последовательность html-тэгов редактируемого текста</returns>
	string HtmlHighlight();


	/// <summary>
	/// Проверяет доступность операции Отмены
	/// </summary>
	/// <returns>Доступность Отмены</returns>
	int CanUndo();


	/// <summary>
	/// Выполняет отмену последнего изменения текста
	/// </summary>
	void Undo();


	/// <summary>
	/// Проверяет доступность операции Возврата отмены
	/// </summary>
	/// <returns>Доступность Возврата</returns>
	int CanRedo();


	/// <summary>
	/// Восстанавливает последнюю операцию отмены
	/// </summary>
	void Redo();


	/// <summary>
	/// Очищает информацию о очередях Отмены и Возврата
	/// </summary>
	void DestroyUndo();


	/// <summary>
	/// Возвращает номер верхней видимой строки
	/// </summary>
	/// <returns>Номер строки</returns>
	int GetTopLine();


	/// <summary>
	/// Устанавливает номер верхней видимой строки
	/// </summary>
	/// <param name="Value">Номер строки</param>
	void SetTopLine([In] int Value);


	/// <summary>
	/// Возвращает позицию левого видимого символа
	/// </summary>
	/// <returns>Позиция символа</returns>
	int GetLeftChar();


	/// <summary>
	/// Устанавливает позицию левого видимого символа
	/// </summary>
	/// <param name="Value">Позиция символа</param>
	void SetLeftChar([In] int Value);


	/// <summary>
	/// Возвращает позицию верхнего левого видимого символа
	/// </summary>
	/// <param name="Top">Номер строки</param>
	/// <param name="Left">Позиция символа</param>
	void GetTopLeftPos([Out] out int Top,
		[Out] out int Left);


	/// <summary>
	/// Устанавливает позицию верхнего левого символа
	/// </summary>
	/// <param name="Top">Номер строки</param>
	/// <param name="Left">Позиция символа</param>
	void SetTopLeftPos([In] int Top,
		[In] int Left);


	/// <summary>
	/// Возвращает вертикальную позицию каретки
	/// </summary>
	/// <param name="Relative">Признак относительной позиции</param>
	int GetCurrentLine([In] int Relative);


	/// <summary>
	/// Устанавливает вертикальную позицию каретки
	/// </summary>
	/// <param name="Value">Номер строки</param>
	/// <param name="Relative">Признак относительной позиции</param>
	void SetCurrentLine([In] int Value,
		[In] int Relative);


	/// <summary>
	/// Возвращает горизонтальную позицию каретки
	/// </summary>
	/// <param name="Relative">Признак относительной позиции</param>
	/// <returns>Позиция символа</returns>
	int GetCurrentChar([In] int Relative);


	/// <summary>
	/// Устанавливает горизонтальную позицию каретки
	/// </summary>
	/// <param name="Value">Номер символа</param>
	/// <param name="Relative">Признак относительной позиции</param>
	void SetCurrentChar([In] int Value,
		[In] int Relative);


	/// <summary>
	/// Возвращает текущую позицию каретки
	/// </summary>
	/// <param name="Line">Номер строки</param>
	/// <param name="Char">Позиция символа</param>
	/// <param name="Relative">Признак относительной позиции</param>
	void GetCurrentPos([Out] out int Line,
		[Out] out int Char,
		[In] int Relative);


	/// <summary>
	/// Устанавливаеет текущую позицию каретки
	/// </summary>
	/// <param name="Line">Номер строки</param>
	/// <param name="Char">Позиция символа</param>
	/// <param name="Relative">Признак относительной позиции</param>
	void SetCurrentPos([In] int Line,
		[In] int Char,
		[In] int Relative);


	/// <summary>
	/// Устанавливает позицию левого верхнего угла так, чтобы текущая позиция была внутри видимой области
	/// </summary>
	void ScrollCaretIntoView();


	/// <summary>
	/// Возвращает максимально допустимые координаты левого верхнего угла текста
	/// </summary>
	/// <param name="MaxLeft">Максимально допустимая позиция символа</param>
	/// <param name="MaxTop">Максимально допустимый номер строки</param>
	void GetMaxPos([Out] out int MaxLeft,
		[Out] out int MaxTop);


	/// <summary>
	/// Возвращает максимальную длину строки текста
	/// </summary>
	/// <returns>Максимальная длина строки</returns>
	int GetMaxLength();


	/// <summary>
	/// Вычисляет максимальную длину строки, используя полученный аргумент в качестве начального значения
	/// </summary>
	/// <param name="Start">Начальное значение</param>
	/// <returns>Найденная максимальная длина строки</returns>
	int SetMaxLength([In] int Start);


	/// <summary>
	/// Проверяет, что редактор находится в режиме простого текста (без подсветки синтаксиса)
	/// </summary>
	/// <returns>Признак режима простого текста</returns>
	int GetPlainMode();


	/// <summary>
	/// Устанавливает режим простого текста редактора
	/// </summary>
	/// <param name="Mode">Устанавливаемое значение</param>
	/// <returns>Установленное значение</returns>
	int SetPlainMode([In] int Mode);


	/// <summary>
	/// Проверяет, что редактор находится в режиме терминала
	/// </summary>
	/// <returns>Признак режима терминала</returns>
	int GetTerminalMode();


	/// <summary>
	/// Устанавливает режим терминала
	/// </summary>
	/// <param name="Mode">Устанавливаемое значение</param>
	/// <returns>Установленное значение</returns>
	int SetTerminalMode([In] int Mode);


	/// <summary>
	/// Проверяет, что редактор находится в режиме вставки
	/// </summary>
	/// <returns>Признак режима вставки</returns>
	int GetInsertMode();


	/// <summary>
	/// Устанавливает режим вставки
	/// </summary>
	/// <param name="Mode">Устанавливаемое значение</param>
	/// <returns>Установленное значение</returns>
	int SetInsertMode([In] int Mode);


	/// <summary>
	/// Возвращает количество пробелов, использующихся для замены символов табуляции
	/// </summary>
	/// <returns>Количество пробелов</returns>
	int GetTabWidth();


	/// <summary>
	/// Устанавливает количество пробелов для замены символов табуляции
	/// </summary>
	/// <param name="Value">Количество пробелов</param>
	void SetTabWidth([In] int Value);


	/// <summary>
	/// Передаёт геометрические размеры областей редактора
	/// </summary>
	/// <param name="ActualWidth">Ширина окна редактора</param>
	/// <param name="ActualHeight">Высота окна редактора</param>
	/// <param name="PaddingWidth">Ширина левого отступа</param>
	/// <param name="RulerWidth">Ширина зоны нумерации строк</param>
	/// <param name="VerticalScrollBarWidth">Ширина вертикальной полосы прокрутки</param>
	/// <param name="HorizontalScrollBarHeight">Высота горизонтальной полосы прокрутки</param>
	/// <param name="CharWidth">Ширина одного символа</param>
	/// <param name="CharHeight">Высота одного символа</param>
	/// <param name="VisibleLineCount">Количество строк текста в видимой области</param>
	/// <param name="VisibleCharCount">Количество символов в видимой области</param>
	void GeometryReport([In] int ActualWidth,
		[In] int ActualHeight,
		[In] int PaddingWidth,
		[In] int RulerWidth,
		[In] int VerticalScrollBarWidth,
		[In] int HorizontalScrollBarHeight,
		[In] int CharWidth,
		[In] int CharHeight,
		[In] int VisibleLineCount,
		[In] int VisibleCharCount);


	/// <summary>
	/// Событие получения фокуса
	/// </summary>
	/// <returns>Признак обработки события</returns>
	int OnFocused();


	/// <summary>
	/// Событие потери фокуса
	/// </summary>
	int OnDefocused();


	/// <summary>
	/// Событие нажатия на клавишу
	/// </summary>
	/// <param name="Key">Код клавиши</param>
	/// <param name="Shift">Состояние клавиш-переключателей</param>
	/// <returns>Признак обработки события</returns>
	int OnKeyDown([In] int Key,
		[In] int Shift);


	/// <summary>
	/// Событие отпускания клавиши
	/// </summary>
	/// <param name="Key">Код клавиши</param>
	/// <param name="Shift">Состояние клавиш-переключателей</param>
	/// <returns>Признак обработки события</returns>
	int OnKeyUp([In] int Key,
		[In] int Shift);


	/// <summary>
	/// Событие ввода текста
	/// </summary>
	/// <param name="Str">Введённый текст</param>
	/// <param name="Shift">Состояние клавиш-переключателей</param>
	/// <returns>Признак обработки события</returns>
	int OnTextInput([In] string Str,
		[In] int Shift);


	/// <summary>
	/// Событие прихода курсора мыши
	/// </summary>
	/// <param name="Shift">Состояние клавиш-переключателей</param>
	/// <returns>Признак обработки события</returns>
	int OnMouseEnter([In] int Shift);


	/// <summary>
	/// Событие ухода курсора мыши
	/// </summary>
	/// <param name="Shift">Состояние клавиш-переключателей</param>
	/// <returns>Признак обработки события</returns>
	int OnMouseLeave([In] int Shift);


	/// <summary>
	/// Событие движения мыши
	/// </summary>
	/// <param name="X">Горизонтальная координата курсора</param>
	/// <param name="Y">Вертикальная координата курсора</param>
	/// <param name="Shift">Состояние клавиш-переключателей</param>
	/// <returns>Признак обработки события</returns>
	int OnMouseMove([In] int X,
		[In] int Y,
		[In] int Shift);


	/// <summary>
	/// Событие нажатия кнопки мыши
	/// </summary>
	/// <param name="Button">Нажатая кнопка</param>
	/// <param name="X">Горизонтальная координата курсора</param>
	/// <param name="Y">Вертикальная координата курсора</param>
	/// <param name="Shift">Состояние клавиш-переключателей</param>
	/// <returns>Признак обработки события</returns>
	int OnMouseDown([In] int Button,
		[In] int X,
		[In] int Y,
		[In] int Shift);


	/// <summary>
	/// Событие отпускания кнопки мыши
	/// </summary>
	/// <param name="Button">Отпущенная кнопка</param>
	/// <param name="X">Горизонтальная координата курсора</param>
	/// <param name="Y">Вертикальная координата курсора</param>
	/// <param name="Shift">Состояние клавиш-переключателей</param>
	/// <returns>Признак обработки события</returns>
	int OnMouseUp([In] int Button,
		[In] int X,
		[In] int Y,
		[In] int Shift);


	/// <summary>
	/// Событие вращения колеса мыши
	/// </summary>
	/// <param name="Delta">Знак вращения</param>
	/// <param name="X">Горизонтальная координата курсора</param>
	/// <param name="Y">Вертикальная координата курсора</param>
	/// <param name="Shift">Состояние клавиш-переключателей</param>
	/// <returns>Признак обработки события</returns>
	int OnMouseWheel([In] int Delta,
		[In] int X,
		[In] int Y,
		[In] int Shift);


	/// <summary>
	/// Событие зависания мыши
	/// </summary>
	/// <param name="X">Горизонтальная координата курсора</param>
	/// <param name="Y">Вертикальная координата курсора</param>
	/// <param name="Shift">Состояние клавиш-переключателей</param>
	/// <param name="Model">Указатель на интерфейс модели для всплывающих подсказок при отладке</param>
	/// <returns>Признак обработки события</returns>
	int OnMouseHover([In] int X,
		[In] int Y,
		[In] int Shift,
		[In, MarshalAs(UnmanagedType.Interface)] IModel Model);


	/// <summary>
	/// Возвращает строку Cookie
	/// </summary>
	/// <param name="Name">Имя Cookie</param>
	/// <returns>Значение Cookie</returns>
	string GetCookie([In] string Name);


	/// <summary>
	/// Устанавливает строку Cookie
	/// </summary>
	/// <param name="Name">Имя Cookie</param>
	/// <param name="Value">Значение Cookie</param>
	void SetCookie([In] string Name,
		[In] string Value);


	/// <summary>
	/// Возвращает слово текста в указанной позиции
	/// </summary>
	/// <param name="Line">Номер строки</param>
	/// <param name="Char">Номер символа</param>
	/// <param name="Relative">Признак относительной позиции</param>
	/// <param name="Pos">Позиция символа в начале слова</param>
	/// <param name="Style">Стиль слова</param>
	/// <returns>Слово текста</returns>
	string GetWordAt([In] int Line,
		[In] int Char,
		[In] int Relative,
		[Out] out int Pos,
		[Out] out int Style);


	/// <summary>
	/// Возвращает символ в указанной позиции
	/// </summary>
	/// <param name="Line">Номер строки</param>
	/// <param name="Char">Номер символа</param>
	/// <param name="Relative">Признак относительной позиции</param>
	/// <param name="Style">Стиль слова</param>
	/// <returns>Символ</returns>
	string GetSymbolAt([In] int Line,
		[In] int Char,
		[In] int Relative,
		[Out] out int Style);


	/// <summary>
	/// Проверяет, что в строке содержится точка прерывания
	/// </summary>
	/// <param name="Line">Номер строки</param>
	/// <param name="Relative">Признак относительной позиции</param>
	/// <returns>Признак точки прерывания</returns>
	int IsBreakpoint([In] int Line,
		[In] int Relative);


	/// <summary>
	/// Изменяет статус точки прерывания в строке
	/// </summary>
	/// <param name="Line">Номер строки</param>
	/// <param name="Relative">Признак относительной позиции</param>
	void ToggleBreakpoint([In] int Line,
		[In] int Relative);


	/// <summary>
	/// Проверяет, что в редакторе имеется выделение
	/// </summary>
	/// <returns>Состояние выделения: 0 – нет выделения, 1 – простое выделение,
	/// 3 – выделение столбиком, 5 – полное выделение</returns>
	int HasSelection();


	/// <summary>
	/// Возвращает текст выделения
	/// </summary>
	/// <returns>Текст выделения</returns>
	string SelectedText();


	/// <summary>
	/// Возвращает границы выделения
	/// </summary>
	/// <param name="StartLine">Начальная строка</param>
	/// <param name="StartChar">Начальный символ</param>
	/// <param name="EndLine">Конечная строка</param>
	/// <param name="EndChar">Конечный символ</param>
	/// <returns>Состояние выделения: 1 – простое выделение,
	/// 3 – выделение столбиком, 5 – полное выделение</returns>
	int GetSelection([Out] out int StartLine,
		[Out] out int StartChar,
		[Out] out int EndLine,
		[Out] out int EndChar);


	/// <summary>
	/// Устанавливает полное выделение
	/// </summary>
	void SelectAll();


	/// <summary>
	/// Устанавливает выделение
	/// </summary>
	/// <param name="StartLine">Начальная строка</param>
	/// <param name="StartChar">Начальный символ</param>
	/// <param name="EndLine">Конечная строка</param>
	/// <param name="EndChar">Конечный символ</param>
	/// <param name="Column">Признак выделения столбиком</param>
	void SetSelection([In] int StartLine,
		[In] int StartChar,
		[In] int EndLine,
		[In] int EndChar,
		[In] int Column);


	/// <summary>
	/// Убирает выделение
	/// </summary>
	void Deselect();


	/// <summary>
	/// Проверяет возможность выделения всего текста
	/// </summary>
	/// <returns>Результат проверки</returns>
	int CanSelectAll();


	/// <summary>
	/// Проверяет возможность копирования текста в буфер обмена
	/// </summary>
	/// <returns>Результат проверки</returns>
	int CanCopy();


	/// <summary>
	/// Копирует выделенный текст в буфер обмена
	/// </summary>
	/// <param name="Mode">Режим добавления</param>
	void CopyToClipboard([In] int Mode);


	/// <summary>
	/// Проверяет возможность вырезания текста в буфер обмена
	/// </summary>
	/// <returns>Результат проверки</returns>
	int CanCut();


	/// <summary>
	/// Вырезает выделенный текст в буфер обмена
	/// </summary>
	/// <param name="Mode">Режим добавления</param>
	void CutToClipboard([In] int Mode);


	/// <summary>
	/// Проверяет возможность вставки текста
	/// </summary>
	/// <returns>Результат проверки</returns>
	int CanPaste();


	/// <summary>
	/// Вставляет текст из буфера обмена
	/// </summary>
	void PasteFromClipboard();


	/// <summary>
	/// Вставляет столбец текста из буфера обмена
	/// </summary>
	void PasteColumnFromClipboard();


	/// <summary>
	/// Вставляет столбец текста
	/// </summary>
	/// <param name="Text">Вставляемый текст</param>
	/// <param name="Select">Признак выделения вставленного текста</param>
	void PasteColumn([In] string Text,
		[In] int Select);


	/// <summary>
	/// Дублирует текущую строку или выделение
	/// </summary>
	void Duplicate();


	/// <summary>
	/// Сортирует выделение
	/// </summary>
	/// <param name="Cmp">Интерфейс обратного вызова, сравнивающий строки.
	/// При нулевом значении используется процедура логического сравнения</param>
	void Sort([In, MarshalAs(UnmanagedType.Interface)] ICompareString Cmp);


	/// <summary>
	/// Переводит выделенный текст в нижний регистр
	/// </summary>
	void LowerCase();


	/// <summary>
	/// Переводит выделенный текст в верхний регистр
	/// </summary>
	void UpperCase();


	/// <summary>
	/// Помечает выделенный текст как комментарий
	/// </summary>
	void Comment();


	/// <summary>
	/// Убирает знаки комментария
	/// </summary>
	void Uncomment();


	/// <summary>
	/// Обрамляет выделение указанными символами
	/// </summary>
	/// <param name="Chr">Обрамляющий символ</param>
	void Quote([In] int Chr);


	/// <summary>
	/// Сворачивает выделение
	/// </summary>
	/// <param name="Width">Ширина строки</param>
	void Fold([In] int Width);


	/// <summary>
	/// Разворачивает выделение
	/// </summary>
	/// <param name="Check">Признак проверки возможности операции</param>
	/// <returns>Результат проверки</returns>
	int Unfold([In] int Check);


	/// <summary>
	/// Выводит текст в редактор в режиме терминала
	/// <summary>
	void Write([In] string Text);


	/// <summary>
	/// Очищает редактор
	/// </summary>
	void Clear();


	/// <summary>
	/// Выполняет поиск текста
	/// </summary>
	/// <param name="Text">Искомый текст</param>
	/// <param name="Flags">Параметры поиска</param>
	int Find([In] string Text,
		[In] int Flags);


	/// <summary>
	/// Возвращает позицию найденного текста
	/// </summary>
	/// <param name="Line">Номер строки</param>
	/// <param name="Char">Номер символа</param>
	/// <param name="Len">Длина текста</param>
	void GetFoundTextPosition([Out] out int Line,
		[Out] out int Char,
		[Out] out int Len);


	/// <summary>
	/// Прекращает отображение найденного текста
	/// </summary>
	void FinishSearch();


	/// <summary>
	/// Заменяет найденный ранее методом Find текст на указанный
	/// </summary>
	/// <param name="Str">Новый текст</param>
	/// <param name="Flags">Параметры поика</param>
	void Replace([In] string Text,
		[In] int Flags);


	/// <summary>
	/// Заменяет все вхождения старого текста на новый
	/// </summary>
	/// <param name="Old">Старый текст</param>
	/// <param name="New">Новый текст</param>
	/// <param name="Flags">Параметры поиска</param>
	/// <returns>Количество выполненных замен</returns>
	int ReplaceAll([In] string Old,
		[In] string New,
		[In] int Flags);


	/// <summary>
	/// Ищет начало или конец блока, начиная с указанной позиции
	/// </summary>
	/// <param name="Line">Номер строки</param>
	/// <param name="Char">Номер символа</param>
	/// <param name="Relative">Признак относительной позиции</param>
	/// <param name="Dir">Направление поиска</param>
	/// <returns>Признак успеха</returns>
	int FindBlockEnd([In] int Line,
		[In] int Char,
		[In] int Relative,
		[In] int Dir);


	/// <summary>
	/// Форматирует документ
	/// </summary>
	void FormatDocument();


	/// <summary>
	/// Определяет точку объявления пользовательского идентификатора в указанной позиции
	/// </summary>
	/// <param name="Line">Номер строки</param>
	/// <param name="Char">Номер символа</param>
	/// <param name="Relative">Признак относительной позиции</param>
	/// <param name="Ident">Идентификатор</param>
	/// <param name="FileName">Имя файла, в котором объявлен идентификатор</param>
	/// <param name="FoundLine">Номер строки, в которой объявлен идентификатор</param>
	/// <param name="FoundChar">Позиция символа, в котором объявлен идентификатор</param>
	void FindDeclaration([In] int Line,
		[In] int Char,
		[In] int Relative,
		[In] string Ident,
		[Out] out string FileName,
		[Out] out int FoundLine,
		[Out] out int FoundChar);


	/// <summary>
	/// Заменяет текущее слово на указанное
	/// </summary>
	/// <param name="Str">Новый текст</param>
	void ReplaceCurrentWord([In] string Str);


	/// <summary>
	/// Запрос показа строки с ошибкой выполнения скрипта
	/// </summary>
	void DisplayError([In] int Line,
		[In] int Char);


	/// <summary>
	/// Запрос показа точки трассировки
	/// </summary>
	void DisplayTrace([In] int Line,
		[In] int Char);

}


/// <summary>
/// Интерфейс обратного вызова для рисования области построения в окне приложения
/// </summary>
[ComImport, Guid("98C50A6B-6F77-45F8-A84A-72EB780B9E86"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IArtboardPainter
{

	/// <summary>
	/// Устанавливает размеры области построения
	/// </summary>
	/// <param name="Width">Ширина области построения</param>
	/// <param name="Height">Высота области построения</param>
	void SetSize([In] double Width,
		[In] double Height);


	/// <summary>
	/// Начинает рисования нового пути
	/// </summary>
	/// <param name="Fill">Признак заполнения внутренней области пути</param>
	/// <param name="FillColor">Цвет заполнения (целое число в формате RGB)</param>
	/// <param name="Stroke">Признак наличия границы</param>
	/// <param name="StrokeColor">Цвет границы (целое число в формате RGB)</param>
	/// <param name="StrokeWidth">Ширина границы</param>
	/// <param name="StrokeDash">Стиль пунктира границы</param>
	void NewPath([In] int Fill,
		[In] int FillColor,
		[In] int Stroke,
		[In] int StrokeColor,
		[In] double StrokeWidth,
		[In] string StrokeDash);


	/// <summary>
	/// Смещает указатель пути в заданную точку
	/// </summary>
	/// <param name="X">Горизонтальная координата</param>
	/// <param name="Y">Вертикальная координата</param>
	void MoveTo([In] double X,
		[In] double Y);


	/// <summary>
	/// Рисует отрезок прямой в точку с заданными координатами
	/// </summary>
	/// <param name="X">Горизонтальная координата</param>
	/// <param name="Y">Вертикальная координата</param>
	void LineTo([In] double X,
		[In] double Y);


	/// <summary>
	/// Чертит отрезок кривой Безье 3-го порядка
	/// </summary>
	/// <param name="X0">Горизонтальная координата опорной точки в начале отрезка</param>
	/// <param name="Y0">Вертикальная координата опорной точки в начале отрезка</param>
	/// <param name="X1">Горизонтальная координата опорной точки в конце отрезка</param>
	/// <param name="Y1">Вертикальная координата опорной точки в конце отрезка</param>
	/// <param name="X1">Горизонтальная координата конца отрезка</param>
	/// <param name="Y1">Вертикальная координата конца отрезка</param>
	void CurveTo([In] double X0,
		[In] double Y0,
		[In] double X1,
		[In] double Y1,
		[In] double X2,
		[In] double Y2);


	/// <summary>
	/// Замыкает текущий путь
	/// </summary>
	void Close();


	/// <summary>
	/// Завершает рисование
	/// </summary>
	void Finish();

}


/// <summary>
/// Интерфейс подсистемы векторной графики
/// </summary>
[ComImport, Guid("41FEBB47-717E-441F-8043-B5AE1A212208"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IGraphicArtboard
{

	/// <summary>
	/// Возвращает размеры области построения
	/// </summary>
	/// <param name="Width">Ширина области построения</param>
	/// <param name="Height">Высота области построения</param>
	void GetSize([Out] out double Width,
		[Out] out double Height);


	/// <summary>
	/// Устанавливает размеры области построения
	/// </summary>
	/// <param name="Width">Ширина области построения</param>
	/// <param name="Height">Высота области построения</param>
	void Resize([In] double Width,
		[In] double Height);


	/// <summary>
	/// Возвращает признак заливки замкнутых контуров
	/// </summary>
	/// <returns>Признак заливки</returns>
	int GetFill();


	/// <summary>
	/// Устанавливает признак заливки замкнутых контуров
	/// </summary>
	/// <param name="Fill">Признак заливки</param>
	void SetFill([In] int Fill);


	/// <summary>
	/// Возвращает цвет заливки замкнутых контуров
	/// </summary>
	/// <param name="H">Оттенок цвета (0..255)</param>
	/// <param name="S">Насыщенность цвета (0..255)</param>
	/// <param name="L">Освещённость цвета (0..255)</param>
	void GetFillColor([Out] out int H,
		[Out] out int S,
		[Out] out int L);


	/// <summary>
	/// Устанавливает цвет заливки замкнутых контуров
	/// </summary>
	/// <param name="H">Оттенок цвета (0..255). При отрицательных значениях S и L передаётся RGB-код цвета</param>
	/// <param name="S">Насыщенность цвета (0..255)</param>
	/// <param name="L">Освещённость цвета (0..255)</param>
	void SetFillColor([In] int H,
		[In] int S,
		[In] int L);


	/// <summary>
	/// Возвращает признак рисования линий
	/// </summary>
	/// <returns>Признак рисования линий</returns>
	int GetStroke();


	/// <summary>
	/// Устанавливает признак рисования линий
	/// </summary>
	/// <param name="Stroke">Признак рисования линий</param>
	void SetStroke([In] int Stroke);


	/// <summary>
	/// Возвращает цвет линий
	/// <param name="H">Оттенок цвета (0..255)</param>
	/// <param name="S">Насыщенность цвета (0..255)</param>
	/// <param name="L">Освещённость цвета (0..255)</param>
	void GetStrokeColor([Out] out int H,
		[Out] out int S,
		[Out] out int L);


	/// <summary>
	/// Устанавливает цвет линий
	/// <param name="H">Оттенок цвета (0..255). При отрицательных значениях S и L передаётся RGB-код цвета</param>
	/// <param name="S">Насыщенность цвета (0..255)</param>
	/// <param name="L">Освещённость цвета (0..255)</param>
	void SetStrokeColor([In] int H,
		[In] int S,
		[In] int L);


	/// <summary>
	/// Возвращает толщину линий
	/// </summary>
	/// <returns>Толщина линий</returns>
	double GetStrokeWidth();


	/// <summary>
	/// Устанавливает толщину линий
	/// </summary>
	/// <param name="Width">Толщина линий</param>
	void SetStrokeWidth([In] double Width);


	/// <summary>
	/// Возвращает стиль пунктира линий
	/// </summary>
	/// <returns>Cтиль пунктира</returns>
	string GetStrokeDash();


	/// <summary>
	/// Устанавливает стиль пунктира линий
	/// </summary>
	/// <param name="Dash">Стиль пунктира (Записаный в строку массив вещественных чисел,
	/// в который содержатся длины заполненных областей с последующими
	/// пустыми зонами. В качестве разделителей используются пробелы)</param>
	void SetStrokeDash([In] string Dash);


	/// <summary>
	/// Возвращает высоту текста
	/// </summary>
	/// <returns>Высота текста</returns>
	double GetTextHeight();


	/// <summary>
	/// Устанавливает высоту текста
	/// </summary>
	/// <param name="Height">Высота текста</param>
	void SetTextHeight([In] double Height);


	/// <summary>
	/// Возвращает выравнивание текста
	/// </summary>
	/// <param name="X">Горизонтальное выравнивание</param>
	/// <param name="Y">Вертикальное выравнивание</param>
	void GetTextAlign([Out] out double X,
		[Out] out double Y);


	/// <summary>
	/// Устанавливает выравнивание текста
	/// </summary>
	/// <param name="X">Горизонтальное выравнивание</param>
	/// <param name="Y">Вертикальное выравнивание</param>
	void SetTextAlign([In] double X,
		[In] double Y);


	/// <summary>
	/// Возращает угол поворота текста
	/// </summary>
	/// <returns>Угол поворота текста</returns>
	double GetTextRotation();


	/// <summary>
	/// Устанавливает угол поворота текста
	/// </summary>
	/// <param name="Rotation">Угол поворота текста</param>
	void SetTextRotation([In] double Rotation);


	/// <summary>
	/// Возвращает имя шрифта
	/// </summary>
	/// <returns>Имя шрифта</returns>
	string GetFontName();


	/// <summary>
	/// Устанавливает имя шрифта
	/// </summary>
	/// <param name="Name">Имя шрифта</param>
	void SetFontName([In] string Name);


	/// <summary>
	/// Возвращает стиль шрифта
	/// </summary>
	/// <param name="Bold">Полужирное начертание</param>
	/// <param name="Italic">Курсивное начертание</param>
	void GetFontStyle([Out] out int Bold,
		[Out] out int Italic);


	/// <summary>
	/// Устанавливает стиль шрифта
	/// </summary>
	/// <param name="Bold">Полужирное начертание</param>
	/// <param name="Italic">Курсивное начертание</param>
	void SetFontStyle([In] int Bold,
		[In] int Italic);


	/// <summary>
	/// Возвращет размеры текста на области построения
	/// </summary>
	/// <param name="Text">Текст
	/// <param name="Width">Ширина текста</param>
	/// <param name="Height">Высота текста</param>
	void GetTextSize([In] string Text,
		[Out] out double Width,
		[Out] out double Height);


	/// <summary>
	/// Начинает рисовать новый путь
	/// </summary>
	void NewPath();


	/// <summary>
	/// Сдвигает указатель текущего пути в указанную точку
	/// </summary>
	/// <param name="X">Горизонтальная координата</param>
	/// <param name="Y">Вертикальная координата</param>
	void MoveTo([In] double X,
		[In] double Y);


	/// <summary>
	/// Чертит отрезок прямой из текущей точки в указанную
	/// </summary>
	/// <param name="X">Горизонтальная координата</param>
	/// <param name="Y">Вертикальная координата</param>
	void LineTo([In] double X,
		[In] double Y);


	/// <summary>
	/// Чертит отрезок кривой Безье 3-го порядка
	/// </summary>
	/// <param name="X0">Горизонтальная координата опорной точки в начале отрезка</param>
	/// <param name="Y0">Вертикальная координата опорной точки в начале отрезка</param>
	/// <param name="X1">Горизонтальная координата опорной точки в конце отрезка</param>
	/// <param name="Y1">Вертикальная координата опорной точки в конце отрезка</param>
	/// <param name="X1">Горизонтальная координата конца отрезка</param>
	/// <param name="Y1">Вертикальная координата конца отрезка</param>
	void CurveTo([In] double X0,
		[In] double Y0,
		[In] double X1,
		[In] double Y1,
		[In] double X2,
		[In] double Y2);


	/// <summary>
	/// Замыкает текущий путь
	/// </summary>
	void Close();


	/// <summary>
	/// Отображает в текущем пути текст
	/// </summary>
	/// <param name="X">Горизонтальная координата</param>
	/// <param name="Y">Вертикальная координата</param>
	/// <param name="Str">Текст</param>
	void Text([In] double X,
		[In] double Y,
		[In] string Str);


	/// <summary>
	/// Отображает в текущем пути прямоугольник
	/// </summary>
	/// <param name="X">Горизонтальная координата центра</param>
	/// <param name="Y">Вертикальная координата центра</param>
	/// <param name="Width">Ширина текста</param>
	/// <param name="Height">Высота текста</param>
	/// <param name="Rotation">Угол поворота</param>
	void Rect([In] double X,
		[In] double Y,
		[In] double Width,
		[In] double Height,
		[In] double Rotation);


	/// <summary>
	/// Отображает в текущем пути эллипс
	/// </summary>
	/// <param name="X">Горизонтальная координата центра</param>
	/// <param name="Y">Вертикальная координата центра</param>
	/// <param name="RX">Горизонтальный радиус</param>
	/// <param name="RY">Вертикальный радиус</param>
	/// <param name="Rotation">Угол поворота</param>
	void Ellipse([In] double X,
		[In] double Y,
		[In] double RX,
		[In] double RY,
		[In] double Rotation);


	/// <summary>
	/// Отображает в текущем пути остриё стрелки
	/// </summary>
	/// <param name="X">Горизонтальная координата вершины</param>
	/// <param name="Y">Вертикальная координата вершины</param>
	/// <param name="Width">Ширина острия</param>
	/// <param name="Height">Высота острия</param>
	/// <param name="Depth">Глубина выемки у «крыльев»</param>
	/// <param name="Rotation">Угол поворота</param>
	void Arrow([In] double X,
		[In] double Y,
		[In] double Width,
		[In] double Height,
		[In] double Depth,
		[In] double Rotation);


	/// <summary>
	/// Смещает все пути на указанное расстояние
	/// </summary>
	/// <param name="X">Горизонтальное смещение</param>
	/// <param name="Y">Вертикальное смещение</param>
	void Shift([In] double X,
		[In] double Y);


	/// <summary>
	/// Поворачивает все пути на указанный угол
	/// </summary>
	/// <param name="X">Горизонтальная координата центра вращения</param>
	/// <param name="Y">Вертикальная координата центра вращения</param>
	/// <param name="Rotation">Угол поворота</param>
	void Rotate([In] double X,
		[In] double Y,
		[In] double Rotation);


	/// <summary>
	/// Масштабирует все пути
	/// </summary>
	/// <param name="PX">Горизонтальная координата опорной точки</param>
	/// <param name="PY">Вертикальная координата опорной точки</param>
	/// <param name="SX">Горизонтальный масштаб</param>
	/// <param name="SY">Вертикальный масштаб</param>
	void Scale([In] double PX,
		[In] double PY,
		[In] double SX,
		[In] double SY);


	/// <summary>
	/// Трансформирует все пути
	/// </summary>
	/// <param name="PX">Горизонтальная координата опорной точки</param>
	/// <param name="PY">Вертикальная координата опорной точки</param>
	/// <param name="M11">Ячейка матрицы трансформации</param>
	/// <param name="M12">Ячейка матрицы трансформации</param>
	/// <param name="M21">Ячейка матрицы трансформации</param>
	/// <param name="M22">Ячейка матрицы трансформации</param>
	void Transform([In] double PX,
		[In] double PY,
		[In] double M11,
		[In] double M12,
		[In] double M21,
		[In] double M22);


	/// <summary>
	/// Искажает все пути
	/// </summary>
	/// <param name="X0">Горизонтальное искажение в левом нижнем углу</param>
	/// <param name="Y0">Вертикальное искажение в левом нижнем углу</param>
	/// <param name="X1">Горизонтальное искажение в правом нижнем углу</param>
	/// <param name="Y1">Вертикальное искажение в правом нижнем углу</param>
	/// <param name="X2">Горизонтальное искажение в правом верхнем углу</param>
	/// <param name="Y2">Вертикальное искажение в правом верхнем углу</param>
	/// <param name="X3">Горизонтальное искажение в левом верхнем углу</param>
	/// <param name="Y3">Вертикальное искажение в левом верхнем углу</param>
	void Distort([In] double X0,
		[In] double Y0,
		[In] double X1,
		[In] double Y1,
		[In] double X2,
		[In] double Y2,
		[In] double X3,
		[In] double Y3);


	/// <summary>
	/// Возвращает минимальные и максимальные координаты объектов картины
	/// </summary>
	/// <param name="MinX">Минимальная горизонтальная координата</param>
	/// <param name="MinY">Минимальная вертикальная координата</param>
	/// <param name="MaxX">Максимальная горизонтальная координата</param>
	/// <param name="MaxY">Максимальная вертикальная координата</param>
	void GetBounds([Out] out double MinX,
		[Out] out double MinY,
		[Out] out double MaxX,
		[Out] out double MaxY);


	/// <summary>
	/// Преобразует координаты объектов картины и её размеры, чтобы изображение
	// помещалось на картине с указанными полями
	/// </summary>
	/// <param name="Left">Левое поле</param>
	/// <param name="Right">Правое поле</param>
	/// <param name="Top">Верхнее поле</param>
	/// <param name="Bottom">Нижнее поле</param>
	void Fit([In] double Left,
		[In] double Right,
		[In] double Top,
		[In] double Bottom);


	/// <summary>
	/// Очищает картину
	/// </summary>
	void Clear();


	/// <summary>
	/// Помещает на картине объекты другой картины
	/// </summary>
	/// <param name="Source">Исходная картина</param>
	void Put([In, MarshalAs(UnmanagedType.Interface)] IGraphicArtboard Source);


	/// <summary>
	/// Сохраняет картину в файл
	/// </summary>
	/// <param name="FileName">Имя файла (допустимые расширения eps, svg, png, jpg, pdf, artboard)</param>
	void SaveToFile([In] string FileName);


	/// <summary>
	/// Загружает картину из двоичного файла
	/// </summary>
	/// <param name="FileName">Имя файла</param>
	void LoadFromFile([In] string FileName);


	/// <summary>
	/// Перебирает пути области построения для их отображения на внешнем носителе
	/// </summary>
	/// <param name="Painter">Интерфейс обратного вызова</param>
	void Paint([In, MarshalAs(UnmanagedType.Interface)] IArtboardPainter Painter);

}


/// <summary>
/// Интерфейс рисования диаграмм
/// </summary>
[ComImport, Guid("23ADDA67-0FE6-4FB2-B0B0-4FD02C350845"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IDiagramBuilder
{

	/// <summary>
	/// Возвращает количество рядов диаграммы
	/// </summary>
	/// <returns>Количество рядов</returns>
	int GetRowCount();


	/// <summary>
	/// Добавляет новый ряд к диаграмме и возвращает его индекс
	/// </summary>
	/// <returns>Индекс нового ряда диаграммы</returns>
	int AddRow();


	/// <summary>
	/// Возвращает массив точек ряда
	/// </summary>
	/// <param name="Index">Индекс ряда</param>
	/// <returns>Массив точек</returns>
	IRealArray GetRowPoints([In] int Index);


	/// <summary>
	/// Устанавливает массив точек ряда
	/// </summary>
	/// <param name="Index">Индекс ряда</param>
	/// <param name="Points">Массив точек</param>
	void SetRowPoints([In] int Index,
		[In, MarshalAs(UnmanagedType.Interface)] IRealArray Points);


	/// <summary>
	/// Возвращает цвет ряда
	/// </summary>
	/// <param name="Index">Индекс ряда</param>
	/// <param name="H">Оттенок цвета</param>
	/// <param name="S">Насыщенность цвета</param>
	/// <param name="L">Освещенность цвета</param>
	void GetRowColor([In] int Index,
		[Out] out int H,
		[Out] out int S,
		[Out] out int L);


	/// <summary>
	/// Устанавливает цвет ряда
	/// </summary>
	/// <param name="Index">Индекс ряда</param>
	/// <param name="H">Оттенок цвета</param>
	/// <param name="S">Насыщенность цвета</param>
	/// <param name="L">Освещенность цвета</param>
	void SetRowColor([In] int Index,
		[In] int H,
		[In] int S,
		[In] int L);


	/// <summary>
	/// Возвращает заголовок ряда
	/// </summary>
	/// <param name="Index">Индекс ряда</param>
	/// <returns>Заголовок ряда</returns>
	string GetRowCaption([In] int Index);


	/// <summary>
	/// Устанавливает заголовок ряда
	/// </summary>
	/// <param name="Index">Индекс ряда</param>
	/// <param name="Caption">Заголовок ряда</param>
	void SetRowCaption([In] int Index,
		[In] string Caption);


	/// <summary>
	/// Возвращает стиль пунктира ряда
	/// </summary>
	/// <param name="Index">Индекс ряда</param>
	/// <returns>Стиль пунктира</returns>
	IRealArray GetRowDash([In] int Index);


	/// <summary>
	/// Устанавливает стиль пунктира ряда
	/// <param name="Index">Индекс ряда</param>
	/// <param name="Dash">Стиль пунктира</param>
	void SetRowDash([In] int Index,
		[In, MarshalAs(UnmanagedType.Interface)] IRealArray Dash);


	/// <summary>
	/// Возвращает признак свободного ряда (содержащего в массиве точек абсциссы и ординаты,
	/// отображающегося без привязки к вертикальной сетке
	/// </summary>
	/// <param name="Index">Индекс ряда</param>
	/// <returns>Признак свободного ряда</returns>
	int GetRowFree([In] int Index);


	/// <summary>
	/// Устанавливает признак свободного ряда
	/// </summary>
	/// <param name="Index">Индекс ряда</param>
	/// <param name="Free">Признак свободного ряда</param>
	void SetRowFree([In] int Index,
		[In] int Free);


	/// <summary>
	/// Возвращает копию массива горизонтальных координат диаграммы
	/// </summary>
	/// <returns>Массив горизонтальных координат</returns>
	IRealArray GetXCoords();


	/// <summary>
	/// Устанавливает массив горизонтальных координат диаграммы
	/// </summary>
	/// <param name="XCoord">Массив горизонтальных координат</param>
	void SetXCoords([In, MarshalAs(UnmanagedType.Interface)] IRealArray XCoords);


	/// <summary>
	/// Возвращает копию массива надписей по горизонтальной оси диаграммы
	/// </summary>
	/// <returns>Массив надписей</returns>
	string GetXCaptions();


	/// <summary>
	/// Устанавливает массив надписей по горизонтальной оси диаграммы
	/// </summary>
	/// <param name="XCaptions">Массив надписей</param>
	void SetXCaptions([In] string XCaptions);


	/// <summary>
	/// Возвращает приблизительное количество горизонтальных линий сетки
	/// </summary>
	/// <returns>Количество линий</returns>
	int GetHorizontalLineCount();


	/// <summary>
	/// Устанавливает приблизительное количество горизонтальных линий сетки
	/// </summary>
	/// <param name="N">Количество линий</para,>
	void SetHorizontalLineCount([In] int N);


	/// <summary>
	/// Возвращает формат преобразования вещественных чисел в строку
	/// </summary>
	/// <returns>Формат преобразования</returns>
	string GetRealFmt();


	/// <summary>
	/// Устанавливает формат преобразования вещественных чисел в строку
	/// </summary>
	/// <param name="Fmt">Формат преобразования</param>
	void SetRealFmt([In] string Fmt);


	/// <summary>
	/// Возвращает размеры диаграммы
	/// </summary>
	/// <param name="Width">Ширина диаграммы</param>
	/// <param name="Height">Высота диаграммы</param>
	/// <param name="Margin">Приблизительная ширина полей диаграммы</param>
	void GetSize([Out] out double Width,
		[Out] out double Height,
		[Out] out double Margin);


	/// <summary>
	/// Устанавливает размеры диаграммы (значения, равные inf или qnan, не устанавливаются)
	/// </summary>
	/// <param name="Width">Ширина диаграммы</param>
	/// <param name="Height">Высота диаграммы</param>
	/// <param name="Margin">Приблизительная ширина полей диаграммы</param>
	void Resize([In] double Width,
		[In] double Height,
		[In] double Margin);


	/// <summary>
	/// Возвращает заголовок диаграммы
	/// </summary>
	/// <returns>Заголовок диаграммы</returns>
	string GetCaption();


	/// <summary>
	/// Устанавливает заголовок диаграммы
	/// </summary>
	/// <param name="Caption">Заголовок диаграммы</param>
	void SetCaption([In] string Caption);


	/// <summary>
	/// Возвращает параметры заголовка диаграммы
	/// </summary>
	/// <param name="AlignX">Горизонтальное выравнивание</param>
	/// <param name="AlignY">Вертикальное выравнивание</param>
	/// <param name="X">Горизонтальная координата</param>
	/// <param name="Y">Вертикальная координата</param>
	/// <param name="Height">Размер шрифта</param>
	void GetCaptionParams([Out] out double AlignX,
		[Out] out double AlignY,
		[Out] out double X,
		[Out] out double Y,
		[Out] out double Height);


	/// <summary>
	/// Устанавливает параметры заголовка диаграммы (значения, равные inf или qnan, не устанавливаются)
	/// </summary>
	/// <param name="AlignX">Горизонтальное выравнивание</param>
	/// <param name="AlignY">Вертикальное выравнивание</param>
	/// <param name="X">Горизонтальная координата</param>
	/// <param name="Y">Вертикальная координата</param>
	/// <param name="Height">Размер шрифта</param>
	void SetCaptionParams([In] double AlignX,
		[In] double AlignY,
		[In] double X,
		[In] double Y,
		[In] double Height);


	/// <summary>
	/// Возвращает признак отображения легенды диаграммы
	/// </summary>
	/// <returns>Признак отображения легенды диаграммы</returns>
	int GetLegend();


	/// <summary>
	/// Устанавливает признак отображения легенды диаграммы
	/// </summary>
	/// <param name="Legend">Признак отображения легенды диаграммы</param>
	void SetLegend([In] int Legend);


	/// <summary>
	/// Возвращает параметры легенды диаграммы
	/// </summary>
	/// <param name="AlignX">Горизонтальное выравнивание</param>
	/// <param name="AlignY">Вертикальное выравнивание</param>
	/// <param name="X">Горизонтальная координата</param>
	/// <param name="Y">Вертикальная координата</param>
	/// <param name="Height">Размер шрифта</param>
	void GetLegendParams([Out] out double AlignX,
		[Out] out double AlighY,
		[Out] out double X,
		[Out] out double Y,
		[Out] out double Height);


	/// <summary>
	/// Устанавливает параметры легенды диаграммы (значения, равные inf или qnan, не устанавливаются)
	/// </summary>
	/// <param name="AlignX">Горизонтальное выравнивание</param>
	/// <param name="AlignY">Вертикальное выравнивание</param>
	/// <param name="X">Горизонтальная координата</param>
	/// <param name="Y">Вертикальная координата</param>
	/// <param name="Height">Размер шрифта</param>
	void SetLegendParams([In] double AlignX,
		[In] double AlighY,
		[In] double X,
		[In] double Y,
		[In] double Height);


	/// <summary>
	/// Возвращает ширину линий диаграммы
	/// </summary>
	/// <param name="Row">Ширина линий рядов</param>
	/// <param name="Grid">Ширина линий сетки</param>
	void GetLineWidth([Out] out double Row,
		[Out] out double Grid);


	/// <summary>
	/// Устанавливает ширину линий диаграммы
	/// </summary>
	/// <param name="Row">Ширина линий рядов</param>
	/// <param name="Grid">Ширина линий сетки</param>
	void SetLineWidth([In] double Row,
		[In] double Grid);


	/// <summary>
	/// Возвращает начальные значения для поиска минимальной и максимальной ординат диаграммы
	/// </summary>
	/// <param name="Min">Начальное значение для поиска минимальной ординаты</param>
	/// <param name="Max">Начальное значение для поиска максимальной ординаты</param>
	void GetOrdinateBounds([Out] out double Min,
		[Out] out double Max);


	/// <summary>
	/// Устанавливает начальные значения для поиска минимальной и максимальной ординат диаграммы
	/// </summary>
	/// <param name="Min">Начальное значение для поиска минимальной ординаты</param>
	/// <param name="Max">Начальное значение для поиска максимальной ординаты</param>
	void SetOrdinateBounds([In] double Min,
		[In] double Max);


	/// <summary>
	/// Возвращает признак обратного знака диаграммы (отрицательные ординаты вверху)
	/// </summary>
	/// <returns>Признак обратного знака диаграммы</returns>
	int GetInverse();


	/// <summary>
	/// Устанавливает признак обратного знака диаграммы (отрицательные ординаты вверху)
	/// </summary>
	/// <param name="Inverse">Признак обратного знака диаграммы</param>
	void SetInverse([In] int Inverse);


	/// <summary>
	/// Возвращает признак представления данных в виде гистограммы
	/// </summary>
	/// <returns>Пизнак гистограммы</returns>
	int GetBarStyle();


	/// <summary>
	/// Устанавливает признак представления данных в виде гистограммы
	/// <param name="BarStyle">Пизнак гистограммы</param>
	void SetBarStyle([In] int BarStyle);


	/// <summary>
	/// Отображает диаграмму на картине
	/// </summary>
	/// <param name="Artboard">Интерфейс картины</param>
	/// <param name="X">Горизонтальная координата левого нижнего угла диаграммы на картине</param>
	/// <param name="Y">Вертикальная координата левого нижнего угла диаграммы на картине</param>
	void Draw([In, MarshalAs(UnmanagedType.Interface)] IGraphicArtboard Artboard,
		[In] double X,
		[In] double Y);

}


/// <summary>
///	Интерфейс стиля ячейки таблицы
/// </summary>
[ComImport, Guid("937B18D5-BB56-453D-9FA2-1785D18B0FDD"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface ICellStyle
{

	/// <summary>
	///	Возвращает идентификатор стиля оттенка цвета фона
	/// </summary>
	long ColorHue();


	/// <summary>
	///	Возвращает идентификатор стиля насыщенности цвета фона
	/// </summary>
	long ColorSaturation();


	/// <summary>
	///	Возвращает идентификатор стиля освещённости цвета фона
	/// </summary>
	long ColorLightness();


	/// <summary>
	///	Возвращает идентификатор стиля оттенка цвета символов
	/// </summary>
	long TextHue();


	/// <summary>
	///	Возвращает идентификатор стиля насыщенности цвета символов
	/// </summary>
	long TextSaturation();


	/// <summary>
	///	Возвращает идентификатор стиля освещённости цвета символов
	/// </summary>
	long TextLightness();


	/// <summary>
	///	Возвращает идентификатор стиля левой границы ячейки
	/// </summary>
	long LeftBorder();


	/// <summary>
	///	Возвращает идентификатор стиля верхней границы ячейки
	long TopBorder();


	/// <summary>
	///	Возвращает идентификатор стиля правой границы ячейки
	/// </summary>
	long RightBorder();


	/// <summary>
	///	Возвращает идентификатор стиля нижней границы ячейки
	/// </summary>
	long BottomBorder();


	/// <summary>
	///	Возващает идентификатор стиля вертикального выравнивания
	long VerticalAlign();


	/// <summary>
	///	Возващает идентификатор стиля горизонтального выравнивания
	/// </summary>
	long HorizontalAlign();


	/// <summary>
	///	Возвращает идентификатор стиля полужирного начертания символов
	long FontBold();


	/// <summary>
	///	Возвращает идентификатор стиля курсивного начертания символов
	/// </summary>
	long FontItalic();


	/// <summary>
	///	Возвращает идентификатор стиля имени шрифта
	/// </summary>
	long FontName();


	/// <summary>
	///	Возвращает идентификатор стиля формата преобразования чисел в строку
	/// </summary>
	long Format();


	/// <summary>
	///	Возвращает идентификатор стиля размера шрифта
	/// </summary>
	long FontSize();


	/// <summary>
	///	Возвращает идентификатор стиля количества точек после запятой
	/// </summary>
	long Precision();


	/// <summary>
	///	Проверяет наличие установленного значения параметра стиля
	/// </summary>
	/// <param name="Id">Идентификатор параметра стиля</param>
	/// <returns>Признак установленного значения</returns>
	int IsParamActive([In] long Id);


	/// <summary>
	///	Очищает значение параметра стиля
	/// </summary>
	/// <param name="Id">Идентификатор параметра стиля</param>
	void ResetParam([In] long Id);


	/// <summary>
	///	Возвращает значение параметра стиля
	/// </summary>
	/// <param name="Id">Идентификатор параметра стиля (не допускается передача параметров FontName и Format)</param>
	/// <returns>Значение параметра</returns>
	int GetParamValue([In] long Id);


	/// <summary>
	///	Устанавливает значение параметра стиля
	/// </summary>
	/// <param name="Id">Идентификатор параметра стиля (не допускается передача параметров FontName и Format)</param>
	/// <param name="Value">Значение параметра</param>
	void SetParamValue([In] long Id,
		[In] int Value);


	/// <summary>
	///	Возвращает строковое значние параметра стиля
	/// </summary>
	/// <param name="Id">Идентификатор параметра стиля (FontName или Format)</param>
	/// <returns>Значение параметра</returns>
	string GetParamString([In] long Id);


	/// <summary>
	///	Устанавливает строковое значние параметра стиля
	/// </summary>
	/// <param name="Id">Идентификатор параметра стиля (FontName или Format)</param>
	/// <param name="Value">Значение параметра</param>
	void SetParamString([In] long Id,
		[In] string Value);

}


/// <summary>
///	Интерфейс ячейки таблицы
[ComImport, Guid("09DEDFBD-7701-44E4-8E39-241B5E3E2D3F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface ISheetCell
{

	/// <summary>
	///	Возвращает указатель на интерфейс стиля ячейки
	/// </summary>
	/// <returns>Стиль ячейки</returns>
	ICellStyle GetStyle();


	/// <summary>
	///	Возвращает текстовое значение ячейки
	/// </summary>
	/// <returns>Значение ячейки</returns>
	string GetString();


	/// <summary>
	///	Устанавливает текстовое значение ячейки
	/// </summary>
	/// <param name="Value">Значение ячейки</returns>
	void SetString([In] string Value);


	/// <summary>
	///	Возвращает вещественное значение ячейки
	/// </summary>
	/// <returns>Значение ячейки</returns>
	double GetReal();


	/// <summary>
	///	Устанавливает вещественное значение ячейки
	/// </summary>
	/// <param name="Value">Значение ячейки</returns>
	void SetReal([In] double Value);

}


/// <summary>
///	Интерфейс листа таблицы
[ComImport, Guid("8A8D68CD-574C-433E-A4D6-1A6E1DC045E3"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface ITableSheet
{

	/// <summary>
	/// Копирует данные другого листа
	/// </summary>
	/// <param name="Source">Лист-источник данных</param>
	void Assign([In, MarshalAs(UnmanagedType.Interface)] ITableSheet Source);


	/// <summary>
	///	Возвращает указатель на интерфейс стиля листа по умолчанию
	/// </summary>
	/// <returns>Стиль листа</returns>
	ICellStyle GetStyle();


	/// <summary>
	///	Возвращает имя листа
	/// </summary>
	/// <returns>Имя листа</returns>
	string GetName();


	/// <summary>
	///	Устанавливает имя листа
	/// </summary>
	/// <param name="Name">Имя листа</param>
	void SetName([In] string Name);


	/// <summary>
	///	Возвращает размеры листа
	/// </summary>
	/// <param name="RowCount">Количество строк</param>
	/// <param name="ColCount">Количество столбцов</param>
	void GetSize([Out] out int RowCount,
		[Out] out int ColCount);


	/// <summary>
	///	Устанавливает размеры листа
	/// </summary>
	/// <param name="RowCount">Количество строк</param>
	/// <param name="ColCount">Количество столбцов</param>
	void Resize([In] int RowCount,
		[In] int ColCount);


	/// <summary>
	///	Вставляет строки листа
	/// </summary>
	/// <param name="Row">Номер строки</param>
	/// <param name="Count">Количество вставляемых строк</param>
	void InsertRows([In] int Row,
		[In] int Count);


	/// <summary>
	///	Добавляет новые строки на лист
	/// </summary>
	/// <param name="Count">Количество добавляемых строк</param>
	void AddRows([In] int Count);


	/// <summary>
	///	Удаляет строки
	/// </summary>
	/// <param name="Row">Номер строки</param>
	/// <param name="Count">Количество удаляемых строк</param>
	void DeleteRows([In] int Row,
		[In] int Count);


	/// <summary>
	///	Вставляет столбцы листа
	/// </summary>
	/// <param name="Col">Номер столбца</param>
	/// <param name="Count">Количество вставляемых столбцов</param>
	void InsertCols([In] int Col,
		[In] int Count);


	/// <summary>
	///	Добавляет новые столбцы на лист
	/// </summary>
	/// <param name="Count">Количество добавляемых столбцов</param>
	void AddCols([In] int Count);


	/// <summary>
	///	Удаляет столбцы
	/// </summary>
	/// <param name="Col">Номер столбца</param>
	/// <param name="Count">Количество удаляемых столбцов</param>
	void DeleteCols([In] int Col,
		[In] int Count);


	/// <summary>
	///	Возвращает указатель на интерфейс ячейки
	/// </summary>
	/// <param name="Row">Номер строки</param>
	/// <param name="Col">Номер столбца</param>
	/// <returns>Интерфейс ячейки</returns>
	ISheetCell GetCell([In] int Row,
		[In] int Col);


	/// <summary>
	///	Возвращает указатель на интерфейс стиля ячейки
	/// </summary>
	/// <param name="Row">Номер строки</param>
	/// <param name="Col">Номер столбца</param>
	/// <returns>Интерфейс стиля ячейки</returns>
	ICellStyle GetCellStyle([In] int Row,
		[In] int Col);


	/// <summary>
	///	Возвращает текстовое значение ячейки
	/// </summary>
	/// <param name="Row">Номер строки</param>
	/// <param name="Col">Номер столбца</param>
	/// <returns>Значение ячейки</returns>
	string GetCellString([In] int Row,
		[In] int Col);


	/// <summary>
	///	Устанавливает текстовое значение ячейки
	/// </summary>
	/// <param name="Row">Номер строки</param>
	/// <param name="Col">Номер столбца</param>
	/// <param name="Str">Значение ячейки</param>
	void SetCellString([In] int Row,
		[In] int Col,
		[In] string Str);


	/// <summary>
	///	Возвращает вещественное значение ячейки
	/// </summary>
	/// <param name="Row">Номер строки</param>
	/// <param name="Col">Номер столбца</param>
	/// <returns>Значение ячейки</returns>
	double GetCellReal([In] int Row,
		[In] int Col);


	/// <summary>
	///	Устанавливает вещественное значение ячейки
	/// </summary>
	/// <param name="Row">Номер строки</param>
	/// <param name="Col">Номер столбца</param>
	/// <param name="Value">Значение ячейки</param>
	void SetCellReal([In] int Row,
		[In] int Col,
		[In] double Value);


	/// <summary>
	///	Возвращает размеры ячейки по умолчанию
	/// </summary>
	/// <param name="RowHeight">Высота строки</param>
	/// <param name="ColWidth">Ширина столбца</param>
	void GetDefaultCellSize([Out] out int RowHeight,
		[Out] out int ColWidth);


	/// <summary>
	///	Устанавливает размеры ячейки по умолчанию (отрицательные значения не устанавливаются)
	/// </summary>
	/// <param name="RowHeight">Высота строки</param>
	/// <param name="ColWidth">Ширина столбца</param>
	void SetDefaultCellSize([In] int RowHeight,
		[In] int ColWidth);


	/// <summary>
	///	Возвращает высоту строки таблицы
	/// </summary>
	/// <param name="Row">Номер строки</param>
	/// <returns>Высота строки</returns>
	int GetRowHeight([In] int Row);


	/// <summary>
	///	Устанавливает высоту строки таблицы
	/// </summary>
	/// <param name="Row">Номер строки</param>
	/// <param name="Height">Высота строки</param>
	void SetRowHeight([In] int Row,
		[In] int Height);


	/// <summary>
	///	Возвращает ширину столбца таблицы
	/// </summary>
	/// <param name="Col">Номер столбца</param>
	/// <returns>Width</returns>
	int GetColWidth([In] int Col);


	/// <summary>
	///	Устанавливает ширину столбца таблицы
	/// </summary>
	/// <param name="Col">Номер столбца</param>
	/// <param name="Width">Ширина столбца</param>
	void SetColWidth([In] int Col,
		[In] int Width);


	/// <summary>
	///	Устанавливает стиль диапазона ячеек
	/// </summary>
	/// <param name="R1">Начальная строка</param>
	/// <param name="C1">Начальный столбец</param>
	/// <param name="R2">Конечная стока</param>
	/// <param name="C2">Конечный столбец</param>
	/// <param name="Style">Cтиль ячеек</param>
	void SetRangeStyle([In] int R1,
		[In] int C1,
		[In] int R2,
		[In] int C2,
		[In, MarshalAs(UnmanagedType.Interface)] ICellStyle Style);


	/// <summary>
	///	Устанавливает стиль границы диапазона ячеек
	/// </summary>
	/// <param name="R1">Начальная строка</param>
	/// <param name="C1">Начальный столбец</param>
	/// <param name="R2">Конечная стока</param>
	/// <param name="C2">Конечный столбец</param>
	/// <param name="Style">Cтиль ячеек</param>
	void SetRangeBorder([In] int R1,
		[In] int C1,
		[In] int R2,
		[In] int C2,
		[In, MarshalAs(UnmanagedType.Interface)] ICellStyle Style);


	/// <summary>
	///	Устанавливает объединение ячеек
	/// </summary>
	/// <param name="Row">Номер строки</param>
	/// <param name="Col">Номер столбца</param>
	/// <param name="VSpan">Количество объединяемых ячеек по вертикали</param>
	/// <param name="HSpan">Количество объединяемых ячеек по горизонтали</param>
	void SetSpan([In] int Row,
		[In] int Col,
		[In] int VSpan,
		[In] int HSpan);

}


/// <summary>
/// Интерфейс табличного вывода
/// </summary>
[ComImport, Guid("CF67C480-08BA-4577-BC62-019A275F99AB"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IOutputTable
{

	/// <summary>
	/// Возвращает указатель на интерфейс стиля таблицы по умолчанию
	/// </summary>
	/// <returns>Стиль таблицы</returns>
	ICellStyle GetStyle();


	/// <summary>
	/// Возвращает имя таблицы
	/// </summary>
	/// <returns>Имя таблицы</returns>
	string GetName();


	/// <summary>
	/// Устанавливает имя таблицы
	/// </summary>
	/// <param name="Name">Имя таблицы</param>
	void SetName([In] string Name);


	/// <summary>
	/// Создаёт стиль ячеек
	/// </summary>
	/// <returns>Стиль ячеек</returns>
	ICellStyle CreateCellStyle();


	/// <summary>
	/// Возвращает количество листов таблицы
	/// </summary>
	/// <returns>Количество листов</returns>
	int GetSheetCount();


	/// <summary>
	/// Возвращает лист таблицы
	/// </summary>
	/// <param name="Index">Номер листа</param>
	/// <returns>Интерфейс листа</returns>
	ITableSheet GetSheet([In] int Index);


	/// <summary>
	/// Создаёт лист таблицы
	/// </summary>
	/// <param name="Name">Имя листа</param>
	/// <param name="RowCount">Количество строк</param>
	/// <param name="ColCount">Количество столбцов</param>
	/// <returns>Интерфейс листа</returns>
	ITableSheet AddSheet([In] string Name,
		[In] int RowCount,
		[In] int ColCount);


	/// <summary>
	/// Удаляет лист таблицы
	/// </summary>
	/// <param name="Index">Номер листа</param>
	void DeleteSheet([In] int Index);


	/// <summary>
	/// Изменяет позицию листа
	/// </summary>
	/// <param name="OldIndex">Старый номер листа</param>
	/// <param name="NewIndex">Новый номер листа</param>
	void InsertSheet([In] int OldIndex,
		[In] int NewIndex);


	/// <summary>
	/// Сохраняет таблицу в файл
	/// </summary>
	/// <param name="FileName">Имя файла. Расширение определяет формат: txt – текст; xls, xml – Excel XML 2003; htm, html – HTML 5.0; остальное – внутренний двоичный формат</param>
	void SaveToFile([In] string FileName);


	/// <summary>
	/// Загружает таблицу из файла внутреннего двоичного формата
	/// </summary>
	/// <param name="FileName">Имя файла</param>
	void LoadFromFile([In] string FileName);

}


/// <summary>
/// Интерфейс вспомогательных функций библиотеки
/// </summary>
[ComImport, Guid("1E22A0DF-77BB-4061-B825-F5329E9EC97D"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IServer
{

	/// <summary>
	/// Возвращает указатель на интерфейс расчётной модели
	/// </summary>
	/// <returns>Интерфейс модели</returns>
	IModel CreateModel();


	/// <summary>
	/// Возвращает интерфейс настроек системы
	/// </summary>
	/// <param name="AppName">Имя приложения</param>
	/// <param name="AppVersion">Версия приложения (может быть пустой)</param>
	/// <param name="AppGuid">Глобальный идентификатор приложения (может быть пустым)</param>
	/// <param name="FileName">Имя файла конфигурации</param>
	/// <param name="Local">0 – использование локального профиля, 1 – использование перемещаемого</param>
	/// <returns>Указатель на интерфейс конфигурации</returns>
	ISettings LoadSettings([In] string AppName,
		[In] string AppVersion,
		[In] string AppGuid,
		[In] string FileName,
		[In] int Local);


	/// <summary>
	/// Возвращает интерфейс контурного сечения
	/// </summary>
	/// <returns>Указатель на интерфейс</returns>
	ISectionShape CreateSectionShape();


	/// <summary>
	/// Возвращает формат преобразования вещественных чисел в текст
	/// </summary>
	/// <param name="Separator">Десятичный разделитель</param>
	/// <param name="Precision">Точность преобразования</param>
	/// <param name="Style">Стиль преобразования (D, E, F, G, R)</param>
	void GetDefaultRealFormat([Out] out string Separator,
		[Out] out int Precision,
		[Out] out string Style);


	/// <summary>
	/// Устанавливает формат преобразования вещественных чисел в текст
	/// </summary>
	/// <param name="Separator">Десятичный разделитель</param>
	/// <param name="Precision">Точность преобразования</param>
	/// <param name="Style">Стиль преобразования (D, E, F, G, R)</param>
	void SetDefaultRealFormat([In] string Separator,
		[In] int Precision,
		[In] string Style);


	/// <summary>
	/// Преобразует вещественное число в текст с учётом текущего формата
	/// </summary>
	/// <param name="Value">Вещественное значение</param>
	/// <returns>Текстовое значение</returns>
	string RealToString([In] double Value);


	/// <summary>
	/// Преобразует вещественное число в текст с учётом указанного формата
	/// </summary>
	/// <param name="Value">Вещественное значение</param>
	/// <param name="Separator">Десятичный разделитель</param>
	/// <param name="Precision">Точность преобразования</param>
	/// <param name="Style">Стиль преобразования (D, E, F, G, R)</param>
	string RealToStringFmt([In] double Value,
		[In] string Separator,
		[In] int Precision,
		[In] string Style);


	/// <summary>
	/// Преобразует текст в вещественное число
	/// </summary>
	/// <param name="Value">Текстовое значение</param>
	/// <returns>Вещественное значение</returns>
	double StringToReal([In] string Value);


	/// <summary>
	/// Преобразует текст в вещественное число, не генерируя исключения в случае ошибки
	/// </summary>
	/// <param name="Value">Текстовое значение</param>
	/// <param name="Success">Признак успешного завершения</param>
	/// <returns>Вещественное значение</returns>
	double TryStringToReal([In] string Value,
		[Out] out int Success);


	/// <summary>
	/// Возвращает точность сравнения вещественных чисел
	/// </summary>
	/// <returns>Точность сравнения</returns>
	double GetEpsilon();


	/// <summary>
	/// Устанавливает точность сравнения вещественных чисел
	/// </summary>
	/// <param name="Eps">Точность сравнения</param>
	void SetEpsilon([In] double Eps);


	/// <summary>
	/// Выполняет сравнение двух вещественных чисел
	/// </summary>
	/// <param name="a">Первое число</param>
	/// <param name="b">Второе число</param>
	/// <returns>Знак разности двух чисел</returns>
	[PreserveSig()]
	int CompareReal([In] double a,
		[In] double b);


	/// <summary>
	/// Выполняет логическое сравнение двух строк
	/// </summary>
	/// <param name="a">Первая строка</param>
	/// <param name="b">Вторая строка</param>
	/// <returns>Результат сравнения</returns>
	[PreserveSig()]
	int CompareString([In] string a,
		[In] string b);


	/// <summary>
	/// Возвращает сгенерированное имя временного файла
	/// </summary>
	/// <param name="Folder">Имя папки (при пустой строке используется системная папка)</param>
	/// <param name="Ext">Расширение файла</param>
	/// <returns>Cгенерированное имя файла, который будет удалён при завершении работы</returns>
	string CreateTemporaryFile([In] string Folder,
		[In] string Ext);


	/// <summary>
	/// Возвращает сгенерированное имя временной папки
	/// </summary>
	/// <returns>Имя папки, которая будет удалена при завершении работы</returns>
	string CreateTemporaryFolder();


	/// <summary>
	/// Возвращает текущую временную папку
	/// </summary>
	/// <returns>Имя папки для временных файлов</returns>
	string GetCurrentTemporaryFolder();


	/// <summary>
	/// Устанавливает текущую временную папку
	/// </summary>
	/// <param name="Name">Имя папки для временных файлов</param>
	void SetCurrentTemporaryFolder([In] string Name);


	/// <summary>
	/// Возвращает текущую рабочую папку
	/// </summary>
	/// <returns>Имя папки</returns>
	string GetCWD();


	/// <summary>
	/// Устанавливает текущую рабочую папку
	/// </summary>
	/// <param name="Name">Имя папки</param>
	void SetCWD([In] string Name);


	/// <summary>
	///  Возвращает положение и размеры окна
	/// </summary>
	/// <param name="Handle">Дескриптор окна</param>
	/// <returns>Размеры окна (Left Top Right Bottom State)</returns>
	string GetWindowPlacement([In] long Handle);


	/// <summary>
	/// Корректирует размеры окна, чтобы оно не оказалось вне видимой области экрана при
	/// восстановлении его положения
	/// </summary>
	/// <param name="Placement"> – сохранённое положение окна, полученное от GetWindowPlacement</param>
	/// <param name="Left">Горизонтальная координата окна</param>
	/// <param name="Top">Вертикальная координата окна</param>
	/// <param name="Width">Ширина окна</param>
	/// <param name="Height">Высота окна</param>
	/// <param name="State">Состояние окна</param>
	void AdjustWindowPlacement([In] string Placement,
		[Out] out int Left,
		[Out] out int Top,
		[Out] out int Width,
		[Out] out int Height,
		[Out] out int State);


	/// <summary>
	/// Определяет, включено ли на системном уровне размытие экранных шрифтов
	/// </summary>
	/// <returns>Признак наличия размытия</returns>
	int GetFontSmoothing();


	/// <summary>
	/// Перечисляет список установленных в системе моноширинных шрифтов
	/// </summary>
	/// <param name="Enum">Интерфейс обратного вызова для перечисления шрифтов</param>
	void EnumFixedPitchFonts([In, MarshalAs(UnmanagedType.Interface)] IStringEnum Enum);


	/// <summary>
	/// Возвращает текстовое содержимое буфера обмена
	/// </summary>
	/// <returns>Cодержимое буфера обмена</returns>
	string GetClipboardText([In] int Mode);


	/// <summary>
	/// Устанавливает текстовое содержимое буфера обмена
	/// </summary>
	/// <param name="Text">Cодержимое буфера обмена</param>
	/// <param name="Mode">Режим вставки (Clipboard.)</param>
	void SetClipboardText([In] string Text,
		[In] int Mode);


	/// <summary>
	/// Создаёт объект редактора
	/// </summary>
	/// <returns>Указатель на интерфейс редактора</returns>
	IScriptEditor CreateEditor();


	/// <summary>
	/// Создаёт графическую область
	/// </summary>
	/// <returns>Указатель на интерфейс области</returns>
	IGraphicArtboard CreateGraphicArtboard();


	/// <summary>
	/// Создаёт построитель диаграмм
	/// </summary>
	IDiagramBuilder CreateDiagramBuilder();


	/// <summary>
	/// Возвращает список глобальных идентификаторов
	/// </summary>
	/// <returns>Указатель на интерфейс списка</returns>
	IIdentList GetGlobalIdentifiers();


	/// <summary>
	/// Создаёт новый многомерный массив данных — карту
	/// </summary>
	/// <param name="FileName">Имя файла данных. При пустой строке файл создаётся как временный</param>
	/// <param name="Idents">Идентификаторы карты. Уровни разделяются как строки, идентификаторы в строка разделяются пробелами</param>
	/// <param name="CheckId">Признак проверки правильности идентификаторов</param>
	/// <returns>Созданная карта с нулевыми значениями ячеек</returns>
	IMapObject CreateMap([In] string FileName,
		[In] string Idents,
		[In] int CheckId);


	/// <summary>
	/// Загружает карту из файла
	/// </summary>
	/// <param name="FileName">Имя файла</param>
	/// <returns>Загруженная карта</returns>
	IMapObject OpenMap([In] string FileName);


	/// <summary>
	/// Создаёт таблицу результатов
	/// </summary>
	/// <returns>Интерфейс таблицы</returns>
	IOutputTable CreateOutputTable();


	/// <summary>
	/// Создаёт объект для чтения данных чертежей в формате DXF
	/// </summary>
	/// <returns>Интерфейс чтения</returns>
	IDxfReader CreateDxfReader();


	/// <summary>
	/// Возвращает список поддерживаемых типов файлов для сохранения модели
	/// В списке представлены попарно типы расширений и их описания, разделённые знаками |
	/// <summary>
	/// <returns>Список типов файлов
	string GetSupportedExtensionList();


	/// <summary>
	/// Возвращает количество тактов процессора, прошедшее с момента его загрузки
	/// </summary>
	long Rdtsc();


	/// <summary>
	/// Возвращает строковое представление цвета dxf
	/// </summary>
	/// <param name="Color">Код цвета</param>
	/// <returns>Строковое представление</returns>
	string DxfColorToString([In] int Color);


	/// <summary>
	/// Возвращает dxf цвет по стровому представлению
	/// </summary>
	/// <param name="Str">Строковое представление</param>
	/// <param name="Color">Код цвета</param>
	/// <returns>Признак успеха операции</returns>
	int StringToDxfColor([In] string Str,
		[Out] out int Color);


	/// <summary>
	/// Возвращает RGB цвет по DXF цвету
	/// </summary>
	/// <param name="Dxf">Код цвета DXF</param>
	/// <returns>Код цвета RGB</returns>
	int DxfColorToRgbColor([In] int Dxf);

}


}