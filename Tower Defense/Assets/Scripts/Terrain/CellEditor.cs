using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Terrain
{
    public class CellEditor : MonoBehaviour
    {
        [SerializeField] private Image _image;
        public E_CellType cellType;
        [HideInInspector] public int x;
        [HideInInspector] public int y;

        private void OnValidate()
        {
            _image.color = cellType switch
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