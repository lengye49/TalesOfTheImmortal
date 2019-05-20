using UnityEngine;
using System.Collections;

public class SkillEffectHandler 
{
    public SkillEffectHandler(){}

    //1.治疗,返回实际加血量
    int Heal(BattleUnit attacker, string param, BattleUnit target)
    {
        int value = int.Parse(param) * attacker.Attack / 10000;
        value = target.Heal(value);
        return value;
    }

    //2.外伤,返回伤害值
    int PhysicalDamage(BattleUnit attacker, string param, BattleUnit target)
    {
        //闪避
        if (MathCalculator.IsDodge(target.Dodge))
        {
            return 0;
        }

        int value = int.Parse(param) * (attacker.Attack - target.Defence) / 10000;

        //生命盾
        if (value > 0 && target.Shield > 0)
            DamageAfterShield(target, ref value);

        value = Mathf.Max(0, value);

        //内力盾
        if (target.DamageToManaShiled > 0)
        {
            target.DamageToManaShiled--;
            value -= target.DamageMp(value);
        }

        //造成伤害
        if (value > 0)
            target.Damage(value);

        return value;
    }

    //3. 内伤
    //int MagicalDamage(Player attacker, string param, Target target){}

    //4. 对自己造成真实伤害
    int DirectDamage(BattleUnit attacker, string param, BattleUnit target)
    {
        int value = int.Parse(param);
        target.Damage(value);
        return value;
    }

    //5. 增加内力
    int AddMp(BattleUnit attacker, string param, BattleUnit target)
    {
        int value = int.Parse(param);
        value = target.HealMp(value);
        return value;
    }

    //6. 召唤
    int Summon(BattleUnit attacker, string param, BattleUnit target)
    {
        //Player dealer = (Player)target;
        //string[] s = SplitParam(param);
        //for (int i = 0; i < s.Length; i++)
        //{
        //    Puppet p = new Puppet(int.Parse(s[i]));
        //    dealer.Puppets.Add(p);
        //}
        return 1;
    }

    //计算扣除护盾后的伤害
    void DamageAfterShield(BattleUnit target, ref int damage)
    {
        if (damage < target.Shield)
        {
            damage = 0;
            target.Shield -= damage;
        }
        else
        {
            damage -= target.Shield;
            //target.View.UpdateBuffShow();
        }
    }

}
