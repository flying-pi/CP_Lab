namespace CP_Lab
{
    public class Novel : LiteraryWork
    {     
        public EGenre Ganre { get; set; } = EGenre.EUnknown;
        public string Illustrator { get; set; } = "";

        public Novel()
        {
            
        }
        
        public Novel(string name = "", int price = 0, string content = "") :base(name,price,content)
        {
        }

        public override string ToString()
        {
            return $"{base.ToString()}\t ganre = {Ganre} \tillustrator = {Illustrator}"; 
        }
    }
}