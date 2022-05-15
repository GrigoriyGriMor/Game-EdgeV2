using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarriorController : MonoBehaviour
{
    public GameObject _player;
    public GameObject _basePlace;
    private Vector3 cpv;
    private Vector2 _way;
    private bool _avoiding = false;

    private Animator animChost;

    private bool xToRightChost = false;

    [SerializeField]
    public float _attackDistance = 5.0f;

    private Rigidbody2D rd;

    [SerializeField]
    public float speedChost = 1.0f;
    [SerializeField]
    public float minDistanceTobase = 5.0f;

    private void Awake()
    {
        animChost = GetComponent<Animator>();
    }

    private void Start()
    {
        _player = GameObject.Find("Player");
        _basePlace = GameObject.Find("Base" + gameObject.name);
        rd = GetComponent<Rigidbody2D>();

    }

    private void Update()
    {
        if (Vector3.Distance(_player.transform.position, transform.position) <= _attackDistance)//определяем игрок в зоне атаки или нет
        {
            TrakingPlayer();
        }
        else
        {
            if (Vector3.Distance(_player.transform.position, transform.position) > _attackDistance)
            {
                if (Vector3.Distance(_basePlace.transform.position, transform.position) > minDistanceTobase)
                {
                    GoToBasePosition();
                }
            }
        }
    }

    private void TransformRotate()
    {
        //записываем размер картинки
        Vector3 _scaleP = gameObject.transform.localScale;
        _scaleP.x *= -1;//разворачиваем картинку
        gameObject.transform.localScale = _scaleP;//присваеваем значение персонажу и картинка смотрит в обратную сторону
    }

    private void TrakingPlayer()//двигаем призрака в сторону игрока, за счет использования velocity призраки немного притормаживают перед игроком, давай ему возможность сбежать
    {
        Vector3 cp_vector = _player.transform.position - gameObject.transform.position;//определяем направление вектора между призраком и игроком
        cpv = cp_vector;

        if (!_avoiding)
        {
            _way.x = Mathf.Clamp(Mathf.Abs(cpv.x), -1, 1);
            _way.y = Mathf.Clamp(Mathf.Abs(cpv.y), -1, 1);
        }

        RaycastHit2D ray = Physics2D.Raycast(gameObject.transform.position,
            cpv, _attackDistance / 5.0f);

        if (gameObject.name == "Chost10" && ray.collider != null)
        {
            Debug.Log(ray.collider.name);
        }
        Debug.DrawRay(gameObject.transform.position, cpv * 10, Color.green, 0.0f, false);
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        if (ray.collider != null && ray.collider.name != _player.name)
        {
            _avoiding = true;
            RaycastHit2D ray_Up = Physics2D.Raycast(gameObject.transform.position, Vector2.up, _attackDistance / 5.0f);
            RaycastHit2D ray_Right = Physics2D.Raycast(gameObject.transform.position, Vector2.right, _attackDistance / 5.0f);
            RaycastHit2D ray_Down = Physics2D.Raycast(gameObject.transform.position, Vector2.down, _attackDistance / 5.0f);
            RaycastHit2D ray_Left = Physics2D.Raycast(gameObject.transform.position, Vector2.left, _attackDistance / 5.0f);

            if ((ray_Left.collider != null && ray_Left.collider.name == ray.collider.name) || (ray_Right.collider != null && ray_Right.collider.name == ray.collider.name))
            {
                if (_way.y >= 0)
                {
                    rd.velocity = new Vector2(0, speedChost);//двигаем привидение в сторону ближайшую к обходу препядствия
                    animChost.SetFloat("SpeedChost", Mathf.Clamp(Mathf.Abs(cpv.x), -1, 1));//говорим аниматору, что мы движемся и надо играть анимацию ходьбы
                }
                else
                {
                    rd.velocity = new Vector2(0, -1 * speedChost);//двигаем привидение в сторону ближайшую к обходу препядствия
                    animChost.SetFloat("SpeedChost", Mathf.Clamp(Mathf.Abs(cpv.x), -1, 1));//говорим аниматору, что мы движемся и надо играть анимацию ходьбы
                }
            }

            if ((ray_Up.collider != null && ray_Up.collider.name == ray.collider.name) || (ray_Down.collider != null && ray_Down.collider.name == ray.collider.name))
            {
                if (_way.x >= 0)
                {
                    rd.velocity = new Vector2(speedChost, 0);//двигаем привидение в сторону ближайшую к обходу препядствия
                    animChost.SetFloat("SpeedChost", Mathf.Clamp(Mathf.Abs(cpv.x), -1, 1));//говорим аниматору, что мы движемся и надо играть анимацию ходьбы
                }
                else
                {
                    rd.velocity = new Vector2(-1 * speedChost, 0);//двигаем привидение в сторону ближайшую к обходу препядствия
                    animChost.SetFloat("SpeedChost", Mathf.Clamp(Mathf.Abs(cpv.x), -1, 1));//говорим аниматору, что мы движемся и надо играть анимацию ходьбы
                }
            }
        }
        else
        {
            rd.velocity = new Vector2(cp_vector.x * speedChost, cp_vector.y * speedChost);//двигаем привидение в сторону игрока
            animChost.SetFloat("SpeedChost", Mathf.Abs(cpv.x));//говорим аниматору, что мы движемся и надо играть анимацию ходьбы
            _avoiding = false;
        }

        //////////////////////////////////////////
        if (cpv.x < 0.0f && xToRightChost)
        {
            xToRightChost = false;
            TransformRotate();
        }
        else
        {
            if (cpv.x >= 0.0f && !xToRightChost)
            {
                xToRightChost = true;
                TransformRotate();
            }
        }
    }

    private void GoToBasePosition()
    {
        Vector3 cp_vector = _basePlace.transform.position - gameObject.transform.position;//определяем направление вектора между призраком и базовым положением
        cpv = cp_vector;

        if (!_avoiding)
        {
            _way.x = Mathf.Clamp(Mathf.Abs(cpv.x), -1, 1);
            _way.y = Mathf.Clamp(Mathf.Abs(cpv.y), -1, 1);
        }

        RaycastHit2D ray = Physics2D.Raycast(gameObject.transform.position,
            cpv, _attackDistance / 5.0f);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        if (ray.collider != null && ray.collider.name != _player.name)
        {
            _avoiding = true;
            RaycastHit2D ray_Up = Physics2D.Raycast(gameObject.transform.position, Vector2.up, _attackDistance / 5.0f);
            RaycastHit2D ray_Right = Physics2D.Raycast(gameObject.transform.position, Vector2.right, _attackDistance / 5.0f);
            RaycastHit2D ray_Down = Physics2D.Raycast(gameObject.transform.position, Vector2.down, _attackDistance / 5.0f);
            RaycastHit2D ray_Left = Physics2D.Raycast(gameObject.transform.position, Vector2.left, _attackDistance / 5.0f);

            if ((ray_Left.collider != null && ray_Left.collider.name == ray.collider.name) || (ray_Right.collider != null && ray_Right.collider.name == ray.collider.name))
            {
                if (_way.y >= 0)
                {
                    rd.velocity = new Vector2(0, speedChost);//двигаем привидение в сторону ближайшую к обходу препядствия
                    animChost.SetFloat("SpeedChost", Mathf.Clamp(Mathf.Abs(cpv.x), -1, 1));//говорим аниматору, что мы движемся и надо играть анимацию ходьбы
                }
                else
                {
                    rd.velocity = new Vector2(0, -1 * speedChost);//двигаем привидение в сторону ближайшую к обходу препядствия
                    animChost.SetFloat("SpeedChost", Mathf.Clamp(Mathf.Abs(cpv.x), -1, 1));//говорим аниматору, что мы движемся и надо играть анимацию ходьбы
                }
            }

            if ((ray_Up.collider != null && ray_Up.collider.name == ray.collider.name) || (ray_Down.collider != null && ray_Down.collider.name == ray.collider.name))
            {
                if (_way.x >= 0)
                {
                    rd.velocity = new Vector2(speedChost, 0);//двигаем привидение в сторону ближайшую к обходу препядствия
                    animChost.SetFloat("SpeedChost", Mathf.Clamp(Mathf.Abs(cpv.x), -1, 1));//говорим аниматору, что мы движемся и надо играть анимацию ходьбы
                }
                else
                {
                    rd.velocity = new Vector2(-1 * speedChost, 0);//двигаем привидение в сторону ближайшую к обходу препядствия
                    animChost.SetFloat("SpeedChost", Mathf.Clamp(Mathf.Abs(cpv.x), -1, 1));//говорим аниматору, что мы движемся и надо играть анимацию ходьбы
                }
            }
        }
        else
        {
            rd.velocity = new Vector2(cp_vector.x * speedChost, cp_vector.y * speedChost);//двигаем привидение в сторону игрока
            animChost.SetFloat("SpeedChost", Mathf.Abs(cpv.x));//говорим аниматору, что мы движемся и надо играть анимацию ходьбы
            _avoiding = false;
        }

        if (cpv.x < 0.0f && xToRightChost)
        {
            xToRightChost = false;
            TransformRotate();
        }
        else
        {
            if (cpv.x >= 0.0f && !xToRightChost)
            {
                xToRightChost = true;
                TransformRotate();
            }
        }
    }
}