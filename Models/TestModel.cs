namespace webapi_client_211223.Models
{
    public class TestModel
    {
        public string? test_key { get; set; }


        public TestModel(string? test_key)
        {
            this.test_key = test_key;
        }
        public TestModel() { }
    }
}
