﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BM.Models;

public partial class NotificationStatus
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    [InverseProperty("NotificationStatus")]
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
