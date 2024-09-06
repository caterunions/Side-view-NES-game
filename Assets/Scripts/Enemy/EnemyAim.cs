using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAim : MonoBehaviour
{
    private Transform _player;
    public Transform Player
    {
        get { return _player; }
        set { _player = value; }
    }

    [SerializeField]
    private bool _locked = false;
    public bool Locked
    {
        get { return _locked; }
        set { _locked = value; }
    }

    private void Update()
    {
        if (Player == null) return;
        if (Locked) return;

        Vector3 targ = Player.transform.position;
        targ.z = 0f;

        Vector3 objectPos = transform.position;
        targ.x -= objectPos.x;
        targ.y -= objectPos.y;

        float angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }
}
