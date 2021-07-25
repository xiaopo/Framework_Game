using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneCamera
{
    public class CameraTest : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        float a = 0;
        void Update()
        {
            this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, a++));
        }
    }
}