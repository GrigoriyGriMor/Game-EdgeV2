using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChostFogController1 : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] GameObject hand;
    [SerializeField] private float chostFogSpeed = 1.0f;
    [SerializeField] private float handAttackSpeed = 5.0f;
    private float _handAttackSpeed;

    [SerializeField] private float chostBasePos = -4.5f;
    [SerializeField] private float chostNewPos;
    [SerializeField] private float handNewPos;

    [SerializeField] private AudioClip[] fogLaughter = new AudioClip[5];

    [SerializeField] private AudioClip fogSound;

    [HideInInspector] public float attackTime = 5.0f; //делаем публичной, что бы когда добавим уровни сложности, можно было легко менять частоту атак

    private float timeFirstAttack = 0.0f;

    [SerializeField] private float levelUpTime = 120.0f;
    private float levelPlusDistance = 0;

    [SerializeField] private Animator EyeAnim; 

    private void Start()
    {
        if (SoundManager.Instance) SoundManager.Instance.ClipLoopAndPlay(fogSound);
        timeFirstAttack = 0.0f;
        _handAttackSpeed = handAttackSpeed;
        handNewPos = gameObject.transform.position.x;

        StartCoroutine(Timer());
    }

    private void FixedUpdate()
    {
        if (!player.root)
        {
            if (player.stopGame) return;

            //=======================================================
            if (player._speedPlayer < 0.9f)
                chostNewPos = player.gameObject.transform.position.x + 2;
            else
                if (player._speedPlayer > 1.0f)
                chostNewPos = chostBasePos - 7.0f;
            else
                    if (player._speedPlayer == 1.0f)
                chostNewPos = chostBasePos;

            //=======================================================
            if (Mathf.FloorToInt(timeFirstAttack) == attackTime)//счетчик до первой атаки (один раз запускает курутину ChostFogAttack)
            {
                StartCoroutine(ChostFogAttack());
                if (SoundManager.Instance) SoundManager.Instance.ClipPlay(fogLaughter[Random.Range(0, fogLaughter.Length)]);
                StartCoroutine(UseEyeLight());
                timeFirstAttack += 1;
            }
            else
                if (Mathf.FloorToInt(timeFirstAttack) <= attackTime)
                timeFirstAttack += Time.deltaTime;
            //=======================================================

        }
        else
        {
            chostNewPos = player.gameObject.transform.position.x + 2;
            StopAllCoroutines();
        }
            

        MoveForward();
    }

    private IEnumerator Timer()
    {
        float time = 0;
        while (time < levelUpTime)
        {
            time += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        chostBasePos += 0.5f;
        if (chostFogSpeed > 0.001f) chostFogSpeed *= 0.95f;
        attackTime *= 0.9f;

        if (player.transform.position.x > chostBasePos || player.transform.position.x - chostBasePos > 0.5f)
            StartCoroutine(Timer());
    }

    private void MoveForward()
    {
        gameObject.transform.position = Vector2.Lerp(gameObject.transform.position, new Vector2(chostNewPos, gameObject.transform.position.y), chostFogSpeed);
        hand.transform.position = Vector2.Lerp(hand.transform.position, new Vector2(handNewPos, hand.transform.position.y), _handAttackSpeed);
    }

    private IEnumerator ChostFogAttack()
    {
        yield return new WaitForSeconds(1);

        hand.transform.position = new Vector2(hand.transform.position.x, player.transform.position.y);
        handNewPos = player.transform.position.x + 4.0f;

        while (hand.transform.position.x < player.transform.position.x && player._speedPlayer <= 1)
        {
            yield return new WaitForEndOfFrame();
        }

        float targetTime = 0;
        while (hand.transform.position.x < player.transform.position.x - targetTime && !player.root)//сработает если предыдущий while выйдет из цикла раньше, чем рука достигнет персонажа
        {
            if (player._speedPlayer > 1)
                _handAttackSpeed = handAttackSpeed / 2;
            else
                _handAttackSpeed = handAttackSpeed;


            targetTime += 0.005f;
            yield return new WaitForEndOfFrame();
        }

        _handAttackSpeed = handAttackSpeed;
        handNewPos = -12;

        yield return new WaitForSeconds(attackTime);

        if (SoundManager.Instance) SoundManager.Instance.ClipPlay(fogLaughter[Random.Range(0, fogLaughter.Length)]);
        StartCoroutine(UseEyeLight());

        StartCoroutine(ChostFogAttack());
    }

    [SerializeField] private float timerEyeLight = 1f;
    private IEnumerator UseEyeLight()
    {
        yield return new WaitForSeconds(timerEyeLight);
        EyeAnim.SetTrigger("Active");
    }
}
