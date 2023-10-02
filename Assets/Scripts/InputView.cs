using TMPro;
using UnityEngine;

public class InputView : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI _inputText;

    public void ShowText(string value) {
        _inputText.text = value;
    }
}
