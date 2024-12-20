using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Stardew_completion_guide_api.Models;

public class Item
{
	public Item()
	{
	}
    [Key]
    public Guid Id { get; set; }
    [ForeignKey("Category")]
    public Guid CategoryId { get; set; }
    public virtual Category Category { get; set; }
    public string Name { get; set; }
    public string? ImageUri { get; set; }
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime? UpdatedDate { get; set; }
  

}





