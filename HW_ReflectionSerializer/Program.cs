using Newtonsoft.Json;
using System.Diagnostics;

namespace HW_ReflectionSerializer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            F f = new F() { i1=1, i2=2, i3=3, i4="4", i5="5" };
            int tryesCount = 10000000;

            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < tryesCount; i++)
            {
                f.Serialize();
            }
            sw.Stop();
            var myTimeSerializeTime = sw.ElapsedMilliseconds;

            string myJson = f.Serialize();
            sw.Restart();
            for (int i = 0; i < tryesCount; i++)
            {
                Serializer.Deserialize<F>(myJson);
            }
            sw.Stop();
            var myDeserializeTime = sw.ElapsedMilliseconds;

            sw.Restart();
            for (int i = 0;i < tryesCount ; i++) 
            {
                JsonConvert.SerializeObject(f);
            }
            sw.Stop();
            var newtonsoftSerializeTime = sw.ElapsedMilliseconds;


            var newtonsoftJSON = JsonConvert.SerializeObject(f);
            sw.Restart();
            for (int i = 0; i < tryesCount; i++)
            {
                JsonConvert.DeserializeObject<F>(newtonsoftJSON);
            }
            sw.Stop();
            var newtonsoftDeserializeTime = sw.ElapsedMilliseconds;


            Console.WriteLine($"""
                Сериализуемый класс: class F {"{ int i1, i2, i3, i4, i5; Get() => new F(){ i1 = 1, i2 = 2, i3 = 3, i4 = 4, i5 = 5 }}; "}
                Сериализованный вид my: {myJson}   
                Сериализованный вид newtonsoft: {newtonsoftJSON}   
                Количество итераций: {tryesCount}
                Мой рефлекшн (мс):
                Время на сериализацию: {myTimeSerializeTime}
                Время на десериализацию: {myDeserializeTime}
                newtonsoft (мс):
                Время на сериализацию: {newtonsoftSerializeTime}
                Время на десериализацию: {newtonsoftDeserializeTime}
                """
                );
        }
    }
}