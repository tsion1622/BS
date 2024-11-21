﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BM.Models;

public partial class RoomPrice
{
    [Key]
    public int Id { get; set; }

    public int RoomId { get; set; }

    public double PricePerM2 { get; set; }

    public DateOnly AppliedDate { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    [ForeignKey("RoomId")]
    [InverseProperty("RoomPrices")]
    public virtual Room Room { get; set; }
}
