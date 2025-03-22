using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEditorOnly : MonoBehaviour
{
    void Start()
    {
        if (gameObject.CompareTag("EditorOnly"))
        {
            Destroy(gameObject);
        }
    }
}
