using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    public string startPoint; // 맵의 이동, 플레이어가 시작될 위치.

    PlayerManager thePlayer;
    CameraManager theCamera;

    private void Start()
    {
        theCamera = FindObjectOfType<CameraManager>();
        thePlayer = FindObjectOfType<PlayerManager>();

        if(startPoint == thePlayer.currentMapName)
        {
            theCamera.transform.position = new Vector3(transform.position.x,
                transform.position.y,
                theCamera.transform.position.z);
            thePlayer.transform.position = this.transform.position;
        }
    }
}
