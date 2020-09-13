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

    public int posX, posY;
    public int HealthPoint;

    private int myType;

    void Awake()
    {
        tileSize = 5f;
        Debug.Log(tileSize);
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
        var sx = posX + (int) x;
        var sy = posY + (int) y;

        if (sx < 0 || sx >= map.GetLength(0))
        {
            return 0;
        }

        if (sy < 0 || sy >= map.Length / map.GetLength(0))
        {
            return 0;
        }

        if (map[sx, sy] == wall || map[sx, sy] == box || map[sx, sy] == enemy || map[sx, sy] == nullPointer)
        {
            return map[sx, sy];
        }

        // 내가 이동하니까, 현재 자리를 이동할 수 있는 길인 1로 초기화
        map[posX, posY] = 1;
        GameManager.Instance.InGameMap[posX, posY] = GameManager.Instance.Obj[1];
        
        posX = sx;
        posY = sy;
        Debug.Log($"tilesize : {tileSize}");
        transform.position = new Vector3(posY * tileSize, -posX * tileSize, 0);
        map[posX, posY] = myType;
        GameManager.Instance.InGameMap[posX, posY] = this.gameObject;

        #if UNITY_EDITOR
        Debug.Log($"In Array : {posX}, {posY}");
        Debug.Log($"In Unity : {transform.position.x}, {transform.position.y}");
        #endif
        
        return 1;
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
}
