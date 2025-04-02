using UnityEngine;
using Utils;

public class ChromaDemo : MonoBehaviour
{
    [SerializeField]
    private Color32 color;

    void Awake()
    {
        TransparentUtils.Init(TransparentUtils.Mode.Chroma, true);
        TransparentUtils.SetChromaKey(color.r, color.g, color.b);
    }
}
