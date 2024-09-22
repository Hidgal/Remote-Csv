using System;
using RemoteCsv.Internal.Parsers;

namespace RemoteCsv
{
    [AttributeUsage(AttributeTargets.Field)]
    public partial class FromCsvAttribute : Attribute
    {
        private static Type _fieldParserType = typeof(IFieldParser);

        private readonly Type _customParserType;

        private int _row;
        private int _column;
        private int _itemsCount;

        public int RowIndex => _row;
        public int ColumnIndex => _column;
        public int ItemsCount => _itemsCount;
        public Type CustomParserType => _customParserType;

        /// <summary>
        /// Special for arrays of classes
        /// </summary>
        /// <param name="itemsCount">Arrays only. Override target items count if > 0</param>
        public FromCsvAttribute(int row, int itemsCount, Type customParserType = null)
        {
            _column = 0;
            
            if(customParserType != null)
            {
                if (_fieldParserType.IsAssignableFrom(customParserType))
                    _customParserType = customParserType;
            }

            SetRowIndex(row);
            SetItemsCount(itemsCount);
        }

        /// <param name="column">Target column with data. Starts from 1</param>
        /// <param name="row">Override target row index. Starts from 1</param>
        /// <param name="itemsCount">Arrays only. Override target items count</param>
        public FromCsvAttribute(int column, int row = 0, int itemsCount = 0, Type customParserType = null)
        {
            if (customParserType != null)
            {
                if (_fieldParserType.IsAssignableFrom(customParserType))
                    _customParserType = customParserType;
            }

            SetRowIndex(row);
            SetColumnIndex(column);
            SetItemsCount(itemsCount);
        }

        /// <param name="column">Target column with data.</param>
        /// <param name="row">Override target row index. Starts from 1</param>
        /// <param name="itemsCount">Arrays only. Override target items count</param>
        public FromCsvAttribute(Column column, int row = 0, int itemsCount = 0, Type customParserType = null)
        {
            _column = (int)column;

            if (customParserType != null)
            {
                if (_fieldParserType.IsAssignableFrom(customParserType))
                    _customParserType = customParserType;
            }

            SetRowIndex(row);
            SetItemsCount(itemsCount);
        }

        public void SetColumnIndex(int column)
        {
            if (column <= 0)
                _column = 0;
            else
                _column = column - 1;
        }

        public void SetRowIndex(int rowIndex)
        {
            if (rowIndex <= 0)
                _row = 0;
            else
                _row = rowIndex - 1;
        }

        public void SetItemsCount(int itemsCount)
        {
            if (itemsCount <= 0)
                _itemsCount = 0;
            else
                _itemsCount = itemsCount;
        }

        public FromCsvAttribute Clone()
        {
            return new FromCsvAttribute(_column + 1, _row + 1, _itemsCount, _customParserType);
        }
    }
}

