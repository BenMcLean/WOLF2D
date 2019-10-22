using Godot;

public class Camera2DFreeLook : Camera2D
{
    //public override void _Ready()
    //{

    //}

    public override void _Process(float delta)
    {
        GlobalPosition += Vector2.Right * 100 * delta;
    }
}
