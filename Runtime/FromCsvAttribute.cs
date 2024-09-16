using System;

namespace RemoteCsv
{
    [AttributeUsage(AttributeTargets.Field)]
    public class FromCsvAttribute : Attribute
    {
        private readonly int _row;
        private readonly int _column;
        private readonly int _itemsCount;

        public int Row => _row;
        public int Column => _column;
        public int ItemsCount => _itemsCount;

        /// <summary>
        /// Special for arrays of classes
        /// </summary>
        /// <param name="itemsCount">Arrays only. Override target items count</param>
        public FromCsvAttribute(int row, int itemsCount)
        {
            _column = 0;

            if (row < 0)
                _row = 0;
            else
                _row = row;

            if (itemsCount <= 0)
                _itemsCount = 0;
            else
                _itemsCount = itemsCount;
        }

        /// <param name="column">Target column with data. Starts from 1</param>
        /// <param name="row">Override target row index. Starts from 1</param>
        /// <param name="itemsCount">Arrays only. Override target items count</param>
        public FromCsvAttribute(int column = 0, int row = 0, int itemsCount = 0)
        {
            if (column <= 0)
                _column = 0;
            else
                _column = column - 1;

            if (row < 0)
                _row = 0;
            else
                _row = row;

            if (itemsCount <= 0)
                _itemsCount = 0;
            else
                _itemsCount = itemsCount;
        }

        /// <param name="column">Target column with data.</param>
        /// <param name="row">Override target row index. Starts from 1</param>
        /// <param name="itemsCount">Arrays only. Override target items count</param>
        public FromCsvAttribute(Column column = RemoteCsv.Column.A, int row = 0, int itemsCount = 0)
        {
            _column = (int)column - 1;

            if (row < 0)
                _row = 0;
            else
                _row = row;

            if (itemsCount <= 0)
                _itemsCount = 0;
            else
                _itemsCount = itemsCount;
        }
    }
}

