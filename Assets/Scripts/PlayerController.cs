using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [Tooltip("Players movement speed")]
    public float speed;
    [Tooltip("Reference to the prefab to represent the bullet")]
    public GameObject playerBullet;
    [Tooltip("Time to wait between firing the bullet in seconds")]
    public float bulletWaitTime;

    public ScriptableBool playerAlive;

    private Animator animator;
    private Vector3 currentVelocity;
    private AudioSource sound;
    private Coroutine fireCoroutine = null;
    private bool isAlive;
    private Vector3 viewPortVec;

    private void Start()
    {
        isAlive = true;
        sound = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update ()
    {
        if (isAlive)
        {
            viewPortVec = Camera.main.WorldToViewportPoint(transform.position);
            transform.position += currentVelocity * Time.deltaTime;

            if (Input.GetKey(KeyCode.A) && viewPortVec.x > 0.02f)
            {
                currentVelocity.x = -speed;
                
            }
            else if (Input.GetKey(KeyCode.D) && viewPortVec.x < 0.98f)
            {
                currentVelocity.x = speed;
            }
            else
            {
                currentVelocity.x = 0;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (fireCoroutine == null)
                {
                    fireCoroutine = StartCoroutine(Fire());
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyBullet" || collision.tag == "Enemy")
        {
            isAlive = false;
            sound.Play();
            playerAlive.currentValue = false;
            animator.SetTrigger("Die");
        }
    }

    private IEnumerator Fire()
    {
        Instantiate(playerBullet, transform.position, transform.rotation);
        yield return new WaitForSeconds(bulletWaitTime);
        fireCoroutine = null;
    }
}
