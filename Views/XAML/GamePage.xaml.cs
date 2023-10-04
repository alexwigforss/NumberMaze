// using static Android.Content.ClipData;
using System.Xml.Linq;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace GridDemos.Views.XAML
{
    public partial class GamePage : ContentPage
    {
		int remIndex;
        Hero hero = new Hero("namnet", new Vector2D(6, 5));
        public GamePage()
        {
            InitializeComponent();
            gameGrid.Add(new BoxView
            {
                ZIndex = -1,
                Color = Colors.Blue
            }, 5, 5);
            gameGrid.Add(new BoxView
            {
                ZIndex = -1,
                Color = Colors.BlueViolet
            }, 6, 5);
            gameGrid.Add(new Image
            {
                StyleId = "heroImage",
                Source = ImageSource.FromFile("dotnet_bot.png"),
                ZIndex = 1,
            }, hero.Position.X, hero.Position.Y);
			// bilden är inlagd så vi sparar dess index som är den sista i gridens
			remIndex = gameGrid.Count - 1;
			msg.Text = "Hej och hå";
        }

		private void Remove()
		{
            gameGrid.RemoveAt(remIndex);
		}
        private void Doit()
        {
				gameGrid.Add(new Image
				{
					Source = ImageSource.FromFile("dotnet_bot.png"),
					ZIndex = 1,
				}, hero.Position.X, hero.Position.Y);
				remIndex = gameGrid.Count - 1;	
        }

		private void Button_Left_Clicked(object sender, EventArgs e)
        {   // Nedan ger inget error men returnar null och gör därmed inte jobbet.
			// Image heroImage = gameGrid.FindByName<Image>("heroImage");
			// gameGrid.Children.Remove(heroImage);
			Remove();
            hero.Move(0);
			Doit();
        }
        private void Button_Up_Clicked(object sender, EventArgs e)
        {
            Remove();
            hero.Move(1);
			Doit();
        }
        private void Button_Down_Clicked(object sender, EventArgs e)
        {
            Remove();
            hero.Move(2);
			Doit();
        }
        private void Button_Right_Clicked(object sender, EventArgs e)
        {
            Remove();
            hero.Move(3);
			Doit();
        }
    }

    struct Vector2D
	{
		public int X { get; set; }
		public int Y { get; set; }

		public Vector2D(int x, int y)
		{
			X = x;
			Y = y;
		}
	}

	class Actor { 
        public string Name { set; get; }
		
		Vector2D position;
        public Vector2D Position// { set; get; }
        {
            get { return position; }   // get method
            set { position = value; }  // set method
        }
        public Actor(string name, Vector2D position)
		{
			Name = name;
			Position = position;
		}

		public void Attack() { }
		public virtual void Move() { }

		public void CollideWall() { }


	}

	class Hero : Actor { 
		public List<Pickups> Inventory { set; get; }


		public Hero(string name, Vector2D position) : base (name, position)
		{
			Inventory = new List<Pickups>();
		}

		public void PickupItem(Pickups item) {
			Inventory.Add(item);
		}

		public void Move(int direction){
			if (direction == 0 && Position.X>0) {
                Position = new Vector2D(Position.X-1, Position.Y);
			}
			if (direction == 1 && Position.Y>0) {
                Position = new Vector2D(Position.X, Position.Y-1);
			}
			if (direction == 2 && Position.Y<9) {
                Position = new Vector2D(Position.X, Position.Y+1);
			}
			if (direction == 3 && Position.X<9) {
                Position = new Vector2D(Position.X+1, Position.Y);
			}
		}

		public void CollideEnemy() { }
	}

	class Enemy : Actor
	{
		public int Id { set; get; }
		public int Strength { set; get; }
		public int Behaviour { set; get; }

		public Enum Behaviours;

		public Enemy(string name, Vector2D vector, int id, int str, int behaviour) : base(name, vector)
		{
			Id = id;
			Strength = str;
			Behaviour = behaviour;
		}

		public void RemoveSelf() { }

		public void CollideOther() { }
	}

	class Pickups
	{
		public string Name { set; get; }
		public Vector2D Vector { set; get; }

		public Pickups(string name, Vector2D vector) {
			Name = name;
			Vector = vector;
		
		}

	}

	class Number : Pickups
	{
		public int Value { set; get; }

		public Number(string name, Vector2D vector, int value) : base(name, vector)
		{ 
			Value = value;
		}

	}

	class Operator : Pickups
	{
		public char[] Signs { set; get; }

		public Operator(string name, Vector2D vector, char[] signs) : base(name, vector)
		{
			Signs = signs;
		}

	}
	class Level
	{
		string blueprint; 
		public string Name{ set; get; }
		public string Blueprint
        {
            get { return blueprint; }   // get method
            set { blueprint = value; }  // set method
        }
        static List<string> LevelArray = new List<string>();
        public int Difficulty { set; get; }
		public int Width { set; get; }
		public int Height { set; get; }


		public Level(string name, string blueprint, int difficulty, int width, int height)
		{
			blueprint = "██████████\n" +
                        "█        █\n" +
						"█  ████  █\n" +
						"█  ████  █\n" +
						"█  █    ██\n" + // (32, 5)
						"█       ██\n" +
						"█ ██    ██\n" +
						"█ ███  ███\n" +
						"█   █  ███\n" +
						"██████████");
            Name = name;
			Blueprint = blueprint;
			Difficulty = difficulty;
			Width = width;
			Height = height;
		}

	}

}
