using UnityEngine;
using BulletMLLib;

public class UnityMLBullet : MonoBehaviour
{
    private MLBullet _bullet;
    public BulletMLPatternManager CombatManager { get; set; }

    public ElementType ElementType { get; private set; }

    public BulletMLVisuals Visuals { get; set; }

    private bool _top = false;
    private float _lifetime = 0;

    public void Initialize(MLBullet bullet)
    {
        _bullet = bullet;
        _top = bullet.Top;

        _lifetime = _bullet.Lifetime / 1000;
        if (_lifetime == 0)
        {
            _lifetime = float.MaxValue;
        }

        transform.position = new Vector2(_bullet.X, _bullet.Y);
        ElementType = _bullet.ElementType;
    }

    public void VisualFix()
    {
        if (Visuals != null && _bullet.FaceDirection)
        {
            Visuals.transform.rotation = Quaternion.Euler(0, 0, (Mathf.Rad2Deg * _bullet.VisualDirection) - 180f);
        }
    }

    private void Update()
    {
        _bullet.Update();

        // top bullet only needs update
        if (_top) return;

        transform.position = new Vector2(_bullet.X, _bullet.Y);
        _lifetime -= Time.deltaTime;
        if (_lifetime <= 0)
        {
            CombatManager.RemoveBullet(_bullet);
        }

        if (_bullet.FaceDirection)
        {
            Visuals.transform.rotation = Quaternion.Euler(0, 0, (Mathf.Rad2Deg * _bullet.VisualDirection) - 180f);
        }
        else if (_bullet.ContinousRotation <= 1 || _bullet.ContinousRotation >= -1)
        {
            float rot = _bullet.ContinousRotation * Time.deltaTime;
            Visuals.transform.rotation = Quaternion.Euler(0, 0, Visuals.transform.rotation.eulerAngles.z + rot);
        }
    }
}
