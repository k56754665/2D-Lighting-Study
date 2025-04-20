using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SpritesToMeshConverter : MonoBehaviour
{
    public Material targetMaterial; // 결합된 메시에 사용할 머티리얼
    public bool combineColliders = true; // 콜라이더도 결합할지 여부
    public bool keepOriginalColliders = false; // 원본 콜라이더를 유지할지 여부

    public void ConvertAndCombine()
    {
        // 모든 자식 스프라이트 렌더러 수집
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        if (spriteRenderers.Length == 0)
        {
            Debug.LogWarning("변환할 스프라이트가 없습니다!");
            return;
        }

        Debug.Log($"총 {spriteRenderers.Length}개의 스프라이트를 찾았습니다.");

        // 머티리얼이 없으면 첫 번째 스프라이트의 머티리얼 사용
        if (targetMaterial == null)
        {
            targetMaterial = spriteRenderers[0].sharedMaterial;
        }

        // 스프라이트들을 메시로 변환하고 결합
        List<CombineInstance> combines = new List<CombineInstance>();
        List<Vector2[]> colliderPoints = new List<Vector2[]>();
        
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            if (spriteRenderer.sprite == null) continue;

            Mesh spriteMesh = CreateMeshFromSprite(spriteRenderer);
            CombineInstance ci = new CombineInstance();
            ci.mesh = spriteMesh;
            ci.transform = spriteRenderer.transform.localToWorldMatrix;
            combines.Add(ci);

            // 콜라이더 정보 수집
            if (combineColliders)
            {
                Collider2D collider = spriteRenderer.GetComponent<Collider2D>();
                if (collider != null)
                {
                    if (collider is BoxCollider2D boxCollider)
                    {
                        Vector2[] points = CreateBoxColliderPoints(boxCollider, spriteRenderer.transform);
                        colliderPoints.Add(points);
                    }
                    else if (collider is PolygonCollider2D polygonCollider)
                    {
                        Vector2[] points = TransformColliderPoints(polygonCollider.points, spriteRenderer.transform);
                        colliderPoints.Add(points);
                    }
                }
            }
        }

        // 결합된 메시를 저장할 새 게임오브젝트 생성
        GameObject combinedObject = new GameObject("Combined Sprites Mesh");
        combinedObject.transform.SetParent(transform);
        combinedObject.transform.localPosition = Vector3.zero;
        combinedObject.transform.localRotation = Quaternion.identity;
        combinedObject.transform.localScale = Vector3.one;

        // 메시 결합
        Mesh combinedMesh = new Mesh();
        combinedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        combinedMesh.CombineMeshes(combines.ToArray());

        // MeshFilter와 MeshRenderer 추가
        MeshFilter meshFilter = combinedObject.AddComponent<MeshFilter>();
        meshFilter.sharedMesh = combinedMesh;

        MeshRenderer meshRenderer = combinedObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = targetMaterial;

        // 콜라이더 결합 및 추가
        if (combineColliders && colliderPoints.Count > 0)
        {
            CreateCombinedCollider(combinedObject, colliderPoints);
        }

        // 원본 오브젝트 처리
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            if (keepOriginalColliders)
            {
                // 스프라이트 렌더러만 비활성화하고 콜라이더는 유지
                sr.enabled = false;
            }
            else
            {
                // 게임오브젝트 자체를 비활성화
                sr.gameObject.SetActive(false);
            }
        }

        Debug.Log("스프라이트들이 성공적으로 메시로 변환되고 결합되었습니다!");
    }

    private Vector2[] CreateBoxColliderPoints(BoxCollider2D boxCollider, Transform transform)
    {
        Vector2 center = boxCollider.offset;
        Vector2 size = boxCollider.size;
        Vector2 halfSize = size * 0.5f;

        Vector2[] points = new Vector2[4];
        points[0] = center + new Vector2(-halfSize.x, -halfSize.y); // 좌하단
        points[1] = center + new Vector2(halfSize.x, -halfSize.y);  // 우하단
        points[2] = center + new Vector2(halfSize.x, halfSize.y);   // 우상단
        points[3] = center + new Vector2(-halfSize.x, halfSize.y);  // 좌상단

        // 로컬 좌표를 월드 좌표로 변환
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = transform.TransformPoint(points[i]);
        }

        return points;
    }

    private Vector2[] TransformColliderPoints(Vector2[] points, Transform transform)
    {
        Vector2[] transformedPoints = new Vector2[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            transformedPoints[i] = transform.TransformPoint(points[i]);
        }
        return transformedPoints;
    }

    private void CreateCombinedCollider(GameObject targetObject, List<Vector2[]> colliderPoints)
    {
        // 모든 포인트를 하나의 리스트로 합치기
        List<Vector2> allPoints = new List<Vector2>();
        foreach (Vector2[] points in colliderPoints)
        {
            allPoints.AddRange(points);
        }

        // PolygonCollider2D 추가 및 설정
        PolygonCollider2D combinedCollider = targetObject.AddComponent<PolygonCollider2D>();
        combinedCollider.points = allPoints.ToArray();

        Debug.Log($"결합된 콜라이더가 생성되었습니다. (총 {allPoints.Count}개의 포인트)");
    }

    private Mesh CreateMeshFromSprite(SpriteRenderer spriteRenderer)
    {
        Sprite sprite = spriteRenderer.sprite;
        Vector2 size = new Vector2(
            sprite.bounds.size.x * spriteRenderer.transform.localScale.x,
            sprite.bounds.size.y * spriteRenderer.transform.localScale.y
        );

        // 스프라이트의 피벗을 고려한 버텍스 위치 계산
        Vector2 pivot = sprite.pivot / sprite.rect.size;
        float left = -pivot.x * size.x;
        float right = (1 - pivot.x) * size.x;
        float bottom = -pivot.y * size.y;
        float top = (1 - pivot.y) * size.y;

        // 메시 생성
        Mesh mesh = new Mesh();
        
        // 버텍스
        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(left, bottom, 0),
            new Vector3(right, bottom, 0),
            new Vector3(left, top, 0),
            new Vector3(right, top, 0)
        };

        // UV 좌표
        Vector2[] uv = new Vector2[4]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };

        // 삼각형 인덱스
        int[] triangles = new int[6]
        {
            0, 2, 1,
            2, 3, 1
        };

        // 메시 데이터 설정
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(SpritesToMeshConverter))]
    public class SpritesToMeshConverterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            SpritesToMeshConverter converter = (SpritesToMeshConverter)target;
            if (GUILayout.Button("Convert and Combine"))
            {
                converter.ConvertAndCombine();
            }
        }
    }
#endif
} 