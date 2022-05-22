using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManage : MonoBehaviour
{
    //[SerializeField] private GameObject buttonChoiceUnit;
    //[SerializeField] private GameObject buttonAttack;
    //[SerializeField] private GameObject buttonPass;

    private Unit currentAttackedUnit; // Юнит противника, которой будет атакован игроком  

    private Game game;

    void Start()
    {
        game = Game.instance;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                Debug.Log("Target: " + hit.collider.gameObject.name);
                var unit = hit.collider.gameObject.GetComponent<Unit>();
                if (unit.isEnemyUnit)
                {
                    Debug.Log("Игрок выбрал атакуемого юнита");
                    currentAttackedUnit = unit;
                    game.OnAttackButton();
                    game.DrawAttentionOnEnemyUnit(unit);
                }
            }
        }
    }

    public void ClickAttack()
    {
        game.AttackEnemyUnit(currentAttackedUnit);
        Debug.Log("Атака");
    }

    public void ClickPass()
    {
        game.Pass();
    }

    /*public void OffPassButton()
    {
        buttonPass.SetActive(false);
    }

    public void OffChoiceButton()
    {
        buttonChoiceUnit.SetActive(false);
    }

    public void OffAttackButton()
    {
        buttonChoiceUnit.SetActive(false);
    }

    public void OnPassButton()
    {
        buttonPass.SetActive(true);
        Debug.Log("Пропуск хода");
    }

    public void OnChoiceButton()
    {
        buttonChoiceUnit.SetActive(true);
    }

    public void OnAttackButton()
    {
        buttonAttack.SetActive(true);
    }*/
}
