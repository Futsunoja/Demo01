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
    public GameObject MapMenu, Girl, talk, go, tra, forg, sav, loa, backmenu;
    public GameObject Save1, Save2, Load1, Load2, Load3;
    public bool SaveRun, LoadRun;
    public GameObject[] cannotuse;
    public GameObject[] direction;
    public GameObject maphaveeventAni;
    public GameObject CallLittleGirl;
    public GameObject DragonWhatToFight;
    public Transform canvas;

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

        #region 讀取玩家存檔，建立初始畫面
        transform.position = map[data.mapNumber].transform.position;                           //初始畫面，玩家位置
        place.GetComponent<Text>().text = map[data.mapNumber].name;                            //初始畫面，地圖名稱
        intro.GetComponent<Text>().text = tintro[data.mapNumber].GetComponent<Text>().text;    //初始畫面，地圖介紹
        pla[0].SetActive(true);                                                                //初始畫面，指標朝左
        pla[1].SetActive(false);
        m = true;                                                                              //初始畫面，小女孩介面打開
        go.GetComponent<Image>().color = Color.green;                                          //初始預設選擇前進
        CallLittleGirl.SetActive(false);                                                       //呼叫小女孩介面關閉
        #endregion

        #region 小女孩介面初始化
        girlani = true;    
        step1 = true;
        Girl.transform.position = new Vector3(-130, 371, 0);
        talk.transform.localScale = Vector3.zero;
        go.transform.localScale = Vector3.zero;
        tra.transform.localScale = Vector3.zero;
        forg.transform.localScale = Vector3.zero;
        sav.transform.localScale = Vector3.zero;
        loa.transform.localScale = Vector3.zero;
        backmenu.transform.localScale = Vector3.zero;
        Save1.SetActive(false);
        Save2.SetActive(false);
        Load1.SetActive(false);
        Load2.SetActive(false);
        Load3.SetActive(false);
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

                if (l == false && transform.position != map[14].transform.position)
                {
                    audMap.PlayOneShot(Hit);
                    DragonWhatToFight.SetActive(true);
                    Invoke("CloseDragonWhatToFight", 2f);
                }
                if (transform.position == map[14].transform.position)
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
        if (m == true && girlani == false && SaveRun==false)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (go.GetComponent<Image>().color == Color.green)
                {
                    go.GetComponent<Image>().color = Color.white;
                    forg.GetComponent<Image>().color = Color.green;
                }
                else if (forg.GetComponent<Image>().color == Color.green)
                {
                    forg.GetComponent<Image>().color = Color.white;
                    tra.GetComponent<Image>().color = Color.green;
                }
                else if (tra.GetComponent<Image>().color == Color.green)
                {
                    tra.GetComponent<Image>().color = Color.white;
                    go.GetComponent<Image>().color = Color.green;
                }
                else if (sav.GetComponent<Image>().color == Color.green)
                {
                    sav.GetComponent<Image>().color = Color.white;
                    backmenu.GetComponent<Image>().color = Color.green;
                }
                else if (backmenu.GetComponent<Image>().color == Color.green)
                {
                    backmenu.GetComponent<Image>().color = Color.white;
                    loa.GetComponent<Image>().color = Color.green;
                }
                else if (loa.GetComponent<Image>().color == Color.green)
                {
                    loa.GetComponent<Image>().color = Color.white;
                    sav.GetComponent<Image>().color = Color.green;
                }
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (go.GetComponent<Image>().color == Color.green)
                {
                    go.GetComponent<Image>().color = Color.white;
                    tra.GetComponent<Image>().color = Color.green;
                }
                else if (forg.GetComponent<Image>().color == Color.green)
                {
                    forg.GetComponent<Image>().color = Color.white;
                    go.GetComponent<Image>().color = Color.green;
                }
                else if (tra.GetComponent<Image>().color == Color.green)
                {
                    tra.GetComponent<Image>().color = Color.white;
                    forg.GetComponent<Image>().color = Color.green;
                }
                else if (sav.GetComponent<Image>().color == Color.green)
                {
                    sav.GetComponent<Image>().color = Color.white;
                    loa.GetComponent<Image>().color = Color.green;
                }
                else if (backmenu.GetComponent<Image>().color == Color.green)
                {
                    backmenu.GetComponent<Image>().color = Color.white;
                    sav.GetComponent<Image>().color = Color.green;
                }
                else if (loa.GetComponent<Image>().color == Color.green)
                {
                    loa.GetComponent<Image>().color = Color.white;
                    backmenu.GetComponent<Image>().color = Color.green;
                }
            }
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A))
            {
                if (go.GetComponent<Image>().color == Color.green)
                {
                    go.GetComponent<Image>().color = Color.white;
                    sav.GetComponent<Image>().color = Color.green;
                }
                else if (forg.GetComponent<Image>().color == Color.green)
                {
                    forg.GetComponent<Image>().color = Color.white;
                    backmenu.GetComponent<Image>().color = Color.green;
                }
                else if (tra.GetComponent<Image>().color == Color.green)
                {
                    tra.GetComponent<Image>().color = Color.white;
                    loa.GetComponent<Image>().color = Color.green;
                }
                else if (sav.GetComponent<Image>().color == Color.green)
                {
                    sav.GetComponent<Image>().color = Color.white;
                    go.GetComponent<Image>().color = Color.green;
                }
                else if (backmenu.GetComponent<Image>().color == Color.green)
                {
                    backmenu.GetComponent<Image>().color = Color.white;
                    forg.GetComponent<Image>().color = Color.green;
                }
                else if (loa.GetComponent<Image>().color == Color.green)
                {
                    loa.GetComponent<Image>().color = Color.white;
                    tra.GetComponent<Image>().color = Color.green;
                }
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                if (go.GetComponent<Image>().color == Color.green)
                {
                    audMap.PlayOneShot(Hit);
                    m = false;
                }
                if (sav.GetComponent<Image>().color == Color.green)
                {
                    audMap.PlayOneShot(Hit);
                    SaveRun = true;
                    StartCoroutine(SaveAni());
                }
                if (loa.GetComponent<Image>().color == Color.green)
                {
                    audMap.PlayOneShot(Hit);
                    LoadRun = true;
                    StartCoroutine(LoadAni());
                }
                if (backmenu.GetComponent<Image>().color == Color.green)
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
            go.transform.localScale = Vector3.zero;
            tra.transform.localScale = Vector3.zero;
            forg.transform.localScale = Vector3.zero;
            sav.transform.localScale = Vector3.zero;
            loa.transform.localScale = Vector3.zero;
            backmenu.transform.localScale = Vector3.zero;
            for (int can = 0; can <= 3; can++)
                cannotuse[can].GetComponent<Image>().fillAmount = 0;
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
            go.transform.localScale = Vector3.MoveTowards(go.transform.localScale, Vector3.one, 6f * Time.deltaTime);
            if (go.transform.localScale.x >= 0.2f)
            {
                tra.transform.localScale = Vector3.MoveTowards(tra.transform.localScale, Vector3.one, 6f * Time.deltaTime);
                if (tra.transform.localScale.x >= 0.2f)
                {
                    forg.transform.localScale = Vector3.MoveTowards(forg.transform.localScale, Vector3.one, 6f * Time.deltaTime);
                    if (forg.transform.localScale.x >= 0.2f)
                    {
                        sav.transform.localScale = Vector3.MoveTowards(sav.transform.localScale, Vector3.one, 6f * Time.deltaTime);
                        if (sav.transform.localScale.x >= 0.2f)
                        {
                            loa.transform.localScale = Vector3.MoveTowards(loa.transform.localScale, Vector3.one, 6f * Time.deltaTime);
                            if (loa.transform.localScale.x >= 0.2f)
                            {
                                backmenu.transform.localScale = Vector3.MoveTowards(backmenu.transform.localScale, Vector3.one, 6f * Time.deltaTime);
                                if (backmenu.transform.localScale == Vector3.one)
                                {
                                    for (int can = 0; can <= 3; can++)
                                        cannotuse[can].GetComponent<Image>().fillAmount += 2 * Time.deltaTime;
                                    if(cannotuse[0].GetComponent<Image>().fillAmount == 1)
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
