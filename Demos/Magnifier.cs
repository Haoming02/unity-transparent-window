using UnityEngine;
using UnityEngine.UI;

/// <summary>Make an UI.Image follow the cursor</summary>
public class Magnifier : MonoBehaviour
{
    private Image handle;

    void Start() { handle = GetComponent<Image>(); }
    void Update() { handle.rectTransform.position = Input.mousePosition; }
}
