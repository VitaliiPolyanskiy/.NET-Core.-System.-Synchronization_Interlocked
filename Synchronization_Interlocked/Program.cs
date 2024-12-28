using System;
using System.Threading.Tasks;
using System.Threading;

namespace Synchronization_Interlocked
{
    class Program
    {
        static int sharedState = 0;
        /*
         поток 1
         mov ax, g;  в регистр процессора помещается значение переменной g 
         inc ax;     значение регистра увеличивается на 1
         mov g, ax;  в переменную g копируется значение регистра

         поток 2
         mov ax, g;  в регистр процессора помещается значение переменной g 
         inc ax;     значение регистра увеличивается на 1
         mov g, ax;  в переменную g копируется значение регистра
        */

        /*
         mov ax, g;  поток1 - в регистр процессора помещается значение переменной g = 0
         inc ax;     поток1 - значение регистра увеличивается на 1
         mov ax, g;  поток2 - в регистр процессора помещается значение переменной g = 0
         inc ax;     поток2 - значение регистра увеличивается на 1
         mov g, ax;  поток1 - в переменную g копируется значение регистра, равное 1
         mov g, ax;  поток2 - в переменную g копируется значение регистра, равное 1
        */

        // static  int g = 0;

        static public void Increment()
        {
            // g++;
            for (int i = 0; i < 500000; i++)
            {
                //++sharedState;
                Interlocked.Increment(ref sharedState);
            }
        }

        static public void Decrement()
        {
            // g++;
            for (int i = 0; i < 500000; i++)
            {
                //--sharedState;
                Interlocked.Decrement(ref sharedState);
            }
        }

        static void Main(string[] args)
        {
            Task tsk1 = new Task(Increment);
            Task tsk2 = new Task(Decrement);
            try
            {
                tsk1.Start();
                tsk2.Start();
                Task.WaitAll(tsk1, tsk2);
                Console.WriteLine("Результирующее значение: {0}", sharedState);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                tsk1.Dispose();
                tsk2.Dispose();
            }
        }
    }
}
