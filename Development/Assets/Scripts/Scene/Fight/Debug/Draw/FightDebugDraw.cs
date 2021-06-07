
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fight;

public class FightDebugDraw : MonoBehaviour
{
    private static FightDebugDraw instance;
    private Transform mtransfomrm;

    public static FightDebugDraw Instance
    {
        get {

            if(instance == null)
            {
                //GameObject obj = XGame.Mapfting.MapFtingPro.instance.fightProgram.gameObject;

                //if (obj == null) return null;

                //instance = obj.AddComponent<FightDebugDraw>();
            }

            return instance;

        }
    }

    private void Start()
    {
        mtransfomrm = gameObject.GetComponent<Transform>();
    }

    public void DrawArea(int targetType ,string targetParam,Vector3 fightPoint, Transform attacker,Transform defencer)
    {
        //#if UNITY_EDITOR
        Clean();
        List<string[]> paramList = StringUtils.AnalysisToArray(targetParam);
        if ((targetType & 8) == 8)
        {
            for (int i = 0; i < paramList.Count; i++)
            {
                string[] param = paramList[i];
                // int tType = int.Parse(param[0]);
                int basePoint = int.Parse(param[1]);//原点类型 @see ETargetBasePoint
                                                    //  int distance = int.Parse(param[2]);//距离，半径

                Vector3 nDot = Vector3.zero;
                Vector3 End = Vector3.zero;
                Vector3 fightP = new Vector3(fightPoint.x, attacker.position.y, fightPoint.z);
                Vector3 dir = fightP - attacker.position;
                dir.Normalize();
                if (basePoint == 1)
                {
                    //以攻击者坐标为原点
                    nDot = attacker.position;
                    End = fightPoint;

                    DebugDraw(attacker, param, nDot, End, dir);
                }
                else if (basePoint == 2)
                {
                    //以防守者坐标为原点

                    nDot = fightPoint;
                    End = attacker.position;
                    if (defencer != null)
                    {
                        DebugDraw(defencer, param, nDot, End, dir);
                    }

                }
            }

        }
        //#endif

    }


    private void DebugDraw(Transform entity, string[] param, Vector3 nDot, Vector3 End, Vector3 dir)
    {

        CPoint nnDot = new CPoint(nDot.x, nDot.z);
        CPoint nnEnd = new CPoint(End.x, End.z);

        int tType = int.Parse(param[0]);
        if (tType == 3  || tType == 2)
        {
            //计算基础坐标
            double hurtR, hurtAngle;
            CPoint rePoint = FightMathf2.changeAbsolute2Relative(nnDot, nnEnd);
            FightMathf2.XYToPolarCoordinate(rePoint, out hurtR, out hurtAngle);

            double offSetDis = int.Parse(param[3]);
            double offAngle = int.Parse(param[4]);

            if (tType == 3)
            {
                offSetDis = int.Parse(param[3]);
                offAngle = int.Parse(param[4]);
            }
            else
            {
                offSetDis = 0;
                offAngle = int.Parse(param[4]);
            }

            //增加偏移
            CPoint shiftPoint = new CPoint();
            FightMathf2.PolarCoordinateToXY(offSetDis, hurtAngle + offAngle, ref shiftPoint);
            nnDot.x += shiftPoint.x;
            nnDot.y += shiftPoint.y;

        }

        Vector3 center = new Vector3((float)nnDot.x, entity.transform.position.y, (float)nnDot.y);
        DebugShape(entity.transform, tType, param, center, dir);

    }

    private List<DrawLineData> lineDatas = new List<DrawLineData>();
    private void DebugShape(Transform target, int tType, string[] param,Vector3 center,Vector3 dir)
    {

        DrawLineData linData = new DrawLineData();
        GameObject tGO = new GameObject("canvs");

        Transform mtrans = tGO.GetComponent<Transform>();
        if(mtrans == null)
        {
            mtrans = tGO.AddComponent<Transform>();
        }

        mtrans.SetParent(mtransfomrm);
        mtrans.localPosition = Vector3.zero;
        mtrans.localRotation = Quaternion.identity;
        mtrans.LookAt(dir);
        linData.linObj = tGO;
        linData.target = mtrans;
        linData.forward = target.forward;
        linData.startTime = Time.time;
        if (tType == 3)
        {
            //圆形攻击
            tGO.name = "圆形范围";
            linData.drawType = FtDrawType.DrawCircle;
            linData.radius = int.Parse(param[2]);
            linData.offSetDis = int.Parse(param[3]);
            linData.offAngle = int.Parse(param[4]);
            linData.center = center;

        }
        else if (tType == 2)
        {
            //扇形攻击
            tGO.name = "扇形范围";
            linData.drawType = FtDrawType.DrawSector;
            linData.radius = int.Parse(param[2]);
            linData.angle = int.Parse(param[3]);
            linData.offAngle = int.Parse(param[4]);
            linData.center = center;
        }
        else if (tType == 1)
        {

            //矩形攻击
            tGO.name = "矩形范围";
            linData.drawType = FtDrawType.DrawRectangle;
            linData.length = int.Parse(param[2]);
            linData.width = int.Parse(param[3]);
            linData.center = center;
            linData.right = target.right;

        }

        lineDatas.Add(linData);
    }

    public void Clean()
    {
       if(lineDatas != null && lineDatas.Count > 0)
        {
            for(int i = 0;i< lineDatas.Count;i++)
            {
                lineDatas[i].target = null;
                GameObject.Destroy(lineDatas[i].linObj);
                lineDatas[i].linObj = null;
            }

            lineDatas.Clear();
        }
        
    }

    void Update()
    {
        if (lineDatas == null || lineDatas.Count <= 0)
        {
            return;
        }

        for (int i = 0; i < lineDatas.Count; i++)
        {
            if(Time.time - lineDatas[i].startTime > 5)
            {
                lineDatas[i].target = null;
                GameObject.Destroy(lineDatas[i].linObj);
                lineDatas[i].linObj = null;

                lineDatas.RemoveAt(i);
                i--;
                continue;
            }
            switch (lineDatas[i].drawType)
            {
                case FtDrawType.DrawSector:
                    DrawTool.DrawSector(lineDatas[i]);
                    break;
                case FtDrawType.DrawCircle:
                    DrawTool.DrawCircle(lineDatas[i]);
                    break;
                case FtDrawType.DrawRectangle:
                    DrawTool.DrawRectangle(lineDatas[i]);
                    break;
                default:
                    break;
            }
        }

       
    }
}
