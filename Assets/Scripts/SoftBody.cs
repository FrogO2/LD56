using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

public class SoftBody : MonoBehaviour
{
#region Constants

private const float splineOffset = 0.2f;
#endregion

    #region Fields
    [SerializeField]
    public SpriteShapeController spriteShape;
    [SerializeField]
    public Transform[] points;

    public SpriteRenderer[] sprites;
#endregion
    #region MonoBehaviour Callbacks
    private void Awake()
    {
        UpdateVerticies();
    }

    private void Update()
    {
        UpdateVerticies();
        UpdateSprites();
    }

    #endregion

    #region privateMethods

    private void UpdateVerticies()
    {
        Vector2 _center = points[6].localPosition;
        for (int i = 0; i < points.Length - 1; i++)
        {
            Vector2 _vertex = points[i].localPosition;
            Vector2 _towardsCenter = (_center - _vertex).normalized;
            float _colliderRadius = points[i].gameObject.GetComponent<CircleCollider2D>().radius;
            try
            {

                spriteShape.spline.SetPosition(i, (_vertex - _towardsCenter * _colliderRadius));
            }
            catch
            {
                Debug.Log("Spline points are too close to each other.. recalculate");
                spriteShape.spline.SetPosition(i, (_vertex - _towardsCenter * (_colliderRadius + splineOffset)));
            }

            Vector2 _It = spriteShape.spline.GetLeftTangent(i);

            Vector2 _newRt = Vector2.Perpendicular(_towardsCenter) * _It.magnitude;
            Vector2 _newLt = Vector2.zero - (_newRt);

            spriteShape.spline.SetRightTangent(i, -_newRt);
            spriteShape.spline.SetLeftTangent(i, -_newLt);
        }
    }
    
    private void UpdateSprites()
    {
        if (points.Length != 7 || sprites.Length != 6)
        {
            return;
        }

        // 在每条线的中点位置放置Sprite
        for (int i = 0; i < 6; i++)
        {
            Vector2 midpoint = (points[i].position + points[(i + 1) % 6].position) / 2f; // 计算中点位置
            
            SpriteRenderer spriteObject = sprites[i]; // 设置Sprite
            spriteObject.transform.position = midpoint; // 设置位置为中点
            Vector3 lastDirection = (points[0].position - points[5].position).normalized;
            float lastAngle = Mathf.Atan2(lastDirection.y, lastDirection.x) * Mathf.Rad2Deg;
            spriteObject.transform.rotation = Quaternion.Euler(0, 0, lastAngle);
        }
    }
    #endregion
}
