//��Ծ
//�����߼� C# ���� ���
using UnityEngine;
using XLua;

namespace Game.MScene
{ 
    public class MapfightProgram :SingleBehaviourTemplate<MapfightProgram>
    {


        #region CSharp Call Lua Delegate
        //����Call �ص�
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

        #region GameObjects����
        protected Transform mtransfrom;
        private Transform mEntityParent;
        public Transform EntityParent => mEntityParent;//ʵ��Ĳ㼶
        private Transform mEntityDeath;
        public Transform EntityDeathParent => mEntityDeath;//ʵ������
        private Transform mEntityFllowPartParent;
        public Transform EntityFllowPartParent => mEntityFllowPartParent;//����ʵ������
        protected Transform mEffectParent;
        public Transform EffectParent => mEffectParent;//��Ч
        protected Transform mEffectDead;
        public Transform EffectDead => mEffectDead;//��Ч�����
        protected Transform mBuffParent;
        public Transform BuffParent => mBuffParent;//Buff
        protected Transform mBuffDead;
        public Transform BuffDead => mBuffDead;//Buff�����

        #endregion

        protected override void OnInitialize()
        {
            mtransfrom = this.gameObject.GetComponent<Transform>();

        
        }

        /// <summary>
        /// ���� ��ͼʵ�壬ս�� ���߼�
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


            //ʵ����ʵ��ֱ�Ӻ�����ײ
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