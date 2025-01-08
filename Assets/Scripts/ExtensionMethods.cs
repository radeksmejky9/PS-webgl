using System.Collections;
using UnityEngine;

public static class Extensions
{
    public static ModelElement GetClosestPipe(this Transform source, ModelElement[] pipes)
    {
        float sphereRadius = 0.001f;
        float maxRadius = 1.0f;
        float radiusIncrement = 0.001f;

        Vector3 sourcePosition = source.position;

        ModelElement closestObject = null;
        float closestDistance = Mathf.Infinity;


        Collider[] initialHitColliders = Physics.OverlapSphere(sourcePosition, maxRadius);
        if (initialHitColliders.Length == 0)
        {
            return null;
        }

        while (sphereRadius <= maxRadius)
        {
            Collider[] hitColliders = Physics.OverlapSphere(sourcePosition, sphereRadius);

            foreach (Collider collider in hitColliders)
            {
                ModelElement hitPipe = collider.GetComponent<ModelElement>();

                if (hitPipe != null && hitPipe.ModelType.Type == "Pipe" && System.Array.Exists(pipes, pipe => pipe == hitPipe))
                {
                    Vector3 directionToTarget = hitPipe.transform.position - sourcePosition;
                    float dSqrToTarget = directionToTarget.sqrMagnitude;

                    if (dSqrToTarget < closestDistance)
                    {
                        closestDistance = dSqrToTarget;
                        closestObject = hitPipe;
                    }
                }
            }
            if (closestObject != null) return closestObject;
            sphereRadius += radiusIncrement;
        }
        return null;
    }

    private static void CreateDebugSphere(Vector3 sourcePosition, float sphereRadius)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = sourcePosition;
        sphere.transform.localScale = new Vector3(sphereRadius, sphereRadius, sphereRadius);
    }
    public static string Debug(this IEnumerable array)
    {
        if (array == null)
            return "The collection is null";

        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        foreach (var item in array)
        {
            if (item == null)
            {
                sb.AppendLine("Item is null");
                continue;
            }

            var nameProperty = item.GetType().GetProperty("name");
            if (nameProperty != null)
            {
                string name = nameProperty.GetValue(item)?.ToString() ?? "null";
                sb.AppendLine($"Name: {name}");
            }
            else
            {
                sb.AppendLine(item.ToString());
            }
        }
        return sb.ToString();
    }

    public static string Base64Encode(this string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }

    public static string Base64Decode(this string base64EncodedData)
    {
        var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }

    public static string RemoveWhiteSpace(this string str)
    {
        return str.Trim().Replace(" ", "_");
    }
}
