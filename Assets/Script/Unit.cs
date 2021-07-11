using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int unitLevel;
    public int atk;
    public int def;
    public float maxHp;
    public float currentHp;
    public int maxSkillPower;
    public int currySkillPower;
    public int exe;
    public int speed;

    public bool TakeDamage(int dmg)
    {
        currentHp -= dmg;
        if (currentHp <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
