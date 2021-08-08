using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.IO;

public class MapMove : MonoBehaviour
{
    public GameObject[] map;
    public GameObject IntroField;
    public GameObject[] pla;
    public GameObject[] tintro;
    public GameObject place, intro;
    public GameObject MapMenu, Girl, talk;
    public GameObject[] MapMenuOption;
    public int MapMenuOptionNumber;
    public GameObject Save1, Save2, Load1, Load2, Load3;
    public bool SaveRun, LoadRun;
    public GameObject[] cannotuse;
    public GameObject[] direction;
    public GameObject maphaveeventAni;
    public GameObject CallLittleGirl;
    public GameObject DragonWhatToFight;
    public Transform canvas;

    public GameObject Strengthen;
    bool OpenStrengthen;
    public GameObject StrengthenChoose;
    public GameObject[] Equipment;
    public GameObject[] EquipmentShow;

    public GameObject Backpack;
    bool OpenBackpack;
    public GameObject BackpackChoose;
    public int BackpackChooseNum;
    public GameObject[] ItemField;
    public int ItemFieldNum;
    public Sprite[] ItemFieldSprite;
    //public GameObject[] ItemBar;
    public GameObject[] ItemBarSprite;
    public ItemManager ItemManager;
    public bool[] ItemFieldOpen = new bool[4];
    public Text itemPageShow;
    int[] itemPage = new int[2];
    int[] itemPageBreakpoint = new int[10];    //輔助紀錄分段編號
    public int[] ItemPotionNumber = new int[150];
    public int[] ItemMaterialNumber = new int[150];
    public int[] ItemTaskNumber = new int[150];
    public GameObject ItemIntroShow;
    public Text[] ItemIntro;

    public GameObject Meet;
    public GameObject FirstPlayer, FirstGirl;
    public Transform PlayerTra, GirlTra;
    public Transform PlayerTalkTra, GirlTalkTra;
    public GameObject[] Dialogue;
    public int dialogue;
    public bool canInput;
    public static bool runDialogue;

    float i = 4;    //調整移動速度
    public int[] j;
    bool l, m;

    public AudioSource audMap;
    public AudioClip MapBGM, Hit, OpenMenu;

    bool girlani, step1, step2, step3;

    [SerializeField]
    PlayerData data;

    private void Start()
    {
        data = JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString("Playerdata"));    //讀取存檔資訊

        audMap.clip = MapBGM;    //設定BGM
        audMap.Play();
        audMap.volume = 0.2f;
        Screen.SetResolution(1280, 720, false);

        for (int i = 0; i < 18; i++)
        {
            ItemBarSprite[i].SetActive(true);
            ItemBarSprite[i].GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }
        ItemIntroShow.SetActive(false);

        #region 讀取玩家存檔，建立初始畫面
        transform.position = map[data.mapNumber].transform.position;                           //初始畫面，玩家位置
        place.GetComponent<Text>().text = map[data.mapNumber].name;                            //初始畫面，地圖名稱
        intro.GetComponent<Text>().text = tintro[data.mapNumber].GetComponent<Text>().text;    //初始畫面，地圖介紹
        pla[0].SetActive(true);                                                                //初始畫面，指標朝左
        pla[1].SetActive(false);
        m = true;                                                                              //初始畫面，小女孩介面打開
        MapMenuOptionNumber = 0;
        MapMenuOption[MapMenuOptionNumber].GetComponent<Image>().color = Color.green;          //初始預設選擇前進
        CallLittleGirl.SetActive(false);                                                       //呼叫小女孩介面關閉
        #endregion

        #region 小女孩介面初始化
        girlani = true;    
        step1 = true;
        Girl.transform.position = new Vector3(-130, 371, 0);
        talk.transform.localScale = Vector3.zero;
        for (int i = 0; i < 9; i++)
        {
            MapMenuOption[i].transform.localScale = Vector3.zero;
        }
        Save1.SetActive(false);
        Save2.SetActive(false);
        Load1.SetActive(false);
        Load2.SetActive(false);
        Load3.SetActive(false);
        Strengthen.SetActive(false);
        Backpack.SetActive(false);
        for(int show = 0; show <= 4; show++)
        {
            EquipmentShow[show].SetActive(false);
        }

        for (int can = 0; can <= 3; can++)
            cannotuse[can].GetComponent<Image>().fillAmount = 0;
        for(int dir=0; dir <= 3; dir++)
            direction[dir].SetActive(false);
        #endregion

        #region 初遇小女孩
        Meet.SetActive(false);
        FirstPlayer.transform.localPosition = new Vector3(470, 0, 0);
        FirstGirl.transform.localPosition = new Vector3(-440, 0, 0);
        canInput = false;
        runDialogue = true;
        dialogue = 0;
        #endregion

