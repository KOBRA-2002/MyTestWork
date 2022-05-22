﻿using System.Collections;
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
        random = new System.Random();
        var res = random.Next(0, 20);
        if (res > 10)
            Pass();
        else
            AttackUnit();
    }

    private void Pass()
    {
        Debug.Log("Враг пропустил ход");
        game.Pass();
    }


    private void AttackUnit()
    {
        var numberUnit = random.Next(0, 4);
        Debug.Log("Враг атаковал игрока");
        //game.DrawAttentionOnPlayerUnits(numberUnit);
        game.AttackPlayerUnit(numberUnit);
    }
}
