using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ClientWF
{
    public partial class GameWindow : Form
    {
        private const int _cellsCount = 11;
        private int _cellSize = 30;
        private string _alphabet = "АБВГДЕЖЗИК";

        private int[,] _map = new int[_cellsCount, _cellsCount];

        private Button[,] _myButtons = new Button[_cellsCount, _cellsCount];
        private Button[,] _enemyButtons = new Button[_cellsCount, _cellsCount];

        private bool _isPlaying = false;

        private CellsToShipsConverter _cellsToShipsConverter;
        private BytesConverter _bytesConverter;


        private Socket _server;
        private bool _isConnected = false;
        
        public GameWindow()
        {
            _cellsToShipsConverter = new CellsToShipsConverter();
            _bytesConverter = new BytesConverter();
            InitializeComponent();
            CreateMaps();
        }
        
        private void CreateMaps()
        {

            for (int i = 0; i < _cellsCount; i++)
            {
                for (int j = 0; j < _cellsCount; j++)
                {
                    _map[i, j] = 0;

                    var button = new Button();
                    FormatButton(button, j, i);

                    if (j == 0 || i == 0)
                    {
                        FormatEdgeButton(button);
                        if (i == 0 && j > 0)
                            button.Text = _alphabet[j - 1].ToString();
                        if (j == 0 && i > 0)
                            button.Text = i.ToString();
                    }
                    else
                    {
                        button.Click += new EventHandler(ConfigureShips);
                    }
                    _myButtons[i, j] = button;
                    panel1.Controls.Add(button);
                }
            }
            for (int i = 0; i < _cellsCount; i++)
            {
                for (int j = 0; j < _cellsCount; j++)
                {
                    _map[i, j] = 0;

                    Button button = new Button();
                    FormatButton(button, j, i);
                    
                    if (j == 0 || i == 0)
                    {
                        FormatEdgeButton(button);
                        if (i == 0 && j > 0)
                            button.Text = _alphabet[j - 1].ToString();
                        if (j == 0 && i > 0)
                            button.Text = i.ToString();
                    }
                    else
                    {
                        // button.Click += new EventHandler(PlayerShoot);
                    }
                    _enemyButtons[i, j] = button;
                    panel2.Controls.Add(button);
                }
            }
            Label map1 = new Label();
            map1.Text = "Карта игрока";
            map1.Location = new Point(_cellsCount * _cellSize / 2, _cellsCount * _cellSize + 10);
            Controls.Add(map1);

            Label map2 = new Label();
            map2.Text = "Карта противника";
            map2.Location = new Point(350 + _cellsCount * _cellSize / 2, _cellsCount * _cellSize + 10);
            Controls.Add(map2);

            Button startButton = new Button();
            startButton.Text = "Начать";
            startButton.Click += Start;
            startButton.Location = new Point(0, _cellsCount * _cellSize + 20);
            Controls.Add(startButton);
        }

        private void Start(object sender, EventArgs e)
        {
            if (_isConnected == false)
            {
                ConnectToServer();
            }
            List<ShipDataJson> shipDataJsons =
                _cellsToShipsConverter.Execute(_map.Clone() as int[,]);
            byte[] bytesShips = 
                _bytesConverter.ConvertShipToBytes(shipDataJsons);
            
            _server.Send(bytesShips);
            if (GetСonfirmation() == false)
            {
                richTextBox1.Text = "Корабли не подходят";
                return;
            }
            

            bool isAllPlayersConnected = false;
            richTextBox1.Text = "Ждем других";
            Thread.Sleep(1000);
            do
            {
                
                
                isAllPlayersConnected = GetСonfirmation();
            } while (isAllPlayersConnected == false);
            richTextBox1.Text = "Игроки найдены";
        }

        private bool GetСonfirmation()
        {
            byte[] infoBuffer = new byte[1];
            _server.Receive(infoBuffer, SocketFlags.None);
            return BitConverter.ToBoolean(infoBuffer, 0);
        }

        private void ConnectToServer()
        {
            int port = 25565;
            string address = "127.0.0.1";
            
            _server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _server.Connect(new IPEndPoint(IPAddress.Parse(address), port));
            _isConnected = true;
        }

        private void FormatButton(Button button, int x, int y)
        {
            button.Location = new Point(x * _cellSize, y * _cellSize);
            button.Size = new Size(_cellSize, _cellSize);
            button.BackColor = Color.White;
        }
        private void FormatEdgeButton(Button button)
        {
            button.Enabled = false;
            button.BackColor = Color.Gray;
        }
        
        public void ConfigureShips(object sender, EventArgs e)
        {
            Button pressedButton = sender as Button;
            if (!_isPlaying)
            {
                if (_map[pressedButton.Location.Y / _cellSize, pressedButton.Location.X / _cellSize] == 0)
                {
                    pressedButton.BackColor = Color.Aqua;
                    _map[pressedButton.Location.Y / _cellSize, pressedButton.Location.X / _cellSize] = 1;
                }
                else
                {
                    pressedButton.BackColor = Color.White;
                    _map[pressedButton.Location.Y / _cellSize, pressedButton.Location.X / _cellSize] = 0;
                }
            }
        }
        
        
        
        
        
        
    }
    
    
    
    
}