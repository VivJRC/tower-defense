using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MAP
{
    public class CellView : MonoBehaviour
    {
        [SerializeField] private Image _image;
        private Cell _cell;
        public Cell Cell => _cell;

        private Color _colorSave;
        private bool _highlight;

        private bool _hasDefense;
        public bool HasDefense => _hasDefense;

        // DEBUG(i think)
        public void SetType(Cell cell)
        {
            _cell = cell;
            _image.color = cell.CellType switch
            {
                E_CellType.DEFAULT => Color.black,
                E_CellType.SLOT => Color.gray,
                E_CellType.PATH => Color.green,
                E_CellType.END => Color.white,
                E_CellType.START => Color.white,
                _ => Color.red,
            };
        }

        public void StartFlash()
        {
            StartCoroutine(Flash());
        }

        public IEnumerator Flash()
        {
            Color color = _image.color;
            yield return _image.DOColor(Color.blue, 0.5f).OnComplete(() => { _image.DOColor(color, 0.5f); });
        }

        public void Hihlight(bool highlight)
        {
            if (!_highlight && highlight)
            {
                _colorSave = _image.color;
                _highlight = true;
                _image.color = Color.yellow;
            }
            if (_highlight && !highlight)
            {
                _image.color = _colorSave;
                _highlight = false;
            }
        }

        public void AddDefense()
        {
            _hasDefense = true;
        }

        public void RemoveDefense()
        {
            _hasDefense = false;
        }
    }
}