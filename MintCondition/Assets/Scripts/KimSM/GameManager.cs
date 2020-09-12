using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] Obj;
    private int[][,] map;

    void MapGeneration(int[,] map)
    {
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.Length/map.GetLength(0); j++)
            {
                Instantiate(Obj[map[i, j]], new Vector3(j, -i, 0),Quaternion.identity);
            }
        }
    }
    
    void Awake()
    {
        // 총 5개의 스테이지를 만들 것이므로 5개의 2차원 배열을 가지는 3차원 가변 배열 생성
        map = new int[5][,];
        // 1스테이지 맵 구조 초기화
        map[0] = new int[6, 9]
        {
            {1, 1, 1, 1, 1, 1, 4, 4, 4},
            {4, 4, 1, 0, 0, 1, 4, 4, 4},
            {4, 4, 1, 0, 0, 1, 4, 4, 4},
            {4, 4, 1, 0, 0, 1, 4, 4, 4},
            {4, 4, 1, 0, 0, 1, 4, 4, 4},
            {4, 4, 1, 1, 1, 3, 1, 1, 1}
        };
        // MapGenereation 메소드에는 각 스테이지-1을 인덱싱 해서 넣어주면 됨.
        MapGeneration(map[0]);
    }
}
