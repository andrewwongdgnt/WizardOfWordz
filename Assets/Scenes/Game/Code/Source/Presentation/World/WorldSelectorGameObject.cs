using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldSelectorGameObject : MonoBehaviour
{

    public Animator animator;

    public List<WorldGameObject> worldGOList;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
