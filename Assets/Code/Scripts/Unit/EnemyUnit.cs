using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class EnemyUnit : PieceUnit
{
    //public float moveSpeed = 2f;
    public float atkCooldown = 1f; // 공격 쿨타임
    private float atkTimer = 0f;

    private List<int2> path = new List<int2>();
    private int pathIndex = 0;
    private PlayerUnit king;

    [SerializeField]
    private float moveDelay = 0.5f;
    private float moveDelayTimer = 0f;

    protected void Start()
    {
        king = StageManager.instance.king;
        //InvokeRepeating(nameof(UpdatePath), 0f, 0.1f);
        moveDelayTimer = moveDelay;
        UpdatePath();
    }

    protected override void Update()
    {
        base.Update();
        if (king == null) { return; }
        atkTimer -= Time.deltaTime;
        if (isMove) return;
        
        PieceUnit target = FindTargetInRange();
        if (target == null)
        {
            FollowPath(); // 공격 대상 없을 때만 이동
        }
    }

    void Attack(PieceUnit target)
    {
        target.TakeDamage(1);
        Debug.Log($"[EnemyUnit] {name} 이(가) {target.name} 에게 공격함");
    }

    void UpdatePath()
    {
        if (king == null) return;

        path = FindSimplePath(gridPos, king.gridPos);
        pathIndex = 0;
    }

    void FollowPath()
    {
        if (0 < moveDelayTimer)
        {
            moveDelayTimer -= Time.deltaTime;
            return;
        }

        if (path == null || pathIndex + 1 >= path.Count)
        {
            UpdatePath();
            if (path != null && pathIndex + 1 < path.Count)
            {
                FollowPath();
            }
            return;
        }

        LookAtTarget(king);
        moveDelayTimer = moveDelay;
        MoveGridPos(path[++pathIndex]);
    }

    // ▼ A*
    List<int2> FindSimplePath(int2 start, int2 goal)
    {
        HashSet<int2> closed = new HashSet<int2>();
        Dictionary<int2, int2> cameFrom = new Dictionary<int2, int2>();
        List<int2> open = new List<int2> { start };
        Dictionary<int2, int> gScore = new Dictionary<int2, int> { [start] = 0 };

        // 도착점까지와 거리가 가장 까가운 위치를 nearst에 기록
        // nearst와 start까지의 거리를 nearstHeuristic에 기록
        int2 nearst = start;
        int nearstHeuristic = Heuristic(start, goal);

        while (open.Count > 0)
        {
            open.Sort((a, b) => (gScore[a] + Heuristic(a, goal)).CompareTo(gScore[b] + Heuristic(b, goal)));
            int2 current = open[0];
            open.RemoveAt(0);

            int currentHeuristic = Heuristic(current, goal);
            if (currentHeuristic < nearstHeuristic)
            {
                nearstHeuristic = currentHeuristic;
                nearst = current;
            }

            if (current.Equals(goal))
                return ReconstructPath(cameFrom, current);

            closed.Add(current);

            foreach (int2 dir in Directions)
            {
                int2 neighbor = current + dir;

                if (closed.Contains(neighbor)) continue;

                // 유효한 타일인가?
                if (StageManager.instance.IsValidTile(neighbor) == false) continue;
                // 이동할 타일이 Ground 타일이 아니면 continue
                if (StageManager.instance.GetTileType(neighbor) != TileType.Ground) continue;

                int tentativeG = gScore[current] + 1;
                if (!gScore.ContainsKey(neighbor) || tentativeG < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeG;
                    if (!open.Contains(neighbor)) open.Add(neighbor);
                }
            }
        }

        // 목표까지 도착하는 경로를 찾지 못한다면 가장 가까운 곳까지의 경로를 반환
        if (nearst.Equals(start) == false)
        {
            return ReconstructPath(cameFrom, nearst);
        }

        return new List<int2>(); // 경로 없음
    }

    List<int2> ReconstructPath(Dictionary<int2, int2> cameFrom, int2 current)
    {
        List<int2> path = new List<int2> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(current);
        }
        path.Reverse();
        return path;
    }
    int Heuristic(int2 a, int2 b) => math.abs(a.x - b.x) + math.abs(a.y - b.y);
    readonly int2[] Directions = new int2[] { new int2(0, 1), new int2(0, -1), new int2(1, 0), new int2(-1, 0) };
}
