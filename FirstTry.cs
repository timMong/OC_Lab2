using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Threads
{
  class Program
  {
    static int GLOBAL_COUNT;
    static bool GLOBAL_CHECK = false;
    static int I = 0, J = 0, K = 0, M = 0, N = 0;
    static int GLOBAL_ARR_0 = 3;
    static int GLOBAL_ARR_1 = 3;
    static int GLOBAL_ARR_2 = 3;
    static int GETSET = 0;
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
      // хэш функции паролей
      string[] hashes = {"1115dd800feaacefdf481f1f9070374a2a81e27880f187396db67958b207cbad",
                         "3a7bd3e2360a3d29eea436fcfb7e44c735d117c42d1c1835420b6b9942dd4f1b",
                         "74e1bb62f8dabb8125a58852b63bdf6eaef667cb56ac7f7cdba6d7305c50a22f"};

                        //ed968e840d10d2d313a870bc131a4e2c311d7ad09bdf32b3418147221f51a6e2 - aaaaa
                        //5ef1b1016a260f0c229c5b24afe87fe24a68b4c80f6f89535b87e0ca72a08623 - aaaab
                        //b3a7dc940ffbb84720f62ede7fc0c59303210e259a5c4c4c85bfc26fb5f04f4d - aaabc

      if (GLOBAL_ARR_0 != 3)
        Array.Clear(hashes, 0, 1);
      if (GLOBAL_ARR_1 != 3)
        Array.Clear(hashes, 1, 1);
      if (GLOBAL_ARR_2 != 3)
        Array.Clear(hashes, 2, 1);
      //если хэш пароля совпадает с хэшом ВОЗМОЖОГО пароля - мы нашли пароль, иначе  - не нашли
      for (int i = 0; i < hashes.Length; i++)
      {
        if (hashes[i] == Encrypt(mayBePass))
        {
          check = true;
          Array.Clear(hashes, i, 1);
          GLOBAL_COUNT = i + 1;
          switch (i)
          {
            case 0: GLOBAL_ARR_0 = 0; break;
            case 1: GLOBAL_ARR_1 = 1; break;
            case 2: GLOBAL_ARR_2 = 2; break;

          };
          break;
        }
        else check = false;
      }
      return check;
    }
    public static string NewWord(string mayBePass, int firstStep, int secondStep)
    {
      string newPass = null;
      int i, j, k, m, n;
      N = firstStep;
      const int sizeOfWord = 5;
      const int sizeOfAlphabet = 26;
      char[] arrPass = new char[sizeOfWord];
      char[] alphabet = new char[sizeOfAlphabet];
      int alphabetStart = 0x0061; // начало алфавита - буква 'a'
      for (i = 0; i < sizeOfAlphabet; i++)    // заполняю массив буквами английского алфавита 
        alphabet[i] = Convert.ToChar(alphabetStart + i);
      for (i = 0; i < sizeOfWord; i++) // заполняю массив символов символами переданного слова размера 5
        arrPass[i] = mayBePass[i];

     // arrPass[4] = Convert.ToChar(Convert.ToInt32(arrPass[4]));
      //////////////////////////////////
      for (i = I; i < sizeOfAlphabet; i++)    // выполняю метод грубого (полного) перебора
      {
        arrPass[0] = alphabet[i];
        for (j = J; j < sizeOfAlphabet; j++)
        {
          arrPass[1] = alphabet[j];
          for (k = K; k < sizeOfAlphabet; k++)
          {
            arrPass[2] = alphabet[k];
            for (m = M; m < sizeOfAlphabet; m++)
            {
              arrPass[3] = alphabet[m];
              for (n = N; n < sizeOfAlphabet; n += secondStep)
              {
                //Console.WriteLine("n = " + n + " | " + "secondStep = " + secondStep);
                arrPass[4] = alphabet[n];
                newPass = new string(arrPass);
                Console.WriteLine("Номер - " + (n + n*m*10 + n*m*k*100 + n*m*k*j*1000 + n*m*k*j*i*10000)+" | Новый пароль - " + newPass + " | " + Checking(newPass));
                //////////////////////////////////////////////////////////////
                // можно функцию сделать, которая бы выводила все комбинации//
                //////////////////////////////////////////////////////////////
                if (Checking(newPass))
                {
                  I = i; J = j; K = k; M = m; N = n;
                  i = j = k = m = sizeOfAlphabet;
                  GLOBAL_CHECK = true;
                  break;
                }
                else continue;
              }
            }
          }
        }
      }
      // Console.WriteLine("Возврат много циклов - " + newPass);
      return newPass;

    }
    public static void setCount(int x)
    {
      GETSET = x;
    }
    public static int getCount()
    {
      return GETSET;
    }
    public static int Fun(int firstStep, int secondStep)
    {
      
      string mayBePass = "aaaaa"; // задал начальное слово при проверке пароля
      int countOfMayBePass = 0;
      int countOfGlobalCount = 0;
      //bool check;  переменная для подтверждения того, что mayBePass = mayBePass
      if (countOfGlobalCount == 3) return countOfGlobalCount;
      do //ищем пароль методом грубого перебора всех вариаций 5-буквенных слов и проверяем их
      {
        //Console.WriteLine(countOfMayBePass + " | " + mayBePass + " | " + GLOBAL_CHECK + " | " + Encrypt(mayBePass));
        if (GLOBAL_CHECK)
        {
          Console.WriteLine($"Пароль по {GLOBAL_COUNT}-ому  хэшу: " + mayBePass + " Поток - " + firstStep);
          GLOBAL_COUNT = 0;
          GLOBAL_CHECK = false;
          countOfGlobalCount++;
        }
        mayBePass = NewWord(mayBePass, firstStep, secondStep); // задаем новую комбинацию
        countOfMayBePass++;
      } while (countOfGlobalCount != 3);
      setCount(countOfGlobalCount);
      return countOfGlobalCount;
    }
    delegate int FUN();
    static void Main(string[] args)
    {
      // cоздаем новые потоки
      Console.Write("Сколько создать дополнительных потоков? (0 - 4) Ответ: ");
      string numOfThreads = Console.ReadLine();
      Console.WriteLine();

      // засекаем время
      Stopwatch watch = new Stopwatch();
      watch.Start();

      if (Convert.ToInt32(numOfThreads) == 0)
      {
        Console.WriteLine("Создано 0 потоков!");
        Fun(0, 1);
      }
      else
      {
        //Console.WriteLine($"Создано {Convert.ToInt32(numOfThreads)} потоков!");
        /*Thread[] myThread = new Thread[Convert.ToInt32(numOfThreads)];
        for (int i = 0; i < Convert.ToInt32(numOfThreads); i++)
        {
            myThread[i] = new Thread(new ThreadStart(fun));
            myThread[i].Start();
            Console.WriteLine($"Работает {i+1} поток!");
        }*/
        // FUN[] arrFUN = new FUN[Convert.ToInt32(numOfThreads)];
        // for (int i = 0; i < Convert.ToInt32(numOfThreads); i++)
        // {
        switch (Convert.ToInt32(numOfThreads))
        {
          case 1:
            Fun(0, Convert.ToInt32(numOfThreads));
            break;
          case 2:
            Parallel.Invoke(
              () => Fun(0, Convert.ToInt32(numOfThreads)),
              () => Fun(1, Convert.ToInt32(numOfThreads)));
            break;
          case 3:
            Parallel.Invoke(
              () => Fun(0, Convert.ToInt32(numOfThreads)),
              () => Fun(1, Convert.ToInt32(numOfThreads)),
              () => Fun(2, Convert.ToInt32(numOfThreads)));
            break;
          case 4:
            Parallel.Invoke(
              () => Fun(1, Convert.ToInt32(numOfThreads)),
              () => Fun(2, Convert.ToInt32(numOfThreads)),
              () => Fun(3, Convert.ToInt32(numOfThreads)),
              () => Fun(4, Convert.ToInt32(numOfThreads)));
            break;

        }
          //Console.WriteLine($"Начало {i + 1} потока!");
          //arrFUN[i] = () => Fun(i, Convert.ToInt32(numOfThreads));
         // int count = arrFUN[i]();
          I = 0; J = 0; K = 0; M = 0; N = 0;
         // if (count == 3) break;
       // }
      }
      watch.Stop();
      Console.WriteLine(
      "Время выполнения программы в миллисекундах : " + watch.ElapsedMilliseconds + "мс.\r\n" +
      "Время выполнения программы в секундах : " + watch.Elapsed.Seconds + "сек.\r\n" +
      "Время выполнения программы в Тиках :" + watch.ElapsedTicks + "\r\n"
      );
    }
  }
}
