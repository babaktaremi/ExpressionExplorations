using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CSharpExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Test Expression

            //Expression<Func<int, bool>> comparer = number => number > 5;
            //Func<int, bool> funcComparer = number =>
            //{
            //    return number > 5; //can have method body
            //};

            //var compiled = comparer.Compile()(3);
            //var compiled2 = comparer.Compile().Invoke(3); //both are same

            #endregion

            #region Expression Dump

            //Expression<Func<User, object>> userExpression = user => user.Age;
            //Expression<Func<User, object>> userExpression = user => user.Name;

            //userExpression.Body.Dump();

            #endregion

            #region Using Expressions

            var user = new User();

            user.CreateUserUrl("Https://Test.com", "age", "name").Dump();
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

                if (body is MemberExpression me) //Members of a class like string etc...
                {
                    fields.Add(me.Member.Name.ToLower());
                }
                else if(body is UnaryExpression ue) //One input results in one output. Like constant values or Convert operation. In this case we are boxing int (for Age property) to object so there is a convert. But for string we do not need it 
                {
                    fields.Add(((MemberExpression)ue.Operand).Member.Name.ToLower()); //Operand is input of unary expression. In this case the Age property
                }
            }

            return $"{url}?fields={string.Join(",",fields)}";
        }

        #endregion
    }
}
