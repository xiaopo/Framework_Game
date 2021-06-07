
using System.Collections.Generic;
using UnityEngine;

namespace Fight
{
    public class BezierUtil
    {

        public static List<Vector3> Bezier(List<Transform> p, float totalNum, List<Vector3> corners = null)
        {
            
            List<Vector3> newp = new List<Vector3>();
    
            for (int i = 0; i < p.Count; i++)
            {
                if(p[i] != null)
                    newp.Add(p[i].position);
            }

            return BezierUtil.Bezier(newp, totalNum, corners);
        }
        /// <summary>
        /// 获得一列路径点
        /// </summary>
        /// <param name="p">关键点</param>
        /// <param name="totalNum">产生多少个点</param>
        /// <returns></returns>
        public static List<Vector3> Bezier( List<Vector3> p, float totalNum, List<Vector3> corners = null)
        {
            if (totalNum < 0) totalNum = 1;
            if (corners == null)
                corners = new List<Vector3>();
            else
                corners.Clear();
            if (p.Count == 0) return corners;
            float dt = 1.0f / totalNum;
            float t = 0;
            while (t < 1)
            {
                t += dt;
                Vector3 pos = Bezier(t, p);
                corners.Add(pos); 
            }

            return corners;
        }

        /// <summary>
        /// 获得一个路径上的点
        /// </summary>
        /// <param name="curTime"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Vector3 Bezier(float curTime, float totalTime, List<Vector3> p)
        {
            if (totalTime <= 0) totalTime = 1;//totaltime

            if (curTime > totalTime) curTime = totalTime;

            float t = curTime / totalTime;

            return Bezier(t, p);

        }

        // n阶曲线，递归实现
        public static Vector3 Bezier(float t, List<Vector3> p)
        {
            if (p.Count < 2)
                return p[0];
            List<Vector3> newp = new List<Vector3>();
            for (int i = 0; i < p.Count - 1; i++)
            {
                //Debug.DrawLine(p[i], p[i + 1]);
                Vector3 p0p1 = (1 - t) * p[i] + t * p[i + 1];
                newp.Add(p0p1);
            }
            return Bezier(t, newp);
        }

        //transform转换为vector3，在调用参数为List<Vector3>的Bezier函数
        public static Vector3 Bezier(float t, List<Transform> p)
        {
            if (p.Count < 2)
                return p[0].position;
            List<Vector3> newp = new List<Vector3>();
            for (int i = 0; i < p.Count; i++)
            {
                newp.Add(p[i].position);
            }
            return Bezier(t, newp);
        }

    }
}
