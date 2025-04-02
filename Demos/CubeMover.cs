using UnityEngine;

/// <summary>Move a Cube...</summary>
public class CubeMover : MonoBehaviour
{
    private static Vector3 position = Vector3.zero;
    private const int distance = 5;
    private const int speed = 50;

    void Update()
    {
        transform.Rotate(speed * Time.deltaTime * Vector3.one);
        position.x = distance * Mathf.Sin(Time.time);
        transform.position = position;
    }
}
