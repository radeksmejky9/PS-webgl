using TMPro;
using UnityEngine;


public class DebugText : MonoBehaviour
{
    [SerializeField]
    private GameObject debug;
    [SerializeField]
    private Transform assets;
    [SerializeField]
    private TextMeshProUGUI debugPositionText;
    [SerializeField]
    private TextMeshProUGUI debugConsoleText;

    private Transform cam;
    private string logMessages = "";
    private void Start()
    {
        cam = Camera.main.transform;
    }

    private void Update()
    {
        UpdateDebugText();
    }

    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;

    }

    public void ActivateDebug()
    {
        debug.SetActive(!debug.activeSelf);
    }
    private void UpdateDebugText()
    {
        if (!debug.activeSelf) return;

        debugPositionText.text = @$"<b><color=#FFD700>Assets Position:</color></b>
X: {assets.position.x}
Y: {assets.position.y}
Z: {assets.position.z}
<b><color=#FFD700>Assets Rotation:</color></b>
X: {assets.rotation.x}
Y: {assets.rotation.y}
Z: {assets.rotation.z}
<b><color=#00FF00>Cam Position:</color></b>
X: {cam.position.x}
Y: {cam.position.y}
Z: {cam.position.z}
<b><color=#00FF00>Cam Rotation:</color></b>
X: {cam.rotation.x}
Y: {cam.rotation.y}
Z: {cam.rotation.z}".Trim();
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Error || type == LogType.Exception)
        {
            logMessages += $"<color=red>{logString}</color>\n";
            logMessages += $"<color=red>{stackTrace}</color>\n";
        }
        else if (type == LogType.Log)
        {
            logMessages += logString + "\n";
        }
        debugConsoleText.text = logMessages;
    }
}
