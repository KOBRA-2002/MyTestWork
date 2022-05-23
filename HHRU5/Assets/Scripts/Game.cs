using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Linq;


public class Game : MonoBehaviour
{
    [SerializeField] Text winText;
    /// <summary>
    /// Чей ход
    /// </summary>
    [SerializeField] Text ownAction;

    /// <summary>
    /// Список юнитов игрока
    /// </summary>
    [SerializeField] private List<Unit> playerUnits;
    /// <summary>
    /// Список юнитов врага
    /// </summary>
    [SerializeField] private List<Unit> enemyUnits;

    [SerializeField] private GameObject buttonAttack;
    [SerializeField] private GameObject buttonPass;

    /// <summary>
    /// Место, где находится юнит игрока, когда сражается
    /// </summary>
    [SerializeField] private Transform playerUnitPlace;

    /// <summary>
    /// Место, где находится юнит противника, когда сражается
    /// </summary>
    [SerializeField] private Transform enemyUnitPlace;

    private int _pointerOnUnitOfEnemy;

    private AIManage ai;

    private Vector3 startPlayerPlace;
    private Vector3 startEnemyPlace;

    private int _deadPlayerUnits = 0;

    /// <summary>
    /// Конец игры
    /// </summary>
    private bool isGameEnd = false;

    /// <summary>
    /// Количество мертвых юнитов игрока
    /// </summary>
    public int deadPlayerUnits
    {
        get { return _deadPlayerUnits; }
        set
        {
            _deadPlayerUnits = value;
            if (_deadPlayerUnits >= playerUnits.Count)
            {
                isGameEnd = true;
                OffPlayerUI();
                OnLooseText();
                Debug.Log("Противник выиграл");
            }
        }
    }

    private int _deadEnemyUnits = 0;
    /// <summary>
    /// Количество мертвых юнитов противника
    /// </summary>
    public int deadEnemyUnits
    {
        get { return _deadEnemyUnits; }
        set
        {
            _deadEnemyUnits = value;
            if (_deadEnemyUnits >= enemyUnits.Count)
            {
                isGameEnd = true;
                OffPlayerUI();
                OnWinText();
                Debug.Log("Игрок выиграл выиграл");
            }
        }
    }

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
        winText.gameObject.SetActive(false);

        _instance = this;

        ai = new AIManage(this); // Создать "ИИ" (Тестовый)

