using UnityEngine;

public class TalkOpenCetral : MonoBehaviour
{
    private void Start()
    {
        StoryOpen.runDialogue = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Destroy(gameObject);
        }
    }
}
