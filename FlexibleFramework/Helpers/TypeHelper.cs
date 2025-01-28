using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FlexibleFramework.Helpers
{
    /// <summary>
    ///     Помощь при работе с типами и их свойствами.<br />
    /// </summary>
    public static class TypeHelper
    {
        //public static Dictionary<string, Assembly> CachedAssemblies = new Dictionary<string, Assembly>();

        /// <summary>
        ///     Значения, которые считать NULL
        /// </summary>
        public static IEnumerable<object> NullValues = new object[] { null, DBNull.Value, double.NaN, float.NaN };

        private static readonly System.Collections.Concurrent.ConcurrentDictionary<Type, Func<object>> _ctorCache
            = new System.Collections.Concurrent.ConcurrentDictionary<Type, Func<object>>();

        /// <summary>
        /// Просты типы
        /// </summary>
        public static Type[] BasicTypes { get; set; } = 
            NumberTypes
            .Concat(BoolTypes)
            .Concat(new Type[] { typeof(string), typeof(DateTime), typeof(DateTime?), typeof(TimeSpan), typeof(Guid), typeof(Guid?), typeof(char), typeof(char?), typeof(Enum) })
            .ToArray();

        /// <summary>
        /// Булевы типы
        /// </summary>
        public static Type[] BoolTypes { get; } = new Type[]
        {
            typeof(bool),
            typeof(bool?),
            typeof(SqlBoolean),
            typeof(SqlBoolean?),
        };

        /// <summary>
        ///     Вещественные типы
        /// </summary>
        public static Type[] FloatNumberTypes { get; } = new Type[]
        {
            typeof(float), typeof(double), typeof(decimal),
            typeof(float?), typeof(double?), typeof(decimal?)
        };

        /// <summary>
        ///     Целочисленные типы
        /// </summary>
        public static IEnumerable<Type> IntNumberTypes { get; } = new[] {
            typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(short), typeof(ushort), typeof(byte), typeof(sbyte),
            typeof(int?), typeof(uint?), typeof(long?), typeof(ulong?), typeof(short?), typeof(ushort?), typeof(byte?), typeof(sbyte?)
        };

        /// <summary>
        ///     Числовые типы
        /// </summary>
        public static Type[] NumberTypes { get; } = IntNumberTypes.Concat(FloatNumberTypes).ToArray();

        /// <summary>
        /// Маппер с интерфейсов на конкретный тип. Используется в <see cref="CreateInstance(Type, object[])"/>
        /// </summary>
        public static Dictionary<Type, Type> InterfaceToInstanceMap { get; } = new Dictionary<Type, Type>()
        {
            {typeof(IEnumerable), typeof(List<object>) },
            {typeof(IEnumerable<>), typeof(List<>) },
            {typeof(ICollection), typeof(ObservableCollection<object>) },
            {typeof(ICollection<>), typeof(ObservableCollection<>) },
            {typeof(IDictionary<,>), typeof(Dictionary<,>) },
        };

        public static object CreateInstance(Type type)
        {
            if (!_ctorCache.TryGetValue(type, out var creator))
            {
                // Создаем лямбда-выражение: () => new T();
                var newExpression = Expression.New(type);
                var lambda = Expression.Lambda<Func<object>>(newExpression);
                creator = lambda.Compile();
                _ctorCache[type] = creator;
            }
            return creator();
        }

        /// <summary>
        /// Get all base types of specific type except object
        /// </summary>
        /// <param name="type">Тип у которого необходимо получить все базовые типы</param>
        /// <param name="includeThis">Включать в результат текущий тип</param>
        /// <param name="getInterfaces">Включать в список все интерфейсы от которых идет наследование</param>
        /// <returns></returns>
        public static Type[] GetBaseTypes(Type type, bool includeThis = false, bool getInterfaces = false)
        {
            var baseTypes = new List<Type>();
            var baseType = type;
            while (baseType.BaseType != null && baseType.BaseType != typeof(object))
            {
                baseType = baseType.BaseType;
                baseTypes.Add(baseType);
            }
            if (getInterfaces)
                baseTypes.AddRange(baseType.GetInterfaces());
            if (includeThis)
                baseTypes.Add(type);
            return baseTypes.ToArray();
        }

        /// <summary>
        /// Если type - коллекция, то возвращается тип элемента коллекции, иначе null
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetCollectionItemType(Type type)
        {
            if (type == null)
                return null;
            var isDic = typeof(IDictionary).IsAssignableFrom(type);
            var ga = type.GetGenericArguments();
            return type.IsArray
                ? type.GetElementType()
                : isDic && ga.Length > 1 ? ga[1] : type.GetGenericArguments().FirstOrDefault();
        }

        /// <summary>
        ///     Получить верхний класс в котором содержится данное свойство или поле
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public static Type GetRootType(MemberInfo member)
        {
            if (member.DeclaringType == null)
                return null;
            var rootType = member.DeclaringType;
            while (rootType.DeclaringType != null)
                rootType = rootType.DeclaringType;
            return rootType;
        }
    }
}