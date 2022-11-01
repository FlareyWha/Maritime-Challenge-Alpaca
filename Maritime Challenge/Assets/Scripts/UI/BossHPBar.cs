using UnityEngine;
using UnityEngine.UI;

public class BossHPBar : MonoBehaviourSingleton<BossHPBar>
{
    [SerializeField]
    private Image HPFill;
    [SerializeField]
    private Text HPFillText;

    private CanvasGroup canvasGroup = null;

    private BaseEnemy LinkedEnemy = null;

    [SerializeField]
    private EelBoss eelBoss;

    protected override void Awake()
    {
        base.Awake();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    protected void Start()
    {
        LinkEnemy(eelBoss);
    }

    public void Activate()
    {
        StartCoroutine(UIManager.ToggleFadeAnim(canvasGroup, 0, 1, 0.6f));
        Activate();
    }

    public void Deactivate()
    {
        StartCoroutine(UIManager.ToggleFadeAnim(canvasGroup, 1, 0, 0.6f));
    }

    public void LinkEnemy(BaseEnemy enemy)
    {
        if (LinkedEnemy != null)
            LinkedEnemy.OnEntityHPChanged -= UpdateHPUI;

        LinkedEnemy = enemy;
        enemy.OnEntityHPChanged += UpdateHPUI;
    }

    protected override void OnDestroy()
    {
        if (LinkedEnemy != null)
            LinkedEnemy.OnEntityHPChanged -= UpdateHPUI;
    }

    private void UpdateHPUI(int _old, int _new)
    {
        HPFill.fillAmount = (float)_new / LinkedEnemy.MaxHP;
        HPFillText.text = _new + "/" + LinkedEnemy.MaxHP;
    }

}
