using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnSystem : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject img;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                Pause();
            }
        }
        if(Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.LoadScene(0);
        }
    }

    void Pause()
    {
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        img.SetActive(true);
    }

    void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        img.SetActive(false);
    }
}
