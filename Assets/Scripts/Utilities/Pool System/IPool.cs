using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPool
{
    void SettingObject();
    void SettingObject(Transform user);
    void RecycleObject();
}
