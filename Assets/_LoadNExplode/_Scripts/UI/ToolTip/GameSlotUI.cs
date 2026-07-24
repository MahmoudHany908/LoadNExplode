using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

//WIP
public class GameSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Data")]
    [SerializeField] private AbilityData data;




    // [SerializeField] private GameTooltipUI tooltipManager;

    void Start()
    {
        // Find the tooltip in the scene (Or use a Singleton/EventSystem)

        UpdateVisuals();

    }

    public void SetData(AbilityData newData)
    {
        data = newData;
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {

        // hover effect maybe change the background color or add a highlight effect


    }



    // --- Event Triggers ---

    public void OnPointerEnter(PointerEventData eventData)
    {
        //if (data != null && tooltipManager != null)
        //{
        //    tooltipManager.Show(data, eventData.position);
        //}
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //if (tooltipManager != null)
        //{
        //    tooltipManager.Hide();
        //}
    }


}