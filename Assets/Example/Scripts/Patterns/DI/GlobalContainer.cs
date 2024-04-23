using System;
using System.Collections.Generic;
using System.Reflection;
using Example.Scripts.Patterns.Singleton;

namespace Example.Scripts.Patterns.DI
{
    public class GlobalContainer : Singleton<GlobalContainer>
    {
        private const BindingFlags BINDING_FLAGS = BindingFlags.NonPublic | BindingFlags.Instance;
        
        private readonly List<IShared> _instances = new();
        private Type _iSharedType = typeof(IShared);
        
        public T Get<T>() where T : IShared
        {
            Type targetType = typeof(T);

            return (T)Get(targetType);
        }

        public IShared Get(Type type)
        {
            IShared result = null;
            
            foreach (var instance in _instances)
            {
                var instanceType = instance.GetType();
                if (instanceType == type || type.IsAssignableFrom(instanceType))
                {
                    result = instance;
                    break;
                }
            }
            
            return result;
        }

        public IReadOnlyList<IShared> GetAll<T>() where T : IShared
        {
            List<IShared> result = new();

            foreach (var instance in _instances)
                if (instance is T)
                    result.Add(instance);

            return result;
        }

        public void Add<T>() where T : IShared
        {
            Type type = typeof(T);
            var instance = CreateInstance(type);
            _instances.Add(instance);
        }
        
        private IShared CreateInstance(Type type)
        {
            return Activator.CreateInstance(type) as IShared;
        }
        
        public void InjectAll()
        {
            foreach (IShared sharedObject in _instances)
                InjectAt(sharedObject);
        }
        
        public void InitAll()
        {
            foreach (var instance in _instances)
                instance.Init();
        }
        
        public void InjectAt(object obj)
        {
            Type type = obj.GetType();
            
            foreach (FieldInfo field in GetSharedFields(type))
            {
                Type fieldType = field.FieldType;
                
                IShared instance = Get(fieldType);
                field.SetValue(obj, instance);
            }
        }
        
        private IEnumerable<FieldInfo> GetSharedFields(Type type)
        {
            foreach (FieldInfo fieldInfo in GetAllFields(type))
                if (fieldInfo.GetCustomAttribute(typeof(InjectAttribute)) != null)
                    yield return fieldInfo;
        }
        
        private FieldInfo[] GetAllFields(Type type)
        {
            List<FieldInfo> fields = new(type.GetFields(BINDING_FLAGS));
        
            while (type.BaseType.IsSubclassOf(typeof(SharedObject)))
            {
                fields.AddRange(type.BaseType.GetFields(BINDING_FLAGS));
                type = type.BaseType;
            }
            
            return fields.ToArray();
        }
    }
}