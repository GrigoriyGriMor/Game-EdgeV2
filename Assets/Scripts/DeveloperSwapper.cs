using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeveloperSwapper : MonoBehaviour
{

    [SerializeField] private GameObject[] imagePool = new GameObject[10];
    private List<GameObject> _image = new List<GameObject>();
    [SerializeField] private GameObject presentsText;

    private void Awake()
    {
        StartCoroutine(StartPresentationTime(4));
        InitImage();
    }

    private void InitImage()
    {
        for (int i = 0; i < imagePool.Length; i++)
        {
            GameObject go = Instantiate(imagePool[i], new Vector2(1, 1), Quaternion.identity);
            _image.Add(go);
            go.SetActive(false);
        }
    }

    private IEnumerator WaitPresentationTime(int time)
    {
        yield return new WaitForSeconds(time);
        nomberImage += 1;
        NextImage(Random.Range(1, 4));
    }

    private IEnumerator StartPresentationTime(int time)
    {

        yield return new WaitForSeconds(time);
        nomberImage = 0;
        NextImage(Random.Range(1, 4));
        presentsText.SetActive(false);
    }

    private int nomberImage = 0;
    [SerializeField] private float moveValue = 0.1f;
    private Vector2 camBorder = new Vector2(9, 5);
    private Vector2 lastPosition;

    private void NextImage(int value)
    {
        switch (value)
        {
            case 1://up
                _image[nomberImage].SetActive(true);
                _image[nomberImage].transform.position = new Vector2(0, camBorder.y + _image[nomberImage].GetComponent<SpriteRenderer>().size.y / 2);
                lastPosition = _image[nomberImage].transform.position;
                break;
            case 2://down
                _image[nomberImage].SetActive(true);
                _image[nomberImage].transform.position = new Vector2(0, -camBorder.y - _image[nomberImage].GetComponent<SpriteRenderer>().size.y / 2);
                lastPosition = _image[nomberImage].transform.position;
                break;
            case 3://left
                _image[nomberImage].SetActive(true);
                _image[nomberImage].transform.position = new Vector2(-camBorder.x - _image[nomberImage].GetComponent<SpriteRenderer>().size.x / 2, 0);
                lastPosition = _image[nomberImage].transform.position;
                break;
            case 4://right
                _image[nomberImage].SetActive(true);
                _image[nomberImage].transform.position = new Vector2(camBorder.x + _image[nomberImage].GetComponent<SpriteRenderer>().size.x / 2, 0);
                lastPosition = _image[nomberImage].transform.position;
                break;
        }

        if (nomberImage < _image.Count - 1) StartCoroutine(WaitPresentationTime(3));
        else StartCoroutine(StartPresentationTime(3));
    }

    private void FixedUpdate()
    {
        if (nomberImage != 0)
        {
            _image[nomberImage].transform.position = Vector2.Lerp(new Vector2(_image[nomberImage].transform.position.x, _image[nomberImage].transform.position.y), new Vector2(0, 0), moveValue);
            _image[nomberImage - 1].transform.position = Vector2.Lerp(_image[nomberImage - 1].transform.position, lastPosition * (-1), moveValue);
        }
        else
        {
            _image[nomberImage].transform.position = Vector2.Lerp(_image[nomberImage].transform.position, new Vector2(0, 0), moveValue);
            _image[_image.Count - 1].transform.position = Vector2.Lerp(_image[_image.Count - 1].transform.position, lastPosition * (-1), moveValue);
            Debug.Log(_image[_image.Count - 1]);
        }
    }
}
