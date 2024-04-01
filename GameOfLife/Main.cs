using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class Main : Form
    {
        private Graphics graphics;// переменная для отрисовки клеток
        private int resolution = 10;// размерность клеток
        private bool[,] field;// массив для красных клеток
        private bool[,] field2;// массив для синих клеток
        private int cols; // кол-во столбцов
        private int rows;// кол-во строк 
        public Main()
        {
            InitializeComponent();
            Main_Resize(null, null);  //изменение размеров поля для игры относительно размеров окна Windows Form 
        }
        private void StartGame() // ф-ия начала игры
        {
            if (timer1.Enabled) // если таймер включен и идет появление след. поколений, то ничего не делаем
            {
                return; // выход из данной ф-ии
            }
            timer1.Start(); // запускаем таймер
        }
        private void NextGeneration() // вычисление и отрисовка поколений клеток
        {
            graphics.Clear(Color.White); // чистим игровое поле для отрисовки след. поколения
            var NewField = new bool[cols,rows];//массив для красных клеток
            var NewField2 = new bool[cols, rows];//массив для синих клеток
            for (int x = 0; x < cols; x++)
            {// циклы для прохода по всем клеткам массива
                for (int y = 0; y < rows; y++)
                {
                    var neighboursCount = CountNeighbours(x, y, field); // нахождение соседей к красной клетки прошлого поколения
                    var hasLife = field[x, y]; // переменная, для определения (жива ли красная клетка)

                    if (!hasLife && neighboursCount == 3) // если клетка не живая и рядом 3 соседки
                        NewField[x, y] = true; // клетка оживает

                    else if (hasLife && (neighboursCount < 2 || neighboursCount > 3)) // иначе если клетка жива и кол-во соседей меньше 2 или больше 3
                        NewField[x, y] = false;// клетка умирает
                    else
                        NewField[x, y] = field[x, y]; // иначе координаты клетки не изменяются
                    if (hasLife)// если клетка жива
                        graphics.FillRectangle(Brushes.Red, x * resolution, y * resolution, resolution - 1, resolution - 1); // то она красная
                }
            }
            for (int x = 0; x < cols; x++) 
            {// цикы для прохода по всем клеткам массива
                for (int y = 0; y < rows; y++)
                {
                    var neighboursCount = CountNeighbours2(x, y,field2); // нахождение соседей к синей клетке прошлого поколения
                    var hasLife = field2[x, y];// переменная, для определения (жива ли синяя клетка)

                    if (!hasLife && neighboursCount == 3)// если клетка не живая и рядом 3 соседки
                        NewField2[x, y] = true;//клетка оживает

                    else if (hasLife && (neighboursCount < 2 || neighboursCount > 3))// иначе если клетка жива и кол-во соседей меньше 2 или больше 3
                        NewField2[x, y] = false;// клетка умирает
                    else
                        NewField2[x, y] = field2[x, y];// иначе координаты клетки не изменяются
                    if (hasLife)// если клетка жва
                        graphics.FillRectangle(Brushes.Blue, x * resolution, y * resolution, resolution - 1, resolution - 1);// то она синяя
                }
            }
            field = NewField;// новое поколение красных клеток становится старым
            field2 = NewField2;//новое поколение синих клеток становится текущим
            pictureBox1.Refresh();// отрисовываем новое поколение
        }
        private int CountNeighbours(int x, int y,bool [,]A)// считаем кол-во соседей у красной клетки
        {
            int count = 0;// считаем кол-во соседей у клетки
            for (int i = -1; i < 2; i++)
            {// считаем кол-во соседей у клетки
                for (int j = -1;j < 2; j++)
                {
                    var col = (x + i + cols) % cols; // нахождение соседних столбцов
                    var row = (y + j + rows) % rows;// нахождение соседних строк
                    var isSelfChecking = col == x && row == y; // самопроверка
                    bool hasLife = A[col, row];// переменная, проверяющая есть жизнь или нет
                    if (hasLife &&  !isSelfChecking)// если клетка жива и не сампроверяется
                    {
                        count++;// записываем соседа
                    }
                }
            }
            return count;
        }

        private int CountNeighbours2(int x, int y, bool[,] B)// считаем кол-во соседей у синей клетки
        {
            int count = 0;// считаем кол-во соседей у клетки
            for (int i = -1; i < 2; i++)
            {// считаем кол-во соседей у клетки
                for (int j = -1; j < 2; j++)
                {
                    var col = (x + i + cols) % cols;// нахождение соседних столбцов
                    var row = (y + j + rows) % rows;// нахождение соседних строк
                    var isSelfChecking = col == x && row == y;// самопроверка
                    bool hasLife = B[col, row];// переменная, проверяющая есть жизнь или нет
                    if (hasLife && !isSelfChecking)// если клетка жива и не самопровеяется
                    {
                        count++;// добавляем соседа
                    }
                }
            }
            return count;
        }
        private void StopGame() // останавливаем игру
        {
            if (!timer1.Enabled) // если таймер не включен
            {
                return; // выходим из данной ф-ии
            }
            timer1.Stop(); // останавливаем таймер
        }

        private void timer1_Tick(object sender, EventArgs e) // таймер
        {
            NextGeneration(); // вызов ф-ии генерации след.поколения
        }
        private void Clear_Click_1(object sender, EventArgs e) // ф-ия очистки игрового поля
        {
            timer1.Enabled = false;  //остановка таймера(а также генерации следущего поколения)
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);  //создание растрового изображения
            graphics = Graphics.FromImage(pictureBox1.Image);  //инициализация переменной graphics растровым изображением
            graphics.Clear(Color.White);  //заполнение поля белым цветом

            for (int i = 0; i < cols; i++)  //цикл, который проходится по всем элементам массива с красными и синими клетками 
            {
                for (int j = 0; j < rows; j++)
                {
                    field[i, j] = false; //и удаляет их с поля
                    field2[i, j] = false; //и удаляет их с поля
                }
            }
        }
        private void Start_Click(object sender, EventArgs e) // ф-ия запускает игру
        {
            StartGame(); // запуск игры
        }

        private void Stop_Click(object sender, EventArgs e) // ф-ия останавливает игру
        {
            StopGame(); // остановка игры
        }
        private bool Granica(int x, int y) // ф-ия для проверки границ рисования клеток
        {
            return x >= 0 && y >= 0 && x < cols && y < rows; // если координаты(при нажатии кнопкой),входят в состав индексов массива, то все ок
        }
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e) // В данном методе мы выполняем отрисовку клеток разных цветов по нажатию на поле
        {
            if (timer1.Enabled)  //таймер включён -> запрещаем рисовать клетки
                return; //выходим из данной функции

            int x = e.Location.X / resolution;  //координата по x в массиве
            int y = e.Location.Y / resolution;  //координата по y в массиве

            if (e.Button == MouseButtons.Left)  //если нажата ЛКМ
            {
                if (Granica(x, y))  //и если координаты клетки в диапазоне индексов массива
                {
                    if (field[x, y])  //и если клетка уже существует в данной координате
                    {
                        field[x, y] = false;  //то, стираем клетку из массива и отрисовываем её белым цветом
                        graphics.FillRectangle(Brushes.White, x * resolution, y * resolution, resolution - 1, resolution - 1);
                    }
                    else  //если клетки ещё не существует в данных координатах
                    {
                        field[x, y] = true;  //то, создаём клетку в данных координатах и отрисовываем её красным цветом
                        graphics.FillRectangle(Brushes.Red, x * resolution, y * resolution, resolution - 1, resolution - 1);
                    }
                }
            }
            if (e.Button == MouseButtons.Right)  //если нажата ПКМ
            {
                if (Granica(x, y))  //если координата клетки в диапазоне индексов массива
                {
                    if (field2[x, y])  //если клетка уже существует в данной координате
                    {
                        field2[x, y] = false;  //стираем клетку из массива и отрисовываем её белым цветом
                        graphics.FillRectangle(Brushes.White, x * resolution, y * resolution, resolution - 1, resolution - 1);
                    }
                    else  //если клетки ещё не существует в данных координатах
                    {
                        field2[x, y] = true;  //создаём клетку в данных координатах и отрисовываем её синим цветом
                        graphics.FillRectangle(Brushes.Blue, x * resolution, y * resolution, resolution - 1, resolution - 1);
                    }
                }
            }
            pictureBox1.Refresh();  //обновляем pictureBox в который внесены изменения с новыми клетками
        }
        private void Exit_Click_1(object sender, EventArgs e) // мтетод выхода из окна
        {
            Application.Exit(); // закрытие окна
        }

        private void Main_Resize(object sender, EventArgs e)//ф-ия для вычисления размерности формы
        {
            rows = pictureBox1.Height / resolution;  //вычисляем размерность экрана(кол-во строк и столбцов)
            cols = pictureBox1.Width / resolution; //делим кол-во пикселей экрана на кол-во пикселей 1 клетки
            // инициализируем размерность массивов 
            field = new bool[cols, rows];
            field2 = new bool[cols, rows];
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height); //создаём растровое изображение в pictureBox
            graphics = Graphics.FromImage(pictureBox1.Image);  //переносим пустое растровое изображение в переменную графики(чтобы потом рисовать там)
            graphics.Clear(Color.White);  //заполняем графику белым цветом
        }
    }
}
