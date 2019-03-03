using UnityEngine;

namespace AStar.ActionGrid
{
    public class NavigationActionGrid : MonoBehaviour
    {
        public IGraph Graph { get; set; }

        public float Width;
        public float Height;
    }
}