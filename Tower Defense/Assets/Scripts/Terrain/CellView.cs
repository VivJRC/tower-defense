using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MAP
{
    public class CellView : MonoBehaviour
    {
        [SerializeField] private Image _image;
        private CellModel _cell;
        public CellModel Cell => _cell;

        private Color _colorSave;
        private bool _highlight;

        // DEBUG(i think)
        public void SetType(CellModel cell)
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
    }
}