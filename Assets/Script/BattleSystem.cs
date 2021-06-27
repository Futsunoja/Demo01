using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BattleState { START, CHOOSEACTION,PLAYERTURN, ENEMYTURN, ENDTRUN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
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
    public GameObject AbilitySettlement, OldLevel, OldAtk, OldDef, NewLevel, NewAtk, NewDef, NewLevelMask, NewAtkMask, NewDefMask, ReStart, LevelUp;
    public GameObject ReStartBattle, ReturnMap, ReturnMenu;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    Unit playerUnit;
    Unit enemyUnit;

    public Text enemyName, enemyHp, playerHp;    //開始介面文字資訊
    public Text Won, getExe, Exe;    //勝利介面文字資訊

    int PlayerOriAtk;    //原攻擊力
    int PlayerOriDef;    //原防禦力
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
        audBattle.clip = BattleBGM;
        audBattle.Play();
        Screen.SetResolution(1280, 720, false);    //固定視窗大小

        c1.transform.localPosition = c;
        c2.transform.localPosition = c;
        c3.transform.localPosition = c;
        c4.transform.localPosition = c;

        GameObject playerGo = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGo.GetComponent<Unit>();
        GameObject enemyGo = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGo.GetComponent<Unit>();

        PlayerOriAtk = playerUnit.atk;
        PlayerOriDef = playerUnit.def;
        PlayerOriExe = playerUnit.curryExp;
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
        playerHp.text = playerUnit.currentHp.ToString() + "/" + playerUnit.maxHp.ToString();
        enemyHp.text = enemyUnit.currentHp.ToString() + "/" + enemyUnit.maxHp.ToString();

        PlayerHp.GetComponent<Image>().fillAmount = playerUnit.currentHp / playerUnit.maxHp;
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
                    SkillPower[playerUnit.currySkillPower].SetActive(false);
                    playerUnit.currySkillPower++;
                    if (playerUnit.currySkillPower >= playerUnit.maxSkillPower)
                    {
                        playerUnit.currySkillPower = playerUnit.maxSkillPower;
                    }
                    SkillPower[playerUnit.currySkillPower].SetActive(true);
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
                SkillItemName.GetComponent<Text>().text = "【紅藥水】　　　　　　擁有"+playerUnit.RedPoison+"個";
                SkillItemEffect.GetComponent<Text>().text = "血量恢復50點。";
            }
            if (c2.transform.localPosition == o)
            {
                SkillItemName.SetActive(true);
                SkillItemEffect.SetActive(true);
                SkillItemName.GetComponent<Text>().text = "【藍藥水】　　　　　　擁有"+playerUnit.BluePoison+"個";
                SkillItemEffect.GetComponent<Text>().text = "能量恢復1點。";
            }
            if (c3.transform.localPosition == o)
            {
                SkillItemName.SetActive(true);
                SkillItemEffect.SetActive(true);
                SkillItemName.GetComponent<Text>().text = "【秘藥】　　　　　　　擁有" + playerUnit.BuffPoison + "個";
                SkillItemEffect.GetComponent<Text>().text = "攻擊力與防禦力大幅提升。";
            }
            if (c4.transform.localPosition == o)
            {
                SkillItemName.SetActive(true);
                SkillItemEffect.SetActive(true);
                SkillItemName.GetComponent<Text>().text = "【萬能藥】　　　　　　擁有" + playerUnit.UndebuffPoison + "個";
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
                            StartCoroutine(AttackSkill(c1.GetComponent<SpriteRenderer>().sprite.name));
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
                    if (c1.transform.localPosition == o && playerUnit.currySkillPower >= 2)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        StartCoroutine(AttackSkill(c1.GetComponent<SpriteRenderer>().sprite.name));
                        SkillItemName.SetActive(false);
                        SkillItemEffect.SetActive(false);
                        i0 = true;
                    }
                    else if (c1.transform.localPosition == o && playerUnit.currySkillPower < 3)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        i6 = false;
                        i0 = false;
                    }
                    if (c2.transform.localPosition == o && playerUnit.currySkillPower >= 1)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        StartCoroutine(AttackSkill(c2.GetComponent<SpriteRenderer>().sprite.name));
                        SkillItemName.SetActive(false);
                        SkillItemEffect.SetActive(false);
                        i0 = true;
                    }
                    else if (c2.transform.localPosition == o && playerUnit.currySkillPower < 1)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        i6 = false;
                        i0 = false;
                    }
                    if (c3.transform.localPosition == o && playerUnit.currySkillPower >= 3)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        StartCoroutine(AttackSkill(c3.GetComponent<SpriteRenderer>().sprite.name));
                        SkillItemName.SetActive(false);
                        SkillItemEffect.SetActive(false);
                        i0 = true;
                    }
                    else if (c3.transform.localPosition == o && playerUnit.currySkillPower < 3)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        i6 = false;
                        i0 = false;
                    }
                    if (c4.transform.localPosition == o && playerUnit.currySkillPower >= 3)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        StartCoroutine(AttackSkill(c4.GetComponent<SpriteRenderer>().sprite.name));
                        SkillItemName.SetActive(false);
                        SkillItemEffect.SetActive(false);
                        i0 = true;
                    }
                    else if (c4.transform.localPosition == o && playerUnit.currySkillPower < 3)
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
                    if (c1.transform.localPosition == o && playerUnit.RedPoison > 0)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        StartCoroutine(Item(c1.GetComponent<SpriteRenderer>().sprite.name));
                        SkillItemName.SetActive(false);
                        SkillItemEffect.SetActive(false);
                        i0 = true;
                    }
                    else if (c1.transform.localPosition == o && playerUnit.RedPoison <= 0)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        i6 = false;
                        i0 = false;
                    }
                    if (c2.transform.localPosition == o && playerUnit.BluePoison > 0)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        StartCoroutine(Item(c2.GetComponent<SpriteRenderer>().sprite.name));
                        SkillItemName.SetActive(false);
                        SkillItemEffect.SetActive(false);
                        i0 = true;
                    }
                    else if (c2.transform.localPosition == o && playerUnit.BluePoison <= 0)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        i6 = false;
                        i0 = false;
                    }
                    if (c3.transform.localPosition == o && playerUnit.BuffPoison > 0)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        StartCoroutine(Item(c3.GetComponent<SpriteRenderer>().sprite.name));
                        SkillItemName.SetActive(false);
                        SkillItemEffect.SetActive(false);
                        i0 = true;
                    }
                    else if (c3.transform.localPosition == o && playerUnit.BuffPoison <= 0)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        i6 = false;
                        i0 = false;
                    }
                    if (c4.transform.localPosition == o && playerUnit.UndebuffPoison > 0)
                    {
                        SoundManager.SoundInstance.SoundEnterHit();
                        StartCoroutine(Item(c4.GetComponent<SpriteRenderer>().sprite.name));
                        SkillItemName.SetActive(false);
                        SkillItemEffect.SetActive(false);
                        i0 = true;
                    }
                    else if (c4.transform.localPosition == o && playerUnit.UndebuffPoison <= 0)
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

    IEnumerator EnemyTrun()    //敵人攻擊
    {
        yield return new WaitForSeconds(1f);

        BattleMessage.GetComponent<Text>().text = enemyUnit.unitName + "施展了攻擊";

        SoundManager.SoundInstance.SoundEnemyAttack();

        int hit = enemyUnit.atk - playerUnit.def;
        if (hit <= 0)
        {
            hit = 0;
        }
        bool isDead = playerUnit.TakeDamage(hit);
        PlayerDamageHpSettle();

        yield return new WaitForSeconds(1f);
        BattleMessage.GetComponent<Text>().text = playerUnit.unitName + "受到了" + hit + "點傷害";

        if (isDead == true)
        {
            yield return new WaitForSeconds(1f);
            BattleMessage.GetComponent<Text>().text = playerUnit.unitName + "倒下了";
            state = BattleState.LOST;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.ENDTRUN;
            StartCoroutine(EndTrun());
        }
    }

    IEnumerator AttackSkill(string SKILLNAME)    //玩家攻擊
    {
        yield return new WaitForSeconds(1f);
        BattleMessage.SetActive(true);
        BattleMessage.GetComponent<Text>().text = playerUnit.unitName + "施展了" + SKILLNAME;

        #region 00_普通攻擊
        if (SKILLNAME == "攻擊")
        {
            SoundManager.SoundInstance.SoundAttack();
            int hit = playerUnit.atk - enemyUnit.def;
            hit = Mathf.Clamp(hit, 0, playerUnit.atk);
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

            int hit = 2 * playerUnit.atk - enemyUnit.def;
            hit = Mathf.Clamp(hit, 0, 2 * playerUnit.atk);
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

            playerUnit.atk += 15;
            playerUnit.def += 10;
            UPcount = 3;

            yield return new WaitForSeconds(1f);
            BattleMessage.GetComponent<Text>().text = playerUnit.unitName + "的攻擊、防禦大幅提升";

            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTrun());
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
                float k = Random.Range(playerUnit.atk, playerUnit.atk * 1.5f) - enemyUnit.def;
                k = Mathf.Clamp(k, 0, playerUnit.atk * 1.5f);
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
            if(playerUnit.currentHp/playerUnit.maxHp<=1 && playerUnit.currentHp / playerUnit.maxHp > 0.66)
            {
                hit = playerUnit.atk * 2.3f - enemyUnit.def;
                hit = Mathf.Clamp(hit, 0, playerUnit.atk * 2.3f);
                EnemyIsDead = enemyUnit.TakeDamage((int)hit);
                EnemyDamageHpSettle();

                yield return new WaitForSeconds(1f);
                BattleMessage.GetComponent<Text>().text = enemyUnit.unitName + "受到了" + (int)hit + "點傷害";
            }
            else if(playerUnit.currentHp / playerUnit.maxHp <= 0.66 && playerUnit.currentHp / playerUnit.maxHp > 0.33f)
            {
                hit = playerUnit.atk * 2.6f - enemyUnit.def;
                hit = Mathf.Clamp(hit, 0, playerUnit.atk * 2.6f);
                EnemyIsDead = enemyUnit.TakeDamage((int)hit);
                EnemyDamageHpSettle();

                yield return new WaitForSeconds(1f);
                BattleMessage.GetComponent<Text>().text = enemyUnit.unitName + "受到了" + (int)hit + "點傷害";
            }
            else if (playerUnit.currentHp / playerUnit.maxHp <= 0.33 && playerUnit.currentHp / playerUnit.maxHp > 0.1f)
            {
                hit = playerUnit.atk * 3f - enemyUnit.def;
                hit = Mathf.Clamp(hit, 0, playerUnit.atk * 3f);
                EnemyIsDead = enemyUnit.TakeDamage((int)hit);
                EnemyDamageHpSettle();

                yield return new WaitForSeconds(1f);
                BattleMessage.GetComponent<Text>().text = enemyUnit.unitName + "受到了" + (int)hit + "點傷害";
            }
            else if (playerUnit.currentHp / playerUnit.maxHp <= 0.1 && playerUnit.currentHp / playerUnit.maxHp > 0)
            {
                hit = playerUnit.atk * 4f - enemyUnit.def;
                hit = Mathf.Clamp(hit, 0, playerUnit.atk * 4f);
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
            StartCoroutine(EnemyTrun());
        }
        #endregion
    }

    IEnumerator Item(string ITEMNAME)    //使用道具
    {
        yield return new WaitForSeconds(1f);
        BattleMessage.SetActive(true);
        BattleMessage.GetComponent<Text>().text = playerUnit.unitName + "使用了" + ITEMNAME;
        SoundManager.SoundInstance.SoundDrink();

        #region 01_紅藥水
        if (ITEMNAME == "紅藥水")
        {
            playerUnit.RedPoison -= 1;

            playerUnit.currentHp += 50;
            playerUnit.currentHp = Mathf.Clamp(playerUnit.currentHp, 0, playerUnit.maxHp);
            PlayerHp.GetComponent<Image>().fillAmount = playerUnit.currentHp / playerUnit.maxHp;
            playerHp.text = playerUnit.currentHp.ToString() + "/" + playerUnit.maxHp.ToString();

            yield return new WaitForSeconds(1f);
            BattleMessage.GetComponent<Text>().text = playerUnit.unitName + "恢復了" + 50 + "點血量";

            StartCoroutine(EnemyIsDeadOrAlive());
        }
        #endregion

        #region 02_藍藥水
        if (ITEMNAME == "藍藥水")
        {
            playerUnit.BluePoison -= 1;

            SkillPowerExpend(-1);

            yield return new WaitForSeconds(1f);
            BattleMessage.GetComponent<Text>().text = playerUnit.unitName + "恢復了" + 1 + "點能量";

            StartCoroutine(EnemyIsDeadOrAlive());
        }
        #endregion

        #region 03_秘藥
        if (ITEMNAME == "秘藥")
        {
            playerUnit.BuffPoison -= 1;

            playerUnit.atk += 15;
            playerUnit.def += 10;
            UPcount = 3;

            yield return new WaitForSeconds(1f);
            BattleMessage.GetComponent<Text>().text = playerUnit.unitName + "的攻擊、防禦大幅提升";

            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTrun());
        }
        #endregion

        #region 04_秘藥
        if (ITEMNAME == "萬能藥")
        {
            playerUnit.UndebuffPoison -= 1;

            yield return new WaitForSeconds(1f);
            SoundManager.SoundInstance.SoundBuff();
            BattleMessage.GetComponent<Text>().text = playerUnit.unitName + "的負面狀態消失了";

            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTrun());
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
        if (POISONcount > 0 && POISONworked == false)
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

        if (state == BattleState.WON)    //勝利
        {
            audBattle.clip = WonBGM;
            audBattle.Play();
            WonSettlement.SetActive(true);
            Won.GetComponent<Text>().text = "你打倒了" + enemyUnit.unitName;
            yield return new WaitForSeconds(1f);

            ExeSettlement.SetActive(true);
            getExe.GetComponent<Text>().text = "獲得了" + enemyUnit.exe + "點經驗值";
            Exe.GetComponent<Text>().text = playerUnit.curryExp + "/" + playerUnit.maxExp[playerUnit.unitLevel];
            ExeLine.GetComponent<Image>().fillAmount = (float)playerUnit.curryExp / playerUnit.maxExp[playerUnit.unitLevel];

            int PlayerFinalExe = PlayerOriExe + enemyUnit.exe;
            bool IsLevelUp = false;
            while (playerUnit.curryExp != PlayerFinalExe)
            {
                playerUnit.curryExp += 1;
                yield return new WaitForSeconds(0.033f);
                Exe.GetComponent<Text>().text = playerUnit.curryExp + "/" + playerUnit.maxExp[playerUnit.unitLevel];
                ExeLine.GetComponent<Image>().fillAmount = (float)playerUnit.curryExp / playerUnit.maxExp[playerUnit.unitLevel];

                if (playerUnit.curryExp == playerUnit.maxExp[playerUnit.unitLevel])
                {
                    IsLevelUp = true;
                    LevelUp.SetActive(true);
                    yield return new WaitForSeconds(1f);
                    playerUnit.curryExp = 0;
                    PlayerFinalExe -= playerUnit.maxExp[playerUnit.unitLevel];
                    playerUnit.unitLevel += 1;
                }
            }
            yield return new WaitForSeconds(1f);

            AbilitySettlement.SetActive(true);
            if (IsLevelUp == true)
            {
                OldLevel.GetComponent<Text>().text = (playerUnit.unitLevel - 1).ToString();
                NewLevel.GetComponent<Text>().text = playerUnit.unitLevel.ToString();
                OldAtk.GetComponent<Text>().text = PlayerOriAtk.ToString();
                NewAtk.GetComponent<Text>().text = (playerUnit.unitLevel * 5).ToString();
                OldDef.GetComponent<Text>().text = PlayerOriDef.ToString();
                NewDef.GetComponent<Text>().text = (playerUnit.unitLevel * 4).ToString();
                yield return new WaitForSeconds(1f);
                while (NewLevelMask.GetComponent<Image>().fillAmount != 1)
                {
                    NewLevelMask.GetComponent<Image>().fillAmount += 0.02f;
                    NewAtkMask.GetComponent<Image>().fillAmount += 0.02f;
                    NewDefMask.GetComponent<Image>().fillAmount += 0.02f;
                    yield return new WaitForSeconds(0.033f);
                }
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

    public void WonClickToRestart()
    {
        if (Wonclicktorestart == true)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                Invoke("DelayRestartGame", 1f);
            }
        }
    }
    public void LoseChoose()
    {
        if (Losechoose == true)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                print("S");
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
                print("W");
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

    public void DelayRestartGame()
    {
        SceneManager.LoadScene("戰鬥畫面");
    }
    public void DelayReturnMap()
    {
        SceneManager.LoadScene("大地圖");
    }
    public void DelayReturnMenu()
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
        if (playerUnit.currentHp <= 0)
        {
            playerUnit.currentHp = 0;
        }
        PlayerHp.GetComponent<Image>().fillAmount = playerUnit.currentHp / playerUnit.maxHp;
        if (playerUnit.currentHp <= 0)
        {
            playerHp.text = 0 + "/" + playerUnit.maxHp.ToString();
        }
        else
        {
            playerHp.text = playerUnit.currentHp.ToString() + "/" + playerUnit.maxHp.ToString();
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
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTrun());
        }
    }

    private void SkillPowerExpend(int h)    //消耗能量點數後的能量顯示
    {
        playerUnit.currySkillPower -= h;
        int i = playerUnit.currySkillPower;

        for (int j = 0; j <= playerUnit.maxSkillPower; j++)
        {
            SkillPower[j].SetActive(false);
        }
        if (i <= 0)
        {
            i = 0;
        }
        else if (i >= playerUnit.maxSkillPower)
        {
            i = playerUnit.maxSkillPower;
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
        else if (c1spr.sprite == spr21 && playerUnit.currySkillPower >= 2)
        {
            c1.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else if (c1spr.sprite == spr21 && playerUnit.currySkillPower < 2)
        {
            c1.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1);
        }
        else if (c1spr.sprite == spr31 && playerUnit.RedPoison != 0)
        {
            c1.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else if (c1spr.sprite == spr31 && playerUnit.RedPoison == 0)
        {
            c1.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1);
        }

        if (c2spr.sprite == spr12)
        {
            c2.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else if (c2spr.sprite == spr22 && playerUnit.currySkillPower >= 1)
        {
            c2.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else if (c2spr.sprite == spr22 && playerUnit.currySkillPower < 1)
        {
            c2.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1);
        }
        else if (c2spr.sprite == spr32 && playerUnit.BluePoison != 0)
        {
            c2.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else if (c2spr.sprite == spr32 && playerUnit.BluePoison == 0)
        {
            c2.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1);
        }

        if (c3spr.sprite == spr13)
        {
            c3.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else if (c3spr.sprite == spr23 && playerUnit.currySkillPower >= 3)
        {
            c3.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else if (c3spr.sprite == spr23 && playerUnit.currySkillPower < 3)
        {
            c3.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1);
        }
        else if (c3spr.sprite == spr33 && playerUnit.BuffPoison != 0)
        {
            c3.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else if (c3spr.sprite == spr33 && playerUnit.BuffPoison == 0)
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
        else if (c4spr.sprite == spr24 && playerUnit.currySkillPower >= 3)
        {
            c4.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else if (c4spr.sprite == spr24 && playerUnit.currySkillPower < 3)
        {
            c4.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1);
        }
        else if (c4spr.sprite == spr34 && playerUnit.UndebuffPoison != 0)
        {
            c4.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else if (c4spr.sprite == spr34 && playerUnit.UndebuffPoison == 0)
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
            playerUnit.atk = PlayerOriAtk;
            playerUnit.def = PlayerOriDef;
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
}