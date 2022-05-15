using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Player Speed")]
    public float _speedPlayer = 1.0f;//скорость персонажа
    private float moveForward = 0.0f;

    [SerializeField] private float _speedFallValue = 0.2f;//скорость персонажа, когда он попадает в ловушку
    private float moveMultiple = 0.1f;

    [Range(0, 15)] private float visibleAcceleration = 0;
    [SerializeField, Range(0, 10)] private float speedAcceleration = 2;

    [Header ("Jump and Slide")]
    [HideInInspector] public bool goBack;
    public float _jumpForce = 400.0f;
    [SerializeField] private float _slideTime = 2.0f;
    [SerializeField] private float _slideStoppedValue = 0.1f;
    public bool JumpActive = false;
    [SerializeField] private bool slide = false;
    private bool fall = false;

    [HideInInspector] public float _healPoint = 1.0f;
    [SerializeField] private float _rayToGroundSize = 0.5f;//регулирует длину луча до земли, что бы определять, стоим мы или в полете
    [HideInInspector] public bool onTheGround = false;

    private Animator anim;//присваиваем при старте аниматор с персонажа

    [HideInInspector] public bool stopGame = true;
    private bool xToRight = true;// проверяем, смотрит ли персонаж на право
    [HideInInspector] public bool yToStandatre = true;// проверяем, перевернут ли персонаж или нет

    private Rigidbody2D _rigidbody2D;
    private CapsuleCollider2D _collider2D;
    private Vector2 _coliderSize;//переменная хранит базовые значения коллайдера при запуске уровня

    private int _goReverse = 0;//за счет этой переменной будем обращать силу прыжка и другие влияющие на персонажа моменты в игре, в зависимости от гравитации

    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip runClip;
    [SerializeField] private AudioClip slideClip;
    [SerializeField] private AudioClip dieClip;

    [Header("Portal")]
    [SerializeField] private GameObject portal;
    private GameObject portalGO;

    [SerializeField] private GameObject spide;
    [SerializeField] private GameObject speedWind;

    [SerializeField] private Transform startPosition;

    private void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<CapsuleCollider2D>();
        _coliderSize = _collider2D.size;
        gameObject.transform.position = startPosition.position;
        Time.timeScale = 1;
        visibleAcceleration = 0;
    }

    public void StartGame()
    {
        stopGame = false;
        gameObject.transform.position = startPosition.position;
        portalGO = Instantiate(portal, new Vector2(10, 0), Quaternion.identity);
        portalGO.SetActive(false);
    }

    public void ResetGame()
    {
        anim.SetTrigger("StopAll");
        gameObject.transform.position = startPosition.position;
    }

    [HideInInspector] public bool inversGravity = false;

    private void GravityControl()
    {
        if (stopGame) return;

        float _goRay = 0.0f;//куда будем пускать луч для определения поверхности под нами

        //---------------------------------------------------------Управление гравитацией------------------------------------------------------------------
        if (inversGravity)//если гравитация обратная
        {
            _goReverse = -1;
            _goRay = (gameObject.GetComponent<Collider2D>().bounds.max.y + 0.01f);

            _rigidbody2D.gravityScale = _goReverse;
            if (yToStandatre)//если гравитация обычная и мы еще не перевернули персонажа
            {
                yToStandatre = false;
                TransformRotate(true);
            }
        }
        else
        {//если гравитация обычная
            _goReverse = 1;
            _goRay = (gameObject.GetComponent<Collider2D>().bounds.min.y - 0.01f);

            _rigidbody2D.gravityScale = _goReverse;
            if (!yToStandatre)//если гравитация обратная и мы еще не перевернули персонажа
            {
                yToStandatre = true;
                TransformRotate(true);
            }
        }

        Debug.DrawRay(new Vector3(gameObject.transform.position.x, _goRay, 0),
            gameObject.transform.up * _goReverse * -_rayToGroundSize, Color.green, 0.0f, false);//визуализировал луч, для удобства настройки
        RaycastHit2D ray = Physics2D.Raycast(new Vector3(gameObject.transform.position.x, _goRay, 0),
            Vector2.up * _goReverse * -_rayToGroundSize, _rayToGroundSize);
        if (ray.collider != null)//если мы стоим на чем-то
        {
            onTheGround = true;
        }
        else
        {
            onTheGround = false;
        }
    }

    internal bool root = false;
    public void RootHand()
    {
        anim.SetTrigger("Root");
        //_rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        gameObject.transform.localScale = new Vector2(1, gameObject.transform.localScale.y);
        _rigidbody2D.velocity = new Vector2(0, 0);
        _rigidbody2D.isKinematic = true;
        root = true;
    }

    private float valueSt = 0;
    private void Move()
    {
        if (stopGame) return;

        if (!root)
        {
            if (!slide && onTheGround && valueSt > 0.35f)
            {
                if (moveForward != 0.0f)
                {
                    if (SoundManager.Instance) if (SoundManager.Instance) SoundManager.Instance.ClipPlay(runClip);
                    valueSt = 0.0f;
                }
            }
            else
                valueSt += Time.deltaTime;

            if (!goBack)
            {
                if (moveForward < 1)
                    moveForward += moveMultiple;
                else
                    moveForward = 1;
            }
            else
            {
                if (moveForward > -1)
                    moveForward -= moveMultiple;
                else
                    moveForward = -1;
            }

            if (moveForward < 0 && xToRight) //если персонаж смотрит направо, а мы двигаемся влево, то разворачиваем картинку
            {
                xToRight = false;
                TransformRotate(false);//если надо повернуть персонажа по горизонтали, то передаем в функцию false, если по вертикали, то true
                _collider2D.offset = new Vector2(0.0f, -0.03f);
                anim.SetBool("SlideUp", false);
                anim.SetBool("SlideDown", false);
            }
            else
            {
                if (moveForward > 0 && !xToRight)// если смотрит налево, а движемся направо, то разворачиваем картинку
                {
                    xToRight = true;
                    TransformRotate(false);
                    _collider2D.offset = new Vector2(0.0f, -0.03f);
                    anim.SetBool("SlideUp", false);
                    anim.SetBool("SlideDown", false);
                }
            }

            {
                /*RaycastHit2D rayForward;
                if (xToRight)
                {
                    Debug.DrawRay(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0),
                        gameObject.transform.right * (_rayToGroundSize * 2), Color.green, 0.0f, false);
                    rayForward = Physics2D.Raycast(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0),
                        Vector2.right * _rayToGroundSize, _rayToGroundSize);
                    if (rayForward.collider == null)
                    {
                        Debug.DrawRay(new Vector3(gameObject.transform.position.x, (_collider2D.bounds.max.y - _collider2D.size.y / 5), 0),
                        gameObject.transform.right * (_rayToGroundSize * 2), Color.green, 0.0f, false);
                        rayForward = Physics2D.Raycast(new Vector3(gameObject.transform.position.x, _collider2D.bounds.max.y, 0),
                            Vector2.right * _rayToGroundSize, _rayToGroundSize);
                        if (rayForward.collider == null)
                        {
                            Debug.DrawRay(new Vector3(gameObject.transform.position.x, (_collider2D.bounds.min.y + _collider2D.size.y / 5), 0),
                                gameObject.transform.right * (_rayToGroundSize * 2), Color.green, 0.0f, false);
                            rayForward = Physics2D.Raycast(new Vector3(gameObject.transform.position.x, _collider2D.bounds.min.y, 0),
                                Vector2.right * _rayToGroundSize, _rayToGroundSize);
                            if (rayForward.collider != null && (!rayForward.collider.isTrigger && rayForward.collider.tag != "Ground"))
                                moveForward = 0.0f;
                        }
                        else
                             if (!rayForward.collider.isTrigger && rayForward.collider.tag != "Ground")
                            moveForward = 0.0f;
                    }
                    else
                        if (!rayForward.collider.isTrigger && rayForward.collider.tag != "Ground")
                        moveForward = 0.0f;
                }
                else
                {
                    Debug.DrawRay(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0),
                        gameObject.transform.right * (-_rayToGroundSize * 2), Color.green, 0.0f, false);
                    rayForward = Physics2D.Raycast(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0),
                        Vector2.right * -_rayToGroundSize, _rayToGroundSize);
                    if (rayForward.collider == null)
                    {
                        Debug.DrawRay(new Vector3(gameObject.transform.position.x, (_collider2D.bounds.max.y - _collider2D.size.y / 5), 0),
                        gameObject.transform.right * (-_rayToGroundSize * 2), Color.green, 0.0f, false);
                        rayForward = Physics2D.Raycast(new Vector3(gameObject.transform.position.x, _collider2D.bounds.max.y, 0),
                            Vector2.right * -_rayToGroundSize, _rayToGroundSize);
                        if (rayForward.collider == null)
                        {
                            Debug.DrawRay(new Vector3(gameObject.transform.position.x, (_collider2D.bounds.min.y + _collider2D.size.y / 5), 0),
                                gameObject.transform.right * (-_rayToGroundSize * 2), Color.green, 0.0f, false);
                            rayForward = Physics2D.Raycast(new Vector3(gameObject.transform.position.x, _collider2D.bounds.min.y, 0),
                                Vector2.right * -_rayToGroundSize, _rayToGroundSize);
                            if (rayForward.collider != null && (!rayForward.collider.isTrigger && rayForward.collider.tag != "Ground"))
                                moveForward = 0.0f;
                        }
                        else
                            if (!rayForward.collider.isTrigger && rayForward.collider.tag != "Ground")
                            moveForward = 0.0f;
                    }
                    else
                        if (!rayForward.collider.isTrigger && rayForward.collider.tag != "Ground")
                        moveForward = 0.0f;
                }*/
            }

            PlayerPosController();
        }
        else
        {
            InstantiateLevel.Instance.moveSpeed = 0.0f;
            stopGame = true;
        }
    }

    [SerializeField] private float multiplayCameraSpeed = 1.8f;
    private void PlayerPosController()
    {
        if (gameObject.transform.position.x < (startPosition.position.x + visibleAcceleration) - _coliderSize.x / 3)
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x + speedAcceleration * Time.deltaTime, gameObject.transform.position.y);
            InstantiateLevel.Instance.moveSpeed = moveForward * _speedPlayer;
        }
        else
        if (gameObject.transform.position.x > (startPosition.position.x + visibleAcceleration) + _coliderSize.x / 3)
        {
            gameObject.transform.position = new Vector2(gameObject.transform.position.x - speedAcceleration * Time.deltaTime, gameObject.transform.position.y);
            InstantiateLevel.Instance.moveSpeed = moveForward * _speedPlayer * multiplayCameraSpeed;
        }
        else
        {
            gameObject.transform.position = new Vector2(startPosition.position.x + visibleAcceleration, gameObject.transform.position.y);
            InstantiateLevel.Instance.moveSpeed = moveForward * _speedPlayer;
        }

        anim.SetFloat("SpeedP", Mathf.Abs(moveForward));
    }

    public void CreatePortal()
    {
        if (!portalGO.activeInHierarchy)
        {
            portalGO.SetActive(true);
            portalGO.transform.position = new Vector2(gameObject.transform.position.x + 0.5f, gameObject.transform.position.y);
        }
    }

    public void Jump()
    {
        if (onTheGround)
        {
            _rigidbody2D.AddForce(new Vector2(0, _jumpForce * _goReverse));//прыгаем вверх, а так же немного в сторону куда бежим, если не бежим, то прыгаем только вверх
            anim.SetTrigger("Jump");
            if (SoundManager.Instance) SoundManager.Instance.ClipPlay(jumpClip);
            _collider2D.size = _coliderSize;
            _collider2D.offset = new Vector2(0.0f, -0.03f);
            anim.SetBool("SlideUp", false);
            anim.SetBool("SlideDown", false);
            if (jumpClip != null) if (SoundManager.Instance) SoundManager.Instance.ClipPlay(jumpClip);
            slide = false;
        }
    }

    private void FixedUpdate()
    {
        if (anim.speed != _speedPlayer)
            anim.speed = _speedPlayer;
    }

    private void Update()
    {
        if (stopGame) return;

        GravityControl();
        Move();

        anim.SetFloat("VelocityY", _rigidbody2D.velocity.y * _goReverse);


        //for test
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))//прыжок
        {
            if (!fall)
                Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!fall)
                SlideStart();
        }

        if (Input.GetKey(KeyCode.LeftShift))//если зажимаешь левый Shift, то коллайдер персонажа уменьается вдвое, когда отпускаем, увеличивается до базового значения
        {
            SlideDrag();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            CreatePortal();
        }
    }

    public void SlideStart()
    {
        if (!slide)
        {
            StopCoroutine(Slide());
            StartCoroutine(Slide());
            slide = true;
            if (slideClip != null) if (SoundManager.Instance) SoundManager.Instance.ClipPlay(slideClip);
        }
    }

    public void SlideDrag()
    {
        if (!slide) return;

        if (onTheGround)
        {
            _collider2D.size = new Vector2(_collider2D.size.x, _collider2D.size.y / 2);
            _collider2D.offset = new Vector2(0.0f, -0.25f);
            anim.SetBool("SlideDown", true);
        }
        else
        {
            _collider2D.size = new Vector2(_collider2D.size.x, _collider2D.size.y / 2);
            _collider2D.offset = new Vector2(0.0f, -0.25f);
            anim.SetBool("SlideUp", true);
        }
    }

    public void SlideStop()
    {
        if (!slide) return;

        _collider2D.size = _coliderSize;
        StopCoroutine(Slide());
        slide = false;

        _collider2D.offset = new Vector2(0.0f, -0.03f);
        anim.SetBool("SlideUp", false);
        anim.SetBool("SlideDown", false);
    }

    private IEnumerator Slide()
    {
        yield return new WaitForSeconds(_slideTime);
        {
            SlideStop();
            slide = false;
        }
    }

    private IEnumerator Fall()
    {
        fall = true;
        anim.SetTrigger("Fall");
        _rigidbody2D.AddForce(new Vector2(0, (_jumpForce * 0.5f) * _goReverse));
        if (speedWind != null && speedWind.activeInHierarchy) speedWind.SetActive(false);

        yield return new WaitForSeconds(0.5f);
        _speedPlayer = _speedPlayer * _speedFallValue;
        _collider2D.size = new Vector2(_collider2D.size.x, _collider2D.size.y / 2);
        _collider2D.offset = new Vector2(0.0f, -0.25f);

        yield return new WaitForSeconds(0.3f);
        _speedPlayer = 1;
        _collider2D.size = _coliderSize;
        _collider2D.offset = new Vector2(0.0f, -0.03f);

        fall = false;
    }

    private IEnumerator SpiderFall()
    {
        if (spide != null) spide.SetActive(true);
        _speedPlayer = _speedPlayer * _speedFallValue * 1.5f;
        _rigidbody2D.drag = 15.0f;
        if (speedWind != null && speedWind.activeInHierarchy) speedWind.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        _speedPlayer = 1;
        _rigidbody2D.drag = 0.0f;

        yield return new WaitForSeconds(0.2f);

        if (spide != null) spide.SetActive(false);
    }

    private IEnumerator SpeedUp()
    {
        if (speedWind != null) speedWind.SetActive(true);
        _speedPlayer = _speedPlayer * speedAcceleration;

        visibleAcceleration = InstantiateLevel.Instance.spawnPosition / 4;

        while (gameObject.transform.position.x < (startPosition.transform.position.x + visibleAcceleration))
            yield return new WaitForEndOfFrame();

        visibleAcceleration = 0;
        

        yield return new WaitForSeconds(2);

        if (speedWind != null) speedWind.SetActive(false);
        _speedPlayer = 1;
    }

    public void GoBack()
    {
        moveForward = -1;
    }

    private void TransformRotate(bool vertical)
    {
        if (!vertical)
        {
            //записываем размер картинки
            Vector3 _scaleP = gameObject.transform.localScale;
            _scaleP.x *= -1;//разворачиваем картинку
            gameObject.transform.localScale = _scaleP;//присваеваем значение персонажу и картинка смотрит в обратную сторону
        }
        else
        {
            Vector3 _scaleP = gameObject.transform.localScale;
            _scaleP.y *= -1;//разворачиваем картинку
            gameObject.transform.localScale = _scaleP;//присваеваем значение персонажу и картинка смотрит в обратную сторону
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<ChostFogController1>())
        {
            stopGame = true;
            anim.SetTrigger("Die");
            anim.SetBool("LDie", true);
            if (dieClip != null) if (SoundManager.Instance) SoundManager.Instance.ClipPlay(dieClip);
            GameLevel.Instance.DieScreenActive();

            if (!root) GameLevel.Instance.OnLose();

            moveForward = 0.0f;
            return;
        }

        if (collision.gameObject.GetComponent<HoleObstacle>())
        {
            InstantiateLevel.Instance.moveSpeed = moveForward * 0.8f;
            stopGame = true;
            anim.SetTrigger("Fall");
            GetComponent<CapsuleCollider2D>().isTrigger = true;
            if (dieClip != null) if (SoundManager.Instance) SoundManager.Instance.ClipPlay(dieClip);
            GameLevel.Instance.DieScreenActive();

            StartCoroutine(WaitTimeAtLose());

            return;
        }

        if (collision.gameObject.GetComponent<PointController>())
        {
            GameLevel.Instance.UpgradePoint(10);
            collision.gameObject.SetActive(false);
            return;
        }

        if (collision.gameObject.GetComponent<StumpFall>())
        {
            if (speedWind != null) speedWind.SetActive(false);
            StartCoroutine(Fall());
            return;
        }

        if (collision.gameObject.GetComponent<SpideFall>())
        {
            if (speedWind != null) speedWind.SetActive(false);
            collision.gameObject.GetComponent<SpideFall>().BreakSpide();
            StartCoroutine(SpiderFall());
            return;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlatformCollision>())
        {
            if (_speedPlayer <= 1)
            {
                if (speedWind != null) speedWind.SetActive(false);
                StartCoroutine(SpeedUp());
            }
        }
    }

    private IEnumerator WaitTimeAtLose()
    {
        yield return new WaitForSeconds(1);
        GameLevel.Instance.OnLose();
    }
}
