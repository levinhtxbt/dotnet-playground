using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.JsonPatch;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {

            //var props = ExpressionExtensions.GetProperties<Student>();
            //foreach (var prop in props)
            //{
            //    Console.WriteLine(prop);
            //}

            JsonPatchDocument<Student> studentPatch = new JsonPatchDocument<Student>();
            studentPatch.Replace(x => x.FirstName, "Hello");
            //studentPatch.Replace(x => x.Age, 1);

            var firstName = studentPatch.GetValue(x => x.FirstName);
            var age = (int?)studentPatch.GetValue(x => x.Age);
            if (age.HasValue)
            {
                Console.WriteLine("AA");
            }
            //var t = studentPatch.GetType().GetGenericArguments()[0];
            //var properties = t.GetProperties();
            Console.ReadKey();

        }


    }

    public static class Extension
    {
        public static object GetValue<T>(this JsonPatchDocument<T> document, Expression<Func<T, object>> exp = null) where T : class
        {
            var me = exp.Body as MemberExpression;
            if (me == null)
            {
                UnaryExpression ubody = (UnaryExpression)exp.Body;
                me = ubody.Operand as MemberExpression;
                //throw new ArgumentException("You must pass a lambda of the form: '() => Class.Property' or '() => object.Property'");
            }
            var propertyName = me.Member.Name;
            var propertyInfo = (PropertyInfo)me.Member;

            var propertyType = propertyInfo.PropertyType;
            var propertyName2 = propertyInfo.Name;

            // check in document
            var o = document.Operations
                .FirstOrDefault(x => x.path.ToLower().Equals($@"/{propertyName}".ToLower()));

            return o == null ? null : Convert.ChangeType(o.value, me.Type);

        }
    }

    public class Student
    {

        public int Id { get; set; }

        private string PrivateId;

        public string FirstName { get; set; }

        public string LastName { get; set; }

        protected string PhoneNumber { get; set; }

        public int Age { get; set; }

    }
}
