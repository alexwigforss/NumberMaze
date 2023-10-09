// using static Android.Content.ClipData;

namespace GridDemos.Views.XAML
{
    public partial class GamePage : ContentPage
    {
        int remIndex;
        int EnemyRemIndex;
        string heroFileName = "walk_attack2.png";
        Level level;// = new Level("Template");
        Hero hero = new Hero("namnet", new Vector2D(6, 5));
        Enemy enemy = new Enemy("fiende", new Vector2D(7,1), 1, 1, 1);
        public GamePage()
        {
            InitializeComponent();
            level = new Level("Template");

            gameGrid.Add(new Image
            {
                StyleId = "heroImage",
                Source = ImageSource.FromFile(heroFileName),
                ZIndex = 1,
            }, hero.Position.X, hero.Position.Y);
            remIndex = gameGrid.Count - 1;

            gameGrid.Add(new Image
            {
                StyleId = "EnemyImage",
                Source = ImageSource.FromFile("orc.png"),
                ZIndex = 1,
            }, enemy.Position.X, enemy.Position.Y);
            EnemyRemIndex = gameGrid.Count - 1;

            DrawMap();
            msg.Text = "";
            //heropos.Text = $"X:{hero.Position.X},Y:{hero.Position.Y}, Random:{enemy.direction}";
            heropos.Text = $"X:{hero.Position.X},Y:{hero.Position.Y},Strength:{hero.Strength}";
        }

        private bool DrawMap()
        {
            for (int i = 0; i <= level.BpArray.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= level.BpArray.GetUpperBound(1); j++)
                {
                    if (level.BpArray[j, i] == 'T')
                    {
						gameGrid.Add(new Image
						{
							StyleId = "obstacle",
							Source = ImageSource.FromFile("fruit_tree2.png"),
							ZIndex = 1,
						}, i, j);
						gameGrid.Add(new BoxView
						{
							ZIndex = 0,
							Color = Colors.DarkGreen
						}, i, j);
					}
					else if (level.BpArray[j, i] == 't')
					{
						gameGrid.Add(new Image
						{
							StyleId = "obstacle",
							Source = ImageSource.FromFile("tree1.png"),
							ZIndex = 1,
						}, i, j);
						gameGrid.Add(new BoxView
						{
							ZIndex = 0,
							Color = Colors.DarkGreen
						}, i, j);
					}
					else if (level.BpArray[j, i] == 'R')
					{
						gameGrid.Add(new Image
						{
							StyleId = "obstacle",
							Source = ImageSource.FromFile("ruins1.png"),
							ZIndex = 1,
						}, i, j);
						gameGrid.Add(new BoxView
						{
							ZIndex = 0,
							Color = Colors.DarkGreen
						}, i, j);
					}
					else if (level.BpArray[j, i] == 'W')
					{
						gameGrid.Add(new Image
						{
							StyleId = "obstacle",
							Source = ImageSource.FromFile("wall.png"),
							ZIndex = 1,
						}, i, j);
						gameGrid.Add(new BoxView
						{
							ZIndex = 0,
							Color = Colors.DarkGreen
						}, i, j);
					}
					else if (level.BpArray[j, i] == 'S')
					{
						gameGrid.Add(new Image
						{
							StyleId = "obstacle",
							Source = ImageSource.FromFile("rock.png"),
							ZIndex = 1,
						}, i, j);
						gameGrid.Add(new BoxView
						{
							ZIndex = 0,
							Color = Colors.DarkGreen
						}, i, j);
					}
					else if (level.BpArray[j, i] == 'F')
					{
						gameGrid.Add(new Image
						{
							StyleId = "obstacle",
							Source = ImageSource.FromFile("fern.png"),
							ZIndex = 1,
						}, i, j);
						gameGrid.Add(new BoxView
						{
							ZIndex = 0,
							Color = Colors.DarkGreen
						}, i, j);
					}
					else if (level.BpArray[j, i] == 'b')
					{
						gameGrid.Add(new Image
						{
							StyleId = "obstacle",
							Source = ImageSource.FromFile("bush.png"),
							ZIndex = 1,
						}, i, j);
						gameGrid.Add(new BoxView
						{
							ZIndex = 0,
							Color = Colors.DarkGreen
						}, i, j);
					}
					else if (char.IsNumber(level.BpArray[j, i]))
					{
						gameGrid.Add(new Label
						{
							StyleId = "obstacle",
							Text = level.BpArray[j, i].ToString(),
                            HorizontalTextAlignment = TextAlignment.Center,
                            VerticalTextAlignment = TextAlignment.Center,
							ZIndex = 1,
						}, i, j);
					}
				}
            }
            return true;
        }

