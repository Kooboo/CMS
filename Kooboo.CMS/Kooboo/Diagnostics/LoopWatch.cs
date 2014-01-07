#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Kooboo.Diagnostics
{
    /// <summary>
    /// 
    /// </summary>
    public class LoopWatch
    {
        #region Fields
        private static Stopwatch _sw;
        private static List<Dictionary<string, long>> _records = new List<Dictionary<string, long>>();

        #endregion

        #region Methods
        /// <summary>
        /// Starts this instance.
        /// </summary>
        public static void Start()
        {
            _sw = new Stopwatch();
        }

        /// <summary>
        /// Waits this instance.
        /// </summary>
        public static void Wait()
        {
            _sw.Restart();
        }

        /// <summary>
        /// Records the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
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

        /// <summary>
        /// Averages this instance.
        /// </summary>
        /// <returns></returns>
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
        #endregion
    }
}
