using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FlexibleGridLayout : LayoutGroup
{
    public enum FitType
    {
        UNIFORM,
        WIDTH,
        HEIGHT, 
        FIXEDROWS,
        FIXEDCOLUMNS
    }

    public FitType m_fitType;
    public int m_rows;
    public int m_columns;
    public Vector2 m_cellSize;
    public Vector2 m_spacing;
    public bool m_fitX;
    public bool m_fitY;

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();

        if (m_fitType == FitType.UNIFORM || m_fitType == FitType.HEIGHT || m_fitType == FitType.WIDTH)
        {
            m_fitX = true;
            m_fitY = true;
            float sqrRt = Mathf.Sqrt(transform.childCount);
            m_rows = Mathf.CeilToInt(sqrRt);
            m_columns = Mathf.CeilToInt(sqrRt);            
        }

        if (m_fitType == FitType.WIDTH || m_fitType == FitType.FIXEDCOLUMNS)
        {
            m_rows = Mathf.CeilToInt(transform.childCount / (float)m_columns);
        }
        if (m_fitType == FitType.HEIGHT || m_fitType == FitType.FIXEDROWS)
        {
            m_columns = Mathf.CeilToInt(transform.childCount / (float)m_rows);
        }


        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        float cellWidth = (parentWidth / m_columns) - ((m_spacing.x / m_columns) * (m_columns - 1)) - (padding.left / m_columns) - (padding.right / m_columns);
        float cellHeight = (parentHeight / m_rows) - ((m_spacing.y / m_rows) * (m_rows - 1)) - (padding.top / m_rows) - (padding.bottom / m_rows);

        m_cellSize.x = m_fitX ? cellWidth : m_cellSize.x;
        m_cellSize.y = m_fitY ? cellHeight : m_cellSize.y;

        int columnCount = 0;
        int rowCount = 0;

        for (int i = 0; i < rectChildren.Count; i++)
        {
            rowCount = i / m_columns;
            columnCount = i % m_columns;

            var item = rectChildren[i];

            float xPos = (m_cellSize.x * columnCount) + (m_spacing.x * columnCount) + padding.left;
            float yPos = (m_cellSize.y * rowCount) + (m_spacing.y * rowCount) + padding.top;

            SetChildAlongAxis(item, 0, xPos, m_cellSize.x);
            SetChildAlongAxis(item, 1, yPos, m_cellSize.y);
        }

        if (!m_fitX)
        {
            RectTransform rect = GetComponent<RectTransform>();
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_cellSize.x * m_columns);
        }
        if (!m_fitY)
        {
            RectTransform rect = GetComponent<RectTransform>();
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_cellSize.y * m_rows);
        }
    }

    private void OnValidate()
    {
        //base.OnValidate();
        m_spacing.x = m_spacing.x < 0 ? 0 : m_spacing.x;
        m_spacing.y = m_spacing.y < 0 ? 0 : m_spacing.y;
    }

    public override void CalculateLayoutInputVertical()
    {

    }

    public override void SetLayoutHorizontal()
    {

    }

    public override void SetLayoutVertical()
    {

    }
}
