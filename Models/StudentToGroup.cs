namespace MaksGym.Models
{
    public class StudentToGroup
    {
        public int StudentToGroupId { get; set; } // PK
        public int StudentId { get; set; }        // FK -> Students.StudentId
        public int GroupsId { get; set; }         // FK -> Groups.GroupsId (увага на назву)

        public Student? Student { get; set; } = null!;
        public Group? Group { get; set; } = null!;
        public bool IsDeleted { get; set; } = false;
    }
}
