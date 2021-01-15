using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public float moveSpeed;
    public float destroyTime;

    Vector2 vector;
    RectTransform floatVector;

    private void Start()
    {
        floatVector = gameObject.GetComponent<RectTransform>();
    }

    private void Update()
    {
        vector.Set(floatVector.anchoredPosition.x,
            floatVector.anchoredPosition.y + (moveSpeed * Time.deltaTime));

        floatVector.anchoredPosition = vector;

        destroyTime -= Time.deltaTime;

        if (destroyTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
