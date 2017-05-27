using class_diagram_lab;

namespace CP_Lab
{
    public class Drama : LiteraryWork
    {
        public Drama()
        {

        }


        public Drama(int sceneCount = 0, string content = "", string name = "", string author = "") : base(content, name, author)
        {

        }

        public int SceneCount { get; set; }

        public override string ToString()
        {
            return base.ToString() + $"\tsceneCount = {SceneCount}";
        }
    }

}