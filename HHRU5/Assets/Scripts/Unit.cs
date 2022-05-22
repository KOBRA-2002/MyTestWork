using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public abstract int damage { get; }
    public abstract bool isEnemyUnit { get; set; }
    public abstract void CauseDamage(int damage);
    public abstract void ChoiceUnit();
    public abstract void Hide();
    public abstract void ShowAttacking();
    public abstract void ShowAttacked();
}
