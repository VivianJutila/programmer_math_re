using UnityEngine;

[CreateAssetMenu(fileName = "CrossSection", menuName = "Scriptable Objects/CrossSection")]
public class CrossSection : ScriptableObject
{
    [System.Serializable]
    public class Vertex
    {
        public Vector2 point;
        public Vector2 normal;
    }

    public Vertex[] vertices;
}
