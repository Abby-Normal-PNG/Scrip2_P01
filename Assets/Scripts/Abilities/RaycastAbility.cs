using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/RaycastAbility")]
public class RaycastAbility : Ability
{
    public int _gunDamage = 1;
    public float _weaponRange = 50f;
    public float _hitForce = 100f;
    public Color _laserColor = Color.white;

    private RaycastShootTriggerable _rcShoot;
    public override void Initialize(GameObject _obj)
    {
        _rcShoot = _obj.GetComponent<RaycastShootTriggerable>();
        _rcShoot.Initialize();

        _rcShoot.gunDamage = _gunDamage;
        _rcShoot.weaponRange = _weaponRange;
        _rcShoot.hitForce = _hitForce;
        _rcShoot.laserLine.material = new Material(Shader.Find("Unlit/Color"));
        _rcShoot.laserLine.material.color = _laserColor;
    }

    public override void TriggerAbility()
    {
        _rcShoot.Fire();
    }
}
