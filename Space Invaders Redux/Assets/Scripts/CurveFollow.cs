using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveFollow : MonoBehaviour
{

    public GameObject target;
    [SerializeField] private float downSpeed = 1f;
    [SerializeField] private float followSpeed = 1.5f;
    private Transform targetTransform;
    public GameObject waypoint;
    // Start is called before the first frame update
    void Start()
    {
        targetTransform = target.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Instantiate(waypoint, transform.position, Quaternion.identity);
    }

    private void FixedUpdate()
    {
        gameObject.GetComponent<Rigidbody2D>().MovePosition(new Vector3(0f, -downSpeed * Time.fixedDeltaTime, 0f));
        Vector3 newPos;
        newPos = Vector3.MoveTowards(transform.position, targetTransform.position, followSpeed * Time.fixedDeltaTime);

        gameObject.GetComponent<Rigidbody2D>().MovePosition(newPos);
    }
}
