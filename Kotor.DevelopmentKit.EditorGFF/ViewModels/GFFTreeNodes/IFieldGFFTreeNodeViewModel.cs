using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;

public interface IFieldGFFTreeNodeViewModel : IGFFTreeNodeViewModel
{
}

public interface IFieldGFFTreeNodeViewModel<T> : IFieldGFFTreeNodeViewModel
{
    public T FieldValue { get; set; }
}
