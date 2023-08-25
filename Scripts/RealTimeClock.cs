using System;
using UnityEngine;
using UnityEngine.UI;

public class RealTimeClock : MonoBehaviour
{
    public RectTransform hoursTransform, minutesTransform, secondsTransform;
    private const float hoursToDegrees = 360f / 12f, minutesToDegrees = 360f / 60f, secondsToDegrees = 360f / 60f;

    private void Update()
    {
        DateTime time = DateTime.Now;
        hoursTransform.localRotation = Quaternion.Euler(0f, 0f, -hoursToDegrees * (time.Hour + time.Minute / 60f));
        minutesTransform.localRotation = Quaternion.Euler(0f, 0f, -minutesToDegrees * time.Minute);
        secondsTransform.localRotation = Quaternion.Euler(0f, 0f, -secondsToDegrees * time.Second);
    }
}