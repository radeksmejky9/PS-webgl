using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class GenerateQR : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void UploadQR(float x, float y, float z, float yRotation);

    [SerializeField] private GameObject QRCube;
    private void OnEnable()
    {
        QRCube = GameObject.Find("QRCube(Clone)");
    }

    public void GenerateQRCode()
    {
        UploadQR(QRCube.transform.position.x, QRCube.transform.position.y, QRCube.transform.position.z, QRCube.transform.rotation.y);
    }
}
