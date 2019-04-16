using System;
using System.Collections;
using System.Collections.Generic;

namespace MyDictionary
{
    public class Prime
    {
        private static int [] PrimeNums = new int[] {2, 3,	5,	7,	11,	13,	17,	19,	23,	29,	31,	37,	41,	43,	47,	53,	59,	61,	67,	71, 73,	
            79,	83,	89,	97,	101, 103, 107, 109, 113, 127, 131, 137, 139, 149, 151, 157,	163, 167, 173, 179, 181, 191, 193, 197, 199};
        
        public static int nextPrime(int Prev)
        {
            for (int i = 0; i < PrimeNums.Length - 1; i++)
            {
                if (Prev == PrimeNums[i])
                {
                    return PrimeNums[i + 1];
                }
            }
            
            return PrimeNums[0];
        }
    }
    
    public class Dictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        
        public TKey Key { get; set; }

        public TValue Value
        {
            get { return Value; }
            set
            {
                if (value != null)
                {
                    Value = value;
                }
            }
        }

        public Dictionary(Int32 capacity)
        {
            Capacity = capacity;
        }
           
        private const float loadfactor = 0.72f;

        private int capacity = 2;

        public int Capacity
        {
            get { return capacity; }
            set
            {
                if (value < 200 && value > 2 )
                    capacity = value;
            }
        } //размер массива hashArray


        List<KeyValuePair<TKey, TValue>> [] hashArray = new List<KeyValuePair<TKey, TValue>>[Capacity];


        //Добавление ключа и значения
        public void Add(TKey key, TValue value)
        {
            // Проверяем входные данные на пустоту.
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            
            //Add вызывает метод ArgumentException при попытке добавить повторяющийся ключ.
            if (ContainsKey(key))
            {
                throw new ArgumentException("Повторяющийся ключ");
            }

            KeyValuePair<TKey, TValue> kvp = new KeyValuePair<TKey, TValue>(key, value);
            if (hashArray[Math.Abs(key.GetHashCode() % hashArray.Length)] == null)
            {
                hashArray[Math.Abs(key.GetHashCode() % hashArray.Length)] = new List<KeyValuePair<TKey, TValue>>();
            }

            hashArray[Math.Abs(key.GetHashCode() % hashArray.Length)].Add(kvp);

            Count++;
            if (Count > Capacity * loadfactor)
            {
                ReSize();
            }
        }
        
        //Добавление пары
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            // Проверяем входные данные на пустоту.
            if (item.Value == null)
            {
                throw new ArgumentNullException(nameof(item.Value));
            }
            
            if (item.Key == null)
            {
                throw new ArgumentNullException(nameof(item.Key));
            }
            
            //Add вызывает метод ArgumentException при попытке добавить повторяющийся ключ.
            if (ContainsKey(item.Key))
            {
                throw new ArgumentException("Повторяющийся ключ");
            }
            

            if (hashArray[Math.Abs(item.Key.GetHashCode() % hashArray.Length)] == null)
            {
                hashArray[Math.Abs(item.Key.GetHashCode() % hashArray.Length)] = new List<KeyValuePair<TKey, TValue>>();
            }
            
            hashArray[item.Key.GetHashCode() % hashArray.Length].Add(item);
            

            Count++;
            
