using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Cell : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField]
    private Image image;

    private TruchetTilesPreset currentPreset;

    private int currentIndex = default;

    private void Reset()
    {
        image = GetComponent<Image>();
    }

    public void Init(TruchetTilesPreset preset)
    {
        currentPreset = preset;
        currentIndex = GetRandomInt();
        UpdateView();
    }

    private void UpdateView()
    {
        image.sprite = currentPreset.TruchetTilesSprites[currentIndex];
        IncrementIndex();
    }

    private int GetRandomInt() => Random.Range(0, currentPreset.TruchetTilesSprites.Count);

    private void IncrementIndex()
    {
        currentIndex++;
        currentIndex %= currentPreset.AmountOfSprites;
    }

    public void Puls() 
    {
        currentIndex = GetRandomInt();
        UpdateView();
    }

    public void OnPointerEnter(PointerEventData eventData) => UpdateView();
}
