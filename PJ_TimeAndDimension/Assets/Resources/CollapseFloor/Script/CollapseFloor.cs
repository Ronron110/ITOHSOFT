using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public interface ICollapseFloorTriggers
{
    void OnDamaged(int value);
}


public class CollapseFloor : MonoBehaviour, ICollapseFloorTriggers
{
    GameObject unstableBlock;
    GameObject collapsedBlock;
    ParticleSystem particle;
    public int blockDamage = 10;

    // Start is called before the first frame update
    void Start()
    {
        unstableBlock = gameObject.transform.Find("UnstableBlock").gameObject;
        collapsedBlock = gameObject.transform.Find("CollapsedBlock").gameObject;
        particle = GetComponent<ParticleSystem>();
        particle.Stop();

    }

    public void OnDamaged(int value)
    {
        blockDamage -= value;
        if (blockDamage <= 0)
        {
            unstableBlock.SetActive(false);
            particle.Play();
            collapsedBlock.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(blockDamage);
    }
    
}
