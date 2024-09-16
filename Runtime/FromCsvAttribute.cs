using System;

namespace RemoteCsv
{
    [AttributeUsage(AttributeTargets.Field)]
    public class FromCsvAttribute: Attribute
    {
        private readonly int _rowStartIndex;
        private readonly int _rowEndIndex;
        private readonly int _columnIdnex;

        public int RowStart => _rowStartIndex;
        public int ItemsCount => _rowEndIndex;
        public int Column => _columnIdnex;

        /// <param name="columnIndex">Target column with data</param>
        /// <param name="rowIndex">Override target row index</param>
        /// <param name="itemsCount">For arrays only. Override target items count</param>
        public FromCsvAttribute(int columnIndex, int rowIndex = -1, int itemsCount = -1)
        {
            if (columnIndex < 0)
                _columnIdnex = 0;
            else
                _columnIdnex = columnIndex;

            _rowStartIndex = rowIndex;
            _rowEndIndex = itemsCount;
        }
    }
}

