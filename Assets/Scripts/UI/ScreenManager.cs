using AsImpL;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public GameObject mainScreen;
    public GameObject loadingScreen;
    private void OnEnable()
    {
        ContentLoader.OnModelLoadStart += OnModelLoadStart;
        MultiObjectImporter.ImportedModel += OnModelImported;
    }

    private void OnDisable()
    {
        ContentLoader.OnModelLoadStart -= OnModelLoadStart;
        MultiObjectImporter.ImportedModel -= OnModelImported;
    }

    private void OnModelLoadStart()
    {
        mainScreen.SetActive(false);
        loadingScreen.SetActive(true);
    }
    private void OnModelImported(GameObject obj, string message)
    {
        mainScreen.SetActive(true);
        loadingScreen.SetActive(false);
    }
}
