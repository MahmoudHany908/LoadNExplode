using System;
using UnityEngine;
using UnityEngine.UI;

public class DeathUIController : MonoBehaviour
{
    [SerializeField] private Button btn_MainMenu;
    [SerializeField] private Button restart_MainMenu;
    //add more buttons here if needed


    private void OnEnable()
    {
        btn_MainMenu.onClick.AddListener(OnHubButtonClicked);
        restart_MainMenu.onClick.AddListener(OnRestartButtonClicked);
    }

    private void OnDisable()
    {
        btn_MainMenu.onClick.RemoveListener(OnHubButtonClicked);
        restart_MainMenu.onClick.RemoveListener(OnRestartButtonClicked);
    }

    private void OnHubButtonClicked()
    {
        //both work fine
        //SceneLoader.LoadScene("MainMenuScene"); 

        EventBus.Publish(new RequestSceneLoadEvent("MainMenuScene"));
    }

    private void OnRestartButtonClicked()
    {
        EventBus.Publish(new OnRestartGameButtonPressedEvent());
    }
}
