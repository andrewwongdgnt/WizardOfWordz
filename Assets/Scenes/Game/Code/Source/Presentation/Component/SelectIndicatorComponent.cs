using TMPro;
using UnityEngine;

public class SelectIndicatorComponent : MonoBehaviour
{
    public TextMeshProUGUI text;

    public void Apply(bool isSelected)
    {
        text.text = isSelected ? "V" : "";
    }
}
