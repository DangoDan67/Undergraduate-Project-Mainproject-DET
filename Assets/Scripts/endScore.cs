using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class endScore : MonoBehaviour
{
    public Text scoreText;

    private void Start()
    {
        this.scoreText.text = passParameter.score.ToString();

    }

}
