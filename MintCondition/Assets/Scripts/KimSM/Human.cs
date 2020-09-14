using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Entity
{
    private Animator playerAnimator;
    public bool IsPlayer;
    
    public int[,] map;

    void Start()
    {
        map = GameManager.Instance.RealMap[0];
        if (IsPlayer)
        {
            playerAnimator = GetComponent<Animator>();
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (IsPlayer)
        {
            GetMovingInput();
            GetAttackInput();
        }
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

    private void GetAttackInput()
    {
        KeyValuePair<int, int> findResult = new KeyValuePair<int, int>(-1, -1);
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            Find(MoveDirection.UpOrLeft, MoveDirection.InPlace, map, out findResult);
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            Find(MoveDirection.InPlace, MoveDirection.UpOrLeft, map, out findResult);
        }
        
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            Find(MoveDirection.DownOrRight, MoveDirection.InPlace, map, out findResult);
        }
        
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            Find(MoveDirection.InPlace, MoveDirection.DownOrRight, map, out findResult);
        }
        
        if (findResult.Key != -1)
        {
            playerAnimator.SetTrigger("StartAttack");
            StartCoroutine(CheckAnimationCompleted("PlayerShot", (() => playerAnimator.SetTrigger("EndAttack"))));
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

    public override GameObject Find(MoveDirection x, MoveDirection y, int[,] map, out KeyValuePair<int,int> index)
    {
        var findResult = base.Find(x, y, map, out index);
        
        if (findResult != null)
        {
            findResult.GetComponent<Entity>().Damaged(index.Key, index.Value, map);
        }

        return null;
    }
}
