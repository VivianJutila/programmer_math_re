using System;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class SimpleMeshGeneration : MonoBehaviour
{
    private enum GeneratorForms
    {
        Circular,
        Square,
        Donut
    }

    [SerializeField]
    private GeneratorForms mesh_generation_form;

    [SerializeField, Range(0.01f, 20f)]
    private float flat_donut_inner_radius;
    [SerializeField, Range(0.1f, 5f)]
    private float flat_donut_thickness;


    [SerializeField, Range(0.1f, 20f)]
    private float mesh_radius;

    [SerializeField, Range((int)3, (int)255)]
    private int circular_mesh_segments;

    private Mesh generating_mesh;

    private void OnDrawGizmos()
    {
        switch(mesh_generation_form)
        {
            case GeneratorForms.Circular:
                GenerateCircularMesh();
                break;

            case GeneratorForms.Square:
                GenerateSquareMesh();
                break;

            case GeneratorForms.Donut:
                GenerateFlatDonutMesh();
                break;

            default:
                Debug.Log("Choose a Valid Generation Form!");
                break;
        }
    }

    private void GenerateFlatDonutMesh()
    {
        if (generating_mesh == null)
        {
            generating_mesh = new Mesh();
        }
        else
        {
            generating_mesh.Clear();
        }

        generating_mesh.name = "Generating Mesh";

        Vector3[] vertices = new Vector3[circular_mesh_segments * 2];
        float delta_angle = Mathf.PI * 2f / circular_mesh_segments;
        for (int i = 0; i < circular_mesh_segments * 2; i+=2)
        {
            // current_angle = i x delta_angle
            // x = cos(current_angle)
            // z = sin(current_angle)

            float x = Mathf.Cos(delta_angle * (i / 2f));
            float z = Mathf.Sin(delta_angle * (i / 2f));

            vertices[i] = new Vector3(x, 0, z) * flat_donut_inner_radius;
            vertices[i + 1] = new Vector3(x, 0, z) * (flat_donut_inner_radius + flat_donut_thickness);
        }

        int[] triangles = new int[circular_mesh_segments * 6];
        for (int i = 0; i < circular_mesh_segments - 1; i++)
        {
            triangles[i * 6] = i*2;
            triangles[i * 6 + 1] = i*2 + 3;
            triangles[i * 6 + 2] = i*2 + 1;

            triangles[i * 6 + 3] = i*2;
            triangles[i * 6 + 4] = i*2 + 2;
            triangles[i * 6 + 5] = i*2 + 3;
        }
        triangles[triangles.Length - 1] = vertices.Length - 2;
        triangles[triangles.Length - 2] = 1;
        triangles[triangles.Length - 3] = 0;

        triangles[triangles.Length - 4] = vertices.Length - 2;
        triangles[triangles.Length - 5] = vertices.Length - 1;
        triangles[triangles.Length - 6] = 1;

        generating_mesh.SetVertices(vertices);
        generating_mesh.SetTriangles(triangles, 0);
        generating_mesh.RecalculateNormals();

        GetComponent<MeshFilter>().sharedMesh = generating_mesh;
    }

    private void GenerateCircularMesh()
    {
        if (generating_mesh == null)
        {
            generating_mesh = new Mesh();
        }
        else
        {
            generating_mesh.Clear();
        }

        generating_mesh.name = "Generating Mesh";

        Vector3[] vertices = new Vector3[circular_mesh_segments + 1];
        vertices[0] = new Vector3(0, 0, 0);
        float delta_angle = Mathf.PI * 2f / circular_mesh_segments;
        for (int i = 0; i < circular_mesh_segments; i++)
        {
            // current_angle = i x delta_angle
            // x = cos(current_angle)
            // z = sin(current_angle)

            float x = Mathf.Cos(delta_angle * i);
            float z = Mathf.Sin(delta_angle * i);

            vertices[i + 1] = new Vector3(x, 0, z) * mesh_radius;
        }

        int[] triangles = new int[circular_mesh_segments * 3];
        for (int i = 0; i < circular_mesh_segments - 1; i++)
        {
            triangles[i*3] = 0;
            triangles[i*3+1] = i+2;
            triangles[i*3+2] = i+1;
        }
        triangles[circular_mesh_segments * 3 - 3] = 0;
        triangles[circular_mesh_segments * 3 - 2] = 1;
        triangles[circular_mesh_segments * 3 - 1] = vertices.Length - 1;


        generating_mesh.SetVertices(vertices);
        generating_mesh.SetTriangles(triangles, 0); 
        generating_mesh.RecalculateNormals();

        GetComponent<MeshFilter>().sharedMesh = generating_mesh;
    }

    private void GenerateSquareMesh()
    {
        if (generating_mesh == null)
        {
            generating_mesh = new Mesh();
        }
        else
        {
            generating_mesh.Clear();
        }

        generating_mesh.name = "Generating Mesh";

        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(-0.5f, 0, -0.5f) * mesh_radius;
        vertices[1] = new Vector3(0.5f, 0, -0.5f) * mesh_radius;
        vertices[2] = new Vector3(-0.5f, 0, 0.5f) * mesh_radius;
        vertices[3] = new Vector3(0.5f, 0, 0.5f) * mesh_radius;

        int[] triangles = new int[12];

        // triangles for the up-side plane
        triangles[0] = 0;
        triangles[1] = 3;
        triangles[2] = 1;

        triangles[3] = 0;
        triangles[4] = 2;
        triangles[5] = 3;

        // triangles for the down-side plane
        triangles[6] = 0;
        triangles[7] = 1;
        triangles[8] = 3;

        triangles[9] = 0;
        triangles[10] = 3;
        triangles[11] = 2;

        generating_mesh.SetVertices(vertices);
        generating_mesh.SetTriangles(triangles, 0);
        generating_mesh.RecalculateNormals();

        GetComponent<MeshFilter>().sharedMesh = generating_mesh;
    }
}
