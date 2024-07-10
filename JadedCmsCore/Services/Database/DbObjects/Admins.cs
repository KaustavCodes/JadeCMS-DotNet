namespace JadedCmsCore.Services.Database.DbObjects;

public class Admins
{
    public int AdminId { get; set; }
    public string FullName { get; set; }
    public string LoginEmail { get; set; }
    public string LoginPasswordHash { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsDeleted { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public int? UpdatedBy { get; set; }
    public int? CreatedBy { get; set; }
}