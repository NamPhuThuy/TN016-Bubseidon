using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScreenSettings : UIScreenBase
{
    public override void Show()
    {
        gameObject.SetActive(true);
    }

    public override void Hide()
    {
        gameObject.SetActive(false);
    }
}
