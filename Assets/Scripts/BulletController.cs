using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {
    public float speed;

    private Vector3 currentVelocity;

    private void Start()
    {
        currentVelocity = new Vector3(0, speed, 0);
    }

    // Update is called once per frame
    private void Update ()
    {
        transform.position += currentVelocity * Time.deltaTime;
        //destroy this if it's out of bounds
        if (Camera.main.WorldToViewportPoint(transform.position).y > 1 || Camera.main.WorldToViewportPoint(transform.position).y < 0)
        {
            Destroy(this.gameObject);
        }
    }
}
