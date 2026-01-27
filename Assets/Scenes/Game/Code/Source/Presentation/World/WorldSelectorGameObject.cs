using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldSelectorGameObject : MonoBehaviour
{
    public Animator animator;

    public List<WorldGameObject> worldGOList;

    public void SetUp(Action<WorldEnum> worldAction)
    {
        worldGOList.ForEach(w =>
        {
            w.action = worldAction;
        });
    }

    public void Appear(bool appear)
    {
       animator.SetBool("Appear", appear);
    }
}
