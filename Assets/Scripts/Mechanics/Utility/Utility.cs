using System.Collections.Generic;
using UnityEngine;

namespace Utilities {
    public static class Utility {
        public static bool isPointInPolygonOld(Vector2 point, Vector2[] polygonBounds) {
            int polygonLength = polygonBounds.Length, i = 0;
            bool inside = false;
            // x, y for tested point.
            float pointX = point.x, pointY = point.y;
            // start / end point for the current polygon segment.
            float startX, startY, endX, endY;
            Vector2 endPoint = polygonBounds[polygonLength - 1];
            endX = endPoint.x;
            endY = endPoint.y;
            while (i < polygonLength) {
                startX = endX; startY = endY;
                endPoint = polygonBounds[i++];
                endX = endPoint.x; endY = endPoint.y;
                inside ^= (endY > pointY ^ startY > pointY) && ((pointX - endX) < (pointY - endY) * (startX - endX) / (startY - endY));
            }
            return inside;
        }

        public static bool isPointInPolygon(Vector2 p, Vector2[] polyPoints) {
            int j = polyPoints.Length - 1;
            bool inside = false;
            for (int i = 0; i < polyPoints.Length; j = i++) {
                Vector2 pi = polyPoints[i];
                Vector2 pj = polyPoints[j];
                if (((pi.y <= p.y && p.y < pj.y) || (pj.y <= p.y && p.y < pi.y)) &&
                    (p.x < (pj.x - pi.x) * (p.y - pi.y) / (pj.y - pi.y) + pi.x))
                    inside = !inside;
            }
            return inside;
        }

        public static bool isPointInPolygon(Vector2 point, List<Vector2> polygonBounds) {
            return isPointInPolygon(point, polygonBounds.ToArray());
        }

        public static void drawPolygon(List<Vector2> polygon, Color color) {
            for (int i = 0; i < polygon.Count - 1; i++) {
                Debug.DrawLine(polygon[i], polygon[i + 1], color);
            }
            Debug.DrawLine(polygon[polygon.Count - 1], polygon[0], color);
        }
    }
}