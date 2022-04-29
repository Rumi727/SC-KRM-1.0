using System.Collections.Generic;
using UnityEngine;

namespace SCKRM.Polygon
{
    [ExecuteAlways]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(PolygonCollider2D))]
    public class PolygonColliderToMesh : MonoBehaviour
    {
        PolygonCollider2D _polygonCollider;
        public PolygonCollider2D polygonCollider
        {
            get
            {
                if (_polygonCollider == null)
                    _polygonCollider = GetComponent<PolygonCollider2D>();

                return _polygonCollider;
            }
        }

        MeshFilter _meshFilter;
        public MeshFilter meshFilter
        {
            get
            {
                if (_meshFilter == null)
                    _meshFilter = GetComponent<MeshFilter>();

                return _meshFilter;
            }
        }



        Mesh mesh;
        void Awake()
        {
            // This creates a unique mesh per instance. If you re-use shapes
            // frequently, then you may want to look into sharing them in a pool.
            mesh = new Mesh();
            mesh.MarkDynamic();

            meshFilter.sharedMesh = mesh;
        }

        void Update() => polygonCollider.ToMesh(mesh);
    }
}