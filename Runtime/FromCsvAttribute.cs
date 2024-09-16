using System;

namespace RemoteCsv
{
    [AttributeUsage(AttributeTargets.Field)]
    public class FromCsvAttribute: Attribute
    {
        private readonly int _rowIndex;
        private readonly int _itemsCount;
        private readonly int _columnIndex;

        public int ColumnIndex => _columnIndex;
        public int RowIndex => _rowIndex;
        public int ItemsCount => _itemsCount;

        /// <param name="columnIndex">Target column with data</param>
        /// <param name="rowIndex">Override target row index. Starts from 1</param>
        /// <param name="itemsCount">Arrays only. Override target items count</param>
        public FromCsvAttribute(int columnIndex, int rowIndex = 0, int itemsCount = 0)
        {
            if (columnIndex < 0)
                _columnIndex = 0;
            else
                _columnIndex = columnIndex;

            if(rowIndex < 0)
                _rowIndex = rowIndex;
            else
                _rowIndex = rowIndex;

            if(itemsCount <= 0)
                _itemsCount = 0;
            else
                _itemsCount = itemsCount;
        }

        /// <param name="column">Target column with data</param>
        /// <param name="rowIndex">Override target row index. Starts from 1</param>
        /// <param name="itemsCount">Arrays only. Override target items count</param>
        public FromCsvAttribute(Column column, int rowIndex = 0, int itemsCount = 0)
        {
            _columnIndex = (int)column;

            if (rowIndex < 0)
                _rowIndex = rowIndex;
            else
                _rowIndex = rowIndex;

            if (itemsCount <= 0)
                _itemsCount = 0;
            else
                _itemsCount = itemsCount;
        }
    }
}

