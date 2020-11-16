using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class score : MonoBehaviour
{
    public Text scoreText;

    private void Start()
    {
        this.scoreText.text = "0";
    }

    public void addScore()
    {
        int newScore = int.Parse(this.scoreText.text) + 1;
        this.scoreText.text = newScore.ToString();
    }

    public int getIntScore()
    {
        return int.Parse(this.scoreText.text);
    }
}
