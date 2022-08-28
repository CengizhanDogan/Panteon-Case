using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClampable
{
    bool CheckClamp { get; set; }
    bool DoClamp(Vector2 anchor);
}
