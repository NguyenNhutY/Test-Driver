namespace DemoGPLX.Models
{
    public class DataUser
    {
        private string _id;
        private List<int> lsCauSai;
        private List<int> lsCau;
        private List<int> luaChons;
        public DataUser()
        {
            luaChons = new List<int>(0);
            lsCau = new List<int>(0);
            lsCauSai = new List<int>(0);
            Id = "1";
        }
        public string Id { get => _id; set => _id = value; }

        public List<int> CauSais { get => lsCauSai; set => lsCauSai = value; }

        public List<int> Caus { get => lsCau; set => lsCau = value; }

        public List<int> LuaChons { get => luaChons; set => luaChons = value; }

    }
}
