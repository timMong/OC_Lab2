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

            ParameterizedThreadStart[] ARRparamThread = new ParameterizedThreadStart[Convert.ToInt32(NUM_OF_THREADS)];
            Thread[] ARRthread = new Thread[Convert.ToInt32(NUM_OF_THREADS)];
            for (int i = 0; i < Convert.ToInt32(NUM_OF_THREADS); i++)
            {
                ARRparamThread[i] = new ParameterizedThreadStart(Fun);
                ARRthread[i] = new Thread(ARRparamThread[i]);
                ARRthread[i].Start(i);
            }
        }
    }
}
