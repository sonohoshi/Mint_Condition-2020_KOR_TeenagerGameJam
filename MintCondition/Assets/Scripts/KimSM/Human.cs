using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Human : Entity
{
    private bool isFinding;
    private int bullet;
    private int moveCount;
    private Animator playerAnimator;
    private List<KeyValuePair<MoveDirection, MoveDirection>> shotDirectionList;
    
    public bool IsPlayer;
    public bool IsGuilty;
    public bool IsGuard;

    public Sprite[] directionSprites;
    public int[,] map;

    void Start()
    {
        isFinding = false;
        moveCount = 0;
        map = GameManager.Instance.IsReal
            ? GameManager.Instance.RealMap[PrivateSceneManager.SceneManager.nowStage - 1]
            : GameManager.Instance.DreamMap[PrivateSceneManager.SceneManager.nowStage - 1];
        
        if (IsPlayer)
        {
            playerAnimator = GetComponent<Animator>();
            bullet = GameManager.Instance.MaxBullets[PrivateSceneManager.SceneManager.nowStage - 1];
            Debug.Log($"bullet : {bullet}");
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!IsPlayer) return;
        GetMovingInput();
        GetAttackInput();
    }

    public Human SetDirection(KeyValuePair<MoveDirection, MoveDirection> direction)
    {
        if (shotDirectionList == null)
        {
            shotDirectionList = new List<KeyValuePair<MoveDirection, MoveDirection>>();
        }
        shotDirectionList.Add(direction);
        
        if (shotDirectionList[0].Key == MoveDirection.DownOrRight &&
            shotDirectionList[0].Value == MoveDirection.InPlace)
        {
            GetComponent<SpriteRenderer>().sprite = directionSprites[0];
        }
        else if (shotDirectionList[0].Key == MoveDirection.InPlace &&
                 shotDirectionList[0].Value == MoveDirection.UpOrLeft)
        {
            GetComponent<SpriteRenderer>().sprite = directionSprites[1];
        }
        else if (shotDirectionList[0].Key == MoveDirection.InPlace &&
                 shotDirectionList[0].Value == MoveDirection.DownOrRight)
        {
            GetComponent<SpriteRenderer>().sprite = directionSprites[2];
        }
        else if (shotDirectionList[0].Key == MoveDirection.UpOrLeft &&
                  shotDirectionList[0].Value == MoveDirection.InPlace)
        {
            GetComponent<SpriteRenderer>().sprite = directionSprites[3];
        }
        
        return this;
    }

    public void FindMyDirection(int [,] map)
    {
        var keyValuePair = new KeyValuePair<int,int>(-1,-1);
        var x = shotDirectionList[0].Key;
        var y = shotDirectionList[0].Value;
        Debug.Log($"{gameObject.name} "+ Find(x, y, map, out keyValuePair));
    }

    private void GetMovingInput()
    {
        if (isFinding)
        {
            return;
        }
        int moveResult = 0;
        if (Input.GetKeyUp(KeyCode.W))
        {
            moveResult = Move(MoveDirection.UpOrLeft, MoveDirection.InPlace, map);
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            moveResult = Move(MoveDirection.InPlace, MoveDirection.UpOrLeft, map);
            var scale = transform.localScale;
            scale.x = -1.5f;
            transform.localScale = scale;
        }
        
        if (Input.GetKeyUp(KeyCode.S))
        {
            moveResult = Move(MoveDirection.DownOrRight, MoveDirection.InPlace, map);
        }
        
        if (Input.GetKeyUp(KeyCode.D))
        {
            moveResult = Move(MoveDirection.InPlace, MoveDirection.DownOrRight, map);
            var scale = transform.localScale;
            scale.x = 1.5f;
            transform.localScale = scale;
        }

        if (moveResult == 2)
        {
            isAnimating = true;
            playerAnimator.SetTrigger("StartKick");
            StartCoroutine(CheckAnimationCompleted("PlayerKick", (() =>
            {
                playerAnimator.SetTrigger("EndKick");
                isAnimating = false;
            })));
        }

        if (moveResult == 1 || moveResult == 6)
        {
            isAnimating = true;
            moveCount++;
            Debug.Log($"move : {moveCount}");
            var turn3 = moveCount == 3;
            playerAnimator.SetTrigger("StartMove");
            StartCoroutine(CheckAnimationCompleted("PlayerMove", (() =>
            {
                playerAnimator.SetTrigger("EndMove");
                isAnimating = false;
                GameManager.Instance.DoFindAll(turn3);
                if (turn3)
                {
                    moveCount = 0;
                }
            })));
        }

        if (!playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("PlayerIdle") && !isMoving)
        {
            playerAnimator.SetTrigger("EndMove");
            playerAnimator.SetTrigger("EndKick");
        }
    }

    private void GetAttackInput()
    {
        if (isFinding || bullet <= 0)
        {
            return;
        }

        KeyValuePair<int, int> findResult = new KeyValuePair<int, int>(-1, -1);
        KeyValuePair<MoveDirection,MoveDirection> firingPos = new KeyValuePair<MoveDirection, MoveDirection>();
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            Find(MoveDirection.UpOrLeft, MoveDirection.InPlace, map, out findResult);
            firingPos = new KeyValuePair<MoveDirection, MoveDirection>(MoveDirection.UpOrLeft, MoveDirection.InPlace);
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            Find(MoveDirection.InPlace, MoveDirection.UpOrLeft, map, out findResult);
            firingPos = new KeyValuePair<MoveDirection, MoveDirection>(MoveDirection.InPlace, MoveDirection.UpOrLeft);
        }
        
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            Find(MoveDirection.DownOrRight, MoveDirection.InPlace, map, out findResult);
            firingPos = new KeyValuePair<MoveDirection, MoveDirection>(MoveDirection.DownOrRight, MoveDirection.InPlace);
        }
        
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            Find(MoveDirection.InPlace, MoveDirection.DownOrRight, map, out findResult);
            firingPos = new KeyValuePair<MoveDirection, MoveDirection>(MoveDirection.InPlace, MoveDirection.DownOrRight);
        }

        if (findResult.Key != -1)
        {
            bullet--;
            isFinding = true;
            playerAnimator.SetTrigger("StartAttack");
            StartCoroutine(CheckAnimationCompleted("PlayerShot", (() =>
            {
                playerAnimator.SetTrigger("EndAttack");
                isFinding = false;
            })));
            
            if (GameManager.Instance.IsReal)
            {
                var guiltyObj = Instantiate(GameManager.Instance.Obj[7], new Vector3(posY * tileSize, -posX * tileSize, 0),
                    Quaternion.identity);
                GameManager.Instance.SubMap[posX,posY] = guiltyObj.GetComponent<Human>().SetIsGuilty(true).
                    SetDirection(firingPos).
                    SetMyType(7).
                    SetXAndY(posX,posY).
                    gameObject;
                GameManager.Instance.SubMap[posX,posY].SetActive(false);
                GameManager.Instance.DreamMap[PrivateSceneManager.SceneManager.nowStage - 1][posX, posY] = 7;
            }
        }

        if (!playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("PlayerIdle") && !isFinding)
        {
            playerAnimator.SetTrigger("EndAttack");
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

        if (moveResult == exit && !GameManager.Instance.IsReal)
        {
            PrivateSceneManager.SceneManager.nowStage++;
            SceneManager.LoadScene("IMGCutScene");
        }
        else if (moveResult == exit && GameManager.Instance.IsReal)
        {
            GameManager.Instance.LoadDream();
        }
        return moveResult;
    }

    public override GameObject Find(MoveDirection x, MoveDirection y, int[,] map, out KeyValuePair<int,int> index)
    {
        var findResult = base.Find(x, y, map, out index);
        
        if (findResult != null)
        {
            findResult.GetComponent<Entity>().Damaged(index.Key, index.Value, map);
        }

        return null;
    }

    public override void Damaged(int x, int y, int[,] map)
    {
        base.Damaged(x, y, map);
        Debug.Log($"human damaged {x}, {y}");
        Destroy(gameObject);
    }

    public Human SetIsPlayer(bool isPlr)
    {
        IsPlayer = isPlr;
        IsGuard = IsGuilty = false;
        return this;
    }

    public Human SetIsGuard(bool isGrd)
    {
        IsGuard = isGrd;
        IsPlayer = IsGuilty = false;
        return this;
    }

    public Human SetIsGuilty(bool isGlt)
    {
        IsGuilty = isGlt;
        IsPlayer = IsGuard = false;
        return this;
    }

    private IEnumerator CheckAnimationCompleted(string currentAnim, Action onComplete)
    {
        while (!playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(currentAnim))
        {
            yield return null;
        }

        onComplete?.Invoke();
    }

    private void UpdateMap()
    {
        map = GameManager.Instance.DreamMap[PrivateSceneManager.SceneManager.nowStage - 1];
    }
}
