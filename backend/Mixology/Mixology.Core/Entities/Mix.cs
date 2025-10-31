using System.Net.Mime;
using Mixology.Core.Base;

namespace Mixology.Core.Entities;

public class Mix : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public double Rating { get; set; }
    public string Image { get; set; }
}