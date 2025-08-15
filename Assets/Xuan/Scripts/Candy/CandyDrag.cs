/*using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyDrag : MonoBehaviour
{
    [Header("Candy")]
    private Candy candyOld;
    private Candy candyNew;
    private Vector2 firstTouchPos;
    private Vector2 finalTouchPos;
    private float swipeResist = 0.1f;
    private float timeDelayMove = 0.3f;

    [Space]
    [Header("Candy Drag")]
    private bool isDragging = false;
    private bool isMove = false;
    private void Update()
    {
        DragCandy();
    }
    public void DragCandy()
    {
        if (isMove) return;

        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    OnDown(touchPosition);
                    break;
                case TouchPhase.Moved:
                    Debug.Log("Cham tay keo tay");
                    OnDrag();
                    break;
                case TouchPhase.Ended:
                    Debug.Log("Tha tay ra khoi man hinh");
                    OnUp(touchPosition);
                    break;

            }
        }
    }
    public void OnDown(Vector2 posTouch)
    {
        Debug.Log("Cham tay vao man hinh");
        RaycastHit2D hit = Physics2D.Raycast(posTouch, Vector2.down);

        if (hit.collider != null)
        {
            Candy candy = hit.collider.GetComponent<Candy>();
            
            if (candy != null)
            {
                candyOld = candy;
                firstTouchPos = posTouch;
                isDragging = true;
                Debug.Log("Candy selected: " + candy.name);
            }
            else
            {
                Debug.Log("Candy is null");
            }
        }
        else
        {
            Debug.Log("No candy hit at position: " + posTouch);
        }
    }
    public void OnDrag()
    {
        if (!isDragging || candyOld == null) return;

        finalTouchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        Vector2 swipeDelta = finalTouchPos - firstTouchPos;

        if (swipeDelta.magnitude > swipeResist)
        {
            float x = swipeDelta.x;
            float y = swipeDelta.y;

            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                // Horizontal
                if (x > 0)
                    MoveCandy(Vector2.right * 0.5f);
                else
                    MoveCandy(Vector2.left * 0.5f);
            }
            else
            {
                // Vertical
                if (y > 0)
                    MoveCandy(Vector2.up * 0.5f);
                else
                    MoveCandy(Vector2.down * 0.5f);
            }

            isDragging = false;
        }
    }
    public void OnUp(Vector2 posTouch)
    {
        candyOld = null;
        candyNew = null;
        isDragging = false;
    }
    private void MoveCandy(Vector2 direction)
    {
        Vector2 targetPos = candyOld.transform.position + (Vector3)direction;
        RaycastHit2D hit = Physics2D.Raycast(targetPos, Vector2.zero);

        if (hit.collider != null)
        {
            Candy targetCandy = hit.collider.GetComponent<Candy>();
            if (targetCandy != null)
            {
                candyNew = targetCandy;
                isMove = true;
                Swap(candyOld, candyNew);
            }
        }
    }
    public void Swap(Candy candy1, Candy candy2)
    {
        if (candy1 == null || candy2 == null) return;

        //Swap candy objects in the scene

        Vector3 tempPosition = candy1.transform.position;

        Sequence seq = DOTween.Sequence();
        seq.Append(candy1.gameObject.transform.DOMove(candy2.transform.position, timeDelayMove));
        seq.Join(candy2.gameObject.transform.DOMove(tempPosition, timeDelayMove));
        seq.OnComplete(() =>
        {
            isMove = false;
            seq.Kill();

            //Swap their row and column values
            int tempRow = candy1.row;
            int tempColumn = candy1.column;
            candy1.SetPosition(candy2.row, candy2.column);
            candy2.SetPosition(tempRow, tempColumn);

            GirdCandy.Instance.SetGridCandy(candy1);
            GirdCandy.Instance.SetGridCandy(candy2);

            Debug.Log("Swapped candies: " + candy1.name + " and " + candy2.name);

            bool hasMatch1 = GirdCandy.Instance.CheckCandy(candy1);
            bool hasMatch2 = GirdCandy.Instance.CheckCandy(candy2);

            if (!hasMatch1 && !hasMatch2)
            {
                // Nếu không có match -> swap ngược lại
                isMove = true;
                Vector3 backPos = candy1.transform.position;

                Sequence seqBack = DOTween.Sequence();
                seqBack.Append(candy1.transform.DOMove(candy2.transform.position, timeDelayMove));
                seqBack.Join(candy2.transform.DOMove(backPos, timeDelayMove));
                seqBack.OnComplete(() =>
                {
                    isMove = false;
                    seqBack.Kill();

                    int backRow = candy1.row;
                    int backCol = candy1.column;
                    candy1.SetPosition(candy2.row, candy2.column);
                    candy2.SetPosition(backRow, backCol);

                    GirdCandy.Instance.SetGridCandy(candy1);
                    GirdCandy.Instance.SetGridCandy(candy2);
                });
            }
        });

   
    }
}
*/