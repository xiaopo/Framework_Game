using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������Ʋ��
/// </summary>
public class MainCameraController : MonoBehaviour
{
 
    void Start()
    {
        this.transform.position = new Vector3(2, 2, 4);
        this.transform.rotation = Quaternion.Euler(16, -167, 0);
    }

 
    void Update()
    {
        
    }
}
