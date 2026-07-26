using System.Collections;
using _LoadNExplode._Scripts.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }
    
    private LoadingScreen loadingScreen;

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

        loadingScreen = GetComponentInChildren<LoadingScreen>();
        loadingScreen.Disable();
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

        yield return loadingScreen.ShowAsync();

        if (sceneName == "GameScene") {
            if (MusicManager.Instance != null) MusicManager.Instance.PlayGameLoopMusic(); //this is not good code, but good for now
        } else if (sceneName == "MainMenuScene") {
            if (MusicManager.Instance != null) MusicManager.Instance.PlayMainMenuMusic();
        }

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
        
        EventBus.Publish(new SceneLoadCompleteEvent(sceneName));
    }
}