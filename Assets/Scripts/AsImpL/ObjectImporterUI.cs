using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AsImpL
{
    /// <summary>
    /// UI controller for <see cref="ObjectImporter"/>
    /// </summary>
    [RequireComponent(typeof(ObjectImporter))]
    public class ObjectImporterUI : MonoBehaviour
    {
        [Tooltip("Text for activity messages")]
        public TextMeshProUGUI progressText;

        [Tooltip("Text for time taken")]
        public TextMeshProUGUI timeText;

        [Tooltip("Slider for the overall progress")]
        public Slider progressSlider;

        [Tooltip("Background image.")]
        public Image backgroundImage;

        [SerializeField]
        private ObjectImporter objImporter;

        private float elapsedTime;

        private void Awake()
        {
            if (progressSlider != null)
            {
                progressSlider.maxValue = 100f;
                progressSlider.gameObject.SetActive(false);
            }
            if (backgroundImage != null)
            {
                backgroundImage.gameObject.SetActive(false);
            }
            if (progressText != null)
            {
                progressText.gameObject.SetActive(false);
            }
            if (timeText != null)
            {
                timeText.gameObject.SetActive(false);
            }
            //objImporter = GetComponent<ObjectImporter>();
            // TODO: check and warn
        }

        private void OnEnable()
        {
            ObjectImporter.ImportingComplete += OnImportComplete;
            ObjectImporter.ImportingStart += OnImportStart;
        }

        private void OnDisable()
        {
            ObjectImporter.ImportingComplete -= OnImportComplete;
            ObjectImporter.ImportingStart -= OnImportStart;
        }

        private void Update()
        {
            bool loading = Loader.totalProgress.singleProgress.Count > 0;
            if (!loading)
            {
                elapsedTime = 0f;
                return;
            }

            elapsedTime += Time.deltaTime;

            int numTotalImports = objImporter.NumImportRequests;
            int numImportCompleted = numTotalImports - Loader.totalProgress.singleProgress.Count;

            if (loading)
            {
                float progress = 100.0f * numImportCompleted / numTotalImports;
                float maxSubProgress = 0.0f;
                foreach (SingleLoadingProgress progr in Loader.totalProgress.singleProgress)
                {
                    if (maxSubProgress < progr.percentage)
                    {
                        maxSubProgress = progr.percentage;
                    }
                }
                progress += maxSubProgress / numTotalImports;
                if (progressSlider != null)
                {
                    progressSlider.value = progress;
                    progressSlider.gameObject.SetActive(loading);
                }
                if (backgroundImage != null)
                {
                    backgroundImage.gameObject.SetActive(loading);
                }
                if (progressText != null)
                {
                    if (loading)
                    {
                        progressText.gameObject.SetActive(loading);
                        var objectCount = Loader.totalProgress.singleProgress.Count;
                        progressText.text = $"Loading {objectCount} object{(objectCount == 1 ? string.Empty : 's')}...";
                        string loadersText = "";
                        int count = 0;
                        foreach (SingleLoadingProgress i in Loader.totalProgress.singleProgress)
                        {
                            if (count > 4) // maximum 4 messages
                            {
                                loadersText += "...";
                                break;
                            }
                            if (!string.IsNullOrEmpty(i.message))
                            {
                                if (count > 0)
                                {
                                    loadersText += "; ";
                                }
                                loadersText += i.message;
                                count++;
                            }
                        }
                        if (loadersText != "")
                        {
                            progressText.text += "\n" + loadersText;
                        }
                    }
                    else
                    {
                        progressText.gameObject.SetActive(false);
                        progressText.text = "";
                    }
                }
                if (timeText != null)
                {
                    timeText.gameObject.SetActive(loading);
                    timeText.text = FormatTime(elapsedTime);
                }
            }
            else
            {
                OnImportComplete();
            }
        }

        private void OnImportStart()
        {
            if (progressText != null)
            {
                progressText.text = "";
            }
            if (progressSlider != null)
            {
                progressSlider.value = 0.0f;
                progressSlider.gameObject.SetActive(true);
            }
            if (backgroundImage != null)
            {
                backgroundImage.gameObject.SetActive(true);
            }
        }

        private void OnImportComplete()
        {
            if (progressText != null)
            {
                progressText.text = "";
            }
            if (progressSlider != null)
            {
                progressSlider.value = 100.0f;
                progressSlider.gameObject.SetActive(false);
            }
            if (backgroundImage != null)
            {
                backgroundImage.gameObject.SetActive(false);
            }
        }
        private string FormatTime(float time)
        {
            if (time < 60)
            {
                return $"{time:F2}s";
            }
            else if (time < 3600)
            {
                int minutes = Mathf.FloorToInt(time / 60);
                int seconds = Mathf.FloorToInt(time % 60);
                return $"{minutes}min {seconds}s";
            }
            else
            {
                int hours = Mathf.FloorToInt(time / 3600);
                int minutes = Mathf.FloorToInt((time % 3600) / 60);
                return $"{hours}h {minutes}min";
            }
        }
    }
}
