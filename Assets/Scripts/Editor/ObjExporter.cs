using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Globalization;

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

public class ObjExporter : ScriptableObject
{
    [MenuItem("File/Export/Wavefront OBJ")]
    static void DoExportWSubmeshes()
    {
        DoExport(true);
    }

    [MenuItem("File/Export/Wavefront OBJ (No Submeshes)")]
    static void DoExportWOSubmeshes()
    {
        DoExport(false);
    }


    static void DoExport(bool makeSubmeshes)
    {
        if (Selection.gameObjects.Length == 0)
        {
            Debug.Log("Didn't Export Any Meshes; Nothing was selected!");
            return;
        }

        string meshName = Selection.gameObjects[0].name;
        string fileName = EditorUtility.SaveFilePanel("Export .obj file", "", "Model", "obj");

        ObjExporterScript.Start();

        var meshString = new StringBuilder();

        meshString.Append($"#{meshName}.obj\n\n");

        Transform t = Selection.gameObjects[0].transform;

        Vector3 originalPosition = t.position;
        t.position = Vector3.zero;

        if (!makeSubmeshes)
        {
            meshString.Append("g ").Append(t.name).Append("\n");
        }
        meshString.Append(ProcessTransform(t, makeSubmeshes));

        WriteToFile(meshString.ToString(), fileName);

        t.position = originalPosition;

        ObjExporterScript.End();
        Debug.Log("Exported Mesh: " + fileName);
    }

    static string ProcessTransform(Transform t, bool makeSubmeshes)
    {
        StringBuilder meshString = new StringBuilder();

        meshString.Append("#" + t.name
                        + "\n#-------"
                        + "\n");

        if (makeSubmeshes)
        {
            meshString.Append("g ").Append(t.name).Append("\n");
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

    static void WriteToFile(string s, string filename)
    {
        using var sw = new StreamWriter(filename);
        sw.Write(s);
    }
}