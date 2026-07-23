using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button btn_StartGame;
    private void Awake()
    {
        btn_StartGame.onClick.AddListener(OnStartGameButtonClicked);
    }


    private void OnStartGameButtonClicked()
    {
        EventBus.Publish(new RequestSceneLoadEvent("GameScene"));
        //SceneLoader.LoadScene("GameScene");
    }
}