        private void Remove()
        {
            gameGrid.RemoveAt(remIndex);
            if (EnemyRemIndex > remIndex) EnemyRemIndex -= 1;
            gameGrid.RemoveAt(EnemyRemIndex);
        }

        private void Doit(int dir = -1)
        {
            if ((dir == 0)&&(heroFileName == "walk_attack2.png"))
                heroFileName = "walk_attack2left.png";
            else if ((dir == 3)&&(heroFileName == "walk_attack2left.png"))
                heroFileName = "walk_attack2.png";

            heropos.Text = $"X:{hero.Position.X},Y:{hero.Position.Y},Strength:{hero.Strength} ";
            gameGrid.Add(new Image
            {
                Source = ImageSource.FromFile(heroFileName),
                ZIndex = 1,
                StyleId = "test",
                ClassId = "test",
            }, hero.Position.X, hero.Position.Y);
            remIndex = gameGrid.Count - 1;
            Image element = this.FindByName<Image>("test");

            enemy.Move();
            gameGrid.Add(new Image
            {
                Source = ImageSource.FromFile("orc.png"),
                ZIndex = 1,
                StyleId = "test",
                ClassId = "test",
            }, enemy.Position.X, enemy.Position.Y);
            EnemyRemIndex = gameGrid.Count - 1;
        }
        private void Button_Left_Clicked(object sender, EventArgs e)
        {
            Remove();
            hero.Move(0);
            Doit(0);
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
            Doit(3);
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

        Vector2D position;
        public Vector2D Position
        {
            get { return position; }
            set { position = value; }
        }
        public Actor(string name, Vector2D position)
        {
            Name = name;
            Position = position;
        }

        public void Attack() { }
        public virtual void Move() { }
        public bool CollideWall(Vector2D position, Level level, int direction)
        {
            bool collide = false;
            if (direction == 0 && level.BpArray[position.Y, position.X - 1] != ' ' && !Char.IsNumber(level.BpArray[position.Y, position.X - 1]))
            {
                collide = true;
            }
            if (direction == 1 && level.BpArray[position.Y - 1, position.X] != ' ' && !Char.IsNumber(level.BpArray[position.Y - 1, position.X]))
            {
                collide = true;
            }
            if (direction == 2 && level.BpArray[position.Y + 1,position.X] != ' ' && !Char.IsNumber(level.BpArray[position.Y + 1, position.X]))
            {
                collide = true;
            }
            if (direction == 3 && level.BpArray[position.Y,position.X + 1] != ' ' && !Char.IsNumber(level.BpArray[position.Y, position.X + 1]))
            {
                collide = true;
            }
            return collide;
        }
        public int CollideWallOrNum(Vector2D position, Level level, int direction)
        {
            if (direction == 0 && level.BpArray[position.Y, position.X - 1] != ' ')
            {
                if (char.IsNumber(level.BpArray[position.Y, position.X - 1]))
                {
                    int slask = level.BpArray[position.Y, position.X - 1];
                    return slask - 48;
                }
                return -1;
            }
            if (direction == 1 && level.BpArray[position.Y - 1, position.X] != ' ')
            {
                if (char.IsNumber(level.BpArray[position.Y - 1, position.X]))
                {
                    int slask = level.BpArray[position.Y - 1, position.X];
                    return slask - 48;
                }
                return -1;
            }
            if (direction == 2 && level.BpArray[position.Y + 1,position.X] != ' ')
            {
                if (char.IsNumber(level.BpArray[position.Y + 1, position.X]))
                {
                    int slask = level.BpArray[position.Y + 1, position.X];
                    return slask - 48;
                }
                return -1;
            }
            if (direction == 3 && level.BpArray[position.Y,position.X + 1] != ' ')
            {
                if (char.IsNumber(level.BpArray[position.Y, position.X + 1]))
                {
                    int slask = level.BpArray[position.Y, position.X + 1];
                    return slask - 48;
                }
                return -1;
            }
            return 0;
        }


    }

    class Hero : Actor
    {
        public List<Pickups> Inventory { set; get; }
        public int Strength { get => strength; set => strength = value; }
        static int preDir = 2;
        private int strength;

        public Hero(string name, Vector2D position) : base(name, position)
        {
            Inventory = new List<Pickups>();
            Strength = 0;
        }

        public void PickupItem(Pickups item)
        {
            Inventory.Add(item);
        }

