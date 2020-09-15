using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Entity
{
    private Animator playerAnimator;
    private List<KeyValuePair<MoveDirection, MoveDirection>> shotDirectionList;
    
    public bool IsPlayer;
    
    public int[,] map;

    void Start()
    {
        map = GameManager.Instance.RealMap[0];
        if (IsPlayer)
        {
            playerAnimator = GetComponent<Animator>();
        }
        Debug.Log("human start!!!");
    }
    
    // Update is called once per frame
    void Update()
    {
        if (IsPlayer)
        {
            GetMovingInput();
            GetAttackInput();
        }
        else
        {
            FindMyDirection(GameManager.Instance.RealMap[0]);
        }
    }

    public Human SetDirection(KeyValuePair<MoveDirection, MoveDirection> direction)
    {
        if (shotDirectionList == null)
        {
            shotDirectionList = new List<KeyValuePair<MoveDirection, MoveDirection>>();
        }
        shotDirectionList.Add(direction);
        return this;
    }

    public void FindMyDirection(int [,] map)
    {
        var x = shotDirectionList[0].Key;
        var y = shotDirectionList[0].Value;
        Debug.Log($"{gameObject.name} "+ Find(x, y, map, out var keyValuePair));
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
            var scale = transform.localScale;
            scale.x = -1.5f;
            transform.localScale = scale;
        }
        
        if (Input.GetKeyUp(KeyCode.S))
        {
            moveResult = Move(MoveDirection.DownOrRight, MoveDirection.InPlace, map);
        }
        
        if (Input.GetKeyUp(KeyCode.D))
        {
            moveResult = Move(MoveDirection.InPlace, MoveDirection.DownOrRight, map);
            var scale = transform.localScale;
            scale.x = 1.5f;
            transform.localScale = scale;
        }

        if (moveResult == 1)
        {
            GameManager.Instance.DoFindAll();
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
            GameManager.Instance.DoFindAll();
            playerAnimator.SetTrigger("StartAttack");
            StartCoroutine(CheckAnimationCompleted("PlayerShot", (() => playerAnimator.SetTrigger("EndAttack"))));
            GameManager.Instance.FiringPosInRealList.Add(new KeyValuePair<int, int>(posX,posY));
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

    public override GameObject Find(MoveDirection x, MoveDirection y, int[,] map, out KeyValuePair<int,int> index)
    {
        var findResult = base.Find(x, y, map, out index);
        
        if (findResult != null)
        {
            Debug.Log($"총 맞은 놈 : {index.Key}, {index.Value}");
            findResult.GetComponent<Entity>().Damaged(index.Key, index.Value, map);
        }

        return null;
    }

    public override void Damaged(int x, int y, int[,] map)
    {
        if (!IsPlayer)
        {
            GameManager.Instance.humans.Remove(this);
        }
        base.Damaged(x, y, map);
    }

    public Human SetIsPlayer(bool isPlr)
    {
        IsPlayer = isPlr;
        return this;
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
