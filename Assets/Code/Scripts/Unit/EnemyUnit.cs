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
    private Transform kingTarget;

    [SerializeField]
    private float moveDelay = 0.5f;
    private float moveDelayTimer = 0f;

    protected override void Start()
    {
        base.Start();
        kingTarget = GameObject.FindWithTag("King")?.transform;
        InvokeRepeating(nameof(UpdatePath), 0f, 1f);
        moveDelayTimer = moveDelay;
    }

    protected override void Update()
    {
        base.Update();

        atkTimer -= Time.deltaTime;
        if (isMove) return;
        
        


        PieceUnit target = FindTargetInRange();
        // 사거리 안에 아군이 있으면 공격 → 없으면 이동
        if (target != null)
        {
            Attack(target);
        }
        else
        {
            FollowPath(); // 공격 대상 없을 때만 이동
        }
    }

    // ▶ 사거리 1칸 내 아군 유닛을 공격 시도
    bool TryAttackNearbyPlayer()
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (math.abs(x) + math.abs(y) > 1) continue; // 상하좌우만 (대각선 제외)

                int2 targetPos = gridPos + new int2(x, y);
                if (!StageManager.instance.IsValidTile(targetPos)) continue;
                PieceUnit target = StageManager.instance.GetUnit(targetPos);

                if (target != null && target.isPlayer)
                {
                    if (atkTimer <= 0f)
                    {
                        Attack(target);
                        atkTimer = atkCooldown;
                    }
                    return true;
                }
            }
        }
        return false;
    }

    void Attack(PieceUnit target)
    {
        target.TakeDamage(1);
        Debug.Log($"[EnemyUnit] {name} 이(가) {target.name} 에게 공격함");
    }

    void UpdatePath()
    {
        if (kingTarget == null) return;

        int2 start = StageManager.instance.WorldToGridPosition(transform.position);
        int2 end = StageManager.instance.WorldToGridPosition(kingTarget.position);

        path = FindSimplePath(start, end);
        pathIndex = 0;
    }

    void FollowPath()
    {
        if (0 < moveDelayTimer)
        {
            moveDelayTimer -= Time.deltaTime;
            return;
        }

        if (path == null || pathIndex+1 >= path.Count) return;
        if (StageManager.instance.GetUnit(path[pathIndex+1]) != null) return;
        
        moveDelayTimer = moveDelay;
        MoveGridPos(path[++pathIndex]);
    }

    // ▼ A*
    List<int2> FindSimplePath(int2 start, int2 goal)
    {
        if(name == "enemy20")
        {
            Debug.Log("길 찾는중");
        }
        HashSet<int2> closed = new HashSet<int2>();
        Dictionary<int2, int2> cameFrom = new Dictionary<int2, int2>();
        List<int2> open = new List<int2> { start };
        Dictionary<int2, int> gScore = new Dictionary<int2, int> { [start] = 0 };

        while (open.Count > 0)
        {
            open.Sort((a, b) => (gScore[a] + Heuristic(a, goal)).CompareTo(gScore[b] + Heuristic(b, goal)));
            int2 current = open[0];
            open.RemoveAt(0);

            if (current.Equals(goal))
                return ReconstructPath(cameFrom, current);

            closed.Add(current);

            foreach (int2 dir in Directions)
            {
                int2 neighbor = current + dir;

                if (closed.Contains(neighbor)) continue;

                // 임시로 작성했습니다, 이동할 타일에 유닛이 있거나 Walkable 타일이 아니면 continue
                if (StageManager.instance.IsValidTile(neighbor) == false) continue;
                if (StageManager.instance.GetUnit(neighbor) != null) continue;
                if (StageManager.instance.GetTileType(neighbor) != TileType.Walkable) continue;

                int tentativeG = gScore[current] + 1;
                if (!gScore.ContainsKey(neighbor) || tentativeG < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeG;
                    if (!open.Contains(neighbor)) open.Add(neighbor);
                }
            }
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