        public int Move(int direction)
        {
            //FIXME Ge actor tillgång till level och byt ut new level till level
            //Left
            if (direction == 0 && Position.X > 0 && CollideWallOrNum(Position, new Level("test"), 0) == 0)
            {
                Position = new Vector2D(Position.X - 1, Position.Y);
            }
            else if(direction == 0 && CollideWallOrNum(Position, new Level("test"), 0) > 0)
            {
                Strength += CollideWallOrNum(Position, new Level("test"), 0);
                Position = new Vector2D(Position.X - 1, Position.Y);
                // siffran skrivas över med tom
            }
            //Up
            if (direction == 1 && Position.Y > 0 && CollideWallOrNum(Position, new Level("test"), 1) == 0)
            {
                Position = new Vector2D(Position.X, Position.Y - 1);
            }
            else if(direction == 1 && CollideWallOrNum(Position, new Level("test"), 1) > 0)
            {
                Strength += CollideWallOrNum(Position, new Level("test"), 1);
                Position = new Vector2D(Position.X, Position.Y - 1);
                // siffran skrivas över med tom
                // streeangth öka med siffra
            }
            //Down
            if (direction == 2 && Position.Y < 9 && CollideWallOrNum(Position, new Level("test"), 2) == 0)
            {
                Position = new Vector2D(Position.X, Position.Y + 1);
            }
            else if(direction == 2 && CollideWallOrNum(Position, new Level("test"), 2) > 0)
            {
                Strength += CollideWallOrNum(Position, new Level("test"), 2);
                Position = new Vector2D(Position.X, Position.Y + 1);
                // siffran skrivas över med tom
                // streeangth öka med siffra
            }
            //Right
            if (direction == 3 && Position.X < 9 && CollideWallOrNum(Position, new Level("test"), 3) == 0)
            {
                Position = new Vector2D(Position.X + 1, Position.Y);
            }
            else if(direction == 3 && CollideWallOrNum(Position, new Level("test"), 3) > 0)
            {
                Strength += CollideWallOrNum(Position, new Level("test"), 3);
                Position = new Vector2D(Position.X + 1, Position.Y);
                // siffran skrivas över med tom
                // streeangth öka med siffra
            }
            return direction;
            //preDir = direction;
        }

        public void CollideEnemy() { }
    }

    class Enemy : Actor
    {
        public int Id { set; get; }
        public int Strength { set; get; }
        public int Behaviour { set; get; }
        public int direction;

        public Enum Behaviours;

        public Enemy(string name, Vector2D vector, int id, int str, int behaviour) : base(name, vector)
        {
            Id = id;
            Strength = str;
            Behaviour = behaviour;
        }

        public void RemoveSelf() { }

        public void CollideOther() { }

        public void Move()
        {
            Random rnd = new Random();
            direction = rnd.Next(0,4);
            //Up
            if (direction == 0 && Position.X > 0 && !CollideWall(Position, new Level("test"), 0))
            {
                Position = new Vector2D(Position.X - 1, Position.Y);
            }
            //Up
            else if (direction == 1 && Position.Y > 0 && !CollideWall(Position, new Level("test"), 1))
            {
                Position = new Vector2D(Position.X, Position.Y - 1);
            }
            //Down
            else if (direction == 2 && Position.Y < 9 && !CollideWall(Position, new Level("test"), 2))
            {
                Position = new Vector2D(Position.X, Position.Y + 1);
            }
            //Right
            else if (direction == 3 && Position.X < 9 && !CollideWall(Position, new Level("test"), 3))
            {
                Position = new Vector2D(Position.X + 1, Position.Y);
            }
            else direction += 0;
        }
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
            Width = 10;
            Height = 10;
            Name = name;
            //blueprint = "TTSBSSBTBT\n" +
            //            "S        S\n" +
            //            "S  TSBT  S\n" +
            //            "T  BSTT  S\n" +
            //            "S  S    TT\n" +
            //            "B       BT\n" +
            //            "B TS    ST\n" +
            //            "S BBT  SBT\n" +
            //            "T   S  TBT\n" +
            //            "BBBBBTSBBB";
            bpLines = new string[]
            {
            "T bTFFSFFT",
            "t    2 11T",
            "t  TTRW11F",
            "T  TSTW11T",
            "F  T    TF",
            "T       TS",
            "T TR    FT",
            "F FTW  TFF",
            "T   W  bTS",
            "bbTbSSTSST"};
            bpArray = StringArrToCharArr(bpLines);
            Difficulty = 0;
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
