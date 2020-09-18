using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected readonly int wall = 0;
    protected readonly int box = 2;
    protected readonly int enemy = 3;
    protected readonly int nullPointer = 4;
    protected readonly int player = 5;
    protected readonly int exit = 6;
    protected readonly int guilty = 7;
    
    protected float tileSize;
    protected bool isMoving;
    protected bool isAnimating;
    protected bool wasExit;
    
    public int posX, posY;

    private int myType;

    void Awake()
    {
        tileSize = 5f;
        isMoving = false;
        isAnimating = false;
        wasExit = false;
    }
    
    // Move 메소드에 각 값을 넣으면 됨.
    public enum MoveDirection
    {
        UpOrLeft = -1,
        DownOrRight = 1,
        InPlace = 0
    }

    public virtual int Move(MoveDirection x, MoveDirection y, int [,] map)
    {
        Debug.Log($"moving : {isMoving}, ani : {isAnimating}");
        if (isMoving || isAnimating)
        {
            return 0;
        }
        
        var toPosX = posX + (int) x;
        var toPosY = posY + (int) y;
        
        if (!CheckIndexOutOfRangeInArray(toPosX, toPosY, map))
        {
            return 0;
        }

        // 경비병이나 상자 등에 의해 막혔을 경우, 왜 막혔는지를 확인하기 위해 진로를 가로막은 오브젝트의 종류를 반환한다.
        if (map[toPosX, toPosY] == wall 
            || map[toPosX, toPosY] == box 
            || map[toPosX, toPosY] == enemy 
            || map[toPosX, toPosY] == nullPointer
            || map[toPosX,toPosY] == guilty)
        {
            return map[toPosX, toPosY];
        }

        isMoving = true;
        
        // 내가 이동하니까, 현재 자리를 이동할 수 있는 길인 1로 초기화
        map[posX, posY] = 1;
        GameManager.Instance.InGameMap[posX, posY] = GameManager.Instance.Obj[1];
        
        posX = toPosX;
        posY = toPosY;

        StartCoroutine(SmoothMove(transform, new Vector3(posY * tileSize, -posX * tileSize, 0)));

        var wasType = map[posX, posY];

        Debug.Log($"{gameObject.name} : was : {wasType}");
        if (wasType == 6)
        {
            wasExit = true;
        }
        
        map[posX, posY] = myType;

        GameManager.Instance.InGameMap[posX, posY] = this.gameObject;

        #if UNITY_EDITOR
        Debug.Log($"In Array : {posX}, {posY}");
        Debug.Log($"In Unity : {transform.position.x}, {transform.position.y}");
        #endif
        
        // 이동에 성공하면 1을 반환한다.
        return wasType;
    }
    
    public virtual GameObject Find(MoveDirection x, MoveDirection y, int[,] map, out KeyValuePair<int,int> storeInThis)
    {
        var isNotOuted = false;
        var toPosX = posX + (int) x;
        var toPosY = posY + (int) y;

        if (!CheckIndexOutOfRangeInArray(toPosX, toPosY, map))
        {
            return null;
        } 
        
        // 배열의 범위를 벗어나지 않고 다음 확인할 수 있는 칸이 지나갈 수 있는 길일 때만 반복
        while(isNotOuted = CheckIndexOutOfRangeInArray(toPosX, toPosY, map))
        {
            if (map[toPosX,toPosY] == box || map[toPosX,toPosY] == enemy || map[toPosX,toPosY] == player)
            {
                break;
            }

            if (map[toPosX, toPosY] == nullPointer)
            {
                isNotOuted = false;
                break;
            }
            
            toPosX += (int) x;
            toPosY += (int) y;
        }
        storeInThis = new KeyValuePair<int, int>(toPosX,toPosY);

        return isNotOuted ? GameManager.Instance.InGameMap[toPosX, toPosY] : null;
    }

    public virtual void Damaged(int x, int y, int[,] map)
    {
        map[x, y] = 1;
        GameManager.Instance.InGameMap[x,y] = GameManager.Instance.Obj[1];
        //Destroy(gameObject);
    }

    public Entity SetXAndY(int x, int y)
    {
        posX = x;
        posY = y;
        return this;
    }

    public Entity SetMyType(int type)
    {
        myType = type;
        return this;
    }

    protected IEnumerator SmoothMove(Transform original, Vector3 moveTo)
    {
        for (float time = 0; time <= 1f; time += 0.2f)
        {
            original.position = Vector3.Lerp(original.position, moveTo, time);
            yield return new WaitForSeconds(.1f);
        }

        isMoving = false;
    }

    protected bool CheckIndexOutOfRangeInArray(int x, int y, int[,] map)
    {
        return (x >= 0 && x < map.GetLength(0)) && (y >= 0 && y < map.Length / map.GetLength(0));
    }
    
    protected IEnumerator CheckAnimationCompleted(string currentAnim, Action onComplete)
    {
        var animator = GetComponent<Animator>();
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            yield return null;
        }

        onComplete?.Invoke();
    }
}
