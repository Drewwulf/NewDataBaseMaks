using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaksGym.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }        // PK

        public int StudentId { get; set; }            // FK -> Students.StudentId
        public Student Student { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Value { get; set; }

        public DateOnly PaymentDate { get; set; }     // date
        public bool PaymentConfirmed { get; set; }    // bit

        public ICollection<StudentsToSubscription> StudentsToSubscriptions { get; set; } = new List<StudentsToSubscription>();

    }
}
