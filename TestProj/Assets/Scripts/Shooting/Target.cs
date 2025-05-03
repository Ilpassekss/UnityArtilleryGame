using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{

    [SerializeField] private float Health = 100.0f;


    void Start()
    {
        Debug.Log(Health);
    }
    public void GetDamage(float amountDamage){
        Health -= amountDamage;
        Debug.Log("Health left" + Health);
        if(Health <= 0){
            Die();
        }
    } 

    private void Die(){
        gameObject.GetComponent<Renderer>().material.color = Color.red;
    } 
}
