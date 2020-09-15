using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public string _abilityName = "New Ability";
    public Sprite _abilitySprite;
    public AudioClip _abilityClip;
    public float _abilityBaseCooldown = 1f;

    public abstract void Initialize(GameObject _obj);
    public abstract void TriggerAbility();

}
