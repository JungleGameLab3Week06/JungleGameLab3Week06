using UnityEngine;

public class BGFX : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] float degree = 0;

    // Update is called once per frame
    void Update()
    {
        degree += Time.deltaTime * 10f;
        transform.position = 3.5f * Mathf.Cos(degree * (Mathf.PI / 180)) * Vector3.right;
        if(degree > 360f) degree = 0f;
    }
}
