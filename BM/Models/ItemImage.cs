﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BM.Models;

public partial class ItemImage
{
    [Key]
    public int Id { get; set; }

    public int ItemId { get; set; }

    [Required]
    [StringLength(150)]
    public string Url { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    [ForeignKey("ItemId")]
    [InverseProperty("ItemImages")]
    public virtual Item Item { get; set; }
}
