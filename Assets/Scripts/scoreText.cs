using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class scoreText : MonoBehaviour
{
    public TextMeshProUGUI textScore;

    private void Start()
    {
        if (passParameter.isNewHighScore)
        {
            this.textScore.text = "New High Score";
        }
        else
        {
            this.textScore.text = "Your Score";
        }

    }
}
