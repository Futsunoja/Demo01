using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Sprite[] BackpackItemSprite;
    public Sprite[] ShowBackpackItemSprite;
    public Sprite[] BattleItemSprite;
    string[] BackpackItemType = { "補品", "材料", "任務" };

    public Sprite BackpackItem;
    public Sprite ShowBackpackItem;
    public string ItemType;
    public string ItemIntro;

    public void Item(string itemName)
    {
        if (itemName == "初級紅藥水")
        {
            BackpackItem = BackpackItemSprite[0];
            ShowBackpackItem = ShowBackpackItemSprite[0];
            ItemType = BackpackItemType[0];
            ItemIntro = "HP恢復20%";
        }
        if (itemName == "中級紅藥水")
        {
            BackpackItem = BackpackItemSprite[1];
            ShowBackpackItem = ShowBackpackItemSprite[1];
            ItemType = BackpackItemType[0];
            ItemIntro = "HP恢復40%";
        }
        if (itemName == "高級紅藥水")
        {
            BackpackItem = BackpackItemSprite[2];
            ShowBackpackItem = ShowBackpackItemSprite[2];
            ItemType = BackpackItemType[0];
            ItemIntro = "HP恢復60%";
        }
        if (itemName == "傳說紅藥水")
        {
            BackpackItem = BackpackItemSprite[3];
            ShowBackpackItem = ShowBackpackItemSprite[3];
            ItemType = BackpackItemType[0];
            ItemIntro = "HP恢復100%";
        }
        if (itemName == "初級藍藥水")
        {
            BackpackItem = BackpackItemSprite[4];
            ShowBackpackItem = ShowBackpackItemSprite[4];
            ItemType = BackpackItemType[0];
            ItemIntro = "能量恢復1點";
        }
        if (itemName == "中級藍藥水")
        {
            BackpackItem = BackpackItemSprite[5];
            ShowBackpackItem = ShowBackpackItemSprite[5];
            ItemType = BackpackItemType[0];
            ItemIntro = "能量恢復2點";
        }
        if (itemName == "高級藍藥水")
        {
            BackpackItem = BackpackItemSprite[6];
            ShowBackpackItem = ShowBackpackItemSprite[6];
            ItemType = BackpackItemType[0];
            ItemIntro = "能量恢復3點";
        }
        if (itemName == "傳說藍藥水")
        {
            BackpackItem = BackpackItemSprite[7];
            ShowBackpackItem = ShowBackpackItemSprite[7];
            ItemType = BackpackItemType[0];
            ItemIntro = "能量恢復5點";
        }
        if (itemName == "初級黃藥水")
        {
            BackpackItem = BackpackItemSprite[8];
            ShowBackpackItem = ShowBackpackItemSprite[8];
            ItemType = BackpackItemType[0];
            ItemIntro = "HP恢復20%\n能量恢復1點";
        }
        if (itemName == "中級黃藥水")
        {
            BackpackItem = BackpackItemSprite[9];
            ShowBackpackItem = ShowBackpackItemSprite[9];
            ItemType = BackpackItemType[0];
            ItemIntro = "HP恢復40%\n能量恢復2點";
        }
        if (itemName == "高級黃藥水")
        {
            BackpackItem = BackpackItemSprite[10];
            ShowBackpackItem = ShowBackpackItemSprite[10];
            ItemType = BackpackItemType[0];
            ItemIntro = "HP恢復60%\n能量恢復3點";
        }
        if (itemName == "傳說黃藥水")
        {
            BackpackItem = BackpackItemSprite[11];
            ShowBackpackItem = ShowBackpackItemSprite[11];
            ItemType = BackpackItemType[0];
            ItemIntro = "HP恢復100%\n能量恢復5點";
        }
        if (itemName == "初級秘藥")
        {
            BackpackItem = BackpackItemSprite[12];
            ShowBackpackItem = ShowBackpackItemSprite[12];
            ItemType = BackpackItemType[0];
            ItemIntro = "能力值小量提升";
        }
        if (itemName == "中級秘藥")
        {
            BackpackItem = BackpackItemSprite[13];
            ShowBackpackItem = ShowBackpackItemSprite[13];
            ItemType = BackpackItemType[0];
            ItemIntro = "能力值中量提升";
        }
        if (itemName == "高級秘藥")
        {
            BackpackItem = BackpackItemSprite[14];
            ShowBackpackItem = ShowBackpackItemSprite[14];
            ItemType = BackpackItemType[0];
            ItemIntro = "能力值大量提升";
        }
        if (itemName == "傳說秘藥")
        {
            BackpackItem = BackpackItemSprite[15];
            ShowBackpackItem = ShowBackpackItemSprite[15];
            ItemType = BackpackItemType[0];
            ItemIntro = "能力值超大量提升";
        }
        if (itemName == "萬能草")
        {
            BackpackItem = BackpackItemSprite[16];
            ShowBackpackItem = ShowBackpackItemSprite[16];
            ItemType = BackpackItemType[0];
            ItemIntro = "消除所有異常狀態";
        }
        if (itemName == "初級紅藥草")
        {
            BackpackItem = BackpackItemSprite[50];
            ShowBackpackItem = ShowBackpackItemSprite[50];
            ItemType = BackpackItemType[1];
            ItemIntro = "";
        }
        if (itemName == "中級紅藥草")
        {
            BackpackItem = BackpackItemSprite[51];
            ShowBackpackItem = ShowBackpackItemSprite[51];
            ItemType = BackpackItemType[1];
            ItemIntro = "";
        }
        if (itemName == "高級紅藥草")
        {
            BackpackItem = BackpackItemSprite[52];
            ShowBackpackItem = ShowBackpackItemSprite[52];
            ItemType = BackpackItemType[1];
            ItemIntro = "";
        }
        if (itemName == "傳說紅藥草")
        {
            BackpackItem = BackpackItemSprite[53];
            ShowBackpackItem = ShowBackpackItemSprite[53];
            ItemType = BackpackItemType[1];
            ItemIntro = "";
        }
        if (itemName == "初級藍藥草")
        {
            BackpackItem = BackpackItemSprite[54];
            ShowBackpackItem = ShowBackpackItemSprite[54];
            ItemType = BackpackItemType[1];
            ItemIntro = "";
        }
        if (itemName == "中級藍藥草")
        {
            BackpackItem = BackpackItemSprite[55];
            ShowBackpackItem = ShowBackpackItemSprite[55];
            ItemType = BackpackItemType[1];
            ItemIntro = "";
        }
        if (itemName == "高級藍藥草")
        {
            BackpackItem = BackpackItemSprite[56];
            ShowBackpackItem = ShowBackpackItemSprite[56];
            ItemType = BackpackItemType[1];
            ItemIntro = "";
        }
        if (itemName == "傳說藍藥草")
        {
            BackpackItem = BackpackItemSprite[57];
            ShowBackpackItem = ShowBackpackItemSprite[57];
            ItemType = BackpackItemType[1];
            ItemIntro = "";
        }
        if (itemName == "初級黃藥草")
        {
            BackpackItem = BackpackItemSprite[58];
            ShowBackpackItem = ShowBackpackItemSprite[58];
            ItemType = BackpackItemType[1];
            ItemIntro = "";
        }
        if (itemName == "中級黃藥草")
        {
            BackpackItem = BackpackItemSprite[59];
            ShowBackpackItem = ShowBackpackItemSprite[59];
            ItemType = BackpackItemType[1];
            ItemIntro = "";
        }
        if (itemName == "高級黃藥草")
        {
            BackpackItem = BackpackItemSprite[60];
            ShowBackpackItem = ShowBackpackItemSprite[60];
            ItemType = BackpackItemType[1];
            ItemIntro = "";
        }
        if (itemName == "傳說黃藥草")
        {
            BackpackItem = BackpackItemSprite[61];
            ShowBackpackItem = ShowBackpackItemSprite[61];
            ItemType = BackpackItemType[1];
            ItemIntro = "";
        }
        if (itemName == "初級黑藥草")
        {
            BackpackItem = BackpackItemSprite[62];
            ShowBackpackItem = ShowBackpackItemSprite[62];
            ItemType = BackpackItemType[1];
            ItemIntro = "";
        }
        if (itemName == "中級黑藥草")
        {
            BackpackItem = BackpackItemSprite[63];
            ShowBackpackItem = ShowBackpackItemSprite[63];
            ItemType = BackpackItemType[1];
            ItemIntro = "";
        }
        if (itemName == "高級黑藥草")
        {
            BackpackItem = BackpackItemSprite[64];
            ShowBackpackItem = ShowBackpackItemSprite[64];
            ItemType = BackpackItemType[1];
            ItemIntro = "";
        }
        if (itemName == "傳說黑藥草")
        {
            BackpackItem = BackpackItemSprite[65];
            ShowBackpackItem = ShowBackpackItemSprite[65];
            ItemType = BackpackItemType[1];
            ItemIntro = "";
        }
        if (itemName == "一階強化石")
        {
            BackpackItem = BackpackItemSprite[100];
            ShowBackpackItem = ShowBackpackItemSprite[100];
            ItemType = BackpackItemType[1];
            ItemIntro = "";
        }
        if (itemName == "二階強化石")
        {
            BackpackItem = BackpackItemSprite[101];
            ShowBackpackItem = ShowBackpackItemSprite[101];
            ItemType = BackpackItemType[1];
            ItemIntro = "";
        }
        if (itemName == "三階強化石")
        {
            BackpackItem = BackpackItemSprite[102];
            ShowBackpackItem = ShowBackpackItemSprite[102];
            ItemType = BackpackItemType[1];
            ItemIntro = "";
        }
        if (itemName == "山銅")
        {
            BackpackItem = BackpackItemSprite[103];
            ShowBackpackItem = ShowBackpackItemSprite[103];
            ItemType = BackpackItemType[1];
            ItemIntro = "";
        }
        if (itemName == "玄鐵")
        {
            BackpackItem = BackpackItemSprite[104];
            ShowBackpackItem = ShowBackpackItemSprite[104];
            ItemType = BackpackItemType[1];
            ItemIntro = "";
        }
        if (itemName == "秘銀")
        {
            BackpackItem = BackpackItemSprite[105];
            ShowBackpackItem = ShowBackpackItemSprite[105];
            ItemType = BackpackItemType[1];
            ItemIntro = "";
        }
        if (itemName == "精金")
        {
            BackpackItem = BackpackItemSprite[106];
            ShowBackpackItem = ShowBackpackItemSprite[106];
            ItemType = BackpackItemType[1];
            ItemIntro = "";
        }
    }
}

///00 初級紅藥水
///01 中級紅藥水
///02 高級紅藥水
///03 傳說紅藥水
///04 初級藍藥水
///05 中級藍藥水
///06 高級藍藥水
///07 傳說藍藥水
///08 初級黃藥水
///09 中級黃藥水
///10 高級黃藥水
///11 傳說黃藥水
///12 初級秘藥
///13 中級秘藥
///14 高級秘藥
///15 傳說秘藥
///16 萬能草
///
///50 初級紅藥草
///51 中級紅藥草
///52 高級紅藥草
///53 傳說紅藥草
///54 初級藍藥草
///55 中級藍藥草
///56 高級藍藥草
///57 傳說藍藥草
///58 初級黃藥草
///59 中級黃藥草
///60 高級黃藥草
///61 傳說黃藥草
///62 初級黑藥草
///63 中級黑藥草
///64 高級黑藥草
///65 傳說黑藥草
/// 
///100 一階強化石
///101 二階強化石
///102 三階強化石
///103 山銅
///104 玄鐵
///105 秘銀
///106 精金
