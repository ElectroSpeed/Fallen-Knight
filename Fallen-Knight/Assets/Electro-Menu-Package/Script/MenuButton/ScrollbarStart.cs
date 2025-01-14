using UnityEngine;
using UnityEngine.UI;

public class ScrollbarStart : MonoBehaviour
{
    void Start()
    {
        this.GetComponent<Scrollbar>().value = 1f;
    }
}
