using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected readonly int wall = 0;
    protected readonly int box = 2;
    protected readonly int enemy = 3;
    protected float tileSize;

    protected int posX, posY;

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

    public int HealthPoint;

    public virtual int Move(MoveDirection x, MoveDirection y, int [,] map)
    {
        var sx = posX + (int) x;
        var sy = posY + (int) y;
        
        if (sx < 0 || sx >= map.GetLength(0)) return 0;
        if (sy < 0 || sy >= map.Length / map.GetLength(0)) return 0;

        if (map[sx, sy] == wall || map[sx, sy] == box || map[sx,sy] == enemy)
            return map[sx, sy];

        posX = sx;
        posY = sy;
        Debug.Log($"tilesize : {tileSize}");
        transform.position = new Vector3(posY * tileSize, -posX * tileSize, 0);
        
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
}
