using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
namespace Terrain
{
    public class Map : ScriptableObject
    {
        public Cell[] cells;

        public List<Cell> GetPath()
        {
            List<Cell> path = new();

            foreach (Cell cell in cells)
            {
                if (cell.cellType == E_CellType.START)
                {
                    path.Add(cell);
                    break;
                }
            }
            if (path.Count == 0)
            {
                Debug.LogError("Couldn't find the start of the map: " + name);
                return null;
            }

            Cell currentCell = path[0];
            bool reachedEnd = false;
            int debug = 1000;

            while (!reachedEnd && debug > 0)
            {
                int x = (int)currentCell.coordinates.x;
                int y = (int)currentCell.coordinates.y;
                foreach (Cell cell in cells)
                {
                    if (cell.cellType == E_CellType.END)
                    {
                        reachedEnd = true;
                        path.Add(cell);
                        break;
                    }

                    if ( cell.cellType == E_CellType.PATH &&
                        ((cell.coordinates.x == x && (cell.coordinates.y == y + 1 || cell.coordinates.y == y - 1)) ||
                        (cell.coordinates.y == y && (cell.coordinates.x == x + 1 || cell.coordinates.x == x - 1))) &&
                        !path.Contains(cell))
                    {
                        currentCell = cell;
                        path.Add(currentCell);
                        break;
                    }
                }
                debug--;
            }

            if (debug == 0 && !reachedEnd)
            {
                Debug.LogError("Couldn't reach the end of the map: " + name);
            }

            return path;
        }
    }
}