using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject CameraControlGO;
    public float transformSpeed = 5.0f;

    [SerializeField]
    private float _cameraSize = 6.0f;

    private void Awake()
    {
        CameraControlGO = GameObject.Find("Camera");
        CameraControlGO.GetComponent<Camera>().orthographicSize = _cameraSize;//сделал, что бы размер камеры не менялся. (Размер 16:10)
    }


    public void FixedUpdate()
    {
        //////////////////////////////////////////////----------Реализация камеры и слежки за персонажем-------------///////////////////////////////////////////////////////
        //Перемещаем камеру по горизонтали, чуть медленее персонажа, для улучшения эфекта восприятия-----------------------

        Vector2 gm = Vector2.Lerp(CameraControlGO.transform.position,
            gameObject.transform.position, transformSpeed * Time.deltaTime);

        CameraControlGO.transform.position = new Vector3(gm.x, CameraControlGO.transform.position.y, -10.0f);
        //-------------------------------------------------------------------------------------------------------------------------------------------------
    }
}

 