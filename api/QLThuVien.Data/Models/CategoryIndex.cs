using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLThuVien.Data.Models;

public class CategoryIndex
{
    public Guid RawFeatureExtractorId { get; set; }
    public Guid CategoryId { get; set; }
    public int Index { get; set; }
}
