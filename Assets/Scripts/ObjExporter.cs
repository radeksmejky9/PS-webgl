using UnityEngine;
using System.IO;
using System.Text;
using System.Globalization;
using System;
using System.Runtime.InteropServices;

public class ObjExporterWrapper : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern int saveModel(string fileID, string file);

    public GameObject ModelParent;

    public void DoExport()
    {
        string meshName = ModelParent.GetComponentInChildren<Transform>().gameObject.name;
        string downloadsFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Downloads";

        string fileName = $"{meshName}.obj";
        string filePath = Path.Combine(downloadsFolder, fileName);

        ObjExporterScript.Start();

        var meshString = new StringBuilder();

        meshString.Append($"#{meshName}.obj\n\n");

        Transform t = ModelParent.GetComponentInChildren<Transform>().transform;

        Vector3 originalPosition = t.position;
        t.position = Vector3.zero;

        meshString.Append(ProcessTransform(t, true));

        //WriteToFile(meshString.ToString(), filePath);
        saveModel(ContentLoader.Instance.UID, meshString.ToString());

        t.position = originalPosition;

        ObjExporterScript.End();
        Debug.Log("Exported Mesh: " + fileName);
    }

    private string ProcessTransform(Transform t, bool makeSubmeshes)
    {
        StringBuilder meshString = new StringBuilder();

        meshString.Append("#" + t.name
                        + "\n#-------"
                        + "\n");

        if (makeSubmeshes)
        {
            meshString.Append("o ").Append(t.name).Append("\n");
        }

        MeshFilter mf = t.GetComponent<MeshFilter>();
        if (mf)
        {
            meshString.Append(ObjExporterScript.MeshToString(mf, t));
        }

        for (int i = 0; i < t.childCount; i++)
        {
            meshString.Append(ProcessTransform(t.GetChild(i), makeSubmeshes));
        }

        return meshString.ToString();
    }

    private void WriteToFile(string s, string filename)
    {
        using var sw = new StreamWriter(filename);
        sw.Write(s);
    }

}

public class ObjExporterScript
{
    private static int StartIndex = 0;

    public static void Start()
    {
        StartIndex = 0;
    }
    public static void End()
    {
        StartIndex = 0;
    }

    public static string MeshToString(MeshFilter mf, Transform t)
    {
        int numVertices = 0;
        Mesh m = mf.sharedMesh;
        if (!m)
        {
            return "####Error####";
        }
        Material[] mats = mf.GetComponent<Renderer>().sharedMaterials;

        var objString = new StringBuilder();

        foreach (Vector3 vv in m.vertices)
        {
            Vector3 v = t.TransformPoint(vv);
            numVertices++;
            objString.Append(string.Format(CultureInfo.InvariantCulture, "v {0} {1} {2}\n", v.x, v.y, v.z));
        }
        objString.Append("\n");

        Quaternion r = t.localRotation;
        foreach (Vector3 nn in m.normals)
        {
            Vector3 v = r * nn;
            objString.Append(string.Format(CultureInfo.InvariantCulture, "vn {0} {1} {2}\n", v.x, v.y, v.z));
        }
        objString.Append("\n");

        foreach (Vector3 v in m.uv)
        {
            objString.Append(string.Format(CultureInfo.InvariantCulture, "vt {0} {1}\n", v.x, v.y));
        }
        for (int material = 0; material < m.subMeshCount; material++)
        {
            objString.Append("\n");
            objString.Append("usemtl ").Append(mats[material].name).Append("\n");
            objString.Append("usemap ").Append(mats[material].name).Append("\n");

            int[] triangles = m.GetTriangles(material);
            for (int i = 0; i < triangles.Length; i += 3)
            {
                var t0 = triangles[i] + 1 + StartIndex;
                var t1 = triangles[i + 1] + 1 + StartIndex;
                var t2 = triangles[i + 2] + 1 + StartIndex;
                objString.Append(string.Format(CultureInfo.InvariantCulture, "f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n", t0, t1, t2));
            }
        }

        StartIndex += numVertices;
        return objString.ToString();
    }
}