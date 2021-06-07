using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace XGUI
{
    public class XImageSequence : XImage
    {
        static Dictionary<string, Material> s_Materials = new Dictionary<string, Material>();
        [SerializeField]
        private string m_GroupName;
        [SerializeField]
        private int m_XCount = 4;
        [SerializeField]
        private int m_YCount = 4;
        [SerializeField]
        private int m_Speed = 15;
        private Material m_SeqMaterial;
        [SerializeField]
        private bool m_IsFirstFramePlay = true;


        public bool isFirstFramePlay { get { return m_IsFirstFramePlay;} set { m_IsFirstFramePlay = value; } }

        public string groupName { get { return m_GroupName; } set { m_GroupName = value; } }

        //public override bool visible
        //{
        //    get { return base.visible; }
        //    set
        //    {
        //        base.visible = value;
        //        if (base.visible)
        //            UpdateSeqMaterial();
        //    }
        //}


        Material GenNewMaterial()
        {
            Material mat = null;
            if (s_Materials.TryGetValue(groupName, out mat))
            {
                if (mat == null || mat.IsNull())
                    s_Materials.Remove(groupName);
            }

            if (mat == null)
            {
                Shader shader = ShaderHandler.GetShader("X_Shader/G_GUI/FrameAnimation");
                if (shader != null)
                {
                    mat = new Material(shader);
                    mat.name = groupName;
                    s_Materials.Add(groupName, mat);
                }
            }
            return mat;
        }


        void CheckMaterial()
        {
            if (string.IsNullOrEmpty(groupName))
            {
                Debug.LogError("XImageSequence::CheckMaterial m_GroupName IsNullOrEmpty");
                return;
            }

            if (m_SeqMaterial == null)
            {
                m_SeqMaterial = GenNewMaterial();
                material = m_SeqMaterial;
            }
        }

        protected override void Start()
        {
            base.Start();
            if (Application.isPlaying)
                InitMaterial();

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if(m_SeqMaterial != null)
                m_SeqMaterial.SetFloat("_STime", Time.timeSinceLevelLoad);
        }


        protected override void OnEnable()
        {
            base.OnEnable();
            UpdateSeqMaterial();
        }

        void InitMaterial()
        {
            CheckMaterial();
            UpdateSeqMaterial();
        }

        void UpdateSeqMaterial()
        {
            if (m_SeqMaterial == null)
                return;

            m_SeqMaterial.SetFloat("_XCount", m_XCount);
            m_SeqMaterial.SetFloat("_YCount", m_YCount);
            m_SeqMaterial.SetFloat("_Speed", m_Speed);

            if (m_IsFirstFramePlay)
            {
                m_SeqMaterial.SetFloat("_STime", Time.timeSinceLevelLoad);
            }
            

            //Debug.LogFormat("id:{0}   _STime:{1}   _Time:{2}", GetInstanceID(), Time.timeSinceLevelLoad,Shader.GetGlobalVector("_Time"));
        }

 
        public void SetSeqData(int xcount, int ycount, int speed = 15)
        {
            this.m_XCount = xcount;
            this.m_YCount = ycount;
            this.m_Speed = speed;
            UpdateSeqMaterial();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
