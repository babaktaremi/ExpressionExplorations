using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CSharpExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            

            Console.WriteLine("Hello World!");

            Expression<Func<User, object>> userExpression = user => user.Age;
            //Expression<Func<User, object>> userExpression = user => user.Name;

            //userExpression.Body.Dump();

            #region Using Expressions

            var user = new User();

            user.CreateUserUrl("Https://Test.com","age","name").Dump();
            user.CreateUserUrl("Https://Test.com", u => u.Age, u => u.Name).Dump();

            #endregion

            Console.ReadKey();
        }
    }

    public class User
    {
        public string Name { get; set; }
        public int Age { get; set; }

        #region Methods

        public  string CreateUserUrl(string url, params string[] fields)
        {
            return $"{url}?fields={string.Join(",",fields)}";
        }

        public  string CreateUserUrl(string url, params Expression<Func<User, object>>[] expressions)
        {
            var fields = new List<string>();

            foreach (var expression in expressions)
            {
                var body = expression.Body;

                if (body is MemberExpression me)
                {
                    fields.Add(me.Member.Name.ToLower());
                }
                else if(body is UnaryExpression ue)
                {
                    fields.Add(((MemberExpression)ue.Operand).Member.Name.ToLower());
                }
            }

            return $"{url}?fields={string.Join(",",fields)}";
        }

        #endregion
    }
}
