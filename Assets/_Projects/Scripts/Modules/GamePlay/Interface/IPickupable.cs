using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickupable
{
    public bool isBeingPicked { get; set; }
    void OnPickUp(Transform hand);
    void OnDropDown(Vector3Int pos);

    /*// public delegate Action OnEnemyBeingPicked();
    public event Action OnBeingPickedEvent;

    // public delegate Action OnEnemyBeingDrop();
    public event Action OnBeingDropEvent;

    public void OnBeingPicked()
    {
        OnBeingPickedEvent?.Invoke();
    }*/
}
