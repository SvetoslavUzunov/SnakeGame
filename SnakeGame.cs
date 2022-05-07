using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;

namespace SnakeGame
{
    struct Position
    {
        public int row;
        public int col;
        public Position(int row, int col)
        {
            this.row = row;
            this.col = col;
        }
    }

    class program
    {
        static void Main(string[] args)
        {
            byte right = 0;
            byte left = 1;
            byte down = 2;
            byte up = 3;
            int lastFoodTime = 0;
            int foodDissapearTime = 8000; // 8000 милисекунди = 8 секунди
            Position[] directions = new Position[]
            {
                new Position(0,1), //Дясно
                new Position(0,-1), //Ляво
                new Position(1,0), //Долу
                new Position(-1,0), //Горе
            };
            double sleepTime = 100;
            int direction = right;
            Random randomNumberGenerator = new Random(); //Генериране на случайни числа, за да увеличаваме размерът на змията
            Console.BufferHeight = Console.WindowHeight; //Премахваме скрол бар-а
            Position food = new Position(randomNumberGenerator.Next(0, Console.WindowHeight), randomNumberGenerator.Next(0, Console.WindowWidth)); // Създаваме ябълка
            lastFoodTime = Environment.TickCount; //Записваме броят милисекунди от началото на системата
            Console.SetCursorPosition(food.col, food.row); //Показваме храната
            Console.Write("@");
            Queue<Position> snakeElements = new Queue<Position>();
            for (int i = 0; i <= 5; i++)
            {
                snakeElements.Enqueue(new Position(0, i));
            }
            foreach (Position position in snakeElements)
            {
                Console.SetCursorPosition(position.col, position.row);
                Console.Write("*");
            }
            while (true)
            {
                if (Console.KeyAvailable) //Проверка, дали сме натиснали дадено копче(Не блокираме конзолата) 
                {
                    ConsoleKeyInfo userInput = Console.ReadKey();
                    if (userInput.Key == ConsoleKey.LeftArrow)
                    {
                        if (direction != right) direction = left;
                    }
                    if (userInput.Key == ConsoleKey.RightArrow)
                    {
                        if (direction != left) direction = right;
                    }
                    if (userInput.Key == ConsoleKey.UpArrow)
                    {
                        if (direction != down) direction = up;
                    }
                    if (userInput.Key == ConsoleKey.DownArrow)
                    {
                        if (direction != up) direction = down;
                    }
                }
                Position snakeHead = snakeElements.Last();
                Position nextDirection = directions[direction];
                Position snakeNewHead = new Position(snakeHead.row + nextDirection.row, snakeHead.col + nextDirection.col); //Нова позиция на змията

                if (snakeNewHead.row < 0 || snakeNewHead.col < 0 || snakeNewHead.row >= Console.WindowHeight
                    || snakeNewHead.col >= Console.WindowWidth || snakeElements.Contains(snakeNewHead)) //Проверяваме дали змията ни излиза извън конзолата и дали минава през себе си
                {
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine("Game over!");
                    Console.WriteLine("Your points are: {0}", (snakeElements.Count - 6) * 100); //Вземаме броят елементи в змията и го превръщаме в точки, използваме плейсхоудър
                    return; //Спираме програмата ни
                }
                snakeElements.Enqueue(snakeNewHead); //Вмъкваме новата глава
                Console.SetCursorPosition(snakeNewHead.col, snakeHead.row);
                Console.Write("*");
                if (snakeNewHead.col == food.col && snakeNewHead.row == food.row) //Проверяваме дали е стъпала на ябълката ни 
                {
                    do
                    {
                        food = new Position(randomNumberGenerator.Next(0, Console.WindowHeight), randomNumberGenerator.Next(0, Console.WindowWidth)); //Нова храна
                    } while (snakeElements.Contains(food)); //Блокираме появяването на нова ябълка, върху змията ни
                    lastFoodTime = Environment.TickCount;
                    Console.SetCursorPosition(food.col, food.row); //Рисуваме храната
                    Console.Write("@");
                    sleepTime--; //С всяка изядена ябълка, увеличаваме скоростта на змията ни 
                }
                else
                {
                    Position last = snakeElements.Dequeue(); //Премахваме последният елемент от опашката на змията
                    Console.SetCursorPosition(last.col, last.row);
                    Console.Write(" "); //Вземаме последният елемет и на негово място рисуваме празен символ
                }
                //Проверка, дали ябълакта ни е с изтекъл срок, ако е добавяме нова храна
                if (Environment.TickCount - lastFoodTime >= foodDissapearTime)
                {
                    //Преамахваме ябълката с изтекъл срок на годност
                    Console.SetCursorPosition(food.col, food.row);
                    Console.Write(" ");
                    //Създадваме новата храна
                    do
                    {
                        food = new Position(randomNumberGenerator.Next(0, Console.WindowHeight), randomNumberGenerator.Next(0, Console.WindowWidth));
                    } while (snakeElements.Contains(food)); //Блокираме появяването на нова ябълка, върху змията ни
                    lastFoodTime = Environment.TickCount; //Запазваме последното време в което сме я създали
                }
                Console.SetCursorPosition(food.col, food.row); //Рисуваме храната
                Console.Write("@");

                sleepTime -= 0.01;
                Thread.Sleep((int)sleepTime); //Забавяме изпълнението на програмата в милисекунди

                //Когато имаме нов елемент на змията, рисуваме
                //Когато местим змията, премахваме старият елемент от опашката
                //Когато хапваме от ябълката, създаваме нова 
            }
        }
    }
}