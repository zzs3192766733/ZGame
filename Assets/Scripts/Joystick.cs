using UnityEngine;
using UnityEngine.UI;

public class Joystick : ScrollRect
{
    private float radius = 0f;
    public Vector3 stickPos;

    protected override void Start()
    {
        base.Start();
        radius = viewRect.rect.width / 2;
    }

    private void Update()
    {
        if (content.localPosition.magnitude > radius)
            content.localPosition = content.localPosition.normalized * radius;
        stickPos = content.localPosition.normalized;
    }
}
