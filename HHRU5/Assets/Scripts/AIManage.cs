using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AIManage : MonoBehaviour
{
    private Game game;
    private System.Random random;

    public AIManage(Game game)
    {
        this.game = game;
    }

    public void MakeAction() // Сделать ход
    {
        game.ClearAttentionOnEnemyUnits();
        game.ClearAttentionOnPlayerUnits();

        random = new System.Random();
        AttackUnit();
    }

    private void Pass()
    {
        game.Pass();
    }


    private void AttackUnit()
    {
        game.AttackPlayerUnit();
    }
}
