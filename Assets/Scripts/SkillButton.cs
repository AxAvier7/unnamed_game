using System;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(HabilidadButtonClick);
    }

    private void HabilidadButtonClick()
    {
        if(TurnManager.Instance.FichaSeleccionadaCont != null && TurnManager.Instance.FichaSeleccionada.CanUseSkill)
            TurnManager.Instance.FichaSeleccionadaCont.GetComponent<FichaController>().UsarHabilidad();
    }
}
