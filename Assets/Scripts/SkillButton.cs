using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(HabilidadButtonClick);
    }

    private void HabilidadButtonClick()
    {
        FichaController controller = TurnManager.Instance.FichaSeleccionadaCont.GetComponent<FichaController>();
        if(controller != null && TurnManager.Instance.FichaSeleccionada.CanUseSkill)
        {
            controller.UsarHabilidad();
        }
    }
}
