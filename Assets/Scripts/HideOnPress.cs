using UnityEngine;

public class HideOnPress : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
            transform.gameObject.SetActive(false);
    }
}
