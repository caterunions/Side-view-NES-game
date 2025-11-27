using BulletMLLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletMLPatternManager : MonoBehaviour, IBulletManager
{
    public static BulletMLPatternManager Instance { get; private set; }

    [SerializeField]
    private UnityMLBullet _bulletPrefab;

    [SerializeField]
    private BulletMLSoundPlayer _soundPlayer;

    private Dictionary<MLBullet, UnityMLBullet> _bullets = new Dictionary<MLBullet, UnityMLBullet>();

    private BulletPattern Pattern
    {
        get
        {
            if (_pattern == null)
            {
                _pattern = new BulletPattern();
            }
            return _pattern;
        }
    }

    [SerializeField]
    private BulletMLVisualsBank _visualsBank;

    private GameObject _combatPlayer;

    private BulletPattern _pattern;

    private void OnEnable()
    {
        //IMSORRY yucky singleton but needed for equations :/
        Instance = this;
    }

    public void Initialize(GameObject combatPlayer)
    {
        _combatPlayer = combatPlayer;
    }

    public void StopPattern()
    {
        ClearBullets();

        _pattern = new BulletPattern();
    }

    public void ClearBullets()
    {
        // store count before modification
        int count = _bullets.Count;

        for (int i = 0; i < count; i++)
        {
            RemoveBullet(_bullets.First().Key);
        }
    }

    public void StartPattern(string path)
    {
        ClearBullets();
        _pattern = new BulletPattern();
        Pattern.ParseXML(path);

        MLBullet top = new MLBullet(this, true);
        UnityMLBullet topBullet = Instantiate(_bulletPrefab);
        top.InitTopNode(Pattern.RootNode);

        topBullet.Initialize(top);

        _bullets.Add(top, topBullet);
    }

    public void StartPattern(TextAsset xmlAsset)
    {
        ClearBullets();
        _pattern = new BulletPattern();
        Pattern.ParseXML(xmlAsset);

        MLBullet top = new MLBullet(this, true);
        UnityMLBullet topBullet = Instantiate(_bulletPrefab);
        top.InitTopNode(Pattern.RootNode);

        topBullet.Initialize(top);

        _bullets.Add(top, topBullet);
    }

    public MLBullet CreateBullet(MLBullet source, bool top)
    {
        MLBullet bullet = new MLBullet(this, top);
        bullet.OnFinishSetup += InitializeUnityBullet;
        UnityMLBullet unityBullet = Instantiate(_bulletPrefab);
        unityBullet.CombatManager = this;

        _bullets.Add(bullet, unityBullet);

        return bullet;
    }

    public Vector2 PlayerPosition(MLBullet targettedBullet)
    {
        return _combatPlayer.transform.position;
    }

    public void RemoveBullet(MLBullet deadBullet)
    {
        Destroy(_bullets[deadBullet].gameObject);
        _bullets.Remove(deadBullet);
    }

    public void Trigger(MLBullet source, string name)
    {
        string[] parameters = name.Split('|');

        if (parameters.Length == 0) return;

        if (parameters[0] == "sfx")
        {
            _soundPlayer.PlaySound(parameters[1]);
        }
    }

    public void InitializeUnityBullet(MLBullet bullet)
    {
        UnityMLBullet uBullet = _bullets[bullet];

        uBullet.Initialize(bullet);

        if (!bullet.Top)
        {
            BulletMLVisuals vis = Instantiate(_visualsBank.GetVisuals(bullet.Visuals), uBullet.transform);
            uBullet.Visuals = vis;
            uBullet.VisualFix();
            //vis.SpriteRenderer.material = _materialBank.GetElementMaterial(bullet.ElementType);
        }

        bullet.OnFinishSetup -= InitializeUnityBullet;
    }
}
