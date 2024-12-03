using System;
using UnityEngine;

[System.Serializable]
public class SnappingPoint
{
    public string Building;
    public string Room;
    public SerializableVector3 Position;
    public float Rotation;
    public string Url;

    public SnappingPoint(string building, string name, Vector3 posistion, float rotation, string url)
    {
        Building = building;
        Room = name;
        Position = posistion;
        Rotation = rotation;
        Url = url;
    }
    public static string Encode(SnappingPoint sp)
    {
        return $"" +
             $"{sp.Building};{sp.Room};{Math.Floor(sp.Position.x * 100)};" +
             $"{Math.Floor(sp.Position.y * 100)};" +
             $"{Math.Floor(sp.Position.z * 100)};" +
             $"{Math.Floor(sp.Rotation * 100)};{sp.Url}";
    }
    public static SnappingPoint Decode(string input)
    {
        string[] fields = input.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        string building = fields[0];
        string room = fields[1];
        Vector3 position = new Vector3(float.Parse(fields[2]) / 100, float.Parse(fields[3]) / 100, float.Parse(fields[4]) / 100);
        float rotation = float.Parse(fields[5]) / 100;
        string url = fields[6];

        return new SnappingPoint(building, room, position, rotation, url);
    }

    [System.Serializable]
    public class SerializableVector3
    {
        public float x;
        public float y;
        public float z;

        public SerializableVector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static implicit operator SerializableVector3(Vector3 v)
        {
            return new SerializableVector3(v.x, v.y, v.z);
        }

        public static implicit operator Vector3(SerializableVector3 sv)
        {
            return new Vector3(sv.x, sv.y, sv.z);
        }
    }
}
