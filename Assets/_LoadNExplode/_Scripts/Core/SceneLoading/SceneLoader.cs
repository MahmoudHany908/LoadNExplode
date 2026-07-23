using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    [Header("References")]
    [SerializeField] private GameObject loadingScreenPrefab;

    private void Awake()
    {
        transform.parent = null;
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {

        EventBus.Subscribe<RequestSceneLoadEvent>(OnSceneLoadRequested);
    }

    private void OnDisable()
    {

        EventBus.Unsubscribe<RequestSceneLoadEvent>(OnSceneLoadRequested);
    }


    private void OnSceneLoadRequested(RequestSceneLoadEvent evt)
    {
        LoadScene(evt.SceneName);
    }

    public static void LoadScene(string sceneName)
    {
        if (Instance == null)
        {
            Debug.LogError("SceneLoader Instance is missing! Make sure it's in your Bootstrap scene.");
            return;
        }
        Instance.StartCoroutine(Instance.LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        EventBus.Publish(new SceneLoadStartEvent(sceneName));

        GameObject screenObj = Instantiate(loadingScreenPrefab);
        LoadingScreen loadingScreen = screenObj.GetComponent<LoadingScreen>();
        yield return loadingScreen.ShowAsync();

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingScreen.UpdateProgress(progress);


            //  it can cause lag.
            // EventBus.Publish(new SceneLoadProgressEvent(progress)); 

            yield return null;
        }

        yield return loadingScreen.HideAsync();
        Destroy(screenObj);

        EventBus.Publish(new SceneLoadCompleteEvent(sceneName));
    }
}