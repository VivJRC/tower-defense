using UnityEngine;

[CreateAssetMenu]
public class Waves : ScriptableObject
{
    [SerializeField] private Wave[] _waves;
    public Wave[] _Waves => _waves;
}
