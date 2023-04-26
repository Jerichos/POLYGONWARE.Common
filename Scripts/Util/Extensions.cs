using UnityEngine;
using UnityEngine.Tilemaps;

namespace POLYGONWARE.Common
{
    public static class Extensions
    {
        public static void Log(this Transform transform, string logMessage)
        {
            Debug.Log(transform.name + ": " + logMessage, transform);
        }
        
        public static void LogWarning(this Transform transform, string logMessage)
        {
            Debug.LogWarning(transform.name + ": " + logMessage, transform);
        }
        
        public static void LogError(this Transform transform, string logMessage)
        {
            Debug.LogError(transform.name + ": " + logMessage, transform);
        }
        
        public static void Log(this GameObject gameObject, string logMessage)
        {
            Debug.Log(gameObject.name + ": " + logMessage, gameObject);
        }
        
        public static void LogWarning(this GameObject gameObject, string logMessage)
        {
            Debug.LogWarning(gameObject.name + ": " + logMessage, gameObject);
        }
        
        public static void LogError(this GameObject gameObject, string logMessage)
        {
            Debug.LogError(gameObject.name + ": " + logMessage, gameObject);
        }

        public static void SetTile(this Tilemap tileMap, Vector2Int position, TileBase tile)
        {
            tileMap.SetTile((Vector3Int)position, tile);
        }
        
        public static Vector2Int WorldToCell2Int(this Tilemap tileMap, Vector3 position)
        {
            return (Vector2Int)tileMap.WorldToCell(position);
        }
    }
}