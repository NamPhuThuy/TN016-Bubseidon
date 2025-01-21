using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveable
{
    float MoveSpeed { get; set; }
    Vector2 MoveDirection { get; set; }
    void MovementHandle();
}
