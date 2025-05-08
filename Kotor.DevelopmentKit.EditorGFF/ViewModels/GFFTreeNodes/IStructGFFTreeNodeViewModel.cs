using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

public interface IStructGFFTreeNodeViewModel : IGFFTreeNodeViewModel
{
    public int StructID { get; set; }
    public void AddField(IFieldGFFTreeNodeViewModel field);
    public void DeleteField(IFieldGFFTreeNodeViewModel field);
}
