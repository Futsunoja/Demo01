using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BattleState { START, CHOOSEACTION,PLAYERTURN, ENEMYTURN, ENDTRUN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    [SerializeField]
    PlayerData data;

    public AudioSource audBattle;
    public AudioClip BattleBGM, WonBGM, LoseBGM;

    public GameObject c1, c2, c3, c4;
    bool i0, i1, i2, i3, i4, i5, i6, i7, ispr1, ispr2, ispr3;
    Vector3 c = new Vector3(-2.7f, 0, 0);
    Vector3 o = new Vector3(0.4f, 0, 0);
    public Sprite spr11, spr12, spr13, spr14, spr21, spr22, spr23, spr24, spr31, spr32, spr33, spr34;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject PlayerHp, EnemyHp;
    public GameObject BattleMessage;
    public GameObject SkillItemName;
    public GameObject SkillItemEffect;
    public GameObject[] SkillPower;
    public GameObject BattleSettlement, WonSettlement, LoseSettlement, ExeSettlement, ExeLine;
    public GameObject AbilitySettlement, OldLevel, OldAtk, OldDef, OldSpd, NewLevel, NewAtk, NewDef, NewSpd, NewLevelMask, NewAtkMask, NewDefMask, NewSpdMask, ReStart, LevelUp;
    public GameObject ReStartBattle, ReturnMap, ReturnMenu;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    Unit enemyUnit;

    public Text enemyName, enemyHp, playerHp;    //開始介面文字資訊
    public Text Won, getExe, Exe;    //勝利介面文字資訊

    int playerOriLevel;  //原等級
    int PlayerOriAtk;    //原攻擊力
    int PlayerOriDef;    //原防禦力
    int PlayerOriSpd;    //原速度
    int PlayerOriExe;    //原經驗值
    int UPcount;         //強化倒數
    int POISONcount;     //毒倒數
    int POISONdamage;    //毒傷害
    bool POISONworked;   //毒傷害是否運作過
    bool EnemyIsDead;    //是否死亡
    bool Wonclicktorestart;
    bool Losechoose;

    public BattleState state;

    private void Start()
    {
        data = JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString("Playerdata"));
        data.currySkillPower = 5;

        audBattle.clip = BattleBGM;
        audBattle.Play();
        Screen.SetResolution(1280, 720, false);    //固定視窗大小

        c1.transform.localPosition = c;
        c2.transform.localPosition = c;
        c3.transform.localPosition = c;
        c4.transform.localPosition = c;

        GameObject playerGo = Instantiate(playerPrefab, playerBattleStation);

        GameObject enemyGo = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGo.GetComponent<Unit>();

        playerOriLevel = data.unitLevel;
        PlayerOriAtk = data.atk;
        PlayerOriDef = data.def;
        PlayerOriSpd = data.speed;
        PlayerOriExe = data.curryExp;
        UPcount = 0;
        POISONcount = 0;

        state = BattleState.START;
        BattleMessage.GetComponent<Text>().text = "遭遇了" + enemyUnit.unitName;
        ReSet();
        SetupBattle();
    }

    private void Update()
    {
        ChooseAction();
        WonClickToRestart();
        LoseChoose();
    }

    private void SetupBattle()    //設定戰鬥必要數據
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

        state = BattleState.CHOOSEACTION;
        ChooseAction();
    }

    private void ChooseAction()    //回合開始、選擇操作、選擇行動整合
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

    private void BS()    //回合開始_選項展開
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
            if (c2.transform.localPosition.x >= -2 && c1.transform.localPosition != Vector3.zero)
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

    private void CT()    //基本選項操作（WASD上下左右；J確定；K取消）
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
                BattleMessage.GetComponent<Text>().text = "無法逃跑\n 請選擇其他行動";
            }
        }
        if (ispr1 == false && ispr2 == true && ispr3 == false)
        {
            if (c1.transform.localPosition == o)
            {
                SkillItemName.SetActive(true);
                SkillItemEffect.SetActive(true);
                SkillItemName.GetComponent<Text>().text = "【狂擊】";
                SkillItemEffect.GetComponent<Text>().text = "消耗2點能量，使出全力攻擊敵人，對\n敵人造成巨大的傷害。";
            }
            if (c2.transform.localPosition == o)
            {
                SkillItemName.SetActive(true);
                SkillItemEffect.SetActive(true);
                SkillItemName.GetComponent<Text>().text = "【高級強化】";
                SkillItemEffect.GetComponent<Text>().text = "消耗1點能量，攻擊力與防禦力大幅提\n升三個回合。";
            }
            if (c3.transform.localPosition == o)
            {
                SkillItemName.SetActive(true);
                SkillItemEffect.SetActive(true);
                SkillItemName.GetComponent<Text>().text = "【三連擊】";
                SkillItemEffect.GetComponent<Text>().text = "消耗3點能量，使出快速的斬擊造成敵\n人三次傷害。";
            }
            if (c4.transform.localPosition == o)
            {
                SkillItemName.SetActive(true);
                SkillItemEffect.SetActive(true);
                SkillItemName.GetComponent<Text>().text = "【瀕死一擊】";
                SkillItemEffect.GetComponent<Text>().text = "消耗3點能量，面臨死亡激發出強大的\n力量進行攻擊，血量越低傷害越高。";
            }
        }
        if (ispr1 == false && ispr2 == false && ispr3 == true)
        {
            if (c1.transform.localPosition == o)
            {
                SkillItemName.SetActive(true);
                SkillItemEffect.SetActive(true);
                SkillItemName.GetComponent<Text>().text = "【紅藥水】　　　　　　擁有"+data.RedPoison+"個";
                SkillItemEffect.GetComponent<Text>().text = "血量恢復50點。";
            }
            if (c2.transform.localPosition == o)
            {
                SkillItemName.SetActive(true);
                SkillItemEffect.SetActive(true);
                SkillItemName.GetComponent<Text>().text = "【藍藥水】　　　　　　擁有"+data.BluePoison+"個";
                SkillItemEffect.GetComponent<Text>().text = "能量恢復1點。";
            }
            if (c3.transform.localPosition == o)
            {
                SkillItemName.SetActive(true);
                SkillItemEffect.SetActive(true);
                SkillItemName.GetComponent<Text>().text = "【秘藥】　　　　　　　擁有" + data.BuffPoison + "個";
                SkillItemEffect.GetComponent<Text>().text = "攻擊力與防禦力大幅提升。";
            }
            if (c4.transform.localPosition == o)
            {
                SkillItemName.SetActive(true);
                SkillItemEffect.SetActive(true);
                SkillItemName.GetComponent<Text>().text = "【萬能藥】　　　　　　擁有" + data.UndebuffPoison + "個";
                SkillItemEffect.GetComponent<Text>().text = "消除所有不良狀態。";
            }
        }
    }

    private void CAD()    //選擇行動
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
                        if (state == BattleState.CHOOSEACTION)
                        {
                            state = BattleState.PLAYERTURN;
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
                        if (state == BattleState.CHOOSEACTION)
                        {
                            if (enemyUnit.unitName != "巨龍")
                            {
                                ispr1 = false;
                                i6 = true;
                                i0 = true;
                                state = BattleState.PLAYERTURN;
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
                    state = BattleState.PLAYERTURN;
                    if (c1.transform.localPosition == o && data.currySkillPower >= 2)
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
                    else if (c1.transform.localPosition == o && data.currySkillPower < 3)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        i6 = false;
                        i0 = false;
                    }
                    if (c2.transform.localPosition == o && data.currySkillPower >= 1)
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
                    else if (c2.transform.localPosition == o && data.currySkillPower < 1)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        i6 = false;
                        i0 = false;
                    }
                    if (c3.transform.localPosition == o && data.currySkillPower >= 3)
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
                    else if (c3.transform.localPosition == o && data.currySkillPower < 3)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        i6 = false;
                        i0 = false;
                    }
                    if (c4.transform.localPosition == o && data.currySkillPower >= 3)
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
                    else if (c4.transform.localPosition == o && data.currySkillPower < 3)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        i6 = false;
                        i0 = false;
                    }
                }
                else if (ispr3 == true)    //道具選項
                {
                    i6 = true;
                    state = BattleState.PLAYERTURN;
                    if (c1.transform.localPosition == o && data.RedPoison > 0)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        StartCoroutine(Item(c1.GetComponent<SpriteRenderer>().sprite.name));
                        SkillItemName.SetActive(false);
                        SkillItemEffect.SetActive(false);
                        i0 = true;
                    }
                    else if (c1.transform.localPosition == o && data.RedPoison <= 0)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        i6 = false;
                        i0 = false;
                    }
                    if (c2.transform.localPosition == o && data.BluePoison > 0)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        StartCoroutine(Item(c2.GetComponent<SpriteRenderer>().sprite.name));
                        SkillItemName.SetActive(false);
                        SkillItemEffect.SetActive(false);
                        i0 = true;
                    }
                    else if (c2.transform.localPosition == o && data.BluePoison <= 0)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        i6 = false;
                        i0 = false;
                    }
                    if (c3.transform.localPosition == o && data.BuffPoison > 0)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        StartCoroutine(Item(c3.GetComponent<SpriteRenderer>().sprite.name));
                        SkillItemName.SetActive(false);
                        SkillItemEffect.SetActive(false);
                        i0 = true;
                    }
                    else if (c3.transform.localPosition == o && data.BuffPoison <= 0)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        i6 = false;
                        i0 = false;
                    }
                    if (c4.transform.localPosition == o && data.UndebuffPoison > 0)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        StartCoroutine(Item(c4.GetComponent<SpriteRenderer>().sprite.name));
                        SkillItemName.SetActive(false);
                        SkillItemEffect.SetActive(false);
                        i0 = true;
                    }
                    else if (c4.transform.localPosition == o && data.UndebuffPoison <= 0)
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
                if (c2.transform.localPosition.x >= -2 && c1.transform.localPosition != Vector3.zero)
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

    private void ReSet()    //恢復初始設定
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

        POISONworked = true;
        ReStartBattle.GetComponent<Image>().color = Color.gray;
    }

    IEnumerator EnemyTrun(string playerSKILLNAME)    //敵人攻擊
    {
        yield return new WaitForSeconds(1f);
        BattleMessage.SetActive(true);
        BattleMessage.GetComponent<Text>().text = enemyUnit.unitName + "施展了攻擊";

        SoundManager.SoundInstance.SoundEnemyAttack();

        int hit = enemyUnit.atk - data.def;
        if (hit <= 0)
        {
            hit = 0;
        }
        bool isDead;
        data.currentHp -= hit;
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
        BattleMessage.GetComponent<Text>().text = data.unitName + "受到了" + hit + "點傷害";

        if (isDead == true)
        {
            yield return new WaitForSeconds(1f);
            BattleMessage.GetComponent<Text>().text = data.unitName + "倒下了";
            state = BattleState.LOST;
            StartCoroutine(EndBattle());
        }
        else
        {
            if (data.speed >= enemyUnit.speed || playerSKILLNAME == null)
            {
                state = BattleState.ENDTRUN;
                StartCoroutine(EndTrun());
            }
            else
            {
                state = BattleState.PLAYERTURN;
                StartCoroutine(AttackSkill(playerSKILLNAME));
            }
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

            data.atk += 15;
            data.def += 10;
            UPcount = 3;

            yield return new WaitForSeconds(1f);
            BattleMessage.GetComponent<Text>().text = data.unitName + "的攻擊、防禦大幅提升";

            if (data.speed >= enemyUnit.speed)
            {
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTrun(null));
            }
            else
            {
                state = BattleState.ENDTRUN;
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
            if(data.currentHp/data.maxHp<=1 && data.currentHp / data.maxHp > 0.66)
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
                hit = data.atk * 4f - enemyUnit.def;
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

            state = BattleState.ENEMYTURN;
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
        if (ITEMNAME == "紅藥水")
        {
            data.RedPoison -= 1;

            data.currentHp += 50;
            data.currentHp = Mathf.Clamp(data.currentHp, 0, data.maxHp);
            PlayerHp.GetComponent<Image>().fillAmount = data.currentHp / data.maxHp;
            playerHp.text = data.currentHp.ToString() + "/" + data.maxHp.ToString();

            yield return new WaitForSeconds(1f);
            BattleMessage.GetComponent<Text>().text = data.unitName + "恢復了" + 50 + "點血量";

            StartCoroutine(EnemyTrun(null));
        }
        #endregion

        #region 02_藍藥水
        if (ITEMNAME == "藍藥水")
        {
            data.BluePoison -= 1;

            SkillPowerExpend(-1);

            yield return new WaitForSeconds(1f);
            BattleMessage.GetComponent<Text>().text = data.unitName + "恢復了" + 1 + "點能量";

            StartCoroutine(EnemyTrun(null));
        }
        #endregion

        #region 03_秘藥
        if (ITEMNAME == "秘藥")
        {
            data.BuffPoison -= 1;

            data.atk += 15;
            data.def += 10;
            UPcount = 3;

            yield return new WaitForSeconds(1f);
            BattleMessage.GetComponent<Text>().text = data.unitName + "的攻擊、防禦大幅提升";

            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTrun(null));
        }
        #endregion

        #region 04_秘藥
        if (ITEMNAME == "萬能藥")
        {
            data.UndebuffPoison -= 1;

            yield return new WaitForSeconds(1f);
            SoundManager.SoundInstance.SoundBuff();
            BattleMessage.GetComponent<Text>().text = data.unitName + "的負面狀態消失了";

            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTrun(null));
        }
        #endregion
    }

    IEnumerator Run()    //玩家逃跑
    {
        yield return new WaitForSeconds(1f);
        print("逃跑");
        ReSet();
    }

    IEnumerator EndTrun()    //此回合結束
    {
        UPCOUNT();
        if (POISONcount > 0 && POISONworked == false)    //有中毒時，計算中毒傷害
        {
            StartCoroutine(POISONCOUNT());
        }
        if (POISONworked == true)
        {
            yield return new WaitForSeconds(1f);
            ReSet();
            print("count" + UPcount);
            print("POISONCOUND" + POISONcount);
            state = BattleState.CHOOSEACTION;
        }
    }

    IEnumerator EndBattle()    //戰鬥結束
    {
        yield return new WaitForSeconds(1.5f);
        BattleSettlement.SetActive(true);

        data.atk = PlayerOriAtk;    //攻擊力回歸初始值
        data.def = PlayerOriDef;    //防禦力回歸初始值

        if (state == BattleState.WON)    //勝利
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
        
        if (state == BattleState.LOST)    //戰敗
        {
            audBattle.clip = LoseBGM;
            audBattle.Play();
            LoseSettlement.SetActive(true);
            Losechoose = true;
        }
    }

    public void WonClickToRestart()    //勝利後點擊重啟戰鬥
    {
        if (Wonclicktorestart == true)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                Invoke("DelayRestartGame", 1f);
            }
        }
    }
    public void LoseChoose()           //死亡後選項操作
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

    public void DelayRestartGame()    //回戰鬥畫面
    {
        SceneManager.LoadScene("戰鬥畫面");
    }
    public void DelayReturnMap()      //回大地圖
    {
        SceneManager.LoadScene("大地圖");
    }
    public void DelayReturnMenu()     //回主畫面
    {
        SceneManager.LoadScene("主畫面");
    }

    private void EnemyDamageHpSettle()     //敵人血條扣除傷害結算
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
    private void PlayerDamageHpSettle()    //玩家血條扣除傷害結算
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
            state = BattleState.WON;
            StartCoroutine(EndBattle());
        }
        else
        {
            if (data.speed >= enemyUnit.speed)
            {
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTrun(null));
            }
            else
            {
                state = BattleState.ENDTRUN;
                StartCoroutine(EndTrun());
            }
        }
    }

    private void SkillPowerExpend(int h)    //消耗能量點數後的能量顯示
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

    private void CanNatUse()    //無法使用技能或道具
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
        else if (c1spr.sprite == spr31 && data.RedPoison != 0)
        {
            c1.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else if (c1spr.sprite == spr31 && data.RedPoison == 0)
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
        else if (c2spr.sprite == spr32 && data.BluePoison != 0)
        {
            c2.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else if (c2spr.sprite == spr32 && data.BluePoison == 0)
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
        else if (c3spr.sprite == spr33 && data.BuffPoison != 0)
        {
            c3.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else if (c3spr.sprite == spr33 && data.BuffPoison == 0)
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
        else if (c4spr.sprite == spr34 && data.UndebuffPoison != 0)
        {
            c4.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else if (c4spr.sprite == spr34 && data.UndebuffPoison == 0)
        {
            c4.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1);
        }
    }

    private void UPCOUNT()    //高級強化倒數
    {
        if (UPcount > 0)
        {
            UPcount--;
        }
        else if(UPcount == 0)
        {
            data.atk = PlayerOriAtk;
            data.def = PlayerOriDef;
        }
    }

    IEnumerator POISONCOUNT()    //施毒傷害及倒數
    {
        if (POISONcount > 0)
        {
            yield return new WaitForSeconds(1f);
            BattleMessage.GetComponent<Text>().text = enemyUnit.unitName + "毒發了";
            EnemyIsDead = enemyUnit.TakeDamage(POISONdamage);
            EnemyDamageHpSettle();

            yield return new WaitForSeconds(1f);
            BattleMessage.GetComponent<Text>().text = enemyUnit.unitName + "受到了" + POISONdamage + "點傷害";
            POISONworked = true;

            if (EnemyIsDead)
            {
                yield return new WaitForSeconds(1f);
                BattleMessage.GetComponent<Text>().text = enemyUnit.unitName + "倒下了";
                state = BattleState.WON;
                EndBattle();
            }
            else
            {
                POISONcount--;
                StartCoroutine(EndTrun());
            }
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