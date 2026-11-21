using UnityEngine;

public class Ineractable : MonoBehaviour, IInteractable
{
    public void interact()
    {
        Destroy(gameObject);
    }
    public void onHover()
    {

    }
    public void exitHover()
    {

    }

}
