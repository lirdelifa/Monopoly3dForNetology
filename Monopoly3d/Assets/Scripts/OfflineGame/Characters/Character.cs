using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : SelectableObject
{
    [SerializeField] private CharacterMovement _characterMovement;
    [SerializeField] private CharacterShooting _characterShooting;
    [SerializeField] private CharacterAnimate _characterAnimate;
    [SerializeField] private CharacterInteractionWithGUI _characterInteractionWithGUI;
    [SerializeField] private float _holdTime;

    private const int _maxNumberOfTurnsInPrison = 3;

    public bool IsBotControlled { get; private set; }
    public int Health { get; private set; }
    public string Name { get; private set; }
    public int Cash { get; private set; }
    public int MonetaryCondition { get; private set; }
    public int TurnCounter { get; private set; }
    public int CountTurnBeforeShooting { get; private set; } = 3;

    public bool IsMyTurn { get; set; }
    public bool CanMove { get; set; }
    public bool CanFlip { get; set; }
    public bool CanShoot { get; set; }
    public bool IsDouble { get; set; }
    public bool CanEndTurn { get; set; }
    public int FellOutNumber { get; set; }

    private int _countTurnsInPrison = 0;
    private const int _maxCountTurnsInPrison = 3;
    private int _doubleCounter = 0;
    private const int _maxCountDouble = 3;
    private const int _maxHealth = 100;


    public void SetCash(int cash) => Cash = cash;
    public void SetMonetaryCondition(int monetaryCondition) => MonetaryCondition = monetaryCondition;
    public void AddCash(int value)
    {
        Cash += value;
        _characterInteractionWithGUI.AddMoney(value);
    }
    public void SetName(string name)
    {
        Name = name;
        _characterInteractionWithGUI.SetNameText(name);
    }
    public void ChangeControlledIntelect(bool isControlledByPlayer)
    {
        _characterInteractionWithGUI.ChangeColor(isControlledByPlayer);
        IsBotControlled = !isControlledByPlayer;
    }


    public void AddHealth(int value)
    {
        Health += value;
        if(Health > _maxHealth) Health = _maxHealth;
        _characterInteractionWithGUI.ChangeHealth(Health);
    }

    public override void TakeDamage(int damage, Vector3 direction)
    {
        base.TakeDamage(damage, direction);
        Health -= damage;
        _characterInteractionWithGUI.ChangeHealth(Health);
        print(Name + " получил " + damage.ToString() + " урона!");
        if (Health <= 0) Dead();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _characterMovement.OnEndMove += EndMove;
        _characterShooting.OnEndShoot += EndShoot;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        _characterMovement.OnEndMove += EndMove;
        _characterShooting.OnEndShoot -= EndShoot;
    }

    public void StartTurn()
    {
        TurnCounter++;
        if(_countTurnsInPrison > 0)
        {
            if(_countTurnsInPrison == _maxCountTurnsInPrison) _countTurnsInPrison = 0;
            else
            {
                _countTurnsInPrison++;
                OnAction?.Invoke(CharacterActionType.Arrest, this);
                return;
            }
        }
        _doubleCounter = 0;
        IsMyTurn = true;
        CanEndTurn = false;
        Debug.Log("Ход игрока " + Name);
        OnAction?.Invoke(CharacterActionType.StartTurn, this);
        OnChangeSaveParameters?.Invoke();
    }

    public void Hold() => StartCoroutine(HoldCoroutine());

    private IEnumerator HoldCoroutine()
    {
        float timer = 0;
        while (timer < _holdTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        OnAction?.Invoke(CharacterActionType.EndHold, this);
    }

    public void Flip()
    {
        OnFlip?.Invoke(this);
        CanFlip = false;
    }

    public void EndFlip(int number, bool isDouble)
    {
        if (isDouble) _doubleCounter++;
        if(_doubleCounter == _maxCountDouble)
        {
            OnAction?.Invoke(CharacterActionType.Arrest, this);
            return;
        }
        FellOutNumber = number;
        IsDouble = isDouble;
        CanMove = true;
        OnAction?.Invoke(CharacterActionType.EndFlip, this);
        OnEndFlip?.Invoke(this);
    }

    public void Move(List<Vector3> points)
    {
        CanMove = false;
        _characterMovement.Move(points);
        _characterAnimate.EnableRunAnimation(true);
        OnStartMove?.Invoke(this);
    }

    private void EndMove()
    {
        _characterAnimate.EnableRunAnimation(false);
        OnEndMove?.Invoke(this);
    }

    public void TryPay() => OnAction?.Invoke(CharacterActionType.TryPay, this);
    public void Resume() => OnAction?.Invoke(CharacterActionType.EndMove, this);

    public void Shoot(Vector3 target)
    {
        CanShoot = false;
        _characterShooting.Shoot(target);
    }

    private void EndShoot()
    {
        OnAction?.Invoke(CharacterActionType.EndShoot, this);
    }

    public void EndTurn()
    {
        IsMyTurn = false;
        CanShoot = false;
        CanMove = false;
        CanShoot = false;
        IsDouble = false;
    }

    public void Arrest()
    {
        _countTurnsInPrison++;
        OnAction?.Invoke(CharacterActionType.Arrest, this);
    }

    public void Dead()
    {
        CharactersData.RemoveCharacter(this);
        OnAction?.Invoke(CharacterActionType.Dead, this);
        Destroy(gameObject);
    }

    public CharacterSaveParameters GetSaveParameters()
    {
        CharacterSaveParameters parameters = new CharacterSaveParameters();
        parameters.isBotControlled = IsBotControlled;
        parameters.health = Health;
        parameters.name = Name;
        parameters.cash = Cash;
        parameters.turnCounter = TurnCounter;
        parameters.isMyTurn = IsMyTurn;
        parameters.countTurnsInPrison = _countTurnsInPrison;
        parameters.positionX = transform.position.x;
        parameters.positionY = transform.position.y;
        parameters.positionZ = transform.position.z;
        parameters.rotationY = transform.rotation.eulerAngles.y;
        return parameters;
    }

    public void SetSaveParameters(CharacterSaveParameters parameters)
    {
        IsBotControlled = parameters.isBotControlled;
        Health = parameters.health;
        _characterInteractionWithGUI.ChangeHealth(Health);
        SetCash(parameters.cash);
        SetName(parameters.name);
        TurnCounter = parameters.turnCounter;
        IsMyTurn = parameters.isMyTurn;
        _countTurnsInPrison = parameters.countTurnsInPrison;
        transform.rotation = Quaternion.Euler(0, parameters.rotationY, 0);
    }

    public delegate void ActionDelegate(CharacterActionType actionType, Character character);
    public event ActionDelegate OnAction;

    public delegate void FlipDelegate(Character character);
    public static event FlipDelegate OnFlip;

    public delegate void EndFlipDelegate(Character character);
    public static event EndFlipDelegate OnEndFlip;

    public delegate void EndMoveDelegate(Character character);
    public static event EndMoveDelegate OnEndMove;

    public delegate void StartMoveDelegate(Character character);
    public static event StartMoveDelegate OnStartMove;

    public delegate void ChangeSaveParameters();
    public static event ChangeSaveParameters OnChangeSaveParameters;
}
