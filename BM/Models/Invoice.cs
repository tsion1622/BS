﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BM.Models;

public partial class Invoice
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }

    public int InvoiceStatusId { get; set; }

    public double Amount { get; set; }

    public DateOnly InvoiceDate { get; set; }

    public DateOnly DueDate { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    [ForeignKey("InvoiceStatusId")]
    [InverseProperty("Invoices")]
    public virtual InvoiceStatus InvoiceStatus { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Invoices")]
    public virtual User User { get; set; }
}
