using UnityEngine;

//VR - This class is a damage system for any target/enemy
public class Target : MonoBehaviour, ISaveable
{
    [Header("Health Settings")]
    public float m_HP = 50.0f;

    bool isDead = false;

    void Awake()
    {   
        LoadDataOnSceneEnter();
        SaveSystem.SaveEvent += SaveDataOnSceneChange;

        if (isDead) GetComponent<MeshRenderer>().enabled = false;
        else GetComponent<MeshRenderer>().enabled = true;
    }

    void OnDisable()
    {
        SaveSystem.SaveEvent -= SaveDataOnSceneChange;
    }

    public void TakeDamage (float damage)
    {
        m_HP -= damage;
        if (m_HP <= 0.0f)
        {
            Die();
        }
    }

    //VR - Currently destroying gameObjects when damaged, can look into creating object pool for recyclable assets later on
    void Die()
    {
        //Destroy(gameObject);
        isDead = true;
        GetComponent<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveDataOnSceneChange()
    {
        SaveSystem.Save(gameObject.name, "isDead", isDead);
        
        SaveSystem.Save(gameObject.name, "posX",transform.position.x);
        SaveSystem.Save(gameObject.name, "posY",transform.position.y);
        SaveSystem.Save(gameObject.name, "posZ",transform.position.z);

        SaveSystem.Save(gameObject.name, "rotX",transform.rotation.x);
        SaveSystem.Save(gameObject.name, "rotY",transform.rotation.y);
        SaveSystem.Save(gameObject.name, "rotZ",transform.rotation.z);
        SaveSystem.Save(gameObject.name, "rotW",transform.rotation.w); 
    }   

    public void LoadDataOnSceneEnter()
    {
        isDead = SaveSystem.LoadBool(gameObject.name, "isDead");

        if (SaveSystem.LoadFloat(gameObject.name,"posX") != 0)
        {
            transform.position = new Vector3(SaveSystem.LoadFloat(gameObject.name,"posX"),SaveSystem.LoadFloat(gameObject.name,"posY"),SaveSystem.LoadFloat(gameObject.name,"posZ"));
        }
        
        transform.rotation = new Quaternion(
            SaveSystem.LoadFloat(gameObject.name,"rotX"),
            SaveSystem.LoadFloat(gameObject.name,"rotY"),
            SaveSystem.LoadFloat(gameObject.name,"rotZ"),
            SaveSystem.LoadFloat(gameObject.name,"rotW"));
    }
}
