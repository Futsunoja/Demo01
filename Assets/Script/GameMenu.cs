using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public Transform pig;
    Vector3 T = new Vector3(-2, -0.8f, 0);
    Vector3 C = new Vector3(-2, -2, 0);
    Vector3 Q = new Vector3(-2, -3.2f, 0);
    Vector3 CHA = new Vector3(15, 15, 1);
    bool i;

    private void Start()
    {
        Screen.SetResolution(1280, 720, false);
        pig.position = T;
    }

    private void Update()
    {
        if (i == false)
        {
            MenuUD();
            MenuCh();
        }
    }

    private void MenuUD()
    {
        if (Input.GetKeyDown(KeyCode.S) && pig.position == T)
        {
            pig.position = C;
        }
        else if(Input.GetKeyDown(KeyCode.S) && pig.position == C)
        {
            pig.position = Q;
        }
        else if (Input.GetKeyDown(KeyCode.S) && pig.position == Q)
        {
            pig.position = T;
        }

        if (Input.GetKeyDown(KeyCode.W) && pig.position == T)
        {
            pig.position = Q;
        }
        else if (Input.GetKeyDown(KeyCode.W) && pig.position == C)
        {
            pig.position = T;
        }
        else if (Input.GetKeyDown(KeyCode.W) && pig.position == Q)
        {
            pig.position = C;
        }
    }

    private void MenuCh()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            i = true;
            if (pig.position == C)
            {
                Invoke("DelayStartGame", 1f);
            }
            if (pig.position == Q)
            {
                Invoke("DelayQuitGame", 1f);
            }
        }
    }

    public void DelayStartGame()
    {
        i = false;
        SceneManager.LoadScene("大地圖");
    }

    public void DelayQuitGame()
    {
        i = false;
        Application.Quit();
    }
}
