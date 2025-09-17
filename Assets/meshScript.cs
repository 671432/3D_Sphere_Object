using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.IO;
using System.Text;
using System.Globalization;
using System.Linq;

public class meshScript : MonoBehaviour
{

    void Start()
    {
        // programatically create meshfilter and meshrenderer and add to gameobject this script is attached to.
        GameObject go = gameObject; // GameObject.Find("GameObjectDp");
        MeshFilter meshFilter = (MeshFilter)go.AddComponent(typeof(MeshFilter));
        MeshRenderer renderer = go.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
    }

    public void createMeshGeometry(List<Vector3> vertices, List<int> indices, bool useTriangles = false)
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.SetVertices(vertices);

        if (useTriangles)
        {
            mesh.SetIndices(indices.ToArray(), MeshTopology.Triangles, 0);
            mesh.RecalculateNormals();
        }
        else
        {
            mesh.SetIndices(indices.ToArray(), MeshTopology.Lines, 0);
        }

        mesh.RecalculateBounds();
    }

    // the code below is for saving the mesh to file in .obj format (see https://en.wikipedia.org/wiki/Wavefront_.obj_file) which can be loaded by e.g. meshlab
    public void MeshToFile(string filename)
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        using (StreamWriter sw = new StreamWriter(filename))
            sw.Write(MeshToString(mf));
        print("Mesh saved to file: " + filename);
    }

    private string MeshToString(MeshFilter mf)
    {
        Mesh m = mf.mesh;
        StringBuilder sb = new StringBuilder();

        sb.Append("g ").Append(mf.name).Append("\n");
        foreach (Vector3 v in m.vertices)
            sb.AppendFormat(CultureInfo.InvariantCulture, "v {0} {1} {2}\n", v.x, v.y, v.z);  //  sb.AppendFormat(string.Format(CultureInfo.InvariantCulture, "v {0} {1} {2}\n", v.x, v.y, v.z));

        sb.Append("\n");
        foreach (Vector3 v in m.normals)
            sb.AppendFormat(CultureInfo.InvariantCulture, "vn {0} {1} {2}\n", v.x, v.y, v.z);//sb.AppendFormat(string.Format(CultureInfo.InvariantCulture,"vn {0} {1} {2}\n", v.x, v.y, v.z));

        sb.Append("\n");
        foreach (Vector3 v in m.uv)
            sb.AppendFormat(CultureInfo.InvariantCulture, "vt {0} {1}\n", v.x, v.y);  //sb.AppendFormat(string.Format(CultureInfo.InvariantCulture,"vt {0} {1}\n", v.x, v.y));

        int[] triangles = m.GetIndices(0);
        for (int i = 0; i < triangles.Length; i += 3)
            sb.AppendFormat(CultureInfo.InvariantCulture, "f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n", triangles[i] + 1, triangles[i + 1] + 1, triangles[i + 2] + 1);

        //sb.AppendFormat(string.Format(CultureInfo.InvariantCulture,"f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n", triangles[i] + 1, triangles[i + 1] + 1,  triangles[i + 2] + 1));

        return sb.ToString();
    }

    public void MeshToFile1(string filename, ref List<Vector3> vertices, ref List<int> indices)
    {
        using (StreamWriter sw = new StreamWriter(filename))
            sw.Write(MeshToString1(ref vertices, ref indices));
        print("Mesh saved to file: " + filename);
    }

    private string MeshToString1(ref List<Vector3> vertices, ref List<int> triangles)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("g ").Append("mesh").Append("\n");
        foreach (Vector3 v in vertices)
            sb.AppendFormat(CultureInfo.InvariantCulture, "v {0} {1} {2}\n", v.x, v.y, v.z);  //  sb.AppendFormat(string.Format(CultureInfo.InvariantCulture, "v {0} {1} {2}\n", v.x, v.y, v.z));

        for (int i = 0; i < triangles.Count; i += 3)
            sb.AppendFormat(CultureInfo.InvariantCulture, "f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n", triangles[i] + 1, triangles[i + 1] + 1, triangles[i + 2] + 1);
        //sb.AppendFormat(string.Format(CultureInfo.InvariantCulture,"f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n", triangles[i] + 1, triangles[i + 1] + 1,  triangles[i + 2] + 1));
        return sb.ToString();
    }

}