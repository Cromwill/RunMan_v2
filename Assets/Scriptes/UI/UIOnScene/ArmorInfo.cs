using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class ArmorInfo : MonoBehaviour
{
    [SerializeField] private Text _bulletCountViewer;

    private Animator _selfAnimator;
    private int _currentBulletCount;

    public void Show(int count)
    {
        if(_selfAnimator == null)
            _selfAnimator = GetComponent<Animator>();

        _selfAnimator.Play("AmmunationFire");
        _currentBulletCount = count;
    }

    public void ChangeValue()
    {
        _bulletCountViewer.text = _currentBulletCount.ToString();
    }
}
