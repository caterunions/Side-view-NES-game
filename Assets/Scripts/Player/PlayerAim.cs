using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    public void HandleAim(Vector2 aimInput)
    {
        Debug.Log(aimInput);
        Vector2 facingDirection = aimInput - new Vector2(transform.position.x, transform.position.y);
        float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }
}
