using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public readonly float tileSize = 5f;
    
    public static GameManager Instance;

    public Sprite[] Backgrounds;
    public GameObject[] Obj;
    public GameObject[,] InGameMap;
    public GameObject[,] SubMap;
    public int[] MaxBullets;
    public int[][,] RealMap;
    public int[][,] DreamMap;
    public bool IsReal = true;

    private readonly int[] cameraSize = new[] {18, 25, 20, 20, 33};
    private int nowStage;

    void Awake()
    {
        MaxBullets = new int[5] {1, 1, 2, 1, 0};
        
        // 총 5개의 스테이지를 만들 것이므로 5개의 2차원 배열을 가지는 3차원 가변 배열 생성
        RealMap = new int[5][,];
        DreamMap = new int[5][,];
        // 1스테이지 맵 구조 초기화
        RealMap[0] = new int[5, 11]
        {
            {4, 4, 1, 1, 3, 4, 4, 4, 4, 4, 4},
            {4, 4, 1, 4, 1, 4, 4, 4, 4, 4, 4},
            {5, 1, 1, 1, 1, 1, 1, 1, 1, 1, 6},
            {4, 4, 1, 4, 4, 4, 1, 4, 4, 4, 4},
            {4, 4, 1, 1, 1, 1, 3, 4, 4, 4, 4}
        };
        DreamMap[0] = new int[5, 11]
        {
            {4, 4, 1, 1, 1, 4, 4, 4, 4, 4, 4},
            {4, 4, 1, 4, 1, 4, 4, 4, 4, 4, 4},
            {6, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5},
            {4, 4, 1, 4, 4, 4, 1, 4, 4, 4, 4},
            {4, 4, 1, 1, 1, 1, 1, 4, 4, 4, 4}
        };
        RealMap[1] = new int[9, 7]
        {
            {4, 4, 4, 1, 4, 4, 4},
            {4, 4, 4, 1, 4, 4, 4},
            {4, 5, 1, 1, 4, 4, 4},
            {4, 4, 4, 3, 4, 4, 4},
            {4, 4, 4, 1, 4, 4, 4},
            {4, 4, 4, 1, 4, 4, 4},
            {4, 4, 1, 1, 1, 4, 4},
            {4, 4, 1, 2, 1, 4, 4},
            {4, 3, 1, 1, 1, 1, 6}
        };
        DreamMap[1] = new int[9, 7]
        {
            {4, 4, 4, 1, 4, 4, 4},
            {4, 4, 4, 1, 4, 4, 4},
            {4, 6, 1, 1, 4, 4, 4},
            {4, 4, 4, 1, 4, 4, 4},
            {4, 4, 4, 1, 4, 4, 4},
            {4, 4, 4, 1, 4, 4, 4},
            {4, 4, 1, 1, 1, 4, 4},
            {4, 4, 1, 2, 1, 4, 4},
            {4, 1, 1, 1, 1, 1, 5}
        };
        RealMap[2] = new int[6, 12]
        {
            {4, 4, 4, 4, 4, 4, 4, 4, 3, 4, 4, 4},
            {4, 4, 4, 1, 1, 1, 1, 1, 1, 4, 4, 4},
            {4, 4, 4, 1, 1, 1, 1, 2, 1, 1, 1, 6},
            {4, 4, 1, 1, 1, 4, 1, 1, 4, 4, 4, 4},
            {4, 4, 1, 1, 3, 4, 4, 4, 4, 4, 4, 4},
            {5, 1, 1, 1, 1, 4, 4, 4, 4, 4, 4, 4}
        };
        DreamMap[2] = new int[6, 12]
        {
            {4, 4, 4, 4, 4, 4, 4, 4, 1, 4, 4, 4},
            {4, 4, 4, 1, 1, 1, 1, 1, 1, 4, 4, 4},
            {4, 4, 4, 1, 1, 1, 1, 2, 1, 1, 1, 5},
            {4, 4, 1, 1, 1, 4, 1, 1, 4, 4, 4, 4},
            {4, 4, 1, 1, 1, 4, 4, 4, 4, 4, 4, 4},
            {6, 1, 1, 1, 1, 4, 4, 4, 4, 4, 4, 4}
        };
        RealMap[3] = new int[5,11]
        {
            {5,1,1,1,1,1,1,1,4,4,4},
            {4,4,4,1,4,1,4,1,4,4,4},
            {4,4,4,1,1,3,2,1,4,4,4},
            {4,4,4,1,4,1,4,1,4,4,4},
            {4,3,1,1,1,1,1,1,1,1,6}
        };
        DreamMap[3] = new int[5,11]
        {
            {6,1,1,1,1,1,1,1,4,4,4},
            {4,4,4,1,4,1,4,1,4,4,4},
            {4,4,4,1,1,1,2,1,4,4,4},
            {4,4,4,1,4,1,4,1,4,4,4},
            {4,1,1,1,1,1,1,1,1,1,5}
        };
        RealMap[4] = new int[11, 12]
        {
            {4, 4, 4, 4, 4, 1, 1, 1, 1, 6, 4, 4},
            {4, 4, 4, 4, 4, 1, 4, 4, 4, 4, 4, 4},
            {4, 4, 4, 4, 4, 1, 1, 1, 1, 1, 1, 3},
            {4, 4, 4, 4, 4, 1, 4, 4, 4, 4, 2, 4},
            {4, 4, 4, 4, 4, 1, 1, 4, 4, 4, 1, 4},
            {4, 4, 4, 4, 4, 1, 2, 4, 4, 4, 1, 4},
            {4, 4, 4, 4, 4, 1, 1, 4, 1, 4, 1, 4},
            {4, 4, 4, 1, 2, 2, 2, 1, 1, 1, 1, 4},
            {4, 4, 1, 2, 2, 1, 1, 2, 2, 1, 4, 4},
            {5, 1, 1, 1, 2, 1, 1, 2, 1, 4, 4, 4},
            {4, 4, 1, 4, 4, 4, 4, 4, 4, 4, 4, 4}
        };
        DreamMap[4] = new int[11, 12]
        {
            {4, 4, 4, 4, 4, 1, 1, 1, 1, 5, 4, 4},
            {4, 4, 4, 4, 4, 1, 4, 4, 4, 4, 4, 4},
            {4, 4, 4, 4, 4, 1, 1, 1, 1, 1, 1, 1},
            {4, 4, 4, 4, 4, 1, 4, 4, 4, 4, 2, 4},
            {4, 4, 4, 4, 4, 1, 1, 4, 4, 4, 1, 4},
            {4, 4, 4, 4, 4, 1, 2, 4, 4, 4, 1, 4},
            {4, 4, 4, 4, 4, 1, 1, 4, 1, 4, 1, 4},
            {4, 4, 4, 1, 2, 2, 2, 1, 1, 1, 1, 4},
            {4, 4, 1, 2, 2, 1, 1, 2, 2, 1, 4, 4},
            {6, 1, 1, 1, 2, 1, 1, 2, 1, 4, 4, 4},
            {4, 4, 1, 4, 4, 4, 4, 4, 4, 4, 4, 4}
        };

        nowStage = PrivateSceneManager.Manager.nowStage - 1;
        MapInitializing(nowStage, IsReal);

        // 모든 초기 작업이 끝난 뒤 싱글턴 인스턴스 초기화
        Instance = this;
        Debug.Log(Instance);
    }

    public void DoFindAll()
    {
        foreach (var guard in FindObjectsOfType<Human>())
        {
            if (guard.IsPlayer)
            {
                continue;
            }
            
            guard.FindMyDirection(IsReal ? RealMap[nowStage] : DreamMap[nowStage]);
        }
    }

    public void LoadDream()
    {
        IsReal = false;
        var objs = GameObject.FindGameObjectsWithTag("Object");
        var bg = GameObject.FindWithTag("StageBG").GetComponent<SpriteRenderer>();
        bg.sprite = Backgrounds[PrivateSceneManager.Manager.nowStage - 1];
        
        foreach (var obj in objs)
        {
            Destroy(obj);
        }
        Debug.Log($"sub map : {SubMap[0,0]}");
        foreach (var obj in SubMap)
        {
            obj?.SetActive(true);
        }
        
        InGameMap = SubMap;
        if (PrivateSceneManager.Manager.nowStage < 3)
        {
            PrivateSceneManager.Manager.isStoryTelling = true;
            SceneManager.LoadScene("IMGCutScene", LoadSceneMode.Additive);
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
                var type = map[i, j];
                InGameMap[i,j] = Instantiate(Obj[map[i, j]], new Vector3(j * tileSize, -i * tileSize, 0),Quaternion.identity);
                if (type == 5)
                {
                    type = 6;
                }
                else if (type == 6)
                {
                    type = 5;
                }
                else if (type == 3)
                {
                    type = 1;
                }
                SubMap[i,j] = Instantiate(Obj[type], new Vector3(j * tileSize, -i * tileSize, 0),Quaternion.identity);
                if (type == 5)
                {
                    SubMap[i, j].GetComponent<Human>().SetIsPlayer(true).
                        SetXAndY(i, j).
                        SetMyType(type);
                }
                SubMap[i,j].SetActive(false);
                if (map[i, j] == 2)
                {
                    InGameMap[i, j].AddComponent<Box>().SetXAndY(i, j).SetMyType(map[i, j]);
                    SubMap[i, j].AddComponent<Box>().SetXAndY(i, j).SetMyType(map[i, j]);
                }

                if (map[i, j] == 3)
                {
                    Debug.Log($"n번째 경비원 : {guardIndex + 1}");
                    var direction = directions[guardIndex++].Split(',');
                    foreach (var dir in direction)
                    {
                        var one = (Entity.MoveDirection) int.Parse(dir.Split('_')[0]);
                        var two = (Entity.MoveDirection) int.Parse(dir.Split('_')[1]);
                        InGameMap[i, j].GetComponent<Human>().SetDirection(new KeyValuePair<Entity.MoveDirection, Entity.MoveDirection>(one,two)).
                            SetIsGuard(true).
                            SetXAndY(i, j).
                            SetMyType(map[i, j]);
                    }
                }

                if (map[i, j] == 5)
                {
                    InGameMap[i, j].GetComponent<Human>().SetIsPlayer(true).
                        SetXAndY(i, j).
                        SetMyType(map[i, j]);
                }
            }
        }
    }

    private void MapInitializing(int stage, bool isReal)
    {
        string[] directions = CSVParser.GetMapFile(stage);
        
        int x = RealMap[stage].GetLength(0);
        int y = RealMap[stage].Length / RealMap[0].GetLength(0);
        InGameMap = new GameObject[x, y];
        SubMap = new GameObject[x,y];
        Camera.main.orthographicSize = cameraSize[stage];

        // MapGenereation 메소드에는 각 스테이지-1을 인덱싱 해서 넣어주면 됨.
        if (isReal)
        {
            MapGeneration(RealMap[stage],directions);
        }
        else
        {
            MapGeneration(DreamMap[stage],directions);
        }
    }
}
