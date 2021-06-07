using System;
namespace TPL
{
    /// <summary>
    /// SDK,后台部分
    /// </summary>
    public class TPLManager : SingleTemplate<TPLManager>
    {
        private event Action action_done;
        private bool b_start;
        public void Start(Action donefun)
        {
            action_done = donefun;
            b_start = true;
        }


        public void Update()
        {
            if (!b_start) return;

            if (action_done != null)
                action_done.Invoke();

            action_done = null;

            b_start = false;
        }
    }
}