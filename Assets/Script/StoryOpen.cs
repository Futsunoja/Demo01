using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoryOpen : MonoBehaviour
{
    public GameObject Player, King, Knight, Knight2;
    public GameObject[] Dialogue;
    public GameObject BackGround;
    public GameObject[] Dialogue2;
    public Transform PlayerTra, KingTra, KnightTra, Knight2Tra;
    public Transform PlayerTalkTra, KingTalkTra, KnightTalkTra, Knight2TalkTra, CentralTra;
    Vector3 StandChange = new Vector3(60, 30, 0);

    bool canInput;
    public static bool runDialogue;
    int dialogue;

    public AudioSource aud;
    public AudioClip BGM, Hit, Skip;

    [SerializeField]
    PlayerData data;

    private void Start()
    {
        #region 能力初始化
        PlayerPrefs.DeleteAll();   //刪除舊有資料

        data.unitName = "勇者";
        data.unitLevel = 20;
        data.atk = 100;
        data.def = 80;
        data.maxHp = 240;
        data.currentHp = 240;
        data.maxSkillPower = 5;
        data.currySkillPower = 5;
        data.maxExp = new int[] { 0, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80, 85, 90, 95, 100, 105, 110, 115, 120 };
        data.curryExp = 0;
        data.ItemMax = 15;
        data.RedPoison = 5;
        data.BluePoison = 5;
        data.BuffPoison = 3;
        data.UndebuffPoison = 2;
        data.speed = 42;
        data.mapNumber = 0;
        data.Story = 0;
        data.ThievesDenOpen = false;
        data.OnTheMountainOpen = false;
        data.GoddessStatueOpen = false;

        PlayerPrefs.SetString("Playerdata", JsonUtility.ToJson(data));    //存取新資料
        #endregion

        canInput = false;
        runDialogue = true;
        dialogue = -1;
        King.transform.localPosition = new Vector3(-330, 0, 0);
        Player.transform.localPosition = new Vector3(530, 0, 0);
        Knight.transform.localPosition = new Vector3(300, 0, 0);
        Knight2.transform.localPosition = new Vector3(300, 0, 0);
        for (int i = 0; i <= Dialogue.Length-1; i++)
        {
            Dialogue[i].SetActive(false);
        }
        BackGround.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f);
        StartCoroutine(Open());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && canInput == true && runDialogue==false)
        {
            aud.PlayOneShot(Hit);
            canInput = false;
            runDialogue = true;
            dialogue++;
        }

        if (dialogue == 0 && canInput == false && runDialogue == true)
        {
            King.transform.localPosition = Vector3.MoveTowards(King.transform.localPosition, Vector3.zero, 600 * Time.deltaTime);
            if (King.transform.localPosition == Vector3.zero)
            {
                canInput = true;
                GameObject talk = Instantiate(Dialogue2[0], KingTalkTra);
            }
        }

        if (dialogue == 1 && canInput == false && runDialogue == true)
        {
            Knight.transform.localPosition = Vector3.MoveTowards(Knight.transform.localPosition, Vector3.zero, 600*Time.deltaTime);
            if (Knight.transform.localPosition == Vector3.zero)
            {
                canInput = true;
                GameObject talk = Instantiate(Dialogue2[1], KnightTalkTra);
            }
        }

        if (dialogue == 2 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[2], KingTalkTra);
        }

        if (dialogue == 3 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[3], KnightTalkTra);
        }

        if (dialogue == 4 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[4], KingTalkTra);
        }

        if (dialogue == 5 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[5], KnightTalkTra);
        }

        if (dialogue == 6 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[6], CentralTra);
        }

        if (dialogue == 7 && canInput == false && runDialogue == true)
        {
            Knight.transform.localPosition = StandChange;
            Knight.GetComponent<Image>().color = Color.gray;
            Knight2.transform.localPosition = Vector3.MoveTowards(Knight2.transform.localPosition, Vector3.zero, 1200 * Time.deltaTime);
            if (Knight2.transform.localPosition == Vector3.zero)
            {
                canInput = true;
                GameObject talk = Instantiate(Dialogue2[7], Knight2TalkTra);
            }
        }

        if (dialogue == 8 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[8], KingTalkTra);
        }

        if (dialogue == 9 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[9], Knight2TalkTra);
        }

        if (dialogue == 10 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[10], KingTalkTra);
        }

        if (dialogue == 11 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[11], Knight2TalkTra);
        }

        if (dialogue == 12 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[12], Knight2TalkTra);
        }

        if (dialogue == 13 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[13], Knight2TalkTra);
        }

        if (dialogue == 14 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[14], Knight2TalkTra);
        }

        if (dialogue == 15 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[15], Knight2TalkTra);
        }

        if (dialogue == 16 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[16], Knight2TalkTra);
        }

        if (dialogue == 17 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[17], Knight2TalkTra);
        }

        if (dialogue == 18 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[18], KingTalkTra);
        }

        if (dialogue == 19 && canInput == false && runDialogue == true)
        {
            KnightTra.transform.SetAsLastSibling();
            Knight.transform.localPosition = Vector3.zero;
            Knight.GetComponent<Image>().color = Color.white;
            Knight2.transform.localPosition = StandChange;
            Knight2.GetComponent<Image>().color = Color.gray;
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[19], KnightTalkTra);
        }

        if (dialogue == 20 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[20], KnightTalkTra);
        }

        if (dialogue == 21 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[21], KnightTalkTra);
        }
        if (dialogue == 22 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[22], KingTalkTra);
        }

        if (dialogue == 23 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[23], KnightTalkTra);
        }

        if (dialogue == 24 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[24], KingTalkTra);
        }

        if (dialogue == 25 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[25], KnightTalkTra);
        }

        if (dialogue == 26 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[26], KnightTalkTra);
        }

        if (dialogue == 27 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[27], KingTalkTra);
        }

        if (dialogue == 28 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[28], KnightTalkTra);
        }

        if (dialogue == 29 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[29], KingTalkTra);
        }

        if (dialogue == 30 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[30], KingTalkTra);
        }

        if (dialogue == 31 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[31], KnightTalkTra);
        }

        if (dialogue == 32 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[32], KingTalkTra);
        }

        if (dialogue == 33 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[33], KnightTalkTra);
        }

        if (dialogue == 34 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[34], KingTalkTra);
        }

        if (dialogue == 35 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[35], KnightTalkTra);
        }

        if (dialogue == 36 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[36], KingTalkTra);
        }

        if (dialogue == 37 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[37], KingTalkTra);
        }

        if (dialogue == 38 && canInput == false && runDialogue == true)
        {
            CentralTra.transform.SetAsLastSibling();
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[38], CentralTra);
        }

        if (dialogue == 39 && canInput == false && runDialogue == true)
        {
            Knight.transform.localPosition = StandChange;
            Knight.GetComponent<Image>().color = Color.gray;
            PlayerTra.transform.SetAsLastSibling();
            Player.transform.localPosition = Vector3.MoveTowards(Player.transform.localPosition, Vector3.zero, 1200 * Time.deltaTime);
            if (Player.transform.localPosition == Vector3.zero)
            {
                canInput = true;
                GameObject talk = Instantiate(Dialogue2[39], PlayerTalkTra);
            }
        }

        if (dialogue == 40 && canInput == false && runDialogue == true)
        {
            KingTra.transform.SetAsLastSibling();
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[40], KingTalkTra);
        }

        if (dialogue == 41 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[41], PlayerTalkTra);
        }

        if (dialogue == 42 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[42], KingTalkTra);
        }

        if (dialogue == 43 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[43], PlayerTalkTra);
        }

        if (dialogue == 44 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[44], KingTalkTra);
        }

        if (dialogue == 45 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[45], PlayerTalkTra);
        }

        if (dialogue == 46 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[46], KingTalkTra);
        }

        if (dialogue == 47 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[47], PlayerTalkTra);
        }

        if (dialogue == 48 && canInput == false && runDialogue == true)
        {
            Player.transform.localScale = new Vector3(-0.45f, 0.45f, 1f);
            Player.transform.localPosition = Vector3.MoveTowards(Player.transform.localPosition, new Vector3(530, 0, 0), 1200 * Time.deltaTime);
            Knight.transform.localPosition = Vector3.zero;
            Knight.GetComponent<Image>().color = Color.white;
            if(Player.transform.localPosition==new Vector3(530, 0, 0))
            {
                canInput = true;
                GameObject talk = Instantiate(Dialogue2[48], CentralTra);
            }
        }

        if (dialogue == 49 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[49], KingTalkTra);
        }

        if (dialogue == 50 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[50], KnightTalkTra);
        }

        if (dialogue == 51 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[51], KingTalkTra);
        }

        if (dialogue == 52 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[52], CentralTra);
        }

        if (dialogue == 53 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[53], KingTalkTra);
        }

        if (dialogue == 54 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[54], KnightTalkTra);
        }

        if (dialogue == 55 && canInput == false && runDialogue == true)
        {
            King.transform.localScale = new Vector3(-0.45f, 0.45f, 1);
            Knight.transform.localScale = new Vector3(-0.5f, 0.5f, 1);
            King.transform.localPosition = Vector3.MoveTowards(King.transform.localPosition, new Vector3(-330, 0, 0), 600 * Time.deltaTime);
            Knight.transform.localPosition = Vector3.MoveTowards(Knight.transform.localPosition, new Vector3(330, 0, 0), 600 * Time.deltaTime);
            Knight2.transform.localPosition = Vector3.zero;
            Knight2.GetComponent<Image>().color = Color.white;
            if (King.transform.localPosition == new Vector3(-330, 0, 0) && Knight.transform.localPosition == new Vector3(330, 0, 0))
            {
                GameObject talk = Instantiate(Dialogue2[55], Knight2TalkTra);
                canInput = true;
            }
        }

        if (dialogue == 56 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[56], Knight2TalkTra);
        }

        if (dialogue == 57 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject talk = Instantiate(Dialogue2[57], Knight2TalkTra);
        }

        if (dialogue == 58 && canInput == false && runDialogue == true)
        {
            Knight2.transform.localScale = new Vector3(-0.5f, 0.5f, 1f);
            Knight2.transform.localPosition = Vector3.MoveTowards(Knight2.transform.localPosition, new Vector3(330, 0, 0), 600 * Time.deltaTime);
            if (Knight2.transform.localPosition == new Vector3(330, 0, 0))
            {
                canInput = true;
                StartCoroutine(Close());
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))    //跳過開頭劇情
        {
            aud.PlayOneShot(Skip);
            Invoke("DelayStartMap", 1f);
        }
    }

    IEnumerator Open()
    {
        for(int i = 0; i <= Dialogue.Length - 1; i++)
        {
            yield return new WaitForSeconds(1f);
            Dialogue[i].SetActive(true);
        }

        yield return new WaitForSeconds(1.5f);

        for (int i = 0; i <= Dialogue.Length - 1; i++)
        {
            Dialogue[i].SetActive(false);
        }

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i <= 50; i++)
        {
            yield return new WaitForSeconds(0.03f);
            BackGround.GetComponent<SpriteRenderer>().color += new Color(0.02f, 0.02f, 0.02f);
            if (BackGround.GetComponent<SpriteRenderer>().color == new Color(1f, 1f, 1f))
            {
                break;
            }
        }

        yield return new WaitForSeconds(0.5f);
        aud.clip = BGM;
        aud.Play();

        yield return new WaitForSeconds(0.5f);
        dialogue++;
    }

    IEnumerator Close()
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i <= 50; i++)
        {
            yield return new WaitForSeconds(0.03f);
            BackGround.GetComponent<SpriteRenderer>().color -= new Color(0.02f, 0.02f, 0.02f);
            if (BackGround.GetComponent<SpriteRenderer>().color == new Color(0f, 0f, 0f))
            {
                BackGround.GetComponent<SpriteRenderer>().color = Color.black;
                break;
            }
        }

        if (BackGround.GetComponent<SpriteRenderer>().color == Color.black)
        {
            Invoke("DelayStartMap", 1f);
        }
    }

    public void DelayStartMap()
    {
        SceneManager.LoadScene("大地圖");
    }

    [System.Serializable]
    public class PlayerData
    {
        public string unitName;
        public int unitLevel;
        public int atk;
        public int def;
        public float maxHp;
        public float currentHp;
        public int maxSkillPower;
        public int currySkillPower;
        public int[] maxExp;
        public int curryExp;
        public int ItemMax;
        public int RedPoison;
        public int BluePoison;
        public int BuffPoison;
        public int UndebuffPoison;
        public int speed;
        public int mapNumber;
        public int Story;
        public bool ThievesDenOpen;
        public bool OnTheMountainOpen;
        public bool GoddessStatueOpen;
    }
}
