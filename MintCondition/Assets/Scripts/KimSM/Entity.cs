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
    
    protected float tileSize;
    protected bool isMoving;
    
    public int posX, posY;
    public int HealthPoint;

    private int myType;

    void Awake()
    {
        tileSize = 5f;
        Debug.Log(tileSize);
        isMoving = false;
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
        if (isMoving)
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
        if (map[toPosX, toPosY] == wall || map[toPosX, toPosY] == box || map[toPosX, toPosY] == enemy || map[toPosX, toPosY] == nullPointer)
        {
            return map[toPosX, toPosY];
        }

        isMoving = true;
        
        // 내가 이동하니까, 현재 자리를 이동할 수 있는 길인 1로 초기화
        map[posX, posY] = 1;
        GameManager.Instance.InGameMap[posX, posY] = GameManager.Instance.Obj[1];
        
        posX = toPosX;
        posY = toPosY;
        Debug.Log($"tilesize : {tileSize}");

        StartCoroutine(SmoothMove(transform, new Vector3(posY * tileSize, -posX * tileSize, 0)));
        
        map[posX, posY] = myType;
        GameManager.Instance.InGameMap[posX, posY] = this.gameObject;

        #if UNITY_EDITOR
        Debug.Log($"In Array : {posX}, {posY}");
        Debug.Log($"In Unity : {transform.position.x}, {transform.position.y}");
        #endif
        
        // 이동에 성공하면 1을 반환한다.
        return 1;
    }
    
    public GameObject Find(MoveDirection x, MoveDirection y, int[,] map)
    {
        var toPosX = posX + (int) x;
        var toPosY = posY + (int) y;

        if (!CheckIndexOutOfRangeInArray(toPosX, toPosY, map))
        {
            return null;
        } 
        
        while(CheckIndexOutOfRangeInArray(toPosX, toPosY, map))
        {
            
        }
        
        
        
        
        
        return null;
    }

    public void Damaged()
    {
        HealthPoint--;
        if (HealthPoint <= 0)
        {
            // To-Do Something... Animation or SE, etc.
            Destroy(gameObject);
        }
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
        for (float time = 0; time <= 1f; time += 0.1f)
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
}
