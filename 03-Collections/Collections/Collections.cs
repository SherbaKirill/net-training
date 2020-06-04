using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Collections.Tasks {

    /// <summary>
    ///  Tree node item 
    /// </summary>
    /// <typeparam name="T">the type of tree node data</typeparam>
    public interface ITreeNode<T> {
        T Data { get; set; }                             // Custom data
        IEnumerable<ITreeNode<T>> Children { get; set; } // List of childrens
    }


    public class Task {

        /// <summary> Generate the Fibonacci sequence f(x) = f(x-1)+f(x-2) </summary>
        /// <param name="count">the size of a required sequence</param>
        /// <returns>
        ///   Returns the Fibonacci sequence of required count
        /// </returns>
        /// <exception cref="System.InvalidArgumentException">count is less then 0</exception>
        /// <example>
        ///   0 => { }  
        ///   1 => { 1 }    
        ///   2 => { 1, 1 }
        ///   12 => { 1, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144 }
        /// </example>
        public static IEnumerable<int> GetFibonacciSequence(int count) {
            if (count < 0)
                throw new ArgumentException();
            int first = 1;
            int second = 0;
            for (int item = 1; item <= count; item++)
            {
                yield return first;
                int result = first + second;
                second = first;
                first = result;                  
            }
                            
        }

        /// <summary>
        ///    Parses the input string sequence into words
        /// </summary>
        /// <param name="reader">input string sequence</param>
        /// <returns>
        ///   The enumerable of all words from input string sequence. 
        /// </returns>
        /// <exception cref="System.ArgumentNullException">reader is null</exception>
        /// <example>
        ///  "TextReader is the abstract base class of StreamReader and StringReader, which ..." => 
        ///   {"TextReader","is","the","abstract","base","class","of","StreamReader","and","StringReader","which",...}
        /// </example>
        public static IEnumerable<string> Tokenize(TextReader reader)
        {
            var delimeters = new[] { ',', ' ', '.', '\t', '\n' };
            if (reader == null)
                throw new System.ArgumentNullException();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] words = line.Split(delimeters, StringSplitOptions.RemoveEmptyEntries);
                foreach (string word in words)
                    yield return word;
            }
        }



        /// <summary>
        ///   Traverses a tree using the depth-first strategy
        /// </summary>
        /// <typeparam name="T">tree node type</typeparam>
        /// <param name="root">the tree root</param>
        /// <returns>
        ///   Returns the sequence of all tree node data in depth-first order
        /// </returns>
        /// <example>
        ///    source tree (root = 1):
        ///    
        ///                      1
        ///                    / | \
        ///                   2  6  7
        ///                  / \     \
        ///                 3   4     8
        ///                     |
        ///                     5   
        ///                   
        ///    result = { 1, 2, 3, 4, 5, 6, 7, 8 } 
        /// </example>
        public static IEnumerable<T> DepthTraversalTree<T>(ITreeNode<T> root) 
        {
            if (root == null)
              throw new ArgumentNullException();
            var stack = new Stack<ITreeNode<T>>();
                stack.Push(root);
                while (stack.Count > 0)
                {
                    var node = stack.Pop();
                    yield return node.Data;
                if (node.Children != null)
                    foreach(ITreeNode<T> nodeChildren in node.Children.Reverse())
                    stack.Push(nodeChildren);
                }
        }

        /// <summary>
        ///   Traverses a tree using the width-first strategy
        /// </summary>
        /// <typeparam name="T">tree node type</typeparam>
        /// <param name="root">the tree root</param>
        /// <returns>
        ///   Returns the sequence of all tree node data in width-first order
        /// </returns>
        /// <example>
        ///    source tree (root = 1):
        ///    
        ///                      1
        ///                    / | \
        ///                   2  3  4
        ///                  / \     \
        ///                 5   6     7
        ///                     |
        ///                     8   
        ///                   
        ///    result = { 1, 2, 3, 4, 5, 6, 7, 8 } 
        /// </example>
        public static IEnumerable<T> WidthTraversalTree<T>(ITreeNode<T> root) {
            if (root == null)
                throw new ArgumentNullException();
            var queue = new Queue<ITreeNode<T>>();
            queue.Enqueue(root);
            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                yield return node.Data;
                if (node.Children != null)
                    foreach (ITreeNode<T> nodeChildren in node.Children)
                        queue.Enqueue(nodeChildren);
            }
        }



        /// <summary>
        ///   Generates all permutations of specified length from source array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">source array</param>
        /// <param name="count">permutation length</param>
        /// <returns>
        ///    All permuations of specified length
        /// </returns>
        /// <exception cref="System.InvalidArgumentException">count is less then 0 or greater then the source length</exception>
        /// <example>
        ///   source = { 1,2,3,4 }, count=1 => {{1},{2},{3},{4}}
        ///   source = { 1,2,3,4 }, count=2 => {{1,2},{1,3},{1,4},{2,3},{2,4},{3,4}}
        ///   source = { 1,2,3,4 }, count=3 => {{1,2,3},{1,2,4},{1,3,4},{2,3,4}}
        ///   source = { 1,2,3,4 }, count=4 => {{1,2,3,4}}
        ///   source = { 1,2,3,4 }, count=5 => ArgumentOutOfRangeException
        /// </example>
        public static IEnumerable<T[]> GenerateAllPermutations<T>(T[] source, int count)
        {
            if (source.Length < count || count < 0)
                throw new ArgumentOutOfRangeException();

            if (count == 0)
                yield break;

            Stack<T> res = new Stack<T>();
            for (int i = 0; i < count; i++)
               res.Push(source[i]);

            T[] resultCombination = res.Reverse().ToArray();
            yield return resultCombination;
            res.Reverse();
            if (count < source.Length)
            {
                while (res.Count != 0)
                    if (Array.IndexOf(source, res.Peek()) < source.Length - count + res.Count - 1)
                    {
                        T item = res.Pop();
                        res.Push(source[Array.IndexOf(source, item) + 1]);
                        for (int j = res.Count; j < count; j++)
                            res.Push(source[Array.IndexOf(source, res.Peek()) + 1]);
                        resultCombination = res.Reverse().ToArray();
                        res.Reverse();
                        yield return resultCombination;
                    }
                    else res.Pop();

            }
        }

    }

    public static class DictionaryExtentions {
        
        /// <summary>
        ///    Gets a value from the dictionary cache or build new value
        /// </summary>
        /// <typeparam name="TKey">TKey</typeparam>
        /// <typeparam name="TValue">TValue</typeparam>
        /// <param name="dictionary">source dictionary</param>
        /// <param name="key">key</param>
        /// <param name="builder">builder function to build new value if key does not exist</param>
        /// <returns>
        ///   Returns a value assosiated with the specified key from the dictionary cache. 
        ///   If key does not exist than builds a new value using specifyed builder, puts the result into the cache 
        ///   and returns the result.
        /// </returns>
        /// <example>
        ///   IDictionary<int, Person> cache = new SortedDictionary<int, Person>();
        ///   Person value = cache.GetOrBuildValue(10, ()=>LoadPersonById(10) );  // should return a loaded Person and put it into the cache
        ///   Person cached = cache.GetOrBuildValue(10, ()=>LoadPersonById(10) );  // should get a Person from the cache
        /// </example>
        public static TValue GetOrBuildValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> builder) {
            if (!dictionary.ContainsKey(key))
                dictionary.Add(key, builder.Invoke());
            return dictionary[key];
        }

    }
}
