using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Profiles/Unit")]
public class unitProfile : ScriptableObject {

    public int totalLife;
    public float baseMovementSpeed;
    public bool isEnemy;
    public bool isFriendly;
    public bool isHero;

    #region Collection Properties

    public UnitRuntimeCollection friendlyList;
    public UnitRuntimeCollection enemyList;
    public FloatVariable taggingRange;

    #endregion

}