        if (isPlayerAction)
            PlayerAction();
        else
            AIAction();
    }

    /// <summary>
    /// Передать ход
    /// </summary>
    public void ChangeOwnAction() // Передает ход
    {
        if (!isGameEnd)
        {
            isPlayerAction = !isPlayerAction;

            StartAction();
            if (isPlayerAction)
            {
                PlayerAction();
            }
            else
            {
                AIAction();
            }
        }
        
    }
    /// <summary>
    /// Ход противника
    /// </summary>
    private void AIAction()
    {
        OffPlayerUI();
        ownAction.text = "Ход противника!";
        ai.MakeAction();
    }

    /// <summary>
    /// Ход игрока
    /// </summary>
    private void PlayerAction()
    {
        OffPlayerUI();
        OnPassButton();
        ownAction.text = "Ваш ход!";
        DrawAttentionOnUnitsOfPlayer(pointerOnUnitOfPlayer);
        ClearAttentionOnEnemyUnits();
    }

    /// <summary>
    /// Пропустить ход
    /// </summary>
    public void Pass() // Пропустить ход
    {
        EndAction();
    }
    /// <summary>
    /// Бот атакует игрока
    /// </summary>
    /// <param name="unit"></param>
    public async void AttackEnemyUnit(Unit unit) // Атаковать юнита (данный метод вызывается игроком)
    {
        // Получаем юнита, который атакует в этом ходу
        var attackingUnit = GetAttackingPlayerUnit();

        // Запоминаем начальные положения юнитов
        startPlayerPlace = attackingUnit.transform.position;
        startEnemyPlace = unit.transform.position;

        // Передвинуть юнитов в место сражения (ассинхронно)
        await attackingUnit.MoveToPlace(playerUnitPlace.position);
        await unit.MoveToPlace(enemyUnitPlace.position);

        // Активируем анимацию
        attackingUnit.Attack();

        // Наносим урон
        var damageValue = attackingUnit.damage;
        unit.CauseDamage(damageValue);

        // Возращаем к начальному положению 
        await attackingUnit.MoveToPlace(startPlayerPlace);
        await unit.MoveToPlace(startEnemyPlace);

        // Завершаем текущий ход
        EndAction();
    }

    /// <summary>
    /// Игрок атакует вражеского юнита
    /// </summary>
    public async void AttackPlayerUnit() // Атаковать юнита с порядковым номером (вызывается AI)
    {
        System.Random rand = new System.Random();

        // ТОЛЬКО ДЛЯ ТЕСТИРОВАНИЯ (выбор случайного не мертвого игрока)
        Unit selectedUnit;
        do
        {
            var numberUnit = rand.Next(0, playerUnits.Count);
            selectedUnit = playerUnits[numberUnit];
        }
        while (selectedUnit.isDeath);

        // Получаем юнита, который атакует в этом ходу
        var attackingUnit = GetAttackinEnemyUnit();

        // Запоминаем начальные положения юнитов
        startEnemyPlace = attackingUnit.transform.position;
        startPlayerPlace = selectedUnit.transform.position;

        // Передвинуть юнитов в место сражения (ассинхронно)
        await attackingUnit.MoveToPlace(enemyUnitPlace.position);
        await selectedUnit.MoveToPlace(playerUnitPlace.position);

        // Активируем анимацию
        attackingUnit.Attack();

        // Наносим урон
        var damageValue = attackingUnit.damage;
        selectedUnit.CauseDamage(damageValue);

        // Возращаем к начальному положению 
        await attackingUnit.MoveToPlace(startEnemyPlace);
        await selectedUnit.MoveToPlace(startPlayerPlace);

        // Завершаем текущий ход
        EndAction();
    }

    /// <summary>
    /// Получить юнита игрока, которым будет атаковать настоящий игрок (Player)
    /// </summary>
    /// <returns></returns>
    private Unit GetAttackingPlayerUnit()
    {
        return playerUnits[pointerOnUnitOfPlayer];
    }

    /// <summary>
    /// Получить вражеского юнита, которым будет атаковать текущий игрок 
    /// </summary>
    /// <returns></returns>
    private Unit GetAttackinEnemyUnit()
    {
        return enemyUnits[pointerOnUnitOfEnemy];
    }

    /// <summary>
    /// Начинает ход и выбирает юнита, которым будет ходить текущий игрок
    /// </summary>
    private void StartAction()
    {
        if (isPlayerAction)
        {
            var isDeath = false;
            do
            {
                pointerOnUnitOfPlayer++;
                isDeath = playerUnits[pointerOnUnitOfPlayer].isDeath;

            } while (isDeath);
        }
        else
        {
            var isDeath = false;
            do
            {
                pointerOnUnitOfEnemy++;
                isDeath = enemyUnits[pointerOnUnitOfEnemy].isDeath;

            } while (isDeath);
        }
    }
    /// <summary>
    /// Заканчивает ход 
    /// </summary>
    private void EndAction()
    {
        ChangeOwnAction();
    }

    /// <summary>
    /// Выделить юнита игрока
    /// </summary>
    /// <param name="numberUnit"></param>
    public void DrawAttentionOnUnitsOfPlayer(int numberUnit) 
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

    /// <summary>
    /// Выделить юнита противника
    /// </summary>
    /// <param name="_unit"></param>
    public void DrawAttentionOnEnemyUnit(Unit _unit) 
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

    /// <summary>
    /// Снять выделение со всех юнитов противника
    /// </summary>
    public void ClearAttentionOnEnemyUnits()
    {
        foreach (Unit unit in enemyUnits)
        {
            unit.Hide();
        }
    }

    /// <summary>
    /// Снять выделение со всех юнитов игрока
    /// </summary>
    public void ClearAttentionOnPlayerUnits()
    {
        foreach (Unit unit in playerUnits)
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

    private void OnLooseText()
    {
        winText.gameObject.SetActive(true);
        winText.text = "Противник выиграл";
    }

    private void OnWinText()
    {
        winText.gameObject.SetActive(true);
        winText.text = "Вы выиграли!";
    }
}
