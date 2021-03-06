using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum Battletest { START, CHOOSEACTION,PLAYERTURN, ENEMYTURN, ENDTRUN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    [SerializeField]
    public PlayerData data;

    public AudioSource audBattle;
    public AudioClip BattleBGM, WonBGM, LoseBGM;

    public GameObject c1, c2, c3, c4;
    bool i0, i1, i2, i3, i4, i5, i6, i7, ispr1, ispr2, ispr3;
    Vector3 c = new Vector3(-2.7f, 0, 0);
    Vector3 o = new Vector3(0.4f, 0, 0);
    public Sprite spr11, spr12, spr13, spr14, spr21, spr22, spr23, spr24, spr31, spr32, spr33, spr34;

    public GameObject playerPrefab;
    public GameObject[] enemyPrefab;
    public GameObject PlayerHp, EnemyHp;
    public GameObject BattleMessage;
    public GameObject SkillItemName;
    public GameObject SkillItemEffect;
    public GameObject ItemQuantity;
    public GameObject[] SkillPower;
    public GameObject BattleSettlement, WonSettlement, LoseSettlement, ExeSettlement, ExeLine;
    public GameObject AbilitySettlement, OldLevel, OldAtk, OldDef, OldSpd, NewLevel, NewAtk, NewDef, NewSpd, NewLevelMask, NewAtkMask, NewDefMask, NewSpdMask, ReStart, LevelUp;
    public GameObject ReStartBattle, ReturnMap, ReturnMenu;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    SkillUnit skillUnit1, skillUnit2, skillUnit3, skillUnit4;
    public ItemManager itemManager;
    Unit enemyUnit;
    int EnemyNumber;
    int EnemyHit;

    bool DragonPower, DragonEndBreath;    //龍之力判定
    int DragonEnchantment;    //龍之結界判定
    int PlayerFireCount;    //玩家燃燒回合數
    bool PlayerAllDown;    //玩家能力值全下降

    public Text enemyName, enemyHp, playerHp;    //開始介面文字資訊
    public Text Won, getExe, Exe;    //勝利介面文字資訊

    int playerOriLevel;  //原等級
    int PlayerOriAtk;    //原攻擊力
    int PlayerOriDef;    //原防禦力
    int PlayerOriSpd;    //原速度
    int PlayerOriExe;    //原經驗值
    int SkillUPcount;    //強化倒數
    int SkillUPtime;     //強化次數
    int POISONcount;     //毒倒數
    int POISONdamage;    //毒傷害
    bool EnemyIsDead;    //是否死亡
    bool Wonclicktorestart;
    bool Losechoose;

    public Battletest state;

    void Start()
    {
        data = JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString("Playerdata"));
        data.currySkillPower = data.maxSkillPower;

        audBattle.clip = BattleBGM;
        audBattle.Play();
        audBattle.volume = 0.2f;
        Screen.SetResolution(1280, 720, false);    //固定視窗大小

        c1.transform.localPosition = c;
        c2.transform.localPosition = c;
        c3.transform.localPosition = c;
        c4.transform.localPosition = c;

        Instantiate(playerPrefab, playerBattleStation);

        MeetEnemy();
        GameObject enemyGo = Instantiate(enemyPrefab[EnemyNumber], enemyBattleStation);
        enemyUnit = enemyGo.GetComponent<Unit>();

        skillUnit1 = gameObject.GetComponent<SkillManager>().skillPrefab[data.SkillNumber[0]].GetComponent<SkillUnit>();
        skillUnit2 = gameObject.GetComponent<SkillManager>().skillPrefab[data.SkillNumber[1]].GetComponent<SkillUnit>();
        skillUnit3 = gameObject.GetComponent<SkillManager>().skillPrefab[data.SkillNumber[2]].GetComponent<SkillUnit>();
        skillUnit4 = gameObject.GetComponent<SkillManager>().skillPrefab[data.SkillNumber[3]].GetComponent<SkillUnit>();

        spr21 = skillUnit1.SkillSprite;
        spr22 = skillUnit2.SkillSprite;
        spr23 = skillUnit3.SkillSprite;
        spr24 = skillUnit4.SkillSprite;

        spr31 = itemManager.BattleItemSprite[data.ItemNumber[0]];
        spr32 = itemManager.BattleItemSprite[data.ItemNumber[1]];
        spr33 = itemManager.BattleItemSprite[data.ItemNumber[2]];
        spr34 = itemManager.BattleItemSprite[data.ItemNumber[3]];

        playerOriLevel = data.unitLevel;
        PlayerOriAtk = data.atk;
        PlayerOriDef = data.def;
        PlayerOriSpd = data.speed;
        PlayerOriExe = data.curryExp;
        SkillUPcount = 0;
        POISONcount = 0;

        state = Battletest.START;
        BattleMessage.GetComponent<Text>().text = "遭遇了" + enemyUnit.unitName;
        ReSet();
        SetupBattle();
    }

    void Update()
    {
        ChooseAction();
        WonClickToRestart();
        LoseChoose();
    }

    void SetupBattle()    //設定戰鬥必要數據
    {
        enemyName.text = enemyUnit.unitName;
        playerHp.text = data.currentHp.ToString() + "/" + data.maxHp.ToString();
        enemyHp.text = enemyUnit.currentHp.ToString() + "/" + enemyUnit.maxHp.ToString();

        PlayerHp.GetComponent<Image>().fillAmount = data.currentHp / data.maxHp;
        EnemyHp.GetComponent<Image>().fillAmount = enemyUnit.currentHp / enemyUnit.maxHp;

        for(int i = 0; i <= 4; i++)
        {
            SkillPower[i].SetActive(true);
        }

        BattleSettlement.SetActive(false);
        WonSettlement.SetActive(false);
        LoseSettlement.SetActive(false);
        ExeSettlement.SetActive(false);
        AbilitySettlement.SetActive(false);
        ReStart.SetActive(false);
        LevelUp.SetActive(false);

        state = Battletest.CHOOSEACTION;
        ChooseAction();
    }

    void ChooseAction()    //回合開始、選擇操作、選擇行動整合
    {
        if (i7 == false)
        {
            i6 = false;
            BS();
        }
        else
        {
            CT();
            CAD();
        }
    }

    void BS()    //回合開始_選項展開
    {
        SpriteRenderer c1spr = c1.GetComponent<SpriteRenderer>();
        SpriteRenderer c2spr = c2.GetComponent<SpriteRenderer>();
        SpriteRenderer c3spr = c3.GetComponent<SpriteRenderer>();
        SpriteRenderer c4spr = c4.GetComponent<SpriteRenderer>();
        float step = 6 * Time.deltaTime;

        CanNatUse();

        if (i7 == false)
        {
            c1spr.sprite = spr11;
            c2spr.sprite = spr12;
            c3spr.sprite = spr13;
            c4spr.sprite = spr14;

            if (c4.transform.localPosition != Vector3.zero)
            {
                c4.transform.localPosition = Vector3.MoveTowards(c4.transform.localPosition, Vector3.zero, step);
            }
            if (c4.transform.localPosition.x >= -2 && c3.transform.localPosition != Vector3.zero)
            {
                c3.transform.localPosition = Vector3.MoveTowards(c3.transform.localPosition, Vector3.zero, step);
            }
            if (c3.transform.localPosition.x >= -2 && c2.transform.localPosition != Vector3.zero)
            {
                c2.transform.localPosition = Vector3.MoveTowards(c2.transform.localPosition, Vector3.zero, step);
            }
            if (c2.transform.localPosition.x >= -2 && c1.transform.localPosition != o)
            {
                c1.transform.localPosition = Vector3.MoveTowards(c1.transform.localPosition, o, step);
                if (c1.transform.localPosition == o)
                {
                    i7 = true;
                    SkillPower[data.currySkillPower].SetActive(false);
                    data.currySkillPower++;
                    if (data.currySkillPower >= data.maxSkillPower)
                    {
                        data.currySkillPower = data.maxSkillPower;
                    }
                    SkillPower[data.currySkillPower].SetActive(true);
                }
            }
        }
    }

    void CT()    //基本選項操作（WS上下；J確定；K取消）
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (i1 == true)
            {
                c1.transform.localPosition = Vector3.zero;
                c2.transform.localPosition = o;
                i1 = false;
                i2 = true;
            }
            else if (i2 == true)
            {
                c2.transform.localPosition = Vector3.zero;
                c3.transform.localPosition = o;
                i2 = false;
                i3 = true;
            }
            else if (i3 == true)
            {
                c3.transform.localPosition = Vector3.zero;
                c4.transform.localPosition = o;
                i3 = false;
                i4 = true;
            }
            else if (i4 == true)
            {
                c4.transform.localPosition = Vector3.zero;
                c1.transform.localPosition = o;
                i4 = false;
                i1 = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (i1 == true)
            {
                c1.transform.localPosition = Vector3.zero;
                c4.transform.localPosition = o;
                i1 = false;
                i4 = true;
            }
            else if (i2 == true)
            {
                c2.transform.localPosition = Vector3.zero;
                c1.transform.localPosition = o;
                i2 = false;
                i1 = true;
            }
            else if (i3 == true)
            {
                c3.transform.localPosition = Vector3.zero;
                c2.transform.localPosition = o;
                i3 = false;
                i2 = true;
            }
            else if (i4 == true)
            {
                c4.transform.localPosition = Vector3.zero;
                c3.transform.localPosition = o;
                i4 = false;
                i3 = true;
            }
        }
        if (ispr1 == true && ispr2 == false && ispr3 == false)
        {
            if (c1.transform.localPosition == o || c2.transform.localPosition == o || c3.transform.localPosition == o)
            {
                BattleMessage.SetActive(true);
                BattleMessage.GetComponent<Text>().text = "請選擇行動";
            }
            if (c4.transform.localPosition == o)
            {
                if (enemyUnit.unitName == "巨龍")
                {
                    BattleMessage.GetComponent<Text>().text = "無法逃跑\n 請選擇其他行動";
                }
                else
                {
                    BattleMessage.GetComponent<Text>().text = "請選擇行動";
                }
            }
        }
        if (ispr1 == false && ispr2 == true && ispr3 == false)
        {
            if (c1.transform.localPosition == o)
            {
                SkillItemName.SetActive(true);
                SkillItemEffect.SetActive(true);
                SkillItemName.GetComponent<Text>().text = "" + skillUnit1.SkillName;
                SkillItemEffect.GetComponent<Text>().text = "" + skillUnit1.SkillIntro;
            }
            if (c2.transform.localPosition == o)
            {
                SkillItemName.SetActive(true);
                SkillItemEffect.SetActive(true);
                SkillItemName.GetComponent<Text>().text = "" + skillUnit2.SkillName;
                SkillItemEffect.GetComponent<Text>().text = "" + skillUnit2.SkillIntro;
            }
            if (c3.transform.localPosition == o)
            {
                SkillItemName.SetActive(true);
                SkillItemEffect.SetActive(true);
                SkillItemName.GetComponent<Text>().text = "" + skillUnit3.SkillName;
                SkillItemEffect.GetComponent<Text>().text = "" + skillUnit3.SkillIntro;
            }
            if (c4.transform.localPosition == o)
            {
                SkillItemName.SetActive(true);
                SkillItemEffect.SetActive(true);
                SkillItemName.GetComponent<Text>().text = "" + skillUnit4.SkillName;
                SkillItemEffect.GetComponent<Text>().text = "" + skillUnit4.SkillIntro;
            }
        }
        if (ispr1 == false && ispr2 == false && ispr3 == true)
        {
            if (c1.transform.localPosition == o)
            {
                SkillItemName.SetActive(true);
                SkillItemEffect.SetActive(true);
                ItemQuantity.SetActive(true);
                SkillItemName.GetComponent<Text>().text = "【" + spr31.name + "】";
                ItemQuantity.GetComponent<Text>().text = "擁有" + data.ItemQuantity[data.ItemNumber[0]] + "個";
                itemManager.Item(spr31.name);
                SkillItemEffect.GetComponent<Text>().text = itemManager.ItemIntro;
            }
            if (c2.transform.localPosition == o)
            {
                SkillItemName.SetActive(true);
                SkillItemEffect.SetActive(true);
                ItemQuantity.SetActive(true);
                SkillItemName.GetComponent<Text>().text = "【" + spr32.name + "】";
                ItemQuantity.GetComponent<Text>().text = "擁有" + data.ItemQuantity[data.ItemNumber[1]] + "個";
                itemManager.Item(spr32.name);
                SkillItemEffect.GetComponent<Text>().text = itemManager.ItemIntro;
            }
            if (c3.transform.localPosition == o)
            {
                SkillItemName.SetActive(true);
                SkillItemEffect.SetActive(true);
                ItemQuantity.SetActive(true);
                SkillItemName.GetComponent<Text>().text = "【" + spr33.name + "】";
                ItemQuantity.GetComponent<Text>().text = "擁有" + data.ItemQuantity[data.ItemNumber[2]] + "個";
                itemManager.Item(spr33.name);
                SkillItemEffect.GetComponent<Text>().text = itemManager.ItemIntro;
            }
            if (c4.transform.localPosition == o)
            {
                SkillItemName.SetActive(true);
                SkillItemEffect.SetActive(true);
                ItemQuantity.SetActive(true);
                SkillItemName.GetComponent<Text>().text = "【" + spr34.name + "】"; 
                ItemQuantity.GetComponent<Text>().text = "擁有" + data.ItemQuantity[data.ItemNumber[3]] + "個";
                itemManager.Item(spr34.name);
                SkillItemEffect.GetComponent<Text>().text = itemManager.ItemIntro;
            }
        }
    }

    void CAD()    //選擇行動
    {
        SpriteRenderer c1spr = c1.GetComponent<SpriteRenderer>();
        SpriteRenderer c2spr = c2.GetComponent<SpriteRenderer>();
        SpriteRenderer c3spr = c3.GetComponent<SpriteRenderer>();
        SpriteRenderer c4spr = c4.GetComponent<SpriteRenderer>();
        float step = 6 * Time.deltaTime;

        CanNatUse();

        if (Input.GetKeyDown(KeyCode.J))
        {
            if (i1 == true || i2 == true || i3 == true || i4 == true)
            {
                if (ispr1 == true)
                {
                    if (c1.transform.localPosition == o)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        ispr1 = false;
                        i6 = true;
                        i0 = true;
                        if (state == Battletest.CHOOSEACTION)
                        {
                            state = Battletest.PLAYERTURN;
                            if (data.speed >= enemyUnit.speed)
                            {
                                StartCoroutine(AttackSkill(c1.GetComponent<SpriteRenderer>().sprite.name));
                            }
                            else
                            {
                                StartCoroutine(EnemyTrun(c1.GetComponent<SpriteRenderer>().sprite.name));
                            }
                        }
                    }
                    if (c2.transform.localPosition == o)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        BattleMessage.SetActive(false);
                        ispr1 = false;
                        ispr2 = true;
                        i0 = true;
                    }
                    if (c3.transform.localPosition == o)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        BattleMessage.SetActive(false);
                        ispr1 = false;
                        ispr3 = true;
                        i0 = true;
                    }
                    if (c4.transform.localPosition == o)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        if (state == Battletest.CHOOSEACTION)
                        {
                            if (enemyUnit.unitName != "巨龍")
                            {
                                ispr1 = false;
                                i6 = true;
                                i0 = true;
                                BattleMessage.SetActive(false);
                                StartCoroutine(Run());
                            }
                            else if (enemyUnit.unitName == "巨龍")
                            {
                                i6 = false;
                                i0 = false;
                            }
                        }
                    }
                }
                else if (ispr2 == true)    //技能選項
                {
                    i6 = true;
                    state = Battletest.PLAYERTURN;
                    if (c1.transform.localPosition == o && data.currySkillPower >= skillUnit1.Cost)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        if (data.speed >= enemyUnit.speed)
                        {
                            StartCoroutine(AttackSkill(c1.GetComponent<SpriteRenderer>().sprite.name));
                        }
                        else
                        {
                            StartCoroutine(EnemyTrun(c1.GetComponent<SpriteRenderer>().sprite.name));
                        }
                        SkillItemName.SetActive(false);
                        SkillItemEffect.SetActive(false);
                        i0 = true;
                    }
                    else if (c1.transform.localPosition == o && data.currySkillPower < skillUnit1.Cost)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        i6 = false;
                        i0 = false;
                    }
                    if (c2.transform.localPosition == o && data.currySkillPower >= skillUnit2.Cost)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        if (data.speed >= enemyUnit.speed)
                        {
                            StartCoroutine(AttackSkill(c2.GetComponent<SpriteRenderer>().sprite.name));
                        }
                        else
                        {
                            StartCoroutine(EnemyTrun(c2.GetComponent<SpriteRenderer>().sprite.name));
                        }
                        SkillItemName.SetActive(false);
                        SkillItemEffect.SetActive(false);
                        i0 = true;
                    }
                    else if (c2.transform.localPosition == o && data.currySkillPower < skillUnit2.Cost)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        i6 = false;
                        i0 = false;
                    }
                    if (c3.transform.localPosition == o && data.currySkillPower >= skillUnit3.Cost)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        if (data.speed >= enemyUnit.speed)
                        {
                            StartCoroutine(AttackSkill(c3.GetComponent<SpriteRenderer>().sprite.name));
                        }
                        else
                        {
                            StartCoroutine(EnemyTrun(c3.GetComponent<SpriteRenderer>().sprite.name));
                        }
                        SkillItemName.SetActive(false);
                        SkillItemEffect.SetActive(false);
                        i0 = true;
                    }
                    else if (c3.transform.localPosition == o && data.currySkillPower < skillUnit3.Cost)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        i6 = false;
                        i0 = false;
                    }
                    if (c4.transform.localPosition == o && data.currySkillPower >= skillUnit4.Cost)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        if (data.speed >= enemyUnit.speed)
                        {
                            StartCoroutine(AttackSkill(c4.GetComponent<SpriteRenderer>().sprite.name));
                        }
                        else
                        {
                            StartCoroutine(EnemyTrun(c4.GetComponent<SpriteRenderer>().sprite.name));
                        }
                        SkillItemName.SetActive(false);
                        SkillItemEffect.SetActive(false);
                        i0 = true;
                    }
                    else if (c4.transform.localPosition == o && data.currySkillPower < skillUnit4.Cost)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        i6 = false;
                        i0 = false;
                    }
                }
                else if (ispr3 == true)    //道具選項
                {
                    i6 = true;
                    state = Battletest.PLAYERTURN;
                    if (c1.transform.localPosition == o && data.ItemQuantity[data.ItemNumber[0]] > 0)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        StartCoroutine(Item(c1.GetComponent<SpriteRenderer>().sprite.name));
                        SkillItemName.SetActive(false);
                        SkillItemEffect.SetActive(false);
                        ItemQuantity.SetActive(false);
                        i0 = true;
                    }
                    else if (c1.transform.localPosition == o && data.ItemQuantity[data.ItemNumber[0]] <= 0)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        i6 = false;
                        i0 = false;
                    }
                    if (c2.transform.localPosition == o && data.ItemQuantity[data.ItemNumber[1]] > 0)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        StartCoroutine(Item(c2.GetComponent<SpriteRenderer>().sprite.name));
                        SkillItemName.SetActive(false);
                        SkillItemEffect.SetActive(false);
                        ItemQuantity.SetActive(false);
                        i0 = true;
                    }
                    else if (c2.transform.localPosition == o && data.ItemQuantity[data.ItemNumber[1]] <= 0)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        i6 = false;
                        i0 = false;
                    }
                    if (c3.transform.localPosition == o && data.ItemQuantity[data.ItemNumber[2]] > 0)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        StartCoroutine(Item(c3.GetComponent<SpriteRenderer>().sprite.name));
                        SkillItemName.SetActive(false);
                        SkillItemEffect.SetActive(false);
                        ItemQuantity.SetActive(false);
                        i0 = true;
                    }
                    else if (c3.transform.localPosition == o && data.ItemQuantity[data.ItemNumber[2]] <= 0)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        i6 = false;
                        i0 = false;
                    }
                    if (c4.transform.localPosition == o && data.ItemQuantity[data.ItemNumber[3]] > 0)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        StartCoroutine(Item(c4.GetComponent<SpriteRenderer>().sprite.name));
                        SkillItemName.SetActive(false);
                        SkillItemEffect.SetActive(false);
                        ItemQuantity.SetActive(false);
                        i0 = true;
                    }
                    else if (c4.transform.localPosition == o && data.ItemQuantity[data.ItemNumber[3]] <= 0)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        i6 = false;
                        i0 = false;
                    }
                }
            }
            
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (i1 == true || i2 == true || i3 == true || i4 == true)
            {
                if (ispr2 == true || ispr3 == true)
                {
                    SoundManager.SoundInstance.SoundHitCancel();
                    SkillItemName.SetActive(false);
                    SkillItemEffect.SetActive(false);
                    ItemQuantity.SetActive(false);
                    ispr2 = false;
                    ispr3 = false;
                    ispr1 = true;
                    i0 = true;
                }
            }
        }

        if (i0 == true)
        {
            if (i1 == true)
            {
                c1.transform.localPosition = Vector3.MoveTowards(c1.transform.localPosition, Vector3.zero, step);
                if (c1.transform.transform.localPosition == Vector3.zero)
                {
                    i1 = false;
                    if (i1 == false && i2 == false && i3 == false && i4 == false)
                    {
                        i5 = true;
                    }
                }
            }
            if (i2 == true)
            {
                c2.transform.localPosition = Vector3.MoveTowards(c2.transform.localPosition, Vector3.zero, step);
                if (c2.transform.transform.localPosition == Vector3.zero)
                {
                    i2 = false;
                    if (i1 == false && i2 == false && i3 == false && i4 == false)
                    {
                        i5 = true;
                    }
                }
            }
            if (i3 == true)
            {
                c3.transform.localPosition = Vector3.MoveTowards(c3.transform.localPosition, Vector3.zero, step);
                if (c3.transform.transform.localPosition == Vector3.zero)
                {
                    i3 = false;
                    if (i1 == false && i2 == false && i3 == false && i4 == false)
                    {
                        i5 = true;
                    }
                }
            }
            if (i4 == true)
            {
                c4.transform.localPosition = Vector3.MoveTowards(c4.transform.localPosition, Vector3.zero, step);
                if (c4.transform.transform.localPosition == Vector3.zero)
                {
                    i4 = false;
                    if (i1 == false && i2 == false && i3 == false && i4 == false)
                    {
                        i5 = true;
                    }
                }
            }

            if (i1 == false && i2 == false && i3 == false && i4 == false && i5 == true)
            {
                if (c1.transform.localPosition != c)
                {
                    c1.transform.localPosition = Vector3.MoveTowards(c1.transform.localPosition, c, step);
                }
                if (c1.transform.localPosition.x <= -0.4 && c2.transform.localPosition != c)
                {
                    c2.transform.localPosition = Vector3.MoveTowards(c2.transform.localPosition, c, step);
                }
                if (c2.transform.localPosition.x <= -0.4 && c3.transform.localPosition != c)
                {
                    c3.transform.localPosition = Vector3.MoveTowards(c3.transform.localPosition, c, step);
                }
                if (c3.transform.localPosition.x <= -0.4 && c4.transform.localPosition != c)
                {
                    c4.transform.localPosition = Vector3.MoveTowards(c4.transform.localPosition, c, step);
                    if (c4.transform.localPosition == c)
                    {
                        if (ispr1 == true)
                        {
                            ispr2 = false;
                            ispr3 = false;
                            c1spr.sprite = spr11;
                            c2spr.sprite = spr12;
                            c3spr.sprite = spr13;
                            c4spr.sprite = spr14;
                        }
                        if (ispr2 == true)
                        {
                            ispr1 = false;
                            c1spr.sprite = spr21;
                            c2spr.sprite = spr22;
                            c3spr.sprite = spr23;
                            c4spr.sprite = spr24;
                            //c1spr.sprite = skillUnit[0].SkillSprite;
                            //c2spr.sprite = skillUnit[1].SkillSprite;
                            //c3spr.sprite = skillUnit[2].SkillSprite;
                            //c4spr.sprite = skillUnit[3].SkillSprite;
                        }
                        if (ispr3 == true)
                        {
                            ispr1 = false;
                            c1spr.sprite = spr31;
                            c2spr.sprite = spr32;
                            c3spr.sprite = spr33;
                            c4spr.sprite = spr34;
                        }
                        i5 = false;
                    }
                }
            }
            if (i1 == false && i2 == false && i3 == false && i4 == false && i5 == false && i6 == false)
            {
                if (c4.transform.localPosition != Vector3.zero)
                {
                    c4.transform.localPosition = Vector3.MoveTowards(c4.transform.localPosition, Vector3.zero, step);
                }
                if (c4.transform.localPosition.x >= -2 && c3.transform.localPosition != Vector3.zero)
                {
                    c3.transform.localPosition = Vector3.MoveTowards(c3.transform.localPosition, Vector3.zero, step);
                }
                if (c3.transform.localPosition.x >= -2 && c2.transform.localPosition != Vector3.zero)
                {
                    c2.transform.localPosition = Vector3.MoveTowards(c2.transform.localPosition, Vector3.zero, step);
                }
                if (c2.transform.localPosition.x >= -2 && c1.transform.localPosition != o)
                {
                    c1.transform.localPosition = Vector3.MoveTowards(c1.transform.localPosition, o, step);
                    if (c1.transform.localPosition == o)
                    {
                        i1 = true;
                        i5 = false;
                        i0 = false;
                    }
                }
            }
        }
    }

    void ReSet()    //恢復初始設定
    {
        c1.transform.localPosition = c;
        c2.transform.localPosition = c;
        c3.transform.localPosition = c;
        c4.transform.localPosition = c;
        i0 = false;     //選項(收)階段一是否開始
        i1 = true;      //i1是否介於(0,0,0)與(0.4,0,0)之間
        i2 = false;     //i2是否介於(0,0,0)與(0.4,0,0)之間
        i3 = false;     //i3是否介於(0,0,0)與(0.4,0,0)之間
        i4 = false;     //i4是否介於(0,0,0)與(0.4,0,0)之間
        i5 = false;     //選項(收)階段二是否開始

        i6 = false;     //是否收後不展開
        i7 = false;     //戰鬥開始

        ispr1 = true;   //是否為主選項
        ispr2 = false;  //是否為技能選項
        ispr3 = false;  //是否為道具選項

        SpriteRenderer c1spr = c1.GetComponent<SpriteRenderer>();
        SpriteRenderer c2spr = c2.GetComponent<SpriteRenderer>();
        SpriteRenderer c3spr = c3.GetComponent<SpriteRenderer>();
        SpriteRenderer c4spr = c4.GetComponent<SpriteRenderer>();
        c1spr.sprite = spr11;
        c2spr.sprite = spr12;
        c3spr.sprite = spr13;
        c4spr.sprite = spr14;

        EnemyHit = 0;

        ReStartBattle.GetComponent<Image>().color = Color.gray;
    }

    IEnumerator EnemyTrun(string playerSKILLNAME)    //敵人攻擊
    {
        yield return new WaitForSeconds(1f);

        //EnemyAttack();
        if (enemyUnit.unitName == "巨龍")
        {
            BattleMessage.SetActive(true);
            if (enemyUnit.currentHp <= enemyUnit.maxHp * 0.8f && DragonPower == false)
            {
                SoundManager.SoundInstance.SoundBuff();
                BattleMessage.GetComponent<Text>().text = enemyUnit.unitName + "施展了龍之力";
                enemyUnit.atk += 25;
                enemyUnit.def += 20;
                DragonPower = true;
                yield return new WaitForSeconds(1f);
                BattleMessage.GetComponent<Text>().text = enemyUnit.unitName + "的攻擊、防禦超大幅提升";
                if (data.speed >= enemyUnit.speed || playerSKILLNAME == null)
                {
                    state = Battletest.ENDTRUN;
                    StartCoroutine(EndTrun());
                }
                else
                {
                    state = Battletest.PLAYERTURN;
                    StartCoroutine(AttackSkill(playerSKILLNAME));
                }
                
            }
            else if (enemyUnit.currentHp <= enemyUnit.maxHp * 0.4f && DragonEnchantment == 0)
            {
                enemyUnit.def += 10;
                DragonEnchantment += 1;
                BattleMessage.GetComponent<Text>().text = enemyUnit.unitName + "展開了龍結界";
                if (data.speed >= enemyUnit.speed || playerSKILLNAME == null)
                {
                    state = Battletest.ENDTRUN;
                    StartCoroutine(EndTrun());
                }
                else
                {
                    state = Battletest.PLAYERTURN;
                    StartCoroutine(AttackSkill(playerSKILLNAME));
                }
            }
            else if (enemyUnit.currentHp <= enemyUnit.maxHp * 0.1f && DragonEndBreath == false)
            {
                BattleMessage.GetComponent<Text>().text = enemyUnit.unitName + "施展了終焉龍息";
                SoundManager.SoundInstance.SoundEnemyAttack();
                EnemyHit = Mathf.CeilToInt(data.maxHp * 0.8f);
                DragonEndBreath = true;

                #region 傷害結算與死亡判定
                if (EnemyHit <= 0)
                {
                    EnemyHit = 0;
                }
                bool isDead;
                data.currentHp -= EnemyHit;
                if (data.currentHp <= 0)
                {
                    isDead = true;
                }
                else
                {
                    isDead = false;
                }

                PlayerDamageHpSettle();

                yield return new WaitForSeconds(1f);
                BattleMessage.GetComponent<Text>().text = data.unitName + "受到了" + EnemyHit + "點傷害";

                if (isDead == true)
                {
                    yield return new WaitForSeconds(1f);
                    BattleMessage.GetComponent<Text>().text = data.unitName + "倒下了";
                    state = Battletest.LOST;
                    StartCoroutine(EndBattle());
                }
                else
                {
                    if (data.speed >= enemyUnit.speed || playerSKILLNAME == null)
                    {
                        state = Battletest.ENDTRUN;
                        StartCoroutine(EndTrun());
                    }
                    else
                    {
                        state = Battletest.PLAYERTURN;
                        StartCoroutine(AttackSkill(playerSKILLNAME));
                    }
                }
                #endregion
            }
            else
            {
                int a = 4;
                int b = 7;
                int i = Random.Range(0, 10);
                if (i >= 0 && i < a)    //龍息
                {
                    BattleMessage.GetComponent<Text>().text = enemyUnit.unitName + "施展了龍息";
                    EnemyHit = Mathf.CeilToInt(enemyUnit.atk * 1.2f - data.def);
                    yield return new WaitForSeconds(1f);

                    #region 傷害結算與死亡判定
                    if (EnemyHit <= 0)
                    {
                        EnemyHit = 0;
                    }
                    bool isDead;
                    data.currentHp -= EnemyHit;
                    if (data.currentHp <= 0)
                    {
                        isDead = true;
                    }
                    else
                    {
                        isDead = false;
                    }

                    PlayerDamageHpSettle();

                    yield return new WaitForSeconds(1f);
                    BattleMessage.GetComponent<Text>().text = data.unitName + "受到了" + EnemyHit + "點傷害";

                    if (isDead == true)
                    {
                        yield return new WaitForSeconds(1f);
                        BattleMessage.GetComponent<Text>().text = data.unitName + "倒下了";
                        state = Battletest.LOST;
                        StartCoroutine(EndBattle());
                    }
                    else
                    {
                        int fire = Random.Range(0, 10);
                        if (fire >= 0 && fire <= 4 && PlayerFireCount == 0)
                        {
                            yield return new WaitForSeconds(1f);
                            PlayerFireCount = 3;
                            BattleMessage.GetComponent<Text>().text = data.unitName + "受到了灼傷";
                        }
                        if (data.speed >= enemyUnit.speed || playerSKILLNAME == null)
                        {
                            state = Battletest.ENDTRUN;
                            StartCoroutine(EndTrun());
                        }
                        else
                        {
                            state = Battletest.PLAYERTURN;
                            StartCoroutine(AttackSkill(playerSKILLNAME));
                        }
                    }
                    #endregion

                }
                else if (i >= a && i < b)    //龍吼
                {
                    if (data.atk > PlayerOriAtk || data.def > PlayerOriDef || data.speed > PlayerOriSpd)
                    {
                        BattleMessage.GetComponent<Text>().text = enemyUnit.unitName + "施展了龍吼";
                        if (data.atk > PlayerOriAtk)
                        {
                            data.atk = PlayerOriAtk;
                        }
                        if (data.def > PlayerOriDef)
                        {
                            data.def = PlayerOriDef;
                        }
                        if (data.speed > PlayerOriSpd)
                        {
                            data.speed = PlayerOriSpd;
                        }
                        SkillUPcount = 0;
                        SkillUPtime = 0;
                        yield return new WaitForSeconds(1f);
                        BattleMessage.GetComponent<Text>().text = data.unitName + "的增益狀態解除了";
                        if (data.speed >= enemyUnit.speed || playerSKILLNAME == null)
                        {
                            state = Battletest.ENDTRUN;
                            StartCoroutine(EndTrun());
                        }
                        else
                        {
                            state = Battletest.PLAYERTURN;
                            StartCoroutine(AttackSkill(playerSKILLNAME));
                        }
                    }
                    else
                    {
                        BattleMessage.GetComponent<Text>().text = enemyUnit.unitName + "施展了攻擊";
                        SoundManager.SoundInstance.SoundEnemyAttack();
                        EnemyHit = enemyUnit.atk - data.def;
                        #region 傷害結算與死亡判定
                        if (EnemyHit <= 0)
                        {
                            EnemyHit = 0;
                        }
                        bool isDead;
                        data.currentHp -= EnemyHit;
                        if (data.currentHp <= 0)
                        {
                            isDead = true;
                        }
                        else
                        {
                            isDead = false;
                        }

                        PlayerDamageHpSettle();

                        yield return new WaitForSeconds(1f);
                        BattleMessage.GetComponent<Text>().text = data.unitName + "受到了" + EnemyHit + "點傷害";

                        if (isDead == true)
                        {
                            yield return new WaitForSeconds(1f);
                            BattleMessage.GetComponent<Text>().text = data.unitName + "倒下了";
                            state = Battletest.LOST;
                            StartCoroutine(EndBattle());
                        }
                        else
                        {
                            if (data.speed >= enemyUnit.speed || playerSKILLNAME == null)
                            {
                                state = Battletest.ENDTRUN;
                                StartCoroutine(EndTrun());
                            }
                            else
                            {
                                state = Battletest.PLAYERTURN;
                                StartCoroutine(AttackSkill(playerSKILLNAME));
                            }
                        }
                        #endregion
                    }
                }
                else
                {
                    BattleMessage.GetComponent<Text>().text = enemyUnit.unitName + "施展了攻擊";
                    SoundManager.SoundInstance.SoundEnemyAttack();
                    EnemyHit = enemyUnit.atk - data.def;
                    #region 傷害結算與死亡判定
                    if (EnemyHit <= 0)
                    {
                        EnemyHit = 0;
                    }
                    bool isDead;
                    data.currentHp -= EnemyHit;
                    if (data.currentHp <= 0)
                    {
                        isDead = true;
                    }
                    else
                    {
                        isDead = false;
                    }

                    PlayerDamageHpSettle();

                    yield return new WaitForSeconds(1f);
                    BattleMessage.GetComponent<Text>().text = data.unitName + "受到了" + EnemyHit + "點傷害";

                    if (isDead == true)
                    {
                        yield return new WaitForSeconds(1f);
                        BattleMessage.GetComponent<Text>().text = data.unitName + "倒下了";
                        state = Battletest.LOST;
                        StartCoroutine(EndBattle());
                    }
                    else
                    {
                        if (data.speed >= enemyUnit.speed || playerSKILLNAME == null)
                        {
                            state = Battletest.ENDTRUN;
                            StartCoroutine(EndTrun());
                        }
                        else
                        {
                            state = Battletest.PLAYERTURN;
                            StartCoroutine(AttackSkill(playerSKILLNAME));
                        }
                    }
                    #endregion
                }

            }
        }
    }

    void MeetEnemy()    //在 N 地點遇見 M 敵人
    {
        if (data.mapNumber == 6)
        {
            EnemyNumber = 1;
        }
        if (data.mapNumber == 14)
        {
            EnemyNumber = 0;
        }
    }

    public void EnemyAttack()
    {
        if (enemyUnit.unitName == "巨龍")
        {
            print("遭遇巨龍");
            BattleMessage.SetActive(true);
            BattleMessage.GetComponent<Text>().text = enemyUnit.unitName + "施展了攻擊";
            SoundManager.SoundInstance.SoundEnemyAttack();
            EnemyHit = enemyUnit.atk - data.def;
        }

        if (enemyUnit.unitName == "拋瓦")
        {
            BattleMessage.SetActive(true);
            BattleMessage.GetComponent<Text>().text = enemyUnit.unitName + "施展了攻擊";
            SoundManager.SoundInstance.SoundEnemyAttack();
            EnemyHit = enemyUnit.atk - data.def;
        }
    }

    IEnumerator AttackSkill(string SKILLNAME)    //玩家攻擊
    {
        yield return new WaitForSeconds(1f);
        BattleMessage.SetActive(true);
        BattleMessage.GetComponent<Text>().text = data.unitName + "施展了" + SKILLNAME;

        #region 00_普通攻擊
        if (SKILLNAME == "攻擊")
        {
            SoundManager.SoundInstance.SoundAttack();
            int hit = data.atk - enemyUnit.def;
            hit = Mathf.Clamp(hit, 0, data.atk);
            EnemyIsDead = enemyUnit.TakeDamage(hit);
            EnemyDamageHpSettle();

            yield return new WaitForSeconds(1f);
            BattleMessage.GetComponent<Text>().text = enemyUnit.unitName + "受到了" + hit + "點傷害";

            StartCoroutine(EnemyIsDeadOrAlive());
        }
        #endregion

        #region 01_狂擊
        if (SKILLNAME == "狂擊")
        {
            SoundManager.SoundInstance.SoundBlast();
            SkillPowerExpend(2);

            int hit = 2 * data.atk - enemyUnit.def;
            hit = Mathf.Clamp(hit, 0, 2 * data.atk);
            EnemyIsDead = enemyUnit.TakeDamage(hit);
            EnemyDamageHpSettle();

            yield return new WaitForSeconds(1f);
            BattleMessage.GetComponent<Text>().text = enemyUnit.unitName + "受到了" + hit + "點傷害";

            StartCoroutine(EnemyIsDeadOrAlive());
        }
        #endregion

        #region 02_高級強化
        if (SKILLNAME == "高級強化")
        {
            SoundManager.SoundInstance.SoundBuff();
            SkillPowerExpend(1);

            if (SkillUPtime < 3)
            {
                data.atk += 15;
                data.def += 10;
                SkillUPcount = 4;
                SkillUPtime += 1;
                yield return new WaitForSeconds(1f);
                BattleMessage.GetComponent<Text>().text = data.unitName + "的攻擊、防禦大幅提升";
                if (SkillUPtime == 3)
                {
                    yield return new WaitForSeconds(1f);
                    BattleMessage.GetComponent<Text>().text = data.unitName + "強化次數已達最大值";
                }
            }
            else if (SkillUPtime == 3)
            {
                yield return new WaitForSeconds(1f);
                BattleMessage.GetComponent<Text>().text = data.unitName + "強化次數已達最大值";
            }
            

            if (data.speed >= enemyUnit.speed)
            {
                state = Battletest.ENEMYTURN;
                StartCoroutine(EnemyTrun(null));
            }
            else
            {
                state = Battletest.ENDTRUN;
                StartCoroutine(EndTrun());
            }
        }
        #endregion

        #region 03_三連擊
        if (SKILLNAME == "三連擊")
        {
            SkillPowerExpend(3);

            for (int i = 1; i <= 3; i++)
            {
                yield return new WaitForSeconds(1f);
                SoundManager.SoundInstance.SoundThirdSlash();
                float k = Random.Range(data.atk, data.atk * 1.5f) - enemyUnit.def;
                k = Mathf.Clamp(k, 0, data.atk * 1.5f);
                int hit = (int)k;
                EnemyIsDead = enemyUnit.TakeDamage(hit);
                EnemyDamageHpSettle();

                yield return new WaitForSeconds(1f);
                BattleMessage.GetComponent<Text>().text = enemyUnit.unitName + "受到了" + hit + "點傷害";
                if (enemyUnit.currentHp <= 0)
                {
                    break;
                }
            }

            StartCoroutine(EnemyIsDeadOrAlive());
        }
        #endregion

        #region 04_瀕死一擊
        if (SKILLNAME == "瀕死一擊")
        {
            SoundManager.SoundInstance.SoundNearDeathSlash();
            SkillPowerExpend(3);

            float hit;
            if (data.currentHp / data.maxHp <= 1 && data.currentHp / data.maxHp > 0.66)
            {
                hit = data.atk * 2.3f - enemyUnit.def;
                hit = Mathf.Clamp(hit, 0, data.atk * 2.3f);
                EnemyIsDead = enemyUnit.TakeDamage((int)hit);
                EnemyDamageHpSettle();

                yield return new WaitForSeconds(1f);
                BattleMessage.GetComponent<Text>().text = enemyUnit.unitName + "受到了" + (int)hit + "點傷害";
            }
            else if(data.currentHp / data.maxHp <= 0.66 && data.currentHp / data.maxHp > 0.33f)
            {
                hit = data.atk * 2.6f - enemyUnit.def;
                hit = Mathf.Clamp(hit, 0, data.atk * 2.6f);
                EnemyIsDead = enemyUnit.TakeDamage((int)hit);
                EnemyDamageHpSettle();

                yield return new WaitForSeconds(1f);
                BattleMessage.GetComponent<Text>().text = enemyUnit.unitName + "受到了" + (int)hit + "點傷害";
            }
            else if (data.currentHp / data.maxHp <= 0.33 && data.currentHp / data.maxHp > 0.1f)
            {
                hit = data.atk * 3f - enemyUnit.def;
                hit = Mathf.Clamp(hit, 0, data.atk * 3f);
                EnemyIsDead = enemyUnit.TakeDamage((int)hit);
                EnemyDamageHpSettle();

                yield return new WaitForSeconds(1f);
                BattleMessage.GetComponent<Text>().text = enemyUnit.unitName + "受到了" + (int)hit + "點傷害";
            }
            else if (data.currentHp / data.maxHp <= 0.1 && data.currentHp / data.maxHp > 0)
            {
                hit = data.atk * 3.5f - enemyUnit.def;
                hit = Mathf.Clamp(hit, 0, data.atk * 4f);
                EnemyIsDead = enemyUnit.TakeDamage((int)hit);
                EnemyDamageHpSettle();

                yield return new WaitForSeconds(1f);
                BattleMessage.GetComponent<Text>().text = enemyUnit.unitName + "受到了" + (int)hit + "點傷害";
            }

            StartCoroutine(EnemyIsDeadOrAlive());
        }
        #endregion

        #region 05_施毒
        if (SKILLNAME == "施毒")
        {
            POISONdamage = 5;
            POISONcount = 4;

            yield return new WaitForSeconds(1f);
            BattleMessage.GetComponent<Text>().text = enemyUnit.unitName + "中毒了";

            state = Battletest.ENEMYTURN;
            StartCoroutine(EnemyTrun(null));
        }
        #endregion
    }

    IEnumerator Item(string ITEMNAME)    //使用道具
    {
        yield return new WaitForSeconds(1f);
        BattleMessage.SetActive(true);
        BattleMessage.GetComponent<Text>().text = data.unitName + "使用了" + ITEMNAME;
        SoundManager.SoundInstance.SoundDrink();

        #region 01_紅藥水
        if (ITEMNAME == "高級紅藥水")
        {
            data.ItemQuantity[2] -= 1;

            float RestoreHP = Mathf.Round(data.maxHp * 0.6f);
            data.currentHp += RestoreHP;
            data.currentHp = Mathf.Clamp(data.currentHp, 0, data.maxHp);
            PlayerHp.GetComponent<Image>().fillAmount = data.currentHp / data.maxHp;
            playerHp.text = data.currentHp.ToString() + "/" + data.maxHp.ToString();

            yield return new WaitForSeconds(1f);
            BattleMessage.GetComponent<Text>().text = data.unitName + "恢復了" + RestoreHP + "點血量";
        }
        #endregion

        #region 02_藍藥水
        if (ITEMNAME == "高級藍藥水")
        {
            data.ItemQuantity[6] -= 1;

            SkillPowerExpend(-3);

            yield return new WaitForSeconds(1f);
            BattleMessage.GetComponent<Text>().text = data.unitName + "恢復了" + 3 + "點能量";
        }
        #endregion

        #region 03_秘藥
        if (ITEMNAME == "高級秘藥")
        {
            data.ItemQuantity[14] -= 1;

            data.atk += 15;
            data.def += 10;
            SkillUPcount = 3;

            yield return new WaitForSeconds(1f);
            BattleMessage.GetComponent<Text>().text = data.unitName + "的攻擊力、防禦力與速度大幅提升";
        }
        #endregion

        #region 04_萬能草
        if (ITEMNAME == "萬能草")
        {
            data.ItemQuantity[16] -= 1;

            yield return new WaitForSeconds(1f);
            SoundManager.SoundInstance.SoundBuff();
            PlayerFireCount = 0;
            BattleMessage.GetComponent<Text>().text = data.unitName + "的異常狀態消失了";
        }
        #endregion

        state = Battletest.ENEMYTURN;
        StartCoroutine(EnemyTrun(null));
    }

    IEnumerator Run()    //玩家逃跑
    {
        yield return new WaitForSeconds(1f);
        SoundManager.SoundInstance.SoundRun();
        yield return new WaitForSeconds(1f);
        BattleMessage.SetActive(true);
        BattleMessage.GetComponent<Text>().text = data.unitName + "逃跑了";
        yield return new WaitForSeconds(1.5f);
        DelayReturnMap();
    }

    IEnumerator EndTrun()    //此回合結束
    {
        #region 勇者燃燒傷害倒數
        if (PlayerFireCount > 0)
        {
            yield return new WaitForSeconds(1f);
            EnemyHit = Mathf.CeilToInt(data.maxHp * 0.1f);
            data.currentHp -= EnemyHit;
            PlayerFireCount -= 1;

            #region 傷害結算與死亡判定
            bool isDead;
            if (data.currentHp <= 0)
            {
                isDead = true;
            }
            else
            {
                isDead = false;
            }

            PlayerDamageHpSettle();

            yield return new WaitForSeconds(1f);
            BattleMessage.GetComponent<Text>().text = data.unitName + "受到了" + EnemyHit + "點的灼傷";

            if (isDead == true)
            {
                yield return new WaitForSeconds(1f);
                BattleMessage.GetComponent<Text>().text = data.unitName + "倒下了";
                state = Battletest.LOST;
                StartCoroutine(EndBattle());
            }
            else
            {
                if (PlayerFireCount == 0)
                {
                    yield return new WaitForSeconds(1f);
                    BattleMessage.GetComponent<Text>().text = data.unitName + "的灼傷解除了";
                }
            }
            #endregion
        }
        #endregion
        #region 勇者強化倒數
        if (SkillUPcount > 0)
        {
            SkillUPcount--;
            if (SkillUPcount == 0)
            {
                data.atk = PlayerOriAtk;
                data.def = PlayerOriDef;
                SkillUPtime = 0;
                yield return new WaitForSeconds(1f);
                BattleMessage.GetComponent<Text>().text = data.unitName + "的攻擊、防禦回復了";
            }
        }
        #endregion
        #region 敵人中毒傷害與倒數
        if (POISONcount > 0)
        {
            yield return new WaitForSeconds(1f);
            BattleMessage.GetComponent<Text>().text = enemyUnit.unitName + "毒發了";
            EnemyIsDead = enemyUnit.TakeDamage(POISONdamage);
            EnemyDamageHpSettle();

            yield return new WaitForSeconds(1f);
            BattleMessage.GetComponent<Text>().text = enemyUnit.unitName + "受到了" + POISONdamage + "點傷害";

            if (EnemyIsDead)
            {
                yield return new WaitForSeconds(1f);
                BattleMessage.GetComponent<Text>().text = enemyUnit.unitName + "倒下了";
                state = Battletest.WON;
                StartCoroutine(EndBattle());
            }
            else
            {
                POISONcount--;
                StartCoroutine(EndTrun());
            }
        }
        #endregion
        #region 聖獸拋瓦改變型態
        if (enemyUnit.unitName == "拋瓦")
        {
            yield return new WaitForSeconds(1f);
            if (enemyUnit.Self.GetComponent<SpriteRenderer>().sprite == enemyUnit.nico)
            {
                enemyUnit.Self.GetComponent<SpriteRenderer>().sprite = enemyUnit.kimo;
            }
            else if (enemyUnit.Self.GetComponent<SpriteRenderer>().sprite == enemyUnit.kimo)
            {
                enemyUnit.Self.GetComponent<SpriteRenderer>().sprite = enemyUnit.nico;
            }
            BattleMessage.GetComponent<Text>().text = "拋瓦的型態改變了";
        }
        #endregion
        yield return new WaitForSeconds(1f);
        if (data.currentHp > 0)
        {
            ReSet();
            state = Battletest.CHOOSEACTION;
        }
    }

    IEnumerator EndBattle()    //戰鬥結束
    {
        yield return new WaitForSeconds(1.5f);
        BattleSettlement.SetActive(true);

        data.atk = PlayerOriAtk;    //攻擊力回歸初始值
        data.def = PlayerOriDef;    //防禦力回歸初始值

        if (state == Battletest.WON)    //勝利
        {
            audBattle.clip = WonBGM;
            audBattle.Play();
            WonSettlement.SetActive(true);
            Won.GetComponent<Text>().text = "你打倒了" + enemyUnit.unitName;
            yield return new WaitForSeconds(1f);

            ExeSettlement.SetActive(true);
            getExe.GetComponent<Text>().text = "獲得了" + enemyUnit.exe + "點經驗值";
            if (data.unitLevel < 25)
            {
                Exe.GetComponent<Text>().text = data.curryExp + "/" + data.maxExp[data.unitLevel];
                ExeLine.GetComponent<Image>().fillAmount = (float)data.curryExp / data.maxExp[data.unitLevel];
            }
            else if (data.unitLevel == 25)
            {
                Exe.GetComponent<Text>().text = "max / max";
                ExeLine.GetComponent<Image>().fillAmount = 1;
            }
            

            int PlayerFinalExe = PlayerOriExe + enemyUnit.exe;
            bool IsLevelUp = false;

            if (data.unitLevel < 25)
            {
                while (data.curryExp != PlayerFinalExe)
                {
                    data.curryExp += 1;
                    yield return new WaitForSeconds(0.033f);
                    Exe.GetComponent<Text>().text = data.curryExp + "/" + data.maxExp[data.unitLevel];
                    ExeLine.GetComponent<Image>().fillAmount = (float)data.curryExp / data.maxExp[data.unitLevel];

                    if (data.curryExp == data.maxExp[data.unitLevel])
                    {
                        IsLevelUp = true;
                        LevelUp.SetActive(true);
                        yield return new WaitForSeconds(1f);
                        data.curryExp = 0;
                        PlayerFinalExe -= data.maxExp[data.unitLevel];
                        data.unitLevel += 1;
                        data.atk += 5;
                        data.def += 4;
                        data.speed += 2;
                        data.maxHp += 10;
                        data.currentHp = data.maxHp;
                        if (data.unitLevel == 25)
                        {
                            Exe.GetComponent<Text>().text = "max / max";
                            ExeLine.GetComponent<Image>().fillAmount = 1;
                            break;
                        }
                    }
                }
            }

            PlayerPrefs.SetString("Playerdata", JsonUtility.ToJson(data));
            yield return new WaitForSeconds(1f);

            AbilitySettlement.SetActive(true);
            if (IsLevelUp == true)
            {
                OldLevel.GetComponent<Text>().text = playerOriLevel.ToString();
                NewLevel.GetComponent<Text>().text = data.unitLevel.ToString();
                OldAtk.GetComponent<Text>().text = PlayerOriAtk.ToString();
                NewAtk.GetComponent<Text>().text = data.atk.ToString();
                OldDef.GetComponent<Text>().text = PlayerOriDef.ToString();
                NewDef.GetComponent<Text>().text = data.def.ToString();
                OldSpd.GetComponent<Text>().text = PlayerOriSpd.ToString();
                NewSpd.GetComponent<Text>().text = data.speed.ToString();
                yield return new WaitForSeconds(1f);
                while (NewLevelMask.GetComponent<Image>().fillAmount != 1)
                {
                    NewLevelMask.GetComponent<Image>().fillAmount += 0.02f;
                    NewAtkMask.GetComponent<Image>().fillAmount += 0.02f;
                    NewDefMask.GetComponent<Image>().fillAmount += 0.02f;
                    NewSpdMask.GetComponent<Image>().fillAmount += 0.02f;
                    yield return new WaitForSeconds(0.033f);
                }
            }
            else if (IsLevelUp == false)
            {
                OldLevel.GetComponent<Text>().text = playerOriLevel.ToString();
                OldAtk.GetComponent<Text>().text = PlayerOriAtk.ToString();
                OldDef.GetComponent<Text>().text = PlayerOriDef.ToString();
                OldSpd.GetComponent<Text>().text = PlayerOriSpd.ToString();
            }
            yield return new WaitForSeconds(1f);

            Wonclicktorestart = true;
            ReStart.SetActive(true);
            ReStart.GetComponent<Text>().color = new Color(0f,0f,0f,0f);
            while (ReStart.GetComponent<Text>().color != new Color(0f, 0f, 0f, 1f))
            {
                ReStart.GetComponent<Text>().color += new Color(0f, 0f, 0f, 1/16f);
                yield return new WaitForSeconds(0.033f);
                if (ReStart.GetComponent<Text>().color == new Color(0f, 0f, 0f, 1f))
                {
                    while (ReStart.GetComponent<Text>().color != new Color(0f, 0f, 0f, 0f))
                    {
                        ReStart.GetComponent<Text>().color -= new Color(0f, 0f, 0f, 1/16f);
                        yield return new WaitForSeconds(0.033f);
                    }
                }
            }
        }
        
        if (state == Battletest.LOST)    //戰敗
        {
            audBattle.clip = LoseBGM;
            audBattle.Play();
            LoseSettlement.SetActive(true);
            Losechoose = true;
        }
    }

    void WonClickToRestart()    //勝利後點擊重啟戰鬥
    {
        if (Wonclicktorestart == true)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                Invoke("DelayRestartGame", 1f);
            }
        }
    }
    void LoseChoose()           //死亡後選項操作
    {
        if (Losechoose == true)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (ReStartBattle.GetComponent<Image>().color == Color.gray)
                {
                    ReStartBattle.GetComponent<Image>().color = Color.white;
                    ReturnMap.GetComponent<Image>().color = Color.gray;
                }
                else if (ReturnMap.GetComponent<Image>().color == Color.gray)
                {
                    ReturnMap.GetComponent<Image>().color = Color.white;
                    ReturnMenu.GetComponent<Image>().color = Color.gray;
                }
                else if (ReturnMenu.GetComponent<Image>().color == Color.gray)
                {
                    ReturnMenu.GetComponent<Image>().color = Color.white;
                    ReStartBattle.GetComponent<Image>().color = Color.gray;
                }
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (ReStartBattle.GetComponent<Image>().color == Color.gray)
                {
                    ReStartBattle.GetComponent<Image>().color = Color.white;
                    ReturnMenu.GetComponent<Image>().color = Color.gray;
                }
                else if (ReturnMap.GetComponent<Image>().color == Color.gray)
                {
                    ReturnMap.GetComponent<Image>().color = Color.white;
                    ReStartBattle.GetComponent<Image>().color = Color.gray;
                }
                else if (ReturnMenu.GetComponent<Image>().color == Color.gray)
                {
                    ReturnMenu.GetComponent<Image>().color = Color.white;
                    ReturnMap.GetComponent<Image>().color = Color.gray;
                }
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                if (ReStartBattle.GetComponent<Image>().color == Color.gray)
                {
                    Invoke("DelayRestartGame", 1f);
                }
                else if (ReturnMap.GetComponent<Image>().color == Color.gray)
                {
                    Invoke("DelayReturnMap", 1f);
                }
                else if (ReturnMenu.GetComponent<Image>().color == Color.gray)
                {
                    Invoke("DelayReturnMenu", 1f);
                }
            }
        }
    }

    void DelayRestartGame()    //回戰鬥畫面
    {
        SceneManager.LoadScene("戰鬥畫面");
    }
    void DelayReturnMap()      //回大地圖
    {
        SceneManager.LoadScene("大地圖");
    }
    void DelayReturnMenu()     //回主畫面
    {
        SceneManager.LoadScene("主畫面");
    }

    void EnemyDamageHpSettle()     //敵人血條扣除傷害結算
    {
        if (enemyUnit.currentHp <= 0)
        {
            enemyUnit.currentHp = 0;
        }
        EnemyHp.GetComponent<Image>().fillAmount = enemyUnit.currentHp / enemyUnit.maxHp;
        if (enemyUnit.currentHp <= 0)
        {
            enemyHp.text = 0 + "/" + enemyUnit.maxHp.ToString();
        }
        else
        {
            enemyHp.text = enemyUnit.currentHp.ToString() + "/" + enemyUnit.maxHp.ToString();
        }
    }
    void PlayerDamageHpSettle()    //玩家血條扣除傷害結算
    {
        if (data.currentHp <= 0)
        {
            data.currentHp = 0;
        }
        PlayerHp.GetComponent<Image>().fillAmount = data.currentHp / data.maxHp;
        if (data.currentHp <= 0)
        {
            playerHp.text = 0 + "/" + data.maxHp.ToString();
        }
        else
        {
            playerHp.text = data.currentHp.ToString() + "/" + data.maxHp.ToString();
        }
    }

    IEnumerator EnemyIsDeadOrAlive()    //敵人是否存活
    {
        if (EnemyIsDead)
        {
            yield return new WaitForSeconds(1f);
            BattleMessage.GetComponent<Text>().text = enemyUnit.unitName + "倒下了";
            state = Battletest.WON;
            StartCoroutine(EndBattle());
        }
        else
        {
            if (data.speed >= enemyUnit.speed)
            {
                state = Battletest.ENEMYTURN;
                StartCoroutine(EnemyTrun(null));
            }
            else
            {
                state = Battletest.ENDTRUN;
                StartCoroutine(EndTrun());
            }
        }
    }

    void SkillPowerExpend(int h)    //消耗能量點數後的能量顯示
    {
        data.currySkillPower -= h;
        int i = data.currySkillPower;

        for (int j = 0; j <= data.maxSkillPower; j++)
        {
            SkillPower[j].SetActive(false);
        }
        if (i <= 0)
        {
            i = 0;
        }
        else if (i >= data.maxSkillPower)
        {
            i = data.maxSkillPower;
        }
        SkillPower[i].SetActive(true);
    }

    void CanNatUse()    //無法使用技能或道具
    {
        SpriteRenderer c1spr = c1.GetComponent<SpriteRenderer>();
        SpriteRenderer c2spr = c2.GetComponent<SpriteRenderer>();
        SpriteRenderer c3spr = c3.GetComponent<SpriteRenderer>();
        SpriteRenderer c4spr = c4.GetComponent<SpriteRenderer>();
        if (c1spr.sprite == spr11)
        {
            c1.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else if (c1spr.sprite == spr21 && data.currySkillPower >= 2)
        {
            c1.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else if (c1spr.sprite == spr21 && data.currySkillPower < 2)
        {
            c1.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1);
        }
        else if (c1spr.sprite == spr31 && data.ItemQuantity[data.ItemNumber[0]] != 0)
        {
            c1.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else if (c1spr.sprite == spr31 && data.ItemQuantity[data.ItemNumber[0]] == 0)
        {
            c1.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1);
        }

        if (c2spr.sprite == spr12)
        {
            c2.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else if (c2spr.sprite == spr22 && data.currySkillPower >= 1)
        {
            c2.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else if (c2spr.sprite == spr22 && data.currySkillPower < 1)
        {
            c2.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1);
        }
        else if (c2spr.sprite == spr32 && data.ItemQuantity[data.ItemNumber[1]] != 0)
        {
            c2.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else if (c2spr.sprite == spr32 && data.ItemQuantity[data.ItemNumber[1]] == 0)
        {
            c2.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1);
        }

        if (c3spr.sprite == spr13)
        {
            c3.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else if (c3spr.sprite == spr23 && data.currySkillPower >= 3)
        {
            c3.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else if (c3spr.sprite == spr23 && data.currySkillPower < 3)
        {
            c3.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1);
        }
        else if (c3spr.sprite == spr33 && data.ItemQuantity[data.ItemNumber[2]] != 0)
        {
            c3.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else if (c3spr.sprite == spr33 && data.ItemQuantity[data.ItemNumber[2]] == 0)
        {
            c3.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1);
        }

        if (c4spr.sprite == spr14 && enemyUnit.unitName != "巨龍")
        {
            c4.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else if (c4spr.sprite == spr14 && enemyUnit.unitName == "巨龍")
        {
            c4.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1);
        }
        else if (c4spr.sprite == spr24 && data.currySkillPower >= 3)
        {
            c4.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else if (c4spr.sprite == spr24 && data.currySkillPower < 3)
        {
            c4.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1);
        }
        else if (c4spr.sprite == spr34 && data.ItemQuantity[data.ItemNumber[3]] != 0)
        {
            c4.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else if (c4spr.sprite == spr34 && data.ItemQuantity[data.ItemNumber[3]] == 0)
        {
            c4.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1);
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