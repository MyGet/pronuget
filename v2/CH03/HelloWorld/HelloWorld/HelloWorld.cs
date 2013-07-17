namespace HelloWorld
{
    public class HelloWorld : IFluentInterface
    {
        public string Greet(string friendlyPerson)
        {
            return string.Format("Hello world, {0}!", friendlyPerson);
        }
    }
}
