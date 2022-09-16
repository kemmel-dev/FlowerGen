using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phyllotaxis : MonoBehaviour
{

    [SerializeField]
    private int nPetals = 100;
    [SerializeField]
    private float alpha = 137.5f;
    [SerializeField]
    private float scale = .25f;

    [SerializeField]
    private float yStep = .001f;

    private Mesh mesh;


    public float thickness;

    //swivel factors
    private float swivelSpeed;
    private float swivel;

    private void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    void Update()
    {
        GenMesh();
    }

    private void GenMesh()
    {
        mesh.SetVertices(GenVertsThickLines());
        mesh.SetIndices(GetIndecesThickLines(mesh.vertices), MeshTopology.Triangles, 0);
    }
    private Vector3[] GenVertsThickLines()
    {
        Vector3[] vertices = new Vector3[nPetals * 2];
        float y = 0;

        for (int i = 0; i < vertices.Length; i+= 2)
        {
            float phi = i * alpha;
            float r = Mathf.Sqrt(i) * scale;

            float x = r * Mathf.Cos(phi);
            float z = r * Mathf.Sin(phi);
            Vector3 pos = new Vector3(x, y, z);

            vertices[i] = pos;
            vertices[i + 1] = pos + new Vector3(thickness, 0, 0);
            y += yStep;
        }

        return vertices;
    }

    private int[] GetIndecesThickLines(Vector3[] vertices)
    {
        int[] indices = new int[vertices.Length * 2];
        int index = 0;
        for (int i = 0; i < indices.Length - 6; i+=6)
        {
            indices[i] = index;
            indices[i + 1] = index + 3;
            indices[i + 2] = index + 1;
            indices[i + 3] = index;
            indices[i + 4] = index + 2;
            indices[index + 5] = index + 3;

            index += 2;
        }


        return indices;
    }

    private int[] GenIndecesLines()
    {

        int[] indeces = new int[nPetals * 2];

        int cnt = 0;
        for (int i = 0; i < indeces.Length - 2; i += 2)
        {
            indeces[i] = cnt;
            indeces[i + 1] = ++cnt;
        }

        return indeces;
    }

    private Vector3[] GenVerts()
    {
        Vector3[] vertices = new Vector3[nPetals];
        float y = 0;

        for (int i = 0; i < nPetals; i++)
        {
            float phi = i * alpha;
            float r = Mathf.Sqrt(i) * scale;
            vertices[i] = new Vector3(r * Mathf.Cos(phi), y, r * Mathf.Sin(phi));
            y += yStep;
        }

        return vertices;
    }


    //private void Swivel()
    //{
    //    float y = 0;

    //    alpha = 130 + Mathf.Sin(swivel += swivelSpeed) * deltaAlpha;

    //    vertices = new Vector3[nPetals];
    //    for (int i = 0; i < nPetals; i++)
    //    {
    //        float phi = i * alpha;
    //        float r = Mathf.Sqrt(i) * scale;
    //        vertices[i] = new Vector3(r * Mathf.Cos(phi), y, r * Mathf.Sin(phi));
    //        y += yStep;
    //    }

    //    triangles = new int[(nPetals - 2) * 3 * 2];

    //    int n = 2;
    //    for (int i = 0; i < triangles.Length / 2; i++)
    //    {
    //        if (i != 0 && i % 3 == 0)
    //        {
    //            n += 4;
    //        }
    //        triangles[i] = n--;
    //    }
    //}

    private void OnDrawGizmos()
    {

        float y = 0;

        Vector3[] verts = GenVertsThickLines();

        for (int i = 0; i < verts.Length; i += 2)
        {
            float phi = i * alpha;
            float r = Mathf.Sqrt(i) * scale;

            float x = r * Mathf.Cos(phi);
            float z = r * Mathf.Sin(phi);
            Vector3 pos = new Vector3(x, y, z);

            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(pos, .015f);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(pos + new Vector3(thickness, 0, 0), .015f);
            y += yStep;
        }
    }
}

