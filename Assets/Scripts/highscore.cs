using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class highscore : MonoBehaviour
{
    public Text bestscore;
    void Start()
    {
        bestscore.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
    }

}
