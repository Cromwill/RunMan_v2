using UnityEngine;

public class PanelActivator : MonoBehaviour
{
    [SerializeField] protected GameObject[] _panels;

    public virtual void ShowPanel(GameObject panel)
    {
        gameObject.SetActive(true);
        SetActiveObject(panel, _panels);
    }

    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
    }

    protected virtual void SetActiveObject(GameObject activeObject, params GameObject[] inactiveObject)
    {
        foreach (var inactive in inactiveObject)
        {
            inactive.SetActive(inactive == activeObject);
        }
    }
}
