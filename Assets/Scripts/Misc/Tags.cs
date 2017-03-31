// .. Store all game tags in a public class to be accessible anywhere.
public class Tags
{
    public const string Player = "Player";
    public const string Asteroid = "Asteroid";
    public const string EnemyShip = "EnemyShip";
    public const string PlayerProjectile = "PlayerProjectile";
    public const string EnemyProjectile = "EnemyProjectile";
    public const string GameManager = "GameController";
    public const string LevelManager = "LevelManager";
    public const string CannonUpgradePickup = "CannonUpgradePickup";
    public const string MobileControls = "MobileControls";
}

public enum MovementDirection
{
    Forward = 1,
    Backward = -1,
}

public enum TurnDirection
{
    Right = 1,
    Left = -1,
}

public enum Axis
{
    Vertical,
    Horizontal
};