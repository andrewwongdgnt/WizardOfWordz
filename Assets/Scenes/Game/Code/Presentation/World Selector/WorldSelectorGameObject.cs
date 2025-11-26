using UnityEngine;

public class WorldSelectorGameObject : MonoBehaviour
{

    //[SerializeField] 
    public Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Appear(bool appear)
    {
       animator.SetBool("Appear", appear);
    }
}
