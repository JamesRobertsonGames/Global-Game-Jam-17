using UnityEngine;
using System.Collections;

using DG.Tweening;

public class BigShip : Ship
{
    public override void Attack(Ship target)
    {
        startRot = transform.rotation.eulerAngles;
        transform.DORotate(Quaternion.LookRotation(transform.position - target.transform.position, Vector3.up).eulerAngles, 1);
        StartCoroutine(FireCannon(target));
    }
}
