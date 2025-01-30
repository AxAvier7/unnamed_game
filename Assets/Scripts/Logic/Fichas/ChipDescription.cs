using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChipDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string description;
    public GameObject descriptionPrefab;
    private GameObject descriptionInstance;
    private bool isActive = false;

    void Start()
    {
        if (descriptionPrefab != null)
        {
            descriptionInstance = Instantiate(descriptionPrefab, FindObjectOfType<Canvas>().transform);
            descriptionInstance.SetActive(false);
        }
    }

    void Update()
    {
        if (descriptionInstance != null && descriptionInstance.activeSelf)
        {
            Vector2 mousePosition = new Vector2(Input.mousePosition.x + 110f, Input.mousePosition.y + 110f);
            descriptionInstance.transform.position = mousePosition;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!isActive)
        {        
            if (descriptionInstance != null)
            {
                descriptionInstance.SetActive(true);
                descriptionInstance.GetComponentInChildren<Text>().text = description;
                isActive = true;
            }
        }    
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (descriptionInstance != null)
        {
            descriptionInstance.SetActive(false);
            isActive = false;
        }
    }
}
