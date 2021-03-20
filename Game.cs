using Godot;
using WOLF2D.View;
using WOLF3D;

public class Game : Node2D
{
	public static string Folder = "WOLF3D";
	public static Assets Assets;

	public override void _Ready()
	{
		DownloadShareware.Main(new string[] { Folder });
		Assets = new Assets(Folder);

		VisualServer.SetDefaultClearColor(new Color(Assets.BackgroundColor));

		Level level = new Level()
		{
			Assets = Assets,
			Map = Assets.Maps[0],
		};
		AddChild(level);


		AddChild(new Label()
		{
			Text = "Dopefish lives!",
			Theme = new Theme()
			{
				DefaultFont = Assets.Fonts[0],
			},
		});
	}
}
