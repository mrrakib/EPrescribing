using System.ComponentModel.DataAnnotations.Schema;

namespace EPrescribing.Web.Models
{
    [Table("OnExaminations")]
    public class OnExamination : BaseEntity<int>
    {
        public string Status { get; set; }    //OnExaminationStatusEnum use it
        public string Description { get; set; }  //Data Comes from Pathology Tables

        public string UpperRight { get; set; }
        public string UpperLeft { get; set; }
        public string BottomRight { get; set; }
        public string BottomLeft { get; set; }

        public int PrescriptionId { get; set; }
        [ForeignKey("PrescriptionId")]
        public virtual Prescription Prescription { get; set; }
        [NotMapped]
        public int No { get; set; }
    }
}
