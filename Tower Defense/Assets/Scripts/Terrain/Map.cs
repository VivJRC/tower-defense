using System.Collections.Generic;
using UnityEngine;

namespace MAP
{
    public class Map : ScriptableObject
    {
        public CellModel[] cells;

        public List<CellModel> GetPath()
        {
            List<CellModel> path = new();

            foreach (CellModel cell in cells)
            {
                if (cell.CellType == E_CellType.START)
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

            CellModel currentCell = path[0];
            bool reachedEnd = false;
            int debug = 1000;

            while (!reachedEnd && debug > 0)
            {
                int x = (int)currentCell.Coordinates.x;
                int y = (int)currentCell.Coordinates.y;
                foreach (CellModel cell in cells)
                {
                    if (cell.CellType == E_CellType.END)
                    {
                        reachedEnd = true;
                        path.Add(cell);
                        break;
                    }

                    if (cell.CellType == E_CellType.PATH &&
                        ((cell.Coordinates.x == x && (cell.Coordinates.y == y + 1 || cell.Coordinates.y == y - 1)) ||
                        (cell.Coordinates.y == y && (cell.Coordinates.x == x + 1 || cell.Coordinates.x == x - 1))) &&
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

        public CellModel GetStart()
        {
            foreach (CellModel cell in cells)
            {
                if (cell.CellType == E_CellType.START)
                {
                    return cell;
                }
            }
            Debug.LogError("Couldn't find the start of the map: " + name);
            return null;
        }
    }
}