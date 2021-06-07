using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Fight
{
    public class CPoint
    {
        public double x = 0.0d;
        public double y = 0.0d;
        public CPoint(double _x = 0.0d, double _y = 0.0d)
        {
            x = _x;
            y = _y;
        }

        //public static bool operator == (CPoint a, CPoint b)
        //{
        //    return a.x == b.x && a.y == b.y;
        //}

        //public static bool operator != (CPoint a, CPoint b)
        //{
        //    return a.x != b.x || a.y != b.y;
        //}
    }

    /// <summary>
    /// 战斗数学类，2D
    /// </summary>
    public class FightMathf2
    {
        /// 坐标轴旋转公式
        /// 假设点(x , y)绕(x0 , y0)逆时针旋转a角后变成(x' , y')
        /// x'- x0 = (x - x0)cosa - (y - y0)sina
        /// y'- y0 = (x - x0)sina + (y - y0)cosa
      
         
        /// <summary>
        ///  旋转坐标系n度
        /// </summary>
        /// <param name="p">被旋转点</param>
        /// <param name="n">旋转角度</param>
        /// <param name="nDot">新圆点</param>
        public static CPoint RotateAndMoveCoordinate(CPoint p, float n,CPoint nDot)
        {
           
            //旋转并且以nDot 为坐标圆点
            double npX = (p.x - nDot.x) * Math.Sin(n) + (p.y - nDot.y) * Math.Cos(n);
            double npy = (p.x - nDot.x) * Math.Cos(n) + (p.y - nDot.y) * Math.Sin(n);

            return new CPoint(npX, npy);
        }

        public static CPoint RotateCoordinate(CPoint p, float n)
        {
            //旋转
            double npx = p.x * Math.Sin(n) + p.y * Math.Cos(n);
            double npy = p.x * Math.Cos(n) + p.y * Math.Sin(n);

            return new CPoint(npx, npy);
        }

        //旋转一个2D向量
        public static void RotateLine(ref CPoint p1,ref CPoint p2,float n)
        {
            CPoint np1 = FightMathf2.RotateCoordinate(p1, n);
            CPoint np2 = FightMathf2.RotateCoordinate(p2, n);

            p1.x = np1.x;
            p1.y = np1.y;
            p2.x = np2.x;
            p2.y = np2.y;
        }

        //旋转后以新点为坐标圆点
        public static void RotateMoveLine(ref CPoint p1, ref CPoint p2, float n,CPoint nDot)
        {
            CPoint np1 = FightMathf2.RotateAndMoveCoordinate(p1, n, nDot);
            CPoint np2 = FightMathf2.RotateAndMoveCoordinate(p2, n, nDot);
  
            p1.x = np1.x;
            p1.y = np1.y;
            p2.x = np2.x;
            p2.y = np2.y;
        }

        //计算2个向量的夹角
        public static double computeVectorAngle(CPoint v1, CPoint v2)
        {
            if ((v1.x == 0 && v1.y == 0) || (v2.x == 0 && v2.y == 0))
            {
                return 0;//其中一个为原点，则长度为0，无法计算，也无需计算
            }
            double lenght1Square = Math.Sqrt(v1.x * v1.x + v1.y * v1.y);
            double lenght2Square = Math.Sqrt(v2.x * v2.x + v2.y * v2.y);
            v1.x /= lenght1Square;
            v1.y /= lenght1Square;
            v2.x /= lenght2Square;
            v2.y /= lenght2Square;

            return RadianToAngle(Math.Acos(v1.x * v2.x + v1.y * v2.y));//角CAB的大小  
        }



        ///////////////////////////////极坐标和笛卡尔坐标//////////////////////////////
        //检测点是否在矩阵中
        public static bool inRect(double minx, double miny, double maxx, double maxy, CPoint p)
        {
            if (p.x >= minx && p.x <= maxx && p.y >= miny && p.y <= maxy)
            {
                return true;
            }

            return false;
        }


        public static double Distance(CPoint from, CPoint to)
        {
	        return Math.Sqrt(Math.Pow(to.x - from.x, 2) + Math.Pow(to.y - from.y, 2));
        }

        public static double RadianToAngle(double radian)
        {
            return radian * 180 / Math.PI;
        }

        public static double AngleToRadian(double angle)
        {
            return angle * Math.PI / 180;
        }
        
        //极坐标转换为迪卡尔坐标
        public static void PolarCoordinateToXY(double r,double angle,ref CPoint p)
        {
            p.x = r * Math.Cos(AngleToRadian(angle));
            p.y = r * Math.Sin(AngleToRadian(angle));
        }

        //迪卡尔坐标转换为极坐标
        public static void XYToPolarCoordinate(CPoint p,out double r, out double angle)
        {
            r = Math.Sqrt(p.x * p.x + p.y * p.y);
            angle = RadianToAngle(Math.Atan2(p.y, p.x));
        }

        /**
	    * 直角坐标--绝对坐标转相对坐标
	    * originPoint 相对坐标系的原点
	    * directionPoint 指向x轴方向的点
	    * changePoint 需要转换的坐标
	    */
        public static CPoint changeAbsolute2Relative(CPoint originPoint, CPoint changePoint)  //没有转换角度
        {
            CPoint rePoint = new CPoint();
            rePoint.x = changePoint.x - originPoint.x;
            rePoint.y = changePoint.y - originPoint.y;
            return rePoint;
        }
          
        public static CPoint changeAbsolute2Relative(CPoint originPoint, CPoint directionPoint, CPoint changePoint)//要旋转角度
        {
            CPoint rePoint = new CPoint();
            if (originPoint.x == directionPoint.x && originPoint.y == directionPoint.y)
            {
                rePoint.x = changePoint.x - originPoint.x;
                rePoint.y = changePoint.y - originPoint.y;
            }
            else
            {
                double a = Distance(directionPoint, changePoint);
                double b = Distance(changePoint, originPoint);
                double c = Distance(directionPoint, originPoint);

                double cosa = (b * b + c * c - a * a) / (2 * b * c);//余弦定理
                rePoint.x = a * cosa;
                rePoint.y = Math.Sqrt(a * a - rePoint.x * rePoint.x);
            }
            return rePoint;
        }
    }
}
