using UnityEngine;

public class AspectRatio : MonoBehaviour
{
    private readonly float targetAspectRatio = 4f / 3f;

    void Start() {
        UpdateCamera();
    }

    void UpdateCamera() {
        float screenAspect = (float)Screen.width / Screen.height;
        float scaleHeight = screenAspect / targetAspectRatio;

        Camera cam = GetComponent<Camera>();

        // cut off top/bottom
        if (scaleHeight < 1.0f) {
            Rect rect = cam.rect;
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
            cam.rect = rect;
        }
        // cut off left/right
        else {
            float scaleWidth = 1.0f / scaleHeight;
            Rect rect = cam.rect;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
            cam.rect = rect;
        }
    }
}
