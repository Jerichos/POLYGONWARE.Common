using UnityEngine;

namespace POLYGONWARE.Common
{
    public class GridEntity : MonoBehaviour
    {
        

        public void PlaceToCell(Vector3 worldPosition)
        {
            worldPosition.y += 0.05f;
            
            worldPosition.x = GetPoint(worldPosition.x);
            worldPosition.z = GetPoint(worldPosition.z);
            
            var lerp = Vector3.Lerp(transform.position, worldPosition, 20 * Time.deltaTime);
            lerp.y = worldPosition.y;
            transform.position = lerp;
        }

        private static float GetPoint(float axis)
        {
            return Mathf.Floor(axis / Grid.CellSize) * Grid.CellSize;
        }
    }
}