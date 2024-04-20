namespace MaxiBot
{
    public class MaxiBot
    {
        public class Program
        {
            public static void Main()
            {
                var response = gptHandler.get_response("Can I post my password on Instagram?");
                Console.WriteLine(response);
            }
        }
    }
}