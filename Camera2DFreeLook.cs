using Godot;

public class Camera2DFreeLook : Camera2D
{
    public override void _Ready()
    {
        Current = true;
    }

    public override void _Process(float delta)
    {
        Position += InputDirection * (speed * delta);
    }

    public static Vector2 InputDirection
    {
        get
        {
            return new Vector2(
            Direction(Input.IsActionPressed("ui_right")) - Direction(Input.IsActionPressed("ui_left")),
            Direction(Input.IsActionPressed("ui_down")) - Direction(Input.IsActionPressed("ui_up"))
            );
        }
    }

    public static int Direction(bool boolean)
    {
        return boolean ? 1 : -1;
    }

    public static readonly float speed = 1000f;
}
