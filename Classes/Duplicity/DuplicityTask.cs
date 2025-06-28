

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text.Json;
using System.Threading.Tasks;

namespace MEMOS.Classes.Tasks
{
    public class DuplicityTask
    {
        public DuplicityTask()
        {
           
        }

        public void GetResult()
        {
            int count=1000000;
           
            int max = 100;

            var rnd = new Random();

            int[] values = new int[count];

            for (var i = 0; i < values.Length; i++)
            {
                values[i] = rnd.Next(0, max);
            }

            ConcurrentDictionary<int, int> duplicity = new ConcurrentDictionary<int, int>();

            try
            {
                Parallel.ForEach(values, x =>

                   duplicity.AddOrUpdate(x, 1, (key, oldVal) => oldVal + 1)
                   );

                Parallel.ForEach(duplicity, item =>
                {
                    if (item.Value > 1)
                    {
                        Console.WriteLine($"Položka {item.Key} je v poli {item.Value} x");
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
    }

}

