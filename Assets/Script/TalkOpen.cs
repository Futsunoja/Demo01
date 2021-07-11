using UnityEngine;

public class TalkOpen : MonoBehaviour
{
    bool i;
    private void Start()
    {
        i = false;
        transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one, 6 * Time.deltaTime);

        if (transform.localScale == Vector3.one && i == false)
        {
            StoryOpen.runDialogue = false;
            MapMove.runDialogue = false;
            i = true;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            if (transform.localScale != Vector3.one)
            {
                transform.localScale = Vector3.one;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
