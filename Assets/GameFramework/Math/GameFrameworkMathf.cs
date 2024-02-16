//========================================================
// 描述:GameFramework数学类
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2022/1/14 13:35:08
//========================================================

using UnityEngine;

namespace GameFramework.Math
{
    public static class GameFrameworkMathf
    {
        /// <summary>
        /// 获取方形区域内随机的点
        /// </summary>
        /// <param name="centreVe3">中心坐标</param>
        /// <param name="radius">半径</param>
        /// <returns></returns>
        public static Vector3 RandomGetBoxRegionPoint(Vector3 centreVe3, float radius)
        {
            var position = centreVe3;
            var minX = position.x - radius;
            var maxX = position.x + radius;
            var minY = position.y - radius;
            var maxY = position.y + radius;
            var minZ = position.z - radius;
            var maxZ = position.z + radius;
            var result = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Random.Range(minZ, maxZ));
            return result;
        }

        /// <summary>
        /// 获取方形区域内随机的点
        /// </summary>
        /// <param name="centreTrans">中心点</param>
        /// <param name="radius">半径</param>
        /// <returns></returns>
        public static Vector3 RandomGetBoxRegionPoint(Transform centreTrans, float radius) =>
            RandomGetBoxRegionPoint(centreTrans.position, radius);

        /// <summary>
        /// 获取球形区域内随机的点
        /// </summary>
        /// <param name="centreVe3">中心点</param>
        /// <param name="minRadius">最小半径</param>
        /// <param name="maxRadius">最大半径</param>
        /// <returns></returns>
        public static Vector3 RandomGetSphereRegionPoint(Vector3 centreVe3, float minRadius, float maxRadius)
        {
            return centreVe3 +
                   new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized
                   * Random.Range(minRadius, maxRadius);
        }

        /// <summary>
        /// 获取球形区域内随机的点
        /// </summary>
        /// <param name="centreTrans">中心点</param>
        /// <param name="minRadius">最小半径</param>
        /// <param name="maxRadius">最大半径</param>
        /// <returns></returns>
        public static Vector3 RandomGetSphereRegionPoint(Transform centreTrans, float minRadius, float maxRadius) =>
            RandomGetSphereRegionPoint(centreTrans.position, minRadius, maxRadius);

        /// <summary>
        /// 获取某个点是否包含在扇形区域内
        /// </summary>
        /// <param name="point">某个点</param>
        /// <param name="centreTrans">中心点</param>
        /// <param name="angle">扇形区域角度</param>
        /// <param name="radius">扇形区域半径</param>
        /// <returns></returns>
        public static bool GetPointIsContainInSectorRegion(Vector3 point, Transform centreTrans, float angle,
            float radius)
        {
            var length = Mathf.Tan(angle / 2 * Mathf.Deg2Rad) * radius;
            var position = centreTrans.position;
            var pos = position + centreTrans.forward * radius;
            var leftTop = new Vector3(pos.x - length, pos.y + length, pos.z);
            var rightTop = new Vector3(pos.x + length, pos.y + length, pos.z);
            var leftLow = new Vector3(pos.x - length, pos.y - length, pos.z);
            var rightLow = new Vector3(pos.x + length, pos.y - length, pos.z);

            #region 画区域范围

#if UNITY_EDITOR
            Debug.DrawLine(position, leftTop, Color.green);
            Debug.DrawLine(position, rightTop, Color.green);
            Debug.DrawLine(position, leftLow, Color.green);
            Debug.DrawLine(position, rightLow, Color.green);
            Debug.DrawLine(leftTop, rightTop, Color.green);
            Debug.DrawLine(rightTop, rightLow, Color.green);
            Debug.DrawLine(rightLow, leftLow, Color.green);
            Debug.DrawLine(leftLow, leftTop, Color.green);
#endif

            #endregion

            var a = Vector3.Angle(centreTrans.forward * radius, point - position);
            if (a <= angle / 2)
            {
                var dis = Vector3.Distance(position, point);
                if (dis <= radius)
                {
                    return true;
                }
            }

            return false;
        }
        
        /// <summary>
        /// 三次贝塞尔曲线
        /// </summary>
        /// <param name="a">from</param>
        /// <param name="b">ctrl1</param>
        /// <param name="c">ctrl2</param>
        /// <param name="d">to</param>
        /// <param name="t">进度(0-1)</param>
        /// <returns></returns>
        public static Vector3 CubicBezier(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
        {
            var aa = a + (b - a) * t;
            var bb = b + (c - b) * t;
            var cc = c + (d - c) * t;

            var aaa = aa + (bb - aa) * t;
            var bbb = bb + (cc - bb) * t;
            return aaa + (bbb - aaa) * t;
        }
        
        
    }
}