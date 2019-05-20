//using System;
using UnityEngine;
using System.Reflection;
using System.Collections;

public class BattleManager : MonoBehaviour
{
    private static BattleMapManager _instance;
    public static BattleMapManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.Find("BattleMapContainer").GetComponent<BattleMapManager>();
            return _instance;
        }
    }

    //Todo 多个目标
    public void CastSkill(BattleUnit attacker,BattleUnit target,Skill skill){
        if (skill == null)
            return;
        SkillEffectHandler handler = new SkillEffectHandler();

        string methodName = skill.EffectType.ToString();
        //string methodName = Enum.GetName(typeof(SkillEffectType), skill.EffectType); //需要using System;
        MethodInfo method = handler.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
        if(method==null){
            Debug.Log("Can not find method: " + methodName);
            return;
        }
        object[] parameters = new object[] { attacker, skill.Power, target };
        int result = (int)method.Invoke(handler, parameters);
        Debug.Log(skill.Name + " released, effect--> " + methodName + ", result--> " + result);
    }
}
