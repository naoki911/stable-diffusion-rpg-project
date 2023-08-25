using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public AudioSource aud;
    public AudioClip create,finish;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateStart(){
        aud.PlayOneShot(create);
    }

    public void CreateFinish(){
        aud.PlayOneShot(finish);
    }
}
