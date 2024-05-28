using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab;

    public uint enemiesPerColumn;
    public uint enemiesPerRow;

    public ScriptableInt numEnemies;
    public ScriptableBool playerAlive;

    public Text gameOverText;

    public Vector2 enemyOffset;

    [Tooltip("Offset from the edge of the screen in viewport coordinates (between 0-1)")]
    public float cutOffSize;
    public float speed;

    public float dropDistance, dropTime;

    private Vector3 direction;
    private List<GameObject> columnGroups;
    private Vector3 spawnPos;
    private bool gameOver;

    private Coroutine isMovingDown;
    
	private void Start ()
    {
        spawnPos = transform.position;
        direction = Vector3.right;
        columnGroups = new List<GameObject>();
        isMovingDown = null;
        gameOver = false;
        numEnemies.currentValue = (int)enemiesPerColumn * (int)enemiesPerRow;
        transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.04f, 0.9f, 0));
		for (int i=0; i<enemiesPerRow; ++i)
        {
            columnGroups.Add(new GameObject("group" + i));
            columnGroups[i].transform.SetParent(transform);
            columnGroups[i].transform.localPosition = Vector3.zero;

            columnGroups[i].transform.position += Vector3.right * i * enemyOffset.x;
            for (int n=0; n<enemiesPerColumn; ++n)
            {
                spawnPos.y = columnGroups[i].transform.position.y - n * enemyOffset.y;
                spawnPos.x = columnGroups[i].transform.position.x;
                Instantiate(enemyPrefab, spawnPos, transform.rotation, columnGroups[i].transform);
            }
        }
	}

    
    private void FixedUpdate()
    {
        Vector3 position;
        if (!gameOver)
        {
            if (isMovingDown == null)
            {
                for (int i = 0; i < columnGroups.Count; ++i)
                {
                    position = columnGroups[i].transform.position;
                    position.x += speed * Time.deltaTime;
                    columnGroups[i].transform.position = position;
                }

                if (columnGroups.Count > 0)
                {
                    if (Camera.main.WorldToViewportPoint(columnGroups[columnGroups.Count - 1].transform.position).x > 1 - cutOffSize)
                    {
                        speed *= -1;
                        isMovingDown = StartCoroutine(MoveDown());
                    }
                    else if (Camera.main.WorldToViewportPoint(columnGroups[0].transform.position).x < 0 + cutOffSize)
                    {
                        speed *= -1;
                        isMovingDown = StartCoroutine(MoveDown());
                    }
                }
            }
            if (numEnemies.currentValue == 0 || playerAlive.currentValue == false)
            {
                gameOver = true;
                StartCoroutine(GameOver());
            }
        }        
    }

    private IEnumerator GameOver()
    {
        if (numEnemies.currentValue == 0)
        {
            gameOverText.text = "YOU WIN!!!";
        }
        else
        {
            gameOverText.text = "GAME OVER!!!";
        }
        gameOverText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(0);
    }

    private IEnumerator MoveDown()
    {
        float startPosY = transform.position.y;
        float endPosY = transform.position.y - dropDistance;
        float percentageComplete = 0;
        float rate = 1.0f / dropTime;
        Vector3 newPos;
        do
        {
            percentageComplete += Time.deltaTime * rate;
            newPos.y = Mathf.Lerp(startPosY, endPosY, percentageComplete);
            newPos.x = transform.position.x;
            newPos.z = transform.position.z;
            transform.position = newPos;
            yield return null;
        } while (percentageComplete < 1.0f);

        isMovingDown = null;
    }

    public void RemoveObjectGroup(GameObject columnGroup)
    {
        columnGroups.Remove(columnGroup);
        Destroy(columnGroup);
    }

}
