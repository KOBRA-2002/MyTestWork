using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManage : MonoBehaviour
{
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
                //Debug.Log("Target: " + hit.collider.gameObject.name);
                var unit = hit.collider.gameObject.GetComponent<Unit>();
                if (unit.isEnemyUnit && !unit.isDeath)
                {
                    //Debug.Log("Игрок выбрал атакуемого юнита");
                    currentAttackedUnit = unit;
                    game.OnAttackButton();
                    game.DrawAttentionOnEnemyUnit(unit);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
            Application.Quit();
    }

    public void ClickAttack()
    {
        game.AttackEnemyUnit(currentAttackedUnit);
        game.OffAttackButton();
        game.OffPassButton();
        //Debug.Log("Атака");
    }

    public void ClickPass()
    {
        game.Pass();
    }
}
