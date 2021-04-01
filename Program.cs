using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Security.Cryptography;
using System.Threading;
namespace LabRab2
{
    class Program
    {
        public static string Encrypt(string pass)
        {
            try
            {
                SHA256 sha = SHA256.Create();
                byte[] bytes = new ASCIIEncoding().GetBytes(pass);
                sha.ComputeHash(bytes);
                pass = Convert.ToBase64String(sha.Hash);
            }
            catch (Exception exception)
            {
                string text1 = "Error in HashCode : " + exception.Message;
            }
            return pass;
        }
        public static bool Checking(string encryptingPass, string mayBePass)
        {
            if (encryptingPass == Encrypt(mayBePass))  //если хэш пароля совпадает с хэшом ВОЗМОЖОГО пароля - мы нашли пароль, иначе  - не нашли
                return true;
            else return false;
        }
        public static string NewWord(string mayBePass, int check_stop)
        {
            string newPass;
            int stop = 26 * 26 * 26 * 26 * 26;
            const int sizeOfWord = 5;
            const int sizeOfAlphabet = 26;
            char[] arrPass = new char[sizeOfWord];
            char[] alphabet = new char[sizeOfAlphabet];
            int alphabetStart = 0x0061; // начало алфавита - буква 'a'
            for (int i = 0; i < sizeOfAlphabet; i++)    // заполняю массив буквами английского алфавита
            {
                alphabet[i] = Convert.ToChar(alphabetStart + i);
            }
            for (int i = 0; i < sizeOfWord; i++) // заполняю массив символов символами переданного слова размера 5
            {
                arrPass[i] = mayBePass[i];
            }

            //////////////////////////////////

            for (int i = 0; i < sizeOfAlphabet; i++)    // выполняю метод грубого (полного) перебора
            {
                arrPass[0] = alphabet[i];
                for (int j = 0; j < sizeOfAlphabet; j++)
                {
                    arrPass[1] = alphabet[j];
                    for (int k = 0; k < sizeOfAlphabet; k++)
                    {
                        arrPass[2] = alphabet[k];
                        for (int m = 0; m < sizeOfAlphabet; m++)
                        {
                            arrPass[3] = alphabet[m];
                            for (int n = 0; n < sizeOfAlphabet; n++)
                            {
                                arrPass[4] = alphabet[n];
                                if (stop == check_stop)
                                {
                                    i = j = k = m = sizeOfAlphabet;
                                    stop--;
                                    break;
                                }
                                else
                                {
                                    stop--;
                                    continue;
                                }
                            }
                        }
                    }
                }
            }
            newPass = new string(arrPass);
            return newPass;

        }
        static void Main(string[] args)
        {
            // хэш функции паролей
            string[] hashes = {"1115dd800feaacefdf481f1f9070374a2a81e27880f187396db67958b207cbad",
                               "3a7bd3e2360a3d29eea436fcfb7e44c735d117c42d1c1835420b6b9942dd4f1b",
                               "74e1bb62f8dabb8125a58852b63bdf6eaef667cb56ac7f7cdba6d7305c50a22f"};

            // начало таймера; измеряем время работы программы
            Stopwatch watch = new Stopwatch();
            watch.Start();
            System.Threading.Thread.Sleep(1000);

            ////////////////////
            // создаем потоки //
            ////////////////////
            
            string mayBePass = "aaaaa"; // задал начальное слово при проверке пароля
            int check_stop = 26 * 26 * 26 * 26 * 26;    // записываем в переменную кол-во всех варинатов перебора слова
            int countOfMayBePass = 1;
            int j = 0;
            bool check; // переменная для подтверждения того, что mayBePass = mayBePass
            for ( int i = 0; i < 3; i++)
            {
                check = Checking(hashes[i], mayBePass);   // проверяем набор "ааааа", если подошел - конец, иначе переходим в цикл
                if (check)
                    Console.WriteLine($"Найденный пароль {i + 1}-ого хэша: " + mayBePass);
            }
          
            do //ищем пароль методом грубого перебора всех вариаций 5-буквенных слов и проверяем их
            {
                mayBePass = NewWord(mayBePass, check_stop); // задаем новую комбинацию
                check_stop--;
                check = Checking(hashes[j], mayBePass);
               // Console.WriteLine(countOfMayBePass + " | " + mayBePass + " | " + check);
                if (check)
                {
                    Console.WriteLine($"Пароль по {j + 1}-ому  хэшу: " + mayBePass);
                    j++;
                }
                countOfMayBePass++;
            } while (check == false);
            

            // конец таймера - выводим время в консоль
            watch.Stop();
            Console.WriteLine(
            "Время выполнения программы в миллисекундах : " + watch.ElapsedMilliseconds + "мс.\r\n" +
            "Время выполнения программы в секундах : " + watch.Elapsed.Seconds + "сек.\r\n" +
            "Время выполнения программы в Тиках :" + watch.ElapsedTicks + "\r\n"
            );

        }
    }
}
