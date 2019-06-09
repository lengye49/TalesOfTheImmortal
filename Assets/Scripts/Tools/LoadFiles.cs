using UnityEngine;
using System.Collections;

public class LoadFiles : MonoBehaviour
{
    public static Skill GetSkill(SkillInfo info){return new Skill();}

    public static Item GetItem() { return new Item(); }

    public static string GetLanguage(int id){
        if (id == 10000)
            return "剑气";
        if (id == 10001)
            return "直线剑气攻击";
        return id.ToString();
    }
}
