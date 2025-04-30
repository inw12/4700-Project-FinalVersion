using UnityEngine;

public class SpriteFlip : MonoBehaviour
{
    public bool FacingLeft { get { return isFacingLeft; } set { isFacingLeft = value; } }
    private bool isFacingLeft = false;
    private SpriteRenderer mySpriteRenderer;

    private void Awake() {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate() {
        AdjustFacingDirection();
    }

    private void AdjustFacingDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerScreenPoint.x) {
            mySpriteRenderer.flipY = true;
            FacingLeft = true;
        } else {
            mySpriteRenderer.flipY = false;
            FacingLeft = false;
        }
    }
}
