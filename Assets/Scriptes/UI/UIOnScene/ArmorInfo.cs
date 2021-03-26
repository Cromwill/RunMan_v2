using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class ArmorInfo : MonoBehaviour
{
    [SerializeField] private Text _bulletCountViewer;
    [SerializeField] private Image _reloadImage;

    private Animator _selfAnimator;
    private int _currentBulletCount;

    public void Show(int count)
    {
        if(_selfAnimator == null)
            _selfAnimator = GetComponent<Animator>();

        _selfAnimator.Play("AmmunationFire");
        _currentBulletCount = count;
    }

    public void ReloadShow(float currentValue, float maxValue)
    {
        _reloadImage.fillAmount = 1 - (1 / maxValue * currentValue);
    }

    public void ChangeValue()
    {
        _bulletCountViewer.text = _currentBulletCount.ToString();
    }
}
