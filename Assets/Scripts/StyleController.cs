using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using System.Collections.Generic;

public class StyleController : MonoBehaviour
{
    [SerializeField]
    private Button playBtn;

    [SerializeField]
    private Image targetImage;

    [SerializeField]
    private Sprite playSprite;
    [SerializeField]
    private Sprite stopSprite;

    [SerializeField]
    private GridController gridController;

    [SerializeField]
    private int flowPerCell = 10;

    [SerializeField]
    private List<TruchetTilesPreset> presets;

    [SerializeField, Range(0f, 1f)]
    private float tempo = .07f;

    private bool state = false;
    private bool presetChanging = false;

    private List<Coroutine> flows;
    private Coroutine presetChangingCorutine;

    private int currentPreset = 0;

    private void Awake()
    {
        flows = new List<Coroutine>();
        state = false;
        presetChanging = false;
        playBtn.onClick.AddListener(SwitchState);
        gridController.OnSizeChanged += Handler;
    }

    private void OnDestroy()
    {
        playBtn.onClick.RemoveListener(SwitchState);
        gridController.OnSizeChanged -= Handler;
    }

    private void Handler() 
    {
        if (state) 
        {
            StopFlow();
            StartFlow();
        }
        if (presetChanging) 
        {
            StopCoroutine(presetChangingCorutine);
            presetChangingCorutine = StartCoroutine(PresetHandler());
        }
    }

    private void SwitchState()
    {
        state = !state;
        targetImage.sprite = state ? stopSprite : playSprite;
        if (state)
        {
            StartFlow();
        }
        else
        {
            StopFlow();
        }
    }

    private void StartFlow()
    {
        int count = gridController.Cells.Count / flowPerCell;
        for (int i = 0; i < count; i++)
        {
            flows.Add(StartCoroutine(Animation()));
        }
    }

    private void StopFlow()
    {
        foreach (var flow in flows)
            StopCoroutine(flow);
        flows.Clear();
    }

    private IEnumerator Animation()
    {
        var res = from r in gridController.Cells orderby Guid.NewGuid() ascending select r;
        while (enabled)
            foreach (var elem in res)
            {
                elem.Puls();
                yield return new WaitForSecondsRealtime(tempo);
            }
    }

    public void NextPreset() 
    {
        currentPreset++;
        currentPreset %= presets.Count;
        presetChangingCorutine =StartCoroutine(PresetHandler());
    }

    private IEnumerator PresetHandler() 
    {
        presetChanging = true;
        var res = from r in gridController.Cells orderby Guid.NewGuid() ascending select r;
        foreach (var elem in res)
        {
            elem.Init(presets[currentPreset]);
            yield return null;
        }
        presetChanging = false;
    }
}
