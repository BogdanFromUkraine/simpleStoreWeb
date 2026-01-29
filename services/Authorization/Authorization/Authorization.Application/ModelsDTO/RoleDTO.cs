namespace Notes_project.Models.ModelsDTO
{
    public class RoleDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        //добавляються дозволи до ролі, типу юсеру можна тільки читати, а адміну редагувати
        //реазізовані зв'язки tables
    }
}