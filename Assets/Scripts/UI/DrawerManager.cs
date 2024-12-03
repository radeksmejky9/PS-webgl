using System.Collections;
using UnityEngine;

public class Drawer : MonoBehaviour
{
    [SerializeField] private GameObject drawerParent;
    [SerializeField] private Vector2 closedPosition, openPosition;
    [SerializeField] private AnimationCurve animationCurve;

    private RectTransform m_RT;
    private float endtime;
    private bool isOpen = false;
    private void Start()
    {
        FindFurthestKeyFrame();
        m_RT = GetComponent<RectTransform>();
        closedPosition = m_RT.anchoredPosition;
    }

    public void ToggleDrawer()
    {
        if (isOpen)
        {
            StartCoroutine(CloseRoutine());
        }
        else
        {
            StartCoroutine(OpenRoutine());
        }
    }

    private IEnumerator OpenRoutine()
    {
        float elapsedTime = 0;
        while (elapsedTime < endtime)
        {
            m_RT.anchoredPosition = Vector3.Lerp(closedPosition, openPosition, animationCurve.Evaluate(elapsedTime));
            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }
        isOpen = true;
    }
    private IEnumerator CloseRoutine()
    {
        float elapsedTime = 0;
        while (elapsedTime < endtime)
        {
            m_RT.anchoredPosition = Vector3.Lerp(openPosition, closedPosition, animationCurve.Evaluate(elapsedTime));
            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }
        isOpen = false;
    }
    private void FindFurthestKeyFrame()
    {
        float maxTime = Mathf.NegativeInfinity;
        foreach (Keyframe frame in animationCurve.keys)
        {
            if (frame.time > maxTime)
                maxTime = frame.time;
        }
        endtime = maxTime;
    }
}
