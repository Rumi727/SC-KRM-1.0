using UnityEngine;

namespace SCKRM.Polygon
{
    [ExecuteAlways]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(LineRenderer))]
    public class LineRendererToFilledMesh : MonoBehaviour
    {
        LineRenderer _lineRenderer;
        public LineRenderer lineRenderer
        {
            get
            {
                if (_lineRenderer == null)
                    _lineRenderer = GetComponent<LineRenderer>();

                return _lineRenderer;
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



        void Awake() => Initialize();

        void Update()
        {
            if (meshFilter.sharedMesh == null)
                Initialize();

            lineRenderer.PositionToFilledMesh(meshFilter.sharedMesh);
        }



        public void Initialize()
        {
            // This creates a unique mesh per instance. If you re-use shapes
            // frequently, then you may want to look into sharing them in a pool.
            Mesh mesh = new Mesh();
            mesh.MarkDynamic();

            meshFilter.sharedMesh = mesh;
        }
    }
}