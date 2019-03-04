using System.Collections.Generic;
using UnityEngine;

namespace AStar.ActionGrid
{
    /// <summary>
    ///     Auto refreshable walkable grid
    /// </summary>
    public class NavigationActionGrid : MonoBehaviour, INavigationActionGrid
    {
        public IGraph Graph { get; set; }
        public IGraphNeighborsService NeighborsService { get; set; }

        public float NotWalkableCost = 1;
        public float WalkableCost = 1;

        private readonly List<Vector2Int> neighborsBuffer = new List<Vector2Int>();
        
        public float Width
        {
            get { return _Width; }
            set { _Width = value; }
        }

        public float Height
        {
            get { return _Height; }
            set { _Height = value; }
        }

        [SerializeField]
        private float _Width;
        
        [SerializeField]
        private float _Height;
        
        /// <summary>
        ///     Refresh transitions for current graph configuration in plane 
        /// </summary>
        [ContextMenu("Refresh transitions")]
        public void RefreshTransitions()
        {
            for (int x = 0; x < Graph.Width; x++)
            {
                for (int y = 0; y < Graph.Height; y++)
                {
                    var point = new Vector2Int(x, y);
                    var position = GetCellPoint(point);
                    
                    NeighborsService.GetNeighbors(Graph, point, neighborsBuffer);
                    foreach (var i in neighborsBuffer)
                    {
                        var neighbor = GetCellPoint(i);

                        var dir = (neighbor - position).normalized;
                        var distance = Vector3.Distance(neighbor, position);

                        var transitionCost = WalkableCost;
                        
                        if (Physics.Raycast(position, dir, distance) || Physics.Raycast(neighbor, -dir, distance))
                        {
                            transitionCost = NotWalkableCost;
                        }
                        
                        Graph.SetTransition(point, i, transitionCost);
                    }
                }
            }
        }

        /// <summary>
        ///    Calculate point in world space relative for graph
        /// </summary>
        public Vector3 GetCellPoint(Vector2Int node)
        {
            return transform.TransformPoint(new Vector3(node.x * (Width / Graph.Width), 0,
                node.y * (Height / Graph.Height)));
        }

        private void OnDrawGizmos()
        {
            if (Graph == null || NeighborsService == null)
            {
                return;
            }
            
            RefreshTransitions();
            
            for (int x = 0; x < Graph.Width; x++)
            {
                for (int y = 0; y < Graph.Height; y++)
                {
                    var point = new Vector2Int(x, y);
                    var position = GetCellPoint(point);
                    
                    NeighborsService.GetNeighbors(Graph, point, neighborsBuffer);
                    foreach (var i in neighborsBuffer)
                    {
                        var neighbor = GetCellPoint(i);

                        var dir = (neighbor - position).normalized;
                        var distance = Vector3.Distance(neighbor, position);
                        var transition = Graph.GetTransition(point, i);

                        Gizmos.color = Color.Lerp(Color.green, Color.red, transition);
                        
                        Gizmos.DrawLine(position, neighbor);
                    }
                    
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawSphere(position, 0.8f);
                }
            }
        }
    }
}