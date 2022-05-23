using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public abstract class Unit : MonoBehaviour
{
    public bool isDeath { get; set; }
    public abstract int damage { get; }
    public abstract bool isEnemyUnit { get; set; }
    public abstract void CauseDamage(int damage); // Причинить урон (анимация получения урона)
    public abstract void Attack(); // (Запуск анимации атаки)
    public abstract void ChoiceUnit();
    public abstract void Hide();
    public abstract void ShowAttacking();
    public abstract void ShowAttacked();
    public abstract Task MoveToPlace(Vector3 placeFight);
}
