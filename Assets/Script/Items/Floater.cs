using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    public float amplitude = 0.15f;
    public float frequency = 0.7f;
    public float degreesPerSecond = 0f;

    float randStart = 0f;
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        randStart = Random.Range(0, 1000)/1000.0f;
        posOffset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector2(0f, Time.deltaTime * degreesPerSecond), Space.World);

        tempPos = posOffset;
        tempPos.y += Mathf.Sin((Time.fixedTime + randStart) * Mathf.PI * frequency) * amplitude;

        transform.position = tempPos;
    }
}