        MapHaveEvent(14);    //地圖編號14 (巨龍山洞) 有事件
    }

    private void Update()
    {
        if (data.Story == 0)
        {
            IntroField.SetActive(false);
            Meet.SetActive(true);
            MeetLittleGirl();
        }

        if (data.Story != 0)
        {
            mapCtrl();
            menuAni();
            mapMenu();
            StrengthenCtrl();
            BackpackCtrl();
        }
    }


    #region 座標
    private void m0()
    {
        float step = i * Time.deltaTime;
        if (transform.position == map[0].transform.position)
        {
            data.mapNumber = 0;
            Direction("A");
            place.GetComponent<Text>().text = map[0].name;
            intro.GetComponent<Text>().text = tintro[0].GetComponent<Text>().text;
            if (Input.GetKeyDown(KeyCode.A))
            {
                j[0] = 1;
            }
        }
        if (j[0] == 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, map[1].transform.position, step);
            if (transform.position == map[1].transform.position)
            {
                j[0] = 0;
            }
        }
    }

    private void m1()
    {
        float step = i * Time.deltaTime;
        if (transform.position == map[1].transform.position)
        {
            data.mapNumber = 1;
            Direction("WASD");
            place.GetComponent<Text>().text = map[1].name;
            intro.GetComponent<Text>().text = tintro[1].GetComponent<Text>().text;
            if (Input.GetKeyDown(KeyCode.D))
            {
                j[1] = 1;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                j[1] = 2;
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                j[1] = 3;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                j[1] = 4;
            }
        }

        if (j[1] == 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, map[0].transform.position, step);
            if (transform.position == map[0].transform.position)
            {
                j[1] = 0;
            }
        }
        if (j[1] == 2)
        {
            transform.position = Vector3.MoveTowards(transform.position, map[2].transform.position, step);
            if (transform.position == map[2].transform.position)
            {
                j[1] = 0;
            }
        }
        if (j[1] == 3)
        {
            transform.position = Vector3.MoveTowards(transform.position, map[15].transform.position, step);
            if (transform.position == map[15].transform.position)
            {
                j[1] = 5;
            }
        }
        if (j[1] == 5)
        {
            transform.position = Vector3.MoveTowards(transform.position, map[16].transform.position, step);
            if (transform.position == map[16].transform.position)
            {
                j[1] = 6;
            }
        }
        if (j[1] == 6)
        {
            transform.position = Vector3.MoveTowards(transform.position, map[3].transform.position, step);
            if (transform.position == map[3].transform.position)
            {
                j[1] = 0;
            }
        }
        if (j[1] == 4)
        {
            transform.position = Vector3.MoveTowards(transform.position, map[4].transform.position, step);
            if (transform.position == map[4].transform.position)
            {
                j[1] = 0;
            }
        }
    }

    private void m2()
    {
        float speed = i * Time.deltaTime;
        if (transform.position == map[2].transform.position)
        {
            data.mapNumber = 2;
            Direction("W");
            place.GetComponent<Text>().text = map[2].name;
            intro.GetComponent<Text>().text = tintro[2].GetComponent<Text>().text;
            if (Input.GetKeyDown(KeyCode.W))
            {
                j[2] = 1;
            }
        }
        if (j[2] == 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, map[1].transform.position, speed);
            if (transform.position == map[1].transform.position)
            {
                j[2] = 0;
            }
        }
    }

    private void m3()
    {
        float step = i * Time.deltaTime;
        if (transform.position == map[3].transform.position)
        {
            data.mapNumber = 3;
            Direction("S");
            place.GetComponent<Text>().text = map[3].name;
            intro.GetComponent<Text>().text = tintro[3].GetComponent<Text>().text;
            if (Input.GetKeyDown(KeyCode.S))
            {
                j[3] = 1;
            }
        }
        if (j[3] == 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, map[16].transform.position, step);
            if (transform.position == map[16].transform.position)
            {
                j[3] = 2;
            }
        }
        if (j[3] == 2)
        {
            transform.position = Vector3.MoveTowards(transform.position, map[15].transform.position, step);
            if (transform.position == map[15].transform.position)
            {
                j[3] = 3;
            }
        }
        if (j[3] == 3)
        {
            transform.position = Vector3.MoveTowards(transform.position, map[1].transform.position, step);
            if (transform.position == map[1].transform.position)
            {
                j[3] = 0;
            }
        }
    }

    private void m4()
    {
        float step = i * Time.deltaTime;
        if (transform.position == map[4].transform.position)
        {
            data.mapNumber = 4;
            Direction("WAD");
            place.GetComponent<Text>().text = map[4].name;
            intro.GetComponent<Text>().text = tintro[4].GetComponent<Text>().text;
            if (Input.GetKeyDown(KeyCode.D))
            {
                j[4] = 1;
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                j[4] = 2;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                j[4] = 3;
            }
        }
        if (j[4] == 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, map[1].transform.position, step);
            if (transform.position == map[1].transform.position)
            {
                j[4] = 0;
            }
        }
        if (j[4] == 2)
        {
            transform.position = Vector3.MoveTowards(transform.position, map[5].transform.position, step);
            if (transform.position == map[5].transform.position)
            {
                j[4] = 0;
            }
        }
        if (j[4] == 3)
        {
            transform.position = Vector3.MoveTowards(transform.position, map[7].transform.position, step);
            if (transform.position == map[7].transform.position)
            {
                j[4] = 0;
            }
        }
    }

    private void m5()
    {
        float step = i * Time.deltaTime;
        if (transform.position == map[5].transform.position)
        {
            data.mapNumber = 5;
            Direction("WS");
            place.GetComponent<Text>().text = map[5].name;
            intro.GetComponent<Text>().text = tintro[5].GetComponent<Text>().text;
            if (Input.GetKeyDown(KeyCode.S))
            {
                j[5] = 1;
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                j[5] = 2;
            }
        }
        if (j[5] == 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, map[4].transform.position, step);
            if (transform.position == map[4].transform.position)
            {
                j[5] = 0;
            }
        }
        if (j[5] == 2)
        {
            transform.position = Vector3.MoveTowards(transform.position, map[6].transform.position, step);
            if (transform.position == map[6].transform.position)
            {
                j[5] = 0;
            }
        }
    }

    private void m6()
    {
        float step = i * Time.deltaTime;
        if (transform.position == map[6].transform.position)
        {
            data.mapNumber = 6;
            Direction("S");
            place.GetComponent<Text>().text = map[6].name;
            intro.GetComponent<Text>().text = tintro[6].GetComponent<Text>().text;
            if (Input.GetKeyDown(KeyCode.S))
            {
                j[6] = 1;
            }
        }
        if (j[6] == 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, map[5].transform.position, step);
            if (transform.position == map[5].transform.position)
            {
                j[6] = 0;
            }
        }
    }

    private void m7()
    {
        float step = i * Time.deltaTime;
        if (transform.position == map[7].transform.position)
        {
            data.mapNumber = 7;
            Direction("ASD");
            place.GetComponent<Text>().text = map[7].name;
            intro.GetComponent<Text>().text = tintro[7].GetComponent<Text>().text;
            if (Input.GetKeyDown(KeyCode.D))
            {
                j[7] = 1;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                j[7] = 2;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                j[7] = 3;
            }
        }
        if (j[7] == 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, map[4].transform.position, step);
            if (transform.position == map[4].transform.position)
            {
                j[7] = 0;
            }
        }
        if (j[7] == 2)
        {
            transform.position = Vector3.MoveTowards(transform.position, map[8].transform.position, step);
            if (transform.position == map[8].transform.position)
            {
                j[7] = 0;
            }
        }
        if (j[7] == 3)
        {
            transform.position = Vector3.MoveTowards(transform.position, map[10].transform.position, step);
            if (transform.position == map[10].transform.position)
            {
                j[7] = 0;
            }
        }
    }

    private void m8()
    {
        float step = i * Time.deltaTime;
        if (transform.position == map[8].transform.position)
        {
            data.mapNumber = 8;
            Direction("AD");
            place.GetComponent<Text>().text = map[8].name;
            intro.GetComponent<Text>().text = tintro[8].GetComponent<Text>().text;
            if (Input.GetKeyDown(KeyCode.D))
            {
                j[8] = 1;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                j[8] = 2;
            }
        }
        if (j[8] == 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, map[7].transform.position, step);
            if (transform.position == map[7].transform.position)
            {
                j[8] = 0;
            }
        }
        if (j[8] == 2)
        {
            transform.position = Vector3.MoveTowards(transform.position, map[9].transform.position, step);
            if (transform.position == map[9].transform.position)
            {
                j[8] = 0;
            }
        }
    }

    private void m9()
    {
        float step = i * Time.deltaTime;
        if (transform.position == map[9].transform.position)
        {
            data.mapNumber = 9;
            Direction("D");
            place.GetComponent<Text>().text = map[9].name;
            intro.GetComponent<Text>().text = tintro[9].GetComponent<Text>().text;
            if (Input.GetKeyDown(KeyCode.D))
            {
                j[9] = 1;
            }
        }
        if (j[9] == 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, map[8].transform.position, step);
            if (transform.position == map[8].transform.position)
            {
                j[9] = 0;
            }
        }
    }

    private void m10()
    {
        float step = i * Time.deltaTime;
        if (transform.position == map[10].transform.position)
        {
            data.mapNumber = 10;
            Direction("WAD");
            place.GetComponent<Text>().text = map[10].name;
            intro.GetComponent<Text>().text = tintro[10].GetComponent<Text>().text;
            if (Input.GetKeyDown(KeyCode.W))
            {
                j[10] = 1;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                j[10] = 2;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                j[10] = 3;
            }
        }
        if (j[10] == 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, map[7].transform.position, step);
            if (transform.position == map[7].transform.position)
            {
                j[10] = 0;
            }
        }
        if (j[10] == 2)
        {
            transform.position = Vector3.MoveTowards(transform.position, map[11].transform.position, step);
            if(transform.position== map[11].transform.position)
            {
                j[10] = 0;
            }
        }
        if (j[10] == 3)
        {
            transform.position = Vector3.MoveTowards(transform.position, map[13].transform.position, step);
            if (transform.position == map[13].transform.position)
            {
                j[10] = 0;
            }
        }
    }

    private void m11()
    {
        float step = i * Time.deltaTime;
        if (transform.position == map[11].transform.position)
        {
            data.mapNumber = 11;
            Direction("AD");
            place.GetComponent<Text>().text = map[11].name;
            intro.GetComponent<Text>().text = tintro[11].GetComponent<Text>().text;
            if (Input.GetKeyDown(KeyCode.A))
            {
                j[11] = 1;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                j[11] = 2;
            }
        }
        if (j[11] == 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, map[10].transform.position, step);
            if (transform.position == map[10].transform.position)
            {
                j[11] = 0;
            }
        }
        if (j[11] == 2)
        {
            transform.position = Vector3.MoveTowards(transform.position, map[17].transform.position, step);
            if (transform.position == map[17].transform.position)
            {
                j[11] = 3;
            }
        }
        if (j[11] == 3)
        {
            transform.position = Vector3.MoveTowards(transform.position, map[12].transform.position, step);
            if (transform.position == map[12].transform.position)
            {
                j[11] = 0;
            }
        }
    }

    private void m12()
    {
        float step = i * Time.deltaTime;
        if (transform.position == map[12].transform.position)
        {
            data.mapNumber = 12;
            Direction("A");
            place.GetComponent<Text>().text = map[12].name;
            intro.GetComponent<Text>().text = tintro[12].GetComponent<Text>().text;
            if (Input.GetKeyDown(KeyCode.A))
            {
                j[12] = 1;
            }
        }
        if (j[12] == 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, map[17].transform.position, step);
            if (transform.position == map[17].transform.position)
            {
                j[12] = 2;
            }
        }
        if (j[12] == 2)
        {
            transform.position = Vector3.MoveTowards(transform.position, map[11].transform.position, step);
            if (transform.position == map[11].transform.position)
            {
                j[12] = 0;
            }
        }
    }

    private void m13()
    {
        float step = i * Time.deltaTime;
        if (transform.position == map[13].transform.position)
        {
            data.mapNumber = 13;
            Direction("AD");
            place.GetComponent<Text>().text = map[13].name;
            intro.GetComponent<Text>().text = tintro[13].GetComponent<Text>().text;
            if (Input.GetKeyDown(KeyCode.D))
            {
                j[13] = 1;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                j[13] = 2;
            }
        }
        if (j[13] == 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, map[10].transform.position, step);
            if (transform.position == map[10].transform.position)
            {
                j[13] = 0;
            }
        }
        if (j[13] == 2)
        {
            transform.position = Vector3.MoveTowards(transform.position, map[14].transform.position, step);
            if (transform.position == map[14].transform.position)
            {
                j[13] = 0;
            }
        }
    }

    private void m14()
    {
        float step = i * Time.deltaTime;
        if (transform.position == map[14].transform.position)
        {
            data.mapNumber = 14;
            Direction("D");
            place.GetComponent<Text>().text = map[14].name;
            intro.GetComponent<Text>().text = tintro[14].GetComponent<Text>().text;
            if (Input.GetKeyDown(KeyCode.D))
            {
                j[14] = 1;
            }
        }
        if (j[14] == 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, map[13].transform.position, step);
            if (transform.position == map[13].transform.position)
            {
                j[14] = 0;
            }
        }
    }
    #endregion

    private void mapCtrl()    //小女孩介面關掉時
    {
        List<int> jList = j.ToList();      //將J陣列轉化為Lisk
        var k = jList.Where(x => x != 0);  //變數k = JLisk中符合條件的元素 (x => x 條件)
        if (k.ToList().Count > 0)          //將符合條件的元素組成新的List，count (元素數量)
        {
            place.GetComponent<Text>().text = "";
            intro.GetComponent<Text>().text = "";
            for (int dir = 0; dir <= 3; dir++)
                direction[dir].SetActive(false);
        }
        if (m == false && DragonWhatToFight.activeSelf == false)
        {
            if (k.ToList().Count == 0)
            {
                l = false;
            }
            else
            {
                l = true;
            }
            if (Input.GetKeyDown(KeyCode.A) && l == false)
            {
                pla[0].SetActive(true);
                pla[1].SetActive(false);
                l = true;
            }
            if (Input.GetKeyDown(KeyCode.D) && l == false)
            {
                pla[0].SetActive(false);
                pla[1].SetActive(true);
                l = true;
            }
            if (Input.GetKeyDown(KeyCode.Q) && l == false)    //打開小女孩介面
            {
                audMap.PlayOneShot(OpenMenu);
                CallLittleGirl.SetActive(false);    //呼叫小女孩介面關閉
                m = true;
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                PlayerPrefs.SetString("Playerdata", JsonUtility.ToJson(data));    //存取進入場景前的資料

                if (l == false && data.mapNumber != 14 && data.mapNumber != 6)
                {
                    audMap.PlayOneShot(Hit);
                    DragonWhatToFight.SetActive(true);
                    Invoke("CloseDragonWhatToFight", 2f);
                }
                if (data.mapNumber == 14 || data.mapNumber == 6)
                {
                    audMap.PlayOneShot(Hit);
                    SceneManager.LoadScene("戰鬥畫面");
                }
            }
            #region 座標操作
            m0();
            m1();
            m2();
            m3();
            m4();
            m5();
            m6();
            m7();
            m8();
            m9();
            m10();
            m11();
            m12();
            m13();
            m14();
            #endregion
        }
    }

    private void mapMenu()    //小女孩介面操作
    {
        if (m == true && girlani == false && SaveRun == false && LoadRun == false && OpenStrengthen == false && OpenBackpack == false)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                MapMenuOptionNumber += 3;
                if (MapMenuOptionNumber > 8)
                {
                    MapMenuOptionNumber -= 9;
                }
                for (int i = 0; i < 9; i++)
                {
                    MapMenuOption[i].GetComponent<Image>().color = Color.white;
                }
                MapMenuOption[MapMenuOptionNumber].GetComponent<Image>().color = Color.green;
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                MapMenuOptionNumber -= 3;
                if (MapMenuOptionNumber < 0)
                {
                    MapMenuOptionNumber += 9;
                }
                for (int i = 0; i < 9; i++)
                {
                    MapMenuOption[i].GetComponent<Image>().color = Color.white;
                }
                MapMenuOption[MapMenuOptionNumber].GetComponent<Image>().color = Color.green;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (MapMenuOptionNumber == 2)
                {
                    MapMenuOptionNumber = 0;
                }
                else if (MapMenuOptionNumber == 5)
                {
                    MapMenuOptionNumber = 3;
                }
                else if (MapMenuOptionNumber == 8)
                {
                    MapMenuOptionNumber = 6;
                }
                else
                {
                    MapMenuOptionNumber += 1;
                }
                for (int i = 0; i < 9; i++)
                {
                    MapMenuOption[i].GetComponent<Image>().color = Color.white;
                }
                MapMenuOption[MapMenuOptionNumber].GetComponent<Image>().color = Color.green;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (MapMenuOptionNumber == 0)
                {
                    MapMenuOptionNumber = 2;
                }
                else if (MapMenuOptionNumber == 3)
                {
                    MapMenuOptionNumber = 5;
                }
                else if (MapMenuOptionNumber == 6)
                {
                    MapMenuOptionNumber = 8;
                }
                else
                {
                    MapMenuOptionNumber -= 1;
                }
                for (int i = 0; i < 9; i++)
                {
                    MapMenuOption[i].GetComponent<Image>().color = Color.white;
                }
                MapMenuOption[MapMenuOptionNumber].GetComponent<Image>().color = Color.green;
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                if (MapMenuOption[0].GetComponent<Image>().color == Color.green)    //前進
                {
                    audMap.PlayOneShot(Hit);
                    m = false;
                }
                if (MapMenuOption[3].GetComponent<Image>().color == Color.green)    //背包
                {
                    OpenBackpack = true;
                    MapMenu.SetActive(false);
                    Backpack.SetActive(true);
                    ItemField[0].transform.SetAsLastSibling();
                    for (int i = 0; i < 4; i++)
                    {
                        ItemField[i].GetComponent<Image>().sprite = ItemFieldSprite[2 * i + 1];
                    }
                    ItemFieldNum = 0;
                    ItemField[0].GetComponent<Image>().sprite = ItemFieldSprite[0];
                    BackpackChooseNum = 0;
                    BackpackChoose.GetComponent<Transform>().SetParent(ItemBarSprite[0].GetComponent<Transform>());
                    BackpackChoose.transform.localPosition = Vector3.zero;
                    ItemFieldOpen[0] = true;
                    itemPage[0] = 1;
                    SetItemFieldArray();
                    ItemPageArrangement(0);
                }
                if (MapMenuOption[4].GetComponent<Image>().color == Color.green)    //強化裝備
                {
                    OpenStrengthen = true;
                    MapMenu.SetActive(false);
                    Strengthen.SetActive(true);
                    StrengthenChoose.transform.localPosition = Equipment[0].transform.localPosition;
                    EquipmentShow[0].SetActive(true);
                }
                if (MapMenuOption[2].GetComponent<Image>().color == Color.green)    //存檔
                {
                    audMap.PlayOneShot(Hit);
                    SaveRun = true;
                    StartCoroutine(SaveAni());
                }
                if (MapMenuOption[5].GetComponent<Image>().color == Color.green)    //讀檔
                {
                    audMap.PlayOneShot(Hit);
                    LoadRun = true;
                    StartCoroutine(LoadAni());
                }
                if (MapMenuOption[8].GetComponent<Image>().color == Color.green)    //主畫面
                {
                    audMap.PlayOneShot(Hit);
                    SceneManager.LoadScene("主畫面");
                }
            }
        }
        else if(m == false)
        {
            girlani = true;
            step1 = true;
            Girl.transform.position = new Vector3(-130, 371, 0);
            talk.transform.localScale = Vector3.zero;
            for (int i = 0; i < 9; i++)
            {
                MapMenuOption[i].transform.localScale = Vector3.zero;
            }
            for (int can = 0; can <= 3; can++)
            {
                cannotuse[can].GetComponent<Image>().fillAmount = 0;
            }  
            CallLittleGirl.SetActive(true);
            MapMenu.SetActive(false);
        }
    }

    private void menuAni()    //小女孩介面簡易動畫
    {
        if (m == true && girlani == true && step1 == true)    //小女孩進入
        {
            MapMenu.SetActive(true);

            Girl.transform.position = Vector3.MoveTowards(Girl.transform.position, new Vector3(110, 371, 0), 480 * Time.deltaTime);
            if(Girl.transform.position == new Vector3(110, 371, 0))
            {
                step1 = false;
                step2 = true;
            }
        }
        if (m == true && girlani == true && step2 == true)    //對話展開
        {
            talk.transform.localScale = Vector3.MoveTowards(talk.transform.localScale, Vector3.one, 6f * Time.deltaTime);
            if (talk.transform.localScale == Vector3.one)
            {
                step2 = false;
                step3 = true;
            }
        }
        if (m == true && girlani == true && step3 == true)    //選項展開
        {
            MapMenuOption[0].transform.localScale = Vector3.MoveTowards(MapMenuOption[0].transform.localScale, Vector3.one, 6f * Time.deltaTime);
            if (MapMenuOption[0].transform.localScale.x >= 0.2f)
            {
                MapMenuOption[1].transform.localScale = Vector3.MoveTowards(MapMenuOption[1].transform.localScale, Vector3.one, 6f * Time.deltaTime);
                if (MapMenuOption[1].transform.localScale.x >= 0.2f)
                {
                    MapMenuOption[2].transform.localScale = Vector3.MoveTowards(MapMenuOption[2].transform.localScale, Vector3.one, 6f * Time.deltaTime);
                    if (MapMenuOption[2].transform.localScale.x >= 0.2f)
                    {
                        MapMenuOption[3].transform.localScale = Vector3.MoveTowards(MapMenuOption[3].transform.localScale, Vector3.one, 6f * Time.deltaTime);
                        if (MapMenuOption[3].transform.localScale.x >= 0.2f)
                        {
                            MapMenuOption[4].transform.localScale = Vector3.MoveTowards(MapMenuOption[4].transform.localScale, Vector3.one, 6f * Time.deltaTime);
                            if (MapMenuOption[4].transform.localScale.x >= 0.2f)
                            {
                                MapMenuOption[5].transform.localScale = Vector3.MoveTowards(MapMenuOption[5].transform.localScale, Vector3.one, 6f * Time.deltaTime);
                                if (MapMenuOption[5].transform.localScale.x >= 0.2f)
                                {
                                    MapMenuOption[6].transform.localScale = Vector3.MoveTowards(MapMenuOption[6].transform.localScale, Vector3.one, 6f * Time.deltaTime);
                                    if (MapMenuOption[6].transform.localScale.x >= 0.2f)
                                    {
                                        MapMenuOption[7].transform.localScale = Vector3.MoveTowards(MapMenuOption[7].transform.localScale, Vector3.one, 6f * Time.deltaTime);
                                        if (MapMenuOption[7].transform.localScale.x >= 0.2f)
                                        {
                                            MapMenuOption[8].transform.localScale = Vector3.MoveTowards(MapMenuOption[8].transform.localScale, Vector3.one, 6f * Time.deltaTime);
                                            if (MapMenuOption[8].transform.localScale == Vector3.one)
                                            {
                                                for (int can = 0; can <= 3; can++)
                                                    cannotuse[can].GetComponent<Image>().fillAmount += 2 * Time.deltaTime;
                                                if (cannotuse[0].GetComponent<Image>().fillAmount == 1)
                                                {
                                                    step3 = false;
                                                    girlani = false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void Direction(string WASD)    //方向指標顯示隱藏
    {
        for (int dir = 0; dir <= 3; dir++)
            direction[dir].SetActive(false);
        if (WASD == "D")
            direction[0].SetActive(true);
        if (WASD == "A")
            direction[1].SetActive(true);
        if (WASD == "W")
            direction[2].SetActive(true);
        if (WASD == "S")
            direction[3].SetActive(true);
        if (WASD == "WA")
        {
            direction[2].SetActive(true);
            direction[1].SetActive(true);
        }
        if (WASD == "WS")
        {
            direction[2].SetActive(true);
            direction[3].SetActive(true);
        }
        if (WASD == "WD")
        {
            direction[2].SetActive(true);
            direction[0].SetActive(true);
        }
        if (WASD == "AS")
        {
            direction[1].SetActive(true);
            direction[3].SetActive(true);
        }
        if (WASD == "AD")
        {
            direction[1].SetActive(true);
            direction[0].SetActive(true);
        }
        if (WASD == "SD")
        {
            direction[3].SetActive(true);
            direction[0].SetActive(true);
        }
        if (WASD == "WAS")
        {
            direction[2].SetActive(true);
            direction[1].SetActive(true);
            direction[3].SetActive(true);
        }
        if (WASD == "WAD")
        {
            direction[2].SetActive(true);
            direction[1].SetActive(true);
            direction[0].SetActive(true);
        }
        if (WASD == "WSD")
        {
            direction[2].SetActive(true);
            direction[3].SetActive(true);
            direction[0].SetActive(true);
        }
        if (WASD == "ASD")
        {
            direction[1].SetActive(true);
            direction[3].SetActive(true);
            direction[0].SetActive(true);
        }
        if (WASD == "WASD")
        {
            for(int dir2 = 0; dir2 <= 3; dir2++)
                direction[dir2].SetActive(true);
        }
    }

    private void MapHaveEvent(int i)    //在編號i的地圖有事件
    {
        GameObject mapHaveEvent = Instantiate(maphaveeventAni, map[i].transform);
    }

    private void CloseDragonWhatToFight()
    {
        DragonWhatToFight.SetActive(false);
    }

    IEnumerator SaveAni()
    {
        talk.SetActive(false);
        Save1.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        PlayerPrefs.SetString("SaveData", JsonUtility.ToJson(data));
        Save1.SetActive(false);
        Save2.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        Save2.SetActive(false);
        talk.SetActive(true);
        SaveRun = false;
    }

    IEnumerator LoadAni()
    {
        if (PlayerPrefs.HasKey("SaveData") == true)
        {
            talk.SetActive(false);
            Load1.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            data = JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString("SaveData"));
            transform.position = map[data.mapNumber].transform.position;
            place.GetComponent<Text>().text = map[data.mapNumber].name;
            intro.GetComponent<Text>().text = tintro[data.mapNumber].GetComponent<Text>().text;
            Load1.SetActive(false);
            Load2.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            Load2.SetActive(false);
            talk.SetActive(true);
            LoadRun = false;
        }
        else
        {
            talk.SetActive(false);
            Load3.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            Load3.SetActive(false);
            talk.SetActive(true);
            LoadRun = false;
        }
    }

    private void StrengthenCtrl()
    {
        if(OpenStrengthen == true)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (StrengthenChoose.transform.localPosition == Equipment[0].transform.localPosition)
                {
                    StrengthenChoose.transform.localPosition = Equipment[1].transform.localPosition;
                    EquipmentShow[0].SetActive(false);
                    EquipmentShow[1].SetActive(true);
                }
                else if (StrengthenChoose.transform.localPosition == Equipment[1].transform.localPosition)
                {
                    StrengthenChoose.transform.localPosition = Equipment[2].transform.localPosition;
                    EquipmentShow[1].SetActive(false);
                    EquipmentShow[2].SetActive(true);
                }
                else if (StrengthenChoose.transform.localPosition == Equipment[2].transform.localPosition)
                {
                    StrengthenChoose.transform.localPosition = Equipment[3].transform.localPosition;
                    EquipmentShow[2].SetActive(false);
                    EquipmentShow[3].SetActive(true);
                }
                else if (StrengthenChoose.transform.localPosition == Equipment[3].transform.localPosition)
                {
                    StrengthenChoose.transform.localPosition = Equipment[4].transform.localPosition;
                    EquipmentShow[3].SetActive(false);
                    EquipmentShow[4].SetActive(true);
                }
                else if (StrengthenChoose.transform.localPosition == Equipment[4].transform.localPosition)
                {
                    StrengthenChoose.transform.localPosition = Equipment[0].transform.localPosition;
                    EquipmentShow[4].SetActive(false);
                    EquipmentShow[0].SetActive(true);
                }
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (StrengthenChoose.transform.localPosition == Equipment[0].transform.localPosition)
                {
                    StrengthenChoose.transform.localPosition = Equipment[4].transform.localPosition;
                    EquipmentShow[0].SetActive(false);
                    EquipmentShow[4].SetActive(true);
                }
                else if (StrengthenChoose.transform.localPosition == Equipment[1].transform.localPosition)
                {
                    StrengthenChoose.transform.localPosition = Equipment[0].transform.localPosition;
                    EquipmentShow[1].SetActive(false);
                    EquipmentShow[0].SetActive(true);
                }
                else if (StrengthenChoose.transform.localPosition == Equipment[2].transform.localPosition)
                {
                    StrengthenChoose.transform.localPosition = Equipment[1].transform.localPosition;
                    EquipmentShow[2].SetActive(false);
                    EquipmentShow[1].SetActive(true);
                }
                else if (StrengthenChoose.transform.localPosition == Equipment[3].transform.localPosition)
                {
                    StrengthenChoose.transform.localPosition = Equipment[2].transform.localPosition;
                    EquipmentShow[3].SetActive(false);
                    EquipmentShow[2].SetActive(true);
                }
                else if (StrengthenChoose.transform.localPosition == Equipment[4].transform.localPosition)
                {
                    StrengthenChoose.transform.localPosition = Equipment[3].transform.localPosition;
                    EquipmentShow[4].SetActive(false);
                    EquipmentShow[3].SetActive(true);
                }
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                audMap.PlayOneShot(Hit);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                audMap.PlayOneShot(OpenMenu);
                Strengthen.SetActive(false);
                MapMenu.SetActive(true);
                OpenStrengthen = false;
                for (int show = 0; show <= 4; show++)
                {
                    EquipmentShow[show].SetActive(false);
                }
            }
        }
    }

    private void BackpackCtrl()
    {
        if (OpenBackpack == true)
        {
            if (ItemFieldNum == 0)
            {
                List<int> BackpackItemList = data.ItemQuantity.ToList();
                var k = BackpackItemList.Where(x => x != 0);
                int ItemPage = Mathf.CeilToInt(k.ToList().Count / 18.0f);
                if (ItemPage == 0)
                {
                    ItemPage = 1;    //即使沒有道具也至少有一頁
                }
                itemPage[1] = ItemPage;    //欄位總頁數
            }
            else if (ItemFieldNum == 1)
            {
                List<int> BackpackItemList = ItemPotionNumber.ToList();
                var k = BackpackItemList.Where(x => x != 0);
                int ItemPage = Mathf.CeilToInt(k.ToList().Count / 18.0f);
                if (ItemPage == 0)
                {
                    ItemPage = 1;
                }
                itemPage[1] = ItemPage;
            }
            else if (ItemFieldNum == 2)
            {
                List<int> BackpackItemList = ItemMaterialNumber.ToList();
                var k = BackpackItemList.Where(x => x != 0);
                int ItemPage = Mathf.CeilToInt(k.ToList().Count / 18.0f);
                if (ItemPage == 0)
                {
                    ItemPage = 1;
                }
                itemPage[1] = ItemPage;
            }
            else if (ItemFieldNum == 3)
            {
                List<int> BackpackItemList = ItemTaskNumber.ToList();
                var k = BackpackItemList.Where(x => x != 0);
                int ItemPage = Mathf.CeilToInt(k.ToList().Count / 18.0f);
                if (ItemPage == 0)
                {
                    ItemPage = 1;
                }
                itemPage[1] = ItemPage;
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                itemPage[0] += 1;
                if (itemPage[0] > itemPage[1])
                {
                    itemPage[0] = 1;
                }
                for (int i = 0; i < 18; i++)
                {
                    ItemBarSprite[i].GetComponent<Image>().sprite = null;
                    ItemBarSprite[i].GetComponent<Image>().color = new Color(1, 1, 1, 0);
                }
                ItemPageArrangement(itemPageBreakpoint[itemPage[0] - 1]);
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                itemPage[0] -= 1;
                if (itemPage[0] == 0)
                {
                    itemPage[0] = itemPage[1];
                }
                for (int i = 0; i < 18; i++)
                {
                    ItemBarSprite[i].GetComponent<Image>().sprite = null;
                    ItemBarSprite[i].GetComponent<Image>().color = new Color(1, 1, 1, 0);
                }
                ItemPageArrangement(itemPageBreakpoint[itemPage[0] - 1]);
            }
            itemPageShow.text = itemPage[0] + "/" + itemPage[1];

            if (Input.GetKeyDown(KeyCode.S) && OpenBackpack == true)
            {
                BackpackChooseNum += 6;
                if (BackpackChooseNum >= 18)
                {
                    BackpackChooseNum -= 18;
                }
                //BackpackChoose.transform.localPosition = ItemBar[BackpackChooseNum].transform.localPosition;
                BackpackChoose.GetComponent<Transform>().SetParent(ItemBarSprite[BackpackChooseNum].GetComponent<Transform>());
                BackpackChoose.transform.localPosition = Vector3.zero;
            }
            if (Input.GetKeyDown(KeyCode.W) && OpenBackpack == true)
            {
                BackpackChooseNum -= 6;
                if (BackpackChooseNum <= -1)
                {
                    BackpackChooseNum += 18;
                }
                //BackpackChoose.transform.localPosition = ItemBar[BackpackChooseNum].transform.localPosition;
                BackpackChoose.GetComponent<Transform>().SetParent(ItemBarSprite[BackpackChooseNum].GetComponent<Transform>());
                BackpackChoose.transform.localPosition = Vector3.zero;
            }
            if (Input.GetKeyDown(KeyCode.D) && OpenBackpack == true)
            {
                if (BackpackChooseNum == 5 || BackpackChooseNum == 11 || BackpackChooseNum == 17)
                {
                    BackpackChooseNum -= 5;
                }
                else
                {
                    BackpackChooseNum += 1;
                }
                //BackpackChoose.transform.localPosition = ItemBar[BackpackChooseNum].transform.localPosition;
                BackpackChoose.GetComponent<Transform>().SetParent(ItemBarSprite[BackpackChooseNum].GetComponent<Transform>());
                BackpackChoose.transform.localPosition = Vector3.zero;
            }
            if (Input.GetKeyDown(KeyCode.A) && OpenBackpack == true)
            {
                if (BackpackChooseNum == 0 || BackpackChooseNum == 6 || BackpackChooseNum == 12)
                {
                    BackpackChooseNum += 5;
                }
                else
                {
                    BackpackChooseNum -= 1;
                }
                //BackpackChoose.transform.localPosition = ItemBar[BackpackChooseNum].transform.localPosition;
                BackpackChoose.GetComponent<Transform>().SetParent(ItemBarSprite[BackpackChooseNum].GetComponent<Transform>());
                BackpackChoose.transform.localPosition = Vector3.zero;
            }

            if (ItemBarSprite[BackpackChooseNum].GetComponent<Image>().sprite != null)
            {
                ItemManager.Item(ItemBarSprite[BackpackChooseNum].GetComponent<Image>().sprite.name);
                ItemIntroShow.SetActive(true);
                ItemIntroShow.GetComponent<Image>().sprite = ItemManager.ShowBackpackItem;
                ItemIntro[0].text = ItemBarSprite[BackpackChooseNum].GetComponent<Image>().sprite.name;
                ItemIntro[1].text = ItemManager.ItemType;
                ItemIntro[2].text = ItemManager.ItemIntro;
            }
            else if (ItemBarSprite[BackpackChooseNum].GetComponent<Image>().sprite == null)
            {
                ItemIntroShow.SetActive(false);
                ItemIntroShow.GetComponent<Image>().sprite = null;
                ItemIntro[0].text = null;
                ItemIntro[1].text = null;
                ItemIntro[2].text = null;
            }

            if (Input.GetKeyDown(KeyCode.E) && OpenBackpack == true)
            {
                ItemFieldNum += 1;
                if (ItemFieldNum > 3)
                {
                    ItemFieldNum = 0;
                }
                for (int i = 0; i < 4; i++)
                {
                    ItemField[i].GetComponent<Image>().sprite = ItemFieldSprite[2 * i + 1];
                }
                ItemField[ItemFieldNum].transform.SetAsLastSibling();
                ItemField[ItemFieldNum].GetComponent<Image>().sprite = ItemFieldSprite[2 * ItemFieldNum];

                for (int i = 0; i < 18; i++)
                {
                    ItemBarSprite[i].GetComponent<Image>().sprite = null;
                    ItemBarSprite[i].GetComponent<Image>().color = new Color(1, 1, 1, 0);
                }
                itemPage[0] = 1;
                ItemPageArrangement(0);
            }
            if (Input.GetKeyDown(KeyCode.Q) && OpenBackpack == true)
            {
                ItemFieldNum -= 1;
                if (ItemFieldNum < 0)
                {
                    ItemFieldNum = 3;
                }
                for (int i = 0; i < 4; i++)
                {
                    ItemField[i].GetComponent<Image>().sprite = ItemFieldSprite[2 * i + 1];
                }
                ItemField[ItemFieldNum].transform.SetAsLastSibling();
                ItemField[ItemFieldNum].GetComponent<Image>().sprite = ItemFieldSprite[2 * ItemFieldNum];

                for (int i = 0; i < 18; i++)
                {
                    ItemBarSprite[i].GetComponent<Image>().sprite = null;
                    ItemBarSprite[i].GetComponent<Image>().color = new Color(1, 1, 1, 0);
                }
                itemPage[0] = 1;
                ItemPageArrangement(0);
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                audMap.PlayOneShot(OpenMenu);
                Backpack.SetActive(false);
                MapMenu.SetActive(true);
                OpenBackpack = false;
                for (int i = 0; i <= 17; i++)
                {
                    ItemBarSprite[i].GetComponent<Image>().sprite = null;
                    ItemBarSprite[i].GetComponent<Image>().color = new Color(1, 1, 1, 0);
                }
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                audMap.PlayOneShot(Hit);
            }
        }
    }

    private void SetItemFieldArray()    //設定道具陣列
    {
        for (int i = 0; i < 50; i++)
        {
            ItemPotionNumber[i] = data.ItemQuantity[i];
        }
        for (int i = 50; i < 120; i++)
        {
            ItemMaterialNumber[i] = data.ItemQuantity[i];
        }
        for (int i = 120; i < 150; i++)
        {
            ItemTaskNumber[i] = data.ItemQuantity[i];
        }
    }

    private void ItemPageArrangement(int Number)    //設定道具顯示
    {
        int[] itemNumber = new int[150];
        if (ItemFieldNum == 0)
        {
            itemNumber = data.ItemQuantity;
        }
        else if (ItemFieldNum == 1)
        {
            itemNumber = ItemPotionNumber;
        }
        else if (ItemFieldNum == 2)
        {
            itemNumber = ItemMaterialNumber;
        }
        else if (ItemFieldNum == 3)
        {
            itemNumber = ItemTaskNumber;
        }
        for (int i = Number; i < 150; i++)
        {
            for (int j = 0; j < 18; j++)
            {
                if (itemNumber[i] != 0 && ItemBarSprite[j].GetComponent<Image>().sprite == null)
                {
                    ItemBarSprite[j].SetActive(true);
                    ItemBarSprite[j].GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    ItemBarSprite[j].GetComponent<Image>().sprite = ItemManager.BackpackItemSprite[i];
                    if (ItemBarSprite[17].GetComponent<Image>().color == new Color(1, 1, 1, 1) && ItemBarSprite[17].GetComponent<Image>().sprite != null)
                    {
                        itemPageBreakpoint[itemPage[0]] = i + 1;
                    }
                    break;
                }
            }
        }
    }

    private void MeetLittleGirl()    //初遇小女孩
    {
        if (Input.GetKeyDown(KeyCode.J) && canInput == true && runDialogue == false)
        {
            audMap.PlayOneShot(Hit);
            canInput = false;
            runDialogue = true;
            dialogue++;
        }

        if (dialogue == 0 && canInput == false && runDialogue == true)
        {
            FirstPlayer.transform.localPosition = Vector3.MoveTowards(FirstPlayer.transform.localPosition, Vector3.zero, 480 * Time.deltaTime);
            if (FirstPlayer.transform.localPosition == Vector3.zero)
            {
                canInput = true;
                GameObject Talk = Instantiate(Dialogue[0], PlayerTalkTra);
            }
        }

        if (dialogue == 1 && canInput == false && runDialogue == true)
        {
            FirstGirl.transform.localPosition = Vector3.MoveTowards(FirstGirl.transform.localPosition, Vector3.zero, 480 * Time.deltaTime);
            if (FirstGirl.transform.localPosition == Vector3.zero)
            {
                canInput = true;
                GameObject Talk = Instantiate(Dialogue[1], GirlTalkTra);
            }
        }

        if (dialogue == 2 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject Talk = Instantiate(Dialogue[2], PlayerTalkTra);
        }

        if (dialogue == 3 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject Talk = Instantiate(Dialogue[3], GirlTalkTra);
        }

        if (dialogue == 4 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject Talk = Instantiate(Dialogue[4], GirlTalkTra);
        }

        if (dialogue == 5 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject Talk = Instantiate(Dialogue[5], PlayerTalkTra);
        }

        if (dialogue == 6 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject Talk = Instantiate(Dialogue[6], GirlTalkTra);
        }

        if (dialogue == 7 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject Talk = Instantiate(Dialogue[7], GirlTalkTra);
        }

        if (dialogue == 8 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject Talk = Instantiate(Dialogue[8], PlayerTalkTra);
        }

        if (dialogue == 9 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject Talk = Instantiate(Dialogue[9], GirlTalkTra);
        }

        if (dialogue == 10 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject Talk = Instantiate(Dialogue[10], GirlTalkTra);
        }

        if (dialogue == 11 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject Talk = Instantiate(Dialogue[11], GirlTalkTra);
        }

        if (dialogue == 12 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject Talk = Instantiate(Dialogue[12], GirlTalkTra);
        }

        if (dialogue == 13 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject Talk = Instantiate(Dialogue[13], GirlTalkTra);
        }

        if (dialogue == 14 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject Talk = Instantiate(Dialogue[14], PlayerTalkTra);
        }

        if (dialogue == 15 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject Talk = Instantiate(Dialogue[15], GirlTalkTra);
        }

        if (dialogue == 16 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject Talk = Instantiate(Dialogue[16], GirlTalkTra);
        }

        if (dialogue == 17 && canInput == false && runDialogue == true)
        {
            canInput = true;
            GameObject Talk = Instantiate(Dialogue[17], PlayerTalkTra);
        }

        if (dialogue == 18 && canInput == false && runDialogue == true)
        {
            Meet.SetActive(false);
            IntroField.SetActive(true);
            data.Story = 1;
            PlayerPrefs.SetString("Playerdata", JsonUtility.ToJson(data));
        }
    }

    [System.Serializable]
    public class PlayerData
    {
        public string unitName;
        public int unitLevel;
        public int atk;
        public int def;
        public int speed;
        public float maxHp;
        public float currentHp;
        public int maxSkillPower;
        public int currySkillPower;
        public int[] maxExp;
        public int curryExp;
        public int[] SkillNumber;
        public int ItemMax;
        public int[] ItemNumber;
        public int[] ItemQuantity;
        public int HeadwearLevel;
        public int SwordLevel;
        public int ShieldLevel;
        public int ClothesLevel;
        public int ShoesLevel;
        public int mapNumber;
        public int Story;
        public bool ThievesDenOpen;
        public bool OnTheMountainOpen;
        public bool GoddessStatueOpen;
    }
}
