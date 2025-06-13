using System;
using UnityEngine;
namespace Terrain
{
    public class Map : ScriptableObject
    {
        public Row[] rows;
        public Vector2 Size => new Vector2(rows.Length, rows[0].cells.Length);

        public void SetSize(int x, int y)
        {
            rows = new Row[x];
            for (int i = 0; i < x; ++i)
            {
                rows[i] = new Row();
                rows[i].cells = new Cell[y];
            }
        }

        public void SetCellType(int x, int y, E_CellType type)
        {
            if (rows[x - 1].cells[y - 1] == null)
                rows[x - 1].cells[y - 1] = new Cell();

            rows[x - 1].cells[y - 1].cellType = type;
        }

        public Cell GetCell(int x, int y)
        {
            return rows[x - 1].cells[y - 1];
        }
    }
}