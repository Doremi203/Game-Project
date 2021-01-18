using UnityEngine;
using UnityEngine.AI;

public class Attacking : IState, IStateEnterCallbackReciver, IStateExitCallbackReciver, IStateTickCallbackReciver
{

    private HumanAI ai;
    private Actor npc;
    private NavMeshAgent agent;
    private WeaponHolder weaponHolder;
    private Vector3 interceptionPoint;
    private Vector3 currentPrediction;
    private float ableToAttackTime;

    public Attacking(HumanAI ai, Actor npc, NavMeshAgent agent, WeaponHolder weaponHolder)
    {
        this.ai = ai;
        this.npc = npc;
        this.agent = agent;
        this.weaponHolder = weaponHolder;
    }

    public void OnEnter()
    {
        agent.ResetPath();
        ableToAttackTime = Time.time + weaponHolder.CurrentWeapon.NPCSettings.FirstAttackDelay;
    }

    public void OnExit()
    {
        weaponHolder.CurrentWeapon.Use(false);
    }

    public void Tick()
    {
        CalculatePrediction();
        npc.GetComponent<RotationController>().LookAtIgnoringY(currentPrediction);
        if (Time.time >= ableToAttackTime) TryShoot();
    }

    private void CalculatePrediction()
    {
        Player _player = Player.Instance;
        currentPrediction = _player.transform.position;

        bool usePredictions = true;

        WeaponProjectilesLauncher _projectilesLauncher = weaponHolder.CurrentWeapon.GetComponent<WeaponProjectilesLauncher>();
        if (_projectilesLauncher && usePredictions)
        {

            Vector3 _playerPosition = _player.transform.position;
            float _bulletSpeed = _projectilesLauncher.BulletSpeed;

            Vector3 IC = CalculateInterceptCourse(_playerPosition, _player.GetComponent<CharacterController>().velocity, npc.transform.position, _bulletSpeed);
            if (IC != Vector3.zero)
            {
                IC.Normalize();
                float interceptionTime1 = FindClosestPointOfApproach(_playerPosition, _player.GetComponent<CharacterController>().velocity, npc.transform.position, IC * _bulletSpeed);
                interceptionPoint = _playerPosition + _player.GetComponent<CharacterController>().velocity * interceptionTime1;
            }

            currentPrediction = interceptionPoint;

        }

        Debug.DrawLine(currentPrediction, currentPrediction + Vector3.up * 10f, Color.green);
    }

    private void TryShoot()
    {
        float _weaponAttackAngle = weaponHolder.CurrentWeapon.NPCSettings.AttackAngle;

        Vector3 _targetDirection = currentPrediction - npc.transform.position;
        _targetDirection.y = 0;

        float _angleToPrediction = Vector3.Angle(_targetDirection, npc.transform.forward);

        bool _shouldShoot = weaponHolder.CurrentWeapon.CanUse() && _angleToPrediction <= _weaponAttackAngle;

        weaponHolder.CurrentWeapon.Use(_shouldShoot);
    }

    public static Vector3 CalculateInterceptCourse(Vector3 aTargetPos, Vector3 aTargetSpeed, Vector3 aInterceptorPos, float aInterceptorSpeed)
    {
        Vector3 targetDir = aTargetPos - aInterceptorPos;
        float iSpeed2 = aInterceptorSpeed * aInterceptorSpeed;
        float tSpeed2 = aTargetSpeed.sqrMagnitude;
        float fDot1 = Vector3.Dot(targetDir, aTargetSpeed);
        float targetDist2 = targetDir.sqrMagnitude;
        float d = (fDot1 * fDot1) - targetDist2 * (tSpeed2 - iSpeed2);
        if (d < 0.1f)  // negative == no possible course because the interceptor isn't fast enough
            return Vector3.zero;
        float sqrt = Mathf.Sqrt(d);
        float S1 = (-fDot1 - sqrt) / targetDist2;
        float S2 = (-fDot1 + sqrt) / targetDist2;
        if (S1 < 0.0001f)
        {
            if (S2 < 0.0001f)
                return Vector3.zero;
            else
                return (S2) * targetDir + aTargetSpeed;
        }
        else if (S2 < 0.0001f)
            return (S1) * targetDir + aTargetSpeed;
        else if (S1 < S2)
            return (S2) * targetDir + aTargetSpeed;
        else
            return (S1) * targetDir + aTargetSpeed;
    }

    public static float FindClosestPointOfApproach(Vector3 aPos1, Vector3 aSpeed1, Vector3 aPos2, Vector3 aSpeed2)
    {
        Vector3 PVec = aPos1 - aPos2;
        Vector3 SVec = aSpeed1 - aSpeed2;
        float d = SVec.sqrMagnitude;
        // if d is 0 then the distance between Pos1 and Pos2 is never changing
        // so there is no point of closest approach... return 0
        // 0 means the closest approach is now!
        if (d >= -0.0001f && d <= 0.0002f)
            return 0.0f;
        return (-Vector3.Dot(PVec, SVec) / d);
    }

}