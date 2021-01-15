using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferMap : MonoBehaviour
{
    public string transferMapName; // 이동할 맵의 이름.
    public Transform target;
    public int targetBGM;
    public BoxCollider2D targetBound;

    PlayerManager thePlayer;
    CameraManager theCamera;
    BGMManager BGM;
    FadeManager theFade;


    private void Start()
    {
        thePlayer = FindObjectOfType<PlayerManager>();
        theCamera = FindObjectOfType<CameraManager>();
        BGM = FindObjectOfType<BGMManager>();
        theFade = FindObjectOfType<FadeManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            StartCoroutine(TransferCoroutine());
        }
    }

    IEnumerator TransferCoroutine()
    {
        theFade.FadeOut();
        thePlayer.canWalk = false;
        thePlayer.StopMove();
        yield return new WaitForSeconds(1f);

        thePlayer.currentMapName = transferMapName;

        theCamera.SetBound(targetBound);

        theCamera.transform.position = new Vector3(target.position.x,
        target.position.y,
        theCamera.transform.position.z);
        thePlayer.transform.position = target.transform.position;

        StartCoroutine(FadeMusic());
        theFade.FadeIn();
        yield return new WaitForSeconds(0.8f);
        thePlayer.canWalk = true;
    }

    IEnumerator FadeMusic()
    {
        BGM.FadeOutMusic();
        yield return new WaitForSeconds(1f);
        BGM.Play(targetBGM);
        BGM.FadeInMusic();
    }

}
