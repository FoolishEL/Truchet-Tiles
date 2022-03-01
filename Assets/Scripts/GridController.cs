using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class GridController : MonoBehaviour
{
    [SerializeField]
    private Cell cellPrefab;

    [SerializeField]
    private GridLayoutGroup gridLayout;

    [SerializeField]
    private int sellSize = 100;

    [SerializeField]
    private TruchetTilesPreset defaultPreset;

    [SerializeField]
    private int CellsCount;

    private int cellsCountRequired;

    private List<Cell> cellsPool;

    public List<Cell> Cells => cellsPool;

    public event Action OnSizeChanged = delegate { };

    private void Awake()
    {
        cellsPool = new List<Cell>();
        gridLayout.cellSize = Vector2.one * sellSize;
        gridLayout.spacing = Vector2.zero;
        StartCoroutine(CalculateCellsCount());
    }

    public void IncreaseSize() 
    {
        if (sellSize + 2 > 90)
            return;
        sellSize += 2;
        gridLayout.cellSize = Vector2.one * sellSize;
        StartCoroutine(CalculateCellsCount());
    }

    public void DecreaseSize() 
    {
        if (sellSize - 2 < 40)
            return;
        sellSize -= 2;
        gridLayout.cellSize = Vector2.one * sellSize;
        StartCoroutine(CalculateCellsCount());
    }

    private IEnumerator CalculateCellsCount()
    {
        var rectT = gridLayout.transform as RectTransform;
        yield return null;
        int hCount = (int)(rectT.rect.height / sellSize);
        hCount++;
        int vCount = (int)(rectT.rect.width / sellSize);
        vCount++;
        cellsCountRequired = vCount * hCount;
        CellsCount = cellsCountRequired;
        SpawnCells();
    }

    private void SpawnCells()
    {
        int activeCellsCount = cellsPool.Where(c => c.gameObject.activeSelf).Count();
        if (activeCellsCount < cellsCountRequired)
        {
            TryGetCells(cellsCountRequired - activeCellsCount);
        }
        else
        {
            HideExcess();
        }
        OnSizeChanged.Invoke();
    }

    private void TryGetCells(int count)
    {
        var inactiveCells = cellsPool.Where(c => !c.gameObject.activeSelf).ToList();
        if (inactiveCells.Count >= count)
        {
            for (int i = 0; i < count; i++)
            {
                inactiveCells[i].gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (var item in inactiveCells)
            {
                item.gameObject.SetActive(item);
            }
            SpawnCells(count - inactiveCells.Count);
        }
    }

    private void SpawnCells(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var instance = Instantiate(cellPrefab, gridLayout.transform);
            instance.Init(defaultPreset);
            instance.gameObject.SetActive(true);
            cellsPool.Add(instance);
        }
    }

    private void HideExcess() 
    {
        var activeCellsCount = cellsPool.Where(c => c.gameObject.activeSelf).Count();
        for(int i = 0; i < activeCellsCount - cellsCountRequired; i++) 
        {
            cellsPool[cellsPool.Count - i - 1].gameObject.SetActive(false);
        }
    }
}
