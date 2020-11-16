using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool gameHasEnded = false;

    public void EndGame()
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            Debug.Log("Game Over");
            Debug.Log("Your Score :" + passParameter.score);
            Cursor.lockState = CursorLockMode.None;
            if (passParameter.score > PlayerPrefs.GetInt("HighScore", 0))
            {
                PlayerPrefs.SetInt("HighScore", passParameter.score);
                PlayerPrefs.Save();
                passParameter.isNewHighScore = true;
                SceneManager.LoadScene("GameOver");
            }
            else
            {
                passParameter.isNewHighScore = false;
                SceneManager.LoadScene("GameOver");
            }

        }
    }
}
