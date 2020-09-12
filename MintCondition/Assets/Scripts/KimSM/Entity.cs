using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public GameObject[] Obj;
    
    private readonly int wall = 0;
    private readonly int box = 2;
    private readonly int enemy = 3;

    private int posX, posY;
    private int[,] map;
    
    // Move 메소드에 각 값을 넣으면 됨.
    public enum MoveDirection
    {
        UpOrLeft = -1,
        DownOrRight = 1,
        InPlace = 0
    }
    
    public int HealthPoint {get; private set;}
    
    void Awake()
    {
        /* ~Test Code~
        String arr = "";
        map = new int[5, 6]
        {
            {0, 0, 0, 0, 0, 0},
            {0, 1, 1, 1, 0, 0},
            {0, 1, 1, 1, 0, 0},
            {1, 1, 0, 0, 0, 0},
            {0, 1, 1, 0, 0, 0}
        };
        posX = 1;
        posY = 2;
        transform.position = new Vector3(posY,-posX,0);
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                Instantiate(Obj[map[i, j]], new Vector3(j, -i, 0),Quaternion.identity);
                arr += $"[{map[i, j]}] ";
            }
            arr += "\n";
        }
        Debug.Log($"{map.GetLength(0)}, {map.Length/map.GetLength(0)}");
        Debug.Log(arr);
        */
        //Move(MoveDirection.Left, MoveDirection.InPlace, new int[100,2]);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.W))
        {
            Debug.Log(Move(MoveDirection.UpOrLeft, MoveDirection.InPlace, map));
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            Debug.Log(Move(MoveDirection.InPlace, MoveDirection.UpOrLeft, map));
        }
        
        if (Input.GetKeyUp(KeyCode.S))
        {
            Debug.Log(Move(MoveDirection.DownOrRight, MoveDirection.InPlace, map));
        }
        
        if (Input.GetKeyUp(KeyCode.D))
        {
            Debug.Log(Move(MoveDirection.InPlace, MoveDirection.DownOrRight, map));
        }
    }

    public int Move(MoveDirection x, MoveDirection y, int [,] map)
    {
        Debug.Log($"Currently Pos : {posX}, {posY}");
        
        var sx = posX + (int) x;
        var sy = posY + (int) y;
        
        Debug.Log($"Post-Move Pos : {sx}, {sy}");
        if (sx < 0 || sx >= map.GetLength(0)) return 0;
        if (sy < 0 || sy >= map.Length / map.GetLength(0)) return 0;

        if (map[sx, sy] == wall || map[sx, sy] == box || map[sx,sy] == enemy)
            return map[sx, sy];

        posX = sx;
        posY = sy;
        transform.position = new Vector3(posY, -posX, 0);
        Debug.Log($"In Array : {posX}, {posY}");
        Debug.Log($"In Unity : {transform.position.x}, {transform.position.y}");
        return 1;
    }
}
