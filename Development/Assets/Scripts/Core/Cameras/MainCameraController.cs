using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 相机控制插件
/// </summary>
public class MainCameraController : MonoBehaviour
{
 
    void Start()
    {
        this.transform.position = new Vector3(21.6f, 30.5f, 89);
        this.transform.rotation = Quaternion.Euler(16, -167, 0);
    }

 
    void Update()
    {
        
    }
}
