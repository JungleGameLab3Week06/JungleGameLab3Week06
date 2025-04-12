using UnityEngine;

public class BGFX : MonoBehaviour
{
    [SerializeField] float degree = 0;

    void Update()
    {
        degree += Time.deltaTime * 10f;
        transform.position = 3.5f * Mathf.Cos(degree * (Mathf.PI / 180)) * Vector3.right;
        if(degree > 360f) degree = 0f;
    }
}