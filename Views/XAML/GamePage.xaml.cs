using static Android.Content.ClipData;

namespace GridDemos.Views.XAML
{
    public partial class GamePage : ContentPage
    {
        public GamePage()
        {
            InitializeComponent();
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
		public Vector2D Vector { set; get; }

		public Actor(string name, Vector2D vector)
		{
			Name = name;
			Vector = vector;
		}

		public void Attack() { }
		public virtual void Move() { }

		public void CollideWall() { }


	}

	class Hero : Actor { 
		public List<Pickups> Inventory { set; get; }


		public Hero(string name, Vector2D vector) : base (name, vector)
		{
			Inventory = new List<Pickups>();
		}

		public void PickupItem(Pickups item) {
			Inventory.Add(item);
		}

		public void Move(Pickups item){ }

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





}
