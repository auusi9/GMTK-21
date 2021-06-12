namespace Code.Utils
{
    using UnityEngine;
 
    public class DottedLineRenderer : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;

        private void ScaleMaterial()
        {
            float width =  _lineRenderer.startWidth;
            _lineRenderer.material.mainTextureScale = new Vector2(1f / width, 1.0f);
        }
 
        private void Update ()
        {
            ScaleMaterial();
        }
    }
}