using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "Visuals Bank", menuName = "BulletML/Visuals Bank")]
public class BulletMLVisualsBank : ScriptableObject
{
    [SerializeField]
    private BulletMLVisuals _errorFallbackVisuals;

    [SerializeField]
    private List<BulletMLVisuals> _bulletBank = new List<BulletMLVisuals>();

    public BulletMLVisuals GetVisuals(string name)
    {
        BulletMLVisuals match = _bulletBank.Find(b => b.name == name);
        if (match != null) return match;

        //Debug.LogWarning($"Couldn't find visuals for name: {name}");
        return _errorFallbackVisuals;
    }
}