using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int pointValue;
    public GameObject bulletPrefab;
    public ScriptableInt score;
    public ScriptableInt numEnemies;

    private bool isAlive;
    private Animator animator;
    private AudioSource sound;

    private void Start()
    {
        isAlive = true;
        animator = GetComponent<Animator>();
        sound = GetComponent<AudioSource>();
        StartCoroutine(Blink());
        StartCoroutine(Shoot());
    }

    private void OnDestroy()
    {
        if (transform.parent.childCount == 1)
        {
            EnemyManager manager = transform.parent.transform.parent.GetComponent<EnemyManager>();
            manager.RemoveObjectGroup(transform.parent.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerBullet" && isAlive)
        {
            Destroy(collision.gameObject);
            Kill();
        }
    }

    private void Kill()
    {
        isAlive = false;
        sound.Play();
        score.currentValue += pointValue;
        numEnemies.currentValue -= 1;
        animator.SetTrigger("Die");
    }

    private IEnumerator Shoot()
    {
        float value = Random.value * 20;
        yield return new WaitForSeconds(value);
        animator.SetTrigger("Shoot");
        yield return new WaitForSeconds(0.5f); //add a brief delay so the bullet better lines up with the animation
        Instantiate(bulletPrefab, transform.position, transform.rotation);
        StartCoroutine(Shoot());
    }

    private IEnumerator Blink()
    {
        float value = Random.value * 5;
        yield return new WaitForSeconds(value);
        animator.SetTrigger("Blink");
        StartCoroutine(Blink());
    }

}
