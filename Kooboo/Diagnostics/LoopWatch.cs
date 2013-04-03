using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Kooboo.Diagnostics
{
    public class LoopWatch
    {
        private static Stopwatch _sw;
        private static List<Dictionary<string, long>> _records = new List<Dictionary<string, long>>();

        public static void Start()
        {
            _sw = new Stopwatch();
        }

        public static void Wait()
        {
            _sw.Restart();
        }

        public static void Record(string key)
        {
            if (!_sw.IsRunning)
                return;

            _sw.Stop();
            if (_records.Count == 0 || _records[_records.Count - 1].ContainsKey(key))
            {
                _records.Add(new Dictionary<string, long>());
            }
            if (_records.Count == 1)
            {
                _records[_records.Count - 1].Add(key, 0);
            }
            else
            {
                _records[_records.Count - 1].Add(key, _sw.ElapsedMilliseconds);
            }
        }

        public static IDictionary<string, long> Average()
        {
            Dictionary<string, long> result = new Dictionary<string, long>();
            foreach (var i in _records)
            {
                foreach (var j in i)
                {
                    if (result.ContainsKey(j.Key))
                    {
                        result[j.Key] += j.Value;
                    }
                    else
                    {
                        result.Add(j.Key, j.Value);
                    }
                }
            }
            foreach (var i in result.Keys.ToArray())
            {
                result[i] = result[i] / _records.Count;
            }
            return result;
        }
    }
}
