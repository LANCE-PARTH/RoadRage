using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 0.3f;

    public enum Lanes {Left, Center, Right };
    public float LaneDistance = 0.5f;
    public float LaneChangeSpeed = 3f;
    private Lanes _currentLane = Lanes.Center;
    private float _desiredXPos;
    [SerializeField]
    private Animator _animator;
    private bool _laneChanged = false;
    [HideInInspector]
    public bool HasCrashed = false;

    public float _timer = 0f;
    public float _speedUpTime = 30.0f; // 30 seconds
    [HideInInspector]
    public bool TutorialActive = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!HasCrashed)
        {
            CheckInput();
            UpdateDesiredXPos();
        }
        _timer += Time.deltaTime;
        if(_timer >= 30.0f)
        {
            _timer = 0f;
            Speed++;
        }
    }

    void LateUpdate()
    {
        if (!HasCrashed)
        {
            if(!TutorialActive)
                MovePlayer();
        }
    }

    void UpdateDesiredXPos()
    {
        switch (_currentLane)
        {
            case Lanes.Left:
                _desiredXPos = -LaneDistance;
                break;
            case Lanes.Center:
                _desiredXPos = 0f;
                break;
            case Lanes.Right:
                _desiredXPos = LaneDistance;
                break;
        }
    }

    void MovePlayer()
    {
        
        var laneUpdatePos = Vector3.Lerp(transform.position, new Vector3(_desiredXPos, transform.position.y, transform.position.z), LaneChangeSpeed * Time.deltaTime);
        transform.position = new Vector3(laneUpdatePos.x, transform.position.y, transform.position.z + Speed * Time.deltaTime);
    }

    void CheckInput()
    {
        if(TutorialActive)
        {
            if(Input.anyKey)
                TutorialActive = false;
        }
        // Go Left
        if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            _laneChanged = false;
            GoLeft();
        }
        // Go Right
        else if(Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            _laneChanged = false;
            GoRight();
        }
    }

    void GoLeft()
    {
        if(_currentLane == Lanes.Center)
        {
            if(!_laneChanged)
            {
                //_animator.SetTrigger("TurnLeftFromCenter");
                _currentLane = Lanes.Left;
                _laneChanged = true;
            }
        }
        else if(_currentLane == Lanes.Right)
        {
            if(!_laneChanged)
            {
                //_animator.SetTrigger("TurnLeftFromRight");
                _currentLane = Lanes.Center;
                _laneChanged = true;
            }
        }
    }

    public float GetDistanceTravelled()
    {
        return transform.position.z;
    }

    void GoRight()
    {
        if(_currentLane == Lanes.Left)
        {
            if(!_laneChanged)
            {
                //_animator.SetTrigger("TurnRightFromLeft");
                _currentLane = Lanes.Center;
                _laneChanged = true;
            }
        }
        else if(_currentLane == Lanes.Center)
        {
            if(!_laneChanged)
            {
                //_animator.SetTrigger("TurnRightFromCenter");
                _currentLane = Lanes.Right;
                _laneChanged = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Obstacle"))
        {
            HasCrashed = true;
        }
    }
}
