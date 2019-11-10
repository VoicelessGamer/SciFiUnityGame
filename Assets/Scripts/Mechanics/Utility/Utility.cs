using System.Collections.Generic;
using UnityEngine;

namespace Utilities {
    public static class Utility {
        public static bool isPointInPolygon(Vector2 point, Vector2[] polygonBounds) {
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

        public static bool isPointInPolygon(Vector2 point, List<Vector2> polygonBounds) {
            return isPointInPolygon(point, polygonBounds.ToArray());
        }
    }
}