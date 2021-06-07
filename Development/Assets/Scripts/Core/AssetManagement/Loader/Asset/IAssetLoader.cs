//龙跃
using System.Collections;
using Object = UnityEngine.Object;
namespace AssetManagement
{
    abstract public class IAssetLoader : IEnumerator
    {
        public object Current => null;
        virtual public string assetName { get { return ""; } }
        public bool MoveNext() { return !IsDone(); }
        public void Reset() { }
        virtual public float Progress() { return 0.0f; }
        virtual public void Update() { }
        virtual public bool IsDone() { return false; }
        virtual public bool IsSucceed() { return false; }
        virtual public void Dispose() { }

    }
}