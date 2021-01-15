using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    static public CameraManager instance;

    public GameObject target; // 카메라가 따라갈 대상.
    public float moveSpeed; // 카메라가 대상을 쫓는 속도.
    public BoxCollider2D bound;

    Vector3 minBound;
    Vector3 maxBound;

    float halfWidth;
    float halfHeight;


    Camera theCamera;

    Vector3 targetPosition; // 대상의 현재 위치.

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    }
    private void Start()
    {
        theCamera = GetComponent<Camera>();
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
        halfHeight = theCamera.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height;
    }

    public Vector3 GetVector3PlayerperCanvas()
    {
        Vector3 vector = target.transform.position - gameObject.transform.position;

        vector = new Vector3(vector.x * Screen.width / halfWidth / 2f,
            vector.y * Screen.height / halfHeight / 2f,
            0);

        return vector;
    }

    private void Update()
    {
        if(target.gameObject != null)
        {
            // target의 오브젝트로부터 x, y의 위치를 받음.
            targetPosition.Set(target.transform.position.x,
                target.transform.position.y,
                this.transform.position.z);

            // 1초에 moveSpeed 만큼 이동.
            this.transform.position = Vector3.Lerp(this.transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime);

            float clampedX = Mathf.Clamp(transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);
            float clampedY = Mathf.Clamp(transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);

            transform.position = new Vector3(clampedX, clampedY, transform.position.z);
        }
    }

    public void SetBound(BoxCollider2D newBound)
    {
        bound = newBound;
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
    }

}
