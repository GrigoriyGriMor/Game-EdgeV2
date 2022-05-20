using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameLevel : MonoBehaviour
{
    private static GameLevel instance;
    public static GameLevel Instance => instance;

    [SerializeField] private Text newHistoryEvent;
    [SerializeField] private Text _time;
    [SerializeField] private Text _point;
    [SerializeField] private Text _pressToStart;
    [SerializeField] private Text _die;
    [SerializeField] private GameObject _dieScreen;

    private int bestResult = 0;
    private float evolutionPoint = 0.0f;
    private float time = 0.0f;

    [SerializeField] private PlayerController player;

    private bool loading = true;
    [SerializeField] private GameObject[] loadScreen = new GameObject[5];
    [HideInInspector] public GameObject LoadingScreen;//если прикрепить анимацию, она будет играть при старте игры 2 секунды

    [SerializeField] GameObject buttonCanvas;


    private bool gameIsPlaying = false;

    [SerializeField] private UnityEvent _onLose = new UnityEvent();

    private enum DifficultyLevel { Easy, Medium, Hurd, Impossible }
    private DifficultyLevel difficultyLevel = DifficultyLevel.Easy;

    private void Start()
    {
        instance = this;
        buttonCanvas.SetActive(false);
        LoadingScreen = Instantiate(loadScreen[Random.Range(0, loadScreen.Length)], new Vector2(0, 0), new Quaternion(0, 0, 0, 0));
        bestResult = PlayerPrefs.GetInt("BestResult");
        _point.text = "YOU BEST RESULT: " + bestResult;
        if (LoadingScreen != null)
        {
            LoadingScreen.SetActive(true);
            StartCoroutine(Loading());// запускаем загрузочный экран, что бы избежать лагающей задержки при загрузке уровня
        }
        if (newHistoryEvent != null) newHistoryEvent.color = new Color(newHistoryEvent.color.r, newHistoryEvent.color.g, newHistoryEvent.color.b, 0);
        if (_die != null) _die.color = new Color(_die.color.r, _die.color.g, _die.color.b, 0);
    }


    public IEnumerator Loading()
    {
        loading = true;
        yield return new WaitForSeconds(2);
        {
            loading = false;
            LoadingScreen.SetActive(false);
            Destroy(LoadingScreen);
            buttonCanvas.SetActive(true);
        }
    }

    private void StartGame()
    {
        if (loading) return;

        player.StartGame();
        _pressToStart.gameObject.SetActive(false);
        gameIsPlaying = true;
    }

    public void OnLose()
    {
        gameIsPlaying = false;
        InstantiateLevel.Instance.moveSpeed = 0.0f;
        StartCoroutine(onDie());
    }

    public void DieScreenActive()
    {
        StartCoroutine(DieScreen());
    }

    public IEnumerator DieScreen()
    {
        float colorSize = 0.0f;
        _dieScreen.SetActive(true);
        SpriteRenderer diescreen = _dieScreen.GetComponent<SpriteRenderer>();

        for (int i = 0; i < 5; i++)
        {
            colorSize += 0.2f;
            diescreen.color = new Color(_die.color.r, _die.color.g, _die.color.b, colorSize);
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator onDie()
    {
        float colorSize = 0.0f;
        for (int i = 0; i < 5; i++)
        {
            colorSize += 0.2f;
            _die.color = new Color(_die.color.r, _die.color.g, _die.color.b, colorSize);
            yield return new WaitForSeconds(0.5f);
        }

        if (Mathf.CeilToInt(evolutionPoint) > bestResult)
        PlayerPrefs.SetInt("BestResult", Mathf.CeilToInt(evolutionPoint));

        _onLose.Invoke();
    }

    public int GetEvolutionPoint()
    {
        return Mathf.CeilToInt(evolutionPoint);
    }

    public void UpgradePoint(int value)
    {
        evolutionPoint += value;
    }

    private void Update()
    {
        if (!gameIsPlaying && _die.color.a <= 0)
            if (Input.GetMouseButtonDown(0) || Input.touchCount > 0) StartGame();

        if (!gameIsPlaying) return;
        EvolutionControl();
    }

    private void EvolutionControl()
    {
        evolutionPoint += InstantiateLevel.Instance.moveSpeed * Time.deltaTime;
        time += Time.deltaTime;

        _time.text = "Evolution: " + Mathf.CeilToInt(evolutionPoint) + " ";
    }




    private bool useNewHistory = false;

    private IEnumerator NewHistoryActive()
    {
        useNewHistory = true;
        newHistoryEvent.color = new Color(newHistoryEvent.color.r, newHistoryEvent.color.g, newHistoryEvent.color.b, 1);

        yield return new WaitForSeconds(0.5f);
        newHistoryEvent.color = new Color(newHistoryEvent.color.r, newHistoryEvent.color.g, newHistoryEvent.color.b, 0);

        yield return new WaitForSeconds(0.5f);
        newHistoryEvent.color = new Color(newHistoryEvent.color.r, newHistoryEvent.color.g, newHistoryEvent.color.b, 1);

        yield return new WaitForSeconds(0.5f);
        newHistoryEvent.color = new Color(newHistoryEvent.color.r, newHistoryEvent.color.g, newHistoryEvent.color.b, 0);

        yield return new WaitForSeconds(0.5f);
        newHistoryEvent.color = new Color(newHistoryEvent.color.r, newHistoryEvent.color.g, newHistoryEvent.color.b, 1);

        yield return new WaitForSeconds(2);
        newHistoryEvent.color = new Color(newHistoryEvent.color.r, newHistoryEvent.color.g, newHistoryEvent.color.b, 0);

    }
}
