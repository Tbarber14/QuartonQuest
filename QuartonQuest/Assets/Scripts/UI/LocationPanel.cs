using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationPanel : MonoBehaviour
{
    public void OnHoverToggle()
    {
        if (AudioManager.instance != null)
            AudioManager.instance.PlaySoundEffect("ButtonHover");
    }

    public void OnClickToggle()
    {
        if (AudioManager.instance != null)
            AudioManager.instance.PlaySoundEffect("ButtonClick");
    }
}
