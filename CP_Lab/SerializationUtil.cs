using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CP_Lab
{
    public static class SerializationUtil
    {
        public static void WriteToStream(Object o, String fileName)
        {
            try
            {
                using (BinaryWriter writer =
                    new BinaryWriter(new FileStream(fileName, FileMode.Create, FileAccess.Write)))
                {
                    WriteToStream(o, writer);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"can not write to file: {ex}");
            }
        }

        public static void WriteToStream(Object o, BinaryWriter writer)
        {
            Console.WriteLine("trying to write object :: " + o.GetType().FullName);
            writer.Write(o.GetType().FullName);
            MethodInfo test = writer.GetType().GetMethod("Write", new[] {o.GetType()});
            if (test != null)
            {
                test.Invoke(writer, new[] {o});

                Console.WriteLine("writing to stream  :: " + o);
            }
            else
            {
                if (o.GetType().IsArray)
                {
                    writer.Write((int) ((Array) o).Length);
                    foreach (object i in (IEnumerable) o)
                    {
                        WriteToStream(i, writer);
                    }
                    return;
                }
                Type objectType = o.GetType();
                List<FieldInfo> filedsList = getFields(o);
                writer.Write(filedsList.Count);
                foreach (FieldInfo field in filedsList)
                {
                    writer.Write(field.Name);
                    Console.WriteLine("trying to write property :: " + field.Name);
                    WriteToStream(field.GetValue(o), writer);
                }
            }
        }

        private static List<FieldInfo> getFields(object o)
        {
            Type objectType = o.GetType();
            List<FieldInfo> filedsList = new List<FieldInfo>();
            while (true)
            {
                FieldInfo[] fields = objectType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic |
                                                          BindingFlags.Public | BindingFlags.FlattenHierarchy);
                foreach (FieldInfo field in fields)
                {
                    filedsList.Add(field);
                }
                if(objectType == typeof(object))
                    break;
                objectType = objectType.BaseType;
            }
            return filedsList;
        }

        public static Object ReadFromStream(String filename)
        {
            object result = null;
            try
            {
                using (BinaryReader reader =
                    new BinaryReader(new FileStream(filename, FileMode.Open, FileAccess.Read)))
                {
                    result = ReadFromStream(reader);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"can not read from file: {ex}");
            }
            return result;
        }

        public static Object ReadFromStream(BinaryReader reader)
        {
            object result;
            Type objectType = Type.GetType(reader.ReadString());
            Console.WriteLine("strating read item :: " + objectType.FullName);
            var methods = reader.GetType().GetMethods();

            MethodInfo readerMethod = null;
            foreach (MethodInfo method in methods)
            {
                if (method.Name.StartsWith("Read") && method.Name.Length > "Read".Length)
                {
                    if (objectType.FullName.Contains(method.Name.Replace("Read", "")))
                    {
                        Console.WriteLine(method.Name + "\t" + objectType.FullName);
                        readerMethod = method;
                        break;
                    }
                }
            }
            if (readerMethod != null)
            {
                result = readerMethod.Invoke(reader, null);
                Console.WriteLine("read simple valye :: \"" + result + "\"");
                return result;
            }

            if (objectType.IsArray)
            {
                Console.WriteLine("reading array :: " + objectType.FullName + "\t" + objectType.GetElementType());
                int size = reader.ReadInt32();
                Array array = Array.CreateInstance(objectType.GetElementType(), size);
                Console.WriteLine("array created!)");
                for (int i = 0; i < size; i++)
                    array.SetValue(ReadFromStream(reader), i);
                return array;
            }

            result = Assembly.GetExecutingAssembly()
                .CreateInstance(objectType.FullName);

            int countField = reader.ReadInt32();

            List<FieldInfo> filedsList = getFields(result);
            Console.WriteLine("cout " + countField);
            for (int i = 0; i < countField; i++)
            {
                String fildName = reader.ReadString();
                Console.WriteLine("\tread field name\t" + fildName);

                FieldInfo field = null;
                foreach (FieldInfo fieldInfo in filedsList)
                {
                    if (fieldInfo.Name.Equals(fildName))
                    {
                        field = fieldInfo;
                        break;
                    }
                }
                object readedField = ReadFromStream(reader);
                if (field != null)
                    field.SetValue(result, readedField);
            }
            return result;
        }
    }
}


//using (BinaryReader writer = new BinaryReader(stream))
//{
//writer.read
//.Invoke(writer, new object[] { o });
//}