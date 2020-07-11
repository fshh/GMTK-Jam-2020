using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadingPip : MonoBehaviour
{
    public float normalized;

    public float speed = 2;

    public float viewSize;

    Vector3 initSize;
    Vector3 smallSize;

    public int order;
    float length = 10;

    public Color smallColor;
    public Color bigColor;

    SpriteRenderer sprite;

    public float radius = 2;
    // Start is called before the first frame update
    void Start()
    {
        initSize = transform.localScale;
        smallSize = initSize * 0.2f;
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float location = (Time.time * speed) % length;
        float dist = Mathf.Min(Mathf.Abs(location - order), Mathf.Abs(location - order + length), Mathf.Abs(location - order - length));
        normalized = dist / viewSize;

        transform.localScale = Vector3.Lerp(smallSize, initSize, normalized);

        sprite.color = Color.Lerp(smallColor, bigColor, normalized);

        float angleOffset = (order * 2 * Mathf.PI) / length;
        float angle = angleOffset + Time.time;
        Vector3 radialPosition = new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle), 0);
        Vector3 linearPosition = new Vector3(order - 5, 0, 0);

        transform.localPosition = Vector3.Lerp(radialPosition, linearPosition, (Mathf.Cos(Time.time / 3) + 1) / 2);
    }
}