            if (Count > Capacity * loadfactor)
            {
                ReSize();
            }
        }
        
        //Содержит ли ключ
        public bool ContainsKey(TKey key)
        {
            
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (hashArray[Math.Abs(key.GetHashCode() % hashArray.Length)] != null)
            {
                foreach (var keyValuePair in hashArray[Math.Abs(key.GetHashCode() % hashArray.Length)])
                {
                    if (keyValuePair.Key.Equals(key))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool Remove(TKey key)
        {
            // Проверяем входные данные на пустоту.
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            foreach (var keyValuePair in hashArray[Math.Abs(key.GetHashCode() % hashArray.Length)])
            {
                if (keyValuePair.Key.Equals(key))
                {
                    Count--;
                    return hashArray[Math.Abs(key.GetHashCode() % hashArray.Length)].Remove(keyValuePair);
                }
            }
            return false;
        }

        public void Clear()
        {
            foreach (var VARIABLE in hashArray)
            {
                VARIABLE.Clear();
            }

            Count = 0;
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            // Проверяем входные данные на пустоту.
            if (item.Value == null)
            {
                throw new ArgumentNullException(nameof(item.Value));
            }
            
            if (item.Key == null)
            {
                throw new ArgumentNullException(nameof(item.Key));
            }

            foreach (var keyValuePair in hashArray[Math.Abs(item.Key.GetHashCode() % hashArray.Length)])
            {
                if (keyValuePair.Key.Equals(item.Key) && keyValuePair.Value.Equals(item.Value))
                {
                    return true;
                }
            }

            return false;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            // Проверяем входные данные на пустоту.
            if (array == null)
            {
                throw new ArgumentNullException("Свойство array имеет значение null.");
            }

            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException("Значение параметра arrayIndex меньше 0.");
            }

            if (Count > array.Length - arrayIndex)
            {
                throw new ArgumentException(
                    "Число элементов в исходной коллекции ICollection<T> больше доступного места от положения, " +
                    "заданного значением параметра arrayIndex, до конца массива назначения array.");
            }
            
            int i = arrayIndex;
           
            foreach (var keyValuePair in this)
            {
                if (i < array.Length)
                {
                    array[i] = keyValuePair;
                    i++;
                }   
            }          
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            // Проверяем входные данные на пустоту.
            if (item.Value == null)
            {
                throw new ArgumentNullException(nameof(item.Value));
            }
            
            if (item.Key == null)
            {
                throw new ArgumentNullException(nameof(item.Key));
            }

            if (Contains(item))
            {
                foreach (var keyValuePair in hashArray[Math.Abs(item.Key.GetHashCode() % hashArray.Length)])
                {
                    if (keyValuePair.Key.Equals(item.Key) && keyValuePair.Value.Equals(item.Value))
                    {
                        Count--;
                        return hashArray[Math.Abs(item.Key.GetHashCode() % hashArray.Length)].Remove(item);
                    }
                }
            }

            return false;
        }

        private static int count = 0;

        public int Count
        {
            get { return count; }
            set
            {
                if (value > -1)
                    count = value;
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }


        public bool TryGetValue(TKey key, out TValue value)
        {
            // Проверяем входные данные на пустоту.
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            
            foreach (var list in hashArray)
            {
                foreach (var keyValuePair in list)
                {
                    if (keyValuePair.Key.Equals(key))
                    {
                        value = keyValuePair.Value;
                        return true;
                    }
                }
            }
            
            value = default(TValue);
            return false;
        }

        public TValue this[TKey key]
        {
            get
            {
                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }

                foreach (var keyValuePair in this)
                {
                    if (keyValuePair.Key.Equals(key))
                    {
                        return keyValuePair.Value;
                    }
                }
                
                //ключ не найден
                throw new KeyNotFoundException(nameof(key));
            }
            
            set 
            { 
                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }

                List<KeyValuePair<TKey, TValue>> list = hashArray[Math.Abs(key.GetHashCode() % hashArray.Length)];
                for (var i = 0; i < list.Count; i++)
                {
                    if (list[i].Key.Equals(key))
                    {
                        //list[i].Value = value;
                    }
                }
            }
        }

        public ICollection<TKey> Keys { get; }
        public ICollection<TValue> Values { get; }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            // Используем перечислитель списка элементов данных множества.
            foreach (var list in hashArray)
            {
                if (list == null)
                {
                    continue;
                }
                
                foreach (var keyValuePair in list)
                {
                    yield return keyValuePair;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            // Используем перечислитель списка элементов данных множества.
            return GetEnumerator();
        }

        private void ReSize() 
        {
            Capacity = Prime.nextPrime(Capacity);
            List<KeyValuePair<TKey, TValue>> [] NewHashArray = new List<KeyValuePair<TKey, TValue>>[Capacity];
            foreach (var keyValuePair in this)
            {
                if (NewHashArray[Math.Abs(keyValuePair.Key.GetHashCode() % Capacity)] == null)
                {
                    NewHashArray[Math.Abs(keyValuePair.Key.GetHashCode() % Capacity)] =
                        new List<KeyValuePair<TKey, TValue>>();
                }

                NewHashArray[Math.Abs(keyValuePair.Key.GetHashCode() % Capacity)].Add(keyValuePair);
            }            
            hashArray = NewHashArray;
        } 
    }
    
    
    internal class Program
    {
        public static void Main(string[] args)
        {
            // Создаем множества.
            Dictionary<int, string> dictionary = new Dictionary<int,string>(11);
 
            // Выполняем операции со множествами.
            dictionary.Add(0, "0");
            dictionary.Add(1, "1");
            dictionary.Add(2, "2");
            dictionary.Add(3, "3");
            dictionary.Add(4, "4");
            dictionary.Add(5, "5");
            dictionary.Add(6, "6");
            dictionary.Add(7, "7");
            dictionary.Add(8, "8");
            dictionary.Add(9, "9");
            dictionary.Add(10, "10");
            dictionary.Add(11, "11");
            dictionary.Add(796, "796");  
            dictionary.Add(4576, "4576");                                          
            dictionary.Add(45097, "45097");
            dictionary.Remove(0);
            dictionary.Remove(9);
            dictionary.Add(new KeyValuePair<int, string>(12345, "12345"));
            
            Console.WriteLine("Содержит ли пару [0,0]:" + dictionary.Contains(new KeyValuePair<int, string>(0, "0")));
            Console.WriteLine("Содержит ли пару [1,1]:" + dictionary.Contains(new KeyValuePair<int, string>(1, "1")));
            
            Console.WriteLine("Содержит ли ключ 123456789: " + dictionary.ContainsKey(123456789));
            Console.WriteLine("Содержит ли ключ 2: " + dictionary.ContainsKey(2));
            
            dictionary.Remove(new KeyValuePair<int, string>(2, "2"));
            Console.WriteLine("Содержит ли ключ 2 после удаления: " + dictionary.ContainsKey(2));

            string value;
            dictionary.TryGetValue(6, out value);
            Console.WriteLine("Значение пары: " + value);

            Console.WriteLine(dictionary[3]);
            
            //Копирование в массив
            KeyValuePair<int, string> [] array = new KeyValuePair<int, string>[20];
            dictionary.CopyTo(array, 2);
            Console.WriteLine("Вывод массива: ");
            foreach (var VARIABLE in array)
            {
                Console.Write($"{VARIABLE} ");
            }
            Console.WriteLine();
            
            // Выводим исходные словарь на консоль.
            PrintDict(dictionary, "Первый словарь: ");
        }
 
        
        private static void PrintDict(Dictionary<int, string>dictionary, string title)
        {
            Console.Write(title);
            foreach (var item in dictionary)
            {
                Console.Write($"{item} ");
            }
            Console.WriteLine();
        }
    }
}