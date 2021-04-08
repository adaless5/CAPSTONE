using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultLight : MonoBehaviour
{

    // Start is called before the first frame update
    public GameObject pieceOne;
    public GameObject pieceTwo;

    public float BobLength;
    public float BobSpeed;
    public float pieceOneSpinSpeed;
    public float pieceTwoSpinSpeed;

    float floatingOffset;
    Vector3 OriginalPosition;
    void Start()
    {
        floatingOffset = Random.Range(0.0f, 6.32f);
        OriginalPosition = transform.position;

        if (pieceOne)
            pieceOne.transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 90.0f));
        if (pieceTwo)
            pieceTwo.transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 90.0f));
    }

    void Float()
    {
        transform.position = new Vector3(OriginalPosition.x, OriginalPosition.y + (BobLength * Mathf.Sin(floatingOffset)), OriginalPosition.z);
        floatingOffset += Time.deltaTime * BobSpeed;
        if (floatingOffset > 6.32f)
        {
            floatingOffset = 0.0f;
        }
    }

    void RotateArmor()
    {
        if (pieceOne)
            pieceOne.transform.Rotate(0.0f, 0.0f, -pieceOneSpinSpeed * Time.deltaTime);
        if (pieceTwo)
            pieceTwo.transform.Rotate(0.0f, 0.0f, pieceTwoSpinSpeed * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        Float();
        RotateArmor();
    }
}
