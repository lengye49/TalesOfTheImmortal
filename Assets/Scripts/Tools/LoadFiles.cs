using UnityEngine;
using System.Collections;

public class LoadFiles : MonoBehaviour
{
    public static Skill GetSkill(){return new Skill();}

    public static Item GetItem() { return new Item(); }

    public static BattleSkill GetBattleSkill(Skill skill){
        return null;
    }
}
