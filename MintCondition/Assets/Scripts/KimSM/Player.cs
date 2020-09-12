using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public int[,] map;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
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
}
