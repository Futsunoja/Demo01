using UnityEngine;

public class SaveLoadDataTest : MonoBehaviour
{
    

    [SerializeField]
    PlayerData data;

    [SerializeField]
    PlayerData2 data2;

    private void Start()
    {

        data = JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString("jsondata"));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            PlayerPrefs.SetString("jsondata", JsonUtility.ToJson(data));
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            data = JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString("jsondata"));
            data2 = JsonUtility.FromJson<PlayerData2>(PlayerPrefs.GetString("jsondata"));
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
        public int exe;
        public int ItemMax;
        public int RedPoison;
        public int BluePoison;
        public int BuffPoison;
        public int UndebuffPoison;
        public int speed;
    }

    [System.Serializable]
    public class PlayerData2
    {
        public int atk;
        public int def;
        public int speed;
    }
}
