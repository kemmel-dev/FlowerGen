using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerGen : MonoBehaviour
{

    private Mesh flower;
    private Mesh stem;


    [Header("Flower Settings")]
    [SerializeField]
    private int nFlowerSegments;
    private int nFlowerVertices;

    [SerializeField]
    private float alfa;
    [SerializeField]
    private float scale;
    [SerializeField]
    private float yStepFlower;
    [SerializeField]
    private Vector3 flowerOffset;


    [Header("Stem settings")]
    [SerializeField]
    private float yStepStem;
    [SerializeField]
    private int nStemSegments;
    private int nStemVertices;
    [SerializeField]
    private Vector3 stemOffset;

    private void Start()
    {
        flower = GetComponent<MeshFilter>().mesh;
        Transform child = Instantiate(new GameObject(), transform).transform;
        child.name = "Stem";

        stem = child.gameObject.AddComponent<MeshRenderer>().gameObject.AddComponent<MeshFilter>().mesh;
    }

    private void Update()
    {
        nFlowerVertices = nFlowerSegments * 2 + 2;
        GenFlowerMesh();
        GenStemMesh();
    }

    private void GenStemMesh()
    {
        Vector3[] vertices = GenStemVertices();
        int[] indices = GenStemIndeces();

        flower.vertices = vertices;
        flower.triangles = indices;
    }

    private Vector3[] GenStemVertices()
    {

        Vector3[] vertices = new Vector3[nStemSegments * 8 - 4];

        float y = 0;

        for (int i = 0; i < vertices.Length; i+= 4)
        {
            vertices[i] = new Vector3(-stemOffset.x / 2, y + stemOffset.y, - stemOffset.z / 2);
            vertices[i + 1] = new Vector3(stemOffset.x / 2, y + stemOffset.y, - stemOffset.z / 2);
            vertices[i + 2] = new Vector3(stemOffset.x / 2, y + stemOffset.y, stemOffset.z / 2);
            vertices[i + 3] = new Vector3(-stemOffset.x / 2, y + stemOffset.y, stemOffset.z / 2);

            y += yStepStem;
        }

        foreach (Vector3 vector in vertices)
        {
            Debug.Log(vector);
        }
        return vertices;
    }

    private void OnDrawGizmos()
    {
        Vector3[] vertices = new Vector3[nStemSegments * 8 - 4];

        float y = 0;

        for (int i = 0; i < vertices.Length; i += 4)
        {
            vertices[i] = new Vector3(-stemOffset.x / 2, y + stemOffset.y, -stemOffset.z / 2);
            vertices[i + 1] = new Vector3(stemOffset.x / 2, y + stemOffset.y, -stemOffset.z / 2);
            vertices[i + 2] = new Vector3(stemOffset.x / 2, y + stemOffset.y, stemOffset.z / 2);
            vertices[i + 3] = new Vector3(-stemOffset.x / 2, y + stemOffset.y, stemOffset.z / 2);

            y += yStepStem;
        }

        int j = 0;

        Color[] colors = { Color.red, Color.green, Color.blue, Color.white};

        foreach (Vector3 vector in vertices)
        {
            Gizmos.color = colors[j++ % colors.Length];
            Debug.Log(j % colors.Length);
            Gizmos.DrawWireSphere(vector, 0.01f);
        }
    }

    private int[] GenStemIndeces()
    {
        throw new NotImplementedException();
    }


    #region Flower
    private void GenFlowerMesh()
    {
        Vector3[] vertices = GenFlowerVertices();
        int[] indices = GenFlowerIndeces();

        flower.vertices = vertices;
        flower.triangles = indices;
        FlipFlowerNormals();
    }

    private void FlipFlowerNormals()
    {
        Vector3[] normals = flower.normals;

        for (int i = 0; i < normals.Length; i++)
        {
            if (normals[i].y > 0)
            {
                normals[i] *= -1;
            }
        }

        flower.normals = normals;
    }

    private int[] GenFlowerIndeces()
    {
        //TODO: handle nVerices = 4
        int[] indices = new int[nFlowerVertices * 3 - 6];
        int currentIndex = 0;
        for (int vertIndex = 0; vertIndex < nFlowerVertices - 2; vertIndex += 2)
        {
            indices[currentIndex++] = vertIndex;
            indices[currentIndex++] = vertIndex + 2;
            indices[currentIndex++] = vertIndex + 1;
            indices[currentIndex++] = vertIndex + 2;
            indices[currentIndex++] = vertIndex + 3;
            indices[currentIndex++] = vertIndex + 1;
        }
        return indices;
    }

    private Vector3[] GenFlowerVertices()
    {
        Vector3[] vertices = new Vector3[nFlowerVertices];

        float x = 0, y = 0, z = 0;

        for (int i = 0; i < nFlowerVertices - 1; i += 2)
        {
            float phi = i * alfa;
            float r = scale * Mathf.Sqrt(i);

            Vector3 pos = new Vector3(r * Mathf.Cos(phi), y += yStepFlower, r * Mathf.Sin(phi));
            vertices[i] = pos;
            vertices[i + 1] = pos + flowerOffset;
        }
        return vertices;
    }
    #endregion
}
