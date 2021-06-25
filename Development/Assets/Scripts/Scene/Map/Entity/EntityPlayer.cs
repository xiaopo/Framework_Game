

using DG.Tweening;
using UnityEngine;

namespace Game.MScene
{
    public class EntityPlayer: EntityMovement
    {
        protected Transform camera_pos;
        public Transform cameraPos => camera_pos;
        public EntityPlayer(string name):base(name)
        {

        }


        public Transform CreateCameraPos()
        {
            camera_pos = m_pool_empty.Get(GopManager.Empty).transform;
            camera_pos.gameObject.SetActive(true);
            camera_pos.name = "Camera_pos";
            camera_pos.transform.SetParentOEx(mtransfrom);
            camera_pos.transform.localPosition = head_temp.localPosition;

            return camera_pos;
        }

        public void TweenCameraPos(float time = 1.0f, Transform target = null)
        {
            if (target == null)
                camera_pos.DOLocalMove(head_temp.localPosition, time);
            else
                camera_pos.DOMove(target.position, time);

        }

        public override void CSClean()
        {
            base.CSClean();


            if (camera_pos != null) m_pool_empty.Release(camera_pos.gameObject, GopManager.Empty);

        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
