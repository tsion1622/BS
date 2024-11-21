﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BM.Models;

public partial class User
{
    [Key]
    public int Id { get; set; }

    public int GenderId { get; set; }

    [Required]
    [StringLength(30)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(30)]
    public string MiddleName { get; set; }

    [StringLength(30)]
    public string LastName { get; set; }

    [Required]
    [StringLength(100)]
    public string Email { get; set; }

    [Required]
    [StringLength(50)]
    public string Password { get; set; }

    [Required]
    [StringLength(50)]
    public string UserName { get; set; }

    [Required]
    [StringLength(50)]
    public string PhoneNumber { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<BuildingEmployee> BuildingEmployees { get; set; } = new List<BuildingEmployee>();

    [InverseProperty("User")]
    public virtual ICollection<Building> Buildings { get; set; } = new List<Building>();

    [InverseProperty("Receiver")]
    public virtual ICollection<Chat> ChatReceivers { get; set; } = new List<Chat>();

    [InverseProperty("Sender")]
    public virtual ICollection<Chat> ChatSenders { get; set; } = new List<Chat>();

    [ForeignKey("GenderId")]
    [InverseProperty("Users")]
    public virtual Gender Gender { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    [InverseProperty("User")]
    public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; } = new List<MaintenanceRequest>();

    [InverseProperty("User")]
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    [InverseProperty("User")]
    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();

    [InverseProperty("User")]
    public virtual ICollection<Shop> Shops { get; set; } = new List<Shop>();

    [InverseProperty("User")]
    public virtual ICollection<UserImage> UserImages { get; set; } = new List<UserImage>();
}
