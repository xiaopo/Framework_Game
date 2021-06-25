//龙跃
//场景逻辑 C# 部分 入口
using UnityEngine;
using XLua;

namespace Game.MScene
{ 
    public class MapfightProgram :SingleBehaviourTemplate<MapfightProgram>
    {


        #region CSharp Call Lua Delegate
        //定义Call 回调
        [ReflectionUse]
        [CSharpCallLua]
        public delegate void LuaUpFun(float deltaTime, float unscaledDeltaTime);
        [ReflectionUse]
        [CSharpCallLua]
        public delegate void LuaFixUpFun(float fixedDeltaTime);
        [ReflectionUse]
        [CSharpCallLua]
        public delegate void LuaLateUpFun();

        private LuaUpFun luaUpdate;
        private LuaFixUpFun luaFixedUpdate;
        private LuaLateUpFun luaLateUpdate;
        #endregion

        #region GameObjects容器
        protected Transform mtransfrom;
        private Transform mEntityParent;
        public Transform EntityParent => mEntityParent;//实体的层级
        private Transform mEntityDeath;
        public Transform EntityDeathParent => mEntityDeath;//实体对象池
        private Transform mEntityFllowPartParent;
        public Transform EntityFllowPartParent => mEntityFllowPartParent;//跟随实体的物件
        protected Transform mEffectParent;
        public Transform EffectParent => mEffectParent;//特效
        protected Transform mEffectDead;
        public Transform EffectDead => mEffectDead;//特效对象池
        protected Transform mBuffParent;
        public Transform BuffParent => mBuffParent;//Buff
        protected Transform mBuffDead;
        public Transform BuffDead => mBuffDead;//Buff对象池

        #endregion

        protected override void OnInitialize()
        {
            mtransfrom = this.gameObject.GetComponent<Transform>();

        
        }

        /// <summary>
        /// 启动 地图实体，战斗 等逻辑
        /// </summary>
        public void AwakeMapFight()
        {

            luaUpdate = LuaLauncher.Instance.LuaEnv.Global.Get<LuaUpFun>("MF_Update");
            luaFixedUpdate = LuaLauncher.Instance.LuaEnv.Global.Get<LuaFixUpFun>("MF_FixedUpdate");
            luaLateUpdate = LuaLauncher.Instance.LuaEnv.Global.Get<LuaLateUpFun>("MF_LateUpdate");

            GameObject entities = new GameObject("Entities");
            mEntityParent = entities.GetComponent<Transform>();
            mEntityParent.SetParentOEx(mtransfrom);

            GameObject entitiesFllows = new GameObject("EntityFllows");
            mEntityFllowPartParent = entitiesFllows.GetComponent<Transform>();
            mEntityFllowPartParent.SetParentOEx(mtransfrom);

            GameObject entities2 = new GameObject("DeathEty");
            mEntityDeath = entities2.GetComponent<Transform>();
            mEntityDeath.SetParentOEx(mtransfrom);

            GameObject effect = new GameObject("Effects");
            mEffectParent = effect.GetComponent<Transform>();
            mEffectParent.SetParentOEx(mtransfrom);

            GameObject deffect = new GameObject("DeadEfct");
            mEffectDead = deffect.GetComponent<Transform>();
            mEffectDead.SetParentOEx(mtransfrom);

            GameObject buff = new GameObject("Buffs");
            mBuffParent = buff.GetComponent<Transform>();
            mBuffParent.SetParentOEx(mtransfrom);

            GameObject dbuff = new GameObject("DeadBff");
            mBuffDead = dbuff.GetComponent<Transform>();
            mBuffDead.SetParentOEx(mtransfrom);


            //实体与实体直接忽略碰撞
            int entityLayer = LayerMask.NameToLayer("Entity");
            int defaultLayer = LayerMask.NameToLayer("Default");
            Physics.IgnoreLayerCollision(entityLayer, entityLayer);
            Physics.IgnoreLayerCollision(entityLayer, defaultLayer);
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnDestroy()
        {
            luaUpdate = null;
            luaFixedUpdate = null;
            luaLateUpdate = null;
        }
    }

}