using UnityEngine;
using Utils;

public class ManualDemo : MonoBehaviour
{
    private int x1;
    private int x2;
    private int y1;
    private int y2;

    void Awake()
    {
        TransparentUtils.Init(TransparentUtils.Mode.Manual, false);

        x1 = Screen.width / 2 - Screen.width / 5;
        x2 = Screen.width / 2 + Screen.width / 5;
        y1 = Screen.height / 2 - Screen.height / 5;
        y2 = Screen.height / 2 + Screen.height / 5;
    }

    void LateUpdate()
    {
        Vector3 cursor = Input.mousePosition;

        if ((x1 < cursor.x && cursor.x < x2) && (y1 < cursor.y && cursor.y < y2))
            TransparentUtils.SetClickThrough(false);
        else
            TransparentUtils.SetClickThrough(true);
    }
}
