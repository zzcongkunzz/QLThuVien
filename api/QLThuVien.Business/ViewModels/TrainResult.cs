using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLThuVien.Business.ViewModels;

public class TrainResult
{
    public double ElapsedSeconds { get; set; }
    public long ParametersCount { get; set; }
    public long SizeInBytes { get; set; }
    public int UsersCount { get; set; }
    public int BooksCount { get; set; }
    public int RatingsCount { get; set; }
    public IEnumerable<double> Costs { get; set; }
}
