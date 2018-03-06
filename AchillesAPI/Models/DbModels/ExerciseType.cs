using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AchillesAPI.Models.DbModels
{
    [Table("ExerciseTypes")]
    public class ExerciseType
    {
        [Key]
        [Required]
        public Guid ExerciseTypeID { get; set; }
        [Required]
        public virtual int ExerciseEnumId
        {
            get
            {
                return (int)this.ExerciseTypeEnum;
            }
            set
            {
                ExerciseTypeEnum = (ExerciseTypes)value;
            }
        }
        [EnumDataType(typeof(ExerciseTypes))]
        public ExerciseTypes ExerciseTypeEnum { get; set; }
    }

    public enum ExerciseTypes
    {
        RepsSets = 0,
        Timed = 1
    }
}
