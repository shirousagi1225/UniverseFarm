using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleItem : MonoBehaviour
{
    public ClientName clientName;

    public void RoleItemClicked()
    {
        PokedexManager.Instance.ShowCustomerPokedex(clientName);
    }
}
