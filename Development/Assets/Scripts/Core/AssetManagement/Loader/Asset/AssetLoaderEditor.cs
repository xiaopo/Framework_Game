//龙跃
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AssetManagement
{
    public class AssetLoaderEditor : AssetLoaderParcel
    {
        private Object _rawObject;
        protected bool isStreamedSceneAsset = false;
        protected bool _isDone;
        protected float _editorProgress;
        public AssetLoaderEditor(string assetName, System.Type type):base(assetName,type)
        {
            isStreamedSceneAsset = assetName.Contains(".unity");
        }
        public override T GetRawObject<T>()
        {
            if(_asyncOperation != null)
            {
                AssetBundleRequest rest = _asyncOperation as AssetBundleRequest;

                Object asset = rest != null ? rest.asset : null;

                return asset as T;
            }


            return this._rawObject as T;
        }

        public override T Instantiate<T>(Transform parent = null)
        {
            if (_rawObject == null) return null;


            return GameObject.Instantiate(_rawObject, parent) as T;
        }

        public override GameObject Instantiate(Transform parent = null)
        {
            GameObject obj = Instantiate<GameObject>(parent);
            if(obj != null) obj.transform.ResetTRS();

            return obj;
        }

        public override float Progress()
        {
            if (_asyncOperation != null)
            {
                return _asyncOperation.progress;
            }

            return _editorProgress;
        }
        public override bool IsDone()
        {
            if (_asyncOperation != null)
            {
                return _asyncOperation.isDone;
            }

            return _isDone;
        }

        public override void Update()
        {
            if (_isDone) return;

            if (_asyncOperation != null)
            {
                //等待加载资源
                //下面的逻辑已经跑完
                return;
            }

#if UNITY_EDITOR
            if (isStreamedSceneAsset)
            {
                _asyncOperation = SceneManager.LoadSceneAsync(_assetName, loadSceneMode);

            }
            else
            {
                this._rawObject = UnityEditor.AssetDatabase.LoadAssetAtPath(this._assetName, this._generateType);
                this._isDone = true;

                //转成文件名缓存
                this._assetName = System.IO.Path.GetFileName(this._assetName);
                if (this._rawObject == null)
                {
                    GameDebug.LogErrorFormat("AssetLoaderEditor::Update UnityEditor.AssetDatabase.LoadAssetAtPath _rawObject is null m_AssetName={0} m_AssetType={1}", this._assetName, this._generateType);
                }

                _editorProgress = 1.0f;
            }
#endif
            
        }

        public override void PDispose(AssetsGetManger protect)
        {
            base.PDispose(protect);

            Resources.UnloadAsset(this._rawObject);
            _rawObject = null;
        }

    }
}