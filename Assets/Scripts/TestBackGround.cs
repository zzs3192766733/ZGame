using GameFramework.UI.UGUI;
using UnityEngine;

public class TestBackGround : MonoBehaviour
{
    private UIListener _uiListenerBackGround;
    private GameObject _image1;
    private GameObject _image2;
    private Vector2 _startPos;
    private Vector2 _currPos;
    [SerializeField] private float radius;
    [SerializeField] private float speed;
    private bool _isMove = false;

    void Start()
    {
        _image1 = transform.GetChild(0).gameObject;
        _image2 = _image1.transform.GetChild(0).gameObject;


        _uiListenerBackGround = gameObject.AddComponent<UIListener>();
        _uiListenerBackGround.BindGameObject(gameObject);


        _uiListenerBackGround.BeginDrag += (eventData) =>
        {
            _isMove = true;
            _image1.SetActive(true);
            _image1.transform.position = eventData.position;
            _startPos = eventData.position;
        };

        _uiListenerBackGround.Drag += (eventData) => { _currPos = eventData.position; };

        _uiListenerBackGround.EndDrag += (eventData) =>
        {
            _isMove = false;
            _image1.SetActive(false);
            _startPos = Vector2.zero;
            _currPos = Vector2.zero;
        };
    }

    private void Update()
    {
        if (!_isMove) return;
        _image2.transform.localPosition = _currPos - _startPos;
        if (_image2.transform.localPosition.magnitude > radius)
            _image2.transform.localPosition = _image2.transform.localPosition.normalized * radius;

        //使操作杆向鼠标位置移动
        if (Vector2.Distance(_currPos, _image1.transform.position) > 0.1f)
        {
            _image1.transform.position =
                Vector2.MoveTowards(_image1.transform.position, _currPos, speed * Time.deltaTime);
            if (_image1.transform.localPosition.magnitude > 300f)
            {
                _image1.transform.localPosition = _image1.transform.localPosition.normalized * 300f;
            }
        }
    }
}