using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    void SaveDataOnSceneChange();

    void LoadDataOnSceneEnter();
}
