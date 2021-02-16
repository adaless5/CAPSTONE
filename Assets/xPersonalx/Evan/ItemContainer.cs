using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainer : MonoBehaviour
{
    public GameObject _Container;
    public GameObject[] _PossiblePickups;
    string[] Tags = { "Creature_Weapon", "Player_Weapon", "Player_Blade", "Player_Mine" };

    bool bIsBroken;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Break(string tag)
    {
        if (!bIsBroken)
            foreach (string t in Tags)
            {
                if (tag == t)
                {
                    GetComponent<Collider>().enabled = false;
                    _Container.SetActive(false);

                    int randint = Random.Range(0, _PossiblePickups.Length);
                    GameObject pickup = Instantiate(_PossiblePickups[randint], transform.position, transform.rotation, transform);
                    pickup.isStatic = false;
                    pickup.SetActive(true);
                    bIsBroken = true;
                }
            }
    }
}
