using UnityEngine;

namespace AStar.ActionGrid
{
    public interface INavigationActionGrid
    {
        float Width { get; set; }
        float Height { get; set; }

        /// <summary>
        ///     Refresh transitions for current graph configuration in plane 
        /// </summary>
        void RefreshTransitions();

        /// <summary>
        ///    Calculate point in world space relative for graph
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        Vector3 GetCellPoint(Vector2Int node);
    }
}