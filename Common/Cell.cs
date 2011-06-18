using System.Collections.Generic;

namespace MazeSolver.Common
{
    public class Cell
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Cell East { get; set; }
        public Cell West { get; set; }
        public Cell North { get; set; }
        public Cell South { get; set; }

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }

        public object this[string key]
        {
            get
            {
                if (_addictionalValues == null)
                    return null;

                object ret;
                if (_addictionalValues.TryGetValue(key, out ret))
                    return ret;

                return null;
            }

            set
            {
                if (_addictionalValues == null)
                    _addictionalValues = new Dictionary<string, object>(1);

                if (_addictionalValues.ContainsKey(key))
                    _addictionalValues[key] = value;
                else
                    _addictionalValues.Add(key, value);
            }
        }

        public void ClearAdditionalValues()
        {
            _addictionalValues.Clear();
        }

        private Dictionary<string, object> _addictionalValues;
    }
}
