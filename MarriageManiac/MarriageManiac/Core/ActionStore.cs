using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarriageManiac.Core
{
    public class ActionStore
    {
        public ActionStore()
        {
            Store = new Dictionary<object, object>();
        }

        private Dictionary<object, object> Store { get; set; }

        public bool IsDone(object actionKey)
        {
            return Store.ContainsKey(actionKey);
        }

        public bool IsNotDone(object actionKey)
        {
            return !IsDone(actionKey);
        }

        public void SetDone(object actionKey)
        {
            SetDone(actionKey, null);
        }

        public void SetDone(object actionKey, object value)
        {
            if (!Store.ContainsKey(actionKey))
            {
                Store.Add(actionKey, null);
            }
            else
            {
                Store[actionKey] = value;
            }
        }

        public void Delete(object actionKey)
        {
            Store.Remove(actionKey);
        }
        
        public T Value<T>(object actionKey)
        {
            object value = null;

            Store.TryGetValue(actionKey, out value);

            if (value != null)
            {
                return (T)value;
            }
            else
            {
                return default(T);
            }
        }
    }
}