using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaikiAPI.EntityModels
{
    [Table("Employee")]
    public class Employee
    {
        [Key]
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public DateTime JoiningDate { get; set; }

        public override bool Equals(object obj)
        {
            var item = obj as Employee;

            if (item == null)
            {
                return false;
            }

            return this.EmpId.Equals(item.EmpId) && this.EmpName.Equals(item.EmpName) && this.Designation.Equals(item.Designation) && this.Department.Equals(item.Department) && this.JoiningDate.Equals(item.JoiningDate);
        }

        public override int GetHashCode()
        {
            return this.EmpId.GetHashCode() ^ this.EmpName.GetHashCode() ^ this.Designation.GetHashCode() ^ this.Department.GetHashCode() ^ this.JoiningDate.GetHashCode();
        }
    }
}