using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fight
{
    public enum ECSkillEffectType
    {
        eET_BeFight = 1,        // 战斗普通特效            SkillCfgBeFight.xml
        eET_BeShoot = 2,        // 射击普通特效            SkillCfgBeShoot.xml
        eET_Fly = 3,            //飞行特效                 SkillCfgBeFly.xml
        eET_CDTime = 4,         //倒计时特效               SkillCfgBeTime.xml
        eET_Link = 5,          //魔法链条链接 特效          SkillCfgBeLink.xml
        eET_Area = 6,          // 范围 特效                SkillCfgBeArea.xml
        eET_Normal = 7,        // 无逻辑 特效              SkillCfgBeNormal.xml
        eET_Passive = 8,       //被动特效                  SkillCfgBePassive
        eET_FlyMore = 9,       //同时多个飞向目标           SkillCfgBeFly.xml
        eET_FightMore = 10,    //创建受击敌人个特效         SkillCfgBeFight.xml
        eET_ShootMore = 11,     // 创建多个普通射击特效     SkillCfgBeShoot.xml

        eEt_ShootArea = 12,     //向一个范围内随机 copyNum 次射击                     SkillCfgBeShoot.xml
        eEt_FlyArea = 13,       //向一个范围内随机 copyNum 次飞行                     SkillCfgBeFly.xml
        eET_UV_Link = 14,       //创建一条】使用 LineRenderer 的闪电治疗链激光链等          SkillCfgBeLink.xml
        eET_UV_More_Link = 15,   //【创建多条】使用 LineRenderer 的闪电治疗链激光链等            SkillCfgBeLink.xml

        eET_Warning_sector = 16,//扇形,可更加配置角度变化的扇 特效
    }

    public class CEAttAniType
    {
        public static string attack = "attack";//攻击
        public static string skill = "skill";//技能
        public static string warning = "warning";//预警
        public static string Passive = "Passive";//被动技能
        public static string assault = "assault";//冲锋
        public static string jump = "jump";//跳跃
        public static string flicker = "flicker";//闪速
        public static string around = "around";//旋转
        public static string roll = "roll";//滚动
        public static string move = "move";//模型移动

        
    }


}
