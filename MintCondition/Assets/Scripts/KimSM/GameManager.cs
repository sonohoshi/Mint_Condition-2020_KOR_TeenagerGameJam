using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public readonly float tileSize = 5f;
    
    public static GameManager Instance;
    
    public GameObject[] Obj;
    public GameObject[,] InGameMap;
    public int[][,] RealMap;
    public int[][,] DreamMap;
    public List<KeyValuePair<int, int>> FiringPosInRealList;

    private int[] cameraSize;
    private Human[] humans;

    void Awake()
    {
        FiringPosInRealList = new List<KeyValuePair<int, int>>();

        // 총 5개의 스테이지를 만들 것이므로 5개의 2차원 배열을 가지는 3차원 가변 배열 생성
        RealMap = new int[5][,];
        cameraSize = new int[5];
        // 1스테이지 맵 구조 초기화
        RealMap[0] = new int[5, 11]
        {
            {4,4,1,1,1,4,4,4,4,4,4},
            {4,4,1,4,1,4,4,4,4,4,4},
            {5,1,1,1,2,1,1,1,1,1,1},
            {4,4,1,4,4,4,1,4,4,4,4},
            {4,4,1,1,1,1,3,4,4,4,4}
        };

        cameraSize[0] = 18;
        
        MapInitializing(0);
        humans = FindObjectsOfType<Human>();
        Debug.Log(humans.Length);
        // 모든 초기 작업이 끝난 뒤 싱글턴 인스턴스 초기화
        Instance = this;
        Debug.Log(Instance);
    }

    public void DoFindAll()
    {
        foreach (var guard in humans)
        {
            if (guard.IsPlayer)
            {
                continue;
            }
        }
    }

    private void MapGeneration(int[,] map, string[] directions)
    {
        int guardIndex = 0;
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.Length/map.GetLength(0); j++)
            {
                /*
                 2차원 배열에서의 좌표계와 유니티 내의 좌표계는 차이가 있기 때문에, x와 y를 서로 바꿔 준 뒤
                 unity 안에서의 y값에 -를 곱해줍니다.
                 */
                InGameMap[i,j] = Instantiate(Obj[map[i, j]], new Vector3(j * tileSize, -i * tileSize, 0),Quaternion.identity);
                if (map[i, j] == 2)
                {
                    InGameMap[i, j].AddComponent<Entity>().SetXAndY(i, j).SetMyType(map[i, j]);
                }

                if (map[i, j] == 3)
                {
                    var direction = directions[guardIndex++].Split(',');
                    foreach (var dir in direction)
                    {
                        var one = (Entity.MoveDirection) int.Parse(dir.Split('_')[0]);
                        var two = (Entity.MoveDirection) int.Parse(dir.Split('_')[1]);
                        Debug.Log($"{one}, {two}");
                        InGameMap[i, j].GetComponent<Human>().SetDirection(new KeyValuePair<Entity.MoveDirection, Entity.MoveDirection>(one,two)).
                            SetXAndY(i, j).
                            SetMyType(map[i, j]);
                    }
                }
            }
        }
    }

    private void MapInitializing(int stage)
    {
        string[] directions = CSVParser.GetMapFile(stage);
        
        int x = RealMap[stage].GetLength(0);
        int y = RealMap[stage].Length / RealMap[0].GetLength(0);
        InGameMap = new GameObject[x, y];
        Camera.main.orthographicSize = cameraSize[stage];
        Debug.Log($"x:{x}, y:{y}");
        Camera.main.transform.position = new Vector3((y * tileSize) * 0.5f, (x * -2f), -10f);
        // MapGenereation 메소드에는 각 스테이지-1을 인덱싱 해서 넣어주면 됨.
        MapGeneration(RealMap[stage],directions);
    }
}
