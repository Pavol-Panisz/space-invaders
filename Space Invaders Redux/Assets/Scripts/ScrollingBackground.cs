using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    Material material;
    Vector2 offset;
    [SerializeField] [Range(0f, 1f)] float scrollSpeed = 0.5f;
    void Start()
    {
        material = gameObject.GetComponent<MeshRenderer>().material;
        offset = new Vector2(0f, scrollSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        material.mainTextureOffset += offset * Time.deltaTime;
    }
}
