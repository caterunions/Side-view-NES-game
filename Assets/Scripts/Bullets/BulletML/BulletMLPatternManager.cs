using BulletMLLib;
using System;
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


    [SerializeField]
    private BulletMLVisualsBank _visualsBank;

    private GameObject _combatPlayer;

    private List<BulletMLPattern> _patterns = new List<BulletMLPattern>();

    private void OnEnable()
    {
        //IMSORRY yucky singleton but needed for equations :/
        Instance = this;
    }

    public void Initialize(GameObject combatPlayer)
    {
        _combatPlayer = combatPlayer;
    }

    public void StopPattern(Guid patternID, bool destroy)
    {
        BulletMLPattern match = _patterns.Find(p => p.GUID == patternID);

        if (match == null) return;

        if (destroy)
        {
            List<MLBullet> bullets = _bullets.Where(b => b.Key.Pattern.GUID == patternID).Select(b => b.Key).ToList();

            foreach (MLBullet bullet in bullets)
            {
                RemoveBullet(bullet);
            }
        }

        _patterns.Remove(match);
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

    public Guid StartPattern(TextAsset xmlAsset)
    {
        BulletMLPattern pattern = new BulletMLPattern();
        pattern.ParseXML(xmlAsset);

        _patterns.Add(pattern);

        MLBullet top = new MLBullet(this, pattern, true);
        UnityMLBullet topBullet = Instantiate(_bulletPrefab);
        top.InitTopNode(pattern.RootNode);

        topBullet.Initialize(top);

        _bullets.Add(top, topBullet);

        return pattern.GUID;
    }

    public MLBullet CreateBullet(MLBullet source, BulletMLPattern pattern, bool top)
    {
        if (!_patterns.Any(p => p.GUID == pattern.GUID)) return null;

        MLBullet bullet = new MLBullet(this, pattern, top);
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
