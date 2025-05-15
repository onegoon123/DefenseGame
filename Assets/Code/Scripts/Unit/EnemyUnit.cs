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

    protected override void Start()
    {
        base.Start();
        kingTarget = GameObject.FindWithTag("King")?.transform;
        InvokeRepeating(nameof(UpdatePath), 0f, 1f);
    }

    protected override void Update()
    {
        base.Update();
        atkTimer -= Time.deltaTime;

        // 사거리 안에 아군이 있으면 공격 → 없으면 이동
        if (!TryAttackNearbyPlayer())
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
                PieceUnit target = StageManager.instance.units[targetPos.x, targetPos.y];

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
        if (path == null || pathIndex >= path.Count) return;

        Vector3 targetPos = StageManager.instance.GridToWorldPosition(path[pathIndex]) + Vector3.up * 0.5f;
        Vector3 dir = (targetPos - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
            pathIndex++;
    }

    // ▼ A* 관련 메서드 생략 
    List<int2> FindSimplePath(int2 start, int2 goal) {  return path; }
    List<int2> ReconstructPath(Dictionary<int2, int2> cameFrom, int2 current) {  return path; }
    int Heuristic(int2 a, int2 b) => math.abs(a.x - b.x) + math.abs(a.y - b.y);
    readonly int2[] Directions = new int2[] { new int2(0, 1), new int2(0, -1), new int2(1, 0), new int2(-1, 0) };
}
