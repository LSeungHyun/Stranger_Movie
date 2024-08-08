using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MovingObject
{
    static public PlayerManager instance;

    public string lastMapName;
    public string currentMapName;

    public string walkSound_1;
    public string walkSound_2;
    public string walkSound_3;
    public string walkSound_4;

    private float soundTimer;
    public float soundCooldown = 280f;

    private AudioManager theAudio;

    public bool canMove = true;

    public bool notMove = false;
    public Vector3 targetPosition;

    public Coroutine moveCoroutine;

    void Start()
    {
        if (instance == null)
        {
            queue = new Queue<string>();
            DontDestroyOnLoad(this.gameObject);
            boxCollider = GetComponent<BoxCollider2D>();
            animator = GetComponent<Animator>();
            theAudio = FindObjectOfType<AudioManager>();
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        SceneManager.sceneLoaded += OnSceneLoaded; 
        animator = GetComponent<Animator>();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine); 
            moveCoroutine = null;
        }
        targetPosition = transform.position; 
        canMove = true; 
    }

    IEnumerator MoveCoroutine()
    {
        while ((Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) && !notMove)
        {
            vector.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);

            if (vector.x != 0)
                vector.y = 0;

            animator.SetFloat("DirX", vector.x);
            animator.SetFloat("DirY", vector.y);

            bool checkCollisionFlag = base.CheckCollision(); // 이 부분을 충돌 판정에 맞게 수정해야 함
            if (checkCollisionFlag)
                break;

            animator.SetBool("Walking", true);
            /*
            if (soundTimer >= soundCooldown)
            {
                int temp = Random.Range(1, 5);
                switch (temp)
                {
                    case 1:
                        theAudio.Play(walkSound_1);
                        break;
                    case 2:
                        theAudio.Play(walkSound_2);
                        break;
                    case 3:
                        theAudio.Play(walkSound_3);
                        break;
                    case 4:
                        theAudio.Play(walkSound_4);
                        break;
                }
                soundTimer = 0; // 사운드 타이머 초기화 필요
            }
            */
            float distanceToMove = speed * Time.deltaTime;
            targetPosition = transform.position + vector * distanceToMove;

            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, distanceToMove);
                soundTimer++;
                yield return null;
            }
        }
        animator.SetBool("Walking", false);
        canMove = true;
    }

    void Update()
    {
        if (canMove && !notMove)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                canMove = false;
                moveCoroutine = StartCoroutine(MoveCoroutine());
            }
        }
    }

    void OnDestroy()
    {
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
