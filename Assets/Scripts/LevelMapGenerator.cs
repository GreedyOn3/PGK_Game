using System.Collections.Generic;
using UnityEngine;

public class LevelMapGenerator : MonoBehaviour
{
    [Header("Map")]
    public int mapSize = 50;
    [Range(0f, 1f)]
    public float elevationChance = 0.075f;

    [Header("Spacing")]
    public float horizontalSpacing = 30f;
    public float verticalSpacing = 30f;

    [Header("GameObjects")]
    public GameObject blockPrefab;
    public GameObject rampPrefab;
    public GameObject bottomBlockPrefab;

    class Cell
    {
        public int x;
        public int z;
        public int elevation;

        public bool isRamp;
        public Vector2Int rampDirection;

        public Cell(int x, int z, int elevation)
        {
            this.x = x;
            this.z = z;
            this.elevation = elevation;
        }
    }

    Cell[,] grid;

    List<Cell> expandableCells = new();
    Cell currentCell;

    Vector2Int lockedDirection;

    void Start()
    {
        GenerateGrid();
        CreateMap();
    }

    void GenerateGrid()
    {
        grid = new Cell[mapSize, mapSize];
        currentCell = CreateCell(Random.Range(0, mapSize), Random.Range(0, mapSize), 0);

        while (currentCell != null)
        {
            Expand();
        }
    }

    void CreateMap()
    {
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                Cell cell = grid[i, j];
                Vector3 pos = new Vector3(cell.x * horizontalSpacing, cell.elevation * verticalSpacing, cell.z * horizontalSpacing);

                GameObject block;
                if (cell.isRamp)
                    block = Instantiate(rampPrefab, pos, GetRampRotation(cell.rampDirection), transform);
                else
                    block = Instantiate(blockPrefab, pos, Quaternion.identity, transform);
                block.transform.localScale = new Vector3(horizontalSpacing / 2f, verticalSpacing / 2f, horizontalSpacing / 2f);

                if (cell.elevation > 0)
                    CreateBottomBlocks(cell.elevation, block.transform);
            }
        }
    }

    void Expand()
    {
        Vector2Int direction = lockedDirection != Vector2Int.zero ? lockedDirection : GetRandomDirection(currentCell);

        if (direction == Vector2Int.zero)
        {
            currentCell = GetNextExpandableCell();
            if (currentCell == null) return;

            direction = GetRandomDirection(currentCell);
        }

        int x = currentCell.x + direction.x;
        int z = currentCell.z + direction.y;
        int elevation = currentCell.elevation;

        currentCell = CreateCell(x, z, elevation);

        bool raise = CanRaiseElevation(currentCell, direction) && Random.value < elevationChance;

        if (raise)
        {
            currentCell.elevation++;

            currentCell.isRamp = true;
            currentCell.rampDirection = direction;

            lockedDirection = direction;
        }
        else
        {
            lockedDirection = Vector2Int.zero;
        }
    }

    void CreateBottomBlocks(int elevation, Transform blockTransform)
    {
        for (int i = 0; i < elevation; i++)
            Instantiate(bottomBlockPrefab, blockTransform.position + Vector3.down * (i + 1) * verticalSpacing, Quaternion.identity, blockTransform);
    }

    Cell CreateCell(int x, int z, int elevation)
    {
        Cell cell = new Cell(x, z, elevation);

        grid[x, z] = cell;

        expandableCells.Add(cell);

        return cell;
    }

    Cell GetNextExpandableCell()
    {
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                Cell cell = grid[i, j];
                if (cell == null || cell.isRamp) continue;

                if (HasAvailableNeighbor(cell)) return cell;
            }
        }

        return null;
    }

    bool HasAvailableNeighbor(Cell cell)
    {
        return IsInsideAndEmpty(cell.x + 1, cell.z) ||
               IsInsideAndEmpty(cell.x - 1, cell.z) ||
               IsInsideAndEmpty(cell.x, cell.z + 1) ||
               IsInsideAndEmpty(cell.x, cell.z - 1);
    }

    Vector2Int GetRandomDirection(Cell cell)
    {
        List<Vector2Int> dirs = new();

        Vector2Int[] possibleDirs =
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        foreach (Vector2Int dir in possibleDirs)
        {
            int x = cell.x + dir.x;
            int z = cell.z + dir.y;
            if (!IsInsideAndEmpty(x, z)) continue;

            dirs.Add(dir);
        }

        if (dirs.Count == 0)
            return Vector2Int.zero;

        return dirs[Random.Range(0, dirs.Count)];
    }

    bool CanRaiseElevation(Cell cell, Vector2Int direction)
    {
        int x = cell.x + direction.x;
        int z = cell.z + direction.y;

        return IsInsideAndEmpty(x, z);
    }

    bool IsInsideAndEmpty(int x, int z)
    {
        return IsInside(x, z) && grid[x, z] == null;
    }

    bool IsInside(int x, int z)
    {
        return x >= 0
            && z >= 0
            && x < mapSize
            && z < mapSize;
    }

    Quaternion GetRampRotation(Vector2Int dir)
    {
        if (dir == Vector2Int.up)
            return Quaternion.Euler(0, 0, 0);
        else if (dir == Vector2Int.down)
            return Quaternion.Euler(0, 180, 0);
        else if (dir == Vector2Int.left)
            return Quaternion.Euler(0, 270, 0);

        return Quaternion.Euler(0, 90, 0);
    }
}
