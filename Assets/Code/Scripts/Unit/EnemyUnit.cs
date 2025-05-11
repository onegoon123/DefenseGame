using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class EnemyUnit : MonoBehaviour
{
    public float moveSpeed = 2f; // 이동 속도
    private List<int2> path = new List<int2>(); // 경로 저장
    private int pathIndex = 0; // 현재 경로 인덱스

    private Transform kingTarget; // 추적할 대상 (Piece_king)

    private void Start()
    {
        // "King" 태그가 붙은 오브젝트 찾기
        kingTarget = GameObject.FindWithTag("Piece_king")?.transform;

        // 일정 주기로 경로 갱신
        InvokeRepeating(nameof(UpdatePath), 0f, 1f);
    }

    private void Update()
    {
        FollowPath(); // 현재 경로 따라 이동
    }

    // A*로 경로 재계산
    void UpdatePath()
    {
        if (kingTarget == null) return;

        int2 start = StageManager.instance.WorldToGridPosition(transform.position);
        int2 end = StageManager.instance.WorldToGridPosition(kingTarget.position);

        path = FindSimplePath(start, end);
        pathIndex = 0;
    }

    // 계산된 경로 이동
    void FollowPath()
    {
        if (path == null || pathIndex >= path.Count) return;

        Vector3 targetPos = StageManager.instance.GridToWorldPosition(path[pathIndex]) + Vector3.up * 0.5f;
        Vector3 dir = (targetPos - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
            pathIndex++;
    }

    // 장애물을 피해서 경로탐색
    List<int2> FindSimplePath(int2 start, int2 goal)
    {
        HashSet<int2> closed = new HashSet<int2>();
        Dictionary<int2, int2> cameFrom = new Dictionary<int2, int2>();
        List<int2> open = new List<int2> { start };
        Dictionary<int2, int> gScore = new Dictionary<int2, int> { [start] = 0 };

        List<int2> blocked = StageManager.instance.GetBlockedTiles(); // 장애물 타일 받아오기

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
                if (blocked.Contains(neighbor)) continue;

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

    // A* 경로 되짚기
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

    int Heuristic(int2 a, int2 b)
    {
        return math.abs(a.x - b.x) + math.abs(a.y - b.y);
    }

    // 방향 이동 (상하좌우)
    readonly int2[] Directions = new int2[]
    {
        new int2(0, 1),
        new int2(0, -1),
        new int2(1, 0),
        new int2(-1, 0)
    };
}
