using System;
using System.Collections.Generic;
using System.Text;

using System.Web.UI.WebControls;

namespace YYControls
{
    /// <summary>
    /// GridViewTableCell
    /// </summary>
    public class GridViewTableCell
    {
        private TableCell _tableCell;
        private int _columnIndex;
        private DataControlRowType _rowType;
        private DataControlRowState _rowState;

        /// <summary>
        /// 构造函数
        /// </summary>
        public GridViewTableCell(TableCell tableCell, int columnIndex, DataControlRowType rowType, DataControlRowState rowState)
        {
            this._tableCell = tableCell;
            this._columnIndex = columnIndex;
            this._rowType = rowType;
            this._rowState = rowState;
        }

        /// <summary>
        /// TableCell
        /// </summary>
        public TableCell TableCell
        {
            get { return _tableCell; }
            set { _tableCell = value; }
        }

        /// <summary>
        /// TableCell所在的列索引
        /// </summary>
        public int ColumnIndex
        {
            get { return _columnIndex; }
            set { _columnIndex = value; }
        }

        /// <summary>
        /// DataControlRowType
        /// </summary>
        public DataControlRowType RowType
        {
            get { return _rowType; }
            set { _rowType = value; }
        }

        /// <summary>
        /// DataControlRowState
        /// </summary>
        public DataControlRowState RowState
        {
            get { return _rowState; }
            set { _rowState = value; }
        }
    }
}
