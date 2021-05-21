using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Security.Cryptography;
using System.Threading;

namespace _2222_____
{

    class Program
    {
        static string[] hashes = {"1115dd800feaacefdf481f1f9070374a2a81e27880f187396db67958b207cbad",//zyzzx
                               "3a7bd3e2360a3d29eea436fcfb7e44c735d117c42d1c1835420b6b9942dd4f1b",//apple
                               "74e1bb62f8dabb8125a58852b63bdf6eaef667cb56ac7f7cdba6d7305c50a22f"};//mmmmm
        static string NUM_OF_THREADS;
        static int NUM_OF_FOUNDED_HASHES;
        static Stopwatch stopWatch = new Stopwatch();
        public static string Encrypt(string pass)
        {
            try
            {
                byte[] data = Encoding.Default.GetBytes(pass);
                var result = new SHA256Managed().ComputeHash(data);
                return BitConverter.ToString(result).Replace("-", "").ToLower();
            }
            catch (Exception exception)
            {
                string text1 = "Error in HashCode : " + exception.Message;
            }
            return pass;
        }
        public static bool Checking(string mayBePass)
        {
            bool check = false;
            string hash = Encrypt(mayBePass);
            for (int i = 0; i < 3; i++)
            {
                if (hashes[i] == hash)
                {
                    Console.WriteLine("Пароль: " + mayBePass + " | Хэш: " + hashes[i]);
                    NUM_OF_FOUNDED_HASHES++;
                    hashes[i] = "";
                    check = true;
                    break;
                }
                else check = false;
            }
            return check;
        }
        public static void NewWord(string mayBePass, int firstStep, int secondStep)
        {
            int i, j, k, m, n;
            const int sizeOfWord = 5;
            const int sizeOfAlphabet = 26;
            char[] arrPass = new char[sizeOfWord];
            char[] alphabet = new char[sizeOfAlphabet];
            int alphabetStart = 0x0061; // начало алфавита - буква 'a'
            for (i = 0; i < sizeOfAlphabet; i++)    // заполняю массив буквами английского алфавита 
                alphabet[i] = Convert.ToChar(alphabetStart + i);
            for (i = 0; i < sizeOfWord; i++) // заполняю массив символов символами переданного слова размера 5
                arrPass[i] = mayBePass[i];
            string newPass = null;

            for (i = 0; i < sizeOfAlphabet; i++)    // выполняю метод грубого (полного) перебора
            {
                arrPass[0] = alphabet[i];
                for (j = 0; j < sizeOfAlphabet; j++)
                {
                    arrPass[1] = alphabet[j];
                    for (k = 0; k < sizeOfAlphabet; k++)
                    {
                        arrPass[2] = alphabet[k];
                        for (m = 0; m < sizeOfAlphabet; m++)
                        {
                            arrPass[3] = alphabet[m];
                            for (n = firstStep; n < sizeOfAlphabet; n += secondStep)
                            {
                                //Номер - " + (n + n * m * 10 + n * m * k * 100 + n * m * k * j * 1000 + n * m * k * j * i * 10000)
                                arrPass[4] = alphabet[n];
                                newPass = new string(arrPass);
                                Checking(newPass);
                                if (NUM_OF_FOUNDED_HASHES == 3)
                                {
                                    stopWatch.Stop();
                                    TimeSpan ts = stopWatch.Elapsed;
                                    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                                                                        ts.Hours, ts.Minutes, ts.Seconds,
                                                                        ts.Milliseconds / 10);

                                    Console.WriteLine("RunTime " + elapsedTime);
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }
        public static void Fun(object a)
        {
            string mayBePass = "aaaaa"; // задал начальное слово
            int fisrtStep = Convert.ToInt32(a);   // номер буквы, откуда начинаем стар
            int secondStep = Convert.ToInt32(NUM_OF_THREADS);   // шаг, с которым потоки выбирют новую свою новую букву
            do //ищем пароль методом грубого перебора всех вариаций 5-буквенных слов и проверяем их
            {
                // задаем новую комбинацию
                NewWord(mayBePass, fisrtStep, secondStep);
            } while (NUM_OF_FOUNDED_HASHES != 3);
        }
        static void Main(string[] args)
        {
            Console.WriteLine($"! ЗДЕСЬ МОЖНО СОЗДАТЬ ДО {Environment.ProcessorCount} ПОТОКОВ !");
            do
            {
                Console.Write("Сколько создать дополнительных потоков? ");
                NUM_OF_THREADS = Console.ReadLine();
            } while (Convert.ToInt32(NUM_OF_THREADS) >= 8);
            Console.WriteLine($"Создан(о) {NUM_OF_THREADS} поток(ов)!");
            stopWatch.Start();
            switch (Convert.ToInt32(NUM_OF_THREADS))
            {
                case 0:
                    {
                        ParameterizedThreadStart paramThread1 = new ParameterizedThreadStart(Fun);
                        Thread thread1 = new Thread(paramThread1);
                        thread1.Start(0);
                    }
                    break;
                case 1:
                    {
                        ParameterizedThreadStart paramThread1 = new ParameterizedThreadStart(Fun);
                        ParameterizedThreadStart paramThread2 = new ParameterizedThreadStart(Fun);
                        Thread thread1 = new Thread(paramThread1);
                        Thread thread2 = new Thread(paramThread2);
                        thread1.Start(0);
                        thread2.Start(1);
                    }
                    break;
                case 2:
                    {
                        ParameterizedThreadStart paramThread1 = new ParameterizedThreadStart(Fun);
                        ParameterizedThreadStart paramThread2 = new ParameterizedThreadStart(Fun);
                        ParameterizedThreadStart paramThread3 = new ParameterizedThreadStart(Fun);
                        Thread thread1 = new Thread(paramThread1);
                        Thread thread2 = new Thread(paramThread2);
                        Thread thread3 = new Thread(paramThread3);
                        thread1.Start(0);
                        thread2.Start(1);
                        thread3.Start(2);
                    }
                    break;
                case 3:
                    {
                        ParameterizedThreadStart paramThread1 = new ParameterizedThreadStart(Fun);
                        ParameterizedThreadStart paramThread2 = new ParameterizedThreadStart(Fun);
                        ParameterizedThreadStart paramThread3 = new ParameterizedThreadStart(Fun);
                        ParameterizedThreadStart paramThread4 = new ParameterizedThreadStart(Fun);
                        Thread thread1 = new Thread(paramThread1);
                        Thread thread2 = new Thread(paramThread2);
                        Thread thread3 = new Thread(paramThread3);
                        Thread thread4 = new Thread(paramThread4);
                        thread1.Start(0);
                        thread2.Start(1);
                        thread3.Start(2);
                        thread4.Start(3);
                    }
                    break;
                case 4:
                    {
                        ParameterizedThreadStart paramThread1 = new ParameterizedThreadStart(Fun);
                        ParameterizedThreadStart paramThread2 = new ParameterizedThreadStart(Fun);
                        ParameterizedThreadStart paramThread3 = new ParameterizedThreadStart(Fun);
                        ParameterizedThreadStart paramThread4 = new ParameterizedThreadStart(Fun);
                        ParameterizedThreadStart paramThread5 = new ParameterizedThreadStart(Fun);
                        Thread thread1 = new Thread(paramThread1);
                        Thread thread2 = new Thread(paramThread2);
                        Thread thread3 = new Thread(paramThread3);
                        Thread thread4 = new Thread(paramThread4);
                        Thread thread5 = new Thread(paramThread5);
                        thread1.Start(0);
                        thread2.Start(1);
                        thread3.Start(2);
                        thread4.Start(3);
                        thread5.Start(4);
                    }
                    break;
                case 5:
                    {
                        ParameterizedThreadStart paramThread1 = new ParameterizedThreadStart(Fun);
                        ParameterizedThreadStart paramThread2 = new ParameterizedThreadStart(Fun);
                        ParameterizedThreadStart paramThread3 = new ParameterizedThreadStart(Fun);
                        ParameterizedThreadStart paramThread4 = new ParameterizedThreadStart(Fun);
                        ParameterizedThreadStart paramThread5 = new ParameterizedThreadStart(Fun);
                        ParameterizedThreadStart paramThread6 = new ParameterizedThreadStart(Fun);
                        Thread thread1 = new Thread(paramThread1);
                        Thread thread2 = new Thread(paramThread2);
                        Thread thread3 = new Thread(paramThread3);
                        Thread thread4 = new Thread(paramThread4);
                        Thread thread5 = new Thread(paramThread5);
                        Thread thread6 = new Thread(paramThread6);
                        thread1.Start(0);
                        thread2.Start(1);
                        thread3.Start(2);
                        thread4.Start(3);
                        thread5.Start(4);
                        thread6.Start(5);
                    }
                    break;
                case 6:
                    {
                        ParameterizedThreadStart paramThread1 = new ParameterizedThreadStart(Fun);
                        ParameterizedThreadStart paramThread2 = new ParameterizedThreadStart(Fun);
                        ParameterizedThreadStart paramThread3 = new ParameterizedThreadStart(Fun);
                        ParameterizedThreadStart paramThread4 = new ParameterizedThreadStart(Fun);
                        ParameterizedThreadStart paramThread5 = new ParameterizedThreadStart(Fun);
                        ParameterizedThreadStart paramThread6 = new ParameterizedThreadStart(Fun);
                        ParameterizedThreadStart paramThread7 = new ParameterizedThreadStart(Fun);
                        Thread thread1 = new Thread(paramThread1);
                        Thread thread2 = new Thread(paramThread2);
                        Thread thread3 = new Thread(paramThread3);
                        Thread thread4 = new Thread(paramThread4);
                        Thread thread5 = new Thread(paramThread5);
                        Thread thread6 = new Thread(paramThread6);
                        Thread thread7 = new Thread(paramThread7);
                        thread1.Start(0);
                        thread2.Start(1);
                        thread3.Start(2);
                        thread4.Start(3);
                        thread5.Start(4);
                        thread6.Start(5);
                        thread7.Start(6);
                    }
                    break;
                case 7:
                    {
                        ParameterizedThreadStart paramThread1 = new ParameterizedThreadStart(Fun);
                        ParameterizedThreadStart paramThread2 = new ParameterizedThreadStart(Fun);
                        ParameterizedThreadStart paramThread3 = new ParameterizedThreadStart(Fun);
                        ParameterizedThreadStart paramThread4 = new ParameterizedThreadStart(Fun);
                        ParameterizedThreadStart paramThread5 = new ParameterizedThreadStart(Fun);
                        ParameterizedThreadStart paramThread6 = new ParameterizedThreadStart(Fun);
                        ParameterizedThreadStart paramThread7 = new ParameterizedThreadStart(Fun);
                        ParameterizedThreadStart paramThread8 = new ParameterizedThreadStart(Fun);
                        Thread thread1 = new Thread(paramThread1);
                        Thread thread2 = new Thread(paramThread2);
                        Thread thread3 = new Thread(paramThread3);
                        Thread thread4 = new Thread(paramThread4);
                        Thread thread5 = new Thread(paramThread5);
                        Thread thread6 = new Thread(paramThread6);
                        Thread thread7 = new Thread(paramThread7);
                        Thread thread8 = new Thread(paramThread8);
                        thread1.Start(0);
                        thread2.Start(1);
                        thread3.Start(2);
                        thread4.Start(3);
                        thread5.Start(4);
                        thread6.Start(5);
                        thread7.Start(6);
                        thread8.Start(7);
                    }
                    break;
                default:
                    Console.WriteLine("! ERROR !\n Введите приавильные данные !");
                    break;
            }
        }
    }
}
