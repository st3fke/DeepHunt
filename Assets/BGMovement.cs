using UnityEngine;
using UnityEngine.UI;

public class BGMovement : MonoBehaviour
{
    public RawImage RawImage;
    public float x,y;
    void Update()
    {
        RawImage.uvRect = new Rect(RawImage.uvRect.position + new Vector2(x,y)*Time.deltaTime, RawImage.uvRect.size);
    }
}
