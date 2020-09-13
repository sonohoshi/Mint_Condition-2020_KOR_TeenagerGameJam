using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    private Animator playerAnimator;
    public int[,] map;

    void Start()
    {
        map = GameManager.Instance.Map[0];
        playerAnimator = GetComponent<Animator>();
    }
    
    // Update is called once per frame
    void Update()
    {
        GetMovingInput();
    }

    private void GetMovingInput()
    {
        int moveResult = 0;
        if (Input.GetKeyUp(KeyCode.W))
        {
            moveResult = Move(MoveDirection.UpOrLeft, MoveDirection.InPlace, map);
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            moveResult = Move(MoveDirection.InPlace, MoveDirection.UpOrLeft, map);
        }
        
        if (Input.GetKeyUp(KeyCode.S))
        {
            moveResult = Move(MoveDirection.DownOrRight, MoveDirection.InPlace, map);
        }
        
        if (Input.GetKeyUp(KeyCode.D))
        {
            moveResult = Move(MoveDirection.InPlace, MoveDirection.DownOrRight, map);
        }

        if (moveResult == 1)
        {
            playerAnimator.SetTrigger("StartMove");
            StartCoroutine(CheckAnimationCompleted("PlayerMove", (() => playerAnimator.SetTrigger("EndMove"))));
        }
    }

    public override int Move(MoveDirection x, MoveDirection y, int[,] map)
    {
        var moveResult = base.Move(x, y, map);
        if (moveResult == box)
        {
            var boxX = (int) x + posX;
            var boxY = (int) y + posY;
            GameManager.Instance.InGameMap[boxX, boxY].GetComponent<Entity>().Move(x, y, map);
        }
        return moveResult;
    }

    private IEnumerator CheckAnimationCompleted(string currentAnim, Action onComplete)
    {
        while (!playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(currentAnim))
        {
            yield return null;
        }

        onComplete?.Invoke();
    }
}
