using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Terrain
{
    public class CellView : MonoBehaviour
    {
        [SerializeField] private Image _image;
        private Cell _cell;
        public Cell Cell => _cell;
        private E_CellType _cellType;

        // DEBUG(i think)
        public void SetType(Cell cell)
        {
            _cell = cell;
            _cellType = cell.CellType;
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

        public IEnumerator Flash()
        {
            Color color = _image.color;
            yield return _image.DOColor(Color.blue, 0.5f).OnComplete(() => { _image.DOColor(color, 0.5f); });
        }
    }
}