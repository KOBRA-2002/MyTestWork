using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public abstract class Unit : MonoBehaviour
{
    /// <summary>
    /// Определяет жив ли юнит 
    /// </summary>
    public bool isDeath { get; set; }
    /// <summary>
    /// Урон юнита
    /// </summary>
    public abstract int damage { get; }
    /// <summary>
    /// Чей юнит (игрока или врага)
    /// </summary>
    public abstract bool isEnemyUnit { get; set; }
    /// <summary>
    /// Наносит ущерб юниту
    /// </summary>
    /// <param name="damage">Значение урона</param>
    public abstract void CauseDamage(int damage); // Причинить урон (анимация получения урона)
    /// <summary>
    /// Запуск анимации атаки
    /// </summary>
    public abstract void Attack(); // (Запуск анимации атаки)
    public abstract void ChoiceUnit(); // TODO: Удалить 
    /// <summary>
    /// Убрать выделение с юнита
    /// </summary>
    public abstract void Hide();
    /// <summary>
    /// Выделить юнита как атакующего
    /// </summary>
    public abstract void ShowAttacking();
    /// <summary>
    /// Выделить юнита как атакуемого
    /// </summary>
    public abstract void ShowAttacked();
    /// <summary>
    /// Асинхронно передвинуть игрока в нужную точку
    /// </summary>
    /// <param name="placeFight"></param>
    /// <returns></returns>
    public abstract Task MoveToPlace(Vector3 placeFight);
}
