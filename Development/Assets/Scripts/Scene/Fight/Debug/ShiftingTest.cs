using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftingTest : MonoBehaviour
{

    class ShitingData
    {
        public float V0 = 1.0f;
        public float accelerate = 1.0f;
        public float endTime = 0.0f;
        public float startTime = 0.0f;
        public float speed = 0;
    }

    // Use this for initialization
    //private float endTime = 0.0f;
    //private float speed = 0.0f;

    public List<string> sparams;//-- 初速度，加速度,时间
    private List<ShitingData> items = new List<ShitingData>();
  

    private Transform mtransform;
    private Vector3 borthPos;

	private bool EnableB = false;
	void Start ()
    {
        mtransform = gameObject.GetComponent<Transform>();
        borthPos = mtransform.position;
    }

    private void FixedUpdate()
    {
		if (EnableB)
		{
            for(int i = 0;i< items.Count;i++)
            {
                if (Time.time >= items[i].startTime)
                {
                    if(Time.time <= items[i].endTime)
                    {
                        //V0t + 1/2at^2 = S;
                        items[i].speed += items[i].accelerate * Time.fixedDeltaTime;

                        Vector3 offest = mtransform.forward * items[i].speed * Time.fixedDeltaTime;
                        mtransform.position += offest;
                    }
                    else
                    {
                        //超时
                        //移除
                        items.RemoveAt(i);
                        i--;
                        continue;
                    }
                }
                
            }

            if (items.Count <= 0) EnableB = false;
            startTotalTime += Time.fixedDeltaTime;
          
        }

	}

    private float startTotalTime;
    public void OnGUI()
    {
        GUILayout.Space(100.0f);

        if (GUILayout.Button("匀变速运动", GUILayout.Width(400.0f), GUILayout.Height(200.0f)))
        {
            startTotalTime = 0.0f;
            items = new List<ShitingData>();
            if (sparams != null && sparams.Count > 0)
            {
                float totlTime = 0;
                for(int i = 0;i< sparams.Count;i++)
                {
                    string ss = sparams[i];
                    string[] par = ss.Split(',');
                    ShitingData data = new ShitingData();
                    data.V0 = par[0] != null ? int.Parse(par[0]) : 0;
                    data.V0 /= 1000.0f;
                    data.accelerate = par[1] != null ? int.Parse(par[1]) : 0;
                    data.accelerate /= 1000.0f;
                    float time = par[2] != null ? int.Parse(par[2]) : 0;
                    time /= 1000.0f;

                    data.startTime = Time.time + totlTime;//开始时间
                    data.endTime = data.startTime + time;//结束时间

                    totlTime += time;

                    items.Add(data);
                }
            }

			EnableB = true;
        }

        if(EnableB)
        {
            GUILayout.TextField("运行时长：" + startTotalTime, 20);
        }
        else
        {
            GUILayout.TextField("运行时长：0.0" , 20);
        }

		GUILayout.Space(200.0f);
		if (GUILayout.Button("复位", GUILayout.Width(400.0f), GUILayout.Height(100.0f)))
		{
            startTotalTime = 0.0f;
            mtransform.position = borthPos;
            items = new List<ShitingData>();
            this.EnableB = false;

        }
    }
}
