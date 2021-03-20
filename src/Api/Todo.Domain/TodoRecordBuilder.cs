namespace Todo.Domain
{
    public class TodoRecordBuilder
    {
        private string _title;

        public static TodoRecordBuilder New => new TodoRecordBuilder();

        private TodoRecordBuilder()
        {
            _title = "A new ToDo";
        }

        public TodoRecordBuilder WithTitle(string title)
        {
            _title = title;
            return this;
        }

        public TodoRecord Build()
        {
            return new TodoRecord(_title);
        }
    }
}
