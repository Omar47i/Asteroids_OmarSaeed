// .. interface that must be implemented by all spaceships
public interface IMovingObject
{
    void Move(float dir);
    void Move(UnityEngine.Vector3 dir);
    void Turn(float dir);
    ObjectHitEventBase OnObjectHit();
    MovingObjectHealth GetMovingObjectHealth();
}
