using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class Game : MonoBehaviour
{
    [SerializeField] Text ownAction; // Чей ход

    [SerializeField] private List<Unit> playerUnits;
    [SerializeField] private List<Unit> enemyUnits;

    [SerializeField] private GameObject buttonChoiceUnit;
    [SerializeField] private GameObject buttonAttack;
    [SerializeField] private GameObject buttonPass;

    private int _pointerOnUnitOfEnemy;

    private AIManage ai;

    public int pointerOnUnitOfEnemy // Номер юнита, которым ходит враг (компьютер)
    { 
        private set
        {
            if (pointerOnUnitOfEnemy < enemyUnits.Count-1)
                _pointerOnUnitOfEnemy = value;
            else
                _pointerOnUnitOfEnemy = 0;
        }
        get
        {
            return _pointerOnUnitOfEnemy;
        }
    } 

    private int _pointerOnUnitOfPlayer = 0; // Номер юнита, которым ходит игрок 

    public int pointerOnUnitOfPlayer
    {
        private set
        {
            if (_pointerOnUnitOfPlayer < playerUnits.Count-1)
                _pointerOnUnitOfPlayer = value;
            else
                _pointerOnUnitOfPlayer = 0;
        }
        get
        {
            return _pointerOnUnitOfPlayer;
        }
    }

    public bool isPlayerAction { private set; get; } = true; // Ход игрока

    private static Game _instance;
    public static Game instance { get { return _instance; } }

    public void Start()
    {
        _instance = this;

        ai = new AIManage(this);

        if (isPlayerAction)
            PlayerAction();
        else
            AIAction();
    }

    public void ChangeOwnAction() // Передает ход
    {
        isPlayerAction = !isPlayerAction;

        if (isPlayerAction)
        {
            PlayerAction();
            //OffPlayerUI();
            //OnPassButton();
            //ownAction.text = "Ваш ход!";
            //DrawAttentionOnUnitsOfPlayer(pointerOnUnitOfPlayer);
            //ClearAttentionOnEnemyUnit();
        }
        else
        {
            AIAction();
            //OffPlayerUI();
            //ownAction.text = "Ход противника!";
            //ai.MakeAction();
        }

    }

    private void AIAction()
    {
        OffPlayerUI();
        ownAction.text = "Ход противника!";
        ai.MakeAction();
    }

    private void PlayerAction()
    {
        OffPlayerUI();
        OnPassButton();
        ownAction.text = "Ваш ход!";
        DrawAttentionOnUnitsOfPlayer(pointerOnUnitOfPlayer);
        ClearAttentionOnEnemyUnit();
    }

    public void Pass() // Пропустить ход
    {
        Debug.Log(pointerOnUnitOfPlayer);
        EndAction();
        //ChangeOwnAction();
    }

    public void AttackEnemyUnit(Unit unit) // Атаковать юнита (данный метод вызывается игроком)
    {
        var attackingUnit = GetAttackingUnit();
        // TODO: Логика боя
        var damageValue = attackingUnit.damage;

        unit.CauseDamage(damageValue);

        EndAction();
    }

    public void AttackPlayerUnit(int unitNumber) // Атаковать юнита с порядковым номером (вызывается AI)
    {
        //DrawAttentionOnPlayerUnits(unitNumber);

        var unit = playerUnits[unitNumber];

        var attackingUnit = GetAttackingUnit();
        // TODO: Логика боя
        var damageValue = attackingUnit.damage;

        unit.CauseDamage(damageValue);

        EndAction();
    }

    private Unit GetAttackingUnit() // Получить юнита, который атакует 
    {
        Unit attackingUnit;
        if (isPlayerAction)
            attackingUnit = playerUnits[pointerOnUnitOfPlayer];
        else
            attackingUnit = enemyUnits[pointerOnUnitOfEnemy];
        return attackingUnit;
    }

    private void EndAction() // Закончить ход
    {
        if (isPlayerAction)
            pointerOnUnitOfPlayer++;
        else
            pointerOnUnitOfEnemy++;

        ChangeOwnAction();
    }

    public void DrawAttentionOnUnitsOfPlayer(int numberUnit) // Игрок выделяет своего юнита
    {
        for (int i = 0; i < playerUnits.Count; i++)
        {
            if (numberUnit == i)
            {
                playerUnits[i].ShowAttacking();
                continue;
            }

            playerUnits[i].Hide();
        }
    }

    public void DrawAttentionOnEnemyUnit(Unit _unit) // Затемнить всех юнитов противника кроме этого
    {
        foreach (Unit unit in enemyUnits)
        {
            if (unit == _unit)
            {
                unit.ShowAttacked(); // Игрок выделяет юнита, которого хочет атаковать 
                continue;
            }

            unit.Hide();
        }
    }

    public void ClearAttentionOnEnemyUnit()
    {
        foreach (Unit unit in enemyUnits)
        {
            unit.Hide();
        }
    }

    // TODO: Вынести в отдельный класс (нарушает принцип единственной ответственности)
    public void OffPassButton()
    {
        buttonPass.SetActive(false);
    }

    public void OnPassButton()
    {
        buttonPass.SetActive(true);
    }

    public void OffAttackButton()
    {
        buttonAttack.SetActive(false);
    }

    public void OnAttackButton()
    {
        buttonAttack.SetActive(true);
    }

    private void OffPlayerUI()
    {
        OffAttackButton();
        OffPassButton();
    }

    private void OnPlayerUI()
    {
        OnAttackButton();
        OnPassButton();
    }
}
