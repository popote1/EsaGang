using UnityEngine;

public class RoofScripte : MonoBehaviour
{
    public GameObject RoofParticul;
    [Range(0, 100)] public float ChanceOfSpawn = 25;
    public GameObject RoofPlate;
    
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.relativeVelocity.magnitude > 10)
        {
            GameObject go = Instantiate(RoofParticul, other.contacts[0].point, Quaternion.identity);

            if (Random.Range(0, 100) < ChanceOfSpawn)
            {
                Instantiate(RoofPlate, other.contacts[0].point, Quaternion.identity);
            }
        }

       
    }
}
