using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public Transform pig;
    Vector3 T = new Vector3(-2, -0.8f, 0);
    Vector3 C = new Vector3(-2, -2, 0);
    Vector3 Q = new Vector3(-2, -3.2f, 0);
    bool i;
    public AudioSource aud;
    public AudioClip MenuBGM, Hit;

    public GameObject[] Talk;

    private void Start()
    {
        aud.clip = MenuBGM;
        aud.Play();
        Screen.SetResolution(1280, 720, false);
        pig.position = T;
        for (int k = 0; k <= 3; k++)
        {
            Talk[k].GetComponent<RectTransform>().localScale = Vector3.zero;
        }
        
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
        int j;
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (i == false)
            {
            aud.PlayOneShot(Hit);
            }
            
            i = true;
            if (pig.position == T)
            {
                j = Random.Range(0, 4);
                StartCoroutine(MenuTalk(j));
            }
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

    IEnumerator MenuTalk(int t)
    {
        while (Talk[t].GetComponent<RectTransform>().localScale != Vector3.one)
        {
            Talk[t].GetComponent<RectTransform>().localScale += new Vector3(0.2f, 0.2f, 0.2f);
            yield return new WaitForSeconds(0.033f);
        }
        i = false;
        yield return new WaitForSeconds(2f);
        Talk[t].GetComponent<RectTransform>().localScale = Vector3.zero;
    }

    public void DelayStartGame()
    {
        i = false;
        SceneManager.LoadScene("劇情_開頭");
    }

    public void DelayQuitGame()
    {
        i = false;
        Application.Quit();
    }
}
