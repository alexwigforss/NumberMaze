// using static Android.Content.ClipData;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace GridDemos.Views.XAML
{
    public partial class GamePage : ContentPage
    {
        int remIndex;
        Level level = new Level("Template");
        
		Obstacle tree1 = new Obstacle("tree1", "tree1", new Vector2D(1, 1), 0);
		Obstacle tree2 = new Obstacle("tree2", "tree2", new Vector2D(2, 2), 1);
		Enemy enemy = new Enemy("enemy", "enemy", new Vector2D(3, 3), 1, 10, 5, 2);
		Hero hero = new Hero("namnet", "hero", 11, new Vector2D(6, 5), 3);
		List<Obstacle> obstacles = new List<Obstacle>();
		List<Enemy> enemies = new List<Enemy>();


		public GamePage()
        {
            InitializeComponent();
			// FIXME För att kartan ska skrivas ut behöver man starta spelet två gånger
			//DrawMap();

			obstacles.Add(tree1);
			obstacles.Add(tree2);
            enemies.Add(enemy);
            //actors.Add(hero);
            
            
            
			gameGrid.Add(new Image
			{
				StyleId = "obstacle",
				Source = ImageSource.FromFile("fruit_tree2.png"),
				ZIndex = 1,
			}, 1,1);
			
			

			gameGrid.Add(new Image
			{
				StyleId = "obstacle",
				Source = ImageSource.FromFile("fruit_tree2.png"),
				ZIndex = 1,
			}, 2,2);

			gameGrid.Add(new Image
			{
				StyleId = "enemy",
				Source = ImageSource.FromFile("orc.png"),
				ZIndex = 1,
			}, 3,3);

			gameGrid.Add(new Image
			{
				StyleId = "heroImage",
				Source = ImageSource.FromFile("dotnet_bot.png"),
				ZIndex = 1,
			}, hero.Position.X, hero.Position.Y);
			remIndex = gameGrid.Count - 1;

		}

        private bool DrawMap()
        {
            for (int i = 0; i <= level.BpArray.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= level.BpArray.GetUpperBound(1); j++)
                {
                    if (level.BpArray[i, j] == '█')
                    {
                        gameGrid.Add(new BoxView
                        {
                            ZIndex = 1,
                            Color = Colors.DarkGray
                        }, i, j);
                    }
                }
            }
            return true;
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
            bool canMove = true;
			foreach (var enemy in enemies)
			{
				if (hero.Position.X == enemy.Position.X && hero.Position.Y - 1 == enemy.Position.Y)
                {
                    if (hero.Strength > enemy.Strength)
                    {
                        gameGrid.RemoveAt(enemy.GridID);
                        remIndex--;
                    }
                    else { 
                        canMove = false;
                    }
                    
				}
				//Remove();
				//hero.Move(1);
				//Doit();
			}
			foreach (var actor in obstacles)
            {
                if (hero.Position.X == actor.Position.X && hero.Position.Y-1 == actor.Position.Y) {
                    canMove = false;
                }
            }
            if (canMove)
            {
				Remove();
				hero.Move(1);
				Doit();
			}
            
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

    class Actor
    {
        public string Name { set; get; }

		public string Type { set; get; }
        public int GridID { set; get; }

		Vector2D position;
        public Vector2D Position// { set; get; }
        {
            get { return position; }   // get method
            set { position = value; }  // set method
        }


		public Actor(string name, string type, Vector2D position, int gridID)
        {
            Name = name;
            Position = position;
            Type = type;
            GridID = gridID;
        }

        public void Attack() { }
        public virtual void Move() { }

        public void CollideWall() { }


    }

    class Hero : Actor
    {
        public List<Pickups> Inventory { set; get; }

		public int Strength { set; get; }


		public Hero(string name, string type, int strength, Vector2D position, int gridID) : base(name, type, position, gridID)
        {
            Inventory = new List<Pickups>();
            Strength = strength;
        }

        public void PickupItem(Pickups item)
        {
            Inventory.Add(item);
        }

        public void Move(int direction)
        {
            if (direction == 0 && Position.X > 0)
            {
                Position = new Vector2D(Position.X - 1, Position.Y);
            }
            if (direction == 1 && Position.Y > 0)
            {
                Position = new Vector2D(Position.X, Position.Y - 1);
            }
            if (direction == 2 && Position.Y < 9)
            {
                Position = new Vector2D(Position.X, Position.Y + 1);
            }
            if (direction == 3 && Position.X < 9)
            {
                Position = new Vector2D(Position.X + 1, Position.Y);
            }
        }

        public void CollideEnemy() { }
    }

	class Obstacle : Actor
	{

		public Obstacle(string name, string type, Vector2D vector, int gridID) : base(name, type, vector, gridID)
		{

		}

		public void RemoveSelf() { }

		public void CollideOther() { }
	}

	class Enemy : Actor
    {
        public int Id { set; get; }
        public int Strength { set; get; }
        public int Behaviour { set; get; }

        public Enum Behaviours;

        public Enemy(string name, string type, Vector2D vector, int id, int str, int behaviour, int gridID) : base(name, type, vector, gridID)
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

        public Pickups(string name, Vector2D vector)
        {
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
        //List<string> bpLines = new List<string>();
        string[] bpLines;
        char[,] bpArray;
        public string Name { set; get; }
        public string Blueprint { get { return blueprint; } set { blueprint = value; } }
        public char[,] BpArray { get => bpArray; set => bpArray = value; }
        static List<string> LevelArray = new List<string>();
        public int Difficulty { set; get; }
        public static int Width { set; get; }
        public static int Height { set; get; }

        //public Level(string name, string blueprint, int difficulty, int width, int height)
        public Level(string name)
        {
            Name = name;
            blueprint = "██████████\n" +
                        "█        █\n" +
                        "█  ████  █\n" +
                        "█  ████  █\n" +
                        "█  █    ██\n" +
                        "█       ██\n" +
                        "█ ██    ██\n" +
                        "█ ███  ███\n" +
                        "█   █  ███\n" +
                        "██████████";
            bpLines = new string[]
            {
            "██████████",
            "█        █",
            "█  ████  █",
            "█  ████  █",
            "█  █    ██",
            "█       ██",
            "█ ██    ██",
            "█ ███  ███",
            "█   █  ███",
            "██████████"};
            bpArray = StringArrToCharArr(bpLines);
            Difficulty = 0;
            Width = 10;
            Height = 10;
        }

        public static char[,] StringArrToCharArr(string[] stringarr)
        {
            // from stringArray TO char 2DcharArray
            char[,] chararr = new char[Width, Height];
            for (int i = 0; i <= chararr.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= chararr.GetUpperBound(1); j++)
                {
                    chararr[i, j] = stringarr[i][j];
                }
                if (i < chararr.GetUpperBound(0)) { }
            }

            return chararr;
        }

        public static string CharArrToString(char[,] chararr)
        {
            // from 2DcharArray BACK TO string
            string landstring = "";
            for (int i = 0; i <= chararr.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= chararr.GetUpperBound(1); j++)
                {
                    landstring += chararr[i, j];
                }
                if (i < chararr.GetUpperBound(0)) landstring += "\n";
            }

            return landstring;
        }

    }

}
