using System.Collections.Generic;

namespace Widget.Core
{
    public class WinMatrix
    {
        readonly List<List<byte>> matrix;
        private int maxRows;
        private int maxCols;

        public int ColumnsCount { get { return maxCols; } }
        public int RowsCount { get { return maxRows; } }

        public int GetRealColumnsCount(int row)
        {

            if (matrix.Count < row)
                return 0;
            return matrix[row].Count;

        }

        public int GetRealRowsCount()
        {
            return matrix.Count;
        }


        /// <summary>
        /// Initilizes a news instance of Matrix class
        /// </summary>
        /// <param name="c">Columns count</param>
        /// <param name="r">Rows count</param>
        public WinMatrix(int c, int r)
        {
            matrix = new List<List<byte>>();
            maxCols = c;
            maxRows = r;
        }

        public void Add(byte item)
        {
            if (matrix.Count == 0)
            {
                matrix.Add(new List<byte>() { item });
                return;
            }

            foreach (var row in matrix)
            {
                if (row.Count >= maxCols)
                    continue;
                row.Add(item);
                return;
            }

            if (matrix.Count < maxRows)
            {
                AddRow();
                Add(item);
                return;
            }

            if (matrix.Count == maxRows)
            {
                AddColumn();
                Add(item);
                return;
            }
        }

        public void AddRow()
        {
            matrix.Add(new List<byte>());
        }

        public void AddColumn()
        {
            maxCols++;
        }

        public byte this[int x, int y]
        {
            get { return matrix[y][x]; }
            set { matrix[y][x] = value; }
        }

        public void Clear()
        {
            foreach (var row in matrix)
            {
                row.Clear();
            }

            matrix.Clear();
        }

        //Fills matrix with zero
        public void ZeroMatrix()
        {
            if (matrix.Count == 0)
            {
                for (var i = 0; i < maxRows * maxCols; i++)
                    Add(0);
            }
            else
            {
                for (var i = 0; i < maxRows; i++)
                    for (var j = 0; j < maxCols; j++)
                        this[j, i] = 0;
            }
        }

        /// <summary>
        /// Finds free cell with enough horizontal space
        /// </summary>
        /// <param name="length">Required horizontal space</param>
        /// <returns>Index of cell where widget can be placed. If there is no free cell, returns -1.</returns>
        public AppCell GetFreeCell(int length)
        {
            for (var i = 0; i < maxCols; i++) //looks up in columns
            {
                var index = GetFreeRow(i, length);
                if (index != -1)
                {
                    var cell = new AppCell(i, index);
                    return cell;
                }
            }
            return new AppCell(-1, -1);
        }

        private int GetFreeRow(int column, int length)
        {
            //если разность между количеством колонок и номер колонки меньше длины, значит указанная колонка где-то в конце и места не хватит
            if (maxCols - column < length)
                return -1;
            //перебираем все строки
            for (var i = 0; i < maxRows; i++)
            {
                var isFree = true; //обозначает свободно ли нужное количество клеток в текущей строке
                //проверяем ячейки в текущей строке указанной колонки и следующими за ней
                for (var j = 0; j < length; j++)
                    if (this[column + j, i] == 1)
                    {
                        isFree = false;
                        break;
                    }
                //если нужное количество ячеек свободно, возвращаем номер строки
                if (isFree)
                    return i;
            }
            return -1;
        }

        public void ReserveSpace(int column, int row, int length)
        {
            if (row >= maxRows)
                return;
            for (var i = column; i < column + length; i++)
            {
                if (i >= maxCols)
                    return;
                this[i, row] = 1;
            }
        }

        public void FreeSpace(int column, int row, int length)
        {
            if (row >= maxRows)
                return;
            for (var i = column; i < column + length; i++)
            {
                if (i >= maxCols)
                    return;
                this[i, row] = 0;
            }
        }

        public bool IsCellFree(int column, int row, int length)
        {
            if (row >= maxRows)
                return false;
            var result = true;
            for (var i = column; i < column + length; i++)
            {
                if (i >= maxCols)
                    return false;
                if (this[i, row] == 1)
                    result = false;
            }
            return result;
        } 
    }
}