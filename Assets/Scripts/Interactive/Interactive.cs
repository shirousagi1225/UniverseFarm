using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    public ItemName requireItem;

    private bool isDone;

    public void CheckItem(ItemName itemName)
    {
        if (itemName== requireItem&& !isDone)
        {
            isDone=true;
            OnClickedAction();
        }
    }

    protected virtual void OnClickedAction()
    {

    }
}
