using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//state tracker used by the enemy - Nova
public enum EnemyState
{
    IDLE,
    FOLLOW,
    WIND_UP,
    ATTACK,
    SEARCHING
}