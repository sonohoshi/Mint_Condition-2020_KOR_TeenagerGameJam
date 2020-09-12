using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public int[,] map;

    void Start()
    {
        map = GameManager.Instance.Map[0];
    }
    
    // Update is called once per frame
    void Update()
    {
        GetMovingInput();
    }

    private void GetMovingInput()
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
}
