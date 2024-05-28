using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public ScriptableInt score;
    Text scoreText;
	// Use this for initialization
	void Start ()
    {
        score.currentValue = 0;
        scoreText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        scoreText.text = score.currentValue.ToString();
	}
}
