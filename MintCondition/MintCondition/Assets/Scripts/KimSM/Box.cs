using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : Entity
{
    public override int Move(MoveDirection x, MoveDirection y, int[,] map)
    {
        var wasX = posX - (int) x;
        var wasY = posY - (int) y;
        var moveResult = base.Move(x, y, map);
        
        if (wasExit && moveResult != 6)
        {
            map[wasX, wasY] = 6;
            wasExit = false;
        }

        return moveResult;
    }

    public override void Damaged(int x, int y, int[,] map)
    {
        base.Damaged(x, y, map);
        if (wasExit)
        {
            map[x, y] = 6;
        }

        Debug.Log($"box damaged {x}, {y}, wasExit : {wasExit}, map : {map[x, y]}");
        GetComponent<Animator>().SetTrigger("BoxDamaged");
        StartCoroutine(CheckAnimationCompleted("BoxDamaged", (() =>
        {
            Destroy(gameObject);
        })));
    }
}
