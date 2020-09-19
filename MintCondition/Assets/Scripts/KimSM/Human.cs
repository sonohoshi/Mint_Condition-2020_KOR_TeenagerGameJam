using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Human : Entity
{
    private bool isFinding;
    private int bullet;
    private Animator playerAnimator;
    private readonly string[] guiltyHexColor = new [] {"#0DE7EE","#6E6EB0","#E3D85D","#6C4397","#BE667B"};
    private List<KeyValuePair<MoveDirection, MoveDirection>> shotDirectionList;

    public bool IsPlayer;
    public bool IsGuilty;
    public bool IsGuard;

    public GameObject HorizontalShootEffect;
    public GameObject VerticalShootEffect;
    public Sprite[] DirectionSprites;
    public int[,] Map;
    
    void Start()
    {
        isFinding = false;
        Map = GameManager.Instance.IsReal
            ? GameManager.Instance.RealMap[PrivateSceneManager.Manager.nowStage - 1]
            : GameManager.Instance.DreamMap[PrivateSceneManager.Manager.nowStage - 1];
        
        if (IsPlayer)
        {
            playerAnimator = GetComponent<Animator>();
            bullet = GameManager.Instance.MaxBullets[PrivateSceneManager.Manager.nowStage - 1];
            Debug.Log($"bullet : {bullet}");
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!IsPlayer || PrivateSceneManager.Manager.isStoryTelling) return;
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
            GetComponent<SpriteRenderer>().sprite = DirectionSprites[0];
        }
        else if (shotDirectionList[0].Key == MoveDirection.InPlace &&
                 shotDirectionList[0].Value == MoveDirection.UpOrLeft)
        {
            GetComponent<SpriteRenderer>().sprite = DirectionSprites[1];
        }
        else if (shotDirectionList[0].Key == MoveDirection.InPlace &&
                 shotDirectionList[0].Value == MoveDirection.DownOrRight)
        {
            GetComponent<SpriteRenderer>().sprite = DirectionSprites[2];
        }
        else if (shotDirectionList[0].Key == MoveDirection.UpOrLeft &&
                  shotDirectionList[0].Value == MoveDirection.InPlace)
        {
            GetComponent<SpriteRenderer>().sprite = DirectionSprites[3];
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
        if (Input.GetKeyDown(KeyCode.W) && GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("PlayerIdle"))
        {
            moveResult = Move(MoveDirection.UpOrLeft, MoveDirection.InPlace, Map);
        }

        if (Input.GetKeyDown(KeyCode.A) && GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("PlayerIdle"))
        {
            moveResult = Move(MoveDirection.InPlace, MoveDirection.UpOrLeft, Map);
            var scale = transform.localScale;
            scale.x = -1.5f;
            transform.localScale = scale;
        }
        
        if (Input.GetKeyDown(KeyCode.S) && GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("PlayerIdle"))
        {
            moveResult = Move(MoveDirection.DownOrRight, MoveDirection.InPlace, Map);
        }
        
        if (Input.GetKeyDown(KeyCode.D) && GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("PlayerIdle"))
        {
            moveResult = Move(MoveDirection.InPlace, MoveDirection.DownOrRight, Map);
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
                GameManager.Instance.DoFindAll();
                isAnimating = false;
            })));
        }

        if (moveResult == 1)
        {
            isAnimating = true;
            playerAnimator.SetTrigger("StartMove");
            StartCoroutine(CheckAnimationCompleted("PlayerMove", (() =>
            {
                playerAnimator.SetTrigger("EndMove");
                isAnimating = false;
                GameManager.Instance.DoFindAll();
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
        var isUpDirection = false;
        var isDownDirection = false;
        var getedInput = false;
        KeyValuePair<int, int> findResult = new KeyValuePair<int, int>(-1, -1);
        KeyValuePair<MoveDirection,MoveDirection> firingPos = new KeyValuePair<MoveDirection, MoveDirection>();
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (isFinding || bullet <= 0 || isMoving || isAnimating || !GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("PlayerIdle"))
            {
                return;
            }
            Debug.Log($"bullet : {bullet}");
            Find(MoveDirection.UpOrLeft, MoveDirection.InPlace, Map, out findResult);
            firingPos = new KeyValuePair<MoveDirection, MoveDirection>(MoveDirection.UpOrLeft, MoveDirection.InPlace);
            isUpDirection = true;
            
            bullet--;
            
            var verticalScale = VerticalShootEffect.transform.localScale;
            if (verticalScale.x < 0)
            {
                verticalScale.x *= -1;
                var transformPosition = VerticalShootEffect.transform.localPosition;
                transformPosition.y *= -1;
                VerticalShootEffect.transform.localScale = verticalScale;
                VerticalShootEffect.transform.localPosition = transformPosition;
            }
            
            var effect = VerticalShootEffect.GetComponent<ShootEffect>();
            effect.Shoot();

            getedInput = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (isFinding || bullet <= 0 || isMoving || isAnimating || !GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("PlayerIdle"))
            {
                return;
            }

            Find(MoveDirection.InPlace, MoveDirection.UpOrLeft, Map, out findResult);
            var scale = transform.localScale;
            scale.x = -1.5f;
            
            bullet--;
            var effect = HorizontalShootEffect.GetComponent<ShootEffect>();
            effect.Shoot();
            
            getedInput = true;
            
            transform.localScale = scale;
            firingPos = new KeyValuePair<MoveDirection, MoveDirection>(MoveDirection.InPlace, MoveDirection.UpOrLeft);
        }
        
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (isFinding || bullet <= 0 || isMoving || isAnimating || !GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("PlayerIdle"))
            {
                return;
            }
            Find(MoveDirection.DownOrRight, MoveDirection.InPlace, Map, out findResult);
            firingPos = new KeyValuePair<MoveDirection, MoveDirection>(MoveDirection.DownOrRight, MoveDirection.InPlace);
            isDownDirection = true;
            
            bullet--;
            
            var verticalScale = VerticalShootEffect.transform.localScale;
            if (verticalScale.x > 0)
            {
                verticalScale.x *= -1;
                var transformPosition = VerticalShootEffect.transform.localPosition;
                transformPosition.y *= -1;
                VerticalShootEffect.transform.localScale = verticalScale;
                VerticalShootEffect.transform.localPosition = transformPosition;
            }
            
            var effect = VerticalShootEffect.GetComponent<ShootEffect>();
            effect.Shoot();
            getedInput = true;
        }
        
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (isFinding || bullet <= 0 || isMoving || isAnimating || !GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("PlayerIdle"))
            {
                return;
            }

            Find(MoveDirection.InPlace, MoveDirection.DownOrRight, Map, out findResult);
            var scale = transform.localScale;
            scale.x = 1.5f;
            
            bullet--;
            var effect = HorizontalShootEffect.GetComponent<ShootEffect>();
            effect.Shoot();
            
            getedInput = true;
            
            transform.localScale = scale;
            firingPos = new KeyValuePair<MoveDirection, MoveDirection>(MoveDirection.InPlace, MoveDirection.DownOrRight);
        }

        if (getedInput)
        {
            GameManager.Instance.MaxBullets[PrivateSceneManager.Manager.nowStage - 1]--;
            isFinding = true;
            if (isUpDirection)
            {
                playerAnimator.SetTrigger("UpShotStart");
                StartCoroutine(CheckAnimationCompleted("PlayerUpShot", (() =>
                {
                    playerAnimator.SetTrigger("UpShotEnd");
                    isFinding = false;
                })));
            }
            else if (isDownDirection)
            {
                playerAnimator.SetTrigger("DownShotStart");
                StartCoroutine(CheckAnimationCompleted("PlayerDownShot", (() =>
                {
                    playerAnimator.SetTrigger("DownShotEnd");
                    isFinding = false;
                })));
            }
            else
            {
                playerAnimator.SetTrigger("StartAttack");

                StartCoroutine(CheckAnimationCompleted("PlayerShot", (() =>
                {
                    playerAnimator.SetTrigger("EndAttack");
                    isFinding = false;
                })));
            }

            if (GameManager.Instance.IsReal)
            {
                var guiltyObj = Instantiate(GameManager.Instance.Obj[7], new Vector3(posY * tileSize, -posX * tileSize, 0),
                    Quaternion.identity);
                GameManager.Instance.SubMap[posX,posY] = guiltyObj.GetComponent<Human>().SetIsGuilty(true).
                    SetDirection(firingPos).
                    SetMyType(7).
                    SetXAndY(posX,posY).
                    gameObject;
                var color = new Color();
                ColorUtility.TryParseHtmlString(guiltyHexColor[PrivateSceneManager.Manager.nowStage-1],out color);
                GameManager.Instance.SubMap[posX, posY].GetComponent<SpriteRenderer>().color = color;
                GameManager.Instance.SubMap[posX, posY].tag = "NotRemove";
                GameManager.Instance.DreamMap[PrivateSceneManager.Manager.nowStage - 1][posX, posY] = 7;
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
            PrivateSceneManager.Manager.nowStage++;
            SceneManager.LoadScene(PrivateSceneManager.Manager.nowStage < 3 || PrivateSceneManager.Manager.nowStage == 6
                ? "IMGCutScene"
                : $"Stage_{PrivateSceneManager.Manager.nowStage}");
        }
        else if (moveResult == exit && GameManager.Instance.IsReal)
        {
            bullet = 0;
            GameManager.Instance.LoadDream();
        }
        return moveResult;
    }

    public override GameObject Find(MoveDirection x, MoveDirection y, int[,] map, out KeyValuePair<int,int> index)
    {
        var findResult = base.Find(x, y, map, out index);

        if (findResult != null)
        {
            if (!IsPlayer && (map[index.Key, index.Value] == 2 || map[index.Key,index.Value] == 4))
            {
                return null;
            }
            findResult.GetComponent<Entity>().Damaged(index.Key, index.Value, map);
        }

        return null;
    }

    public override void Damaged(int x, int y, int[,] map)
    {
        base.Damaged(x, y, map);
        Debug.Log($"human damaged {x}, {y}");

        if (IsPlayer)
        {
            isMoving = true;
            isAnimating = true;
            isFinding = true;
            playerAnimator.SetTrigger("Dead");
            StartCoroutine(CheckAnimationCompleted("PlayerDead", () =>
            {
                //IsPlayer = false;
                GetComponent<SpriteRenderer>().sprite = null;
                playerAnimator.SetTrigger("DeadEnd");
            }));
        }
        else
        {
            if (shotDirectionList[0].Value > 0)
            {
                var transformScale = transform.localScale;
                transformScale.x *= -1;
                transform.localScale = transformScale;
            }
            var animator = GetComponent<Animator>();
            animator.SetTrigger("Damaged");
            StartCoroutine(CheckAnimationCompleted("GuardDamaged", (() => Destroy(gameObject))));
        }
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
}
