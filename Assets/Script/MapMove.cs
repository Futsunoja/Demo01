using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections.Generic;

public class MapMove : MonoBehaviour
{
    public GameObject[] map;
    public GameObject[] pla;
    public GameObject[] tintro;
    public GameObject place, intro;
    public GameObject MapManu, Girl, go, tra, forg, sav, loa, backmenu;
    float i = 4;
    public int[] j;
    int loaMap;
    bool l, m;

    private void Start()
    {
        loaMap = 1;                                                                    //讀取玩家檔案的地圖編號
        transform.position = map[loaMap].transform.position;                           //初始畫面，玩家位置
        place.GetComponent<Text>().text = map[loaMap].name;                            //初始畫面，地圖名稱
        intro.GetComponent<Text>().text = tintro[loaMap].GetComponent<Text>().text;    //初始畫面，地圖介紹
        pla[0].SetActive(true);                                                        //初始畫面，指標朝左
        pla[1].SetActive(false);
        m = true;                                                                      //初始畫面，小女孩介面打開
        go.GetComponent<Image>().color = Color.green;
    }

    private void Update()
    {
        List<int> jList = j.ToList();      //將J陣列轉化為Lisk
        var k = jList.Where(x => x != 0);  //變數k = JLisk中符合條件的元素 (x => x 條件)
        if (k.ToList().Count > 0)          //將符合條件的元素組成新的List，count (元素數量)
        {
            place.GetComponent<Text>().text = "";
            intro.GetComponent<Text>().text = "";
        }

        mapManu();

        if (m == false)  //小女孩介面關掉時
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
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                m = true;
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                if (transform.position == map[14].transform.position)
                {
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


    #region 座標
    private void m0()
    {
        float step = i * Time.deltaTime;
        if (transform.position == map[0].transform.position)
        {
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

    private void mapManu()
    {
        if (m == true)
        {
            MapManu.SetActive(true);
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
                    m = false;
                }
                if (backmenu.GetComponent<Image>().color == Color.green)
                {
                    SceneManager.LoadScene("主畫面");
                }
            }
        }
        else if(m == false)
        {
            MapManu.SetActive(false);
        }
    }

    //   private void TEST()
    //   float step = 6 * Time.deltaTime;
    //   {
    //       if (transform.position == m11 || py != m14.y)
    //       {
    //           transform.position = Vector3.MoveTowards(transform.position, m14, step);
    //       }
    //   }
}
