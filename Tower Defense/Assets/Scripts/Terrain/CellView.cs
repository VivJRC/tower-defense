using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Terrain
{
    public class CellView : MonoBehaviour
    {
        [SerializeField] private Image _image;
        private E_CellType _cellType;

        // DEBUG(i think)
        public void SetType(E_CellType cellType)
        {
            _cellType = cellType;
            _image.color = _cellType switch
            {
                E_CellType.DEFAULT => Color.black,
                E_CellType.SLOT => Color.gray,
                E_CellType.PATH => Color.green,
                E_CellType.END => Color.white,
                E_CellType.START => Color.white,
                _ => Color.red,
            };
        }
    }
}